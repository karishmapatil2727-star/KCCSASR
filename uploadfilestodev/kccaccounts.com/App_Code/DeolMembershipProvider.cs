using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using System.Web;
using System.Configuration;
//using University;
using System.Data;
using System.IO;
using UniversityDAL;

public class DeolMembershipProvider : MembershipProvider
{
    Security security = new Security();
    public override bool ChangePassword(string username, string oldPassword, string newPassword)
    {
        throw new NotImplementedException();
    }
    //public override bool ChangeCorrespondencePassword(string username, string oldPassword, string newPassword)
    //{
    //    throw new NotImplementedException();
    //}

    public bool ChangePassword(string username, string oldPassword, string newPassword, string passwordHint)
    {
        using (DataContext usersContext = new DataContext())
        {
            //MembershipUser currentUser = GetUser(username, true /* userIsOnline */);
            //if (currentUser != null)
            //{
            //string encryPass = EncryPasswordWithMd5Hasher(oldPassword);
            string encryPass = security.Encrypt(oldPassword);

            var user = usersContext.GetUserByUserAccountIDPassword(Convert.ToInt32(username), encryPass);
            if (user != null)
            {
                //user.UA_AccountPassword = EncryPasswordWithMd5Hasher(newPassword);
                user.UA_AccountPassword = security.Encrypt(newPassword);
                user.UA_AccountPasswordHint = passwordHint;
                user.UA_LastPasswordChargedDate = DateTime.Now;
                user.UA_UpdateDate = DateTime.Now;
                usersContext.Update(user);
                return true;
            };
            //}
            return false;
        }
    }
    public bool ChangeCorrespondencePassword(string username, string oldPassword, string newPassword, string passwordHint="")
    {
        using (DataContext usersContext = new DataContext())
        {
            //MembershipUser currentUser = GetUser(username, true /* userIsOnline */);
            //if (currentUser != null)
            //{
            //string encryPass = EncryPasswordWithMd5Hasher(oldPassword);
            string encryPass = security.Encrypt(oldPassword);

            var user = usersContext.GetUserByUserIdPasswordForCorrespondenceAuthentication(Convert.ToInt32(username), encryPass);
            if (user != null)
            {
                //user.UA_AccountPassword = EncryPasswordWithMd5Hasher(newPassword);
                user.UA_CorrespondencePassword= security.Encrypt(newPassword);
              
                user.UA_LastPasswordChargedDate = DateTime.Now;
                user.UA_UpdateDate = DateTime.Now;
                usersContext.Update(user);
                return true;
            };
            //}
            return false;
        }
    }
    public string UserPassword(string username)
    {
        DataContext context = new DataContext();
        return context.UserPassword(username);
    }
    public string UserCorrespondencePassword(string username)
    {
        DataContext context = new DataContext();
        return context.UserCorrespondencePassword(username);
    }
    public string UserCorrespondencePassword(int UserAccountId)
    {
        DataContext context = new DataContext();
        return context.UserCorrespondencePassword(UserAccountId);
    }

    public string UserPassword(int userId)
    {
        DataContext context = new DataContext();
        return context.UserPassword(userId);
    }

    public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
    {
        throw new NotImplementedException();
    }

