using MyService.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace MyService.Controllers
{
    public class SensorController : ApiController
    {
        //proxy 配置 device 到 sensor 最後感測資料
        [HttpGet]
        [HttpPost]
        [RouteAttribute("cht/Sensor/sensorLastData/deviceid/{deviceid}/sensor/{sensorid}")]
        public SensorData sensorLastData([FromUri]String deviceid, [FromUri]String sensorid)
        {
            //取得 Hosted
            String hostName = Models.AppKeyUtility.getKeyValue("hosted");
            String apikey = Models.AppKeyUtility.getKeyValue("apikey");
            String endPoint = String.Format($"{hostName}/device/{deviceid}/sensor/{sensorid}/rawdata");

            WebClient client = new WebClient();
            //header CK key
            client.Headers.Add("CK", apikey);
            //callback delegate (事件委派)
            client.DownloadStringCompleted += downloadComplete; //callback function

            //client.DownloadStringAsync(new Uri(endPoint));

            //String result = await client.DownloadStringTaskAsync(endPoint);

            String result = client.DownloadString(endPoint);

            //JObject obj = Newtonsoft.Json.JsonConvert.DeserializeObject(result) as JObject;
            //result = obj.GetValue("time").ToString();
            //JToken jt = obj.GetValue("value"); //不是字串也不是一個整數[....] Json Array
            //result = jt[0].ToString(); //jt[indexer] 內建集合物件 使用索引子語法[]--特殊類型屬性

            SensorData data = JsonConvert.DeserializeObject(result, typeof(SensorData))
                as SensorData;
            return data;
        }
        //事件程序
        private void downloadComplete(object sender, DownloadStringCompletedEventArgs e)
        {
            String jsonString = e.Result;
            System.Console.WriteLine("This is callback");
        }
    }
}
