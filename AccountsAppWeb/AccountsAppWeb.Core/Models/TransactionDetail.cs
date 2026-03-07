using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsAppWeb.Core.Models
{
    public class TransactionDetail
    {
        public int transactionDetailsIdField { get; set; }
        public int transactionMasterIdField { get; set; }
        public int ledgerIdField { get; set; }
        public string chequeNoField { get; set; }
        public DateTime? chequeDateField { get; set; }
        public string narrationField { get; set; }
        public bool isCashEntryField { get; set; }
        public Decimal debitField { get; set; }
        public Decimal creditField { get; set; }
        public string uniqueIDField { get; set; }
        public int? forInstIdField { get; set; }
    }
}
