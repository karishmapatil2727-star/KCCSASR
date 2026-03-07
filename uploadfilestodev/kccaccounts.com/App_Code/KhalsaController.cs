using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CollegeDatabase;
using System.Web.Script.Serialization;
public class KhalsaController : ApiController
{

    FeeClass fee=new FeeClass ();

    //// GET api/<controller>

  
    
    //public List<MyBrach> Ge11t(int Instid)
    //{
    //    KhalsaCollegeEntities db = new KhalsaCollegeEntities();


    //    List<MyBrach> result = (from fa in db.FeeHeadBranches
    //                join fb in db.FeeHeaderAmountMasters on fa.FeeHeaderAmountMasterId equals fb.FeeHeaderAmountMasterId
    //                join fbe in db.Branches on fa.BranchId equals fbe.BR_ID
    //                where fa.InstitutionId == Instid
    //                orderby fa.Batch, fa.SemesterId, fbe.BR_TITLE
    //                select new MyBrach
    //                {
    //                   Batch=  fa.Batch,
    //                  SemesterId=  fa.SemesterId,
    //                BR_TITLE=    fbe.BR_TITLE,
    //                 Title=   fb.Title,
    //                  TotalPayable=  fb.TotalPayable
    //                }).ToList();
    //   return result;


      
    //            //List<FacultyList1> result = (from f in db.Faculty_Infos
    //            //                             join fd in db.Faculty_Desgs on f.Fac_FDID equals fd.FD_ID
    //            //                             where f.Fac_InstID == CentreId && !db.ExamDutyDetails.Any(t2 => t2.Edd_FacUserID == f.Fac_UserID && t2.Edd_EdID == EDId)
    //            //                             orderby fd.FD_Title, f.Fac_UserID
                                            
    //            //                             select new FacultyList1 { Fac_Name = f.Fac_Name, Fac_UserID = Convert.ToInt32(f.Fac_UserID), FD_Title = fd.FD_Title }).ToList();
    //            //return result;
            



    //}

    //public class MyBrach
    //{
    //    public int Batch { get; set; }
    //     public int SemesterId { get; set; }
    //     public int TotalPayable { get; set; }
    //     public string BR_TITLE { get; set; }
    //     public string Title { get; set; }
    //}
       


    //// GET api/<controller>
    //public IEnumerable<string> Get()
    //{
    //    return new string[] { "value1", "value2" };
    //}

    //// GET api/<controller>/5
    //public string Get(int id)
    //{
    //    return "value";
    //}

    //// POST api/<controller>
    //public void Post([FromBody]string value)
    //{
    //}

    //// PUT api/<controller>/5
    //public void Put(int id, [FromBody]string value)
    //{
    //}

    //// DELETE api/<controller>/5
    //public void Delete(int id)
    //{
    //}
}
