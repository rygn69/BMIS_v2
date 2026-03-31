using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models.DashBoard;
using iFMIS_BMS.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace iFMIS_BMS.BusinessLayer.Layers
{
  
    public class DashboardLayer
    {
        public IEnumerable<DashBoardModel> ProjectedVsProposed(int? BudgetYear=0)
        {
            var ProposedAmount = 0.0;
            var ApprovedAmount = 0.0;            
            var ApprovedDifference = 0.0;
            var Projected = 0.0;
            List<DashBoardModel> PvsPList = new List<DashBoardModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                con.Open();
                SqlCommand proposedGF = new SqlCommand(@"select  isnull(sum(BudgetYearAmount),0) from (
                                                                select distinct  a.OfficeID,ProgramID,AccountID,a.BudgetYearAmount from tbl_R_BMSConsoliditedTotal as a
                                                                            LEFT JOIN tbl_R_BMSOffices as b on b.OfficeID = a.OfficeID
                                                                            where b.FundID = (select top 1 FundID from tbl_R_BMSOffices where OfficeID = 4) and a.YearOf = "+ BudgetYear + ") as x", con);
                DashBoardModel dataList = new DashBoardModel();
                DashBoardModel dataList2 = new DashBoardModel();
                DashBoardModel dataList3 = new DashBoardModel();
                ApprovedAmount = Convert.ToDouble(proposedGF.ExecuteScalar().ToString());

                SqlCommand projected = new SqlCommand(@"dbo.sp_bms_ProjectedTotalAllOffice " + BudgetYear + ", 0", con);
                Projected = Convert.ToDouble(projected.ExecuteScalar().ToString());

                SqlCommand OriginalProposedAmount = new SqlCommand(@"select isnull((select sum(ProposalAmount) from tbl_T_BMSBudgetProposal where ProposalYear = " + BudgetYear + " and ProposalActioncode = 1 and programid not in (44,45,48,96,142,162,165)),0) " + ""
                            + " +isnull((select sum(total) from tbl_R_BMSSubmittedForFundingData as a " + ""
                            + " INNER JOIN tbl_R_BMSProposedNewItem as b on b.ProposedItemID = a.ProposedItemID " + ""
                            + " where GroupTag = 'For Funding' and a.Yearof =" + BudgetYear + "  and b.ActionCode = 2 and a.officeid not in (37,38,41)),0)", con);
                //SqlCommand OriginalProposedAmount = new SqlCommand(@"select isnull((select sum(ProposalAmount) from tbl_T_BMSBudgetProposal where ProposalYear = " + BudgetYear + " and ProposalActioncode = 1 and programid not in (44,45,48,96,142,162,165)),0)", con);
                ProposedAmount = Convert.ToDouble(OriginalProposedAmount.ExecuteScalar().ToString());

                dataList.BudgetYear = "Budget Year " + BudgetYear;

                ApprovedDifference = ApprovedAmount - Projected;

                dataList.col1 = Projected;
                dataList2.col1 = 0;
                dataList3.col1 = 0;

                dataList.col2 = ApprovedDifference;
                dataList2.col2 = 0;
                dataList3.col2 = 0;

                dataList.col3 = 0;
                dataList2.col3 = ApprovedAmount;
                dataList3.col3 = 0;

                dataList.col4 = 0;
                dataList2.col4 = 0;
                dataList3.col4 = ProposedAmount;

                PvsPList.Add(dataList);
                PvsPList.Add(dataList2);
                PvsPList.Add(dataList3);
                  
            }
            return PvsPList;
        }

        public IEnumerable<DashBoardModel> OfficesWithProposal(int? BudgetYear=0)
        {
            var TotalNoOfOffices = 0;
            var OfficesWithProposal = 0;            
            var OfficesWithSubmittedProposal = 0;
            var noProposalYet = 0;
            List<DashBoardModel> PvsPList = new List<DashBoardModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                con.Open();
                DashBoardModel dataList = new DashBoardModel();
                DashBoardModel dataList2 = new DashBoardModel();
                DashBoardModel dataList3 = new DashBoardModel();
                //SqlCommand comOfficesWithProposal = new SqlCommand(@"with cte as(select count(b.OfficeID) as cnt from tbl_T_BMSAccountDenomination as a 
                //                                                    LEFT JOIN tbl_R_BMSOffices as b on b.OfficeID = a.OfficeID
                //                                                    where TransactionYear = "+ BudgetYear +" and ActionCode = 1 and a.OfficeID not in( "+""
                //                                                    +" SELECT c.OfficeID as Cnt  from tbl_T_BMSBudgetProposal as a "+""
                //                                                    +" LEFT JOIN tbl_R_BMSOfficePrograms as b on b.ProgramID = a.ProgramID  "+""
                //                                                    +" and b.ProgramYear = a.ProposalYear "+""
                //                                                    +" LEFT JOIN tbl_R_BMSOffices as c on c.OfficeID = b.OfficeID "+""
                //                                                    +" where a.Proposalyear = "+ BudgetYear + " and ProposalActionCOde = 1  and b.ActionCode=1" + ""
                //                                                    +" ) GROUP BY b.OfficeID, b.OfficeName) select isnull(count(cnt),0) from cte", con);
                SqlCommand comOfficesWithProposal = new SqlCommand(@"select count(cnt) as cnt from (select distinct a.OfficeID as cnt from tbl_T_BMSAccountDenomination as a 
                                                                            inner JOIN tbl_R_BMSOffices as b on b.OfficeID = a.OfficeID
                                                                            where TransactionYear =" + BudgetYear + " and a.ActionCode = 1 and b.OfficeID not in ( " + ""
                                                                           + "   SELECT xx.OfficeID  from tbl_T_BMSBudgetProposal as x inner join" + ""
                                                                            + "     ifmis.dbo.tbl_R_BMSOfficePrograms as xx on xx.ProgramID=x.ProgramID and xx.ActionCode=1 and xx.ProgramYear=x.ProposalYear" + ""
                                                                           + "  where x.Proposalyear = " + BudgetYear +" and ProposalActionCOde = 1  and xx.OfficeID=b.OfficeID )) as x", con);
                OfficesWithProposal = comOfficesWithProposal.ExecuteScalar() == null ? 0: Convert.ToInt32(comOfficesWithProposal.ExecuteScalar().ToString());

                SqlCommand comOfficesWithSubmittedProposal = new SqlCommand(@"with cte as(
                                                                            SELECT case when isnull(count(a.ProposalID),0) > 0 then 1 else 0 end as Cnt  from tbl_T_BMSBudgetProposal as a
                                                                            LEFT JOIN tbl_R_BMSOfficePrograms as b on b.ProgramID = a.ProgramID and b.ActionCode = a.ProposalActionCode 
                                                                            and b.ProgramYear = a.ProposalYear
                                                                            LEFT JOIN tbl_R_BMSOffices as c on c.OfficeID = b.OfficeID
                                                                            where a.Proposalyear = "+ BudgetYear +" and ProposalActionCOde = 1 GROUP BY c.OfficeID, c.OfficeName) select isnull(sum(Cnt),0) from cte", con);
                OfficesWithSubmittedProposal = comOfficesWithSubmittedProposal.ExecuteScalar() == null ? 0 : Convert.ToInt32(comOfficesWithSubmittedProposal.ExecuteScalar().ToString());

                SqlCommand comTotalNoOfOffices = new SqlCommand(@"with cte as(
                                                                            SELECT case when isnull(count(a.ProposalID),0) > 0 then 1 else 0 end as Cnt  from tbl_T_BMSBudgetProposal as a
                                                                            LEFT JOIN tbl_R_BMSOfficePrograms as b on b.ProgramID = a.ProgramID and b.ActionCode = a.ProposalActionCode 
                                                                            and b.ProgramYear = a.ProposalYear
                                                                            LEFT JOIN tbl_R_BMSOffices as c on c.OfficeID = b.OfficeID
                                                                            where a.Proposalyear = " + (BudgetYear - 1) + " and ProposalActionCOde = 1 and FundID <> 201 and PMISOfficeID is not null and c.OfficeID <> 43 GROUP BY c.OfficeID, c.OfficeName) select isnull(sum(Cnt),0) from cte", con);
                TotalNoOfOffices = comTotalNoOfOffices.ExecuteScalar() == null ? 0 : Convert.ToInt32(comTotalNoOfOffices.ExecuteScalar().ToString());

                noProposalYet = TotalNoOfOffices - OfficesWithSubmittedProposal - OfficesWithProposal;
                dataList.BudgetYear = "Budget Year " + BudgetYear;


                dataList.col1 = OfficesWithSubmittedProposal;
                dataList2.col1 = 0;


                dataList.col2 = OfficesWithProposal;
                dataList2.col2 = 0;


                dataList.col3 = noProposalYet;
                dataList2.col3 = 0;
                

                dataList.col4 = 0;
                dataList2.col4 = TotalNoOfOffices;

                PvsPList.Add(dataList);
                PvsPList.Add(dataList2);
                //PvsPList.Add(dataList3);
                  
            }
            return PvsPList;
        }
        public IEnumerable<DashBoardModel> TotalPSMooeCA(int? BudgetYear=0)
        {
            List<DashBoardModel> PvsPList = new List<DashBoardModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                SqlCommand selectTotal = new SqlCommand(@"exec sp_BMS_PieChartPSMOOECO'" + BudgetYear + "'", con);
                //SqlCommand selectTotal = new SqlCommand(@"exec sp_BMS_PieChartPerOfficeExpense 2019,4", con);
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

        public IEnumerable<DashBoardModel> TotalPSMooe(int? BudgetYear=0)
        {
            List<DashBoardModel> PvsPList = new List<DashBoardModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();

                SqlCommand selectTotal = new SqlCommand(@"exec sp_BMS_PieChartNonOffice " + BudgetYear  + "", con);
                SqlDataReader reader_selectTotal = selectTotal.ExecuteReader();

                while (reader_selectTotal.Read())
                {
                    DashBoardModel dataList = new DashBoardModel();
                    dataList.OOEIDPie = Convert.ToInt32(reader_selectTotal.GetValue(0));
                    dataList.LegendPiePSMooeCA = Convert.ToString(reader_selectTotal.GetValue(1));
                   
                        dataList.TotalPSMooeCA = Convert.ToDouble(reader_selectTotal.GetValue(2));
                   
                    PvsPList.Add(dataList);
                   
                }
            }
            return PvsPList;
        }
        public IEnumerable<DashBoardModel> TotalAllOffices(int? BudgetYear=0)
        {
            List<DashBoardModel> PvsPList = new List<DashBoardModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                //Update on 7/17/2018 - xXx - changed to stored proc
                SqlCommand selectTotal = new SqlCommand(@"exec [sp_BMS_PieChartPerOffice]'" + BudgetYear + "'", con);
//                SqlCommand selectTotal = new SqlCommand(@"declare @TempTable table (OfficeName VARCHAR(225),  OfficeID int, AccountName VARCHAR(500), OOEID int, OOEAbrevation VARCHAR(10), AMOUNT money, OOENAME VARCHAR(255))
//                    insert into @TempTable EXEC dbo.sp_bms_getForFundingData '" + BudgetYear + "',0,9,2 " +


//                    "SELECT z.OfficeID, MIN(z.OfficeName) as OfficeName, SUM(z.Amount) as AMOUNT " +
//                    "FROM " +
//                    "( " +
//                    "SELECT  " +
//                    "e.OfficeName as OfficeName, " +
//                    "b.OfficeID, c.AccountName, d.OOEID,  " +
//                    "d.OOEAbrevation as OOEAbrevation,  " +
//                    "isnull((SELECT top 1 Amount from tbl_R_AmountHistory where ProposalID = a.ProposalID ORDER BY amountHistoryID DESC),a.ProposalAmount) as Amount,   " +
//                    "d.OOEName as OOEName " +
//                    "FROM tbl_T_BMSBudgetProposal as a  " +
//                    "LEFT JOIN tbl_R_BMSOfficePrograms as b ON b.ProgramID = a.ProgramID  " +
//                    "LEFT JOIN tbl_R_BMSProgramAccounts as c ON c.ProgramID = a.ProgramID and c.AccountID = a.AccountID  " +
//                    "LEFT JOIN tbl_R_BMSOffices as e ON e.OfficeID = b.OfficeID  " +
//                    "LEFT JOIN tbl_R_BMSObjectOfExpenditure as d ON d.OOEID = c.ObjectOfExpendetureID  " +
//                    "WHERE a.ProposalActionCode = 1 and a.ProposalYear = '" + BudgetYear + "' AND  " +
//                    "b.ActionCode = 1 and b.ProgramYear = '" + BudgetYear + "' AND  " +
//                    "c.ActionCode = 1 and c.AccountYear = '" + BudgetYear + "' AND d.OOEID != '' AND a.ProgramID not in(select ProgramID FROM tbl_R_BMSOfficePrograms where OfficeID in(37,38,41) and ProgramYear = a.ProposalYear and ActionCode = a.ProposalActionCode) " +

//                    "UNION " +

//                    "SELECT * FROM @TempTable  " +

//                    "UNION " +

//                                        "SELECT c.OfficeName as OfficeName,  " +
//                                        "b.OfficeID, b.AccountName as AccountName,  " +
//                                        "d.OOEID, d.OOEAbrevation as OOEAbrevation, " +
//                                        "isnull((SELECT top 1 Amount from tbl_R_AmountHistory where ProposalID = a.ProposalID ORDER BY amountHistoryID DESC),a.ProposalAmount) as Amount,   " +
//                                        "d.OOEName as OOEName " +
//                                        "FROM tbl_T_BMSBudgetProposal as a " +
//                                        "LEFT JOIN tbl_R_BMSProposedAccounts as b ON b.ProgramID = a.ProgramID and b.AccountID = a.AccountID " +
//                                        "LEFT JOIN tbl_R_BMSOffices as c ON c.OfficeID = b.OfficeID " +
//                                        "LEFT JOIN tbl_R_BMSObjectOfExpenditure as d ON d.OOEID = b.OOEID " +
//                                        "WHERE " +
//                                        "a.ProposalActionCode = 1 and a.ProposalYear = '" + BudgetYear + "' AND " +
//                                        "b.ActionCode = 1 and b.ProposalYear = '" + BudgetYear + "' AND d.OOEID != '' AND a.ProgramID not in(select ProgramID FROM tbl_R_BMSOfficePrograms where OfficeID in(37,38,41) and ProgramYear = a.ProposalYear and ActionCode = a.ProposalActionCode) " +

//                    ") as z " +
//                    "WHERE z.OfficeID not in(37,38,41) " +
//                    "GROUP BY z.OfficeID " +
//                    "ORDER BY z.OfficeID", con);
                SqlDataReader reader_selectTotal = selectTotal.ExecuteReader();

                while (reader_selectTotal.Read())
                {
                    DashBoardModel dataList = new DashBoardModel();
                    dataList.OfficeID = Convert.ToInt32(reader_selectTotal.GetValue(0));
                    dataList.OfficeLegend = Convert.ToString(reader_selectTotal.GetValue(1));
                    dataList.TotalAmountOffice = Convert.ToDouble(reader_selectTotal.GetValue(2));
                    PvsPList.Add(dataList);
                    
                }
            }
            return PvsPList;
        }

        public IEnumerable<DashBoardModel> TotalSourceOfFunds(int? BudgetYear=0 )
        {
            List<DashBoardModel> PvsPList = new List<DashBoardModel>();
            if (BudgetYear <= 2017)
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    con.Open();

                    SqlCommand select_funds = new SqlCommand(@"SELECT a.Type_ID, MIN(a.Type_Desc) as Type_Desc, ISNULL(SUM(c.Year3_Amount),0) as Amount, MIN(d.Fund_Desc) as Fund_Desc FROM tbl_R_BMS_A_TypeFunds as a
                            LEFT JOIN tbl_R_BMS_A_Sub1 as b ON b.Type_ID = a.Type_ID
                            LEFT JOIN tbl_R_BMS_A_Sub2 as c ON c.Sub1_ID = b.Sub1_ID
                            LEFT JOIN tbl_R_BMS_A_SourceFunds as d ON d.Fund_ID = a.Fund_ID
                            WHERE a.Action_Code = 1 and a.Year_Of = '" + BudgetYear + "' AND " +
                                "b.Action_Code = 1 and b.Year_Of = '" + BudgetYear + "' AND " +
                                "c.Action_Code = 1 and c.Year_Of = '" + BudgetYear + "' AND c.Year3_Amount != 0 " +
                                "GROUP BY a.Type_ID " +
                                "ORDER BY a.Type_ID", con);
                    SqlDataReader reader_funds = select_funds.ExecuteReader();
                    while (reader_funds.Read())
                    {
                        DashBoardModel dataList = new DashBoardModel();
                        dataList.SourceOfFundsID = Convert.ToString(reader_funds.GetValue(0));
                        var type_desc = Convert.ToString(reader_funds.GetValue(1));
                        var fund_desc = Convert.ToString(reader_funds.GetValue(3));
                        dataList.LegendSourceOfFunds = type_desc + "(" + fund_desc + ")";
                        dataList.SourceOfFundsAmount = Convert.ToDouble(reader_funds.GetValue(2));
                        PvsPList.Add(dataList);
                    }
                }    
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    con.Open();

                    SqlCommand select_funds = new SqlCommand(@"WITH Hierarchy(Parents,SeriesID,isUseTotalInGraph,Particular,ItemLevel)
                                                AS
                                                (
                                                SELECT CAST(SeriesID AS VARCHAR(MAX)),SeriesID,isUseTotalInGraph,Particular,ItemLevel
                                                FROM tbl_R_BMSLBPForm1Data AS FirtGeneration
                                                WHERE MainSeriesID IS NULL    
                                                UNION ALL
                                                SELECT CAST(CASE WHEN Parent.Parents = ''
                                                THEN(CAST(NextGeneration.MainSeriesID AS VARCHAR(MAX)) + '.' + CAST(NextGeneration.SeriesID AS VARCHAR(MAX)))
                                                ELSE(Parent.Parents + '.' + CAST(NextGeneration.SeriesID AS VARCHAR(MAX)))
                                                END AS VARCHAR(MAX)),
                                                NextGeneration.SeriesID,NextGeneration.isUseTotalInGraph,NextGeneration.Particular,NextGeneration.ItemLevel
                                                FROM tbl_R_BMSLBPForm1Data AS NextGeneration
                                                INNER JOIN Hierarchy AS Parent ON NextGeneration.MainSeriesID = Parent.SeriesID    
                                                WHERE NextGeneration.ActionCode = 1 and NextGeneration.YearOf = " + BudgetYear + " and NextGeneration.FundType = 1" + ""
                                                + ") SELECT (select SeriesID from fn_BMS_GetLBPForm1GroupFooterTotal(Hierarchy.Parents," + BudgetYear + ",1)) as SeriesID, " + ""
                                                + " Particular as Particular,'' as AccountCode, " + ""
                                                + " '' as IncomeClassification, (select PastYear from fn_BMS_GetLBPForm1GroupFooterTotal(Hierarchy.Parents," + BudgetYear + ",1)) as PastYear, " + ""
                                                + " (select CurrentYear from fn_BMS_GetLBPForm1GroupFooterTotal(Hierarchy.Parents," + BudgetYear + ",1)) as CurrentYear, " + ""
                                                + " isnull((select BudgetYear from fn_BMS_GetLBPForm1GroupFooterTotal(Hierarchy.Parents," + BudgetYear + ",1)),0) as BudgetYear, " + ""
                                                + " (select OrderNo + 1 from fn_BMS_GetLBPForm1GroupFooterTotal(Hierarchy.Parents," + BudgetYear + ",1)) as OrderNo, " + ""
                                                + " (select isBold from fn_BMS_GetLBPForm1GroupFooterTotal(Hierarchy.Parents," + BudgetYear + ",1)) as isBold " + ""
                                                + " FROM Hierarchy where isUseTotalInGraph = 1 OPTION(MAXRECURSION 32767)", con);
                    SqlDataReader reader_funds = select_funds.ExecuteReader();
                    while (reader_funds.Read())
                    {
                        DashBoardModel dataList = new DashBoardModel();
                        //dataList.SourceOfFundsID = Convert.ToString(reader_funds.GetValue(0));
                        var type_desc = Convert.ToString(reader_funds.GetValue(1));
                        //var fund_desc = Convert.ToString(reader_funds.GetValue(3));
                        dataList.LegendSourceOfFunds = type_desc;
                        dataList.SourceOfFundsAmount = Convert.ToDouble(reader_funds.GetValue(6));
                        PvsPList.Add(dataList);
                    }
                }
            }
            return PvsPList;
        }
        public IEnumerable<OfficeListModel> grOfficeListData(string gridData,int YearOf)
        {
            var Query = "";
            if (gridData == "Submitted")
            {
                //Query = @"SELECT c.OfficeID, c.OfficeName, Concat(count(a.ProposalID), ' Out Of ',
                //        (SELECT Count(ProgramAccountID) FROM tbl_R_BMSProgramAccounts as aa
                //        LEFT JOIN tbl_R_BMSOfficePrograms as bb on bb.ProgramID = aa.ProgramID 
                //        and bb.ProgramYear = aa.AccountYear
                //        and aa.ActionCode = bb.ActionCode
                //        where aa.ActionCode = 1 and aa.AccountYear = "+YearOf
                //        + " and bb.OfficeID = c.OfficeID and aa.AccountName is not null) + (select Count(ProposedID)  from tbl_R_BMSProposedAccounts where OfficeID = c.OfficeID and ProgramID != 0 and ProposalYear = " + YearOf + " and ActionCode = 1)), " + ""
                //        +" Concat(FORMAT(min(cast(ProposalDateTime as DATE)),'MMMM dd, yyyy'),' - ' "+""
                //        +" ,FORMAT(max(cast(ProposalDateTime as DATE)),'MMMM dd, yyyy')), "+""
                //        +" case when count(a.ProposalID) >= "+""
                //        +" (SELECT Count(ProgramAccountID) FROM tbl_R_BMSProgramAccounts as aa "+""
                //        +" LEFT JOIN tbl_R_BMSOfficePrograms as bb on bb.ProgramID = aa.ProgramID "+""
                //        +" and bb.ProgramYear = aa.AccountYear "+""
                //        +" and aa.ActionCode = bb.ActionCode "+""
                //        +" where aa.ActionCode = 1 and aa.AccountYear = "+YearOf+" "+""
                //        +" and bb.OfficeID = c.OfficeID) + (select Count(ProposedID)  from tbl_R_BMSProposedAccounts where OfficeID = c.OfficeID and ProposalYear = "+YearOf+" and ActionCode = 1) "+""
                //        +" then 1 else 0 end "+""
                //        +" from tbl_T_BMSBudgetProposal as a "+""
                //        +" LEFT JOIN tbl_R_BMSOfficePrograms as b on b.ProgramID = a.ProgramID and b.ActionCode = 1 and b.ProgramYear = a.ProposalYear "+""
                //        +" LEFT JOIN tbl_R_BMSOffices as c on c.OfficeID = b.OfficeID "+""
                //        +" where a.Proposalyear = "+YearOf+" "+""
                //        +" GROUP BY c.OfficeID, c.OfficeName";
                Query = @"exec sp_BMS_SubmittedProposal "+ YearOf + "";
            }
            else if (gridData == "With Proposal Not yet Submitted")
            {
                //Query = @"with cte as(select count(b.OfficeID) as cnt,b.OfficeName from tbl_T_BMSAccountDenomination as a 
                //          LEFT JOIN tbl_R_BMSOffices as b on b.OfficeID = a.OfficeID
                //          where TransactionYear = "+YearOf+ " and ActionCode = 1 and b.OfficeName not like '%sef-%' and a.OfficeID not in(  " + ""
                //        +" SELECT c.OfficeID as Cnt  from tbl_T_BMSBudgetProposal as a "+""
                //        +" LEFT JOIN tbl_R_BMSOfficePrograms as b on b.ProgramID = a.ProgramID  "+""
                //        +" and b.ProgramYear = a.ProposalYear "+""
                //        +" LEFT JOIN tbl_R_BMSOffices as c on c.OfficeID = b.OfficeID  "+""
                //        +" where a.Proposalyear = "+YearOf+ " and ProposalActionCOde = 1 and b.ActionCode=1 " + ""
                //        +" ) GROUP BY b.OfficeID, b.OfficeName) "+""
                //        + " select *,'','',0 from cte order by officeName";
                Query = @"sp_BMS_NotYetSubmittedProposal " + YearOf + "";
            }
            else if (gridData == "No Proposal")
            {
                Query = "sp_BMS_getOfficesWithoutProposal " + YearOf + "";
            }
            List<OfficeListModel> List = new List<OfficeListModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(Query, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    OfficeListModel Item = new OfficeListModel();
                    Item.OfficeName = reader.GetValue(1).ToString();
                    Item.NoofAccountsSubmitted = reader.GetValue(2).ToString();
                    Item.DateTimeSubmitted = reader.GetValue(3).ToString();
                    Item.isComplete = Convert.ToInt32(reader.GetValue(4));
                    List.Add(Item);
                }
            }


            return List;
        }
        public IEnumerable<DashBoardModel> TotalPSMooeCAConsolidated(int? BudgetYear = 0)
        {
            List<DashBoardModel> PvsPList = new List<DashBoardModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                SqlCommand selectTotal = new SqlCommand(@"exec sp_BMS_PieChartPSMOOECOConsolidated'" + BudgetYear + "'", con);
                //SqlCommand selectTotal = new SqlCommand(@"exec sp_BMS_PieChartPerOfficeExpense 2019,4", con);
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
        public IEnumerable<DashBoardModel> TotalFilledVacantPosition(int? BudgetYear = 0)
        {
            List<DashBoardModel> PvsPList = new List<DashBoardModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                SqlCommand selectTotal = new SqlCommand(@"exec sp_BMS_PieChartFilledVacantPos'" + BudgetYear + "',1", con);
                SqlDataReader reader_selectTotal = selectTotal.ExecuteReader();

                while (reader_selectTotal.Read())
                {
                    DashBoardModel dataList = new DashBoardModel();
                    dataList.OOEIDPie = Convert.ToInt32(reader_selectTotal.GetValue(0));
                    dataList.LegendPiePSMooeCA = Convert.ToString(reader_selectTotal.GetValue(1));
                    dataList.TotalPSMooeCA = Convert.ToDouble(reader_selectTotal.GetValue(2));
                    PvsPList.Add(dataList);
                }
            }
            return PvsPList;
        }
        public IEnumerable<DashBoardModel> AppropriationSumm(int? BudgetYear = 0)
        {
            List<DashBoardModel> PvsPList = new List<DashBoardModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Open();
                SqlCommand selectTotal = new SqlCommand(@"exec [sp_bms_appropriationsummary]'" + BudgetYear + "',12", con);
                selectTotal.CommandTimeout = 0;
                SqlDataReader reader_selectTotal = selectTotal.ExecuteReader();

                while (reader_selectTotal.Read())
                {
                    DashBoardModel dataList = new DashBoardModel();
                    dataList.OfficeID = Convert.ToInt32(reader_selectTotal.GetValue(0));
                    dataList.OfficeLegend = Convert.ToString(reader_selectTotal.GetValue(8));
                    dataList.TotalAmountOffice = Convert.ToDouble(reader_selectTotal.GetValue(1));
                   
                    PvsPList.Add(dataList);
                   // Session["isCasual"] = Convert.ToDouble(reader_selectTotal.GetValue(4));
                    //ViewBag.totalapp    = Convert.ToDouble(reader_selectTotal.GetValue(4));

                }
            }
            return PvsPList;
        }
        public IEnumerable<DashBoardModel> FinUtilization(int? BudgetYear = 0,int? fundsource=0)
        {
            List<DashBoardModel> prog = new List<DashBoardModel>();
            if (fundsource == 1)
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    //SqlCommand com = new SqlCommand(@"SELECT sum(appropriation), sum(allotment) ,sum(pre_obligation),sum(obligation),sum(disbursement),sum(utilisation),(sum(allotment)/sum(appropriation)) * 100 as AllPct ,
                    //                            (sum(pre_obligation)/sum(appropriation)) * 100 as PreopPct,(sum(obligation)/sum(appropriation)) * 100 as ObligationPct,
                    //         
                    SqlCommand com = new SqlCommand(@"SELECT isnull(sum(appropriation),0), isnull(sum(allotment),0) ,isnull(sum(pre_obligation),0),isnull(sum(obligation),0),isnull(sum(disbursement),0),
                                    isnull(sum(utilisation),0),isnull(((sum(allotment)/sum(appropriation)) * 100),0) as AllPct ,
                                                isnull(((sum(pre_obligation)/sum(appropriation)) * 100),0) as PreopPct,isnull((sum(obligation)/sum(appropriation)) * 100,0) as ObligationPct,
                                                isnull((sum(disbursement)/sum(appropriation)) * 100,0) as DisbursePct,isnull((sum(utilisation)/sum(appropriation)) * 100,0) as UtizationPct,100 as AppPct  
			                                    FROM [IFMIS].[dbo].[tbl_T_BMSUtilization_Dashboard] where yearof=" + BudgetYear + " and office is not null and accountid !=0 ", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        DashBoardModel emp1 = new DashBoardModel();
                        DashBoardModel emp2 = new DashBoardModel();
                        DashBoardModel emp3 = new DashBoardModel();
                        DashBoardModel emp4 = new DashBoardModel();
                        DashBoardModel emp5 = new DashBoardModel();
                        DashBoardModel emp6 = new DashBoardModel();
                        DashBoardModel emp7 = new DashBoardModel();

                        emp1.col1 = Convert.ToDouble(reader.GetValue(0));
                        //emp1.col2 = Convert.ToDouble(reader.GetValue(1)); // stack
                        emp1.col2 = 0;
                        //Convert.ToDouble(reader.GetValue(1));

                        emp2.col2 = Convert.ToDouble(reader.GetValue(1));
                        emp2.col1 = 0;
                        emp3.col3 = Convert.ToDouble(reader.GetValue(2));
                        emp4.col4 = Convert.ToDouble(reader.GetValue(3));
                        emp5.col5 = Convert.ToDouble(reader.GetValue(4));
                        emp6.col6 = Convert.ToDouble(reader.GetValue(5));
                        emp2.col7 = Convert.ToDouble(reader.GetValue(6));
                        emp3.col8 = Convert.ToDouble(reader.GetValue(7));
                        emp4.col9 = Convert.ToDouble(reader.GetValue(8));
                        emp5.col10 = Convert.ToDouble(reader.GetValue(9));
                        emp6.col11 = Convert.ToDouble(reader.GetValue(10));
                        emp1.col12 = Convert.ToDouble(reader.GetValue(11));

                        prog.Add(emp1);
                        prog.Add(emp2);
                        prog.Add(emp3);
                        prog.Add(emp4);
                        prog.Add(emp5);
                        prog.Add(emp6);
                        //     prog.Add(emp7);
                    }
                }
            }
            else
            {
                if (fundsource == 101)
                {
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"SELECT sum(appropriation), sum(allotment) ,sum(pre_obligation),sum(obligation),sum(disbursement),sum(utilisation),(sum(allotment)/sum(appropriation)) * 100 as AllPct ,
                                                (sum(pre_obligation)/sum(appropriation)) * 100 as PreopPct,(sum(obligation)/sum(appropriation)) * 100 as ObligationPct,
                                                (sum(disbursement)/sum(appropriation)) * 100 as DisbursePct,(sum(utilisation)/sum(appropriation)) * 100 as UtizationPct,100 as AppPct  FROM [IFMIS].[dbo].[tbl_T_BMSUtilization_Dashboard] where yearof=" + BudgetYear + " and fundid=" + fundsource + " and programid != 218 and accountid not in (59410,59802) and accountid !=0 ", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            DashBoardModel emp1 = new DashBoardModel();
                            DashBoardModel emp2 = new DashBoardModel();
                            DashBoardModel emp3 = new DashBoardModel();
                            DashBoardModel emp4 = new DashBoardModel();
                            DashBoardModel emp5 = new DashBoardModel();
                            DashBoardModel emp6 = new DashBoardModel();
                            DashBoardModel emp7 = new DashBoardModel();

                            emp1.col1 = Convert.ToDouble(reader.GetValue(0));
                            //emp1.col2 = Convert.ToDouble(reader.GetValue(1)); // stack
                            emp1.col2 = 0;
                            //Convert.ToDouble(reader.GetValue(1));

                            emp2.col2 = Convert.ToDouble(reader.GetValue(1));
                            emp2.col1 = 0;
                            emp3.col3 = Convert.ToDouble(reader.GetValue(2));
                            emp4.col4 = Convert.ToDouble(reader.GetValue(3));
                            emp5.col5 = Convert.ToDouble(reader.GetValue(4));
                            emp6.col6 = Convert.ToDouble(reader.GetValue(5));
                            emp2.col7 = Convert.ToDouble(reader.GetValue(6));
                            emp3.col8 = Convert.ToDouble(reader.GetValue(7));
                            emp4.col9 = Convert.ToDouble(reader.GetValue(8));
                            emp5.col10 = Convert.ToDouble(reader.GetValue(9));
                            emp6.col11 = Convert.ToDouble(reader.GetValue(10));
                            emp1.col12 = Convert.ToDouble(reader.GetValue(11));

                            prog.Add(emp1);
                            prog.Add(emp2);
                            prog.Add(emp3);
                            prog.Add(emp4);
                            prog.Add(emp5);
                            prog.Add(emp6);
                            //     prog.Add(emp7);
                        }
                    }
                }
                else if (fundsource == 127)
                {
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"SELECT sum(appropriation), sum(allotment) ,sum(pre_obligation),sum(obligation),sum(disbursement),sum(utilisation),(sum(allotment)/sum(appropriation)) * 100 as AllPct ,
                                                (sum(pre_obligation)/sum(appropriation)) * 100 as PreopPct,(sum(obligation)/sum(appropriation)) * 100 as ObligationPct,
                                                (sum(disbursement)/sum(appropriation)) * 100 as DisbursePct,(sum(utilisation)/sum(appropriation)) * 100 as UtizationPct,100 as AppPct  FROM [IFMIS].[dbo].[tbl_T_BMSUtilization_Dashboard] where yearof=" + BudgetYear + " and fundid=101 and programid = 218 and accountid !=0 ", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            DashBoardModel emp1 = new DashBoardModel();
                            DashBoardModel emp2 = new DashBoardModel();
                            DashBoardModel emp3 = new DashBoardModel();
                            DashBoardModel emp4 = new DashBoardModel();
                            DashBoardModel emp5 = new DashBoardModel();
                            DashBoardModel emp6 = new DashBoardModel();
                            DashBoardModel emp7 = new DashBoardModel();

                            emp1.col1 = Convert.ToDouble(reader.GetValue(0));
                            //emp1.col2 = Convert.ToDouble(reader.GetValue(1)); // stack
                            emp1.col2 = 0;
                            //Convert.ToDouble(reader.GetValue(1));

                            emp2.col2 = Convert.ToDouble(reader.GetValue(1));
                            emp2.col1 = 0;
                            emp3.col3 = Convert.ToDouble(reader.GetValue(2));
                            emp4.col4 = Convert.ToDouble(reader.GetValue(3));
                            emp5.col5 = Convert.ToDouble(reader.GetValue(4));
                            emp6.col6 = Convert.ToDouble(reader.GetValue(5));
                            emp2.col7 = Convert.ToDouble(reader.GetValue(6));
                            emp3.col8 = Convert.ToDouble(reader.GetValue(7));
                            emp4.col9 = Convert.ToDouble(reader.GetValue(8));
                            emp5.col10 = Convert.ToDouble(reader.GetValue(9));
                            emp6.col11 = Convert.ToDouble(reader.GetValue(10));
                            emp1.col12 = Convert.ToDouble(reader.GetValue(11));

                            prog.Add(emp1);
                            prog.Add(emp2);
                            prog.Add(emp3);
                            prog.Add(emp4);
                            prog.Add(emp5);
                            prog.Add(emp6);
                            //     prog.Add(emp7);
                        }
                    }
                }
                else if (fundsource == 200)
                {
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"SELECT sum(appropriation), sum(allotment) ,sum(pre_obligation),sum(obligation),sum(disbursement),sum(utilisation),(sum(allotment)/sum(appropriation)) * 100 as AllPct ,
                                                (sum(pre_obligation)/sum(appropriation)) * 100 as PreopPct,(sum(obligation)/sum(appropriation)) * 100 as ObligationPct,
                                                (sum(disbursement)/sum(appropriation)) * 100 as DisbursePct,(sum(utilisation)/sum(appropriation)) * 100 as UtizationPct,100 as AppPct  FROM [IFMIS].[dbo].[tbl_T_BMSUtilization_Dashboard] where yearof=" + BudgetYear + " and fundid=101 and programid = 211 and accountid in (59410,59802)", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            DashBoardModel emp1 = new DashBoardModel();
                            DashBoardModel emp2 = new DashBoardModel();
                            DashBoardModel emp3 = new DashBoardModel();
                            DashBoardModel emp4 = new DashBoardModel();
                            DashBoardModel emp5 = new DashBoardModel();
                            DashBoardModel emp6 = new DashBoardModel();
                            DashBoardModel emp7 = new DashBoardModel();

                            emp1.col1 = Convert.ToDouble(reader.GetValue(0));
                            //emp1.col2 = Convert.ToDouble(reader.GetValue(1)); // stack
                            emp1.col2 = 0;
                            //Convert.ToDouble(reader.GetValue(1));

                            emp2.col2 = Convert.ToDouble(reader.GetValue(1));
                            emp2.col1 = 0;
                            emp3.col3 = Convert.ToDouble(reader.GetValue(2));
                            emp4.col4 = Convert.ToDouble(reader.GetValue(3));
                            emp5.col5 = Convert.ToDouble(reader.GetValue(4));
                            emp6.col6 = Convert.ToDouble(reader.GetValue(5));
                            emp2.col7 = Convert.ToDouble(reader.GetValue(6));
                            emp3.col8 = Convert.ToDouble(reader.GetValue(7));
                            emp4.col9 = Convert.ToDouble(reader.GetValue(8));
                            emp5.col10 = Convert.ToDouble(reader.GetValue(9));
                            emp6.col11 = Convert.ToDouble(reader.GetValue(10));
                            emp1.col12 = Convert.ToDouble(reader.GetValue(11));

                            prog.Add(emp1);
                            prog.Add(emp2);
                            prog.Add(emp3);
                            prog.Add(emp4);
                            prog.Add(emp5);
                            prog.Add(emp6);
                            //     prog.Add(emp7);
                        }
                    }
                }
                else
                {
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"SELECT sum(appropriation), sum(allotment) ,sum(pre_obligation),sum(obligation),sum(disbursement),sum(utilisation),(sum(allotment)/sum(appropriation)) * 100 as AllPct ,
                                                (sum(pre_obligation)/sum(appropriation)) * 100 as PreopPct,(sum(obligation)/sum(appropriation)) * 100 as ObligationPct,
                                                (sum(disbursement)/sum(appropriation)) * 100 as DisbursePct,(sum(utilisation)/sum(appropriation)) * 100 as UtizationPct,100 as AppPct  FROM [IFMIS].[dbo].[tbl_T_BMSUtilization_Dashboard] where yearof=" + BudgetYear + " and fundid=" + fundsource + " and accountid !=0 ", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            DashBoardModel emp1 = new DashBoardModel();
                            DashBoardModel emp2 = new DashBoardModel();
                            DashBoardModel emp3 = new DashBoardModel();
                            DashBoardModel emp4 = new DashBoardModel();
                            DashBoardModel emp5 = new DashBoardModel();
                            DashBoardModel emp6 = new DashBoardModel();
                            DashBoardModel emp7 = new DashBoardModel();

                            emp1.col1 = Convert.ToDouble(reader.GetValue(0));
                            //emp1.col2 = Convert.ToDouble(reader.GetValue(1)); // stack
                            emp1.col2 = 0;
                            //Convert.ToDouble(reader.GetValue(1));

                            emp2.col2 = Convert.ToDouble(reader.GetValue(1));
                            emp2.col1 = 0;
                            emp3.col3 = Convert.ToDouble(reader.GetValue(2));
                            emp4.col4 = Convert.ToDouble(reader.GetValue(3));
                            emp5.col5 = Convert.ToDouble(reader.GetValue(4));
                            emp6.col6 = Convert.ToDouble(reader.GetValue(5));
                            emp2.col7 = Convert.ToDouble(reader.GetValue(6));
                            emp3.col8 = Convert.ToDouble(reader.GetValue(7));
                            emp4.col9 = Convert.ToDouble(reader.GetValue(8));
                            emp5.col10 = Convert.ToDouble(reader.GetValue(9));
                            emp6.col11 = Convert.ToDouble(reader.GetValue(10));
                            emp1.col12 = Convert.ToDouble(reader.GetValue(11));

                            prog.Add(emp1);
                            prog.Add(emp2);
                            prog.Add(emp3);
                            prog.Add(emp4);
                            prog.Add(emp5);
                            prog.Add(emp6);
                            //     prog.Add(emp7);
                        }
                    }
                }
            }
            return prog;

        }

    }
}