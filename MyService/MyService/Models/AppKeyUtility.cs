using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyService.Models
{
    public class AppKeyUtility
    {
        public static String getKeyValue(String keyName)
        {
            return System.Web.Configuration.WebConfigurationManager.AppSettings[keyName];
        }
    }
}