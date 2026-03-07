using System.Collections.Generic;

namespace AccountsAppWeb.Core.Models
{
    public class IncomeExpenditureDetailsViewModel
    {
        public int SeriallId { get; set; }
        public string AccountGroupName1 { get; set; }
        public string AccountGroupName2 { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Debit { get; set; }
        public int? ForInstId1 { get; set; }
        public string AccountGroupId1 { get; set; }
        public string AccountGroupId2 { get; set; }
    }
    public class IncomeExpenditureDetailsReportViewModel
    {
        public string TotalDebit { get; set; }
        public string TotalCredit { get; set; }
        public List<IncomeExpenditureDetailsViewModel> incomeExpenditures { get; set; }
    }
}
