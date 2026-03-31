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
    using System.Linq;
    using iFMIS_BMS.Base;
    using iFMIS_BMS.Classes;
    using System.Configuration;

    /// <summary>
    /// Summary description for BOI.
    /// </summary>
    public partial class AppropriationFund : Telerik.Reporting.Report
    {
        public AppropriationFund(int? year = 0)
        {
           
            InitializeComponent();

            DataTable dt = new DataTable();
            
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"exec sp_BMS_UtilizationProcurement_fund "+ year + ",1,0", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    dt.Load(com.ExecuteReader());
                }
            table2.DataSource = dt;
            
            barcode1.Value = FUNCTION.GeneratePISControl();
            textBox19.Value = "Appropriation Per Fund (Non-Procurment/Procurement) - C.Y. "+ year;
            textBox22.Value = "As Of ";
            DataTable _dt2 = new DataTable();
            string _sqlQuery2 = "Select format(getdate(),'MM/dd/yyyy hh:mm:ss tt'), format(getdate(),'MMMM dd, yyyy')";
            _dt2 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery2).Tables[0];
            if (_dt2.Rows.Count > 0)
            {
                textBox13.Value = "Date Printed: " + _dt2.Rows[0][0].ToString();
                textBox22.Value = "As Of " +_dt2.Rows[0][1].ToString(); 
            }
            
            DataTable reportlog = new DataTable();
            using (SqlConnection conUser = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@" exec [sp_BMS_ReportLog] 14," + Account.UserInfo.eid + ",'AppropriationPerFund','" + barcode1.Value + "'", conUser);
                conUser.Open();
                reportlog.Load(com.ExecuteReader());

            }
        }
    }
}