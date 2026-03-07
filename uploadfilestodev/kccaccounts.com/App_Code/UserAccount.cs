using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Data.Linq;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Web.UI;

/// <summary>
/// Summary description for User
/// </summary>
[Serializable]
public class UserAccount //: IPrincipal
{
    private const string SESSION_ROLE = "UserRole";
    public UserAccount()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public UserAccount(int userId, string userName, string password, string passwordHint, string emailAddress, IIdentity identity, string[] roles, DateTime lastLoginDate, DateTime secondLastLoginDate, string passwordRecoveryCode, string securityCode, string actionCode, int forgotPasswordAttempt, DateTime lastPasswordChargedDate, DateTime suspendFromDate, DateTime suspendToDate, bool isDetainedCompletely, int centreId, string examType, string userType, bool isActive, DateTime insertDate, int insertUserAccountId, DateTime updateDate, int updateUserAccountId, string CorrespondencePassword)
    {
        UserAccountId = userId;
        UA_AccountName = userName;
        UA_AccountPassword = password;
        UA_AccountPasswordHint = passwordHint;
        UA_EmailAddress = emailAddress;

        _identity = identity;
        _roles = new string[roles.Length];
        roles.CopyTo(_roles, 0);
        Array.Sort(_roles);

        UA_LastLoginDate = lastLoginDate;
        UA_SecondLastLoginDate = secondLastLoginDate;
        UA_PasswordRecoveryCode = passwordRecoveryCode;
        UA_SecurityCode = securityCode;
        UA_ActionCode = actionCode;
        UA_ForgotPasswordAttempt = forgotPasswordAttempt;
        UA_LastPasswordChargedDate = lastPasswordChargedDate;
        UA_SuspendFromDate = suspendFromDate;
        UA_SuspendToDate = suspendToDate;
        UA_IsDetainedCompletely = isDetainedCompletely;
        UA_CentreId = centreId;
        UA_ExamType = examType;
        UA_UserType = userType;
        UA_IsActive = isActive;
        UA_InsertDate = insertDate;
        UA_InsertUserAccountId = insertUserAccountId;
        UA_UpdateDate = updateDate;
        UA_UpdateUserAccountId = updateUserAccountId;
        UA_CorrespondencePassword = CorrespondencePassword;
    }

    public virtual int UserAccountId { get; set; }
    public virtual string UA_AccountName { get; set; }
    public virtual string UA_AccountPassword { get; set; }
    public virtual string UA_AccountPasswordHint { get; set; }
    public virtual string UA_EmailAddress { get; set; }
    public virtual DateTime UA_LastLoginDate { get; set; }
    public virtual DateTime UA_SecondLastLoginDate { get; set; }
    public virtual string UA_PasswordRecoveryCode { get; set; }
    public virtual string UA_SecurityCode { get; set; }
    public virtual string UA_ActionCode { get; set; }
    public virtual int UA_ForgotPasswordAttempt { get; set; }
    public virtual DateTime UA_LastPasswordChargedDate { get; set; }
    public virtual DateTime UA_SuspendFromDate { get; set; }
    public virtual DateTime UA_SuspendToDate { get; set; }
    public virtual bool UA_IsDetainedCompletely { get; set; }
    public virtual int UA_CentreId { get; set; }
    public virtual string UA_ExamType { get; set; }
    public virtual string UA_UserType { get; set; }
    public virtual bool UA_IsActive { get; set; }
    public virtual DateTime UA_InsertDate { get; set; }
    public virtual int UA_InsertUserAccountId { get; set; }
    public virtual DateTime UA_UpdateDate { get; set; }
    public virtual int UA_UpdateUserAccountId { get; set; }
    public virtual string UA_CorrespondencePassword { get; set; }
   // public virtual Binary UA_Version { get; set; }

    private DataSet ds;
    private IIdentity _identity;
    public string[] _roles;

    public IIdentity Identity
    {
        get
        {
            return _identity;
        }
    }

    public string[] Roles
    {
        get
        {
            return (string[])HttpContext.Current.Session[SESSION_ROLE];
        }
        set
        {
            HttpContext.Current.Session[SESSION_ROLE] = value;
            HttpContext.Current.Session.Timeout = 60;
        }
    }
    private string xmlPath = HttpContext.Current.Server.MapPath("~/Z1H2J3F4Q6AQ9S9B8S7S6D/MenuItemCustom.xml");
    public DataSet CustomMenu
    {
        get
        {
            ds = new DataSet();
            FileInfo file = new FileInfo(xmlPath);
            if (file.Exists)
            {
                ds.ReadXml(xmlPath);
                return ds;
            }
            return null;
        }
    }

