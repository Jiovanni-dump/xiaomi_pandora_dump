# agmcap file.wav [-D card] [-d device] [-p period_size] [-n n_periods]
# $1 capture time
# $2 mark headset twice to insert recording, the value can be left and right

set -x

echo "enabling usb mic"
agmcap /data/usbmic_$2.wav -D 100 -d 101 -c 1 -r 48000 -b 16 -i USB_AUDIO-TX -skv 0xB1000001 -dkv 0xA3000005 -ikv 2 -T $1

