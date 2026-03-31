using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers.Maintenance
{
    public class SearchAccountName
    {
        public string search_AccountName(string AccountCode)
        {
            string result = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand("Select AccountName from tbl_R_BMSAccounts where ChildAccountCode='" + AccountCode + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = reader.GetString(0);
                }
            }
            return result;
        }
        public int search_AccountID(string OfficeID, string AccountCode)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"SELECT DISTINCT
                    dbo.tbl_R_BMSProgramAccounts.AccountID
                    FROM
                    dbo.tbl_R_BMSProgramAccounts
                    INNER JOIN dbo.tbl_R_BMSAccounts ON dbo.tbl_R_BMSAccounts.FMISAccountCode = dbo.tbl_R_BMSProgramAccounts.AccountID
                    INNER JOIN dbo.tbl_R_BMSOfficePrograms ON dbo.tbl_R_BMSProgramAccounts.ProgramID = dbo.tbl_R_BMSOfficePrograms.ProgramID
                    WHERE
                    dbo.tbl_R_BMSAccounts.ChildAccountCode = '"+AccountCode+"' and dbo.tbl_R_BMSOfficePrograms.OfficeID = "+OfficeID+" and dbo.tbl_R_BMSProgramAccounts.AccountYear = YEAR(GETDATE())", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = Convert.ToInt32(reader.GetValue(0));
                }
            }
            return result;
        }
        public int search_OrderByNo(string AccountName)
        {
            int result2;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand("Select OrderNo from tbl_R_BMSProgramAccounts where AccountName='" + AccountName + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read()) 
                {
                    result2 = Convert.ToInt32(reader.GetValue(0));
                   
                }
            }
            return 0;
        }
    }
}