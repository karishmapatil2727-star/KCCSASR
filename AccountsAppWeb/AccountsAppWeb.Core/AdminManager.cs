using AccountsAppWeb.Core.com.kccsasr.accounts;
using AccountsAppWeb.Core.Models;
using System.Collections.Generic;
using System.Data;
using AccountsAppWeb.Core.Extensions;
using System;
using System.Linq;
using System.Data.SqlClient;
using System.Configuration;

namespace AccountsAppWeb.Core
{
    public class AdminManager
    {
        private readonly AccountsAppAPI accountsAppAPI;
        string sKey = string.Empty;
        public AdminManager()
        {
            accountsAppAPI = new AccountsAppAPI();
        }

        #region account ledger
        public AccountLedgerModel GetAccountLedger(int instituteId, int financialYearId, int deptId, int ledgerId, int showInTransactionPage)
        {
            DataSet dSet = accountsAppAPI.AccountLedgerForTransaction(sKey, instituteId, financialYearId, deptId, 1, showInTransactionPage);
            if (dSet != null)
            {
                DataTable dt = dSet.Tables[0].Select("ForInstId=" + instituteId).CopyToDataTable<DataRow>();
                var accountLedgerLists = dt.DataTableToList<AccountLedgerModel>();
                return accountLedgerLists.Where(rec => rec.LedgerId == ledgerId).FirstOrDefault();
            }
            else
                return new AccountLedgerModel();
        }
        public List<AccountLedgerListViewModel> GetAccountLedgerList(int instituteId, int financialYearId, int deptId, int showInTransactionPage)
        {
            DataSet dSet = accountsAppAPI.AccountLedgerForTransaction(sKey, instituteId, financialYearId, deptId, 1, showInTransactionPage);
            if (dSet != null)
                return dSet.Tables[0].DataTableToList<AccountLedgerListViewModel>();
            else
                return new List<AccountLedgerListViewModel>();
        }
        public bool IsAccountLedgerIsAlreadyExist(int instituteId, string ledgeName, int ledgerId)
        {
            return accountsAppAPI.IsAccountLedgerIsAlreadyExist(sKey, instituteId, ledgeName, ledgerId, instituteId);
        }
        public string DeleteAccountLedger(int ledgerId, int instituteId)
        {
            DataTable dt = accountsAppAPI.AccountLedgerDeleteById(sKey, ledgerId, instituteId);
            if (dt != null)
            {
                return dt.Rows[0][1].ToString();
            }
            return string.Empty;
        }
        public List<DepartmentModel> GetDepartmentsList()
        {
            DataSet dSet = accountsAppAPI.InstDepartmentList(sKey);
            if (dSet != null)
                return dSet.Tables[0].DataTableToList<DepartmentModel>();
            return new List<DepartmentModel>();
        }
        public List<FinancialYearModel> GetFinancialyear(int instituteId)
        {
            DataSet dataSet = accountsAppAPI.GetLoginFinancialID(sKey, instituteId);
            if (dataSet != null)
                return dataSet.Tables[0].DataTableToList<FinancialYearModel>();
            return new List<FinancialYearModel>();
        }
        public bool InsertAccountLedger(int userId, int departmentID, int instituteId, int financialYearId, bool IsModify, AccountLedgerModel ledgerModel)
        {
            AccountLedgerMaster alm = new AccountLedgerMaster()
            {
                AccountGroupId = ledgerModel.AccountGroupId,
                LedgerName = ledgerModel.LedgerName,
                LedgerNameAlias = ledgerModel.LedgerName,
                LedgerNamePrint = ledgerModel.LedgerName,
                IsAdminLedger = true,
                IsDefault = true,
                CrOrDr = ledgerModel.CrOrDr,
                IsActive = true,
                InsertUserAccountId = userId,
                InsertDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdateUserAccountId = userId,
                UserAccountId = userId,
                Narration = ledgerModel.Narration,
                Address = ledgerModel.Address,
                Mobile = ledgerModel.Mobile,
                TIN = ledgerModel.TIN,
                CST = ledgerModel.CST,
                PAN = ledgerModel.PAN,
            };

            alm.IsAdminLedger = false;
            alm.LedgerId = ledgerModel.LedgerId;
            alm.IsUnderSecretary = false;
            alm.IsShowMainCollege = false;
            string crOrDr = alm.CrOrDr;
            bool flag = accountsAppAPI.InsertAccountLedger(sKey, alm, new AccountLedger()
            {
                DepartmentId = departmentID,
                InstId = instituteId,
                ForInstId = instituteId,
                LedgerId = 0
            }, new AccountLedgerOpeningBalance()
            {
                OB_InstId = instituteId,
                OB_DepartmentId = departmentID,
                OB_FinancialYearId = financialYearId,
                OB_CrOrDr = crOrDr,
                OB_OpeningBalance = ledgerModel.OpeningBalance,
                OB_InsertDate = DateTime.Now,
                OB_ForInstId = instituteId
            }, IsModify, instituteId, financialYearId, ledgerModel.GSTNo, ledgerModel.IsGSTSales,ledgerModel.HSNCode,ledgerModel.IsItem);

            if (IsModify)
            {
                accountsAppAPI.UpdateAccountLedger(sKey, instituteId, instituteId, instituteId, ledgerModel.LedgerId, ledgerModel.OpeningBalance, financialYearId);
            }
            return flag;
        }
        #endregion

