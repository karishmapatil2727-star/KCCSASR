using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Web.Mail;
using System.Configuration;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Specialized;
using System.Web.SessionState;
using System.Data.SqlClient;
using System.Web.Caching;
using System;
using System.Net.Mail;
using System.Net;
using GurpreetControls;
using System.Diagnostics;

public class Security : System.Web.UI.Page
{
    public int UserAccountID { get; set; }
    public string MenuItemCode { get; set; }

    LoginLogs log = new LoginLogs(); // Insert Logs /

    DataSet ds;
    DataTable dt;
    SqlParameter[] param;



    // this Create an Errors
    private string _IPAddress;
    public string IPAddress
    {
        get
        {
            try
            {
                if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString() == "")
                {
                    _IPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else
                {
                    _IPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                }
            }
            catch
            { _IPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString(); }
            return _IPAddress;
        }
        private set { }
    }



    private static byte[] key = { };
    private static byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
    private static string EncryptionKey = "!5623a#de";
    private static string EncryptionKeyCode = "gurpreetd";
    public string Decrypt(string Input)
    {
        Byte[] inputByteArray = new Byte[Input.Length];
        try
        {
            key = System.Text.Encoding.UTF8.GetBytes(EncryptionKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(Input);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            Encoding encoding = Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }
        catch (Exception ex)
        {
            return "";
        }
    }


    // i copied this code
    public string Encrypt(string Input)
    {
        try
        {
            key = System.Text.Encoding.UTF8.GetBytes(EncryptionKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            Byte[] inputByteArray = Encoding.UTF8.GetBytes(Input);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }
        catch (Exception ex)
        {
            return "";
        }
    }



    public string MessageMobileNo { get; set; }
    public string MessageBody { get; set; }
    public int MessageSenderUserId { get; set; }
    public string username
    {
        get
        {
            return "gsmental";
        }
    }
    public string password
    {
        get
        {
            return "90926674";
        }
    }
    public string domain
    {
        get
        {
            //return "67.23.229.95";
            return "199.189.250.157";
        }
    }

    public string senderid
    {
        get
        {
            return "ALERTS";
        }
    }

    public string SenderName { get; set; }

    private string SendSMS(string strUrl)
    {
        //string strUrl = "http://67.23.229.95/smsclient/api.php?username=username&password=password&source=senderid&dmobile=919815005454&message=message";
        //WebRequest request = HttpWebRequest.Create(strUrl);
        //HttpWebResponse response = (HttpWebResponse)request.EndGetResponse();
        //Stream s = (Stream)response.GetResponseStream();
        //StreamReader readStream = new StreamReader(s);
        //string dataString = readStream.ReadToEnd();
        //response.Close();
        //s.Close();
        //readStream.Close();
        return "";
    }

    ///<summary>
    ///Send text message to 1st API. It is Enable.
    ///</summary>
    ///<param name="message"></param>
    ///<param name="mobileno"></param>
    public bool SendMessage()
    {
        try
        {
            string result = apicall("http://" + domain + "/smsclient/api.php?username=" + username + "&password=" + password + "&source=" + senderid + "&dmobile=" + MessageMobileNo + "&message=" + MessageBody);
            if (!result.StartsWith("Wrong Username or Password"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }

        //ValidationErrors validationErrors = new ValidationErrors();
        //TextMessageEO textMessageEO = new TextMessageEO();
        //textMessageEO.Load(0);

        //TextMessageEO textMessage = textMessageEO;
        //LoadObjectFromScreen(textMessage, MessageBody, MessageMobileNo, MessageSenderUserId, IsSent);
        //if (textMessage.Save(ref validationErrors, MessageSenderUserId))
        //{

        //}

        return false;
    }

    private string apicall(string url)
    {
        HttpWebRequest httpreq = (HttpWebRequest)WebRequest.Create(url);
        try
        {
            HttpWebResponse httpres = (HttpWebResponse)httpreq.GetResponse();
            StreamReader sr = new StreamReader(httpres.GetResponseStream());
            string results = sr.ReadToEnd();
            sr.Close();
            return results;
        }
        catch
        {
            return "0";
        }
    }


    /// <summary>
    /// You must pass in a string that uses the QueryStringHelper.DELIMITER as the delimiter.
    /// This will also append the "?" to the beginning of the query string.
    /// </summary>
    /// <param name="queryString"></param>
    /// <returns></returns>
    public static string EncryptQueryString(string queryString)
    {
        return StringHelpers.EncryptQueryString(queryString);
    }

    /// <summary>
    /// Function to check or compare file size of uploading file
    /// </summary>
    /// <param name="IsValid">It will return true if file size is less than parameter fileSize</param>
    /// <param name="fileUploader">Provide file uploader ID</param>
    /// <param name="fileSize">Enter approving file size in interger in kilobyte(KB). It will be compared with file's size.</param>
    public void FileSizeValid(ref bool isValid, FileUpload fileUploader, int fileSize, ref int returnFileSize)
    {
        if ((fileUploader.PostedFile.ContentLength / 1024) < fileSize)
        {
            isValid = true;
        }
        else
        {
            isValid = false;
        }
        returnFileSize = (fileUploader.PostedFile.ContentLength / 1024);
    }

    public int GetIdByQueryString(string GetIdByQueryStringValue)
    {
        //Decrypt the query string
        NameValueCollection queryString = DecryptQueryString(Request.QueryString.ToString());

        if (queryString == null)
        {
            return 0;
        }
        else
        {
            //Check if the id was passed in.
            string id = queryString[GetIdByQueryStringValue];

            if ((id == null) || (id == "0"))
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(id);
            }
        }
    }

    public static NameValueCollection DecryptQueryString(string queryString)
    {
        return StringHelpers.DecryptQueryString(queryString);
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
}