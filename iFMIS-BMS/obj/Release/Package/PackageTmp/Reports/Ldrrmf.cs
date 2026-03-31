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

    /// <summary>
    /// Summary description for Ldrrmf.
    /// </summary>
    public partial class Ldrrmf : Telerik.Reporting.Report
    {
        public string getYear(int excessacctval)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Select [YearOf] From [tbl_T_BMSExcessAppropriation] Where [TransactionNo] = " + excessacctval + " and [ActionCode]=1", con);
                    con.Open();
                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public Ldrrmf(int year, int month_, string monthofname,int excessacctval)
        {
            
            InitializeComponent();
            var tempYear = getYear(excessacctval);
            textBox1.Value = "Local Disaster Risk Reduction and Management Fund(LDRRMF) " + tempYear + " Utilization Summary";
            textBox16.Value = "As Of " + monthofname + " " + year + "";

            DataTable dt = new DataTable();
            dt.Columns.Add("accountname");              //dr[0]
            dt.Columns.Add("appropriation");         //dr[1]
            dt.Columns.Add("earmark");      //dr[2]
            dt.Columns.Add("obligation");//dr[3]
            dt.Columns.Add("disbursement");//dr[4]
            dt.Columns.Add("apppropriationbal");//dr[4]
            dt.Columns.Add("obligationbal");//dr[4]
            
            using (SqlConnection con = new SqlConnection(Common.MyConn2()))
            {
                //if (summary == 1)
                //{
                SqlCommand com = new SqlCommand(@"EXEC sp_BMS_Ldrrmf " + year + "," + tempYear + "," + month_ + "", con);
                con.Open();

                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = reader.GetValue(5).ToString();
                    dr[1] = reader.GetValue(6).ToString();
                    dr[2] = reader.GetValue(7).ToString();
                    dr[3] = reader.GetValue(8).ToString();
                    dr[4] = reader.GetValue(9).ToString();
                    dr[5] = reader.GetValue(10).ToString();
                    dr[6] = reader.GetValue(11).ToString();

                    dt.Rows.Add(dr);
                }
            }
            textBox30.Value = "Date Printed : " + DateTime.Now.ToString();
            textBox7.Value = "Printed By : " + Account.UserInfo.empName;
            table1.DataSource = dt;
        }
    }
}