import librosa.display
import matplotlib.pyplot as plt
import numpy as np
import soundfile
import json
import time

filename = 'soundtrack1.wav'
DURATION = 60
WINDOW = 5
THRESHOLD = .8
ACTIVATIONS_NEEDED = 4

# filename = 'soundtrack1.wav'
# DURATION = 60
# WINDOW = 5
# THRESHOLD = .84
# ACTIVATIONS_NEEDED = 2


# filename = 'soundtrack2.wav'
# DURATION = 60
# WINDOW = 5
# THRESHOLD = .935
# ACTIVATIONS_NEEDED = 1

# filename = 'soundtrack3.wav'
# DURATION = 60
# WINDOW = 5
# THRESHOLD = .93
# ACTIVATIONS_NEEDED = 2

y, sr = librosa.load(f"soundtracks/{filename}", duration=DURATION)
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
img = librosa.display.specshow(S_dB, x_axis='time',
                               y_axis='mel', sr=sr,
                               fmax=8000, ax=ax)
fig.colorbar(img, ax=ax, format='%+2.0f dB')
ax.set(title='Mel-frequency spectrogram')
plt.show()

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

fig2, ax2 = plt.subplots()
ax2.set_xlabel("Time")
ax2.set_ylabel("Db")
for key, value in type_db_mean.items():
    ax2.plot(value, label=key)
ax2.legend()
plt.show()


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

fig3, ax3 = plt.subplots(2, figsize=(20, 10))
ax3[0].set_xlabel("Time")
ax3[0].set_ylabel("Activation")
for i, (key, value) in enumerate(type_db_filtered.items()):
    t = librosa.frames_to_time([i for i in range(len(value))], sr=sr, hop_length=hop_length, n_fft=n_fft)
    ax3[0].plot(t, value + i * 1.1, label=key)

ax3[1].plot(librosa.times_like(onset_env),
            librosa.util.normalize(onset_env),
            label='Onset strength')

ax3[0].vlines(times[beats_plp], 0, 8, alpha=0.5, color='r',
              linestyle='--', label='PLP Beats')

plt.show()

to_click = []
allowed_channels = ["BASS", "LOWER_MIDRANGE", "MIDRANGE", "UPPER_MIDRANGE"]

for beat_time in beats_plp.tolist():
    activations = [key for key, value in type_db_filtered.items() if value[beat_time - WINDOW: beat_time + WINDOW].astype(bool).any() and key in allowed_channels]
    if len(activations) >= ACTIVATIONS_NEEDED:
        to_click.append(beat_time)

clicks = librosa.clicks(times[to_click], sr=sr, length=len(y))

if __name__ == '__main__':
    soundfile.write(f'output_songs/{filename}', y + clicks, sr)
    with open(f"output_beatmap/{filename}.json", "w") as file:
        file.write(json.dumps({"times": times[to_click].tolist()}))




