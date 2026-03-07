namespace AccountsAppWeb.Core.Models
{
    public class FinancialYearModel
    {
        public int Fin_ID { get; set; }
        public string Fin_Title { get; set; }
        public string Fin_StartDate { get; set; }
        public string Fin_EndDate { get; set; }
        public int Inst_ID { get; set; }
        public string Inst_Title { get; set; }
        public bool BI_IsTransactionAddAllow { get; set; }
        public bool BI_IsTransactionEditAllow { get; set; }
        public bool BI_IsOpeningBalanceEditAllow { get; set; }
        public bool BI_IsNewLedgerAddAllow { get; set; }
    }
}
