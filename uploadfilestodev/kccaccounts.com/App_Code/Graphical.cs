using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;
using UniversityDAL;

public class Graphical : DataBaseConnection
{
    DataSet ds;
    SqlParameter[] param;
    public DataSet GraphicalView_StateStrength(int InstID)
    {
        ds = new DataSet();
        param = new SqlParameter[1];
        param[0] = new SqlParameter("@InstID", InstID);
        return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GraphicalView_StateStrength", param);
    }
    public DataSet GraphicalView_InstStrength(int InstID)
    {
        ds = new DataSet();
        param = new SqlParameter[1];
        param[0] = new SqlParameter("@InstID", InstID);
        return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GraphicalView_InstStrength", param);
    }
    public DataSet GraphicalView_GenderStrength(int InstID)
    {
        ds = new DataSet();
        param = new SqlParameter[1];
        param[0] = new SqlParameter("@InstID", InstID);
        return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GraphicalView_GenderStrength", param);
    }

    public DataSet GraphicalView_CourseStrength(int InstID)
    {
        ds = new DataSet();
        param = new SqlParameter[1];
        param[0] = new SqlParameter("@InstID", InstID);
        return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GraphicalView_CourseStrength", param);
    }

    public DataSet GraphicalView_StudentsAreaStrength(int InstID)
    {
        ds = new DataSet();
        param = new SqlParameter[1];
        param[0] = new SqlParameter("@InstID", InstID);
        return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GraphicalView_StudentsAreaStrength", param);
    }

    public DataSet GraphicalView_StudentsAdmissionYear(int InstID)
    {
        ds = new DataSet();
        param = new SqlParameter[1];
        param[0] = new SqlParameter("@InstID", InstID);
        return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GraphicalView_StudentsYearOfAdmission", param);
    }
  
    /**************************Library Graphs***************************/
    public DataSet GraphicalView_LibraryIssuedBooksByCategory(int InstID)
    {
        ds = new DataSet();
        param = new SqlParameter[1];
        param[0] = new SqlParameter("@InstID", InstID);
        return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GraphicalView_LibraryIssuedBooksByCategory", param);
    }

    public DataSet GraphicalView_LibraryIssuedBooksByBranch(int InstID)
    {
        ds = new DataSet();
        param = new SqlParameter[1];
        param[0] = new SqlParameter("@InstID", InstID);
        return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GraphicalView_LibraryIssuedBooksByBranch", param);
    }

    public DataSet GraphicalView_LibraryIssuedBooksReturned(int InstID)
    {
        ds = new DataSet();
        param = new SqlParameter[1];
        param[0] = new SqlParameter("@InstID", InstID);
        return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GraphicalView_LibraryIssuedBooksReturned", param);
    }

    
    /**************************Correspondence Graphs***************************/
    public DataSet GraphicalView_CorrespondenceStat(int ForID)
    {
        ds = new DataSet();
        param = new SqlParameter[1];
        param[0] = new SqlParameter("@ForID", ForID);
        return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GraphicalView_CorrespondenceStat", param);
    }

    public DataSet GraphicalView_CorrespondenceTypeStat(int ForID)
    {
        ds = new DataSet();
        param = new SqlParameter[1];
        param[0] = new SqlParameter("@ForID", ForID);
        return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GraphicalView_CorrespondenceTypeStat", param);
    }

    public DataSet GraphicalView_CorrespondenceSentStat(int FromID)
    {
        ds = new DataSet();
        param = new SqlParameter[1];
        param[0] = new SqlParameter("@FromID", FromID);
        return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GraphicalView_CorrespondenceSentStat", param);
    }
    public DataSet GraphicalView_CorrespondenceSentTypeStat(int FromID)
    {
        ds = new DataSet();
        param = new SqlParameter[1];
        param[0] = new SqlParameter("@FromID", FromID);
        return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GraphicalView_CorrespondenceSentTypeStat", param);
    }

    /**************************Payroll Graphs***************************/
    public DataSet GraphicalView_PayrollOverallStats(int FinID)
    {
        ds = new DataSet();
        param = new SqlParameter[1];
        param[0] = new SqlParameter("@FinID", FinID);
        return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GraphicalView_PayrollOverallStatistics", param);
    }

    public DataSet GraphicalView_PayrollMonthWiseStats(int FinID)
    {
        ds = new DataSet();
        param = new SqlParameter[1];
        param[0] = new SqlParameter("@FinID", FinID);
        return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GraphicalView_PayrollMonthWiseStatistics", param);
    }

    public DataSet Payroll_AdminReportGraph(int InstID, int FinID ,string UserType )
    {
        ds = new DataSet();
        param = new SqlParameter[3];
        param[0] = new SqlParameter("@InstID", InstID);
        param[1] = new SqlParameter("@FinID", FinID);
        param[2] = new SqlParameter("@UserType", UserType);
        return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Payroll_AdminReportGraph", param);
         
    }

    public DataSet StudentStatistics(int InstID, string UserType)
    {
        ds = new DataSet();
        param = new SqlParameter[3];
        param[0] = new SqlParameter("@InstID", InstID);
        param[1] = new SqlParameter("@UserType", UserType);
        return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "StudentStatistics", param);

    }

    public DataSet GraphicalView_StudentsResidenceArea(int InstID)
    {
        ds = new DataSet();
        param = new SqlParameter[1];
        param[0] = new SqlParameter("@InstID", InstID);
        return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "GraphicalView_StudentsAreaStrength", param);
    }


    public DataSet OnlineFacultyOnline()
    {
        ds = new DataSet();
        param = new SqlParameter[0];
   
        return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Grapicalview_OnlineFacultyOnline", param);
    }
}