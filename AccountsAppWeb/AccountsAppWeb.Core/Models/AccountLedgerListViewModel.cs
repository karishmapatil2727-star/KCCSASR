namespace AccountsAppWeb.Core.Models
{
    public class AccountLedgerListViewModel
    {
        public int LedgerId { get; set; }
        public string LedgerName { get; set; }
        public string AccountGroupName { get; set; }
        public string Inst_ShortTitle { get; set; }
        public string CrOrDr { get; set; }
        public decimal OpeningBalance { get; set; }
       public int IsEnable { get; set; }
    }
}
