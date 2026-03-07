using System;

namespace AccountsAppWeb.Core.Models
{
    public class ResetVochersViewModel
    {
        public long NewNotiNum { get; set; }
        public int TransactionMasterId { get; set; }
        public string TransactionDate { get; set; }
        public DateTime TransactionDateOriginal { get; set; }
        public string VoucherNo { get; set; }
        public int VocherTypeId { get; set; }
        public string VocherTypeName { get; set; }
        public int UniqueTransactionNo { get; set; }
        public decimal TotalAmount { get; set; }
        public string InsertDate { get; set; }
        public string ChequeNo { get; set; }
        public string NewVoucherNo { get; set; }
    }
}
