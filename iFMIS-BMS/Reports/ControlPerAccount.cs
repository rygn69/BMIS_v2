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
    using iFMIS_BMS.Base;
    using iFMIS_BMS.Classes;
    using System.Configuration;

    /// <summary>
    /// Summary description for BOI.
    /// </summary>
    public partial class ControlPerAccount : Telerik.Reporting.Report
    {
        public ControlPerAccount(int? OfficeID, int? year, string controlno="")
        {
           
            InitializeComponent();

            DataTable dt = new DataTable();
            
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_CurrentComputation_Control] "+ OfficeID + ","+ year + ",'"+ controlno + "'", con);
                    con.Open();
                    dt.Load(com.ExecuteReader());
                }
            table2.DataSource = dt;

            DataTable _dtSig = new DataTable();
            string _sqlQuery = "exec sp_BMS_ControlHeader '" + controlno + "'";
            _dtSig = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery).Tables[0];
            textBox50.Value = _dtSig.Rows[0]["OfficeName"].ToString();
            textBox51.Value = _dtSig.Rows[0]["ProgramDescription"].ToString();
            textBox53.Value = _dtSig.Rows[0]["ppsascode"].ToString();
            textBox52.Value = _dtSig.Rows[0]["OOE_Name"].ToString();
            textBox18.Value = _dtSig.Rows[0]["AccountName"].ToString();
            textBox17.Value = _dtSig.Rows[0]["datetime"].ToString();

            barcode1.Value = FUNCTION.GeneratePISControl();


            DataTable _dt2 = new DataTable();
            string _sqlQuery2 = "Select [EmpNameFull],[Position] from[vwMergeAllEmployee] where eid = " + Account.UserInfo.eid + "";
            _dt2 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisqldb"].ToString(), CommandType.Text, _sqlQuery2).Tables[0];
            textBox4.Value = _dt2.Rows[0][0].ToString();
            textBox6.Value = _dt2.Rows[0][1].ToString();

            DataTable reportlog = new DataTable();
            using (SqlConnection conUser = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@" exec [sp_BMS_ReportLog] 14," + Account.UserInfo.eid + ",'Control','" + barcode1.Value + "'", conUser);
                conUser.Open();
                reportlog.Load(com.ExecuteReader());

            }
        }
    }
}