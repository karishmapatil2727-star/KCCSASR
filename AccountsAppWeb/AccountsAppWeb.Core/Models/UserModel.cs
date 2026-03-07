using System;

namespace AccountsAppWeb.Core.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        public int InstituteId { get; set; }
        public bool IsLoginSuccess { get; set; }
        public bool IsNewLedgerAddAllow { get; set; }
        public bool IsOpeningBalanceEditAllow { get; set; }
        public bool IsTransactionAddAllow { get; set; }
        public bool IsTransactionEditAllow { get; set; }
        public bool IsEnableGroupPage { get; set; }
        public int DepartmentID { get; set; } = 1;
        public int FinancialYearId { get; set; }
        public DateTime FinancialYearStartDate { get; set; }
        public DateTime FinancialYearEndDate { get; set; }
        public string FinTitle { get; set; }
        public string FinTitle1 { get; set; }
        public string FinTitle2 { get; set; }
        public string FinTitle3 { get; set; }
        public string FinTitle4 { get; set; }
        public string InstName { get; set; }
    }
}
