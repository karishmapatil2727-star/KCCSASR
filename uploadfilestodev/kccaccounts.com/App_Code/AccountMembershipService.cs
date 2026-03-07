using System;
using System.Web.Security;

public class AccountMembershipService : IMembershipService
{
    private readonly DeolMembershipProvider _provider;
    private readonly DeolRoleProvider _roleProvider;
    public AccountMembershipService()
        : this(null)
    {
    }

    public AccountMembershipService(DeolMembershipProvider provider)
    {
        _provider = new DeolMembershipProvider();
        _roleProvider = new DeolRoleProvider();
    }

    public int MinPasswordLength
    {
        get
        {
            return _provider.MinRequiredPasswordLength;
        }
    }

    public bool ValidateUser(ref int userId, string userName, string password)
    {
        if (String.IsNullOrEmpty(userName)) MemberAccount.Message = "UserName cannot be null or empty.<br />";//throw new ArgumentException("Value cannot be null or empty.", "UserName");
        if (String.IsNullOrEmpty(password)) MemberAccount.Message += "Password cannot be null or empty."; //throw new ArgumentException("Value cannot be null or empty.", "password");
        if (MemberAccount.Message.Trim().Length != 0)
            return false;
        return _provider.ValidateUser(ref userId, userName, password);
    }
    public bool ValidateUserForCorrespondenceAuthentication(ref int userId, int UserAccountId, string password)
    {

        if (String.IsNullOrEmpty(password)) MemberAccount.Message += "Password cannot be null or empty."; //throw new ArgumentException("Value cannot be null or empty.", "password");
        if (MemberAccount.Message.Trim().Length != 0)
            return false;
        return _provider.ValidateUserForCorrespondenceAuthentication(ref userId, UserAccountId, password);
    }

    public MembershipCreateStatus CreateUser(int userId, string userName, string password, string passwordHint, string email, DateTime lastLoginDate, DateTime secondLastLoginDate, string passwordRecoveryCode, string securityCode, string actionCode, int forgotPasswordAttempt, DateTime lastPasswordChargedDate, DateTime suspendFromDate, DateTime suspendToDate, bool isDetainedCompletely, int centreId, string examType, string userType, bool isActive, DateTime insertDate, int insertUserAccountId, DateTime updateDate, int updateUserAccountId)
    {
        if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
        if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");
        //if (password.Trim().Length < MinPasswordLength) throw new ArgumentException("Password must be greater than " + MinPasswordLength.ToString() + " Characters", "password");
        //if (String.IsNullOrEmpty(email)) throw new ArgumentException("Value cannot be null or empty.", "email");
        if (String.IsNullOrEmpty(passwordHint)) throw new ArgumentException("Value cannot be null or empty.", "passwordHint");
        if (String.IsNullOrEmpty(lastLoginDate.ToShortDateString())) throw new ArgumentException("Value cannot be null or empty.", "lastLoginDate");
        if (String.IsNullOrEmpty(secondLastLoginDate.ToShortDateString())) throw new ArgumentException("Value cannot be null or empty.", "secondLastLoginDate");
        //if (String.IsNullOrEmpty(passwordRecoveryCode)) throw new ArgumentException("Value cannot be null or empty.", "passwordRecoveryCode");
        //if (String.IsNullOrEmpty(securityCode)) throw new ArgumentException("Value cannot be null or empty.", "securityCode");
        //if (String.IsNullOrEmpty(actionCode)) throw new ArgumentException("Value cannot be null or empty.", "actionCode");
        if (String.IsNullOrEmpty(forgotPasswordAttempt.ToString())) throw new ArgumentException("Value cannot be null or empty.", "forgotPasswordAttempt");
        if (String.IsNullOrEmpty(lastPasswordChargedDate.ToShortDateString())) throw new ArgumentException("Value cannot be null or empty.", "lastPasswordChargedDate");
        if (String.IsNullOrEmpty(suspendFromDate.ToShortDateString())) throw new ArgumentException("Value cannot be null or empty.", "suspendFromDate");
        if (String.IsNullOrEmpty(suspendToDate.ToShortDateString())) throw new ArgumentException("Value cannot be null or empty.", "suspendToDate");
        if (String.IsNullOrEmpty(centreId.ToString())) throw new ArgumentException("Value cannot be null or empty.", "centreId");
        //if (String.IsNullOrEmpty(examType)) throw new ArgumentException("Value cannot be null or empty.", "examType");
        //if (String.IsNullOrEmpty(userType)) throw new ArgumentException("Value cannot be null or empty.", "userType");
        if (String.IsNullOrEmpty(insertDate.ToShortDateString())) throw new ArgumentException("Value cannot be null or empty.", "insertDate");
        if (String.IsNullOrEmpty(insertUserAccountId.ToString())) throw new ArgumentException("Value cannot be null or empty.", "insertUserAccountId");
        if (String.IsNullOrEmpty(updateDate.ToShortDateString())) throw new ArgumentException("Value cannot be null or empty.", "updateDate");
        if (String.IsNullOrEmpty(updateUserAccountId.ToString())) throw new ArgumentException("Value cannot be null or empty.", "updateUserAccountId");

        MembershipCreateStatus status;
        _provider.CreateUser(userId, userName, password, passwordHint, email, lastLoginDate, secondLastLoginDate, passwordRecoveryCode, securityCode, actionCode, forgotPasswordAttempt, lastPasswordChargedDate, suspendFromDate, suspendToDate, isDetainedCompletely, centreId, examType, userType, isActive, insertDate, insertUserAccountId, updateDate, updateUserAccountId, out status);
        return status;
    }

