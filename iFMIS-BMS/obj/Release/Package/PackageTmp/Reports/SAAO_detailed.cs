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
    /// Summary description for SAAO_detailed.
    /// </summary>
    public partial class SAAO_detailed : Telerik.Reporting.Report
    {
        public SAAO_detailed(int? OfficeID, int? month_, int? month_To, int? year, int? classtype, int? SAAO_type, int? Detail_type)
        {
         
            InitializeComponent();



            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();

            if (SAAO_type == 1)
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"IFMIS.dbo.sp_Monthly_SAAO_REPORT_DETAILS '" + month_ + "','" + month_To + "','" + year + "','" + OfficeID + "',2", con);
                    com.CommandTimeout = 0;
                    con.Open();
                    dt.Load(com.ExecuteReader());

                }
                this.table3.DataSource = dt;
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"IFMIS.dbo.sp_Monthly_SAAO_REPORT_DETAILS '" + month_ + "','" + month_To + "','" + year + "','" + OfficeID + "',1", con);
                    com.CommandTimeout = 0;
                    con.Open();
                    dt.Load(com.ExecuteReader());

                }
                this.table3.DataSource = dt;
            }

            DataTable _dt2 = new DataTable();
            string _sqlQuery2 = "Select [EmpNameFull],[Position] from[vwMergeAllEmployee] where eid = " + Account.UserInfo.eid + "";
            _dt2 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisqldb"].ToString(), CommandType.Text, _sqlQuery2).Tables[0];

            textBox35.Value = _dt2.Rows[0][0].ToString();
            textBox44.Value = _dt2.Rows[0][1].ToString();


            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT CONCAT((SELECT DATENAME(month, DATEADD(month, '" + month_ + "'-1, CAST('2008-01-01' AS datetime)))), " +
                                               " ' - ', (SELECT DATENAME(month, DATEADD(month, '" + month_To + "'-1, CAST('2008-01-01' AS datetime))))) + ' ' + '" + year + "'", con);
                con.Open();
                TXT_for_the.Value = "For the period of " + com.ExecuteScalar().ToString();

            }
            if (SAAO_type == 1)
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@" select OfficeName FROM IFMIS.dbo.tbl_R_BMSOffices where OfficeID = '" + OfficeID + "'", con);
                    com.CommandTimeout = 0;
                    con.Open();
                    TXT_office_name.Value = com.ExecuteScalar().ToString();

                }
            }
            else
            {
                TXT_office_name.Value = "";
            }

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select Class_Type FROM IFMIS.dbo.tbl_R_BMS_A_Class where Class_ID = '1'", con);
                con.Open();
                TXT_fund_type.Value = com.ExecuteScalar().ToString();

            }

            DataTable _dt4 = new DataTable();
            string _sqlQuery4 = "SELECT [lgu],[province],isnull([address],'') address,[budgetofficer],[position],tagline FROM [IFMIS].[dbo].[tbl_R_BMSLgu] where actioncode=1";
            _dt4 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery4).Tables[0];
            if (_dt4.Rows.Count > 0)
            {
                textBox2.Value = _dt4.Rows[0][0].ToString();
                textBox3.Value = _dt4.Rows[0][1].ToString();
                //textBox3.Value = _dt4.Rows[0][2].ToString();
                textBox21.Value = _dt4.Rows[0][0].ToString();// + "- Abante Tayo! Asenso Tacurong!";
                textBox36.Value = _dt4.Rows[0][3].ToString();
                textBox43.Value = _dt4.Rows[0][4].ToString();
            }


            txt_todaydate.Value = "Date Printed : " + DateTime.Now.ToString();
            txt_user.Value = "Printed By : " + Account.UserInfo.empName;







        }
    }
}