using System.Collections.Generic;

namespace AccountsAppWeb.Core.Models
{
    public class LedgerIncomeExpendituteDetails
    {
        public List<LedgerIncomeExpendituteDebit> ledgerIncomeExpendituteDebits { get; set; }
        public List<LedgerIncomeExpendituteCredit> ledgerIncomeExpendituteCredits { get; set; }
        public decimal? TotalDebit { get; set; }
        public decimal? TotalCredit { get; set; }
    }

    public class LedgerIncomeExpendituteDebit
    {
        public int SerialId { get; set; }
        public string AccountGroupName { get; set; }
        public decimal? Debit { get; set; }
        public string ClassName { get; set; }
    }

    public class LedgerIncomeExpendituteCredit
    {
        public int SerialId { get; set; }
        public string AccountGroupName { get; set; }
        public decimal? Credit { get; set; }
        public string ClassName { get; set; }
    }
}
