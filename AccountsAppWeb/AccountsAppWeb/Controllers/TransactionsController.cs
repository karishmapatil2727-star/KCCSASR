using AccountsAppWeb.Core;
using AccountsAppWeb.Core.Models;
using AccountsAppWeb.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AccountsAppWeb.Controllers
{
    [LogonAuthorize]
    public class TransactionsController : BaseController
    {
        private readonly TransactionsManager transactionsManager;
        private UserModel user;
        public TransactionsController()
        {
            transactionsManager = new TransactionsManager();
            user = UserManager.User;
        }

        // GET: Transactions
        public ActionResult Index()
        {
            var cashBalance = transactionsManager.CashBalance(user.InstituteId, user.FinancialYearId);
            ViewBag.CashBalance = cashBalance;
            return View();
        }
        public JsonResult SelectMaximumNewVoucherNoAutoGenerate(int VoucherTypeId)
        {
            var uniqueTransactionId = transactionsManager.SelectMaximumNewVoucherNoAutoGenerate(user.InstituteId, user.FinancialYearId, VoucherTypeId);
            return Json(uniqueTransactionId, JsonRequestBehavior.AllowGet);
        }
        #region transactions
        public ActionResult List()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetLedgerVoucherList(TransactionListRequestModel requestModel)
        {
            requestModel.ToInstituteId = user.InstituteId != 300010 ? user.InstituteId : requestModel.ForInstituteId;
            var tupleList = transactionsManager.CreateLedgerVoucherList(user.DepartmentID, requestModel);
            var response = new SearchTransactionViewModel()
            {
                transactionSearchLedgers = tupleList.transactionLedger,
                transactionAmount = tupleList.totalTransactionAmount
            };
            var jsonResult = Json(response, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult AddTransactions(TransactionsViewModel transactionsViewModel)
        {
            var response = "";
            if (transactionsViewModel.FinancialYearId == user.FinancialYearId)
            {
                response = transactionsManager.TransactionMasterInsert(transactionsViewModel, user.InstituteId, user.FinancialYearId, user.DepartmentID, Convert.ToInt32(user.UserName));
            }
            else
            {
                response = "Invalid Financial Year. Please refresh your page and try again";
            }
            var cashBalance = string.Empty;
            if (response == "Transaction saved")
            {
                 cashBalance = transactionsManager.CashBalance(user.InstituteId, user.FinancialYearId);
                ViewBag.CashBalance = cashBalance;
                return Json(new { result = true, message = response , cashBalance = cashBalance }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { result = false, message = response}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ModifyTransaction(string voucherNo=null, string voucherType=null)
        {
            ViewBag.voucherNo = voucherNo;
            return View();
        }
      

        [HttpPost]
        public JsonResult TransactionMasterAndDetailByVoucherNo(int vocherType, string vocherNumber)
        {
            int transactionMasterId = transactionsManager.TransactionMasterAndDetailByVoucherNo(user.InstituteId, vocherType, vocherNumber);

            var transactionViewModel = transactionsManager.TransactionMasterAndDetailById(transactionMasterId, user.InstituteId);
            return Json(transactionViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateTransactions(TransactionsViewModel transactionsViewModel)
        {
            var response = transactionsManager.TransactionMasterUpdate(transactionsViewModel, user.InstituteId, user.FinancialYearId, user.DepartmentID, Convert.ToInt32(user.UserName));
            if (response == "Transaction saved")
            {
                var cashBalance = transactionsManager.CashBalance(user.InstituteId, user.FinancialYearId);
                ViewBag.CashBalance = cashBalance;
                return Json(new { result = true, message = response }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { result = false, message = response }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region search transactions

        public ActionResult SearchTransaction()
        {
            return View();
        }

        public JsonResult GetLedgerVoucherBykeywordorList(string searchValue)
        {
            var tupleList = transactionsManager.CreateLedgerVoucherBykeyword(user.InstituteId, user.FinancialYearId, searchValue);
            var response = new SearchTransactionViewModel()
            {
                transactionSearchLedgers = tupleList.transactionLedger,
                transactionAmount = tupleList.totalTransactionAmount
            };
            var jsonResult = Json(response, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;        
        }

        #endregion
        #region Reset vochers        
        public ActionResult ResetVouchers()
        {
            return View();
        }

        public JsonResult GetVoucherTypes()
        {
            var vochers = transactionsManager.GetVoucherType();
            return Json(vochers, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateTransactionMasterByMasterID(DateTime fromDate, int voucherTypeId)
        {
            var resetVochersViewModels = transactionsManager.UpdateTransactionMasterByMasterID(user.InstituteId, user.FinancialYearId, fromDate, voucherTypeId);
            return Json(resetVochersViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateTransactionMasterWithTable(ResetVocherSaveModel resetVochers)
        {
            var isUpdated = transactionsManager.UpdateTransactionMasterWithTable(user.InstituteId, user.FinancialYearId, resetVochers.VocherTypeId, resetVochers.NewResetVochers);
            return Json(isUpdated, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region  move transactions   
        public ActionResult MoveTransactions()
        {
            return View();
        }

        public JsonResult MoveTransactiontoOtherLedger(int fromLedgerId, int toLedgerId)
        {
            var isMoved = transactionsManager.MoveTransactiontoOtherLedtger(user.InstituteId, user.FinancialYearId, fromLedgerId, toLedgerId);
            return Json(isMoved, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}