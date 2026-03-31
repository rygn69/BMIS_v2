using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;
using iFMIS_BMS.BusinessLayer.Models.DashBoard;
using iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation;
using System.Data.SqlClient;
using System.IO;
using iFMIS_BMS.FMIS;
using System.Data;
using iFMIS_BMS.Base;

namespace iFMIS_BMS.BusinessLayer.Layers
{
    public class zLineUtility_Layer
    {

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
        public IEnumerable<LineGraph_Model> Statistics(long? ProposalID=0, int? gLineAccountCode=0, int? gLineProgramID=0, int? gLineAccountID=0, int? LineYear=0, int? LineOffice=0, int? LineOOE=0)
        {

            List<LineGraph_Model> stats = new List<LineGraph_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"exec sp_LineGraph " + ProposalID.ToString() + "," + gLineAccountCode.ToString() + "," + gLineProgramID.ToString() + "," + gLineAccountID.ToString() + "," + LineYear.ToString() + "," + LineOffice.ToString() + "," + LineOOE.ToString() + "", con);

                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    LineGraph_Model stat = new LineGraph_Model();


                    stat.amount1 = Convert.ToDouble(reader.GetValue(0));
                    stat.amount2 = Convert.ToDouble(reader.GetValue(1));
                    stat.amount3 = Convert.ToDouble(reader.GetValue(2));
                    stat.amount4 = Convert.ToDouble(reader.GetValue(3));
                    
