using System.Collections.Generic;

namespace AccountsAppWeb.Core.Models
{
    public class FinancialReportViewModel
    {
        public int SerialId { get; set; }
        public string AccountGroupId { get; set; }
        public string AccountGroupName { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public string ClassName { get; set; }
        public bool IsLedger { get; set; }
    }
    public class ClosingTrailBalanceReportViewModel
    {
        public List<FinancialReportViewModel> finamncialReportViews { get; set; }
        public string TotalCredit { get; set; }
        public string TotalDebit { get; set; }
    }
}
