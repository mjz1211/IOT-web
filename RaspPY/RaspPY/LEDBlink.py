#閃爍的LD Blind
import RPi.GPIO as gpio
import time as time
#定義output通5V PIN
ledpin=5
#設定pin模式
gpio.setmode(gpio.BCM) #設定板子pin編號方式
gpio.setup(ledpin,gpio.OUT) #設定輸出方向
#定義閃爍led模式
def blink(led):
	#輸出亮起來
	gpio.output(led,gpio.HIGH)
	time.sleep(0.5)
	gpio.output(led,gpio.LOW)
	time.sleep(0.5)

gpio.output(ledpin,gpio.LOW)
print('通電中-LED恆亮')
#呼叫自訂程序
#停留時間
time.sleep(5)
for i in range(0,20):
	blink(ledpin)


