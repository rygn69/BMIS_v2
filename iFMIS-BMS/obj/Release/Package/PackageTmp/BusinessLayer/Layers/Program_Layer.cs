using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Layers;
using iFMIS_BMS.BusinessLayer.Models;
using System.Data.SqlClient;
using iFMIS_BMS.BusinessLayer.Models.Budget_Preparation;
using iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation;
using System.Web.Mvc;
using System.Data;

namespace iFMIS_BMS.BusinessLayer.Layers
{
    public class Program_Layer
    {

        public IEnumerable<AccountsModel> ApprovedAccounts(int? prog_ID, int? proy_ID)
        {
            List<AccountsModel> prog = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT DISTINCT a.ProposalID, a.AccountID,  e.AccountName, g.FundName, f.OOEAbrevation,a.ProposalYear,a.ProposalAllotedAmount,a.UserID,a.ProgramID, a.ProposalStatusCommittee, a.ProposalAmount FROM dbo.tbl_T_BMSBudgetProposal AS a
                                                    left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID 
                                                    left JOIN dbo.tbl_R_BMSAccounts AS b ON b.AccountID = e.AccountID 
                                                    left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID 
                                                    left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID 
                                                    LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID 
                                                    LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode
                                                    WHERE c.OfficeID = '" + Account.UserInfo.Department.ToString() + "' and  c.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proy_ID + "' and a.ProposalActionCode = '1' and a.ProposalStatusCommittee='1' order by  AccountName", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel emp = new AccountsModel();
                    emp.ProposalID = Convert.ToInt32(reader.GetValue(0));
                    emp.AccountID = reader.GetInt32(1);
                    emp.AccountName = reader.GetValue(2).ToString();
                    emp.FundName = reader.GetString(3);
                    emp.OOEName = reader.GetString(4);
                    emp.ProposalYear = reader.GetInt32(5);
                    emp.ProposalAllotedAmount = Convert.ToDouble(reader.GetValue(6));
                    emp.UserID = reader.GetString(7);
                    emp.ProgramID = reader.GetInt32(8);
                    if (reader.GetValue(9).ToString() == "1")
                    {
                        emp.ProposalStatusCommittee = "Approved";
                    }
                    else if (reader.GetValue(9).ToString() == "2")
                    {
                        emp.ProposalStatusCommittee = "For Approval";
                    }
                    else if (reader.GetValue(9).ToString() == "3")
                    {
                        emp.ProposalStatusCommittee = "Cancelled";
                    }
                    emp.ProposalAmmount = Convert.ToDouble(reader.GetValue(9));

                    prog.Add(emp);
                }
            }
            return prog;
        }
        public IEnumerable<AccountsModel> forApprovalAccounts(int? prog_ID, int? proy_ID, int? ooe_id)
        {
            UserRoleLayer UserRoleLayer = new UserRoleLayer();
            var UserRule = UserRoleLayer.AdditionalRules("3");
            List<AccountsModel> prog = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"WITH x AS 
(
SELECT DISTINCT a.ProposalID, a.AccountID,  e.AccountName, f.OOEAbrevation,a.ProposalYear,a.ProposalAmount, a.ProposalStatusHR, a.ProposalStatusInCharge, a.ProposalDenominationCode, a.ProgramID, b.AccountCode FROM dbo.tbl_T_BMSBudgetProposal AS a
                                                    left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID 
                                                    left JOIN dbo.tbl_R_BMSAccounts AS b ON b.AccountID = e.AccountID 
                                                    left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID 
                                                    left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID 
                                                    LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID 
                                                    LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode
                                                    WHERE c.OfficeID = '" + Account.UserInfo.Department.ToString() + "' and  a.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proy_ID + "' and e.AccountYear = '" + proy_ID + "' and a.ProposalActionCode = '1' and e.ActionCode='1' and a.ProposalStatusCommittee = '2') " +

"" + "SELECT DISTINCT x.ProposalID, x.AccountID,  x.AccountName, x.OOEAbrevation,x.ProposalYear,a.ProposalAllotedAmount as PastYear, x.ProposalAmount as presentYear, x.ProposalStatusHR, x.ProposalStatusInCharge, x.ProposalDenominationCode, e.ProgramID, b.AccountCode FROM dbo.tbl_T_BMSBudgetProposal AS a " +
                                                    "" + "left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID " +
                                                    "" + "left JOIN dbo.tbl_R_BMSAccounts AS b ON b.AccountID = e.AccountID " +
                                                    "" + "left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID " +
                                                    "" + "left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID " +
                                                    "" + "LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID " +
                                                    "" + "LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode" +
                                                    "" + " left JOIN x as x on a.AccountID = x.AccountID" +
                                                    "" + " WHERE x.ProposalStatusHR != 3 and f.OOEID = '" + ooe_id + "' and c.OfficeID = '" + Account.UserInfo.Department.ToString() + "' and  a.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proy_ID + "' - 1 and e.AccountYear = '" + proy_ID + "' - 1 and a.ProposalActionCode = '1' and e.ActionCode='1' and a.AccountID= x.AccountID ORDER BY AccountName", con);

                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel emp = new AccountsModel();
                    emp.ProposalID = Convert.ToInt32(reader.GetValue(0));
                    emp.AccountID = reader.GetInt32(1);
                    emp.AccountName = reader.GetValue(2).ToString();
                    emp.OOEName = reader.GetString(3);
                    emp.ProposalYear = reader.GetInt32(4);
                    emp.PastProposalAmmount = Convert.ToDouble(reader.GetValue(5));
                    emp.ProposalAllotedAmount = Convert.ToDouble(reader.GetValue(6));

                    emp.ProposalStatusInCharge = Convert.ToInt32(reader.GetValue(8));
                    if (reader.GetString(3) == "PS")
                    {
                        if (UserRule.canReviewPS == 1)
                        {
                            emp.ProposalStatusHR = Convert.ToInt32(reader.GetValue(7));
                        }
                        else
                        {
                            emp.ProposalStatusHR = 2;
                        }
                    }
                    if (reader.GetString(3) == "MOOE")
                    {
                        if (UserRule.canReviewMOOE == 1)
                        {
                            emp.ProposalStatusHR = Convert.ToInt32(reader.GetValue(7));
                        }
                        else
                        {
                            emp.ProposalStatusHR = 2;
                        }
                    }
                    if (reader.GetString(3) == "CO")
                    {
                        if (UserRule.canReviewCO == 1)
                        {
                            emp.ProposalStatusHR = Convert.ToInt32(reader.GetValue(7));
                        }
                        else
                        {
                            emp.ProposalStatusHR = 2;
                        }
                    }
                    if (reader.GetString(3) == "FE")
                    {
                        if (UserRule.canReviewFE == 1)
                        {
                            emp.ProposalStatusHR = Convert.ToInt32(reader.GetValue(7));
                            emp.ProposalStatusInCharge = Convert.ToInt32(reader.GetValue(8));

                        }
                    }
                    emp.ProposalDenominationCode = Convert.ToInt32(reader.GetValue(9));
                    emp.ProgramID = Convert.ToInt32(reader.GetValue(10));
                    emp.AccountCode = Convert.ToInt32(reader.GetValue(11));
                    prog.Add(emp);
                }
                return prog;
            }
        }
        public IEnumerable<Cancelled> CancelledAccounts(int? prog_ID, int? proy_ID, int? ooe_id)
        {
            List<Cancelled> prog = new List<Cancelled>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT DISTINCT
dbo.tbl_T_BMSBudgetProposal.ProposalID,
dbo.tbl_T_BMSBudgetProposal.ProposalYear,
dbo.tbl_T_BMSBudgetProposal.ProposalAmount,
dbo.tbl_R_BMSProposalRemark.Remarks,
dbo.tbl_R_BMSProgramAccounts.AccountName,
dbo.tbl_R_BMSObjectOfExpenditure.OOEAbrevation,
dbo.tbl_R_BMSProposalRemark.OfficeLevel,
dbo.tbl_R_BMSOffices.OfficeName,
pmis.dbo.vwMergeAllEmployee.EmpName,
dbo.tbl_R_BMSProgramAccounts.ActionCode,
dbo.tbl_R_BMSProgramAccounts.AccountYear,
dbo.tbl_R_BMSOffices.OfficeID,
dbo.tbl_T_BMSBudgetProposal.ProgramID,
dbo.tbl_T_BMSBudgetProposal.AccountID,
dbo.tbl_T_BMSBudgetProposal.ProposalYear,
dbo.tbl_T_BMSBudgetProposal.ProposalDenominationCode

FROM
dbo.tbl_T_BMSBudgetProposal
INNER JOIN dbo.tbl_R_BMSProposalRemark ON dbo.tbl_T_BMSBudgetProposal.ProposalID = dbo.tbl_R_BMSProposalRemark.ProposalID
INNER JOIN dbo.tbl_R_BMSProgramAccounts ON dbo.tbl_T_BMSBudgetProposal.AccountID = dbo.tbl_R_BMSProgramAccounts.AccountID
INNER JOIN dbo.tbl_R_BMSObjectOfExpenditure ON dbo.tbl_R_BMSProgramAccounts.ObjectOfExpendetureID = dbo.tbl_R_BMSObjectOfExpenditure.OOEID
INNER JOIN dbo.tbl_R_BMSOfficePrograms ON dbo.tbl_R_BMSProgramAccounts.ProgramID = dbo.tbl_R_BMSOfficePrograms.ProgramID
INNER JOIN dbo.tbl_R_BMSOffices ON dbo.tbl_R_BMSOfficePrograms.OfficeID = dbo.tbl_R_BMSOffices.OfficeID
INNER JOIN pmis.dbo.vwMergeAllEmployee ON dbo.tbl_R_BMSProposalRemark.UserID = pmis.dbo.vwMergeAllEmployee.eid
                            WHERE
                            ProposalYear = '" + proy_ID + "' AND Remarks != 'Approved' AND dbo.tbl_R_BMSObjectOfExpenditure.OOEID = '" + ooe_id + "' AND dbo.tbl_R_BMSProgramAccounts.ActionCode = 1 AND AccountYear = '" + proy_ID + "' AND dbo.tbl_T_BMSBudgetProposal.ProgramID = '" + prog_ID + "' AND dbo.tbl_R_BMSOffices.OfficeID = '" + Account.UserInfo.Department.ToString() + "' ORDER BY AccountName", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Cancelled emp = new Cancelled();
                    emp.ProposalID = reader.GetInt64(0);
                    emp.ProposalYear = Convert.ToInt32(reader.GetValue(1));
                    emp.ProposalAmount = Convert.ToDouble(reader.GetValue(2));
                    emp.Remarks = reader.GetString(3);
                    emp.AccountName = reader.GetString(4);
                    emp.OOE = reader.GetString(5);
                    emp.OfficeLevel = reader.GetString(6);
                    emp.OfficeName = reader.GetString(7);
                    emp.EmpName = reader.GetString(8);
                    emp.ProgramID = Convert.ToInt32(reader.GetValue(12));
                    emp.AccountID = Convert.ToInt32(reader.GetValue(13));
                    emp.ProposalYear = Convert.ToInt32(reader.GetValue(14));
                    emp.ProposalDenominationCode = Convert.ToInt32(reader.GetValue(15));
                    prog.Add(emp);
                }
            }
            return prog;
            // Last Modified by : Ranel Cator
        }

        public void UpdateAccount1(IEnumerable<AccountsModel> Accounts)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                foreach (AccountsModel Account in Accounts)
                {
                    SqlCommand updateAmount = new SqlCommand(@"UPDATE tbl_T_BMSBudgetProposal set ProposalAmount = " + Account.ProposalAllotedAmount + " WHERE ProposalID = " + Account.ProposalID + "", con);
                    updateAmount.ExecuteNonQuery();
                }
            }
        }
        public void UpdateCurrentAmounts(IEnumerable<AccountsModel> Accounts)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                foreach (AccountsModel Account in Accounts)
                {
                    var ProposalID = Account.ProposalID;
                    var Amount = Account.ProposalAllotedAmount;

                    try
                    {
                        SqlCommand updateAmount = new SqlCommand(@"UPDATE tbl_T_BMSBudgetProposal set ProposalActionCode = 2, ProposalAmount = " + Amount + " WHERE ProposalID = " + ProposalID + "", con);
                        updateAmount.ExecuteNonQuery();
                    }
                    catch
                    {

                    }
                }
            }
        }



        //_________________________________________________________________________________________________________________________


        public IEnumerable<AccountsModel> BudgetApprovedAccountsHRMOInCharge(int? prog_ID, int? office_ID, int? proy_ID, int? ooe_id)
        {
            List<AccountsModel> prog = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                prog_ID = prog_ID == null ? 0 : prog_ID;
                proy_ID = proy_ID == null ? 0 : proy_ID;
                ooe_id = ooe_id == null ? 0 : ooe_id;
                office_ID = office_ID == null ? 0 : office_ID;

                SqlCommand com = new SqlCommand(@"SELECT DISTINCT a.ProposalID, a.AccountID,  e.AccountName, f.OOEAbrevation,a.ProposalYear,a.ProposalAllotedAmount, a.ProposalAmount, c.ProgramID, b.AccountCode FROM dbo.tbl_T_BMSBudgetProposal AS a
                                                    left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID 
                                                    left JOIN dbo.tbl_R_BMSAccounts AS b ON b.AccountID = e.AccountID 
                                                    left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID 
                                                    left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID 
                                                    LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID 
                                                    LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode
                                                    WHERE c.OfficeID = '" + office_ID + "' and f.OOEID = " + ooe_id + " and  c.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proy_ID + "' and e.AccountYear = '" + proy_ID
                                                    + "' and a.ProposalActionCode = '1' and e.ActionCode='1' and a.ProposalStatusHR = '1' ORDER BY AccountName", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel emp = new AccountsModel();
                    emp.ProposalID = reader.GetInt64(0);
                    emp.AccountID = reader.GetInt32(1);
                    emp.AccountName = reader.GetValue(2).ToString();
                    emp.OOEName = reader.GetString(3);
                    emp.ProposalYear = reader.GetInt32(4);
                    emp.ProposalAllotedAmount = Convert.ToDouble(reader.GetValue(5));
                    emp.ProposalAmmount = Convert.ToDouble(reader.GetValue(6));
                    emp.ProgramID = Convert.ToInt32(reader.GetValue(7));
                    emp.AccountCode = Convert.ToInt32(reader.GetValue(8));
                    //emp.PastYear = getPastYearAmount(proy_ID, reader.GetValue(8).ToString(), office_ID);
                    var programID = prog_ID.ToString();
                    emp.PastYear = getPastYearAmount(proy_ID, reader.GetValue(1).ToString(), office_ID, programID, prog_ID);
                    prog.Add(emp);
                }
            }
            return prog;
        }
        public IEnumerable<AccountsModel> BudgetApprovedAccountsBudgetInCharge(int? prog_ID, int? office_ID, int? proy_ID, int? ooe_id)
        {
            List<AccountsModel> prog = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                prog_ID = prog_ID == null ? 0 : prog_ID;
                proy_ID = proy_ID == null ? 0 : proy_ID;
                ooe_id = ooe_id == null ? 0 : ooe_id;
                office_ID = office_ID == null ? 0 : office_ID;
                SqlCommand com = new SqlCommand(@"SELECT DISTINCT a.ProposalID, a.AccountID,  e.AccountName, f.OOEAbrevation,a.ProposalYear,a.ProposalAllotedAmount, a.ProposalAmount, c.ProgramID, a.ProposalDenominationCode, b.AccountCode FROM dbo.tbl_T_BMSBudgetProposal AS a
                                                    left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID 
                                                    left JOIN dbo.tbl_R_BMSAccounts AS b ON b.AccountID = e.AccountID 
                                                    left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID 
                                                    left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID 
                                                    LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID 
                                                    LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode
                                                    WHERE c.OfficeID = '" + office_ID + "' and f.OOEID = " + ooe_id + " and  c.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proy_ID + "' and e.AccountYear = '" + proy_ID
                                                    + "' and a.ProposalActionCode = '1' and e.ActionCode='1' and a.ProposalStatusInCharge = '1' ORDER BY AccountName", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel emp = new AccountsModel();
                    emp.ProposalID = reader.GetInt64(0);
                    emp.AccountID = reader.GetInt32(1);
                    emp.AccountName = reader.GetValue(2).ToString();
                    emp.OOEName = reader.GetString(3);
                    emp.ProposalYear = reader.GetInt32(4);
                    emp.ProposalAllotedAmount = Convert.ToDouble(reader.GetValue(5));
                    emp.ProposalAmmount = Convert.ToDouble(reader.GetValue(6));
                    emp.ProgramID = Convert.ToInt32(reader.GetValue(7));
                    emp.ProposalDenominationCode = Convert.ToInt32(reader.GetValue(8));
                    emp.AccountCode = Convert.ToInt32(reader.GetValue(9));
                    //emp.PastYear = getPastYearAmount(proy_ID, reader.GetValue(9).ToString(), office_ID);
                    var programID = prog_ID.ToString();
                    emp.PastYear = getPastYearAmount(proy_ID, reader.GetValue(1).ToString(), office_ID, programID, prog_ID);
                    prog.Add(emp);
                }
            }
            return prog;
        }

        public IEnumerable<AccountsModel> BudgetApprovedAccountsCommittee(int? prog_ID, int? office_ID, int? proy_ID, int? ooe_id)
        {
            List<AccountsModel> prog = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                prog_ID = prog_ID == null ? 0 : prog_ID;
                proy_ID = proy_ID == null ? 0 : proy_ID;
                ooe_id = ooe_id == null ? 0 : ooe_id;
                office_ID = office_ID == null ? 0 : office_ID;

                SqlCommand com = new SqlCommand(@"SELECT DISTINCT a.ProposalID, a.AccountID,  e.AccountName, f.OOEAbrevation,a.ProposalYear,a.ProposalAllotedAmount, a.ProposalAmount, c.ProgramID, a.ProposalDenominationCode, b.AccountCode FROM dbo.tbl_T_BMSBudgetProposal AS a
                                                    left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID and e.ActionCode = a.ProposalActionCode
                                                    left JOIN dbo.tbl_R_BMSAccounts AS b ON b.FMISAccountCode = e.AccountID 
                                                    left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID 
                                                    left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID 
                                                    LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID 
                                                    LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode
                                                    WHERE c.OfficeID = '" + office_ID + "' and f.OOEID = " + ooe_id + " and  c.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proy_ID + "' and e.AccountYear = '" + proy_ID
                                                    + "' and a.ProposalActionCode = '1' and e.ActionCode='1' and a.ProposalStatusCommittee = '1' ORDER BY AccountName", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel emp = new AccountsModel();
                    emp.ProposalID = reader.GetInt64(0);
                    emp.AccountID = reader.GetInt32(1); ;
                    emp.AccountName = reader.GetValue(2).ToString();
                    emp.OOEName = reader.GetString(3);
                    emp.ProposalYear = reader.GetInt32(4);
                    emp.ProposalAllotedAmount = Convert.ToDouble(reader.GetValue(5));
                    emp.ProposalAmmount = Convert.ToDouble(reader.GetValue(6));
                    emp.ProgramID = Convert.ToInt32(reader.GetValue(7));
                    //emp.ProposalDenominationCode = Convert.ToInt32(reader.GetValue(8));
                    emp.AccountCode = Convert.ToInt32(reader.GetValue(9));
                    //emp.PastYear = getPastYearAmount(proy_ID, reader.GetValue(9).ToString(), office_ID);
                    var programID = prog_ID.ToString();
                    emp.PastYear = getPastYearAmount(proy_ID, reader.GetValue(1).ToString(), office_ID, programID, prog_ID);

                    prog.Add(emp);
                }
            }
            return prog;
        }
        #region Old Budget Incharge Proposed Grid Data Unused
