using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Services;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Configuration;
using DeveloperUtilz;
using CollegeDatabase;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class Attendance : System.Web.Services.WebService
{
   
    Leave lv = new Leave();
    DataSet ds = new DataSet();
    
    [WebMethod]
    public string ImportAttendance(int USERID, DateTime Date, string IN1, string OUT1, string IN2, string OUT2, string WorkingHours, byte[] INIMAGE1, byte[] INIMAGE2, byte[] OUTIMAGE1, byte[] OUTIMAGE2, int InsertUserAccountId)
    {

        DataSet ds = new DataSet();
        int FUSERID = Convert.ToInt32(USERID);

        if (Date.ToString() == "")
        {
            Date = DateTime.Now;
        }
        string INA = IN1;
        string OUTA = OUT1;
        string INB = IN2;
        string OUTB = OUT2;
        string MyImgIn1 = null, MyImgIn2 = null, MyImgOut1 = null, MyImgOut2 = null;
       
        string WorkingHour = WorkingHours;
     

        if (INIMAGE1 != null)
        {
            MyImgIn1 = BytesToIMage(INIMAGE1);
               
        }
         if (INIMAGE2 != null)
        {   MyImgIn2 = BytesToIMage(INIMAGE2);
               
        }
         if (OUTIMAGE1 != null)
        {  MyImgOut1=BytesToIMage(OUTIMAGE1);
               
        }
         if (OUTIMAGE2 != null)
        {   MyImgOut2=BytesToIMage(OUTIMAGE2);
        }
         ds = lv.ImportAttendanceFromBioTapNoImage(FUSERID, Date, INA, OUTA, INB, OUTB, WorkingHour, MyImgIn1, MyImgIn2, MyImgOut1, MyImgOut2, InsertUserAccountId);
        return "true";
    }

    private string BytesToIMage(byte[] Img)
    {
        string ImageSave = Guid.NewGuid().ToString()+ ".jpg";

        string targetFile = Server.MapPath("~/Upload/BioMachinePhoto/" + ImageSave);
        System.IO.File.WriteAllBytes(targetFile, Img);
        return ImageSave;
    }
   
}
