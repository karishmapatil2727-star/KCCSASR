using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for User
/// </summary>
public static class MemberAccount
{
    static int _AccountId;
    public static int UserAccountId
    {
        get
        {
            return _AccountId;
        }
        set
        {
            _AccountId = value;
        }
    }

    static string _AccountName;
    public static string AccountName
    {
        get
        {
            return _AccountName;
        }
        set
        {
            _AccountName = value;
        }
    }

    static int _MenuCode;
    public static int MenuCode
    {
        get
        {
            return _MenuCode;
        }
        set
        {
            _MenuCode = value;
        }
    }

    static string _Message;
    public static string Message
    {
        get
        {
            return _Message;
        }
        set
        {
            _Message = value;
        }
    }
}