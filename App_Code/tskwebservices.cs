using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
    Utility ud = new Utility();
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
            if (Cust_ID != null)
            {
                if (tokenno != "")
                {
                    string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
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



                                    JObject rjd = new JObject(
                                    new JProperty("result", "true"),
                                    new JProperty("Data", jarray));
                                    this.Context.Response.Clear();


                                    //JsonString = JsonConvert.SerializeObject(rjd).ToString();
                                    //this.Context.Response.ContentType = "application/json";
                                    //this.Context.Response.AddHeader("content-length", JsonString.Length.ToString());
                                    //this.Context.Response.Flush();
                                    //this.Context.Response.Write(JsonString);
                                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                                }



                            }
                        }
                    }
                }
                JObject t = new JObject();
                t.Add("result", "");
                JObject tval = t;
                string toption = (string)tval["result"];
                string tJsonString = "";

                tval = t;
                toption = (string)tval["result"];
                //           val["status"] = status;
                tval["result"] = "false";
                tval["message"] = "Please Enter Token_no";
            }
            JObject jd = new JObject();
            jd.Add("result", "");
            JObject val = jd;
            string option = (string)val["result"];
            string JsonString = "";

            val = jd;
            option = (string)val["result"];
            //           val["status"] = status;
            val["result"] = "false";
            val["message"] = "Please enter CustomerID";

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

    //#region #UserRegistration
    //[WebMethod]
    //public void UserRegistration(int serialno, int CusID, int Cust_SponserID, string Cust_Location, DateTime Entry_Date, string Cust_Name, string Email, string Cust_Country, Int64 Cust_mobileNo, string Cust_Password, string Cust_Package, int Approved, int Pin_Approved, int plannerId, int totaldir, string tokenno)
    //{
    //    try
    //    {
    //        string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
    //        using (SqlConnection con = new SqlConnection(constr))
    //        {
    //            ConCls.UserRegistration(serialno, CusID, Cust_SponserID, Cust_Location, Entry_Date, Cust_Name, Email, Cust_Country, Cust_mobileNo, Cust_Password, Cust_Package, Approved, Pin_Approved, plannerId, totaldir, tokenno);


    //            if (Cust_SponserID != null)
    //            {
    //                if (Cust_Location != "")
    //                {
    //                    if (Entry_Date != null)
    //                    {
    //                        if (Cust_Name != null)
    //                        {
    //                            if (Email != "")
    //                            {
    //                                if (Cust_Country != "")
    //                                {
    //                                    if (Cust_mobileNo != null)
    //                                    {
    //                                        if (Cust_Password != "")
    //                                        {
    //                                            if (Cust_Package != "")
    //                                            {
    //                                                if (Approved != null)
    //                                                {
    //                                                    if (Pin_Approved != null)
    //                                                    {
    //                                                        if (plannerId != null)
    //                                                        {
    //                                                            if (totaldir != null)
    //                                                            {
    //                                                                if (tokenno != null)
    //                                                                {



    //                                                                    using (SqlCommand cmd = new SqlCommand("INSERT INTO CustRecords(serialno,CusID,Cust_SponserID,Cust_Location,Entry_Date,Cust_Name,Email,Cust_Country,Cust_mobileNo,Cust_Password,Cust_Package,Approved,Pin_Approved,plannerId,totaldir) VALUES (@serialno,'" + CusID.ToString() + "',@Cust_SponserID,@Cust_Location,'" + DateTime.Now.ToString() + "',@Cust_Name, @Email, @Cust_Country,'" + Cust_mobileNo.ToString() + "', @Cust_Password,0,1,0,0,1,Do9234iwenas#$!@1*s9)"))

    //                                                                    {
    //                                                                        using (SqlDataAdapter sda = new SqlDataAdapter())
    //                                                                        {
    //                                                                            //cmd.Connection = con;
    //                                                                            sda.SelectCommand = cmd;
    //                                                                            using (DataTable dt = new DataTable())
    //                                                                            {
    //                                                                                dt.TableName = "CustRecords";
    //                                                                                sda.Fill(dt);
    //                                                                                //                                                               return dt; 
    //                                                                                dynamic jsonResponse = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dt));
    //                                                                                JArray jarray = new JArray(jsonResponse);
    //                                                                                Showdata show = new Showdata();
    //                                                                                show.result = "true";
    //                                                                                show.data = jarray;
    //                                                                                string json1 = JsonConvert.SerializeObject(show);
    //                                                                                this.Context.Response.Write(json1);
    //                                                                            }
    //                                                                        }
    //                                                                    }
    //                                                                }

    //                                                                JObject tot = new JObject();
    //                                                                tot.Add("result", "");
    //                                                                JObject totval = tot;
    //                                                                string totoption = (string)tot["result"];
    //                                                                string totJsonString = "";

    //                                                                totval = tot;
    //                                                                totoption = (string)totval["result"];
    //                                                                //           val["status"] = status;
    //                                                                totval["result"] = "false";
    //                                                                totval["message"] = "Please Enter tokenno";



    //                                                            }
    //                                                            JObject tdir = new JObject();
    //                                                            tdir.Add("result", "");
    //                                                            JObject tdirval = tdir;
    //                                                            string tdiroption = (string)tdirval["result"];
    //                                                            string tdirJsonString = "";

    //                                                            tdirval = tdir;
    //                                                            tdiroption = (string)tdirval["result"];
    //                                                            //           val["status"] = status;
    //                                                            tdirval["result"] = "false";
    //                                                            tdirval["message"] = "Please Enter TotalDIR";
    //                                                        }
    //                                                        JObject tpdin = new JObject();
    //                                                        tpdin.Add("result", "");
    //                                                        JObject tdpinval = tpdin;
    //                                                        string tdpinoption = (string)tdpinval["result"];
    //                                                        string tdpJsonString = "";

    //                                                        tdpinval = tpdin;
    //                                                        tdpinoption = (string)tdpinval["result"];
    //                                                        //           val["status"] = status;
    //                                                        tdpinval["result"] = "false";
    //                                                        tdpinval["message"] = "Please Enter PlannerID";
    //                                                    }

    //                                                    JObject tpin = new JObject();
    //                                                    tpin.Add("result", "");
    //                                                    JObject tpinval = tpin;
    //                                                    string tpinoption = (string)tpinval["result"];
    //                                                    string tpinJsonString = "";

    //                                                    tpinval = tpin;
    //                                                    tpinoption = (string)tpinval["result"];
    //                                                    //           val["status"] = status;
    //                                                    tpinval["result"] = "false";
    //                                                    tpinval["message"] = "Please Enter PIN_APPROVED";
    //                                                }
    //                                                JObject tapp = new JObject();
    //                                                tapp.Add("result", "");
    //                                                JObject tappval = tapp;
    //                                                string tappoption = (string)tappval["result"];
    //                                                string tappJsonString = "";

    //                                                tappval = tapp;
    //                                                tappoption = (string)tappval["result"];
    //                                                //           val["status"] = status;
    //                                                tappval["result"] = "false";
    //                                                tappval["message"] = "Please Enter APPROVED";
    //                                            }
    //                                            JObject tcp = new JObject();
    //                                            tcp.Add("result", "");
    //                                            JObject tppval = tcp;
    //                                            string toption = (string)tppval["result"];
    //                                            string tcpJsonString = "";

    //                                            tppval = tcp;
    //                                            toption = (string)tppval["result"];
    //                                            //           val["status"] = status;
    //                                            tppval["result"] = "false";
    //                                            tppval["message"] = "Please Enter Customer Package";
    //                                        }
    //                                        JObject tp = new JObject();
    //                                        tp.Add("result", "");
    //                                        JObject tcpval = tp;
    //                                        string tpoption = (string)tcpval["result"];
    //                                        string tpJsonString = "";

    //                                        tcpval = tp;
    //                                        tpoption = (string)tcpval["result"];
    //                                        //           val["status"] = status;
    //                                        tcpval["result"] = "false";
    //                                        tcpval["message"] = "Please Enter Customer Password";

    //                                    }
    //                                    JObject tmo = new JObject();
    //                                    tmo.Add("result", "");
    //                                    JObject tmopval = tmo;
    //                                    string tmooption = (string)tmopval["result"];
    //                                    string tmJsonString = "";

    //                                    tmopval = tmo;
    //                                    tmooption = (string)tmopval["result"];
    //                                    //           val["status"] = status;
    //                                    tmopval["result"] = "false";
    //                                    tmopval["message"] = "Please Enter Mobile Number";
    //                                }
    //                                JObject tco = new JObject();
    //                                tco.Add("result", "");
    //                                JObject tcoval = tco;
    //                                string cooption = (string)tcoval["result"];
    //                                string tcoJsonString = "";

    //                                tcoval = tco;
    //                                cooption = (string)tcoval["result"];
    //                                //           val["status"] = status;
    //                                tcoval["result"] = "false";
    //                                tcoval["message"] = "Please Enter country";

    //                            }
    //                            JObject email = new JObject();
    //                            email.Add("result", "");
    //                            JObject emailval = email;
    //                            string tcooption = (string)emailval["result"];
    //                            string tcJsonString = "";

    //                            emailval = email;
    //                            tcooption = (string)emailval["result"];
    //                            //           val["status"] = status;
    //                            emailval["result"] = "false";
    //                            emailval["message"] = "Please Enter Email";

    //                        }
    //                        JObject name = new JObject();
    //                        name.Add("result", "");
    //                        JObject nval = name;
    //                        string nmoption = (string)nval["result"];
    //                        string nmJsonString = "";

    //                        nval = name;
    //                        nmoption = (string)nval["result"];
    //                        //           val["status"] = status;
    //                        nval["result"] = "false";
    //                        nval["message"] = "Please Enter Name";

    //                    }
    //                    JObject date = new JObject();
    //                    date.Add("result", "");
    //                    JObject dval = date;
    //                    string doption = (string)dval["result"];
    //                    string dJsonString = "";

    //                    dval = date;
    //                    doption = (string)dval["result"];
    //                    //           val["status"] = status;
    //                    dval["result"] = "false";
    //                    dval["message"] = "Please Enter Date";

    //                }
    //                JObject loc = new JObject();
    //                loc.Add("result", "");
    //                JObject lval = loc;
    //                string loption = (string)lval["result"];
    //                string lJsonString = "";

    //                lval = loc;
    //                loption = (string)lval["result"];
    //                //           val["status"] = status;
    //                lval["result"] = "false";
    //                lval["message"] = "Please Enter location";

    //            }
    //            JObject sid = new JObject();
    //            sid.Add("result", "");
    //            JObject sval = sid;
    //            string soption = (string)sval["result"];
    //            string sJsonString = "";

    //            sval = sid;
    //            soption = (string)sval["result"];
    //            //           val["status"] = status;
    //            sval["result"] = "false";
    //            sval["message"] = "Please Enter Sponsor ID";

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        JObject jd = new JObject();
    //        jd.Add("result", "");
    //        JObject val = jd;
    //        string option = (string)val["result"];
    //        string JsonString = "";

    //        val = jd;
    //        option = (string)val["result"];
    //        //           val["status"] = status;
    //        val["result"] = "false";
    //        val["message"] = "Bad network connection. please try after some time";

    //        this.Context.Response.Clear();
    //        JsonString = JsonConvert.SerializeObject(jd).ToString();
    //        this.Context.Response.ContentType = "application/json";
    //        this.Context.Response.AddHeader("content-length", JsonString.Length.ToString());
    //        this.Context.Response.Flush();
    //        this.Context.Response.Write(JsonString);
    //        HttpContext.Current.ApplicationInstance.CompleteRequest();
    //    }
    //}
    //#endregion

    public void RegistrationPre(string token, string CusID, string SponserID, string LeftRight, string Name, string Email, string Country, string CountryCode, string MobileNo, string Password, string TxPassword)
    {
        try
        {
            if (CusID == "")
            {
                Service_Stop service = new Service_Stop();
                service.result = "FAILED";
                service.message = "Please enter Cust ID";
                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);
                return;
            }
            if (CusID.All(char.IsDigit) == false)
            {
                Service_Stop service = new Service_Stop();
                service.result = "FAILED";
                service.message = "Please Enter Cust ID is only Number";
                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);
                return;
            }
            if (Name.Trim().ToString() == "" || Email.Trim().ToString() == "" || Password.Trim().ToString() == "" || TxPassword.Trim().ToString() == "" || SponserID.Trim().ToString() == "" || MobileNo.Trim() == "")
            {
                Service_Stop service = new Service_Stop();
                service.result = "FAILED";
                service.message = "Please Enter All Values";
                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);
                return;
            }
            string message1 = "";
            string ErrCode1 = "";
            string tokenno = token;
            string statusflag = "0";
            string serviceaction = "RegistrationPre";
            string APITitle = "RegistrationPre-API";
            string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                ConCls.LOGWebApi(SponserID, tokenno, serviceaction, APITitle, out statusflag, out message1, out ErrCode1);

                if (statusflag == "1")
                {
                    string Newid = "0";
                    string Errocode = "";
                    ConCls.inserrtnewcustrecords(SponserID, Email, "", "", "", "", "", Country, Password, "", Name, "", "", "", "", MobileNo, "", "", "", "", "", 0, "", "", "", "", "", "", TxPassword, out Newid, out Errocode);
                    if (Newid.Length > 4)
                    {
                        string rotp = "";
                        rotp = Newid.ToString();
                        ud.SendSMS(Newid, Name, Email, "", rotp);
                        var result = char.ConvertFromUtf32(34);
                        var json = "[{" + result + "CustID:" + result + ":" + result + Newid + result + "," + result + "OTP:" + result + ":" + result + rotp + result + "," + result + "SponserID:" + result + ":" + result + SponserID + result + "}]";
                        dynamic jsonResponse = JsonConvert.DeserializeObject(json);
                        JArray jarray = new JArray(jsonResponse);
                        ShowdataNew show = new ShowdataNew();
                        show.result = "SUCCESS";
                        show.Message = "Send OTP Successfully";
                        show.Statuscode = "200";
                        show.Data = jarray;
                        string json1 = JsonConvert.SerializeObject(show);
                        this.Context.Response.Write(json1);
                    }
                }
                else
                {
                    Service_Stop service = new Service_Stop();
                    service.result = "FAILED";
                    service.message = ErrCode1;
                    string json = JsonConvert.SerializeObject(service);
                    this.Context.Response.Write(json);
                }

            }
        }
        catch (Exception ex)
        {
            Service_Stop service = new Service_Stop();
            service.result = "FAILED";
            service.message = ex.Message;
            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);
        }
    }

    [WebMethod]
    public void Registration(string token, string CusID, string SponserID, string OTP)
    {
        try
        {
            if (CusID == "")
            {
                Service_Stop service = new Service_Stop();
                service.result = "FAILED";
                service.message = "Please enter Cust ID";
                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);
                return;
            }
            if (CusID.All(char.IsDigit) == false)
            {
                Service_Stop service = new Service_Stop();
                service.result = "FAILED";
                service.message = "Please Enter Cust ID is only Number";
                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);
                return;
            }
            if (token.Trim().ToString() == "" || CusID.Trim().ToString() == "" || OTP.Trim().ToString() == "" || SponserID.Trim().ToString() == "")
            {
                Service_Stop service = new Service_Stop();
                service.result = "FAILED";
                service.message = "Please Enter All Values";
                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);
                return;
            }
            string message1 = "";
            string ErrCode1 = "";
            string tokenno = token;
            string statusflag = "0";
            string serviceaction = "Registration";
            string APITitle = "Registration-API";
            string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                ConCls.LOGWebApi(SponserID, tokenno, serviceaction, APITitle, out statusflag, out message1, out ErrCode1);

                if (statusflag == "1")
                {
                    int flag = 0; string errMsg = "0";
                    ConCls.verification_byregistration(OTP, out flag, out errMsg);
                    string NewID = "";
                    NewID = flag.ToString();
                    if (NewID.Length > 4)
                    {
                        string xxx_xx = "";
                        xxx_xx = errMsg.ToString().Trim();
                        if (xxx_xx.ToString() == "SUCCESS")
                        {
                            string _Cust_Name = "";
                            string _email = "";
                            string _pass = "";
                            string txpws = "";
                            SqlDataReader sdr1 = ConCls.GetdataReader("SELECT * from CustRecords where cusid = " + NewID);
                            if (sdr1.HasRows)
                            {
                                if (sdr1.Read())
                                {
                                    _email = sdr1["Email"].ToString();
                                    _Cust_Name = sdr1["cust_name"].ToString();
                                    _pass = sdr1["Cust_Password"].ToString();
                                    txpws = sdr1["Cust_Title"].ToString();
                                }
                            }
                            ud.WelcomeSendSMS(NewID, _Cust_Name, _email, _pass, txpws);
                            var result = char.ConvertFromUtf32(34);
                            var json = "[{" + result + "CustID:" + result + ":" + result + NewID + result + "," + result + "cust_name:" + result + ":" + result + _Cust_Name + result + "," + result + "Password:" + result + ":" + result + _pass + result + "}]";
                            dynamic jsonResponse = JsonConvert.DeserializeObject(json);
                            JArray jarray = new JArray(jsonResponse);
                            ShowdataNew show = new ShowdataNew();
                            show.result = "SUCCESS";
                            show.Message = "Registration Successfully";
                            show.Statuscode = "200";
                            show.Data = jarray;
                            string json1 = JsonConvert.SerializeObject(show);
                            this.Context.Response.Write(json1);
                        }
                    }
                }
                else
                {
                    Service_Stop service = new Service_Stop();
                    service.result = "FAILED";
                    service.message = ErrCode1;
                    string json = JsonConvert.SerializeObject(service);
                    this.Context.Response.Write(json);
                }

            }
        }
        catch (Exception ex)
        {
            Service_Stop service = new Service_Stop();
            service.result = "FAILED";
            service.message = ex.Message;
            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);
        }
    }

    #region CheckLogin
    [WebMethod]
    public void CheckLogin(string CusID, string Password, string token)
    {
        try
        {
            if (CusID == "")
            {
                Service_Stop service = new Service_Stop();
                service.result = "FAILED";
                service.message = "Please enter Cust ID";
                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);
                return;
            }
            if (CusID.All(char.IsDigit) == false)
            {
                Service_Stop service = new Service_Stop();
                service.result = "FAILED";
                service.message = "Please Enter Cust ID is only Number";
                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);
                return;
            }
            string message1 = "";
            string ErrCode1 = "";
            string tokenno = token;
            string statusflag = "0";
            string serviceaction = "CheckLogin";
            string APITitle = "CheckLogin-API";
            string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                ConCls.LOGWebApi(CusID, tokenno, serviceaction, APITitle, out statusflag, out message1, out ErrCode1);

                if (statusflag == "1")
                {
                    using (SqlCommand cmd = new SqlCommand(" select cast(cusid AS varchar(20)) AS UserID,Cust_Name AS Name,Cust_UserName AS UserName  from  Custrecords where cusid = " + CusID + " AND Cust_Password='" + Password + "'"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "CheckLogin";
                                sda.Fill(dt);
                                var json = JsonConvert.SerializeObject(dt);
                                dynamic jsonResponse = JsonConvert.DeserializeObject(json);
                                JArray jarray = new JArray(jsonResponse);
                                ShowdataNew show = new ShowdataNew();
                                if (dt.Rows.Count == 0)
                                {
                                    show.result = "FAILED";
                                    show.Message = "Invalid User Name And Password";
                                    show.Statuscode = "404";
                                    show.Data = jarray;
                                }
                                else
                                {
                                    show.result = "SUCCESS";
                                    show.Message = "Login Successfully";
                                    show.Statuscode = "200";
                                    show.Data = jarray;
                                }

                                string json1 = JsonConvert.SerializeObject(show);
                                this.Context.Response.Write(json1);
                            }
                        }
                    }
                }
                else
                {
                    Service_Stop service = new Service_Stop();
                    service.result = "FAILED";
                    service.message = ErrCode1;
                    string json = JsonConvert.SerializeObject(service);
                    this.Context.Response.Write(json);
                }

            }
        }
        catch (Exception ex)
        {
            Service_Stop service = new Service_Stop();
            service.result = "FAILED";
            service.message = ex.Message;
            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);
        }
    }
    #endregion


}



