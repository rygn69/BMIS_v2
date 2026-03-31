using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;
using iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation;
using System.Data.SqlClient;
using System.IO;
using iFMIS_BMS.FMIS;
using System.Data;
using iFMIS_BMS.BusinessLayer.Models.BudgetControl;

namespace iFMIS_BMS.BusinessLayer.Dropdowns
{
    public class DropdownLayers
    {
        ServiceSoapClient FMIS = new ServiceSoapClient();

        Int16 CanReviewPS, CanReviewMOOE, CanReviewCO, CanReviewFE;
        Int16 PSID, MOOEID, COID, FEID;
        public IEnumerable<office> office_code()
        {
            List<office> OfficeList = new List<office>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select OfficeID, CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') from tbl_R_BMSOffices where PMISOfficeID is not null  ORDER BY OfficeName", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    office office_list = new office();
                    office_list.office_id = reader.GetInt32(0);
                    office_list.office_name = reader.GetString(1);
                    OfficeList.Add(office_list);
                }
            }
            return OfficeList;
        }

         public IEnumerable<office> GetOffice_AIPVerse(int? regularaipid, int? yearof)
        {
            List<office> OfficeList = new List<office>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (regularaipid == 1) //annual budget
                {
                    SqlCommand com = new SqlCommand("select OfficeID, CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') from tbl_R_BMSOffices where PMISOfficeID is not null  ORDER BY OfficeName", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        office office_list = new office();
                        office_list.office_id = reader.GetInt32(0);
                        office_list.office_name = reader.GetString(1);
                        OfficeList.Add(office_list);
                    }
                }
                else
                {
                    SqlCommand com = new SqlCommand("select distinct b.OfficeID, CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') as Office " +
                                                       "from ifmis.dbo.tbl_R_BMSOffices as a inner join " +
                                                       "ifmis.dbo.tbl_R_BMSOfficePrograms as b on b.OfficeID = a.OfficeID and b.ActionCode = 1 and b.ProgramYear = " + yearof + " " +
                                                       "where isnull(PMISOfficeID, '') != '' " +
                                                       "and  b.ProgramID in (select z.ProgramID from [IFMIS].[dbo].[tbl_T_BMSAccountDenomination] as z where z.ProgramID = b.ProgramID and z.ActionCode = 1 and z.TransactionYear = b.ProgramYear and  isnull(z.supplemental_tag, 0) = 1) " +
                                                       "ORDER BY  Office", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        office office_list = new office();
                        office_list.office_id = reader.GetInt32(0);
                        office_list.office_name = reader.GetString(1);
                        OfficeList.Add(office_list);
                    }
                }
            }
            return OfficeList;
        }
        public IEnumerable<office> OfficeControl()
        {
            List<office> OfficeList = new List<office>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_ddlOfficeControl "+Account.UserInfo.Department+", "+Account.UserInfo.UserTypeID+"", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    office office_list = new office();
                    office_list.office_id = reader.GetInt32(0);
                    office_list.office_name = reader.GetString(1);
                    OfficeList.Add(office_list);
                }
            }
            return OfficeList;
        }

        public IEnumerable<office> dllgetEmployee(int? officeid=0)
        {
            List<office> OfficeList = new List<office>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT [eid],[EmpName] FROM [pmis].[dbo].[vwMergeAllEmployee_Modified] where [Department] in (SElect PMISOfficeID from ifmis.dbo.tbl_R_BMSOffices where officeid=" + officeid + ") order by [EmpName]", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    office office_list = new office();
                    office_list.eid = reader.GetInt64(0);
                    office_list.EmpName = reader.GetString(1);
                    OfficeList.Add(office_list);
                }
            }
            return OfficeList;
        }
        public IEnumerable<office> ddlOfficesWithProposal()
        {
            List<office> OfficeList = new List<office>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select DISTINCT c.OfficeID,Concat(c.OfficeName, ' (',REPLACE(OfficeAbbrivation, ' ', ''),')') from tbl_T_BMSBudgetProposal as a
                                                INNER JOIN tbl_R_BMSOfficePrograms as b on b.ProgramID = a.ProgramID 
                                                and a.ProposalActionCode = b.ActionCode and a.ProposalYear = b.ProgramYear
                                                INNER JOIN tbl_R_BMSOffices as c on c.OfficeID = b.OfficeID
                                                where a.ProposalYear = year(GETDATE()) + 1 and a.ProposalActionCode = 1
                                                ORDER BY Concat(c.OfficeName, ' (',REPLACE(OfficeAbbrivation, ' ', ''),')')", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    office office_list = new office();
                    office_list.office_id = reader.GetInt32(0);
                    office_list.office_name = reader.GetString(1);
                    OfficeList.Add(office_list);
                }
            }
            return OfficeList;
        }
        public IEnumerable<office> ddlMainOfficesWithProposal()
        {
            List<office> OfficeList = new List<office>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select DISTINCT c.OfficeID,Concat(c.OfficeName, ' (',REPLACE(OfficeAbbrivation, ' ', ''),')') from tbl_T_BMSBudgetProposal as a
                                                INNER JOIN tbl_R_BMSOfficePrograms as b on b.ProgramID = a.ProgramID 
                                                and a.ProposalActionCode = b.ActionCode and a.ProposalYear = b.ProgramYear
                                                INNER JOIN tbl_R_BMSOffices as c on c.OfficeID = b.OfficeID
                                                where a.ProposalYear = year(GETDATE()) + 1 and a.ProposalActionCode = 1 and 
                                                c.OfficeID in(select MainOfficeID from tbl_R_BMSOrdinanceOfficeGrouping)
                                                ORDER BY Concat(c.OfficeName, ' (',REPLACE(OfficeAbbrivation, ' ', ''),')')", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    office office_list = new office();
                    office_list.office_id = reader.GetInt32(0);
                    office_list.office_name = reader.GetString(1);
                    OfficeList.Add(office_list);
                }
            }
            return OfficeList;
        }
        
        
        
        public IEnumerable<AccountsModel> getPrograms(int? OfficeID, int? TrasactionYear)
        {
            List<AccountsModel> ProgramList = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT DISTINCT ProgramID, ProgramDescription FROM tbl_R_BMSOfficePrograms WHERE OfficeID='"+OfficeID+"' and ProgramYear = '"+TrasactionYear+"' and actioncode=1  ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while(reader.Read()){
                    AccountsModel program_list = new AccountsModel();
                    program_list.setProgramID = Convert.ToInt32(reader.GetValue(0));
                    program_list.setProgramDesc = reader.GetString(1);
                    ProgramList.Add(program_list);
                }
            
            }
            return ProgramList;
        }
        public IEnumerable<lbp5> OfficeAccountList(int? OfficeID)
        {
            List<lbp5> AccountList = new List<lbp5>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT
                        dbo.tbl_R_BMSProgramAccounts.AccountID,
                        dbo.tbl_R_BMSProgramAccounts.AccountName,
                        dbo.tbl_T_BMSBudgetProposal.ProposalAmount
                        FROM
                        dbo.tbl_R_BMSOfficePrograms
                        INNER JOIN dbo.tbl_R_BMSProgramAccounts ON dbo.tbl_R_BMSProgramAccounts.ProgramID = dbo.tbl_R_BMSOfficePrograms.ProgramID
                        INNER JOIN dbo.tbl_T_BMSBudgetProposal ON dbo.tbl_R_BMSProgramAccounts.AccountID = dbo.tbl_T_BMSBudgetProposal.AccountID AND dbo.tbl_R_BMSProgramAccounts.ProgramID = dbo.tbl_T_BMSBudgetProposal.ProgramID
                        WHERE
                        dbo.tbl_R_BMSOfficePrograms.OfficeID = '" + OfficeID+"' AND "+
                        "dbo.tbl_R_BMSOfficePrograms.ProgramYear = Year(GETDATE())+1 AND "+
                        "dbo.tbl_R_BMSProgramAccounts.ActionCode = 1 AND "+
                        "dbo.tbl_R_BMSOfficePrograms.ActionCode = 1 AND "+
                        "dbo.tbl_R_BMSProgramAccounts.AccountYear = Year(GETDATE())+1 AND " +
                        "dbo.tbl_T_BMSBudgetProposal.ProposalYear = Year(GETDATE())+1 AND " +
                        "dbo.tbl_T_BMSBudgetProposal.ProposalActionCode = 1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    lbp5 lbp5_list = new lbp5();
                    //lbp5_list.AccountID = Convert.ToInt32(reader.GetValue(0));
                    var checkAccountID = Convert.ToInt32(reader.GetValue(0));
                    var resultCheckAccountID = checkAccountCount(checkAccountID, OfficeID);
                    if(resultCheckAccountID == 0){
                        lbp5_list.AccountID = Convert.ToInt32(reader.GetValue(0));
                        lbp5_list.AccountName = reader.GetString(1);
                        lbp5_list.AccountAmount = Convert.ToDouble(reader.GetValue(2));
                        lbp5_list.AmountAccountString = "(₱ " + lbp5_list.AccountAmount + ") " + lbp5_list.AccountName;
                        AccountList.Add(lbp5_list);
                    }
                }
            }
            return AccountList;
        }
        public int checkAccountCount(int? checkAccountID, int? OfficeID)
        {
            int resultCheckAccountID = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand checkAccount = new SqlCommand(@"SELECT TOP 1
                        isnull(b.PPA_ID, 0)
                        FROM
                        dbo.tbl_R_LBP5_PPA_MFO as a
                        INNER JOIN dbo.tbl_R_LBP5_PPA_Denomination as b ON b.PPA_MFO_ID = a.PPA_MFO_ID
                        WHERE a.OfficeID = '"+OfficeID+"' and b.AccountID= '"+checkAccountID+"'", con);
                con.Open();
                resultCheckAccountID = Convert.ToInt32(checkAccount.ExecuteScalar());
                return resultCheckAccountID;
            }
            
        }
        public IEnumerable<ooe> ooe_list(int? ProgramID)
        {
            List<ooe> OOETypeList = new List<ooe>();
            if (ProgramID == 43)
            {
                //DataTable dt = FMIS.GetPrograms(43, DateTime.Now.Year);
                //foreach (DataRow item in dt.Rows)
                //{
                //    ooe OOE_desc = new ooe();
                //    OOE_desc.ooe_id = Convert.ToInt32(item["ID"]);
                //    OOE_desc.ooe_name = item["Program"].ToString();

                //    OOETypeList.Add(OOE_desc);
                //}
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"SELECT ProgramID, ProgramDescription FROM tbl_R_BMSOfficePrograms WHERE OfficeID = 43 and ProgramYear = YEAR(GETDATE())+1", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        ooe OOE_desc = new ooe();
                        OOE_desc.ooe_id = Convert.ToInt32(reader.GetValue(0));
                        OOE_desc.ooe_name = reader.GetString(1);
                        OOETypeList.Add(OOE_desc);
                    }
                }
            }
            else
            {
                if (Account.UserInfo.lgu == 1)
                {
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"SELECT * FROM tbl_R_BMSObjectOfExpenditure  order by [OOEID]", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            ooe OOE_desc = new ooe();
                            OOE_desc.ooe_id = reader.GetInt32(0);
                            OOE_desc.ooe_name = reader.GetString(2);

                            OOETypeList.Add(OOE_desc);
                        }
                    }
                }
                else { 
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"SELECT * FROM tbl_R_BMSObjectOfExpenditure  where ooeid < 4", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            ooe OOE_desc = new ooe();
                            OOE_desc.ooe_id = reader.GetInt32(0);
                            OOE_desc.ooe_name = reader.GetString(2);

                            OOETypeList.Add(OOE_desc);
                        }
                    }
                }
            }
            return OOETypeList;
        }
        public IEnumerable<ooe> filtered_ooe_list(int? OfficeID,int? ProgramID, int? ProposalYear)
        {
            List<ooe> OOETypeList = new List<ooe>();
            if (ProgramID == 43 &&  @Account.UserInfo.lgu == 0)
            {
                //DataTable dt = FMIS.GetPrograms(43, DateTime.Now.Year);
                //foreach (DataRow item in dt.Rows)
                //{
                //    ooe OOE_desc = new ooe();
                //    OOE_desc.ooe_id = Convert.ToInt32(item["ID"]);
                //    OOE_desc.ooe_name = item["Program"].ToString();

                //    OOETypeList.Add(OOE_desc);
                //}
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"SELECT ProgramID, ProgramDescription FROM tbl_R_BMSOfficePrograms WHERE OfficeID = 43 and ProgramYear = "+ ProposalYear +" order by [OrderNo],[ProgramDescription]", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        ooe OOE_desc = new ooe();
                        OOE_desc.ooe_id = Convert.ToInt32(reader.GetValue(0));
                        OOE_desc.ooe_name = reader.GetString(1);
                        OOETypeList.Add(OOE_desc);
                    }
                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"SELECT
                                                a.ObjectOfExpendetureID,
                                                b.OOEName
                                                FROM
                                                dbo.tbl_R_BMSProgramAccounts as a 
                                                INNER JOIN dbo.tbl_R_BMSObjectOfExpenditure as b ON a.ObjectOfExpendetureID = b.OOEID
                                                WHERE ProgramID = '" + ProgramID + "' and AccountYear = '" + ProposalYear + "'" +
                                                "UNION " +
                                                "SELECT " +
                                                "a.OOEID, " +
                                                "b.OOEName " +
                                                "FROM " +
                                                "dbo.tbl_R_BMSProposedAccounts as a " +
                                                "INNER JOIN dbo.tbl_R_BMSObjectOfExpenditure as b ON a.OOEID = b.OOEID " +
                                                "WHERE ProgramID = '" + ProgramID + "' and ProposalYear = '" + ProposalYear + "'", con);
                                                //"union "+
                                                //"SELECT 1,'Personal Services' FROM [IFMIS].[dbo].[tbl_R_BMSSubmittedForFundingData] "+
                                                // "where officeid="+ OfficeID  + " and yearof=" + ProposalYear + " " +
                                                // "union "+
                                                // "SELECT 1,'Personal Services' FROM [IFMIS].[dbo].[tbl_R_BMSProposedNewItem] "+
                                                // "where officeid=" + OfficeID + " and yearof=" + ProposalYear + " and actioncode =1
                                                 //", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        ooe OOE_desc = new ooe();
                        OOE_desc.ooe_id = reader.GetInt32(0);
                        OOE_desc.ooe_name = reader.GetString(1);
                        OOETypeList.Add(OOE_desc);
                    }
                }
                //}


            }
            return OOETypeList;
        }
        public IEnumerable<fund> fund_code()
        {
            List<fund> FundList = new List<fund>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("Select * from tbl_R_BMSFunds", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    fund fund_list = new fund();
                    fund_list.fund_id = reader.GetInt32(1);
                    fund_list.fund_name = reader.GetString(2);
                    FundList.Add(fund_list);
                }
            }
            return FundList;
        }
        public IEnumerable<EmployeeType> EmployeeType()
        {
            List<EmployeeType> EmployeeType = new List<EmployeeType>();
            //var path = Path.Combine(Directory.GetCurrentDirectory(), "EmployeeType.txt");
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString()+"TextFiles/EmployeeType.txt";
            string[] lines = System.IO.File.ReadAllLines(path);
            foreach (string line in lines)
            {
                EmployeeType type = new EmployeeType();
                type.NameType = line;
                EmployeeType.Add(type);
            }
            return EmployeeType;
        }
        public IEnumerable<lbp5> ddlPPA_DESC(int OfficeID, int MFO_ID)
        {
            List<lbp5> PPA_DESC = new List<lbp5>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT
                            a.PPA_ID,
                            a.PPA_Description
                            FROM
                            dbo.tbl_R_LBP5_PPA_Denomination as a
                            INNER JOIN dbo.tbl_R_LBP5_PPA_MFO as b ON b.PPA_MFO_ID = a.PPA_MFO_ID
                            WHERE b.OfficeID = '" + OfficeID + "' and b.PPA_MFO_ID = '"+MFO_ID+"' and a.PPA_Description != '' and a.ActionCode = 1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while(reader.Read()){
                    lbp5 ppa_list = new lbp5();
                    ppa_list.PPA_ID = Convert.ToInt32(reader.GetValue(0));
                    ppa_list.PPA_Description = reader.GetString(1);
                    PPA_DESC.Add(ppa_list);
                }

            }
            return PPA_DESC;
        }
        public IEnumerable<account_code> account_code()
        {
            List<account_code> AccodeCodeList = new List<account_code>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("Select * from tbl_R_BMSProgramAccounts", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    account_code code_list = new account_code();
                    code_list.code_id = reader.GetInt32(0);
                    code_list.code_desc = reader.GetString(1);
                    code_list.code = reader.GetString(2);
                    AccodeCodeList.Add(code_list);
                }
            }
            return AccodeCodeList;
        }
        public IEnumerable<AccountsModel> MagnaCartaAccount()
        {
            List<AccountsModel> OfficeList = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT
                            dbo.tbl_R_BMSAccounts.AccountCode,
                            dbo.tbl_R_BMSAccounts.AccountName
                            FROM
                            dbo.tbl_R_BMSAccounts
                            WHERE dbo.tbl_R_BMSAccounts.FMISAccountCode = 316 or dbo.tbl_R_BMSAccounts.FMISAccountCode = 311", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while(reader.Read())
                {
                    AccountsModel account_list = new AccountsModel();
                    account_list.AccountCode = Convert.ToInt32(reader.GetValue(0));
                    account_list.AccountName = reader.GetString(1);
                    OfficeList.Add(account_list); 
                }
            }
            return OfficeList;

        }
        public IEnumerable<account_code> ChildAccountCodelist(string ChildAccountCode)
        {
            List<account_code> AccountCode = new List<account_code>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT DISTINCT dbo.tbl_R_BMSAccounts.AccountName, dbo.tbl_R_BMSAccounts.ChildAccountCode FROM tbl_R_BMSAccounts where Active = 1 and AccountName != '' and ChildAccountCode LIKE '%" + ChildAccountCode + "%' ORDER BY ChildAccountCode ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    account_code code_list = new account_code();
                    code_list.AccountName = reader.GetString(0);
                    code_list.ChildAccountCode = reader.GetString(1);
                    AccountCode.Add(code_list);
                    
                }
            }
            return AccountCode;
        }
        public IEnumerable<account_code> CodeAccountName()
        {
            List<account_code> CodeAccountName = new List<account_code>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT DISTINCT
                            dbo.tbl_R_BMSAccounts.AccountName,
                            dbo.tbl_R_BMSAccounts.FMISAccountCode
                            FROM tbl_R_BMSAccounts
                            WHERE AccountName != '' and FundType = 'General Fund Proper' and Active = 1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while(reader.Read()){
                    account_code account_list = new account_code();
                    account_list.AccountName = reader.GetString(0);
                    account_list.AccountCode = Convert.ToInt32(reader.GetValue(1));
                    CodeAccountName.Add(account_list);
                }
            }
            return CodeAccountName;
        }
        public IEnumerable<level_group> thirdLvlDesc()
        {
            List<level_group> ThirdLevelList = new List<level_group>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("Select * from tbl_R_BMSThirdLevelGroup ORDER BY ThirdLevelDescription", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    level_group level_list = new level_group();
                    level_list.ThirdLevelDescription = reader.GetString(1);
                    ThirdLevelList.Add(level_list);
                }
                return ThirdLevelList;
            }
        }

        public IEnumerable<programs> filter_program_code(string OfficeDropdown)
        {
            List<programs> ProgramList = new List<programs>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Select ProgramID, ProgramDescription from tbl_R_BMSOfficePrograms where 
                                                OfficeID="+OfficeDropdown+" and ProgramYear = YEAR(GETDATE())", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    programs office_list = new programs();
                    office_list.program_id = Convert.ToInt32(reader.GetValue(0));
                    office_list.program_name = reader.GetString(1);
                    ProgramList.Add(office_list);
                }
            }
            return ProgramList;
        }
        public IEnumerable<programs> filter_program_cascade(string OfficeDropdown,int YearOf)
        {
            List<programs> ProgramList = new List<programs>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Select ProgramID, ProgramDescription from tbl_R_BMSOfficePrograms where 
                                                OfficeID=" + OfficeDropdown + " and ProgramYear = " + YearOf + " and ActionCode = 1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    programs office_list = new programs();
                    office_list.program_id = Convert.ToInt32(reader.GetValue(0));
                    office_list.program_name = reader.GetString(1);
                    ProgramList.Add(office_list);
                }
            }
            return ProgramList;
        }

        public IEnumerable<account_code> account_list()
        {
            List<account_code> AccodeCodeList = new List<account_code>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("Select * from tbl_R_BMSProgramAccounts", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    account_code code_list = new account_code();
                    code_list.ref_account_id = reader.GetInt32(0);
                    code_list.account_desc = reader.GetString(1);
                    AccodeCodeList.Add(code_list);
                }
            }
            return AccodeCodeList;
        }
        public IEnumerable<account_code> filter_account()
        {
            List<account_code> AccountsList = new List<account_code>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT DISTINCT AccountName, ChildAccountCode  FROM tbl_R_BMSAccounts where Active = 1 and AccountName != '' ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    account_code account_list = new account_code();
                    account_list.Account_Name = reader.GetString(0);
                    account_list.ChildAccountCode = reader.GetString(1);
                    AccountsList.Add(account_list);

                }
            }
            return AccountsList;
        }
        public IEnumerable<orderby> orderby()
        {
            List<orderby> OrderbyList = new List<orderby>();
            for (var num = 1; num <= 100;num++ )
            {
                orderby orderlist = new orderby();
                orderlist.x = num;
                OrderbyList.Add(orderlist);
            }
            return OrderbyList;
        }
        public IEnumerable<account_code> search_account(string OfficeDropdown, string ProgramDropdown)
        {
            List<account_code> AccodeCodeList = new List<account_code>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT account_id, account_desc FROM sample_account 
                                                    LEFT JOIN ref_account 
                                                    ON sample_account.ref_account_id = ref_account.ref_account_id", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    account_code code_list = new account_code();
                    code_list.ref_account_id = reader.GetInt32(0);
                    code_list.account_desc = reader.GetString(1);
                    AccodeCodeList.Add(code_list);
                }
            }
            return AccodeCodeList;
        }
        public IEnumerable<account_code> search_fund(string AccountDropdown)
        {
            List<account_code> FundCodeList = new List<account_code>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT * FROM sample_fund where fund_id="+AccountDropdown, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    account_code code_list = new account_code();
                    code_list.code_id = reader.GetInt32(0);
                    code_list.code = reader.GetString(1);
                    FundCodeList.Add(code_list);
                }
            }
            return FundCodeList;
        }
        public IEnumerable<fund> search_code(string AccountDropdown)
        {
            List<fund> FundCodeList = new List<fund>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT * FROM sample_fund where fund_id=" + AccountDropdown, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    fund code_list = new fund();
                    code_list.fund_id = reader.GetInt32(0);
                    code_list.fund_name = reader.GetString(1);
                    FundCodeList.Add(code_list);
                }
            }
            return FundCodeList;
        }
        public IEnumerable<dp_PorposalYear_Model> proposal_year()
        {
            List<dp_PorposalYear_Model> proposalyear = new List<dp_PorposalYear_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select distinct  AccountYear from  tbl_R_BMSProgramAccounts where actioncode=1 order by AccountYear desc", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    dp_PorposalYear_Model app = new dp_PorposalYear_Model();

                    app.ProposalYear = reader.GetInt32(0);

                    proposalyear.Add(app);
                }
            }
            return proposalyear;
        }
        public IEnumerable<ooe> ooe_list_noneOfficeLevel()
        {
            List<ooe> OOETypeList = new List<ooe>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("SELECT * FROM tbl_R_BMSObjectOfExpenditure where [OOEID] < 4", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ooe OOE_desc = new ooe();
                    OOE_desc.ooe_id = reader.GetInt32(0);
                    OOE_desc.ooe_name = reader.GetString(2);
                    OOETypeList.Add(OOE_desc);
                }
            }
            return OOETypeList;
        }
        public IEnumerable<ooe> ooe_list_noneOfficeLevelnoco()
        {
            List<ooe> OOETypeList = new List<ooe>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("SELECT * FROM tbl_R_BMSObjectOfExpenditure where [OOEID] < 3", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ooe OOE_desc = new ooe();
                    OOE_desc.ooe_id = reader.GetInt32(0);
                    OOE_desc.ooe_name = reader.GetString(2);
                    OOETypeList.Add(OOE_desc);
                }
            }
            return OOETypeList;
        }
        public IEnumerable<ooe> ExpenseClassLFC(int? OfficeID=0, int? ProgramID=0, int? YearOf=0)
        {
            List<ooe> OOETypeList = new List<ooe>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("sp_BMS_Get_ExpenseClassLFC " + OfficeID + "," + ProgramID + "," + YearOf + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ooe OOE_desc = new ooe();
                    OOE_desc.ooe_id = reader.GetInt32(0);
                    OOE_desc.ooe_name = reader.GetString(1);
                    OOETypeList.Add(OOE_desc);
                }
            }
            return OOETypeList;
        }
        public IEnumerable<ProgramsModel> ProgramsLFC(int? propYear, int? office_ID)
        {
            List<ProgramsModel> pross = new List<ProgramsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select Distinct ProgramID,ProgramDescription from tbl_R_BMSOfficePrograms where ProgramYear = '" + propYear + "' and OfficeID = '" + office_ID + "' and ActionCode=1 order by ProgramDescription", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ProgramsModel app = new ProgramsModel();
                    app.ProgramID = reader.GetValue(0).ToString();
                    app.ProgramDescription = reader.GetString(1);

                    pross.Add(app);
                }
                if (office_ID != 43)
                {
                    ProgramsModel NonOffice = new ProgramsModel();
                    NonOffice.ProgramID = "43";
                    NonOffice.ProgramDescription = "Non-Office";
                    //pross.Add(NonOffice);    
                }
            }
            return pross;
        }
        public IEnumerable<account_code> Accounts(int YearActive)
        {
            List<account_code> AccodeCodeList = new List<account_code>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select distinct AccountCode,AccountName from tbl_R_BMSAccounts  where AccountName != '' and Active = 1 and AccountCode not in(SELECT AccountCode from tbl_R_BMSAccountComputation where yearActive = " + YearActive + ") ORDER BY AccountCode ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    account_code code_list = new account_code();
                    code_list.ref_account_id = reader.GetInt32(0);
                    code_list.account_desc = reader.GetString(1);
                    AccodeCodeList.Add(code_list);
                }
            }
            return AccodeCodeList;
        }
        public IEnumerable<PlantillaModel> ddlPlantillaDivision(int OfficeID)
        {
            OfficeAdmin_Layer OfficeAdmin_Layer = new OfficeAdmin_Layer();
            List<PlantillaModel> DivisionList = new List<PlantillaModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select DivID,DivName from pmis.dbo.edge_tblPlantillaDivision where OfficeID = " + OfficeAdmin_Layer.getPmisOfficeID(OfficeID) +" order by OrderNo", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    PlantillaModel Division = new PlantillaModel();
                    Division.DivID = reader.GetInt32(0);
                    Division.DivName = reader.GetValue(1).ToString();
                    DivisionList.Add(Division);
                }
            }
            return DivisionList;
        }
        public IEnumerable<PlantillaModel> ddlPlantillaPosition(string DivisionID)
        {
            var query = "";
            OfficeAdmin_Layer OfficeAdmin_Layer = new OfficeAdmin_Layer();
            query = @"select a.id,d.Pos_Name from pmis.dbo.EDGE_tblPlantillaYearScheduling as a 
                      inner join pmis.dbo.EDGE_tblPlantilla_ItemNo b on a.Plantilla_ItemNoID=b.ItemNoId
                      inner join pmis.dbo.EDGE_tblPlantillaDivision c on b.DivID=c.DivID
					  inner join pmis.dbo.RefsPositions d on d.PositionCode =b.PosCode
					  WHERE b.divID = " + DivisionID + " and a.YearOf= DATEPART(yyyy, GETDATE()) and a.eid is null and a.id not in(select PlantillaitemID from tbl_R_BMSProposedNewItem where ActionCode = 1)";
            List<PlantillaModel> PositionList = new List<PlantillaModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    PlantillaModel Position = new PlantillaModel();
                    Position.PositionID = reader.GetInt32(0);
                    Position.Position = reader.GetValue(1).ToString();
                    PositionList.Add(Position);
                }
            }
            return PositionList;
        }
        public IEnumerable<ProgramsModel> ddlNewAccountProgram(int OfficeID, int YearOf)
        {
            List<ProgramsModel> ProgramList = new List<ProgramsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select ProgramID, ProgramDescription from tbl_R_BMSOfficePrograms 
                where OfficeID = " + OfficeID + " and ActionCode = 1 and ProgramYear = " + YearOf + " ORDER BY OrderNo", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ProgramsModel Program = new ProgramsModel();
                    Program.ProgramID = reader.GetValue(0).ToString();
                    Program.ProgramDescription = reader.GetValue(1).ToString();
                    ProgramList.Add(Program);
                }
            }
            return ProgramList;
        }
        
        public IEnumerable<PlantillaModel> ddlEmploymentStatus()
        {
            List<PlantillaModel> EmploymentStatusList = new List<PlantillaModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select EmploymentStatus_ID,description from pmis.dbo.refEmploymentStatus where employmentstatus_ID in (1,2)", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    PlantillaModel EmploymentStatus = new PlantillaModel();
                    EmploymentStatus.EmploymentStatusID = reader.GetInt32(0);
                    EmploymentStatus.EmploymentStatus = reader.GetValue(1).ToString();
                    EmploymentStatusList.Add(EmploymentStatus);
                }
            }
            return EmploymentStatusList;
        }
        public IEnumerable<PlantillaModel> CasualRate(int CRYear)
        {
            List<PlantillaModel> CasualrateList = new List<PlantillaModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                //SqlCommand com = new SqlCommand("select CasualrateId,CasualrateName FROM tbl_R_BMSCasualRate where ActionCode = 1 and BudgetYear = " + (DateTime.Now.Year + 1) + "", con);
                SqlCommand com = new SqlCommand("select CasualrateId,CasualrateName FROM tbl_R_BMSCasualRate where ActionCode = 1 and BudgetYear = " + CRYear + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    PlantillaModel CasualRate = new PlantillaModel();
                    CasualRate.EmploymentStatusID = reader.GetInt32(0);
                    CasualRate.EmploymentStatus = reader.GetString(1);
                    CasualrateList.Add(CasualRate);
                }
            }
            return CasualrateList;
        }


        public IEnumerable<UserDetails_Model> userOffice()
        {
            List<UserDetails_Model> OfficeList = new List<UserDetails_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select a.UserID,a.IFMISOfficeID, e.OfficeName from [dbo].[tbl_R_BMSUsers] as a LEFT JOIN [pmis].[dbo].[vwLoginParameter] as b ON a.eid = b.eid "+
																				"LEFT JOIN [pmis].[dbo].[vwMergeAllEmployee] as c on a.eid = c.eid  " +
																				"LEFT JOIN [dbo].[tbl_R_BMSUserTypes] as d on a.UserTypeID = d.UserTypeID  " +
																				"INNER JOIN [dbo].[tbl_R_BMSOffices] as e on a.IFMISOfficeID = e.OfficeID", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    UserDetails_Model User = new UserDetails_Model();
                    User.UserID = reader.GetInt32(0);
                    User.IFMISOfficeID = reader.GetInt32(1);
                    User.OfficeName = reader.GetString(2);
                    OfficeList.Add(User);
                }
            }
            return OfficeList;
        }


        public IEnumerable<Classes_Model> classes()
        {
            List<Classes_Model> clases = new List<Classes_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("SELECT FundFlag,Class_Type FROM IFMIS.dbo.tbl_R_BMS_A_Class   order by Class_ID", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Classes_Model app = new Classes_Model();

                    app.Class_ID = reader.GetInt32(0);
                    app.Class_Type = reader.GetString(1);

                    clases.Add(app);
                }
            }
            return clases;
        }
        public IEnumerable<Classes_Model> classesARO()
        {
            List<Classes_Model> clases = new List<Classes_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("SELECT FundFlag,Class_Type FROM IFMIS.dbo.tbl_R_BMS_A_Class  where FundFlag <> 0  order by Class_ID", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Classes_Model app = new Classes_Model();

                    app.Class_ID = reader.GetInt32(0);
                    app.Class_Type = reader.GetString(1);

                    clases.Add(app);
                }
            }
            return clases;
        }
        public IEnumerable<BudgetControlModel> transactionType()
        {
            List<BudgetControlModel> data = new List<BudgetControlModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("SELECT * from tbl_R_BMSTransType", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while(reader.Read()){
                    BudgetControlModel value = new BudgetControlModel();
                    value.TransTypeID = Convert.ToInt32(reader.GetValue(0));
                    value.TransTypeName = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
                return data;
            }
        }
        public IEnumerable<BudgetControlModel> ModeOfExpense(int? TransTypeID)
        {
            List<BudgetControlModel> data = new List<BudgetControlModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT ModeOfExpenseID, ModeOfExpenseName FROM tbl_R_BMSModeOfExpense Where TransTypeID = '" + TransTypeID + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while(reader.Read()){
                    BudgetControlModel value = new BudgetControlModel();
                    value.ModeOfExpenseID = Convert.ToInt32(reader.GetValue(0));
                    value.ModeOfExpenseName = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<BudgetControlModel> FundType()
        {
            List<BudgetControlModel> data = new List<BudgetControlModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT FundCode, FundMedium FROM tbl_R_BMSFunds order by FundID", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while(reader.Read()){
                    BudgetControlModel value = new BudgetControlModel();
                    value.FundTypeID = Convert.ToInt32(reader.GetValue(0));
                    value.FundTypeName = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<BudgetControlModel> checkChecked(int? OfficeID, int? Program, int? TransactionYear)
        {
            List<BudgetControlModel> data = new List<BudgetControlModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                    SqlCommand com = new SqlCommand(@"SELECT a.AccountID, a.AccountName , a.ObjectOfExpendetureID
                            FROM tbl_R_BMSProgramAccounts as a 
                          --  LEFT JOIN tbl_R_BMSOfficePrograms as b ON b.ProgramID = a.ProgramID
                          --  LEFT JOIN tbl_R_BMSAccounts as c ON c.AccountID = a.AccountID
                            WHERE a.ProgramID = '" + Program + "' and a.AccountYear = " + TransactionYear + " " +
                            "and a.ActionCode = 1 and a.AccountID != '' " +
                            "ORDER BY a.AccountID", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        BudgetControlModel value = new BudgetControlModel();
                        value.item_ID = Convert.ToInt32(reader.GetValue(0));
                        value.item_data = Convert.ToString(reader.GetValue(1));
                        data.Add(value);
                    }   
            }
            return data;
        }
        public IEnumerable<BudgetControlModel> TEVControlNo(string ControlNo, int? trnnoID)
        {
            List<BudgetControlModel> data = new List<BudgetControlModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec dbo.sp_SearchTravelOrder '"+ControlNo+"' ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while(reader.Read()){
                    BudgetControlModel value = new BudgetControlModel();
                    value.eid = Convert.ToInt32(reader.GetValue(0));
                    value.Employee_Name = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
            }
            return data;
        }
        public string changeExpense(string item, int? param, int? Program, int? TransactionYear)
        {
            var OOEID = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT a.ObjectOfExpendetureID
                            FROM tbl_R_BMSProgramAccounts as a 
                            LEFT JOIN tbl_R_BMSOfficePrograms as b ON b.ProgramID = a.ProgramID
                            LEFT JOIN tbl_R_BMSAccounts as c ON c.AccountID = a.AccountID
                            WHERE a.ProgramID = '" + Program + "' and a.AccountYear =" + TransactionYear + " " +
                            "and b.ActionCode = 1 and b.ProgramYear =" + TransactionYear + " and a.ActionCode = 1 and a.AccountID ='" + item + "' " +
                            "ORDER BY c.AccountCode", con);
                con.Open();
                OOEID = Convert.ToString(com.ExecuteScalar());
               // while()
            }
            return OOEID;
        }
        public IEnumerable<BudgetControlModel> searchAccounts(int? ProgramID, int? OOE, int? TYear)
        {
            List<BudgetControlModel> data = new List<BudgetControlModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                //SqlCommand com = new SqlCommand(@"SELECT a.AccountID, a.AccountName , a.ObjectOfExpendetureID
                //        FROM tbl_R_BMSProgramAccounts as a 
                //        LEFT JOIN tbl_R_BMSOfficePrograms as b ON b.ProgramID = a.ProgramID
                //        LEFT JOIN tbl_R_BMSAccounts as c ON c.AccountID = a.AccountID
                //        WHERE a.ProgramID = '" + ProgramID + "' and a.AccountYear = YEAR(GETDATE()) " +
                //        "and b.ActionCode = 1 and b.ProgramYear = YEAR(GETDATE()) and a.ActionCode = 1 and a.AccountID != '' " +
                //        "ORDER BY a.AccountID, a.ObjectOfExpendetureID", con);
                SqlCommand com = new SqlCommand(@"SELECT a.AccountID, a.AccountName , a.ObjectOfExpendetureID
                            FROM tbl_R_BMSProgramAccounts as a 
                            LEFT JOIN tbl_R_BMSOfficePrograms as b ON b.ProgramID = a.ProgramID
                            LEFT JOIN tbl_R_BMSAccounts as c ON c.AccountID = a.AccountID
                            WHERE a.ProgramID = '" + ProgramID + "' and a.AccountYear = "+ TYear + " "+
                        "and b.ActionCode = 1 and b.ProgramYear = "+ TYear + " and a.ActionCode = 1 and a.AccountID != '' " +
                        "ORDER BY a.AccountID, a.ObjectOfExpendetureID", con);
                con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        BudgetControlModel value = new BudgetControlModel();
                        value.AccountID = Convert.ToInt32(reader.GetValue(0));
                        value.AccountName = Convert.ToString(reader.GetValue(1));
                        data.Add(value);
                    }
            }
            return data;
        }
        public IEnumerable<BudgetControlModel> searchAccountCharged(int? ProgramID, int? OOE, int? trnnoID, int? ModeIndicator, string OBRNo)
        {
            List<BudgetControlModel> data = new List<BudgetControlModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var query = "";
                if (ModeIndicator == 0)
                {
                    query = "SELECT a.AccountID, a.AccountName , a.ObjectOfExpendetureID " +
                            "FROM tbl_R_BMSProgramAccounts as a " +
                            "LEFT JOIN tbl_R_BMSOfficePrograms as b ON b.ProgramID = a.ProgramID " +
                            "LEFT JOIN tbl_R_BMSAccounts as c ON c.AccountID = a.AccountID " +
                            "WHERE a.ProgramID = '" + ProgramID + "' and a.AccountYear = YEAR(GETDATE()) " +
                            "and b.ActionCode = 1 and b.ProgramYear = YEAR(GETDATE()) and a.ActionCode = 1 and a.AccountID != '' " +
                            "and a.AccountID NOT IN (SELECT d.Budget_AcctCharge FROM tbl_T_BMSSubsidiaryLedger as d LEFT JOIN tbl_R_BMSObrLogs as e ON d.OBRNo = e.OBRNo WHERE (e.trnno = RIGHT('" + OBRNo + "', 5) or e.OBRNo = '" + OBRNo + "')) " +
                            "ORDER BY a.AccountID, a.ObjectOfExpendetureID";
                }
                else if(ModeIndicator == 1)
                {
                    query = "SELECT a.AccountID, a.AccountName , a.ObjectOfExpendetureID " +
                           "FROM tbl_R_BMSProgramAccounts as a " +
                           "LEFT JOIN tbl_R_BMSOfficePrograms as b ON b.ProgramID = a.ProgramID " +
                           "LEFT JOIN tbl_R_BMSAccounts as c ON c.AccountID = a.AccountID " +
                           "WHERE a.ProgramID = '" + ProgramID + "' and a.AccountYear = YEAR(GETDATE()) " +
                           "and b.ActionCode = 1 and b.ProgramYear = YEAR(GETDATE()) and a.ActionCode = 1 and a.AccountID != '' " +
                           "and a.AccountID NOT IN (SELECT d.Budget_AcctCharge FROM tbl_T_BMSSubsidiaryLedger as d LEFT JOIN tbl_R_BMSObrLogs as e ON d.OBRNo = e.OBRNo WHERE (e.trnno = RIGHT('" + OBRNo + "', 5) or e.OBRNo = '" + OBRNo + "') and d.trnno != '" + trnnoID + "' ) " +
                           "ORDER BY a.AccountID, a.ObjectOfExpendetureID";
                }
                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    BudgetControlModel value = new BudgetControlModel();
                    value.AccountID = Convert.ToInt32(reader.GetValue(0));
                    value.AccountName = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<OrdinanceSectionModel> ddlSections()
        {
            List<OrdinanceSectionModel> SectionList = new List<OrdinanceSectionModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select SectionID, SectionName from tbl_R_BMSOrdinanceSection where YearOf = year(getdate()) and ActionCode = 1 order by SectionOrder", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    OrdinanceSectionModel Section = new OrdinanceSectionModel();

                    Section.SectionID = Convert.ToInt32(reader.GetValue(0));
                    Section.SectionName = reader.GetValue(1).ToString();

                    SectionList.Add(Section);
                }
            }
            return SectionList;
        }


        public IEnumerable<OBRLogger> TransactionYearsExcess()
        {
            List<OBRLogger> data = new List<OBRLogger>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"SELECT DISTINCT YearOf FROM dbo.fn_BMS_ViewExcessAppropriations() ORDER BY YearOf DESC", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    OBRLogger value = new OBRLogger();
                    value.YearOf = Convert.ToInt32(reader.GetValue(0));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<OBRLogger> TransactionYears()
        {
            List<OBRLogger> data = new List<OBRLogger>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"SELECT DISTINCT a.YearOf as YearOf FROM tbl_R_BMS_Release as a ORDER BY YearOf DESC", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    OBRLogger value = new OBRLogger();
                    value.YearOf = Convert.ToInt32(reader.GetValue(0));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<PPSASCode_Model> PPSASCode()
        {
            List<PPSASCode_Model> data = new List<PPSASCode_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"SELECT DISTINCT a.AccountYear 
                    FROM tbl_R_BMSProgramAccounts as a where actioncode=1
                    ORDER BY a.AccountYear DESC", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    PPSASCode_Model value = new PPSASCode_Model();
                    value.Years = Convert.ToInt32(reader.GetValue(0));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<PPSASCode_Model> NonOfficeCodeYears()
        {
            List<PPSASCode_Model> data = new List<PPSASCode_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"SELECT DISTINCT a.YearOf FROM dbo.fn_BMS_MemisPPA() as a where len(YearOf)=4 ORDER BY a.YearOf DESC", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    PPSASCode_Model value = new PPSASCode_Model();
                    value.Years = Convert.ToInt32(reader.GetValue(0));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<PPSASCode_Model> PPSASAccounts(int? Year)
        {
            List<PPSASCode_Model> data = new List<PPSASCode_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"SELECT * FROM dbo.fn_BMS_DropdownPPSASAccounts("+Year+") ORDER BY Particular", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    PPSASCode_Model value = new PPSASCode_Model();
                    value.PPSASCode = Convert.ToString(reader.GetValue(0));
                    value.AccountName = Convert.ToString(reader.GetValue(1)); 
                    data.Add(value);
                }
            }
            return data;
        }
        
        public IEnumerable<OBRLogger> TransactionExcessYears()
        {
            List<OBRLogger> data = new List<OBRLogger>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                //SqlCommand query = new SqlCommand(@"SELECT DISTINCT TOP 1 YearOf-1 as YearOf
                //        FROM tbl_T_BMSExcessAppropriation

                //        UNION ALL

                //        SELECT DISTINCT YearOf+1
                //        FROM tbl_T_BMSExcessAppropriation
                //        ORDER BY YearOf DESC", con);
                SqlCommand query = new SqlCommand(@"exec sp_BMS_ExcessYear", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    OBRLogger value = new OBRLogger();
                    value.YearOf = Convert.ToInt32(reader.GetValue(0));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<AccountsModel> getProgramAccount(int? ProgramID, int? TransactionYear)
        {
            List<AccountsModel> data = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_ProgramAccounts "+ProgramID+", "+TransactionYear+"", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel value = new AccountsModel();
                    value.AccountID = Convert.ToInt32(reader.GetValue(0));
                    value.AccountName = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<AccountsModel> ExcessAppropriations(int? FundType, int? Years)
        {
            List<AccountsModel> data = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_ViewExcessAppropriations "+FundType+", 1, 0, "+Years+"", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel value = new AccountsModel();
                    value.AccountID = Convert.ToInt32(reader.GetValue(0));
                    value.AccountName = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<ExcessModel> PPAAccounts(int? TransactionYear, int? AccountID)
        {
            List<ExcessModel> data = new List<ExcessModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_ViewMemisPPA " + TransactionYear + ", " + AccountID + "", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    ExcessModel value = new ExcessModel();
                    value.PPAID = Convert.ToInt32(reader.GetValue(0));
                    value.PPAAccount = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<ExcessModel> RootPPA(int? Year)
        {
            List<ExcessModel> data = new List<ExcessModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_ViewMemisRootPPA " + Year + ", 0, 1", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    ExcessModel value = new ExcessModel();
                    value.PPAID = Convert.ToInt32(reader.GetValue(0));
                    value.PPAAccount = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<ExcessModel> SubPPAAccounts(int? Year, int? Account)
        {
            List<ExcessModel> data = new List<ExcessModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_ViewMemisRootPPA " + Year + ", " + Account + ", 2", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    ExcessModel value = new ExcessModel();
                    value.PPAID = Convert.ToInt32(reader.GetValue(0));
                    value.PPAAccount = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<office> PPANonOffice(int? ProgramID=0,int? AccountID=0, int? Year=0,int? Excessid=0)
        {
            List<office> data = new List<office>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var query_string = "";
                query_string = @"dbo.sp_BMS_DropdownNonOffice " + ProgramID + "," + AccountID + ", " + Year + ","+ Excessid + "";
                SqlCommand query = new SqlCommand(query_string, con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    office office_list = new office();
                    office_list.office_id = reader.GetInt32(0);
                    office_list.office_name = reader.GetString(1);
                    data.Add(office_list);
                }
            }
            return data;
        }
        public IEnumerable<office> PPANonOfficeExcess(int? ProgramID = 0, int? AccountID = 0, int? Year = 0,int? excessid=0)
        {
            List<office> data = new List<office>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var query_string = "";
                query_string = @"exec sp_BMS_DropdownNonOfficeExcess "+ ProgramID + ","+ AccountID + ","+ Year + ","+ excessid + "";
                SqlCommand query = new SqlCommand(query_string, con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    office office_list = new office();
                    office_list.office_id = reader.GetInt64(0);
                    office_list.office_name = reader.GetString(1);
                    data.Add(office_list);
                }
            }
            return data;
        }
        public IEnumerable<BudgetControlModel> MainPPA(int? TransactionYear)
        {
            List<BudgetControlModel> data = new List<BudgetControlModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_DropdownPPA " + TransactionYear + ", 2861, 3", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while(reader.Read())
                {
                    BudgetControlModel value = new BudgetControlModel();
                    value.RootPPA = Convert.ToInt32(reader.GetValue(0));
                    value.PPADescription = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<BudgetControlModel> PPAs(int? TransactionYear, int? AccountID)
        {
            List<BudgetControlModel> data = new List<BudgetControlModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_DropdownPPA " + TransactionYear + ", " + AccountID + ", 4", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    BudgetControlModel value = new BudgetControlModel();
                    value.PPA = Convert.ToInt32(reader.GetValue(0));
                    value.PPADescription = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
            }
            return data;
        }


        public IEnumerable<Monthly_DD_Model> MOS_accounts(int? propYear,int ooe_id, int? prog_id,int? vwAllAccountsid = 0)
        {
            List<Monthly_DD_Model> account_list = new List<Monthly_DD_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (vwAllAccountsid == 1)
                {
                    SqlCommand com = new SqlCommand(@"SELECT a.AccountID,a.AccountName FROM IFMIS.dbo.tbl_R_BMSProgramAccounts as a inner join 
                                                 [IFMIS].[dbo].[tbl_T_BMSBudgetProposal] as b on a.programid=b.[ProgramID] and a.[AccountID]=b.[AccountID] and accountyear=proposalyear 
                                                   where a.AccountYear = '" + propYear + "' and a.ProgramID = '" + prog_id + "' and [ObjectOfExpendetureID]=" + ooe_id + " and actioncode = 1 and proposalactioncode=1 and [ProposalStatusCommittee]=1 order by [ObjectOfExpendetureID],isnull([OrderNo],9999)", con);

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Monthly_DD_Model app = new Monthly_DD_Model();

                        app.account_id = reader.GetInt32(0);
                        app.account_name = reader.GetValue(1).ToString();

                        account_list.Add(app);
                    }
                }
                else
                {
                    SqlCommand com = new SqlCommand(@"SELECT a.AccountID,a.AccountName FROM IFMIS.dbo.tbl_R_BMSProgramAccounts as a inner join 
                                            [IFMIS].[dbo].[tbl_T_BMSBudgetProposal] as b on a.programid=b.[ProgramID] and a.[AccountID]=b.[AccountID] and accountyear=proposalyear 
                                            where a.AccountYear = '" + propYear + "' and a.ProgramID = '" + prog_id + "' and [ObjectOfExpendetureID]=" + ooe_id + " and actioncode = 1 and proposalactioncode=1 and proposalallotedamount <> 0 and [ProposalStatusCommittee]=1 order by [ObjectOfExpendetureID],isnull([OrderNo],9999)", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Monthly_DD_Model app = new Monthly_DD_Model();

                        app.account_id = reader.GetInt32(0);
                        app.account_name = reader.GetValue(1).ToString();

                        account_list.Add(app);
                    }
                }
            }
            return account_list;
        }

        public IEnumerable<Monthly_DD_Model> MOS_AccountNoEXP(int? propYear, int? prog_id)
        {
            List<Monthly_DD_Model> account_list = new List<Monthly_DD_Model>();
            if (propYear >= 2017)
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"SELECT a.AccountID,a.AccountName FROM IFMIS.dbo.tbl_R_BMSProgramAccounts as a inner join 
                                                 [IFMIS].[dbo].[tbl_T_BMSBudgetProposal] as b on a.programid=b.[ProgramID] and a.[AccountID]=b.[AccountID] and accountyear=proposalyear
                                                  where a.AccountYear = '" + propYear + "' and a.ProgramID = '" + prog_id + "'  and actioncode = 1 and proposalactioncode=1 and [ProposalStatusCommittee]=1  order by a.AccountName,isnull([OrderNo],9999)", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Monthly_DD_Model app = new Monthly_DD_Model();

                        app.account_id = reader.GetInt32(0);
                        app.account_name = reader.GetValue(1).ToString();

                        account_list.Add(app);
                    }
                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"SELECT [FMISAccountCode] as AccountID,[BudgetAcctName] as AccountName FROM [fmis].[dbo].[tblBMS_AnnualBudget_Account] as a where [YearOf] = '" + propYear + "' and a.[FMISProgramCode] = '" + prog_id + "'  and actioncode = 1 order by a.[BudgetAcctName]", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Monthly_DD_Model app = new Monthly_DD_Model();

                        app.account_id = reader.GetInt32(0);
                        app.account_name = reader.GetValue(1).ToString();

                        account_list.Add(app);
                    }
                }
            }
            return account_list;
        }


        public IEnumerable<Monthly_DD_Model> Account_Mos_From(int? office_ID_from, int? prog_id_from, int? ooe_id_from, int? year_, int? office_ID_to, int? prog_id_to, int? ooe_id_to, int? account_id_to)
        {
            List<Monthly_DD_Model> account_list = new List<Monthly_DD_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_MonthlyRelease_Accounts '" + office_ID_from + "', '" + prog_id_from + "', '" + ooe_id_from + "', '" + year_ + "', '" + office_ID_to + "', '" + prog_id_to + "', '" + ooe_id_to + "', '" + account_id_to + "' ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_DD_Model app = new Monthly_DD_Model();

                    app.account_id = reader.GetInt32(0);
                    app.account_name = reader.GetValue(1).ToString();

                    account_list.Add(app);
                }
            }
            return account_list;
        }
        public IEnumerable<Monthly_DD_Model> Account_Mos_TO(int? office_ID_from, int? prog_id_from, int? ooe_id_from, int? account_id_from, int? year_, int? office_ID_to, int? prog_id_to, int? ooe_id_to)
        {
            List<Monthly_DD_Model> account_list = new List<Monthly_DD_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_MonthlyRelease_Accounts_To '" + office_ID_from + "', '" + prog_id_from + "', '" + ooe_id_from + "', '" + account_id_from + "', '" + year_ + "', '" + office_ID_to + "', '" + prog_id_to + "', '" + ooe_id_to + "' ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_DD_Model app = new Monthly_DD_Model();

                    app.account_id = reader.GetInt32(0);
                    app.account_name = reader.GetValue(1).ToString();

                    account_list.Add(app);
                }
            }
            return account_list;
        } 



        public IEnumerable<ObjectOfExpendetureModel> dp_OOE()
        {
            List<ObjectOfExpendetureModel> ObjectOfExpendetureModelList = new List<ObjectOfExpendetureModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("SELECT OOEID,OOEName FROM IFMIS.dbo.tbl_R_BMSObjectOfExpenditure", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ObjectOfExpendetureModel ObjectOfExpendetureModel_list = new ObjectOfExpendetureModel();
                    ObjectOfExpendetureModel_list.OOEID = reader.GetInt32(0);
                    ObjectOfExpendetureModel_list.OOEName = reader.GetString(1);
                    ObjectOfExpendetureModelList.Add(ObjectOfExpendetureModel_list);
                }
            }
            return ObjectOfExpendetureModelList;
        }

        #region Execution Utilities desu
        public IEnumerable<ChangeTransactionCharge> OfficeControlFrom(int? trnno, int? Year, int? FundType)
        {
            List<ChangeTransactionCharge> data = new List<ChangeTransactionCharge>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_ExecutionUtilities_DropDown "+trnno+", 1, "+Year+", 1", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    ChangeTransactionCharge value = new ChangeTransactionCharge();
                    value.OfficeID = Convert.ToInt32(reader.GetValue(0));
                    value.OfficeName = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<ChangeTransactionCharge> OfficeControlFrom_Excess(int? trnno, int? YearOF, int? FundType)
        {
            List<ChangeTransactionCharge> data = new List<ChangeTransactionCharge>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_ExecutionUtilities_DropDown_Excess " + trnno + ", 1, " + YearOF + ", 1", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    ChangeTransactionCharge value = new ChangeTransactionCharge();
                    value.OfficeID = Convert.ToInt32(reader.GetValue(0));
                    value.OfficeName = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<ChangeTransactionCharge> AcctCharge_Excess(int? trnno, int? YearOF, int? FundType)
        {
            List<ChangeTransactionCharge> data = new List<ChangeTransactionCharge>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_ExecutionUtilities_DropDown_Excess " + trnno + ", 2, " + YearOF + ", 0", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    ChangeTransactionCharge value = new ChangeTransactionCharge();
                    value.AccountID = Convert.ToInt32(reader.GetValue(0));
                    value.AccountName = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<ChangeTransactionCharge> ProgramsFrom(int? trnno, int? Year, int? FundType)
        {
            List<ChangeTransactionCharge> data = new List<ChangeTransactionCharge>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_ExecutionUtilities_DropDown " + trnno + ", 2, " + Year + ", 1", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    ChangeTransactionCharge value = new ChangeTransactionCharge();
                    value.ProgramID = Convert.ToInt32(reader.GetValue(0));
                    value.ProgramName = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<ChangeTransactionCharge> ProgramsFrom_Excess(int? trnno, int? YearOF, int? FundType)
        {
            List<ChangeTransactionCharge> data = new List<ChangeTransactionCharge>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_ExecutionUtilities_DropDown_Excess " + trnno + ", 4, " + YearOF + ", 1", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    ChangeTransactionCharge value = new ChangeTransactionCharge();
                    value.ProgramID = Convert.ToInt32(reader.GetValue(0));
                    value.ProgramName = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<ChangeTransactionCharge> AccountFrom_Excess(int? trnno, int? YearOF, int? FundType)
        {
            List<ChangeTransactionCharge> data = new List<ChangeTransactionCharge>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_ExecutionUtilities_DropDown_Excess " + trnno + ", 4, " + YearOF + ", 1", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    ChangeTransactionCharge value = new ChangeTransactionCharge();
                    value.ProgramID = Convert.ToInt32(reader.GetValue(0));
                    value.ProgramName = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<ChangeTransactionCharge> ObjOfExpenditureFrom(int? trnno, int? Year, int? FundType)
        {
            List<ChangeTransactionCharge> data = new List<ChangeTransactionCharge>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_ExecutionUtilities_DropDown " + trnno + ", 3 , " + Year + ", 1 ", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    ChangeTransactionCharge value = new ChangeTransactionCharge();
                    value.AccountID = Convert.ToInt32(reader.GetValue(0));
                    value.AccountName = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<ChangeTransactionCharge> OfficeControlByFund(int? FundType, int? YearOF)
        {
            List<ChangeTransactionCharge> data = new List<ChangeTransactionCharge>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_ExecutionUtilities_DropdownByFund " + FundType + ", 0 , 1, " + YearOF + "", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    ChangeTransactionCharge value = new ChangeTransactionCharge();
                    value.OfficeID = Convert.ToInt32(reader.GetValue(0));
                    value.OfficeName = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<ChangeTransactionCharge> OfficeControlByFund_Excess(int? FundType, int? YearOF)
        {
            List<ChangeTransactionCharge> data = new List<ChangeTransactionCharge>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_ExecutionUtilities_DropDownByFund_Excess " + FundType + ", 0 , 1, " + YearOF + "", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    ChangeTransactionCharge value = new ChangeTransactionCharge();
                    value.OfficeID = Convert.ToInt32(reader.GetValue(0));
                    value.OfficeName = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<ChangeTransactionCharge> ProgramByFund_Excess(int? OfficeID, int? YearOF)
        {
            List<ChangeTransactionCharge> data = new List<ChangeTransactionCharge>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_ExecutionUtilities_DropDownByFund_Excess 0, " + OfficeID + ", 4," + YearOF + "", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    ChangeTransactionCharge value = new ChangeTransactionCharge();
                    value.ProgramID = Convert.ToInt32(reader.GetValue(0));
                    value.ProgramName = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<ChangeTransactionCharge> AcctByFund_Excess(int? ProgramID, int? YearOF)
        {
            List<ChangeTransactionCharge> data = new List<ChangeTransactionCharge>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_ExecutionUtilities_DropDownByFund_Excess 0, " + ProgramID + ", 5," + YearOF + "", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    ChangeTransactionCharge value = new ChangeTransactionCharge();
                    value.AccountID = Convert.ToInt32(reader.GetValue(0));
                    value.AccountName = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<ChangeTransactionCharge> AcctChargeByFund_Excess(int? FundType, int? YearOF)
        {
            List<ChangeTransactionCharge> data = new List<ChangeTransactionCharge>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_ExecutionUtilities_DropDownByFund_Excess " + FundType + ", 0 , 2, " + YearOF + "", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    ChangeTransactionCharge value = new ChangeTransactionCharge();
                    value.AccountID = Convert.ToInt32(reader.GetValue(0));
                    value.AccountName = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<ChangeTransactionCharge> ProgramsByFund(int? FundType, int? OfficeID, int? YearOF)
        {
            List<ChangeTransactionCharge> data = new List<ChangeTransactionCharge>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_ExecutionUtilities_DropdownByFund " + FundType + "," + OfficeID + ", 2, " + YearOF + "", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    ChangeTransactionCharge value = new ChangeTransactionCharge();
                    value.ProgramID = Convert.ToInt32(reader.GetValue(0));
                    value.ProgramName = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
            }
            return data;
        }
        public IEnumerable<ChangeTransactionCharge> CheckedByFund(int? FundType, int? ProgramID, int? YearOF)
        {
            List<ChangeTransactionCharge> data = new List<ChangeTransactionCharge>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand query = new SqlCommand(@"dbo.sp_BMS_ExecutionUtilities_DropdownByFund " + FundType + ", " + ProgramID + ", 3, " + YearOF + "", con);
                con.Open();
                SqlDataReader reader = query.ExecuteReader();
                while (reader.Read())
                {
                    ChangeTransactionCharge value = new ChangeTransactionCharge();
                    value.AccountID = Convert.ToInt32(reader.GetValue(0));
                    value.AccountName = Convert.ToString(reader.GetValue(1));
                    data.Add(value);
                }
            }
            return data;
        }

        public IEnumerable<Monthly_DD_Model> MOS_AllAccount(int? propYear)
        {
            List<Monthly_DD_Model> account_list = new List<Monthly_DD_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"SELECT distinct a.AccountID,a.AccountName FROM IFMIS.dbo.tbl_R_BMSProgramAccounts as a inner join 
                                            [IFMIS].[dbo].[tbl_T_BMSBudgetProposal] as b on a.programid=b.[ProgramID] and a.[AccountID]=b.[AccountID] and accountyear=proposalyear 
                                            where a.AccountYear = '" + propYear + "' and [ObjectOfExpendetureID]=2 and actioncode = 1 and proposalactioncode=1 and proposalallotedamount <> 0 order by [ObjectOfExpendetureID],isnull([OrderNo],9999)", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_DD_Model app = new Monthly_DD_Model();

                    app.account_id = reader.GetInt32(0);
                    app.account_name = reader.GetValue(1).ToString();

                    account_list.Add(app);
                }

            }
          
            return account_list;
        }
        #endregion
        public IEnumerable<dp_PorposalYear_Model> proposal_yearnew()
        {
            List<dp_PorposalYear_Model> proposalyear = new List<dp_PorposalYear_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select  [ProposalYear] from  tbl_R_BMSProposalYear order by [ProposalYear] desc", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    dp_PorposalYear_Model app = new dp_PorposalYear_Model();

                    app.ProposalYear = reader.GetInt32(0);

                    proposalyear.Add(app);
                }
            }
            return proposalyear;
        }
        public IEnumerable<Monthly_DD_Model> MOS_accountsNoMOOE(int? propYear, int? prog_id)
        {
            List<Monthly_DD_Model> account_list = new List<Monthly_DD_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
               
                    SqlCommand com = new SqlCommand(@"SELECT a.AccountID,a.AccountName FROM IFMIS.dbo.tbl_R_BMSProgramAccounts as a inner join 
                                            [IFMIS].[dbo].[tbl_T_BMSBudgetProposal] as b on a.programid=b.[ProgramID] and a.[AccountID]=b.[AccountID] and accountyear=proposalyear 
                                            where a.AccountYear = '" + propYear + "' and a.ProgramID = '" + prog_id + "' and actioncode = 1 and proposalactioncode=1 and [ProposalStatusCommittee] =1 order by [ObjectOfExpendetureID],isnull([OrderNo],9999)", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Monthly_DD_Model app = new Monthly_DD_Model();

                        app.account_id = reader.GetInt32(0);
                        app.account_name = reader.GetValue(1).ToString();

                        account_list.Add(app);
                    }
                
            }
            return account_list;
        }
        public IEnumerable<office> office_code20()
        {
            List<office> OfficeList = new List<office>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select OfficeID, CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') from tbl_R_BMSOffices where officeid <> 43 ORDER BY id", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    office office_list = new office();
                    office_list.office_id = reader.GetInt32(0);
                    office_list.office_name = reader.GetString(1);
                    OfficeList.Add(office_list);
                }
            }
            return OfficeList;
        }
        public IEnumerable<AccountsModel> getAccounts(int? programID, int? TrasactionYear)
        {
            List<AccountsModel> ProgramList = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec sp_BMS_ProposedExistAccounts '" + programID + "' , '" + TrasactionYear + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel program_list = new AccountsModel();
                    program_list.setAccountID = Convert.ToInt32(reader.GetValue(0));
                    program_list.setAccountname = reader.GetString(1);
                    ProgramList.Add(program_list);
                }

            }
            return ProgramList;
        }
        public IEnumerable<AccountsModel> getAccountsLink(int? programID, int? TrasactionYear)
        {
            List<AccountsModel> ProgramList = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT DISTINCT [AccountID],  isnull([AccountName],'')  as AccountName FROM [IFMIS].[dbo].[tbl_R_BMSProgramAccounts] WHERE [ProgramID]=" + programID + " and [AccountYear] = " + TrasactionYear + " and actioncode=1  order by AccountName", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel program_list = new AccountsModel();
                    program_list.setAccountID = Convert.ToInt32(reader.GetValue(0));
                    program_list.setAccountname = reader.GetString(1);
                    ProgramList.Add(program_list);
                }

            }
            return ProgramList;
        }
        public IEnumerable<AccountsModel> ddlgetProgramsPastyear(int? OfficeID, int? TrasactionYear)
        {
            List<AccountsModel> ProgramList = new List<AccountsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT DISTINCT ProgramID, ProgramDescription FROM tbl_R_BMSOfficePrograms WHERE OfficeID='" + OfficeID + "' and ProgramYear = '" + TrasactionYear + "' and actioncode=1  ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AccountsModel program_list = new AccountsModel();
                    program_list.setProgramID = Convert.ToInt32(reader.GetValue(0));
                    program_list.setProgramDesc = reader.GetString(1);
                    ProgramList.Add(program_list);
                }

            }
            return ProgramList;
        }
    }
}