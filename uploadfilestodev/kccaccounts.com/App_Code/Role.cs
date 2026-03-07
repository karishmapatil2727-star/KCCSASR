using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Data.Linq;

/// <summary>
/// Summary description for Role
/// </summary>
public class Role
{
    public Role() { }
    public Role(int roleId, string roleName, string roleShortName, bool isMaster, bool isActive, DateTime activeUptoDate, DateTime insertDate, int insertUserAccountId, DateTime updateDate, int updateUserAccountId, string remarks)
    {
        RoleId = roleId;
        R_RoleName = roleName;
        R_RoleShortName = roleShortName;
        R_IsMaster = isMaster;
        R_IsActive = isActive;
        R_ActiveUptoDate = activeUptoDate;
        R_InsertDate = insertDate;
        R_InsertUserAccountId = insertUserAccountId;
        R_UpdateDate = updateDate;
        R_UpdateUserAccountId = updateUserAccountId;
        R_Remarks = remarks;
    }

    public virtual int RoleId { get; set; }
    public virtual string R_RoleName { get; set; }
    public virtual string R_RoleShortName { get; set; }
    public virtual bool R_IsMaster { get; set; }
    public virtual bool R_IsActive { get; set; }
    public virtual DateTime R_ActiveUptoDate { get; set; }
    public virtual DateTime R_InsertDate { get; set; }
    public virtual int R_InsertUserAccountId { get; set; }
    public virtual DateTime R_UpdateDate { get; set; }
    public virtual int R_UpdateUserAccountId { get; set; }
    //public virtual Binary R_Version { get; set; }
    public virtual string R_Remarks { get; set; }

    //public virtual IList<RoleUserAccount> RoleUserAccounts { get; set; }
}