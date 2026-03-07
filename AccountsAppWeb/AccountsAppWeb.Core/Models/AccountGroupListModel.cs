namespace AccountsAppWeb.Core.Models
{
    public class AccountGroupListModel
    {
        public int AccountGroupId { get; set; }
        public string AccountGroupName { get; set; }
        public string UnderGroupTitle { get; set; }
        public string Nature { get; set; } 
        public int IsEnable { get; set; }
       public bool IsCommonGroup { get; set; }
    }
}
