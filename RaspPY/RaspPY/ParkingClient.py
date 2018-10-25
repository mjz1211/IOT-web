import paho.mqtt.client as mqttClient
import json
import pyodbc as odbc
import datetime as datetime
import time as time
#建構一個mqtt client物件
host="iot.cht.com.tw"
id='lens_4gzOzxSqMnsu4ZVKr9f2TsKcaaa'
username='PKWVLF9JM7MOIEFS40'
password='PKWVLF9JM7MOIEFS40'
#azure sql server資訊
server='tainandb.database.windows.net'
dbusername='brette0119'
dbpassword='eAeB#0119'
database='tainandb'


#設定訂閱MQTT function後通知
def onconnect(client,userdata,flags,rc):
	
	print('連接成功!!')
	#進行訂閱主題
	client.subscribe('/v1/device/5604707091/sensor/park01/rawdata')

#message callback
def callback(client,userdata,msg):
	dataString=msg.payload.decode('UTF-8')
	
	#透過json 將字串封裝成json物件
	jsonobject=json.loads(dataString)	#dict
	print('主題:%s 內容:%s' %(msg.topic,jsonobject))	
	#記錄送到azure新增
	try:
		#透過連接物件取出cursor
		cursor=connect.cursor()
		print(type(cursor)) #pyodbc.Cursor
		#準備新增語法 
		iso8601=jsonobject['time']	
		#iso8601字串格式轉換成datetime物件 要設定pattern
		curdate=datetime.datetime.strptime(iso8601,'%Y-%m-%dT%H:%M:%S.%fZ')
		print(type(curdate)) #datetime.datetime
		localeDay=curdate.strftime('%Y-%m-%d %H:%M:%S')
		print(localeDay)
		sql="insert into PyHSR04(sensorid,status,location,late,long,createdate) values(?,?,?,?,?,?)"
		param=(jsonobject['id'],jsonobject['value'][0],'classroom',jsonobject['lat'],jsonobject['lon'],localeDay) #tuple資料當作參數
		print(type(param))
		result=cursor.execute(sql,param)
		print(jsonobject['value'])
		cursor.commit()
		print('新增記錄成功')
	except Exception as  e:
		print('發生錯誤:%s' %e)	


#print(type(jsonobject)) #dict類型
#主程式
if __name__=='__main__':
	#main program
	#建立連接雲端資料庫的連接物件
	#連接字串描述架構:DRIVER={機器安裝的odbc driver版本描述};SERVER=;DATABASE=;UID=;PWD=
	try:
		connectingString='DRIVER={ODBC Driver 13 for SQL Server};SERVER='+server+';DATABASE='+database+';UID='+dbusername+';PWD='+dbpassword
		
		connect=odbc.connect(connectingString)
		print(type(connect)) 
		#pyodbc.Connection 
		#是否開啟連接資料庫的連接物件


		client=mqttClient.Client(id,clean_session=True)
		#設定MQTT username as password
		client.username_pw_set(username,password)
		#設定聆聽事件程序
		client.on_connect=onconnect
		client.on_message=callback
		print(type(client)) #paho.mqtt.client.Client
		#建立連接
		client. connect(host,port=1883)
		client.loop_forever()
	except:
		print('資料庫/MQTT連接有問題');


