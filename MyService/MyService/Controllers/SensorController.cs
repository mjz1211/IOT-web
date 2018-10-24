using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MyService.Controllers
{
    public class SensorController : ApiController
    {
        //proxy 配置 device 到 sensor 最後感測資料
        [HttpGet]
        [RouteAttribute("cht/Sensor/sensorLastData/deviceid/{deviceid}/sensor/{sensorid}")]
        public async Task<String> sensorLastData([FromUri]String deviceid, [FromUri]String sensorid)
        {
            //取得 Hosted
            String hostName = Models.AppKeyUtility.getKeyValue("hosted");
            String endPoint = String.Format($"{hostName}/device/{deviceid}/sensor/{sensorid}/rawdata");

            WebClient client = new WebClient();
            //header CK key
            client.Headers.Add("CK", "DKGM95TKZK3FYUKKEU");
            //callback delegate (事件委派)
            client.DownloadStringCompleted += downloadComplete;

            //client.DownloadStringAsync(new Uri(endPoint));

            String result = await client.DownloadStringTaskAsync(endPoint);

            return endPoint;
        }
        //事件程序
        private void downloadComplete(object sender, DownloadStringCompletedEventArgs e)
        {
            String jsonString = e.Result;
        }
    }
}
