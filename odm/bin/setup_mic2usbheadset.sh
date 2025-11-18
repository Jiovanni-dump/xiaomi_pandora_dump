# agmplay file.wav [-D card] [-d device] [-p period_size] [-n n_periods]
# sample usage: playback.sh 2000.wav  1
# $1 capture time(s)
# $2 mic
set -x

echo "capture time=$1s"
echo "enabling $2 mic"
audio-factory-test -f enable_$2-mic
# start recording
agmcap /data/data/$2mic2usb.wav -r 48000 -b 16 -c 2 -p 1024 -n 4 -D 100 -d 101 -i CODEC_DMA-LPAIF_RXTX-TX-3 -T $1

echo "disabling $2 mic"
audio-factory-test -f disable_$2-mic

sleep 1

echo "enabling usb headset"
agmplay /data/data/$2mic2usb.wav -D 100 -d 100 -i USB_AUDIO-RX -num_intf 1 -skv 0xA100000E -dkv 0xA2000005 -ikv 1 -dppkv 0xAC000002

