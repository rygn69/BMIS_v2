using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Layers;

namespace iFMIS_BMS.BusinessLayer.Layers
{
    public class EditHome2_Layer
    {

        public IEnumerable<AccountsModel> Programss(int? prog_ID, int? propYear1)
        {
            List<AccountsModel> prog = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT DISTINCT e.AccountID,  e.AccountName,  f.OOEAbrevation,a.ProposalYear,a.ProposalAllotedAmount,e.AccountYear,e.ProgramID FROM dbo.tbl_T_BMSBudgetProposal AS a
                                                    left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID 
                                                    left JOIN dbo.tbl_R_BMSAccounts AS b ON b.AccountID = e.AccountID 
                                                    left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID 
                                                    left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID 
                                                    LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID 
                                                    LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode
                                                    WHERE c.OfficeID = '" + Account.UserInfo.Department.ToString() + "' and  c.ProgramID = '" + prog_ID + "' and a.ProposalYear = " + propYear1
                                                    + "-1 and a.ProposalActionCode = '1' and a.ProposalStatusCommittee='1' and e.AccountYear='" + propYear1 + "' and (e.AccountID  NOT in(select AccountID from tbl_T_BMSBudgetProposal where ProposalYear = '" + propYear1 + "' )) ORDER BY AccountName", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel emp = new AccountsModel();
                    emp.AccountID = Convert.ToInt32(reader.GetValue(0));
                    emp.AccountName = reader.GetString(1);
                    // emp.FundName = reader.GetString(2);
                    emp.OOEName = reader.GetString(2);
                    emp.PastProposalYear = reader.GetInt32(3);
                    emp.PastProposalAmmount = Convert.ToDouble(reader.GetValue(4));
                    //emp.UserID = reader.GetString(6);
                    emp.ProposalYear = reader.GetInt32(5);
                    emp.ProgramID = reader.GetInt32(6);

                    prog.Add(emp);
                }
            }
            return prog;
        }

        
        public void UpdateAccountHome2(IEnumerable<AccountsModel> Accounts)
        {
            var CanPS = 0;
            var CanMOOE = 0;
            var CanCO = 0;
            var CanFE = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();


                SqlCommand query_HR = new SqlCommand(@"SELECT * FROM tbl_R_BMSUserTypes WHERE UserTypeID = 3", con);
                SqlDataReader reader_HR = query_HR.ExecuteReader();
                while (reader_HR.Read())
                {
                    CanPS = Convert.ToInt32(reader_HR.GetValue(2));
                    CanMOOE = Convert.ToInt32(reader_HR.GetValue(3));
                    CanCO = Convert.ToInt32(reader_HR.GetValue(4));
                    CanFE = Convert.ToInt32(reader_HR.GetValue(5));
                }
                reader_HR.Close();
                foreach (AccountsModel accounts in Accounts)
                {

                    SqlCommand query_time = new SqlCommand(@"SELECT CONVERT(VARCHAR(30),GETDATE(),101) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 109), 13, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 16, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 19, 2) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 100), 18, 2)", con);

                    SqlDataReader reader = query_time.ExecuteReader();
                    var timeDate = "";
                    while (reader.Read())
                    {
                        timeDate = reader.GetString(0);
                    }
                    string OOEName = accounts.OOEName;

                    reader.Close();
                    var userID = Account.UserInfo.eid;
                    SqlCommand com = new SqlCommand(@"
                        SELECT * FROM tbl_T_BMSAccountDenomination WHERE DenominationName = '" + accounts.AccountName + "' and ProgramID = '" + accounts.ProgramID + "' and AccountID = '" + accounts.AccountID + "'" +
                        "IF @@ROWCOUNT=0 insert into tbl_T_BMSAccountDenomination values('" + accounts.AccountName + "', '" + accounts.setProposalAllotedAmount + "', '" + timeDate + "', '" + userID + "', 3, '" + accounts.ProposalYear + "', '" + accounts.ProgramID + "', '" + accounts.AccountID + "')" +
                        "ELSE Update tbl_T_BMSAccountDenomination set DenominationAmount = '" + accounts.setProposalAllotedAmount + "' WHERE DenominationName = '" + accounts.AccountName + "' and ProgramID = '" + accounts.ProgramID + "' and AccountID = '" + accounts.AccountID + "' ", con);
                    //con.Open();
                    com.ExecuteNonQuery();
                   #region MyRegion
//                    SqlCommand com = new SqlCommand("update a set ProposalActionCode= 2  FROM dbo.tbl_T_BMSBudgetProposal AS a left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID left JOIN dbo.tbl_R_BMSAccounts AS b ON b.AccountID = e.AccountID left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode WHERE  a.AccountID = " + accounts.AccountID + "and a.ProposalYear = " + accounts.ProposalYear, con);
//                    com.ExecuteNonQuery();

//                    if (OOEName == "PS")
//                    {
//                        if (CanPS == 1)
//                        {
//                            SqlCommand query_program = new SqlCommand(@"insert into tbl_T_BMSBudgetProposal(AccountID,UserID,ProgramID,ProposalYear,ProposalDateTime,ProposalStatusHR,ProposalStatusInCharge,ProposalStatusCommittee,ProposalAmount,ProposalAllotedAmount,ProposalActionCode, ProposalDenominationCode) values (" + accounts.AccountID + ",'" + Account.UserInfo.eid.ToString() + "'," + accounts.ProgramID + "," + accounts.ProposalYear + ",'" + timeDate + "',2,2,2," + accounts.setProposalAllotedAmount + ",0.00,1,0)", con);
//                            query_program.ExecuteNonQuery();
//                        }
//                        else
//                        {
//                            SqlCommand query_program = new SqlCommand(@"insert into tbl_T_BMSBudgetProposal(AccountID,UserID,ProgramID,ProposalYear,ProposalDateTime,ProposalStatusHR,ProposalStatusInCharge,ProposalStatusCommittee,ProposalAmount,ProposalAllotedAmount,ProposalActionCode, ProposalDenominationCode) values (" + accounts.AccountID + ",'" + Account.UserInfo.eid.ToString() + "'," + accounts.ProgramID + "," + accounts.ProposalYear + ",'" + timeDate + "',1,2,2," + accounts.setProposalAllotedAmount + ",0.00,1,0)", con);
//                            query_program.ExecuteNonQuery();
//                        }
//                    }
//                    if (OOEName == "MOOE")
//                    {
//                        if (CanMOOE == 1)
//                        {
//                            SqlCommand query_program = new SqlCommand(@"insert into tbl_T_BMSBudgetProposal(AccountID,UserID,ProgramID,ProposalYear,ProposalDateTime,ProposalStatusHR,ProposalStatusInCharge,ProposalStatusCommittee,ProposalAmount,ProposalAllotedAmount,ProposalActionCode, ProposalDenominationCode) values (" + accounts.AccountID + ",'" + Account.UserInfo.eid.ToString() + "'," + accounts.ProgramID + "," + accounts.ProposalYear + ",'" + timeDate + "',2,2,2," + accounts.setProposalAllotedAmount + ",0.00,1,0)", con);
//                            query_program.ExecuteNonQuery();
//                        }
//                        else
//                        {
//                            SqlCommand query_program = new SqlCommand(@"insert into tbl_T_BMSBudgetProposal(AccountID,UserID,ProgramID,ProposalYear,ProposalDateTime,ProposalStatusHR,ProposalStatusInCharge,ProposalStatusCommittee,ProposalAmount,ProposalAllotedAmount,ProposalActionCode, ProposalDenominationCode) values (" + accounts.AccountID + ",'" + Account.UserInfo.eid.ToString() + "'," + accounts.ProgramID + "," + accounts.ProposalYear + ",'" + timeDate + "',1,2,2," + accounts.setProposalAllotedAmount + ",0.00,1,0)", con);
//                            query_program.ExecuteNonQuery();
//                        }
//                    }
//                    if (OOEName == "CO")
//                    {
//                        if (CanCO == 1)
//                        {
//                            SqlCommand query_program = new SqlCommand(@"insert into tbl_T_BMSBudgetProposal(AccountID,UserID,ProgramID,ProposalYear,ProposalDateTime,ProposalStatusHR,ProposalStatusInCharge,ProposalStatusCommittee,ProposalAmount,ProposalAllotedAmount,ProposalActionCode, ProposalDenominationCode) values (" + accounts.AccountID + ",'" + Account.UserInfo.eid.ToString() + "'," + accounts.ProgramID + "," + accounts.ProposalYear + ",'" + timeDate + "',2,2,2," + accounts.setProposalAllotedAmount + ",0.00,1,0)", con);
//                            query_program.ExecuteNonQuery();
//                        }
//                        else
//                        {
//                            SqlCommand query_program = new SqlCommand(@"insert into tbl_T_BMSBudgetProposal(AccountID,UserID,ProgramID,ProposalYear,ProposalDateTime,ProposalStatusHR,ProposalStatusInCharge,ProposalStatusCommittee,ProposalAmount,ProposalAllotedAmount,ProposalActionCode, ProposalDenominationCode) values (" + accounts.AccountID + ",'" + Account.UserInfo.eid.ToString() + "'," + accounts.ProgramID + "," + accounts.ProposalYear + ",'" + timeDate + "',1,2,2," + accounts.setProposalAllotedAmount + ",0.00,1,0)", con);
//                            query_program.ExecuteNonQuery();
//                        }
//                    }
//                    if (OOEName == "FE")
//                    {
//                        if (CanFE == 1)
//                        {
//                            SqlCommand query_program = new SqlCommand(@"insert into tbl_T_BMSBudgetProposal(AccountID,UserID,ProgramID,ProposalYear,ProposalDateTime,ProposalStatusHR,ProposalStatusInCharge,ProposalStatusCommittee,ProposalAmount,ProposalAllotedAmount,ProposalActionCode, ProposalDenominationCode) values (" + accounts.AccountID + ",'" + Account.UserInfo.eid.ToString() + "'," + accounts.ProgramID + "," + accounts.ProposalYear + ",'" + timeDate + "',2,2,2," + accounts.setProposalAllotedAmount + ",0.00,1,0)", con);
//                            query_program.ExecuteNonQuery();
//                        }
//                        else
//                        {
//                            SqlCommand query_program = new SqlCommand(@"insert into tbl_T_BMSBudgetProposal(AccountID,UserID,ProgramID,ProposalYear,ProposalDateTime,ProposalStatusHR,ProposalStatusInCharge,ProposalStatusCommittee,ProposalAmount,ProposalAllotedAmount,ProposalActionCode, ProposalDenominationCode) values (" + accounts.AccountID + ",'" + Account.UserInfo.eid.ToString() + "'," + accounts.ProgramID + "," + accounts.ProposalYear + ",'" + timeDate + "',1,2,2," + accounts.setProposalAllotedAmount + ",0.00,1,0)", con);
//                            query_program.ExecuteNonQuery();
//                        }
//                    }
//                    SqlCommand Insert_CopiedFrom = new SqlCommand(@"insert into tbl_R_BMSCopiedFrom
//                                                                    select proposalID, " + accounts.ProposalYear + " -1 from tbl_T_BMSBudgetProposal where ProposalYear = " + accounts.ProposalYear + " and ProgramID = " + accounts.ProgramID + " and UserID = " + Account.UserInfo.eid.ToString() + " and AccountID =" + accounts.AccountID + " and ProposalID NOT in (SELECT ProposalID FROM tbl_R_BMSCopiedFrom)", con);
//                    Insert_CopiedFrom.ExecuteNonQuery(); 
                #endregion
                }
            }
        }


    }
}