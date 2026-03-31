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
    public partial class SAAO : Telerik.Reporting.Report
    {
        public SAAO(int? OfficeID, int? month_, int? month_To, int? year, int? classtype, int? SAAO_type,int? earmark_type,int? repxmlhistory)
        {
         
            InitializeComponent();

            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();

            var dte = "";
           
            if (repxmlhistory != 0)
            {
                DataTable _dt2 = new DataTable();
                string _sqlQuery2 = "Select [username],[dateprinted],[preparedby],[preparedbyposition],[notedby],[notedbyposition],[qrcode] from [IFMIS].[dbo].[tbl_T_BMSReportXML] where [rprtid] = " + repxmlhistory + "";
                _dt2 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery2).Tables[0];
                txt_user.Value = "Printed By : " + _dt2.Rows[0][0].ToString();
                textBox35.Value = _dt2.Rows[0][2].ToString();
                textBox44.Value = _dt2.Rows[0][3].ToString();
                txt_todaydate.Value = "Date Printed : " + _dt2.Rows[0][1].ToString();
                textBox36.Value= _dt2.Rows[0][4].ToString();
                textBox43.Value = _dt2.Rows[0][5].ToString();
                barcode1.Value = _dt2.Rows[0][6].ToString();
            }
            else
            {
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
            }

            GlobalFunctions.QR_globalstr = barcode1.Value;
            if (earmark_type == 3)
            {
                textBox7.Value = "Note: Obligation includes earmarked amount!";
            }
            if (SAAO_type == 1)
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    if (repxmlhistory != 0) {
                        SqlCommand com = new SqlCommand(@"exec sp_BMS_ReportXML_read 1," + repxmlhistory + ",0,"+ OfficeID + "", con);
                        com.CommandTimeout = 0;
                        con.Open();
                        dt3.Load(com.ExecuteReader());
                    }
                    else
                    {
                        SqlCommand com = new SqlCommand(@"IFMIS.dbo.sp_Monthly_SAAO_REPORT_Test '" + month_ + "','" + month_To + "','" + year + "','" + OfficeID + "',2," + earmark_type + "," + Account.UserInfo.eid + ",'"+ barcode1.Value + "','" + textBox35.Value + "','" + textBox44.Value + "','JAVE NHORIEL N. BORDAJE, CPA','Provincial Budget Officer','"+ dte + "',0", con);
                        com.CommandTimeout = 0;
                        con.Open();
                        dt3.Load(com.ExecuteReader());
                    }
                }
                this.table3.DataSource = dt3;
            }
            else {

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"IFMIS.dbo.sp_Monthly_SAAO_REPORT_Test '" + month_ + "','" + month_To + "','" + year + "','" + OfficeID + "',1," + earmark_type + "", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    dt3.Load(com.ExecuteReader());

                }
                this.table3.DataSource = dt3;
               // this.table1.DataSource = dt3;
               // this.table2.DataSource = dt3;
            }

           

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT CONCAT((SELECT DATENAME(month, DATEADD(month, '" + month_ + "'-1, CAST('2008-01-01' AS datetime)))), " +
                                               " ' - ', (SELECT DATENAME(month, DATEADD(month, '" + month_To + "'-1, CAST('2008-01-01' AS datetime))))) + ' ' + '" + year + "'", con);
                con.Open();
                if (month_To == 1) {
                    TXT_for_the.Value = "For the period of January " + year;
                }
                else
                {
                    TXT_for_the.Value = "For the period of " + com.ExecuteScalar().ToString();
                }

            }
            if (SAAO_type == 1)
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    if (year <= 2021)
                    {
                        SqlCommand com = new SqlCommand(@" select case when OfficeID=27 then 'Bunawan District Hospital' else OfficeName end OfficeName FROM IFMIS.dbo.tbl_R_BMSOffices where OfficeID = '" + OfficeID + "'", con);
                        com.CommandTimeout = 0;
                        con.Open();
                        TXT_office_name.Value = com.ExecuteScalar().ToString();
                    }
                    else
                    {
                        SqlCommand com = new SqlCommand(@" select OfficeName FROM IFMIS.dbo.tbl_R_BMSOffices where OfficeID = '" + OfficeID + "'", con);
                        com.CommandTimeout = 0;
                        con.Open();
                        TXT_office_name.Value = com.ExecuteScalar().ToString();
                    }

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
            //barcode1.Value = FUNCTION.GeneratePISControl();
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
                textBox21.Value = _dt4.Rows[0][0].ToString(); //+ "- Abante Tayo! Asenso Tacurong!"; //tacurong city
                textBox36.Value = _dt4.Rows[0][3].ToString();
                textBox43.Value = _dt4.Rows[0][4].ToString();
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                con.Close();
                SqlCommand com = new SqlCommand(@"select  controlno from ifmis.dbo.tbl_T_BMSReportXML where qrcode='" + barcode1.Value + "' and office=" + OfficeID + " and yearof= " + year + " ", con);
                con.Open();
                textBox14.Value = com.ExecuteScalar().ToString();

            }
        }
    }
}