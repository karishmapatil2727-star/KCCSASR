using System;
using System.Collections.Generic;

namespace AccountsAppWeb.Core.Models
{
    public class AccountBooksReportViewModel
    {
        public int SerialNo { get; set; }
        public string TransactionDate { get; set; }
        public string VoucherTypeName { get; set; }
        public string VoucherNo { get; set; }
        public string ChildLedgerName { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public int? TransactionMasterId { get; set; }
        public string ChequeNo { get; set; }
        public string Balance { get; set; }
        public string MasterNarration { get; set; }
        public string ClassName { get; set; }
    }

    public class LedgerAccountStatementModel
    {
        public decimal? TotalDebit { get; set; }
        public decimal? TotalCredit { get; set; }
        public string ClosingBalance { get; set; }
        public string OpeningBalance { get; set; }
        public List<AccountBooksReportViewModel> accountBooksReports { get; set; }
    }
}
