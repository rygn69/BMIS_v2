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
    /// <summary>
    /// Summary description for RAO_Non_Office.
    /// </summary>
    public partial class NonOffice : Telerik.Reporting.Report
    {
        public NonOffice(int? OfficeID, int? programID, int? OOE_ID, int? accountID, int? classtype, int? year, int? sort_,int? air, string ComputerIP)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();


            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dtabs = new DataTable();
          

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select distinct a.FundType  FROM IFMIS.dbo.tbl_T_BMSCurrentControl AS a where a.OfficeID =  '" + OfficeID + "' and actioncode = 1 ", con);
                con.Open();
                txt_classtype.Value = com.ExecuteScalar().ToString();

            }

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select distinct FunctionID  from IFMIS.dbo.tbl_R_BMSOffices  where officeID =   '" + OfficeID + "' ", con);
                con.Open();
                txt_FunctionID.Value = com.ExecuteScalar().ToString();

            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select	CONCAT(c.OfficeName,' - ', d.ProgramDescription) as office_prog from  IFMIS.dbo.tbl_R_BMSOfficePrograms as d
								inner join IFMIS.dbo.tbl_R_BMSOffices as c on d.OfficeID = c.OfficeID where d.OfficeID = '" + OfficeID + "'  and actionCode = 1 " +
                                "and d.programyear = '" + year + "' ", con);
                con.Open();
                txt_office_prog.Value = com.ExecuteScalar().ToString();

            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select isnull((select top 1 CONCAT( e.AccountID ,'     ',  e.AccountName ) as account_id from IFMIS.dbo.tbl_R_BMSProgramAccounts as e where AccountYear = '" + year + "' and ActionCode = 1 and e.AccountID = '" + accountID + "'),0)", con);
                con.Open();
                txt_account.Value = com.ExecuteScalar().ToString();
                 
            } 
           

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (sort_ == 0 && air == 0)
                {

                    SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_RAO_Balance_Non] '" + OfficeID + "','" + programID + "','" + OOE_ID + "','" + accountID + "','" + year + "'", con);
                    con.Open();
                    dtabs.Load(com.ExecuteReader());

                }
                else if (sort_ == 1 && air == 0)
                {

                    SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_RAO_Balance_Non_PGO] '" + OfficeID + "','" + programID + "','" + OOE_ID + "','" + accountID + "','" + year + "'", con);
                    con.Open();
                    dtabs.Load(com.ExecuteReader());

                }
                else if (sort_ == 0 && air == 1)
                {

                    SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_RAO_Balance_Non_Air] '" + OfficeID + "','" + programID + "','" + OOE_ID + "','" + accountID + "','" + year + "'", con);
                    con.Open();
                    dtabs.Load(com.ExecuteReader());

                }
                else if (sort_ == 1 && air == 1)
                {

                    SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_RAO_Non_PGO_Air] '" + OfficeID + "','" + programID + "','" + OOE_ID + "','" + accountID + "','" + year + "'", con);
                    //SqlCommand com = new SqlCommand(@"ifmis.dbo.[sp_MonthlyRelease_RAO_Balance_Non_PGO_Air] '" + OfficeID + "','" + programID + "','" + OOE_ID + "','" + accountID + "','" + year + "'", con);
                    con.Open();
                    dtabs.Load(com.ExecuteReader());

                }

            }


            DataRow rowtabs = dtabs.Rows[0];
            txt_TotAllotment.Value = rowtabs["totalAll"].ToString();
            txt_TotObligation.Value = rowtabs["totalObli"].ToString();




            txt_todaydate.Value = "Date Printed : " + DateTime.Now.ToString();
            txt_user.Value = "Printed By : " + Account.UserInfo.empName;



           this.ReportParameters["yearss"].Value = year;



           if (sort_ == 1)
           {
           }
           else
           {
               txt_pgo.Value = "";
           }
           if (air == 1)
           {
               txt_air.Value = "This report includes Earmark";
               txt_pgo.Value = "This report includes PGO Control";
           }
           else
           {
               txt_user.Value = "";
           }

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (sort_ == 0 && air == 0)
                {

                    SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_RAO_Non] '" + OfficeID + "','" + programID + "','" + OOE_ID + "','" + accountID + "','" + year + "'", con);
                    con.Open();
                    dt2.Load(com.ExecuteReader());

                }
                else if (sort_ == 1 && air == 0)
                {

                    SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_RAO_Non_PGO] '" + OfficeID + "','" + programID + "','" + OOE_ID + "','" + accountID + "','" + year + "'", con);
                    con.Open();
                    dt2.Load(com.ExecuteReader());

                }
                else if (sort_ == 0 && air == 1)
                {

                    SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_RAO_Non_Air] '" + OfficeID + "','" + programID + "','" + OOE_ID + "','" + accountID + "','" + year + "'", con);
                    con.Open();
                    dt2.Load(com.ExecuteReader());

                }
                else if (sort_ == 1 && air == 1)
                {

                    SqlCommand com = new SqlCommand(@"ifmis.dbo.[sp_MonthlyRelease_RAO_Balance_Non_PGO_Air] '" + OfficeID + "','" + programID + "','" + OOE_ID + "','" + accountID + "','" + year + "'", con);
                    //SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_RAO_Non_PGO_Air] '" + OfficeID + "','" + programID + "','" + OOE_ID + "','" + accountID + "','" + year + "'", con);
                    con.Open();
                    dt2.Load(com.ExecuteReader());

                }

            }
            this.dt2.DataSource = dt2;
            DataTable _dt4 = new DataTable();
            string _sqlQuery4 = "SELECT [lgu],[province],isnull([address],'') address FROM [IFMIS].[dbo].[tbl_R_BMSLgu] where actioncode=1";
            _dt4 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery4).Tables[0];
            if (_dt4.Rows.Count > 0)
            {
                textBox3.Value = _dt4.Rows[0][0].ToString();
                textBox4.Value = _dt4.Rows[0][1].ToString();
                //textBox3.Value = _dt4.Rows[0][2].ToString();
                textBox40.Value = _dt4.Rows[0][0].ToString() + " - Making your task easier...";
            }
        }

    }
}