    public MembershipUser CreateUser(int userId, string userName, string password, string passwordHint, string email, DateTime lastLoginDate, DateTime secondLastLoginDate, string passwordRecoveryCode, string securityCode, string actionCode, int forgotPasswordAttempt, DateTime lastPasswordChargedDate, DateTime suspendFromDate, DateTime suspendToDate, bool isDetainedCompletely, int centreId, string examType, string userType, bool isActive, DateTime insertDate, int insertUserAccountId, DateTime updateDate, int updateUserAccountId, out MembershipCreateStatus status)
    {
        var args = new ValidatePasswordEventArgs(userName, password, true);
        OnValidatingPassword(args);

        if (args.Cancel)
        {
            status = MembershipCreateStatus.InvalidPassword;
            return null;
        }

        //if (RequiresUniqueEmail && GetUserNameByEmail(email) != string.Empty)
        //{
        //    status = MembershipCreateStatus.DuplicateEmail;
        //    return null;
        //}
        if (MinRequiredPasswordLength > password.Trim().Length)
        {
            status = MembershipCreateStatus.InvalidPassword;
            return null;
        }

        var user = GetUser(userName, true);

        if (user == null)
        {
            var userObj = new UniversityDAL.UserAccount
            {
                UserAccountId = userId,
                UA_AccountName = userName,
                UA_AccountPassword = security.Encrypt(password),
                UA_AccountPasswordHint = passwordHint,
                UA_EmailAddress = email,
                UA_LastLoginDate = lastLoginDate,
                UA_SecondLastLoginDate = secondLastLoginDate,
                UA_PasswordRecoveryCode = passwordRecoveryCode,
                UA_SecurityCode = securityCode,
                UA_ActionCode = actionCode,
                UA_ForgotPasswordAttempt = forgotPasswordAttempt,
                UA_LastPasswordChargedDate = lastPasswordChargedDate,
                UA_SuspendFromDate = suspendFromDate,
                UA_SuspendToDate = suspendToDate,
                UA_IsDetainedCompletely = isDetainedCompletely,
                UA_CentreId = centreId,
                UA_ExamType = examType,
                UA_UserType = userType,
                UA_IsActive = isActive,
                UA_InsertDate = insertDate,
                UA_InsertUserAccountId = insertUserAccountId,
                UA_UpdateDate = updateDate,
                UA_UpdateUserAccountId = updateUserAccountId
            };

            using (var usersContext = new DataContext())
            {
                usersContext.AddUser(userObj);
            }

            status = MembershipCreateStatus.Success;

            return GetUser(userName, true);
        }
        status = MembershipCreateStatus.DuplicateUserName;

        return null;
    }

    public MembershipUser UpdateUser(int userId, string userName, string email, bool isActive, DateTime updateDate, int updateUserAccountId, out MembershipCreateStatus status)
    {
        if (RequiresUniqueEmail && GetUserNameByEmail(userId, email) != string.Empty)
        {
            status = MembershipCreateStatus.DuplicateEmail;
            return null;
        }
        var usersContext = new DataContext();
        EntitiesModel model = new EntitiesModel();

        UniversityDAL.UserAccount userAccount = usersContext.GetUser(ref model, userId);

        userAccount.UA_AccountName = userName;
        userAccount.UA_EmailAddress = email;
        userAccount.UA_IsActive = isActive;
        userAccount.UA_UpdateDate = updateDate;
        userAccount.UA_UpdateUserAccountId = updateUserAccountId;

        model.SaveChanges();

        status = MembershipCreateStatus.Success;

        return GetUser(userName, true);

        status = MembershipCreateStatus.DuplicateUserName;

        return null;
    }

    public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
    {
        status = MembershipCreateStatus.ProviderError;
        return null;
    }

    public string GetUserNameByEmail(int userId, string email)
    {
        var usersContext = new DataContext();
        var requiredUser = usersContext.GetUserByEmailAddress(userId, email);
        if (requiredUser != null)
        {
            return requiredUser.UA_EmailAddress;
        }
        return String.Empty;
    }

    public override bool DeleteUser(string username, bool deleteAllRelatedData)
    {
        throw new NotImplementedException();
    }

    public override bool EnablePasswordReset
    {
        get { throw new NotImplementedException(); }
    }

    public override bool EnablePasswordRetrieval
    {
        get { throw new NotImplementedException(); }
    }

    public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
    {
        throw new NotImplementedException();
    }

    public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
    {
        throw new NotImplementedException();
    }

    public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
    {
        throw new NotImplementedException();
    }

    public override int GetNumberOfUsersOnline()
    {
        throw new NotImplementedException();
    }

    public override string GetPassword(string username, string answer)
    {
        throw new NotImplementedException();
    }

