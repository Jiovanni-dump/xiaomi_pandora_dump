
mount -t debugfs debugfs /sys/kernel/debug

echo -n "file audio_machine.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file kaanapali.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file msm_common.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file lpass-cdc.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file lpass-cdc-utils.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file lpass-cdc-clk-rsc.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file lpass-cdc-comp.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file lpass-cdc-rx-macro.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file lpass-cdc-tx-macro.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file lpass-cdc-va-macro.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file lpass-cdc-wsa-macro.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file lpass-cdc-wsa2-macro.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file swr-haptics.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file wcd939x.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file wcd939x-slave.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file wcd939x-mbhc.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file wcd-mbhc-adc.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file wcd-mbhc-v2.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file wcd-clsh.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file swr-mstr-ctrl.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file soundwire.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file soc-core.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file soc-dapm.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file soc-pcm.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file soc-dai.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file lpass-cdc-utils.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file swr-mstr-ctrl.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file wcd-usbss-utils.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file wcd939x-i2c.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file usci_glink.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file qcom-hv-haptics.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file audio-ext-clk-up.c +p" > /sys/kernel/debug/dynamic_debug/control
echo -n "file regmap-swr.c +p" > /sys/kernel/debug/dynamic_debug/control

setprop persist.offlinelog.kernel.size 640
setprop persist.offlinelog.logcat.size 640