                    stats.Add(stat);
                }


            }
            return stats;
        }


        public string getAbbrivation(long? ProposalID, int? gLineAccountCode, int? gLineProgramID, int? gLineAccountID, int? LineYear, int? LineOffice, int? LineOOE)
        {
            var officeAb = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                //   SqlCommand query_time = new SqlCommand(@"SELECT distinct  isnull(e.AccountName,xx.AccountName) AccountName FROM dbo.tbl_T_BMSBudgetProposal AS a
                //                                       left	JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID and e.ActionCode='1'  and  e.AccountYear = a.ProposalYear
                //                                       left JOIN dbo.tbl_R_BMSAccounts AS b ON b.FMISAccountCode = e.AccountID 
                //                                       left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = a.ProgramID 
                //                                       left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID 
                //                                       LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID 
                //                                       LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode
                //                                       LEFT JOIN dbo.tbl_R_BMSProposalRemark as pr ON a.ProposalID = pr.ProposalID 
                //                                       left join tbl_R_BMSProposedAccounts as xx on xx.ProgramID=a.ProgramID and xx.AccountID=a.AccountID and xx.ProposalYear=a.ProposalYear and xx.ActionCode=1
                //                                       left join [IFMIS].[dbo].[tbl_R_BMSAccountsToMerge] as xy on (xy.AccountIDTo=a.AccountID or xy.AccountIDFrom=a.AccountID) and (xy.ProgramIDTo=a.ProgramID or xy.AccountIDTo=a.ProgramID)
                //WHERE (c.OfficeID = " + LineOffice + " or c.OfficeID=xy.OfficeIDFrom) and  a.ProgramID = " + gLineProgramID + " and a.ProposalYear = " + LineYear + " and a.ProposalActionCode = '78'  and a.ProposalStatusCommittee = '2' and (a.AccountID = " + gLineAccountID + " "+
                //                                       "or a.AccountID in (select xy.AccountIDFrom from [IFMIS].[dbo].[tbl_R_BMSAccountsToMerge] as xy where (xy.AccountIDTo=" + gLineAccountID + " or xy.AccountIDFrom=" + gLineAccountID + ") and (xy.ProgramIDTo= " + gLineProgramID + " or xy.ProgramIDFrom= " + gLineProgramID + ")))", con);
                //   con.Open();
                SqlCommand query_time = new SqlCommand(@"EXEC sp_BMS_GetAccountname "+ ProposalID + ","+ gLineAccountCode + ","+ gLineProgramID + ","+ gLineAccountID + ","+ LineYear + ","+ LineOffice + ","+ LineOOE + "", con);
                con.Open();

                officeAb = query_time.ExecuteScalar().ToString();
                return officeAb;

            }
        }



        public IEnumerable<AllLineGraph_Model> allLine(int? Year_Of, int? OOE_ID, int? ACC_ID)
        {

            List<AllLineGraph_Model> stats = new List<AllLineGraph_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"sp_LineGraph_allOffice " + Year_Of + "," + OOE_ID + "," + ACC_ID + "", con);

                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    AllLineGraph_Model stat = new AllLineGraph_Model();





                    stat.office1 = Convert.ToDouble(reader.GetValue(0));
                    stat.office1v = Convert.ToDouble(reader.GetValue(1));
                    stat.office2 = Convert.ToDouble(reader.GetValue(2));
                    stat.office2v = Convert.ToDouble(reader.GetValue(3));
                    stat.office4 = Convert.ToDouble(reader.GetValue(4));
                    stat.office4v = Convert.ToDouble(reader.GetValue(5));
                    stat.office5 = Convert.ToDouble(reader.GetValue(6));
                    stat.office5v = Convert.ToDouble(reader.GetValue(7));
                    stat.office7 = Convert.ToDouble(reader.GetValue(8));
                    stat.office7v = Convert.ToDouble(reader.GetValue(9));
                    stat.office9 = Convert.ToDouble(reader.GetValue(10));
                    stat.office9v = Convert.ToDouble(reader.GetValue(11));
                    stat.office12 = Convert.ToDouble(reader.GetValue(12));
                    stat.office12v = Convert.ToDouble(reader.GetValue(13));
                    stat.office14 = Convert.ToDouble(reader.GetValue(14));
                    stat.office14v = Convert.ToDouble(reader.GetValue(15));
                    stat.office15 = Convert.ToDouble(reader.GetValue(16));
                    stat.office15v = Convert.ToDouble(reader.GetValue(17));
                    stat.office16 = Convert.ToDouble(reader.GetValue(18));
                    stat.office16v = Convert.ToDouble(reader.GetValue(19));
                    stat.office17 = Convert.ToDouble(reader.GetValue(20));
                    stat.office17v = Convert.ToDouble(reader.GetValue(21));
                    stat.office18 = Convert.ToDouble(reader.GetValue(22));
                    stat.office18v = Convert.ToDouble(reader.GetValue(23));
                    stat.office19 = Convert.ToDouble(reader.GetValue(24));
                    stat.office19v = Convert.ToDouble(reader.GetValue(25));
                    stat.office21 = Convert.ToDouble(reader.GetValue(26));
                    stat.office21v = Convert.ToDouble(reader.GetValue(27));
                    stat.office22 = Convert.ToDouble(reader.GetValue(28));
                    stat.office22v = Convert.ToDouble(reader.GetValue(29));
                    stat.office23 = Convert.ToDouble(reader.GetValue(30));
                    stat.office23v = Convert.ToDouble(reader.GetValue(31));
                    stat.office24 = Convert.ToDouble(reader.GetValue(32));
                    stat.office4v = Convert.ToDouble(reader.GetValue(33));
                    stat.office25 = Convert.ToDouble(reader.GetValue(34));
                    stat.office25v = Convert.ToDouble(reader.GetValue(35));
                    stat.office26 = Convert.ToDouble(reader.GetValue(36));
                    stat.office26v = Convert.ToDouble(reader.GetValue(37));
                    stat.office27 = Convert.ToDouble(reader.GetValue(38));
                    stat.office27v = Convert.ToDouble(reader.GetValue(39));
                    stat.office28 = Convert.ToDouble(reader.GetValue(40));
                    stat.office28v = Convert.ToDouble(reader.GetValue(41));
                    stat.office29 = Convert.ToDouble(reader.GetValue(42));
                    stat.office29v = Convert.ToDouble(reader.GetValue(43));
                    stat.office30 = Convert.ToDouble(reader.GetValue(44));
                    stat.office30v = Convert.ToDouble(reader.GetValue(45));
                    stat.office31 = Convert.ToDouble(reader.GetValue(46));
                    stat.office31v = Convert.ToDouble(reader.GetValue(47));
                    stat.office32 = Convert.ToDouble(reader.GetValue(48));
                    stat.office32v = Convert.ToDouble(reader.GetValue(49));
                    stat.office33 = Convert.ToDouble(reader.GetValue(50));
                    stat.office33v = Convert.ToDouble(reader.GetValue(51));
                    stat.office35 = Convert.ToDouble(reader.GetValue(52));
                    stat.office35v = Convert.ToDouble(reader.GetValue(53));
                    stat.office36 = Convert.ToDouble(reader.GetValue(54));
                    stat.office36v = Convert.ToDouble(reader.GetValue(55));
                    stat.office37 = Convert.ToDouble(reader.GetValue(56));
                    stat.office37v = Convert.ToDouble(reader.GetValue(57));
                    stat.office38 = Convert.ToDouble(reader.GetValue(58));
                    stat.office38v = Convert.ToDouble(reader.GetValue(59));
                    stat.office39 = Convert.ToDouble(reader.GetValue(60));
                    stat.office39v = Convert.ToDouble(reader.GetValue(61));
                    stat.office41 = Convert.ToDouble(reader.GetValue(62));
                    stat.office41v = Convert.ToDouble(reader.GetValue(63));
                    stat.office43 = Convert.ToDouble(reader.GetValue(64));
                    stat.office43v = Convert.ToDouble(reader.GetValue(65));
                    stat.office49 = Convert.ToDouble(reader.GetValue(66));
                    stat.office49v = Convert.ToDouble(reader.GetValue(67));
                    stat.office51 = Convert.ToDouble(reader.GetValue(68));
                    stat.office51v = Convert.ToDouble(reader.GetValue(69));
                    stat.office53 = Convert.ToDouble(reader.GetValue(70));
                    stat.office53v = Convert.ToDouble(reader.GetValue(71));
                    stat.office57 = Convert.ToDouble(reader.GetValue(72));
                    stat.office57v = Convert.ToDouble(reader.GetValue(73));
                    stat.office63 = Convert.ToDouble(reader.GetValue(74));
                    stat.office63v = Convert.ToDouble(reader.GetValue(75));
                    stat.office65 = Convert.ToDouble(reader.GetValue(76));
                    stat.office65v = Convert.ToDouble(reader.GetValue(77));
                    stat.office69 = Convert.ToDouble(reader.GetValue(78));
                    stat.office9v = Convert.ToDouble(reader.GetValue(79));
                    stat.office70 = Convert.ToDouble(reader.GetValue(80));
                    stat.office70v = Convert.ToDouble(reader.GetValue(81));



                    stats.Add(stat);
                }


            }
            return stats;
        }


        public IEnumerable<separate_Model> separate()
        {
            List<separate_Model> proposalyear = new List<separate_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("SELECT per_ID,per_Desc FROM IFMIS.dbo.tbl_R_BMS_line_separator", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    separate_Model app = new separate_Model();

                    app.per_ID = reader.GetInt32(0);
                    app.per_Desc = reader.GetValue(1).ToString();

                    proposalyear.Add(app);
                }
            }
            return proposalyear;
        }
        public IEnumerable<separate_Model> getAccountID(int? Year_Of, int? OOE_ID)
        {
            List<separate_Model> accountID = new List<separate_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT distinct AccountID,AccountName FROM IFMIS.dbo.tbl_R_BMSProgramAccounts where ObjectOfExpendetureID = '" + OOE_ID + "' and AccountYear = '" + Year_Of + "' and [ActionCode]=1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    separate_Model app = new separate_Model();

                    app.AccountID = reader.GetInt32(0);
                    app.AccountName = reader.GetValue(1).ToString();

                    accountID.Add(app);
                }
            }
            return accountID;
        }

        public IEnumerable<separate_Model> getOOEID()
        {
            List<separate_Model> accountID = new List<separate_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("SELECT OOEID,OOEName FROM IFMIS.dbo.tbl_R_BMSObjectOfExpenditure", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    separate_Model app = new separate_Model();

                    app.OOEID = reader.GetInt32(0);
                    app.OOEName = reader.GetValue(1).ToString();

                    accountID.Add(app);
                }
            }
            return accountID;
        }

        public IEnumerable<accChart_Model> teststatistic(int? ProposalID, int? gLineAccountCode, int? gLineProgramID, int? gLineAccountID, int? LineYear, int? LineOffice, int? LineOOE)
        {

            List<accChart_Model> stats = new List<accChart_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"IFMIS.dbo.sp_chart_peroffice_PIE 4,2017", con);

                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    accChart_Model stat = new accChart_Model();


                    stat.name_type = Convert.ToString(reader.GetValue(0));
                    stat.mounts = Convert.ToDouble(reader.GetValue(1));
                    stat.percentage = Convert.ToDecimal(reader.GetValue(2));


                    stats.Add(stat);
                }


            }
            return stats;
        }
        public IEnumerable<DashBoardModel> OfficePSMooeCA(int? BudgetYear = 0, int? OfficeId = 0, int? ooeid = 0)
        {
            List<DashBoardModel> PvsPList = new List<DashBoardModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                SqlCommand selectTotal = new SqlCommand(@"exec sp_BMS_PieChartPerOfficeExpense " + BudgetYear + "," + OfficeId + "", con);
                SqlDataReader reader_selectTotal = selectTotal.ExecuteReader();

                while (reader_selectTotal.Read())
                {
                    DashBoardModel dataList = new DashBoardModel();
                    dataList.OOEIDPie = Convert.ToInt32(reader_selectTotal.GetValue(0));
                    dataList.LegendPiePSMooeCA = Convert.ToString(reader_selectTotal.GetValue(1));
                    // reader_selectTotal.Close();
                    dataList.TotalPSMooeCA = Convert.ToDouble(reader_selectTotal.GetValue(2));// + '(' + Convert.ToDouble(reader_selectTotal.GetValue(3)) + ')'; 
                    // dataList.TotalPSMooeCAPercent = Convert.ToDouble(reader_selectTotal.GetValue(3));
                    PvsPList.Add(dataList);
                    //reader_selectTotal.Read();
                }
            }
            return PvsPList;
        }
        public IEnumerable<DashBoardModel> OfficePSMooeCAProgram(int? BudgetYear = 0, int? OfficeId = 0, int? ooeid = 0)
        {
            List<DashBoardModel> PvsPList = new List<DashBoardModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                SqlCommand selectTotal = new SqlCommand(@"exec sp_BMS_PieChartPerOfficeProgram " + BudgetYear + "," + OfficeId + "", con);
                SqlDataReader reader_selectTotal = selectTotal.ExecuteReader();

                while (reader_selectTotal.Read())
                {
                    DashBoardModel dataList = new DashBoardModel();
                    dataList.OOEIDPie = Convert.ToInt32(reader_selectTotal.GetValue(0));
                    dataList.LegendPiePSMooeCA = Convert.ToString(reader_selectTotal.GetValue(1));
                    // reader_selectTotal.Close();
                    dataList.TotalPSMooeCA = Convert.ToDouble(reader_selectTotal.GetValue(2));// + '(' + Convert.ToDouble(reader_selectTotal.GetValue(3)) + ')'; 
                    // dataList.TotalPSMooeCAPercent = Convert.ToDouble(reader_selectTotal.GetValue(3));
                    PvsPList.Add(dataList);
                    //reader_selectTotal.Read();
                }
            }
            return PvsPList;
        }
        public IEnumerable<DashBoardModel> StatisticsExpenseClass(int? LineYear, int? LineOffice, int? LineOOE)
        {

            List<DashBoardModel> stats = new List<DashBoardModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"exec sp_LineGraphEC " + LineYear.ToString() + "," + LineOffice.ToString() + "," + LineOOE.ToString() + "", con);

                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    DashBoardModel stat = new DashBoardModel();


                    stat.amount1 = Convert.ToDouble(reader.GetValue(0));
                    stat.amount2 = Convert.ToDouble(reader.GetValue(1));
                    stat.amount3 = Convert.ToDouble(reader.GetValue(2));


                    stats.Add(stat);
                }
            }
            return stats;
        }
        public IEnumerable<DashBoardModel> PieChartPSMooeCADetails(int? BudgetYear = 0, int? OfficeId = 0, int? ooeid = 0)
        {
            List<DashBoardModel> PvsPList = new List<DashBoardModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                SqlCommand selectTotal = new SqlCommand(@"exec sp_BMS_PieChartPerOfficeECDetails " + BudgetYear + "," + OfficeId + "," + ooeid + "", con);
                SqlDataReader reader_selectTotal = selectTotal.ExecuteReader();

                while (reader_selectTotal.Read())
                {
                    DashBoardModel dataList = new DashBoardModel();
                    dataList.OOEIDPie = Convert.ToInt32(reader_selectTotal.GetValue(0));    
                    dataList.LegendPiePSMooeCA = Convert.ToString(reader_selectTotal.GetValue(1));
                    // reader_selectTotal.Close();
                    dataList.TotalPSMooeCA = Convert.ToDouble(reader_selectTotal.GetValue(2));// + '(' + Convert.ToDouble(reader_selectTotal.GetValue(3)) + ')'; 
                    // dataList.TotalPSMooeCAPercent = Convert.ToDouble(reader_selectTotal.GetValue(3));
                    PvsPList.Add(dataList);
                    //reader_selectTotal.Read();
                }
            }
            return PvsPList;
        }
        public IEnumerable<DashBoardModel> PieChartPSMooeCADetailsP2(int? yrof = 0, int? offId = 0, int? expid = 0, int? expid2 = 0)
        {
            List<DashBoardModel> PvsPList = new List<DashBoardModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                SqlCommand selectTotal = new SqlCommand(@"exec sp_BMS_PieChartPerOfficeECDetailsP2 " + yrof + "," + offId + "," + expid + "," + expid2 + "", con);
                SqlDataReader reader_selectTotal = selectTotal.ExecuteReader();

                while (reader_selectTotal.Read())
                {
                    DashBoardModel dataList = new DashBoardModel();
                    dataList.OOEIDPie = Convert.ToInt32(reader_selectTotal.GetValue(0));
                    dataList.LegendPiePSMooeCA = Convert.ToString(reader_selectTotal.GetValue(1));
                    // reader_selectTotal.Close();
                    dataList.TotalPSMooeCA = Convert.ToDouble(reader_selectTotal.GetValue(2));// + '(' + Convert.ToDouble(reader_selectTotal.GetValue(3)) + ')'; 
                    // dataList.TotalPSMooeCAPercent = Convert.ToDouble(reader_selectTotal.GetValue(3));
                    PvsPList.Add(dataList);
                    //reader_selectTotal.Read();
                }
            }
            return PvsPList;
        }
        public IEnumerable<DashBoardModel> GetAccountDetails(int? yrof = 0, int? offId = 0, int? expid=0, string selPie = "")
        {
           
            List<DashBoardModel> PvsPList = new List<DashBoardModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                SqlCommand selectTotal = new SqlCommand(@"exec sp_BMS_GridAccount " + yrof + "," + offId + ",'" + selPie + "'," + expid + "", con);
                SqlDataReader reader_selectTotal = selectTotal.ExecuteReader();

                while (reader_selectTotal.Read())
                {
                    DashBoardModel dataList = new DashBoardModel();
                    dataList.OOEIDPie = Convert.ToInt32(reader_selectTotal.GetValue(0));
                    dataList.LegendPiePSMooeCA = Convert.ToString(reader_selectTotal.GetValue(1));
                    // reader_selectTotal.Close();
                    dataList.TotalPSMooeCA = Convert.ToDouble(reader_selectTotal.GetValue(2));// + '(' + Convert.ToDouble(reader_selectTotal.GetValue(3)) + ')'; 
                    // dataList.TotalPSMooeCAPercent = Convert.ToDouble(reader_selectTotal.GetValue(3));
                    PvsPList.Add(dataList);
                    //reader_selectTotal.Read();
                }
            }
            return PvsPList;
        }
        public IEnumerable<DashBoardModel> getaccountdetailsgrid(int? yrof = 0, int? offId = 0, int? expid = 0, string selPie = "")
        {
            //GRID//
            List<DashBoardModel> prog = new List<DashBoardModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec sp_BMS_GridAccount " + yrof + "," + offId + ",'" + selPie + "'," + expid + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    DashBoardModel emp = new DashBoardModel();
                    emp.id = Convert.ToInt32(reader.GetValue(0));
                    emp.AccountName = reader.GetValue(1).ToString();
                    emp.Amount = Convert.ToDouble(reader.GetValue(2));
                    emp.qty = Convert.ToInt32(reader.GetValue(3));
                    prog.Add(emp);
                }

            }
            return prog;
        }
        public IEnumerable<DashBoardModel> GetProposeCount(int? yrof = 0, int? offId = 0, string expid2 = "")
        {
            List<DashBoardModel> PvsPList = new List<DashBoardModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                SqlCommand selectTotal = new SqlCommand(@"exec sp_BMS_PieChartProposedCount " + yrof + "," + offId + ",'" + expid2 + "'", con);
                SqlDataReader reader_selectTotal = selectTotal.ExecuteReader();

                while (reader_selectTotal.Read())
                {
                    DashBoardModel dataList = new DashBoardModel();
                    dataList.OOEIDPie = Convert.ToInt32(reader_selectTotal.GetValue(0));
                    dataList.LegendPiePSMooeCA = Convert.ToString(reader_selectTotal.GetValue(1));
                    // reader_selectTotal.Close();
                    dataList.TotalPSMooeCA = Convert.ToDouble(reader_selectTotal.GetValue(2));// + '(' + Convert.ToDouble(reader_selectTotal.GetValue(3)) + ')'; 
                    // dataList.TotalPSMooeCAPercent = Convert.ToDouble(reader_selectTotal.GetValue(3));
                    PvsPList.Add(dataList);
                    //reader_selectTotal.Read();
                }
            }
            return PvsPList;   
        }
        public IEnumerable<DashBoardModel> GetPSDetail(int? yrof = 0, int? offId = 0, string selPie = "")
        {
            //GRID//
            List<DashBoardModel> prog = new List<DashBoardModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var MotherOfficeID = 0;
                var pmisOfficeID = 0;
                SqlCommand comm = new SqlCommand(@"SELECT isnull(MainOfficeID_PMIS,0) 
                    FROM [dbo].[tbl_R_BMSMainAndSubOffices] where SubOfficeID_IFMIS =" + offId + "", con);
                con.Open();
                MotherOfficeID = Convert.ToInt32(comm.ExecuteScalar());
                con.Close();

                SqlCommand comm2 = new SqlCommand(@"SELECT PMISOfficeID from tbl_R_BMSOffices where OfficeID=" + offId  + "", con);
                con.Open();
                pmisOfficeID = Convert.ToInt32(comm2.ExecuteScalar());
                con.Close();

                if (selPie == "No. of Proposed for Funding")
                {
                    SqlCommand com = new SqlCommand(@"exec sp_BMS_PieChartPSDetails " + yrof + "," + offId + "", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        DashBoardModel emp = new DashBoardModel();
                        emp.id = Convert.ToInt32(reader.GetValue(0));
                        emp.Position = reader.GetValue(1).ToString();
                        emp.Amount = Convert.ToDouble(reader.GetValue(2));
                        prog.Add(emp);
                    }
                }
                else if (selPie == "No. of Filled-up")
                {
                    if (MotherOfficeID == 0)
                    {
                        SqlCommand com = new SqlCommand(@"exec sp_get_data_from_PlantillaAdminView_Filled " + yrof + "," + pmisOfficeID + ",0,0", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            DashBoardModel emp = new DashBoardModel();
                            emp.id = Convert.ToInt32(reader.GetValue(0));
                            emp.Position = reader.GetValue(14).ToString();
                            emp.Amount = Convert.ToDouble(reader.GetValue(12));
                            prog.Add(emp);
                        }
                    }
                    else
                    {
                        SqlCommand com = new SqlCommand(@"exec sp_get_data_from_PlantillaAdminView_Filled " + yrof + "," + MotherOfficeID + "," + offId + ",1", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            DashBoardModel emp = new DashBoardModel();
                            emp.id = Convert.ToInt32(reader.GetValue(0));
                            emp.Position = reader.GetValue(14).ToString();
                            emp.Amount = Convert.ToDouble(reader.GetValue(12));
                            prog.Add(emp);
                        }
                    }
                }
                else if (selPie == "No. of Vacant")
                {
                    if (MotherOfficeID == 0)
                    {
                        SqlCommand com = new SqlCommand(@"exec [sp_get_data_from_PlantillaAdminView_Vacant] " + yrof + "," + pmisOfficeID + ",0,0", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            DashBoardModel emp = new DashBoardModel();
                            emp.id = Convert.ToInt32(reader.GetValue(0));
                            emp.Position = reader.GetValue(2).ToString();
                            emp.Amount = Convert.ToDouble(reader.GetValue(12));
                            prog.Add(emp);
                        }
                    }
                    else
                    {
                        SqlCommand com = new SqlCommand(@"exec [sp_get_data_from_PlantillaAdminView_Vacant] " + yrof + "," + MotherOfficeID + "," + offId + ",1", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            DashBoardModel emp = new DashBoardModel();
                            emp.id = Convert.ToInt32(reader.GetValue(0));
                            emp.Position = reader.GetValue(2).ToString();
                            emp.Amount = Convert.ToDouble(reader.GetValue(12));
                            prog.Add(emp);
                        }
                    }
                }
                else if (selPie == "No. of Casual")
                {
                    SqlCommand com = new SqlCommand(@"exec [sp_get_data_from_PlantillaAdminViewCasual_Grid] " + yrof + "," + pmisOfficeID + "", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        DashBoardModel emp = new DashBoardModel();
                        emp.id = Convert.ToInt32(reader.GetValue(0));
                        emp.Position = reader.GetValue(2).ToString();
                        emp.Amount = Convert.ToDouble(reader.GetValue(12));
                        prog.Add(emp);
                    }
                }
            }
            return prog;
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
        public IEnumerable<DashBoardModel> BarAccountComparison(int? BudgetYear = 0, string acctname = "")
        {
            List<DashBoardModel> PvsPList = new List<DashBoardModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                SqlCommand selectTotal = new SqlCommand(@"exec sp_BMS_AccountTable " + BudgetYear + ",'" + acctname + "'", con);
                SqlDataReader reader_selectTotal = selectTotal.ExecuteReader();

                while (reader_selectTotal.Read())
                {
                    DashBoardModel dataList = new DashBoardModel();
                    dataList.LegendPiePSMooeCA = Convert.ToString(reader_selectTotal.GetValue(1));
                    dataList.OOEIDPie = Convert.ToInt32(reader_selectTotal.GetValue(0));
                    dataList.TotalPSMooeCA = Convert.ToDouble(reader_selectTotal.GetValue(3));// + '(' + Convert.ToDouble(reader_selectTotal.GetValue(3)) + ')'; 
                    // dataList.TotalPSMooeCAPercent = Convert.ToDouble(reader_selectTotal.GetValue(3));
                    PvsPList.Add(dataList);
                    //reader_selectTotal.Read();
                }
            }
            return PvsPList;
        }
        public IEnumerable<DashBoardModel> GetAppropriationSummary(int? yearof = 0,int? monof = 0,int? earmark=0, int? officeid=0)
        {
            //GRID//
            List<DashBoardModel> prog = new List<DashBoardModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                
                SqlCommand com = new SqlCommand(@"exec [dbo].[sp_Monthly_SAAO_REPORT_View] 1," + monof + "," + yearof + ","+ officeid + ",1,"+ earmark + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    DashBoardModel emp = new DashBoardModel();
                    emp.trnno = Convert.ToInt32(reader.GetValue(15));
                    emp.Programname = reader.GetValue(8).ToString();
                    emp.AccountName = reader.GetValue(6).ToString();
                    emp.appropriation = Convert.ToDouble(reader.GetValue(0));
                    emp.allotment = Convert.ToDouble(reader.GetValue(1));
                    emp.obligation = Convert.ToDouble(reader.GetValue(2));
                    emp.Allotment_balance = Convert.ToDouble(reader.GetValue(3));
                    emp.Appropriation_balance = Convert.ToDouble(reader.GetValue(4));
                    prog.Add(emp);
                }

            }
            return prog;

        }
        public IEnumerable<ReportModel> BarAppropriationComparison(int? yearof = 0, int accountid = 0)
        {
            List<ReportModel> PvsPList = new List<ReportModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                SqlCommand selectTotal = new SqlCommand(@"exec sp_BMS_appropriationproportion " + yearof + "," + accountid + "", con);
                SqlDataReader reader_selectTotal = selectTotal.ExecuteReader();

                while (reader_selectTotal.Read())
                {
                    ReportModel dataList = new ReportModel();
                    dataList.OfficeName = Convert.ToString(reader_selectTotal.GetValue(2));
                    dataList.pmisofficeid = Convert.ToInt32(reader_selectTotal.GetValue(1));
                    dataList.proportion = Convert.ToDouble(reader_selectTotal.GetValue(7));// + '(' + Convert.ToDouble(reader_selectTotal.GetValue(3)) + ')'; 
                    // dataList.TotalPSMooeCAPercent = Convert.ToDouble(reader_selectTotal.GetValue(3));
                    PvsPList.Add(dataList);
                    //reader_selectTotal.Read();
                }
            }
            return PvsPList;
        }
        public IEnumerable<SAAODA> GetSAAODA(int? yearof, int? office = 0)
        {
            List<SAAODA> PvsPList = new List<SAAODA>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                SqlCommand selectTotal = new SqlCommand(@"exec[sp_bms_appropriationutilization_accounted] " + yearof + ",12," + office + "", con);
                SqlDataReader reader_selectTotal = selectTotal.ExecuteReader();

                while (reader_selectTotal.Read())
                {
                    SAAODA dataList = new SAAODA();
                    dataList.trnno = Convert.ToInt32(reader_selectTotal.GetValue(0));
                    dataList.AccountName = Convert.ToString(reader_selectTotal.GetValue(3));
                    dataList.Appropriation = Convert.ToDecimal(reader_selectTotal.GetValue(4));
                    dataList.Allotment = Convert.ToDecimal(reader_selectTotal.GetValue(5));
                    dataList.Obligation = Convert.ToDecimal(reader_selectTotal.GetValue(6));
                    dataList.Disbursed = Convert.ToDecimal(reader_selectTotal.GetValue(7));
                    dataList.Accounted = Convert.ToDecimal(reader_selectTotal.GetValue(8));
                    PvsPList.Add(dataList);
                }
            }
            return PvsPList;
        }
        public IEnumerable<DashBoardModel> UtilizationDenom(int? year = 0, string status="",int? fundsource=0)
        {
            List<DashBoardModel> PvsPList = new List<DashBoardModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                SqlCommand selectTotal = new SqlCommand(@"exec sp_BMS_Utilization_Denomination "+ year + ",'"+ status + "',"+ fundsource + "", con);
                SqlDataReader reader_selectTotal = selectTotal.ExecuteReader();

                while (reader_selectTotal.Read())
                {
                    DashBoardModel dataList = new DashBoardModel();
                    dataList.fundid = Convert.ToInt32(reader_selectTotal.GetValue(0));
                    dataList.fundname = Convert.ToString(reader_selectTotal.GetValue(1));
                    // reader_selectTotal.Close();
                    dataList.finamount = Convert.ToDouble(reader_selectTotal.GetValue(2));// + '(' + Convert.ToDouble(reader_selectTotal.GetValue(3)) + ')'; 
                    // dataList.TotalPSMooeCAPercent = Convert.ToDouble(reader_selectTotal.GetValue(3));
                    PvsPList.Add(dataList);
                    //reader_selectTotal.Read();
                }
            }
            return PvsPList;
        }
        public IEnumerable<DashBoardModel> UtilizationDetailOffice(int? year = 0, string status = "",string fundname="", int? fundsource = 0)
        {
            List<DashBoardModel> PvsPList = new List<DashBoardModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                SqlCommand selectTotal = new SqlCommand(@"exec [sp_BMS_Utilization_DenominationPerOffice] " + year + ",'" + status + "','"+ fundname + "',"+ fundsource + "", con);
                SqlDataReader reader_selectTotal = selectTotal.ExecuteReader();

                while (reader_selectTotal.Read())
                {
                    DashBoardModel dataList = new DashBoardModel();
                    dataList.fundid = Convert.ToInt32(reader_selectTotal.GetValue(0));
                    dataList.fundname = Convert.ToString(reader_selectTotal.GetValue(1));
                    // reader_selectTotal.Close();
                    dataList.finamount = Convert.ToDouble(reader_selectTotal.GetValue(2));// + '(' + Convert.ToDouble(reader_selectTotal.GetValue(3)) + ')'; 
                    // dataList.TotalPSMooeCAPercent = Convert.ToDouble(reader_selectTotal.GetValue(3));
                    PvsPList.Add(dataList);
                    //reader_selectTotal.Read();
                }
            }
            return PvsPList;
        }
    }
    
}