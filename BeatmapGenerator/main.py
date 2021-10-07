from random import random

import librosa.display
import matplotlib.pyplot as plt
import numpy as np
import soundfile
import json
from collections import deque

shapes = {"circle": (1.75, 2), "check": (1, 4.5), "flash": (1.5, 7), "x": (2, 8), "square": (2.75, 8.5),
          "star": (3, 10), "tap": (0.5, 15)}


def get_random_note():
    rand_number = random() * 15
    for item in shapes.items():
        if rand_number < item[1][1]:
            return item[0]


# filename = 'soundtrack1.wav'
# DURATION = 47
# WINDOW = 5
# THRESHOLD = .8
# ACTIVATIONS_NEEDED = 4
# OFFSET = 0

# filename = 'soundtrack2.wav'
# DURATION = 78
# WINDOW = 5
# THRESHOLD = .84
# ACTIVATIONS_NEEDED = 1
# OFFSET = 15.5


filename = 'soundtrack3.wav'
DURATION = 91
WINDOW = 5
THRESHOLD = .93
ACTIVATIONS_NEEDED = 2
OFFSET = 0


y, sr = librosa.load(f"soundtracks/{filename}", duration=DURATION, offset=OFFSET)
onset_env = librosa.onset.onset_strength(y=y, sr=sr)
pulse = librosa.beat.plp(onset_envelope=onset_env, sr=sr)
beats_plp = np.flatnonzero(librosa.util.localmax(pulse))
times = librosa.times_like(pulse, sr=sr)

n_fft = 2048
n_mels = 128
hop_length = 512
S = librosa.feature.melspectrogram(y, sr=sr, n_fft=n_fft, hop_length=hop_length, n_mels=n_mels)
S_DB = librosa.power_to_db(S, ref=np.max)

fig, ax = plt.subplots()
S_dB = librosa.power_to_db(S, ref=np.max)

freq_ranges = {
    "SUB_BASS": (0, 60),
    "BASS": (60, 250),
    "LOWER_MIDRANGE": (250, 500),
    "MIDRANGE": (500, 2000),
    "UPPER_MIDRANGE": (2000, 4000),
    "PRESENCE": (4000, 6000),
    "BRILLIANCE": (6000, 8000),
}

mel_bins = librosa.mel_frequencies(n_mels=n_mels, fmin=0, fmax=8000)

ranges_idx = {key: np.where(np.logical_and(mel_bins >= low, mel_bins <= high)) for key, (low, high) in
              freq_ranges.items()}
ranges_idx = {key: value[0] for key, value in ranges_idx.items() if len(value) > 0}
mel_ranges = {key: (np.min(value), np.max(value)) for key, value in ranges_idx.items()}

type_db_mean = {key: S_DB[low:high].mean(axis=0) for key, (low, high) in mel_ranges.items()}

type_db_filtered = dict()
for key, value in type_db_mean.items():
    arr = value - np.min(value)
    mean_value = arr.mean()
    relu_filter = lambda x0: x0 if x0 >= mean_value else 0
    f = np.vectorize(relu_filter)
    rectified = f(arr)
    norm = np.linalg.norm(rectified)
    normalized = rectified / norm
    normalized_max = normalized.max()
    threshold_filter = np.vectorize(lambda x0: 1 if x0 >= THRESHOLD * normalized_max else 0)
    type_db_filtered[key] = threshold_filter(normalized)

to_click = []
allowed_channels = ["BASS", "LOWER_MIDRANGE", "MIDRANGE", "UPPER_MIDRANGE", "PRESENCE", "BRILLIANCE"]

dtempo = librosa.beat.tempo(onset_envelope=onset_env, sr=sr,
                            aggregate=None)

# timestamps_with_shapes = {"times": [], "tempo": dtempo.tolist()}
timestamps_with_shapes = {"times": [], "tempo": times[beats_plp.tolist()].tolist()}

for beat_time in beats_plp.tolist():
    activations = [key for key, value in type_db_filtered.items() if
                   value[beat_time - WINDOW: beat_time + WINDOW].astype(bool).any() and key in allowed_channels]
    if len(activations) >= ACTIVATIONS_NEEDED:
        # if i % 6 == 0:
        #     timestamps_with_shapes["times"].append({"time": times[beat_time], "shape": "tap"})
        # else:
        to_click.append(beat_time)
        # i += 1

times_queue = deque(times[to_click].tolist())
last_time = 0
while len(times_queue) != 0:
    note = get_random_note()
    note_time = shapes[note][0]
    current_time = times_queue.popleft()
    while len(times_queue) != 0 and current_time - last_time < note_time:
        current_time = times_queue.popleft()
    if current_time - last_time >= note_time:
        timestamps_with_shapes["times"].append({"time": current_time, "shape": note})
        last_time = current_time

clicks = librosa.clicks(np.array([click["time"] for click in timestamps_with_shapes["times"]]), sr=sr, length=len(y))

if __name__ == '__main__':
    soundfile.write(f'output_songs/{filename}', y, sr)
    with open(f"output_beatmap/{filename[:-4]}.json", "w") as file:
        file.write(json.dumps(timestamps_with_shapes))
