using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsAppWeb.Core.Models
{
    public class LedgerIncomeExpenditurePartialViewModel
    {
        public int SerialId  { get; set; }
        public string CreditAccountGroupName { get; set; }
        public decimal? Credit { get; set; }
        public string DebitAccountGroupName { get; set; }
        public decimal? Debit { get; set; }
        public string ClassName { get; set; }
        public int CreditAccountId { get; set; }
        public int DebitAccountId { get; set; }
    }
    public class LedgerIncomeExpenditurePartialViewModelPopup
    {
        public List<LedgerIncomeExpenditurePartialViewModel> ledgerIncomeExpenditures { get; set; }
        public decimal? TotalDebit { get; set; }
        public decimal? TotalCredit { get; set; }
    }

}
