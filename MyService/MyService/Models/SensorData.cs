using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyService.Models
{
    public class SensorData
    {
        public String id { set; get; }
        public String deviceId { set; get; }
        public DateTime time { set; get; }
        //public Double lon { set; get; }
        //public Double lat { set; get; }
        public List<String> value { set; get; }

    }
}