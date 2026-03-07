namespace AccountsAppWeb.Core.Models
{
    public class ReconciliationReportModel
    {
        public int? MyLederId { get; set; }
        public int? MyInstId { get; set; }
        public int? ToLederId { get; set; }
        public int? ToInstId { get; set; }
        public string MyLedgerName { get; set; }
        public string MyDebit { get; set; }
        public string MyCredit { get; set; }
        public string InstLedgerName { get; set; }
        public string InstDebit { get; set; }
        public string InstCredit { get; set; }
        public string Difference { get; set; }
    }
}
