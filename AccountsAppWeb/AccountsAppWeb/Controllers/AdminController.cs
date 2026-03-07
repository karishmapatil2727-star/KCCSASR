using AccountsAppWeb.Core;
using AccountsAppWeb.Core.Models;
using AccountsAppWeb.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AccountsAppWeb.Controllers
{
    [LogonAuthorize]
    public class AdminController : BaseController
    {
        private UserModel user;
        private AdminManager adminManager;

        public AdminController()
        {
            user = UserManager.User;
            adminManager = new AdminManager();
        }
        [OutputCache(Duration = 20)]
        public JsonResult GetDepartmentsList()
        {
            var deptList = adminManager.GetDepartmentsList();
            return Json(deptList, JsonRequestBehavior.AllowGet);
        }

        #region account ledger
        // GET: Admin
        public ActionResult AccountLedger()
        {
            return View();
        }
        [OutputCache(Duration = 20)]
        public JsonResult GetFinancialyear()
        {
            var financialYears = adminManager.GetFinancialyear(UserManager.User.InstituteId);            
            return Json(financialYears, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAccountLedgerList(int showInTransactionPage=0)
        {
            var accountLedgerList = adminManager.GetAccountLedgerList(UserManager.User.InstituteId, user.FinancialYearId, UserManager.User.DepartmentID, showInTransactionPage);
            return Json(accountLedgerList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAccountLedger(int ledgerId, int showInTransactionPage = 0)
        {
            var accountLedger = adminManager.GetAccountLedger(UserManager.User.InstituteId, user.FinancialYearId, UserManager.User.DepartmentID, ledgerId, showInTransactionPage);
            return Json(accountLedger, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteAccountLedger(int ledgerId)
        {
            var status = adminManager.DeleteAccountLedger(ledgerId, user.InstituteId);
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateOrUpdateAccountLedger(AccountLedgerModel accountLedgerModel)
        {
            bool isModify = false;
            if (accountLedgerModel.LedgerId == 0)
            {
                var isLedgerExists = adminManager.IsAccountLedgerIsAlreadyExist(UserManager.User.InstituteId, accountLedgerModel.LedgerName, accountLedgerModel.LedgerId);
                if (isLedgerExists)
                {
                    return Json("Ledger name already exists.Please choose different name", JsonRequestBehavior.AllowGet);
                }
                var isInserted = adminManager.InsertAccountLedger(Convert.ToInt32(user.UserName), user.DepartmentID, user.InstituteId, user.FinancialYearId, isModify, accountLedgerModel);
                if (!isInserted)
                {
                    return Json("Failed to insert the account ledger", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                isModify = true;
                var isUpdated = adminManager.InsertAccountLedger(Convert.ToInt32(user.UserName), user.DepartmentID, user.InstituteId, user.FinancialYearId, isModify, accountLedgerModel);
                if (!isUpdated)
                {
                    return Json("Failed to insert the account ledger", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Account Group
        public ActionResult AccountGroup()
        {
            return View();
        }
        public JsonResult GetAccountGroupsList(int showInLedger, int financialYearId)
        {
            var groupList = adminManager.GetAccountGroupsList(UserManager.User.InstituteId, showInLedger,financialYearId);
            return Json(groupList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNatureGroupByAll()
        {
            var natureList = adminManager.GetAccountNatureList(UserManager.User.InstituteId);
            return Json(natureList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAccountGroup(int groupId, int showInLedger, int financialYearId)
        {
            var accountLedger = adminManager.GetAccountGroup(UserManager.User.InstituteId, groupId, showInLedger,financialYearId);
            return Json(accountLedger, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteAccountGroup(int groupId)
        {
            var status = adminManager.DeleteAccountGroup(groupId, user.InstituteId);
            return Json(status, JsonRequestBehavior.AllowGet);
        }
       
        public JsonResult EnableAccountGroup(int groupId,string btnText)
        {
            var status = adminManager.EnableAccountGroup(groupId, btnText);
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EnableAccountLedger(int ledgerId, string btnText)
        {
            var status = adminManager.EnableAccountLedger(ledgerId, btnText);
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateOrUpdateAccountGroup(AccountGroupModel accountGroupModel)
        {
            bool isModify = false;
            if (accountGroupModel.AccountGroupId == 0)
            {
                var isLedgerExists = adminManager.IsAccountGroupIsAlreadyExist(UserManager.User.InstituteId, accountGroupModel.AccountGroupName, accountGroupModel.AccountGroupId);
                if (isLedgerExists)
                {
                    return Json("Group name already exists.Please choose different name", JsonRequestBehavior.AllowGet);
                }
                var isInserted = adminManager.InsertAccountGroup(Convert.ToInt32(user.UserName), user.InstituteId, isModify, accountGroupModel,user.FinancialYearId);
                if (!isInserted)
                {
                    return Json("Failed to insert the account group", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                isModify = true;
                var isUpdated = adminManager.InsertAccountGroup(Convert.ToInt32(user.UserName), user.InstituteId, isModify, accountGroupModel, user.FinancialYearId);
                if (!isUpdated)
                {
                    return Json("Failed to insert the account group", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Notification Ledger Master
        public ActionResult NotificationLedgerMaster()
        {
            return View();
        }
        public JsonResult GetNotificationPendingLedgerList()
        {
            var pendingLedgerList = adminManager.NotificationPendingLedgerList(user.InstituteId);
            return Json(pendingLedgerList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNotificationConfirmedLedgerList()
        {
            var confirmedLedgerList = adminManager.NotificationConfirmedLedgerList(user.InstituteId);
            return Json(confirmedLedgerList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult NotificationPendingLedgerUpdate(int notificationInstId, int Id)
        {
            adminManager.NotificationPendingLedgerUpdate(notificationInstId, Id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult NotificationConfirmedLedgerDelete(int Id)
        {
            adminManager.NotificationConfirmedLedgerDelete(Id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Reconciliation Master
        public ActionResult ReconciliationMaster()
        {
            return View();
        }

        public JsonResult GetReconciliationLedgerList()
        {
            var reconcilationList = adminManager.ReconciliationLedgerList(user.InstituteId);
            return Json(reconcilationList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReconciliationLedgerConfirmedUpdate(int notificationToLedgerId, int id)
        {
            adminManager.ReconciliationLedgerConfirmedUpdate(notificationToLedgerId, id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReconciliationLedgerConfirmedDelete(int id)
        {
            adminManager.ReconciliationLedgerConfirmedDelete(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region negative-balance
        public ActionResult NegativeBalance()
        {
            return View();
        }

        public JsonResult GetNegativeCashBalance()
        {
            var negativeblance = adminManager.ViewNegativeCashBalance(user.InstituteId, user.FinancialYearId);
            return Json(negativeblance, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region balance transfer
        public ActionResult BalanceTransfer()
        {
            return View();
        }
        public JsonResult GetNextYear()
        {
            var financialYears = adminManager.GetFinancialyear(user.InstituteId);
            var nextYear = financialYears.Where(rec => rec.Fin_ID == user.FinancialYearId + 1).FirstOrDefault();
            if (nextYear != null)
            {
                return Json(nextYear.Fin_Title, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult BalanceTransferEndYear()
        {
            var isSuccess = adminManager.BalanceTransferEndYear(user.FinancialYearId, user.InstituteId, user.FinancialYearStartDate, user.FinancialYearEndDate.AddDays(1.0));
            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Transaction Permissions
        public ActionResult TransactionPermissions()
        {
            return View();
        }
        public JsonResult LoadTransactionPermissions()
        {
            var transactionPermissions = adminManager.LoadTransactionPermissions(user.FinancialYearId);
            return Json(transactionPermissions, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateTransactionPermissions(int instituteId, bool isTransactionAddAllow, bool isTransactionEditAllow, bool isOpeningBalanceEditAllow, bool isNewLedgerAddAllow)
        {
            var isUpdated = adminManager.UpdateTransactionPermissions(user.FinancialYearId, instituteId, isTransactionAddAllow, isTransactionEditAllow, isOpeningBalanceEditAllow, isNewLedgerAddAllow);
            if(isUpdated)
            {
                var model = User;
                user.IsTransactionAddAllow = isTransactionAddAllow;
                user.IsOpeningBalanceEditAllow = isOpeningBalanceEditAllow;
                user.IsTransactionAddAllow = isTransactionAddAllow;
                user.IsTransactionEditAllow = isTransactionEditAllow;
                UpdateAuthCookie(user);
            }
            return Json(isUpdated, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Table Content
        public ActionResult TableContent()
        {
            return View();
        }

        public JsonResult LoadTableContent(DateTime toDate)
        {           
           // DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var tableContents = adminManager.LoadTableContent(user.InstituteId, user.FinancialYearId, user.FinancialYearStartDate, toDate);
            return Json(tableContents, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveableContent(List<TableContentModel> tableContentModels)
        {
            var isSaved = adminManager.SaveableContent(user.InstituteId, user.FinancialYearId, tableContentModels);
            return Json(isSaved, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //public JsonResult GetPartyList(int showInTransactionPage = 0)
        //{
        //    var accountLedgerList = adminManager.GetPartyList(UserManager.User.InstituteId, user.FinancialYearId, UserManager.User.DepartmentID, showInTransactionPage);
        //    return Json(accountLedgerList, JsonRequestBehavior.AllowGet);
        //}
    }
}