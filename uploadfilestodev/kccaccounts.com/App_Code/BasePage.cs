using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Collections.Specialized;
using UniversityDAL;
using System.Text;
using System.Reflection;
//using ExamNodalCentre;

/// <summary>
/// Summary description for BasePage
/// </summary>
public abstract class BasePage : Security
{
    public EntitiesModel model = new EntitiesModel();
    public const string SESSION_USER = "SessionUserAccount";
    private const string SESSION_MENU = "MenuItemTemp";
    public const string SESSION_SELECTED_ITEM = "SelectedItem";
    public const string VIEW_STATE_UPDATERECORD = "_UpdateRecord";
    public IFormsAuthenticationService FormsService { get; set; }
    public IMembershipService MembershipService { get; set; }
    public UserAccount userAccount { get; set; }

    #region Properities

    public WebUserAccount CurrentUser
    {
        get
        {
            bool IsLogin = false;
            if (User.Identity.IsAuthenticated && User.Identity.Name != "")
            {
                IsLogin = true;
            }
            return Globals.LoadUser(UserAccountId, IsLogin);
        }
    }

    public int UserAccountId
    {
        get
        {
            int userId = 1;
            if (User.Identity.IsAuthenticated && User.Identity.Name != "")
            {
                userId = Convert.ToInt32(User.Identity.Name);
            }
            return userId;
        }
    }
    public static WebUserAccount MobCurrentUser()
    {
        return (WebUserAccount)HttpContext.Current.Session[SESSION_USER];
    }

    public string AccountName
    {
        get
        {
            if (Session[ConfigurationManager.AppSettings["currentUser"]] == null)
            {
                Session[ConfigurationManager.AppSettings["currentUser"]] = CurrentUser.UA_AccountName;
            }
            return Session[ConfigurationManager.AppSettings["currentUser"]].ToString();
        }
    }

    public string DisplayName
    {
        get
        {
            return CurrentUser.UA_DisplayName;
        }
    }


    public string Display_InstName
    {
        get
        {
            return CurrentUser.InstName;
        }
    }

    public int FinYear
    {
        get
        {
            return 2;
        }
    }

    public Object UpdateRecord
    {
        get { return ViewState[VIEW_STATE_UPDATERECORD]; }
        set { ViewState[VIEW_STATE_UPDATERECORD] = value; }
    }

    #endregion
    public void SignOut()
    {
        // i think i can use set guest account login into a function//
        FormsService.SignOut();
        //HttpContext.Current.Session[System.Configuration.ConfigurationManager.AppSettings["currentUserAccountId"].ToString()] = null;
        Session[SESSION_USER] = null;
        Session[SESSION_MENU] = null;
        Response.Redirect("~/Default.aspx" + Security.EncryptQueryString("action=logout"));
    }
    public abstract int MenuCode();
    public static int MenuCodeTemp
    {
        get;
        set;
    }

