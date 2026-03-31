using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers.Maintenance
{
    public class CreateNewAccount_Layer
    {
        public string createNewAccounts(int? AccountID, string AccountName, int? OfficeID, int? ProgramID, int? OOEID, double? amount, string OOEName,int? yearof)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                AccountID = AccountID == null || AccountID <= 0 ? 0 : AccountID;
                var UserInfo = Account.UserInfo.eid.ToString();

                SqlCommand query_time = new SqlCommand(@"SELECT CONVERT(VARCHAR(30),GETDATE(),101) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 109), 13, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 16, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 19, 2) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 100), 18, 2)", con);
                con.Open();
                var timeDate = query_time.ExecuteScalar().ToString();
                var FMISAccountCode = 0;
                SqlCommand searchAccount = new SqlCommand(@"SELECT
                        a.FMISAccountCode
                        FROM tbl_R_BMSAccounts as a
                        WHERE a.FMISAccountCode = '" + AccountID + "' and a.AccountName = '" + AccountName.Replace("'", "''") + "'", con);
                FMISAccountCode = Convert.ToInt32(searchAccount.ExecuteScalar());
                var tempAC = FMISAccountCode.ToString();
                if (tempAC == null || tempAC == "0")
                {
                    Random _rdm = new Random();
                    int _min = 1000;
                    int _max = 9999;
                    var rand = _rdm.Next(_min, _max).ToString();
                    var TempOffice = OfficeID.ToString() ;
                    var TempProgramID = ProgramID.ToString();

                    tempAC = TempOffice + rand + TempProgramID;
                }

                if (ProgramID == 0 || OOEID == 0 || AccountName == null || OOEID== null || OOEName=="" || OOEName == null)
                {
                    return "All Fields are Required!";
                }
                else
                {
                    try
                    {
                        SqlCommand comGetAccount = new SqlCommand(@"select FMISAccountCode from tbl_R_BMSAccounts where FMISAccountCode = '" + AccountID + "'", con);
                        //var ID = Convert.ToInt32(comGetAccount.ExecuteScalar().ToString());
                        var ID = AccountID == 0 ? Convert.ToInt32(tempAC) : Convert.ToInt32(comGetAccount.ExecuteScalar().ToString());
                        var AccountCode = AccountID == 0 ? 0 : Convert.ToInt32(ID);
                        SqlCommand removeAccount = new SqlCommand(@"Insert into tbl_R_BMSProposedAccounts ([AccountID] ,[AccountName],[OOEName] ,[ProposalYear] ,[ProgramID],[AccountCode] ,[OOEID],[OfficeID],[ActionCode]) values('" + ID + "', '" + AccountName.Replace("'", "''") + "', '" + OOEName + "', "+ yearof + ", '" + ProgramID + "', '" + AccountCode + "', '" + OOEID + "', '" + OfficeID + "', 1)", con);   
                        try
                        {
                            
                            removeAccount.ExecuteNonQuery();
                            return "Success";
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
            }
        }

        public string createSuggestNewAccounts(createNewAccount NewAccount)
        {
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
                con.Close();
                SqlCommand query_program = new SqlCommand(@"insert into tbl_R_BMSProgramAccounts values(" + NewAccount.program_id + ", (SELECT TOP 1 AccountID+ 1  FROM tbl_R_BMSProgramAccounts ORDER BY AccountID DESC), " + NewAccount.ooe_id + ", 99, '" + timeDate + "', '" + NewAccount.account_name + "', '" + UserInfo + "', " + NewAccount.orderBy + ", YEAR(GETDATE())+1) ", con);
                SqlCommand query_programfmis = new SqlCommand(@"insert into fmis.dbo.tblBMS_AnnualBudget_Account values(" + NewAccount.program_id + ", (SELECT TOP 1 AccountID+ 1  FROM tbl_R_BMSProgramAccounts ORDER BY AccountID DESC), " + NewAccount.ooe_id + ", 99, '" + timeDate + "', '" + NewAccount.account_name + "', '" + UserInfo + "', " + NewAccount.orderBy + ", YEAR(GETDATE())+1) ", con);

                con.Open();
                if (NewAccount.program_id == 0 || NewAccount.ooe_id == 0 || NewAccount.account_name == null || NewAccount.orderBy == 0)
                {
                    return "All Fields are Required!";
                }
                else
                {
                    try
                    {
                        query_program.ExecuteNonQuery();
                        query_programfmis.ExecuteNonQuery();
                        SqlCommand query_proposal = new SqlCommand(@"insert into tbl_T_BMSBudgetProposal values((SELECT TOP 1 AccountID  FROM tbl_R_BMSProgramAccounts where AccountName = '" + NewAccount.account_name + "' and UserID = '" + UserInfo + "' and AccountYear = YEAR(GETDATE())+1 ORDER BY AccountID DESC), '" + UserInfo + "', " + NewAccount.program_id + ", YEAR(GETDATE())+1, '" + timeDate + "', 2, 2, 2, " + NewAccount.ammount_money + ", 0, 1)", con);
                        SqlCommand query_proposalfmis = new SqlCommand(@"insert into fmis.dbo.tblBMS_AnnualBudget values((SELECT TOP 1 AccountID  FROM ifmis.dbo.tbl_R_BMSProgramAccounts where AccountName = '" + NewAccount.account_name + "' and UserID = '" + UserInfo + "' and AccountYear = YEAR(GETDATE())+1 ORDER BY AccountID DESC), '" + UserInfo + "', " + NewAccount.program_id + ", YEAR(GETDATE())+1, '" + timeDate + "', 0, " + NewAccount.ammount_money + ", 1)", con);
                        try
                        {
                            query_proposal.ExecuteNonQuery();
                            query_proposalfmis.ExecuteNonQuery();
                            return "Success";
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                        //return "Success";
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
            }
        }
    }
}