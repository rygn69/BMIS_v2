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
    /// Summary description for SAAO_mid.
    /// </summary>
    public partial class SAAO_mid : Telerik.Reporting.Report
    {
        public SAAO_mid(int? OfficeID, int? month_, int? month_To, int? year, int? classtype, int? SAAO_type, int? earmark_type, int? repxmlhistory, string dtetime, int? saaotag)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();

            if (repxmlhistory != 0)
            {
                DataTable _dt2 = new DataTable();
                string _sqlQuery2 = "Select [username],[dateprinted],[preparedby],[preparedbyposition],[notedby],[notedbyposition],[qrcode] from [IFMIS].[dbo].[tbl_T_BMSReportXML] where [rprtid] = " + repxmlhistory + "";
                _dt2 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery2).Tables[0];
                txt_user.Value = "Printed By : " + _dt2.Rows[0][0].ToString();
                txt_todaydate.Value = "Date Printed : " + _dt2.Rows[0][1].ToString();
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"exec sp_BMS_ReportXML_read 4," + repxmlhistory + "," + saaotag + "," + OfficeID + "", con);
                    com.CommandTimeout = 0;
                    con.Open();
                    dt3.Load(com.ExecuteReader());
                }
                this.table3.DataSource = dt3;
            }
            else
            {

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"IFMIS.dbo.sp_Monthly_SAAO_REPORT_Test '" + month_ + "','" + month_To + "','" + year + "','" + OfficeID + "',1," + earmark_type + "," + Account.UserInfo.eid + ",'','','','','','" + dtetime + "',1", con);
                    com.CommandTimeout = 0;
                    con.Open();
                    dt3.Load(com.ExecuteReader());

                }
                this.table3.DataSource = dt3;

                txt_todaydate.Value = "Date Printed : " + dtetime;
                txt_user.Value = "Printed By : " + Account.UserInfo.empName;
            }

            DataTable _dt4 = new DataTable();
            string _sqlQuery4 = "SELECT [lgu],[province],isnull([address],'') address,[budgetofficer],[position] FROM [IFMIS].[dbo].[tbl_R_BMSLgu] where actioncode=1";
            _dt4 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery4).Tables[0];
            if (_dt4.Rows.Count > 0)
            {
                //textBox2.Value = _dt4.Rows[0][0].ToString();
                //textBox3.Value = _dt4.Rows[0][1].ToString();
                //textBox3.Value = _dt4.Rows[0][2].ToString();
                textBox21.Value = _dt4.Rows[0][0].ToString();// + "- Abante Tayo! Asenso Tacurong!";

            }
        }
    }
}