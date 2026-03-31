namespace iFMIS_BMS.Reports
{
    using iFMIS_BMS.BusinessLayer.Connector;
    using iFMIS_BMS.BusinessLayer.Layers;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for BudgetReport.
    /// </summary>
    public partial class BudgetReport : Telerik.Reporting.Report
    {
        public static string  OfficeName, ProgramName, ReportID;
        public static int OfficeIDVar, ProgramID, Year;
        public static bool isPrintAll;
        string office, Program;
        public static string OfficeHead,Designation;
        public static decimal totalPastYear = 0;
        public static decimal totalCurrentYear = 0;
        public static decimal totalBudgetYear = 0;

        public static string MyFormat(double num)
        {
            string format = "{0:N2}";
            return string.Format(format, Math.Abs(num));
        }
        public static string getTotalPast()
        {
            string format = "{0:N2}";
            return string.Format(format, Math.Abs(totalPastYear));
        }
        public static string getTotalCurrent()
        {
            string format = "{0:N2}";
            return string.Format(format, Math.Abs(totalCurrentYear));
        }
        public static string getTotalBudget()
        {
            string format = "{0:N2}";
            return string.Format(format, Math.Abs(totalBudgetYear));
        }
        
        public BudgetReport(int Year, int OfficeID, string OfficeName, string ComputerIP)
        {
            OfficeIDVar = OfficeID;
                foreach (char text in OfficeName)
                {
                    if (text == '&')
                    {
                        office = office + "&amp;";
                    }
                    else
                    {
                        office = office + text;
                    }
                }
                if (OfficeID == 37 || OfficeID == 38 || OfficeID == 41)
                {
                    Program = "Economic Enterprise";
                }
                else
                {
                    Program = "General Fund";
                }

                InitializeComponent();
                    txtOfficeDepartment.Value = "Office/Department : <b>" + office + "</b>";
                    txtYear.Value = "of Year <b>" + Year + "</b>";
                    txtPageAndYear.Value = "Annual Budget Calendar Year " + Year + " . . . . LBP Form No. 3 . . . Page ";
                    txtProgram.Value = "Fund/Special Account : <b>" + Program + "</b>";
                    DataSet ds = new DataSet();
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand("sp_ViewReport " + Year + ","+ OfficeID +"", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("AccountName");
                        dt.Columns.Add("AccountCode");
                        dt.Columns.Add("PastYearApproved");
                        dt.Columns.Add("CurrentYearApproved");
                        dt.Columns.Add("BudgetYearApproved");
                        dt.Columns.Add("OOEName");
                        dt.Columns.Add("OOEID");
                        dt.Columns.Add("Indicator");

                        while (reader.Read())
                        {
                            DataRow dr = dt.NewRow();

                            dr[0] = reader.GetValue(0).ToString();
                            dr[1] = reader.GetValue(1).ToString();
                            dr[2] = Convert.ToDecimal(reader.GetValue(2));
                            dr[3] = Convert.ToDecimal(reader.GetValue(3));
                            dr[4] = Convert.ToDecimal(reader.GetValue(4));
                            dr[5] = reader.GetValue(5).ToString();
                            dr[6] = reader.GetValue(7).ToString();
                            dr[7] = reader.GetValue(8).ToString();

                            dt.Rows.Add(dr);
                        }
                       
                        
                        

                        chAccounts.Value = "Accounts" + Environment.NewLine + "" + "" + Environment.NewLine + Environment.NewLine + "(1)";
                        chAccountCode.Value = "Account Code" + Environment.NewLine + "" + Environment.NewLine + "" + Environment.NewLine + "(2)";
                        chPastYear.Value = "Past Year" + Environment.NewLine + "(" + (Year - 2) + ")" + Environment.NewLine + "(Actual)" + Environment.NewLine + "(3)";
                        chCurrentYear.Value = "Current Year" + Environment.NewLine + "(" + (Year - 1) + ")" + Environment.NewLine + "(Estimates)" + Environment.NewLine + "(4)";
                        chBudgetYear.Value = "Budget Year" + Environment.NewLine + "(" + Year + ")" + Environment.NewLine + "(Proposed)" + Environment.NewLine + "(5)";
                        this.DataSource = dt;
                    }
                    
                    var BudgetOfficeID = 21;
                    var GovernorsOfficeID = 1;
                    var PrintedBy = "";
                    var PrintedByPosition = "";
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com2 = new SqlCommand(@"select top 1 UPPER(Firstname + ' ' + left(MI,1) + '. ' + LASTNAME), Position from pmis.dbo.employee where eid = " + Account.UserInfo.eid + "", con);
                        con.Open();
                        SqlDataReader reader = com2.ExecuteReader();
                        reader.Read();
                        PrintedBy = reader.GetValue(0).ToString();
                        PrintedByPosition = reader.GetValue(1).ToString();
                    }

                    txtOfficehead.Value = GlobalFunctions.getOfficeHead(OfficeID);
                    txtDesignation.Value = GlobalFunctions.getOfficeHeadDesignation(OfficeID);
                    txtBudgetHead.Value = GlobalFunctions.getOfficeHead(BudgetOfficeID);
                    txtBudgetHeadDesignation.Value = GlobalFunctions.getOfficeHeadDesignation(BudgetOfficeID);
                    txtProvincialGovernor.Value = GlobalFunctions.getOfficeHeadDesignation(GovernorsOfficeID);
                    txtProvincialGovernorDesignatiom.Value = GlobalFunctions.getOfficeHeadDesignation(GovernorsOfficeID);
                    QRCode.Value = GlobalFunctions.QRCodeValue(PrintedBy, ComputerIP);
        }
    }
}