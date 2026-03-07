using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for WebUserAccount
/// </summary>
public class WebUserAccount
{
    public WebUserAccount()
    {
    }
    public int UserAccountId { get; set; }
    public int ID { get; set; }
    public string UA_AccountName { get; set; }
    public string UA_DisplayName { get; set; }
    public DateTime UA_LastLoginDate { get; set; }
    public DateTime UA_SecondLastLoginDate { get; set; }
    public DateTime UA_LastPasswordChargedDate { get; set; }
    public string UA_UserType { get; set; }
    public int UA_FacUserId { get; set; }
    public int UA_InstId { get; set; }
    public string InstShortname { get; set; }
    public int RoleId { get; set; }
    public int[] UserRoles { get; set; }
    public short FinId { get; set; }
    public int DepartmentID { get; set; }
    public int StudentBranchId { get; set; }
    public int StudentSemId { get; set; }
    public string InstName { get; set; }
    public int UA_InstType { get; set; }
    public int LeaveSessionId { get; set; }
    
}