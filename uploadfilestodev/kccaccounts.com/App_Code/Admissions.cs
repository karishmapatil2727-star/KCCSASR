using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;

namespace ESchoolDAL
{
    public class Admissions
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString);
        SqlParameter[] param;
        DataSet ds;
        string SQL;

        public DataSet SearchTypeList()
        {
            return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "SearchTypeList");
        }
        
        public DataSet BranchListByInst(int InstId)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@InstId", InstId);
            return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "Common_InstWiseBranchList", param);

        }

        public DataSet SearchListByInst(int InstId, string Type, int BrId)
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@InstId", InstId);
            param[1] = new SqlParameter("@BrId", BrId);
            param[2] = new SqlParameter("@Type", Type);
            return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "SearchResult", param);
        }

        public DataSet SearchDetailByInst(int InstId, string Type, int BrId, string Id)
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@InstId", InstId);
            param[1] = new SqlParameter("@BrId", BrId);
            param[2] = new SqlParameter("@Type", Type);
            param[3] = new SqlParameter("@Id", Id);
            return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "SearchResultDetail", param);
        }
    }
}
