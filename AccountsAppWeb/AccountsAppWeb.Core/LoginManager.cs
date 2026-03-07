using AccountsAppWeb.Core.Models;
using System;
using AccountsAppWeb.Core.com.kccsasr.accounts;
using System.Data;

namespace AccountsAppWeb.Core
{
    public class LoginManager
    {
        private readonly AccountsAppAPI accountsAppAPI;
        string error = string.Empty;

        public LoginManager()
        {
            accountsAppAPI = new AccountsAppAPI();
        }
        public UserModel AuthenticateUser(string userName, string password)
        {
            var response = accountsAppAPI.LoginCollege(userName, password, error, DateTime.Now);
            //code to fetch deleted transaction
            DataSet dataSet = accountsAppAPI.GetVoucherDetails("", userName);
            if (response != null && response.Tables.Count == 0)
            {
                return new UserModel()
                {
                    IsLoginSuccess = false
                };
            }
            else
            {
                int instituteId = Convert.ToInt32(response.Tables[0].Rows[0]["UA_InstId"].ToString());
                return new UserModel()
                {
                    IsLoginSuccess = true,
                    InstituteId = instituteId,
                    UserName = response.Tables[0].Rows[0]["UA_AccountName"].ToString(),
                    DepartmentID = Convert.ToInt32(response.Tables[0].Rows[0]["DepartmentID"]),
                    IsEnableGroupPage = (instituteId == 300010 || instituteId == 0) ? true : false,
                    InstName = !(response.Tables[0].Rows[0]["InstName"].ToString() == "Under Secretary") ? response.Tables[0].Rows[0]["InstName"].ToString() : "Joint Account"
                };
            }
        }

        private void GetFinancialByInstituteId(int instituteId)
        {
            var response = accountsAppAPI.GetLoginFinancialID(string.Empty, instituteId);
            if (response != null && response.Tables.Count == 0)
            {

            }
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            return accountsAppAPI.ChangePassword(error, userName, oldPassword, newPassword);
        }
        public ViewPasswordModel GetPasswordDetails(string userName)
        {
            ViewPasswordModel passwordModel = new ViewPasswordModel();
            DataSet dataSet = accountsAppAPI.GetPasswordByAccountName(error, userName);
            if (dataSet != null && dataSet.Tables[0].Rows.Count == 1)
            {
                DataRow row = dataSet.Tables[0].Rows[0];              
                passwordModel.Inst_Title = row["Inst_Title"].ToString();
                passwordModel.UA_AccountName = row["UA_AccountName"].ToString();
                passwordModel.Password = row["Password"].ToString();
            }
            return passwordModel;
        }
    }
}
