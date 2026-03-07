using AccountsAppWeb.Core.Models;
using System.Security.Principal;

namespace AccountsAppWeb.Infrastructure
{
    public class MyPrincipal : IPrincipal
    {
        public MyPrincipal(IIdentity identity)
        {
            Identity = identity;
        }
        public IIdentity Identity
        {
            get;
            private set;
        }
        public UserModel User { get; set; }

        public bool IsInRole(string role)
        {
            return true;
        }
    }


}