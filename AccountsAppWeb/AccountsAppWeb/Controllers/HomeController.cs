using AccountsAppWeb.Core;
using AccountsAppWeb.Infrastructure;
using System.Web.Mvc;

namespace AccountsAppWeb.Controllers
{
    [LogonAuthorize]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
