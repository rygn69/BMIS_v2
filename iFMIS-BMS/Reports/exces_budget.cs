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
    /// Summary description for exces_budget.
    /// </summary>
    public partial class exces_budget : Telerik.Reporting.Report
    {
        public exces_budget(int? Fundtype, string changed_To, int? month_, int? month_To, int? year, int? air,int? pgo_,int? repxmlhistory,int? tyear)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            DataTable dt = new DataTable();

            var dte = "";
            dte= DateTime.Now.ToString();

            TXT_for_the.Value = "As Of C.Y. " + tyear;

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@" select  CONCAT(FundName, ' ' , '(' +FundMedium +')') as FundName FROM [IFMIS].[dbo].[tbl_R_BMSFunds] where FundFlag = '" + Fundtype + "'", con);
                con.Open();
                txt_FundType.Value = com.ExecuteScalar().ToString();

            }
            if (repxmlhistory != 0)
            {


                DataTable _dt2 = new DataTable();
                string _sqlQuery2 = "Select [dateprinted],[username],appropriation,obligation from [IFMIS].[dbo].[tbl_T_BMSReportXML] where [rprtid] = " + repxmlhistory + "";
                _dt2 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery2).Tables[0];
                txt_obligation.Value = _dt2.Rows[0][3].ToString();
                txt_Appropriation.Value = _dt2.Rows[0][2].ToString();
                txt_todaydate.Value = "Date Printed : " + _dt2.Rows[0][0].ToString();
                txt_user.Value = "Printed by : " + _dt2.Rows[0][1].ToString();
            }
            else
            {
                txt_todaydate.Value = "Date Printed : " + dte;
                txt_user.Value = "Printed By : " + Account.UserInfo.empName;
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    if (year == 0 || year == null)
                    {
                        if (air == 2 && pgo_ == 1)
                        {
                            SqlCommand com = new SqlCommand(@"exec sp_BMS_Get_ExcessControlTotal_Earmark " + changed_To + "," + Fundtype + "," + month_ + "," + month_To + "," + year + "", con);
                            con.Open();
                            txt_obligation.Value = com.ExecuteScalar().ToString();
                        }
                        else if (pgo_ == 2)
                        {
                            DataTable _dt2 = new DataTable();
                            string _sqlQuery2 = "exec sp_BMS_ExcessControlTotal 0,'" + changed_To + "'," + month_ + "," + month_To + "," + air + "," + pgo_ + "";
                            _dt2 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery2).Tables[0];

                            txt_obligation.Value = _dt2.Rows[0][0].ToString();

                        }
                        else
                        {
                            SqlCommand com = new SqlCommand(@"select format(isnull(cast((SELECT format(SUM(a.Amount),'N2')  FROM IFMIS.dbo.tbl_T_BMSExcessControl as a
                                                inner join IFMIS.dbo.tbl_T_BMSExcessAppropriation  as b
                                                on cast(a.ExcessAppropriationNo as bigint) = cast(b.TransactionNo as bigint) and a.ActionCode = b.ActionCode
                                                where cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) != '20-' and a.[ExcessAppropriationNo] = '" + changed_To + "' and a.actionCode = 1  " +
                                                                                      "   and  a.ActionCode = 1 AND LEN(a.[OBRNo]) = 19  " +
                                                       "  and  case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '201'    " +
                                                       "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '101'     " +
                                                       "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '119'   " +
                                                        "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '118'    " +
                                                        "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '127'    " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '109'" +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '128' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '129' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '131' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '132' " +
                                                       "   then    " +
                                                       "   cast(SUBSTRING(a.[OBRNo], 13, 2) as int )    " +
                                                       "   else case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '20-'    " +
                                                       "   then   " +
                                                       "   cast(SUBSTRING(a.[OBRNo], 6, 2) as int )    " +
                                                       "   else   " +
                                                        "    cast(SUBSTRING(a.[OBRNo], 3, 2) as int )    " +
                                                       " 	end end    " +
                                                       " 	>= '" + month_ + "'  and case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '201'    " +
                                                       "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '101'     " +
                                                        "  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '119'   " +
                                                        "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '118'    " +
                                                        "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '127'    " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '109'" +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '128' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '129' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '131' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '132' " +
                                                        "  then    " +
                                                        "  cast(SUBSTRING(a.[OBRNo], 13, 2) as int )    " +
                                                       "   else case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '20-'    " +
                                                       "   then   " +
                                                       "   cast(SUBSTRING(a.[OBRNo], 6, 2) as int )    " +
                                                       "   else   " +
                                                        "    cast(SUBSTRING(a.[OBRNo], 3, 2) as int )    " +
                                                           " end end  <= '" + month_To + "'" +
                                                       ") as money),0),'N2')", con);
                            con.Open();
                            txt_obligation.Value = com.ExecuteScalar().ToString();
                        }
                    }

                    else
                    {
                        if (air == 2)
                        {
                            SqlCommand com = new SqlCommand(@"exec sp_BMS_Get_ExcessControlTotal " + changed_To + "," + Fundtype + "," + month_ + "," + month_To + "," + year + "", con);
                            con.Open();
                            txt_obligation.Value = com.ExecuteScalar().ToString();
                        }
                        else if (pgo_ == 2)
                        {
                            DataTable _dt2 = new DataTable();
                            string _sqlQuery2 = "exec sp_BMS_ExcessControlTotal " + year + ",'" + changed_To + "'," + month_ + "," + month_To + "," + air + "," + pgo_ + "";
                            _dt2 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery2).Tables[0];

                            txt_obligation.Value = _dt2.Rows[0][0].ToString();

                        }
                        else
                        {
                            SqlCommand com = new SqlCommand(@"select format(isnull(cast((SELECT format(SUM(a.Amount),'N2')  FROM IFMIS.dbo.tbl_T_BMSExcessControl as a
                                                inner join IFMIS.dbo.tbl_T_BMSExcessAppropriation  as b
                                                on cast(a.ExcessAppropriationNo as bigint) = cast(b.TransactionNo as bigint) and a.ActionCode = b.ActionCode
                                                where cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) != '20-' and a.[ExcessAppropriationNo] = '" + changed_To + "' and a.actionCode = 1  " +
                                                                               "   and  a.ActionCode = 1 AND LEN(a.[OBRNo]) = 19  " +
                                                "  and  case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '201'    " +
                                                "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '101'     " +
                                                "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '119'   " +
                                                 "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '118'    " +
                                                 "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '127'    " +
                                                    "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '109'" +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '128' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '129' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '131' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '132' " +
                                                "   then    " +
                                                "   cast(SUBSTRING(a.[OBRNo], 13, 2) as int )    " +
                                                "   else case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '20-'    " +
                                                "   then   " +
                                                "   cast(SUBSTRING(a.[OBRNo], 6, 2) as int )    " +
                                                "   else   " +
                                                 "    cast(SUBSTRING(a.[OBRNo], 3, 2) as int )    " +
                                                " 	end end    " +
                                                " 	>= '" + month_ + "'  and case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '201'    " +
                                                "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '101'     " +
                                                 "  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '119'   " +
                                                 "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '118'    " +
                                                 "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '127'    " +
                                                    "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '109'" +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '128' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '129' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '131' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '132' " +
                                                 "  then    " +
                                                 "  cast(SUBSTRING(a.[OBRNo], 13, 2) as int )    " +
                                                "   else case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '20-'    " +
                                                "   then   " +
                                                "   cast(SUBSTRING(a.[OBRNo], 6, 2) as int )    " +
                                                "   else   " +
                                                 "    cast(SUBSTRING(a.[OBRNo], 3, 2) as int )    " +
                                                    " end end  <= '" + month_To + "' and " +
                                                 "    case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '201' " +
                                                "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '101'  " +
                                                 "  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '119' " +
                                                 "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '118'  " +
                                                  "  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '127' " +
                                                     "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '109'" +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '128' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '129' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '131' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '132' " +
                                                 "  then " +
                                                 "  cast('20' + SUBSTRING(a.[OBRNo], 10, 2) as int ) " +
                                                 "  else case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '20-' " +
                                                 "  then " +
                                                 "  cast('20' + SUBSTRING(a.[OBRNo], 4, 2) as int ) " +
                                                 "  else " +
                                                  "   cast('20' + SUBSTRING(a.[OBRNo], 1, 2) as int ) " +
                                                    " end end = '" + year + "' " +
                                                ") as money),0),'N2')", con);
                            con.Open();
                            txt_obligation.Value = com.ExecuteScalar().ToString();
                        }

                    }
                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    if (year == 0 || year == null)
                    {
                        if (air == 2 && pgo_ == 1)
                        {
                            SqlCommand com = new SqlCommand(@"exec [sp_BMS_Get_ExcessAppropriationBal_Earmark] " + changed_To + "," + Fundtype + "," + month_ + "," + month_To + "," + year + "", con);
                            con.Open();
                            txt_Appropriation.Value = com.ExecuteScalar().ToString();
                        }
                        else if (pgo_ == 2)
                        {
                            DataTable _dt2 = new DataTable();
                            string _sqlQuery2 = "exec sp_BMS_ExcessControlTotal 0,'" + changed_To + "'," + month_ + "," + month_To + "," + air + "," + pgo_ + "";
                            _dt2 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery2).Tables[0];

                            txt_Appropriation.Value = _dt2.Rows[0][1].ToString();

                        }
                        else
                        {
                            SqlCommand com = new SqlCommand(@"select format(isnull(cast((SELECT format((b.Amount - SUM(a.Amount)),'N2') FROM IFMIS.dbo.tbl_T_BMSExcessControl as a
                                                inner join IFMIS.dbo.tbl_T_BMSExcessAppropriation  as b
                                                on cast(a.ExcessAppropriationNo as bigint) = cast(b.TransactionNo as bigint) and a.ActionCode = b.ActionCode
                                                where cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) != '20-' and a.[ExcessAppropriationNo] = '" + changed_To + "' and a.actionCode = 1  " +
                                                                                             "   and  a.ActionCode = 1 AND LEN(a.[OBRNo]) = 19  " +
                                                       "  and  case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '201'    " +
                                                       "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '101'     " +
                                                       "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '119'   " +
                                                        "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '118'    " +
                                                        "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '127'    " +
                                                           "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '109'" +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '128' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '129' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '131' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '132' " +
                                                       "   then    " +
                                                       "   cast(SUBSTRING(a.[OBRNo], 13, 2) as int )    " +
                                                       "   else case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '20-'    " +
                                                       "   then   " +
                                                       "   cast(SUBSTRING(a.[OBRNo], 6, 2) as int )    " +
                                                       "   else   " +
                                                        "    cast(SUBSTRING(a.[OBRNo], 3, 2) as int )    " +
                                                       " 	end end    " +
                                                       " 	>= '" + month_ + "'  and case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '201'    " +
                                                       "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '101'     " +
                                                        "  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '119'   " +
                                                        "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '118'    " +
                                                        "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '127'    " +
                                                           "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '109'" +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '128' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '129' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '131' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '132' " +
                                                        "  then    " +
                                                        "  cast(SUBSTRING(a.[OBRNo], 13, 2) as int )    " +
                                                       "   else case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '20-'    " +
                                                       "   then   " +
                                                       "   cast(SUBSTRING(a.[OBRNo], 6, 2) as int )    " +
                                                       "   else   " +
                                                        "    cast(SUBSTRING(a.[OBRNo], 3, 2) as int )    " +
                                                           " end end  <= '" + month_To + "' " +
                                                           " GROUP BY b.TransactionNo,b.amount) as money),0),'N2')", con);
                            con.Open();
                            txt_Appropriation.Value = com.ExecuteScalar().ToString();
                        }

                    }
                    else
                    {
                        if (air == 2 && pgo_ == 1)
                        {
                            SqlCommand com = new SqlCommand(@"exec sp_BMS_Get_ExcessAppropriationBal " + changed_To + "," + Fundtype + "," + month_ + "," + month_To + "," + year + "", con);
                            con.Open();
                            txt_Appropriation.Value = com.ExecuteScalar().ToString();
                        }
                        else if (pgo_ == 2)
                        {
                            DataTable _dt2 = new DataTable();
                            string _sqlQuery2 = "exec sp_BMS_ExcessControlTotal " + year + ",'" + changed_To + "'," + month_ + "," + month_To + "," + air + "," + pgo_ + "";
                            _dt2 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery2).Tables[0];

                            txt_Appropriation.Value = _dt2.Rows[0][1].ToString();

                        }
                        else
                        {
                            SqlCommand com = new SqlCommand(@"select format(isnull(cast((SELECT format((b.Amount - SUM(a.Amount)),'N2') FROM IFMIS.dbo.tbl_T_BMSExcessControl as a
                                                inner join IFMIS.dbo.tbl_T_BMSExcessAppropriation  as b
                                                on cast(a.ExcessAppropriationNo as bigint) = cast(b.TransactionNo as bigint) and a.ActionCode = b.ActionCode
                                                where cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) != '20-' and a.[ExcessAppropriationNo] = '" + changed_To + "' and a.actionCode = 1  " +
                                                                                              "   and  a.ActionCode = 1 AND LEN(a.[OBRNo]) = 19  " +
                                                        "  and  case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '201'    " +
                                                        "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '101'     " +
                                                        "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '119'   " +
                                                         "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '118'    " +
                                                         "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '127'    " +
                                                            "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '109'" +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '128' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '129' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '131' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '132' " +
                                                        "   then    " +
                                                        "   cast(SUBSTRING(a.[OBRNo], 13, 2) as int )    " +
                                                        "   else case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '20-'    " +
                                                        "   then   " +
                                                        "   cast(SUBSTRING(a.[OBRNo], 6, 2) as int )    " +
                                                        "   else   " +
                                                     "    cast(SUBSTRING(a.[OBRNo], 3, 2) as int )    " +
                                                    " 	end end    " +
                                                    " 	>= '" + month_ + "'  and case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '201'    " +
                                                    "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '101'     " +
                                                     "  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '119'   " +
                                                     "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '118'    " +
                                                     "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '127'    " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '109'" +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '128' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '129' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '131' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '132' " +
                                                     "  then    " +
                                                     "  cast(SUBSTRING(a.[OBRNo], 13, 2) as int )    " +
                                                    "   else case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '20-'    " +
                                                    "   then   " +
                                                    "   cast(SUBSTRING(a.[OBRNo], 6, 2) as int )    " +
                                                    "   else   " +
                                                     "    cast(SUBSTRING(a.[OBRNo], 3, 2) as int )    " +
                                                        " end end  <= '" + month_To + "' and " +
                                                     "    case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '201' " +
                                                    "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '101'  " +
                                                     "  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '119' " +
                                                     "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '118'  " +
                                                      "  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '127' " +
                                                         "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '109'" +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '128' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '129' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '131' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '132' " +
                                                     "  then " +
                                                     "  cast('20' + SUBSTRING(a.[OBRNo], 10, 2) as int ) " +
                                                     "  else case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '20-' " +
                                                     "  then " +
                                                     "  cast('20' + SUBSTRING(a.[OBRNo], 4, 2) as int ) " +
                                                     "  else " +
                                                      "   cast('20' + SUBSTRING(a.[OBRNo], 1, 2) as int ) " +
                                                        " end end = '" + year + "' " +

                                                                                         " GROUP BY b.TransactionNo,b.amount) as money),0),'N2')", con);
                            con.Open();
                            txt_Appropriation.Value = com.ExecuteScalar().ToString();
                        }

                    }
                }
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (year == 0 || year == null)
                {


                    if (air == 2 && pgo_ == 1)
                    {


                        SqlCommand com = new SqlCommand(@"SELECT a.OBRNo ,a.[Description] ,a.Amount ,b.Account,format(b.Amount,'N2') as mount,a.DateTimeEntered FROM IFMIS.dbo.tbl_T_BMSExcessControl as a
                                                inner join IFMIS.dbo.tbl_T_BMSExcessAppropriation  as b
                                                on cast(a.ExcessAppropriationNo as bigint) = cast(b.TransactionNo as bigint) and a.ActionCode = b.ActionCode
                                                where a.[ExcessAppropriationNo] = '" + changed_To + "' and a.actionCode = 1 " +
                                                      "   and  a.ActionCode = 1 AND LEN(a.[OBRNo]) = 19  " +
                                                      "  and  case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '201'    " +
                                                      "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '101'     " +
                                                      "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '119'   " +
                                                       "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '118'    " +
                                                       "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '127'    " +
                                                          "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '109'" +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '128' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '129' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '131' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '132' " +
                                                      "   then    " +
                                                      "   cast(SUBSTRING(a.[OBRNo], 13, 2) as int )    " +
                                                      "   else case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '20-'    " +
                                                      "   then   " +
                                                      "   cast(SUBSTRING(a.[OBRNo], 6, 2) as int )    " +
                                                      "   else   " +
                                                       "    cast(SUBSTRING(a.[OBRNo], 3, 2) as int )    " +
                                                      " 	end end    " +
                                                      " 	>= '" + month_ + "'  and case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '201'    " +
                                                      "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '101'     " +
                                                       "  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '119'   " +
                                                       "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '118'    " +
                                                       "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '127'    " +
                                                          "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '109'" +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '128' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '129' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '131' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '132' " +
                                                       "  then    " +
                                                       "  cast(SUBSTRING(a.[OBRNo], 13, 2) as int )    " +
                                                      "   else case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '20-'    " +
                                                      "   then   " +
                                                      "   cast(SUBSTRING(a.[OBRNo], 6, 2) as int )    " +
                                                      "   else   " +
                                                       "    cast(SUBSTRING(a.[OBRNo], 3, 2) as int )    " +
                                                          " end end  <= '" + month_To + "' and cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) != '20-' " +
                                                          "order by cast(substring(a.[OBRNo],10,2) as int),cast(substring(a.[OBRNo],13,2) as int),cast(a.DateTimeEntered as date) asc", con);

                        con.Open();
                        dt.Load(com.ExecuteReader());
                        con.Close();
                        SqlCommand com2 = new SqlCommand(@"SELECT c.RefNo as OBRNo ,'' as [Description] ,c.Amount ,b.Account,format(b.Amount,'N2') as mount,isnull(format(cast(c.DateEntered + ' ' + c.TimeEntered as datetime),'MM/dd/yyyy hh:mm:ss tt'),'') DateTimeEntered FROM --IFMIS.dbo.tbl_T_BMSExcessControl as a
                                                             IFMIS.dbo.tbl_T_BMSExcessAppropriation  as b
		                                                    inner join fmis.dbo.tblBMS_AirMark as c on b.[TransactionNo] = c.ExcessAppropriationID 
		                                                    and b.ActionCode = c.ActionCode
                                                            where c.ExcessAppropriationID = '" + changed_To + "' and c.[ActionCode] = 1 " +
                                                               //" and '20'+substring(c.[RefNo],14,2)='" + year + "' " +
                                                               " order by cast(c.[DateEntered] as date),c.[TimeEntered]", con);

                        con.Open();
                        dt.Load(com2.ExecuteReader());

                    }
                    else if (pgo_ == 2)
                    {
                        SqlCommand com = new SqlCommand(@"exec sp_BMS_ExcessControl 0,'" + changed_To + "'," + month_ + "," + month_To + "," + air + "," + pgo_ + "", con);
                        con.Open();
                        dt.Load(com.ExecuteReader());
                        con.Close();
                    }
                    else
                    {

                        SqlCommand com = new SqlCommand(@"exec sp_BMS_BOIExcess '" + changed_To + "'," + Fundtype + "," + month_ + "," + month_To + "," + repxmlhistory + "," + Account.UserInfo.eid + ",'" + dte + "','" + txt_Appropriation.Value  + "','" + txt_obligation.Value + "'", con);

                        con.Open();
                        dt.Load(com.ExecuteReader());
                    }
                }
                else
                {
                    if (air == 2 && pgo_ == 1)
                    {
                        SqlCommand com = new SqlCommand(@"SELECT a.OBRNo ,a.[Description] ,a.Amount ,b.Account,format(b.Amount,'N2') as mount,a.DateTimeEntered FROM IFMIS.dbo.tbl_T_BMSExcessControl as a
                                                inner join IFMIS.dbo.tbl_T_BMSExcessAppropriation  as b
                                                on cast(a.ExcessAppropriationNo as bigint) =cast(b.TransactionNo as bigint) and a.ActionCode = b.ActionCode
                                                where cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) != '20-' and a.[ExcessAppropriationNo] = '" + changed_To + "' and a.actionCode = 1 " +
                                                      "   and  a.ActionCode = 1 AND LEN(a.[OBRNo]) = 19  " +
                                                      "  and  case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '201'    " +
                                                      "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '101'     " +
                                                      "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '119'   " +
                                                       "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '118'    " +
                                                       "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '127'    " +
                                                          "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '109'" +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '128' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '129' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '131' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '132' " +
                                                      "   then    " +
                                                      "   cast(SUBSTRING(a.[OBRNo], 13, 2) as int )    " +
                                                      "   else case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '20-'    " +
                                                      "   then   " +
                                                      "   cast(SUBSTRING(a.[OBRNo], 6, 2) as int )    " +
                                                      "   else   " +
                                                       "    cast(SUBSTRING(a.[OBRNo], 3, 2) as int )    " +
                                                      " 	end end    " +
                                                      " 	>= '" + month_ + "'  and case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '201'    " +
                                                      "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '101'     " +
                                                       "  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '119'   " +
                                                       "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '118'    " +
                                                       "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '127'    " +
                                                          "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '109'" +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '128' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '129' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '131' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '132' " +
                                                       "  then    " +
                                                       "  cast(SUBSTRING(a.[OBRNo], 13, 2) as int )    " +
                                                      "   else case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '20-'    " +
                                                      "   then   " +
                                                      "   cast(SUBSTRING(a.[OBRNo], 6, 2) as int )    " +
                                                      "   else   " +
                                                       "    cast(SUBSTRING(a.[OBRNo], 3, 2) as int )    " +
                                                          " end end  <= '" + month_To + "' and " +
                                                       "    case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '201' " +
                                                      "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '101'  " +
                                                       "  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '119' " +
                                                       "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '118'  " +
                                                        "  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '127' " +
                                                           "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '109'" +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '128' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '129' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '131' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '132' " +
                                                       "  then " +
                                                       "  cast('20' + SUBSTRING(a.[OBRNo], 10, 2) as int ) " +
                                                       "  else case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '20-' " +
                                                       "  then " +
                                                       "  cast('20' + SUBSTRING(a.[OBRNo], 4, 2) as int ) " +
                                                       "  else " +
                                                        "   cast('20' + SUBSTRING(a.[OBRNo], 1, 2) as int ) " +
                                                          " end end = '" + year + "' " +
                                                          "order by cast(substring(a.[OBRNo],10,2) as int),cast(substring(a.[OBRNo],13,2) as int),cast(a.DateTimeEntered as date) asc", con);

                        con.Open();
                        dt.Load(com.ExecuteReader());
                        con.Close();
                        SqlCommand com2 = new SqlCommand(@"SELECT c.RefNo as OBRNo ,'' as [Description] ,c.Amount ,b.Account,format(b.Amount,'N2') as mount,a.DateTimeEntered FROM --IFMIS.dbo.tbl_T_BMSExcessControl as a
                                                             IFMIS.dbo.tbl_T_BMSExcessAppropriation  as b
		                                                    inner join fmis.dbo.tblBMS_AirMark as c on b.[TransactionNo] = c.ExcessAppropriationID 
		                                                    and b.ActionCode = c.ActionCode
                                                            where c.ExcessAppropriationID = '" + changed_To + "' and c.[ActionCode] = 1 " +
                                                            " and '20'+substring(c.[RefNo],14,2)='" + year + "' " +
                                                               " order by cast(c.[DateEntered] as date),c.[TimeEntered]", con);

                        con.Open();
                        dt.Load(com2.ExecuteReader());


                    }
                    else if (pgo_ == 2)
                    {
                        SqlCommand com = new SqlCommand(@"exec sp_BMS_ExcessControl " + year + ",'" + changed_To + "'," + month_ + "," + month_To + "," + air + "," + pgo_ + "", con);
                        con.Open();
                        dt.Load(com.ExecuteReader());
                        con.Close();
                    }
                    else
                    {
                        SqlCommand com = new SqlCommand(@"SELECT a.OBRNo ,a.[Description] ,a.Amount ,b.Account,format(b.Amount,'N2') as mount,a.DateTimeEntered FROM IFMIS.dbo.tbl_T_BMSExcessControl as a
                                                inner join IFMIS.dbo.tbl_T_BMSExcessAppropriation  as b
                                                on cast(a.ExcessAppropriationNo as bigint) = cast(b.TransactionNo as bigint) and a.ActionCode = b.ActionCode
                                                where cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) != '20-' and a.[ExcessAppropriationNo] = '" + changed_To + "' and a.actionCode = 1 " +
                                                      "   and  a.ActionCode = 1 AND LEN(a.[OBRNo]) = 19  " +
                                                      "  and  case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '201'    " +
                                                      "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '101'     " +
                                                      "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '119'   " +
                                                       "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '118'    " +
                                                       "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '127'    " +
                                                          "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '109'" +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '128' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '129' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '131' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '132' " +
                                                      "   then    " +
                                                      "   cast(SUBSTRING(a.[OBRNo], 13, 2) as int )    " +
                                                      "   else case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '20-'    " +
                                                      "   then   " +
                                                      "   cast(SUBSTRING(a.[OBRNo], 6, 2) as int )    " +
                                                      "   else   " +
                                                       "    cast(SUBSTRING(a.[OBRNo], 3, 2) as int )    " +
                                                      " 	end end    " +
                                                      " 	>= '" + month_ + "'  and case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '201'    " +
                                                      "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '101'     " +
                                                       "  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '119'   " +
                                                       "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '118'    " +
                                                       "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '127'    " +
                                                          "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '109'" +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '128' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '129' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '131' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '132' " +
                                                       "  then    " +
                                                       "  cast(SUBSTRING(a.[OBRNo], 13, 2) as int )    " +
                                                      "   else case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '20-'    " +
                                                      "   then   " +
                                                      "   cast(SUBSTRING(a.[OBRNo], 6, 2) as int )    " +
                                                      "   else   " +
                                                       "    cast(SUBSTRING(a.[OBRNo], 3, 2) as int )    " +
                                                          " end end  <= '" + month_To + "' and " +
                                                       "    case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '201' " +
                                                      "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '101'  " +
                                                       "  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '119' " +
                                                       "   or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '118'  " +
                                                        "  or cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '127' " +
                                                           "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '109'" +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '128' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '129' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '131' " +
                                                        "   or cast(SUBSTRING(a.[OBRNo], 1, 3) as varchar) like '132' " +
                                                       "  then " +
                                                       "  cast('20' + SUBSTRING(a.[OBRNo], 10, 2) as int ) " +
                                                       "  else case when cast( SUBSTRING(a.[OBRNo], 1, 3) as varchar ) like '20-' " +
                                                       "  then " +
                                                       "  cast('20' + SUBSTRING(a.[OBRNo], 4, 2) as int ) " +
                                                       "  else " +
                                                        "   cast('20' + SUBSTRING(a.[OBRNo], 1, 2) as int ) " +
                                                          " end end = '" + year + "' " +
                                                          "order by cast(substring(a.[OBRNo],10,2) as int),cast(substring(a.[OBRNo],13,2) as int),cast(a.DateTimeEntered as date) asc", con);

                        con.Open();
                        dt.Load(com.ExecuteReader());
                    }
                }
            }
            
            this.dt.DataSource = dt;
            DataTable _dt4 = new DataTable();
            string _sqlQuery4 = "SELECT [lgu],[province],isnull([address],'') address,[budgetofficer],[position],tagline FROM [IFMIS].[dbo].[tbl_R_BMSLgu] where actioncode=1";
            _dt4 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery4).Tables[0];
            if (_dt4.Rows.Count > 0)
            {
                textBox15.Value = "City Government of Tacurong";
                textBox16.Value = "City Budget Office";
              
            }

        }
    }
}       