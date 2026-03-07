using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;

namespace ESchoolDAL
{

    public class Payroll 
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString);
    
        SqlParameter[] param;
        DataSet ds = new DataSet();
        public DataSet AllowanceSelectByFacUserId(int facUserId,int instId)
        {
            param = new SqlParameter[2];
            param[0] = new SqlParameter("@facUserId", facUserId);
            param[1] = new SqlParameter("@instId", instId);
            ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "PayrollAllowanceSelectByFacUserId", param);
            return ds;
        }

        public DataSet AllowanceInsert(SqlParameter[] param)
        {
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "PayrollAllowanceInsert", param);
            return ds;
        }

        public DataSet DeductionSelectByFacUserId(int facUserId, int instId)
        {
            param = new SqlParameter[2];
            param[0] = new SqlParameter("@facUserId", facUserId);
            param[1] = new SqlParameter("@instId", instId);
            return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "PayrollDeductionSelectByFacUserId", param);
        }
        public DataSet FacultyListInstWise(int InstID)
        {
            param = new SqlParameter[1];
            param[0] = new SqlParameter("@InstID", InstID);
            return ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "FacultyInfoSelectByInstId", param);
        }
        public DataSet InstitutePrincipalList()
        {

            return ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "InstitutePrincipalList", param);
        }

        public DataSet Common_MonthList()
        {
            return ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Common_MonthList");
        }

        public void CalculateSalary(int InstID, int FacUserID, int MID, int FinID, int InsertUserID, string Remarks, string AbsentDays, int ExtraAllowance, int ExtraDeduction, bool ISEPFDeduction, bool ISESIDeduction, int ElectricCharges, int IncomeTax, string IsFromAtt)
        {
            param = new SqlParameter[14];
            param[0] = new SqlParameter("@InstID", InstID);
            param[1] = new SqlParameter("@FinID", FinID);
            param[2] = new SqlParameter("@MID", MID);
            param[3] = new SqlParameter("@FacUserID", FacUserID);
            param[4] = new SqlParameter("@InsertUserID", InsertUserID);
            param[5] = new SqlParameter("@Remarks", Remarks);
            param[6] = new SqlParameter("@AbsentDays", AbsentDays);
            param[7] = new SqlParameter("@ExtraAllowance", ExtraAllowance);
            param[8] = new SqlParameter("@ExtraDeduction", ExtraDeduction);
            param[9] = new SqlParameter("@ISEPFDeduction", ISEPFDeduction);
            param[10] = new SqlParameter("@ISESIDeduction", ISESIDeduction);
            param[11] = new SqlParameter("@ElectricCharges", ElectricCharges);
            param[12] = new SqlParameter("@IncomeTax", IncomeTax);
            param[13] = new SqlParameter("@IsFromAtt", IsFromAtt);
            
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Payroll_CalculateSalary", param);
        }

        public DataSet Payroll_SalaryDetailUserWise(int FacUserID, int MID, int FinID)
        {
            param = new SqlParameter[3];
            param[0] = new SqlParameter("@FacUserID", FacUserID);
            param[1] = new SqlParameter("@MID", MID);
            param[2] = new SqlParameter("@FinID", FinID);
            return ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Payroll_SalaryDetailUserWise", param);
        }

        public DataSet Payroll_BasicPayUserWise(int FacUserID, int instId)
        {
            param = new SqlParameter[2];
            param[0] = new SqlParameter("@FacUserID", FacUserID);
            param[1] = new SqlParameter("@instId", instId);
            return ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Payroll_BasicPayUserWise", param);
        }


        /************************** Ledger ****************************/
        public DataSet PayRollUserWiseLedger(int InstID,int FinId,string Type,int JobCategoryId)
        {
            param = new SqlParameter[4];
            param[0] = new SqlParameter("@InstId", InstID);
            param[1] = new SqlParameter("@FinId", FinId);
            param[2] = new SqlParameter("@Type", Type);
            param[3] = new SqlParameter("@JobCategoryId", JobCategoryId);
            return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "PayRollUserWiseLedger", param);
        }

        public DataSet PayRollUserWiseLedgerUser(int InstID, int FinId, int UserID)
        {
            param = new SqlParameter[3];
            param[0] = new SqlParameter("@InstId", InstID);
            param[1] = new SqlParameter("@FinId", FinId);
            param[2] = new SqlParameter("@FacUserID", UserID);
            return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "PayRollUserWiseLedgerUser", param);
        }

        public DataSet PayRollMonthWiseLedger(int InstID, int FacUserId,int FinId)
        {
            param = new SqlParameter[3];
            param[0] = new SqlParameter("@InstId", InstID);
            param[1] = new SqlParameter("@FacUserId", FacUserId);
            param[2] = new SqlParameter("@FinId", FinId);
            return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "PayRollMonthWiseLedger", param);
        }

        public DataSet PayRollLedgerMonthly(int InstID, int MID, int FinId, string Type, int JobCategoryId)
        {
            param = new SqlParameter[5];
            param[0] = new SqlParameter("@InstId", InstID);
            param[1] = new SqlParameter("@MID", MID);
            param[2] = new SqlParameter("@FinId", FinId);
            param[3] = new SqlParameter("@Type", Type);
            param[4] = new SqlParameter("@JobCategoryId", JobCategoryId);
            return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "PayRollLedgerMonthly", param);
        }

        //public DataSet Payroll_SalaryDetailUserWise(int FacUserID, int MID, int FinID)
        //{
        //    param = new SqlParameter[3];
        //    param[0] = new SqlParameter("@FacUserID", FacUserID);
        //    param[1] = new SqlParameter("@MID", MID);
        //    param[2] = new SqlParameter("@FinID", FinID);
        //    return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Payroll_SalaryDetailUserWise", param);
        //}

        public void Payroll_SalaryConfirm(int AA_Id)
        {
            param = new SqlParameter[1];
            param[0] = new SqlParameter("@AA_Id", AA_Id);
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Payroll_SalaryConfirm", param);
        }
        public void Payroll_SalaryConfirmTransferMonth(int AA_Id, int MonthId)
        {
            param = new SqlParameter[2];
            param[0] = new SqlParameter("@AA_Id", AA_Id);
            param[1] = new SqlParameter("@MonthID", MonthId);
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Payroll_SalaryConfirmTransferMonth", param);
        }

        public DataSet Payroll_SalaryUnconfirmDetail(int FacUserID, int MID, int FinId)
        {
            param = new SqlParameter[3];
            param[0] = new SqlParameter("@FacUserID", FacUserID);
            param[1] = new SqlParameter("@MID", MID);
            param[2] = new SqlParameter("@FinId", FinId);
            return ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Payroll_SalaryDetailForUnlock", param);

        }

        public void Payroll_SalaryUnConfirm(int AA_Id)
        {
            param = new SqlParameter[1];
            param[0] = new SqlParameter("@AA_Id", AA_Id);
            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Payroll_SalaryUnConfirm", param);
        }

        public DataSet InstitutionList()
        {
            return ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Payroll_InstitutionList");
        }
        public DataSet DepartmentList()
        {
            return ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Payroll_DepartmentList");
        }
        public DataSet PayRollFacultyReport(int ReportTypeID,string FacType,int DesigID,int InstID, int FinID)
        {
            param = new SqlParameter[5];
            param[0] = new SqlParameter("@InstId", InstID);
            param[1] = new SqlParameter("@ReportTypeID", ReportTypeID);
            param[2] = new SqlParameter("@FinID", FinID);
            param[3] = new SqlParameter("@FDID", DesigID);
            param[4] = new SqlParameter("@FacType", FacType);
            return ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "PayRollFacultyReport",param);
        }

        public DataSet usp_PayrollEPFDeductionReport(int InstId, int FinId, int MonthId)
        {
            param = new SqlParameter[3];
            param[0] = new SqlParameter("@InstId", InstId);
            param[1] = new SqlParameter("@FinId", FinId);
            param[2] = new SqlParameter("@MonthId", MonthId);
            return ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_PayrollEPFDeductionReport", param);
        }

        public DataSet PayRollFacultyReportSalaryWise(int InstID,int FinID, int ReportTypeID, int RangeTypeID, int? BasicPayFrom, int? BasicPayTo, int? BasicPayUpto)
        {
            param = new SqlParameter[7];
            param[0] = new SqlParameter("@InstId", InstID);
            param[1] = new SqlParameter("@ReportTypeID", ReportTypeID);
            param[2] = new SqlParameter("@RangeTypeID", RangeTypeID);
            param[3] = new SqlParameter("@BasicPayFrom", BasicPayFrom);
            param[4] = new SqlParameter("@BasicPayto", BasicPayTo);
            param[5] = new SqlParameter("@BasicPayUpto", BasicPayUpto);
            param[6] = new SqlParameter("@FinID", FinID);
            return ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "PayRollFacultyReportSalaryWise", param);
        }

        public DataSet Common_FinancialYear()
        {
            return ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Common_FinancialYear", param);
        }

        public DataSet Payroll_AdminReport(int FinID, int InstID)
        {
            param = new SqlParameter[2];
            param[0] = new SqlParameter("@FinID", FinID);
            param[1] = new SqlParameter("@InstID", InstID);
            return ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Payroll_AdminReport", param);            
        }

        public void CalculateSalary_DateWise(int InstID, int FacUserID, int MID, int FinID, int InsertUserID, string Remarks, decimal AbsentDays, decimal ExtraAllowance, decimal ExtraDeduction, string StartDate, string EndDate, int TotalDays, int ElectricCharges, int IncomeTax)
        {
            param = new SqlParameter[14];
            param[0] = new SqlParameter("@InstID", InstID);
            param[1] = new SqlParameter("@FinID", FinID);
            param[2] = new SqlParameter("@MID", MID);
            param[3] = new SqlParameter("@FacUserID", FacUserID);
            param[4] = new SqlParameter("@InsertUserID", InsertUserID);
            param[5] = new SqlParameter("@Remarks", Remarks);
            param[6] = new SqlParameter("@AbsentDays", AbsentDays);
            param[7] = new SqlParameter("@ExtraAllowance", ExtraAllowance);
            param[8] = new SqlParameter("@ExtraDeduction", ExtraDeduction);
            param[9] = new SqlParameter("@StartDate", StartDate);
            param[10] = new SqlParameter("@EndDate", EndDate);
            param[11] = new SqlParameter("@TotalDays", TotalDays);
            param[12] = new SqlParameter("@ElectricCharges", ElectricCharges);
            param[13] = new SqlParameter("@IncomeTax", IncomeTax);

            SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "Payroll_CalculateSalary_DateWise", param);
        }


        public DataSet Payroll_SalaryDetailMonthWise(int InstId, int MID, int FinID,int JobCategoryId)
        {
            param = new SqlParameter[4];
            param[0] = new SqlParameter("@InstId", InstId);
            param[1] = new SqlParameter("@MID", MID);
            param[2] = new SqlParameter("@FinID", FinID);
            param[3] = new SqlParameter("@JobCategoryId", JobCategoryId);
           
            
             return ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Payroll_SalaryDetailMonthWise", param);
        }


        public DataSet Payroll_BindFacultiesList(int InstId, int MonthId, int FinID, int JobCategoryId,DateTime FromDate, DateTime ToDate)
        {
            param = new SqlParameter[6];
            param[0] = new SqlParameter("@InstId", InstId);
            param[1] = new SqlParameter("@MonthId", MonthId);
            param[2] = new SqlParameter("@FinID", FinID);
            param[3] = new SqlParameter("@JobCategoryId", JobCategoryId);
            param[4] = new SqlParameter("@FromDate", FromDate);
            param[5] = new SqlParameter("@ToDate", ToDate);
            return ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Payroll_FacBind", param);
        }

        public DataSet Payroll_OverallReport(int InstId,int FinID, int MonthId,int JobCatId,string DType)
        {

            param = new SqlParameter[5];
            param[0] = new SqlParameter("@InstId", InstId);
            param[1] = new SqlParameter("@FinId", FinID);
            param[2] = new SqlParameter("@Monthid", MonthId);
            param[3] = new SqlParameter("@JobCatId", JobCatId);
            param[4] = new SqlParameter("@DType", DType);


            return ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Payroll_OverallReport", param);
        }
        public DataSet Payroll_GetESIReport(int InstId, int FinID, int MonthId)
        {
            param = new SqlParameter[4];
            param[0] = new SqlParameter("@InstId", InstId);
            param[1] = new SqlParameter("@FinId", FinID);
            param[2] = new SqlParameter("@Monthid", MonthId);

            return ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Payroll_GetESIReport", param);
        }
        public DataSet Payroll_GetEPFReport(int InstId, int FinID, int MonthId)
        {
            param = new SqlParameter[4];
            param[0] = new SqlParameter("@InstId", InstId);
            param[1] = new SqlParameter("@FinId", FinID);
            param[2] = new SqlParameter("@Monthid", MonthId);

            return ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Payroll_GetEPFReport", param);
        }


          public DataSet payroll_PaySlipData(int Inst_id, int Fin_Id, int Month_Id, int Year)
        {
            param = new SqlParameter[4];
            param[0] = new SqlParameter("@Inst_id", Inst_id);
            param[1] = new SqlParameter("@Fin_Id", Fin_Id);
            param[2] = new SqlParameter("@Month_Id", Month_Id);
            param[3] = new SqlParameter("@Year", Year);
            ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "payroll_PaySlipData", param);
            return ds;
            
        }
        public DataSet usp_PayrollLICReport(int InstId, int FinID, int MonthId)
        {
            param = new SqlParameter[3];
            param[0] = new SqlParameter("@InstId", InstId);
            param[1] = new SqlParameter("@Yearid", FinID);
            param[2] = new SqlParameter("@MonthId", MonthId);

            return ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "usp_PayrollLICReport", param);
        }





        public DataSet Payroll_ReconcilatioReport(int InstId, DateTime FromDate, DateTime ToDate)
        {
            param = new SqlParameter[3];
            param[0] = new SqlParameter("@InstId", InstId);
            param[1] = new SqlParameter("@FromDate", FromDate);
            param[2] = new SqlParameter("@ToDate", ToDate);

            return ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Payroll_ReconcilationReport", param);
        }

        public DataSet Payroll_OverallReportUserWise(int InstId, int FinId, int MonthId, string GroupIDs)
        {
            param = new SqlParameter[4];
            param[0] = new SqlParameter("@InstId", InstId);
            param[1] = new SqlParameter("@FinId", FinId);
            param[2] = new SqlParameter("@MonthId", MonthId);
            param[3] = new SqlParameter("@GroupIDs", GroupIDs);

            return ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Payroll_OverallReportUserWise", param);
        }

        public DataSet Payroll_OverallReportUserWiseLeagalPayslip(int InstId, int FinId, int MonthId, int UserId)
        {
            param = new SqlParameter[4];
            param[0] = new SqlParameter("@InstId", InstId);
            param[1] = new SqlParameter("@FinId", FinId);
            param[2] = new SqlParameter("@MonthId", MonthId);
            param[3] = new SqlParameter("@UserId", UserId);

            return ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Payroll_OverallReportUserWiseLeagalPayslip", param);
        }


        public DataSet AllFacultyListInstWise(int InstID)
        {
            param = new SqlParameter[1];
            param[0] = new SqlParameter("@InstID", InstID);
            return ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "AllFacultyInfoSelectByInstId", param);
        }

        public DataSet payroll_PaySlipDataForBank(int InstID, int Fin_Id, int Month_Id, int Year)
        {
            param = new SqlParameter[4];
            param[0] = new SqlParameter("@Inst_id", InstID);
            param[1] = new SqlParameter("@Fin_Id", Fin_Id);
            param[2] = new SqlParameter("@Month_Id", Month_Id);
            param[3] = new SqlParameter("@Year", Year);
            ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "payroll_PaySlipDataForBank", param);
            return ds;
        }
    }
}