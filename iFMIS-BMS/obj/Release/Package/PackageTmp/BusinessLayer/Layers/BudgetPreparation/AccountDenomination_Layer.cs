using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using iFMIS_BMS.Classes;

namespace iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation
{
    public class AccountDenomination_Layer
    {
        public string SaveAccountDenomination(AccountDenomination DenominationInfo)
        {
            try
            {
                var returnResult = "";
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    var UserInfo = Account.UserInfo.eid.ToString();

                    SqlCommand query_time = new SqlCommand(@"SELECT CONVERT(VARCHAR(30),GETDATE(),101) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 109), 13, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 16, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 19, 2) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 100), 18, 2)", con);
                    con.Open();
                    SqlDataReader reader = query_time.ExecuteReader();
                    var timeDate = "";
                    while (reader.Read())
                    {
                        timeDate = reader.GetString(0);
                    }
                    reader.Close();
                    String singleQuotes = DenominationInfo.DenominationName;
                    String ParticularName = singleQuotes.Replace("'", "''");
                    //SqlCommand query_insert = new SqlCommand(@"insert into tbl_T_BMSAccountDenomination output INSERTED.AccountDenominationID values('" + ParticularName + "', '" + DenominationInfo.DenominationAmount + "', '" + timeDate + "', '" + UserInfo + "', 1, " + DenominationInfo.TransactionYear + ", " + DenominationInfo.ProgramID + ", '" + DenominationInfo.AccountID + "', 0, '" + DenominationInfo.QuantityPercentage + "', '" + DenominationInfo.TotalDenominationAmount + "', '" + DenominationInfo.OfficeID + "','" + DenominationInfo.DenominationMonth + "')", con);
                    SqlCommand query_insert = new SqlCommand(@"exec sp_BMS_InsertNewDenomination '" + ParticularName + "', '" + DenominationInfo.DenominationAmount + "', '" + timeDate + "', '" + UserInfo + "', " + DenominationInfo.TransactionYear + ", " + DenominationInfo.ProgramID + ", '" + DenominationInfo.AccountID + "', '" + DenominationInfo.QuantityPercentage + "', '" + DenominationInfo.TotalDenominationAmount + "', '" + DenominationInfo.OfficeID + "','" + DenominationInfo.DenominationMonth + "'", con);
                    returnResult = query_insert.ExecuteScalar().ToString();
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Update tbl_R_BMSPreparationCieling set TotalProposed = TotalProposed + " + DenominationInfo.TotalDenominationAmount + " where OfficeID = " + DenominationInfo.OfficeID + "", con);
                    con.Open();
                    com.ExecuteNonQuery();

                }
                return "success_" + returnResult;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string SaveAccountDenominationLFC(AccountDenomination DenominationInfo)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    var UserInfo = Account.UserInfo.eid.ToString();

                    String singleQuotes = DenominationInfo.DenominationName;
                    String ParticularName = singleQuotes.Replace("'", "''");
                    SqlCommand query_insert = new SqlCommand(@"dbo.sp_BMS_AddParticularLFC '" + ParticularName + "', " + DenominationInfo.DenominationAmount
                                                            + ", '" + UserInfo + "', " + DenominationInfo.TransactionYear + ", " + DenominationInfo.ProgramID
                                                            + ", '" + DenominationInfo.AccountID + "', '" + DenominationInfo.QuantityPercentage
                                                            + "', '" + DenominationInfo.TotalDenominationAmount + "', '" + DenominationInfo.Office
                                                            + "','" + DenominationInfo.DenominationMonth + "'", con);
                    con.Open();
                    var Result = query_insert.ExecuteScalar().ToString();
                    if (Result == "1")
                    {
                        return "success";
                    }
                    else 
                    {
                        return Result;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string DeleteAccountDenomination(int? AccountDenominationID,int OfficeID, double Amount)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand query_AccountDenomination = new SqlCommand(@"UPDATE tbl_T_BMSAccountDenomination set ActionCode = 2 where AccountDenominationID='" + AccountDenominationID + "'", con);
                    con.Open();
                    query_AccountDenomination.ExecuteNonQuery();
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Update tbl_R_BMSPreparationCieling set TotalProposed = TotalProposed - " + Amount + " where OfficeID = " + OfficeID + "", con);
                    con.Open();
                    com.ExecuteNonQuery();

                }
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
        }
        public string UpdateAccountDenominationData(AccountDenomination UpdateInfo)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand query_UpdateDenomination = new SqlCommand(@"UPDATE tbl_T_BMSAccountDenomination set  DenominationAmount = '" + UpdateInfo.EditDenominationAmount + "', quantityOrPercentage = '" + UpdateInfo.EditQuantityPercentage 
                                                                        + "', TotalAmount = '" + UpdateInfo.EditTotalDenominationAmount + "',Month = '" + UpdateInfo.EditDenominationMonth + "' ,UserID=" + Account.UserInfo.eid
                                                                        + "  where AccountDenominationID = '" + UpdateInfo.AccountDenominationID + "'", con);
                    con.Open();
                    query_UpdateDenomination.ExecuteNonQuery();
                }
                
