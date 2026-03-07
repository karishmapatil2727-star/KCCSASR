using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsAppWeb.Core.Models
{
    public class TransactionMasterModel
    {
        public int transactionMasterIdField { get; set; }
        public int instIdField { get; set; }
        public int departmentIdField { get; set; }
        public string voucherNoField { get; set; }
        public DateTime transactionDateField { get; set; }
        public int masterLedgerIdField { get; set; }
        public Decimal totalAmountField { get; set; }
        public int voucherTypeIdField { get; set; }
        public int financialYearIdField { get; set; }
        public int insertUserAccountIdField { get; set; }
        public DateTime insertDateField { get; set; }
        public DateTime updateDateField { get; set; }
        public int updateUserAccountIdField { get; set; }
        public int? adminInsertUserAccountIdField { get; set; }
        public DateTime? adminInsertDateField { get; set; }
        public string masterNarrationField { get; set; }
        public int forInstIdField { get; set; }
        public string chequeNoField { get; set; }
        public int? uniqueTransactionNoField { get; set; }
    }
}
