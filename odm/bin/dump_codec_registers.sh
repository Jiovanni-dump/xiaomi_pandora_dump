sleep 1
echo Y > /d/regmap/wcd939x-slave.e01170223/cache_bypass
echo Y > /d/regmap/soc:spf_core_platform:lpass-cdc/cache_bypass
cat /d/regmap/wcd939x-slave.e01170223/registers > /data/wcd939x_$1.txt
cat /d/regmap/soc:spf_core_platform:lpass-cdc/registers > /data/lpass_$1.txt
