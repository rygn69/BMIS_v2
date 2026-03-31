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
    /// Summary description for total_release.
    /// </summary>
    public partial class total_release : Telerik.Reporting.Report
    {
        public total_release(int? year, int? month_, int? month_To,int? repxmlhistory)
        {
        
            InitializeComponent();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            //--GF
            if (repxmlhistory == 0)
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"dbo.sp_MonthlyRelease_Report_TR '" + year + "','" + month_ + "','" + month_To + "'," + Account.UserInfo.eid.ToString() + "," + repxmlhistory + ",1,0", con);
                    con.Open();
                    dt.Load(com.ExecuteReader());

                }
                //--EE

                this.dt.DataSource = dt;

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"dbo.sp_MonthlyRelease_Report_TR_EE '" + year + "','" + month_ + "','" + month_To + "'", con);
                    con.Open();
                    dt2.Load(com.ExecuteReader());

                }

                this.data_table.DataSource = dt2;

                //--total

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"dbo.sp_MonthlyRelease_Report_TR_total '" + year + "','" + month_ + "','" + month_To + "'", con);
                    con.Open();
                    dt3.Load(com.ExecuteReader());

                }

                this.total_tabs.DataSource = dt3;

                //--service debt

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    //SqlCommand com = new SqlCommand(@"select AccountName, AccountID  FROM IFMIS.dbo.tbl_R_BMSProgramAccounts where AccountID = 478 and AccountYear = '" + year + "' and ActionCode = 1", con);
                    SqlCommand com = new SqlCommand(@"select AccountName  FROM IFMIS.dbo.tbl_R_BMSProgramAccounts where AccountID = 478 and AccountYear = '" + year + "' and ActionCode = 1", con);
                    con.Open();
                    service_debt.Value = com.ExecuteScalar().ToString();

                }


                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select format(CAST(isnull(( SELECT   format(isnull(sum(a.AmountPS),0),'N2') 
                                                   FROM [IFMIS].[dbo].[tbl_R_BMS_Release] as a
                                                  where a.YearOf = '" + year + "' and a.ActionCode = 1 and a.FMISAccountCode = 478 and a.MonthOf >= '" + month_ + "' and a.MonthOf <= '" + month_To + "'),0)AS MONEY),'N2')", con);
                    con.Open();
                    sb_PS.Value = com.ExecuteScalar().ToString();

                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select format(CAST(isnull(( SELECT   format(isnull(sum(a.AmountMOOE),0),'N2')
                                                   FROM [IFMIS].[dbo].[tbl_R_BMS_Release]  as a
                                                  where a.YearOf = '" + year + "' and a.ActionCode = 1 and a.FMISAccountCode = 478 and a.MonthOf >= '" + month_ + "' and a.MonthOf <= '" + month_To + "' ),0)AS MONEY),'N2')", con);
                    con.Open();
                    sb_MOOE.Value = com.ExecuteScalar().ToString();

                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select format(CAST(isnull(( SELECT format(isnull(sum(a.AmountCO),0),'N2')
                                                   FROM [IFMIS].[dbo].[tbl_R_BMS_Release] as a
                                                  where a.YearOf = '" + year + "' and a.ActionCode = 1 and a.FMISAccountCode = 478 and a.MonthOf >= '" + month_ + "' and a.MonthOf <= '" + month_To + "'),0)AS MONEY),'N2')", con);
                    con.Open();
                    sb_CO.Value = com.ExecuteScalar().ToString();

                }
                //--non office

                if (year < 2021)
                {
                
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"  select distinct b.OfficeName
                                                          from IFMIS.dbo.tbl_R_BMS_Release  as a
                                                          left join IFMIS.dbo.tbl_R_BMSOffices as b
                                                          on a.FMISOfficeCode = b.OfficeID
                                                          where a.YearOf = '" + year + "' and a.ActionCode = 1 and FMISOfficeCode = 43 " +
                                                              "and FMISAccountCode not in (478,2861) and a.MonthOf >= '" + month_ + "' and a.MonthOf <= '" + month_To + "'", con);
                        con.Open();
                        non_office.Value = com.ExecuteScalar().ToString();

                    }



                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"select format(CAST(isnull(( SELECT format(isnull(sum(a.AmountPS),0),'N2')
                                                       FROM [IFMIS].[dbo].[tbl_R_BMS_Release] as a
                                                      where a.YearOf = '" + year + "' and a.ActionCode = 1 and a.FMISOfficeCode = 43 and a.FMISAccountCode not in (478,2861) and a.MonthOf >= '" + month_ + "' and a.MonthOf <= '" + month_To + "'),0)AS MONEY),'N2')", con);
                        con.Open();
                        no_PS.Value = com.ExecuteScalar().ToString();

                    }


                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"exec sp_BMS_ReleaseNOMooe " + year + "," + month_To + "", con);
                        con.Open();
                        no_MOOE.Value = com.ExecuteScalar().ToString();

                    }


                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"select format(CAST(isnull(( SELECT format(isnull(sum(a.AmountCO),0),'N2')
                                                       FROM [IFMIS].[dbo].[tbl_R_BMS_Release] as a
                                                      where a.YearOf = '" + year + "' and a.ActionCode = 1 and a.FMISOfficeCode = 43 and a.FMISAccountCode not in (478,2861) and a.MonthOf >= '" + month_ + "' and a.MonthOf <= '" + month_To + "'),0)AS MONEY),'N2')", con);
                        con.Open();
                        no_CO.Value = com.ExecuteScalar().ToString();

                    }
                    //---20% dev

                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@" select AccountName from IFMIS.dbo.tbl_R_BMSAccounts where FMISAccountCode = 2861", con);
                        con.Open();
                        dev_.Value = com.ExecuteScalar().ToString();

                    }



                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"select format(CAST(isnull(( SELECT   format(isnull(sum(a.AmountPS),0),'N2') 
                                                       FROM [IFMIS].[dbo].[tbl_R_BMS_Release] as a
                                                      where a.YearOf = '" + year + "' and a.ActionCode = 1 and a.FMISAccountCode = 2861 and a.MonthOf >= '" + month_ + "' and a.MonthOf <= '" + month_To + "'),0)AS MONEY),'N2')", con);
                        con.Open();
                        dev_PS.Value = com.ExecuteScalar().ToString();

                    }


                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"select format(CAST(isnull(( SELECT   format(isnull(sum(a.AmountMOOE),0),'N2') 
                                                       FROM [IFMIS].[dbo].[tbl_R_BMS_Release] as a
                                                      where a.YearOf = '" + year + "' and a.ActionCode = 1 and a.FMISAccountCode = 2861 and a.MonthOf >= '" + month_ + "' and a.MonthOf <= '" + month_To + "'),0)AS MONEY),'N2')", con);
                        con.Open();
                        dev_MOOE.Value = com.ExecuteScalar().ToString();

                    }


                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"select format(CAST(isnull(( SELECT   format(isnull(sum(a.AmountCO),0),'N2') 
                                                       FROM [IFMIS].[dbo].[tbl_R_BMS_Release] as a
                                                      where a.YearOf = '" + year + "' and a.ActionCode = 1 and a.FMISAccountCode = 2861 and a.MonthOf >= '" + month_ + "' and a.MonthOf <= '" + month_To + "'),0)AS MONEY),'N2')", con);
                        con.Open();
                        dev_CO.Value = com.ExecuteScalar().ToString();

                    }
                }
                else
                {
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"  select distinct b.OfficeName
                                                      from IFMIS.dbo.tbl_R_BMS_Release  as a
                                                      left join IFMIS.dbo.tbl_R_BMSOffices as b
                                                      on a.FMISOfficeCode = b.OfficeID
                                                      where a.YearOf = '" + year + "' and a.ActionCode = 1 and FMISOfficeCode = 1 " +
                                                              "and FMISAccountCode not in (478,2861) and a.MonthOf >= '" + month_ + "' and a.MonthOf <= '" + month_To + "'", con);
                        con.Open();
                        non_office.Value = com.ExecuteScalar().ToString();

                    }



                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"select format(CAST(isnull(( SELECT format(isnull(sum(a.AmountPS),0),'N2')
                                                   FROM [IFMIS].[dbo].[tbl_R_BMS_Release] as a
                                                  where a.YearOf = '" + year + "' and a.ActionCode = 1 and a.FMISOfficeCode = 1 and a.FMISAccountCode not in (478,2861) and a.MonthOf >= '" + month_ + "' and a.MonthOf <= '" + month_To + "'),0)AS MONEY),'N2')", con);
                        con.Open();
                        no_PS.Value = com.ExecuteScalar().ToString();

                    }


                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"exec sp_BMS_ReleaseNOMooe " + year + "," + month_To + "", con);
                        con.Open();
                        no_MOOE.Value = com.ExecuteScalar().ToString();

                    }


                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"select format(CAST(isnull(( SELECT format(isnull(sum(a.AmountCO),0),'N2')
                                                   FROM [IFMIS].[dbo].[tbl_R_BMS_Release] as a
                                                  where a.YearOf = '" + year + "' and a.ActionCode = 1 and a.FMISOfficeCode = 1 and a.FMISAccountCode not in (478,2861) and a.MonthOf >= '" + month_ + "' and a.MonthOf <= '" + month_To + "'),0)AS MONEY),'N2')", con);
                        con.Open();
                        no_CO.Value = com.ExecuteScalar().ToString();

                    }
                    //---20% dev

                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@" select AccountName from IFMIS.dbo.tbl_R_BMSAccounts where FMISAccountCode = 2861", con);
                        con.Open();
                        dev_.Value = com.ExecuteScalar().ToString();

                    }



                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"select format(CAST(isnull(( SELECT   format(isnull(sum(a.AmountPS),0),'N2') 
                                                   FROM [IFMIS].[dbo].[tbl_R_BMS_Release] as a
                                                  where a.YearOf = '" + year + "' and a.ActionCode = 1 and a.FMISAccountCode = 2861 and a.MonthOf >= '" + month_ + "' and a.MonthOf <= '" + month_To + "'),0)AS MONEY),'N2')", con);
                        con.Open();
                        dev_PS.Value = com.ExecuteScalar().ToString();

                    }


                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"select format(CAST(isnull(( SELECT   format(isnull(sum(a.AmountMOOE),0),'N2') 
                                                   FROM [IFMIS].[dbo].[tbl_R_BMS_Release] as a
                                                  where a.YearOf = '" + year + "' and a.ActionCode = 1 and a.FMISAccountCode = 2861 and a.MonthOf >= '" + month_ + "' and a.MonthOf <= '" + month_To + "'),0)AS MONEY),'N2')", con);
                        con.Open();
                        dev_MOOE.Value = com.ExecuteScalar().ToString();

                    }


                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"select format(CAST(isnull(( SELECT   format(isnull(sum(a.AmountCO),0),'N2') 
                                                   FROM [IFMIS].[dbo].[tbl_R_BMS_Release] as a
                                                  where a.YearOf = '" + year + "' and a.ActionCode = 1 and a.FMISAccountCode = 2861 and a.MonthOf >= '" + month_ + "' and a.MonthOf <= '" + month_To + "'),0)AS MONEY),'N2')", con);
                        con.Open();
                        dev_CO.Value = com.ExecuteScalar().ToString();

                    }
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"dbo.sp_MonthlyRelease_Report_TR_grandTotal '" + year + "','" + month_ + "','" + month_To + "'", con);
                    con.Open();
                    grand_total.Value = "GRAND TOTAL ====>>>>  " + com.ExecuteScalar().ToString();

                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"SELECT CONCAT((SELECT DATENAME(month, DATEADD(month, '" + month_ + "'-1, CAST('2008-01-01' AS datetime)))), " +
                                                   " ' - ', (SELECT DATENAME(month, DATEADD(month, '" + month_To + "'-1, CAST('2008-01-01' AS datetime)))))", con);
                    con.Open();
                    date_desuni.Value = com.ExecuteScalar().ToString();

                }


                txt_todaydate.Value = "Date Printed : " + DateTime.Now.ToString();
                txt_user.Value = "Printed by : " + Account.UserInfo.empName;
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"dbo.sp_MonthlyRelease_Report_TR '" + year + "','" + month_ + "','" + month_To + "'," + Account.UserInfo.eid.ToString() + "," + repxmlhistory + ",1,0", con);
                    con.Open();
                    dt.Load(com.ExecuteReader());

                }
                //--EE

                this.dt.DataSource = dt;

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"dbo.sp_MonthlyRelease_Report_TR '" + year + "','" + month_ + "','" + month_To + "'," + Account.UserInfo.eid.ToString() + "," + repxmlhistory + ",2,0", con);
                    con.Open();
                    dt2.Load(com.ExecuteReader());

                }

                this.data_table.DataSource = dt2;

                //--total

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"dbo.sp_MonthlyRelease_Report_TR '" + year + "','" + month_ + "','" + month_To + "'," + Account.UserInfo.eid.ToString() + "," + repxmlhistory + ",0,1", con);
                    con.Open();
                    dt3.Load(com.ExecuteReader());
                }
                this.total_tabs.DataSource = dt3;

                DataTable _dt2 = new DataTable();
                string _sqlQuery2 = "exec dbo.sp_MonthlyRelease_Report_TR '" + year + "','" + month_ + "','" + month_To + "'," + Account.UserInfo.eid.ToString() + "," + repxmlhistory + ",4,0";
                _dt2 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery2).Tables[0];
                service_debt.Value = _dt2.Rows[0][4].ToString();
                sb_PS.Value = _dt2.Rows[0][0].ToString();
                sb_MOOE.Value = _dt2.Rows[0][1].ToString();
                sb_CO.Value = _dt2.Rows[0][2].ToString();

                DataTable _dt3 = new DataTable();
                string _sqlQuery3 = "exec dbo.sp_MonthlyRelease_Report_TR '" + year + "','" + month_ + "','" + month_To + "'," + Account.UserInfo.eid.ToString() + "," + repxmlhistory + ",3,0";
                _dt3 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery3).Tables[0];
                non_office.Value = _dt3.Rows[0][4].ToString();
                no_PS.Value = _dt3.Rows[0][0].ToString();
                no_MOOE.Value = _dt3.Rows[0][1].ToString();
                no_CO.Value = _dt3.Rows[0][2].ToString();

                DataTable _dt4 = new DataTable();
                string _sqlQuery4 = "exec dbo.sp_MonthlyRelease_Report_TR '" + year + "','" + month_ + "','" + month_To + "'," + Account.UserInfo.eid.ToString() + "," + repxmlhistory + ",5,0";
                _dt4 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery4).Tables[0];
                dev_.Value = _dt4.Rows[0][4].ToString();
                dev_PS.Value = _dt4.Rows[0][0].ToString();
                dev_MOOE.Value = _dt4.Rows[0][1].ToString();
                dev_CO.Value = _dt4.Rows[0][2].ToString();

                DataTable _dt5 = new DataTable();
                string _sqlQuery5 = "exec dbo.sp_MonthlyRelease_Report_TR '" + year + "','" + month_ + "','" + month_To + "'," + Account.UserInfo.eid.ToString() + "," + repxmlhistory + ",0,2";
                _dt5 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery5).Tables[0];
                grand_total.Value = "GRAND TOTAL ====>>>>  " + _dt5.Rows[0][0].ToString();

                DataTable _dt6 = new DataTable();
                string _sqlQuery6 = "Select [username],[dateprinted],case when [monthof] =1 then 'January' else 'January - ' + format(cast(cast([monthof] as varchar(2)) + '/1/'+ cast(yearof as varchar(4)) as date),'MMMM') end 'month' from [IFMIS].[dbo].[tbl_T_BMSReportXML] where [rprtid]=" + repxmlhistory + "";
                _dt6 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery6).Tables[0];
                txt_todaydate.Value = "Date Printed : " + _dt6.Rows[0][1].ToString();
                txt_user.Value = "Printed by : " + _dt6.Rows[0][0].ToString();
                date_desuni.Value = _dt6.Rows[0][2].ToString();
            }
         
        }
    }
}