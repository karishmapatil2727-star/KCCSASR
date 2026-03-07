using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CollegeDatabase;

public class apivBranchController : ApiController
{
    private static WebUserAccount ApiCurrentUser(string UserAccountId)
    {
        DataContext dc = new DataContext();
        WebUserAccount user = dc.LoadWebUser(Convert.ToInt32(UserAccountId));
        return user;
    }


    [HttpGet]
    public object vBranch()
    {
        return UniversalClass.vBranch(ApiCurrentUser(User.Identity.Name).UA_InstId);
    }
}

public class apiMasterSemestersController : ApiController
{
    [HttpGet]
    public object MasterSemesters()
    {
        return UniversalClass.MasterSemesters();
    }
}

public class apiMasterSectionsController : ApiController
{
    [HttpGet]
    public object MasterSections()
    {
        return UniversalClass.MasterSections();
    }
}


public class apiMasterFinancialYearController : ApiController
{
    [HttpGet]
    public object MasterFinancialYear()
    {
        return UniversalClass.MasterFinancialYear();
    }
}





