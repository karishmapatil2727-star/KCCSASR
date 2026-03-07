using System;
using System.Collections.Generic;

namespace AccountsAppWeb.Core.Models
{
    public class TransactionsViewModel
    {
        public int MasterTransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int DeptId { get; set; }
        public int VocherType { get; set; }
        public string NewVocherNo { get; set; }
        public string ChequeorCash { get; set; }
        public string MasterNarration { get; set; }
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }

        public int FinancialYearId { get; set; }
        public List<AccountLedgers> accountLedgers { get; set; }
    }

    public class AccountLedgers
    {
        public char CrorDr { get; set; }
        public int LedgerId { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }
}