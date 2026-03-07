using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RoleMenuItem
/// </summary>
public class RoleMenuItem
{
    public RoleMenuItem() { }
    public RoleMenuItem(int roleMenuItemId, int RM_menuItemCode, int RM_roleID, bool RM_isCustom, DateTime RM_insertDate, int RM_insertUserAccountID, DateTime rm_StartDate, DateTime rm_EndDate, int rm_CustomUserAccountID)
    {
        RoleMenuItemId = roleMenuItemId;
        RM_MenuItemCode = RM_menuItemCode;
        RM_RoleID = RM_roleID;
        RM_IsCustom = RM_isCustom;
        RM_InsertDate = RM_insertDate;
        RM_InsertUserAccountID = RM_insertUserAccountID;
        RM_StartDate = rm_StartDate;
        RM_EndDate = rm_EndDate;
        RM_CustomUserAccountID = rm_CustomUserAccountID;
    }

    public virtual int RoleMenuItemId { get; set; }
    public virtual int RM_MenuItemCode { get; set; }
    public virtual int RM_RoleID { get; set; }
    public virtual bool RM_IsCustom { get; set; }
    public virtual DateTime RM_InsertDate { get; set; }
    public virtual int RM_InsertUserAccountID { get; set; }

    public virtual DateTime RM_StartDate { get; set; }
    public virtual DateTime RM_EndDate { get; set; }
    public virtual int RM_CustomUserAccountID { get; set; }
}