//        public IEnumerable<AccountsModel> BudgetInChargeProposedAccounts(int? prog_ID, int? office_ID, int? proy_ID, int? ooe_id)
//        {
//            List<AccountsModel> prog = new List<AccountsModel>();
//            using (SqlConnection con = new SqlConnection(Common.MyConn()))
//            {

//                SqlCommand com = new SqlCommand(@"WITH x AS 
//                                                    (
//                                                    SELECT DISTINCT a.ProposalID, a.AccountID,  e.AccountName, f.OOEAbrevation,a.ProposalYear,a.ProposalAmount, c.ProgramID, a.ProposalStatusHR, a.ProposalStatusInCharge, a.ProposalStatusCommittee, a.ProposalDenominationCode, b.AccountCode
//                                                    FROM dbo.tbl_T_BMSBudgetProposal AS a
//                                                    left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID 
//                                                    left JOIN dbo.tbl_R_BMSAccounts AS b ON b.FMISAccountCode = e.AccountID 
//                                                    left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID 
//                                                    left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID 
//                                                    LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID 
//                                                    LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode
//
//                                                    WHERE c.OfficeID = '" + office_ID + "' and  a.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proy_ID
//                                                    + "' and e.AccountYear = '" + proy_ID
//                                                    + "' and a.ProposalActionCode = '1' " + ""
//                                                    + ") " + ""

//                                                    + " SELECT DISTINCT x.ProposalID, x.AccountID,  x.AccountName, x.OOEAbrevation,x.ProposalYear,a.ProposalAllotedAmount " + ""
//                                                    + " as PastYear, x.ProposalAmount as presentYear, c.ProgramID,x.ProposalStatusHR, x.ProposalStatusInCharge, x.ProposalStatusCommittee, isnull(x.ProposalDenominationCode, 0), b.AccountCode FROM dbo.tbl_T_BMSBudgetProposal AS a " + ""
//                                                    + " left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID " + ""
//                                                    + " left JOIN dbo.tbl_R_BMSAccounts AS b ON b.FMISAccountCode = e.AccountID " + ""
//                                                    + " left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID " + ""
//                                                    + " left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID " + ""
//                                                    + " LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID " + ""
//                                                    + " LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode " + ""
//                                                    + " left JOIN x as x on a.AccountID = x.AccountID " + ""
//                                                    + " WHERE c.OfficeID = '" + office_ID + "' and f.OOEID = '" + ooe_id + "' and  a.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proy_ID
//                                                    + "' - 1 and e.AccountYear = '" + proy_ID
//                                                    + "' - 1 and a.ProposalActionCode = '1' and e.ActionCode='1' and a.AccountID= x.AccountID ORDER BY AccountName", con);

//                con.Open();
//                SqlDataReader reader = com.ExecuteReader();
//                while (reader.Read())
//                {
//                    AccountsModel emp = new AccountsModel();
//                    emp.ProposalID = reader.GetInt64(0);
//                    emp.AccountID = reader.GetInt32(1);
//                    emp.AccountName = reader.GetValue(2).ToString();
//                    emp.OOEName = reader.GetString(3);
//                    emp.ProposalYear = reader.GetInt32(4);
//                    emp.PastProposalAmmount = Convert.ToDouble(reader.GetValue(5));
//                    emp.ProposalAmmount = Convert.ToDouble(reader.GetValue(6));
//                    emp.ProgramID = Convert.ToInt32(reader.GetValue(7));
//                    emp.ProposalStatusHR = Convert.ToInt32(reader.GetValue(8));
//                    emp.ProposalStatusInCharge = Convert.ToInt32(reader.GetValue(9));
//                    emp.ProposalStatusCommitteeINT = Convert.ToInt32(reader.GetValue(10));
//                    emp.ProposalDenominationCode = Convert.ToInt32(reader.GetValue(11));
//                    emp.AccountCode = Convert.ToInt32(reader.GetValue(12));
//                    //emp.PastYear = getPastYearAmount(proy_ID, reader.GetValue(12).ToString(), office_ID);
//                    var programID = prog_ID.ToString();
//                    emp.PastYear = getPastYearAmount(proy_ID, reader.GetValue(1).ToString(), office_ID, programID, prog_ID);
//                    prog.Add(emp);
//                }

//                reader.Close();
//                SqlCommand Proposed = new SqlCommand(@"WITH x AS 
//                                                    (
//                                                    SELECT DISTINCT a.ProposalID, a.AccountID,  e.AccountName, f.OOEAbrevation,a.ProposalYear,a.ProposalAmount, c.ProgramID, a.ProposalStatusHR, a.ProposalStatusInCharge, a.ProposalStatusCommittee, a.ProposalDenominationCode, b.AccountCode
//                                                    FROM dbo.tbl_T_BMSBudgetProposal AS a
//                                                    left JOIN dbo.tbl_R_BMSProposedAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID 
//                                                    left JOIN dbo.tbl_R_BMSAccounts AS b ON b.FMISAccountCode = e.AccountID 
//                                                    left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID 
//                                                    left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID 
//                                                    LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.OOEID 
//                                                    LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode
//
//                                                    WHERE c.OfficeID = '" + office_ID + "' and  a.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proy_ID
//                                                    + "' and e.ProposalYear = '" + proy_ID
//                                                    + "' and a.ProposalActionCode = '1' " + ""
//                                                    + ") " + ""

//                                                    + " SELECT DISTINCT x.ProposalID, x.AccountID,  x.AccountName, x.OOEAbrevation,x.ProposalYear,a.ProposalAllotedAmount " + ""
//                                                    + " as PastYear, x.ProposalAmount as presentYear, c.ProgramID,x.ProposalStatusHR, x.ProposalStatusInCharge, x.ProposalStatusCommittee, isnull(x.ProposalDenominationCode, 0), b.AccountCode FROM dbo.tbl_T_BMSBudgetProposal AS a " + ""
//                                                    + " left JOIN dbo.tbl_R_BMSProposedAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID " + ""
//                                                    + " left JOIN dbo.tbl_R_BMSAccounts AS b ON b.FMISAccountCode = e.AccountID " + ""
//                                                    + " left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID " + ""
//                                                    + " left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID " + ""
//                                                    + " LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.OOEID " + ""
//                                                    + " LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode " + ""
//                                                    + " left JOIN x as x on a.AccountID = x.AccountID " + ""
//                                                    + " WHERE c.OfficeID = '" + office_ID + "' and f.OOEID = '" + ooe_id + "' and  a.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proy_ID
//                                                    + "' - 1 and e.ProposalYear = '" + proy_ID
//                                                    + "' - 1 and a.ProposalActionCode = '1' and e.ActionCode='1' and a.AccountID= x.AccountID ORDER BY AccountName", con);

