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
    using System.Configuration;
    using iFMIS_BMS.Base;
    using iFMIS_BMS.Classes;

    /// <summary>
    /// Summary description for exces_budget.
    /// </summary>
    public partial class Commitment : Telerik.Reporting.Report
    {
        public Commitment(int OfficeID,int? program, long? account, int? year, int? excess,string accountname,int? commitmentthistory)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            DataTable dt = new DataTable();

            barcode1.Value = FUNCTION.GeneratePISControl();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (commitmentthistory != 0) {
                    SqlCommand com = new SqlCommand(@"exec ifmis.dbo.[sp_BMS_Commitmentreadxml] " + commitmentthistory + "", con);
                    con.Open();
                    dt.Load(com.ExecuteReader());
                    con.Close();
                }
                else
                {
                    SqlCommand com = new SqlCommand(@"exec ifmis.dbo.[sp_BMS_CommitmentReport] " + OfficeID + "," + program + "," + account + "," + year + "," + excess + ",'" + barcode1.Value + "'," + Account.UserInfo.eid + "", con);
                    con.Open();
                    dt.Load(com.ExecuteReader());
                    con.Close();
                }
            }


            this.dt.DataSource = dt;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (excess == 0)
                {
                    SqlCommand com = new SqlCommand(@" Select format(dbo.fn_BMS_Appropriation (" + OfficeID + "," + account + "," + program + "," + year + "),'##,##0.00')", con);
                    con.Open();
                    txt_FundType.Value = accountname; //+" (Current charges)";
                    textBox19.Value = com.ExecuteScalar().ToString();
                }
                else
                {
                    DataTable _dt2 = new DataTable();
                    string _sqlQuery2 = "select  Account,format(amount,'##,##0.00') FROM ifmis.dbo.tbl_T_BMSExcessAppropriation where TransactionNo = " + account + " and [ActionCode]=1";
                    _dt2 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery2).Tables[0];

                    txt_FundType.Value = _dt2.Rows[0][0].ToString();// + "  (Continuing charges)";
                    textBox19.Value = _dt2.Rows[0][1].ToString();
                }
            }
            if (commitmentthistory != 0)
            {
                DataTable _dt2 = new DataTable();
                string _sqlQuery2 = "select format(cast(datetimegenerate as date),'MMMM yyyy'),  [datetimegenerate],b.Lastname + ', '+b.Firstname +' '+ left(b.MI,1) + '. '+isnull(b.Suffix,'') FROM [IFMIS].[dbo].[tbl_T_BMSCommitment_xml]  as a left join " +
                                        "pmis.dbo.employee as b on b.eid = a.userid where [com_id] = " + commitmentthistory + " ";

                _dt2 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery2).Tables[0];
                textBox17.Value = "As of " + _dt2.Rows[0][0].ToString();
                txt_todaydate.Value = "Date Printed : " + _dt2.Rows[0][1].ToString();
                txt_user.Value = "Printed by : "+ _dt2.Rows[0][2].ToString(); ;
            }
            else
            {
                textBox17.Value = "As of " + DateTime.Now.ToString("MMMM yyyy");
                txt_todaydate.Value = "Date Printed : " + DateTime.Now.ToString();
                txt_user.Value = "Printed by : " + Account.UserInfo.empName;
            }
        }
    }
}       