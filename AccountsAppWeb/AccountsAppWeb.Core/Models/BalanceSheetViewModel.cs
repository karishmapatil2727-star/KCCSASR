using System.Collections.Generic;

namespace AccountsAppWeb.Core.Models
{
    public class BalanceSheetViewModel
    {
        public int SeriallId { get; set; }
        public int? AccountGroupId1 { get; set; }
        public string AccountGroupName1 { get; set; }
        public decimal? Credit1 { get; set; }
        public decimal? Credit2 { get; set; }
        public bool IsLedger1 { get; set; }

        public int? AccountGroupId2 { get; set; }
        public string AccountGroupName2 { get; set; }
        public decimal? Debit1 { get; set; }
        public decimal? Debit2 { get; set; }
        public bool IsLedger2 { get; set; }
        public string AccountGroup1ClassName { get; set; }
        public string AccountGroup2ClassName { get; set; }
    }

    public class BalanceSheetReportViewModel
    {
        public string TotalCredit { get; set; }
        public string TotalDebit { get; set; }
        public List<BalanceSheetViewModel> balanceSheets { get; set; }
    }
}
