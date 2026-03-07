using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MenuItemCustom
/// </summary>
public class MenuItemCustom
{
    public MenuItemCustom() { }

    public MenuItemCustom(int menuItemCustomId, string userName, int menuItemId, bool isTimeLimit, DateTime startTime, DateTime endTime, bool isActive, DateTime insertDate, int insertUserAccountId, DateTime updateDate, int updateUserAccountId)
    {
        MenuItemCustomId = menuItemCustomId;
        UserName = userName;
        MenuItemId = menuItemId;
        IsTimeLimit = isTimeLimit;
        StartTime = startTime;
        EndTime = endTime;
        IsActive = isActive;
        InsertDate = insertDate;
        InsertUserAccountId = insertUserAccountId;
        UpdateDate = updateDate;
        UpdateUserAccountId = updateUserAccountId;
    }

    public virtual int MenuItemCustomId { get; set; }
    public virtual string UserName { get; set; }
    public virtual int MenuItemId { get; set; }
    public virtual bool IsTimeLimit { get; set; }
    public virtual DateTime StartTime { get; set; }
    public virtual DateTime EndTime { get; set; }
    public virtual bool IsActive { get; set; }
    public virtual DateTime InsertDate { get; set; }
    public virtual int InsertUserAccountId { get; set; }
    public virtual DateTime UpdateDate { get; set; }
    public virtual int UpdateUserAccountId { get; set; }
}