using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Web.SessionState;
using System.Web; // to Pass Session values;
using System.Web.UI;
using UniversityDAL;

public class LoginLogs : DataBaseConnection
{

    DataSet ds = new DataSet();
    SqlParameter[] param;
    public int UserAccountID { get; set; }
    public string HostName { get; set; }
    public string IPAddress { get; set; }
    public string BrowserName { get; set; }
    public string Status { get; set; }
    public string MenuItemCode { get; set; }
    public int RoleID { get; set; }
    public string guid { get; set; }
    public string AccountName { get; set; }
    public string AccountPassword { get; set; }
    public string VisitedPage { get; set; }
    public string ErrorType { get; set; }

    public string LastErrorMessage { get; set; }
    public string Message { get; set; }
    public string HelpLink { get; set; }
    public string StackTrace { get; set; }
    public string Source { get; set; }


    public void LoginLogInsert()  // Only When User Login and LogOut/////////
    {
        param = new SqlParameter[5];
        param[0] = new SqlParameter("@UserAccountID", UserAccountID);
        param[1] = new SqlParameter("@HostName", HostName);
        param[2] = new SqlParameter("@IPAddress", IPAddress);
        param[3] = new SqlParameter("@BrowserName", BrowserName);
        param[4] = new SqlParameter("@guid", guid);
        SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "usp_LoginLogInsert", param);
    }

    public void LoginLogUpdate() // Only When User Login and LogOut/////////
    {
        param = new SqlParameter[1];
        param[0] = new SqlParameter("@guid", guid);
        SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "usp_LoginLogUpdate", param);
    }



    public bool LoginIsUserAccountHasCapability(int UseraccountID, string MenuItemCode, int RoleID)
    {
        string IpAddress;
        if (HttpContext.Current.Session["IpAddress"] == null)
        {
            IpAddress = "SignOut";
        }
        else
        {
            IpAddress = HttpContext.Current.Session["IpAddress"].ToString();
        }

        param = new SqlParameter[4];
        param[0] = new SqlParameter("@UserAccountID", UseraccountID);
        param[1] = new SqlParameter("@MenuItemCode", MenuItemCode);
        param[2] = new SqlParameter("@RoleID", RoleID);
        param[3] = new SqlParameter("@IpAddress", IpAddress);
        ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_LoginUserAccountCapability", param);
        return Convert.ToBoolean(ds.Tables[0].Rows[0]["IsCapability"]);
    }


    public void LoginNoAccess()
    {
        param = new SqlParameter[4];
        param[0] = new SqlParameter("@UserAccountID", UserAccountID);
        param[1] = new SqlParameter("@MenuItemCode", MenuItemCode);
        param[2] = new SqlParameter("@IPAddress", IPAddress);
        param[3] = new SqlParameter("@RoleID", RoleID);
        SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_LoginNoAccess", param);
    }

    public DataSet LoginLoginWithUserNameAndPassword()
    {
        param = new SqlParameter[2];
        param[0] = new SqlParameter("@AccountName", AccountName);
        param[1] = new SqlParameter("@AccountPassword", AccountPassword);
        return ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_LoginLoginWithUserNameAndPassword", param);
    }


    public DataTable LoginMenuBinding()
    {
        param = new SqlParameter[1];
        param[0] = new SqlParameter("@UserAccountID", UserAccountID);
        ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_LoginMenuBind", param);
        return ds.Tables[0];
    }

    public void Error_Log(string messge, string source, string stackTrace, string AbsolutePath)
    {
        int useraccountid = 0;
        try
        {
            useraccountid = Convert.ToInt32(HttpContext.Current.Session[System.Configuration.ConfigurationManager.AppSettings["currentUserAccountId"].ToString()]);
        }
        catch
        {
            useraccountid = 0;

            param = new SqlParameter[8];
            param[0] = new SqlParameter("@UserAccountID", useraccountid);
            param[1] = new SqlParameter("@HostName", HttpContext.Current.Request.UserHostName);
            param[2] = new SqlParameter("@IPAddress", HttpContext.Current.Request.UserHostAddress);
            param[3] = new SqlParameter("@Browser", HttpContext.Current.Request.Browser.Browser);
            param[4] = new SqlParameter("@VisitedPage", AbsolutePath);
            param[5] = new SqlParameter("@Message", messge);
            param[6] = new SqlParameter("@Source", source);
            param[7] = new SqlParameter("@StackTrace", stackTrace);
            SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "usp_LoginErrorLogInsert", param);
        }
    }

    public DataSet LoginLoginGetDefaultValuesForCurrentUser(int UserAccountID)
    {
        param = new SqlParameter[1];
        param[0] = new SqlParameter("@UserAccountID", UserAccountID);
        return ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "LoginGetDefaultValuesForCurrentUser", param);
    }




    //
    //{
    //    int UserAccountID = 0;
    //    try
    //    {
    //        UserAccountID = Convert.ToInt32(HttpContext.Current.Session[System.Configuration.ConfigurationManager.AppSettings["currentUserAccountId"].ToString()]);
    //    }
    //    catch { UserAccountID = 0; }

    //    LoginLogs log = new LoginLogs();
    //    log.UserAccountID = UserAccountID;
    //    log.HostName = HttpContext.Current.Request.UserHostName;
    //    log.IPAddress = HttpContext.Current.Request.UserHostAddress;
    //    log.BrowserName = HttpContext.Current.Request.Browser.Browser;
    //    log.VisitedPage = AbsolutePath;
    //    log.Message = messge;
    //    log.Source = source;
    //    log.StackTrace = stackTrace;
    //    log.LoginErrorLogInsert();
    //menntal
    //}




    //public void LoginOtherLogInsert()
    //{
    //    param = new SqlParameter[6];
    //    param[0] = new SqlParameter("@UserAccountID", UserAccountID);
    //    param[1] = new SqlParameter("@HostName", HostName);
    //    param[2] = new SqlParameter("@IPAddress", IPAddress);
    //    param[3] = new SqlParameter("@Browser", BrowserName);
    //    param[4] = new SqlParameter("@VisitedPage", VisitedPage);
    //    param[5] = new SqlParameter("@Type", ErrorType);
    //    SqlHelper.ExecuteScalar(con, CommandType.StoredProcedure, "usp_LoginOtherErrorInsert", param);
    //}

}