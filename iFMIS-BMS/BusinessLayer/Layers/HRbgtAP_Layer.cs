using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Layers;
using iFMIS_BMS.BusinessLayer.Models;
using System.Data.SqlClient;
using iFMIS_BMS.BusinessLayer.Models.Budget_Preparation;
using System.Net.Mail;
using System.Data;

namespace iFMIS_BMS.BusinessLayer.Layers
{
    public class HRbgtAP_Layer
    {

        public IEnumerable<ProgramsModel> gPrograms(int? off_ID, int? propYear)
        {
            List<ProgramsModel> pross = new List<ProgramsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select ProgramID,ProgramDescription from tbl_R_BMSOfficePrograms where ProgramYear = '" + propYear + "' and OfficeID = '" + off_ID + "'" , con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ProgramsModel app = new ProgramsModel();
                    app.ProgramID = reader.GetValue(0).ToString();
                    app.ProgramDescription = reader.GetString(1);

                    pross.Add(app);
                }
            }
            return pross;
        }
        public string ApproveProposalBudgetInCharge(string[] ProposalID)
        {
            try
            {
                foreach (var item in ProposalID)
                {
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand("update tbl_T_BMSBudgetProposal set ProposalStatusInCharge='1' where ProposalID='" + item + "'", con);
                        con.Open();
                        com.ExecuteNonQuery();
                    }
                    //using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    //{
                    //    SqlCommand com = new SqlCommand("insert into tbl_R_BMSProposalRemark values('" + item + "','" + Account.UserInfo.eid + "','Approved','" + Account.UserInfo.UserTypeDesc + "')", con);
                    //    con.Open();
                    //    com.ExecuteNonQuery();
                    //}
               }
                    return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            } 
        }
        public string ApproveProposalHRMOInCharge(string[] ProposalID)
        {
            try
            {
                foreach (var item in ProposalID)
                {
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand("update tbl_T_BMSBudgetProposal set ProposalStatusHR='1' where ProposalID='" + item + "'", con);
                        con.Open();
                        com.ExecuteNonQuery();
                    }
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand("insert into tbl_R_BMSProposalRemark values('" + item + "','" + Account.UserInfo.eid + "','Approved', '" + Account.UserInfo.UserTypeDesc + "')", con);
                        con.Open();
                        com.ExecuteNonQuery();
                    }
                }
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        decimal ProposalAmount;
        decimal ProposalAllotedAmount;
        string AccountName;
        
        int UserID;
        string EmailAdd = "";

        public string ApproveProposalBudgetCommittee(string[] ProposalID, string[] ProposedAmounts, string[] SlashAmount, int regularaipid)
        {
            
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ProposalID");
                dt.Columns.Add("ProposedAmount");
                dt.Columns.Add("ApprovedAmount");
                var idx = 0;
                foreach (var Proposal in ProposalID)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = ProposalID[idx];
                    dr[1] = ProposedAmounts[idx];
                    dr[2] = SlashAmount[idx];
                    dt.Rows.Add(dr);
                    idx++;
                }
                if (regularaipid == 1)
                {
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand("dbo.sp_BMS_ApproveAccounts", con);
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.Add(new SqlParameter("@ProposalList", dt));
                        com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                        con.Open();

                        return com.ExecuteScalar().ToString();
                    }
                }
                else
                {
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand("dbo.sp_BMS_ApproveAccounts_supplemental", con);
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.Add(new SqlParameter("@ProposalList", dt));
                        com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                        con.Open();

                        return com.ExecuteScalar().ToString();
                    }
                }
               // return "1";
                #region Old Approval Query
              
                #endregion
                #region Function for Seding Mail
                
                #endregion
                
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string ApproveAllAccounts(int YearOf, int regularaipid) 
        {
            try
            {
                if (regularaipid == 1) //annual budget
                {
                    DataTable dt = new DataTable();
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"select b.ProposalID,b.ProposalAmount,a.BudgetYearAmount from tbl_R_BMSConsoliditedTotal as a
                                                    left join tbl_T_BMSBudgetProposal as b on b.AccountID = a.AccountID and b.ProgramID = a.ProgramID 
                                                    and b.ProposalYear = a.YearOf  and b.ProposalActionCode = 1 
                                                    where a.YearOf = " + YearOf + " and b.ProposalID is not null and ProposalStatusCommittee = 2 and a.BudgetYearAmount != 0", con);
                        con.Open();
                        com.CommandTimeout = 0;
                        dt.Load(com.ExecuteReader());
                    }
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand("dbo.sp_BMS_ApproveAccounts", con);
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.Add(new SqlParameter("@ProposalList", dt));
                        com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                        con.Open();
                        com.CommandTimeout = 0;
                        return com.ExecuteScalar().ToString();
                    }
                }
                else //supplemental
                {
                    DataTable dt = new DataTable();
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"select b.ProposalID,b.ProposalAmount,a.SupplementalAmount as BudgetYearAmount from tbl_R_BMSConsoliditedTotal as a
                                                    left join tbl_T_BMSBudgetProposal as b on b.AccountID = a.AccountID and b.ProgramID = a.ProgramID 
                                                    and b.ProposalYear = a.YearOf  and b.ProposalActionCode = 1 
                                                    where a.YearOf = " + YearOf + " and b.ProposalID is not null and ProposalStatusCommittee = 1 and isnull(b.SupplementalAmount,0) != 0", con);
                        con.Open();
                        com.CommandTimeout = 0;
                        dt.Load(com.ExecuteReader());
                    }
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand("dbo.sp_BMS_ApproveAccounts_supplemental", con);
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.Add(new SqlParameter("@ProposalList", dt));
                        com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                        con.Open();
                        com.CommandTimeout = 0;
                        return com.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string DeclineProposalBudgetInCharge(string[] ProposalID, string Remarks)
        {
            try
            {
                foreach (var item in ProposalID)
                {
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand("update tbl_T_BMSBudgetProposal set ProposalStatusInCharge='3', ProposalActionCode = '2' where ProposalID='" + item + "'", con);
                        con.Open();
                        com.ExecuteNonQuery();
                    }
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand("insert into tbl_R_BMSProposalRemark values('" + item + "','" + Account.UserInfo.eid + "','" + Remarks + "','" + Account.UserInfo.UserTypeDesc + "')", con);
                        con.Open();
                        com.ExecuteNonQuery();
                    }
                }
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string DeclineProposalHRMOInCharge(string[] ProposalID, string Remarks)
        {
            try
            {
                foreach (var item in ProposalID)
                {
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand("update tbl_T_BMSBudgetProposal set ProposalStatusHR='3', ProposalActionCode = '2' where ProposalID='" + item + "'", con);
                        con.Open();
                        com.ExecuteNonQuery();
                    }
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand("insert into tbl_R_BMSProposalRemark values('" + item + "','" + Account.UserInfo.eid + "','" + Remarks + "','" + Account.UserInfo.UserTypeDesc + "')", con);
                        con.Open();
                        com.ExecuteNonQuery();
                    }
                }
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string DeclineProposalBudgetCommittee(string[] ProposalID, string Remarks)
        {
            try
            {
                foreach (var item in ProposalID)
                {
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand("update tbl_T_BMSBudgetProposal set ProposalStatusCommittee='3', ProposalActionCode = '2' where ProposalID='" + item + "'", con);
                        con.Open();
                        com.ExecuteNonQuery();
                    }
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand("insert into tbl_R_BMSProposalRemark values('" + item + "','" + Account.UserInfo.eid + "','" + Remarks + "','" + Account.UserInfo.UserTypeDesc + "')", con);
                        con.Open();
                        com.ExecuteNonQuery();
                    }
                }
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public Cancelled CancelledDetails(int ProposalID, int OfficeID, int Year, int ProgramID, string OfficeLevel)
        {
            Cancelled cm = new Cancelled();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT top 1 a.ProposalYear,a.ProposalAmount,b.Remarks,c.AccountName,d.OOEAbrevation,
                                                  b.OfficeLevel,g.EmpName FROM dbo.tbl_T_BMSBudgetProposal as a
                                                  INNER JOIN dbo.tbl_R_BMSProposalRemark as b ON a.ProposalID = b.ProposalID
                                                  INNER JOIN dbo.tbl_R_BMSProgramAccounts as c ON a.AccountID = c.AccountID
                                                  INNER JOIN dbo.tbl_R_BMSObjectOfExpenditure as d ON c.ObjectOfExpendetureID = d.OOEID
                                                  INNER JOIN dbo.tbl_R_BMSOfficePrograms as e ON a.ProgramID = e.ProgramID
                                                  INNER JOIN dbo.tbl_R_BMSOffices as f ON e.OfficeID = f.OfficeID
                                                  INNER JOIN pmis.dbo.vwMergeAllEmployee as g ON b.UserID = g.eid

                                                  where a." + OfficeLevel + " = '3' and c.ActionCode = '1' and c.AccountYear = '" + Year 
                                                  + "' and a.ProposalYear = '" + Year 
                                                  + "' and e.officeID ='" + OfficeID + "' and e.ProgramID = '" + ProgramID 
                                                  + "' and a.ProposalID = '" + ProposalID + "' order BY b.RemarkID DESC", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    cm.ProposalYear = reader.GetInt32(0);
                    cm.ProposalAmount = Convert.ToDouble(reader.GetValue(1));
                    cm.Remarks = reader.GetString(2);
                    cm.AccountName = reader.GetString(3);
                    cm.OOE = reader.GetString(4);
                    cm.OfficeLevel = reader.GetString(5);
                    cm.EmpName = reader.GetString(6);

                }
            }
            return cm;
        }
        public string UndoActionApproved(int ProposalID, string OfficeLevel, int regularaipid)
        {
            try
            {
                if (regularaipid == 1)
                {
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand("update tbl_T_BMSBudgetProposal set " + OfficeLevel + "='2',ProposalAllotedAmount='0' where ProposalID='" + ProposalID + "'", con);
                        con.Open();
                        com.ExecuteNonQuery();
                    }
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand("Delete from tbl_R_BMSProposalRemark where ProposalID = '" + ProposalID + "' and Remarks ='Approved' and OfficeLevel='" + Account.UserInfo.UserTypeDesc + "'", con);
                        con.Open();
                        com.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand("update [IFMIS].[dbo].tbl_T_BMSBudgetProposal set [SupplementalAmount_approve]=0 where ProposalID='" + ProposalID + "'", con);
                        con.Open();
                        com.ExecuteNonQuery();
                    }
                }
                
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string UndoActionCancelled(int ProposalID, string OfficeLevel)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("update tbl_T_BMSBudgetProposal set " + OfficeLevel + "='2', ProposalActionCode='1' where ProposalID='" + ProposalID + "'", con);
                    con.Open();
                    com.ExecuteNonQuery();
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("Delete from tbl_R_BMSProposalRemark where ProposalID = '" + ProposalID + "' and Remarks !='Approved' and OfficeLevel='" + Account.UserInfo.UserTypeDesc + "'", con);
                    con.Open();
                    com.ExecuteNonQuery();
                }

                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string BuildAndApproveExistingAccount(int Yearof, int OfficeID, double ApprovedAmount, int ProposalID, int ProgramID, int OOEID, int AccountID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("dbo.sp_BMS_ApproveNewProposedAccountsExisting " + AccountID + ", " + ProgramID + ", " + OOEID + ", " + Account.UserInfo.eid + "," + Yearof + ", " + ApprovedAmount + ", " + ProposalID + "", con);
                try
                {
                    con.Open();
                    return com.ExecuteScalar().ToString();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public string  BuildAndApproveAccount(int Yearof, int OfficeID, double ApprovedAmount, int ProposalID, int ProgramID, 
                                            int OOEID, string AccountName, int AccountCode , string ChildAccountCode,
                                            string ThirdLevelGroup, string FundType)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("dbo.sp_BMS_ApproveAndBuildProposedAccounts " + ProgramID + ", " + OOEID + ", " + Account.UserInfo.eid + "," + Yearof + ", " + ApprovedAmount + ", " + ProposalID + ",'"+ AccountName +"', " + AccountCode + ", '" + ChildAccountCode + "','" + ThirdLevelGroup + "','" + FundType + "'", con);
                try
                {
                    con.Open();
                    return com.ExecuteScalar().ToString();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public string ApproveProposalNewAccounts(string[] ProposalID, string[] ProposedAmounts)
        {
            if (Account.UserInfo.UserTypeDesc == "Budget Office")
            {
                try
                {
                    foreach (var item in ProposalID)
                    {
                        using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        {
                            SqlCommand com = new SqlCommand("update tbl_Temp_BMSNewAccount set ProposalStatusInCharge='1' where TempID='" + item + "'", con);
                            con.Open();
                            com.ExecuteNonQuery();
                        }
                        using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        {
                            SqlCommand com = new SqlCommand("insert into tbl_R_BMSProposalRemark values('" + item + "','" + Account.UserInfo.eid + "','Approved','" + Account.UserInfo.UserTypeDesc + "')", con);
                            con.Open();
                            com.ExecuteNonQuery();
                        }
                    }
                    return "1";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            else if (Account.UserInfo.UserTypeDesc == "HRMO In-Charge")
            {
                try
                {
                    foreach (var item in ProposalID)
                    {
                        using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        {
                            SqlCommand com = new SqlCommand("update tbl_Temp_BMSNewAccount set ProposalStatusHR='1' where TempID='" + item + "'", con);
                            con.Open();
                            com.ExecuteNonQuery();
                        }
                        using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        {
                            SqlCommand com = new SqlCommand("insert into tbl_R_BMSProposalRemark values('" + item + "','" + Account.UserInfo.eid + "','Approved','" + Account.UserInfo.UserTypeDesc + "')", con);
                            con.Open();
                            com.ExecuteNonQuery();
                        }
                    }
                    return "1";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            else //Budget In Committee & Super Admin
            {
                try
                {
                    int a= 0;
                    foreach (var item in ProposalID)
                    {
                        using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        {
                            SqlCommand com = new SqlCommand(@"INSERT INTO  tbl_T_BMSBudgetProposal
                                                            SELECT a.AccountID, a.UserID, a.ProgramID, a.ProposalYear, a.ProposalDateTime,
                                                            a.ProposalStatusHR, a.ProposalStatusInCharge, '1',
                                                            a.ProposalAmount, '" + ProposedAmounts[a]
                                                            + "', a.ProposalActionCode FROM tbl_Temp_BMSNewAccount  AS a WHERE a.tempID='" + item 
                                                            + "' DELETE FROM tbl_Temp_BMSNewAccount WHERE TempID ='" + item + "'", con);
                            con.Open();
                            com.ExecuteNonQuery();
                        }
                        using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        {
                            SqlCommand com = new SqlCommand("insert into tbl_R_BMSProposalRemark values('" + item + "','" + Account.UserInfo.eid + "','Approved','" + Account.UserInfo.UserTypeDesc + "')", con);
                            con.Open();
                            com.ExecuteNonQuery();
                        }
                        a++;
                    }
                    return "1";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }

        }
        public string CheckifApproved(string proposalID)
        {
            var proposalStatus = "";
            if (Account.UserInfo.UserTypeDesc == "Budget Office")
            {
                proposalStatus = "ProposalStatusCommittee";
            }
            else if (Account.UserInfo.UserTypeDesc == "HRMO In-Charge")
            {
                proposalStatus = "ProposalStatusInCharge";
            }
            else //Budget In Committee & Super Admin
            {
                return proposalStatus = "2";
            }
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("SELECT " + proposalStatus + " from tbl_T_BMSBudgetProposal where ProposalID='" + proposalID + "'", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        proposalStatus = reader.GetValue(0).ToString();
                    }
                }
                return proposalStatus;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}