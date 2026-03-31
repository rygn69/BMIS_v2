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
    using System.Configuration;
    using System.Data.SqlTypes;
    using iFMIS_BMS.Base;
    using iFMIS_BMS.Classes;

    /// <summary>
    /// Summary description for LBEF.
    /// </summary>
    public partial class CAF : Telerik.Reporting.Report
    {
        public CAF(string cafno,int? year=0,string issuedate="",int? applynewdate=0, string certid = "")
        {
            
            InitializeComponent();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            barcode1.Value = FUNCTION.GeneratePISControl();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec [sp_BMS_CAFReport] '" + cafno + "'", con);
                con.Open();
                dt2.Load(com.ExecuteReader());
            }
            this.table1.DataSource = dt2;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                DataTable _dt = new DataTable();
                string _sqlQuery = "exec [sp_BMS_CAFTotalinWords] '" + cafno + "'," + @year + ",'" + issuedate + "'," + applynewdate + ",'" + certid + "'";
                _dt= OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery).Tables[0];
                textBox7.Value = _dt.Rows[0][0].ToString();//ARO no.
                textBox16.Value = _dt.Rows[0][1].ToString();
                textBox29.Value = cafno;
            }
            DataTable _dt4 = new DataTable();
            string _sqlQuery4 = "SELECT [lgu],[province],isnull([address],'') address FROM [IFMIS].[dbo].[tbl_R_BMSLgu] where actioncode=1";
            _dt4 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery4).Tables[0];
            if (_dt4.Rows.Count > 0)
            {
                textBox1.Value = _dt4.Rows[0][0].ToString();
                textBox2.Value = _dt4.Rows[0][1].ToString();
                textBox3.Value = _dt4.Rows[0][2].ToString();
                textBox48.Value = _dt4.Rows[0][0].ToString() + " - Making your task easier...";
            }
        }
    }
}