using System.ComponentModel.DataAnnotations;

namespace AccountsAppWeb.Core.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter user name")]
        [MinLength(3, ErrorMessage = "Please enter valid user name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter password")]
        [MinLength(3, ErrorMessage = "Please enter valid password")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string RedirectUrl { get; set; }
    }
}
