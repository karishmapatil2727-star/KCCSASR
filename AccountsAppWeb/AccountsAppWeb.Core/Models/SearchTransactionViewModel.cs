using System.Collections.Generic;

namespace AccountsAppWeb.Core.Models
{
    public class SearchTransactionViewModel
    {
        public List<TransactionSearchLedgerViewModel> transactionSearchLedgers { get; set; }
        public TotalTransactionAmount transactionAmount { get; set; }
    }
}
