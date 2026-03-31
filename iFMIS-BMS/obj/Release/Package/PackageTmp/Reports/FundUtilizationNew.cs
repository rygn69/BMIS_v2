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
    /// Summary description for FundUtilization.
    /// </summary>
    public partial class FundUtilizationNew : Telerik.Reporting.Report
    {
        public FundUtilizationNew(int year, int fundtypeid, string Unique_Code)
        {
            InitializeComponent();

            textBox16.Value = "C.Y. " + year +"";
            QRCode2.Value = Unique_Code;
            DataTable dt = new DataTable();
            dt.Columns.Add("FundName");              //dr[0]
            dt.Columns.Add("appropriation");         //dr[1]
            dt.Columns.Add("allotment");  
            dt.Columns.Add("earmark");      //dr[2]
            dt.Columns.Add("obligation");//dr[3]
            dt.Columns.Add("rci");//dr[3]
            dt.Columns.Add("disbursed");//dr[4]
            dt.Columns.Add("AppBal");//dr[4]
            dt.Columns.Add("ObliBal");//dr[4]
            
          
            using (SqlConnection con = new SqlConnection(Common.MyConn2()))
            {
                //if (summary == 1)
                //{
                SqlCommand com = new SqlCommand(@"EXEC sp_BMS_FundUtilization " + year + "," + fundtypeid + "", con);
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
                    dr[8] = reader.GetValue(9).ToString();

                    dt.Rows.Add(dr);
                }
            }
            textBox30.Value = "Date Printed : " + DateTime.Now.ToString();
            textBox7.Value = "Printed By : " + Account.UserInfo.empName;
            table1.DataSource = dt;
         
        }
    }
}