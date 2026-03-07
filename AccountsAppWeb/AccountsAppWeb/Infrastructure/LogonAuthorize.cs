using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AccountsAppWeb.Infrastructure
{
    public sealed class LogonAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                                     filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);

            // If the method did not exclusively opt-out of security (via the AllowAnonmousAttribute), then check for an authentication ticket.
            if (!skipAuthorization)
            {
                base.OnAuthorization(filterContext);
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.Request.IsAuthenticated)
                return false;
            return true;
        }

        //Called when access is denied	        
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                filterContext.Result = new HttpStatusCodeResult(403, "Sorry, you do not have the required permission to perform this action.");

            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
            }               //User is logged in but has no access	            

        }
    }
}
