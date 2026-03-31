namespace iFMIS_BMS.Reports
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    /// <summary>
    /// Summary description for OrdinanceReportFrontPage.
    /// </summary>
    public partial class OrdinanceReportFrontPage : Telerik.Reporting.Report
    {
        public OrdinanceReportFrontPage(int Year)
        {
            //
            // Required for telerik Reporting designer support
            //
            
            InitializeComponent();
            txtAnnualBudget.Value = "Annual Budget " + (Year + 1);
            txtSeries.Value = "Series of " + Year;

            DataTable _dt4 = new DataTable();
            string _sqlQuery4 = "SELECT [lgu],[province],isnull([address],'') address FROM [IFMIS].[dbo].[tbl_R_BMSLgu] where actioncode=1";
            _dt4 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery4).Tables[0];
            if (_dt4.Rows.Count > 0)
            {
                pictureBox1.Value = "http://localhost/ifmis_bms/Images/tacurong.png";
                textBox2.Value= _dt4.Rows[0][0].ToString(); 
                textBox3.Value= _dt4.Rows[0][2].ToString(); 
            }
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}