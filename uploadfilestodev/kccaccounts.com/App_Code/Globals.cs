using System.Web;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.EnterpriseServices;
using System;

/// <summary>
/// Summary description for Globals
/// </summary>
public static class Globals
{
    #region Constants

    static AbsoluteTime _AbsoulteTime = new AbsoluteTime(DateTime.Now.AddMonths(6));
    public const string SESSION_USER = "SessionUserAccount";
    public const string CACHE_ROLEMENUITEM = "CacheRoleMenuItem";

    #endregion Constants

    public static WebUserAccount LoadUser(int userAccountId, bool IsLogin)
    {
        if (HttpContext.Current.Session[SESSION_USER] == null || !IsLogin)
        {
            DataContext db = new DataContext();
            HttpContext.Current.Session[SESSION_USER] = db.LoadWebUser(userAccountId);
        }
        return (WebUserAccount)HttpContext.Current.Session[SESSION_USER];
    }

    public static WebUserAccount LoadUserStatic()
    {
        return (WebUserAccount)HttpContext.Current.Session[SESSION_USER];
    }

    public static List<UniversityDAL.RoleMenuItem> RoleMenuItemList()
    {
        ICacheManager _objCacheManager = CacheFactory.GetCacheManager();
        // Check If Key is in Cache Collection
        if (!_objCacheManager.Contains(CACHE_ROLEMENUITEM))
        {
            using (DataContext context = new DataContext())
            {
                _objCacheManager.Add(CACHE_ROLEMENUITEM, context.GetRoleMenuItem(), CacheItemPriority.Normal, null, _AbsoulteTime);
            }
        }
        return (List<UniversityDAL.RoleMenuItem>)_objCacheManager.GetData(CACHE_ROLEMENUITEM);
    }

    //Remove all cache
    public static void RemoveAll()
    {
        ICacheManager _objCacheManager = CacheFactory.GetCacheManager();
        _objCacheManager.Flush();
    }

    //Remove cache by name
    public static bool Remove(string name)
    {
        ICacheManager _objCacheManager = CacheFactory.GetCacheManager();
        if (_objCacheManager.Contains(name))
        {
            _objCacheManager.Remove(name);
            return true;
        }
        return false;
    }
}