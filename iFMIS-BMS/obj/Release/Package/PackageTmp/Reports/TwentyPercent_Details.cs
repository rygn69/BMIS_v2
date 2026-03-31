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
    using System.Globalization;
    using iFMIS_BMS.BusinessLayer.Connector;
    using iFMIS_BMS.BusinessLayer.Layers;
    using iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation;
    using System.Configuration;

    public partial class TwentyPercent_Details : Telerik.Reporting.Report
    {
        public TwentyPercent_Details(int year, int year2, int month_, string monthname, long rootppaid, long subppaid, int pgo_, int summary,int earmark_type)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            textBox1.Value = "Status of Utilization of " + year + " 20% Development Fund";
            textBox2.Value = "As Of " + monthname + " " + year2 + "";

            DataTable dt = new DataTable();
            dt.Columns.Add("psector");              //dr[0]
            dt.Columns.Add("description");              //dr[1]
            dt.Columns.Add("Appropriation");         //dr[2]
            dt.Columns.Add("alobsno");      //dr[3]
            dt.Columns.Add("particulars");
            dt.Columns.Add("Amount");
            dt.Columns.Add("TotalAppropriation");
            dt.Columns.Add("IsExcess");
            dt.Columns.Add("RunBalance");
            dt.Columns.Add("DateTime");
            using (SqlConnection con = new SqlConnection(Common.MyConn2()))
            {
                if (summary == 0)
                {
                    SqlCommand com = new SqlCommand(@"EXEC [sp_PPAccumulateExpense] " + year + "," + year2 + "," + month_ + "," + rootppaid + ", " + subppaid + "," + pgo_  + ","+ earmark_type + "", con);
                    con.Open();

                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = reader.GetValue(1).ToString();
                        dr[1] = reader.GetValue(6).ToString();
                        dr[2] = reader.GetValue(8).ToString();
                        dr[3] = reader.GetValue(7).ToString();
                        dr[4] = reader.GetValue(10).ToString();
                        dr[5] = reader.GetValue(9).ToString();
                        dr[6] = reader.GetValue(14).ToString();
                        dr[7] = reader.GetValue(12).ToString();
                        dr[8] = reader.GetValue(15).ToString();
                        dr[9] = reader.GetValue(11).ToString();
                        dt.Rows.Add(dr);
                    }
                }

            }
            textBox26.Value = "Date Printed : " + DateTime.Now.ToString();
            textBox43.Value = "Printed By : " + Account.UserInfo.empName;
            table1.DataSource = dt;

            DataTable _dt4 = new DataTable();
            string _sqlQuery4 = "SELECT [lgu],[province],isnull([address],'') address,[budgetofficer],[position] FROM [IFMIS].[dbo].[tbl_R_BMSLgu] where actioncode=1";
            _dt4 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery4).Tables[0];
            if (_dt4.Rows.Count > 0)
            {
                //textBox2.Value = _dt4.Rows[0][0].ToString();
                //textBox3.Value = _dt4.Rows[0][1].ToString();
                //textBox3.Value = _dt4.Rows[0][2].ToString();
                textBox48.Value = _dt4.Rows[0][0].ToString() + " - Making your task easier...";

            }
        }
    }
}