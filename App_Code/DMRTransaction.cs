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
/// Summary description for yours4organic
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class DMRTransaction : System.Web.Services.WebService {

    Connectioncls ConCls = new Connectioncls();
    public DMRTransaction() {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }


    string ErrCode = "";

   
    //DMR Get Sender Info
    [WebMethod]
    public void DMRlogin(string SenderMobileNo)
    {
        string UMobileNo = "8452056444";
        string Password = "nG143J7HQ2oZ/YUEH42TzQ==";


        // Creating Parameters List
        NameValueCollection Parameterslist = System.Web.HttpUtility.ParseQueryString(string.Empty);

        Parameterslist["SenderMobileNo"] = SenderMobileNo.ToString();
        Parameterslist["UMobileNo"] = UMobileNo.ToString();
        Parameterslist["Password"] = Password.ToString();

        var link = "http://staging.mrupay.in//Api/DMRWebService.asmx/GetSenderInfo?" + Parameterslist.ToString();

        // Getting JSON Output
        WebClient c_euro = new WebClient();
        var data_euro = c_euro.DownloadString("http://staging.mrupay.in//Api/DMRWebService.asmx/GetSenderInfo?" + Parameterslist.ToString());

        XDocument xml = XDocument.Parse(data_euro);
        // XElement myEle = XElement.Parse(data_euro);

        List<string> list = xml.Descendants("RESPONSESTATUS").Select(e => e.Value).ToList();

        string RESPONSESTATUS = string.Join(Environment.NewLine, list.ToArray());

        List<string> list1 = xml.Descendants("MESSAGE").Select(e => e.Value).ToList();

        string MESSAGE = string.Join(Environment.NewLine, list1.ToArray());

        if (RESPONSESTATUS == "0")
        {
            DMRRegister service = new DMRRegister();
            service.MESSAGE = MESSAGE;

            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);
        }
        else if (RESPONSESTATUS == "1")
        {
            DMRRegister service = new DMRRegister();
            service.MESSAGE = "Login Successfully";

            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);

        }

    }

    //DMR Create Account
    [WebMethod]
    public void DMRRegister(string SenderMobileNo, string SenderName)
    {
        string UMobileNo = "8452056444";
        string Password = "nG143J7HQ2oZ/YUEH42TzQ==";


        // Creating Parameters List
        NameValueCollection Parameterslist = System.Web.HttpUtility.ParseQueryString(string.Empty);

        Parameterslist["SenderMobileNo"] = SenderMobileNo.ToString();
        Parameterslist["SenderName"] = SenderName.ToString();
        Parameterslist["UMobileNo"] = UMobileNo.ToString();
        Parameterslist["Password"] = Password.ToString();
        //Parameterslist["frm"] = "json";

        var link = "http://staging.mrupay.in//Api/DMRWebService.asmx/CreateSender?" + Parameterslist.ToString();

        // Getting JSON Output  
        WebClient c_euro = new WebClient();
        var data_euro = c_euro.DownloadString("http://staging.mrupay.in//Api/DMRWebService.asmx/CreateSender?" + Parameterslist.ToString());

        XDocument xml = XDocument.Parse(data_euro);
        // XElement myEle = XElement.Parse(data_euro);


        List<string> list = xml.Descendants("RESPONSESTATUS").Select(e => e.Value).ToList();

        string RESPONSESTATUS = string.Join(Environment.NewLine, list.ToArray());

    

        if(RESPONSESTATUS == "0")
        {
            RESPONSESTATUS0(SenderMobileNo, SenderName);

        
        }
        else if (RESPONSESTATUS == "1")
        {

            RESPONSESTATUS1(SenderMobileNo, SenderName);

        
        }
     }


    //DMR Resend OTP 
    [WebMethod]
    public void DMRResendOTP(string SenderMobileNo)
    {
        string UMobileNo = "8452056444";
        string Password = "nG143J7HQ2oZ/YUEH42TzQ==";


        // Creating Parameters List
        NameValueCollection Parameterslist = System.Web.HttpUtility.ParseQueryString(string.Empty);

        Parameterslist["SenderMobileNo"] = SenderMobileNo.ToString();
        Parameterslist["UMobileNo"] = UMobileNo.ToString();
        Parameterslist["Password"] = Password.ToString();

        var link = "http://staging.mrupay.in//Api/DMRWebService.asmx/ResendOtp?" + Parameterslist.ToString();

        // Getting JSON Output
        WebClient c_euro = new WebClient();
        var data_euro = c_euro.DownloadString("http://staging.mrupay.in//Api/DMRWebService.asmx/ResendOtp?" + Parameterslist.ToString());

        XDocument xml = XDocument.Parse(data_euro);
        // XElement myEle = XElement.Parse(data_euro);

        List<string> list = xml.Descendants("RESPONSESTATUS").Select(e => e.Value).ToList();

        string RESPONSESTATUS = string.Join(Environment.NewLine, list.ToArray());

        List<string> list1 = xml.Descendants("MESSAGE").Select(e => e.Value).ToList();

        string MESSAGE = string.Join(Environment.NewLine, list1.ToArray());

        if (RESPONSESTATUS == "0")
        {
            DMRRegister service = new DMRRegister();
            service.MESSAGE = "OTP Not Send";

            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);
        }
        else if (RESPONSESTATUS == "1")
        {
            DMRRegister service = new DMRRegister();
            service.MESSAGE = MESSAGE;

            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);

        }

    }

    public void RESPONSESTATUS0(string SenderMobileNo, string SenderName)
    {
        string UMobileNo = "8452056444";
        string Password = "nG143J7HQ2oZ/YUEH42TzQ==";


        // Creating Parameters List
        NameValueCollection Parameterslist = System.Web.HttpUtility.ParseQueryString(string.Empty);

        Parameterslist["SenderMobileNo"] = SenderMobileNo.ToString();
        Parameterslist["SenderName"] = SenderName.ToString();
        Parameterslist["UMobileNo"] = UMobileNo.ToString();
        Parameterslist["Password"] = Password.ToString();
        //Parameterslist["frm"] = "json";

        var link = "http://staging.mrupay.in//Api/DMRWebService.asmx/CreateSender?" + Parameterslist.ToString();

        // Getting JSON Output
        WebClient c_euro = new WebClient();
        var data_euro = c_euro.DownloadString("http://staging.mrupay.in//Api/DMRWebService.asmx/CreateSender?" + Parameterslist.ToString());

        XDocument xml = XDocument.Parse(data_euro);
        // XElement myEle = XElement.Parse(data_euro);

        List<string> list = xml.Descendants("MESSAGE").Select(e => e.Value).ToList();

        string MESSAGE = string.Join(Environment.NewLine, list.ToArray());
        
        DMRRegister service = new DMRRegister();
        service.MESSAGE = MESSAGE;

        string json = JsonConvert.SerializeObject(service);
        this.Context.Response.Write(json);
    }


    public void RESPONSESTATUS1(string SenderMobileNo, string SenderName)
    {
        string UMobileNo = "8452056444";
        string Password = "nG143J7HQ2oZ/YUEH42TzQ==";
         
        ConCls.ExecuteSqlnonQuery("insert into DMRCustromer values(" + SenderMobileNo + ",'" + SenderName + "')");

        // Creating Parameters List
        NameValueCollection Parameterslist = System.Web.HttpUtility.ParseQueryString(string.Empty);

        Parameterslist["SenderMobileNo"] = SenderMobileNo.ToString();
        Parameterslist["SenderName"] = SenderName.ToString();
        Parameterslist["UMobileNo"] = UMobileNo.ToString();
        Parameterslist["Password"] = Password.ToString();
        //Parameterslist["frm"] = "json";

        var link = "http://staging.mrupay.in//Api/DMRWebService.asmx/CreateSender?" + Parameterslist.ToString();

        // Getting JSON Output
        WebClient c_euro = new WebClient();
        var data_euro = c_euro.DownloadString("http://staging.mrupay.in//Api/DMRWebService.asmx/CreateSender?" + Parameterslist.ToString());

        XDocument xml = XDocument.Parse(data_euro);
        // XElement myEle = XElement.Parse(data_euro);
        List<string> list = xml.Descendants("MESSAGE").Select(e => e.Value).ToList();

        string MESSAGE = string.Join(Environment.NewLine, list.ToArray());

        DMRRegister service = new DMRRegister();
        service.MESSAGE = MESSAGE;

        string json = JsonConvert.SerializeObject(service);
        this.Context.Response.Write(json);

   
    }

    //DMR Verify Sender
    [WebMethod]
    public void DMRVerifySender(string SenderMobileNo, string otp)
    {
        string UMobileNo = "8452056444";
        string Password = "nG143J7HQ2oZ/YUEH42TzQ==";


        // Creating Parameters List
        NameValueCollection Parameterslist = System.Web.HttpUtility.ParseQueryString(string.Empty);

        Parameterslist["SenderMobileNo"] = SenderMobileNo.ToString();
        Parameterslist["OTP"] = otp.ToString();
        Parameterslist["UMobileNo"] = UMobileNo.ToString();
        Parameterslist["Password"] = Password.ToString();
        //Parameterslist["frm"] = "json";

        var link = "http://staging.mrupay.in//Api/DMRWebService.asmx/VerifySender?" + Parameterslist.ToString();

        // Getting JSON Output
        WebClient c_euro = new WebClient();
        var data_euro = c_euro.DownloadString("http://staging.mrupay.in//Api/DMRWebService.asmx/VerifySender?" + Parameterslist.ToString());

        XDocument xml = XDocument.Parse(data_euro);
        // XElement myEle = XElement.Parse(data_euro);


        List<string> list = xml.Descendants("RESPONSESTATUS").Select(e => e.Value).ToList();

        string RESPONSESTATUS = string.Join(Environment.NewLine, list.ToArray());

        List<string> list1 = xml.Descendants("MESSAGE").Select(e => e.Value).ToList();

        string MESSAGE = string.Join(Environment.NewLine, list1.ToArray());


        if (RESPONSESTATUS == "0")
        {
            DMRRegister service = new DMRRegister();
            service.MESSAGE = "Wrong OTP";

            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);
        }
        else if (RESPONSESTATUS == "1")
        {
            DMRRegister service = new DMRRegister();
            service.MESSAGE = "Correct OTP";

            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);

        }
    }



    //DMR Add Beneficiary
    [WebMethod]
    public void DMRAddBeneficiary(string SenderMobileNo, string name, string RMobileNo, string BankAccount,string BankCode_IFSC)
    {
        string UMobileNo = "8452056444";
        string Password = "nG143J7HQ2oZ/YUEH42TzQ==";


        // Creating Parameters List
        NameValueCollection Parameterslist = System.Web.HttpUtility.ParseQueryString(string.Empty);

        Parameterslist["SenderMobileNo"] = SenderMobileNo.ToString();
        Parameterslist["name"] = name.ToString();
        Parameterslist["UMobileNo"] = UMobileNo.ToString();
        Parameterslist["Password"] = Password.ToString();
        Parameterslist["RMobileNo"] = RMobileNo.ToString();
        Parameterslist["BankAccount"] = BankAccount.ToString();
        Parameterslist["BankCode_IFSC"] = BankCode_IFSC.ToString();


        var link = "http://staging.mrupay.in//Api/DMRWebService.asmx/AddBeneficiary?" + Parameterslist.ToString();

        // Getting JSON Output
        WebClient c_euro = new WebClient();
        var data_euro = c_euro.DownloadString("http://staging.mrupay.in//Api/DMRWebService.asmx/AddBeneficiary?" + Parameterslist.ToString());

        XDocument xml = XDocument.Parse(data_euro);
        // XElement myEle = XElement.Parse(data_euro);


        List<string> list = xml.Descendants("RESPONSESTATUS").Select(e => e.Value).ToList();

        string RESPONSESTATUS = string.Join(Environment.NewLine, list.ToArray());

        List<string> list1 = xml.Descendants("MESSAGE").Select(e => e.Value).ToList();

        string MESSAGE = string.Join(Environment.NewLine, list1.ToArray());


        if (RESPONSESTATUS == "0")
        {
            
        }
        else if (RESPONSESTATUS == "1")
        {
            List<string> list3 = xml.Descendants("RECIPIENTID").Select(e => e.Value).ToList();

            string RECIPIENTID = string.Join(Environment.NewLine, list3.ToArray());

            ConCls.ExecuteSqlnonQuery("insert into DMRBeneficiary (SenderMobileNo,name,RMobileNo,BankAccount,BankCode_IFSC,Verify_status,recipirntid) values('" + SenderMobileNo+"','"+name+"','"+RMobileNo+"','"+BankAccount+"','"+BankCode_IFSC+"',0,'"+ RECIPIENTID + "')");

        }

        DMRRegister service = new DMRRegister();
        service.MESSAGE = MESSAGE;

        string json = JsonConvert.SerializeObject(service);
        this.Context.Response.Write(json);
    }


    //DMR Verify Beneficiary
    [WebMethod]
    public void DMRVerifyBeneficiary(string SenderMobileNo,string BankAccount, string BackCode)
    {
        string UMobileNo = "8452056444";
        string Password = "nG143J7HQ2oZ/YUEH42TzQ==";


        // Creating Parameters List
        NameValueCollection Parameterslist = System.Web.HttpUtility.ParseQueryString(string.Empty);

        Parameterslist["SenderMobileNo"] = SenderMobileNo.ToString();
        Parameterslist["BankAccount"] = BankAccount.ToString();
        Parameterslist["BackCode"] = BackCode.ToString();
        Parameterslist["UMobileNo"] = UMobileNo.ToString();
        Parameterslist["Password"] = Password.ToString();


        var link = "http://staging.mrupay.in//Api/DMRWebService.asmx/VerifyBeneficiary?" + Parameterslist.ToString();

        // Getting JSON Output
        WebClient c_euro = new WebClient();
        var data_euro = c_euro.DownloadString("http://staging.mrupay.in//Api/DMRWebService.asmx/VerifyBeneficiary?" + Parameterslist.ToString());

        XDocument xml = XDocument.Parse(data_euro);
        // XElement myEle = XElement.Parse(data_euro);


        List<string> list = xml.Descendants("RESPONSESTATUS").Select(e => e.Value).ToList();

        string RESPONSESTATUS = string.Join(Environment.NewLine, list.ToArray());

        List<string> list1 = xml.Descendants("MESSAGE").Select(e => e.Value).ToList();

        string MESSAGE = string.Join(Environment.NewLine, list1.ToArray());


        if (RESPONSESTATUS == "0")
        {
            DMRRegister service = new DMRRegister();
            service.MESSAGE = MESSAGE;

            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);
        }
        else if (RESPONSESTATUS == "1")
        {

            ConCls.ExecuteSqlnonQuery("update DMRBeneficiary set Verify_status=1 where SenderMobileNo='" + SenderMobileNo + "' AND BankAccount='" + BankAccount + "'");
            DMRRegister service = new DMRRegister();
            service.MESSAGE = MESSAGE;

            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);

        }


    }


    //DMR Delete Beneficiary
    [WebMethod]
    public void DMRDeleteBeneficiaryBeneficiary(string SenderMobileNo, string RecipientId)
    {
        string UMobileNo = "8452056444";
        string Password = "nG143J7HQ2oZ/YUEH42TzQ==";


        // Creating Parameters List
        NameValueCollection Parameterslist = System.Web.HttpUtility.ParseQueryString(string.Empty);

        Parameterslist["SenderMobileNo"] = SenderMobileNo.ToString();
        Parameterslist["UMobileNo"] = UMobileNo.ToString();
        Parameterslist["Password"] = Password.ToString();
        Parameterslist["RecipientId"] = RecipientId.ToString();


        var link = "http://staging.mrupay.in//Api/DMRWebService.asmx/DeleteBeneficiary?" + Parameterslist.ToString();

        // Getting JSON Output
        WebClient c_euro = new WebClient();
        var data_euro = c_euro.DownloadString("http://staging.mrupay.in//Api/DMRWebService.asmx/DeleteBeneficiary?" + Parameterslist.ToString());

        XDocument xml = XDocument.Parse(data_euro);
        // XElement myEle = XElement.Parse(data_euro);


        List<string> list = xml.Descendants("RESPONSESTATUS").Select(e => e.Value).ToList();

        string RESPONSESTATUS = string.Join(Environment.NewLine, list.ToArray());

        List<string> list1 = xml.Descendants("MESSAGE").Select(e => e.Value).ToList();

        string MESSAGE = string.Join(Environment.NewLine, list1.ToArray());


        if (RESPONSESTATUS == "0")
        {
            DMRRegister service = new DMRRegister();
            service.MESSAGE = MESSAGE;

            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);
        }
        else if (RESPONSESTATUS == "1")
        {

            ConCls.ExecuteSqlnonQuery("Delete from DMRBeneficiary where recipirntid='" + RecipientId + "'");


            DMRRegister service = new DMRRegister();
            service.MESSAGE = "Delete Beneficiary Successfully";

            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);

        }

    }



    [WebMethod]
    public void DMRBeneficiaryList(string SenderMobileNo)
    {

        string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("select * from DMRBeneficiary where SenderMobileNo='"+ SenderMobileNo + "' AND Verify_status=1"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        dt.TableName = "DMRBeneficiary";
                        sda.Fill(dt);
                        var json = JsonConvert.SerializeObject(dt);
                        //return dt;

                        dynamic jsonResponse = JsonConvert.DeserializeObject(json);

                        JArray jarray = new JArray(jsonResponse);

                        JObject jd = new JObject(
                        new JProperty("result", "true"),
                        new JProperty("message", jarray));

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
    public void BankDetail()
    {

        string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("select * from BankDetail"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        dt.TableName = "BankDetail";
                        sda.Fill(dt);
                        var json = JsonConvert.SerializeObject(dt);
                        //return dt;

                        dynamic jsonResponse = JsonConvert.DeserializeObject(json);

                        JArray jarray = new JArray(jsonResponse);

                        JObject jd = new JObject(
                        new JProperty("result", "true"),
                        new JProperty("message", jarray));

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



    //DMR Send Money
    [WebMethod]
    public void DMRSendMoney(string CusID, string SenderMobileNo, string BankAccount, string Amount, string Recipientid, string Channel,string IMEI,string balance)
    {
        string CustID = CusID;
        string amount2 = Amount;
        string ErrCode1 = "";
        string message1 = "";

        ConCls.Walletbalance(CusID, amount2, balance, out message1, out ErrCode1);

        if (message1 == "1")
        {
            string flag = "";
            string errMsg = "";

            ConCls.checklimit(CusID, Amount, out flag, out errMsg);

            if (flag == "1")
            {
                DMRRegister service = new DMRRegister();
                service.MESSAGE = errMsg;

                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);

            }
            else
            {
                string UMobileNo = "8452056444";
                string Password = "nG143J7HQ2oZ/YUEH42TzQ==";


                // Creating Parameters List
                NameValueCollection Parameterslist = System.Web.HttpUtility.ParseQueryString(string.Empty);

                Parameterslist["SenderMobileNo"] = SenderMobileNo.ToString();
                Parameterslist["BankAccount"] = BankAccount.ToString();
                Parameterslist["UMobileNo"] = UMobileNo.ToString();
                Parameterslist["Password"] = Password.ToString();
                Parameterslist["Amount"] = Amount.ToString();
                Parameterslist["BankAccount"] = BankAccount.ToString();
                Parameterslist["Recipientid"] = Recipientid.ToString();
                Parameterslist["Channel"] = Channel.ToString();
                Parameterslist["IMEI"] = IMEI.ToString();

                var link = "http://staging.mrupay.in//Api/DMRWebService.asmx/SendMoney?" + Parameterslist.ToString();

                // Getting JSON Output
                WebClient c_euro = new WebClient();
                var data_euro = c_euro.DownloadString("http://staging.mrupay.in//Api/DMRWebService.asmx/SendMoney?" + Parameterslist.ToString());

                XDocument xml = XDocument.Parse(data_euro);
                // XElement myEle = XElement.Parse(data_euro);


                List<string> list = xml.Descendants("RESPONSESTATUS").Select(e => e.Value).ToList();

                string RESPONSESTATUS = string.Join(Environment.NewLine, list.ToArray());

                List<string> list1 = xml.Descendants("MESSAGE").Select(e => e.Value).ToList();

                string MESSAGE = string.Join(Environment.NewLine, list1.ToArray());

                string NewID = "";
                if (RESPONSESTATUS == "0")
                {
                    ConCls.DMRTransactionFail(CusID, SenderMobileNo, BankAccount, Amount, Recipientid, Channel, IMEI, MESSAGE, out NewID, out ErrCode);

                    DMRRegister service = new DMRRegister();
                    service.MESSAGE = MESSAGE;

                    string json = JsonConvert.SerializeObject(service);
                    this.Context.Response.Write(json);
                }
                else if (RESPONSESTATUS == "1")
                {
                    ConCls.DMRTransaction(CusID, SenderMobileNo, BankAccount, Amount, Recipientid, Channel, IMEI, MESSAGE, out NewID, out ErrCode);

                    DMRRegister service = new DMRRegister();
                    service.MESSAGE = MESSAGE;

                    string json = JsonConvert.SerializeObject(service);
                    this.Context.Response.Write(json);

                }
            }
        }
        else
        {
            DMRRegister service = new DMRRegister();
            service.MESSAGE = ErrCode1;

            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);
        }
    }



    //DMR Quick Send Money
    [WebMethod]
    public void DMRQuickSendMoney(string CusID, string SenderMobileNo, string BankAccount, string Amount, string Recipientid, string Channel, string IMEI,string balance)
    {
        string CustID = CusID;
        string amount2 = Amount;
        string ErrCode1 = "";
        string message1 = "";

        ConCls.Walletbalance(CusID, amount2, balance, out message1, out ErrCode1);

        if (message1 == "1")
        {
            string flag = "";
        string errMsg = "";

        ConCls.checklimit(CusID, Amount, out flag, out errMsg);

        if (flag == "1")
        {
            DMRRegister service = new DMRRegister();
            service.MESSAGE = errMsg;

            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);

        }
        else
        {
            string UMobileNo = "8452056444";
            string Password = "nG143J7HQ2oZ/YUEH42TzQ==";


            // Creating Parameters List
            NameValueCollection Parameterslist = System.Web.HttpUtility.ParseQueryString(string.Empty);

            Parameterslist["SenderMobileNo"] = SenderMobileNo.ToString();
            Parameterslist["BankAccount"] = BankAccount.ToString();
            Parameterslist["UMobileNo"] = UMobileNo.ToString();
            Parameterslist["Password"] = Password.ToString();
            Parameterslist["Amount"] = Amount.ToString();
            Parameterslist["BankAccount"] = BankAccount.ToString();
            Parameterslist["Recipientid"] = Recipientid.ToString();
            Parameterslist["Channel"] = Channel.ToString();
            Parameterslist["IMEI"] = IMEI.ToString();

            var link = "http://staging.mrupay.in//Api/DMRWebService.asmx/SendMoney?" + Parameterslist.ToString();

            // Getting JSON Output
            WebClient c_euro = new WebClient();
            var data_euro = c_euro.DownloadString("http://staging.mrupay.in//Api/DMRWebService.asmx/SendMoney?" + Parameterslist.ToString());

            XDocument xml = XDocument.Parse(data_euro);
            // XElement myEle = XElement.Parse(data_euro);


            List<string> list = xml.Descendants("RESPONSESTATUS").Select(e => e.Value).ToList();

            string RESPONSESTATUS = string.Join(Environment.NewLine, list.ToArray());

            List<string> list1 = xml.Descendants("MESSAGE").Select(e => e.Value).ToList();

            string MESSAGE = string.Join(Environment.NewLine, list1.ToArray());

            string NewID = "";
            if (RESPONSESTATUS == "0")
            {
                ConCls.DMRTransactionFail(CusID, SenderMobileNo, BankAccount, Amount, Recipientid, Channel, IMEI, MESSAGE, out NewID, out ErrCode);

                DMRRegister service = new DMRRegister();
                service.MESSAGE = MESSAGE;

                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);
            }
            else if (RESPONSESTATUS == "1")
            {
                ConCls.DMRTransaction(CusID, SenderMobileNo, BankAccount, Amount, Recipientid, Channel, IMEI, MESSAGE, out NewID, out ErrCode);

                DMRRegister service = new DMRRegister();
                service.MESSAGE = MESSAGE;

                string json = JsonConvert.SerializeObject(service);
                this.Context.Response.Write(json);

            }
        }
        }
        else
        {
            DMRRegister service = new DMRRegister();
            service.MESSAGE = ErrCode1;

            string json = JsonConvert.SerializeObject(service);
            this.Context.Response.Write(json);
        }
    }



    [WebMethod]
    public void DMRTransactionList(string CusID)
    {

        string constr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("select * from DMR_Transaction where CusID='"+CusID+"'"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        dt.TableName = "DMR_Transaction";
                        sda.Fill(dt);
                        var json = JsonConvert.SerializeObject(dt);
                        //return dt;

                        dynamic jsonResponse = JsonConvert.DeserializeObject(json);

                        JArray jarray = new JArray(jsonResponse);

                        JObject jd = new JObject(
                        new JProperty("result", "true"),
                        new JProperty("message", jarray));

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


}