        #region accountgroup
        public List<AccountGroupListModel> GetAccountGroupsList(int instituteId, int showInLedger, int financialYearId)
        {
            DataSet dSet = accountsAppAPI.GetAccountGroup(sKey, instituteId, showInLedger, financialYearId);
            if (dSet != null)
                return dSet.Tables[0].DataTableToList<AccountGroupListModel>();
            return new List<AccountGroupListModel>();
        }
        public List<NatureModel> GetAccountNatureList(int instituteId)
        {
            DataSet dSet = accountsAppAPI.AccountGroupNature(sKey);
            if (dSet != null)
                return dSet.Tables[0].DataTableToList<NatureModel>();
            return new List<NatureModel>();
        }
        public bool IsAccountGroupIsAlreadyExist(int instituteId, string groupName, int groupId)
        {
            return accountsAppAPI.IsAccountGroupIsAlreadyExist(sKey, instituteId, groupName, groupId);
        }
        public bool InsertAccountGroup(int userId, int instituteId, bool IsModify, AccountGroupModel groupModel, int financialYearId)
        {
            return accountsAppAPI.InsertAccountGroup(sKey, new AccountGroupMaster()
            {
                AccountGroupId = groupModel.AccountGroupId,
                AccountGroupName = groupModel.AccountGroupName,
                AccountGroupNameAlias = groupModel.AccountGroupNameAlias,
                GroupUnder = groupModel.GroupUnder,
                IsDefault = true,
                Nature = groupModel.Nature,
                AffectGrossProfit = true,
                InsertUserAccountId = userId,
                InsertDate = DateTime.Now,
                UpdateUserAccountId = userId,
                UpdateDate = DateTime.Now,
                IsAdminGroup = false
            }, instituteId, IsModify, groupModel.IsCommonGroup, financialYearId, groupModel.IsParty);
        }
        public AccountGroupModel GetAccountGroup(int instituteId, int groupId, int showInLedger, int financialYearId)
        {
            DataSet dSet = accountsAppAPI.GetAccountGroup(sKey, instituteId, showInLedger, financialYearId);
            if (dSet != null)
            {
                var accountGroupList = dSet.Tables[0].DataTableToList<AccountGroupModel>();
                return accountGroupList.Where(rec => rec.AccountGroupId == groupId).FirstOrDefault();              
            }
            else
                return new AccountGroupModel();
        }
        public string DeleteAccountGroup(int groupId, int instituteId)
        {
            DataTable dt = accountsAppAPI.AccountGroupDeleteById(sKey, groupId, instituteId);
            if (dt != null)
            {
                return dt.Rows[0][1].ToString();
            }
            return string.Empty;
        }

