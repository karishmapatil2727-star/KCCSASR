using System.Web.Security;
using System;

public interface IMembershipService
{
    int MinPasswordLength { get; }
    bool ValidateUser(ref int userId,string userName, string password);
    bool ValidateUserForCorrespondenceAuthentication(ref int userId, int UserAccountId, string password);
    MembershipCreateStatus CreateUser(int userId, string userName, string password, string passwordHint, string email, DateTime lastLoginDate, DateTime secondLastLoginDate, string passwordRecoveryCode, string securityCode, string actionCode, int forgotPasswordAttempt, DateTime lastPasswordChargedDate, DateTime suspendFromDate, DateTime suspendToDate, bool isDetainedCompletely, int centreId, string examType, string userType, bool isActive, DateTime insertDate, int insertUserAccountId, DateTime updateDate, int updateUserAccountId);
    MembershipCreateStatus UpdateUser(int userId, string userName, string email, bool isActive, DateTime updateDate, int updateUserAccountId);
    bool ChangePassword(string userName, string oldPassword, string newPassword, string passwordHint);
    bool ChangeCorrespondencePassword(string userName, string oldPassword, string newPassword);
    bool IsInRole(string userName, string roleName);
    bool IsInRole(string userName, string[] roleName);
    bool IsInRole(string[] roleName);
    string UserPassword(string username);
    string UserCorrespondencePassword(string username);
    string UserCorrespondencePassword(int UserAccountId);
    string UserPassword(int userId);
}