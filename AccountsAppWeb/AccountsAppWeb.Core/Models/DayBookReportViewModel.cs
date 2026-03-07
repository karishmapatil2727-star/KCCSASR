namespace AccountsAppWeb.Core.Models
{
    public class DayBookReportViewModel
    {
        public int SerialNo { get; set; }
        public string LedgerName1 { get; set; }
        public string VoucherTypeName1 { get; set; }
        public string VoucherNo1 { get; set; }
        public string Credit { get; set; }       
        public string CashCredit { get; set; }   
        public string TransactionMasterId1 { get; set; }
        public string ClassName1 { get; set; }

        public string LedgerName2 { get; set; }
        public string VoucherTypeName2 { get; set; }
        public string VoucherNo2 { get; set; }
        public string Debit { get; set; }
        public string CashDebit { get; set; }
        public string TransactionMasterId2 { get; set; }
        public string ClassName2 { get; set; }
    }   
}