    public MembershipCreateStatus UpdateUser(int userId, string userName, string email, bool isActive, DateTime updateDate, int updateUserAccountId)
    {
        if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
        if (String.IsNullOrEmpty(email)) throw new ArgumentException("Value cannot be null or empty.", "email");

        MembershipCreateStatus status;
        _provider.UpdateUser(userId, userName, email, isActive, updateDate, updateUserAccountId, out status);
        return status;
    }

    public bool ChangePassword(string userName, string oldPassword, string newPassword, string passwordHint)
    {
        if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
        if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
        if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");
        // The underlying ChangePassword() will throw an exception rather
        // than return false in certain failure scenarios.
        try
        {
            return _provider.ChangePassword(userName, oldPassword, newPassword, passwordHint);
        }
        catch (ArgumentException)
        {
            return false;
        }
        catch (MembershipPasswordException)
        {
            return false;
        }
    }
    public bool ChangeCorrespondencePassword(string userName, string oldPassword, string newPassword)
    {
        if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
        if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
        if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");
        // The underlying ChangePassword() will throw an exception rather
        // than return false in certain failure scenarios.
        try
        {
            return _provider.ChangeCorrespondencePassword(userName, oldPassword, newPassword);
        }
        catch (ArgumentException)
        {
            return false;
        }
        catch (MembershipPasswordException)
        {
            return false;
        }
    }

    public bool IsInRole(string userName, string roleName)
    {
        if (String.IsNullOrEmpty(roleName)) MemberAccount.Message = "Role name cannot be null or empty";
        if (String.IsNullOrEmpty(userName)) MemberAccount.Message = "User name cannot be null or empty";
        if (MemberAccount.Message.Trim().Length != 0)
            return false;
        try
        {
            return _roleProvider.IsUserInRole(userName, roleName);
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public bool IsInRole(string[] roleName)
    {
        if (roleName.Length == 0) MemberAccount.Message = "User name cannot be null or empty";
        if (MemberAccount.Message.Trim().Length != 0)
            return false;
        try
        {
            return _roleProvider.IsUserInRole(roleName);
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public bool IsInRole(string userName, string[] roleName)
    {
        foreach (string role in roleName)
        {
            if (String.IsNullOrEmpty(role)) MemberAccount.Message = "Role name cannot be null or empty";
        }
        if (String.IsNullOrEmpty(userName)) MemberAccount.Message = "User name cannot be null or empty";
        if (MemberAccount.Message.Trim().Length != 0)
            return false;
        try
        {
            return _roleProvider.IsUserInAnyRole(userName, roleName);
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    
    public string UserPassword(string username)
    {
        if (String.IsNullOrEmpty(username)) throw new ArgumentException("Value cannot be null or empty.", "userName");
        // The underlying ChangePassword() will throw an exception rather
        // than return false in certain failure scenarios.
        try
        {
            return _provider.UserPassword(username);
        }
        catch (ArgumentException)
        {
            return null;
        }
    }
    public string UserCorrespondencePassword(string username)
    {
        if (String.IsNullOrEmpty(username)) throw new ArgumentException("Value cannot be null or empty.", "userName");
        // The underlying ChangePassword() will throw an exception rather
        // than return false in certain failure scenarios.
        try
        {
            return _provider.UserCorrespondencePassword(username);
        }
        catch (ArgumentException)
        {
            return null;
        }
    }
    public string UserCorrespondencePassword(int UserAccountId)
    {
        if (UserAccountId <= 0) throw new ArgumentException("Value cannot be null or empty.", "userName");
        // The underlying ChangePassword() will throw an exception rather
        // than return false in certain failure scenarios.
        try
        {
            return _provider.UserCorrespondencePassword(UserAccountId);
        }
        catch (ArgumentException)
        {
            return null;
        }
    }



    public string UserPassword(int userAccountId)
    {
        if (userAccountId <= 0) throw new ArgumentException("Value cannot be null or empty.", "UserAccountId");
        // The underlying ChangePassword() will throw an exception rather
        // than return false in certain failure scenarios.
        try
        {
            return _provider.UserPassword(userAccountId);
        }
        catch (ArgumentException)
        {
            return null;
        }
    }
}