    public override MembershipUser GetUser(string username, bool userIsOnline)
    {
        var usersContext = new DataContext();
        var user = usersContext.GetUser(username);
        string myguid = Guid.NewGuid().ToString().Replace("-", "");
        if (user != null)
        {
            var memUser = new MembershipUser("DeolMembershipProvider", username, myguid, user.UA_EmailAddress,
                                                        string.Empty, string.Empty,
                                                        true, false, DateTime.MinValue,
                                                        DateTime.MinValue,
                                                        DateTime.MinValue,
                                                        DateTime.Now, DateTime.Now);
            return memUser;
        }
        return null;
    }

    public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
    {
        throw new NotImplementedException();
    }

    public override string GetUserNameByEmail(string email)
    {
        using (var usersContext = new DataContext())
        {
            var requiredUser = usersContext.GetUserByEmailAddress(email);
            if (requiredUser != null)
            {
                return requiredUser.UA_EmailAddress;
            }
            return String.Empty;
        }
    }

    public override int MaxInvalidPasswordAttempts
    {
        get { throw new NotImplementedException(); }
    }

    public override int MinRequiredNonAlphanumericCharacters
    {
        get { throw new NotImplementedException(); }
    }

    public override int MinRequiredPasswordLength
    {
        get { return 6; }
    }

    public override int PasswordAttemptWindow
    {
        get { throw new NotImplementedException(); }
    }

    public override MembershipPasswordFormat PasswordFormat
    {
        get { throw new NotImplementedException(); }
    }

    public override string PasswordStrengthRegularExpression
    {
        get { throw new NotImplementedException(); }
    }

    public override bool RequiresQuestionAndAnswer
    {
        get { return false; }
    }

    public override bool RequiresUniqueEmail
    {
        get { return false; }
    }

    public override string ResetPassword(string username, string answer)
    {
        throw new NotImplementedException();
    }

    public override bool UnlockUser(string userName)
    {
        throw new NotImplementedException();
    }

    public override void UpdateUser(MembershipUser user)
    {
        throw new NotImplementedException();
    }

    public override bool ValidateUser(string username, string password)
    {
        throw new NotImplementedException();
    }

    //public bool ValidateUser(ref int userId, string username, string password)
    //{
    //    var md5Hash = EncryPasswordWithMd5Hasher(password);
    //    using (var usersContext = new DataContext())
    //    {
    //        var requiredUser = usersContext.GetUser(username, md5Hash);
    //        if (requiredUser != null)
    //        {
    //            var memUser = new MembershipUser("DeolMembershipProvider", requiredUser.UserAccountId.ToString(), "ASPX_e10adc3949ba59ab1be56e057f20f883e", requiredUser.UA_EmailAddress,
    //                                                    string.Empty, string.Empty,
    //                                                    true, false, DateTime.MinValue,
    //                                                    DateTime.MinValue,
    //                                                    DateTime.MinValue,
    //                                                    DateTime.Now, DateTime.Now);
    //            userId = requiredUser.UserAccountId;
    //            HttpContext.Current.Session[ConfigurationManager.AppSettings["currentUserAccountId"].ToString()] = requiredUser.UA_AccountName;
    //            return requiredUser != null;
    //        }
    //        return requiredUser != null;
    //    }
    //}

    public static string EncryPasswordWithMd5Hasher(string value)
    {
        var md5Hasher = MD5.Create();
        var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
        var sBuilder = new StringBuilder();
        for (var i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }
        return sBuilder.ToString();
    }

    public override string ApplicationName
    {
        get
        {
            throw new NotImplementedException();
        }
        set
        {
            throw new NotImplementedException();
        }
    }

