using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for Connection
/// </summary
/// >
/// 

public class Connectioncls
{

    //private ClsDataAccess objClsDataccess = new ClsDataAccess(@"Server=SYS04;Database=zondaDaily;integrated security=true");

     //public DataSet Countrow;
     // public SqlDataAdapter adpt;


    public SqlConnection connc = new SqlConnection(ConfigurationManager.AppSettings["conc"]);
 //   public SqlCommand cmd_connc = new SqlCommand();[spUpdate_BIXC_Entry_New_Success_Pending]
    public SqlDataReader cmd_reader;

    //private ClsDataAccess objClsDataccess = new ClsDataAccess(@"Server=SYS04;Database=zondaDaily;integrated security=true");

    private dal objClsDataccess = new dal(ConfigurationManager.ConnectionStrings["myConnectionString"].ToString());
    private SqlDataAdapter adpt;
    private DataSet Countrow;

    //   private ClsDataAccess objClsDataccess = new ClsDataAccess(ConfigurationManager.AppSettings[0].ToString());

    public SqlDataReader GetdataReader(string strSqlSelectCommand)
    {

        SqlDataReader sdr = objClsDataccess.GetSqlDataReader(strSqlSelectCommand);
        return sdr;
    }

    public DataTable getdataSet(string sqlSelectCommand, string tableName)
    {
        return objClsDataccess.GetDataSet(sqlSelectCommand, tableName);
    }
    public void RowCount(string str)
    {
        try
        {
            SqlConnection.ClearAllPools();
            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings[0].ToString());
            SqlCommand cmd = new SqlCommand(str, conn);

            cmd.CommandTimeout = 1000;
            conn.Open();
            adpt = new SqlDataAdapter(cmd);
            Countrow = new DataSet();
            adpt.Fill(Countrow);


            conn.Close();

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    public int ExecuteSqlnonQuery(string strSqldmlCommand)
    {
        return objClsDataccess.ExecuteDML(strSqldmlCommand);
    }


    #region APKVersionCheck

    public int APKVersionCheck(string version, out string NewID, out string ErrCode)
    {
        return objClsDataccess.APKVersionCheck(version, out NewID, out ErrCode);
    }

    #endregion

    #region notification

    public int notification(string DeviceID, string RegisterDeviceID, out string NewID, out string ErrCode)
    {
        return objClsDataccess.notification(DeviceID, RegisterDeviceID, out NewID, out ErrCode);
    }

    #endregion


    #region loginuser
    public int UserLogin(string Email, string Cust_Password, string tokenno, out string message, out string ErrCode)
    {
        return objClsDataccess.UserLogin( Email,  Cust_Password,  tokenno, out  message, out  ErrCode);
    }


    #endregion

    #region wallet_services

    public int wallet_services(string CusID, string ewpws, string tokenno, string servicecall, string marketid, string amount, string description, out string flag, out string message1, out string ErrCode1)
    {
        return objClsDataccess.wallet_services(CusID, ewpws, tokenno, servicecall, marketid, amount, description, out flag, out message1, out ErrCode1);
    }

    #endregion


    #region wallet_login

    public int wallet_login(string CusID, string password, string tokenno, string servicecall, out string flag, out string message1, out string ErrCode1)
    {
        return objClsDataccess.wallet_login(CusID, password, tokenno, servicecall, out flag, out message1, out ErrCode1);
    }

    #endregion


    #region loginauthenticate

    public int loginauthenticate(string CusID, string password, string tokenno, string servicecall, out string flag, out string message1, out string ErrCode1)
    {
        return objClsDataccess.loginauthenticate(CusID, password, tokenno, servicecall, out flag, out message1, out ErrCode1);
    }

    #endregion


    #region loginauthenticateauto

    public int loginauthenticateauto(string mobileid, string IPaddress, string tokenno, string servicecall, out string flag, out string message1, out string ErrCode1)
    {
        return objClsDataccess.loginauthenticateauto(mobileid, IPaddress, tokenno, servicecall, out flag, out message1, out ErrCode1);
    }

    #endregion


    #region tokencheck

    public int tokencheck(string CusID, string tokenno, string servicecall, out string flag, out string message1, out string ErrCode1)
    {
        return objClsDataccess.tokencheck(CusID, tokenno, servicecall, out flag, out message1, out ErrCode1);
    }

    #endregion


    #region Walletbalance

    public int Walletbalance(string CusID, string amount, string balance, out string message1, out string ErrCode1)
    {
        return objClsDataccess.Walletbalance(CusID, amount, balance, out message1, out ErrCode1);
    }

    #endregion

    #region PinGenerate

    public int PinGenerate(string Cust_id, string pin, out string NewID, out string ErrCode)
    {
        return objClsDataccess.PinGenerate(Cust_id, pin, out NewID, out ErrCode);
    }

    #endregion



    #region mobile_pin_Verification

    public int mobile_pin_Verification(string Cust_id, string pin, out string NewID, out string ErrCode)
    {
        return objClsDataccess.mobile_pin_Verification(Cust_id, pin, out NewID, out ErrCode);
    }

    #endregion

    #region Forget_mobile_pin

    public int Forget_mobile_pin(string Email, out string NewID, out string ErrCode)
    {
        return objClsDataccess.Forget_mobile_pin(Email, out NewID, out ErrCode);
    }

    #endregion

    #region otp_manage

    public int otp_manage(string CusID, string OTP, out string ErrCode1)
    {
        return objClsDataccess.otp_manage(CusID, OTP, out ErrCode1);
    }

    #endregion


    #region otpverification

    public int otpverification(string CusID, string otp, string device_id, string remark, out string NewID, out string ErrCode)
    {
        return objClsDataccess.otpverification(CusID, otp, device_id, remark, out NewID, out ErrCode);
    }

    #endregion

    #region ReEnter_mobile_pin

    public int ReEnter_mobile_pin(string CusID, string pin, out string NewID, out string ErrCode)
    {
        return objClsDataccess.ReEnter_mobile_pin(CusID, pin, out NewID, out ErrCode);
    }

    #endregion

    #region mobilerechargepre

    public int mobilerechargepre(string CusID, string mobile_no, string operator_name, string circle_id, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        return objClsDataccess.mobilerechargepre(CusID, mobile_no, operator_name, circle_id, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
    }

    #endregion


    #region transactionfailpostmobile

    public int transactionfailpostmobile(string CusID, string mobile_no, string operator_name, string circle_id, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        return objClsDataccess.transactionfailpostmobile(CusID, mobile_no, operator_name, circle_id, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
    }

    #endregion

    #region mobilerechargepost

    public int mobilerechargepost(string CusID, string mobile_no, string operator_name, string circle_id, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        return objClsDataccess.mobilerechargepost(CusID, mobile_no, operator_name, circle_id, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
    }

    #endregion

    #region data_card_api

    public int data_card_api(string CusID, string mobile_no, string operator_name, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        return objClsDataccess.data_card_api(CusID, mobile_no, operator_name, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
    }

    #endregion

    #region transactionfailpdth

    public int transactionfailpdth(string CusID, string mobile_no, string operator_name, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        return objClsDataccess.transactionfailpdth(CusID, mobile_no, operator_name, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
    }

    #endregion

    #region DTH_recharge_api

    public int DTH_recharge_api(string CusID, string mobile_no, string operator_name, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        return objClsDataccess.DTH_recharge_api(CusID, mobile_no, operator_name, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
    }

    #endregion

    #region transactionfailpElectricity

    public int transactionfailpElectricity(string CusID, string mobile_no, string operator_name, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        return objClsDataccess.transactionfailpElectricity(CusID, mobile_no, operator_name, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
    }

    #endregion

    #region Electricity_api

    public int Electricity_api(string CusID, string mobile_no, string operator_name, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        return objClsDataccess.Electricity_api(CusID, mobile_no, operator_name, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
    }

    #endregion

    #region Gas_api

    public int Gas_api(string CusID, string mobile_no, string operator_name, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        return objClsDataccess.Gas_api(CusID, mobile_no, operator_name, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
    }

    #endregion

    #region Insurance_api

    public int Insurance_api(string CusID, string mobile_no, string operator_name, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        return objClsDataccess.Insurance_api(CusID, mobile_no, operator_name, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
    }

    #endregion

    #region transactionfailpremobile

    public int transactionfailpremobile(string CusID, string mobile_no, string operator_name, string circle_id, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        return objClsDataccess.transactionfailpremobile(CusID, mobile_no, operator_name, circle_id, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
    }

    #endregion

    #region transactionfailpgas

    public int transactionfailpgas(string CusID, string mobile_no, string operator_name, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        return objClsDataccess.transactionfailpgas(CusID, mobile_no, operator_name, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
    }

    #endregion

    #region transactionfailInsurance

    public int transactionfailInsurance(string CusID, string mobile_no, string operator_name, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        return objClsDataccess.transactionfailInsurance(CusID, mobile_no, operator_name, your_cost, amount2, balance, txid, user_txid, status, number, operator_ref, error_code, message, time, out trID1, out status1, out ErrCode);
    }

    #endregion

    #region RechargeStatus

    public int RechargeStatus(string CusID, string STATUS, string MOBILE, string AMOUNT, string RPID, string AGENTID, string OPID, string BAL, string MSG, out string err_code, out string NewID)
    {
        return objClsDataccess.RechargeStatus(CusID, STATUS, MOBILE, AMOUNT, RPID, AGENTID, OPID, BAL, MSG, out err_code, out NewID);
    }

    #endregion

    #region checklimit

    public int checklimit(string CusID, string Amount, out string flag, out string errMsg)
    {
        return objClsDataccess.checklimit(CusID, Amount, out flag, out errMsg);
    }

    #endregion

    #region DMRTransactionFail

    public int DMRTransactionFail(string CusID, string SenderMobileNo, string BankAccount, string Amount, string Recipientid, string Channel, string IMEI, string MESSAGE, out string NewID, out string ErrCode)
    {
        return objClsDataccess.DMRTransactionFail(CusID, SenderMobileNo, BankAccount, Amount, Recipientid, Channel, IMEI, MESSAGE, out NewID, out ErrCode);
    }

    #endregion

    #region DMRTransaction

    public int DMRTransaction(string CusID, string SenderMobileNo, string BankAccount, string Amount, string Recipientid, string Channel, string IMEI, string MESSAGE, out string NewID, out string ErrCode)
    {
        return objClsDataccess.DMRTransaction(CusID, SenderMobileNo, BankAccount, Amount, Recipientid, Channel, IMEI, MESSAGE, out NewID, out ErrCode);
    }

    #endregion

    #region userRegistration

    public int UserRegistration(int serialno, int CusID, float Cust_SponserID, string Cust_Location, DateTime Entry_Date, string Cust_Name, string Email, string Cust_Country, Int64 Cust_mobileNo, string Cust_Password, string Cust_Package, int Approved, int Pin_Approved, int plannerId, int totaldir, string tokenno)
    {
        return objClsDataccess.userRegistration(serialno, CusID, Cust_SponserID, Cust_Location, Entry_Date, Cust_Name, Email, Cust_Country, Cust_mobileNo, Cust_Password, Cust_Package, Approved, Pin_Approved, plannerId, totaldir, tokenno);

    }
    #endregion

}