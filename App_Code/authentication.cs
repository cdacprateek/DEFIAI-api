using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net.Mime;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections.Specialized;
using System.Net;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml;

/// <summary>
/// Summary description for authentication
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class authentication : System.Web.Services.WebService {
    Connectioncls ConCls = new Connectioncls();
    string ErrCode = "";
    public authentication () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }



    [WebMethod]
    public void loginauthenticate(string CusID, string Password, string token)
    {
        try
        {
            string message1 = "";
            string ErrCode1 = "";
            string tokenno = token;
            string statusflag = "0";
            string serviceaction = "loginauthenticate";
            string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                ConCls.wallet_login(CusID, Password, tokenno, serviceaction, out statusflag, out message1, out ErrCode1);

                if (statusflag == "1")
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT CusID as [Loginid], cust_name as name ,  convert(varchar, Entry_Date, 6) as doj, email as [email], cust_mobileno as mobileno, Cust_City as city,  CASE WHEN pin_Approved =0 THEN 'N' WHEN Approved >=1 THEN 'Y'  END AS isactive from custrecords where CusID = " + CusID + ""))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "custrecords";
                                sda.Fill(dt);
                                var json = JsonConvert.SerializeObject(dt);
                                //return dt;

                                dynamic jsonResponse = JsonConvert.DeserializeObject(json);

                                JArray jarray = new JArray(jsonResponse);


                                Showdata show = new Showdata();
                                show.result = "true";
                                show.data = jarray;


                                string json1 = JsonConvert.SerializeObject(show);
                                this.Context.Response.Write(json1);
                            }
                        }
                    }
                }
                else
                {
                    Service_Stop service = new Service_Stop();
                    service.result = "false";
                    service.message = ErrCode1;


                    string json = JsonConvert.SerializeObject(service);
                    this.Context.Response.Write(json);
                }

            }
        }
        catch (Exception ex)
        {
            Service_Stop service = new Service_Stop();
            service.result = "false";
            service.message = ErrCode;


            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);
        }
    }


     

    [WebMethod]
    public void loginauthenticateauto(string mobileid, string IPaddress, string token)
    {
        try
        {
            string message1 = "";
            string ErrCode1 = "";
            string tokenno = token;
            string statusflag = "0";
            string serviceaction = "loginauthenticate";
            string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                ConCls.loginauthenticateauto(mobileid, IPaddress, tokenno, serviceaction, out statusflag, out message1, out ErrCode1);

                if (statusflag == "1")
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT CusID as [Loginid], cust_name as name ,  convert(varchar, Entry_Date, 6) as doj, email as [email], cust_mobileno as mobileno, Cust_City as city,  CASE WHEN pin_Approved =0 THEN 'N' WHEN Approved >=1 THEN 'Y'  END AS isactive from custrecords where CusID = " + mobileid + ""))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "custrecords";
                                sda.Fill(dt);
                                var json = JsonConvert.SerializeObject(dt);
                                //return dt;

                                dynamic jsonResponse = JsonConvert.DeserializeObject(json);

                                JArray jarray = new JArray(jsonResponse);


                                Showdata show = new Showdata();
                                show.result = "true";
                                show.data = jarray;


                                string json1 = JsonConvert.SerializeObject(show);
                                this.Context.Response.Write(json1);
                            }
                        }
                    }
                }
                else
                {
                    Service_Stop service = new Service_Stop();
                    service.result = "false";
                    service.message = ErrCode1;


                    string json = JsonConvert.SerializeObject(service);
                    this.Context.Response.Write(json);
                }

            }
        }
        catch (Exception ex)
        {
            Service_Stop service = new Service_Stop();
            service.result = "false";
            service.message = ErrCode;


            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);
        }
    }

}
