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
    /// Summary description for Report1.
    /// </summary>
    public partial class SAAO_Excess_Summary : Telerik.Reporting.Report
    {
        public SAAO_Excess_Summary(int? Fundtype, string changed_To, int? month_, int? month_To, int? year,int? tyear)
        {
         
            InitializeComponent();

            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();

            var dte = "";
           
           
            DataTable _dt2 = new DataTable();
            string _sqlQuery2 = "Select [EmpNameFull],[Position] from[vwMergeAllEmployee] where eid = " + Account.UserInfo.eid + "";
            _dt2 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisqldb"].ToString(), CommandType.Text, _sqlQuery2).Tables[0];
            if (_dt2.Rows.Count > 0)
            {
                textBox35.Value =  _dt2.Rows[0][0].ToString();
                textBox44.Value = _dt2.Rows[0][1].ToString();
                txt_user.Value = "Printed By : " + Account.UserInfo.empName;
            }
            else
            {
                textBox35.Value = "";
                textBox44.Value = "";
                txt_user.Value = "";
            }
            dte = DateTime.Now.ToString();
            txt_todaydate.Value = "Date Printed : " + dte;
            barcode1.Value = FUNCTION.GeneratePISControl();

            textBox36.Value = "JAVE NHORIEL N. BORDAJE, CPA";
          
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@" select  CONCAT(FundName, ' ' , '(' +FundMedium +')') as FundName FROM [IFMIS].[dbo].[tbl_R_BMSFunds] where FundFlag = '" + Fundtype + "'", con);
                con.Open();
                TXT_fund_type.Value = com.ExecuteScalar().ToString();

            }

            GlobalFunctions.QR_globalstr = barcode1.Value;

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"IFMIS.dbo.sp_BMS_BOIExcess_SAAO '" + changed_To + "'," + Fundtype + "," + month_ + "," + month_To + ",0," + Account.UserInfo.eid + ",1", con);
                con.Open();
                com.CommandTimeout = 0;
                dt3.Load(com.ExecuteReader());

            }
            this.table2.DataSource = dt3;
          
          
            DataTable reportlog = new DataTable();
            using (SqlConnection conUser = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@" exec [sp_BMS_ReportLog] 14," + Account.UserInfo.eid + ",'SAAO','" + barcode1.Value + "'", conUser);
                conUser.Open();
                reportlog.Load(com.ExecuteReader());

            }
            DataTable _dt4 = new DataTable();
            string _sqlQuery4 = "SELECT [lgu],[province],isnull([address],'') address,[budgetofficer],[position],tagline FROM [IFMIS].[dbo].[tbl_R_BMSLgu] where actioncode=1";
            _dt4 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery4).Tables[0];
            if (_dt4.Rows.Count > 0)
            {
                textBox2.Value = _dt4.Rows[0][0].ToString();
                textBox3.Value = _dt4.Rows[0][1].ToString();
                //textBox3.Value = _dt4.Rows[0][2].ToString();
                textBox21.Value = _dt4.Rows[0][0].ToString();// + "- Abante Tayo! Asenso Tacurong!"; //tacurong city
                textBox36.Value = _dt4.Rows[0][3].ToString();
                textBox43.Value = _dt4.Rows[0][4].ToString();
            }
           
        }
        
    }
}