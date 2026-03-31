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
    /// <summary>
    /// Summary description for LBPF6.
    /// </summary>
    public partial class LBPF6 : Telerik.Reporting.Report
    {
        public LBPF6(int yearof)
        {

            InitializeComponent();

            DataTable _dt3 = new DataTable();
            string _sqlQuery3 = "Select getdate() as ServerDate";
            _dt3 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery3).Tables[0];

            textBox84.Value = _dt3.Rows[0]["ServerDate"].ToString();
            textBox4.Value = "Budget Year:  Calendar Year " + yearof + "";


            DataTable dt = new DataTable();
            dt.Columns.Add("groupaccountdescription");
            dt.Columns.Add("AccountName");
            dt.Columns.Add("amount");

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"EXEC sp_BMS_LBP6 "+ yearof + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = reader.GetValue(4).ToString();
                    dr[1] = reader.GetValue(1).ToString();
                    dr[2] = reader.GetValue(2).ToString();
                    dt.Rows.Add(dr);

                }
            }
            table1.DataSource = dt;
        }
    }
}