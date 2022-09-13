using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Notification : System.Web.UI.Page
{
    Connectioncls ConCls = new Connectioncls();
    private readonly object YOUR_FCM_DEVICE_ID;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnsendnotification_Click(object sender, EventArgs e)
    {
        SendMessage(txttitle.Text, txtMessage.Text,txtimgurl.Text,txticonurl.Text);
    }

    public class ClassName
    {
        public string Register_device_ID { get; set; }
    }
    public string SendMessage(string title, string message,string imageurl,string iconurl)
    {
        string serverKey = "AAAAZBsmWzs:APA91bFBDa0VH7WtfaddhaxgZGAG_qpcM8JKZHoBSxnP12L_tJF9wqKIkr8DFxN92tQ0sty8xMiZogLWoHhtQThMeM2b97__xizePpJCzlH0D8ESyXY7QHpcNuKYTubU0Zgz85Fc4qaM";

        try
        {
            var result = "-1";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString);




            List<string> list = new List<string>();

            using (SqlDataReader reader = ConCls.GetdataReader("select Register_device_ID from NotificationDeviceID"))
            {
                while (reader.Read())
                {
                    list.Add(reader["Register_device_ID"].ToString());
                }
            }
            string[] arr = list.ToArray();

            var requestUri = "https://fcm.googleapis.com/fcm/send";

            WebRequest webRequest = WebRequest.Create(requestUri);
            webRequest.Method = "POST";
            webRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
            webRequest.Headers.Add(string.Format("Sender: id={0}", "429952228155"));
            webRequest.ContentType = "application/json";

            var data = new
            {
                 to = YOUR_FCM_DEVICE_ID, // Uncoment this if you want to test for single device
                registration_ids = arr, // this is for topic 
                data = new
                {
                    title = title,
                    message = message,
                    is_background = "false",
                    icon = iconurl,
                    image = imageurl,
                    timestamp = System.DateTime.Now.ToString(),
                    priority=10,
                    payload = new
                    {

                    },
                    notification = new
                    {
                        title = title,
                        sound = "default",
                        vibrate = "true",
                    }
                }
            };


            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(data);

            Byte[] byteArray = Encoding.UTF8.GetBytes(json);

            webRequest.ContentLength = byteArray.Length;
            using (Stream dataStream = webRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);

                using (WebResponse webResponse = webRequest.GetResponse())
                {
                    using (Stream dataStreamResponse = webResponse.GetResponseStream())
                    {
                        using (StreamReader tReader = new StreamReader(dataStreamResponse))
                        {
                            String sResponseFromServer = tReader.ReadToEnd();
                            // result.Response = sResponseFromServer;
                            //txttitle.Text = "";
                            //txtMessage.Text = "";
                            lblStatus.Text = sResponseFromServer;
                            return sResponseFromServer;
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
            lblStatus.Text = ex.ToString();
            return "err";
        }
    }
}