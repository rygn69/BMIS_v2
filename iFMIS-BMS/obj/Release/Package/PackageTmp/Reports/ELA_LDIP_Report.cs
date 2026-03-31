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
    /// Summary description for ELA_LDIP_Report.
    /// </summary>
    public partial class ELA_LDIP_Report : Telerik.Reporting.Report
    {
        public ELA_LDIP_Report(int SectorID,string InclusiveYear)
        {
    
            InitializeComponent();
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"get_ELA_LDIP_Report "+ SectorID +"", con);
                con.Open();
                dt.Load(com.ExecuteReader());
            }
            this.ReportParameters["SectorDescription"].Value = dt.Rows[0]["SectorDescription"];
            this.ReportParameters["Year1Param"].Value = InclusiveYear.Split(',')[0];
            this.ReportParameters["Year2Param"].Value =InclusiveYear.Split(',')[1];
            this.ReportParameters["Year3Param"].Value = InclusiveYear.Split(',')[2];
            this.table1.DataSource = dt;
        }
    }
}