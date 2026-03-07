namespace AccountsAppWeb.Core.Models
{
    public class ReconciliationLedgerModel
    {
        public string Inst_Title { get; set; }
        public string ClientLedgerName { get; set; }
        public int Id { get; set; }
        public string ToInstLedgerName { get; set; }
        public int NotificationToLedgerId { get; set; }
    }
}
