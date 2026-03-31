namespace iFMIS_BMS.Reports.Design
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using iFMIS_BMS.BusinessLayer.Connector;
    using iFMIS_BMS.BusinessLayer.Layers;
    using iFMIS_BMS.BusinessLayer.Models;
    using System.Data.SqlClient;
    using iFMIS_BMS.BusinessLayer.Models.Budget_Preparation;
    using Kendo.Mvc.UI;
    using iFMIS_BMS.PPNP;

    public partial class rpt_SOO : Telerik.Reporting.Report
    {
        public static string OfficeName, ProgramName, ReportID, grants, others;
        public static int OfficeID, Year;
        public static bool isPrintAll;
        public static string OfficeHead, Designation;
        public static decimal totalPS = 0;
        public static decimal totalMOOE = 0;
        public static decimal totalCO = 0;
        public static double totalTOTAL = 0;
        public static double dtotalTOTAL = 0;
        public static double totalSUPPLIES = 0;
        public static double TOTAL = 0;
        public static double TOTALS = 0;
        public static double totalt = 0;
        public static double totaltw = 0;
        public static double result1 = 0;
        public static double result2 = 0;
        public static double result3 = 0;
        public static double totalresult = 0;
        public static double wholetotal = 0;
        public static double wholetotals = 0;

        public static double trundstotal = 0;
        public static double trundstotals = 0;

        public static double grandstotal = 0;
        public static double grandstotals = 0;

        public static double othertotal = 0;
        public static double othertotals = 0;

        public static double OtherSource = 0;
        public static double OtherSources = 0;
        public static string MyFormat(double num)
        {
            string format = "{0:N2}";
            return string.Format(format, Math.Abs(num));
        }




        serviceSoapClient PPMP = new serviceSoapClient();
        public rpt_SOO(int OfficeIDParam, string OfficeNameParam, int YearParam)
        {
            OfficeID = OfficeIDParam;
            Year = YearParam;

            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            dprt_office.Value = "Department : <b><u>" + OfficeNameParam + "</u></b>";
            budget_yr.Value = "Budget Year: <b>CY" + YearParam + "</b>";



            DataTable dt = new DataTable();
            dt.Columns.Add("particular");
            dt.Columns.Add("ps");
            dt.Columns.Add("mooe");
            dt.Columns.Add("co");
            dt.Columns.Add("total");
        
                          
                    DataRow dr = dt.NewRow();
                    dr[0] = "A. CY" + (Year - 2) + " (Actual)";
                    dr[1] = "";
                    dr[2] = "";
                    dr[3] = "";
                    dr[4] = "";
                    dt.Rows.Add(dr);
                
                DataRow dr2 = dt.NewRow();
                dr2[0] = "   1. Provincial Allocation";
                dr2[1] = "";
                dr2[2] = "";
                dr2[3] = "";
                dr2[4] = "";
                dt.Rows.Add(dr2);
            
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"sp_rpt_SOO " + OfficeID + "," + (Year - 2) + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result1 = Convert.ToDouble(reader.GetValue(1));
                    result2 = Convert.ToDouble(reader.GetValue(2));
                    result3 = Convert.ToDouble(reader.GetValue(3));
                    totalTOTAL = result1 + result2 + result3;

                    DataRow dr4 = dt.NewRow();
                    dr4[0] = "       1.1. Office Allocation";
                    dr4[1] = "₱ " + Convert.ToDouble(reader.GetValue(1)).ToString("N2");
                    dr4[2] = "₱ " + Convert.ToDouble(reader.GetValue(2)).ToString("N2");
                    dr4[3] = "₱ " + Convert.ToDouble(reader.GetValue(3)).ToString("N2");
                    dr4[4] = "₱ " + totalTOTAL.ToString("N2");
                    dt.Rows.Add(dr4);


                }

                DataRow dr3 = dt.NewRow();
                dr3[0] = "        1.2. Non-Office (Pls. Specify)";
                dr3[1] = "";
                dr3[2] = "";
                dr3[3] = "";
                dr3[4] = "";
                dt.Rows.Add(dr3);

            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"SELECT b.AccountName, a.Amount FROM fmis.dbo.tblBMS_NonOfficeAppropriation as a
                                                       inner join dbo.[tbl_R_BMSAccounts] as b on b.FMISAccountCode = a.FmisAccountID 
	                                                   where FmisOfficeID = " + OfficeID + " and YearOf=" + (Year - 2) + "", con);

                con.Open();
                SqlDataReader reader = com.ExecuteReader();
               
                var x = 1;
                while (reader.Read())
                {
                    DataRow drsa = dt.NewRow();
                    drsa[0] = "           1.2." + x + ". " + reader.GetValue(0).ToString(); ;
                    drsa[1] = "";
                    drsa[2] = "₱ " + Convert.ToDouble(reader.GetValue(1)).ToString("N2");
                    drsa[3] = "";
                    drsa[4] = "₱ " + Convert.ToDouble(reader.GetValue(1)).ToString("N2");
                    dt.Rows.Add(drsa);
                    x = x + 1;
                    result1 = Convert.ToDouble(reader.GetValue(1));
                    totalTOTAL = totalTOTAL + result1;
                }


                con.Close();

                SqlCommand com2 = new SqlCommand(@"SELECT BudgetAcctName,SooAmount FROM IFMIS.dbo.tbl_R_BMS_SOONONoffice where ActionCode = 1 and OfficeID = '"+Account.UserInfo.Department+"' and YEARof = '" + (Year - 2) + "'", con);
                con.Open();
                SqlDataReader reader2 = com2.ExecuteReader();


                while (reader2.Read())
                {
                    DataRow drsa = dt.NewRow();
                    drsa[0] = "           1.2." + x + ". " + reader2.GetValue(0).ToString(); ;
                    drsa[1] = "";
                    drsa[2] = "₱ " + Convert.ToDouble(reader2.GetValue(1)).ToString("N2");
                    drsa[3] = "";
                    drsa[4] = "₱ " + Convert.ToDouble(reader2.GetValue(1)).ToString("N2");
                    dt.Rows.Add(drsa);
                    result2 = Convert.ToDouble(reader2.GetValue(1));
                    totalTOTAL = totalTOTAL + result2;
                }










            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"sp_rpt_SOO " + OfficeID + "," + (Year - 2) + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    DataRow dr4 = dt.NewRow();
                    dr4[0] = "                  TOTAL";
                    dr4[1] = "";
                    dr4[2] = "";
                    dr4[3] = "";
                    dr4[4] = "₱ " + totalTOTAL.ToString("N2");
                    dt.Rows.Add(dr4);
                }
            }
            DataRow dr5 = dt.NewRow();
            dr5[0] = "";
            dr5[1] = "";
            dr5[2] = "";
            dr5[3] = "";
            dr5[4] = "";
            dt.Rows.Add(dr5);

            DataRow dr6 = dt.NewRow();
            dr6[0] = "   2. Other Sources";
            dr6[1] = "";
            dr6[2] = "";
            dr6[3] = "";
            dr6[4] = "";
            dt.Rows.Add(dr6);


            DataRow dr7 = dt.NewRow();
            dr7[0] = "       2.1. Trust Funds";
            dr7[1] = "";
            dr7[2] = "";
            dr7[3] = "";
            dr7[4] = "";
            dt.Rows.Add(dr7);

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT b.Trans_name,a.Trans_Amount FROM dbo.tbl_R_BMSTrundsAmount as a 
                                                    inner join dbo.tbl_R_BMSTrunds as b on a.Trans_ID = b.Trans_ID where officeID= " + OfficeID + " and Year = " + (Year - 2) + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                var x = 1;
                while (reader.Read())
                {
                    DataRow drqq = dt.NewRow();
                    drqq[0] = "           2.1." + x + ". " + reader.GetValue(0).ToString();
                    drqq[1] = "";
                    drqq[2] = "";
                    drqq[3] = "";
                    drqq[4] = "₱ " + Convert.ToDouble(reader.GetValue(1)).ToString("N2");
                 
                    dt.Rows.Add(drqq);
                    x = x + 1;
                    result1 = Convert.ToDouble(reader.GetValue(1));
                    trundstotal = trundstotal + result1;
                }

            }
            DataRow ddr7123 = dt.NewRow();
            ddr7123[0] = "           Trust Funds Total";
            ddr7123[1] = "";
            ddr7123[2] = "";
            ddr7123[3] = "";
            ddr7123[4] = "₱ " + trundstotal.ToString("N2");
            dt.Rows.Add(ddr7123);

            DataRow dr71 = dt.NewRow();
            dr71[0] = "       2.2. Grants";
            dr71[1] = "";
            dr71[2] = "";
            dr71[3] = "";
            dr71[4] = "";
            dt.Rows.Add(dr71);
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT  grants_name, grants_amount FROM dbo.tbl_R_BMSgrants where OfficeID= " + Account.UserInfo.Department + " and Year = " + (Year - 2) + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                var x = 1;
                while (reader.Read())
                {

                    DataRow dr8 = dt.NewRow();
                    dr8[0] = "           2.2." + x + ". " + reader.GetValue(0).ToString();
                    dr8[1] = "";
                    dr8[2] = "";
                    dr8[3] = "";
                    dr8[4] = "₱ " + Convert.ToDouble(reader.GetValue(1)).ToString("N2");
                    dt.Rows.Add(dr8);
                    x = x + 1;
                    result1 = Convert.ToDouble(reader.GetValue(1));
                    grandstotal = grandstotal + result1;
                }

            }
            DataRow ddr71235 = dt.NewRow();
            ddr71235[0] = "           Grants Total";
            ddr71235[1] = "";
            ddr71235[2] = "";
            ddr71235[3] = "";
            ddr71235[4] = "₱ " + grandstotal.ToString("N2");
            dt.Rows.Add(ddr71235);
            DataRow dr72 = dt.NewRow();
            dr72[0] = "       2.3. Others";
            dr72[1] = "";
            dr72[2] = "";
            dr72[3] = "";
            dr72[4] = "";
            dt.Rows.Add(dr72);

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT other_name ,other_amount FROM dbo.tbl_R_BMSothers where OfficeID= " + Account.UserInfo.Department + " and Year = " + (Year - 2) + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                var x = 1;
                while (reader.Read())
                {

                    DataRow dr8 = dt.NewRow();
                    dr8[0] = "         2.3." + x + ". " + reader.GetValue(0).ToString();
                    dr8[1] = "";
                    dr8[2] = "";
                    dr8[3] = "";
                    dr8[4] = "₱ " + Convert.ToDouble(reader.GetValue(1)).ToString("N2");
                    dt.Rows.Add(dr8);
                    x = x + 1;
                    result1 = Convert.ToDouble(reader.GetValue(1));
                    othertotal = othertotal + result1;
                }

            }
            DataRow ddr712351 = dt.NewRow();
            ddr712351[0] = "           Others Total";
            ddr712351[1] = "";
            ddr712351[2] = "";
            ddr712351[3] = "";
            ddr712351[4] = "₱ " + othertotal.ToString("N2");
            dt.Rows.Add(ddr712351);
            OtherSource = trundstotal + grandstotal + othertotal;

            DataRow dr10 = dt.NewRow();
            dr10[0] = "                  TOTAL";
            dr10[1] = "";
            dr10[2] = "";
            dr10[3] = "";
            dr10[4] = "₱ " + OtherSource.ToString("N2");
            dt.Rows.Add(dr10);
            wholetotal = totalTOTAL + OtherSource;
            DataRow dr101 = dt.NewRow();
            dr101[0] = "                                    TOTAL AMOUNT";
            dr101[1] = "";
            dr101[2] = "";
            dr101[3] = "";
            dr101[4] = "₱ " + wholetotal.ToString("N2");
            dt.Rows.Add(dr101);
            DataRow dr11 = dt.NewRow();
            dr11[0] = "";
            dr11[1] = "";
            dr11[2] = "";
            dr11[3] = "";
            dr11[4] = "";
            dt.Rows.Add(dr11);






      
                    DataRow ddrww = dt.NewRow();
                    ddrww[0] = "A. CY" + (Year - 1) + " (Estimated)";
                    ddrww[1] = "";
                    ddrww[2] = "";
                    ddrww[3] = "";
                    ddrww[4] = "";
                    dt.Rows.Add(ddrww);
                
                DataRow ddr2rr = dt.NewRow();
                ddr2rr[0] = "   1. Provincial Allocation";
                ddr2rr[1] = "";
                ddr2rr[2] = "";
                ddr2rr[3] = "";
                ddr2rr[4] = "";
                dt.Rows.Add(ddr2rr);
            
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"sp_rpt_SOO " + OfficeID + "," + (Year - 1)  + "", con);
                //SqlCommand com = new SqlCommand(@"sp_new_year " + OfficeID + "," + Year + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {


                    result1 = Convert.ToDouble(reader.GetValue(1));
                    result2 = Convert.ToDouble(reader.GetValue(2));
                    result3 = Convert.ToDouble(reader.GetValue(3));
                    dtotalTOTAL = result1 + result2 + result3;

                    DataRow ddr4 = dt.NewRow();
                    ddr4[0] = "       1.1. Office Allocation";
                    ddr4[1] = "₱ " + Convert.ToDouble(reader.GetValue(1)).ToString("N2");
                    ddr4[2] = "₱ " + Convert.ToDouble(reader.GetValue(2)).ToString("N2");
                    ddr4[3] = "₱ " + Convert.ToDouble(reader.GetValue(3)).ToString("N2");
                    ddr4[4] = "₱ " + dtotalTOTAL.ToString("N2");
                    dt.Rows.Add(ddr4);
                }

                DataRow ddr3 = dt.NewRow();
                ddr3[0] = "        1.2. Non-Office (Pls. Specify)";
                ddr3[1] = "";
                ddr3[2] = "";
                ddr3[3] = "";
                ddr3[4] = "";
                dt.Rows.Add(ddr3);

            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT b.AccountName, a.Amount FROM fmis.dbo.tblBMS_NonOfficeAppropriation as a
                                                       inner join dbo.[tbl_R_BMSAccounts] as b on b.FMISAccountCode = a.FmisAccountID 
	                                                   where FmisOfficeID = " + OfficeID + " and YearOf=" + (Year - 1) + "", con);
                //SqlCommand com = new SqlCommand(@"sp_TSG_SOO " + OfficeID + "," + Year + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                var x = 1;
                while (reader.Read())
                {
                    //DataRow ddr = dt.NewRow();
                    //ddr[0] = "           1.2." + x + ". " + reader.GetValue(1).ToString(); 
                    //ddr[1] = "";
                    //ddr[2] = "₱ " + Convert.ToDouble(reader.GetValue(2)).ToString("N2");
                    //ddr[3] = "";
                    //ddr[4] = "₱ " + Convert.ToDouble(reader.GetValue(2)).ToString("N2");
                    //dt.Rows.Add(ddr);
                    //result1 = Convert.ToDouble(reader.GetValue(2));
                    //dtotalTOTAL = result1 + dtotalTOTAL;
                    DataRow ddr = dt.NewRow();
                    ddr[0] = "           1.2." + x + ". " + reader.GetValue(0).ToString();
                    ddr[1] = "";
                    ddr[2] = "₱ " + Convert.ToDouble(reader.GetValue(1)).ToString("N2");
                    ddr[3] = "";
                    ddr[4] = "₱ " + Convert.ToDouble(reader.GetValue(1)).ToString("N2");
                    dt.Rows.Add(ddr);
                    x = x + 1;
                    result1 = Convert.ToDouble(reader.GetValue(1));
                    dtotalTOTAL = result1 + dtotalTOTAL;
                }

                con.Close();

                SqlCommand com2 = new SqlCommand(@"SELECT BudgetAcctName,SooAmount FROM IFMIS.dbo.tbl_R_BMS_SOONONoffice where ActionCode = 1 and OfficeID = '" + Account.UserInfo.Department + "' and YEARof = '" + (Year - 1) + "'", con);
              
                con.Open();
                SqlDataReader reader2 = com2.ExecuteReader();

               
                while (reader2.Read())
                {
                   
                    DataRow ddr = dt.NewRow();
                    ddr[0] = "           1.2." + x + ". " + reader2.GetValue(0).ToString();
                    ddr[1] = "";
                    ddr[2] = "₱ " + Convert.ToDouble(reader2.GetValue(1)).ToString("N2");
                    ddr[3] = "";
                    ddr[4] = "₱ " + Convert.ToDouble(reader2.GetValue(1)).ToString("N2");
                    dt.Rows.Add(ddr);
                    result2 = Convert.ToDouble(reader2.GetValue(1));
                    dtotalTOTAL = result2 + dtotalTOTAL;
                }





            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"sp_rpt_SOO " + OfficeID + "," + (Year - 1) + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    DataRow ddr4 = dt.NewRow();
                    ddr4[0] = "                                    TOTAL";
                    ddr4[1] = "";
                    ddr4[2] = "";
                    ddr4[3] = "";
                    ddr4[4] = "₱ " + dtotalTOTAL.ToString("N2");
                    dt.Rows.Add(ddr4);
                }
            }
            DataRow ddr5 = dt.NewRow();
            ddr5[0] = "";
            ddr5[1] = "";
            ddr5[2] = "";
            ddr5[3] = "";
            ddr5[4] = "";
            dt.Rows.Add(ddr5);

            DataRow ddr6 = dt.NewRow();
            ddr6[0] = "   2. Other Sources";
            ddr6[1] = "";
            ddr6[2] = "";
            ddr6[3] = "";
            ddr6[4] = "";
            dt.Rows.Add(ddr6);


            DataRow ddr7 = dt.NewRow();
            ddr7[0] = "       2.1. Trust Funds";
            ddr7[1] = "";
            ddr7[2] = "";
            ddr7[3] = "";
            ddr7[4] = "";
            dt.Rows.Add(ddr7);

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT b.Trans_name,a.Trans_Amount FROM dbo.tbl_R_BMSTrundsAmount as a 
                                                    inner join dbo.tbl_R_BMSTrunds as b on a.Trans_ID = b.Trans_ID where officeID= " + OfficeID + " and Year = " + (Year - 1) + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                var x = 1;
                while (reader.Read())
                {
                    DataRow drii = dt.NewRow();
                    drii[0] = "           2.1." + x + ". " + reader.GetValue(0).ToString();
                    drii[1] = "";
                    drii[2] = "";
                    drii[3] = "";
                    drii[4] = "₱ " + Convert.ToDouble(reader.GetValue(1)).ToString("N2");
                    
                    dt.Rows.Add(drii);
                    x = x + 1;
                    result1 = Convert.ToDouble(reader.GetValue(1));
                    trundstotals = trundstotals + result1;
                }
                

            }
            DataRow ddr712 = dt.NewRow();
            ddr712[0] = "           Total";
            ddr712[1] = "";
            ddr712[2] = "";
            ddr712[3] = "";
            ddr712[4] = "₱ " + trundstotals.ToString("N2");
            dt.Rows.Add(ddr712);

            DataRow dr712 = dt.NewRow();
            dr712[0] = "       2.2. Grants";
            dr712[1] = "";
            dr712[2] = "";
            dr712[3] = "";
            dr712[4] = "";
            dt.Rows.Add(dr712);
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT  grants_name, grants_amount FROM dbo.tbl_R_BMSgrants where OfficeID= " + Account.UserInfo.Department + " and Year = " + (Year - 1) + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                var x = 1;
                while (reader.Read())
                {

                    DataRow dr8 = dt.NewRow();
                    dr8[0] = "           2.2." + x + ". " + reader.GetValue(0).ToString();
                    dr8[1] = "";
                    dr8[2] = "";
                    dr8[3] = "";
                    dr8[4] = "₱ " + Convert.ToDouble(reader.GetValue(1)).ToString("N2");
                    dt.Rows.Add(dr8);
                    x = x + 1;
                    result1 = Convert.ToDouble(reader.GetValue(1));
                    grandstotals = grandstotals + result1;
                }

            }
            DataRow sddr71235 = dt.NewRow();
            sddr71235[0] = "           Grants Total";
            sddr71235[1] = "";
            sddr71235[2] = "";
            sddr71235[3] = "";
            sddr71235[4] = "₱ " + grandstotals.ToString("N2");
            dt.Rows.Add(sddr71235);
            DataRow dr721 = dt.NewRow();
            dr721[0] = "       2.3. Others";
            dr721[1] = "";
            dr721[2] = "";
            dr721[3] = "";
            dr721[4] = "";
            dt.Rows.Add(dr721);

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT other_name ,other_amount  FROM dbo.tbl_R_BMSothers where OfficeID= " + Account.UserInfo.Department + " and Year = " + (Year - 1) + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();

                var x = 1;
                while (reader.Read())
                {

                    DataRow dr8 = dt.NewRow();
                    dr8[0] = "           2.3." + x + ". " + reader.GetValue(0).ToString();
                    dr8[1] = "";
                    dr8[2] = "";
                    dr8[3] = "";
                    dr8[4] = "₱ " + Convert.ToDouble(reader.GetValue(1)).ToString("N2");
                    dt.Rows.Add(dr8);
                    x = x + 1;
                    result1 = Convert.ToDouble(reader.GetValue(1));
                    othertotals = othertotals + result1;
                }
                //   wholetotal = dtotalTOTAL + totalresults + trundstotals + granttotals + othertotals;
            }
            DataRow ssddr71235 = dt.NewRow();
            ssddr71235[0] = "           Others Total";
            ssddr71235[1] = "";
            ssddr71235[2] = "";
            ssddr71235[3] = "";
            sddr71235[4] = "₱ " + othertotals.ToString("N2");
            dt.Rows.Add(ssddr71235);
            OtherSources = trundstotals + grandstotals + othertotals;
            DataRow dddr10 = dt.NewRow();
            dddr10[0] = "                  TOTAL";
            dddr10[1] = "";
            dddr10[2] = "";
            dddr10[3] = "";
            dddr10[4] = "₱ " + OtherSources.ToString("N2");
            dt.Rows.Add(dddr10);
            wholetotals = OtherSources + dtotalTOTAL;
            DataRow ddr10 = dt.NewRow();
            ddr10[0] = "                      TOTAL AMOUNT";
            ddr10[1] = "";
            ddr10[2] = "";
            ddr10[3] = "";
            ddr10[4] = "₱ " + wholetotals.ToString("N2");
            dt.Rows.Add(ddr10);

            table1.DataSource = dt;

            trundstotal = 0;
            grandstotal = 0;
            othertotal = 0;
            trundstotals = 0;
            grandstotals = 0;
            othertotals = 0;
        }



        //public IEnumerable<ppmp> PPMP_NoNOffdata(int Year, int OfficeID, int ProgramID, int AccountID)
        //{
        //    Year = Year == 0 ? 0 : Year;
        //    OfficeID = OfficeID == 0 ? 0 : OfficeID;
        //    AccountID = AccountID == 0 ? 0 : AccountID;

        //    List<ppmp> dataTable_list = new List<ppmp>();


        //    DataTable dt = PPMP.PPMPNonOffice(2015, 4, 336);

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        ppmp dataTable = new ppmp();
        //        dataTable.itemname = dt.Rows[i]["itemname"].ToString();
        //        dataTable.unit = dt.Rows[i]["unit"].ToString();
        //        dataTable.qty1 = Convert.ToInt32(dt.Rows[i]["qty1cost"]);
        //        dataTable.qty2 = Convert.ToInt32(dt.Rows[i]["qty2cost"]);
        //        dataTable.qty3 = Convert.ToInt32(dt.Rows[i]["qty3cost"]);
        //        dataTable.qty4 = Convert.ToInt32(dt.Rows[i]["qty4cost"]);
        //        dataTable.amount = Convert.ToDouble(dt.Rows[i]["amount"]);
        //        dataTable_list.Add(dataTable);
        //    }



        //    return dataTable_list;
        //}


        public IEnumerable<ppmp> PPMP_NoNOffdata(int Year, int OfficeID)
        {
            Year = Year == 0 ? 0 : Year;
            OfficeID = OfficeID == 0 ? 0 : OfficeID;
            //AccountID = AccountID == 0 ? 0 : AccountID;

            List<ppmp> dataTable_list = new List<ppmp>();


            DataTable dt = PPMP.PPMPNonOffice(Year, OfficeID, 334);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ppmp dataTable = new ppmp();
                dataTable.itemname = dt.Rows[i]["itemname"].ToString();
                dataTable.unit = dt.Rows[i]["unit"].ToString();
                dataTable.unitcost = Convert.ToInt32(dt.Rows[i]["unitcost"]);
                dataTable.qty1 = Convert.ToInt32(dt.Rows[i]["qty1"]);
                dataTable.qty2 = Convert.ToInt32(dt.Rows[i]["qty2"]);
                dataTable.qty3 = Convert.ToInt32(dt.Rows[i]["qty3"]);
                dataTable.qty4 = Convert.ToInt32(dt.Rows[i]["qty4"]);
                dataTable.amount = Convert.ToDouble(dt.Rows[i]["amount"]);
                dataTable_list.Add(dataTable);

                totalSUPPLIES = dataTable.amount + totalSUPPLIES;
            }



            return dataTable_list;
        }


        //public double PPMP_NoNOffdata(int Year, int OfficeID)
        //{

        //    var dt = PPMP.PPMPAmount(2015, 4, 4, 334);

        //    return dt;
        //}





        public static string GetBudgetHead(string isOfficeHead)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select CONCAT(b.Firstname,' ', SUBSTRING(b.MI, 1, 1),'. ',b.Lastname) as 'Name', CONCAT(c.OfficeAbbr,' - ',d.PosShortName) as 'Office & Position'
                                                      from pmis.dbo.m_tblOfficeHeadSignatory as a
                                                      LEFT JOIN pmis.dbo.employee as b on b.eid = a.eid
                                                      LEFT JOIN pmis.dbo.OfficeDescription as c on c.OfficeID = a.OfficeID
                                                      LEFT JOIN pmis.dbo.RefsPositions as d on d.PositionCode = b.position_id
													  where  c.OfficeID = 
                                                    (select b.PMISOfficeID from ifmis.dbo.tbl_R_BMSOffices as b where b.OfficeID = 21) ", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        OfficeHead = reader.GetValue(0).ToString();
                        Designation = reader.GetValue(1).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            if (isOfficeHead == "1")
            {
                return OfficeHead;
            }
            else
            {
                return Designation;
            }

        }













    }
}