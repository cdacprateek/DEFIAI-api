using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for dal
/// </summary>
/// 


public class dal
{
    #region Public Constructor , takes one argument as sqlconnection string

    /// <summary>
    /// Public Constructor , takes one argument as sqlconnection string
    /// </summary>
    /// <param name="SQLconnectionString">SQLconnectionString</param>
    /// 
    public string ConnectionString1 = "";
    private string NewID;
    private string message;
    private string ErrCode;
    private readonly int value;
 
    private readonly object tokenno;
    private readonly object CusID;

    public dal(string SQLconnectionString)
    {
        ConnectionString1 = SQLconnectionString;
    }
    #endregion

    #region GetSqlDataReader
    public SqlDataReader GetSqlDataReader(string SqlSelectCommand)
    {
        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand(SqlSelectCommand, conc);
            if (conc.State == ConnectionState.Closed)
                conc.Open();
            SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return sdr;
        }
        catch (SqlException ex)
        {
            conc.Close();
            return null;
        }
    }
    #endregion

    #region GetDataSet
    public DataTable GetDataSet(string SqlSelectCommamnd, string dataSettableName)
    {
        SqlConnection conc = new SqlConnection(ConnectionString1);
        SqlDataAdapter adp = new SqlDataAdapter(SqlSelectCommamnd, conc);
        try
        {
            if (conc.State == ConnectionState.Closed)
                conc.Open();
            DataTable dt = new DataTable(dataSettableName);
            adp.Fill(dt);
            adp.Dispose();
            return dt;
        }
        catch
        {
            return null;
        }
        finally
        {
            adp.Dispose();
            conc.Close();
        }
    }
    #endregion

    #region Executes Non-Query Statements

    public int ExecuteDML(string SqlDMLcmd)
    {
        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            int returnval = 0;
            SqlCommand cmd = new SqlCommand(SqlDMLcmd, conc);
            if (conc.State == ConnectionState.Closed)
                conc.Open();
            returnval = cmd.ExecuteNonQuery();
            cmd.Dispose();
            conc.Close();
            return returnval;
        }
        catch (SqlException ex)
        {
            return ex.ErrorCode;
        }
        finally
        {
            conc.Close();
        }
    }
    #endregion

    #region "Data Table"
    /// <summary>
    /// Get datatable
    /// </summary>
    /// <param name="SqlSelectCommamnd">Sql Select Command</param>
    /// <param name="dataSettableName">DataTable name</param>
    /// <returns></returns>
    #endregion
   

    #region APKVersionCheck

    public int APKVersionCheck(string version, out string NewID, out string ErrCode)
    {
        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd1 = new SqlCommand("APKVersionCheck", conc);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.Add(new SqlParameter("@version", version));

            SqlParameter SqlParaNewID = new SqlParameter("@NewID", SqlDbType.VarChar, 500);
            SqlParaNewID.Direction = ParameterDirection.Output;
            cmd1.Parameters.Add(SqlParaNewID).Value = "";

            SqlParameter SqlParaErrMsg = new SqlParameter("@ErrCode", SqlDbType.VarChar, 500);
            SqlParaErrMsg.Direction = ParameterDirection.Output;
            cmd1.Parameters.Add(SqlParaErrMsg).Value = "TRY";

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd1.ExecuteNonQuery();
            NewID = (string)SqlParaNewID.Value;
            ErrCode = (string)SqlParaErrMsg.Value;
            return value;

        }
        catch (Exception ex)
        {
            NewID = "";
            ErrCode = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }
    }

    #endregion

    #region notification

    public int notification(string DeviceID, string RegisterDeviceID, out string NewID, out string ErrCode)
    {
        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand("DeviceNotification", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DeviceID", DeviceID);
            cmd.Parameters.AddWithValue("@RegisterDeviceID", RegisterDeviceID);

            SqlParameter SqlParaNewID = new SqlParameter("@NewID", SqlDbType.VarChar, 500);
            SqlParaNewID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaNewID).Value = "";

            SqlParameter SqlParaErrMsg = new SqlParameter("@ErrCode", SqlDbType.VarChar, 500);
            SqlParaErrMsg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrMsg).Value = "";

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            NewID = (string)SqlParaNewID.Value;
            ErrCode = (string)SqlParaErrMsg.Value;
            return value;

        }
        catch (Exception ex)
        {
            NewID = "0";
            ErrCode = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion



    #region UserLogin


    public int UserLogin(string Email, string Cust_Password, string tokenno, out string message, out string ErrCode)
    {
      
        SqlConnection conc = new SqlConnection(ConnectionString1);
       
        try
        {   
            SqlCommand cmd = new SqlCommand("api_login", conc);
            //            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "api_login";
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Cust_Password", Cust_Password);
            cmd.Parameters.AddWithValue("@tokenno", tokenno);
            
            // conc.Open();

            SqlParameter SqlParaErrCode = new SqlParameter("@message", SqlDbType.VarChar, 500);
            SqlParaErrCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode).Value = 0;

            SqlParameter SqlParaErrCode1 = new SqlParameter("@ErrCode", SqlDbType.VarChar, 500);
            SqlParaErrCode1.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode1).Value = 0;


            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            message = (string)SqlParaErrCode.Value;
            ErrCode = (string)SqlParaErrCode1.Value;
           

            return value;

        }
        catch (Exception ex)
        {
            message = "0";
            ErrCode = ex.Message;
          
            return 0;
        }
        finally
        {
            conc.Close();
        }


    }

    #endregion


    #region Walletbalance

    public int Walletbalance(string CusID, string amount2, string balance, out string message1, out string ErrCode1)
    {
        float amount1 = 0;
        float balance1 = 0;
        amount1 = float.Parse(amount2.ToString());
        balance1 = float.Parse(balance.ToString());

        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand("recharge_api", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "recharge_api";
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@balance1", amount1);
            // conc.Open();

            SqlParameter SqlParaErrCode = new SqlParameter("@message1", SqlDbType.VarChar, 500);
            SqlParaErrCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode).Value = 0;

            SqlParameter SqlParaErrCode1 = new SqlParameter("@ErrCode1", SqlDbType.VarChar, 500);
            SqlParaErrCode1.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode1).Value = 0;

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            message1 = (string)SqlParaErrCode.Value;
            ErrCode1 = (string)SqlParaErrCode1.Value;
            return value;




        }
        catch (Exception ex)
        {
            message1 = "0";
            ErrCode1 = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion


    #region loginauthenticate

    public int loginauthenticate(string CusID, string password, string tokenno, string servicecall, out string errflag, out string message1, out string ErrCode1)
    {
        //float amount1 = 0;
        //float balance1 = 0;
        //amount1 = float.Parse(amount2.ToString());
        //balance1 = float.Parse(balance.ToString());

        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand("api_login", conc);
//            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "api_login";
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@tokenno", tokenno);
            cmd.Parameters.AddWithValue("@servicecall", servicecall);
            // conc.Open();

            SqlParameter SqlParaErrCode = new SqlParameter("@message1", SqlDbType.VarChar, 500);
            SqlParaErrCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode).Value = 0;

            SqlParameter SqlParaErrCode1 = new SqlParameter("@ErrCode1", SqlDbType.VarChar, 500);
            SqlParaErrCode1.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode1).Value = 0;

            SqlParameter SqlParaflag = new SqlParameter("@flag", SqlDbType.VarChar, 500);
            SqlParaflag.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaflag).Value = 0;

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            message1 = (string)SqlParaErrCode.Value;
            ErrCode1 = (string)SqlParaErrCode1.Value;
            errflag = (string)SqlParaflag.Value;

            return value;

        }
        catch (Exception ex)
        {
            message1 = "0";
            ErrCode1 = ex.Message;
            errflag = "0";
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion


    #region loginauthenticateauto

    public int loginauthenticateauto(string mobileid, string IPaddress, string tokenno, string servicecall, out string errflag, out string message1, out string ErrCode1)
    {
        //float amount1 = 0;
        //float balance1 = 0;
        //amount1 = float.Parse(amount2.ToString());
        //balance1 = float.Parse(balance.ToString());

        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand("api_login", conc);
//            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "api_autologin";
            cmd.Parameters.AddWithValue("@CusID", mobileid);
            cmd.Parameters.AddWithValue("@password", IPaddress);
            cmd.Parameters.AddWithValue("@tokenno", tokenno);
            cmd.Parameters.AddWithValue("@servicecall", servicecall);
            // conc.Open();

            SqlParameter SqlParaErrCode = new SqlParameter("@message1", SqlDbType.VarChar, 500);
            SqlParaErrCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode).Value = 0;

            SqlParameter SqlParaErrCode1 = new SqlParameter("@ErrCode1", SqlDbType.VarChar, 500);
            SqlParaErrCode1.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode1).Value = 0;

            SqlParameter SqlParaflag = new SqlParameter("@flag", SqlDbType.VarChar, 500);
            SqlParaflag.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaflag).Value = 0;

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            message1 = (string)SqlParaErrCode.Value;
            ErrCode1 = (string)SqlParaErrCode1.Value;
            errflag = (string)SqlParaflag.Value;

            return value;

        }
        catch (Exception ex)
        {
            message1 = "0";
            ErrCode1 = ex.Message;
            errflag = "0";
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion


    #region wallet_login

    public int wallet_login(string CusID, string password , string tokenno, string servicecall, out string errflag, out string message1, out string ErrCode1)
    {
        //float amount1 = 0;
        //float balance1 = 0;
        //amount1 = float.Parse(amount2.ToString());
        //balance1 = float.Parse(balance.ToString());

        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand("api_login", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "api_login";
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@tokenno", tokenno);
            cmd.Parameters.AddWithValue("@servicecall", servicecall);
            // conc.Open();

            SqlParameter SqlParaErrCode = new SqlParameter("@message1", SqlDbType.VarChar, 500);
            SqlParaErrCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode).Value = 0;

            SqlParameter SqlParaErrCode1 = new SqlParameter("@ErrCode1", SqlDbType.VarChar, 500);
            SqlParaErrCode1.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode1).Value = 0;

            SqlParameter SqlParaflag = new SqlParameter("@flag", SqlDbType.VarChar, 500);
            SqlParaflag.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaflag).Value = 0;

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            message1 = (string)SqlParaErrCode.Value;
            ErrCode1 = (string)SqlParaErrCode1.Value;
            errflag = (string)SqlParaflag.Value;

            return value;

        }
        catch (Exception ex)
        {
            message1 = "0";
            ErrCode1 = ex.Message;
            errflag = "0";
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion


    #region tokencheck

    public int tokencheck(string CusID, string tokenno, string servicecall, out string errflag, out string message1, out string ErrCode1)
    {
        //float amount1 = 0;
        //float balance1 = 0;
        //amount1 = float.Parse(amount2.ToString());
        //balance1 = float.Parse(balance.ToString());

        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand("tokencheck", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "tokencheck";
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@tokenno", tokenno);
            cmd.Parameters.AddWithValue("@servicecall", servicecall);
            // conc.Open();

            SqlParameter SqlParaErrCode = new SqlParameter("@message1", SqlDbType.VarChar, 500);
            SqlParaErrCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode).Value = 0;

            SqlParameter SqlParaErrCode1 = new SqlParameter("@ErrCode1", SqlDbType.VarChar, 500);
            SqlParaErrCode1.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode1).Value = 0;

            SqlParameter SqlParaflag = new SqlParameter("@flag", SqlDbType.VarChar, 500);
            SqlParaflag.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaflag).Value = 0;

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            message1 = (string)SqlParaErrCode.Value;
            ErrCode1 = (string)SqlParaErrCode1.Value;
            errflag = (string)SqlParaflag.Value;

            return value; 
             
        }
        catch (Exception ex)
        {
            message1 = "0";
            ErrCode1 = ex.Message;
            errflag = "0";
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion

 

    #region wallet_services

    public int wallet_services(string CusID, string ewpws, string tokenno, string servicecall, string marketid, string amount, string description, out string errflag, out string message1, out string ErrCode1)
    {
        //float amount1 = 0;
        //float balance1 = 0;
        //amount1 = float.Parse(amount2.ToString());
        //balance1 = float.Parse(balance.ToString());

        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand("api_wallet_services", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "api_wallet_services";
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@ewpws", ewpws);
            cmd.Parameters.AddWithValue("@tokenno", tokenno);
            cmd.Parameters.AddWithValue("@servicecall", servicecall);
            cmd.Parameters.AddWithValue("@marketid", marketid);
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.Parameters.AddWithValue("@description", description);
            
            // conc.Open();

            SqlParameter SqlParaErrCode = new SqlParameter("@message1", SqlDbType.VarChar, 500);
            SqlParaErrCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode).Value = 0;

            SqlParameter SqlParaErrCode1 = new SqlParameter("@ErrCode1", SqlDbType.VarChar, 500);
            SqlParaErrCode1.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode1).Value = 0;

            SqlParameter SqlParaflag = new SqlParameter("@flag", SqlDbType.VarChar, 500);
            SqlParaflag.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaflag).Value = 0;

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            message1 = (string)SqlParaErrCode.Value;
            ErrCode1 = (string)SqlParaErrCode1.Value;
            errflag = (string)SqlParaflag.Value;

            return value;

        }
        catch (Exception ex)
        {
            message1 = "0";
            ErrCode1 = ex.Message;
            errflag = "0";
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion



    #region PinGenerate

    public int PinGenerate(string Cust_id, string pin, out string NewID, out string ErrCode)
    {
        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {

            SqlCommand cmd = new SqlCommand("PinGenerate", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PinGenerate";
            cmd.Parameters.AddWithValue("@Cust_id", Cust_id);
            cmd.Parameters.AddWithValue("@pin", pin);
            cmd.Connection = conc;


            SqlParameter SqlParaNewID = new SqlParameter("@NewID", SqlDbType.VarChar, 500);
            SqlParaNewID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaNewID).Value = "";

            SqlParameter SqlParaErrCode = new SqlParameter("@ErrCode", SqlDbType.VarChar, 500);
            SqlParaErrCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode).Value = "";

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            NewID = (string)SqlParaNewID.Value;
            ErrCode = (string)SqlParaErrCode.Value;
            return value;




        }
        catch (Exception ex)
        {
            NewID = "0";
            ErrCode = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion


    #region mobile_pin_Verification

    public int mobile_pin_Verification(string Cust_id, string pin, out string NewID, out string ErrCode)
    {
        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand("mobile_pin_Verification", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Cust_id", Cust_id);
            cmd.Parameters.AddWithValue("@pin", pin);

            SqlParameter SqlParaNewID = new SqlParameter("@NewID", SqlDbType.VarChar, 500);
            SqlParaNewID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaNewID).Value = "";

            SqlParameter SqlParaErrMsg = new SqlParameter("@ErrCode", SqlDbType.VarChar, 500);
            SqlParaErrMsg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrMsg).Value = "";

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            NewID = (string)SqlParaNewID.Value;
            ErrCode = (string)SqlParaErrMsg.Value;
            return value;

        }
        catch (Exception ex)
        {
            NewID = "0";
            ErrCode = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion


    #region Forget_mobile_pin

    public int Forget_mobile_pin(string Email, out string NewID, out string ErrCode)
    {
        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand("Forget_mobile_pin", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", Email);


            SqlParameter SqlParaNewID = new SqlParameter("@NewID", SqlDbType.VarChar, 500);
            SqlParaNewID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaNewID).Value = "";

            SqlParameter SqlParaErrMsg = new SqlParameter("@ErrCode", SqlDbType.VarChar, 500);
            SqlParaErrMsg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrMsg).Value = "";

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            NewID = (string)SqlParaNewID.Value;
            ErrCode = (string)SqlParaErrMsg.Value;
            return value;

        }
        catch (Exception ex)
        {
            NewID = "0";
            ErrCode = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion


    #region otp_manage

    public int otp_manage(string CusID, string OTP, out string ErrCode1)
    {
        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand("OTP_Inset_manage", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "OTP_Inset_manage";
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@OTP", OTP);

            SqlParameter SqlParaErrCode = new SqlParameter("@ErrCode1", SqlDbType.VarChar, 500);
            SqlParaErrCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode).Value = "";

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();

            // NewIDSub = value;
            ErrCode1 = (string)SqlParaErrCode.Value;
            return value;

        }
        catch (Exception ex)
        {
            ErrCode1 = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion



    #region otpverification

    public int otpverification(string CusID, string otp, string device_id, string remark, out string NewID, out string ErrCode)
    {
        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand("otpverify", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "otpverify";
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@otp", otp);
            cmd.Parameters.AddWithValue("@device_id", device_id);
            cmd.Parameters.AddWithValue("@remark", remark);
            cmd.Parameters.AddWithValue("@status", 1);
            cmd.Connection = conc;


            SqlParameter SqlParaNewID = new SqlParameter("@NewID", SqlDbType.VarChar, 500);
            SqlParaNewID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaNewID).Value = 0;

            SqlParameter SqlParaErrCode = new SqlParameter("@ErrCode", SqlDbType.VarChar, 500);
            SqlParaErrCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode).Value = "";

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            NewID = (string)SqlParaNewID.Value;
            ErrCode = (string)SqlParaErrCode.Value;
            return value;

        }
        catch (Exception ex)
        {
            NewID = "0";
            ErrCode = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion



    #region ReEnter_mobile_pin

    public int ReEnter_mobile_pin(string CusID, string pin, out string NewID, out string ErrCode)
    {

        float CustID = 0;
        CustID = float.Parse(CusID.ToString());

        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand("ReEnter_mobile_pin", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@pin", pin);

            SqlParameter SqlParaNewID = new SqlParameter("@NewID", SqlDbType.VarChar, 500);
            SqlParaNewID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaNewID).Value = "";

            SqlParameter SqlParaErrMsg = new SqlParameter("@ErrCode", SqlDbType.VarChar, 500);
            SqlParaErrMsg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrMsg).Value = "";

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            NewID = (string)SqlParaNewID.Value;
            ErrCode = (string)SqlParaErrMsg.Value;
            return value;

        }
        catch (Exception ex)
        {
            NewID = "0";
            ErrCode = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion

    #region mobilerechargepre

    public int mobilerechargepre(string CusID, string mobile_no, string operator_name, string circle_id, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        float amount1 = 0;
        float balance1 = 0;
        amount1 = float.Parse(amount2.ToString());
        balance1 = float.Parse(balance.ToString());

        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand("mobile_recharge_pre_api", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "mobile_recharge_pre_api";
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@mobile_no", mobile_no);
            cmd.Parameters.AddWithValue("@operator_name", operator_name);
            cmd.Parameters.AddWithValue("@circle_id", circle_id);
            cmd.Parameters.AddWithValue("@your_cost", your_cost);
            cmd.Parameters.AddWithValue("@amount2", amount1);
            cmd.Parameters.AddWithValue("@balance", balance1);
            cmd.Parameters.AddWithValue("@balance1", balance);
            cmd.Parameters.AddWithValue("@txid", txid);
            cmd.Parameters.AddWithValue("@user_txid", user_txid);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@number", number);
            cmd.Parameters.AddWithValue("@operator_ref", operator_ref);
            cmd.Parameters.AddWithValue("@error_code", error_code);
            cmd.Parameters.AddWithValue("@message", message);
            cmd.Parameters.AddWithValue("@time", time);

            cmd.Connection = conc;


            SqlParameter SqltrID = new SqlParameter("@trID1", SqlDbType.VarChar, 500);
            SqltrID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqltrID).Value = "";

            SqlParameter SqlParaNewID = new SqlParameter("@status1", SqlDbType.VarChar, 500);
            SqlParaNewID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaNewID).Value = "";

            SqlParameter SqlParaErrCode = new SqlParameter("@ErrCode", SqlDbType.VarChar, 500);
            SqlParaErrCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode).Value = "";

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            status1 = (string)SqlParaNewID.Value;
            ErrCode = (string)SqlParaErrCode.Value;
            trID1 = (string)SqltrID.Value;
            return value;




        }
        catch (Exception ex)
        {

            trID1 = "0";
            ErrCode = "";
            status1 = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion




    #region transactionfailpostmobile

    public int transactionfailpostmobile(string CusID, string mobile_no, string operator_name, string circle_id, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        float amount1 = 0;
        float balance1 = 0;
        amount1 = float.Parse(amount2.ToString());
        balance1 = float.Parse(balance.ToString());

        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand("transactionfailpostmobile", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "transactionfailpostmobile";
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@mobile_no", mobile_no);
            cmd.Parameters.AddWithValue("@operator_name", operator_name);
            cmd.Parameters.AddWithValue("@circle_id", circle_id);
            cmd.Parameters.AddWithValue("@your_cost", your_cost);
            cmd.Parameters.AddWithValue("@amount2", amount1);
            cmd.Parameters.AddWithValue("@balance", balance1);
            cmd.Parameters.AddWithValue("@balance1", balance);
            cmd.Parameters.AddWithValue("@txid", txid);
            cmd.Parameters.AddWithValue("@user_txid", user_txid);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@number", number);
            cmd.Parameters.AddWithValue("@operator_ref", operator_ref);
            cmd.Parameters.AddWithValue("@error_code", error_code);
            cmd.Parameters.AddWithValue("@message", message);
            cmd.Parameters.AddWithValue("@time", time);

            cmd.Connection = conc;


            SqlParameter SqltrID = new SqlParameter("@trID1", SqlDbType.VarChar, 500);
            SqltrID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqltrID).Value = "";

            SqlParameter SqlParaNewID = new SqlParameter("@status1", SqlDbType.VarChar, 500);
            SqlParaNewID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaNewID).Value = "";

            SqlParameter SqlParaErrCode = new SqlParameter("@ErrCode", SqlDbType.VarChar, 500);
            SqlParaErrCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode).Value = "";

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            status1 = (string)SqlParaNewID.Value;
            ErrCode = (string)SqlParaErrCode.Value;
            trID1 = (string)SqltrID.Value;
            return value;




        }
        catch (Exception ex)
        {

            trID1 = "0";
            ErrCode = "";
            status1 = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion


    #region mobilerechargepost

    public int mobilerechargepost(string CusID, string mobile_no, string operator_name, string circle_id, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        float amount1 = 0;
        float balance1 = 0;
        amount1 = float.Parse(amount2.ToString());
        balance1 = float.Parse(balance.ToString());

        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand("mobile_recharge_post_api", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "mobile_recharge_post_api";
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@mobile_no", mobile_no);
            cmd.Parameters.AddWithValue("@operator_name", operator_name);
            cmd.Parameters.AddWithValue("@circle_id", circle_id);
            cmd.Parameters.AddWithValue("@your_cost", your_cost);
            cmd.Parameters.AddWithValue("@amount2", amount1);
            cmd.Parameters.AddWithValue("@balance", balance1);
            cmd.Parameters.AddWithValue("@balance1", balance);
            cmd.Parameters.AddWithValue("@txid", txid);
            cmd.Parameters.AddWithValue("@user_txid", user_txid);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@number", number);
            cmd.Parameters.AddWithValue("@operator_ref", operator_ref);
            cmd.Parameters.AddWithValue("@error_code", error_code);
            cmd.Parameters.AddWithValue("@message", message);
            cmd.Parameters.AddWithValue("@time", time);

            cmd.Connection = conc;


            SqlParameter SqltrID = new SqlParameter("@trID1", SqlDbType.VarChar, 500);
            SqltrID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqltrID).Value = "";

            SqlParameter SqlParaNewID = new SqlParameter("@status1", SqlDbType.VarChar, 500);
            SqlParaNewID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaNewID).Value = "";

            SqlParameter SqlParaErrCode = new SqlParameter("@ErrCode", SqlDbType.VarChar, 500);
            SqlParaErrCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode).Value = "";

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            status1 = (string)SqlParaNewID.Value;
            ErrCode = (string)SqlParaErrCode.Value;
            trID1 = (string)SqltrID.Value;
            return value;




        }
        catch (Exception ex)
        {

            trID1 = "0";
            ErrCode = "";
            status1 = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion



    #region data_card_api

    public int data_card_api(string CusID, string mobile_no, string operator_name, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        float amount1 = 0;
        float balance1 = 0;
        amount1 = float.Parse(amount2.ToString());
        balance1 = float.Parse(balance.ToString());

        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand("data_card_api", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "data_card_api";
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@mobile_no", mobile_no);
            cmd.Parameters.AddWithValue("@operator_name", operator_name);
            cmd.Parameters.AddWithValue("@your_cost", your_cost);
            cmd.Parameters.AddWithValue("@amount2", amount1);
            cmd.Parameters.AddWithValue("@balance", balance1);
            cmd.Parameters.AddWithValue("@balance1", balance);
            cmd.Parameters.AddWithValue("@txid", txid);
            cmd.Parameters.AddWithValue("@user_txid", user_txid);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@number", number);
            cmd.Parameters.AddWithValue("@operator_ref", operator_ref);
            cmd.Parameters.AddWithValue("@error_code", error_code);
            cmd.Parameters.AddWithValue("@message", message);
            cmd.Parameters.AddWithValue("@time", time);

            cmd.Connection = conc;


            SqlParameter SqltrID = new SqlParameter("@trID1", SqlDbType.VarChar, 500);
            SqltrID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqltrID).Value = "";

            SqlParameter SqlParaNewID = new SqlParameter("@status1", SqlDbType.VarChar, 500);
            SqlParaNewID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaNewID).Value = "";

            SqlParameter SqlParaErrCode = new SqlParameter("@ErrCode", SqlDbType.VarChar, 500);
            SqlParaErrCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode).Value = "";

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            status1 = (string)SqlParaNewID.Value;
            ErrCode = (string)SqlParaErrCode.Value;
            trID1 = (string)SqltrID.Value;
            return value;




        }
        catch (Exception ex)
        {

            trID1 = "0";
            ErrCode = "";
            status1 = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion


    #region transactionfailpdth

    public int transactionfailpdth(string CusID, string mobile_no, string operator_name, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        float amount1 = 0;
        float balance1 = 0;
        amount1 = float.Parse(amount2.ToString());
        balance1 = float.Parse(balance.ToString());

        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand("transactionfailpdth", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "transactionfailpdth";
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@mobile_no", mobile_no);
            cmd.Parameters.AddWithValue("@operator_name", operator_name);
            cmd.Parameters.AddWithValue("@your_cost", your_cost);
            cmd.Parameters.AddWithValue("@amount2", amount1);
            cmd.Parameters.AddWithValue("@balance", balance1);
            cmd.Parameters.AddWithValue("@balance1", balance);
            cmd.Parameters.AddWithValue("@txid", txid);
            cmd.Parameters.AddWithValue("@user_txid", user_txid);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@number", number);
            cmd.Parameters.AddWithValue("@operator_ref", operator_ref);
            cmd.Parameters.AddWithValue("@error_code", error_code);
            cmd.Parameters.AddWithValue("@message", message);
            cmd.Parameters.AddWithValue("@time", time);

            cmd.Connection = conc;


            SqlParameter SqltrID = new SqlParameter("@trID1", SqlDbType.VarChar, 500);
            SqltrID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqltrID).Value = "";

            SqlParameter SqlParaNewID = new SqlParameter("@status1", SqlDbType.VarChar, 500);
            SqlParaNewID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaNewID).Value = "";

            SqlParameter SqlParaErrCode = new SqlParameter("@ErrCode", SqlDbType.VarChar, 500);
            SqlParaErrCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode).Value = "";

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            status1 = (string)SqlParaNewID.Value;
            ErrCode = (string)SqlParaErrCode.Value;
            trID1 = (string)SqltrID.Value;
            return value;




        }
        catch (Exception ex)
        {

            trID1 = "0";
            ErrCode = "";
            status1 = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion


    #region DTH_recharge_api

    public int DTH_recharge_api(string CusID, string mobile_no, string operator_name, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        float amount1 = 0;
        float balance1 = 0;
        amount1 = float.Parse(amount2.ToString());
        balance1 = float.Parse(balance.ToString());

        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand("DTH_recharge_api", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "DTH_recharge_api";
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@mobile_no", mobile_no);
            cmd.Parameters.AddWithValue("@operator_name", operator_name);
            cmd.Parameters.AddWithValue("@your_cost", your_cost);
            cmd.Parameters.AddWithValue("@amount2", amount1);
            cmd.Parameters.AddWithValue("@balance", balance1);
            cmd.Parameters.AddWithValue("@balance1", balance);
            cmd.Parameters.AddWithValue("@txid", txid);
            cmd.Parameters.AddWithValue("@user_txid", user_txid);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@number", number);
            cmd.Parameters.AddWithValue("@operator_ref", operator_ref);
            cmd.Parameters.AddWithValue("@error_code", error_code);
            cmd.Parameters.AddWithValue("@message", message);
            cmd.Parameters.AddWithValue("@time", time);

            cmd.Connection = conc;


            SqlParameter SqltrID = new SqlParameter("@trID1", SqlDbType.VarChar, 500);
            SqltrID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqltrID).Value = "";

            SqlParameter SqlParaNewID = new SqlParameter("@status1", SqlDbType.VarChar, 500);
            SqlParaNewID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaNewID).Value = "";

            SqlParameter SqlParaErrCode = new SqlParameter("@ErrCode", SqlDbType.VarChar, 500);
            SqlParaErrCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode).Value = "";

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            status1 = (string)SqlParaNewID.Value;
            ErrCode = (string)SqlParaErrCode.Value;
            trID1 = (string)SqltrID.Value;
            return value;




        }
        catch (Exception ex)
        {

            trID1 = "0";
            ErrCode = "";
            status1 = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion


    #region transactionfailpElectricity

    public int transactionfailpElectricity(string CusID, string mobile_no, string operator_name, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        float amount1 = 0;
        float balance1 = 0;
        amount1 = float.Parse(amount2.ToString());
        balance1 = float.Parse(balance.ToString());

        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand("transactionfailpElectricity", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "transactionfailpElectricity";
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@mobile_no", mobile_no);
            cmd.Parameters.AddWithValue("@operator_name", operator_name);
            cmd.Parameters.AddWithValue("@your_cost", your_cost);
            cmd.Parameters.AddWithValue("@amount2", amount1);
            cmd.Parameters.AddWithValue("@balance", balance1);
            cmd.Parameters.AddWithValue("@balance1", balance);
            cmd.Parameters.AddWithValue("@txid", txid);
            cmd.Parameters.AddWithValue("@user_txid", user_txid);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@number", number);
            cmd.Parameters.AddWithValue("@operator_ref", operator_ref);
            cmd.Parameters.AddWithValue("@error_code", error_code);
            cmd.Parameters.AddWithValue("@message", message);
            cmd.Parameters.AddWithValue("@time", time);

            cmd.Connection = conc;


            SqlParameter SqltrID = new SqlParameter("@trID1", SqlDbType.VarChar, 500);
            SqltrID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqltrID).Value = "";

            SqlParameter SqlParaNewID = new SqlParameter("@status1", SqlDbType.VarChar, 500);
            SqlParaNewID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaNewID).Value = "";

            SqlParameter SqlParaErrCode = new SqlParameter("@ErrCode", SqlDbType.VarChar, 500);
            SqlParaErrCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode).Value = "";

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            status1 = (string)SqlParaNewID.Value;
            ErrCode = (string)SqlParaErrCode.Value;
            trID1 = (string)SqltrID.Value;
            return value;




        }
        catch (Exception ex)
        {

            trID1 = "0";
            ErrCode = "";
            status1 = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion


    #region Electricity_api

    public int Electricity_api(string CusID, string mobile_no, string operator_name, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        float amount1 = 0;
        float balance1 = 0;
        amount1 = float.Parse(amount2.ToString());
        balance1 = float.Parse(balance.ToString());

        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand("DTH_recharge_api", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "DTH_recharge_api";
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@mobile_no", mobile_no);
            cmd.Parameters.AddWithValue("@operator_name", operator_name);
            cmd.Parameters.AddWithValue("@your_cost", your_cost);
            cmd.Parameters.AddWithValue("@amount2", amount1);
            cmd.Parameters.AddWithValue("@balance", balance1);
            cmd.Parameters.AddWithValue("@balance1", balance);
            cmd.Parameters.AddWithValue("@txid", txid);
            cmd.Parameters.AddWithValue("@user_txid", user_txid);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@number", number);
            cmd.Parameters.AddWithValue("@operator_ref", operator_ref);
            cmd.Parameters.AddWithValue("@error_code", error_code);
            cmd.Parameters.AddWithValue("@message", message);
            cmd.Parameters.AddWithValue("@time", time);

            cmd.Connection = conc;


            SqlParameter SqltrID = new SqlParameter("@trID1", SqlDbType.VarChar, 500);
            SqltrID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqltrID).Value = "";

            SqlParameter SqlParaNewID = new SqlParameter("@status1", SqlDbType.VarChar, 500);
            SqlParaNewID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaNewID).Value = "";

            SqlParameter SqlParaErrCode = new SqlParameter("@ErrCode", SqlDbType.VarChar, 500);
            SqlParaErrCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode).Value = "";

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            status1 = (string)SqlParaNewID.Value;
            ErrCode = (string)SqlParaErrCode.Value;
            trID1 = (string)SqltrID.Value;
            return value;




        }
        catch (Exception ex)
        {

            trID1 = "0";
            ErrCode = "";
            status1 = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion


    #region Gas_api

    public int Gas_api(string CusID, string mobile_no, string operator_name, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        float amount1 = 0;
        float balance1 = 0;
        amount1 = float.Parse(amount2.ToString());
        balance1 = float.Parse(balance.ToString());

        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand("Gas_api", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Gas_api";
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@mobile_no", mobile_no);
            cmd.Parameters.AddWithValue("@operator_name", operator_name);
            cmd.Parameters.AddWithValue("@your_cost", your_cost);
            cmd.Parameters.AddWithValue("@amount2", amount1);
            cmd.Parameters.AddWithValue("@balance", balance1);
            cmd.Parameters.AddWithValue("@balance1", balance);
            cmd.Parameters.AddWithValue("@txid", txid);
            cmd.Parameters.AddWithValue("@user_txid", user_txid);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@number", number);
            cmd.Parameters.AddWithValue("@operator_ref", operator_ref);
            cmd.Parameters.AddWithValue("@error_code", error_code);
            cmd.Parameters.AddWithValue("@message", message);
            cmd.Parameters.AddWithValue("@time", time);

            cmd.Connection = conc;


            SqlParameter SqltrID = new SqlParameter("@trID1", SqlDbType.VarChar, 500);
            SqltrID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqltrID).Value = "";

            SqlParameter SqlParaNewID = new SqlParameter("@status1", SqlDbType.VarChar, 500);
            SqlParaNewID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaNewID).Value = "";

            SqlParameter SqlParaErrCode = new SqlParameter("@ErrCode", SqlDbType.VarChar, 500);
            SqlParaErrCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode).Value = "";

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            status1 = (string)SqlParaNewID.Value;
            ErrCode = (string)SqlParaErrCode.Value;
            trID1 = (string)SqltrID.Value;
            return value;




        }
        catch (Exception ex)
        {

            trID1 = "0";
            ErrCode = "";
            status1 = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion



    #region Insurance_api

    public int Insurance_api(string CusID, string mobile_no, string operator_name, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        float amount1 = 0;
        float balance1 = 0;
        amount1 = float.Parse(amount2.ToString());
        balance1 = float.Parse(balance.ToString());

        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand("Insurance_api", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Insurance_api";
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@mobile_no", mobile_no);
            cmd.Parameters.AddWithValue("@operator_name", operator_name);
            cmd.Parameters.AddWithValue("@your_cost", your_cost);
            cmd.Parameters.AddWithValue("@amount2", amount1);
            cmd.Parameters.AddWithValue("@balance", balance1);
            cmd.Parameters.AddWithValue("@balance1", balance);
            cmd.Parameters.AddWithValue("@txid", txid);
            cmd.Parameters.AddWithValue("@user_txid", user_txid);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@number", number);
            cmd.Parameters.AddWithValue("@operator_ref", operator_ref);
            cmd.Parameters.AddWithValue("@error_code", error_code);
            cmd.Parameters.AddWithValue("@message", message);
            cmd.Parameters.AddWithValue("@time", time);

            cmd.Connection = conc;


            SqlParameter SqltrID = new SqlParameter("@trID1", SqlDbType.VarChar, 500);
            SqltrID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqltrID).Value = "";

            SqlParameter SqlParaNewID = new SqlParameter("@status1", SqlDbType.VarChar, 500);
            SqlParaNewID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaNewID).Value = "";

            SqlParameter SqlParaErrCode = new SqlParameter("@ErrCode", SqlDbType.VarChar, 500);
            SqlParaErrCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode).Value = "";

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            status1 = (string)SqlParaNewID.Value;
            ErrCode = (string)SqlParaErrCode.Value;
            trID1 = (string)SqltrID.Value;
            return value;




        }
        catch (Exception ex)
        {

            trID1 = "0";
            ErrCode = "";
            status1 = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion


    #region transactionfailpremobile

    public int transactionfailpremobile(string CusID, string mobile_no, string operator_name, string circle_id, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        float amount1 = 0;
        float balance1 = 0;
        amount1 = float.Parse(amount2.ToString());
        balance1 = float.Parse(balance.ToString());

        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand("transactionfailpremobile", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "transactionfailpremobile";
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@mobile_no", mobile_no);
            cmd.Parameters.AddWithValue("@operator_name", operator_name);
            cmd.Parameters.AddWithValue("@circle_id", circle_id);
            cmd.Parameters.AddWithValue("@your_cost", your_cost);
            cmd.Parameters.AddWithValue("@amount2", amount1);
            cmd.Parameters.AddWithValue("@balance", balance1);
            cmd.Parameters.AddWithValue("@balance1", balance);
            cmd.Parameters.AddWithValue("@txid", txid);
            cmd.Parameters.AddWithValue("@user_txid", user_txid);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@number", number);
            cmd.Parameters.AddWithValue("@operator_ref", operator_ref);
            cmd.Parameters.AddWithValue("@error_code", error_code);
            cmd.Parameters.AddWithValue("@message", message);
            cmd.Parameters.AddWithValue("@time", time);

            cmd.Connection = conc;


            SqlParameter SqltrID = new SqlParameter("@trID1", SqlDbType.VarChar, 500);
            SqltrID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqltrID).Value = "";

            SqlParameter SqlParaNewID = new SqlParameter("@status1", SqlDbType.VarChar, 500);
            SqlParaNewID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaNewID).Value = "";

            SqlParameter SqlParaErrCode = new SqlParameter("@ErrCode", SqlDbType.VarChar, 500);
            SqlParaErrCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode).Value = "";

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            status1 = (string)SqlParaNewID.Value;
            ErrCode = (string)SqlParaErrCode.Value;
            trID1 = (string)SqltrID.Value;
            return value;




        }
        catch (Exception ex)
        {

            trID1 = "0";
            ErrCode = "";
            status1 = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion


    #region transactionfailpgas

    public int transactionfailpgas(string CusID, string mobile_no, string operator_name, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        float amount1 = 0;
        float balance1 = 0;
        amount1 = float.Parse(amount2.ToString());
        balance1 = float.Parse(balance.ToString());

        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand("transactionfailpgas", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "transactionfailpdth";
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@mobile_no", mobile_no);
            cmd.Parameters.AddWithValue("@operator_name", operator_name);
            cmd.Parameters.AddWithValue("@your_cost", your_cost);
            cmd.Parameters.AddWithValue("@amount2", amount1);
            cmd.Parameters.AddWithValue("@balance", balance1);
            cmd.Parameters.AddWithValue("@balance1", balance);
            cmd.Parameters.AddWithValue("@txid", txid);
            cmd.Parameters.AddWithValue("@user_txid", user_txid);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@number", number);
            cmd.Parameters.AddWithValue("@operator_ref", operator_ref);
            cmd.Parameters.AddWithValue("@error_code", error_code);
            cmd.Parameters.AddWithValue("@message", message);
            cmd.Parameters.AddWithValue("@time", time);

            cmd.Connection = conc;


            SqlParameter SqltrID = new SqlParameter("@trID1", SqlDbType.VarChar, 500);
            SqltrID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqltrID).Value = "";

            SqlParameter SqlParaNewID = new SqlParameter("@status1", SqlDbType.VarChar, 500);
            SqlParaNewID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaNewID).Value = "";

            SqlParameter SqlParaErrCode = new SqlParameter("@ErrCode", SqlDbType.VarChar, 500);
            SqlParaErrCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode).Value = "";

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            status1 = (string)SqlParaNewID.Value;
            ErrCode = (string)SqlParaErrCode.Value;
            trID1 = (string)SqltrID.Value;
            return value;




        }
        catch (Exception ex)
        {

            trID1 = "0";
            ErrCode = "";
            status1 = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion

    #region transactionfailInsurance

    public int transactionfailInsurance(string CusID, string mobile_no, string operator_name, string your_cost, string amount2, string balance, string txid, string user_txid, string status, string number, string operator_ref, string error_code, string message, string time, out string trID1, out string status1, out string ErrCode)
    {
        float amount1 = 0;
        float balance1 = 0;
        amount1 = float.Parse(amount2.ToString());
        balance1 = float.Parse(balance.ToString());

        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
            SqlCommand cmd = new SqlCommand("transactionfailInsurance", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "transactionfailInsurance";
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@mobile_no", mobile_no);
            cmd.Parameters.AddWithValue("@operator_name", operator_name);
            cmd.Parameters.AddWithValue("@your_cost", your_cost);
            cmd.Parameters.AddWithValue("@amount2", amount1);
            cmd.Parameters.AddWithValue("@balance", balance1);
            cmd.Parameters.AddWithValue("@balance1", balance);
            cmd.Parameters.AddWithValue("@txid", txid);
            cmd.Parameters.AddWithValue("@user_txid", user_txid);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@number", number);
            cmd.Parameters.AddWithValue("@operator_ref", operator_ref);
            cmd.Parameters.AddWithValue("@error_code", error_code);
            cmd.Parameters.AddWithValue("@message", message);
            cmd.Parameters.AddWithValue("@time", time);

            cmd.Connection = conc;


            SqlParameter SqltrID = new SqlParameter("@trID1", SqlDbType.VarChar, 500);
            SqltrID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqltrID).Value = "";

            SqlParameter SqlParaNewID = new SqlParameter("@status1", SqlDbType.VarChar, 500);
            SqlParaNewID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaNewID).Value = "";

            SqlParameter SqlParaErrCode = new SqlParameter("@ErrCode", SqlDbType.VarChar, 500);
            SqlParaErrCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode).Value = "";

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            status1 = (string)SqlParaNewID.Value;
            ErrCode = (string)SqlParaErrCode.Value;
            trID1 = (string)SqltrID.Value;
            return value;




        }
        catch (Exception ex)
        {

            trID1 = "0";
            ErrCode = "";
            status1 = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion

    #region RechargeStatus

    public int RechargeStatus(string CusID, string STATUS, string MOBILE, string AMOUNT, string RPID, string AGENTID, string OPID, string BAL, string MSG, out string err_code, out string NewID)
    {
        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {

            SqlCommand cmd = new SqlCommand("update_rech_status", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@STATUS", STATUS);
            cmd.Parameters.AddWithValue("@MOBILE", MOBILE);
            cmd.Parameters.AddWithValue("@AMOUNT", AMOUNT);
            cmd.Parameters.AddWithValue("@RPID", RPID);
            cmd.Parameters.AddWithValue("@AGENTID", AGENTID);
            cmd.Parameters.AddWithValue("@OPID", OPID);
            cmd.Parameters.AddWithValue("@BAL", BAL);
            cmd.Parameters.AddWithValue("@MSG", MSG);


            SqlParameter SqlParaNewID = new SqlParameter("@NewID", SqlDbType.VarChar, 500);
            SqlParaNewID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaNewID).Value = "";

            SqlParameter SqlParaErrMsg = new SqlParameter("@err_code", SqlDbType.VarChar, 500);
            SqlParaErrMsg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrMsg).Value = "";

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            NewID = (string)SqlParaNewID.Value;
            err_code = (string)SqlParaErrMsg.Value;
            return value;

        }
        catch (Exception ex)
        {
            NewID = "";
            err_code = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion


    #region checklimit

    public int checklimit(string CusID, string Amount, out string flag, out string errMsg)
    {
        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {

            SqlCommand cmd = new SqlCommand("DMR_checklimit", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@Amount", Amount);


            SqlParameter SqlParaNewID = new SqlParameter("@flag", SqlDbType.VarChar, 500);
            SqlParaNewID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaNewID).Value = "";

            SqlParameter SqlParaErrMsg = new SqlParameter("@errMsg", SqlDbType.VarChar, 500);
            SqlParaErrMsg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrMsg).Value = "";

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            flag = (string)SqlParaNewID.Value;
            errMsg = (string)SqlParaErrMsg.Value;
            return value;

        }
        catch (Exception ex)
        {
            flag = "";
            errMsg = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion

    #region DMRTransactionFail

    public int DMRTransactionFail(string CusID, string SenderMobileNo, string BankAccount, string Amount, string Recipientid, string Channel, string IMEI, string MESSAGE, out string NewID, out string ErrCode)
    {
        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {

            SqlCommand cmd = new SqlCommand("DMRTransactionFail", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@SenderMobileNo", SenderMobileNo);
            cmd.Parameters.AddWithValue("@BankAccount", BankAccount);
            cmd.Parameters.AddWithValue("@Amount", Amount);
            cmd.Parameters.AddWithValue("@Recipientid", Recipientid);
            cmd.Parameters.AddWithValue("@Channel", Channel);
            cmd.Parameters.AddWithValue("@IMEI", IMEI);
            cmd.Parameters.AddWithValue("@MESSAGE", MESSAGE);


            SqlParameter SqlParaNewID = new SqlParameter("@NewID", SqlDbType.VarChar, 500);
            SqlParaNewID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaNewID).Value = "";

            SqlParameter SqlParaErrMsg = new SqlParameter("@ErrCode", SqlDbType.VarChar, 500);
            SqlParaErrMsg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrMsg).Value = "";

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            NewID = (string)SqlParaNewID.Value;
            ErrCode = (string)SqlParaErrMsg.Value;
            return value;

        }
        catch (Exception ex)
        {
            NewID = "";
            ErrCode = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion

    #region DMRTransaction

    public int DMRTransaction(string CusID, string SenderMobileNo, string BankAccount, string Amount, string Recipientid, string Channel, string IMEI, string MESSAGE, out string NewID, out string ErrCode)
    {
        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {

            SqlCommand cmd = new SqlCommand("DMRTransaction", conc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CusID", CusID);
            cmd.Parameters.AddWithValue("@SenderMobileNo", SenderMobileNo);
            cmd.Parameters.AddWithValue("@BankAccount", BankAccount);
            cmd.Parameters.AddWithValue("@Amount", Amount);
            cmd.Parameters.AddWithValue("@Recipientid", Recipientid);
            cmd.Parameters.AddWithValue("@Channel", Channel);
            cmd.Parameters.AddWithValue("@IMEI", IMEI);
            cmd.Parameters.AddWithValue("@MESSAGE", MESSAGE);


            SqlParameter SqlParaNewID = new SqlParameter("@NewID", SqlDbType.VarChar, 500);
            SqlParaNewID.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaNewID).Value = "";

            SqlParameter SqlParaErrMsg = new SqlParameter("@ErrCode", SqlDbType.VarChar, 500);
            SqlParaErrMsg.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrMsg).Value = "";

            if (conc.State == ConnectionState.Closed)
                conc.Open();
            int value = cmd.ExecuteNonQuery();
            NewID = (string)SqlParaNewID.Value;
            ErrCode = (string)SqlParaErrMsg.Value;
            return value;

        }
        catch (Exception ex)
        {
            NewID = "";
            ErrCode = ex.Message;
            return 0;
        }
        finally
        {
            conc.Close();
        }

    }

    #endregion

    #region userRegistration

    public int userRegistration(int serialno, int CusID, float Cust_SponserID, string Cust_Location, DateTime Entry_Date, string Cust_Name, string Email, string Cust_Country, Int64 Cust_mobileNo, string Cust_Password, string Cust_Package, int Approved, int Pin_Approved, int plannerId, int totaldir, string tokenno)
    {
        SqlConnection conc = new SqlConnection(ConnectionString1);
        try
        {
//            SqlCommand cmd = new SqlCommand("api_login", conc);
//            cmd.CommandType = CommandType.StoredProcedure;
            SqlCommand cmd = new SqlCommand();
//            cmd.CommandText = "api_autologin";


            cmd.Parameters.AddWithValue("@serialno", serialno).ToString();
            cmd.Parameters.AddWithValue("@CusID", CusID).ToString();
            cmd.Parameters.AddWithValue("@Cust_SponserID", Cust_SponserID).ToString();
            cmd.Parameters.AddWithValue("@Cust_Location", Cust_Location);
            cmd.Parameters.AddWithValue("@Entry_Date", Entry_Date);
            cmd.Parameters.AddWithValue("@Cust_Name", Cust_Name);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Cust_Country", Cust_Country);
            cmd.Parameters.AddWithValue("@Cust_mobileNo", Cust_mobileNo);
            cmd.Parameters.AddWithValue("@Cust_Password", Cust_Password);
            cmd.Parameters.AddWithValue("@Cust_Package", Cust_Package);
            cmd.Parameters.AddWithValue("@Approved", Approved).ToString();
            cmd.Parameters.AddWithValue("@Pin_Approved", Pin_Approved);
            cmd.Parameters.AddWithValue("@plannerId", plannerId);
            cmd.Parameters.AddWithValue("@totaldir", totaldir);
       
           
        
         
             conc.Open();

            SqlParameter SqlParaErrCode = new SqlParameter("@message", SqlDbType.VarChar, 500);
            SqlParaErrCode.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode).Value = 0;

            SqlParameter SqlParaErrCode1 = new SqlParameter("@ErrCode", SqlDbType.VarChar, 500);
            SqlParaErrCode1.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(SqlParaErrCode1).Value = 0;



            if (conc.State == ConnectionState.Closed)
                conc.Open();
           int value = cmd.ExecuteNonQuery();
            message = (string)SqlParaErrCode.Value;
            ErrCode = (string)SqlParaErrCode1.Value;
          

            return value;

        }
        catch (Exception ex)
        {
            message = "0";
            ErrCode = ex.Message;
      
            return 0;
        }
        finally
        {
            conc.Close();
        }


    }
    #endregion

}