                var Amount = UpdateInfo.EditTotalDenominationAmount - UpdateInfo.OriginalAmount;//@Model.TotalAmount
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Update tbl_R_BMSPreparationCieling set TotalProposed = TotalProposed + " + Amount + " where OfficeID = " + UpdateInfo.OfficeID + "", con);
                    con.Open();
                    com.ExecuteNonQuery();

                }
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string UpdateTotalAmount(int AccountID, int ProgramID, int OfficeID, int ProposalYear) 
        {
            //var str = "";
            using (SqlConnection conpro = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec sp_BMS_UpdateProposeAmount "+ OfficeID + ","+ ProgramID + ","+ AccountID + ","+ ProposalYear + "", conpro);
                conpro.Open();
                com.ExecuteNonQuery().ToString();
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query_UpdateDenomination = new SqlCommand(@"Select isnull(Sum(TotalAmount),0) from tbl_T_BMSAccountDenomination where ProgramID = " + ProgramID 
                                                                    + " and AccountID = " + AccountID + " and TransactionYear = " + ProposalYear 
                                                                    + " and ActionCode = 1 and OfficeID = " + OfficeID + "", con);
                con.Open();
                
                return GlobalFunctions.CurrencyFormatString(Convert.ToDouble(query_UpdateDenomination.ExecuteScalar()));
            }
        }
        public string SaveDenomination(int? AccountID, int? ProgramID, int? ProposalYear, int? ProposalID, string OOEName)
        {
            var CanPS = 0;
            var CanMOOE = 0;
            var CanCO = 0;
            var CanFE = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query_time = new SqlCommand(@"SELECT CONVERT(VARCHAR(30),GETDATE(),101) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 109), 13, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 16, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 19, 2) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 100), 18, 2)", con);
                con.Open();
                SqlDataReader reader = query_time.ExecuteReader();
                var timeDate = "";
                while (reader.Read())
                {
                    timeDate = reader.GetString(0);
                }

                reader.Close();

                //SqlCommand com = new SqlCommand("update a set ProposalActionCode= 2  FROM dbo.tbl_T_BMSBudgetProposal AS a left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID left JOIN dbo.tbl_R_BMSAccounts AS b ON b.AccountID = e.AccountID left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode WHERE  a.AccountID = " + AccountID + "and a.ProposalYear = " + ProposalYear, con);
                //com.ExecuteNonQuery();

                SqlCommand query_DenominationAmount = new SqlCommand(@"SELECT SUM(DenominationAmount) FROM tbl_T_BMSAccountDenomination WHERE ActionCode = 1 or ActionCode = 2 and AccountID ='" + AccountID + "'and ProgramID ='" + ProgramID + "'and TransactionYear = '" + ProposalYear + "'", con);
                SqlDataReader reader_DenominationAmount = query_DenominationAmount.ExecuteReader();
                decimal Amount = 0;
                while (reader_DenominationAmount.Read())
                {

                    Amount = reader_DenominationAmount.GetDecimal(0);
                }
                reader_DenominationAmount.Close();

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

                SqlCommand query_updateAccount = new SqlCommand("Update tbl_T_BMSBudgetProposal set ProposalAmount ='" + Amount + "' WHERE ProposalID = '" + ProposalID + "'", con);

                #region CANPS --
                //if (OOEName == "PS")
                //{
                //    if (CanPS == 1)
                //    {
                //        SqlCommand query_program = new SqlCommand(@"insert into tbl_T_BMSBudgetProposal(AccountID,UserID,ProgramID,ProposalYear,ProposalDateTime,ProposalStatusHR,ProposalStatusInCharge,ProposalStatusCommittee,ProposalAmount,ProposalAllotedAmount,ProposalActionCode, ProposalDenominationCode) values (" + AccountID + ",'" + Account.UserInfo.eid.ToString() + "'," + ProgramID + "," + ProposalYear + ",'" + timeDate + "',2,2,2," + Amount + ",0.00,1, 1)", con);
                //        query_program.ExecuteNonQuery();
                //    }
                //    else
                //    {
                //        SqlCommand query_program = new SqlCommand(@"insert into tbl_T_BMSBudgetProposal(AccountID,UserID,ProgramID,ProposalYear,ProposalDateTime,ProposalStatusHR,ProposalStatusInCharge,ProposalStatusCommittee,ProposalAmount,ProposalAllotedAmount,ProposalActionCode, ProposalDenominationCode) values (" + AccountID + ",'" + Account.UserInfo.eid.ToString() + "'," + ProgramID + "," + ProposalYear + ",'" + timeDate + "',1,2,2," + Amount + ",0.00,1, 1)", con);
                //        query_program.ExecuteNonQuery();
                //    }
                //}
                //if (OOEName == "MOOE")
                //{
                //    if (CanMOOE == 1)
                //    {
                //        SqlCommand query_program = new SqlCommand(@"insert into tbl_T_BMSBudgetProposal(AccountID,UserID,ProgramID,ProposalYear,ProposalDateTime,ProposalStatusHR,ProposalStatusInCharge,ProposalStatusCommittee,ProposalAmount,ProposalAllotedAmount,ProposalActionCode, ProposalDenominationCode) values (" + AccountID + ",'" + Account.UserInfo.eid.ToString() + "'," + ProgramID + "," + ProposalYear + ",'" + timeDate + "',2,2,2," + Amount + ",0.00,1, 1)", con);
                //        query_program.ExecuteNonQuery();
                //    }
                //    else
                //    {
                //        SqlCommand query_program = new SqlCommand(@"insert into tbl_T_BMSBudgetProposal(AccountID,UserID,ProgramID,ProposalYear,ProposalDateTime,ProposalStatusHR,ProposalStatusInCharge,ProposalStatusCommittee,ProposalAmount,ProposalAllotedAmount,ProposalActionCode, ProposalDenominationCode) values (" + AccountID + ",'" + Account.UserInfo.eid.ToString() + "'," + ProgramID + "," + ProposalYear + ",'" + timeDate + "',1,2,2," + Amount + ",0.00,1, 1)", con);
                //        query_program.ExecuteNonQuery();
                //    }
                //}
                //if (OOEName == "CO")
                //{
                //    if (CanCO == 1)
                //    {
                //        SqlCommand query_program = new SqlCommand(@"insert into tbl_T_BMSBudgetProposal(AccountID,UserID,ProgramID,ProposalYear,ProposalDateTime,ProposalStatusHR,ProposalStatusInCharge,ProposalStatusCommittee,ProposalAmount,ProposalAllotedAmount,ProposalActionCode, ProposalDenominationCode) values (" + AccountID + ",'" + Account.UserInfo.eid.ToString() + "'," + ProgramID + "," + ProposalYear + ",'" + timeDate + "',2,2,2," + Amount + ",0.00,1, 1)", con);
                //        query_program.ExecuteNonQuery();
                //    }
                //    else
                //    {
                //        SqlCommand query_program = new SqlCommand(@"insert into tbl_T_BMSBudgetProposal(AccountID,UserID,ProgramID,ProposalYear,ProposalDateTime,ProposalStatusHR,ProposalStatusInCharge,ProposalStatusCommittee,ProposalAmount,ProposalAllotedAmount,ProposalActionCode, ProposalDenominationCode) values (" + AccountID + ",'" + Account.UserInfo.eid.ToString() + "'," + ProgramID + "," + ProposalYear + ",'" + timeDate + "',1,2,2," + Amount + ",0.00,1, 1)", con);
                //        query_program.ExecuteNonQuery();
                //    }
                //}
                //if (OOEName == "FE")
                //{
                //    if (CanFE == 1)
                //    {
                //        SqlCommand query_program = new SqlCommand(@"insert into tbl_T_BMSBudgetProposal(AccountID,UserID,ProgramID,ProposalYear,ProposalDateTime,ProposalStatusHR,ProposalStatusInCharge,ProposalStatusCommittee,ProposalAmount,ProposalAllotedAmount,ProposalActionCode, ProposalDenominationCode) values (" + AccountID + ",'" + Account.UserInfo.eid.ToString() + "'," + ProgramID + "," + ProposalYear + ",'" + timeDate + "',2,2,2," + Amount + ",0.00,1, 1)", con);
                //        query_program.ExecuteNonQuery();
                //    }
                //    else
                //    {
                //        SqlCommand query_program = new SqlCommand(@"insert into tbl_T_BMSBudgetProposal(AccountID,UserID,ProgramID,ProposalYear,ProposalDateTime,ProposalStatusHR,ProposalStatusInCharge,ProposalStatusCommittee,ProposalAmount,ProposalAllotedAmount,ProposalActionCode, ProposalDenominationCode) values (" + AccountID + ",'" + Account.UserInfo.eid.ToString() + "'," + ProgramID + "," + ProposalYear + ",'" + timeDate + "',1,2,2," + Amount + ",0.00,1, 1)", con);
                //        query_program.ExecuteNonQuery();
                //    }
                //}
                #endregion
                SqlCommand query_updateActionCode = new SqlCommand("Update tbl_T_BMSAccountDenomination set ActionCode = 2 WHERE ProgramID = '" + ProgramID + "' and AccountID = '" + AccountID + "'", con);
                query_updateActionCode.ExecuteNonQuery();

                SqlCommand insert_copiedFrom = new SqlCommand(@"insert into tbl_R_BMSCopiedFrom
                                                                select proposalID, " + ProposalYear + " -1 from tbl_T_BMSBudgetProposal where ProposalYear = " + ProposalYear + " and ProgramID = " + ProgramID + " and UserID = " + Account.UserInfo.eid.ToString() + " and AccountID =" + AccountID + " and ProposalID NOT in (SELECT ProposalID FROM tbl_R_BMSCopiedFrom)", con);
                insert_copiedFrom.ExecuteNonQuery();
                return "success";
            }
        }
        public string UpdateAccountDenomination(int? AccountID, int? ProgramID, int? ProposalYear, int? ProposalID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query_DenominationAmount = new SqlCommand(@"SELECT SUM(DenominationAmount) FROM tbl_T_BMSAccountDenomination WHERE AccountID ='" + AccountID + "'and ProgramID ='" + ProgramID + "'and TransactionYear = '" + ProposalYear + "' and ActionCode = 1", con);
                SqlDataReader reader_DenominationAmount = query_DenominationAmount.ExecuteReader();
                decimal Amount = 0;
                while (reader_DenominationAmount.Read())
                {

                    Amount = reader_DenominationAmount.GetDecimal(0);
                }
                reader_DenominationAmount.Close();

                SqlCommand update_query = new SqlCommand(@"UPDATE tbl_T_BMSBudgetProposal set ProposalAmount = '" + Amount + "' WHERE ProposalID = '" + ProposalID + "'", con);
                update_query.ExecuteNonQuery();
                return "success";
            }
        }
        public string UpdateDenomination(int? AccountID, int? ProgramID, int? ProposalYear, int? ProposalID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query_time = new SqlCommand(@"SELECT CONVERT(VARCHAR(30),GETDATE(),101) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 109), 13, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 16, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 19, 2) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 100), 18, 2)", con);
                con.Open();
                SqlDataReader reader = query_time.ExecuteReader();
                var timeDate = "";
                while (reader.Read())
                {
                    timeDate = reader.GetString(0);
                }

                reader.Close();

                //SqlCommand com = new SqlCommand("update a set ProposalActionCode= 2  FROM dbo.tbl_T_BMSBudgetProposal AS a left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID left JOIN dbo.tbl_R_BMSAccounts AS b ON b.AccountID = e.AccountID left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode WHERE  a.AccountID = " + AccountID + "and a.ProposalYear = " + ProposalYear, con);
                //com.ExecuteNonQuery();

                SqlCommand query_DenominationAmount = new SqlCommand(@"SELECT SUM(DenominationAmount) FROM tbl_T_BMSAccountDenomination WHERE AccountID ='" + AccountID + "'and ProgramID ='" + ProgramID + "'and TransactionYear = '" + ProposalYear + "' and ActionCode = 1", con);
                SqlDataReader reader_DenominationAmount = query_DenominationAmount.ExecuteReader();
                decimal Amount = 0;
                while (reader_DenominationAmount.Read())
                {
                    Amount = reader_DenominationAmount.GetDecimal(0);
                }
                reader_DenominationAmount.Close();
                SqlCommand query_updateAccount = new SqlCommand("Update tbl_T_BMSBudgetProposal set ProposalAmount ='" + Amount + "' WHERE ProposalID = '" + ProposalID + "'", con);
                //SqlCommand query_program = new SqlCommand(@"insert into tbl_T_BMSBudgetProposal(AccountID,UserID,ProgramID,ProposalYear,ProposalDateTime,ProposalStatusHR,ProposalStatusInCharge,ProposalStatusCommittee,ProposalAmount,ProposalAllotedAmount,ProposalActionCode, ProposalDenominationCode) values (" + AccountID + ",'" + Account.UserInfo.eid.ToString() + "'," + ProgramID + "," + ProposalYear + ",'" + timeDate + "',2,2,2," + Amount + ",0.00,1, 1)", con);
                query_updateAccount.ExecuteNonQuery();

                return "success";
            }
        }
        public string UpdateProgramAccount(int? AccountID, int? ProgramID, int? ProposalYear, int? ProposalID, string OfficeLevel)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query_time = new SqlCommand(@"SELECT CONVERT(VARCHAR(30),GETDATE(),101) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 109), 13, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 16, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 19, 2) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 100), 18, 2)", con);
                con.Open();
                SqlDataReader reader = query_time.ExecuteReader();
                var timeDate = "";
                while (reader.Read())
                {
                    timeDate = reader.GetString(0);
                }

                reader.Close();

                SqlCommand query_DenominationAmount = new SqlCommand(@"SELECT SUM(DenominationAmount) FROM tbl_T_BMSAccountDenomination WHERE AccountID ='" + AccountID + "'and ProgramID ='" + ProgramID + "'and TransactionYear = '" + ProposalYear + "' and ActionCode = 1", con);
                SqlDataReader reader_DenominationAmount = query_DenominationAmount.ExecuteReader();
                decimal Amount = 0;
                while (reader_DenominationAmount.Read())
                {
                    Amount = reader_DenominationAmount.GetDecimal(0);
                }
                reader_DenominationAmount.Close();
                if (OfficeLevel == "HRMO In-Charge")
                {
                    SqlCommand query_updateAccount = new SqlCommand("Update tbl_T_BMSBudgetProposal set ProposalAmount ='" + Amount + "', ProposalActionCode = 1, ProposalStatusHR = 2 WHERE ProposalID = '" + ProposalID + "'", con);
                    query_updateAccount.ExecuteNonQuery();
                }
                else if (OfficeLevel == "Budget Office")
                {
                    SqlCommand query_updateAccount = new SqlCommand("Update tbl_T_BMSBudgetProposal set ProposalAmount ='" + Amount + "', ProposalActionCode = 1, ProposalStatusInCharge = 2 WHERE ProposalID = '" + ProposalID + "'", con);
                    query_updateAccount.ExecuteNonQuery();
                }
                else if (OfficeLevel == "Budget Committee / Local Finance Committee")
                {
                    SqlCommand query_updateAccount = new SqlCommand("Update tbl_T_BMSBudgetProposal set ProposalAmount ='" + Amount + "', ProposalActionCode = 1, ProposalStatusCommittee = 2 WHERE ProposalID = '" + ProposalID + "'", con);
                    query_updateAccount.ExecuteNonQuery();
                }

                SqlCommand query_Delete = new SqlCommand(@"Update tbl_R_BMSProposalRemark set Remarks = 'Approved' WHERE ProposalId =" + ProposalID, con);
                query_Delete.ExecuteNonQuery();

                return "success";
            }
        }
        public string UpdateBudgetDenomination(int? AccountID, int? ProgramID, int? ProposalYear, int? ProposalID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query_time = new SqlCommand(@"SELECT CONVERT(VARCHAR(30),GETDATE(),101) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 109), 13, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 16, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 19, 2) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 100), 18, 2)", con);
                con.Open();
                SqlDataReader reader = query_time.ExecuteReader();
                var timeDate = "";
                while (reader.Read())
                {
                    timeDate = reader.GetString(0);
                }

                reader.Close();

                //SqlCommand com = new SqlCommand("update a set ProposalActionCode= 2  FROM dbo.tbl_T_BMSBudgetProposal AS a left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID left JOIN dbo.tbl_R_BMSAccounts AS b ON b.AccountID = e.AccountID left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = e.ProgramID left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode WHERE  a.AccountID = " + AccountID + "and a.ProposalYear = " + ProposalYear, con);
                //com.ExecuteNonQuery();

                SqlCommand query_DenominationAmount = new SqlCommand(@"SELECT SUM(DenominationAmount) FROM tbl_T_BMSAccountDenomination WHERE AccountID ='" + AccountID + "'and ProgramID ='" + ProgramID + "'and TransactionYear = '" + ProposalYear + "' and ActionCode = 1", con);
                SqlDataReader reader_DenominationAmount = query_DenominationAmount.ExecuteReader();
                decimal Amount = 0;
                while (reader_DenominationAmount.Read())
                {
                    Amount = reader_DenominationAmount.GetDecimal(0);
                }
                reader_DenominationAmount.Close();
                SqlCommand query_updateAccount = new SqlCommand("Update tbl_T_BMSBudgetProposal set ProposalAllotedAmount ='" + Amount + "', ProposalStatusCommittee = 1 WHERE ProposalID = '" + ProposalID + "'", con);
                //SqlCommand query_program = new SqlCommand(@"insert into tbl_T_BMSBudgetProposal(AccountID,UserID,ProgramID,ProposalYear,ProposalDateTime,ProposalStatusHR,ProposalStatusInCharge,ProposalStatusCommittee,ProposalAmount,ProposalAllotedAmount,ProposalActionCode, ProposalDenominationCode) values (" + AccountID + ",'" + Account.UserInfo.eid.ToString() + "'," + ProgramID + "," + ProposalYear + ",'" + timeDate + "',2,2,2," + Amount + ",0.00,1, 1)", con);
                query_updateAccount.ExecuteNonQuery();

                return "success";
            }
        }
    }
}