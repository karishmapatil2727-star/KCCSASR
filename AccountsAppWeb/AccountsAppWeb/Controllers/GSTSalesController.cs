using AccountsAppWeb.Core;
using AccountsAppWeb.Core.Models;
using System;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using System.Web;
using System.Collections;
using AccountsAppWeb.Infrastructure;
using NLog;

namespace AccountsAppWeb.Controllers
{
    [Infrastructure.LogonAuthorize]
    public class GSTSalesController : BaseController
    {
        // GET: GSTSales
        public ActionResult Index()
        {
            var model = CreateDefaultModel();
            return View(model);
        }

        // POST: GSTSales (Proceed)
        [HttpPost]
        public ActionResult Index(GSTSalesHeaderVM model)
        {
            model.ShowItemsSection = true;
            ReloadLists(model);
            return View(model);
        }

        // POST: Add Item
        [HttpPost]
        public ActionResult AddItem(GSTSalesHeaderVM model)
        {
            decimal cgst = 0, sgst = 0, igst = 0;

            if (model.GSTSlab > 0)
            {
                if (model.GSTType == "CGST & SGST")
                {
                    decimal halfGST = model.GSTSlab / 2;
                    cgst = model.Amount * halfGST / 100;
                    sgst = model.Amount * halfGST / 100;
                }
                else if (model.GSTType == "IGST")
                {
                    igst = model.Amount * model.GSTSlab / 100;
                }
            }

            decimal totalGST = cgst + sgst + igst;

            model.Items.Add(new GSTSalesItemVM
            {
                ItemName = model.SelectedItemName,
                HSN = model.HSN,
                BaseAmount = model.Amount,
                CGST = cgst,
                SGST = sgst,
                IGST = igst,
                TotalGST = totalGST,
                FinalAmount = model.Amount + totalGST
            });

            model.Amount = 0;
            model.ShowItemsSection = true;

            ReloadLists(model);

            // 🔴 THIS LINE FIXES DROPDOWN RESET ISSUE
            ModelState.Clear();

            return View("Index", model);
        }


        // POST: Delete Item
        [HttpPost]
        public ActionResult DeleteItem(int index, GSTSalesHeaderVM model)
        {
            if (model.Items != null && index >= 0 && index < model.Items.Count)
            {
                model.Items.RemoveAt(index);
            }

            model.ShowItemsSection = true;
            ReloadLists(model);
            return View("Index", model);
        }

        // ================= HELPER METHODS =================

        private string GetFinancialYear(DateTime date)
        {
            int startYear;
            int endYear;

            // Financial year starts in April
            if (date.Month >= 4)
            {
                startYear = date.Year;
                endYear = date.Year + 1;
            }
            else
            {
                startYear = date.Year - 1;
                endYear = date.Year;
            }

            return $"{startYear}-{endYear.ToString().Substring(2)}";
        }

        private GSTSalesHeaderVM CreateDefaultModel()
        {
            var today = DateTime.Today;

            string financialYear = GetFinancialYear(today);
            int invoiceNumber = 1; // or static 1 if no persistence

            return new GSTSalesHeaderVM
            {
                InvoiceDate = today,
                InvoiceNo = $"{invoiceNumber}/{financialYear}",
                ShowItemsSection = false
            };
        }


        private void ReloadLists(GSTSalesHeaderVM model)
        {
            model.PartyList = new GSTSalesHeaderVM().PartyList;
            model.GSTSlabList = new GSTSalesHeaderVM().GSTSlabList;
            model.GSTTypeList = new GSTSalesHeaderVM().GSTTypeList;
            model.ItemList = new GSTSalesHeaderVM().ItemList;
        }
    }
}
