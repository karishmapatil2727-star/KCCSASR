using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
namespace AccountsAppWeb.Core.Models
{
    public class GSTSalesHeaderVM
    {
        // ===== Header =====
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNo { get; set; }

        [Required(ErrorMessage = "Please select Party")]
        public int SelectedPartyId { get; set; }
        public string PartyName { get; set; }

        [Range(1, 100, ErrorMessage = "Please select GST Slab")]
        public decimal GSTSlab { get; set; }

        [Required(ErrorMessage = "Please select GST Type")]
        public string GSTType { get; set; }

        // ===== Item Entry =====
        public int SelectedItemId { get; set; }          // ← NEW: bound to Item dropdown value

        [Required(ErrorMessage = "Please select Item")]
        public string SelectedItemName { get; set; }

        [Display(Name = "HSN Code")]
        public string HSNCode { get; set; }

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
        public IEnumerable<SelectListItem> PartyList { get; set; } = new List<SelectListItem>();

        public List<decimal> GSTSlabList { get; set; } = new List<decimal> { 5, 12, 18 };

        public List<string> GSTTypeList { get; set; } = new List<string>
        {
            "CGST & SGST",
            "IGST"
        };

        // ── CHANGED: was List<string>, now List<SelectListItem> for dynamic DB data ──
        public List<SelectListItem> ItemList { get; set; } = new List<SelectListItem>();

        // ── NEW: JSON map used by JS to auto-fill HSN Code when item selected ──
        public string ItemHSNMapJson { get; set; } = "[]";
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

    // ── NEW: model that maps to DB result from GetItemsByIncomeGSTSalesGroup ──
    public class GSTItemModel
    {
        public int LedgerId { get; set; }
        public string LedgerName { get; set; }
        public string HSNCode { get; set; }
    }
}
