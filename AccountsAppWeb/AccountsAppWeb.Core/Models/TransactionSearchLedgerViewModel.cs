namespace AccountsAppWeb.Core.Models
{
    public class TransactionSearchLedgerViewModel
    {
        public int SerialId { get; set; }
        public string TransactionMasterId { get; set; }
        public string Debit { get; set; } = string.Empty;
        public string Credit { get; set; } = string.Empty;
        public string VoucherTypeName { get; set; } = string.Empty;
        public string TransactionDate { get; set; } = string.Empty;
        public string VoucherNo { get; set; } = string.Empty;
        public string MasterNarration { get; set; } = string.Empty;
        public string LedgerName { get; set; } = string.Empty;
        public string ChequeNo { get; set; } = string.Empty;
        public string AccountShortTitle { get; set; } = string.Empty;
        public string className { get; set; }
    }
}
