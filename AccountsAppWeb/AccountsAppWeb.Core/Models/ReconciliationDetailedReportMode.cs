using System.Collections.Generic;

namespace AccountsAppWeb.Core.Models
{
    public class ReconciliationDetailedReportModel
    {
        public string MyLedgerCreditDebitBalance { get; set; }
        public string InstLedgerCreditDebitBalance { get; set; }
        public List<LedgerDetailedReport> MyledgerDetails { get; set; }
        public List<LedgerDetailedReport> InstLedgerDetails { get; set; }
    }

    public class LedgerDetailedReport
    {
        public string TransactionDate { get; set; }
        public string VoucherTypeName { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public string VoucherNo { get; set; }
        public string MasterNarration { get; set; }
        public string ChequeNo { get; set; }
    }
}
