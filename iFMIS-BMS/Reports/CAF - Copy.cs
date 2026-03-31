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
        public CAF(string cafno,int? year=0,string issuedate="")
        {
            
            InitializeComponent();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            barcode1.Value = FUNCTION.GeneratePISControl();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                //SqlCommand com = new SqlCommand(@"exec [sp_BMS_CAFReport] '" + cafno + "'", con);
                //con.Open();
                //dt2.Load(com.ExecuteReader());
                SqlCommand com = new SqlCommand(@"exec [sp_BMS_CAFReport] '" + cafno + "'", con);
                con.Open();
                dt2.Load(com.ExecuteReader());
            }
            this.table1.DataSource = dt2;
            //using (SqlConnection con = new SqlConnection(Common.MyConn()))
            //{
            //    SqlCommand com = new SqlCommand(@"exec [sp_BMS_CAFTotalinWords] '" + cafno + "',"+ @year + ",'"+ issuedate + "'", con);
            //    con.Open();
            //    textBox7.Value = com.ExecuteScalar().ToString();
            //}
            //textBox16.Value ="Issued this " + Convert.ToDateTime(issuedate).Day + " of " + System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToDateTime(issuedate).Month) + ", " + Convert.ToDateTime(issuedate).Year + ".";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                DataTable _dt = new DataTable();
                string _sqlQuery = "exec [sp_BMS_CAFTotalinWords] '" + cafno + "'," + @year + ",'" + issuedate + "'";
                _dt= OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery).Tables[0];
                textBox7.Value = _dt.Rows[0][0].ToString();//ARO no.
                textBox16.Value = _dt.Rows[0][1].ToString();
            }
           
        }
    }
}