        public string EnableAccountGroup(int groupId, string btnText)
        {
            int IsEnable = btnText.Equals("Enable") ? 1 : 0;
            DataTable dt = accountsAppAPI.AccountGroupEnableById(sKey, groupId, IsEnable);
            if (dt != null)
            {
                return dt.Rows[0][1].ToString();
            }
            return string.Empty;
        }
        public string EnableAccountLedger(int ledgerId, string btnText)
        {
            int IsEnable = btnText.Equals("Enable") ? 1 : 0;
            DataTable dt = accountsAppAPI.AccountLedgerEnableById(sKey, ledgerId, IsEnable);
            if (dt != null)
            {
                return dt.Rows[0][1].ToString();
            }
            return string.Empty;
        }
        #endregion


        #region Notification Ledger Master
        public List<NotificationPendingLedger> NotificationPendingLedgerList(int instituteId)
        {
            DataSet dataSet = accountsAppAPI.NotificationPendingLedger(sKey, instituteId);
            if (dataSet != null)
                return dataSet.Tables[0].DataTableToList<NotificationPendingLedger>();
            else
                return new List<NotificationPendingLedger>();
        }
        public List<NotificationConfirmedLedgerModel> NotificationConfirmedLedgerList(int instituteId)
        {
            DataSet dataSet = accountsAppAPI.NotificationConfirmedLedger(sKey, instituteId);
            if (dataSet != null)
                return dataSet.Tables[0].DataTableToList<NotificationConfirmedLedgerModel>();
            else
                return new List<NotificationConfirmedLedgerModel>();
        }
        public void NotificationPendingLedgerUpdate(int notificationInstId, int Id)
        {
            accountsAppAPI.NotificationConfirmedUpdate(sKey, notificationInstId, Id);
        }
        public void NotificationConfirmedLedgerDelete(int Id)
        {
            accountsAppAPI.NotificationConfirmedDelete(sKey, Id);
        }
        #endregion
        #region Reconciliation Master
        public List<ReconciliationLedgerModel> ReconciliationLedgerList(int instituteId)
        {
            DataSet dataSet = accountsAppAPI.ReconciliationLedgerList(sKey, instituteId);
            if (dataSet != null)
                return dataSet.Tables[0].DataTableToList<ReconciliationLedgerModel>();
            else
                return new List<ReconciliationLedgerModel>();
        }
        public void ReconciliationLedgerConfirmedUpdate(int notificationToLedgerId, int id)
        {
            accountsAppAPI.ReconciliationLedgerConfirmedUpdate(sKey, notificationToLedgerId, id);
        }
        public void ReconciliationLedgerConfirmedDelete(int id)
        {
            accountsAppAPI.ReconciliationLedgerConfirmedDelete(sKey, id);
        }
        #endregion

        #region Negative balance
        public List<NegativeCashBalanceModel> ViewNegativeCashBalance(int instituteId, int financialId)
        {
            DataSet dataSet = accountsAppAPI.NegativeCashBalance(sKey, instituteId, financialId);
            if (dataSet != null)
                return dataSet.Tables[0].DataTableToList<NegativeCashBalanceModel>();
            else
                return new List<NegativeCashBalanceModel>();
        }
        #endregion

        #region balance transfer
        public bool BalanceTransferEndYear(int financialId, int instituteId, DateTime finStartDate, DateTime finEndDate)
        {
            return accountsAppAPI.BalanceTransferEndYear(sKey, finStartDate, finEndDate, financialId, instituteId);
        }
        #endregion

