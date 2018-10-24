using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.Models
{
    public class AppUtility
    {
        public static String getKey(String keyName)
        {
            return System.Web.Configuration.WebConfigurationManager.AppSettings[keyName];
        }
    }
}