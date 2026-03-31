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

    /// <summary>
    /// Summary description for ProposedPositionReport.
    /// </summary>
    public partial class ProposedPositionReport : Telerik.Reporting.Report
    {
        public ProposedPositionReport(int year,int OfficeID)
        {
            InitializeComponent();

            DataTable dt = new DataTable();

            dt.Columns.Add("OfficeName"); //dr[0]
            dt.Columns.Add("Position");//dr[1]
            dt.Columns.Add("SG");//dr[2]
            dt.Columns.Add("AnnualSalary");//dr[3]
            dt.Columns.Add("YearEndBonus");//dr[4]
            dt.Columns.Add("MidYearBonus");//dr[5]
            dt.Columns.Add("Philhealth");//dr[6]
            dt.Columns.Add("ECC");//dr[7]
            dt.Columns.Add("GSIS");//dr[8]
            dt.Columns.Add("PERA");//dr[9]
            dt.Columns.Add("PagIbig");//dr[10]
            dt.Columns.Add("Clothing");//dr[11]
            dt.Columns.Add("CashGift");//dr[12]
            dt.Columns.Add("Subsistence");//dr[13]
            dt.Columns.Add("Hazard");//dr[14]
            dt.Columns.Add("PBB");//dr[15]
            dt.Columns.Add("PEI");//dr[16]
            dt.Columns.Add("FundType");//dr[17]
            dt.Columns.Add("StartDate");//dr[18]
            dt.Columns.Add("RA");//dr[19]
            dt.Columns.Add("TA");//dr[20]
            dt.Columns.Add("Total");//dr[21]

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_GetForFundingSummary " + (year - 1) + ",2," + OfficeID + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = reader.GetValue(1).ToString();
                    dr[1] = reader.GetValue(2).ToString();
                    dr[2] = reader.GetValue(3).ToString();
                    dr[3] = Convert.ToDouble(reader.GetValue(4));
                    dr[4] = Convert.ToDouble(reader.GetValue(5));
                    dr[5] = Convert.ToDouble(reader.GetValue(6));
                    dr[6] = Convert.ToDouble(reader.GetValue(7));
                    dr[7] = Convert.ToDouble(reader.GetValue(8));
                    dr[8] = Convert.ToDouble(reader.GetValue(9));
                    dr[9] = Convert.ToDouble(reader.GetValue(10));
                    dr[10] = Convert.ToDouble(reader.GetValue(11));
                    dr[11] = Convert.ToDouble(reader.GetValue(12));
                    dr[12] = Convert.ToDouble(reader.GetValue(13));
                    dr[13] = Convert.ToDouble(reader.GetValue(14));
                    dr[14] = Convert.ToDouble(reader.GetValue(15));
                    dr[15] = Convert.ToDouble(reader.GetValue(16));
                    dr[16] = Convert.ToDouble(reader.GetValue(17));
                    dr[17] = reader.GetValue(18).ToString();
                    dr[18] = reader.GetDateTime(19).ToShortDateString();
                    dr[19] = Convert.ToDouble(reader.GetValue(20));
                    dr[20] = Convert.ToDouble(reader.GetValue(21));
                    dr[21] = Convert.ToDouble(reader.GetValue(22));
                    
                    dt.Rows.Add(dr);
                }
            }
            this.ReportParameters["BudgetYear"].Value = year;
            this.DataSource = dt;
        }
    }
}