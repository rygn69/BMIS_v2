namespace iFMIS_BMS.Reports
{
    using iFMIS_BMS.BusinessLayer.Connector;
    using iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for LBP2NewSummary.
    /// </summary>
    public partial class LBP2NewSummary : Telerik.Reporting.Report
    {
        public static double getPercentageIncreaseTotal(double Difference, double BudgetYearAmount) {
            if (BudgetYearAmount == 0)
            {
                return 0;   
            }
            else
            {
                return Difference / BudgetYearAmount * 100;                
            }
        }
        public LBP2NewSummary(int OfficeID, int YearOf, int ReportTypeID)
        {
            OfficeAdmin_Layer OfficeAdmin_Layer = new OfficeAdmin_Layer();
            var ReportType = "";
            var OldDataTableCount = 0;
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            InitializeComponent();
            DataTable OfficeIDList = new DataTable();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select a.OfficeID,a.OfficeName,a.fundID,b.FundName from tbl_R_BMSOffices as a 
                                                    LEFT JOIN tbl_R_BMSFunds as b on b.FundCode = a.FundID
                                                    where a.PMISOfficeID != 0 order by a.fundID, isnull(a.OrderNo,999999)", con);
                con.Open();
                OfficeIDList.Load(com.ExecuteReader());
            }
            dt.Columns.Add("OfficeName");
            dt.Columns.Add("OfficeID");
            dt.Columns.Add("FundTypeID");
            dt.Columns.Add("FundType");

            ReportType = ReportTypeID == 2 ? "Proposed" : "Consolidated";
            for (int x = 0; x < OfficeIDList.Rows.Count; x++)
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"sp_BMS_Get_LBP2Report " + Convert.ToInt32(OfficeIDList.Rows[x][0]) + ", " + YearOf + ", " + ReportTypeID + "", con);
                    con.Open();
                    dt.Load(com.ExecuteReader());
                    //var OfficeName = getOfficeName(Convert.ToInt32(OfficeIDList.Rows[x][0]));
                    for (int i = OldDataTableCount; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i].SetField("OfficeID", Convert.ToInt32(OfficeIDList.Rows[x][0]));
                        dt.Rows[i].SetField("OfficeName", OfficeIDList.Rows[x][1].ToString());
                        dt.Rows[i].SetField("FundTypeID", Convert.ToInt32(OfficeIDList.Rows[x][2]));
                        dt.Rows[i].SetField("FundType", OfficeIDList.Rows[x][3].ToString());

                    }
                    OldDataTableCount = dt.Rows.Count;
                }
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                string queryString =
                @"with cte as(
                                select PastYearApproved,CurrentYearApproved, BudgetYearAmount,
                                case when ProgramID in(select ProgramID FROM tbl_R_BMSOfficePRograms where ProgramYear = @YearOf and ActionCode = 1 and ProgramDescription like '%SPECIAL PROJECTS/ACTIVITIES%'  or ProgramDescription like '%Special Projects / Activities%')
                                then 9999 else a.OOEID end as OOEID,
                                case when ProgramID in(select ProgramID FROM tbl_R_BMSOfficePRograms where ProgramYear = @YearOf and ActionCode = 1 and ProgramDescription like '%SPECIAL PROJECTS/ACTIVITIES%'  or ProgramDescription like '%Special Projects / Activities%') 
                                then 'Special Projects' else a.OOENAme end as OOENAme
                                ,b.FundID,c.Fundname from tbl_R_BMSConsoliditedTotal as a
                                left join tbl_R_BMSOffices as b on b.OfficeID = a.officeID
                                LEFT JOIN tbl_R_BMSfunds as c on c.FundCode = b.FundID
                                where a.OOEID in (1,2,3) and b.FundID in(101,119) and YearOf = @YearOf
                            )
                            select sum(PastYearApproved) as PastYearApproved,
                            sum(CurrentYearApproved) as CurrentYearApproved,
                            sum(BudgetYearAmount) as BudgetYearAmount,
                            sum(BudgetYearAmount) - sum(CurrentYearApproved) as 'Difference',
                            case when isnull(Sum(BudgetYearAmount),0) = 0 then 0 else ((isnull(sum(BudgetYearAmount),0) - isnull(sum(CurrentYearApproved),0))/isnull(Sum(BudgetYearAmount),0)) end * 100 as 'IncreaseDecrease',
                            OOEName,FundID,FundName from cte 
                            group by OOEID,OOENAme,FundID,FundName
                            Order By FundID,OOEID
                            ";
                SqlCommand com = new SqlCommand(queryString, con);
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@YearOf";
                param.Value = YearOf;
                com.Parameters.Add(param);
                con.Open();
                dt2.Load(com.ExecuteReader());
            }
            this.ReportParameters["YearOf"].Value = YearOf;
            this.table1.DataSource = dt;
            this.table2.DataSource = dt2;
        }
    }
}