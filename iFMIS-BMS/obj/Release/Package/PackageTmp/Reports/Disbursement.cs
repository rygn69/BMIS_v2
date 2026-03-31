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

    /// <summary>
    /// Summary description for Disbursement.
    /// </summary>
    public partial class Disbursement : Telerik.Reporting.Report
    {
        public Disbursement(int Disyear, int OfficeID, int sectorid, int subsudid, int ldffrmfid, int Dismonth)
        {
            
            InitializeComponent();

            textBox2.Value = "C.Y. " + Disyear + "";
            //textBox2.Value = "As Of " + Convert.ToDateTime(monthof + "/1/" + year).ToString("MMM") + " " + year;
          
            DataTable dt = new DataTable();
            dt.Columns.Add("officename");              //dr[0]
            dt.Columns.Add("fmisaccountcode");
            dt.Columns.Add("BudgetAcctName");              //dr[1]
            dt.Columns.Add("Appropriation");         //dr[2]
            dt.Columns.Add("Disbursement");
            dt.Columns.Add("Balance");
            
            using (SqlConnection con = new SqlConnection(Common.MyConn2()))
            {
                SqlCommand com = new SqlCommand(@"EXEC sp_rygn_BudgetUtilization_Annual " + Disyear + "," + OfficeID + "," + sectorid + "," + subsudid + ", " + ldffrmfid + ", " + Dismonth + "", con);
                con.Open();

                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = reader.GetValue(1).ToString();
                    dr[1] = reader.GetValue(6).ToString();
                    dr[2] = reader.GetValue(7).ToString();
                    dr[3] = reader.GetValue(8).ToString();
                    dr[4] = reader.GetValue(9).ToString();
                    dr[5] = Convert.ToDouble(reader.GetValue(8).ToString()) - Convert.ToDouble(reader.GetValue(9).ToString());
                    dt.Rows.Add(dr);
                }
            }
            txt_todaydate.Value = "Date Printed : " + DateTime.Now.ToString();
            txt_user.Value = "Printed by : " + Account.UserInfo.empName;
            table1.DataSource = dt;
            
        }
    }
}