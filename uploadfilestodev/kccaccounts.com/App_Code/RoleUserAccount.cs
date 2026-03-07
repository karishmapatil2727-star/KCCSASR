using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;

/// <summary>
/// Summary description for RoleUserAccount
/// </summary>
public class RoleUserAccount
{
    public RoleUserAccount() { }

    public RoleUserAccount(int roleUserAccountId, int roleId, int userAccountId, DateTime insertDate, int insertUserAccountId, DateTime updateDate, int updateUserAccountId)
    {
        RoleUserAccountId = roleUserAccountId;
        RUA_RoleId = roleId;
        RUA_UserAccountId = userAccountId;
        RUA_InsertDate = insertDate;
        RUA_InsertUserAccountId = insertUserAccountId;
        RUA_UpdateDate = updateDate;
        RUA_UpdateUserAccountId = updateUserAccountId;
    }

    public virtual int RoleUserAccountId { get; set; }
    public virtual int RUA_RoleId { get; set; }
    public virtual int RUA_UserAccountId { get; set; }
    public virtual DateTime RUA_InsertDate { get; set; }
    public virtual int RUA_InsertUserAccountId { get; set; }
    public virtual DateTime RUA_UpdateDate { get; set; }
    public virtual int RUA_UpdateUserAccountId { get; set; }
    //public virtual Binary RUA_Version { get; set; }
}