#/cygdrive/c/Users/conor.williams/Desktop/ffmpeg -framerate 1 -pattern_type glob -i '*.png' -c:v libx264 -pix_fmt yuv420p out.mp4
/cygdrive/c/Users/conor.williams/Desktop/ffmpeg -f concat -i in.txt -i hungarian.m4a -c:v libx264 -c:a copy -shortest -r 30 -pix_fmt yuv420p out.mp4

