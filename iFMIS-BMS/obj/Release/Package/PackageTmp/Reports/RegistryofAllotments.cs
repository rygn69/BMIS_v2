namespace iFMIS_BMS.Reports
{
    using iFMIS_BMS.BusinessLayer.Connector;
    using iFMIS_BMS.BusinessLayer.Models;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Configuration;
    using System.Globalization;

    /// <summary>
    /// Summary description for RegistryofAllotments.
    /// </summary>
    public partial class RegistryofAllotments : Telerik.Reporting.Report
    {
        public RegistryofAllotments(int? OfficeID, int? programID, int? OOE_ID, int? accountID, int? classtype, int? year, int? sort_, int? air, string ComputerIP, int includex, int monthof, int excessacctval, string exmonhtofcessacct, int byYear,string txtsearch, int? repxmlhistory,string monthname)
        {


            InitializeComponent();

            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dtabs = new DataTable();
            var dte = "";
            dte = DateTime.Now.ToString();
            var acctname_tmp = "";

            if (repxmlhistory != 0)
            {
                DataTable _dt2 = new DataTable();
                string _sqlQuery2 = "Select [dateprinted],[username],classtype,functionid,office_prog,account,appropriation from [IFMIS].[dbo].[tbl_T_BMSReportXML] where [rprtid] = " + repxmlhistory + "";
                _dt2 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery2).Tables[0];
                txt_user.Value = "Printed By : " + _dt2.Rows[0][1].ToString();
             
                txt_classtype.Value = _dt2.Rows[0][2].ToString();
                txt_todaydate.Value = "Date Printed : " + _dt2.Rows[0][0].ToString();
                txt_FunctionID.Value = _dt2.Rows[0][3].ToString();
                txt_office_prog.Value = _dt2.Rows[0][4].ToString();
                txt_account.Value = _dt2.Rows[0][5].ToString();
                txt_appropraite.Value = _dt2.Rows[0][6].ToString();
           
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select isnull((select top 1 a.FundType  FROM IFMIS.dbo.tbl_T_BMSCurrentControl AS a where a.OfficeID =  '" + OfficeID + "' and actioncode = 1 and ProgramID = '" + programID + "' and PTOAccountCode like '%" + accountID + "%'), 'GF-Proper') ", con);
                    con.Open();
                    txt_classtype.Value = com.ExecuteScalar().ToString();

                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select distinct FunctionID  from IFMIS.dbo.tbl_R_BMSOffices  where officeID =   '" + OfficeID + "' ", con);
                    con.Open();
                    txt_FunctionID.Value = com.ExecuteScalar().ToString();

                }
                if (year >= 2017)
                {
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"select	CONCAT(c.OfficeName,' - ', d.ProgramDescription) as office_prog from  IFMIS.dbo.tbl_R_BMSOfficePrograms as d
								inner join IFMIS.dbo.tbl_R_BMSOffices as c on d.OfficeID = c.OfficeID where d.OfficeID = '" + OfficeID + "'  and actionCode = 1 " +
                                        "and d.programyear = '" + year + "' and d.ProgramID = '" + programID + "'", con);
                        con.Open();
                        txt_office_prog.Value = com.ExecuteScalar().ToString();

                    }
                }
                else
                {
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"select	CONCAT(c.OfficeName,' - ', d.[ProgramDescription]) as office_prog from  [fmis].[dbo].[tblRefBMS_BudgetProgram] as d
								inner join IFMIS.dbo.tbl_R_BMSOffices as c on d.[FmisOfficeCode] = c.OfficeID where d.[FmisOfficeCode] = '" + OfficeID + "'  and actionCode = 1 " +
                                        "and d.[YearOf] = '" + year + "' and d.[FmisProgramCode] = '" + programID + "'", con);
                        con.Open();
                        txt_office_prog.Value = com.ExecuteScalar().ToString();

                    }
                }
                if (year >= 2017)
                {
                    //using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    //{
                    //    SqlCommand com = new SqlCommand(@"select CONCAT( e.AccountID ,'     ',  e.AccountName ) as account_id from IFMIS.dbo.tbl_R_BMSProgramAccounts as e
                    //                                    inner join [IFMIS].[dbo].[tbl_R_BMSAccounts] as a on a.FMISAccountCode=e.AccountID where programID = '" + programID + "' and e.accountid = '" + accountID + "' and AccountYear = '" + year + "' and ActionCode = 1 ", con);
                    //    con.Open();
                    //    txt_account.Value = com.ExecuteScalar().ToString();
                    //    acctname_tmp = com.ExecuteScalar().ToString();

                    //}

                    DataTable _txtcode = new DataTable();
                    string _sqlqry_code = "select CONCAT( e.AccountID ,'     ',  e.AccountName ) as account_id,CONCAT(isnull(ChildAccountCode,''),'     ',  e.AccountName) as ChildAccountCode from IFMIS.dbo.tbl_R_BMSProgramAccounts as e  inner join[IFMIS].[dbo].[tbl_R_BMSAccounts] as a on a.FMISAccountCode=e.AccountID where programID = '" + programID + "' and e.accountid = '" + accountID + "' and AccountYear = '" + year + "' and ActionCode = 1 ";
                    _txtcode = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlqry_code).Tables[0];
                    if (_txtcode.Rows.Count > 0)
                    {
                        txt_account.Value = _txtcode.Rows[0][1].ToString();
                        acctname_tmp = _txtcode.Rows[0][1].ToString();
                    
                    }
                }
                else
                {
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"select CONCAT( e.[FMISAccountCode] ,'     ',  e.[BudgetAcctName] ) as account_id from [fmis].[dbo].[tblBMS_AnnualBudget_Account] as e where [FMISProgramCode] = '" + programID + "' and [FMISAccountCode] = '" + accountID + "' and [YearOf] = '" + year + "' and ActionCode = 1 ", con);
                        con.Open();
                        txt_account.Value = com.ExecuteScalar().ToString();
                        acctname_tmp = com.ExecuteScalar().ToString();

                    }
                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_Total_Approprait] '" + OfficeID + "','" + programID + "','" + OOE_ID + "','" + accountID + "','" + year + "'," + includex + "," + excessacctval + "", con);
                    con.Open();
                    txt_appropraite.Value = com.ExecuteScalar().ToString();

                }
                txt_todaydate.Value = "Date Printed : " + dte;
                txt_user.Value = "Printed By : " + Account.UserInfo.empName;
            }

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (sort_ == 0 && air == 0 && includex == 0)
                {

                    SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_RAO_Balance] '" + OfficeID + "','" + programID + "','" + OOE_ID + "','" + accountID + "','" + year + "'," + monthof + ",'"+ txtsearch + "'," + repxmlhistory + "", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    dtabs.Load(com.ExecuteReader());

                }
                else if (sort_ == 1 && air == 0 && includex == 0)
                {

                    SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_RAO_Balance_PGO] '" + OfficeID + "','" + programID + "','" + OOE_ID + "','" + accountID + "','" + year + "'," + monthof + ",'" + txtsearch + "'", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    dtabs.Load(com.ExecuteReader());

                }
                else if (sort_ == 0 && air == 1 && includex == 0)
                {

                    SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_RAO_Air_Balance] '" + OfficeID + "','" + programID + "','" + OOE_ID + "','" + accountID + "','" + year + "'," + monthof + ",'" + txtsearch + "'", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    dtabs.Load(com.ExecuteReader());

                }
                else if (sort_ == 1 && air == 1 && includex == 0)
                {

                    SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_RAO_Balance_PGO_Air] '" + OfficeID + "','" + programID + "','" + OOE_ID + "','" + accountID + "','" + year + "'," + monthof + ",'" + txtsearch + "'", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    dtabs.Load(com.ExecuteReader());

                }
                else if (includex == 1)
                {

                    SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_RAOMergeExcess_Total] '" + OfficeID + "','" + programID + "','" + accountID + "','" + year + "'," + monthof + "," + includex + "," + excessacctval + ",'" + exmonhtofcessacct + "'," + air + "," + sort_ + "," + byYear + "", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    dtabs.Load(com.ExecuteReader());

                }

            }


            DataRow rowtabs = dtabs.Rows[0];
            txt_TotAllotment.Value = rowtabs["totalAll"].ToString();
            txt_TotObligation.Value = rowtabs["totalObli"].ToString();

            decimal totalAllotment = Convert.ToDecimal(rowtabs["totalAll"]);
            decimal totalObligation = Convert.ToDecimal(rowtabs["totalObli"]);
            decimal result = totalAllotment - totalObligation;
            //textBox37.Value = result.ToString("C", CultureInfo.CurrentCulture); //dollar sign
            textBox37.Value = result.ToString("N2",CultureInfo.CurrentCulture);

            if (byYear == 1 && includex == 1)
            {
                textBox5.Value = "As of C.Y. " + year;
            }
            else
            {
                //textBox5.Value = "As of " + Convert.ToDateTime(monthof + "/25/"+ year).ToString("MMMM") + " " + year;
                textBox5.Value = "As of " + monthname + " " + year;
            }
           
            


            this.ReportParameters["year"].Value = year;
            DataTable _dt33 = new DataTable();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (sort_ == 0 && air == 0 && includex == 0)
                {
                  
                    SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_RAO] '" + OfficeID + "','" + programID + "','" + OOE_ID + "','" + accountID + "','" + year + "'," + monthof + ",'" + txtsearch + "'," + Account.UserInfo.eid + ",'"+ dte + "',"+ repxmlhistory + ",'"+ 
                    txt_classtype.Value + "', '" + txt_FunctionID.Value + "', '" + txt_office_prog.Value.Replace("'","''") + "', '" + acctname_tmp.Replace("'","''") + "', '" + txt_appropraite.Value + "'", con);
                    com.CommandTimeout = 0;
                    con.Open();
                    dt2.Load(com.ExecuteReader());
                    
                }
                else if (sort_ == 1 && air == 0 && includex == 0)
                {

                    SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_RAO_PGO] '" + OfficeID + "','" + programID + "','" + OOE_ID + "','" + accountID + "','" + year + "'," + monthof + ",'" + txtsearch + "'", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    dt2.Load(com.ExecuteReader());

                }
                else if (sort_ == 0 && air == 1 && includex == 0)
                {

                    SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_RAO_Air] '" + OfficeID + "','" + programID + "','" + OOE_ID + "','" + accountID + "','" + year + "'," + monthof + ",'" + txtsearch + "'", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    dt2.Load(com.ExecuteReader());

                }
                else if (sort_ == 1 && air == 1 && includex == 0)
                {

                    SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_RAO_PGO_Air] '" + OfficeID + "','" + programID + "','" + OOE_ID + "','" + accountID + "','" + year + "'," + monthof + ",'" + txtsearch + "'", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    dt2.Load(com.ExecuteReader());

                }
                else if (includex == 1)
                {
                    txt_OONAME.Value = exmonhtofcessacct;
                    SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_RAOMergeExcess] '" + OfficeID + "','" + programID + "','" + accountID + "','" + year + "'," + monthof + "," + includex + "," + excessacctval + ",'" + exmonhtofcessacct + "'," + air + "," + sort_ + "," + byYear + "", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    dt2.Load(com.ExecuteReader());
                    
                }

            }
            this.dt2.DataSource = dt2;

            //if (sort_ == 1)
            //{
            //    txt_pgo.Value = "This report includes PGO Control!";
            //}
            //else
            //{lgu\
            //    txt_pgo.Value = "";
            //}
            if (air == 1 && sort_ == 0)
            {
                txt_air.Value = "This report includes earmarked transaction(s)!";
            }

            else if (air == 0 && sort_ == 1)
            {
                txt_air.Value = "This report includes PGO control!";
            }
            else if (sort_ == 1 && air == 1)
            {
                txt_air.Value = "This report includes earmarked transaction(s) and PGO control!";
            }
            else
            {
                txt_air.Value = "";
                textBox31.Value = "";
            }
            DataTable _dt4 = new DataTable();
            string _sqlQuery4 = "SELECT [lgu],[province],isnull([address],'') address FROM [IFMIS].[dbo].[tbl_R_BMSLgu] where actioncode=1";
            _dt4 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery4).Tables[0];
            if (_dt4.Rows.Count > 0)
            {
                textBox3.Value = _dt4.Rows[0][0].ToString();
                textBox4.Value = _dt4.Rows[0][1].ToString();
                //textBox3.Value = _dt4.Rows[0][2].ToString();
                textBox40.Value = _dt4.Rows[0][0].ToString();// + "- Abante Tayo! Asenso Tacurong!"; //tacurong city

                textBox36.Value = "CITY BUDGET OFFICE";
            }
        }

    }
}