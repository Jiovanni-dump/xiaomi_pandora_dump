mount -t debugfs debugfs /sys/kernel/debug

node0="13-0040"
node1="13-0042"

logcat -d > /data/logcat_spk.txt
dmesg > /data/dmesg_spk.txt
tinymix > /data/tinymix_spk.txt
cat /d/gpio > /data/gpio_spk.txt
cat /d/regmap/$node0/registers > /data/register_$node0.txt
cat /d/regmap/$node1/registers > /data/register_$node1.txt
