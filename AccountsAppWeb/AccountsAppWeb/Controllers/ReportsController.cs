using AccountsAppWeb.Core;
using AccountsAppWeb.Core.Models;
using AccountsAppWeb.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountsAppWeb.Controllers
{
    [LogonAuthorize]
    public class ReportsController : BaseController
    {
        private readonly ReportsManager reportsManager;
        private UserModel user;
        public ReportsController()
        {
            reportsManager = new ReportsManager();
            user = UserManager.User;
        }

        #region Balance sheets
        // GET: Reports
        public ActionResult FinalResultBalanceSheet()
        {
            return View();
        }

        public ActionResult CommonFinalResultBalanceSheet()
        {
            return View();
        }

        public JsonResult GenerateBalanceSheet(DateTime fromDate, DateTime toDate, int instituteId)
        {
            string surPlusGuid = Guid.NewGuid().ToString();
            var surPlusDeficitAmount = reportsManager.CreateLedgerIncomeExpendituteCollegeGroup(fromDate, toDate, instituteId, user.FinancialYearId, user.DepartmentID, surPlusGuid, user.InstituteId);
            string guid = Guid.NewGuid().ToString() + "BS";
            TempData["guid"] = guid;
            var balanceSheet = reportsManager.AccountGenreteBalanceSheet(user.FinancialYearId, 1, user.DepartmentID, instituteId, fromDate, toDate, user.InstituteId, guid, surPlusDeficitAmount);
            var jsonResult = Json(balanceSheet, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult GenerateCommonBalanceSheet(DateTime fromDate, DateTime toDate, int instituteId)
        {
            string surPlusGuid = Guid.NewGuid().ToString();
            var surPlusDeficitAmount = reportsManager.CreateLedgerCommonIncomeExpendituteCollegeGroup(fromDate, toDate, instituteId, user.FinancialYearId, user.DepartmentID, surPlusGuid, user.InstituteId);
            string guid = Guid.NewGuid().ToString() + "BS";
            TempData["guid"] = guid;
            var balanceSheet = reportsManager.AccountGenreteCommonBalanceSheet(user.FinancialYearId, 1, user.DepartmentID, instituteId, fromDate, toDate, user.InstituteId, guid, surPlusDeficitAmount);
            var jsonResult = Json(balanceSheet, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult IncomeExpenditureDetails()
        {
            return View();
        }
       public ActionResult CommonIncomeExpenditureDetails()
        {
            return View();
        }
        public JsonResult GenerateIncomeExpenditureDetails(DateTime fromDate, DateTime toDate, int instituteId)
        {
            string guid = Guid.NewGuid().ToString();
            TempData["guid"] = guid;
            var incomeExpenditure = reportsManager.CreateLedgerIncomeExpendituteCollegeGroupFoReport(fromDate, toDate, instituteId, user.FinancialYearId, user.DepartmentID, guid, user.InstituteId);
            var jsonResult = Json(incomeExpenditure, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GenerateCommonIncomeExpenditureDetails(DateTime fromDate, DateTime toDate, int instituteId)
        {
            string guid = Guid.NewGuid().ToString();
            TempData["guid"] = guid;
            var incomeExpenditure = reportsManager.CreateLedgerCommonIncomeExpendituteCollegeGroupFoReport(fromDate, toDate, instituteId, user.FinancialYearId, user.DepartmentID, guid, user.InstituteId);
            var jsonResult = Json(incomeExpenditure, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult CloseTrailBalance()
        {
            return View();
        }
        public JsonResult CloseTrailBalanceJsonReport(DateTime fromDate, DateTime toDate, int instituteId)
        {
            string guid = Guid.NewGuid().ToString();
            var closingTrail = reportsManager.FinancialReportsBalanceSheet(user.FinancialYearId, 1, user.DepartmentID, fromDate, toDate.AddDays(1.0), instituteId, guid, instituteId);
            var jsonResult = Json(closingTrail, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult CloseTrailBalanceSchedule()
        {
            return View();
        }
        public JsonResult CloseTrailBalanceScheduleJsonReport(DateTime fromDate, DateTime toDate, int instituteId)
        {
            string guid = Guid.NewGuid().ToString();
            TempData["guid"] = guid;
            var closingTrail = reportsManager.FinancialReportsBalanceSheet(user.FinancialYearId, 1, user.DepartmentID, fromDate, toDate.AddDays(1.0), instituteId, guid, instituteId, true, false, true);
            return Json(closingTrail, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CloseTrailBalanceDetailed()
        {
            return View();
        }
        public JsonResult CloseTrailBalanceDetailedJsonReport(DateTime fromDate, DateTime toDate, int instituteId)
        {
            string guid = Guid.NewGuid().ToString();
            TempData["guid"] = guid;
            var closingTrail = reportsManager.OpeningTrialDetailed(fromDate, toDate.AddDays(1.0), user.FinancialYearId, instituteId, user.InstituteId);
            return Json(closingTrail, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OpeningTrailBalance()
        {
            return View();
        }
        public JsonResult OpenTrailBalanceJsonReport(DateTime fromDate, DateTime toDate, int instituteId)
        {
            string guid = Guid.NewGuid().ToString();
            TempData["guid"] = guid;
            var closingTrail = reportsManager.FinancialReportsBalanceSheet(user.FinancialYearId, 1, user.DepartmentID, fromDate, fromDate, instituteId, guid, instituteId, true, true, false);
            return Json(closingTrail, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OpenTrailBalanceSchedule()
        {
            return View();
        }
        public JsonResult OpenTrailBalanceScheduleJsonReport(DateTime fromDate, DateTime toDate, int instituteId)
        {
            string guid = Guid.NewGuid().ToString();
            TempData["guid"] = guid;
            var closingTrail = reportsManager.FinancialReportsBalanceSheet(user.FinancialYearId, 1, user.DepartmentID, fromDate, fromDate, instituteId, guid, instituteId, true, true, true);
            return Json(closingTrail, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OpenTrailBalanceDetailed()
        {
            return View();
        }
        public JsonResult OpenTrailBalanceDetailedJsonReport(DateTime fromDate, DateTime toDate, int instituteId)
        {
            string guid = Guid.NewGuid().ToString();
            TempData["guid"] = guid;
            var closingTrail = reportsManager.OpeningTrialDetailed(fromDate, fromDate, user.FinancialYearId, instituteId, user.InstituteId);
            return Json(closingTrail, JsonRequestBehavior.AllowGet);
        }
        #endregion Balance sheets

        public ActionResult SingleLedgerAccountBook()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SingleLedgerAccountStatement(DateTime fromDate, DateTime toDate, int ledgerId, int instituteId)
        {
            var statementReport = reportsManager.AccountStatementLedgerReport(fromDate, toDate, ledgerId, instituteId, user.InstituteId, user.DepartmentID, user.FinancialYearId);
            var jsonResult = Json(statementReport, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult AllLedgerAccountStatement(DateTime fromDate, DateTime toDate, int instituteId)
        {
            TempData["AllLegder"] = fromDate + "$" + toDate + "$" + instituteId;
            var statementReport = reportsManager.AccountStatementAllLedersReport(fromDate, toDate, instituteId, user.InstituteId, user.DepartmentID, user.FinancialYearId, 0);
            return new JsonResult()
            {
                Data = statementReport.accountBooksReports,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }
        [HttpGet]
        public ActionResult AllLedgersStartementReportPdf()
        {
            if (TempData["AllLegder"] != null)
            {
                string values = TempData["AllLegder"] as string;
                TempData.Keep("AllLegder");
                string[] inputsParams = values.Split(new Char[] { '$' });
                DateTime fromDate = Convert.ToDateTime(inputsParams[0]);
                DateTime toDate = Convert.ToDateTime(inputsParams[1]);
                int instituteId = Convert.ToInt32(inputsParams[2]);
                var streamArray = reportsManager.AllReportPdf(fromDate, toDate, instituteId, user.InstituteId, user.DepartmentID, user.FinancialYearId, 0, user.InstName);
                return File(streamArray, "application/pdf", "All-Ledger-Report-"+DateTime.Now.ToString("dd/MM/yyyy")+".pdf");
            }
            return Json("error");
        }

        [HttpGet]
        public ActionResult AllLedgersStartementReportPdfIndex()
        {
            if (TempData["AllLegder"] != null)
            {
                string values = TempData["AllLegder"] as string;
                TempData.Keep("AllLegder");
                string[] inputsParams = values.Split(new Char[] { '$' });
                DateTime fromDate = Convert.ToDateTime(inputsParams[0]);
                DateTime toDate = Convert.ToDateTime(inputsParams[1]);
                int instituteId = Convert.ToInt32(inputsParams[2]);
                var streamArray = reportsManager.AllReportPdfIndex(fromDate, toDate, instituteId, user.InstituteId, user.DepartmentID, user.FinancialYearId, 0, user.InstName);
                return File(streamArray, "application/pdf", "Ledger-Index-" + DateTime.Now.ToString("dd/MM/yyyy") + ".pdf");
            }
            return Json("error");
        }

        [HttpPost]
        public JsonResult AccountGroupAccountStatement(DateTime fromDate, DateTime toDate, int instituteId, int groupId)
        {
            var statementReport = reportsManager.AccountStatementAllLedersReport(fromDate, toDate, instituteId, user.InstituteId, user.DepartmentID, user.FinancialYearId, groupId);
            return Json(statementReport, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CashDayBook()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GenerateAccountDayBook(DateTime fromDate, DateTime toDate, int instituteId)
        {
            TempData["CashBook"] = fromDate + "$" + toDate + "$" + instituteId;
            var dayReport = reportsManager.GenerateAccountDayBook(fromDate, toDate, instituteId, 0, user.InstituteId, user.DepartmentID, user.FinancialYearId, 0);
            var jsonResult = Json(dayReport, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpGet]
        public ActionResult GenerateAccountDayBookReportPdf()
        {
            if (TempData["CashBook"] != null)
            {
                string values = TempData["CashBook"] as string;
                TempData.Keep("CashBook");
                string[] inputsParams = values.Split(new Char[] { '$' });
                DateTime fromDate = Convert.ToDateTime(inputsParams[0]);
                DateTime toDate = Convert.ToDateTime(inputsParams[1]);
                int instituteId = Convert.ToInt32(inputsParams[2]);
                var streamArray = reportsManager.GenerateCashBookDayPdf(fromDate, toDate, instituteId, 0, user.InstituteId, user.DepartmentID, user.FinancialYearId, 0,user.InstName);
                return File(streamArray, "application/pdf", "Cash-Book-Report-" + DateTime.Now.ToString("dd/MM/yyyy") + ".pdf");
            }
            return Json("error");
        }
        public ActionResult ViewNotification()
        {
            return View();
        }
        [HttpPost]
        public JsonResult TransactionNotification(int isCompTransaction)
        {
            var transactionList = reportsManager.TransactionNotification(user.InstituteId, isCompTransaction, user.FinancialYearId);
            return Json(transactionList, JsonRequestBehavior.AllowGet);

        }
        public JsonResult TransactionNotificationUpdate(int uniqueId)
        {
            reportsManager.TransactionNotificationUpdate(uniqueId, Convert.ToInt32(user.UserName));
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult ReconciliationReport()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ReconciliationSelectAllLedgers(DateTime toDate, int instituteId)
        {
            var reconciliationLedgers = reportsManager.ReconciliationSelectAllLedgers(toDate, instituteId, user.InstituteId, user.FinancialYearId);
            var jsonResult = Json(reconciliationLedgers, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult ReconciliationDetailedGrid(DateTime toDate, int myLedgerId, int MyInstId, int toLedgerId, int toInstId)
        {
            var reconciliationDetails = reportsManager.ReconcilationReportDetails(toDate, user.InstituteId, user.FinancialYearId, myLedgerId, toLedgerId, MyInstId, toInstId, user.FinancialYearStartDate);
            return Json(reconciliationDetails, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenerateRevenueReport()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetGeneralRevenueReport(DateTime fromDate, DateTime toDate, int instituteId)
        {
            var generalRevenueRport = reportsManager.ConsolidatedGeneralRevenue(fromDate, toDate, instituteId, user.DepartmentID, user.FinancialYearId, user.FinancialYearStartDate);
            var jsonResult = Json(generalRevenueRport, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult ScheduledWiseReport()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetScheduledWiseReport(DateTime fromDate, DateTime toDate, int instituteId, int groupId)
        {
            var scheduleReport = reportsManager.GetScheduledWiseReport(toDate.AddDays(1.0), groupId, user.FinancialYearId);
            return Json(scheduleReport, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult TransactionMasterAndDetailByVoucherNo(int transactionMasterId)
        {
            TransactionsManager transactionsManager = new TransactionsManager();
            var transactionViewModel = transactionsManager.TransactionMasterAndDetailById(transactionMasterId, user.InstituteId);
            return Json(transactionViewModel, JsonRequestBehavior.AllowGet);
        }
        private int tinstidforCenterOffice(int tForInstId)
        {
            return user.InstituteId != 300010 ? user.InstituteId : tForInstId;
        }
        [HttpPost]
        public JsonResult BindGroupSummaryReport(int accountGroupId, string accountGroupName, DateTime toDate)
        {
            string guid = (string)TempData["guid"];
            TempData.Keep();
            var groupSummaryRpt = reportsManager.BindGroupSummaryReport(accountGroupId, accountGroupName, user.InstituteId, user.FinancialYearId, toDate, guid);
            var jsonResult = Json(groupSummaryRpt, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult CreateLedgerIncomeExpenditute(int instituteId, DateTime fromDate, DateTime toDate)
        {
            string guid = (string)TempData["guid"];
            TempData.Keep();
            var ledgerIncomeExpenditute = reportsManager.CreateLedgerIncomeExpenditute(user.InstituteId, instituteId, user.DepartmentID, user.FinancialYearId, fromDate, toDate, guid);
            var jsonResult = Json(ledgerIncomeExpenditute, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult CreateLedgerCommonIncomeExpenditute(int instituteId, DateTime fromDate, DateTime toDate)
        {
            string guid = (string)TempData["guid"];
            TempData.Keep();
            var ledgerIncomeExpenditute = reportsManager.CreateLedgerCommonIncomeExpenditute(user.InstituteId, instituteId, user.DepartmentID, user.FinancialYearId, fromDate, toDate, guid);
            var jsonResult = Json(ledgerIncomeExpenditute, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult BindGroupSchSummaryReport(int instituteId, int accountGroupId, string accountGroupName, DateTime toDate)
        {
            string guid = (string)TempData["guid"];
            TempData.Keep();
            //if (accountGroupId == 68)
            //{
            //    var groupSummaryRpt = reportsManager.BindGroupSchSummaryReport(accountGroupId, accountGroupName, instituteId, user.InstituteId, user.FinancialYearId, user.FinancialYearStartDate, toDate, guid, user.DepartmentID);
            //    var jsonResult = Json(groupSummaryRpt, JsonRequestBehavior.AllowGet);
            //    jsonResult.MaxJsonLength = int.MaxValue;
            //    return jsonResult;
            //}
            //else
            {
                var groupSummaryRpt = reportsManager.BindGroupSummaryReport(accountGroupId, accountGroupName, user.InstituteId, user.FinancialYearId, toDate, guid);
                var jsonResult = Json(groupSummaryRpt, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
        }
    }
}