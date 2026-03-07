using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AccountsAppWeb.Core.Models
{
    public class GSTSalesHeaderVM
    {
        // ===== Header =====
        public DateTime InvoiceDate { get; set; }

        public string InvoiceNo { get; set; }

        [Required(ErrorMessage = "Please select Party")]
        public string PartyName { get; set; }

        [Range(1, 100, ErrorMessage = "Please select GST Slab")]
        public decimal GSTSlab { get; set; }

        [Required(ErrorMessage = "Please select GST Type")]
        public string GSTType { get; set; }

        // ===== Item Entry =====
        [Required(ErrorMessage = "Please select Item")]
        public string SelectedItemName { get; set; }
        public string HSN { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        // ===== UI Flags =====
        public bool ShowItemsSection { get; set; }

        // ===== Items Grid =====
        public List<GSTSalesItemVM> Items { get; set; } = new List<GSTSalesItemVM>();

        public decimal TotalBaseAmount => Items.Sum(x => x.BaseAmount);
        public decimal TotalGSTAmount => Items.Sum(x => x.TotalGST);
        public decimal GrandTotal => Items.Sum(x => x.FinalAmount);

        // ===== Dropdown Data =====
        public List<string> PartyList { get; set; } = new List<string>
        {
            "HDFC Bank Limited"
        };

        public List<decimal> GSTSlabList { get; set; } = new List<decimal>
        {
            5, 12, 18
        };

        public List<string> GSTTypeList { get; set; } = new List<string>
        {
            "CGST & SGST",
            "IGST"
        };

        public List<string> ItemList { get; set; } = new List<string>
        {
            "Sponsorship – Book Festival Unit – Khalsa College, Amritsar"
        };
    }

    public class GSTSalesItemVM
    {
        public string ItemName { get; set; }
        public string HSN { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }

        public decimal TotalGST { get; set; }
        public decimal FinalAmount { get; set; }
    }
}
