using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;


/// <summary>
/// This class contains methods required for databaseconnection
/// Created By: Lakhwinder
/// </summary>
/// 
namespace AccountsDB
{
    public class DataBaseConnection : System.Web.Services.WebService
    {
        public SqlConnection con = new SqlConnection();
        protected DataBaseConnection()
        {
            con.ConnectionString = ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString;
        }
        protected void OpenConnection()
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
        }
        protected void CloseConnection()
        {
            if (con.State == ConnectionState.Open)
                con.Close();
        }
        public static string Connection()
        {
            string s = string.Empty;
            s = ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString;
            return s;
        }
    }

}