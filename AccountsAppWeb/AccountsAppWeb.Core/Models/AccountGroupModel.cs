using System.ComponentModel.DataAnnotations;

namespace AccountsAppWeb.Core.Models
{
    public class AccountGroupModel
    {
        public int AccountGroupId { get; set; }
        [Required(ErrorMessage = "Please enter group name")]
        public string AccountGroupName { get; set; }
        [Required(ErrorMessage = "Please enter group alias name")]
        public string AccountGroupNameAlias { get; set; }
        public int GroupUnder { get; set; }
        public string UnderGroupTitle { get; set; }
        public string Nature { get; set; }
        public bool IsAdminGroup { get; set; }
        public bool IsCommonGroup { get; set; }
    }
}