    public abstract string[] AccessRoles();
    public string PageTitle
    {
        get
        {
            StringBuilder sb = new StringBuilder();
            if (Session[SESSION_MENU] != null)
            {
                DataRow[] dr = ((DataTable)Session[SESSION_MENU]).Select("MI_MenuItemCode =" + MenuCode());
                if (dr.Length > 0)
                {
                    sb.Append("<table cellpadding=\"0\" cellspacing=\"0\" style=\"width:100%;\">");
                    sb.Append("<tr>");
                    sb.Append("<th>");
                    sb.Append(dr[0]["MI_MenuItemName"].ToString());
                    sb.Append("</th>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td>");
                    sb.Append("<div>");
                    sb.Append(dr[0]["MenuDiscription"].ToString());
                    sb.Append("</div>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                }
            }
            return sb.ToString();
        }
    }
   
    public void BindMenu(ref RadMenu menu)
    {
        LoginLogs log = new LoginLogs();
        log.UserAccountID = ((BasePage)Page).UserAccountId;

        //try
        //{
        if (Session[SESSION_MENU] == null)
            Session[SESSION_MENU] = log.LoginMenuBinding();
        menu.DataSource = (DataTable)Session[SESSION_MENU];
        menu.DataFieldID = "MenuItemId";
        menu.DataFieldParentID = "MI_ParentMenuItemId";
        menu.DataNavigateUrlField = "MI_Url";
        menu.DataTextField = "MI_MenuItemName";
        menu.DataValueField = "MI_MenuItemCode";
        menu.DataBind();
        //}
        //catch (Exception ex)
        //{
        //    //Response.Redirect("~/", false);
        //    return;
        //}
    }

    protected override void OnInit(EventArgs e)
    {
        MemberAccount.UserAccountId = this.UserAccountId;
        MemberAccount.MenuCode = Convert.ToInt32(MenuCode());
        MemberAccount.AccountName = User.Identity.Name;
        MemberAccount.Message = String.Empty;
        if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
        if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
        if (userAccount == null) { userAccount = new UserAccount(); }

        if (!IsPostBack && MenuCodeTemp != MemberAccount.MenuCode)
        {
            int status = 0;

            if (!userAccount.IsInRole(ref status, User.Identity.IsAuthenticated && User.Identity.Name != null && User.Identity.Name != "", CurrentUser.UserRoles))
            {
                status = MemberAccount.MenuCode;
                //Response.Redirect("~/ErrorPage.aspx?Error=" + Request.RawUrl);
                Response.Redirect("~/ErrorPage.aspx?Error=" + status.ToString());
            }
            MenuCodeTemp = MenuCode();
        }

        base.OnInit(e);
    }

    private int intH = 0;

    public string Height(int row)
    {
        if (row == 1)
        {
            intH = (row * 80);
        }
        else if (row == 2)
        {
            intH = (row * 60);
        }
        else if (row == 3)
        {
            intH = (row * 50);
        }
        else if (row == 4)
        {
            intH = (row * 47);
        }
        else
        {
            intH = (row * 45);
        }
        return intH.ToString();
    }

    public bool ReadOnly { get; set; }

    public enum MessageType
    {
        Alert,
        Common,
        Empty,
        Error,
        NoStyle
    }

    public static NameValueCollection DecryptQueryString(string queryString)
    {
        return StringHelpers.DecryptQueryString(queryString);
    }
    public static string EncryptQueryString(NameValueCollection queryString)
    {
        return StringHelpers.EncryptQueryString(queryString);
    }


    public string GetValueStringByQueryString(string GetValueByQueryStringValue)
    {
        //Decrypt the query string
        NameValueCollection queryString = DecryptQueryString(Request.QueryString.ToString());

        if (queryString == null)
        {
            return "0";
        }
        else
        {
            //Check if the id was passed in.
            string id = queryString[GetValueByQueryStringValue];

            if ((id == null) || (id == "0"))
            {
                return "0";
            }
            else
            {
                return id;
            }
        }
    }

    //public static void Message(Control page, string msg)
    //{
    //    string myScript = String.Format("alert('{0}');", msg);

    //    ScriptManager.RegisterStartupScript(page, page.GetType(), "MyScript", myScript, true);
    //}

    //public static bool IsPasswordStrong(string password)
    //{
    //    bool IsTrue = false;
    //    int lowers = password.Count(p => char.IsLower(p));
    //    int uppers = password.Count(p => char.IsUpper(p));
    //    int digits = password.Count(p => char.IsDigit(p));
    //    if ((lowers >= 1) || (uppers >= 1) && (digits >= 1) && (password.Length >= 6) && (password.Length < 10))
    //    { IsTrue = true; }
    //    return IsTrue;
    //}

    public static bool IsPasswordStrong(string password)
    {
        int TotVale = 0;
        bool IsTrue = false;
        int lowers = password.Count(p => char.IsLower(p));
        int uppers = password.Count(p => char.IsUpper(p));
        int digits = password.Count(p => char.IsDigit(p));
        int specials = password.Count(p => !char.IsLetterOrDigit(p));
        TotVale=((lowers >= 3 ? 1 : 0) +
               (uppers >= 1 ? 1 : 0) +
               (digits >= 1 ? 1 : 0) +
               (specials >= 1 ? 1 : 0));
        if (TotVale > 3) 
        { IsTrue=true; }
        return IsTrue;
    }


    protected DataTable ConvertListToDataTable<T>(List<T> list)
    {
        DataTable dt = new DataTable();

        foreach (PropertyInfo info in typeof(T).GetProperties())
        {
            try
            {
                dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
            }
            catch
            {
                if (info.Name.Contains("Date"))
                    dt.Columns.Add(new DataColumn(info.Name, Type.GetType("System.DateTime")));
                else
                    dt.Columns.Add(new DataColumn(info.Name, Type.GetType("System.Boolean")));
            }
        }
        foreach (T t in list)
        {
            DataRow row = dt.NewRow();
            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                row[info.Name] = info.GetValue(t, null);
            }
            dt.Rows.Add(row);
        }
        return dt;
    }
}