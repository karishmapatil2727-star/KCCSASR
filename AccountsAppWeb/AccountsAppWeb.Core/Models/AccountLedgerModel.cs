using System.ComponentModel.DataAnnotations;

namespace AccountsAppWeb.Core.Models
{
    public class AccountLedgerModel
    {
        public int LedgerId { get; set; }

        public int AccountGroupId { get; set; }

        [Required(ErrorMessage = "Please enter ledger name")]
        public string LedgerName { get; set; }

        public string LedgerNameAlias { get; set; }

        public string LedgerNamePrint { get; set; }

        [Range(0, 9999999999999999.99,ErrorMessage = "Please enter value in correct format")]
        public decimal OpeningBalance { get; set; }

        public string Narration { get; set; }

        public string Address { get; set; }

        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string Mobile { get; set; }

        public string TIN { get; set; }

        public string CST { get; set; }

        public string PAN { get; set; }

        public string CrOrDr { get; set; }

        public bool IsGSTSales { get; set; }

        public string GST { get; set; }
    }
}
