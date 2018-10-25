#門禁管制超音播應用
import RPi.GPIO as GPIO
import SensorDataEntity
import datetime as datetime
import time as time
import requests as request
import SensorDataEntity as data
import json as json
#引用自訂的dht11 modules
import dht11
import threading

#雲端服務端點位址
url='https://iot.cht.com.tw/iot/v1/device/5604707091/rawdata'
apiKey='PKWVLF9JM7MOIEFS40'
#設定腳位
trigger=17
echo=27
ledpin=5

#設定板子腳位模式 板子初始化
GPIO.setmode(GPIO.BCM)

GPIO.setwarnings(False)

#設定trigger output echo-input
GPIO.setup(trigger,GPIO.OUT)
GPIO.setup(echo,GPIO.IN)
GPIO.setup(ledpin,GPIO.OUT)

#定義計算距離的method
def distance(stop):
	#啟動一個觸發 透過超音波發出射頻
	GPIO.output(trigger,True) #產生一個射頻
	time.sleep(0.0001)  #0.01ms
	#停止射頻
	GPIO.output(trigger,False)
	#擷取現在時間點與截止時間點
	starttime=time.time()
	stoptime=time.time()
	#恆為等待trigger時間點即時更新
	while GPIO.input(echo)==0:
		#重新設定起始時間
		starttime=time.time() #持續更新
	#有回朔了
	while GPIO.input(echo)==1:
		#持續更新回朔的最新時間
		stoptime=time.time()
	#trigger and echo形成一個循環
	#計算距離
	#經過時間
	pasedTime=stoptime-starttime
	distance=(pasedTime*34300)/2
	#判斷是否有近距離物件侵入了
	print('狀態:%s' %stop)
	if distance<=5 and stop==False:
		#判斷是否連續停車狀態
		
		global isParking
		isParking=True


		#亮起led 通知雲端訊息
		GPIO.output(ledpin,GPIO.HIGH)
		#取出系統日期與時間ISO8601格式,去毫秒數 UTC時區
		curdate=datetime.datetime.utcnow().replace(tzinfo=datetime.timezone.utc).isoformat()
		sensor=data.SensorDataEntity()
		sensor.id='park01'
		sensor.save=True
		sensor.lat=24.95
		sensor.lon=121.16
		sensor.time=curdate
		sensor.value=['ON'] #list狀態(另一個json array) 不是字串
		#物件如何轉換成Jsonarray串
		dict={'id':sensor.id,'time':sensor.time,'lat':sensor.lat,'lon':sensor.lon,'save':sensor.save,'value':sensor.value}
		
		
		list=[dict] #封裝成資料辭典 在序列化成json array
		jsonString=json.dumps(list)

	
		print(jsonString)
		#提出服務請求 傳送方式post
		try:
			#設定Key與傳送資料mime 格式
			header={'CK':apiKey,'Content-Type':'application/json'}
			#使用header加apikey 傳送json資料使用data parameter
			response=request.post(url,data=jsonString,headers=header) #防例外產生

			
			print('停車後處理之後的狀態碼:%s  %d' %(response.text,response.status_code))
			if response.status_code==200:
				print('停車資料已經上傳成功!!')
		except :
			print('發生錯誤')

		
			#print('資料:%s' %jsonString)
			#寫上雲端資料庫紀錄狀態
	elif distance>5 and isParking==True:
		GPIO.output(ledpin,GPIO.LOW)
		#停車位置閒置出來了
		global isParking
		isParking=False #沒有停車了
		#取出系統日期與時間ISO8601格式,去毫秒數 UTC時區
		curdate=datetime.datetime.utcnow().replace(tzinfo=datetime.timezone.utc).isoformat()
		sensor=data.SensorDataEntity()
		sensor.id='park01'
		sensor.save=True
		sensor.lat=24.95
		sensor.lon=121.16
		sensor.time=curdate
		sensor.value=['OFF'] #list狀態(另一個json array) 不是字串
			#物件如何轉換成Jsonarray串
		dict={'id':sensor.id,'time':sensor.time,'lat':sensor.lat,'lon':sensor.lon,'save':sensor.save,'value':sensor.value}
		
		
		list=[dict] #封裝成資料辭典 在序列化成json array
		jsonString=json.dumps(list)

	
		print(jsonString)
		#提出服務請求 傳送方式post
		try:
			#設定Key與傳送資料mime 格式
			header={'CK':apiKey,'Content-Type':'application/json'}
			#使用header加apikey 傳送json資料使用data parameter
			response=request.post(url,data=jsonString,headers=header) #防例外產生

			
			print('空車位之後的狀態碼:%s  %d' %(response.text,response.status_code))
			if response.status_code==200:
					print('停車資料已經上傳成功!!')
		except :
			print('發生錯誤')


	return distance

#執行緒的target delegate委派
def dhtSensor():
	#建構一個溫溼度物件
	dht=dht11.DHT11(pin=16)
	while True:
		#讀取溫溼度
		result=dht.read()
		if result.is_valid:
			#讀取溫溼度
			temp=result.temperature
			humi=result.humidity
			print('DHT temper:%d humi:%d' %(temp,humi));
		time.sleep(2)


#主程式
if __name__=='__main__':
	#建構一個Threading物件 委派程序
	dhtThread=threading.Thread(target=dhtSensor)
	#送進去執行緒集區
	dhtThread.start()

	global isParking
	isParking=False
	while True:
		
		dist=distance(isParking)
		print('距離:%f 公分' %dist)
		time.sleep(1)





