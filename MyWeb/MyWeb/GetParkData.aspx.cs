using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyWeb
{
    public partial class GetParkData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected async void btnGetParking_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                //HttpResponse response = this.Response;

                //response.Write("按鈕被觸發" + ((Button)sender).ID );
            }

            HttpClient client = new HttpClient();
            HttpRequestHeaders headers = client.DefaultRequestHeaders;
            headers.Add("CK", Models.AppUtility.getKey("ck"));

            HttpResponseMessage response = await client.GetAsync(Models.AppUtility.getKey("park01"));

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                txtStatus.Text = json;

                Object obj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                var jsonObject = obj as Newtonsoft.Json.Linq.JObject;
                var jsonArray = jsonObject.GetValue("value") as Newtonsoft.Json.Linq.JArray;
                txtStatus.Text = jsonArray[0].ToString();

            }
        }
    }
}