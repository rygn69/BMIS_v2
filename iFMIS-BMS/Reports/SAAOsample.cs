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

    /// <summary>
    /// Summary description for Report1.
    /// </summary>
    public partial class SAAOsample : Telerik.Reporting.Report
    {
        public SAAOsample(int? OfficeID, int? month_, int? month_To, int? year, int? classtype, int? SAAO_type)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();


            if (SAAO_type == 1)
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"IFMIS.dbo.sp_Monthly_SAAO_REPORT '" + month_ + "','" + month_To + "','" + year + "','" + OfficeID + "',2", con);
                    com.CommandTimeout = 0;
                    con.Open();
                    dt3.Load(com.ExecuteReader());

                }
                this.table3.DataSource = dt3;
                //this.table1.DataSource = dt3;
                this.DataSource = dt3;
               // textBox5.Value = dt3.Rows[0]["Office_Name"].ToString();
            }
            else {

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"IFMIS.dbo.sp_Monthly_SAAO_REPORT '" + month_ + "','" + month_To + "','" + year + "','" + OfficeID + "',1", con);
                    com.CommandTimeout = 0;
                    con.Open();
                    dt3.Load(com.ExecuteReader());

                }
                this.table3.DataSource = dt3;
                //this.table1.DataSource = dt3;
               // textBox5.Value = dt3.Rows[0]["Office_Name"].ToString();
            }


            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT CONCAT((SELECT DATENAME(month, DATEADD(month, '" + month_ + "'-1, CAST('2008-01-01' AS datetime)))), " +
                                               " ' - ', (SELECT DATENAME(month, DATEADD(month, '" + month_To + "'-1, CAST('2008-01-01' AS datetime)))))", con);
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


            txt_todaydate.Value = "Date Printed : " + DateTime.Now.ToString();
            txt_user.Value = "Printed By : " + Account.UserInfo.empName;
        
        }
    }
}