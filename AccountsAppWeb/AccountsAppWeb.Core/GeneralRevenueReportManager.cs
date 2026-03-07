using AccountsAppWeb.Core.com.kccsasr.accounts;
using AccountsAppWeb.Core.Extensions;
using AccountsAppWeb.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsAppWeb.Core
{
    public class GeneralRevenueReportManager
    {
        DataTable dtAllCollegeNutShellValue = null;
        DataTable dtMyStatement = null;
        private readonly AccountsAppAPI accountsAppAPI;
        private int GridLeftHandFilledRowNo = 0, GridRightHandFilledRowNo = 0;
        private decimal CollectiveTotalCredit = 0, CollectiveTotalDebit = 0, MyBigValue = 0, TotalSCHD = 0;

        public GeneralRevenueReportManager()
        {
            accountsAppAPI = new AccountsAppAPI();
            dtAllCollegeNutShellValue = dtCollegeData();
            dtMyStatement = dtMyStatementdt();
        }
        public ConsolidatedGeneralRevenueRport GetGeneralRevenueManager(DateTime fromDate, DateTime toDate, int instituteId, int deptmentId, int finId, DateTime finStartDate)
        {
            AllCollegeLoopForGeneralValues(fromDate, toDate, instituteId, deptmentId, finId);
            int index1 = GridLeftHandFilledRowNo;
            if (GridRightHandFilledRowNo > GridLeftHandFilledRowNo)
                index1 = GridRightHandFilledRowNo;
            decimal num2 = 0;
            int num3;
            //CollectiveTotalDebit = 351338559.20M;
            if (CollectiveTotalCredit > CollectiveTotalDebit)
            {
                MyBigValue = CollectiveTotalCredit;
                num2 = CollectiveTotalCredit - CollectiveTotalDebit;
                dtAllCollegeNutShellValue.Rows[index1]["InstId"] = (object)"";
                dtAllCollegeNutShellValue.Rows[index1]["AccountGroupName1"] = (object)"To balance Carried down";
                dtAllCollegeNutShellValue.Rows[index1]["Debit"] = (object)num2.ToString();
                dtAllCollegeNutShellValue.AcceptChanges();
                num3 = index1 + 1;
            }
            else
            {
                MyBigValue = CollectiveTotalDebit;
                num2 = CollectiveTotalDebit - CollectiveTotalCredit;
                dtAllCollegeNutShellValue.Rows[index1]["InstId"] = (object)"";
                dtAllCollegeNutShellValue.Rows[index1]["AccountGroupName1"] = (object)"To balance Carried down";
                dtAllCollegeNutShellValue.Rows[index1]["Credit"] = (object)num2.ToString();
                dtAllCollegeNutShellValue.AcceptChanges();
                num3 = index1 + 1;
            }
            int index2 = num3 + 1;
            dtAllCollegeNutShellValue.Rows[index2]["InstId"] = (object)"";
            dtAllCollegeNutShellValue.Rows[index2]["Credit"] = (object)MyBigValue.ToString();
            dtAllCollegeNutShellValue.Rows[index2]["Debit"] = (object)MyBigValue.ToString();
            dtAllCollegeNutShellValue.Rows[index2]["IsBold"] = (object)true.ToString();
            dtAllCollegeNutShellValue.AcceptChanges();
            int index3 = index2 + 1 + 1;
            double num4 = Convert.ToDouble(Decimal.Round(Convert.ToDecimal(OpeningBalance(finStartDate, 3759, 300010, finId).Tables[0].Rows[0]["Debit"]), 2, MidpointRounding.AwayFromZero));
            double num5 = Convert.ToDouble(Decimal.Round(Convert.ToDecimal(num4 * 0.1), 2, MidpointRounding.AwayFromZero));
            Convert.ToDouble(Decimal.Round(Convert.ToDecimal(num4 - num5), 2, MidpointRounding.AwayFromZero));
            dtAllCollegeNutShellValue.Rows[index3]["InstId"] = (object)"0";
            dtAllCollegeNutShellValue.Rows[index3]["AccountGroupName1"] = (object)"To Depreciation on Moveable Assets";
            dtAllCollegeNutShellValue.Rows[index3]["Credit"] = (object)num5.ToString();
            dtAllCollegeNutShellValue.Rows[index3]["AccountGroupName2"] = (object)"By Balance brought down";
            dtAllCollegeNutShellValue.Rows[index3]["Debit"] = (object)num2.ToString();
            int index4 = index3 + 1;
            dtAllCollegeNutShellValue.Rows[index4]["InstId"] = (object)"0";
            dtAllCollegeNutShellValue.Rows[index4]["AccountGroupName1"] = (object)"To Amount trfd. to Property Equalisation fund";
            dtAllCollegeNutShellValue.Rows[index4]["Credit"] = (object)"";
            dtAllCollegeNutShellValue.Rows[index4]["AccountGroupName2"] = (object)"By Capital Exp. as per Sch-D";
            dtAllCollegeNutShellValue.Rows[index4]["Debit"] = (object)TotalSCHD.ToString();
            int index5 = index4 + 1;
            dtAllCollegeNutShellValue.AcceptChanges();
            dtAllCollegeNutShellValue.Rows[index5]["InstId"] = (object)"0";
            dtAllCollegeNutShellValue.Rows[index5]["AccountGroupName1"] = (object)"To Balance Carried to General Fund";
            DataRow row1 = dtAllCollegeNutShellValue.Rows[index5];
            string index6 = "Credit";
            Decimal num6 = TotalSCHD + num2 - Convert.ToDecimal(num5);
            string str1 = num6.ToString();
            row1[index6] = (object)str1;
            dtAllCollegeNutShellValue.Rows[index5]["AccountGroupName2"] = (object)"";
            dtAllCollegeNutShellValue.Rows[index5]["Debit"] = (object)"";
            dtAllCollegeNutShellValue.AcceptChanges();
            int index7 = index5 + 1 + 1;
            dtAllCollegeNutShellValue.Rows[index7]["InstId"] = (object)"0";
            dtAllCollegeNutShellValue.Rows[index7]["AccountGroupName1"] = (object)"";
            DataRow row2 = dtAllCollegeNutShellValue.Rows[index7];
            string index8 = "Credit";
            num6 = TotalSCHD + num2;
            string str2 = num6.ToString();
            row2[index8] = (object)str2;
            dtAllCollegeNutShellValue.Rows[index7]["AccountGroupName2"] = (object)"";
            DataRow row3 = dtAllCollegeNutShellValue.Rows[index7];
            string index9 = "Debit";
            num6 = TotalSCHD + num2;
            string str3 = num6.ToString();
            row3[index9] = (object)str3;
            dtAllCollegeNutShellValue.Rows[index7]["IsBold"] = (object)true.ToString();
            dtAllCollegeNutShellValue.AcceptChanges();
            int index10 = index7 + 1;
            for (int index11 = index10; index11 <= 100; ++index11)
            {
                if (dtAllCollegeNutShellValue.Rows.Count > index10)
                    dtAllCollegeNutShellValue.Rows.Remove(dtAllCollegeNutShellValue.Rows[index10]);
            }
            dtAllCollegeNutShellValue.AcceptChanges();
            var consolidatedGeneralRevenueModelList = dtAllCollegeNutShellValue.DataTableToList<ConsolidatedGeneralRevenueModel>();
            DataRow row4 = dtMyStatement.NewRow();
            row4["InstId"] = (object)"";
            row4["AccountGroupName"] = (object)"Total";
            row4["Income"] = (object)null;
            row4["Expenses"] = (object)null;
            row4["SchD"] = (object)null;
            row4["ExpensesSchD"] = (object)null;
            row4["NetSaving"] = (object)null;
            row4["NetLoss"] = (object)null;
            row4["NetSavingSchD"] = (object)null;
            row4["NetLossSchD"] = (object)null;
            dtMyStatement.Rows.Add(row4);
            dtMyStatement.AcceptChanges();
            decimal num7 = 0, num8 = 0, num9 = 0, num10 = 0, num11 = 0, num12 = 0, num13 = 0, num14 = 0;
            int serialNo = 0;
            foreach (DataRow row5 in (InternalDataCollectionBase)dtMyStatement.Rows)
            {
                row5["Id"] = serialNo;
                if (row5["Income"] != DBNull.Value)
                    num7 += Convert.ToDecimal(row5["Income"]);
                if (row5["Expenses"] != DBNull.Value)
                    num8 += Convert.ToDecimal(row5["Expenses"]);
                if (row5["SchD"] != DBNull.Value)
                    num9 += Convert.ToDecimal(row5["SchD"]);
                if (row5["ExpensesSchD"] != DBNull.Value)
                    num10 += Convert.ToDecimal(row5["ExpensesSchD"]);
                if (row5["NetSaving"] != DBNull.Value)
                    num11 += Convert.ToDecimal(row5["NetSaving"]);
                if (row5["NetLoss"] != DBNull.Value)
                    num12 += Convert.ToDecimal(row5["NetLoss"]);
                if (row5["NetSavingSchD"] != DBNull.Value)
                    num13 += Convert.ToDecimal(row5["NetSavingSchD"]);
                if (row5["NetLossSchD"] != DBNull.Value)
                    num14 += Convert.ToDecimal(row5["NetLossSchD"]);
                serialNo += 1;
            }
            DataRow row6 = dtMyStatement.NewRow();
            row6["Id"] = serialNo;
            row6["InstId"] = (object)"";
            row6["AccountGroupName"] = (object)"Total";
            row6["Income"] = (object)num7;
            row6["Expenses"] = (object)num8;
            row6["SchD"] = (object)num9;
            row6["ExpensesSchD"] = (object)num10;
            row6["NetSaving"] = (object)num11;
            row6["NetLoss"] = (object)num12;
            row6["NetSavingSchD"] = (object)num13;
            row6["NetLossSchD"] = (object)num14;
            dtMyStatement.Rows.Add(row6);
            dtMyStatement.AcceptChanges();
            var statementList = dtMyStatement.DataTableToList<StatementRevenueModel>();
            ConsolidatedGeneralRevenueRport consolidatedGeneralRevenue = new ConsolidatedGeneralRevenueRport();
            consolidatedGeneralRevenue.consolidatedGeneralRevenues = consolidatedGeneralRevenueModelList;
            consolidatedGeneralRevenue.statementRevenues = statementList;
            return consolidatedGeneralRevenue;
        }

        private void AllCollegeLoopForGeneralValues(DateTime fromDate, DateTime toDate, int instituteId, int deptmentId, int finId)
        {
            AdminManager adminManager = new AdminManager();
            var departments = adminManager.GetDepartmentsList();
            foreach (var dept in departments)
            {
                BindCollegeListOfTotal(fromDate, toDate, finId, dept.Inst_Id, dept.Inst_Title);
            }
        }

        private void BindCollegeListOfTotal(DateTime fromDate, DateTime toDate, int finId, int selectedInstituteId, string selectedInstituteName)
        {
            decimal num1 = 0, num2 = 0, num3 = 0, num4 = 0, num5 = 0;
            string tguid = Guid.NewGuid().ToString();
            DataSet ds = CreateLedgerIncomeExpendituteCollegeGroup(true, false, true, fromDate, toDate, selectedInstituteId, 0, tguid, false, 0, finId);
            if (ds.Tables[0].Rows.Count == 0 && ds.Tables[1].Rows.Count == 0 && ds.Tables[2].Rows.Count == 0)
                return;
            DataRowCollection rows1 = ds.Tables[0].Rows;
            DataRowCollection rows2 = ds.Tables[1].Rows;
            for (int index = 0; index < rows1.Count; ++index)
            {
                if (rows1[index]["Debit"] != DBNull.Value)
                    num3 = Convert.ToDecimal(rows1[index]["Debit"]);
                if (rows1[index]["Credit"] != DBNull.Value)
                    num4 = Convert.ToDecimal(rows1[index]["Credit"]);
            }
            for (int index = 0; index < rows2.Count; ++index)
            {
                if (index <= rows1.Count - 1 && rows1[index]["Debit"] != DBNull.Value)
                    num5 = Convert.ToDecimal(rows2[index]["Debit"]);
            }
            Decimal num6 = num1 + num3;
            Decimal num7 = num2 + num4;
            TotalSCHD += num5;
            DataRow row = dtMyStatement.NewRow();
            row["InstId"] = selectedInstituteId;
            row["AccountGroupName"] = selectedInstituteName.Replace("&", "and");
            if (num4 > Decimal.Zero)
                row["Income"] = (object)num4.ToString();
            if (num3 > Decimal.Zero)
                row["Expenses"] = (object)num3.ToString();
            if (num5 > Decimal.Zero)
                row["SchD"] = (object)num5.ToString();
            if (num3 > Decimal.Zero || num5 > Decimal.Zero || num4 > Decimal.Zero)
            {
                row["ExpensesSchD"] = (object)(num3 - num5).ToString();
                if (num4 > num3)
                    row["NetSaving"] = (object)(num4 - num3).ToString();
                else
                    row["NetLoss"] = (object)(num3 - num4).ToString();
                if (num4 + num5 > num3)
                    row["NetSavingSchD"] = (object)(num4 + num5 - num3).ToString();
                else
                    row["NetLossSchD"] = (object)(num3 - (num4 + num5)).ToString();
            }
            dtMyStatement.Rows.Add(row);
            if (num6 > num7)
            {
                Decimal num8 = num6 - num7;
                dtAllCollegeNutShellValue.Rows[GridLeftHandFilledRowNo]["InstId"] = selectedInstituteId;
                dtAllCollegeNutShellValue.Rows[GridLeftHandFilledRowNo]["AccountGroupName1"] = (object)selectedInstituteName.Replace("&", "and");
                dtAllCollegeNutShellValue.Rows[GridLeftHandFilledRowNo]["Credit"] = (object)num8.ToString();
                ++GridLeftHandFilledRowNo;
                CollectiveTotalCredit += num8;
            }
            else if (num7 > num6)
            {
                Decimal num8 = num7 - num6;
                dtAllCollegeNutShellValue.Rows[GridRightHandFilledRowNo]["InstId"] = selectedInstituteId;
                dtAllCollegeNutShellValue.Rows[GridRightHandFilledRowNo]["AccountGroupName2"] = (object)selectedInstituteName.Replace("&", "and");
                dtAllCollegeNutShellValue.Rows[GridRightHandFilledRowNo]["Debit"] = (object)num8.ToString();
                ++GridRightHandFilledRowNo;
                CollectiveTotalDebit += num8;
            }
            dtAllCollegeNutShellValue.AcceptChanges();
        }      

        private DataSet CreateLedgerIncomeExpendituteCollegeGroup(bool IsShowZeroBalance, bool ShowOnlyOpeningBalaces, bool IsIncomeExpenditure, DateTime FromDate, DateTime ToDate, int ForInstId, int AccountGroupId, string Tguid, bool IsShowOnlyOpeningBalacesJas, int departmentId, int finId)
        {
            accountsAppAPI.Timeout = 100000;
            return accountsAppAPI.CreateLedgerIncomeExpendituteCollegeGroup(string.Empty, 1, IsIncomeExpenditure, ForInstId, departmentId, IsShowZeroBalance, finId, ShowOnlyOpeningBalaces, FromDate, ToDate, ForInstId, AccountGroupId, Tguid, IsShowOnlyOpeningBalacesJas);
        }
        private DataSet OpeningBalance(DateTime FromDate, int LedgerId, int ForInstId, int finId)
        {
            accountsAppAPI.Timeout = 100000;
            return accountsAppAPI.OpeningBalance(string.Empty, ForInstId, finId, ForInstId, FromDate, LedgerId, 1111111);
        }

        private DataTable dtMyStatementdt()
        {
            return new DataTable()
            {
                Columns = {
          {
            "InstId",
            typeof (string)
          },
          {
            "AccountGroupName",
            typeof (string)
          },
          {
            "Expenses",
            typeof (string)
          },
          {
            "ExpensesSchD",
            typeof (string)
          },
          {
            "Income",
            typeof (string)
          },
          {
            "SchD",
            typeof (string)
          },
          {
            "NetSaving",
            typeof (string)
          },
          {
            "NetSavingSchD",
            typeof (string)
          },
          {
            "NetLoss",
            typeof (string)
          },
          {
            "NetLossSchD",
            typeof (string)
          },
           {
            "Id",
            typeof (int)
          }
        }
            };
        }
        private DataTable dtCollegeData()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("InstId", typeof(string));
            dataTable.Columns.Add("AccountGroupName1", typeof(string));
            dataTable.Columns.Add("AccountGroupName2", typeof(string));
            dataTable.Columns.Add("Debit", typeof(string));
            dataTable.Columns.Add("Credit", typeof(string));
            dataTable.Columns.Add("IsBold", typeof(bool));
            dataTable.Columns.Add("Id", typeof(int));

            for (int index = 0; index <= 100; ++index)
            {
                DataRow row = dataTable.NewRow();
                row["InstId"] = (object)"";
                row["AccountGroupName1"] = (object)"";
                row["Credit"] = (object)"";
                row["AccountGroupName2"] = (object)"";
                row["Debit"] = (object)"";
                row["IsBold"] = (object)false.ToString();
                row["Id"] = index;
                dataTable.Rows.Add(row);
            }
            return dataTable;
        }

    }
}
