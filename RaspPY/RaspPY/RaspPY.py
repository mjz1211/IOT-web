import time as time
import RPi.GPIO as gpio
gmt=time.gmtime()
print('現在日期: %d年 %d 月 %d日' %(gmt.tm_year,gmt.tm_mon,gmt.tm_mday))
