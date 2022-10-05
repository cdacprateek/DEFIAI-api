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
/// Summary description for Utility
/// </summary>
public class Utility : System.Web.UI.Page
{
    public Utility()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region RegistrastionOTP

    public void SendSMS(string Newid, string name, string email, string _custusername, string _rotp)
    {
        try
        {
            WebClient client = new WebClient();
            DataSet ds = new DataSet();
            string body1 = this.PopulateBody1(Newid, name, email, _custusername, _rotp);
            var fromAddress1 = ConfigurationManager.AppSettings["smtpsenderEmail"].ToString();
            var toAddress = email;
            string fromPassword = ConfigurationManager.AppSettings["smtpPassword"].ToString();
            string subject = "Email Verification... DEFI-AI";
            MailMessage mailMsg = new MailMessage();
            // To
            mailMsg.To.Add(new MailAddress(email));
            // From
            mailMsg.From = new MailAddress(ConfigurationManager.AppSettings["smtpsenderEmail"].ToString(), "DEFI-AI ");
            // Subject and multipart/alternative Body
            mailMsg.Subject = subject;
            string text = "text body";
            string html = body1;
            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
            // Init SmtpClient and send
            SmtpClient smtpClient = new SmtpClient(ConfigurationManager.AppSettings["smtpHost"].ToString(), Convert.ToInt32(ConfigurationManager.AppSettings["smtpport"].ToString()));
            smtpClient.EnableSsl = true;
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtpUsername"].ToString(), ConfigurationManager.AppSettings["smtpPassword"].ToString());
            smtpClient.Credentials = credentials;
            smtpClient.Send(mailMsg);
        }
        catch (Exception ex)
        {

        }

    }

    private string PopulateBody1(string Newid, string name, string email, string _custusername, string _rotp)
    {
        string body = string.Empty;
        using (StreamReader reader = new StreamReader(Server.MapPath("~/HTMLPage/email_verify.htm")))
        {
            body = reader.ReadToEnd();
        }
        string _dateti = DateTime.Now.ToString();
        body = body.Replace("{date}", _dateti);
        body = body.Replace("{name}", name);
        body = body.Replace("{email}", email);
        body = body.Replace("{otp}", Newid);
        // Creating the query string to pass
        body = body.Replace("{rotp}", _rotp);
        string name111 = Newid;
        body = body.Replace("{encoderegno}", name111);
        return body;
    }

    #endregion

    #region RegistrastionWelcome

    public void WelcomeSendSMS(string id, string name, string email, string pass, string txpws)
    {
        try
        {
            string body = this.WelcomeSendSMSPopulateBody(id, name, email, pass, txpws);
            // DateTime letterdt = Connectioncls.getIndianDateTime();
            //string dat = letterdt.ToShortDateString();
            // Gmail Address from where you send the mail
            var fromAddress = ConfigurationManager.AppSettings["smtpsenderEmail"].ToString();
            // any address where the email will be sending
            var toAddress = email;
            //Password of your gmail address
            string fromPassword = ConfigurationManager.AppSettings["smtpPassword"].ToString();
            // Passing the values and make a email formate to display
            //string subject = "app.bitglobalcoin.net Welcome to website...!";
            string subject = "Registration defiai.io";
            // body = bodyy; 
            MailMessage mailMsg = new MailMessage();
            // To
            mailMsg.To.Add(new MailAddress(email));
            // From
            //mailMsg.From = new MailAddress("alert@app.bitglobalcoin.net", " Registration  Detail");
            mailMsg.From = new MailAddress(ConfigurationManager.AppSettings["smtpsenderEmail"].ToString(), "DEFI-AI");
            // Subject and multipart/alternative Body
            mailMsg.Subject = subject;
            string text = "text body";
            string html = body;
            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
            // Init SmtpClient and send
            SmtpClient smtpClient = new SmtpClient(ConfigurationManager.AppSettings["smtpHost"].ToString(), Convert.ToInt32(ConfigurationManager.AppSettings["smtpport"].ToString()));
            smtpClient.EnableSsl = true;
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtpUsername"].ToString(), ConfigurationManager.AppSettings["smtpPassword"].ToString());
            smtpClient.Credentials = credentials;
            smtpClient.Send(mailMsg);
        }
        catch (Exception ex)
        {

        }
    }

    private string WelcomeSendSMSPopulateBody(string id, string name, string email, string pass, string txpws)
    {
        string body = string.Empty;
        using (StreamReader reader = new StreamReader(Server.MapPath("~/HTMLPage/welcome.html")))
        {
            body = reader.ReadToEnd();
        }
        string _dateti = DateTime.Now.ToString();
        body = body.Replace("{idno}", id);
        body = body.Replace("{Password}", pass);
        body = body.Replace("{txpws}", txpws);
        return body;
    }

    #endregion

}