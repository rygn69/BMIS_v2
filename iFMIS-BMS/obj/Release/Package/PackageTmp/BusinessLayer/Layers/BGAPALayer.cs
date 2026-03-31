using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Layers;
using iFMIS_BMS.BusinessLayer.Models;
using System.Data.SqlClient;

namespace iFMIS_BMS.BusinessLayer.Layers
{
    public class BGAPALayer
    {
        public IEnumerable<AccountsModel> Programss(int? prog_ID, int? propYear, int? office_ID)
        {
            List<AccountsModel> prog = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT DISTINCT a.AccountID,  b.AccountName, g.FundName, f.OOEName,a.ProposalYear,a.ProposalAllotedAmount,a.UserID,a.ProgramID FROM dbo.tbl_T_BMSBudgetProposal AS a
                                                    left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID 
                                                    left JOIN dbo.tbl_R_BMSAccounts AS b ON b.AccountID = e.AccountID 
                                                    left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID 
                                                    left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID 
                                                    LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID 
                                                    LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode
                                                    WHERE c.OfficeID = '" + office_ID + "' AND c.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + propYear + "' and a.ProposalActionCode = '1'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel emp = new AccountsModel();
                    emp.AccountID = reader.GetInt32(0);
                    emp.AccountName = reader.GetValue(1).ToString();
                    emp.FundName = reader.GetString(2);
                    emp.OOEName = reader.GetString(3);
                    emp.ProposalYear = reader.GetInt32(4);
                    emp.ProposalAllotedAmount = Convert.ToDouble(reader.GetValue(5));
                    emp.UserID = reader.GetString(6);
                    emp.ProgramID = reader.GetInt32(7);


                    prog.Add(emp);
                }




            }
            return prog;



        }

        public void UpdateAccount(IEnumerable<AccountsModel> Accounts)
        {


            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                foreach (AccountsModel Account in Accounts)
                {


                    SqlCommand query_time = new SqlCommand(@"SELECT CONVERT(VARCHAR(30),GETDATE(),101) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 109), 13, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 16, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 19, 2) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 100), 18, 2)", con);

                    SqlDataReader reader = query_time.ExecuteReader();
                    var timeDate = "";
                    while (reader.Read())
                    {
                        timeDate = reader.GetString(0);
                    }

                    reader.Close();



                    //   SqlCommand com = new SqlCommand("update a set ProposalAllotedAmount= " + Account.ProposalAllotedAmount + "  FROM dbo.tbl_T_BMSBudgetProposal AS a left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID left JOIN dbo.tbl_R_BMSAccounts AS b ON b.AccountID = e.AccountID left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode WHERE  a.AccountID = " + Account.AccountID + "and a.ProposalYear = " + Account.ProposalYear, con);
                    SqlCommand com = new SqlCommand("update a set ProposalActionCode= 2  FROM dbo.tbl_T_BMSBudgetProposal AS a left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID left JOIN dbo.tbl_R_BMSAccounts AS b ON b.AccountID = e.AccountID left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode WHERE  a.AccountID = " + Account.AccountID + "and a.ProposalYear = " + Account.ProposalYear, con);

                    com.ExecuteNonQuery();



                    //  SqlCommand query_program = new SqlCommand(@"insert into tbl_T_BMSBudgetProposal(AccountID,UserID,ProgramID,ProposalYear,ProposalDateTime,ProposalStatusHR,ProposalStatusInCharge,ProposalStatusCommittee,ProposalAmount,ProposalAllotedAmount,ProposalActionCode) values (299," + Account.AccountID + "," + Account.UserID + "," + Account.ProposalYear + ",'02/19/2016 10:19:46 AM',1,1,1,0.00, " + Account.ProposalAllotedAmount + ",1)", con);
                    SqlCommand query_program = new SqlCommand(@"insert into tbl_T_BMSBudgetProposal(AccountID,UserID,ProgramID,ProposalYear,ProposalDateTime,ProposalStatusHR,ProposalStatusInCharge,ProposalStatusCommittee,ProposalAmount,ProposalAllotedAmount,ProposalActionCode) values (" + Account.AccountID + ",0834," + Account.ProgramID + "," + Account.ProposalYear + ",'" + timeDate + "',1,1,1,0.00, " + Account.ProposalAllotedAmount + ",1)", con);

                    query_program.ExecuteNonQuery();

                }
            }
        }


    }
}