    protected void LoadRoles()
    {
        using (DataContext db = new DataContext())
        {
            Roles = db.LoadRole(MemberAccount.UserAccountId);
        }
    }


    //protected void LoadRolesWithDate()
    //{
    //    using (DataContext db = new DataContext())
    //    {
    //        Roles = db.LoadRoleWithDate(MemberAccount.UserAccountId,MemberAccount.MenuCode);
    //    }
    //}


    //public bool IsInRole(string role)
    //{
    //    bool IsRoleExists = false;
    //    if (Roles == null || Roles.Count() <= 0)
    //        LoadRoles();
    //    IsRoleExists = Array.BinarySearch(Roles, role) >= 0 ? true : false;

    //    if (!IsRoleExists && IsMenuExists(MemberAccount.UserName, MemberAccount.MenuCode))
    //    {
    //        IsRoleExists = true;
    //    }
    //    return IsRoleExists;
    //}

    public bool IsInRole(string[] roles)
    {
        int status=0;
        return IsRoleExist(roles,ref status);
    }

    public bool IsInRole(ref int status, bool loggedIn, params int[] roles)
    {
        //int IsInRoleBool = 0;
        //bool IsRoleExists = false;
        if (MemberAccount.MenuCode >= 1000 && MemberAccount.MenuCode <= 2000)
        {
            if (MemberAccount.MenuCode > 1400)
            {
                return false;
            }
            //Public Pages
            return true;
        }
        else if (MemberAccount.MenuCode > 2000 && MemberAccount.MenuCode <= 3000)
        {
            //Authenticated Pages
            return loggedIn == true;
        }
        else
        {
            return (from r in Globals.RoleMenuItemList()
                    where r.RM_MenuItemCode == MemberAccount.MenuCode
                    && r.RM_StartDate.Date <= DateTime.Today.Date && r.RM_EndDate.Date >= DateTime.Today.Date
                    && ((r.RM_IsCustom == false && roles.Contains(r.RM_RoleID) && r.RM_CustomUserAccountID == 0)
                    || (r.RM_IsCustom == true && r.RM_CustomUserAccountID == MemberAccount.UserAccountId && r.RM_RoleID == 0))
                    select new
                    {
                        r.RoleMenuItemId
                    }).ToList().Count > 0;
        }
        //else if (MemberAccount.MenuCode >= 3000 && MemberAccount.MenuCode <= 4000)
        //{
        //    //Role Base Without Date or Time Limit
        //    if (Roles == null || Roles.Count() <= 0)
        //        LoadRoles();
            
        //    return IsRoleExist(roles, ref status);

        //}
        //else if (MemberAccount.MenuCode >= 4000 && MemberAccount.MenuCode <= 5000)
        //{
        //    using (DataContext db = new DataContext())
        //    {
        //        return db.LoadRoleWithDate(MemberAccount.UserAccountId, MemberAccount.MenuCode);
        //    }
        //}
        //else if (MemberAccount.MenuCode >= 5000 && MemberAccount.MenuCode <= 6000)
        //{
        //    //Custom Pages assigned to User
        //    //return IsMenuExists(MemberAccount.UserAccountId, MemberAccount.MenuCode);

        //    using (DataContext db = new DataContext())
        //    {
        //        return db.LoadCustomMenu(MemberAccount.UserAccountId, MemberAccount.MenuCode);
        //    }
        //}
        //else if (MemberAccount.MenuCode >= 6000 && MemberAccount.MenuCode <= 7000)
        //{
        //    //Secure Pages need to check security
        //    return IsRoleExists == true;
        //}
        //else if (MemberAccount.MenuCode >= 7000 && MemberAccount.MenuCode <= 8000)
        //{
        //    return IsRoleExists == true;
        //}
        //else if (MemberAccount.MenuCode >= 8000 && MemberAccount.MenuCode <= 9000)
        //{
        //    return IsRoleExists == true;
        //}
        //else if (MemberAccount.MenuCode >= 9000 && MemberAccount.MenuCode <= 10000)
        //{
        //    return IsRoleExists == true;
        //}
        //else
        //{
        //    return IsRoleExists == true;
        //}
    }

    protected bool IsRoleExist(string[] roles, ref int status )
    {
        foreach (string searchrole in roles)
        {
            foreach (string role in Roles)
            {
                if (role == searchrole)
                    status = 0;
                    return true;
            }
        }
        status = 102;
        return false;
    }

    protected bool IsMenuExists(int UserAccountID, int menuCode)
    {
        if (CustomMenu != null)
        {
            if (CustomMenu.Tables.Count > 0)
            {
                DataRow[] dr = CustomMenu.Tables[0].Select("UserName = '" + UserAccountID.ToString() + "' AND MenuItemId = " + menuCode);
                return dr.Length > 0;
            }
        }
        return false;
    }
}