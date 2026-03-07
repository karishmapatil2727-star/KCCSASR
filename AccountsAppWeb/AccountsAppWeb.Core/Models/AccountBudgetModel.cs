using System.ComponentModel.DataAnnotations;

namespace AccountsAppWeb.Core.Models
{
    public class AccountBudgetModel
    {
        public int LedgerId { get; set; }

        [Required(ErrorMessage = "Please enter ledger name")]
        public string LedgerName { get; set; }

        public int InstId { get; set; }

        public string Inst_ShortTitle { get; set; }

        public decimal BD_BudgetAmount { get; set; }

        public decimal BD_BudgetAmount_4_Months { get; set; }

        public decimal PBD_Financial0BudgetAmount { get; set; }

        public decimal PBD_Financial1BudgetAmount { get; set; }

        public decimal PBD_Financial2BudgetAmount { get; set; }

        public decimal PBD_Financial3BudgetAmount { get; set; }

        public decimal PBD_Financial4BudgetAmount { get; set; }

        public int FinalFinancialYearId { get; set; }

    }
}