//                SqlDataReader readerProposed = Proposed.ExecuteReader();
//                while (readerProposed.Read())
//                {
//                    AccountsModel emp = new AccountsModel();
//                    emp.ProposalID = readerProposed.GetInt64(0);
//                    emp.AccountID = readerProposed.GetInt32(1);
//                    emp.AccountName = readerProposed.GetValue(2).ToString();
//                    emp.OOEName = readerProposed.GetString(3);
//                    emp.ProposalYear = readerProposed.GetInt32(4);
//                    emp.PastProposalAmmount = Convert.ToDouble(readerProposed.GetValue(5));
//                    emp.ProposalAmmount = Convert.ToDouble(readerProposed.GetValue(6));
//                    emp.ProgramID = Convert.ToInt32(readerProposed.GetValue(7));
//                    emp.ProposalStatusHR = Convert.ToInt32(readerProposed.GetValue(8));
//                    emp.ProposalStatusInCharge = Convert.ToInt32(readerProposed.GetValue(9));
//                    emp.ProposalStatusCommitteeINT = Convert.ToInt32(readerProposed.GetValue(10));
//                    emp.ProposalDenominationCode = Convert.ToInt32(readerProposed.GetValue(11));
//                    emp.AccountCode = Convert.ToInt32(readerProposed.GetValue(12));
//                    //emp.PastYear = getPastYearAmount(proy_ID, reader.GetValue(12).ToString(), office_ID);
//                    var programID = prog_ID.ToString();
//                    emp.PastYear = getPastYearAmount(proy_ID, reader.GetValue(1).ToString(), office_ID, programID, prog_ID);
//                    prog.Add(emp);
//                }
//            }
//            return prog;
//        } 
        #endregion
        public IEnumerable<AccountsModel> BudgetInChargeProposedAccounts(int? prog_ID, int? office_ID, int? proy_ID, int? ooe_id)
        {
            List<AccountsModel> prog = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_getDataProposedTabOfficeAdmin " + proy_ID + ", " + Account.UserInfo.Department + "", con);

                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel emp = new AccountsModel();
                    
                    emp.AccountName = reader.GetValue(0).ToString();
                    emp.AccountCode = Convert.ToInt32(reader.GetValue(1));
                    emp.PastYear = Convert.ToDouble(reader.GetValue(2));
                    emp.PastProposalAmmount = Convert.ToDouble(reader.GetValue(3));
                    emp.ProposalAmmount = Convert.ToDouble(reader.GetValue(4));
                    emp.AccountID = reader.GetInt32(6);
                    emp.OOEID = Convert.ToInt32(reader.GetValue(7));
                    emp.ProgramID = Convert.ToInt32(reader.GetValue(8));
                    emp.ProposalYear = Convert.ToInt32(proy_ID);
                    prog.Add(emp);
                }
            }
            return prog;
        }
        public IEnumerable<AccountsModel> getExistingAccounts()
        {
            List<AccountsModel> prog = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"select AccountID, AccountName + ' (' + ChildAccountCode + ')' as 'AccountName',FundType from tbl_R_BMSAccounts 
                                                WHERE Active =1 
                                                and AccountName != ''
                                                ORDER BY AccountName", con);

                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel emp = new AccountsModel();

                    emp.AccountID = Convert.ToInt32(reader.GetValue(0));
                    emp.AccountName = reader.GetValue(1).ToString();
                    emp.FundName = reader.GetValue(2).ToString();

                    prog.Add(emp);
                }
            }
            return prog;
        }
        
        public IEnumerable<AccountsModel> BudgetforApprovalAccountsBudgetInCharge(int? prog_ID, int? office_ID, int? proy_ID, int? ooe_id, double? percent)
        {
            List<AccountsModel> prog = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"WITH x AS 
                                                    (
                                                    SELECT DISTINCT a.ProposalID, a.AccountID,  e.AccountName, f.OOEAbrevation,a.ProposalYear,a.ProposalAmount, c.ProgramID, a.ProposalStatusHR, a.ProposalStatusInCharge, a.ProposalStatusCommittee, a.ProposalDenominationCode, b.AccountCode, f.OOEID, pr.Remarks
                                                    FROM dbo.tbl_T_BMSBudgetProposal AS a
                                                    left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID 
                                                    left JOIN dbo.tbl_R_BMSAccounts AS b ON b.FMISAccountCode = e.AccountID 
                                                    left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID 
                                                    left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID 
                                                    LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID 
                                                    LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode
                                                    LEFT JOIN dbo.tbl_R_BMSProposalRemark as pr ON a.ProposalID = pr.ProposalID
                                                    WHERE c.OfficeID = '" + office_ID + "' and  a.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proy_ID
                                                    + "' and e.AccountYear = '" + proy_ID
                                                    + "' and a.ProposalActionCode = '1' and e.ActionCode='1' and a.ProposalStatusHR = '1' and a.ProposalStatusInCharge='2' " + ""
                                                    + " and a.ProposalStatusCommittee = '2' " + ""
                                                    + ") " + ""

                                                    + " SELECT DISTINCT x.ProposalID, x.AccountID,  x.AccountName, x.OOEAbrevation,x.ProposalYear,a.ProposalAllotedAmount " + ""
                                                    + " as PastYear, x.ProposalAmount as presentYear, c.ProgramID,x.ProposalStatusHR, x.ProposalStatusInCharge, x.ProposalStatusCommittee, x.ProposalDenominationCode, b.AccountCode, f.OOEID, d.OfficeID, ISNULL(x.Remarks, '') as Remarks FROM dbo.tbl_T_BMSBudgetProposal AS a " + ""
                                                    + " left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID " + ""
                                                    + " left JOIN dbo.tbl_R_BMSAccounts AS b ON b.FMISAccountCode = e.AccountID " + ""
                                                    + " left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID " + ""
                                                    + " left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID " + ""
                                                    + " LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID " + ""
                                                    + " LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode " + ""
                                                    + " left JOIN x as x on a.AccountID = x.AccountID LEFT JOIN dbo.tbl_R_BMSProposalRemark as pr ON pr.ProposalID = a.ProposalID " + ""
                                                    + " WHERE c.OfficeID = '" + office_ID + "' and f.OOEID = '" + ooe_id + "' and  a.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proy_ID
                                                    + "' - 1 and e.AccountYear = '" + proy_ID
                                                    + "' - 1 and a.ProposalActionCode = '1' and e.ActionCode='1' and a.AccountID= x.AccountID ORDER BY AccountCode, AccountName", con);
                #region OldQuery
                //                SqlCommand com = new SqlCommand(@"SELECT DISTINCT a.ProposalID, a.AccountID,  e.AccountName, f.OOEAbrevation,a.ProposalYear,a.ProposalAmount FROM dbo.tbl_T_BMSBudgetProposal AS a
                //                                                    left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID 
                //                                                    left JOIN dbo.tbl_R_BMSAccounts AS b ON b.AccountID = e.AccountID 
                //                                                    left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID 
                //                                                    left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID 
                //                                                    LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID 
                //                                                    LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode
                //                                                    WHERE c.OfficeID = '" + office_ID + "' and  a.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proy_ID + "' and e.AccountYear = '" + proy_ID 
                //                                                    + "' and a.ProposalActionCode = '1' and e.ActionCode='1' and a.ProposalStatusHR = '1' and a.ProposalStatusInCharge='2' and a.ProposalStatusCommittee = '2'", con); 
                #endregion
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel emp = new AccountsModel();
                    emp.ProposalID = reader.GetInt64(0);
                    emp.AccountID = reader.GetInt32(1);
                    emp.AccountName = reader.GetValue(2).ToString();
                    emp.OOEName = reader.GetString(3);
                    emp.ProposalYear = reader.GetInt32(4);
                    emp.PastProposalAmmount = Convert.ToDouble(reader.GetValue(5));
                    emp.ProposalAmmount = Convert.ToDouble(reader.GetValue(6));
                    emp.ProgramID = Convert.ToInt32(reader.GetValue(7));
                    emp.ProposalStatusHR = Convert.ToInt32(reader.GetValue(8));
                    emp.ProposalStatusInCharge = Convert.ToInt32(reader.GetValue(9));
                    emp.ProposalStatusCommitteeINT = Convert.ToInt32(reader.GetValue(10));
                    emp.ProposalDenominationCode = Convert.ToInt32(reader.GetValue(11));
                    emp.AccountCode = Convert.ToInt32(reader.GetValue(12));
                    //emp.PastYear = getPastYearAmount(proy_ID, reader.GetValue(12).ToString(), office_ID);
                    var programID = prog_ID.ToString();
                    emp.PastYear = getPastYearAmount(proy_ID, reader.GetValue(1).ToString(), office_ID, programID, prog_ID);
                    var percentMultiply = emp.PastProposalAmmount * percent; // GET Total Amount with Percentage
                    var percentDif = emp.PastProposalAmmount - percentMultiply; // GET Difference from PastProposal and Amount Percentage as Difference A
                    var DifferenceA = Math.Abs(Convert.ToDouble(percentDif)); // Convert to Absolute Value
                    var dif = emp.PastProposalAmmount - emp.ProposalAmmount; // GET Difference from Past Proposal and Proposed Amount as Difference B
                    var DifferenceB = Math.Abs(dif); // Convert to Absolute Value
                    if (percent != 1)
                    {
                        if (emp.ProposalAmmount > emp.PastProposalAmmount)
                        {
                            if (DifferenceB > DifferenceA) // Compare if Difference B is greater than Difference A then Proposed exceeded to Percentage Increase
                            {
                                emp.AmountStatus = 1;
                            }
                            else
                            {
                                emp.AmountStatus = 0;
                            }

                        }
                        else if (emp.ProposalAmmount < emp.PastProposalAmmount)
                        {
                            emp.AmountStatus = 3;
                        }
                    }

                    emp.OOEID = Convert.ToInt32(reader.GetValue(13));
                    emp.SelectedOfficeID = Convert.ToInt32(reader.GetValue(14));
                    emp.setRemarks = reader.GetString(15);
                    prog.Add(emp);
                }

                if (ooe_id == 1)
                {

                    AccountsModel ForFundingAccount = new AccountsModel();
                    ForFundingAccount.ProposalID = 0;
                    ForFundingAccount.AccountID = 0;
                    ForFundingAccount.AccountCode = 0;
                    ForFundingAccount.AccountName = "Proposed Positions";
                    ForFundingAccount.OOEName = "PS";
                    ForFundingAccount.CurrentProposalAmount = 0;
                    ForFundingAccount.ProposalYear = 2017;
                    ForFundingAccount.ProgramID = Convert.ToInt32(prog_ID);
                    ForFundingAccount.OOEID = 1;
                    ForFundingAccount.ProposalStatusHR = 2;

                    if (ForFundingAccount.OOEID == 1 || ForFundingAccount.OOEID == 2 || ForFundingAccount.OOEID == 3)
                    {
                        ForFundingAccount.SlashAmount = getforFundingTotal(proy_ID, office_ID);
                        ForFundingAccount.ProposalAmmount = getforFundingSubmittedTotal(proy_ID, office_ID);
                    }
                    else
                    {
                        ForFundingAccount.ProposalAmmount = 0;
                    }
                    if (ForFundingAccount.ProposalAmmount == 0)
                    {
                        ForFundingAccount.checker = 0;
                    }
                    else
                    {
                        ForFundingAccount.checker = 1;
                    }
                    ForFundingAccount.PastProposalAmmount = 0;
                    if (getCountUnreviewedPosition(proy_ID, office_ID, "StatusBudget") != 0)
                    {
                        prog.Add(ForFundingAccount);
                    }


                    AccountsModel ForFundingAccountCasual = new AccountsModel();
                    ForFundingAccountCasual.ProposalID = 0;
                    ForFundingAccountCasual.AccountID = 0;
                    ForFundingAccountCasual.AccountCode = 0;
                    ForFundingAccountCasual.AccountName = "Proposed Casual";
                    ForFundingAccountCasual.OOEName = "PS";
                    ForFundingAccountCasual.CurrentProposalAmount = 0;
                    ForFundingAccountCasual.ProposalYear = 2017;
                    ForFundingAccountCasual.ProgramID = Convert.ToInt32(prog_ID);
                    ForFundingAccountCasual.OOEID = 1;
                    ForFundingAccountCasual.ProposalStatusHR = 2;

                    if (ForFundingAccountCasual.OOEID == 1 || ForFundingAccountCasual.OOEID == 2 || ForFundingAccountCasual.OOEID == 3)
                    {
                        ForFundingAccountCasual.SlashAmount = getforFundingTotalCasual(proy_ID, office_ID);
                        ForFundingAccountCasual.ProposalAmmount = getforFundingSubmittedCasual(proy_ID, office_ID);
                    }
                    else
                    {
                        ForFundingAccountCasual.ProposalAmmount = 0;
                    }
                    if (ForFundingAccountCasual.ProposalAmmount == 0)
                    {
                        ForFundingAccountCasual.checker = 0;
                    }
                    else
                    {
                        ForFundingAccountCasual.checker = 1;
                    }
                    ForFundingAccountCasual.PastProposalAmmount = 0;
                    if (getCountUnreviewedCasual(proy_ID, office_ID, "StatusBudget") != 0)
                    {
                        prog.Add(ForFundingAccountCasual);
                    }

                }
                reader.Close();
                // GET Proposed New Amount
                SqlCommand PNA = new SqlCommand(@"SELECT a.AccountID, a.AccountName, 
                    a.AccountCode, b.ProposalID, b.ProposalAmount, 
                    b.ProposalStatusHR, b.ProposalStatusInCharge, b.ProposalStatusCommittee, 
                    b.ProposalDenominationCode, a.OOEID, ISNULL(c.Remarks, '')  FROM tbl_R_BMSProposedAccounts as a " +
                    "LEFT JOIN tbl_T_BMSBudgetProposal as b ON a.AccountID = b.AccountID  and a.ProgramID = b.ProgramID " +
                    "LEFT JOIN tbl_R_BMSProposalRemark as c ON c.ProposalID = b.ProposalID " +
                    "WHERE a.OfficeID = '" + office_ID + "'and a.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proy_ID + "' and a.OOEID = '" + ooe_id + "' and a.ActionCode = 1  and b.ProposalActionCode = 1 and ProposalStatusInCharge = 2", con);
                SqlDataReader reader_PNA = PNA.ExecuteReader();
                while (reader_PNA.Read())
                {
                    AccountsModel PNAlist = new AccountsModel();
                    PNAlist.AccountID = Convert.ToInt32(reader_PNA.GetValue(0));
                    PNAlist.AccountName = reader_PNA.GetString(1);
                    PNAlist.AccountCode = Convert.ToInt32(reader_PNA.GetValue(2));
                    PNAlist.ProposalID = Convert.ToInt32(reader_PNA.GetValue(3));
                    PNAlist.ProposalAmmount = Convert.ToDouble(reader_PNA.GetValue(4));
                    PNAlist.ProposalStatusHR = Convert.ToInt32(reader_PNA.GetValue(5));
                    PNAlist.ProposalStatusInCharge = Convert.ToInt32(reader_PNA.GetValue(6));
                    PNAlist.ProposalStatusCommitteeINT = Convert.ToInt32(reader_PNA.GetValue(7));
                    PNAlist.ProposalDenominationCode = Convert.ToInt32(reader_PNA.GetValue(8));
                    PNAlist.OOEID = Convert.ToInt32(reader_PNA.GetValue(9));
                    PNAlist.setRemarks = reader_PNA.GetString(10);
                    prog.Add(PNAlist);
                }
                reader_PNA.Close();

            }
            return prog;
        }
        public IEnumerable<AccountsModel> BudgetforApprovalAccountsHRMOInCharge(int? prog_ID, int? office_ID, int? proy_ID, int? ooe_id)
        {
            List<AccountsModel> prog = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"WITH x AS 
                                                    (
                                                    SELECT DISTINCT a.ProposalID, a.AccountID,  e.AccountName, f.OOEAbrevation,a.ProposalYear,a.ProposalAmount, c.ProgramID, a.ProposalStatusHR, a.ProposalStatusInCharge, a.ProposalStatusCommittee, a.ProposalDenominationCode, b.AccountCode
                                                    FROM dbo.tbl_T_BMSBudgetProposal AS a
                                                    left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID 
                                                    left JOIN dbo.tbl_R_BMSAccounts AS b ON b.FMISAccountCode = e.AccountID 
                                                    left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID 
                                                    left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID 
                                                    LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID 
                                                    LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode

                                                    WHERE c.OfficeID = '" + office_ID + "' and  a.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proy_ID
                                                    + "' and e.AccountYear = '" + proy_ID
                                                    + "' and a.ProposalActionCode = '1' and e.ActionCode='1' and a.ProposalStatusHR = '2' and a.ProposalStatusInCharge='2' " + ""
                                                    + " and a.ProposalStatusCommittee = '2' " + ""
                                                    + ") " + ""

                                                    + " SELECT DISTINCT x.ProposalID, x.AccountID,  x.AccountName, x.OOEAbrevation,x.ProposalYear,a.ProposalAllotedAmount " + ""
                                                    + " as PastYear, x.ProposalAmount as presentYear, c.ProgramID,x.ProposalStatusHR, x.ProposalStatusInCharge, x.ProposalStatusCommittee, x.ProposalDenominationCode, b.AccountCode FROM dbo.tbl_T_BMSBudgetProposal AS a " + ""
                                                    + " left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID " + ""
                                                    + " left JOIN dbo.tbl_R_BMSAccounts AS b ON b.FMISAccountCode = e.AccountID " + ""
                                                    + " left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID " + ""
                                                    + " left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID " + ""
                                                    + " LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID " + ""
                                                    + " LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode " + ""
                                                    + " left JOIN x as x on a.AccountID = x.AccountID " + ""
                                                    + " WHERE c.OfficeID = '" + office_ID + "' and f.OOEID = '" + ooe_id + "' and  a.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proy_ID
                                                    + "' - 1 and e.AccountYear = '" + proy_ID
                                                    + "' - 1 and a.ProposalActionCode = '1' and e.ActionCode='1' and a.AccountID= x.AccountID ORDER BY AccountName", con);

                #region oldQuery
                //                SqlCommand com = new SqlCommand(@"SELECT DISTINCT a.ProposalID, a.AccountID,  e.AccountName, f.OOEAbrevation,a.ProposalYear,a.ProposalAmount FROM dbo.tbl_T_BMSBudgetProposal AS a
                //                                                    left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID 
                //                                                    left JOIN dbo.tbl_R_BMSAccounts AS b ON b.AccountID = e.AccountID 
                //                                                    left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID 
                //                                                    left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID 
                //                                                    LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID 
                //                                                    LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode
                //                                                    WHERE c.OfficeID = '" + office_ID + "' and  a.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proy_ID + "' and e.AccountYear = '" + proy_ID
                //                                                    + "' and a.ProposalActionCode = '1' and e.ActionCode='1' and a.ProposalStatusHR = '2' and a.ProposalStatusInCharge='2' and a.ProposalStatusCommittee = '2'", con); 
                #endregion
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel emp = new AccountsModel();
                    emp.ProposalID = reader.GetInt64(0);
                    emp.AccountID = reader.GetInt32(1);
                    emp.AccountName = reader.GetValue(2).ToString();
                    emp.OOEName = reader.GetString(3);
                    emp.ProposalYear = reader.GetInt32(4);
                    emp.PastProposalAmmount = Convert.ToDouble(reader.GetValue(5));
                    emp.ProposalAmmount = Convert.ToDouble(reader.GetValue(6));
                    emp.ProgramID = Convert.ToInt32(reader.GetValue(7));
                    emp.ProposalStatusHR = Convert.ToInt32(reader.GetValue(8));
                    emp.ProposalStatusInCharge = Convert.ToInt32(reader.GetValue(9));
                    emp.ProposalStatusCommitteeINT = Convert.ToInt32(reader.GetValue(10));
                    emp.ProposalDenominationCode = Convert.ToInt32(reader.GetValue(11));
                    emp.AccountCode = Convert.ToInt32(reader.GetValue(12));
                    //emp.PastYear = getPastYearAmount(proy_ID, reader.GetValue(12).ToString(), office_ID);
                    var programID = prog_ID.ToString();
                    emp.PastYear = getPastYearAmount(proy_ID, reader.GetValue(1).ToString(), office_ID, programID, prog_ID);
                    prog.Add(emp);
                }
            }
            AccountsModel ForFundingAccount = new AccountsModel();
            ForFundingAccount.ProposalID = 0;
            ForFundingAccount.AccountID = 0;
            ForFundingAccount.AccountCode = 0;
            ForFundingAccount.AccountName = "Proposed Positions";
            ForFundingAccount.OOEName = "PS";
            ForFundingAccount.CurrentProposalAmount = 0;
            ForFundingAccount.ProposalYear = 2021;
            ForFundingAccount.ProgramID = Convert.ToInt32(prog_ID);
            ForFundingAccount.OOEID = 1;
            ForFundingAccount.ProposalStatusHR = 2;

            if (ForFundingAccount.OOEID == 1 || ForFundingAccount.OOEID == 2 || ForFundingAccount.OOEID == 3)
            {
                ForFundingAccount.SlashAmount = getforFundingTotal(proy_ID, office_ID);
                ForFundingAccount.ProposalAmmount = getforFundingSubmittedTotal(proy_ID, office_ID);
            }
            else
            {
                ForFundingAccount.ProposalAmmount = 0;
            }
            if (ForFundingAccount.ProposalAmmount == 0)
            {
                ForFundingAccount.checker = 0;
            }
            else
            {
                ForFundingAccount.checker = 1;
            }
            ForFundingAccount.PastProposalAmmount = 0;
            if (getCountUnreviewedPosition(proy_ID, office_ID, "StatusHR") != 0)
            {
                prog.Add(ForFundingAccount);
            }


            AccountsModel ForFundingAccountCasual = new AccountsModel();
            ForFundingAccountCasual.ProposalID = 0;
            ForFundingAccountCasual.AccountID = 0;
            ForFundingAccountCasual.AccountCode = 0;
            ForFundingAccountCasual.AccountName = "Proposed Casual";
            ForFundingAccountCasual.OOEName = "PS";
            ForFundingAccountCasual.CurrentProposalAmount = 0;
            ForFundingAccountCasual.ProposalYear = 2017;
            ForFundingAccountCasual.ProgramID = Convert.ToInt32(prog_ID);
            ForFundingAccountCasual.OOEID = 1;
            ForFundingAccountCasual.ProposalStatusHR = 2;

            if (ForFundingAccountCasual.OOEID == 1 || ForFundingAccountCasual.OOEID == 2 || ForFundingAccountCasual.OOEID == 3)
            {
                ForFundingAccountCasual.SlashAmount = getforFundingTotalCasual(proy_ID, office_ID);
                ForFundingAccountCasual.ProposalAmmount = getforFundingSubmittedCasual(proy_ID, office_ID);
            }
            else
            {
                ForFundingAccountCasual.ProposalAmmount = 0;
            }
            if (ForFundingAccountCasual.ProposalAmmount == 0)
            {
                ForFundingAccountCasual.checker = 0;
            }
            else
            {
                ForFundingAccountCasual.checker = 1;
            }
            ForFundingAccountCasual.PastProposalAmmount = 0;
            if (getCountUnreviewedCasual(proy_ID, office_ID, "StatusHR") != 0)
            {
                prog.Add(ForFundingAccountCasual);
            }

            return prog;
        }
        public double getforFundingTotal(int? ProposalYear, int? OfficeID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_bms_getForFundingData '" + ProposalYear + "','" + OfficeID + "',0,2", con);
                con.Open();
                return Convert.ToDouble(com.ExecuteScalar());
            }
        }
        public double getforFundingSubmittedTotal(int? ProposalYear, int? OfficeID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select isnull(sum(a.Total),0) as 'Submitted Positiion' from tbl_R_BMSSubmittedForFundingData as a
                                                inner JOIN tbl_R_BMSProposedNewItem as b on b.ProposedItemID = a.ProposedItemID
                                                where a.OfficeID = " + OfficeID + " and a.yearof = " + ProposalYear + " and a.Grouptag = 'For Funding' and  b.PositionID is not null and b.DivisionID is not null", con);
                con.Open();
                return Convert.ToDouble(com.ExecuteScalar());
            }
        }
        public double getforFundingSubmittedCasual(int? ProposalYear, int? OfficeID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select isnull(Sum(a.Total),0) as 'Submitted Casual' from tbl_R_BMSSubmittedForFundingData as a
                                                    inner JOIN tbl_R_BMSProposedNewItem as b on b.ProposedItemID = a.ProposedItemID
                                                    where a.OfficeID = " + OfficeID + " and a.yearof = " + ProposalYear + " and b.PositionID is null and b.DivisionID is null", con);
                con.Open();
                return Convert.ToDouble(com.ExecuteScalar());
            }
        }
        public double getforFundingTotalCasual(int? ProposalYear, int? OfficeID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_bms_getForFundingDataCasual '" + ProposalYear + "','" + OfficeID + "',0,2", con);
                con.Open();
                return Convert.ToDouble(com.ExecuteScalar());
            }
        }
        public int getCountUnreviewedCasual(int? ProposalYear, int? OfficeID, string OfficeLevel)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select Count(a.ProposedItemID) FROM tbl_R_BMSSubmittedForFundingData as a
                                                INNER JOIN tbl_R_BMSProposedNewItem as b on b.ProposedItemID = a.ProposedItemID
                                                where a." + OfficeLevel + " = 2 and a.OfficeID = " + OfficeID + " and a.YearOF = " + ProposalYear
                                                + " and b.DivisionID is null and b.PositionID is null", con);
                con.Open();
                return Convert.ToInt32(com.ExecuteScalar());
            }
        }
        public int getCountUnreviewedPosition(int? ProposalYear, int? OfficeID, string OfficeLevel)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select Count(a.ProposedItemID) FROM tbl_R_BMSSubmittedForFundingData as a
                                                INNER JOIN tbl_R_BMSProposedNewItem as b on b.ProposedItemID = a.ProposedItemID
                                                where a." + OfficeLevel + " = 2 and a.OfficeID = " + OfficeID + " and a.YearOF = " + ProposalYear
                                                + " and b.DivisionID is not null and b.PositionID is not null and b.ActionCode = 2", con);
                con.Open();
                return Convert.ToInt32(com.ExecuteScalar());
            }
        }

        public IEnumerable<AccountsModel> BudgetforApprovalAccountsCommittee(int? prog_ID, int? office_ID, int? proy_ID, int? ooe_id, double? percent, int? regularaipid)
        {
            List<AccountsModel> prog = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec sp_BMS_getAccountsForApproval "+ office_ID +","+ prog_ID +","+proy_ID +","+ ooe_id +","+ regularaipid + "", con);
                #region oldQuery
                //                SqlCommand com = new SqlCommand(@"SELECT DISTINCT a.ProposalID, a.AccountID,  e.AccountName, f.OOEAbrevation,a.ProposalYear,a.ProposalAmount FROM dbo.tbl_T_BMSBudgetProposal AS a
                //                                                    left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID 
                //                                                    left JOIN dbo.tbl_R_BMSAccounts AS b ON b.AccountID = e.AccountID 
                //                                                    left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID 
                //                                                    left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID 
                //                                                    LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID 
                //                                                    LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode
                //                                                    WHERE c.OfficeID = '" + office_ID + "' and  a.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proy_ID + "' and e.AccountYear = '" + proy_ID
                //                                                    + "' and a.ProposalActionCode = '1' and e.ActionCode='1' and a.ProposalStatusHR = '1' and a.ProposalStatusInCharge='1' and a.ProposalStatusCommittee = '2'", con); 
                #endregion
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel emp = new AccountsModel();
                    emp.WithCheckBox = 1; ///YES
                    emp.ProposalID = reader.GetInt64(0);
                    emp.AccountID = reader.GetInt32(1);
                    emp.AccountName = reader.GetValue(2).ToString();
                    emp.OOEName = reader.GetString(3);
                    emp.ProposalYear = reader.GetInt32(4);
                    emp.PastProposalAmmount = Convert.ToDouble(reader.GetValue(5));
                    emp.ProposalAmmount = Convert.ToDouble(reader.GetValue(6));
                    emp.ProgramID = Convert.ToInt32(reader.GetValue(7));
                    emp.ProposalStatusInCharge = Convert.ToInt32(reader.GetValue(9));
                    emp.ProposalStatusCommitteeINT = Convert.ToInt32(reader.GetValue(10));
                    emp.ProposalDenominationCode = Convert.ToInt32(reader.GetValue(11));
                    emp.AccountCode = Convert.ToInt32(reader.GetValue(12));
                    //Edit on 7/20/2018 -xXx
                    emp.ProposalAmmount = Convert.ToDouble(reader.GetValue(13));
                    emp.OldOffice = Convert.ToInt32(reader.GetValue(14));
                    emp.NewProgramID = Convert.ToInt32(reader.GetValue(17));
                    emp.NewOffice = Convert.ToInt32(reader.GetValue(18));
                    emp.NewAccountID = Convert.ToInt32(reader.GetValue(19));
                    emp.aipversion = Convert.ToInt32(regularaipid);
                    //temporay hide -in case ug gamiton lang
                    //emp.ProposalAmmount = Convert.ToDouble(getActualProposed(office_ID, proy_ID, emp.AccountID));
                    var SlashAmount = "";

                    if (regularaipid == 1) //Annual budget
                    {
                        SlashAmount = getSlashAmount(reader.GetInt64(0));
                        if (SlashAmount == "No Data")
                        {
                            emp.SlashAmount = Convert.ToDouble(reader.GetValue(13));
                        }
                        else if (SlashAmount.Contains("With_Computation"))
                        {
                            emp.SlashAmount = Convert.ToDouble(SlashAmount.Replace(" With_Computation", ""));
                            if (Convert.ToInt32(reader.GetValue(16)) == 0 && ooe_id == 1)
                            {
                                emp.ProposalStatusHR = 2;
                            }
                        }
                        else
                        {
                            emp.SlashAmount = Convert.ToDouble(SlashAmount);
                            emp.ProposalStatusHR = Convert.ToInt32(reader.GetValue(8));
                        }
                    }
                    else //supplemental
                    {
                        emp.SlashAmount = Convert.ToDouble(reader.GetValue(22));
                        emp.ProposalStatusHR = Convert.ToInt32(reader.GetValue(8));
                        emp.ProposalAmmount = Convert.ToDouble(reader.GetValue(23));
                    }
                    var percentMultiply = emp.PastProposalAmmount * percent; // GET Total Amount with Percentage
                    var percentDif = emp.PastProposalAmmount - percentMultiply; // GET Difference from PastProposal and Amount Percentage as Difference A
                    var DifferenceA = Math.Abs(Convert.ToDouble(percentDif)); // Convert to Absolute Value
                    var dif = emp.ProposalAmmount - emp.PastProposalAmmount; // GET Difference from Past Proposal and Proposed Amount as Difference B
                    var DifferenceB = Math.Abs(dif); // Convert to Absolute Value
                    if (percent != 1)
                    {
                        if (emp.ProposalAmmount > emp.PastProposalAmmount)
                        {
                            if (DifferenceB > DifferenceA) // Compare if Difference B is greater than Difference A then Proposed exceeded to Percentage Increase
                            {
                                emp.AmountStatus = 1;
                            }
                            else
                            {
                                emp.AmountStatus = 0;
                            }

                        }
                        else if (emp.ProposalAmmount < emp.PastProposalAmmount)
                        {
                            emp.AmountStatus = 3;
                        }
                    }
                    if (regularaipid == 1)
                    {
                        emp.Difference = emp.SlashAmount - emp.PastProposalAmmount;
                    }
                    else
                    {
                        emp.Difference = (emp.SlashAmount + emp.ProposalAmmount) - emp.PastProposalAmmount;
                    }
                   //emp.Difference = emp.PastProposalAmmount - emp.SlashAmount ;
                    var programID = emp.ProgramID.ToString();//prog_ID.ToString();
                    emp.PastYear = getPastYearAmount(proy_ID, reader.GetValue(1).ToString(), emp.OldOffice, programID, prog_ID);
                    // emp.PastYear = getPastYearAmount(proy_ID, reader.GetValue(1), office_ID);
                    emp.CheckComp = Convert.ToInt32(reader.GetValue(20));
                    emp.SelectedOfficeID = Convert.ToInt32(reader.GetValue(14));
                    emp.setRemarks = reader.GetString(15);
                    prog.Add(emp);
                }

                if (ooe_id == 1)
                {
                    AccountsModel ForFundingAccount = new AccountsModel();
                    if (Account.UserInfo.UserTypeDesc == "HRMO In-Charge" || Account.UserInfo.UserTypeDesc == "Budget Office")
                    {
                        ForFundingAccount.WithCheckBox = 2; ///NO CheckBox No Button
                        
                    }
                    else
                    {
                        ForFundingAccount.WithCheckBox = 0; ///NO    
                    }
                    
                    ForFundingAccount.ProposalID = 0;
                    ForFundingAccount.AccountID = 0;
                    ForFundingAccount.AccountCode = 0;
                    ForFundingAccount.AccountName = "Proposed Positions";
                    ForFundingAccount.OOEName = "PS";
                    ForFundingAccount.CurrentProposalAmount = 0;
                    ForFundingAccount.ProposalYear = Convert.ToInt32(proy_ID);
                    ForFundingAccount.ProgramID = Convert.ToInt32(prog_ID);
                    ForFundingAccount.OOEID = 1;
                    ForFundingAccount.ProposalStatusHR = 2;

                    if (ForFundingAccount.OOEID == 1 || ForFundingAccount.OOEID == 2 || ForFundingAccount.OOEID == 3)
                    {
                        ForFundingAccount.SlashAmount = getforFundingTotal(proy_ID, office_ID);
                        ForFundingAccount.ProposalAmmount = getforFundingSubmittedTotal(proy_ID, office_ID);
                    }
                    else
                    {
                        ForFundingAccount.ProposalAmmount = 0;
                    }
                    if (ForFundingAccount.ProposalAmmount == 0)
                    {
                        ForFundingAccount.checker = 0;
                    }
                    else
                    {
                        ForFundingAccount.checker = 1;
                    }
                    ForFundingAccount.PastProposalAmmount = 0;
                    ForFundingAccount.Difference = ForFundingAccount.SlashAmount;
                    if (getCountUnreviewedPosition(proy_ID, office_ID, "StatusLFC") != 0)
                    {
                        prog.Add(ForFundingAccount);
                    }

                }
                reader.Close();
                ////Update on 7/20201 - xXx - used of stored proc
                //SqlCommand PNA = new SqlCommand(@"SELECT a.AccountID, a.AccountName, 
                //    a.AccountCode, b.ProposalID, b.ProposalAmount, 
                //    b.ProposalStatusHR, b.ProposalStatusInCharge, b.ProposalStatusCommittee, 
                //    b.ProposalDenominationCode, a.OOEID, ISNULL(c.Remarks, '')  FROM tbl_R_BMSProposedAccounts as a " +
                //    "LEFT JOIN tbl_T_BMSBudgetProposal as b ON a.AccountID = b.AccountID and a.ProgramID = b.ProgramID " +
                //    "LEFT JOIN tbl_R_BMSProposalRemark as c ON c.ProposalID = b.ProposalID " +
                //    "WHERE a.OfficeID = '" + office_ID + "'and a.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proy_ID + "' and a.OOEID = '" + ooe_id + "' and a.ActionCode = 1  and b.ProposalActionCode = 1 and ProposalStatusCommittee = 2 ", con);
                SqlCommand PNA = new SqlCommand(@"exec dbo.[sp_BMS_ProposedAccountSubmitted] '" + office_ID + "','" + prog_ID + "','" + ooe_id + "','" + proy_ID + "',"+ regularaipid + "", con);
                SqlDataReader reader_PNA = PNA.ExecuteReader();
                while (reader_PNA.Read())
                {
                    AccountsModel PNAlist = new AccountsModel();
                    AccountsModel ForFundingAccount = new AccountsModel();
                    if (Account.UserInfo.UserTypeDesc == "HRMO In-Charge" || Account.UserInfo.UserTypeDesc == "Budget Office")
                    {
                        PNAlist.WithCheckBox = 1; ///Yes

                    }
                    else
                    {
                        PNAlist.WithCheckBox = 0; ///NO    
                    }
                    PNAlist.AccountID = Convert.ToInt32(reader_PNA.GetValue(0));
                    PNAlist.AccountName = reader_PNA.GetString(1);
                    PNAlist.AccountCode = Convert.ToInt32(reader_PNA.GetValue(2));
                    PNAlist.ProposalID = Convert.ToInt32(reader_PNA.GetValue(3));
                    PNAlist.ProposalAmmount = Convert.ToDouble(reader_PNA.GetValue(4));
                    PNAlist.ProposalStatusHR = Convert.ToInt32(reader_PNA.GetValue(5));
                    PNAlist.ProposalStatusInCharge = Convert.ToInt32(reader_PNA.GetValue(6));
                    PNAlist.ProposalStatusCommitteeINT = Convert.ToInt32(reader_PNA.GetValue(7));
                    PNAlist.ProposalDenominationCode = Convert.ToInt32(reader_PNA.GetValue(8));
                    PNAlist.OOEID = Convert.ToInt32(reader_PNA.GetValue(9));
                    PNAlist.setRemarks = reader_PNA.GetString(10);
                    PNAlist.ProgramID = Convert.ToInt32(prog_ID);
                    PNAlist.ProposalYear =Convert.ToInt32(proy_ID);
                    PNAlist.SelectedOfficeID = Convert.ToInt32(office_ID);
                    PNAlist.PastProposalAmmount = Convert.ToDouble(reader_PNA.GetValue(11));
                    PNAlist.NewProgramID = Convert.ToInt32(reader_PNA.GetValue(14));
                    PNAlist.NewAccountID = Convert.ToInt32(reader_PNA.GetValue(15));
                    PNAlist.NewOffice = Convert.ToInt32(reader_PNA.GetValue(16));
                    PNAlist.aipversion = Convert.ToInt32(regularaipid);
                    var SlashAmount = "";
                    SlashAmount = getSlashAmount(reader_PNA.GetInt64(3));
                    if (SlashAmount == "No Data")
                    {
                        PNAlist.SlashAmount = Convert.ToDouble(reader_PNA.GetValue(4));
                    }
                    else if ((SlashAmount.Contains(" With_Computation")) && (ooe_id == 1))
                    {
                        PNAlist.SlashAmount = Convert.ToDouble(SlashAmount.Replace(" With_Computation", ""));
                        PNAlist.ProposalStatusHR = 2;
                    }
                    else
                    {
                        PNAlist.SlashAmount = Convert.ToDouble(SlashAmount.Replace(" With_Computation", ""));//Convert.ToDouble(SlashAmount);
                        PNAlist.ProposalStatusHR = Convert.ToInt32(reader_PNA.GetValue(5));
                    }
                    var programID = prog_ID.ToString();
                    PNAlist.PastYear = getPastYearAmount(proy_ID, reader_PNA.GetValue(13).ToString(), Convert.ToInt32(reader_PNA.GetValue(17).ToString()), reader_PNA.GetValue(12).ToString(), prog_ID);
                    PNAlist.Difference = PNAlist.SlashAmount; //- emp.PastProposalAmmount;
                    prog.Add(PNAlist);
                }
                reader_PNA.Close();
                
            }
            return prog;
        }

        public string getActualProposed(int? OfficeID=0, int? propYear=0, int? AccountID=0)
        {
            var actualProposed = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand query_Slashing = new SqlCommand(@"dbo.sp_bms_getComputationTotal 0," + getPmisOfficeID2(OfficeID) + "," + propYear + ",0,0,0," + AccountID + ",5580", con);
                    con.Open();
                    query_Slashing.CommandTimeout = 0;
                    actualProposed = query_Slashing.ExecuteScalar().ToString();
                }
            return actualProposed;

        }

        public int getPmisOfficeID2(int? OfficeID=0)
        {
            var PMISOfficeID = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT PMISOfficeID from tbl_R_BMSOffices where OfficeID='" + OfficeID + "'", con);
                con.Open();
                PMISOfficeID = Convert.ToInt32(com.ExecuteScalar().ToString());
            }
            return PMISOfficeID;
        }

        public string getSlashAmount(Int64? ProposalID)
        {
            var SlashingAmount = "";
            var yearnow = (DateTime.Now.Year);
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query_Slashing = new SqlCommand(@"dbo.sp_bms_getComputationTotalAdminView " + ProposalID + "", con);
                con.Open();
                query_Slashing.CommandTimeout = 0;
                SlashingAmount = query_Slashing.ExecuteScalar().ToString() + " With_Computation";
            }
            if (SlashingAmount == "0.0000 With_Computation" || SlashingAmount == "0.00 With_Computation")
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    //SqlCommand query_Slashing = new SqlCommand(@"SELECT Top 1 a.Amount FROM tbl_R_AmountHistory as a WHERE ProposalID = '" + ProposalID + "' ORDER BY AmountHistoryID DESC", con);
                    SqlCommand query_Slashing = new SqlCommand(@"IF EXISTS (SELECT Top 1 a.Amount FROM tbl_R_AmountHistory as a WHERE ProposalID = '" + ProposalID + "' and year(cast([date] as date)) + 1 >= "+ yearnow + " ORDER BY AmountHistoryID DESC) " + ""
                                                                + " BEGIN " + ""
                                                                + " SELECT Top 1 a.Amount FROM tbl_R_AmountHistory as a WHERE ProposalID = '" + ProposalID + "' and year(cast([date] as date)) + 1 >= " + yearnow + " ORDER BY AmountHistoryID DESC " + ""
                                                                + " END " + ""
                                                                + " ELSE " + ""
                                                                + " BEGIN " + ""
                                                                + " SELECT 'No Data' " + ""
                                                                + " END", con);

                    con.Open();
                    query_Slashing.CommandTimeout = 0;
                    SlashingAmount = query_Slashing.ExecuteScalar().ToString();
                }
            }
            return SlashingAmount;
        }
        //public double getPastYearAmount(int? BudgetYear, string AccountID, int? officeID)
        //{
        //       double PastAmount = 0;
        //       using (SqlConnection con = new SqlConnection(Common.MyConn()))
        //         {
        //             SqlCommand com = new SqlCommand(@"SELECT isnull(sum(amount),0) from fmis.dbo.tblAMIS_SE_byYearOffice where rcenter = '" + officeID + "' and year_ = '" + BudgetYear + "' -2 and AccountCode = '" + AccountID + "'", con);
        //             con.Open();
        //             SqlDataReader reader = com.ExecuteReader();
        //             while (reader.Read())
        //                {
        //                    PastAmount = Convert.ToDouble(reader.GetValue(0));
        //                }
        //             }
        //             return PastAmount;
        // } 
        public double getPastYearAmount(int? BudgetYear, string AccountID, int? officeID, string ProgramID, int? prog_ID)
        {
            double PastAmount = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                //SqlCommand com = new SqlCommand(@"SELECT isnull(sum(amount),0)  from fmis.dbo.tblAMIS_SE_byYearOffice where rcenter = '" + officeID + "'  and year_ = '" + BudgetYear + "' -2 and AccountCode = '" + AccountID + "'", con);
                //Update on 8/7/2019 - xXx
                //SqlCommand com = new SqlCommand(@"SELECT case when (appropriations - Obligation) < 0 then appropriations else Obligation end as Obligation FROM tbl_R_BMSObligatedAccounts WHERE " +
                //        "fmisOfficeID = '" + (prog_ID == 43 ? prog_ID : officeID) + "' And fmisProgramCode = '" + ProgramID + "' and fmisAccountCode = '" + AccountID + "' and YearOf = '" + BudgetYear + "' - 2", con);
                //con.Open();

                SqlCommand com = new SqlCommand(@"exec sp_BMS_getAccountsPriorUtilization "+ BudgetYear + ","+ AccountID  + ","+ officeID + ","+ ProgramID + ","+ prog_ID + "", con);
                con.Open();
                //Update on 8/7/2019 - xXx

                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    PastAmount = Convert.ToDouble(reader.GetValue(0));
                }
            }
            return PastAmount;
        }

        public IEnumerable<AccountsModel> BudgetCancelledAccountsBudgetInCharge(int? prog_ID, int? office_ID, int? proy_ID, int? ooe_id)
        {
            List<AccountsModel> prog = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                prog_ID = prog_ID == null ? 0 : prog_ID;
                proy_ID = proy_ID == null ? 0 : proy_ID;
                ooe_id = ooe_id == null ? 0 : ooe_id;
                office_ID = office_ID == null ? 0 : office_ID;

                SqlCommand com = new SqlCommand(@"SELECT DISTINCT a.proposalID, a.AccountID,  e.AccountName, f.OOEAbrevation,a.ProposalYear,a.ProposalAmount FROM dbo.tbl_T_BMSBudgetProposal AS a
                                                    left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID 
                                                    left JOIN dbo.tbl_R_BMSAccounts AS b ON b.AccountID = e.AccountID 
                                                    left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID 
                                                    left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID 
                                                    LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID 
                                                    LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode
                                                    WHERE c.OfficeID = '" + office_ID + "' and f.OOEID = " + ooe_id + " and  c.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proy_ID + "'and e.AccountYear = '" + proy_ID
                                                    + "' and e.ActionCode = '1' and a.ProposalActionCode = '2' and a.ProposalStatusInCharge='3' ORDER BY AccountName", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel emp = new AccountsModel();
                    emp.ProposalID = reader.GetInt64(0);
                    emp.AccountID = reader.GetInt32(1);
                    emp.AccountName = reader.GetValue(2).ToString();
                    emp.OOEName = reader.GetString(3);
                    emp.ProposalYear = reader.GetInt32(4);
                    emp.ProposalAmmount = Convert.ToDouble(reader.GetValue(5));

                    prog.Add(emp);
                }
            }
            return prog;
        }
        public IEnumerable<AccountsModel> BudgetCancelledAccountsHRMOInCharge(int? prog_ID, int? office_ID, int? proy_ID, int? ooe_id)
        {
            List<AccountsModel> prog = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                prog_ID = prog_ID == null ? 0 : prog_ID;
                proy_ID = proy_ID == null ? 0 : proy_ID;
                ooe_id = ooe_id == null ? 0 : ooe_id;
                office_ID = office_ID == null ? 0 : office_ID;

                SqlCommand com = new SqlCommand(@"SELECT DISTINCT a.proposalID, a.AccountID,  e.AccountName, f.OOEAbrevation,a.ProposalYear,a.ProposalAmount FROM dbo.tbl_T_BMSBudgetProposal AS a
                                                    left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID 
                                                    left JOIN dbo.tbl_R_BMSAccounts AS b ON b.AccountID = e.AccountID 
                                                    left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID 
                                                    left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID 
                                                    LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID 
                                                    LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode
                                                    WHERE c.OfficeID = '" + office_ID + "' and f.OOEID = " + ooe_id + " and  c.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proy_ID + "'and e.AccountYear = '" + proy_ID
                                                    + "' and e.ActionCode = '1' and a.ProposalActionCode = '2' and a.ProposalStatusHR='3' ORDER BY AccountName", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel emp = new AccountsModel();
                    emp.ProposalID = reader.GetInt64(0);
                    emp.AccountID = reader.GetInt32(1);
                    emp.AccountName = reader.GetValue(2).ToString();
                    emp.OOEName = reader.GetString(3);
                    emp.ProposalYear = reader.GetInt32(4);
                    emp.ProposalAmmount = Convert.ToDouble(reader.GetValue(5));

                    prog.Add(emp);
                }
            }
            return prog;
        }
        public IEnumerable<AccountsModel> BudgetCancelledAccountsCommittee(int? prog_ID, int? office_ID, int? proy_ID, int? ooe_id)
        {
            List<AccountsModel> prog = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                prog_ID = prog_ID == null ? 0 : prog_ID;
                proy_ID = proy_ID == null ? 0 : proy_ID;
                ooe_id = ooe_id == null ? 0 : ooe_id;
                office_ID = office_ID == null ? 0 : office_ID;

                SqlCommand com = new SqlCommand(@"SELECT DISTINCT a.proposalID, a.AccountID,  e.AccountName, f.OOEAbrevation,a.ProposalYear,a.ProposalAmount FROM dbo.tbl_T_BMSBudgetProposal AS a
                                                    left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID 
                                                    left JOIN dbo.tbl_R_BMSAccounts AS b ON b.AccountID = e.AccountID 
                                                    left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID 
                                                    left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID 
                                                    LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID 
                                                    LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode
                                                    WHERE c.OfficeID = '" + office_ID + "' and f.OOEID = " + ooe_id + " and  c.ProgramID = '" + prog_ID + "' and a.ProposalYear = '" + proy_ID + "'and e.AccountYear = '" + proy_ID
                                                    + "' and e.ActionCode = '1' and a.ProposalActionCode = '2' and a.ProposalStatusCommittee='3' ORDER BY AccountName", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel emp = new AccountsModel();
                    emp.ProposalID = reader.GetInt64(0);
                    emp.AccountID = reader.GetInt32(1);
                    emp.AccountName = reader.GetValue(2).ToString();
                    emp.OOEName = reader.GetString(3);
                    emp.ProposalYear = reader.GetInt32(4);
                    emp.ProposalAmmount = Convert.ToDouble(reader.GetValue(5));

                    prog.Add(emp);
                }
            }
            return prog;
        }

        public void newUpdateAccount(IEnumerable<AccountsModel> Accounts)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                foreach (AccountsModel Account in Accounts)
                {
                    SqlCommand com = new SqlCommand("", con);
                    com.ExecuteNonQuery();
                }
            }
        }
        public IEnumerable<AccountsModel> BudgetNewAccounts(int? prog_ID, int? office_ID, int? proy_ID)
        {
            List<AccountsModel> prog = new List<AccountsModel>();
            if (Account.UserInfo.UserTypeDesc == "Budget Office")
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com = new SqlCommand(@"SELECT a.TempID, a.AccountName, b.OOEAbrevation, a.ProposalYear, a.ProposalAmount 
                                                  FROM tbl_Temp_BMSNewAccount AS a 
                                                  LEFT JOIN tbl_R_BMSObjectOfExpenditure AS b
                                                  ON b.OOEID = a.OOEID WHERE a.ProposalActionCode ='1' AND c.ActionCode ='1' AND
                                                  a.ProposalStatusHR = '1' AND a.ProposalStatusInCharge = '2' and a.ProposalStatusCommittee = '2' AND
                                                  a.ProgramID = '" + prog_ID + "' AND c.OfficeID = '" + office_ID
                                                      + "' AND a.ProposalYear ='" + proy_ID + "' AND c.ProgramYear='" + proy_ID + "' ORDER BY AccountName", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        AccountsModel emp = new AccountsModel();
                        emp.ProposalID = reader.GetInt64(0);
                        emp.AccountName = reader.GetValue(1).ToString();
                        emp.OOEName = reader.GetString(2);
                        emp.ProposalYear = reader.GetInt32(3);
                        emp.ProposalAmmount = Convert.ToDouble(reader.GetValue(4));

                        prog.Add(emp);
                    }
                }
            }
            else if (Account.UserInfo.UserTypeDesc == "HRMO In-Charge")
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com = new SqlCommand(@"SELECT a.TempID, a.AccountName, b.OOEAbrevation, a.ProposalYear, a.ProposalAmount 
                                                  FROM tbl_Temp_BMSNewAccount AS a 
                                                  LEFT JOIN tbl_R_BMSObjectOfExpenditure AS b
                                                  ON b.OOEID = a.OOEID WHERE a.ProposalActionCode ='1' AND c.ActionCode ='1' AND                                              
                                                  a.ProposalStatusHR = '2' AND a.ProposalStatusInCharge = '2' and a.ProposalStatusCommittee = '2' AND
                                                  a.ProgramID = '" + prog_ID + "' AND c.OfficeID = '" + office_ID
                                                      + "' AND a.ProposalYear ='" + proy_ID + "' AND c.ProgramYear='" + proy_ID + "' ORDER BY AccountName", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        AccountsModel emp = new AccountsModel();
                        emp.ProposalID = reader.GetInt64(0);
                        emp.AccountName = reader.GetValue(1).ToString();
                        emp.OOEName = reader.GetString(2);
                        emp.ProposalYear = reader.GetInt32(3);
                        emp.ProposalAmmount = Convert.ToDouble(reader.GetValue(4));

                        prog.Add(emp);
                    }
                }
            }
            else // for budget Committee & Super Admin
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com = new SqlCommand(@"SELECT a.TempID, a.AccountName, b.OOEAbrevation, a.ProposalYear, a.ProposalAmount 
                                                  FROM tbl_Temp_BMSNewAccount AS a 
                                                  LEFT JOIN tbl_R_BMSObjectOfExpenditure AS b
                                                  ON b.OOEID = a.OOEID 
                                                  LEFT JOIN tbl_R_BMSOfficePrograms AS c
                                                  ON c.ProgramID = a.ProgramID 
                                                  WHERE a.ProposalActionCode ='1' AND c.ActionCode ='1' AND
                                                  a.ProposalStatusHR = '1' AND a.ProposalStatusInCharge = '1' and a.ProposalStatusCommittee = '2' AND
                                                  a.ProgramID = '" + prog_ID + "' AND c.OfficeID = '" + office_ID
                                                      + "' AND a.ProposalYear ='" + proy_ID + "' AND c.ProgramYear='" + proy_ID + "' ORDER BY AccountName", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        AccountsModel emp = new AccountsModel();
                        emp.ProposalID = reader.GetInt64(0);
                        emp.AccountName = reader.GetValue(1).ToString();
                        emp.OOEName = reader.GetString(2);
                        emp.ProposalYear = reader.GetInt32(3);
                        emp.ProposalAmmount = Convert.ToDouble(reader.GetValue(4));

                        prog.Add(emp);
                    }
                }
            }
            return prog;
        }

        public IEnumerable<CopyProposedAmountModel> ProposedAmountList()
        {
            List<CopyProposedAmountModel> prog = new List<CopyProposedAmountModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"select a.ProposalID,d.OfficeID,c.ObjectOfExpendetureID,c.AccountName,a.ProposalAmount,E.OOEAbrevation, d.OfficeName from tbl_T_BMSBudgetProposal as a 
                                                    INNER JOIN tbl_R_BMSOfficePrograms as b on b.ProgramID = a.ProgramID and b.ProgramYear = a.ProposalYear and b.ActionCode = ProposalActionCode
                                                    INNER JOIN tbl_R_BMSProgramAccounts as c on c.ProgramID = a.ProgramID and c.AccountID = a.AccountID and C.AccountYear = a.ProposalYear and C.ActionCode = a.ProposalActionCode
                                                    INNER JOIN tbl_R_BMSObjectOfExpenditure as e on e.OOEID = c.ObjectOfExpendetureID
                                                    INNER JOIN tbl_R_BMSOffices as d on d.OfficeID = b.OfficeID
                                                    where a.ProposalActionCode = 1 and a.ProposalYear = year(getdate()) + 1 ORDER BY OfficeID, OOEID", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    CopyProposedAmountModel emp = new CopyProposedAmountModel();
                    emp.ProposalID = Convert.ToInt32(reader.GetValue(0));
                    emp.OfficeID = reader.GetInt32(1);
                    emp.OOEID = reader.GetInt32(2);
                    emp.AccountName = reader.GetValue(3).ToString();
                    emp.ProposedAmount = Convert.ToDouble(reader.GetValue(4).ToString());
                    emp.ApprovedAmount = getApprovedAmount(Convert.ToInt32(reader.GetValue(0)));
                    emp.OOEName = reader.GetValue(5).ToString();
                    emp.OfficeName = reader.GetValue(6).ToString();
                    prog.Add(emp);
                }
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com2 = new SqlCommand(@"select a.ProposalID,d.OfficeID,c.OOEID,c.AccountName,a.ProposalAmount,E.OOEAbrevation, d.OfficeName 
                                                        from tbl_T_BMSBudgetProposal as a 
                                                        INNER JOIN tbl_R_BMSOfficePrograms as b on b.ProgramID = a.ProgramID and b.ProgramYear = a.ProposalYear and b.ActionCode = ProposalActionCode
                                                        INNER JOIN tbl_R_BMSProposedAccounts as c on c.ProgramID = a.ProgramID and c.AccountID = a.AccountID and C.ProposalYear = a.ProposalYear and C.ActionCode = a.ProposalActionCode
                                                        INNER JOIN tbl_R_BMSObjectOfExpenditure as e on e.OOEID = c.OOEID
                                                        INNER JOIN tbl_R_BMSOffices as d on d.OfficeID = b.OfficeID
                                                        where a.ProposalActionCode = 1 and a.ProposalYear = year(getdate()) + 1 ORDER BY d.OfficeID, e.OOEID", con);
                con.Open();
                SqlDataReader reader2 = com2.ExecuteReader();
                while (reader2.Read())
                {
                    CopyProposedAmountModel emp = new CopyProposedAmountModel();
                    emp.ProposalID = Convert.ToInt32(reader2.GetValue(0));
                    emp.OfficeID = reader2.GetInt32(1);
                    emp.OOEID = reader2.GetInt32(2);
                    emp.AccountName = reader2.GetValue(3).ToString();
                    emp.ProposedAmount = Convert.ToDouble(reader2.GetValue(4).ToString());
                    emp.ApprovedAmount = getApprovedAmount(Convert.ToInt32(reader2.GetValue(0)));
                    emp.OOEName = reader2.GetValue(5).ToString();
                    emp.OfficeName = reader2.GetValue(6).ToString();

                    prog.Add(emp);
                }
            }
            return prog;
        }
        public IEnumerable<AccountToUpdateModel> GetListOfAccountsForEdit(int OfficeID)
        {
            List<AccountToUpdateModel> AccountList = new List<AccountToUpdateModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"select b.ProgramAccountID,b.AccountName,b.ObjectOfExpendetureID,b.ProgramID,0 as isNewAccountProposed,isnull(b.OrderNo,999) as OrderNo,ISNULL(d.ProjectID,0) as ProjectID,d.ProjectDesc,b.AccountID,isnull(c.OrderNo,999) as ProgramOrder from tbl_T_BMSBudgetProposal as a
                                                left join tbl_R_BMSProgramAccounts as b on b.AccountID = a.AccountID and b.AccountYear = a.ProposalYear and b.ActionCode = a.ProposalActionCode
												Left Join tbl_R_BMSOfficePrograms as c on c.ProgramID = b.ProgramID and c.ActionCode = a.ProposalActionCode and c.ProgramYear = a.ProposalYear
												Left Join tbl_R_BMSProjectDescription as d on d.AccountID = b.AccountID and d.ActionCode = a.ProposalActionCode and d.OfficeID = @OfficeID
                                                where a.AccountID not in(select AccountID from tbl_R_BMSProposedAccounts where ProposalYear = @YearOf and ActionCode = 1 and OfficeID = @OfficeID) and Proposalyear = @YearOf and ProposalActionCode = 1 and c.OfficeID = @OfficeID
                                                union
                                                select b.ProposedID,b.AccountName,b.OOEID,b.ProgramID,1 as isNewAccountProposed,9999 as OrderNo,ISNULL(d.ProjectID,0) as ProjectID,d.ProjectDesc,b.AccountID,isnull(c.OrderNo,999) as ProgramOrder from tbl_T_BMSBudgetProposal as a
                                                left join tbl_R_BMSProposedAccounts as b on b.AccountID = a.AccountID and b.ProposalYear = a.ProposalYear and b.ActionCode = a.ProposalActionCode
												Left Join tbl_R_BMSOfficePrograms as c on c.ProgramID = b.ProgramID and c.ActionCode = a.ProposalActionCode and c.ProgramYear = a.ProposalYear
												Left Join tbl_R_BMSProjectDescription as d on d.AccountID = b.AccountID and d.ActionCode = a.ProposalActionCode and d.OfficeID = @OfficeID
                                                where a.AccountID in(select AccountID from tbl_R_BMSProposedAccounts where ProposalYear = @YearOf and ActionCode = 1 and OfficeID = @OfficeID) and a.Proposalyear = @YearOf and ProposalActionCode = 1  and c.OfficeID = @OfficeID
												ORDER BY b.AccountName", con);
                
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@YearOf";
                param.Value = (DateTime.Now.Year + 1);
                SqlParameter param2 = new SqlParameter();
                param2.ParameterName = "@OfficeID";
                param2.Value = OfficeID;
                com.Parameters.Add(param);
                com.Parameters.Add(param2);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountToUpdateModel Item = new AccountToUpdateModel();
                    Item.PrimaryKey = Convert.ToInt64(reader.GetValue(0));
                    Item.AccountName = reader.GetValue(1).ToString();
                    Item.OOEID = reader.GetInt32(2);
                    Item.ProgramID = reader.GetInt32(3);
                    Item.isNewProposed = reader.GetInt32(4);
                    Item.OfficeID = OfficeID;
                    Item.ProjectID = Convert.ToInt32(reader.GetValue(6));
                    Item.ProjectTittle = reader.GetValue(7).ToString();
                    Item.AccountID = reader.GetValue(8).ToString();
                    AccountList.Add(Item);
                }
            }
            return AccountList;
        }
        public IEnumerable<AccountToUpdateModel> GetAccountsForUpdate_Original(IEnumerable<AccountToUpdateModel> Accounts,int OfficeID)
        {
            List<AccountToUpdateModel> AccountList = new List<AccountToUpdateModel>();
            List<long> OldAccounts = (from x in Accounts where x.isNewProposed == 0 select x.PrimaryKey).ToList();
            List<long> newAccounts = (from x in Accounts where x.isNewProposed == 1 select x.PrimaryKey).ToList();
            var ConvertedIDsOldAccounts = "0";
            var ConvertedIDsNewAccounts = "0";
            foreach (var item in OldAccounts)
            {
                ConvertedIDsOldAccounts = ConvertedIDsOldAccounts + "," + item;
            }
            foreach (var item in newAccounts)
            {
                ConvertedIDsNewAccounts = ConvertedIDsNewAccounts + "," + item;
            }
            var FistQuery = @"select b.ProgramAccountID,b.AccountName,b.ObjectOfExpendetureID,b.ProgramID,0 as isNewAccountProposed,isnull(b.OrderNo,999) as OrderNo,ISNULL(d.ProjectID,0) as ProjectID,d.ProjectDesc,isnull(c.OrderNo,999) as ProgramOrder from tbl_T_BMSBudgetProposal as a
                                                left join tbl_R_BMSProgramAccounts as b on b.AccountID = a.AccountID and b.AccountYear = a.ProposalYear and b.ActionCode = a.ProposalActionCode
                                                Left Join tbl_R_BMSOfficePrograms as c on c.ProgramID = b.ProgramID and c.ActionCode = a.ProposalActionCode and c.ProgramYear = a.ProposalYear
                                                Left Join tbl_R_BMSProjectDescription as d on d.AccountID = b.AccountID and d.ActionCode = a.ProposalActionCode and d.OfficeID = c.OfficeID
                                                where b.ProgramAccountID in(" + ConvertedIDsOldAccounts + ")";
            var SecondQuery = @"select b.ProposedID,b.AccountName,b.OOEID,b.ProgramID,1 as isNewAccountProposed,9999 as OrderNo,ISNULL(d.ProjectID,0) as ProjectID,d.ProjectDesc,isnull(c.OrderNo,999) as ProgramOrder from tbl_T_BMSBudgetProposal as a
                                                left join tbl_R_BMSProposedAccounts as b on b.AccountID = a.AccountID and b.ProposalYear = a.ProposalYear and b.ActionCode = a.ProposalActionCode
                                                Left Join tbl_R_BMSOfficePrograms as c on c.ProgramID = b.ProgramID and c.ActionCode = a.ProposalActionCode and c.ProgramYear = a.ProposalYear
                                                Left Join tbl_R_BMSProjectDescription as d on d.AccountID = b.AccountID and d.ActionCode = a.ProposalActionCode and d.OfficeID = c.OfficeID
                                                where b.ProposedID in("+ConvertedIDsNewAccounts+") ORDER BY ProgramOrder,ObjectOfExpendetureID,OrderNo";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@""+ FistQuery +" union "+ SecondQuery +"", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountToUpdateModel Item = new AccountToUpdateModel();
                    Item.PrimaryKey = Convert.ToInt64(reader.GetValue(0));
                    Item.AccountName = reader.GetValue(1).ToString();
                    Item.OOEID = reader.GetInt32(2);
                    Item.ProgramID = reader.GetInt32(3);
                    Item.isNewProposed = reader.GetInt32(4);
                    Item.OfficeID = OfficeID;
                    Item.ProjectID = Convert.ToInt32(reader.GetValue(6));
                    Item.ProjectTittle = reader.GetValue(7).ToString();
                    AccountList.Add(Item);
                }
            }
            return AccountList;
        }
        public IEnumerable<ObjectOfExpendetureModel> PopulateObjectofExpenditure() 
        {
            List<ObjectOfExpendetureModel> OOEList = new List<ObjectOfExpendetureModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"select OOEID,OOEName from tbl_R_BMSObjectOfExpenditure", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ObjectOfExpendetureModel OOE = new ObjectOfExpendetureModel();
                    OOE.OOEID = Convert.ToInt32(reader.GetValue(0));
                    OOE.OOEName = reader.GetValue(1).ToString();
                    OOEList.Add(OOE);
                }
            }
            return OOEList;
        }
        public IEnumerable<ProgramsModel> PopulatePrograms(int OfficeID) 
        {
            List<ProgramsModel> ProgramList = new List<ProgramsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var whereStatement = OfficeID == 0 ? "where ProgramYear = " + (DateTime.Now.Year + 1) + " and ActionCode = 1" : "where ProgramYear = " + (DateTime.Now.Year + 1) + " and OfficeID = " + OfficeID + " and ActionCode = 1";

                SqlCommand com = new SqlCommand(@"select ProgramID,OfficeID,ProgramDescription from tbl_R_BMSOfficePrograms "+ whereStatement +"  order by isnull(OrderNo,999)", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ProgramsModel Program = new ProgramsModel();
                    Program.ProgramID = reader.GetValue(0).ToString();
                    Program.OfficeID = Convert.ToInt32(reader.GetValue(1));
                    Program.ProgramDescription = reader.GetValue(2).ToString();
                    ProgramList.Add(Program);
                }
            }
            return ProgramList;
        }
        public IEnumerable<AccountsModel> PopulateAccounts()
        {
            List<AccountsModel> AccountList = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select a.AccountID,b.AccountName from tbl_R_BMSProgramAccounts  as a
                                                inner JOIN tbl_R_BMSAccounts as b on b.FMISAccountCode = a.AccountID and a.ActionCode = b.Active
                                                 where ActionCode = 1 Group By a.AccountID,b.AccountName
                                                union
                                                select a.AccountID,isnull(b.AccountName,a.AccountName) as AccountName from tbl_R_BMSProposedAccounts  as a
                                                inner JOIN tbl_R_BMSAccounts as b on b.FMISAccountCode = a.AccountID
                                                where ActionCode = 1 and ProposalYear = 2018 Group BY a.AccountID,isnull(b.AccountName,a.AccountName)
                                                Order By AccountName", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel Account = new AccountsModel();
                    Account.AccountID = Convert.ToInt32(reader.GetValue(0));
                    Account.AccountName = reader.GetValue(1).ToString();
                    AccountList.Add(Account);
                }
            }
            return AccountList;
        }
        public string UpdateAccounts(IEnumerable<AccountToUpdateModel> Accounts)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PrimaryKey");
            dt.Columns.Add("AccountName");
            dt.Columns.Add("ProjectID");
            dt.Columns.Add("ProjectTittle");
            dt.Columns.Add("OOEID");
            dt.Columns.Add("ProgramID");
            dt.Columns.Add("isNewProposed");
            dt.Columns.Add("OfficeID");
            dt.Columns.Add("AccountID");
            foreach (var item in Accounts)
            {
                DataRow dr = dt.NewRow();
                dr[0] = item.PrimaryKey;
                dr[1] = item.AccountName;
                dr[2] = item.ProjectID;
                dr[3] = item.ProjectTittle;
                dr[4] = item.OOEID;
                dr[5] = item.ProgramID;
                dr[6] = item.isNewProposed;
                dr[7] = item.OfficeID;
                dr[8] = item.AccountID;
                dt.Rows.Add(dr);
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("dbo.sp_BMSUpdateAccount", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add(new SqlParameter("@AccountsForUpdateModel", dt));
                com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                com.Parameters.Add(new SqlParameter("@YearOf", DateTime.Now.AddYears(1).Year));
                con.Open();
                //  DataTable dt_record_holder = new DataTable();
                // dt_record_holder.Load(com.ExecuteReader());
                return com.ExecuteScalar().ToString();
            }
 
        }
        public double getApprovedAmount(int ProposalID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"dbo.sp_bms_getComputationTotalAdminView " + ProposalID + "", con);
                con.Open();
                return Convert.ToDouble(com.ExecuteScalar());
            }
        }
        public string UpdateAllProposedAmount()
        {
            try
            {
                List<CopyProposedAmountModel> prog = new List<CopyProposedAmountModel>();
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com = new SqlCommand(@"select a.ProposalID,d.OfficeID,c.ObjectOfExpendetureID,c.AccountName,a.ProposalAmount,E.OOEAbrevation, d.OfficeName from tbl_T_BMSBudgetProposal as a 
                                                    INNER JOIN tbl_R_BMSOfficePrograms as b on b.ProgramID = a.ProgramID and b.ProgramYear = a.ProposalYear and b.ActionCode = ProposalActionCode
                                                    INNER JOIN tbl_R_BMSProgramAccounts as c on c.ProgramID = a.ProgramID and c.AccountID = a.AccountID and C.AccountYear = a.ProposalYear and C.ActionCode = a.ProposalActionCode
                                                    INNER JOIN tbl_R_BMSObjectOfExpenditure as e on e.OOEID = c.ObjectOfExpendetureID
                                                    INNER JOIN tbl_R_BMSOffices as d on d.OfficeID = b.OfficeID
                                                    where a.ProposalActionCode = 1 and a.ProposalYear = year(getdate()) + 1 ORDER BY OfficeID, OOEID", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        CopyProposedAmountModel emp = new CopyProposedAmountModel();
                        emp.ProposalID = Convert.ToInt32(reader.GetValue(0));
                        emp.OfficeID = reader.GetInt32(1);
                        emp.OOEID = reader.GetInt32(2);
                        emp.AccountName = reader.GetValue(3).ToString();
                        emp.ProposedAmount = Convert.ToDouble(reader.GetValue(4).ToString());
                        emp.ApprovedAmount = getApprovedAmount(Convert.ToInt32(reader.GetValue(0)));
                        emp.OOEName = reader.GetValue(5).ToString();
                        emp.OfficeName = reader.GetValue(6).ToString();

                        prog.Add(emp);
                    }
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com2 = new SqlCommand(@"select a.ProposalID,d.OfficeID,c.OOEID,c.AccountName,a.ProposalAmount,E.OOEAbrevation, d.OfficeName 
                                                        from tbl_T_BMSBudgetProposal as a 
                                                        INNER JOIN tbl_R_BMSOfficePrograms as b on b.ProgramID = a.ProgramID and b.ProgramYear = a.ProposalYear and b.ActionCode = ProposalActionCode
                                                        INNER JOIN tbl_R_BMSProposedAccounts as c on c.ProgramID = a.ProgramID and c.AccountID = a.AccountID and C.ProposalYear = a.ProposalYear and C.ActionCode = a.ProposalActionCode
                                                        INNER JOIN tbl_R_BMSObjectOfExpenditure as e on e.OOEID = c.OOEID
                                                        INNER JOIN tbl_R_BMSOffices as d on d.OfficeID = b.OfficeID
                                                        where a.ProposalActionCode = 1 and a.ProposalYear = year(getdate()) + 1 ORDER BY d.OfficeID, e.OOEID", con);
                    con.Open();
                    SqlDataReader reader2 = com2.ExecuteReader();
                    while (reader2.Read())
                    {
                        CopyProposedAmountModel emp = new CopyProposedAmountModel();
                        emp.ProposalID = Convert.ToInt32(reader2.GetValue(0));
                        emp.OfficeID = reader2.GetInt32(1);
                        emp.OOEID = reader2.GetInt32(2);
                        emp.AccountName = reader2.GetValue(3).ToString();
                        emp.ProposedAmount = Convert.ToDouble(reader2.GetValue(4).ToString());
                        emp.ApprovedAmount = getApprovedAmount(Convert.ToInt32(reader2.GetValue(0)));
                        emp.OOEName = reader2.GetValue(5).ToString();
                        emp.OfficeName = reader2.GetValue(6).ToString();

                        prog.Add(emp);
                    }
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    con.Open();
                    foreach (var item in prog)
                    {
                        if (item.ApprovedAmount != 0)
                        {
                            SqlCommand com2 = new SqlCommand(@"Update tbl_T_BMSBudgetProposal set ProposalAmount = '" + item.ApprovedAmount + "' where ProposalID = '" + item.ProposalID + "' ", con);
                            com2.ExecuteNonQuery();
                        }
                    }
                }

                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public IEnumerable<ProposedPositionsSummaryModel> getProposedPositionSummary(int? YearOf)
        {
            List<ProposedPositionsSummaryModel> PlantillaSummaryList = new List<ProposedPositionsSummaryModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_GetForFundingSummary '"+ (YearOf - 1) +"', 2,0", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ProposedPositionsSummaryModel ProposedPositionsSummaryModel = new ProposedPositionsSummaryModel();
                    ProposedPositionsSummaryModel.SeriesID = Convert.ToInt64(reader.GetValue(0));
                    ProposedPositionsSummaryModel.OfficeName = reader.GetValue(1).ToString();
                    ProposedPositionsSummaryModel.Position = reader.GetValue(2).ToString();
                    ProposedPositionsSummaryModel.SalaryGrade = Convert.ToInt32(reader.GetValue(3));
                    ProposedPositionsSummaryModel.YearlySalary = Convert.ToDouble(reader.GetValue(4));
                    ProposedPositionsSummaryModel.YearEndBonus = Convert.ToDouble(reader.GetValue(5));
                    ProposedPositionsSummaryModel.MidYearBonus = Convert.ToDouble(reader.GetValue(6));
                    ProposedPositionsSummaryModel.Philhealth = Convert.ToDouble(reader.GetValue(7));
                    ProposedPositionsSummaryModel.ecc = Convert.ToDouble(reader.GetValue(8));
                    ProposedPositionsSummaryModel.GSIS = Convert.ToDouble(reader.GetValue(9));
                    ProposedPositionsSummaryModel.PERA = Convert.ToDouble(reader.GetValue(10));
                    ProposedPositionsSummaryModel.PagIbig = Convert.ToDouble(reader.GetValue(11));
                    ProposedPositionsSummaryModel.Clothing = Convert.ToDouble(reader.GetValue(12));
                    ProposedPositionsSummaryModel.CashGift = Convert.ToDouble(reader.GetValue(13));
                    ProposedPositionsSummaryModel.Subsistence = Convert.ToDouble(reader.GetValue(14));
                    ProposedPositionsSummaryModel.Hazard = Convert.ToDouble(reader.GetValue(15));
                    ProposedPositionsSummaryModel.PBB = Convert.ToDouble(reader.GetValue(16));
                    ProposedPositionsSummaryModel.PEI = Convert.ToDouble(reader.GetValue(17));
                    ProposedPositionsSummaryModel.FundType = reader.GetValue(18).ToString();
                    ProposedPositionsSummaryModel.StartDate = reader.GetDateTime(19).ToShortDateString();
                    ProposedPositionsSummaryModel.RepresentationAllowance = Convert.ToDouble(reader.GetValue(20));
                    ProposedPositionsSummaryModel.TransportationAllowance = Convert.ToDouble(reader.GetValue(21));
                    ProposedPositionsSummaryModel.Total = Convert.ToDouble(reader.GetValue(22));

                    PlantillaSummaryList.Add(ProposedPositionsSummaryModel);
                }
            }
            return PlantillaSummaryList;
        }
        public IEnumerable<PlantillaSummaryModel> getPlantillaSummary(int? YearOf)
        {
            List<PlantillaSummaryModel> PlantillaSummaryList = new List<PlantillaSummaryModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Select OfficeID, OfficeName + ' (' + REPLACE(OfficeAbbrivation, ' ', '') + ')', case when OfficeID in(37,38,41) then 'Economic Enterprise' else 'General Fund' end from tbl_R_BMSOffices where PMISOfficeID != 0 ORDER BY OfficeName + ' (' + REPLACE(OfficeAbbrivation, ' ', '') + ')' ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    var VacantCount = 0;
                    var FilledCount = 0;
                    var VacantAnnual = 0.0;
                    var FilledAnnual = 0.0;
                    var PeraVacant = 0.0;
                    var PeraFilled = 0.0;
                    var ClothingVacant = 0.0;
                    var ClothingFilled = 0.0;
                    var CashGiftVacant = 0.0;
                    var CashGiftFilled = 0.0;
                    var GSISVacant = 0.0;
                    var GSISFilled = 0.0;
                    var PagIbigVacant = 0.0;
                    var PagIbigFilled = 0.0;
                    var PhilhealthVacant = 0.0;
                    var PhilhealthFilled = 0.0;
                    var ECCVacant = 0.0;
                    var ECCFilled = 0.0;
                    var YearEndVacant = 0.0;
                    var YearEndFilled = 0.0;
                    var MidYearVacant = 0.0;
                    var MidYearFilled = 0.0;
                    var Casual = 0;
                    var CasualAnnual = 0.0;
                    var CasualPERA = 0.0;
                    var CasualCashGift = 0.0;
                    var CasualGSIS = 0.0;
                    var CasualPagIbig = 0.0;
                    var CasualPhilhealth = 0.0;
                    var CasualECC = 0.0;
                    var CasualYearEnd = 0.0;


                    GetPlantillaCount(YearOf, reader.GetInt32(0), ref VacantCount, ref FilledCount, ref VacantAnnual, ref FilledAnnual,
                        ref PeraVacant, ref PeraFilled, ref ClothingVacant, ref ClothingFilled, ref CashGiftVacant, ref CashGiftFilled,
                        ref GSISVacant, ref GSISFilled, ref PagIbigVacant, ref PagIbigFilled, ref PhilhealthVacant, ref PhilhealthFilled,
                        ref ECCVacant, ref ECCFilled, ref YearEndVacant, ref YearEndFilled, ref MidYearVacant, ref MidYearFilled,
                        ref Casual, ref CasualAnnual, ref CasualPERA, ref CasualCashGift, ref CasualGSIS, ref CasualPagIbig,
                        ref CasualPhilhealth, ref CasualECC, ref CasualYearEnd);

                    PlantillaSummaryModel PlantillaAnnual = new PlantillaSummaryModel();
                    PlantillaAnnual.OfficeID = reader.GetInt32(0);
                    PlantillaAnnual.OfficeName = reader.GetValue(1).ToString() + " - Filled (" + FilledCount + ") Vacant (" + VacantCount + ") Casual (" + Casual + ")";
                    PlantillaAnnual.PSComponent = "Annual Salary";
                    PlantillaAnnual.VacantAmount = VacantAnnual;
                    PlantillaAnnual.FilledAmount = FilledAnnual;
                    PlantillaAnnual.CasualAmount = CasualAnnual;
                    PlantillaAnnual.Casual = Casual;
                    PlantillaAnnual.Filled = FilledCount;
                    PlantillaAnnual.Vacant = VacantCount;
                    PlantillaAnnual.FundType = reader.GetValue(2).ToString();

                    PlantillaSummaryModel PlantillaPera = new PlantillaSummaryModel();
                    PlantillaPera.OfficeID = reader.GetInt32(0);
                    PlantillaPera.OfficeName = reader.GetValue(1).ToString() + " - Filled (" + FilledCount + ") Vacant (" + VacantCount + ") Casual (" + Casual + ")";
                    PlantillaPera.PSComponent = "PERA";
                    PlantillaPera.VacantAmount = PeraVacant;
                    PlantillaPera.FilledAmount = PeraFilled;
                    PlantillaPera.CasualAmount = CasualPERA;
                    PlantillaPera.FundType = reader.GetValue(2).ToString();

                    PlantillaSummaryModel PlantillaClothing = new PlantillaSummaryModel();
                    PlantillaClothing.OfficeID = reader.GetInt32(0);
                    PlantillaClothing.OfficeName = reader.GetValue(1).ToString() + " - Filled (" + FilledCount + ") Vacant (" + VacantCount + ") Casual (" + Casual + ")";
                    PlantillaClothing.PSComponent = "Clothing Allowance";
                    PlantillaClothing.VacantAmount = ClothingVacant;
                    PlantillaClothing.FilledAmount = ClothingFilled;                    
                    PlantillaClothing.FundType = reader.GetValue(2).ToString();

                    PlantillaSummaryModel PlantillaCashGift = new PlantillaSummaryModel();
                    PlantillaCashGift.OfficeID = reader.GetInt32(0);
                    PlantillaCashGift.OfficeName = reader.GetValue(1).ToString() + " - Filled (" + FilledCount + ") Vacant (" + VacantCount + ") Casual (" + Casual + ")";
                    PlantillaCashGift.PSComponent = "Cash Gift";
                    PlantillaCashGift.VacantAmount = CashGiftVacant;
                    PlantillaCashGift.FilledAmount = CashGiftFilled;
                    PlantillaCashGift.CasualAmount = CasualCashGift;
                    PlantillaCashGift.FundType = reader.GetValue(2).ToString();

                    PlantillaSummaryModel PlantillaGSIS = new PlantillaSummaryModel();
                    PlantillaGSIS.OfficeID = reader.GetInt32(0);
                    PlantillaGSIS.OfficeName = reader.GetValue(1).ToString() + " - Filled (" + FilledCount + ") Vacant (" + VacantCount + ") Casual (" + Casual + ")";
                    PlantillaGSIS.PSComponent = "Life And Retirement Contributions";
                    PlantillaGSIS.VacantAmount = GSISVacant;
                    PlantillaGSIS.FilledAmount = GSISFilled;
                    PlantillaGSIS.CasualAmount = CasualGSIS;
                    PlantillaGSIS.FundType = reader.GetValue(2).ToString();

                    PlantillaSummaryModel PlantillaPagIbig = new PlantillaSummaryModel();
                    PlantillaPagIbig.OfficeID = reader.GetInt32(0);
                    PlantillaPagIbig.OfficeName = reader.GetValue(1).ToString() + " - Filled (" + FilledCount + ") Vacant (" + VacantCount + ") Casual (" + Casual + ")";
                    PlantillaPagIbig.PSComponent = "Pag-Ibig Contributions";
                    PlantillaPagIbig.VacantAmount = PagIbigVacant;
                    PlantillaPagIbig.FilledAmount = PagIbigFilled;
                    PlantillaPagIbig.CasualAmount = CasualPagIbig;
                    PlantillaPagIbig.FundType = reader.GetValue(2).ToString();

                    PlantillaSummaryModel PlantillaPhilHealth = new PlantillaSummaryModel();
                    PlantillaPhilHealth.OfficeID = reader.GetInt32(0);
                    PlantillaPhilHealth.OfficeName = reader.GetValue(1).ToString() + " - Filled (" + FilledCount + ") Vacant (" + VacantCount + ") Casual (" + Casual + ")";
                    PlantillaPhilHealth.PSComponent = "Philhealth Contributions";
                    PlantillaPhilHealth.VacantAmount = PhilhealthVacant;
                    PlantillaPhilHealth.FilledAmount = PhilhealthFilled;
                    PlantillaPhilHealth.CasualAmount = CasualPhilhealth;
                    PlantillaPhilHealth.FundType = reader.GetValue(2).ToString();

                    PlantillaSummaryModel PlantillaECC = new PlantillaSummaryModel();
                    PlantillaECC.OfficeID = reader.GetInt32(0);
                    PlantillaECC.OfficeName = reader.GetValue(1).ToString() + " - Filled (" + FilledCount + ") Vacant (" + VacantCount + ") Casual (" + Casual + ")";
                    PlantillaECC.PSComponent = "ECC Contributions";
                    PlantillaECC.VacantAmount = ECCVacant;
                    PlantillaECC.FilledAmount = ECCFilled;
                    PlantillaECC.CasualAmount = CasualECC;
                    PlantillaECC.FundType = reader.GetValue(2).ToString();

                    PlantillaSummaryModel PlantillaYearEnd = new PlantillaSummaryModel();
                    PlantillaYearEnd.OfficeID = reader.GetInt32(0);
                    PlantillaYearEnd.OfficeName = reader.GetValue(1).ToString() + " - Filled (" + FilledCount + ") Vacant (" + VacantCount + ") Casual (" + Casual + ")";
                    PlantillaYearEnd.PSComponent = "Year End";
                    PlantillaYearEnd.VacantAmount = YearEndVacant;
                    PlantillaYearEnd.FilledAmount = YearEndFilled;
                    PlantillaYearEnd.CasualAmount = CasualYearEnd;
                    PlantillaYearEnd.FundType = reader.GetValue(2).ToString();

                    PlantillaSummaryModel PlantillaMidYear = new PlantillaSummaryModel();
                    PlantillaMidYear.OfficeID = reader.GetInt32(0);
                    PlantillaMidYear.OfficeName = reader.GetValue(1).ToString() + " - Filled (" + FilledCount + ") Vacant (" + VacantCount + ") Casual (" + Casual + ")";
                    PlantillaMidYear.PSComponent = "Mid year";
                    PlantillaMidYear.VacantAmount = MidYearVacant;
                    PlantillaMidYear.FilledAmount = MidYearFilled;
                    PlantillaMidYear.CasualAmount = CasualYearEnd;
                    PlantillaMidYear.FundType = reader.GetValue(2).ToString();


                    if (FilledCount != 0 || VacantCount != 0)
                    {
                        PlantillaSummaryList.Add(PlantillaAnnual);
                        PlantillaSummaryList.Add(PlantillaPera);
                        PlantillaSummaryList.Add(PlantillaClothing);
                        PlantillaSummaryList.Add(PlantillaCashGift);
                        PlantillaSummaryList.Add(PlantillaGSIS);
                        PlantillaSummaryList.Add(PlantillaPagIbig);
                        PlantillaSummaryList.Add(PlantillaPhilHealth);
                        PlantillaSummaryList.Add(PlantillaECC);
                        PlantillaSummaryList.Add(PlantillaYearEnd);
                        PlantillaSummaryList.Add(PlantillaMidYear);
                    }
                }
            }
            return PlantillaSummaryList;
        }
        public void GetPlantillaCount(int? YearOf, int OfficeID, ref int VacantCount, ref int FilledCount,
            ref double VacantAnnual, ref double FilledAnnual, ref double PeraVacant, ref double PeraFilled,
            ref double ClothingVacant, ref double ClothingFilled, ref double CashGiftVacant, ref double CashGiftFilled,
            ref double GSISVacant, ref double GSISFilled, ref double PagIbigVacant, ref double PagIbigFilled,
            ref double PhilhealthVacant, ref double PhilhealthFilled, ref double ECCVacant, ref double ECCFilled,
            ref double YearEndVacant, ref double YearEndFilled, ref double MidYearVacant, ref double MidYearFilled,
            ref int Casual, ref double CasualAnnual, ref double CasualPERA, ref double CasualCashGift, ref double CasualGSIS,
            ref double CasualPagIbig, ref double CasualPhilhealth, ref double CasualECC, ref double CasualYearEnd)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_getPlantillaSummary " + YearOf + ", " + OfficeID + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    FilledCount = Convert.ToInt32(reader.GetValue(0));
                    VacantCount = Convert.ToInt32(reader.GetValue(1));
                    VacantAnnual = Convert.ToDouble(reader.GetValue(2));
                    FilledAnnual = Convert.ToDouble(reader.GetValue(3));
                    PeraVacant = Convert.ToDouble(reader.GetValue(4));
                    PeraFilled = Convert.ToDouble(reader.GetValue(5));
                    ClothingVacant = Convert.ToDouble(reader.GetValue(6));
                    ClothingFilled = Convert.ToDouble(reader.GetValue(7));
                    CashGiftVacant = Convert.ToDouble(reader.GetValue(8));
                    CashGiftFilled = Convert.ToDouble(reader.GetValue(9));
                    GSISVacant = Convert.ToDouble(reader.GetValue(10));
                    GSISFilled = Convert.ToDouble(reader.GetValue(11));
                    PagIbigVacant = Convert.ToDouble(reader.GetValue(12));
                    PagIbigFilled = Convert.ToDouble(reader.GetValue(13));
                    PhilhealthVacant = Convert.ToDouble(reader.GetValue(14));
                    PhilhealthFilled = Convert.ToDouble(reader.GetValue(15));
                    ECCVacant = Convert.ToDouble(reader.GetValue(16));
                    ECCFilled = Convert.ToDouble(reader.GetValue(17));
                    YearEndVacant = Convert.ToDouble(reader.GetValue(18));
                    YearEndFilled = Convert.ToDouble(reader.GetValue(19));
                    MidYearVacant = Convert.ToDouble(reader.GetValue(20));
                    MidYearFilled = Convert.ToDouble(reader.GetValue(21));
                    Casual = Convert.ToInt32(reader.GetValue(22));
                    CasualAnnual = Convert.ToDouble(reader.GetValue(23));
                    CasualPERA = Convert.ToDouble(reader.GetValue(24));
                    CasualCashGift = Convert.ToDouble(reader.GetValue(25));
                    CasualGSIS = Convert.ToDouble(reader.GetValue(26));
                    CasualPagIbig = Convert.ToDouble(reader.GetValue(27));
                    CasualPhilhealth = Convert.ToDouble(reader.GetValue(28));
                    CasualECC = Convert.ToDouble(reader.GetValue(29));
                    CasualYearEnd = Convert.ToDouble(reader.GetValue(30));
                }
            }
        }
    }
}