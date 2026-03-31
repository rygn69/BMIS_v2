namespace iFMIS_BMS.Reports
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Data;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using iFMIS_BMS.BusinessLayer.Connector;
    using System.Linq;
    using iFMIS_BMS.Base;
    using iFMIS_BMS.Classes;
    using System.Configuration;
  
    public partial class LBPF7 : Telerik.Reporting.Report
    {
        public LBPF7(int? yearof)
        {
           
            InitializeComponent();
            DataTable _dt3 = new DataTable();
            string _sqlQuery3 = "Select getdate() as ServerDate";
            _dt3 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery3).Tables[0];

            textBox1.Value = "STATEMENT OF FUND ALLOCATION  By Sector CY " + yearof;

            textBox84.Value = _dt3.Rows[0]["ServerDate"].ToString();
   //         textBox4.Value = "Budget Year:  Calendar Year " + yearof + "";


            DataTable dt = new DataTable();
            dt.Columns.Add("Program");
            dt.Columns.Add("ExpClass");
            dt.Columns.Add("accountname");
            dt.Columns.Add("ppsascode");
            dt.Columns.Add("GS");
            dt.Columns.Add("SS");
            dt.Columns.Add("ES");
            dt.Columns.Add("OS");

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"EXEC sp_BMS_LBP7 " + yearof + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = reader.GetValue(0).ToString();
                    dr[1] = reader.GetValue(2).ToString();
                    dr[2] = reader.GetValue(3).ToString();
                    dr[3] = reader.GetValue(4).ToString();
                    dr[4] = reader.GetValue(5).ToString();
                    dr[5] = reader.GetValue(6).ToString();
                    dr[6] = reader.GetValue(7).ToString();
                    dr[7] = reader.GetValue(8).ToString();
                    dt.Rows.Add(dr);

                }
            }
            table1.DataSource = dt;
            barcode1.Value = FUNCTION.GeneratePISControl();
            GlobalFunctions.QR_globalstr = barcode1.Value;

            DataTable reportlog2 = new DataTable();
            using (SqlConnection conUpdateRep = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec [sp_BMS_LBPReport_Update] " + yearof + ",0,7,'" + barcode1.Value + "'", conUpdateRep);
                conUpdateRep.Open();
                reportlog2.Load(com.ExecuteReader());

            }
        }
    }
}