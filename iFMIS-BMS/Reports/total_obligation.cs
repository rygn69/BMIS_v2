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


    public partial class total_obligation : Telerik.Reporting.Report
    {
        public total_obligation(int? year, int? month_, int? month_To)
        {
            
            InitializeComponent();
            
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            //--GF
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_MonthlyRelease_Report_TO '" + year + "','" + month_ + "','" + month_To + "'", con);
                con.Open();
                dt.Load(com.ExecuteReader());

            }
            //--EE

            this.dt.DataSource = dt;

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_MonthlyRelease_Report_TO_EE '" + year + "','" + month_ + "','" + month_To + "'", con);
                con.Open();
                dt2.Load(com.ExecuteReader());

            }

            this.data_table.DataSource = dt2;



            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_MonthlyRelease_Report_TO_total '" + year + "','" + month_ + "','" + month_To + "'", con);
                con.Open();
                dt3.Load(com.ExecuteReader());

            }

            this.total_tabs.DataSource = dt3;

            //--service debt

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select AccountName, AccountID  FROM IFMIS.dbo.tbl_R_BMSProgramAccounts where AccountID = 478 and AccountYear = '" +  year +"' and ActionCode = 1", con);
                con.Open();
                service_debt.Value = com.ExecuteScalar().ToString();

            }


            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select format(CAST(isnull((SELECT  format(isnull(SUM(A.Amount),0),'N2') FROM IFMIS.dbo.tbl_T_BMSSubsidiaryLedger as a
                                                  WHERE  case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '201' 
													  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '101'  
													  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '119'
													   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '118' 
													   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '127' 
                                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '109'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '128'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '131'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '129'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '132'
													  then cast('20' + SUBSTRING(a.[OBRNo], 10, 2) as varchar ) 
													  end like '" + year + "' and a.ActionCode = 1 " +
                                                  "and a.FMISAccountCode = 478 and a.OOEID = 1 and case when cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '201' " +
     "  or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '101' " +
     "  or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '119'" +
     "  or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '118' " +
     "  or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '127' " +
     "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '109' " +
     "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '128' " +
     "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '131' " +
     "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '129' " +
     "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '132' " +
     " then  cast(SUBSTRING([OBRNo], 13, 2) as int ) " +
     " end >= '" + month_ + "' and " + " case when cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '201'   or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '101' " +
     "  or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '119'" +
     "  or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '118' " +
     "  or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '127' " +
     "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '109' " +
     "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '128' " +
     "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '131' " +
     "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '129' " +
     "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '132' " +
     " then  cast(SUBSTRING([OBRNo], 13, 2) as int ) " +
     " end <= '" + month_To + "'and len(a.[OBRNo]) = 19 and left(a.[OBRNo],3) <> '20-' " +
                                                  "GROUP BY a.FMISAccountCode),0)AS MONEY),'N2')", con);
                con.Open();
                sb_PS.Value = com.ExecuteScalar().ToString();

            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select format(CAST(isnull((SELECT  format(isnull(SUM(A.Amount),0),'N2') FROM IFMIS.dbo.tbl_T_BMSSubsidiaryLedger as a
                                                  WHERE  case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '201' 
													  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '101'  
													  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '119'
													   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '118' 
													   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '127' 
                                                       or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '109'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '128'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '131'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '129'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '132'
													  then cast('20' + SUBSTRING(a.[OBRNo], 10, 2) as varchar ) 
													  end like '" + year + "' and a.ActionCode = 1 " +
                                                       "and a.FMISAccountCode = 478 and a.OOEID = 2  " +
                                                       "and case when cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '201'  " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '101' " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '119' " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '118'   " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '127'  " +
                                                       "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '109' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '128' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '131' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '129' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '132' " +
                                                       "then  cast(SUBSTRING([OBRNo], 13, 2) as int )  " +
                                                       "  end >=  '" + month_ + "' " +
                                                       "and  case when cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '201'  " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '101'   " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '119'  " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '118'   " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '127'  " +
                                                       "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '109' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '128' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '131' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '129' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '132' " +
                                                       "then  cast(SUBSTRING([OBRNo], 13, 2) as int )  " +
                                                       " end <= '" + month_To + "' and len(a.[OBRNo]) = 19 and left(a.[OBRNo],3) <> '20-'" +
                                                  "GROUP BY a.FMISAccountCode),0)AS MONEY),'N2')", con);
                con.Open();
                sb_MOOE.Value = com.ExecuteScalar().ToString();

            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select format(CAST(isnull((SELECT  format(isnull(SUM(A.Amount),0),'N2') FROM IFMIS.dbo.tbl_T_BMSSubsidiaryLedger as a
                                                  WHERE  case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '201' 
													  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '101'  
													  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '119'
													   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '118' 
													   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '127'
                                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '109'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '128'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '131'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '129'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '132'
													  then cast('20' + SUBSTRING(a.[OBRNo], 10, 2) as varchar ) " +
                                                      "end like '" + year + "' and a.ActionCode = 1 and a.FMISAccountCode = 478 and a.OOEID = 3 " +
                                                       "and case when cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '201'  " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '101' " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '119' " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '118'   " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '127'  " +
                                                       "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '109' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '128' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '131' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '129' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '132' " +
                                                       "then  cast(SUBSTRING([OBRNo], 13, 2) as int )  " +
                                                       " end >=  '" + month_ + "' " +
                                                       "and  case when cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '201'  " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '101'   " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '119'  " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '118'   " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '127'  " +
                                                       "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '109' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '128' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '131' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '129' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '132' " +
                                                       "then  cast(SUBSTRING([OBRNo], 13, 2) as int )  " +
                                                       "  end <= '" + month_To + "'  and len(a.[OBRNo]) = 19 and left(a.[OBRNo],3) <> '20-' " +
                                                       "GROUP BY a.FMISAccountCode),0)AS MONEY),'N2')", con);
                con.Open();
                sb_CO.Value = com.ExecuteScalar().ToString();

            }
            //--non office


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
                SqlCommand com = new SqlCommand(@"select format(CAST(isnull((SELECT distinct format(isnull(SUM(Amount),0),'N2') FROM [IFMIS].[dbo].[tbl_T_BMSSubsidiaryLedger] as a
                                                    left join IFMIS.dbo.tbl_R_BMSOfficePrograms as b
                                                    on a.ProgramID = b.ProgramID
                                                    WHERE  case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '201' 
													  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '101'  
													  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '119'
													   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '118' 
													   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '127' 
                                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '109'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '128'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '131'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '129'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '132'
													  then cast('20' + SUBSTRING(a.[OBRNo], 10, 2) as varchar ) 
													  end like '" + year + "' and a.ActionCode = 1 and b.ProgramYear = '" + year + "' " +
                                                    "and  b.ActionCode = 1 and a.OOEID = 1 and a.FMISAccountCode not in (478,2861) and b.OfficeID = 43 " +
                                                       "and case when cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '201'  " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '101' " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '119' " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '118'   " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '127'  " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '109' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '128' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '131' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '129' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '132' " +
                                                       "then  cast(SUBSTRING([OBRNo], 13, 2) as int )  " +
                                                       " end >=  '" + month_ + "' " +
                                                       "and  case when cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '201'  " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '101'   " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '119'  " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '118'   " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '127'  " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '109' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '128' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '131' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '129' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '132' " +
                                                       "then  cast(SUBSTRING([OBRNo], 13, 2) as int )  " +
                                                       " end <= '" + month_To + "'  and len(a.[OBRNo]) = 19 and left(a.[OBRNo],3) <> '20-'" +
                                                    "group by b.OfficeID),0)AS MONEY),'N2')", con);
                con.Open();
                no_PS.Value = com.ExecuteScalar().ToString();

            }


            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec sp_BMS_ObligationNOTotal '" + year + "',1,'" + month_To + "'", con);
                con.Open();
                no_MOOE.Value = com.ExecuteScalar().ToString();

            }


            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select format(CAST(isnull((SELECT distinct format(isnull(SUM(Amount),0),'N2') FROM [IFMIS].[dbo].[tbl_T_BMSSubsidiaryLedger] as a
                                                    left join IFMIS.dbo.tbl_R_BMSOfficePrograms as b
                                                    on a.ProgramID = b.ProgramID
                                                    WHERE   case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '201' 
													  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '101'  
													  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '119'
													   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '118' 
													   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '127' 
                                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '109'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '128'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '131'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '129'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '132'
													  then cast('20' + SUBSTRING(a.[OBRNo], 10, 2) as varchar ) 
													  end like '" + year + "' and a.ActionCode = 1 and b.ProgramYear = '" + year + "' " +
                                                    "and  b.ActionCode = 1 and a.OOEID = 3 and a.FMISAccountCode not in (478,2861) and b.OfficeID = 43 " +
                                                    "and case when cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '201'  " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '101' " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '119' " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '118'   " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '127'  " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '109' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '128' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '131' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '129' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '132' " +
                                                       "then  cast(SUBSTRING([OBRNo], 13, 2) as int )  " +
                                                       " end >=  '" + month_ + "' " +
                                                       "and  case when cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '201'  " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '101'   " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '119'  " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '118'   " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '127'  " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '109' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '128' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '131' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '129' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '132' " +
                                                       "then  cast(SUBSTRING([OBRNo], 13, 2) as int )  " +
                                                       " end <= '" + month_To + "' and len(a.[OBRNo]) = 19 and left(a.[OBRNo],3) <> '20-'" +
                                                    "group by b.OfficeID),0)AS MONEY),'N2')", con);
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
                SqlCommand com = new SqlCommand(@"select format(CAST(isnull((SELECT format(isnull(SUM(A.Amount),0),'N2') 
                                                      FROM IFMIS.dbo.tbl_T_BMSSubsidiaryLedger as a
                                                        WHERE  case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '201' 
													  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '101'  
													  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '119'
													   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '118' 
													   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '127' 
                                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '109'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '128'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '131'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '129'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '132'
													  then cast('20' + SUBSTRING(a.[OBRNo], 10, 2) as varchar ) 
													  end like '" + year + "' and a.ActionCode = 1 " +
                                                        "and a.FMISAccountCode = 2861 and a.OOEID = 1 " +
                                                       "and case when cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '201'  " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '101' " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '119' " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '118'   " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '127'  " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '109' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '128' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '131' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '129' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '132' " +
                                                       "then  cast(SUBSTRING([OBRNo], 13, 2) as int )  " +
                                                       "  end >=  '" + month_ + "' " +
                                                       "and  case when cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '201'  " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '101'   " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '119'  " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '118'   " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '127'  " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '109' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '128' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '131' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '129' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '132' " +
                                                       "then  cast(SUBSTRING([OBRNo], 13, 2) as int )  " +
                                                       " end <= '" + month_To + "' and len(a.[OBRNo]) = 19 and left(a.[OBRNo],3) <> '20-'" +
     "),0)AS MONEY),'N2')", con);
                con.Open();
                dev_PS.Value = com.ExecuteScalar().ToString();

            }


            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select format(CAST(isnull((SELECT format(isnull(SUM(A.Amount),0),'N2') 
                                                      FROM IFMIS.dbo.tbl_T_BMSSubsidiaryLedger as a
                                                        WHERE  case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '201' 
													  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '101'  
													  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '119'
													   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '118' 
													   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '127'
                                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '109'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '128'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '131'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '129'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '132' 
													  then cast('20' + SUBSTRING(a.[OBRNo], 10, 2) as varchar ) 
                                                        end like '" + year + "' and a.ActionCode = 1 " +
                                                        "and a.FMISAccountCode = 2861 and a.OOEID = 2 " +
                                                         "and case when cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '201'  " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '101' " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '119' " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '118'   " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '127'  " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '109' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '128' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '131' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '129' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '132' " +
                                                       "then  cast(SUBSTRING([OBRNo], 13, 2) as int )  " +
                                                       " end >=  '" + month_ + "' " +
                                                       "and  case when cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '201'  " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '101'   " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '119'  " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '118'   " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '127'  " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '109' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '128' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '131' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '129' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '132' " +
                                                       "then  cast(SUBSTRING([OBRNo], 13, 2) as int )  " +
                                                       " end <= '" + month_To + "' and len(a.[OBRNo]) = 19 and left(a.[OBRNo],3) <> '20-'" +
     "),0)AS MONEY),'N2')", con);
                con.Open();
                dev_MOOE.Value = com.ExecuteScalar().ToString();

            }


            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select format(CAST(isnull((SELECT format(isnull(SUM(A.Amount),0),'N2') 
                                                      FROM IFMIS.dbo.tbl_T_BMSSubsidiaryLedger as a
                                                        WHERE  case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '201' 
													  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '101'  
													  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '119'
													   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '118' 
													   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '127' 
                                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '109'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '128'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '131'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '129'
				                                        or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) = '132' 
													  then cast('20' + SUBSTRING(a.[OBRNo], 10, 2) as varchar ) 
                                                        end like '" + year + "' and a.ActionCode = 1 " +
                                                        "and a.FMISAccountCode = 2861 and a.OOEID = 3 " +
                                                       "and case when cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '201'  " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '101' " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '119' " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '118'   " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '127'  " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '109' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '128' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '131' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '129' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '132' " +
                                                       "then  cast(SUBSTRING([OBRNo], 13, 2) as int )  " +
                                                       " end >=  '" + month_ + "' " +
                                                       "and  case when cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '201'  " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '101'   " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '119'  " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '118'   " +
                                                       "or cast( SUBSTRING([OBRNo], 1, 3) as varchar ) = '127'  " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '109' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '128' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '131' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '129' " +
                                                        "  or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) = '132' " +
                                                       "then  cast(SUBSTRING([OBRNo], 13, 2) as int )  " +
                                                       " end <= '" + month_To + "'  and len(a.[OBRNo]) = 19 and left(a.[OBRNo],3) <> '20-'" +
     "),0)AS MONEY),'N2')", con);
                con.Open();
                dev_CO.Value = com.ExecuteScalar().ToString();

            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_MonthlyRelease_Report_TO_grandTotal '" + year + "','" + month_ + "','" + month_To + "'", con);
                con.Open();
                grand_total.Value = "GRAND TOTAL ====>>>>  " + com.ExecuteScalar().ToString();

            }

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT CONCAT((SELECT DATENAME(month, DATEADD(month, '" + month_ + "'-1, CAST('2008-01-01' AS datetime)))), "+
                                               " ' - ', (SELECT DATENAME(month, DATEADD(month, '" + month_To + "'-1, CAST('2008-01-01' AS datetime)))))", con);
                con.Open();
                date_desuni.Value = com.ExecuteScalar().ToString();

            }


            txt_todaydate.Value = "Date Printed : " + DateTime.Now.ToString();
            txt_user.Value = "Printed by : " + Account.UserInfo.empName;
        }
    }
}