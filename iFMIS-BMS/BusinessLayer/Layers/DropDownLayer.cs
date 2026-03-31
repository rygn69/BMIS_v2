using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers
{
    public class DropDownLayer
    {

        public IEnumerable<PMISUserModel> Users()
        {
            List<PMISUserModel> UserList = new List<PMISUserModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec sp_BMS_EmployeeNationalOffice "+ Account.UserInfo.eid +"", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    PMISUserModel usr = new PMISUserModel();
                    usr.eid = reader.GetInt64(0);
                    usr.empName = reader.GetValue(1).ToString();

                    UserList.Add(usr);
                }
            }
            return UserList;
        }
        public IEnumerable<PMISUserModel> ddlEmployeeForOrdinanceAttendance()
        {
            List<PMISUserModel> UserList = new List<PMISUserModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select a.eid, Upper(a.lastname + ', ' + a.firstname + ' ' + case when len(a.mi) > 0 then left(a.mi,1) + '.' else '' end )
                                                from [pmis].[dbo].[employee] as a
                                                where a.eid not 
                                                in(select eid from tbl_R_BMSOrdinanceAttendance where 
                                                Yearof = year(getdate()) and ActionCOde = 1) ORDER BY a.lastname + ', ' + a.firstname + ' ' + 
                                                case when len(a.mi) > 0 then left(a.mi,1) + '.' else '' end  ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    PMISUserModel usr = new PMISUserModel();
                    usr.eid = reader.GetInt64(0);
                    usr.empName = reader.GetValue(1).ToString();

                    UserList.Add(usr);
                }
            }
            return UserList;
        }
        public IEnumerable<PMISUserModel> ddlOrdinanceAttendanceDesignation()
        {
            List<PMISUserModel> UserList = new List<PMISUserModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select distinct Designation from tbl_R_BMSOrdinanceAttendance where ActionCode = 1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    PMISUserModel usr = new PMISUserModel();
                    usr.empName = reader.GetValue(0).ToString();

                    UserList.Add(usr);
                }
            }
            return UserList;
        }
        
        public IEnumerable<PMISUserModel> UsersPerOffice(int OfficeID)
        {

            List<PMISUserModel> UserList = new List<PMISUserModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                //  SqlCommand com = new SqlCommand("select eid,EmpName from [pmis].[dbo].[vwMergeAllEmployee]", con);
                SqlCommand com = new SqlCommand(@"select eid, Lastname,Firstname, MI
                                                from pmis.dbo.vw_RGPermanentAndCasual where OfficeID = " + getPmisOfficeID(OfficeID)
                                                + " and employmentgroup = 1 and basic != 0", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    PMISUserModel usr = new PMISUserModel();
                    usr.eid = reader.GetInt64(0);
                    usr.empName = reader.GetValue(1).ToString() + ", " + reader.GetValue(2).ToString() + " " + reader.GetValue(3).ToString();

                    UserList.Add(usr);
                }
            }
            return UserList;
        }
        public int getPmisOfficeID(int OfficeID)
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
        public IEnumerable<UserTypeModel> UserType()
        {
            List<UserTypeModel> UserTypeList = new List<UserTypeModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("SELECT * FROM tbl_R_BMSUserTypes", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    UserTypeModel Utype = new UserTypeModel();
                    Utype.UserTypeID = reader.GetInt32(0);
                    Utype.UserTypeDesc = reader.GetString(1);

                    UserTypeList.Add(Utype);
                }
            }
            return UserTypeList;
        }
        
        public IEnumerable<OfficesModel> Offices()
        {
            List<OfficesModel> OfficeList = new List<OfficesModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (Account.UserInfo.UserTypeID >= 4 || Account.UserInfo.UserTypeID == 2) // system admin
                {
                    SqlCommand com = new SqlCommand("select OfficeID, CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') from tbl_R_BMSOffices where isnull(PMISOfficeID,'') != '' ORDER BY  OfficeName ", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        OfficesModel Office = new OfficesModel();
                        Office.OfficeID = reader.GetValue(0).ToString();
                        Office.OfficeName = reader.GetString(1);

                        OfficeList.Add(Office);
                    }
                }
                else
                {
                    SqlCommand com = new SqlCommand("select OfficeID, CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') from tbl_R_BMSOffices where officeid=" + Account.UserInfo.Department + "", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        OfficesModel Office = new OfficesModel();
                        Office.OfficeID = reader.GetValue(0).ToString();
                        Office.OfficeName = reader.GetString(1);

                        OfficeList.Add(Office);
                    }
                }
            }
            return OfficeList;
        }

        public IEnumerable<OfficesModel> GetOffice_AIPVerse(int? regularaipid,int? yearof)
        {
            List<OfficesModel> OfficeList = new List<OfficesModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (regularaipid == 1) //annual budget
                {
                    SqlCommand com = new SqlCommand("select OfficeID, CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') from tbl_R_BMSOffices where isnull(PMISOfficeID,'') != '' ORDER BY  OfficeName ", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        OfficesModel Office = new OfficesModel();
                        Office.OfficeID = reader.GetValue(0).ToString();
                        Office.OfficeName = reader.GetString(1);

                        OfficeList.Add(Office);
                    }
                }
                else
                {
                    SqlCommand com = new SqlCommand("select distinct b.OfficeID, CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') as Office " +
                                                        "from ifmis.dbo.tbl_R_BMSOffices as a inner join " +
                                                        "ifmis.dbo.tbl_R_BMSOfficePrograms as b on b.OfficeID = a.OfficeID and b.ActionCode = 1 and b.ProgramYear = " + yearof + " " +
                                                        "where isnull(PMISOfficeID, '') != '' " +
                                                        "and  b.ProgramID in (select z.ProgramID from[IFMIS].[dbo].[tbl_T_BMSBudgetProposal] as z where z.ProgramID = b.ProgramID and z.ProposalActionCode = 1 and z.ProposalYear = b.ProgramYear and  isnull([SupplementalAmount], 0) != 0) " +
                                                        "ORDER BY  Office", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        OfficesModel Office = new OfficesModel();
                        Office.OfficeID = reader.GetValue(0).ToString();
                        Office.OfficeName = reader.GetString(1);

                        OfficeList.Add(Office);
                    }
                }
            }
            return OfficeList;
        }
        public IEnumerable<OfficesModel> ddlOfficeWithAll()
        {
            List<OfficesModel> OfficeList = new List<OfficesModel>();

            OfficesModel AllOffice = new OfficesModel();
            AllOffice.OfficeID = "0";
            AllOffice.OfficeName = "All Office";

            OfficeList.Add(AllOffice);

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select OfficeID, CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') from tbl_R_BMSOffices where [PMISOfficeID] is not null ORDER BY  OfficeName,ISNULL(OrderNo, 999999) ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    OfficesModel Office = new OfficesModel();
                    Office.OfficeID = reader.GetValue(0).ToString();
                    Office.OfficeName = reader.GetString(1);

                    OfficeList.Add(Office);
                }
            }
            return OfficeList;
        }
        
        public int checkIFMISUser(string eid)
        {

            int OfficeID = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select OfficeID, CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') from tbl_R_BMSOffices where PMISOfficeID in (SELECT Office FROM [pmis].[dbo].[employee] where eid='" + eid + "') order BY officeName ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {

                    OfficeID = reader.GetInt32(0);


                }
            }
            return OfficeID;
        }


        public IEnumerable<FundModel> funds()
        {
            List<FundModel> fundList = new List<FundModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("SELECT FundID,FundCode,FundName,FundMedium  FROM tbl_R_BMSFunds ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {

                    FundModel fund = new FundModel();
                    fund.FundID = reader.GetInt32(0);
                    fund.FundCode = reader.GetInt32(1);
                    fund.FundName = reader.GetValue(2).ToString();
                    fund.FundMedium = reader.GetValue(3).ToString();

                    fundList.Add(fund);
                }
            }
            return fundList;
        }



        public IEnumerable<ObjectOfExpendetureModel> OOE()
        {
            List<ObjectOfExpendetureModel> OOEList = new List<ObjectOfExpendetureModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("SELECT OOEID,OOEAbrevation,OOEName FROM tbl_R_BMSObjectOfExpenditure", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {

                    ObjectOfExpendetureModel ooe = new ObjectOfExpendetureModel();
                    ooe.OOEID = reader.GetInt32(0);
                    ooe.OOEAbrevation = reader.GetValue(1).ToString();
                    ooe.OOEName = reader.GetValue(2).ToString();

                    OOEList.Add(ooe);
                }
            }
            return OOEList;
        }



        public IEnumerable<AccountNameModel> acc()
        {
            List<AccountNameModel> AccountNameList = new List<AccountNameModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("SELECT AccountName FROM tbl_R_BMSAccounts", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {

                    AccountNameModel an = new AccountNameModel();

                    an.AccountName = reader.GetValue(0).ToString();


                    AccountNameList.Add(an);
                }
            }
            return AccountNameList;
        }


        public IEnumerable<dp_PorposalYear_Model> Year()
        {
            List<dp_PorposalYear_Model> proposalyear = new List<dp_PorposalYear_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select distinct  AccountYear from  tbl_R_BMSProgramAccounts order by AccountYear desc", con);
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
        public IEnumerable<dp_PorposalYear_Model> proposal_year()
        {
            List<dp_PorposalYear_Model> proposalyear = new List<dp_PorposalYear_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select distinct  AccountYear from  tbl_R_BMSProgramAccounts order by AccountYear desc", con);
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
        public IEnumerable<StepIncrement> Step()
        {
            List<StepIncrement> SalaryGradeList = new List<StepIncrement>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT name,column_id FROM pmis.sys.columns WHERE object_id = OBJECT_ID('pmis.dbo.sgTrance_refs') and name like '%Step%'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    StepIncrement StepIncrement = new StepIncrement();
                    StepIncrement.StepColumn = reader.GetValue(0).ToString();
                    StepIncrement.Step = Convert.ToInt16(reader.GetValue(1));

                    SalaryGradeList.Add(StepIncrement);
                }
            }
            return SalaryGradeList;
        }
        public IEnumerable<PlantillaModel> GetOfficeDivision(int OfficeID)
        {
            List<PlantillaModel> OfficeDivisionList = new List<PlantillaModel>();
            OfficeAdmin_Layer OfficeAdmin_Layer = new OfficeAdmin_Layer();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var Query = "select DivID, Divname from pmis.dbo.EDGE_tblPlantillaDivision where OfficeID = case when " + OfficeID + " in(select SubOfficeID_IFMIS from tbl_R_BMSMainAndSubOffices) then (select top 1 MainOfficeID_PMIS from tbl_R_BMSMainAndSubOffices where SubOfficeID_IFMIS = " + OfficeID + ") ELSE " + OfficeAdmin_Layer.getPmisOfficeID(OfficeID) + " END";
               // SqlCommand com = new SqlCommand(@"select DivID, Divname from pmis.dbo.EDGE_tblPlantillaDivision where OfficeID = " + OfficeAdmin_Layer.getPmisOfficeID(OfficeID) + "", con);
                SqlCommand com = new SqlCommand(Query, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    PlantillaModel OfficeDivision = new PlantillaModel();
                    OfficeDivision.DivID = reader.GetInt32(0);
                    OfficeDivision.DivName = reader.GetValue(1).ToString();

                    OfficeDivisionList.Add(OfficeDivision);
                }
            }
            return OfficeDivisionList;
        }


        //ORAAAAAAAAAAAAAAAAAAAAAAAAAAAYTTTTTT 
        public IEnumerable<OfficesModel> OfficesUser(int? eid)
        {
            List<OfficesModel> OfficeList = new List<OfficesModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select OfficeID, CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') from tbl_R_BMSOffices order BY officeName ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    OfficesModel Office = new OfficesModel();
                    Office.OfficeID = reader.GetValue(0).ToString();
                    Office.OfficeName = reader.GetString(1);

                    OfficeList.Add(Office);
                }
            }
            return OfficeList;
        }



        public IEnumerable<PMISUserModel> Users2(int? eid)
        {
            List<PMISUserModel> UserList = new List<PMISUserModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                //  SqlCommand com = new SqlCommand("select eid,EmpName from [pmis].[dbo].[vwMergeAllEmployee]", con);
                SqlCommand com = new SqlCommand(@"select a.eid, c.EmpName 
                                                  from [pmis].[dbo].[vwLoginParameter] as a
                                                  inner JOIN [pmis].[dbo].[vwMergeAllEmployee] as c
                                                  on a.eid = c.eid where a.eid = " + eid + " order by c.EmpName", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    PMISUserModel usr = new PMISUserModel();
                    usr.eid = reader.GetInt64(0);
                    usr.empName = reader.GetValue(1).ToString();

                    UserList.Add(usr);
                }
            }
            return UserList;
        }


        public int checkOPISUser(string eid)
        {

            int OfficeID = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select OfficeID, CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') from tbl_R_BMSOffices where PMISOfficeID in (SELECT Office FROM [pmis].[dbo].[employee] where eid='" + eid + "') order BY officeName ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {

                    OfficeID = reader.GetInt32(0);


                }
            }
            return OfficeID;
        }


        public int userid(string eid)
        {

            int eids = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select a.eid from [pmis].[dbo].[vwLoginParameter] as a inner JOIN [pmis].[dbo].[vwMergeAllEmployee] as c on a.eid = c.eid where a.eid = " + eid + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {

                    eids = reader.GetInt32(0);


                }
            }
            return eids;
        }

        public IEnumerable<OfficesModel> OfficesOF(int? eid)
        {
            if (eid == null)
            {
                eid = 0;
            }

            List<OfficesModel> OfficeList = new List<OfficesModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select distinct OfficeID, CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')(',REPLACE(b.UserTypeDesc, ' ', ''),')'), b.UserTypeID from tbl_R_BMSOffices as c " +
                      "inner join  IFMIS.dbo.tbl_R_BMSUsers as a on c.OfficeID = a.IFMISOfficeID inner join IFMIS.dbo.tbl_R_BMSUserTypes as b on a.UserTypeID = b.UserTypeID " +
                      "where c.OfficeID in(  select OfficeID from dbo.tbl_R_BMSOffices AS a inner join dbo.tbl_R_BMSUsers as b on a.OfficeID = b.IFMISOfficeID where b.eid= '" + eid + "') and a.eid = '" + eid + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    OfficesModel Office = new OfficesModel();
                    Office.OfficeID = reader.GetValue(0).ToString();
                    Office.OfficeName = reader.GetString(1);
                    Office.UserTypeID = reader.GetInt32(2);

                    OfficeList.Add(Office);
                }
            }
            return OfficeList;
        }

        public IEnumerable<OfficesModel> reserve_office(int? office_id)
        {
            List<OfficesModel> OfficeList = new List<OfficesModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select OfficeID, CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') from tbl_R_BMSOffices where OfficeID = '" + office_id + "' order BY officeName ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    OfficesModel Office = new OfficesModel();
                    Office.OfficeID = reader.GetValue(0).ToString();
                    Office.OfficeName = reader.GetString(1);

                    OfficeList.Add(Office);
                }
            }
            return OfficeList;
        }



    }
}