using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for yours4organic
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class smartdigital : System.Web.Services.WebService
{
    private Connectioncls ConCls = new Connectioncls();
    public smartdigital()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    private string ErrCode = "";
    private string NewID;
    private readonly string CusID;

    [WebMethod]
    public void APKVersion(string version)
    {
        try
        {
            string NewID = "";
            string ErrCode = "";
            ConCls.APKVersionCheck(version, out NewID, out ErrCode);
            if (NewID == "1")
            {
                Service_Stop service = new Service_Stop();
                service.result = "true";
                service.message = ErrCode;


                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);
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
        catch (Exception ex)
        {
            Service_Stop service = new Service_Stop();
            service.result = "true";
            service.message = "Bad network connection. please try after some time";


            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);

        }
    }


    [WebMethod]
    public void NotificationDeviceID(string DeviceID, string RegisterDeviceID)
    {

        if (DeviceID == "" || RegisterDeviceID == "")
        {
            Service_Stop service = new Service_Stop();
            service.result = "false";
            service.message = "Please enter all fields";


            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);
        }
        else
        {
            string NewID = "";

            ConCls.notification(DeviceID, RegisterDeviceID, out NewID, out ErrCode);

            Service_Stop service = new Service_Stop();
            service.result = "true";
            service.message = ErrCode;


            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);
        }
    }

    [WebMethod]
    public void loginwithAffiliate(string Cust_ID)
    {
        string crypto_wallet = "";
        SqlDataReader sdr = ConCls.GetdataReader("SELECT crypto_wallet FROM CustRecords WHERE CusID='" + Cust_ID + "'");
        if (sdr.HasRows)
        {
            if (sdr.Read())
            {
                crypto_wallet = sdr["crypto_wallet"].ToString();

                Service_Stop service = new Service_Stop();
                service.result = "true";
                service.message = crypto_wallet;


                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);

            }
        }
        sdr.Dispose();
    }

    [WebMethod]
    public void wallet_services(string CusID, string ewpws, string token, string action, string Marketid, string amount, string description)
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
                ConCls.wallet_services(CusID, ewpws, tokenno, serviceaction, Marketid, amount, description, out statusflag, out message1, out ErrCode1);

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
                    //            show.message = jarray; 
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


    [WebMethod]
    public void utility_Wallet(string CusID, string token)
    {
        try
        {
            string message1 = "";
            string ErrCode1 = "";
            string tokenno = token;
            string statusflag = "0";
            string serviceaction = "utlility_balance_check";
            string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                ConCls.tokencheck(CusID, tokenno, serviceaction, out statusflag, out message1, out ErrCode1);

                if (statusflag == "1")
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT CusID,Cust_Name,Email,Cust_mobileNo,credit,debit,balance FROM CustRecords INNER JOIN e_wallet_coin ON CustRecords.CusID = e_wallet_coin.[CusID] where  marketid = 5 and CustRecords.CusID=" + CusID + ""))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "e_wallet_coin";
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
                    service.message = "Invalid Token";


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
    public void Main_Wallet(string CusID)
    {

        try
        {
            string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT CusID,Cust_Name,Email,Cust_mobileNo,credit,debit,balance FROM CustRecords INNER JOIN pin_Wallet ON CustRecords.CusID = pin_Wallet.[custid] where CustRecords.CusID=" + CusID + ""))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "pin_Wallet";
                            sda.Fill(dt);
                            string json = JsonConvert.SerializeObject(dt);
    //                        return dt;

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
    public void PinGenerate(string Cust_id, string pin)
    {

        SqlDataReader sdr3 = ConCls.GetdataReader("SELECT * FROM Config WHERE Permission=0 AND PageName='PinGenerate'");
        if (sdr3.HasRows)
        {
            if (sdr3.Read())
            {
                Service_Stop service = new Service_Stop();
                service.result = "false";
                service.message = "This Service is Temporary Unavailable";

                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);
            }
        }
        else
        {
            string NewID = "";
            string ErrCode = "";
            ConCls.PinGenerate(Cust_id, pin, out NewID, out ErrCode);
            if (NewID == "0")
            {
                Service_Stop service = new Service_Stop();
                service.result = "true";
                service.message = ErrCode;


                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);
            }
            else
            {

                Service_Stop service = new Service_Stop();
                service.result = "false";
                service.message = "Activity log not saved";


                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);

            }
        }
    }


    [WebMethod]
    public void mobile_pin_Verification(string Cust_id, string pin)
    {

        SqlDataReader sdr3 = ConCls.GetdataReader("SELECT * FROM Config WHERE Permission=0 AND PageName='mobile_pin_Verification'");
        if (sdr3.HasRows)
        {
            if (sdr3.Read())
            {
                Service_Stop service = new Service_Stop();
                service.result = "false";
                service.message = "This Service is Temporary Unavailable";


                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);

            }
        }
        else
        {

            string NewID = "";
            string ErrCode = "";

            ConCls.mobile_pin_Verification(Cust_id, pin, out NewID, out ErrCode);
            if (NewID == "1")
            {
                Service_Stop service = new Service_Stop();
                service.result = "true";
                service.message = ErrCode;


                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);

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


    //Forget mobile pin
    [WebMethod]
    public void Forget_mobile_pin(string Email)
    {

        SqlDataReader sdr3 = ConCls.GetdataReader("SELECT * FROM Config WHERE Permission=0 AND PageName='Forget_mobile_pin'");
        if (sdr3.HasRows)
        {
            if (sdr3.Read())
            {

                Service_Stop service = new Service_Stop();
                service.result = "false";
                service.message = "This Service is Temporary Unavailable";


                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);

            }
        }
        else
        {

            string numbers = "1234567890";

            // FOR GENERATING NUMERIC OTP
            string characters = numbers;

            // FOR GENERATING ALPHANUMERIC OTP
            //characters += alphabets + small_alphabets + numbers;

            // OTP LENGTH
            int length = 6;

            //INITIAL OTP
            string otp = string.Empty;


            //GENERATING OTP
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }


            //  string JsonString = JsonConvert.SerializeObject(jd).ToString();

            //Output Parameters
            string NewID = "";
            string ErrCode = "";

            ConCls.Forget_mobile_pin(Email, out NewID, out ErrCode);

            if (NewID == "1")
            {
                SendEmailOTP1(Email, otp);

                // Get OTP Code
                string CusID = "";
                SqlDataReader sdr = ConCls.GetdataReader("SELECT CusID FROM CustRecords WHERE Email='" + Email + "'");
                if (sdr.HasRows)
                {
                    if (sdr.Read())
                    {
                        CusID = sdr["CusID"].ToString();
                    }
                }
                sdr.Dispose();

                string ErrCode1 = "";
                ConCls.otp_manage(CusID, otp, out ErrCode1);

                if (ErrCode1 == "SUCCESS")
                {
                    ForgetPasswordTrue forget = new ForgetPasswordTrue();
                    forget.result = "true";
                    forget.CusID = CusID;
                    forget.Email = Email;
                    forget.message = ErrCode;

                    string json = JsonConvert.SerializeObject(forget);
                    this.Context.Response.Write(json);

                }


            }
            else
            {
                ForgetPasswordFalse forget = new ForgetPasswordFalse();
                forget.result = "false";
                forget.Email = Email;
                forget.message = ErrCode;

                string json = JsonConvert.SerializeObject(forget);
                this.Context.Response.Write(json);

            }
        }
    }


    private string PopulateBody(string otp)
    {

        // Name:{Name}Email-Id:{email}Sponsor Id:{Sponsorid}ID No: {idno}Password:{pass}

        string body = string.Empty;
        using (StreamReader reader = new StreamReader(Server.MapPath("OTP.html")))
        {
            body = reader.ReadToEnd();
        }
        string _dateti = DateTime.Now.ToString();
        // body = body.Replace("{Email}", Email);
        // body = body.Replace("{Cust_Password}", Cust_Password);
        body = body.Replace("{otp}", otp);

        return body;
    }

    //Forget password
    public void SendEmailOTP1(string Email, string otp)
    {
        try
        {

            string body = this.PopulateBody(otp);

            // Gmail Address from where you send the mail
            string fromAddress = "postmaster@smartdigital.life";
            // any address where the email will be sending
            string toAddress = Email;
            //Password of your gmail address
            const string fromPassword = "5fe50b965d01027db6414df7f4f728c4-bd350f28-b131c73d";
            // Passing the values and make a email formate to display
            string subject = "OTP Verification";
            // body = bodyy;


            MailMessage mailMsg = new MailMessage();

            // To
            mailMsg.To.Add(new MailAddress(Email));

            // From
            mailMsg.From = new MailAddress("postmaster@smartdigital.life", "OTP Verification Code : ");

            // Subject and multipart/alternative Body
            mailMsg.Subject = subject;
            string text = "text body";
            string html = body;
            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            // Init SmtpClient and send
            SmtpClient smtpClient = new SmtpClient("smtp.mailgun.org", Convert.ToInt32(587));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("postmaster@smartdigital.life", "5fe50b965d01027db6414df7f4f728c4-bd350f28-b131c73d");
            smtpClient.Credentials = credentials;
            smtpClient.Send(mailMsg);

        }
        catch (Exception ex)
        {

        }


    }

    //ReEnter mobile pin
    [WebMethod]
    public void ReEnter_mobile_pin(string CusID, string pin)
    {
        string NewID = "";
        string ErrCode = "";
        ConCls.ReEnter_mobile_pin(CusID, pin, out NewID, out ErrCode);
        if (NewID == "1")
        {
            ReEnter_mobile_pin_True pin1 = new ReEnter_mobile_pin_True();
            pin1.result = "true";
            pin1.CusID = CusID;
            pin1.message = ErrCode;

            string json = JsonConvert.SerializeObject(pin1);
            this.Context.Response.Write(json);


            string Email = "";
            SqlDataReader sdr = ConCls.GetdataReader("SELECT Email FROM CustRecords WHERE CusID='" + CusID + "'");
            if (sdr.HasRows)
            {
                if (sdr.Read())
                {
                    Email = sdr["Email"].ToString();

                    forget_mob_pin_SuccessfullyEmail(Email);
                }
            }
            sdr.Dispose();
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



    public void forget_mob_pin_SuccessfullyEmail(string Email)
    {
        string body = this.PopulateBody4();
        // this.SendHtmlFormattedEmail("care.hardindouble@gmail.com","HELP-YUG!", body); 
        // string body = " Welcome to world of helpyug Dear, " + name + "  Thanks For Joining With HELP-YUG ,Your account registred  now with following details :    NAME :" + name + "   Email-Id: " + email + "  Sponsor Id:" + sponserID + "   ID No: IN" + id + " <br />Password:" + pass + " <br>  Account Password : 123456 © Copyright 2013. All Rights Reserved.";

        // DateTime letterdt = ConCls.getIndianDateTime();
        // string dat = letterdt.ToShortDateString();


        // Gmail Address from where you send the mail
        string fromAddress = "postmaster@smartdigital.life";
        // any address where the email will be sending
        string toAddress = Email;
        //Password of your gmail address
        const string fromPassword = "5fe50b965d01027db6414df7f4f728c4-bd350f28-b131c73d";
        // Passing the values and make a email formate to display
        string subject = "Mobile PIN Reset Successfully";
        // body = bodyy;


        MailMessage mailMsg = new MailMessage();

        // To
        mailMsg.To.Add(new MailAddress(Email));

        // From
        mailMsg.From = new MailAddress("postmaster@smartdigital.life", "Mobile PIN Reset Successfully : ");

        // Subject and multipart/alternative Body
        mailMsg.Subject = subject;
        string text = "text body";
        string html = body;
        mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
        mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

        // Init SmtpClient and send
        SmtpClient smtpClient = new SmtpClient("smtp.mailgun.org", Convert.ToInt32(587));
        System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("postmaster@smartdigital.life", "5fe50b965d01027db6414df7f4f728c4-bd350f28-b131c73d");
        smtpClient.Credentials = credentials;
        smtpClient.Send(mailMsg);
    }

    private string PopulateBody4()
    {

        // Name:{Name}Email-Id:{email}Sponsor Id:{Sponsorid}ID No: {idno}Password:{pass}

        string body = string.Empty;
        using (StreamReader reader = new StreamReader(Server.MapPath("forget_mobile_pin.html")))
        {
            body = reader.ReadToEnd();
        }
        string _dateti = DateTime.Now.ToString();
        // body = body.Replace("{Email}", Email);

        return body;
    }

    [WebMethod]
    public void forget_mobile_otpverification(string CusID, string otp, string device_id, string remark)
    {

        SqlDataReader sdr3 = ConCls.GetdataReader("SELECT * FROM Config WHERE Permission=0 AND PageName='otpverification'");
        if (sdr3.HasRows)
        {
            if (sdr3.Read())
            {

                Service_Stop service = new Service_Stop();
                service.result = "false";
                service.message = "This Service is Temporary Unavailable";


                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);

            }
        }
        else
        {

            string NewID = "";
            string ErrCode = "";


            ConCls.otpverification(CusID, otp, device_id, remark, out NewID, out ErrCode);

            if (NewID == "1")
            {
                // otpverify_result(CusID, device_id, remark, otp, ErrCode);

                otpverify_result service = new otpverify_result();
                service.CusID = CusID;
                service.device_id = device_id;
                service.remark = remark;
                service.otp = otp;
                service.ErrCode = ErrCode;

                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);
            }

            else
            {

                Service_Stop service = new Service_Stop();
                service.result = "false";
                service.message = ErrCode;


                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);

                // otpverify_resultwrong(ErrCode);
            }
        }

    }




    //Recharge_Circle
    [WebMethod]
    public void Recharge_Circle()
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select * from Recharge_Circle"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "Recharge_Circle";
                            sda.Fill(dt);

                            string json = JsonConvert.SerializeObject(dt);
                            //return dt;

                            dynamic jsonResponse = JsonConvert.DeserializeObject(json);

                            JArray jarray = new JArray(jsonResponse);

                            JObject jd = new JObject(
                            new JProperty("result", "true"),
                            new JProperty("Data", jarray));

                            string JsonString = jd.ToString();
                            //  string JsonString = "";

                            this.Context.Response.Clear();
                            //JsonString = JsonConvert.SerializeObject(jd).ToString();
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
        catch (Exception ex)
        {
            JObject jd = new JObject();
            jd.Add("result", "");
            JObject val = jd;
            string option = (string)val["result"];
            string JsonString = "";

            val = jd;
            option = (string)val["result"];
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



    //Prepaid_Mobile_Operators
    [WebMethod]
    public void Prepaid_Mobile_Operators()
    {
        try
        {

            string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select * from Prepaid_Mobile_Operators1"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "Prepaid_Mobile_Operators";
                            sda.Fill(dt);

                            string json = JsonConvert.SerializeObject(dt);
                            //return dt;

                            dynamic jsonResponse = JsonConvert.DeserializeObject(json);

                            JArray jarray = new JArray(jsonResponse);

                            JObject jd = new JObject(
                            new JProperty("result", "true"),
                            new JProperty("Data", jarray));

                            string JsonString = jd.ToString();
                            //  string JsonString = "";

                            this.Context.Response.Clear();
                            //JsonString = JsonConvert.SerializeObject(jd).ToString();
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
        catch (Exception ex)
        {
            JObject jd = new JObject();
            jd.Add("result", "");
            JObject val = jd;
            string option = (string)val["result"];
            string JsonString = "";

            val = jd;
            option = (string)val["result"];
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


    //POSTPAIDOP_DETAILS
    [WebMethod]
    public void POSTPAIDOP_DETAILS()
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select * from POSTPAIDOP_DETAILS1"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "POSTPAIDOP_DETAILS";
                            sda.Fill(dt);

                            string json = JsonConvert.SerializeObject(dt);
                            //return dt;

                            dynamic jsonResponse = JsonConvert.DeserializeObject(json);

                            JArray jarray = new JArray(jsonResponse);

                            JObject jd = new JObject(
                            new JProperty("result", "true"),
                            new JProperty("Data", jarray));

                            string JsonString = jd.ToString();
                            //  string JsonString = "";

                            this.Context.Response.Clear();
                            //JsonString = JsonConvert.SerializeObject(jd).ToString();
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
        catch (Exception ex)
        {
            JObject jd = new JObject();
            jd.Add("result", "");
            JObject val = jd;
            string option = (string)val["result"];
            string JsonString = "";

            val = jd;
            option = (string)val["result"];
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


    //BROADBAND_OPERATOR_DETAILS 
    [WebMethod]
    public void BROADBAND_OPERATOR_DETAILS()
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select * from BROADBAND_OPERATOR_DETAILS1 "))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "BROADBAND_OPERATOR_DETAILS1 ";
                            sda.Fill(dt);

                            string json = JsonConvert.SerializeObject(dt);
                            //return dt;

                            dynamic jsonResponse = JsonConvert.DeserializeObject(json);

                            JArray jarray = new JArray(jsonResponse);

                            JObject jd = new JObject(
                            new JProperty("result", "true"),
                            new JProperty("Data", jarray));

                            string JsonString = jd.ToString();
                            //  string JsonString = "";

                            this.Context.Response.Clear();
                            //JsonString = JsonConvert.SerializeObject(jd).ToString();
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
        catch (Exception ex)
        {
            JObject jd = new JObject();
            jd.Add("result", "");
            JObject val = jd;
            string option = (string)val["result"];
            string JsonString = "";

            val = jd;
            option = (string)val["result"];
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



    //DATACARD_OPERATOR_DETAILS
    [WebMethod]
    public void DATACARD_OPERATOR_DETAILS()
    {
        try
        {

            string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select * from DATACARD_OPERATOR_DETAILS"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "DATACARD_OPERATOR_DETAILS";
                            sda.Fill(dt);

                            var json = JsonConvert.SerializeObject(dt);
                            //return dt;

                            dynamic jsonResponse = JsonConvert.DeserializeObject(json);

                            JArray jarray = new JArray(jsonResponse);

                            JObject jd = new JObject(
                            new JProperty("result", "true"),
                            new JProperty("Data", jarray));

                            string JsonString = jd.ToString();
                            //  string JsonString = "";

                            this.Context.Response.Clear();
                            //JsonString = JsonConvert.SerializeObject(jd).ToString();
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
        catch (Exception ex)
        {
            JObject jd = new JObject();
            jd.Add("result", "");
            var val = jd;
            string option = (string)val["result"];
            string JsonString = "";

            val = jd;
            option = (string)val["result"];
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


    [WebMethod]
    public void INSURANCE_OPERATOR_DETAILS()
    {
        try
        {

            string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select * from INSURANCE_OPERATOR_DETAILS1"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "INSURANCE_OPERATOR_DETAILS";
                            sda.Fill(dt);

                            string json = JsonConvert.SerializeObject(dt);
                            //return dt;

                            dynamic jsonResponse = JsonConvert.DeserializeObject(json);

                            JArray jarray = new JArray(jsonResponse);

                            JObject jd = new JObject(
                            new JProperty("result", "true"),
                            new JProperty("Data", jarray));

                            string JsonString = jd.ToString();
                            //  string JsonString = "";

                            this.Context.Response.Clear();
                            //JsonString = JsonConvert.SerializeObject(jd).ToString();
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
        catch (Exception ex)
        {
            JObject jd = new JObject();
            jd.Add("result", "");
            JObject val = jd;
            string option = (string)val["result"];
            string JsonString = "";

            val = jd;
            option = (string)val["result"];
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



    //DTH_OPERATOR_DETAILS
    [WebMethod]
    public void DTH_OPERATOR_DETAILS()
    {
        try
        {

            string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select * from DTH_OPERATOR_DETAILS1"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "DTH_OPERATOR_DETAILS1";
                            sda.Fill(dt);

                            string json = JsonConvert.SerializeObject(dt);
                            //return dt;

                            dynamic jsonResponse = JsonConvert.DeserializeObject(json);

                            JArray jarray = new JArray(jsonResponse);

                            JObject jd = new JObject(
                            new JProperty("result", "true"),
                            new JProperty("Data", jarray));

                            string JsonString = jd.ToString();
                            //  string JsonString = "";

                            this.Context.Response.Clear();
                            //JsonString = JsonConvert.SerializeObject(jd).ToString();
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
        catch (Exception ex)
        {
            JObject jd = new JObject();
            jd.Add("result", "");
            JObject val = jd;
            string option = (string)val["result"];
            string JsonString = "";

            val = jd;
            option = (string)val["result"];
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




    //ELECTRCITY_OPERATOR_DETAILS
    [WebMethod]
    public void ELECTRCITY_OPERATOR_DETAILS()
    {
        try
        {

            string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select * from ELECTRCITY_OPERATOR_DETAILS1"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "ELECTRCITY_OPERATOR_DETAILS";
                            sda.Fill(dt);

                            string json = JsonConvert.SerializeObject(dt);
                            //return dt;

                            dynamic jsonResponse = JsonConvert.DeserializeObject(json);

                            JArray jarray = new JArray(jsonResponse);

                            JObject jd = new JObject(
                            new JProperty("result", "true"),
                            new JProperty("Data", jarray));

                            string JsonString = jd.ToString();
                            //  string JsonString = "";

                            this.Context.Response.Clear();
                            //JsonString = JsonConvert.SerializeObject(jd).ToString();
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
        catch (Exception ex)
        {
            JObject jd = new JObject();
            jd.Add("result", "");
            JObject val = jd;
            string option = (string)val["result"];
            string JsonString = "";

            val = jd;
            option = (string)val["result"];
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



    //GAS_OPERATOR_DETAILS
    [WebMethod]
    public void GAS_OPERATOR_DETAILS()
    {
        try
        {

            string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select * from GAS_OPERATOR_DETAILS1"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "GAS_OPERATOR_DETAILS";
                            sda.Fill(dt);

                            string json = JsonConvert.SerializeObject(dt);
                            //return dt;

                            dynamic jsonResponse = JsonConvert.DeserializeObject(json);

                            JArray jarray = new JArray(jsonResponse);

                            JObject jd = new JObject(
                            new JProperty("result", "true"),
                            new JProperty("Data", jarray));

                            string JsonString = jd.ToString();
                            //  string JsonString = "";

                            this.Context.Response.Clear();
                            //JsonString = JsonConvert.SerializeObject(jd).ToString();
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
        catch (Exception ex)
        {
            JObject jd = new JObject();
            jd.Add("result", "");
            JObject val = jd;
            string option = (string)val["result"];
            string JsonString = "";

            val = jd;
            option = (string)val["result"];
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


    //Mobile Recharge Prepaid
    [WebMethod]
    public void mobilerechargepre(string CusID, string mobile_no, string operator_name, string circle_id, string amount2, string balance)
    {

        if (CusID == "" || mobile_no == "" || operator_name == "" || circle_id == "" || amount2 == "" || balance == "")
        {

            Service_Stop service = new Service_Stop();
            service.result = "false";
            service.message = "Please enter all fields";


            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);

        }
        else
        {
            try
            {

                SqlDataReader sdr3 = ConCls.GetdataReader("SELECT * FROM Config WHERE Permission=0 AND PageName='mobilerechargepre'");
                if (sdr3.HasRows)
                {
                    if (sdr3.Read())
                    {
                        Service_Stop service = new Service_Stop();
                        service.result = "false";
                        service.message = "This Service is Temporary Unavailable";


                        string json = JsonConvert.SerializeObject(service);
                        this.Context.Response.Write(json);

                    }
                }
                else
                {
                    string status1 = "0";
                    string ErrCode = "";
                    string trID1 = "";
                    string message1 = "";
                    string ErrCode1 = "";

                    SqlDataReader sdr = ConCls.GetdataReader("select max(AGENTID)+1 from mobile_log");
                    if (sdr.HasRows)
                    {
                        if (sdr.Read())
                        {
                            string trID11 = sdr[0].ToString();
                            //decimal trID111 = decimal.Parse(trID11);
                            //decimal tr = trID111 + 1;
                            //string tr1 = tr.ToString();

                            ConCls.Walletbalance(CusID, amount2, balance, out message1, out ErrCode1);

                            if (message1 == "1")
                            {
                                string userid = "8452056444";
                                string pass = "996581";
                                string mob = mobile_no;
                                string opt = operator_name;
                                string agentid = trID11;
                                // string circle = circle_id;
                                string amt = amount2;
                                string usertx = trID1;
                                string fmt = "json";
                                // string version = "4";


                                // Creating Parameters List
                                NameValueCollection Parameterslist = System.Web.HttpUtility.ParseQueryString(string.Empty);

                                Parameterslist["userid"] = userid.ToString();
                                Parameterslist["pass"] = pass.ToString();
                                Parameterslist["mob"] = mob.ToString();
                                Parameterslist["opt"] = opt.ToString();
                                Parameterslist["agentid"] = agentid.ToString();
                                // Parameterslist["circle"] = circle.ToString();
                                Parameterslist["amt"] = amt.ToString();
                                Parameterslist["fmt"] = fmt.ToString();
                                // Parameterslist["version"] = version.ToString();

                                string link = "http://roundpayapi.in/API/APIService.aspx?" + Parameterslist.ToString();

                                // Getting JSON Output
                                WebClient c_euro = new WebClient();
                                string data_euro = c_euro.DownloadString("http://roundpayapi.in/API/APIService.aspx?" + Parameterslist.ToString());
                                //Console.WriteLine(data);
                                JObject o_euro = JObject.Parse(data_euro);

                                // GEtting Values From JSON Data
                                JObject jObj = (JObject)JsonConvert.DeserializeObject(o_euro.ToString());
                                int countelement = jObj.Count;

                                string txid = "";
                                string user_txid = "";
                                string status = "";
                                string amount = "";
                                string your_cost = "";
                                //   string balancetest = "";
                                string number = "";
                                string operators = "";
                                string operator_ref = "";
                                string error_code = "";
                                string message = "";
                                string time = "";

                                txid = o_euro["AGENTID"].ToString();
                                user_txid = o_euro["RPID"].ToString();
                                status = o_euro["STATUS"].ToString();
                                amount = o_euro["AMOUNT"].ToString();
                                your_cost = o_euro["BAL"].ToString();
                                balance = o_euro["AMOUNT"].ToString();
                                number = o_euro["MOBILE"].ToString();
                                operators = o_euro["OPID"].ToString();
                                //  operators = "ss";
                                operator_ref = "ss";
                                error_code = "ss";
                                message = o_euro["MSG"].ToString();
                                time = DateTime.Now.ToShortDateString();
                                circle_id = "ss";


                                if (status == "PENDING" || status == "SUCCESS")
                                {
                                    ConCls.mobilerechargepre(CusID, mobile_no, operator_name, circle_id, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
                                    if (status1 == "1")
                                    {

                                        mobilerechargepretrue service = new mobilerechargepretrue();
                                        service.txid = txid;
                                        service.user_txid = user_txid;
                                        service.status = status;
                                        service.amount = amount;
                                        service.your_cost = your_cost;
                                        service.balance = balance;
                                        service.number = number;
                                        service.operators = operators;
                                        service.operator_ref = operator_ref;
                                        service.error_code = error_code;
                                        service.message = message;
                                        service.time = time;

                                        string json = JsonConvert.SerializeObject(service);
                                        this.Context.Response.Write(json);

                                        //mobilerechargepretrue(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);
                                    }
                                    else
                                    {
                                        //   ConCls.transactionfailpremobile(CusID, mobile_no, operator_name, circle_id, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);

                                        mobilerechargeprefalse service = new mobilerechargeprefalse();
                                        service.txid = txid;
                                        service.user_txid = user_txid;
                                        service.status = status;
                                        service.amount = amount;
                                        service.your_cost = your_cost;
                                        service.balance = balance;
                                        service.number = number;
                                        service.operators = operators;
                                        service.operator_ref = operator_ref;
                                        service.error_code = error_code;
                                        service.message = message;
                                        service.time = time;

                                        string json = JsonConvert.SerializeObject(service);
                                        this.Context.Response.Write(json);

                                        //mobilerechargeprefalse(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);
                                    }
                                }
                                else if (status == "FAILED")
                                {
                                    ConCls.transactionfailpremobile(CusID, mobile_no, operator_name, circle_id, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
                                    // mobilerechargeprefalse(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);

                                    mobilerechargeprefalse service = new mobilerechargeprefalse();
                                    service.txid = txid;
                                    service.user_txid = user_txid;
                                    service.status = status;
                                    service.amount = amount;
                                    service.your_cost = your_cost;
                                    service.balance = balance;
                                    service.number = number;
                                    service.operators = operators;
                                    service.operator_ref = operator_ref;
                                    service.error_code = error_code;
                                    service.message = message;
                                    service.time = time;

                                    string json = JsonConvert.SerializeObject(service);
                                    this.Context.Response.Write(json);
                                }
                            }
                        }
                    }
                    else
                    {
                        string txid = "";
                        string user_txid = "";
                        string status = "";
                        string your_cost = "";
                        string operator_ref = "";
                        string error_code = ErrCode1;
                        string message = ErrCode1;
                        string time = "";

                        string number = mobile_no;
                        string operators = operator_name;
                        string circle = circle_id;
                        string amount = amount2;
                        string usertx = trID1;

                        mobilerechargeprefalse service = new mobilerechargeprefalse();
                        service.txid = txid;
                        service.user_txid = user_txid;
                        service.status = status;
                        service.amount = amount;
                        service.your_cost = your_cost;
                        service.balance = balance;
                        service.number = number;
                        service.operators = operators;
                        service.operator_ref = operator_ref;
                        service.error_code = error_code;
                        service.message = message;
                        service.time = time;

                        string json = JsonConvert.SerializeObject(service);
                        this.Context.Response.Write(json);
                        ConCls.transactionfailpremobile(CusID, mobile_no, operator_name, circle_id, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);

                        //mobilerechargeprefalse(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);
                        //InsufficiantBalance(ErrCode1);

                    }
                }
            }
            catch (Exception ex)
            {
                Service_Stop service = new Service_Stop();
                service.result = "false";
                service.message = "Bad network connection. please try after some time";

                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);

            }
        }
    }

    //Mobile Recharge Postpaid
    [WebMethod]
    public void mobilerechargepost(string CusID, string mobile_no, string operator_name, string circle_id, string amount2, string balance, string accountno)
    {

        if (CusID == "" || mobile_no == "" || operator_name == "" || circle_id == "" || amount2 == "" || balance == "")
        {
            Service_Stop service = new Service_Stop();
            service.result = "false";
            service.message = "Please enter all fields";


            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);
        }
        else
        {

            try
            {

                SqlDataReader sdr3 = ConCls.GetdataReader("SELECT * FROM Config WHERE Permission=0 AND PageName='mobilerechargepost'");
                if (sdr3.HasRows)
                {

                    if (sdr3.Read())
                    {
                        Service_Stop service = new Service_Stop();
                        service.result = "false";
                        service.message = "This Service is Temporary Unavailable";


                        string json = JsonConvert.SerializeObject(service);
                        this.Context.Response.Write(json);

                    }
                }
                else
                {
                    string status1 = "0";
                    string ErrCode = "";
                    string trID1 = "";
                    string message1 = "";
                    string ErrCode1 = "";

                    SqlDataReader sdr = ConCls.GetdataReader("select max(AGENTID)+1 from mobile_log");
                    if (sdr.HasRows)
                    {
                        if (sdr.Read())
                        {
                            string trID11 = sdr[0].ToString();
                            //decimal trID111 = decimal.Parse(trID11);
                            //decimal tr = trID111 + 1;
                            //string tr1 = tr.ToString();

                            ConCls.Walletbalance(CusID, amount2, balance, out message1, out ErrCode1);

                            if (message1 == "1")
                            {
                                string userid = "8452056444";
                                string pass = "996581";
                                string mob = mobile_no;
                                string opt = operator_name;
                                string agentid = trID11;
                                // string circle = circle_id;
                                string amt = amount2;
                                string usertx = trID1;
                                string fmt = "json";
                                // string version = "4";


                                // Creating Parameters List
                                NameValueCollection Parameterslist = System.Web.HttpUtility.ParseQueryString(string.Empty);

                                Parameterslist["userid"] = userid.ToString();
                                Parameterslist["pass"] = pass.ToString();
                                Parameterslist["mob"] = mob.ToString();
                                Parameterslist["opt"] = opt.ToString();
                                Parameterslist["agentid"] = agentid.ToString();
                                // Parameterslist["circle"] = circle.ToString();
                                Parameterslist["amt"] = amt.ToString();
                                Parameterslist["fmt"] = fmt.ToString();
                                // Parameterslist["version"] = version.ToString();

                                string link = "http://roundpayapi.in/API/APIService.aspx?" + Parameterslist.ToString();

                                // Getting JSON Output
                                WebClient c_euro = new WebClient();
                                string data_euro = c_euro.DownloadString("http://roundpayapi.in/API/APIService.aspx?" + Parameterslist.ToString());
                                //Console.WriteLine(data);
                                JObject o_euro = JObject.Parse(data_euro);

                                // GEtting Values From JSON Data
                                JObject jObj = (JObject)JsonConvert.DeserializeObject(o_euro.ToString());
                                int countelement = jObj.Count;

                                string txid = "";
                                string user_txid = "";
                                string status = "";
                                string amount = "";
                                string your_cost = "";
                                //   string balancetest = "";
                                string number = "";
                                string operators = "";
                                string operator_ref = "";
                                string error_code = "";
                                string message = "";
                                string time = "";

                                txid = o_euro["AGENTID"].ToString();
                                user_txid = o_euro["RPID"].ToString();
                                status = o_euro["STATUS"].ToString();
                                amount = o_euro["AMOUNT"].ToString();
                                your_cost = o_euro["BAL"].ToString();
                                balance = o_euro["AMOUNT"].ToString();
                                number = o_euro["MOBILE"].ToString();
                                operators = o_euro["OPID"].ToString();
                                //  operators = "ss";
                                operator_ref = "ss";
                                error_code = o_euro["MSG"].ToString();
                                message = o_euro["OPID"].ToString();
                                time = DateTime.Now.ToShortDateString();
                                circle_id = "ss";



                                if (status == "PENDING" || status == "SUCCESS")
                                {
                                    ConCls.mobilerechargepost(CusID, mobile_no, operator_name, circle_id, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
                                    if (status1 == "1")
                                    {
                                        mobilerechargepretrue service = new mobilerechargepretrue();
                                        service.txid = txid;
                                        service.user_txid = user_txid;
                                        service.status = status;
                                        service.amount = amount;
                                        service.your_cost = your_cost;
                                        service.balance = balance;
                                        service.number = number;
                                        service.operators = operators;
                                        service.operator_ref = operator_ref;
                                        service.error_code = error_code;
                                        service.message = message;
                                        service.time = time;

                                        string json = JsonConvert.SerializeObject(service);
                                        this.Context.Response.Write(json);

                                        // mobilerechargepretrue(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);
                                    }
                                    else
                                    {

                                        mobilerechargeprefalse service = new mobilerechargeprefalse();
                                        service.txid = txid;
                                        service.user_txid = user_txid;
                                        service.status = status;
                                        service.amount = amount;
                                        service.your_cost = your_cost;
                                        service.balance = balance;
                                        service.number = number;
                                        service.operators = operators;
                                        service.operator_ref = operator_ref;
                                        service.error_code = error_code;
                                        service.message = message;
                                        service.time = time;

                                        string json = JsonConvert.SerializeObject(service);
                                        this.Context.Response.Write(json);
                                        //  ConCls.transactionfailpostmobile(CusID, mobile_no, operator_name, circle_id, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
                                        //mobilerechargeprefalse(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);
                                    }
                                }
                                else if (status == "FAILED")
                                {
                                    ConCls.transactionfailpostmobile(CusID, mobile_no, operator_name, circle_id, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
                                    // mobilerechargeprefalse(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);


                                    mobilerechargeprefalse service = new mobilerechargeprefalse();
                                    service.txid = txid;
                                    service.user_txid = user_txid;
                                    service.status = status;
                                    service.amount = amount;
                                    service.your_cost = your_cost;
                                    service.balance = balance;
                                    service.number = number;
                                    service.operators = operators;
                                    service.operator_ref = operator_ref;
                                    service.error_code = error_code;
                                    service.message = message;
                                    service.time = time;

                                    string json = JsonConvert.SerializeObject(service);
                                    this.Context.Response.Write(json);
                                }
                            }
                        }
                    }
                    else
                    {
                        string txid = "";
                        string user_txid = "";
                        string status = "";
                        string your_cost = "";
                        string operator_ref = "";
                        string error_code = ErrCode1;
                        string message = ErrCode1;
                        string time = "";

                        string number = mobile_no;
                        string operators = operator_name;
                        string circle = circle_id;
                        string amount = amount2;
                        string usertx = trID1;

                        mobilerechargeprefalse service = new mobilerechargeprefalse();
                        service.txid = txid;
                        service.user_txid = user_txid;
                        service.status = status;
                        service.amount = amount;
                        service.your_cost = your_cost;
                        service.balance = balance;
                        service.number = number;
                        service.operators = operators;
                        service.operator_ref = operator_ref;
                        service.error_code = error_code;
                        service.message = message;
                        service.time = time;

                        string json = JsonConvert.SerializeObject(service);
                        this.Context.Response.Write(json);

                        // mobilerechargeprefalse(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);
                        //InsufficiantBalance(ErrCode1);
                    }
                }
            }

            catch (Exception ex)
            {
                Service_Stop service = new Service_Stop();
                service.result = "false";
                service.message = "Bad network connection. please try after some time";


                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);

            }

        }
    }



    ////Data Card recharge
    //[WebMethod]
    //public void data_card_api(string CusID, string mobile_no, string operator_name, string circle_id, string amount2, string balance)
    //{

    //    if (CusID == "" || mobile_no == "" || operator_name == "" || circle_id == "" || amount2 == "" || balance == "")
    //    {
    //        Service_Stop service = new Service_Stop();
    //        service.result = "false";
    //        service.message = "Please enter all fields";


    //        string json = JsonConvert.SerializeObject(service);
    //        this.Context.Response.Write(json);

    //    }
    //    else
    //    {

    //            try
    //            {

    //                SqlDataReader sdr3 = ConCls.GetdataReader("SELECT * FROM Config WHERE Permission=0 AND PageName='mobilerechargepost'");
    //                if (sdr3.HasRows)
    //                {

    //                    if (sdr3.Read())
    //                    {
    //                    Service_Stop service = new Service_Stop();
    //                    service.result = "false";
    //                    service.message = "This Service is Temporary Unavailable";


    //                    string json = JsonConvert.SerializeObject(service);
    //                    this.Context.Response.Write(json);

    //                    }
    //                }
    //                else
    //                {
    //                    string status1 = "0";
    //                    string ErrCode = "";
    //                    string trID1 = "";
    //                    string message1 = "";
    //                    string ErrCode1 = "";

    //                    SqlDataReader sdr = ConCls.GetdataReader("select max(AGENTID)+1 from mobile_log");
    //                    if (sdr.HasRows)
    //                    {
    //                        if (sdr.Read())
    //                        {
    //                            string trID11 = sdr[0].ToString();
    //                            //decimal trID111 = decimal.Parse(trID11);
    //                            //decimal tr = trID111 + 1;
    //                            //string tr1 = tr.ToString();

    //                            ConCls.Walletbalance(CusID, amount2, balance, out message1, out ErrCode1);

    //                            if (message1 == "1")
    //                            {
    //                                string userid = "8452056444";
    //                                string pass = "996581";
    //                                string mob = mobile_no;
    //                                string opt = operator_name;
    //                                string agentid = trID11;
    //                                // string circle = circle_id;
    //                                string amt = amount2;
    //                                string usertx = trID1;
    //                                string fmt = "json";
    //                                // string version = "4";


    //                                // Creating Parameters List
    //                                NameValueCollection Parameterslist = System.Web.HttpUtility.ParseQueryString(string.Empty);

    //                                Parameterslist["userid"] = userid.ToString();
    //                                Parameterslist["pass"] = pass.ToString();
    //                                Parameterslist["mob"] = mob.ToString();
    //                                Parameterslist["opt"] = opt.ToString();
    //                                Parameterslist["agentid"] = agentid.ToString();
    //                                // Parameterslist["circle"] = circle.ToString();
    //                                Parameterslist["amt"] = amt.ToString();
    //                                Parameterslist["fmt"] = fmt.ToString();
    //                                // Parameterslist["version"] = version.ToString();

    //                                var link = "http://roundpayapi.in/API/APIService.aspx?" + Parameterslist.ToString();

    //                                // Getting JSON Output
    //                                WebClient c_euro = new WebClient();
    //                                var data_euro = c_euro.DownloadString("http://roundpayapi.in/API/APIService.aspx?" + Parameterslist.ToString());
    //                                //Console.WriteLine(data);
    //                                JObject o_euro = JObject.Parse(data_euro);

    //                                // GEtting Values From JSON Data
    //                                JObject jObj = (JObject)JsonConvert.DeserializeObject(o_euro.ToString());
    //                                int countelement = jObj.Count;

    //                                string txid = "";
    //                                string user_txid = "";
    //                                string status = "";
    //                                string amount = "";
    //                                string your_cost = "";
    //                                //   string balancetest = "";
    //                                string number = "";
    //                                string operators = "";
    //                                string operator_ref = "";
    //                                string error_code = "";
    //                                string message = "";
    //                                string time = "";

    //                                txid = o_euro["AGENTID"].ToString();
    //                                user_txid = o_euro["RPID"].ToString();
    //                                status = o_euro["STATUS"].ToString();
    //                                amount = o_euro["AMOUNT"].ToString();
    //                                your_cost = o_euro["BAL"].ToString();
    //                                balance = o_euro["AMOUNT"].ToString();
    //                                number = o_euro["MOBILE"].ToString();
    //                                operators = o_euro["OPID"].ToString();
    //                                //  operators = "ss";
    //                                operator_ref = "ss";
    //                                error_code = o_euro["MSG"].ToString();
    //                                message = o_euro["OPID"].ToString();
    //                                time = DateTime.Now.ToShortDateString();
    //                                circle_id = "ss";



    //                                if (status == "PENDING" || status == "SUCCESS")
    //                                {
    //                                    ConCls.mobilerechargepost(CusID, mobile_no, operator_name, circle_id, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
    //                                    if (status1 == "1")
    //                                    {
    //                                    mobilerechargepretrue service = new mobilerechargepretrue();
    //                                    service.txid = txid;
    //                                    service.user_txid = user_txid;
    //                                    service.status = status;
    //                                    service.amount = amount;
    //                                    service.your_cost = your_cost;
    //                                    service.balance = balance;
    //                                    service.number = number;
    //                                    service.operators = operators;
    //                                    service.operator_ref = operator_ref;
    //                                    service.error_code = error_code;
    //                                    service.message = message;
    //                                    service.time = time;

    //                                    string json = JsonConvert.SerializeObject(service);
    //                                    this.Context.Response.Write(json);
    //                                    // mobilerechargepretrue(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);
    //                                }
    //                                    else
    //                                    {
    //                                    mobilerechargeprefalse service = new mobilerechargeprefalse();
    //                                    service.txid = txid;
    //                                    service.user_txid = user_txid;
    //                                    service.status = status;
    //                                    service.amount = amount;
    //                                    service.your_cost = your_cost;
    //                                    service.balance = balance;
    //                                    service.number = number;
    //                                    service.operators = operators;
    //                                    service.operator_ref = operator_ref;
    //                                    service.error_code = error_code;
    //                                    service.message = message;
    //                                    service.time = time;

    //                                    string json = JsonConvert.SerializeObject(service);
    //                                    this.Context.Response.Write(json);
    //                                    // mobilerechargeprefalse(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);
    //                                }
    //                                }
    //                                else if (status == "FAILED")
    //                                {
    //                                    ConCls.transactionfailpostmobile(CusID, mobile_no, operator_name, circle_id, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
    //                                // mobilerechargeprefalse(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);
    //                                mobilerechargeprefalse service = new mobilerechargeprefalse();
    //                                service.txid = txid;
    //                                service.user_txid = user_txid;
    //                                service.status = status;
    //                                service.amount = amount;
    //                                service.your_cost = your_cost;
    //                                service.balance = balance;
    //                                service.number = number;
    //                                service.operators = operators;
    //                                service.operator_ref = operator_ref;
    //                                service.error_code = error_code;
    //                                service.message = message;
    //                                service.time = time;

    //                                string json = JsonConvert.SerializeObject(service);
    //                                this.Context.Response.Write(json);
    //                            }
    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        string txid = "";
    //                        string user_txid = "";
    //                        string status = "";
    //                        string your_cost = "";
    //                        string operator_ref = "";
    //                        string error_code = ErrCode1;
    //                        string message = ErrCode1;
    //                        string time = "";

    //                        string number = mobile_no;
    //                        string operators = operator_name;
    //                        string circle = circle_id;
    //                        string amount = amount2;
    //                        string usertx = trID1;


    //                    mobilerechargeprefalse service = new mobilerechargeprefalse();
    //                    service.txid = txid;
    //                    service.user_txid = user_txid;
    //                    service.status = status;
    //                    service.amount = amount;
    //                    service.your_cost = your_cost;
    //                    service.balance = balance;
    //                    service.number = number;
    //                    service.operators = operators;
    //                    service.operator_ref = operator_ref;
    //                    service.error_code = error_code;
    //                    service.message = message;
    //                    service.time = time;

    //                    string json = JsonConvert.SerializeObject(service);
    //                    this.Context.Response.Write(json);

    //                    //    mobilerechargeprefalse(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);
    //                    //InsufficiantBalance(ErrCode1);
    //                }
    //                }

    //            }
    //            catch (Exception ex)
    //            {
    //            Service_Stop service = new Service_Stop();
    //            service.result = "false";
    //            service.message = "Bad network connection. please try after some time";


    //            string json = JsonConvert.SerializeObject(service);
    //            this.Context.Response.Write(json);


    //            }

    //    }
    //}



    //DTH recharge


    [WebMethod]
    public void DTH(string CusID, string mobile_no, string operator_name, string amount2, string balance)
    {

        if (CusID == "" || mobile_no == "" || operator_name == "" || amount2 == "" || balance == "")
        {

            Service_Stop service = new Service_Stop();
            service.result = "false";
            service.message = "Please enter all fields";


            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);

        }
        else
        {

            try
            {

                SqlDataReader sdr3 = ConCls.GetdataReader("SELECT * FROM Config WHERE Permission=0 AND PageName='DTH'");
                if (sdr3.HasRows)
                {

                    if (sdr3.Read())
                    {

                        Service_Stop service = new Service_Stop();
                        service.result = "false";
                        service.message = "This Service is Temporary Unavailable";


                        string json = JsonConvert.SerializeObject(service);
                        this.Context.Response.Write(json);

                    }
                }
                else
                {
                    string status1 = "0";
                    string ErrCode = "";
                    string trID1 = "";
                    string message1 = "";
                    string ErrCode1 = "";

                    SqlDataReader sdr = ConCls.GetdataReader("select max(AGENTID)+1 from mobile_log");
                    if (sdr.HasRows)
                    {
                        if (sdr.Read())
                        {
                            string trID11 = sdr[0].ToString();
                            //decimal trID111 = decimal.Parse(trID11);
                            //decimal tr = trID111 + 1;
                            //string tr1 = tr.ToString();

                            ConCls.Walletbalance(CusID, amount2, balance, out message1, out ErrCode1);

                            if (message1 == "1")
                            {

                                string userid = "8452056444";
                                string pass = "996581";
                                string mob = mobile_no;
                                string opt = operator_name;
                                string agentid = trID11;
                                // string circle = circle_id;
                                string amt = amount2;
                                string usertx = trID1;
                                string fmt = "json";
                                // string version = "4";


                                // Creating Parameters List
                                NameValueCollection Parameterslist = System.Web.HttpUtility.ParseQueryString(string.Empty);

                                Parameterslist["userid"] = userid.ToString();
                                Parameterslist["pass"] = pass.ToString();
                                Parameterslist["mob"] = mob.ToString();
                                Parameterslist["opt"] = opt.ToString();
                                Parameterslist["agentid"] = agentid.ToString();
                                // Parameterslist["circle"] = circle.ToString();
                                Parameterslist["amt"] = amt.ToString();
                                Parameterslist["fmt"] = fmt.ToString();
                                // Parameterslist["version"] = version.ToString();

                                string link = "http://roundpayapi.in/API/APIService.aspx?" + Parameterslist.ToString();

                                // Getting JSON Output
                                WebClient c_euro = new WebClient();
                                string data_euro = c_euro.DownloadString("http://roundpayapi.in/API/APIService.aspx?" + Parameterslist.ToString());
                                //Console.WriteLine(data);
                                JObject o_euro = JObject.Parse(data_euro);

                                // GEtting Values From JSON Data
                                JObject jObj = (JObject)JsonConvert.DeserializeObject(o_euro.ToString());
                                int countelement = jObj.Count;

                                string txid = "";
                                string user_txid = "";
                                string status = "";
                                string amount = "";
                                string your_cost = "";
                                //   string balancetest = "";
                                string number = "";
                                string operators = "";
                                string operator_ref = "";
                                string error_code = "";
                                string message = "";
                                string time = "";

                                txid = o_euro["AGENTID"].ToString();
                                user_txid = o_euro["RPID"].ToString();
                                status = o_euro["STATUS"].ToString();
                                amount = o_euro["AMOUNT"].ToString();
                                your_cost = o_euro["BAL"].ToString();
                                balance = o_euro["AMOUNT"].ToString();
                                number = o_euro["MOBILE"].ToString();
                                operators = o_euro["OPID"].ToString();
                                //  operators = "ss";
                                operator_ref = "ss";
                                error_code = "ss";
                                message = o_euro["MSG"].ToString();
                                time = DateTime.Now.ToShortDateString();



                                if (status == "PENDING" || status == "SUCCESS")
                                {
                                    ConCls.DTH_recharge_api(CusID, mobile_no, operator_name, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
                                    if (status1 == "1")
                                    {
                                        mobilerechargepretrue service = new mobilerechargepretrue();
                                        service.txid = txid;
                                        service.user_txid = user_txid;
                                        service.status = status;
                                        service.amount = amount;
                                        service.your_cost = your_cost;
                                        service.balance = balance;
                                        service.number = number;
                                        service.operators = operators;
                                        service.operator_ref = operator_ref;
                                        service.error_code = error_code;
                                        service.message = message;
                                        service.time = time;

                                        string json = JsonConvert.SerializeObject(service);
                                        this.Context.Response.Write(json);
                                        // mobilerechargepretrue(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);
                                    }
                                    else
                                    {
                                        mobilerechargeprefalse service = new mobilerechargeprefalse();
                                        service.txid = txid;
                                        service.user_txid = user_txid;
                                        service.status = status;
                                        service.amount = amount;
                                        service.your_cost = your_cost;
                                        service.balance = balance;
                                        service.number = number;
                                        service.operators = operators;
                                        service.operator_ref = operator_ref;
                                        service.error_code = error_code;
                                        service.message = message;
                                        service.time = time;

                                        string json = JsonConvert.SerializeObject(service);
                                        this.Context.Response.Write(json);

                                        // mobilerechargeprefalse(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);
                                    }
                                }
                                else if (status == "FAILED")
                                {
                                    ConCls.transactionfailpdth(CusID, mobile_no, operator_name, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
                                    // mobilerechargeprefalse(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);

                                    mobilerechargeprefalse service = new mobilerechargeprefalse();
                                    service.txid = txid;
                                    service.user_txid = user_txid;
                                    service.status = status;
                                    service.amount = amount;
                                    service.your_cost = your_cost;
                                    service.balance = balance;
                                    service.number = number;
                                    service.operators = operators;
                                    service.operator_ref = operator_ref;
                                    service.error_code = error_code;
                                    service.message = message;
                                    service.time = time;

                                    string json = JsonConvert.SerializeObject(service);
                                    this.Context.Response.Write(json);
                                }
                            }
                            else
                            {
                                string txid = "";
                                string user_txid = "";
                                string status = "";
                                string your_cost = "";
                                string operator_ref = "";
                                string error_code = ErrCode1;
                                string message = ErrCode1;
                                string time = "";

                                string number = mobile_no;
                                string operators = operator_name;
                                string circle = "";
                                string amount = amount2;
                                string usertx = trID1;

                                mobilerechargeprefalse service = new mobilerechargeprefalse();
                                service.txid = txid;
                                service.user_txid = user_txid;
                                service.status = status;
                                service.amount = amount;
                                service.your_cost = your_cost;
                                service.balance = balance;
                                service.number = number;
                                service.operators = operators;
                                service.operator_ref = operator_ref;
                                service.error_code = error_code;
                                service.message = message;
                                service.time = time;

                                string json = JsonConvert.SerializeObject(service);
                                this.Context.Response.Write(json);

                                //  mobilerechargeprefalse(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);
                                //InsufficiantBalance(ErrCode1);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Service_Stop service = new Service_Stop();
                service.result = "false";
                service.message = "Bad network connection. please try after some time";


                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);

            }

        }
    }




    //Electricity Payment
    [WebMethod]
    public void Electricity(string CusID, string mobile_no, string operator_name, string amount2, string balance)
    {
        JObject jd = new JObject();
        jd.Add("result", "");
        JObject val = jd;
        string option = (string)val["result"];
        string JsonString = "";

        if (CusID == "" || mobile_no == "" || operator_name == "" || amount2 == "" || balance == "")
        {
            Service_Stop service = new Service_Stop();
            service.result = "false";
            service.message = "Please enter all fields";


            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);

        }
        else
        {
            try
            {
                SqlDataReader sdr3 = ConCls.GetdataReader("SELECT * FROM Config WHERE Permission=0 AND PageName='Electricity'");
                if (sdr3.HasRows)
                {

                    if (sdr3.Read())
                    {
                        Service_Stop service = new Service_Stop();
                        service.result = "false";
                        service.message = "This Service is Temporary Unavailable";


                        string json = JsonConvert.SerializeObject(service);
                        this.Context.Response.Write(json);
                    }
                }
                else
                {

                    string status1 = "0";
                    string ErrCode = "";
                    string trID1 = "";
                    string message1 = "";
                    string ErrCode1 = "";

                    SqlDataReader sdr = ConCls.GetdataReader("select max(AGENTID)+1 from mobile_log");
                    if (sdr.HasRows)
                    {
                        if (sdr.Read())
                        {
                            string trID11 = sdr[0].ToString();
                            //decimal trID111 = decimal.Parse(trID11);
                            //decimal tr = trID111 + 1;
                            //string tr1 = tr.ToString();

                            ConCls.Walletbalance(CusID, amount2, balance, out message1, out ErrCode1);

                            if (message1 == "1")
                            {
                                string userid = "8452056444";
                                string pass = "996581";
                                string mob = mobile_no;
                                string opt = operator_name;
                                string agentid = trID11;
                                // string circle = circle_id;
                                string amt = amount2;
                                string usertx = trID1;
                                string fmt = "json";
                                // string version = "4";


                                // Creating Parameters List
                                NameValueCollection Parameterslist = System.Web.HttpUtility.ParseQueryString(string.Empty);

                                Parameterslist["userid"] = userid.ToString();
                                Parameterslist["pass"] = pass.ToString();
                                Parameterslist["mob"] = mob.ToString();
                                Parameterslist["opt"] = opt.ToString();
                                Parameterslist["agentid"] = agentid.ToString();
                                // Parameterslist["circle"] = circle.ToString();
                                Parameterslist["amt"] = amt.ToString();
                                Parameterslist["fmt"] = fmt.ToString();
                                // Parameterslist["version"] = version.ToString();

                                string link = "http://roundpayapi.in/API/APIService.aspx?" + Parameterslist.ToString();

                                // Getting JSON Output
                                WebClient c_euro = new WebClient();
                                string data_euro = c_euro.DownloadString("http://roundpayapi.in/API/APIService.aspx?" + Parameterslist.ToString());
                                //Console.WriteLine(data);
                                JObject o_euro = JObject.Parse(data_euro);

                                // GEtting Values From JSON Data
                                JObject jObj = (JObject)JsonConvert.DeserializeObject(o_euro.ToString());
                                int countelement = jObj.Count;

                                string txid = "";
                                string user_txid = "";
                                string status = "";
                                string amount = "";
                                string your_cost = "";
                                //   string balancetest = "";
                                string number = "";
                                string operators = "";
                                string operator_ref = "";
                                string error_code = "";
                                string message = "";
                                string time = "";

                                txid = o_euro["AGENTID"].ToString();
                                user_txid = o_euro["RPID"].ToString();
                                status = o_euro["STATUS"].ToString();
                                amount = o_euro["AMOUNT"].ToString();
                                your_cost = o_euro["BAL"].ToString();
                                balance = o_euro["AMOUNT"].ToString();
                                number = o_euro["MOBILE"].ToString();
                                operators = o_euro["OPID"].ToString();
                                //  operators = "ss";
                                operator_ref = "ss";
                                error_code = o_euro["MSG"].ToString();
                                message = o_euro["MSG"].ToString();
                                time = DateTime.Now.ToShortDateString();




                                if (status == "PENDING" || status == "SUCCESS")
                                {
                                    ConCls.Electricity_api(CusID, mobile_no, operator_name, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
                                    if (status1 == "1")
                                    {
                                        mobilerechargepretrue service = new mobilerechargepretrue();
                                        service.txid = txid;
                                        service.user_txid = user_txid;
                                        service.status = status;
                                        service.amount = amount;
                                        service.your_cost = your_cost;
                                        service.balance = balance;
                                        service.number = number;
                                        service.operators = operators;
                                        service.operator_ref = operator_ref;
                                        service.error_code = error_code;
                                        service.message = message;
                                        service.time = time;

                                        string json = JsonConvert.SerializeObject(service);
                                        this.Context.Response.Write(json);
                                        // mobilerechargepretrue(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);
                                    }
                                    else
                                    {
                                        mobilerechargeprefalse service = new mobilerechargeprefalse();
                                        service.txid = txid;
                                        service.user_txid = user_txid;
                                        service.status = status;
                                        service.amount = amount;
                                        service.your_cost = your_cost;
                                        service.balance = balance;
                                        service.number = number;
                                        service.operators = operators;
                                        service.operator_ref = operator_ref;
                                        service.error_code = error_code;
                                        service.message = message;
                                        service.time = time;

                                        string json = JsonConvert.SerializeObject(service);
                                        this.Context.Response.Write(json);

                                        // mobilerechargeprefalse(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);
                                    }
                                }
                                else if (status == "FAILED")
                                {
                                    ConCls.transactionfailpElectricity(CusID, mobile_no, operator_name, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
                                    //  mobilerechargeprefalse(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);

                                    mobilerechargeprefalse service = new mobilerechargeprefalse();
                                    service.txid = txid;
                                    service.user_txid = user_txid;
                                    service.status = status;
                                    service.amount = amount;
                                    service.your_cost = your_cost;
                                    service.balance = balance;
                                    service.number = number;
                                    service.operators = operators;
                                    service.operator_ref = operator_ref;
                                    service.error_code = error_code;
                                    service.message = message;
                                    service.time = time;

                                    string json = JsonConvert.SerializeObject(service);
                                    this.Context.Response.Write(json);
                                }
                            }
                            else
                            {
                                string txid = "";
                                string user_txid = "";
                                string status = "";
                                string your_cost = "";
                                string operator_ref = "";
                                string error_code = ErrCode1;
                                string message = ErrCode1;
                                string time = "";

                                string number = mobile_no;
                                string operators = operator_name;
                                string circle = "";
                                string amount = amount2;
                                string usertx = trID1;

                                mobilerechargeprefalse service = new mobilerechargeprefalse();
                                service.txid = txid;
                                service.user_txid = user_txid;
                                service.status = status;
                                service.amount = amount;
                                service.your_cost = your_cost;
                                service.balance = balance;
                                service.number = number;
                                service.operators = operators;
                                service.operator_ref = operator_ref;
                                service.error_code = error_code;
                                service.message = message;
                                service.time = time;

                                string json = JsonConvert.SerializeObject(service);
                                this.Context.Response.Write(json);

                                //  mobilerechargeprefalse(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);
                                //InsufficiantBalance(ErrCode1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Service_Stop service = new Service_Stop();
                service.result = "false";
                service.message = "Bad network connection. please try after some time";


                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);

            }
        }
    }



    //Gas Payment
    [WebMethod]
    public void GasPayment(string CusID, string mobile_no, string operator_name, string amount2, string balance)
    {
        JObject jd = new JObject();
        jd.Add("result", "");
        JObject val = jd;
        string option = (string)val["result"];
        string JsonString = "";

        if (CusID == "" || mobile_no == "" || operator_name == "" || amount2 == "" || balance == "")
        {
            Service_Stop service = new Service_Stop();
            service.result = "false";
            service.message = "Please enter all fields";


            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);

        }
        else
        {

            try
            {

                SqlDataReader sdr3 = ConCls.GetdataReader("SELECT * FROM Config WHERE Permission=0 AND PageName='GasPayment'");
                if (sdr3.HasRows)
                {

                    if (sdr3.Read())
                    {
                        Service_Stop service = new Service_Stop();
                        service.result = "false";
                        service.message = "This Service is Temporary Unavailable";


                        string json = JsonConvert.SerializeObject(service);
                        this.Context.Response.Write(json);


                    }
                }
                else
                {
                    string status1 = "0";
                    string ErrCode = "";
                    string trID1 = "";
                    string message1 = "";
                    string ErrCode1 = "";

                    SqlDataReader sdr = ConCls.GetdataReader("select max(AGENTID)+1 from mobile_log");
                    if (sdr.HasRows)
                    {
                        if (sdr.Read())
                        {
                            string trID11 = sdr[0].ToString();
                            //decimal trID111 = decimal.Parse(trID11);
                            //decimal tr = trID111 + 1;
                            //string tr1 = tr.ToString();

                            ConCls.Walletbalance(CusID, amount2, balance, out message1, out ErrCode1);

                            if (message1 == "1")
                            {
                                string userid = "8452056444";
                                string pass = "996581";
                                string mob = mobile_no;
                                string opt = operator_name;
                                string agentid = trID11;
                                // string circle = circle_id;
                                string amt = amount2;
                                string usertx = trID1;
                                string fmt = "json";
                                // string version = "4";


                                // Creating Parameters List
                                NameValueCollection Parameterslist = System.Web.HttpUtility.ParseQueryString(string.Empty);

                                Parameterslist["userid"] = userid.ToString();
                                Parameterslist["pass"] = pass.ToString();
                                Parameterslist["mob"] = mob.ToString();
                                Parameterslist["opt"] = opt.ToString();
                                Parameterslist["agentid"] = agentid.ToString();
                                // Parameterslist["circle"] = circle.ToString();
                                Parameterslist["amt"] = amt.ToString();
                                Parameterslist["fmt"] = fmt.ToString();
                                // Parameterslist["version"] = version.ToString();

                                string link = "http://roundpayapi.in/API/APIService.aspx?" + Parameterslist.ToString();

                                // Getting JSON Output
                                WebClient c_euro = new WebClient();
                                string data_euro = c_euro.DownloadString("http://roundpayapi.in/API/APIService.aspx?" + Parameterslist.ToString());
                                //Console.WriteLine(data);
                                JObject o_euro = JObject.Parse(data_euro);

                                // GEtting Values From JSON Data
                                JObject jObj = (JObject)JsonConvert.DeserializeObject(o_euro.ToString());
                                int countelement = jObj.Count;

                                string txid = "";
                                string user_txid = "";
                                string status = "";
                                string amount = "";
                                string your_cost = "";
                                //   string balancetest = "";
                                string number = "";
                                string operators = "";
                                string operator_ref = "";
                                string error_code = "";
                                string message = "";
                                string time = "";

                                txid = o_euro["AGENTID"].ToString();
                                user_txid = o_euro["RPID"].ToString();
                                status = o_euro["STATUS"].ToString();
                                amount = o_euro["AMOUNT"].ToString();
                                your_cost = o_euro["BAL"].ToString();
                                balance = o_euro["AMOUNT"].ToString();
                                number = o_euro["MOBILE"].ToString();
                                operators = o_euro["OPID"].ToString();
                                //  operators = "ss";
                                operator_ref = "ss";
                                error_code = o_euro["MSG"].ToString();
                                message = o_euro["MSG"].ToString();
                                time = DateTime.Now.ToShortDateString();




                                if (status == "PENDING" || status == "SUCCESS")
                                {
                                    ConCls.Gas_api(CusID, mobile_no, operator_name, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
                                    if (status1 == "1")
                                    {
                                        mobilerechargepretrue service = new mobilerechargepretrue();
                                        service.txid = txid;
                                        service.user_txid = user_txid;
                                        service.status = status;
                                        service.amount = amount;
                                        service.your_cost = your_cost;
                                        service.balance = balance;
                                        service.number = number;
                                        service.operators = operators;
                                        service.operator_ref = operator_ref;
                                        service.error_code = error_code;
                                        service.message = message;
                                        service.time = time;

                                        string json = JsonConvert.SerializeObject(service);
                                        this.Context.Response.Write(json);
                                        // mobilerechargepretrue(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);
                                    }
                                    else
                                    {
                                        mobilerechargeprefalse service = new mobilerechargeprefalse();
                                        service.txid = txid;
                                        service.user_txid = user_txid;
                                        service.status = status;
                                        service.amount = amount;
                                        service.your_cost = your_cost;
                                        service.balance = balance;
                                        service.number = number;
                                        service.operators = operators;
                                        service.operator_ref = operator_ref;
                                        service.error_code = error_code;
                                        service.message = message;
                                        service.time = time;

                                        string json = JsonConvert.SerializeObject(service);
                                        this.Context.Response.Write(json);
                                        //  mobilerechargeprefalse(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);
                                    }
                                }
                                else if (status == "FAILED")
                                {
                                    ConCls.transactionfailpgas(CusID, mobile_no, operator_name, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
                                    //mobilerechargeprefalse(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);

                                    mobilerechargeprefalse service = new mobilerechargeprefalse();
                                    service.txid = txid;
                                    service.user_txid = user_txid;
                                    service.status = status;
                                    service.amount = amount;
                                    service.your_cost = your_cost;
                                    service.balance = balance;
                                    service.number = number;
                                    service.operators = operators;
                                    service.operator_ref = operator_ref;
                                    service.error_code = error_code;
                                    service.message = message;
                                    service.time = time;

                                    string json = JsonConvert.SerializeObject(service);
                                    this.Context.Response.Write(json);
                                }
                            }
                            else
                            {
                                string txid = "";
                                string user_txid = "";
                                string status = "";
                                string your_cost = "";
                                string operator_ref = "";
                                string error_code = ErrCode1;
                                string message = ErrCode1;
                                string time = "";

                                string number = mobile_no;
                                string operators = operator_name;
                                string circle = "";
                                string amount = amount2;
                                string usertx = trID1;

                                // mobilerechargeprefalse(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);

                                mobilerechargeprefalse service = new mobilerechargeprefalse();
                                service.txid = txid;
                                service.user_txid = user_txid;
                                service.status = status;
                                service.amount = amount;
                                service.your_cost = your_cost;
                                service.balance = balance;
                                service.number = number;
                                service.operators = operators;
                                service.operator_ref = operator_ref;
                                service.error_code = error_code;
                                service.message = message;
                                service.time = time;

                                string json = JsonConvert.SerializeObject(service);
                                this.Context.Response.Write(json);
                                //InsufficiantBalance(ErrCode1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Service_Stop service = new Service_Stop();
                service.result = "false";
                service.message = "Bad network connection. please try after some time";


                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);


            }

        }
    }


    //Insurance Payment
    [WebMethod]
    public void Insurance(string CusID, string mobile_no, string operator_name, string amount2, string balance, string dob1)
    {

        if (CusID == "" || mobile_no == "" || operator_name == "" || amount2 == "" || balance == "")
        {

            Service_Stop service = new Service_Stop();
            service.result = "false";
            service.message = "Please enter all fields";


            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);

        }
        else
        {
            try
            {
                SqlDataReader sdr3 = ConCls.GetdataReader("SELECT * FROM Config WHERE Permission=0 AND PageName='Insurance'");
                if (sdr3.HasRows)
                {

                    if (sdr3.Read())
                    {
                        Service_Stop service = new Service_Stop();
                        service.result = "false";
                        service.message = "This Service is Temporary Unavailable";


                        string json = JsonConvert.SerializeObject(service);
                        this.Context.Response.Write(json);

                    }
                }
                else
                {
                    string status1 = "0";
                    string ErrCode = "";
                    string trID1 = "";
                    string message1 = "";
                    string ErrCode1 = "";

                    SqlDataReader sdr = ConCls.GetdataReader("select max(AGENTID)+1 from mobile_log");
                    if (sdr.HasRows)
                    {
                        if (sdr.Read())
                        {
                            string trID11 = sdr[0].ToString();
                            //decimal trID111 = decimal.Parse(trID11);
                            //decimal tr = trID111 + 1;
                            //string tr1 = tr.ToString();

                            ConCls.Walletbalance(CusID, amount2, balance, out message1, out ErrCode1);

                            if (message1 == "1")
                            {

                                string userid = "8452056444";
                                string pass = "996581";
                                string mob = mobile_no;
                                string opt = operator_name;
                                string agentid = trID11;
                                // string circle = circle_id;
                                string amt = amount2;
                                string usertx = trID1;
                                string fmt = "json";
                                // string version = "4";


                                // Creating Parameters List
                                NameValueCollection Parameterslist = System.Web.HttpUtility.ParseQueryString(string.Empty);

                                Parameterslist["userid"] = userid.ToString();
                                Parameterslist["pass"] = pass.ToString();
                                Parameterslist["mob"] = mob.ToString();
                                Parameterslist["opt"] = opt.ToString();
                                Parameterslist["agentid"] = agentid.ToString();
                                // Parameterslist["circle"] = circle.ToString();
                                Parameterslist["amt"] = amt.ToString();
                                Parameterslist["fmt"] = fmt.ToString();
                                // Parameterslist["version"] = version.ToString();

                                string link = "http://roundpayapi.in/API/APIService.aspx?" + Parameterslist.ToString();

                                // Getting JSON Output
                                WebClient c_euro = new WebClient();
                                string data_euro = c_euro.DownloadString("http://roundpayapi.in/API/APIService.aspx?" + Parameterslist.ToString());
                                //Console.WriteLine(data);
                                JObject o_euro = JObject.Parse(data_euro);

                                // GEtting Values From JSON Data
                                JObject jObj = (JObject)JsonConvert.DeserializeObject(o_euro.ToString());
                                int countelement = jObj.Count;

                                string txid = "";
                                string user_txid = "";
                                string status = "";
                                string amount = "";
                                string your_cost = "";
                                //   string balancetest = "";
                                string number = "";
                                string operators = "";
                                string operator_ref = "";
                                string error_code = "";
                                string message = "";
                                string time = "";

                                txid = o_euro["AGENTID"].ToString();
                                user_txid = o_euro["RPID"].ToString();
                                status = o_euro["STATUS"].ToString();
                                amount = o_euro["AMOUNT"].ToString();
                                your_cost = o_euro["BAL"].ToString();
                                balance = o_euro["AMOUNT"].ToString();
                                number = o_euro["MOBILE"].ToString();
                                operators = o_euro["OPID"].ToString();
                                //  operators = "ss";
                                operator_ref = "ss";
                                error_code = "ss";
                                message = o_euro["MSG"].ToString();
                                time = DateTime.Now.ToShortDateString();



                                if (status == "PENDING" || status == "SUCCESS")
                                {
                                    ConCls.Gas_api(CusID, mobile_no, operator_name, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
                                    if (status1 == "1")
                                    {
                                        mobilerechargepretrue service = new mobilerechargepretrue();
                                        service.txid = txid;
                                        service.user_txid = user_txid;
                                        service.status = status;
                                        service.amount = amount;
                                        service.your_cost = your_cost;
                                        service.balance = balance;
                                        service.number = number;
                                        service.operators = operators;
                                        service.operator_ref = operator_ref;
                                        service.error_code = error_code;
                                        service.message = message;
                                        service.time = time;

                                        string json = JsonConvert.SerializeObject(service);
                                        this.Context.Response.Write(json);
                                        // mobilerechargepretrue(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);
                                    }
                                    else
                                    {

                                        mobilerechargeprefalse service = new mobilerechargeprefalse();
                                        service.txid = txid;
                                        service.user_txid = user_txid;
                                        service.status = status;
                                        service.amount = amount;
                                        service.your_cost = your_cost;
                                        service.balance = balance;
                                        service.number = number;
                                        service.operators = operators;
                                        service.operator_ref = operator_ref;
                                        service.error_code = error_code;
                                        service.message = message;
                                        service.time = time;

                                        string json = JsonConvert.SerializeObject(service);
                                        this.Context.Response.Write(json);

                                        //mobilerechargeprefalse(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);
                                    }
                                }
                                else if (status == "FAILED")
                                {
                                    ConCls.transactionfailInsurance(CusID, mobile_no, operator_name, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
                                    //  mobilerechargeprefalse(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);

                                    mobilerechargeprefalse service = new mobilerechargeprefalse();
                                    service.txid = txid;
                                    service.user_txid = user_txid;
                                    service.status = status;
                                    service.amount = amount;
                                    service.your_cost = your_cost;
                                    service.balance = balance;
                                    service.number = number;
                                    service.operators = operators;
                                    service.operator_ref = operator_ref;
                                    service.error_code = error_code;
                                    service.message = message;
                                    service.time = time;

                                    string json = JsonConvert.SerializeObject(service);
                                    this.Context.Response.Write(json);
                                }
                            }
                            else
                            {
                                string txid = "";
                                string user_txid = "";
                                string status = "";
                                string your_cost = "";
                                string operator_ref = "";
                                string error_code = ErrCode1;
                                string message = ErrCode1;
                                string time = "";

                                string number = mobile_no;
                                string operators = operator_name;
                                string circle = "";
                                string amount = amount2;
                                string usertx = trID1;


                                mobilerechargeprefalse service = new mobilerechargeprefalse();
                                service.txid = txid;
                                service.user_txid = user_txid;
                                service.status = status;
                                service.amount = amount;
                                service.your_cost = your_cost;
                                service.balance = balance;
                                service.number = number;
                                service.operators = operators;
                                service.operator_ref = operator_ref;
                                service.error_code = error_code;
                                service.message = message;
                                service.time = time;

                                string json = JsonConvert.SerializeObject(service);
                                this.Context.Response.Write(json);
                                // mobilerechargeprefalse(txid, user_txid, status, amount, your_cost, balance, number, operators, operator_ref, error_code, message, time);
                                //InsufficiantBalance(ErrCode1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Service_Stop service = new Service_Stop();
                service.result = "false";
                service.message = "Bad network connection. please try after some time";


                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);

            }

        }
    }


    [WebMethod]
    public void rechargeHistory(string CusID)
    {

        string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("select * from mobile_log where CusID=" + CusID + " AND message!='Credited by BIXC Coin' order by AGENTID desc"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        dt.TableName = "mobile_log";
                        sda.Fill(dt);
                        string json = JsonConvert.SerializeObject(dt);
                        //return dt;

                        dynamic jsonResponse = JsonConvert.DeserializeObject(json);

                        JArray jarray = new JArray(jsonResponse);

                        JObject jd = new JObject(
                        new JProperty("result", "true"),
                        new JProperty("Data", jarray));

                        string JsonString = jd.ToString();

                        this.Context.Response.Clear();
                        // JsonString = JsonConvert.SerializeObject(dt).ToString();
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


    [WebMethod]
    public void recharge_status(string CusID, string agent_id)
    {
        JObject jd = new JObject();
        jd.Add("result", "");
        JObject val = jd;
        string option = (string)val["result"];
        string JsonString = "";


        string userid = "8452056444";
        string pass = "996581";
        string csagentid = agent_id;
        string FMT = "JSON";


        // Creating Parameters List
        NameValueCollection Parameterslist = System.Web.HttpUtility.ParseQueryString(string.Empty);

        Parameterslist["userid"] = userid.ToString();
        Parameterslist["pass"] = pass.ToString();
        Parameterslist["csagentid"] = csagentid.ToString();
        Parameterslist["FMT"] = FMT.ToString();


        string link = "http://roundpayapi.in/API/StatusCheck.aspx?" + Parameterslist.ToString();

        // Getting JSON Output
        WebClient c_euro = new WebClient();
        string data_euro = c_euro.DownloadString("http://roundpayapi.in/API/StatusCheck.aspx?" + Parameterslist.ToString());
        //Console.WriteLine(data);
        JObject o_euro = JObject.Parse(data_euro);

        // GEtting Values From JSON Data
        JObject jObj = (JObject)JsonConvert.DeserializeObject(o_euro.ToString());
        int countelement = jObj.Count;

        string STATUS = "";
        string MOBILE = "";
        string AMOUNT = "";
        string RPID = "";
        string AGENTID = "";
        string OPID = "";
        string BAL = "";
        string MSG = "";




        STATUS = o_euro["STATUS"].ToString();
        MOBILE = o_euro["MOBILE"].ToString();
        AMOUNT = o_euro["AMOUNT"].ToString();
        RPID = o_euro["RPID"].ToString();
        AGENTID = o_euro["AGENTID"].ToString();
        OPID = o_euro["OPID"].ToString();
        BAL = o_euro["BAL"].ToString();
        MSG = o_euro["MSG"].ToString();

        string err_code = "";
        string NewID = "";

        ConCls.RechargeStatus(CusID, STATUS, MOBILE, AMOUNT, RPID, AGENTID, OPID, BAL, MSG, out err_code, out NewID);

        val = jd;
        option = (string)val["result"];
        val["result"] = "true";
        val["status"] = STATUS;
        val["MOBILE"] = MOBILE;
        val["AMOUNT"] = AMOUNT;
        val["RPID"] = RPID;
        val["AGENTID"] = AGENTID;
        val["OPID"] = OPID;
        val["BAL"] = BAL;
        val["MSG"] = MSG;
        val["MESSAGE"] = err_code;



        this.Context.Response.Clear();
        JsonString = JsonConvert.SerializeObject(jd).ToString();
        this.Context.Response.ContentType = "application/json";
        this.Context.Response.AddHeader("content-length", JsonString.Length.ToString());
        this.Context.Response.Flush();
        this.Context.Response.Write(JsonString);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }


    #region WALLET_DETAILS
    [WebMethod]
    public void WALLET_DETAILS(string CusID, string tokenno)
    {
        try
        {

            string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select top 10 * from e_Wallet_main  WHERE CusID='" + CusID + "'"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "e_Wallet_main";
                            sda.Fill(dt);

                            string json = JsonConvert.SerializeObject(dt);
 //                           return dt;

                            dynamic jsonResponse = JsonConvert.DeserializeObject(json);

                            JArray jarray = new JArray(jsonResponse);

                            JObject jd = new JObject(
                            new JProperty("result", "true"),
                            new JProperty("Data", jarray));

                            string JsonString = jd.ToString();
 //                           string JsonString = "";

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
        catch (Exception ex)
        {
            JObject jd = new JObject();
            jd.Add("result", "");
            JObject val = jd;
            string option = (string)val["result"];
            string JsonString = "";

            val = jd;
            option = (string)val["result"];
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

    #region #STACKING_PACKAGE_HISTORY
    [WebMethod]
    public void STACKING_PACKAGE_HISTORY(String CusID, string tokenno)

    {
        try
        {
            if (CusID != "")
            {


                string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("select serialno,cusid	,linktable,	month1	,status1,payment_relased,puramt,	purdate, (puramt*2 ) as Maximumern,	maxwithdraw,(maxwithdraw-payment_relased) as remain,	stat,	wallettype,	validity,receiptno,categories,rateofroi,expirytime,ROIEXPIRY ,boostertxn,main_w_tx,inc_w,inc_w_tx, coin_qty, coin_rate,pkgtype  from  investment_roi_monthly  where enab_disa = 1 and jtype=0  and cusid = " + CusID.ToString() + " ORDER BY purdate DESC"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "investment_roi_monthly";
                                sda.Fill(dt);

                                string json = JsonConvert.SerializeObject(dt);
  //                              return dt;

                                dynamic jsonResponse = JsonConvert.DeserializeObject(json);

                                JArray jarray = new JArray(jsonResponse);

                                JObject jd = new JObject(
                                new JProperty("result", "true"),
                                new JProperty("Data", jarray));

                                string JsonString = jd.ToString();
 //                               string JsonString = "";

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
    # endregion

    #region #LATEST_TRANSACTION_Dashboard
    [WebMethod]
    public void LATEST_TRANSACTION(String CusID, string tokenno)

    {
        try
        {
            if (CusID != "" || tokenno != "")
            {


                string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("Select top 10 TrID,CustID,e_wallet_mycoinwallet.dot,Credit,Debit,Description,servicecharge,TxID,Crypto_tx,BTCAmt,BTC_Rate,e_wallet_mycoinwallet.Status,MarketName  from e_wallet_mycoinwallet ,Market_Coin  Where   CustID=" + CusID.ToString() + " and e_wallet_mycoinwallet.MarketID=Market_Coin.MarketID order by e_wallet_mycoinwallet.dot desc"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "e_wallet_mycoinwallet";
                                sda.Fill(dt);

                                string json = JsonConvert.SerializeObject(dt);
                                //return dt;

                                dynamic jsonResponse = JsonConvert.DeserializeObject(json);

                                JArray jarray = new JArray(jsonResponse);

                                JObject jd = new JObject(
                                new JProperty("result", "true"),
                                new JProperty("Data", jarray));

                                string JsonString = jd.ToString();
                                //  string JsonString = "";

                                this.Context.Response.Clear();
                                //JsonString = JsonConvert.SerializeObject(jd).ToString();
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

    #region #GetTransactions_WithdrawFundBTC

    [WebMethod]
    public void GetTransactions_WithdrawFundBTC(string CustID)
    {
        try
        {
            if (CusID != "")
            {
                string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT rqno, custid,  PtbWidrawRequest.status, RqAmount,AppAmount,DedAmount, reqDate, AdminStatus, ApprovedDate, AdminComments, WithdrawAddress, TFee,ModeType,tstatus,BTC_Amt,'BTC' AS Category,TxID,PTC_Amt,PTC_Rate FROM PtbWidrawRequest Where custid = " + CustID + " AND ModeType=1"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "PtbWidrawRequest";
                                sda.Fill(dt);

                                string json = JsonConvert.SerializeObject(dt);
                                //return dt;

                                dynamic jsonResponse = JsonConvert.DeserializeObject(json);

                                JArray jarray = new JArray(jsonResponse);

                                JObject jd = new JObject(
                                new JProperty("result", "true"),
                                new JProperty("Data", jarray));

                                string JsonString = jd.ToString();
                                //  string JsonString = "";

                                this.Context.Response.Clear();
                                //JsonString = JsonConvert.SerializeObject(jd).ToString();
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


    #region #STAKING_PACKAGE_HISTORY_Dashboard
    [WebMethod]
    public void STAKING_PACKAGE_HISTORY(String CusID, string tokenno)

    {
        try
        {
            if (CusID != "")
            {


                string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("Select serialno,cusid	,linktable,	month1	,status1,payment_relased,puramt,	purdate,	maxwithdraw,	stat,	wallettype,	validity,receiptno,categories from  investment_roi_monthly  where enab_disa = 1 and cusid = " + CusID.ToString() + " ORDER BY purdate DESC"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "investment_roi_monthly";
                                sda.Fill(dt);

                                string json = JsonConvert.SerializeObject(dt);
                                //return dt;

                                dynamic jsonResponse = JsonConvert.DeserializeObject(json);

                                JArray jarray = new JArray(jsonResponse);

                                JObject jd = new JObject(
                                new JProperty("result", "true"),
                                new JProperty("Data", jarray));

                                string JsonString = jd.ToString();
                                //  string JsonString = "";

                                this.Context.Response.Clear();
                                //JsonString = JsonConvert.SerializeObject(jd).ToString();
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

    #region #BUY_SHARE_PROFIT_STAKING_PACKAGE_HISTORY
    [WebMethod]
    public void BUY_SHARE_PROFIT(String CusID,String linktable, string tokenno)

    {
        try
        {
            if (CusID != "" || tokenno != "" || linktable != "")
            {


                string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("Select slno,linktable,txno,invmnt_no,custid,dot,credit,debit,description,rate,pno from investment_profit WHERE custid =" + CusID.ToString() + " and linktable =" + linktable.ToString() + "   order by  dot desc"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "investment_profit";
                                sda.Fill(dt);

                                string json = JsonConvert.SerializeObject(dt);
                                //return dt;

                                dynamic jsonResponse = JsonConvert.DeserializeObject(json);

                                JArray jarray = new JArray(jsonResponse);

                                JObject jd = new JObject(
                                new JProperty("result", "true"),
                                new JProperty("Data", jarray));

                                string JsonString = jd.ToString();
                                //  string JsonString = "";

                                this.Context.Response.Clear();
                                //JsonString = JsonConvert.SerializeObject(jd).ToString();
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

    #region #WITHDRAW_DETAILS   
    [WebMethod]
    public void WITHDRAW_DETAILS(String CusID, string tokenno)

    {
        try
        {
            if (CusID != "" || tokenno != "")
            {


                string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT rqno, custid, dbo.CustRecords.Cust_UserName, status, RqAmount, AppAmount, DedAmount, reqDate, AdminStatus, ApprovedDate, AdminComments, WithdrawAddress, TFee, ModeType, tstatus, BTC_Amt FROM PtbWidrawRequest INNER JOIN CustRecords ON custid = CusID Where custid = " + CusID.ToString() + "  order by reqDate desc "))
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

                                JObject jd = new JObject(
                                new JProperty("result", "true"),
                                new JProperty("Data", jarray));

                                string JsonString = jd.ToString();
                                //  string JsonString = "";

                                this.Context.Response.Clear();
                                //JsonString = JsonConvert.SerializeObject(jd).ToString();
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

    #region #GET_TOTALINCOME     
    [WebMethod]
    public void GET_TOTALINCOME(String CusID, string tokenno)

    {
        try
        {
            if (CusID != "" || tokenno != "")
            {


                string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("select sum(credit) as Amt from e_wallet_mycoinwallet  where marketid=1 and  CustID=" + CusID))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "e_wallet_mycoinwallet";
                                sda.Fill(dt);

                                string json = JsonConvert.SerializeObject(dt);
                                //return dt;

                                dynamic jsonResponse = JsonConvert.DeserializeObject(json);

                                JArray jarray = new JArray(jsonResponse);

                                JObject jd = new JObject(
                                new JProperty("result", "true"),
                                new JProperty("Data", jarray));

                                string JsonString = jd.ToString();
                                //  string JsonString = "";

                                this.Context.Response.Clear();
                                //JsonString = JsonConvert.SerializeObject(jd).ToString();
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

    #region #GET_WithdrawBalance     
    [WebMethod]
    public void GET_WithdrawBalance(String CusID, string tokenno)

    {
        try
        {
            if (CusID != "" || tokenno != "")
            {


                string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("Select ISNULL(SUM(RqAmount),0) AS Amt from    PtbWidrawRequest where AdminStatus ='Approved' and  custid=" + CusID))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "PtbWidrawRequest";
                                sda.Fill(dt);

                                string json = JsonConvert.SerializeObject(dt);
                                //return dt;

                                dynamic jsonResponse = JsonConvert.DeserializeObject(json);

                                JArray jarray = new JArray(jsonResponse);

                                JObject jd = new JObject(
                                new JProperty("result", "true"),
                                new JProperty("Data", jarray));

                                string JsonString = jd.ToString();
                                //  string JsonString = "";

                                this.Context.Response.Clear();
                                //JsonString = JsonConvert.SerializeObject(jd).ToString();
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


}
