using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;



/// <summary>
/// Summary description for tskwebservices
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class tskwebservices : System.Web.Services.WebService
{
    private Connectioncls ConCls = new Connectioncls();
    private string ErrCode = "";
    private string CustID;
    private string message;
    private string NewID;
    private readonly string device_id;

    public tskwebservices()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    #region #TestAppHttps
    [WebMethod]
    public string TestAppHttps()
    {
        return "Hello World - Success - Test Success";
    }
    #endregion

    #region #wallet_services
    [WebMethod]
    public void wallet_services(string CusID, string ewpws, string token, string action, string Wallet_type, string amount, string description)
    {
        try
        {
            string message1 = "";
            string ErrCode1 = "";
            string tokenno = token;
            string statusflag = "0";
            string serviceaction = action;

            string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                ConCls.wallet_services(CusID, ewpws, tokenno, serviceaction, Wallet_type, amount, description, out statusflag, out message1, out ErrCode1);

                if (statusflag == "1")
                {

                    //using (SqlCommand cmd = new SqlCommand("SELECT CusID,Cust_Name,Email,Cust_mobileNo,credit,debit,balance FROM CustRecords INNER JOIN e_wallet_coin ON CustRecords.CusID = e_wallet_coin.[CusID] where  marketid = 5 and CustRecords.CusID=" + CusID + ""))
                    //{
                    //    using (SqlDataAdapter sda = new SqlDataAdapter())
                    //    {
                    //        cmd.Connection = con;
                    //        sda.SelectCommand = cmd;
                    //        using (DataTable dt = new DataTable())
                    //        {
                    //            dt.TableName = "e_wallet_coin";
                    //            sda.Fill(dt);
                    //            var json = JsonConvert.SerializeObject(dt);
                    //            //return dt; 
                    //            dynamic jsonResponse = JsonConvert.DeserializeObject(json); 
                    //            JArray jarray = new JArray(jsonResponse); 
                    //            Showdata show = new Showdata();
                    //            show.result = "true";
                    //            show.Data = jarray; 
                    //            string json1 = JsonConvert.SerializeObject(show);
                    //            this.Context.Response.Write(json1);
                    //        }
                    //    }
                    //} 

                    Service_Stop service = new Service_Stop();
                    service.result = message1;
                    service.message = ErrCode1;
                    // service.message = message1;

                    string json = JsonConvert.SerializeObject(service);
                    this.Context.Response.Write(json);
                }
                else
                {
                    Service_Stop service = new Service_Stop();
                    //service.result = "false";
                    //service.message = "Invalid Token";
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

    #endregion

    #region #Wallet_balance
    [WebMethod]
    public void Wallet_balance(string CusID, string token, string wallet_type)
    {
        try
        {
            string message1 = "";
            string ErrCode1 = "";
            string tokenno = token;
            string wallet_ty = wallet_type;
            string statusflag = "0";
            string serviceaction = "utlility_balance_check";
            string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                ConCls.tokencheck(CusID, tokenno, serviceaction, out statusflag, out message1, out ErrCode1);

                if (statusflag == "1")
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT CusID as [Loginid], 'OK' as response , balance as ewallet, 'success' as msg FROM CustRecords INNER JOIN e_wallet_coin ON CustRecords.CusID = e_wallet_coin.[CusID] where  marketid = " + wallet_ty + " and CustRecords.CusID=" + CusID + ""))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "CustRecords";
                                sda.Fill(dt);
                                string json = JsonConvert.SerializeObject(dt);
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
    #endregion

    #region #Wallet_login
    [WebMethod]
    public void Wallet_login(string CusID, string Password, string token)
    {
        try
        {
            string message1 = "";
            string ErrCode1 = "";
            string tokenno = token;
            string statusflag = "0";
            string serviceaction = "Wallet_login";
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
                                string json = JsonConvert.SerializeObject(dt);
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
    #endregion

    #region #fetchUserByID
    [WebMethod]
    //First Get API-29-08-22
    public void fetchUserByID(int Cust_ID, string tokenno)
    {
        try
        {


            string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                if (tokenno != "")
                {
                    using (SqlCommand cmd = new SqlCommand("select * from CustRecords where CusID=" + Cust_ID + ""))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "CustRecords";
                                sda.Fill(dt);

                                string json = JsonConvert.SerializeObject(dt);
                                //return dt;

                                dynamic jsonResponse = JsonConvert.DeserializeObject(json);

                                JArray jarray = new JArray(jsonResponse);


                                SqlParameter SqlParaNewID = new SqlParameter("@status", SqlDbType.VarChar, 500);
                                SqlParaNewID.Direction = ParameterDirection.Output;
                                cmd.Parameters.Add(SqlParaNewID).Value = "";



                                JObject jd = new JObject(
                            new JProperty("result", "true"),
                            new JProperty("Data", jarray));

                                string JsonString = jd.ToString();

                                this.Context.Response.Clear();
                                JsonString = JsonConvert.SerializeObject(jd).ToString();
                                this.Context.Response.ContentType = "application/json";
                                this.Context.Response.AddHeader("content-length", JsonString.Length.ToString());
                                this.Context.Response.Flush();
                                this.Context.Response.Write(JsonString);
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            JObject jd = new JObject();
            jd.Add("result", "");
            JObject val = jd;
            string option = (string)val["result"];
            string JsonString = "";

            val = jd;
            option = (string)val["result"];
            //           val["status"] = status;
            val["result"] = "false";
            val["message"] = "Bad network connection. please try after some time";

            this.Context.Response.Clear();
            JsonString = JsonConvert.SerializeObject(jd).ToString();
            this.Context.Response.ContentType = "application/json";
            this.Context.Response.AddHeader("content-length", JsonString.Length.ToString());
            this.Context.Response.Flush();
            this.Context.Response.Write(JsonString);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
    #endregion

    #region #UserRegistration
    [WebMethod]
    public void UserRegistration(int serialno, int CusID, float Cust_SponserID, string Cust_Location, DateTime Entry_Date, string Cust_Name, string Email, string Cust_Country, Int64 Cust_mobileNo, string Cust_Password, string Cust_Package, int Approved, int Pin_Approved, int plannerId, int totaldir, string tokenno)
    {
        try
        {


            string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                ConCls.UserRegistration(serialno, CusID, Cust_SponserID, Cust_Location, Entry_Date, Cust_Name, Email, Cust_Country, Cust_mobileNo, Cust_Password, Cust_Package, Approved, Pin_Approved, plannerId, totaldir, tokenno);

                if (Cust_Location == "" || Cust_Name == "" || Email == "" || Cust_Country == "" || Cust_Password == "")

                {

                    Service_Stop service = new Service_Stop();
                    string json = JsonConvert.SerializeObject(service);
                    this.Context.Response.Write(json);
                    service.message = ErrCode;
                    service.result = message;
                   
                }
                else
                {

                    using (SqlCommand cmd = new SqlCommand("INSERT INTO CustRecords(serialno,CusID,Cust_SponserID,Cust_Location,Entry_Date,Cust_Name,Email,Cust_Country,Cust_mobileNo,Cust_Password,Cust_Package,Approved,Pin_Approved,plannerId,totaldir) VALUES (@serialno,@CusID,@Cust_SponserID, @Cust_Location,'" + DateTime.Now.ToString() + "',@Cust_Name, @Email, @Cust_Country, @Cust_mobileNo, @Cust_Password,@Cust_Package,@Approved,@Pin_Approved,@plannerId,@totaldir)"))
                   

                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            //cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "CustRecords";
        //                        sda.Fill(dt);
//      //                          return dt; 
                                dynamic jsonResponse = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dt));
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

            }
        }
        catch (Exception ex)
        {
            Service_Stop service = new Service_Stop();
            service.result = message;
            service.message = ErrCode;

            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);
        }


    }
    #endregion

    #region #UserLogin
    [WebMethod]
    public void UserLogin(string Email, string Cust_Password, string tokenno)
    {

        try
        {

            string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                ConCls.UserLogin(Email, Cust_Password, tokenno, out message, out ErrCode);

                if (Email != "" || Cust_Password != "" || tokenno != "")
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT loginusername,CusID FROM CustRecords WHERE loginusername='" + Email + "' AND Cust_Password='" + Cust_Password + "'"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "custrecords";
                                sda.Fill(dt);
                                string json = JsonConvert.SerializeObject(dt);
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

                    service.message = ErrCode;


                    string json = JsonConvert.SerializeObject(service);
                    this.Context.Response.Write(json);
                }

            }
        }
        catch (Exception ex)
        {
            Service_Stop service = new Service_Stop();
            service.result = "false";



            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);
        }
    }
    #endregion
}