#!/usr/bin/env python3


import keyboard
import argparse
import pyaudio
import wave
import json
import sys

CHUNK = 1024

parser = argparse.ArgumentParser()
parser.add_argument("input", type=str, help="Input song [.wav]")
parser.add_argument("output", type=str, help="Output file location")
parser.add_argument("--chunk", type=int, help="Chunk size")
args = parser.parse_args()

if args.chunk is not None:
    CHUNK = args.chunk


wf = wave.open(args.input, 'rb')

# instantiate PyAudio (1)
p = pyaudio.PyAudio()

# open stream (2)
stream = p.open(format=p.get_format_from_width(wf.getsampwidth()),
                channels=wf.getnchannels(),
                rate=wf.getframerate(),
                output=True)


beats = []
frame_count = 0

# read data
data = wf.readframes(CHUNK)
frame_count += CHUNK
sr = wf.getframerate()



# play stream (3)
still_pressed = False
while len(data) > 0:
    try:
        stream.write(data)
        data = wf.readframes(CHUNK)
        frame_count += CHUNK

        if keyboard.is_pressed("z") or keyboard.is_pressed("x") and not still_pressed:
            still_pressed = True
            while keyboard.is_pressed("z") or keyboard.is_pressed("x"):
                stream.write(data)
                data = wf.readframes(CHUNK)
                frame_count += CHUNK
            still_pressed = False
            beats.append(frame_count / sr)
            print(f"Beat at {frame_count / sr}")
    except KeyboardInterrupt:
        with open(args.output, "w") as f:
            f.write(json.dumps({"times": beats}))
        print(f"Saved beatmap to {args.output}")
        sys.exit()

# stop stream (4)
stream.stop_stream()
stream.close()

# close PyAudio (5)
p.terminate()

# Save file
with open(args.output, "w") as f:
    f.write(json.dumps({"times": beats}))
print(f"Saved beatmap to {args.output}")