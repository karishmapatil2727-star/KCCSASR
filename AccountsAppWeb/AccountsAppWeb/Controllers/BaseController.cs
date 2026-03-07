using AccountsAppWeb.Core;
using AccountsAppWeb.Core.Models;
using AccountsAppWeb.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AccountsAppWeb.Controllers
{
    [LogonAuthorize]
    public class BaseController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }
        public JsonResult ChangeFinancialyear(int yearId, DateTime startDate, DateTime endDate, string finTitle)
        {
            HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(cookie.Value);
                var identity = new FormsIdentity(authTicket);

                // Get the custom user data encrypted in the ticket.
                string userData = identity.Ticket.UserData;
                var userModel = JsonConvert.DeserializeObject<UserModel>(userData);
                if (UserManager.User != null)
                {
                    AdminManager adminManager = new AdminManager();
                    var finYears = adminManager.GetFinancialyear(UserManager.User.InstituteId);
                    var currentYear = finYears.Where(x => x.Fin_ID == yearId).FirstOrDefault();
                    userModel.IsNewLedgerAddAllow = currentYear.BI_IsNewLedgerAddAllow;
                    userModel.IsOpeningBalanceEditAllow = currentYear.BI_IsOpeningBalanceEditAllow;
                    userModel.IsTransactionAddAllow = currentYear.BI_IsTransactionAddAllow;
                    userModel.IsTransactionEditAllow = currentYear.BI_IsTransactionEditAllow;
                }
                int sub1 = Convert.ToInt32(finTitle.Substring(0, 4));
                int sub2 = Convert.ToInt32(finTitle.Substring(5));
                string str = string.Empty;

                for (int i = 1; i <= 4; i++)
                {
                    sub1 = sub1 - 1;
                    sub2 = sub2 - 1;

                    switch (i) {
                     case 1:
                            userModel.FinTitle1 = sub1.ToString()+"-" +sub2.ToString();
                             break;
                        case 2:
                            userModel.FinTitle2 = sub1.ToString() + "-" + sub2.ToString();
                            break;
                        case 3:
                            userModel.FinTitle3 = sub1.ToString() + "-" + sub2.ToString();
                            break;
                        case 4:
                            userModel.FinTitle4 = sub1.ToString() + "-" + sub2.ToString();
                            break;
                    }

                }
                    
                userModel.FinancialYearId = yearId;
                userModel.FinancialYearStartDate = startDate;
                userModel.FinancialYearEndDate = endDate;
                userModel.FinTitle = finTitle;               
                UpdateAuthCookie(userModel);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public void UpdateAuthCookie(UserModel userModel)
        {
            HttpCookie cookie = FormsAuthentication.GetAuthCookie(FormsAuthentication.FormsCookieName, true);
            if (cookie != null)
            {
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                var identity = new GenericIdentity(ticket.Name, "Forms");
                var principal = new MyPrincipal(identity);

                var updatedData = JsonConvert.SerializeObject(userModel);
                var newticket = new FormsAuthenticationTicket(ticket.Version,
                                                              ticket.Name,
                                                              ticket.IssueDate,
                                                              ticket.Expiration,
                                                              true, //persistent 
                                                              updatedData,
                                                              ticket.CookiePath);

                cookie.Value = FormsAuthentication.Encrypt(newticket);
                cookie.Expires = newticket.Expiration.AddDays(30);
                HttpContext.Response.Cookies.Set(cookie);
            }
        }

        public JsonResult GetUserData()
        {
            var response = JsonConvert.SerializeObject(UserManager.User);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
