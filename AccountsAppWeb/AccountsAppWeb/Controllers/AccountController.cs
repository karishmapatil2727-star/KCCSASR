using AccountsAppWeb.Core;
using AccountsAppWeb.Core.Models;
using System;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using System.Web;
using System.Collections;
using AccountsAppWeb.Infrastructure;
using NLog;

namespace AccountsAppWeb.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel loginModel, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(loginModel);

            LoginManager adminRepository = new LoginManager();
            var userModel = adminRepository.AuthenticateUser(loginModel.UserName, loginModel.Password);
            if (userModel.IsLoginSuccess)
            {
                FormsAuthentication.SetAuthCookie(loginModel.UserName, loginModel.RememberMe);
                string userData = JsonConvert.SerializeObject(userModel);
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, loginModel.UserName,
                    DateTime.Now, DateTime.Now.AddDays(30), true, userData, FormsAuthentication.FormsCookiePath);
                string encTicket = FormsAuthentication.Encrypt(ticket);
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                else
                {                                
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please try again, Username or Password are invalid!");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Logout()
        {
            foreach (DictionaryEntry dEntry in HttpContext.Cache)
            {
                HttpContext.Cache.Remove(dEntry.Key.ToString());
            }
            // Delete the user details from cache.
            Session.Abandon();
            // Delete the authentication ticket and sign out.
            FormsAuthentication.SignOut();
            // Clear authentication cookie.
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);
            return RedirectToAction("Login");
        }

        [LogonAuthorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ChangeCurrentPassword(string userName, string currentPassword, string newPassword)
        {
            LoginManager adminRepository = new LoginManager();
            var isSuccess = adminRepository.ChangePassword(userName, currentPassword, newPassword);
            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }

        [LogonAuthorize]
        public ActionResult ViewPassword()
        {
            return View();
        }
 
        public JsonResult GetPasswordDetails(string userName)
        {
            LoginManager adminRepository = new LoginManager();
            var userDetails = adminRepository.GetPasswordDetails(userName);
            return Json(userDetails, JsonRequestBehavior.AllowGet);
        }
    }
}
