namespace AccountsAppWeb.Core.Models
{
    public class TrailBalanceDetailedReportViewModel
    {
        public int ForInstId { get; set; }
        public int AccountId { get; set; }
        public int AccountGroupName { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
    }
}
