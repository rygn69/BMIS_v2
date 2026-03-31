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
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using iFMIS_BMS.BusinessLayer.Connector;
    using iFMIS_BMS.BusinessLayer.Layers;
    using iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using iFMIS_BMS.Base;

    /// <summary>
    /// Summary description for NonOfficeSub.
    /// </summary>
    public partial class NonOfficeSubExcess : Telerik.Reporting.Report
    {
        public NonOfficeSubExcess(int progid, long accountid, int nonoffid, int year, int pgocntrl, int monthof,int exaccount,int earmark_type)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            DataTable dt = new DataTable();
            dt.Columns.Add("AccountName");              //dr[0]
            dt.Columns.Add("Appropriation");              //dr[1]
            dt.Columns.Add("datetimeentered");         //dr[2]
            dt.Columns.Add("obrno");      //dr[3]
            dt.Columns.Add("OtherExpense");
            dt.Columns.Add("description");
            dt.Columns.Add("Balance");
            dt.Columns.Add("TotalAppropriation");
            dt.Columns.Add("MainPPA");
            dt.Columns.Add("MainPPAAppropriation");
            //dt.Columns.Add("GenAppropriation");
            //dt.Columns.Add("GenAccountName");
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"EXEC sp_BMS_NonOfficeSubAccount_Excess " + progid + "," + accountid + "," + nonoffid + "," + year + "," + pgocntrl + "," + monthof + ","+ exaccount + "," + earmark_type  + "", con);
                con.Open();

                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = reader.GetValue(4).ToString();
                    dr[1] = reader.GetValue(5).ToString();
                    dr[2] = reader.GetValue(10).ToString();
                    dr[3] = reader.GetValue(6).ToString();
                    dr[4] = reader.GetValue(8).ToString();
                    dr[5] = reader.GetValue(9).ToString();
                    dr[6] = reader.GetValue(16).ToString();
                    dr[7] = reader.GetValue(17).ToString();
                    dr[8] = reader.GetValue(13).ToString();
                    dr[9] = reader.GetValue(14).ToString();
                    dt.Rows.Add(dr);
                }
            }
           
            DataTable _dt = new DataTable();
            string _sqlQuery1 = "Select [AccountName],[Appropriation] from [vwBMS_AccountApropriationExcess] where [TransactionNo]="+ exaccount  + "";
            _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery1).Tables[0];

            txt_appropriate.Value = _dt.Rows[0]["Appropriation"].ToString();
            txt_account.Value =  _dt.Rows[0]["AccountName"].ToString();

            textBox5.Value = "As Of " + Convert.ToDateTime(monthof + "/1/" + year ).ToString("MMM") + " " + year;
            table1.DataSource = dt;
        }
    }
}