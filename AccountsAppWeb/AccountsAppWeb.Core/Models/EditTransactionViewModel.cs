using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsAppWeb.Core.Models
{
    public class EditTransactionViewModel
    {
        public int TransactionMasterId { get; set; }
        public string VoucherNo { get; set; }
        public int FinancialYearId { get; set; }
        public int VoucherTypeId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string MasterNarration { get; set; }
        public Decimal TotalAmount { get; set; }
        public int ForInstId { get; set; }
        public string ChequeNo { get; set; }
        public int UniqueTransactionNo { get; set; }
        public List<EditTransactionLedgerViewModel> ledgerViewModels { get; set; }
    }
    public class EditTransactionLedgerViewModel
    {
        public int TransactionDetailsId { get; set; }
        public char CrorDr { get; set; }
        public int LedgerId { get; set; }
        public string LedgerName { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }
}
