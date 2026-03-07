using System.Collections.Generic;

namespace AccountsAppWeb.Core.Models
{
    public class ConsolidatedGeneralRevenueModel
    {
        public int Id { get; set; }
        public string InstId { get; set; }
        public string AccountGroupName1 { get; set; }
        public string AccountGroupName2 { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
        public bool IsBold { get; set; }
    }
    public class StatementRevenueModel
    {
        public int Id { get; set; }
        public string InstId { get; set; }
        public string AccountGroupName { get; set; }
        public string Expenses { get; set; }
        public string ExpensesSchD { get; set; }
        public string Income { get; set; }
        public string SchD { get; set; }
        public string NetSaving { get; set; }
        public string NetSavingSchD { get; set; }
        public string NetLoss { get; set; }
        public string NetLossSchD { get; set; }
    }

    public class ConsolidatedGeneralRevenueRport
    {
        public List<ConsolidatedGeneralRevenueModel> consolidatedGeneralRevenues { get; set; }
        public List<StatementRevenueModel> statementRevenues { get; set; }
    }
}
