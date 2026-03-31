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
    /// Summary description for Realignment.
    /// </summary>
    public partial class Realignment : Telerik.Reporting.Report
    {
        public Realignment(int? OfficeID, int? month_, int? month_To, int? year, int? classtype, int? All_Type, int? realign_type)
        {
           
            InitializeComponent();

            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_MonthlyRelease_Report_Realignment '" + OfficeID + "','" + month_ + "','" + month_To + "','" + year + "','" + All_Type + "','" + realign_type + "'", con);
                com.CommandTimeout = 0;
                con.Open();
                dt.Load(com.ExecuteReader());

            }
            this.table1.DataSource = dt;


            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT CONCAT((SELECT DATENAME(month, DATEADD(month, '" + month_ + "'-1, CAST('2008-01-01' AS datetime)))), " +
                                               " ' - ', (SELECT DATENAME(month, DATEADD(month, '" + month_To + "'-1, CAST('2008-01-01' AS datetime)))))", con);
                con.Open();
                TXT_for_the.Value = "For the period of " + com.ExecuteScalar().ToString();

            }

            if (All_Type == 1)
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
                TXT_office_name.Value = "All Offices";
            }
            if (realign_type == 1)
            { TXT_realign_type.Value = "Realign From"; }
            else 
            { TXT_realign_type.Value = "Realign To"; }
           
               
               

            
            txt_todaydate.Value = "Date Printed : " + DateTime.Now.ToString();
            txt_user.Value = "Printed by : " + Account.UserInfo.empName;

        }
    }
}