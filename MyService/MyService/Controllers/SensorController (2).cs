using MyService.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyService.Controllers
{
    public class SensorController : ApiController
    {
        //Proxy 配置到device 到sensor最後感測資料 預設為queryString
        [HttpGet]
        [HttpPost]
        [RouteAttribute("cht/Sensor/sensorLastData/deviceid/{deviceid}/sensor/{sensorid}")]
        public SensorData sensorLastData([FromUri] String deviceid, [FromUri]String sensorid)
        {
            //整理正式CHT IOT 端點 再提出REST請求
            //取得Hosted
            String hostName=Models.AppKeyUtility.getKeyValue("hosted");
            String endPoint=String.Format($"{hostName}/device/{deviceid}/sensor/{sensorid}/rawdata");
            WebClient client = new WebClient();
            //Header金鑰 /Http Method??
            client.Headers.Add("CK", Models.AppKeyUtility.getKeyValue("apikey"));
            //callback delegate(事件委派)
            client.DownloadStringCompleted += downLoadComplete; //配合非同步處理的callback 
            //client.DownloadStringAsync(new Uri(endPoint)); 非同步
            String result=client.DownloadString(endPoint); //sync 同步
            //處理字串如何Parser Convert to 物件Newton json
            //JObject obj = Newtonsoft.Json.JsonConvert.DeserializeObject(result) as JObject;
            //String time = obj.GetValue("time").ToString();
            //JToken jt = obj.GetValue("value"); //不是字串也不是一個整數[....] Json Array
            //result= jt[0].ToString(); //jt[indexer] 內建集合物件 使用索引子語法[]--特殊類型屬性

            //===========================part2=================================
            SensorData data=Newtonsoft.Json.JsonConvert.DeserializeObject(result, typeof(SensorData)) 
                as SensorData;
            return data;
          }

        //事件程序
        public void downLoadComplete(Object sender, DownloadStringCompletedEventArgs e)
        {
            //透過參數取出內容
            String jsonString=e.Result;
        }

    }
}