    //public void LoadValuesInSession(string AccountName, string Password)
    //{
    //    LoginLogs loginLog = new LoginLogs();
    //    loginLog.AccountName = AccountName;
    //    loginLog.AccountPassword = Password;
    //    DataSet dataSet = loginLog.LoginLoginWithUserNameAndPassword();
    //    DataTable item = dataSet.Tables[0];
    //    if (item.Rows.Count == 1 && item.Rows[0]["UA_AccountPassword"].ToString() == Password && Convert.ToBoolean(item.Rows[0]["UA_IsActive"]))
    //    {
    //        HttpContext.Current.Session["RoleID"] = item.Rows[0]["RoleID"];
    //        HttpContext.Current.Session["EvalID"] = item.Rows[0]["EvalID"];

    //        if (item.Rows[0]["RoleID"].ToString() == "14" || item.Rows[0]["RoleID"].ToString() == "4")
    //        {
    //            HttpContext.Current.Session["CollegeID"] = item.Rows[0]["CollegeID"];
    //            HttpContext.Current.Session["IsCollegeDevLock"] = Convert.ToInt32(item.Rows[0]["College_IsModuleLock"]);
    //        }
    //    }    
    //}

    public bool ValidateUser(ref int userId, string username, string password)
    {
        bool flag;
        string str = security.Encrypt(password);
        using (DataContext dataContext = new DataContext())
        {
            UserAccount user = dataContext.GetUserByUserIdPassword(username, str);
            if (user == null)
            {
                flag = user != null;
            }
            else
            {
                int userAccountId = user.UserAccountId;
                new MembershipUser("DeolMembershipProvider", userAccountId.ToString(), "ASPX_e10adc3949ba59ab1be56e057f20f883e", user.UA_EmailAddress, string.Empty, string.Empty, true, false, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.Now, DateTime.Now);
                userId = user.UserAccountId;
                HttpContext.Current.Session[ConfigurationManager.AppSettings["currentUser"].ToString()] = user.UA_AccountName;
                flag = user != null;

                /// Password Strength checker/
                HttpCookie StudentCookies = new HttpCookie("StrongPassword");
                if (BasePage.IsPasswordStrong(password))
                {
                    HttpContext.Current.Session["StrongPassword"] = "";
                    //StudentCookies.Value= "";
                    
                }
                else
                {
                    //StudentCookies.Value = "Your password is not strong, Please chanage your password.";
                    HttpContext.Current.Session["StrongPassword"] = "Your password is not strong, Please chanage your password.";
                }

                //StudentCookies.Expires = DateTime.Now.AddHours(1);
                //HttpContext.Current.Response.Cookies.Add(StudentCookies);
            }
        }
        return flag;
    }
    public bool ValidateUserForCorrespondenceAuthentication(ref int userId, int UserAccountId, string password)
    {
        bool flag;
        string str = security.Encrypt(password);
        using (DataContext dataContext = new DataContext())
        {
            UserAccount user = dataContext.GetUserByUserIdPasswordForCorrespondenceAuthentication(UserAccountId, str);
            if (user == null)
            {
                flag = user != null;
            }
            else
            {
                int userAccountId = user.UserAccountId;
                new MembershipUser("DeolMembershipProvider", userAccountId.ToString(), "ASPX_e10adc3949ba59ab1be56e057f20f883e", user.UA_EmailAddress, string.Empty, string.Empty, true, false, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.Now, DateTime.Now);
                userId = user.UserAccountId;
                HttpContext.Current.Session[ConfigurationManager.AppSettings["currentUser"].ToString()] = user.UA_AccountName;
                flag = user != null;

                /// Password Strength checker/
                HttpCookie StudentCookies = new HttpCookie("StrongPassword");
                if (BasePage.IsPasswordStrong(password))
                {
                    HttpContext.Current.Session["StrongPassword"] = "";
                    //StudentCookies.Value= "";

                }
                else
                {
                    //StudentCookies.Value = "Your password is not strong, Please chanage your password.";
                    HttpContext.Current.Session["StrongPassword"] = "Your password is not strong, Please chanage your password.";
                }

                //StudentCookies.Expires = DateTime.Now.AddHours(1);
                //HttpContext.Current.Response.Cookies.Add(StudentCookies);
            }
        }
        return flag;
    }
}