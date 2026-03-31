namespace iFMIS_BMS.Reports.Design
{
    using iFMIS_BMS.BusinessLayer.Connector;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using iFMIS_BMS.BusinessLayer.Layers;
    using iFMIS_BMS.BusinessLayer.Models;

    /// <summary>
    /// Summary description for rpt_AIP.
    /// </summary>
    public partial class rpt_AIP : Telerik.Reporting.Report
    {
        public static string OfficeNameReport, ReportID;
        public static int OfficeID, Year;
        public static string OfficeName;
        public static string MyFormat(double num)
        {
            string format = "{0:N2}";
            return string.Format(format, Math.Abs(num));
        }


        public rpt_AIP()
        {

            //officeName.Value = "OFFICE : " + OfficeName;



            DataTable dt = new DataTable();
            dt.Columns.Add("PPA_MFO_ID");
            dt.Columns.Add("PPA_Description");
            dt.Columns.Add("PPA_BreakdownDescription");

            dt.Columns.Add("PSCost");
            dt.Columns.Add("MOOECost");
            dt.Columns.Add("SPACost");
            dt.Columns.Add("COCost");

            dt.Columns.Add("AccountID");
            dt.Columns.Add("ObjectOfExpendetureID");



            InitializeComponent();


            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("SELECT	a.PPA_MFO_ID, a.PPA_Description, b.PPA_Description as PPA_BreakdownDescription, c.ProposalAmount as Cost, b.AccountID, d.ObjectOfExpendetureID FROM dbo.tbl_R_LBP5_PPA_MFO as a INNER JOIN dbo.tbl_R_LBP5_PPA_Denomination as b ON a.PPA_MFO_ID = b.PPA_MFO_ID INNER JOIN dbo.tbl_T_BMSBudgetProposal as c ON c.AccountID = b.AccountID INNER JOIN dbo.tbl_R_BMSProgramAccounts as d on d.AccountID = b.AccountID and d.AccountYear = a.TransactionYear and d.ActionCode = a.ActionCode and d.ProgramID = c.ProgramID WHERE a.OfficeID = 4 and a.TransactionYear = 2017 and a.ActionCode = 1 and b.TransactionYear = 2017 and b.ActionCode = 1 AND c.ProposalYear = 2017 and d.ObjectOfExpendetureID = 1 ORDER BY a.PPA_MFO_ID", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {

                    DataRow dr = dt.NewRow();
                    dr[0] = reader.GetValue(0).ToString();
                    dr[1] = reader.GetValue(1).ToString();
                    dr[2] = reader.GetValue(2).ToString();
                    dr[3] = reader.GetValue(3).ToString();
                    dr[4] = "";
                    dr[5] = "";
                    dr[6] = "" + Environment.NewLine;



                    dt.Rows.Add(dr);


                }
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("SELECT	a.PPA_MFO_ID, a.PPA_Description, b.PPA_Description as PPA_BreakdownDescription, c.ProposalAmount as Cost, b.AccountID, d.ObjectOfExpendetureID FROM dbo.tbl_R_LBP5_PPA_MFO as a INNER JOIN dbo.tbl_R_LBP5_PPA_Denomination as b ON a.PPA_MFO_ID = b.PPA_MFO_ID INNER JOIN dbo.tbl_T_BMSBudgetProposal as c ON c.AccountID = b.AccountID INNER JOIN dbo.tbl_R_BMSProgramAccounts as d on d.AccountID = b.AccountID and d.AccountYear = a.TransactionYear and d.ActionCode = a.ActionCode and d.ProgramID = c.ProgramID WHERE a.OfficeID = 4 and a.TransactionYear = 2017 and a.ActionCode = 1 and b.TransactionYear = 2017 and b.ActionCode = 1 AND c.ProposalYear = 2017 and d.ObjectOfExpendetureID = 2 ORDER BY a.PPA_MFO_ID", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {

                    DataRow dr = dt.NewRow();
                    dr[0] = reader.GetValue(0).ToString();
                    dr[1] = reader.GetValue(1).ToString();
                    dr[2] = reader.GetValue(2).ToString();
                    dr[3] = "";
                    dr[4] = reader.GetValue(3).ToString();
                    dr[5] = "";
                    dr[6] = "" + Environment.NewLine;



                    dt.Rows.Add(dr);


                }
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("SELECT	a.PPA_MFO_ID, a.PPA_Description, b.PPA_Description as PPA_BreakdownDescription, c.ProposalAmount as Cost, b.AccountID, d.ObjectOfExpendetureID FROM dbo.tbl_R_LBP5_PPA_MFO as a INNER JOIN dbo.tbl_R_LBP5_PPA_Denomination as b ON a.PPA_MFO_ID = b.PPA_MFO_ID INNER JOIN dbo.tbl_T_BMSBudgetProposal as c ON c.AccountID = b.AccountID INNER JOIN dbo.tbl_R_BMSProgramAccounts as d on d.AccountID = b.AccountID and d.AccountYear = a.TransactionYear and d.ActionCode = a.ActionCode and d.ProgramID = c.ProgramID WHERE a.OfficeID = 4 and a.TransactionYear = 2017 and a.ActionCode = 1 and b.TransactionYear = 2017 and b.ActionCode = 1 AND c.ProposalYear = 2017 and d.ObjectOfExpendetureID = 3 ORDER BY a.PPA_MFO_ID", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {

                    DataRow dr = dt.NewRow();
                    dr[0] = reader.GetValue(0).ToString();
                    dr[1] = reader.GetValue(1).ToString();
                    dr[2] = reader.GetValue(2).ToString();
                    dr[3] = "";
                    dr[4] = "";
                    dr[5] = "";
                    dr[6] = reader.GetValue(3).ToString() + Environment.NewLine;



                    dt.Rows.Add(dr);


                }
            }
            this.DataSource = dt;



          

        }

    }
}