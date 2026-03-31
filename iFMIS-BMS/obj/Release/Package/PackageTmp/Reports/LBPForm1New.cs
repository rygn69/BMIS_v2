namespace iFMIS_BMS.Reports
{
    using iFMIS_BMS.BusinessLayer.Connector;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using iFMIS_BMS.Classes;

    /// <summary>
    /// Summary description for LBPForm1New.
    /// </summary>
    public partial class LBPForm1New : Telerik.Reporting.Report
    {
        public LBPForm1New(int YearOf,int FundType, long eid)
        {
            InitializeComponent();
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select SeriesID,Concat((select dbo.fn_BMS_GetIndent(ItemLevel)),RowNo,Particular) as Particular,AccountCode,IncomeClassification,PastYear,CurrentYear,BudgetYear,OrderNo,isBold,Obligated from tbl_R_BMSLBPForm1Data WHERE ActionCode = 1 and YearOf = "+ YearOf +" and FundType = "+ FundType +"", con);
                con.Open();
                dt.Load(com.ExecuteReader());
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"WITH Hierarchy(Parents,SeriesID,hasFooterTotal,Particular,ItemLevel)
                                                AS
                                                (
                                                SELECT CAST(SeriesID AS VARCHAR(MAX)),SeriesID,hasFooterTotal,Particular,ItemLevel
                                                FROM tbl_R_BMSLBPForm1Data AS FirtGeneration
                                                WHERE MainSeriesID IS NULL and ActionCode = 1 and YearOf = "+ YearOf +" and FundType = "+ FundType +""  
                                                +" UNION ALL "+""
                                                +" SELECT CAST(CASE WHEN Parent.Parents = '' "+""
                                                +" THEN(CAST(NextGeneration.MainSeriesID AS VARCHAR(MAX)) + '.' + CAST(NextGeneration.SeriesID AS VARCHAR(MAX))) "+""
                                                +" ELSE(Parent.Parents + '.' + CAST(NextGeneration.SeriesID AS VARCHAR(MAX))) "+""
                                                +" END AS VARCHAR(MAX)), "+""
                                                +" NextGeneration.SeriesID,NextGeneration.hasFooterTotal,NextGeneration.Particular,NextGeneration.ItemLevel "+""
                                                +" FROM tbl_R_BMSLBPForm1Data AS NextGeneration "+""
                                                +" INNER JOIN Hierarchy AS Parent ON NextGeneration.MainSeriesID = Parent.SeriesID "+""
                                                +" WHERE NextGeneration.ActionCode = 1 and NextGeneration.YearOf = " + YearOf + " and NextGeneration.FundType = " + FundType
                                                + ") SELECT (select SeriesID from fn_BMS_GetLBPForm1GroupFooterTotal(Hierarchy.Parents," + YearOf + "," + FundType + ")) as SeriesID, " + ""
                                                + " Concat((select dbo.fn_BMS_GetIndent(ItemLevel)),'Total ', Particular) as Particular,'' as AccountCode, " + ""
                                                + " '' as IncomeClassification, (select PastYear from fn_BMS_GetLBPForm1GroupFooterTotal(Hierarchy.Parents," + YearOf + "," + FundType + ")) as PastYear, " + ""
                                                + " (select CurrentYear from fn_BMS_GetLBPForm1GroupFooterTotal(Hierarchy.Parents," + YearOf + "," + FundType + ")) as CurrentYear, " + ""
                                                + " (select BudgetYear from fn_BMS_GetLBPForm1GroupFooterTotal(Hierarchy.Parents," + YearOf + "," + FundType + ")) as BudgetYear, " + ""
                                                + " (select OrderNo + "
                                                + " (case when Hierarchy.seriesid=20135 then 6 "
                                                + " when Hierarchy.seriesid = 20123 then 4 "
                                                + " when Hierarchy.seriesid = 20122 then 6 "
                                                + " when Hierarchy.seriesid = 20121 or Hierarchy.seriesid =20119 then 5 "
                                                + " else 1 end)"
                                                + " from fn_BMS_GetLBPForm1GroupFooterTotal(Hierarchy.Parents," + YearOf + "," + FundType + ")) as OrderNo, " + ""
                                                + " (select isBold from fn_BMS_GetLBPForm1GroupFooterTotal(Hierarchy.Parents," + YearOf + "," + FundType + ")) as isBold, " + ""
                                                + " (select Obligated from fn_BMS_GetLBPForm1GroupFooterTotal(Hierarchy.Parents," + YearOf + "," + FundType + ")) as Obligated " + ""
                                                + " FROM Hierarchy where hasFooterTotal = 1 OPTION(MAXRECURSION 32767)", con);
                con.Open();
                dt.Load(com.ExecuteReader());
            }
            DataRow[] foundRows;
            foundRows = dt.Select("OrderNo > 0", "orderNo Desc");
            int LastOrderNo = Convert.ToInt32(foundRows[0][7]);
            int FundID = FundType == 1 ? 101 : 119;
                //Generate Object Of Expenditures as Header
            #region Old Query
            //            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            //            {
            //                SqlCommand com = new SqlCommand(@"select max(a.AccountID),isnull(min(a.OrderNo),ROW_NUMBER() OVER(PARTITION BY a.ObjectOfExpendetureID ORDER BY a.ObjectOfExpendetureID,min(a.AccountID) ASC)) - 1 as SeriesID,Concat((select dbo.fn_BMS_GetIndent(1)),e.OOEName) as Particular,'' as AccountCode,
            //                                                        '' as IncomeClassification,NULL as PastYear,NULL as CurrentYear,NULL as BudgetYear,
            //                                                        a.ObjectOfExpendetureID + " + LastOrderNo + " as OrderNo,1 as IsBold  " + ""
            //                                                    + " from tbl_R_BMSProgramAccounts as a " + ""
            //                                                    + " LEFT JOIN tbl_R_BMSPPSASCode as b on b.FMISAccountCode = a.AccountID and a.AccountYear = b.YearOf and a.ActionCode = b.ActionCode " + ""
            //                                                    + " LEFT JOIN tbl_T_BMSBudgetProposal as c on c.AccountID = a.AccountID and c.ProgramID = a.ProgramID and c.ProposalYear = a.AccountYear and c.ProposalActionCode = a.ActionCode " + ""
            //                                                    + " LEFT JOIN tbl_R_BMSOfficePrograms as d on d.ProgramID = a.ProgramID and d.ProgramYear = a.AccountYear and d.ActionCode = a.ActionCode " + ""
            //                                                    + " LEFT JOIN tbl_R_BMSObjectOfExpenditure as e on e.OOEID = a.ObjectOfExpendetureID " + ""
            //                                                    + " where a.AccountYear = " + YearOf + " and a.ActionCode = 1 and d.OfficeID " + inStatement + "(37,38,41) " + ""
            //                                                    + " GROUP BY a.ObjectOfExpendetureID,e.OOEName", con);
            //                con.Open();
            //                dt.Load(com.ExecuteReader());
            //            } 
            #endregion

                 #region Modified 14-10-2017 Saturday
//		   using (SqlConnection con = new SqlConnection(Common.MyConn()))
//                    {
//                        SqlCommand com = new SqlCommand(@"select max(AccountID),
//                                                        isnull(min(OrderNo),ROW_NUMBER() OVER(PARTITION BY isnull(OOEID,2) ORDER BY isnull(OOEID,2),min(AccountID) ASC)) - 1 as SeriesID,
//                                                        Concat((select dbo.fn_BMS_GetIndent(1)),isnull(OOEName,'Maintenance and Other Operating Expenses')) as Particular,
//                                                        '' as AccountCode,'' as IncomeClassification, NULL as PastYear,null as CurrentYear,NULL as BudgetYear,
//                                                        isnull(OOEID,2) + " + LastOrderNo + " as OrderNo,1 as IsBold  "+""
//                                                        +" from tbl_R_BMSConsoliditedTotal "+""
//                                                        + " where OfficeID in(select OfficeID from tbl_R_BMSOffices where OrderNo != 0 and FundID= " + FundID + ") and YEarOf = " + YearOf 
//                                                        +" GROUP BY isnull(OOEID,2),isnull(OOEName,'Maintenance and Other Operating Expenses')", con);
//                        con.Open();
//                        dt.Load(com.ExecuteReader());
//                    } 
	#endregion
            //PS Header
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select max(AccountID),
                                                                        isnull(min(OrderNo),ROW_NUMBER() OVER(PARTITION BY isnull(OOEID,2) ORDER BY isnull(OOEID,2),min(AccountID) ASC)) - 1 as SeriesID,
                                                                        Concat((select dbo.fn_BMS_GetIndent(1)),isnull(OOEName,'Maintenance and Other Operating Expenses')) as Particular,
                                                                        '' as AccountCode,'' as IncomeClassification, NULL as PastYear,null as CurrentYear,NULL as BudgetYear,
                                                                        isnull(OOEID,2) + " + LastOrderNo + " as OrderNo,1 as IsBold  " + ""
                                                + " from tbl_R_BMSConsoliditedTotal " + ""
                                                + " where OOEID = 1 and OfficeID in(select OfficeID from tbl_R_BMSOffices where OrderNo is not null and FundID= " + FundID + ") and YEarOf = " + YearOf
                                                + " GROUP BY isnull(OOEID,2),isnull(OOEName,'Maintenance and Other Operating Expenses')", con);
                con.Open();
                dt.Load(com.ExecuteReader());
            } 
                //Generate Accounts
                    #region Old Query
                    //using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    //{
                    //    SqlCommand com = new SqlCommand(@"select max(a.AccountID),isnull(max(a.OrderNo),ROW_NUMBER() OVER(PARTITION BY a.ObjectOfExpendetureID ORDER BY a.ObjectOfExpendetureID,max(a.AccountID) ASC)) as SeriesID,Concat((select dbo.fn_BMS_GetIndent(2)),a.AccountName) as Particular,b.PPSASCode as AccountCode, " + ""
                    //                                    + " '' as IncomeClassification,sum(0) as PastYear,sum(f.TotalApproved) as CurrentYear,sum(c.ProposalAllotedAmount) as BudgetYear, " + ""
                    //                                    +" a.ObjectOfExpendetureID + " + LastOrderNo + " as OrderNo,0 as IsBold,sum(f.Obligated) as 'Obligated'  " + ""
                    //                                    +" from tbl_R_BMSProgramAccounts as a "+""
                    //                                    +" LEFT JOIN tbl_R_BMSPPSASCode as b on b.FMISAccountCode = a.AccountID and a.AccountYear = b.YearOf and a.ActionCode = b.ActionCode " + ""
                    //                                    +" LEFT JOIN tbl_T_BMSBudgetProposal as c on c.AccountID = a.AccountID and c.ProgramID = a.ProgramID and c.ProposalYear = a.AccountYear and c.ProposalActionCode = a.ActionCode "+""
                    //                                    +" LEFT JOIN tbl_R_BMSOfficePrograms as d on d.ProgramID = a.ProgramID and d.ProgramYear = a.AccountYear and d.ActionCode = a.ActionCode "+""
                    //                                    +" LEFT JOIN tbl_T_BMSBudgetProposal as e on e.AccountID = a.AccountID and e.ProgramID = a.ProgramID and e.ProposalYear = a.AccountYear - 1 and e.ProposalActionCode = a.ActionCode " + ""
                    //                                    + " LEFT JOIN tbl_R_BMSCurrentYearObligated as f on  f.AccountID = a.AccountID and f.Budgetyear = a.AccountYear and f.OfficeID = d.OfficeID" + ""
                    //                                    + " where a.AccountYear = " + YearOf + " and a.ActionCode = 1 and d.OfficeID " + inStatement + "(37,38,41) " + ""
                    //                                    + " GROUP BY a.ObjectOfExpendetureID,a.AccountName,b.PPSASCode", con);
                    //    con.Open();
                    //    dt.Load(com.ExecuteReader());
                    //} 
                    #endregion

            #region Modified 14-10-2017 Saturday
            //                using (SqlConnection con = new SqlConnection(Common.MyConn()))
            //                {
            //                    SqlCommand com = new SqlCommand(@"select max(a.AccountID),
            //                                                    isnull(max(a.OrderNo),ROW_NUMBER() OVER(PARTITION BY isnull(a.OOEID,2) ORDER BY isnull(a.OOEID,2),max(a.AccountID) ASC)) as SeriesID,
            //                                                    Concat((select dbo.fn_BMS_GetIndent(2)),a.AccountName) as Particular,b.PPSASCode as AccountCode,
            //                                                    '' as IncomeClassification, sum(isnull(a.PastYearApproved,0)) as PastYear, sum(isnull(a.CurrentYearApproved,0)) as CurrentYear,sum(isnull(a.BudgetYearAmount,0)) as BudgetYear,
            //                                                    isnull(a.OOEID,2) + "+LastOrderNo+" as OrderNo,0 as IsBold,sum(isnull(a.Current_Year_Obligated,0)) as Obligated "+""
            //                                                    +" from tbl_R_BMSConsoliditedTotal as a "+""
            //                                                    +" LEFT JOIN tbl_R_BMSPPSASCode as b on FMISAccountCode = a.AccountID and a.YearOf = b.YearOf and b.ActionCode = 1 "+""
            //                                                    + " where a.YearOf = " + YearOf + " and a.OfficeID in(select OfficeID from tbl_R_BMSOffices where OrderNo != 0 and FundID= " + FundID + ") " + ""
            //                                                    +" GROUP BY isnull(a.OOEID,2),a.AccountName,b.PPSASCode", con);
            //                    con.Open();
            //                    dt.Load(com.ExecuteReader());
            //                } 
            #endregion
            //PS
            using (SqlConnection con = new SqlConnection(Common.MyConn())) //ps breakdown
            {
                //SqlCommand com = new SqlCommand(@"
                //                                    with cte as(
                //                                    select *, rn = ROW_NUMBER()OVER(Partition by FMISAccountCode ORder by id) from tbl_R_BMSPPSASCode
                //                                    )
                //                                    select max(a.AccountID),
                //                                                    min(isnull(c.FMISAccountCode,99999)) as SeriesID,
                //                                                    Concat((select dbo.fn_BMS_GetIndent(2)),a.AccountName) as Particular,b.PPSASCode as AccountCode,
                //                                                    '' as IncomeClassification, sum(isnull(a.PastYearApproved,0)) as PastYear, sum(isnull(a.CurrentYearApproved,0)) as CurrentYear,sum(isnull(a.BudgetYearAmount,0)) as BudgetYear,
                //                                                    isnull(a.OOEID,2) + " + LastOrderNo + " as OrderNo,0 as IsBold,sum(isnull(a.Current_Year_Obligated,0)) as Obligated " + ""
                //                                + " from tbl_R_BMSConsoliditedTotal as a " + ""
                //                                + " LEFT JOIN cte as b on FMISAccountCode = a.AccountID and a.YearOf = b.YearOf and b.ActionCode = 1 and b.rn = 1 " + ""
                //                                + " left join tbl_R_BMSAccounts as c on c.FMISAccountCode = a.AccountID and c.Active = 1" + ""
                //                                + " where a.OOEID = 1 and a.YearOf = " + YearOf + " and a.OfficeID in(select OfficeID from tbl_R_BMSOffices where OrderNo is not null and FundID= " + FundID + " and OfficeID != 43) " + ""
                //                                + " GROUP BY isnull(a.OOEID,2),a.AccountName,b.PPSASCode", con);
                SqlCommand com = new SqlCommand(@"exec ifmis.dbo.sp_BMS_LBP1 " + YearOf + "," + FundID + "," + LastOrderNo + ",1,0,"+ eid + "", con);
                con.Open();
                dt.Load(com.ExecuteReader());
            }
            //Non-Office PS
            foundRows = dt.Select("OrderNo > 0", "orderNo Desc");
            LastOrderNo = Convert.ToInt32(foundRows[0][7]);
            //hide on 9/7/2020- double entry from above conde "stored proc- sp_BMS_LBP1"
            //using (SqlConnection con = new SqlConnection(Common.MyConn()))
            //{
            //    SqlCommand com = new SqlCommand(@"
            //                                            with cte as(
            //                                            select *, rn = ROW_NUMBER()OVER(Partition by FMISAccountCode ORder by id) from tbl_R_BMSPPSASCode
            //                                            )
            //                                                        select max(a.AccountID),
            //                                                        min(c.FMISAccountCode) as SeriesID,
            //                                                        Concat((select dbo.fn_BMS_GetIndent(2)),a.AccountName) as Particular,b.PPSASCode as AccountCode,
            //                                                        '' as IncomeClassification, sum(isnull(a.PastYearApproved,0)) as PastYear, sum(isnull(a.CurrentYearApproved,0)) as CurrentYear,sum(isnull(a.BudgetYearAmount,0)) as BudgetYear,
            //                                                        isnull(a.OOEID,2) + " + LastOrderNo + " as OrderNo,0 as IsBold,sum(isnull(a.Current_Year_Obligated,0)) as Obligated " + ""
            //                                    + " from tbl_R_BMSConsoliditedTotal as a " + ""
            //                                    + " LEFT JOIN cte as b on FMISAccountCode = a.AccountID and a.YearOf = b.YearOf and b.ActionCode = 1 and b.rn = 1" + ""
            //                                    + " left join tbl_R_BMSAccounts as c on c.FMISAccountCode = a.AccountID and c.Active = 1" + ""
            //                                    + " where a.OOEID = 1 and a.YearOf = " + YearOf + " and a.OfficeID in(43) " + ""
            //                                    + " GROUP BY isnull(a.OOEID,2),a.AccountName,b.PPSASCode", con);
            //    con.Open();
            //    dt.Load(com.ExecuteReader());
            //}
            //hide on 9/7/2020- double entry from above conde "stored proc- sp_BMS_LBP1" 

            //PS Total
            #region Modified 14-10-2017 Saturday
            //                using (SqlConnection con = new SqlConnection(Common.MyConn()))
            //                {
            //                    SqlCommand com = new SqlCommand(@"select max(AccountID),
            //                                                        isnull(min(OrderNo),ROW_NUMBER() OVER(PARTITION BY isnull(OOEID,2) ORDER BY isnull(OOEID,2),min(AccountID) ASC)) + 999999 as SeriesID,
            //                                                        Concat((select dbo.fn_BMS_GetIndent(1)),'Total ',isnull(OOEName,'Maintenance and Other Operating Expenses')) as Particular,
            //                                                        '' as AccountCode,'' as IncomeClassification, sum(isnull(PastYearApproved,0)) as PastYear,sum(isnull(CurrentYearApproved,0)) as CurrentYear,sum(isnull(BudgetYearAmount,0)) as BudgetYear,
            //                                                        isnull(OOEID,2) + " + LastOrderNo + " as OrderNo,1 as IsBold,sum(isnull(Current_Year_Obligated,0)) as Obligated  " + ""
            //                                                    + " from tbl_R_BMSConsoliditedTotal " + ""
            //                                                    + " where OfficeID in(select OfficeID from tbl_R_BMSOffices where OrderNo != 0 and FundID= " + FundID + ") and YEarOf = " + YearOf
            //                                                    + " GROUP BY isnull(OOEID,2),isnull(OOEName,'Maintenance and Other Operating Expenses')", con);
            //                    con.Open();
            //                    dt.Load(com.ExecuteReader());
            //                } 
            #endregion
            foundRows = dt.Select("OrderNo > 0", "orderNo Desc");
            LastOrderNo = Convert.ToInt32(foundRows[0][7]);
            using (SqlConnection con = new SqlConnection(Common.MyConn())) //total ps
            {
                //SqlCommand com = new SqlCommand(@"select max(AccountID),
                //                                                    isnull(min(OrderNo),ROW_NUMBER() OVER(PARTITION BY isnull(OOEID,2) ORDER BY isnull(OOEID,2),min(AccountID) ASC)) + 999999 as SeriesID,
                //                                                    Concat((select dbo.fn_BMS_GetIndent(1)),'Total ',isnull(OOEName,'Maintenance and Other Operating Expenses')) as Particular,
                //                                                    '' as AccountCode,'' as IncomeClassification, sum(isnull(PastYearApproved,0)) as PastYear,sum(isnull(CurrentYearApproved,0)) as CurrentYear,sum(isnull(BudgetYearAmount,0)) as BudgetYear,
                //                                                    isnull(OOEID,2) + " + LastOrderNo + " as OrderNo,1 as IsBold,sum(isnull(Current_Year_Obligated,0)) as Obligated  " + ""
                //                                + " from tbl_R_BMSConsoliditedTotal " + ""
                //                                + " where OOEID = 1 and OfficeID in(select OfficeID from tbl_R_BMSOffices where OrderNo is not null and FundID= " + FundID + ") and YEarOf = " + YearOf
                //                                + " GROUP BY isnull(OOEID,2),isnull(OOEName,'Maintenance and Other Operating Expenses')", con);
                SqlCommand com = new SqlCommand(@"exec ifmis.dbo.sp_BMS_LBP1 " + YearOf + "," + FundID + "," + LastOrderNo + ",1,1," + eid + "", con);
                con.Open();
                dt.Load(com.ExecuteReader());
            } 
            //Regular MOOE Header
            foundRows = dt.Select("OrderNo > 0", "orderNo Desc");
            LastOrderNo = Convert.ToInt32(foundRows[0][7]);
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select max(AccountID),
                                                                        isnull(min(OrderNo),ROW_NUMBER() OVER(PARTITION BY isnull(OOEID,2) ORDER BY isnull(OOEID,2),min(AccountID) ASC)) - 1 as SeriesID,
                                                                        Concat((select dbo.fn_BMS_GetIndent(1)),isnull(OOEName,'Maintenance and Other Operating Expenses')) as Particular,
                                                                        '' as AccountCode,'' as IncomeClassification, NULL as PastYear,null as CurrentYear,NULL as BudgetYear,
                                                                        isnull(OOEID,2) + " + LastOrderNo + " as OrderNo,1 as IsBold  " + ""
                                                + " from tbl_R_BMSConsoliditedTotal " + ""
                                                + " where OOEID = 2 and OfficeID in(select OfficeID from tbl_R_BMSOffices where OrderNo is not null and FundID= " + FundID + ") and YEarOf = " + YearOf
                                                + " GROUP BY isnull(OOEID,2),isnull(OOEName,'Maintenance and Other Operating Expenses')", con);
                con.Open();
                dt.Load(com.ExecuteReader());
            } 
            //Regular Mooe
            foundRows = dt.Select("OrderNo > 0", "orderNo Desc");
            LastOrderNo = Convert.ToInt32(foundRows[0][7]);
            using (SqlConnection con = new SqlConnection(Common.MyConn())) //mooe breakdown
            {
                //SqlCommand com = new SqlCommand(@"
                //                                    with cte as(
                //                                    select *, rn = ROW_NUMBER()OVER(Partition by FMISAccountCode ORder by id) from tbl_R_BMSPPSASCode
                //                                    )
                //                                    select max(a.AccountID),
                //                                                    min(isnull(c.FMISAccountCode,99999)) as SeriesID,
                //                                                    Concat((select dbo.fn_BMS_GetIndent(2)),a.AccountName) as Particular,b.PPSASCode as AccountCode,
                //                                                    '' as IncomeClassification, sum(isnull(a.PastYearApproved,0)) as PastYear, sum(isnull(a.CurrentYearApproved,0)) as CurrentYear,sum(isnull(a.BudgetYearAmount,0)) as BudgetYear,
                //                                                    isnull(a.OOEID,2) + " + LastOrderNo + " as OrderNo,0 as IsBold,sum(isnull(a.Current_Year_Obligated,0)) as Obligated " + ""
                //                                + " from tbl_R_BMSConsoliditedTotal as a " + ""
                //                                + " LEFT JOIN cte as b on FMISAccountCode = a.AccountID and a.YearOf = b.YearOf and b.ActionCode = 1 and b.rn = 1 " + ""
                //                                + " left join tbl_R_BMSAccounts as c on c.FMISAccountCode = a.AccountID and c.Active = 1" + ""
                //                                + " where a.OOEID = 2 and a.YearOf = " + YearOf + " and a.OfficeID in(select OfficeID from tbl_R_BMSOffices where OrderNo is not null and FundID= " + FundID + ") " + ""
                //                                + " and a.ProgramID not in(select ProgramID FROM tbl_R_BMSOfficePRograms where ProgramYear = " + YearOf + " and ActionCode = 1 and ProgramDescription like '%SPECIAL PROJECTS/ACTIVITIES%' or ProgramDescription like '%Special Projects / Activities%')" + ""
                //                                + " GROUP BY isnull(a.OOEID,2),a.AccountName,b.PPSASCode", con);
                SqlCommand com = new SqlCommand(@"exec sp_BMS_LBP1 " + YearOf + "," + FundID + "," + LastOrderNo + ",2,0," + eid + "", con);
                con.Open();
                dt.Load(com.ExecuteReader());
            }
            //Total Regular MOOE
            foundRows = dt.Select("OrderNo > 0", "orderNo Desc");
            LastOrderNo = Convert.ToInt32(foundRows[0][7]);
            using (SqlConnection con = new SqlConnection(Common.MyConn())) //mooe total
            {
                //SqlCommand com = new SqlCommand(@"select max(AccountID),
                //                                                    isnull(min(OrderNo),ROW_NUMBER() OVER(PARTITION BY isnull(OOEID,2) ORDER BY isnull(OOEID,2),min(AccountID) ASC)) + 999999 as SeriesID,
                //                                                    Concat((select dbo.fn_BMS_GetIndent(1)),'Total ',isnull(OOEName,'Maintenance and Other Operating Expenses')) as Particular,
                //                                                    '' as AccountCode,'' as IncomeClassification, sum(isnull(PastYearApproved,0)) as PastYear,sum(isnull(CurrentYearApproved,0)) as CurrentYear,sum(isnull(BudgetYearAmount,0)) as BudgetYear,
                //                                                    isnull(OOEID,2) + " + LastOrderNo + " as OrderNo,1 as IsBold,sum(isnull(Current_Year_Obligated,0)) as Obligated  " + ""
                //                                + " from tbl_R_BMSConsoliditedTotal " + ""
                //                                + " where OOEID = 2 and OfficeID in(select OfficeID from tbl_R_BMSOffices where OrderNo is not null and FundID= " + FundID + ") and YEarOf = " + YearOf
                //                                + " and ProgramID not in(select ProgramID FROM tbl_R_BMSOfficePRograms where ProgramYear = " + YearOf + " and ActionCode = 1 and ProgramDescription like '%SPECIAL PROJECTS/ACTIVITIES%' or ProgramDescription like '%Special Projects / Activities%')" + ""
                //                                + " GROUP BY isnull(OOEID,2),isnull(OOEName,'Maintenance and Other Operating Expenses')", con);
                SqlCommand com = new SqlCommand(@"exec sp_BMS_LBP1 " + YearOf + "," + FundID + "," + LastOrderNo + ",2,1," + eid + "", con);
                con.Open();
                dt.Load(com.ExecuteReader());
            }
            //20% development fund-FE - HEADER
            foundRows = dt.Select("OrderNo > 0", "orderNo Desc");
            LastOrderNo = Convert.ToInt32(foundRows[0][7]);
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select max(AccountID),
                                                                        isnull(min(OrderNo),ROW_NUMBER() OVER(PARTITION BY isnull(OOEID,3) ORDER BY isnull(OOEID,3),min(AccountID) ASC)) - 1 as SeriesID,
                                                                        Concat((select dbo.fn_BMS_GetIndent(1)),'Financial Expenses') as Particular,
                                                                        '' as AccountCode,'' as IncomeClassification, NULL as PastYear,null as CurrentYear,NULL as BudgetYear,
                                                                        isnull(OOEID,3) + " + LastOrderNo + " as OrderNo,1 as IsBold  " + ""
                                                + " from tbl_R_BMSConsoliditedTotal " + ""
                                                + " where OOEID = 2 and OfficeID in(select OfficeID from tbl_R_BMSOffices where OrderNo is not null and FundID= " + FundID + ") and YEarOf = " + YearOf
                                                + " GROUP BY isnull(OOEID,3)", con);
                con.Open();
                dt.Load(com.ExecuteReader());
            }
            //20% development fund-FE
            foundRows = dt.Select("OrderNo > 0", "orderNo Desc");
            LastOrderNo = Convert.ToInt32(foundRows[0][7]);
            using (SqlConnection con = new SqlConnection(Common.MyConn())) //mooe breakdown
            {
                //SqlCommand com = new SqlCommand(@"
                //                                    with cte as(
                //                                    select *, rn = ROW_NUMBER()OVER(Partition by FMISAccountCode ORder by id) from tbl_R_BMSPPSASCode
                //                                    )
                //                                    select max(a.AccountID),
                //                                                    min(isnull(c.FMISAccountCode,99999)) as SeriesID,
                //                                                    Concat((select dbo.fn_BMS_GetIndent(2)),a.AccountName) as Particular,b.PPSASCode as AccountCode,
                //                                                    '' as IncomeClassification, sum(isnull(a.PastYearApproved,0)) as PastYear, sum(isnull(a.CurrentYearApproved,0)) as CurrentYear,sum(isnull(a.BudgetYearAmount,0)) as BudgetYear,
                //                                                    isnull(a.OOEID,2) + " + LastOrderNo + " as OrderNo,0 as IsBold,sum(isnull(a.Current_Year_Obligated,0)) as Obligated " + ""
                //                                + " from tbl_R_BMSConsoliditedTotal as a " + ""
                //                                + " LEFT JOIN cte as b on FMISAccountCode = a.AccountID and a.YearOf = b.YearOf and b.ActionCode = 1 and b.rn = 1 " + ""
                //                                + " left join tbl_R_BMSAccounts as c on c.FMISAccountCode = a.AccountID and c.Active = 1" + ""
                //                                + " where a.OOEID = 2 and a.YearOf = " + YearOf + " and a.OfficeID in(select OfficeID from tbl_R_BMSOffices where OrderNo is not null and FundID= " + FundID + ") " + ""
                //                                + " and a.ProgramID not in(select ProgramID FROM tbl_R_BMSOfficePRograms where ProgramYear = " + YearOf + " and ActionCode = 1 and ProgramDescription like '%SPECIAL PROJECTS/ACTIVITIES%' or ProgramDescription like '%Special Projects / Activities%')" + ""
                //                                + " GROUP BY isnull(a.OOEID,2),a.AccountName,b.PPSASCode", con);
                SqlCommand com = new SqlCommand(@"exec sp_BMS_LBP1 " + YearOf + "," + FundID + "," + LastOrderNo + ",6,0," + eid + "", con);
                con.Open();
                dt.Load(com.ExecuteReader());
            }
            //Total Regular MOOE
            foundRows = dt.Select("OrderNo > 0", "orderNo Desc");
            LastOrderNo = Convert.ToInt32(foundRows[0][7]);
            using (SqlConnection con = new SqlConnection(Common.MyConn())) //mooe total
            {
                //SqlCommand com = new SqlCommand(@"select max(AccountID),
                //                                                    isnull(min(OrderNo),ROW_NUMBER() OVER(PARTITION BY isnull(OOEID,2) ORDER BY isnull(OOEID,2),min(AccountID) ASC)) + 999999 as SeriesID,
                //                                                    Concat((select dbo.fn_BMS_GetIndent(1)),'Total ',isnull(OOEName,'Maintenance and Other Operating Expenses')) as Particular,
                //                                                    '' as AccountCode,'' as IncomeClassification, sum(isnull(PastYearApproved,0)) as PastYear,sum(isnull(CurrentYearApproved,0)) as CurrentYear,sum(isnull(BudgetYearAmount,0)) as BudgetYear,
                //                                                    isnull(OOEID,2) + " + LastOrderNo + " as OrderNo,1 as IsBold,sum(isnull(Current_Year_Obligated,0)) as Obligated  " + ""
                //                                + " from tbl_R_BMSConsoliditedTotal " + ""
                //                                + " where OOEID = 2 and OfficeID in(select OfficeID from tbl_R_BMSOffices where OrderNo is not null and FundID= " + FundID + ") and YEarOf = " + YearOf
                //                                + " and ProgramID not in(select ProgramID FROM tbl_R_BMSOfficePRograms where ProgramYear = " + YearOf + " and ActionCode = 1 and ProgramDescription like '%SPECIAL PROJECTS/ACTIVITIES%' or ProgramDescription like '%Special Projects / Activities%')" + ""
                //                                + " GROUP BY isnull(OOEID,2),isnull(OOEName,'Maintenance and Other Operating Expenses')", con);
                SqlCommand com = new SqlCommand(@"exec sp_BMS_LBP1 " + YearOf + "," + FundID + "," + LastOrderNo + ",6,1," + eid + "", con);
                con.Open();
                dt.Load(com.ExecuteReader());
            }
            
            //Capital Outlay Header
            foundRows = dt.Select("OrderNo > 0", "orderNo Desc");
            LastOrderNo = Convert.ToInt32(foundRows[0][7]);
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select max(AccountID),
                                                                        isnull(min(OrderNo),ROW_NUMBER() OVER(PARTITION BY isnull(OOEID,2) ORDER BY isnull(OOEID,2),min(AccountID) ASC)) - 1 as SeriesID,
                                                                        Concat((select dbo.fn_BMS_GetIndent(1)),isnull(OOEName,'Maintenance and Other Operating Expenses')) as Particular,
                                                                        '' as AccountCode,'' as IncomeClassification, NULL as PastYear,null as CurrentYear,NULL as BudgetYear,
                                                                        isnull(OOEID,2) + " + LastOrderNo + " as OrderNo,1 as IsBold  " + ""
                                                + " from tbl_R_BMSConsoliditedTotal " + ""
                                                + " where OOEID = 3 and OfficeID in(select OfficeID from tbl_R_BMSOffices where OrderNo is not null and FundID= " + FundID + ") and YEarOf = " + YearOf
                                                + " GROUP BY isnull(OOEID,2),isnull(OOEName,'Maintenance and Other Operating Expenses')", con);
                con.Open();
                dt.Load(com.ExecuteReader());
            }
            //Capital Outlay
            foundRows = dt.Select("OrderNo > 0", "orderNo Desc");
            LastOrderNo = Convert.ToInt32(foundRows[0][7]);
            using (SqlConnection con = new SqlConnection(Common.MyConn())) //capital outlay breakdown
            {
                //SqlCommand com = new SqlCommand(@"
                //                                    with cte as(
                //                                    select *, rn = ROW_NUMBER()OVER(Partition by FMISAccountCode ORder by id) from tbl_R_BMSPPSASCode
                //                                    )
                //                                    select max(a.AccountID),
                //                                                    min(isnull(c.FMISAccountCode,99999)) as SeriesID,
                //                                                    Concat((select dbo.fn_BMS_GetIndent(2)),a.AccountName) as Particular,b.PPSASCode as AccountCode,
                //                                                    '' as IncomeClassification, sum(isnull(a.PastYearApproved,0)) as PastYear, sum(isnull(a.CurrentYearApproved,0)) as CurrentYear,sum(isnull(a.BudgetYearAmount,0)) as BudgetYear,
                //                                                    isnull(a.OOEID,2) + " + LastOrderNo + " as OrderNo,0 as IsBold,sum(isnull(a.Current_Year_Obligated,0)) as Obligated " + ""
                //                                + " from tbl_R_BMSConsoliditedTotal as a " + ""
                //                                + " LEFT JOIN cte as b on FMISAccountCode = a.AccountID and a.YearOf = b.YearOf and b.ActionCode = 1 and b.rn = 1 " + ""
                //                                + " left join tbl_R_BMSAccounts as c on c.FMISAccountCode = a.AccountID and c.Active = 1" + ""
                //                                + " where a.OOEID = 3 and a.YearOf = " + YearOf + " and a.OfficeID in(select OfficeID from tbl_R_BMSOffices where OrderNo is not null and FundID= " + FundID + ") " + ""
                //                                + " GROUP BY isnull(a.OOEID,2),a.AccountName,b.PPSASCode", con);
                SqlCommand com = new SqlCommand(@"exec sp_BMS_LBP1 " + YearOf + "," + FundID + "," + LastOrderNo + ",3,0," + eid + "", con);
                con.Open();
                dt.Load(com.ExecuteReader());
            }
            //Capital Outlay Footer
            foundRows = dt.Select("OrderNo > 0", "orderNo Desc");
            LastOrderNo = Convert.ToInt32(foundRows[0][7]);
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                //SqlCommand com = new SqlCommand(@"select max(AccountID),
                //                                                    isnull(min(OrderNo),ROW_NUMBER() OVER(PARTITION BY isnull(OOEID,2) ORDER BY isnull(OOEID,2),min(AccountID) ASC)) + 999999 as SeriesID,
                //                                                    Concat((select dbo.fn_BMS_GetIndent(1)),'Total ',isnull(OOEName,'Maintenance and Other Operating Expenses')) as Particular,
                //                                                    '' as AccountCode,'' as IncomeClassification, sum(isnull(PastYearApproved,0)) as PastYear,sum(isnull(CurrentYearApproved,0)) as CurrentYear,sum(isnull(BudgetYearAmount,0)) as BudgetYear,
                //                                                    isnull(OOEID,2) + " + LastOrderNo + " as OrderNo,1 as IsBold,sum(isnull(Current_Year_Obligated,0)) as Obligated  " + ""
                //                                + " from tbl_R_BMSConsoliditedTotal " + ""
                //                                + " where OOEID = 3 and OfficeID in(select OfficeID from tbl_R_BMSOffices where OrderNo is not null and FundID= " + FundID + ") and YEarOf = " + YearOf
                //                                + " GROUP BY isnull(OOEID,2),isnull(OOEName,'Maintenance and Other Operating Expenses')", con);
                SqlCommand com = new SqlCommand(@"exec sp_BMS_LBP1 " + YearOf + "," + FundID + "," + LastOrderNo + ",3,1," + eid + "", con);
                con.Open();
                dt.Load(com.ExecuteReader());
            }
            //Special Project MOOE Header
            foundRows = dt.Select("OrderNo > 0", "orderNo Desc");
            LastOrderNo = Convert.ToInt32(foundRows[0][7]);
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select max(AccountID),
                                                                        isnull(min(OrderNo),ROW_NUMBER() OVER(PARTITION BY isnull(OOEID,2) ORDER BY isnull(OOEID,2),min(AccountID) ASC)) - 1 as SeriesID,
                                                                        Concat((select dbo.fn_BMS_GetIndent(1)),'Special Projects/Activities') as Particular,
                                                                        '' as AccountCode,'' as IncomeClassification, NULL as PastYear,null as CurrentYear,NULL as BudgetYear,
                                                                        isnull(OOEID,2) + " + LastOrderNo + " as OrderNo,1 as IsBold  " + ""
                                                + " from tbl_R_BMSConsoliditedTotal " + ""
                                                + " where OOEID = 2 and OfficeID in(select OfficeID from tbl_R_BMSOffices where OrderNo is not null and FundID= " + FundID + ") and YEarOf = " + YearOf
                                                + " GROUP BY isnull(OOEID,2),isnull(OOEName,'Maintenance and Other Operating Expenses')", con);
                con.Open();
                dt.Load(com.ExecuteReader());
            }
            //Special Project MOOE
            foundRows = dt.Select("OrderNo > 0", "orderNo Desc");
            LastOrderNo = Convert.ToInt32(foundRows[0][7]);
            using (SqlConnection con = new SqlConnection(Common.MyConn()))//special project breakdown
            {
                //SqlCommand com = new SqlCommand(@"
                //                                    with cte as(
                //                                    select *, rn = ROW_NUMBER()OVER(Partition by FMISAccountCode ORder by id) from tbl_R_BMSPPSASCode
                //                                    )
                //                                    select max(a.AccountID),
                //                                                    min(isnull(c.FMISAccountCode,99999)) as SeriesID,
                //                                                    Concat((select dbo.fn_BMS_GetIndent(2)),a.AccountName) as Particular,b.PPSASCode as AccountCode,
                //                                                    '' as IncomeClassification, sum(isnull(a.PastYearApproved,0)) as PastYear, sum(isnull(a.CurrentYearApproved,0)) as CurrentYear,sum(isnull(a.BudgetYearAmount,0)) as BudgetYear,
                //                                                    isnull(a.OOEID,2) + " + LastOrderNo + " as OrderNo,0 as IsBold,sum(isnull(a.Current_Year_Obligated,0)) as Obligated " + ""
                //                                + " from tbl_R_BMSConsoliditedTotal as a " + ""
                //                                + " LEFT JOIN cte as b on FMISAccountCode = a.AccountID and a.YearOf = b.YearOf and b.ActionCode = 1 and b.rn = 1 " + ""
                //                                + " left join tbl_R_BMSAccounts as c on c.FMISAccountCode = a.AccountID and c.Active = 1" + ""
                //                                + " where a.OOEID = 2 and a.YearOf = " + YearOf + " and a.OfficeID in(select OfficeID from tbl_R_BMSOffices where OrderNo is not null and FundID= " + FundID + ") " + ""
                //                                + " and a.ProgramID in(select ProgramID FROM tbl_R_BMSOfficePRograms where ProgramYear = " + YearOf + " and ActionCode = 1 and ProgramDescription like '%SPECIAL PROJECTS/ACTIVITIES%' or ProgramDescription like '%Special Projects / Activities%')" + ""
                //                                + " GROUP BY isnull(a.OOEID,2),a.AccountName,b.PPSASCode", con);
                SqlCommand com = new SqlCommand(@"exec sp_BMS_LBP1 " + YearOf + "," + FundID + "," + LastOrderNo + ",4,0," + eid + "", con);
                con.Open();
                dt.Load(com.ExecuteReader());
            }
            //Special Project MOOE Footer
            foundRows = dt.Select("OrderNo > 0", "orderNo Desc");
            LastOrderNo = Convert.ToInt32(foundRows[0][7]);
            using (SqlConnection con = new SqlConnection(Common.MyConn())) //special project total
            {
                //SqlCommand com = new SqlCommand(@"select max(AccountID),
                //                                                    isnull(min(OrderNo),ROW_NUMBER() OVER(PARTITION BY isnull(OOEID,2) ORDER BY isnull(OOEID,2),min(AccountID) ASC)) + 999999 as SeriesID,
                //                                                    Concat((select dbo.fn_BMS_GetIndent(1)),'Total ',isnull(OOEName,'Maintenance and Other Operating Expenses'),' - Special Projects') as Particular,
                //                                                    '' as AccountCode,'' as IncomeClassification, sum(isnull(PastYearApproved,0)) as PastYear,sum(isnull(CurrentYearApproved,0)) as CurrentYear,sum(isnull(BudgetYearAmount,0)) as BudgetYear,
                //                                                    isnull(OOEID,2) + " + LastOrderNo + " as OrderNo,1 as IsBold,sum(isnull(Current_Year_Obligated,0)) as Obligated  " + ""
                //                                + " from tbl_R_BMSConsoliditedTotal " + ""
                //                                + " where OOEID = 2 and OfficeID in(select OfficeID from tbl_R_BMSOffices where OrderNo is not null and FundID= " + FundID + ") and YEarOf = " + YearOf
                //                                + " and ProgramID in(select ProgramID FROM tbl_R_BMSOfficePRograms where ProgramYear = " + YearOf + " and ActionCode = 1 and ProgramDescription like '%SPECIAL PROJECTS/ACTIVITIES%' or ProgramDescription like '%Special Projects / Activities%')" + ""
                //                                + " GROUP BY isnull(OOEID,2),isnull(OOEName,'Maintenance and Other Operating Expenses')", con);
                SqlCommand com = new SqlCommand(@"exec sp_BMS_LBP1 " + YearOf + "," + FundID + "," + LastOrderNo + ",4,1," + eid + "", con);
                con.Open();
                dt.Load(com.ExecuteReader());
            }
            //Generate Total of All Accounts
            foundRows = dt.Select("OrderNo > 0", "orderNo Desc");
                LastOrderNo = Convert.ToInt32(foundRows[0][7]) + 1;
                #region Old Query
                //using (SqlConnection con = new SqlConnection(Common.MyConn()))
                //{
                //    SqlCommand com = new SqlCommand(@"select max(a.AccountID),isnull(max(a.OrderNo),ROW_NUMBER() OVER(PARTITION BY NULL ORDER BY max(a.AccountID) ASC)) as SeriesID,Concat((select dbo.fn_BMS_GetIndent(0)),'Total') as Particular,'' as AccountCode, " + ""
                //                                    + " '' as IncomeClassification,sum(0) as PastYear,sum(e.ProposalAllotedAmount) as CurrentYear,sum(c.ProposalAllotedAmount) as BudgetYear, " + ""
                //                                    + " " + LastOrderNo + " as OrderNo,1 as IsBold,sum(f.Obligated) as 'Obligated'  " + ""
                //                                    + " from tbl_R_BMSProgramAccounts as a " + ""
                //                                    + " LEFT JOIN tbl_R_BMSPPSASCode as b on b.FMISAccountCode = a.AccountID and a.AccountYear = b.YearOf and a.ActionCode = b.ActionCode " + ""
                //                                    + " LEFT JOIN tbl_T_BMSBudgetProposal as c on c.AccountID = a.AccountID and c.ProgramID = a.ProgramID and c.ProposalYear = a.AccountYear and c.ProposalActionCode = a.ActionCode " + ""
                //                                    + " LEFT JOIN tbl_R_BMSOfficePrograms as d on d.ProgramID = a.ProgramID and d.ProgramYear = a.AccountYear and d.ActionCode = a.ActionCode " + ""
                //                                    + " LEFT JOIN tbl_T_BMSBudgetProposal as e on e.AccountID = a.AccountID and e.ProgramID = a.ProgramID and e.ProposalYear = a.AccountYear - 1 and e.ProposalActionCode = a.ActionCode " + ""
                //                                    + " LEFT JOIN tbl_R_BMSCurrentYearObligated as f on  f.AccountID = a.AccountID and f.Budgetyear = a.AccountYear and f.OfficeID = d.OfficeID" + ""
                //                                    + " where a.AccountYear = " + YearOf + " and a.ActionCode = 1 and d.OfficeID " + inStatement + "(37,38,41) " + "", con);
                //    con.Open();
                //    dt.Load(com.ExecuteReader());
                //} 
                #endregion
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                //SqlCommand com = new SqlCommand(@"select max(AccountID),isnull(max(OrderNo),ROW_NUMBER() OVER(PARTITION BY NULL ORDER BY max(AccountID) ASC)) as SeriesID,
                //                                    Concat((select dbo.fn_BMS_GetIndent(0)),'Total Expenditures') as Particular,'' as AccountCode,''as IncomeClassification,
                //                                    sum(PastYearApproved) as  PastYear,sum(CurrentYearApproved) as  CurrentYear, sum (isnull(BudgetYearAmount,0)) as Budgetyear,
                //                                    "+LastOrderNo+" as OrderNo,1 as IsBold,sum(isnull(Current_Year_Obligated,0)) as Obligated "+""
                //                                    + " from tbl_R_BMSConsoliditedTotal where YearOf = " + YearOf + " and OfficeID in(select OfficeID from tbl_R_BMSOffices where OrderNo is not null and FundID= " + FundID + " and OfficeID != 43)", con);
                SqlCommand com = new SqlCommand(@"exec sp_BMS_LBP1 " + YearOf + "," + FundID + "," + LastOrderNo + ",5,0," + eid + "", con);
                con.Open();
                    dt.Load(com.ExecuteReader());
                }
            textBox8.Value = "Annual Budget CY " + YearOf + " - LBP Form No. 1";
            htmlTextBox5.Value = FundType == 1?"GENERAL FUND":"ECONOMIC ENTERPRISE";
            htmlTextBox3.Value = GlobalFunctions.getReportTextBoxValue(5,1);
            htmlTextBox4.Value = GlobalFunctions.getReportTextBoxValue(5, 2);
            barcode1.Value = FUNCTION.GeneratePISControl();
            GlobalFunctions.QR_globalstr = barcode1.Value;
            this.table1.DataSource = dt;

            DataTable reportlog2 = new DataTable();
            using (SqlConnection conUpdateRep = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec [sp_BMS_LBPReport_Update] " + YearOf + ",0,6,'" + barcode1.Value + "'", conUpdateRep);
                conUpdateRep.Open();
                reportlog2.Load(com.ExecuteReader());

            }

        }
    }
}