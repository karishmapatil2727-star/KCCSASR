namespace AccountsAppWeb.Core.Models
{
    public class BalanceSheetResponseModel
    {
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public int AAC_ChildAccountGroupId { get; set; }
        public int AAC_ParentAccountGroupId { get; set; }
        public string AccountGroupName { get; set; }
        public string Nature { get; set; }
    }
}