        public List<TransactionPermissionsModel> LoadTransactionPermissions(int financialId)
        {
            DataSet dataSet = accountsAppAPI.TransactionPermissionSelect(sKey, financialId);
            if (dataSet != null)
                return dataSet.Tables[0].DataTableToList<TransactionPermissionsModel>();
            else
                return new List<TransactionPermissionsModel>();
        }

        public bool UpdateTransactionPermissions(int financialId, int instituteId, bool isTransactionAddAllow, bool isTransactionEditAllow, bool isOpeningBalanceEditAllow, bool isNewLedgerAddAllow)
        {
            return accountsAppAPI.TransactionPermissionUpdate(sKey, financialId, instituteId, isTransactionAddAllow, isTransactionEditAllow, isOpeningBalanceEditAllow, isNewLedgerAddAllow);
        }

        public List<TableContentModel> LoadTableContent(int instituteId, int financialId, DateTime yearStartDate, DateTime toDate)
        {
            DataSet ds = accountsAppAPI.OpeningTrialDetailed(sKey, instituteId, financialId, instituteId, 0, yearStartDate, toDate.AddDays(1.0));
            var tableContent = ds.Tables[0].DataTableToList<TableContentModel>();
            ds = accountsAppAPI.LedgerTableConrentLoadData(sKey, instituteId, financialId);

            foreach (var record in tableContent)
            {
                DataRow[] dataRowArray = ds.Tables[0].Select("TC_LedgerId=" + Convert.ToInt32(record.AccountId).ToString());
                if ((uint)dataRowArray.Length > 0U)
                    record.PageNumber = dataRowArray[0]["TC_PageNo"].ToString();
            }
            return tableContent;
        }
        public bool SaveableContent(int instituteId, int financialId, List<TableContentModel> tableContentModels)
        {
            DataTable table = CreateTable();
            foreach (var record in tableContentModels)
            {
                int int32 = record.AccountId;
                string str = !string.IsNullOrEmpty(record.PageNumber) ? record.PageNumber : "";
                DataRow row2 = table.NewRow();
                row2["StringColumn1"] = (object)int32.ToString();
                row2["StringColumn2"] = (object)str.ToString();
                table.Rows.Add(row2);
                table.AcceptChanges();
            }
            return accountsAppAPI.LedgerTableConrent(sKey, instituteId, financialId, table);
        }

        private DataTable CreateTable()
        {
            return new DataTable("Menntal")
            {
                Columns = {
          {
            "StringColumn1",
            Type.GetType("System.String")
          },
          {
            "StringColumn2",
            Type.GetType("System.String")
          }
        }
            };
        }

        public List<AccountLedgerListViewModel> GetPartyList(string sKey, int instituteId, int financialYearId)
        {
            try
            {
                string connStr = System.Configuration.ConfigurationManager
                                    .ConnectionStrings["DataContext"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[Accounts].[GetPartyLedgersForGST]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InstId", instituteId);
                    cmd.Parameters.AddWithValue("@FinancialYearId", financialYearId);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds != null && ds.Tables.Count > 0)
                        return ds.Tables[0].DataTableToList<AccountLedgerListViewModel>();
                    return new List<AccountLedgerListViewModel>();
                }
            }
            catch (Exception ex)
            {
                throw ex;   
            }
        }

        public List<GSTItemModel> GetItemsByIncomeGSTSalesGroup(string sKey, int instituteId, int financialYearId)
        {
            try
            {
                string connStr = System.Configuration.ConfigurationManager
                                    .ConnectionStrings["DataContext"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[Accounts].[GetItemsByIncomeGSTSalesGroup]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InstId", instituteId);
                    cmd.Parameters.AddWithValue("@FinancialYearId", financialYearId);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds != null && ds.Tables.Count > 0)
                        return ds.Tables[0].DataTableToList<GSTItemModel>();
                    return new List<GSTItemModel>();
                }
            }
            catch (Exception ex)
            {
                throw ex;   
            }
        }
    }
}
