using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;

/// <summary>
/// Summary description for MiscFunc
/// </summary>
public class MiscFunc
{
    public MiscFunc()
    {
    }
    public DataTable udtt_dtBulk4Import(DataTable dt, string String1, string String2, string String3, string String4, string String5)
    {
        if (dt == null)
        {
            dt = mydtBulk4Import();
        }

        DataRow dr;
        dr = dt.NewRow();
        dr["String1"] = String1;
        dr["String2"] = String2;
        dr["String3"] = String3;
        dr["String4"] = String4;
        dr["String5"] = String5;
        dt.Rows.Add(dr);
        return dt;
    }

    private DataTable mydtBulk4Import()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("String1", Type.GetType("System.String"));
        dt.Columns.Add("String2", Type.GetType("System.String"));
        dt.Columns.Add("String3", Type.GetType("System.String"));
        dt.Columns.Add("String4", Type.GetType("System.String"));
        dt.Columns.Add("String5", Type.GetType("System.String"));
        return dt;
    }
}


public static class ClientMsg
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="page"></param>
    //<param name="MessageType">s for Success, e for Error, w for Warning, i for Information </param>
    /// <param name="Msg"></param>
    /// <param name="Title"></param>

    static public void toastrMessage(Control page, string messageType, string Msg, string Title = null)
    {
        messageType = messageType.ToLower();
        if (messageType != "e" && messageType != "w" && messageType != "s") { messageType = "i"; }
        //string myScript = String.Format("alert('{0}');", msg);
        //string myScript = String.Format("notification('w', 'Hellow this is gudffsdfdrpreet Singh Menntal"+DateTime.Now.ToString()+"' ,'Warning')");

    
        string myScript = String.Format("notification('{0}','{1}','{2}');", messageType, Msg, Title);
        ScriptManager.RegisterStartupScript(page, page.GetType(),
          "MyScript", myScript, true);
    }

}