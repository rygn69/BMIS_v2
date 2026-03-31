using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers.Maintenance
{
    public class BuildupMagnaCarta_Layer
    {
        public string BuildupMagnaCarta(MagnaCarta BuildupMagnaCartaData)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var UserInfo = Account.UserInfo.eid.ToString();

                SqlCommand query_time = new SqlCommand(@"SELECT CONVERT(VARCHAR(30),GETDATE(),101) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 109), 13, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 16, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 19, 2) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 100), 18, 2)", con);
                con.Open();
                SqlDataReader reader_time = query_time.ExecuteReader();
                var timeDate = "";
                while (reader_time.Read())
                {
                    timeDate = reader_time.GetString(0);
                }
                reader_time.Close();

                SqlCommand query_AccountID = new SqlCommand(@"SELECT DISTINCT FMISAccountCode FROM tbl_R_BMSAccounts WHERE AccountCode = '"+BuildupMagnaCartaData.Account+"' ", con);
                string reader_AccountID = query_AccountID.ExecuteScalar().ToString();

                SqlCommand query_OfficeID = new SqlCommand(@"SELECT DISTINCT PMISOfficeID FROM tbl_R_BMSOffices WHERE OfficeID = '" + BuildupMagnaCartaData.OfficeID + "'", con);
                string reader_OfficeID = query_OfficeID.ExecuteScalar().ToString();

                SqlCommand query_MagnaCarta = new SqlCommand(@"Insert into tbl_R_BMSMagnaCarta values('" + BuildupMagnaCartaData.Account + "', '" + reader_OfficeID + "', '" + BuildupMagnaCartaData.type + "', '" + BuildupMagnaCartaData.BudgetYear + "', '" + UserInfo + "', '" + timeDate + "')", con);
               
                try
                {
                    query_MagnaCarta.ExecuteNonQuery();
                    SqlCommand query_ProgramAccounts = new SqlCommand(@"Insert into tbl_R_BMSProgramAccounts values('" + BuildupMagnaCartaData.ProgramID + "', '" + reader_AccountID + "', '1', '1', '" + timeDate + "', '" + BuildupMagnaCartaData.AccountName + "', '" + UserInfo + "', '9', '" + BuildupMagnaCartaData.BudgetYear + "')", con);
                    query_ProgramAccounts.ExecuteNonQuery();
                    //SqlCommand query_BudgetProposal = new SqlCommand(@"Insert into tbl_T_BMSBudgetProposal values('')")
                    SqlCommand query_CheckBudgetProposal = new SqlCommand(@"SELECT COUNT(ProposalID) FROM tbl_T_BMSBudgetProposal WHERE AccountID = '" + reader_AccountID + "' and ProgramID = '" + BuildupMagnaCartaData.ProgramID + "' ", con);
                    string reader_CheckBudgetProposal = query_CheckBudgetProposal.ExecuteScalar().ToString();
                    if (reader_CheckBudgetProposal == "0")
                    {
                        SqlCommand query_BudgetProposal = new SqlCommand(@"Insert into tbl_T_BMSBudgetProposal values('" + reader_AccountID + "', '" + UserInfo + "', '" + BuildupMagnaCartaData.ProgramID + "', " + BuildupMagnaCartaData.BudgetYear + " - 1, '" + timeDate + "', 1, 1, 1, 0, 0, 1, '')", con);
                        query_BudgetProposal.ExecuteNonQuery();
                    }
                    
                    return "success";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
    }
}