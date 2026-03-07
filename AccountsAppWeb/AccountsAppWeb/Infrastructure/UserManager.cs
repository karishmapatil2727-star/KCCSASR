using AccountsAppWeb.Core.Models;
using System.Web;

namespace AccountsAppWeb.Infrastructure
{
    public class UserManager
    {
        /// <summary>
        /// Returns the User from the Context.User.Identity by decrypting the forms auth ticket and returning the user object.
        /// </summary>
        public static UserModel User
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    // The user is authenticated. Return the user from the forms auth ticket.
                    return ((MyPrincipal)(HttpContext.Current.User)).User;
                }
                else if (HttpContext.Current.Items.Contains("User"))
                {
                    // The user is not authenticated, but has successfully logged in.
                    return (UserModel)HttpContext.Current.Items["User"];
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
