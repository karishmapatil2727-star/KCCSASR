using System;
using System.Web.Security;
using System.Web;
using System.Web.UI;

public class FormsAuthenticationService : IFormsAuthenticationService
{
    public const string SESSION_USER = "SessionUserAccount";

    public void SignIn(string userName, bool createPersistentCookie)
    {
        if (String.IsNullOrEmpty(userName))
            MemberAccount.Message = "Username can not be null or empty.";

        FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
    }

    public void SignOut()
    {
        try { FormsAuthentication.SignOut(); }
        catch { }
        try
        {
            HttpContext.Current.Session.RemoveAll(); HttpContext.Current.Session.Abandon();

            HttpContext.Current.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            HttpContext.Current.Response.Cookies[FormsAuthentication.FormsCookieName].Expires = DateTime.Now;
            HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }

            HttpContext.Current.Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", Guid.NewGuid().ToString()));
        }
        catch { }
    }
}