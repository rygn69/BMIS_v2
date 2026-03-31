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
    public class EditAccountLayer
    {



        public IEnumerable<AccountsModel> Programss(int prog_ID, int propYear, int office_ID)
        {
            List<AccountsModel> prog = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT DISTINCT a.AccountID,  b.AccountName, g.FundName, f.OOEName,a.ProposalYear,a.ProposalAllotedAmount FROM dbo.tbl_T_BMSBudgetProposal AS a
                                                    left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AcountID = a.AccountID 
                                                    left JOIN dbo.tbl_R_BMSAccounts AS b ON b.AccountID = e.AcountID 
                                                    left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID 
                                                    left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID 
                                                    LEFT JOIN dbo.tbl_R_BMSObjectOfExpendeture as f on f.OOEID = e.ObjectOfExpendetureID 
                                                    LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode
                                                    WHERE c.OfficeID = " + office_ID + " AND c.ProgramID = " + prog_ID + " and a.ProposalYear = " + propYear + " and a.ProposalActionCode = '1'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel emp = new AccountsModel();
                    emp.AccountID = reader.GetInt32(0);
                    emp.AccountName = reader.GetString(1);
                    emp.OOEName = reader.GetString(2);
                    emp.ProposalYear = reader.GetInt32(3);
                    emp.ProposalAllotedAmount = Convert.ToDouble(reader.GetValue(4));


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
                    SqlCommand com = new SqlCommand("update accounts set ammount=" + Account.AccountName + " where acc_ID=" + Account.AccountID, con);
                    com.ExecuteNonQuery();
                }
            }
        }


    }
}