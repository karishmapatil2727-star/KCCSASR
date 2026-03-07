using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsAppWeb.Core.Models
{
    public class TransactionNotificationReportModel
    {
        public int SerialNo { get; set; }
        public string TransactionDate { get; set; }
        public string AccountShortTitle { get; set; }
        public string VoucherTypeName { get; set; }
        public string VoucherNo { get; set; }
        public string LedgerName { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
        public string TransactionMasterId { get; set; }
        public string ChequeNo { get; set; }
        public string UniqueId { get; set; }
        public string ClassName { get; set; }
    }
}
