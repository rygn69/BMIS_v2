using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;
using iFMIS_BMS.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers.Grid
{
    public class grAccounts
    {
        public IEnumerable<account_code> grAccount_list(string FundDropdown)
        {
            List<account_code> AccountList = new List<account_code>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                FundDropdown = FundDropdown == null || FundDropdown == "" ? "0" : FundDropdown;
                SqlCommand com = new SqlCommand(@"SELECT a.ProposedID, a.AccountID, a.AccountName , a.ProposalYear, a.ProgramID, a.AccountCode, a.OOEID, a.OfficeID, c.OfficeName, b.ProgramDescription, d.OOEName
                                            FROM tbl_R_BMSProposedAccounts as a
                                            LEFT JOIN tbl_R_BMSOfficePrograms as b ON a.ProgramID = b.ProgramID and a.OfficeID = b.OfficeID
                                            LEFT JOIN tbl_R_BMSOffices as c ON c.OfficeID = a.OfficeID " +
                                            "LEFT JOIN tbl_R_BMSObjectOfExpenditure as d ON d.OOEID = a.OOEID " +
                                            "LEFT JOIN tbl_T_BMSBudgetProposal as e ON e.AccountID = a.AccountID " +
                                            "WHERE a.OOEID = '" + FundDropdown + "' and a.ActionCode = 1 and b.ProgramYear = 2017 and b.ActionCode = 1 and a.AccountCode = 0 and e.ProposalYear = YEAR(GETDATE())+1 and e.ProposalStatusCommittee = 1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    account_code account_list = new account_code();
                    account_list.ProposalID = Convert.ToInt32(reader.GetValue(0));
                    account_list.account_id = Convert.ToInt32(reader.GetValue(1));
                    account_list.account_desc = Convert.ToString(reader.GetValue(2));
                    account_list.ProposalDate = Convert.ToString(reader.GetValue(3));
                    account_list.programID = Convert.ToInt32(reader.GetValue(4));
                    account_list.AccountCode = Convert.ToInt32(reader.GetValue(5));
                    account_list.OOEid = Convert.ToInt32(reader.GetValue(6));
                    account_list.officeID = Convert.ToInt32(reader.GetValue(7));
                    account_list.office_desc = Convert.ToString(reader.GetValue(8));
                    account_list.program_desc = Convert.ToString(reader.GetValue(9));
                    account_list.OOEName = Convert.ToString(reader.GetValue(10));
                    //account_list
                    AccountList.Add(account_list);
                }
            }
            return AccountList;
        }
        public IEnumerable<account_code> getExistingAccounts()
        {
            List<account_code> AccountList = new List<account_code>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select fmisAccountCode,ThirdLevelGroup,isnull(AccountCode,0),ChildSeriesNumber,AccountName,FundType from tbl_R_BMSAccounts where Active = 1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    account_code account_list = new account_code();
                    account_list.account_id = Convert.ToInt32(reader.GetValue(0));
                    account_list.ThirdLevelGroup = reader.GetValue(1).ToString();
                    account_list.AccountCode = Convert.ToInt32(reader.GetValue(2));
                    account_list.child_series_no = reader.GetValue(3).ToString();
                    account_list.Account_Name = reader.GetValue(4).ToString();
                    account_list.FundType = reader.GetValue(5).ToString();
                    
                    //account_list
                    AccountList.Add(account_list);
                }
            }
            return AccountList;
        }
        public IEnumerable<account_code> grBuildProposed(string FundDropdown)
        {
            List<account_code> AccountList = new List<account_code>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                FundDropdown = FundDropdown == null || FundDropdown == "" ? "0" : FundDropdown;
                SqlCommand com = new SqlCommand(@"SELECT a.ProposedID, a.AccountID, a.AccountName , a.ProposalYear, a.ProgramID, a.AccountCode, a.OOEID, a.OfficeID, c.OfficeName, b.ProgramDescription, d.OOEName
                                            FROM tbl_R_BMSProposedAccounts as a
                                            LEFT JOIN tbl_R_BMSOfficePrograms as b ON a.ProgramID = b.ProgramID and a.OfficeID = b.OfficeID
                                            LEFT JOIN tbl_R_BMSOffices as c ON c.OfficeID = a.OfficeID " +
                                            "LEFT JOIN tbl_R_BMSObjectOfExpenditure as d ON d.OOEID = a.OOEID " +
                                            "LEFT JOIN tbl_T_BMSBudgetProposal as e ON e.AccountID = a.AccountID " +
                                            "WHERE a.OOEID = '" + FundDropdown + "' and a.ActionCode = 1 and b.ProgramYear = YEAR(GETDATE())+1 and b.ActionCode = 1 and a.AccountCode != 0  and e.ProposalYear = YEAR(GETDATE())+1 and e.ProposalStatusCommittee = 1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    account_code account_list = new account_code();
                    account_list.ProposalID = Convert.ToInt32(reader.GetValue(0));
                    account_list.account_id = Convert.ToInt32(reader.GetValue(1));
                    account_list.account_desc = Convert.ToString(reader.GetValue(2));
                    account_list.ProposalDate = Convert.ToString(reader.GetValue(3));
                    account_list.programID = Convert.ToInt32(reader.GetValue(4));
                    account_list.AccountCode = Convert.ToInt32(reader.GetValue(5));
                    account_list.OOEid = Convert.ToInt32(reader.GetValue(6));
                    account_list.officeID = Convert.ToInt32(reader.GetValue(7));
                    account_list.office_desc = Convert.ToString(reader.GetValue(8));
                    account_list.program_desc = Convert.ToString(reader.GetValue(9));
                    account_list.OOEName = Convert.ToString(reader.GetValue(10));
                    //account_list
                    AccountList.Add(account_list);
                }
            }
            return AccountList;
        }
        public IEnumerable<ProgramAccountsModel> getOfficeAccounts(int? OfficeID, int? YearOf)
        {
            List<ProgramAccountsModel> AccountList = new List<ProgramAccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select a.ProgramAccountID,a.AccountID,a.ProgramID,a.ObjectOfExpendetureID,isnull(a.OrderNo,1),
                                                a.AccountName,b.ProgramDescription,c.OOEName from tbl_R_BMSProgramAccounts as a
                                                LEFT JOIN tbl_R_BMSOfficePrograms as b on b.ProgramID = a.ProgramID 
                                                and b.ProgramYear = a.AccountYear and b.ActionCode =a.ActionCode
                                                LEFT JOIN tbl_R_BMSObjectOfExpenditure as c on c.ooeid = a.objectofexpendetureID
                                                where a.ActionCode= 1 and a.AccountYear = "+YearOf+" and b.OfficeID = "+OfficeID
                                                +"ORDER BY b.OrderNo,a.ObjectOfExpendetureID,a.OrderNo", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ProgramAccountsModel Account = new ProgramAccountsModel();
                    Account.ProgramAccountID = Convert.ToInt64(reader.GetValue(0));
                    Account.AccountID = Convert.ToInt32(reader.GetValue(1));
                    Account.OfficeProgramID = Convert.ToInt32(reader.GetValue(2));
                    Account.ObjectOfExpendetureID = Convert.ToInt32(reader.GetValue(3));
                    Account.OrderNo = Convert.ToInt32(reader.GetValue(4));
                    Account.AccountDescripttion = reader.GetValue(5).ToString();
                    Account.ProgramDescription = reader.GetValue(6).ToString();
                    Account.OOEDesciption = reader.GetValue(7).ToString();
                    Account.isProposed = 0;
                    
                    AccountList.Add(Account);
                }
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select a.ProposedID,a.AccountID,a.ProgramID,a.OOEID,1,a.AccountName,b.ProgramDescription
                                                from tbl_R_BMSProposedAccounts as a
                                                LEFT JOIN tbl_R_BMSOfficePrograms as b on b.ProgramID = a.ProgramID 
                                                and b.ProgramYear = a.ProposalYear and b.ActionCode = a.ActionCode
                                                where a.ProposalYear = "+YearOf+" and b.OfficeID = "+OfficeID+"", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ProgramAccountsModel Account = new ProgramAccountsModel();
                    Account.ProgramAccountID = Convert.ToInt64(reader.GetValue(0));
                    Account.AccountID = Convert.ToInt32(reader.GetValue(1));
                    Account.OfficeProgramID = Convert.ToInt32(reader.GetValue(2));
                    Account.ObjectOfExpendetureID = Convert.ToInt32(reader.GetValue(3));
                    Account.OrderNo = Convert.ToInt32(reader.GetValue(4));
                    Account.AccountDescripttion = reader.GetValue(5).ToString();
                    Account.ProgramDescription = reader.GetValue(6).ToString();
                    Account.isProposed = 0;

                    AccountList.Add(Account);
                }
            }
            return AccountList;
        }
        public IEnumerable<account_code> grAcountProposed(string FundDropdown)
        {
            List<account_code> AccountList = new List<account_code>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                FundDropdown = FundDropdown == null || FundDropdown == "" ? "0" : FundDropdown;
                SqlCommand com = new SqlCommand(@"SELECT a.ProposedID, a.AccountID, a.AccountName , a.ProposalYear, a.ProgramID, a.AccountCode, a.OOEID, a.OfficeID, c.OfficeName, b.ProgramDescription, d.OOEName
                                            FROM tbl_R_BMSProposedAccounts as a
                                            LEFT JOIN tbl_R_BMSOfficePrograms as b ON a.ProgramID = b.ProgramID and a.OfficeID = b.OfficeID
                                            LEFT JOIN tbl_R_BMSOffices as c ON c.OfficeID = a.OfficeID " +
                                            "LEFT JOIN tbl_R_BMSObjectOfExpenditure as d ON d.OOEID = a.OOEID " +
                                            "WHERE a.OOEID = '" + FundDropdown + "' and a.ActionCode = 1 and b.ProgramYear = 2017 and b.ActionCode = 1 and a.AccountCode != 0", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    account_code account_list = new account_code();
                    account_list.ProposalID = Convert.ToInt32(reader.GetValue(0));
                    account_list.account_id = Convert.ToInt32(reader.GetValue(1));
                    account_list.account_desc = Convert.ToString(reader.GetValue(2));
                    account_list.ProposalDate = Convert.ToString(reader.GetValue(3));
                    account_list.programID = Convert.ToInt32(reader.GetValue(4));
                    account_list.AccountCode = Convert.ToInt32(reader.GetValue(5));
                    account_list.OOEid = Convert.ToInt32(reader.GetValue(6));
                    account_list.officeID = Convert.ToInt32(reader.GetValue(7));
                    account_list.office_desc = Convert.ToString(reader.GetValue(8));
                    account_list.program_desc = Convert.ToString(reader.GetValue(9));
                    account_list.OOEName = Convert.ToString(reader.GetValue(10));
                    //account_list
                    AccountList.Add(account_list);
                }
            }
            return AccountList;
        }
        public IEnumerable<account_code> grActiveAccount_list()
        {
            List<account_code> AccountList = new List<account_code>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("Select AccountID, AccountName, FundType from tbl_R_BMSAccounts where Active = 1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    account_code account_list = new account_code();
                    account_list.account_id = Convert.ToInt32(reader.GetValue(0));
                    account_list.account_desc = reader.GetValue(1).ToString();
                    account_list.FundType = reader.GetValue(2).ToString();
                    AccountList.Add(account_list);
                }
            }
            return AccountList;
        }
        public IEnumerable<RGEmployeeSalary> grGetEmployeeSalary(int? OfficeID, int? BudgetYear)
        {
            List<RGEmployeeSalary> EmployeeSalary = new List<RGEmployeeSalary>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                OfficeID = OfficeID == null ? 0 : OfficeID;
                BudgetYear = BudgetYear == null ? 0 : BudgetYear;
                con.Open();
                SqlCommand query_OfficeID = new SqlCommand(@"SELECT COALESCE(SUM(PMISOfficeID), 0) AS PMISOfficeID FROM tbl_R_BMSOffices WHERE OfficeID = " + OfficeID + " ", con);  
                string PMISOfficeID = query_OfficeID.ExecuteScalar().ToString();
                var Year = BudgetYear - 1;
                SqlCommand query_EmployeeSalary = new SqlCommand(@"SELECT a.eID, a.Lastname, a.Firstname, a.MI, Case WHEN a.EmploymentGroup = 1 then a.BasicNew else a.basic * 22 end, Case WHEN b.EmploymentGroup = 1 then b.BasicNew else b.basic * 22 end FROM dbo.tbl_R_BMSStepIncrement as a LEFT JOIN pmis.dbo.vw_RGPermanentAndCasual as b ON a.eid = b.eid WHERE a.OfficeID = '" + PMISOfficeID + "' and a.DateCopied LIKE '%" + Year + "%' and a.ActionCode = 1 and a.Basic != 0", con);                
                SqlDataReader reader_EmployeeSalary = query_EmployeeSalary.ExecuteReader();
                while(reader_EmployeeSalary.Read()){
                    RGEmployeeSalary data = new RGEmployeeSalary();
                    data.EmployeeID = Convert.ToInt32(reader_EmployeeSalary.GetValue(0));
                    data.Lastname = reader_EmployeeSalary.GetString(1);
                    data.Firstname = reader_EmployeeSalary.GetString(2);
                    data.MI = reader_EmployeeSalary.GetString(3);
                    data.EmployeeName = data.Lastname+", "+data.Firstname+" "+data.MI+".";
                    data.BudgetSalary = Convert.ToDecimal(reader_EmployeeSalary.GetValue(4));
                    data.AppointmentSalary = Convert.ToDecimal(reader_EmployeeSalary.GetValue(5));
                    EmployeeSalary.Add(data);
                }
            }
            return EmployeeSalary;
        }
        public IEnumerable<EmployeeType> grMagnaCarta(int? AccountID, int? BudgetYear)
        {
            List<EmployeeType> MagnaCartaOffice = new List<EmployeeType>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                AccountID = AccountID == null ? 0 : AccountID;
                SqlCommand com = new SqlCommand(@"SELECT
                        dbo.tbl_R_BMSOffices.OfficeName
                        FROM
                        dbo.tbl_R_BMSOfficePrograms
                        INNER JOIN dbo.tbl_R_BMSProgramAccounts ON dbo.tbl_R_BMSProgramAccounts.ProgramID = dbo.tbl_R_BMSOfficePrograms.ProgramID
                        INNER JOIN dbo.tbl_R_BMSOffices ON dbo.tbl_R_BMSOfficePrograms.OfficeID = dbo.tbl_R_BMSOffices.OfficeID
                        INNER JOIN dbo.tbl_R_BMSAccounts ON dbo.tbl_R_BMSAccounts.FMISAccountCode = dbo.tbl_R_BMSProgramAccounts.AccountID
                        WHERE dbo.tbl_R_BMSAccounts.AccountCode = '" + AccountID + "' and AccountYear = '"+BudgetYear+"' and ProgramYear = '"+BudgetYear+"'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while(reader.Read()){
                    EmployeeType eType = new EmployeeType();
                    eType.OfficeName = reader.GetString(0);
                    MagnaCartaOffice.Add(eType);
                }

            }
            return MagnaCartaOffice;
        }
        public IEnumerable<account_code> grAccountDelete_list()
        {
            List<account_code> AccountList = new List<account_code>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("Select AccountID, AccountName, FundType from tbl_R_BMSAccounts where Active = 0", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    account_code account_list = new account_code();
                    account_list.account_id = Convert.ToInt32(reader.GetValue(0));
                    account_list.account_desc = reader.GetValue(1).ToString();
                    account_list.FundType = reader.GetValue(2).ToString();
                    AccountList.Add(account_list);
                }
            }
            return AccountList;
        }
        public IEnumerable<account_code> grAccount_listData(string AccountData)
        {
            List<account_code> AccountList = new List<account_code>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                AccountData = AccountData == null || AccountData == "" ? "0" : AccountData;
                SqlCommand com = new SqlCommand("Select AccountID, AccountName, ChildAccountCode from tbl_R_BMSAccounts where ChildAccountCode='" + AccountData + "' or AccountName='" + AccountData + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    account_code account_list = new account_code();
                    account_list.account_id = Convert.ToInt32(reader.GetValue(0));
                    account_list.account_desc = reader.GetString(1);
                    account_list.ChildAccountCode = reader.GetString(2);
                    AccountList.Add(account_list);
                }
            }
            return AccountList;
        }
        public IEnumerable<account_code> grAccount_listOOEData(string AccountData, string ProgramID)
        {
            List<account_code> AccountList = new List<account_code>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                AccountData = AccountData == null || AccountData == "" ? "0" : AccountData;
                SqlCommand com = new SqlCommand("Select AccountID, AccountName, ChildAccountCode, OrderNo from vw_BMS_Accounts where ProgramID='" + ProgramID + "' and ObjectOfExpendetureID='" + AccountData + "' and AccountYear = YEAR(GETDATE())", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    account_code account_list = new account_code();
                    account_list.account_id = Convert.ToInt32(reader.GetValue(0));
                    account_list.account_desc = reader.GetString(1);
                    account_list.ChildAccountCode = reader.GetString(2);
                    account_list.OrderNo = Convert.ToInt32(reader.GetValue(3));
                    AccountList.Add(account_list);
                }
            }
            return AccountList;
        }

        public IEnumerable<account_code> grAccount_Info(string OfficeDropdown, string ProgramDropdown, string AccountDropdown)
        {
            List<account_code> AccountList = new List<account_code>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                AccountDropdown = AccountDropdown == null || AccountDropdown == "" ? "0" : AccountDropdown;
                ProgramDropdown = ProgramDropdown == null || ProgramDropdown == "" ? "0" : ProgramDropdown;
                OfficeDropdown = OfficeDropdown == null || OfficeDropdown == "" ? "0" : OfficeDropdown;

                SqlCommand com = new SqlCommand("Select * from vw_account_info where ref_account_id="+AccountDropdown+" and program_id="+ProgramDropdown+" and office_id="+OfficeDropdown+" and status = 1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    account_code account_list = new account_code();
                    account_list.office_desc = reader.GetString(2);
                    account_list.program_desc = reader.GetString(1);                   
                    account_list.account_desc = reader.GetString(3);
                    account_list.account_id = reader.GetInt32(0);
                    account_list.program_id = reader.GetInt32(6);
                    account_list.office_id = reader.GetInt32(5);
                    account_list.ref_account_id = reader.GetInt32(4);
                    AccountList.Add(account_list);
                }
            }
            return AccountList;
        }

        public IEnumerable<account_code> grAccountNonActive_Info(string OfficeDropdown, string ProgramDropdown, string AccountDropdown)
        {
            List<account_code> AccountList = new List<account_code>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                AccountDropdown = AccountDropdown == null || AccountDropdown == "" ? "0" : AccountDropdown;
                ProgramDropdown = ProgramDropdown == null || ProgramDropdown == "" ? "0" : ProgramDropdown;
                OfficeDropdown = OfficeDropdown == null || OfficeDropdown == "" ? "0" : OfficeDropdown;

                SqlCommand com = new SqlCommand("Select * from vw_account_info where ref_account_id=" + AccountDropdown + " and program_id=" + ProgramDropdown + " and office_id=" + OfficeDropdown + " and status = 2", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    account_code account_list = new account_code();
                    account_list.office_desc = reader.GetString(2);
                    account_list.program_desc = reader.GetString(1);
                    account_list.account_desc = reader.GetString(3);
                    account_list.account_id = reader.GetInt32(0);
                    account_list.program_id = reader.GetInt32(6);
                    account_list.office_id = reader.GetInt32(5);
                    account_list.ref_account_id = reader.GetInt32(4);
                    AccountList.Add(account_list);
                }
            }
            return AccountList;
        }
        public IEnumerable<AccountComputationModel> grAccountComputationList(int? YearActive)
        {
            List<AccountComputationModel> AccountList = new List<AccountComputationModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Select DISTINCT a.ComputationID, a.AccountCode, b.AccountName,
                                                  CASE WHEN a.Amount = 0 then 'Salary Based' ELSE CONVERT(nvarchar,CAST(a.Amount AS INT)) END,
                                                  CASE WHEN a.NoOfMonths = 12 THEN 'Monthly' ELSE 'Yearly' END, a.Percentage,
                                                  CASE WHEN isRoundOf = 1 THEN 'Yes' ELSE 'No' END,a.MaxAmount,
                                                  ISNULL(c.type, CASE WHEN a.EmployeeType = 1 THEN 'Regular' WHEN a.EmployeeType = 2 then 'Casual' WHEN a.EmployeeType = 3 THEN 'Casual & Regular' END ) 
                                                  FROM tbl_R_BMSAccountComputation as a 
                                                  LEFT JOIN tbl_R_BMSAccounts as b on b.AccountCode = a.AccountCode 
                                                  LEFT JOIN tbl_R_BMSMagnaCarta as c on c.AccountID = a.AccountCode
                                                  WHERE A.YearActive = " + YearActive + " order by b.accountName", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountComputationModel account = new AccountComputationModel();
                    account.ComputationID = reader.GetInt64(0);
                    account.AccountCode = reader.GetInt32(1);
                    account.AccountName = reader.GetValue(2).ToString();
                    account.strAmount = reader.GetValue(3).ToString();
                    account.Period = reader.GetValue(4).ToString();
                    account.Percentage = Convert.ToDouble(reader.GetValue(5));
                    account.RoundedOff = reader.GetValue(6).ToString();
                    account.MaxAmount = Convert.ToDouble(reader.GetValue(7));
                    account.EmpType = reader.GetValue(8).ToString();

                    AccountList.Add(account);

                }
            }
            return AccountList;
        }
        public IEnumerable<AccountsForBuildUpModel> GetNewCreatedAccountsFromProposal()
        {
            List<AccountsForBuildUpModel> AccountList = new List<AccountsForBuildUpModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select a.AccountID,b.AccountName, e.OfficeName, isnull(c.OrderNo,0) from tbl_R_BMSAccountsForBuildUp as a
                LEFT JOIN tbl_R_BMSAccounts as b on b.FMISAccountCode = a.AccountID and a.ActionCode = 1
                LEFT JOIN tbl_R_BMSProgramAccounts as c on c.AccountID = a.AccountID 
                and c.ActionCode = a.ActionCode and c.AccountYear = a.YearOf
                LEFT JOIN tbl_R_BMSOfficePrograms as d on d.ProgramID = c.ProgramID 
                and d.ActionCode = 1 and d.ProgramYear = c.AccountYear and a.YearOf = c.AccountYear
                LEFT JOIN tbl_R_BMSOffices as e on e.OfficeID = d.OfficeID 
                where a.ActionCode = 1 ORDER BY e.OfficeName,b.AccountName", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountsForBuildUpModel account = new AccountsForBuildUpModel();
                    account.AccountID = Convert.ToInt32(reader.GetValue(0));
                    account.AccountName = reader.GetValue(1).ToString();
                    account.OfficeName = reader.GetValue(2).ToString();
                    account.OrderNo = Convert.ToInt32(reader.GetValue(3));
                    AccountList.Add(account);

                }
            }
            return AccountList;
        }
        public IEnumerable<AccountsModel> grTargetAccount()
        {
            List<AccountsModel> account_list = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Select FMISAccountCode, AccountName + '- ('+ FundTyPe +')' from tbl_R_BMSAccounts 
                                                  where FMISAccountCode not in(select AccountID from tbl_R_BMSAccountsForBuildUp where ActionCode = 1) 
                                                  and Active = 1 and FundTyPe in('General Fund Proper','Economic Enterprises')", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel app = new AccountsModel();

                    app.AccountID = Convert.ToInt32(reader.GetValue(0));
                    app.AccountName = reader.GetValue(1).ToString();

                    account_list.Add(app);
                }
            }
            return account_list;
        }
        
    }
}