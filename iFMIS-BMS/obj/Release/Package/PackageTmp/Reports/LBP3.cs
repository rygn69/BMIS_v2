namespace iFMIS_BMS.Reports
{
    using iFMIS_BMS.BusinessLayer.Connector;
    using iFMIS_BMS.BusinessLayer.Layers;
    using iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation;
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
    public partial class LBP3 : Telerik.Reporting.Report
    {
        //public static string OfficeName, ProgramName, ReportID;
        public static int OfficeIDVar;
        //public static int OfficeIDVar, ProgramID, Year;
        public static bool isPrintAll;
        string office;
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
       
        public LBP3(int OfficeID, string OfficeName, int Year, string ReportID,string ComputerIP)
        {
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
            OfficeIDVar = OfficeID;
            OfficeAdmin_Layer OfficeAdmin_Layer = new OfficeAdmin_Layer();
            totalPastYear = 0;
            totalCurrentYear = 0;
            totalBudgetYear = 0;
            var ProgramID="";
            var ProgramName = "";
            #region Approved Budget Report
            var fundType = "";
            
            if (OfficeID == 37 || OfficeID == 38 || OfficeID == 41)
            {
                fundType = "Economic Enterprise";
            }
            else
            {
                fundType = "General Fund";
            }

            if (ReportID == "PreparedReport")
            {
                InitializeComponent();
                txtOfficeDepartment.Value = "Office/Department : <b>" + office + "</b>";
                txtYear.Value = "of Year <b>" + Year + "</b>";
                txtProgram.Value = "Fund/Special Account : <b>" + fundType + "</b>";
                txtPageAndYear.Value = "Annual Budget Calendar Year " + Year + " . . . . LBP Form No. 3 . . . Page ";
                DataTable dt = new DataTable();
                dt.Columns.Add("AccountName");              //dr[0]
                dt.Columns.Add("AccountCode");              //dr[1]
                dt.Columns.Add("PastYearApproved");         //dr[2]
                dt.Columns.Add("CurrentYearApproved");      //dr[3]
                dt.Columns.Add("BudgetYearApproved");       //dr[4]
                dt.Columns.Add("OOEName");                  //dr[5]
                dt.Columns.Add("OOEID");                    //dr[6]
                dt.Columns.Add("Indicator");                //dr[7]
                dt.Columns.Add("SubTotal");                 //dr[8]
                dt.Columns.Add("BudgetYear1");              //dr[9]
                dt.Columns.Add("BudgetYear2");              //dr[10]
                dt.Columns.Add("ProgramID");                //dr[11]
                dt.Columns.Add("ProgramName");              //dr[12]
                dt.Columns.Add("SubTotalProgram");          //dr[13]

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"SELECT DISTINCT e.AccountID,  e.AccountName,  f.OOEName,a.ProposalYear,a.ProposalAllotedAmount,e.AccountYear,e.ProgramID, isnull(b.AccountCode,0), e.ObjectOfExpendetureID,c.ProgramDescription FROM dbo.tbl_T_BMSBudgetProposal AS a
                                                    left JOIN dbo.tbl_R_BMSProgramAccounts AS e ON e.ProgramID = a.ProgramID AND e.AccountID = a.AccountID
                                                    left JOIN dbo.tbl_R_BMSAccounts AS b ON b.FMISAccountCode = a.AccountID 
                                                    left JOIN dbo.tbl_R_BMSOfficePrograms AS c ON c.ProgramID = a.ProgramID 
                                                    left JOIN dbo.tbl_R_BMSOffices AS d ON d.OfficeID = c.OfficeID 
                                                    LEFT JOIN dbo.tbl_R_BMSObjectOfExpenditure as f on f.OOEID = e.ObjectOfExpendetureID 
                                                    LEFT JOIN dbo.tbl_R_BMSFunds as g on g.FundCode = c.FundCode
                                                    WHERE c.OfficeID = '" + OfficeID + "' and   a.ProposalYear = " + Year + "-1 and e.ActionCode = 1 and a.ProposalActionCode = 1  and e.AccountYear='" + Year + "' and c.ProgramYear = '" + Year + "' ORDER BY e.ObjectOfExpendetureID, isnull(b.AccountCode,0)", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        var AllotedAmount = 0.0;
                        DataRow dr = dt.NewRow();
                        dr[0] = reader.GetValue(1).ToString();
                        dr[1] = reader.GetValue(7).ToString();
                        dr[2] = OfficeAdmin_Layer.getPastYearAmount(Year, reader.GetValue(0).ToString(), OfficeID, reader.GetValue(6).ToString(), Convert.ToInt32(reader.GetValue(6)));
                        dr[3] = Convert.ToDouble(reader.GetValue(4));

                        if (Convert.ToInt32(reader.GetValue(8)) == 1 || Convert.ToInt32(reader.GetValue(8)) == 2 || Convert.ToInt32(reader.GetValue(8)) == 3)
                        {
                           dr[4] = Convert.ToDouble(OfficeAdmin_Layer.setProposalAllotedAmount(Convert.ToInt32(reader.GetValue(7)), OfficeID, Year, Convert.ToInt32(reader.GetValue(8)), Convert.ToInt32(reader.GetValue(6)), Convert.ToInt32(reader.GetValue(0))));
                           AllotedAmount = Convert.ToDouble(OfficeAdmin_Layer.setProposalAllotedAmount(Convert.ToInt32(reader.GetValue(7)), OfficeID, Year, Convert.ToInt32(reader.GetValue(8)), Convert.ToInt32(reader.GetValue(6)), Convert.ToInt32(reader.GetValue(0))));
                        }
                        else
                        {
                            dr[4] = 0;
                        }

                        dr[5] = reader.GetValue(2).ToString();
                        dr[6] = reader.GetValue(8).ToString();
                        dr[7] = 0;
                        dr[8] = "Sub Total " + reader.GetValue(2).ToString();
                        dr[9] = AllotedAmount;
                        dr[10] = AllotedAmount;
                        //if (getPSBudgetYear(reader.GetInt32(7), OfficeID, (Year + 1)) == 0)
                        //{
                        //    dr[9] = Convert.ToDouble(OfficeAdmin_Layer.setProposalAllotedAmount(Convert.ToInt32(reader.GetValue(7)), OfficeID, (Year), Convert.ToInt32(reader.GetValue(8)), Convert.ToInt32(reader.GetValue(6)), Convert.ToInt32(reader.GetValue(0))));
                        //}
                        //else
                        //{
                        //    dr[9] = getPSBudgetYear(reader.GetInt32(7), OfficeID, (Year + 1));
                        //}
                        //if (getPSBudgetYear(reader.GetInt32(7), OfficeID, (Year + 2)) == 0)
                        //{
                        //    dr[10] = Convert.ToDouble(OfficeAdmin_Layer.setProposalAllotedAmount(Convert.ToInt32(reader.GetValue(7)), OfficeID, (Year), Convert.ToInt32(reader.GetValue(8)), Convert.ToInt32(reader.GetValue(6)), Convert.ToInt32(reader.GetValue(0))));
                        //}
                        //else
                        //{
                        //    dr[10] = getPSBudgetYear(reader.GetInt32(7), OfficeID, (Year + 2));
                        //}
                        dr[11] = reader.GetValue(6).ToString();
                        dr[12] = reader.GetValue(9).ToString();
                        dr[13] = "Sub Total " + reader.GetValue(9).ToString();

                        dt.Rows.Add(dr);
                        if (reader.GetValue(8).ToString() == "1")
                        {
                              ProgramID = reader.GetValue(6).ToString();
                              ProgramName = reader.GetValue(9).ToString();
                        }
                    }
                    DataRow dr2 = dt.NewRow();
                    dr2[0] = "Proposed Positions For Funding";
                    dr2[1] = 0;
                    dr2[2] = Convert.ToDouble(0);
                    dr2[3] = Convert.ToDouble(0);
                    dr2[4] = OfficeAdmin_Layer.getforFundingTotal(Year, OfficeID) + OfficeAdmin_Layer.getforFundingCasualTotal(Year, OfficeID);
                    dr2[5] = "Personal Services";
                    dr2[6] = 1;
                    dr2[7] = 0;
                    dr2[8] = "Sub Total Personal Services";
                    dr2[9] = OfficeAdmin_Layer.getforFundingTotal(Year, OfficeID) + OfficeAdmin_Layer.getforFundingCasualTotal(Year, OfficeID);
                    dr2[10] = OfficeAdmin_Layer.getforFundingTotal(Year, OfficeID) + OfficeAdmin_Layer.getforFundingCasualTotal(Year, OfficeID);
                    dr2[11] = ProgramID;
                    dr2[12] = ProgramName;
                    dr2[13] = "Sub Total " + ProgramName;
                    if (OfficeAdmin_Layer.getforFundingTotal(Year, OfficeID) != 0)
                    {
                        dt.Rows.Add(dr2);
                    }
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"SELECT a.ProposedID,
                        a.AccountID, a.AccountName, c.OOEName, a.ProposalYear, a.ProgramID, a.AccountCode, a.OOEID, a.OfficeID, a.ActionCode, b.ProgramDescription 
                        FROM tbl_R_BMSProposedAccounts as a 
                        INNER JOIN dbo.tbl_R_BMSOfficePrograms AS b ON a.ProgramID = b.ProgramID and a.ActionCode = b.ActionCode and a.proposalYear = b.Programyear
                        INNER JOIN dbo.tbl_R_BMSObjectOfExpenditure as c ON a.OOEID = c.OOEID
                        WHERE a.OfficeID = '" + OfficeID + "' and b.ProgramYear = '" + Year + "' and  a.ActionCode = 1 ", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        DataRow dr3 = dt.NewRow();
                        dr3[0] = reader.GetValue(2).ToString();
                        dr3[1] = reader.GetValue(6).ToString();
                        dr3[2] = Convert.ToDouble(0);
                        dr3[3] = Convert.ToDouble(0);
                        dr3[4] = OfficeAdmin_Layer.setProposalAllotedAmount(Convert.ToInt32(reader.GetValue(6)), OfficeID, Year, Convert.ToInt32(reader.GetValue(7)), Convert.ToInt32(reader.GetValue(5)), Convert.ToInt32(reader.GetValue(1)));
                        dr3[5] = reader.GetValue(3).ToString();
                        dr3[6] = reader.GetValue(7).ToString();
                        dr3[7] = 0;
                        dr3[8] = "Sub Total " + reader.GetValue(3).ToString();
                        dr3[9] = OfficeAdmin_Layer.setProposalAllotedAmount(Convert.ToInt32(reader.GetValue(6)), OfficeID, Year, Convert.ToInt32(reader.GetValue(7)), Convert.ToInt32(reader.GetValue(5)), Convert.ToInt32(reader.GetValue(1)));
                        dr3[10] = OfficeAdmin_Layer.setProposalAllotedAmount(Convert.ToInt32(reader.GetValue(6)), OfficeID, Year, Convert.ToInt32(reader.GetValue(7)), Convert.ToInt32(reader.GetValue(5)), Convert.ToInt32(reader.GetValue(1)));
                        dr3[11] = reader.GetValue(5).ToString();
                        dr3[12] = reader.GetValue(10).ToString();
                        dr3[13] = "Sub Total " + reader.GetValue(10).ToString();

                        dt.Rows.Add(dr3);
                    }
                }
                chAccounts.Value = "Accounts" + Environment.NewLine + "" + "" + Environment.NewLine + Environment.NewLine + "(1)";
                chAccountCode.Value = "Account Code" + Environment.NewLine + "" + Environment.NewLine + "" + Environment.NewLine + "(2)";
                chPastYear.Value = "Past Year" + Environment.NewLine + "(" + (Year - 2) + ")" + Environment.NewLine + "(Actual)" + Environment.NewLine + "(3)";
                chCurrentYear.Value = "Current Year" + Environment.NewLine + "(" + (Year - 1) + ")" + Environment.NewLine + "(Estimates)" + Environment.NewLine + "(4)";
                chBudgetYear.Value = "Budget Year" + Environment.NewLine + "(" + Year + ")" + Environment.NewLine + "(Proposed)" + Environment.NewLine + "(5)";
                chBudgetYear1.Value = "Budget Year" + Environment.NewLine + "(" + (Year + 1) + ")" + Environment.NewLine + "(Proposed)" + Environment.NewLine + "(6)";
                chBudgetYear2.Value = "Budget Year" + Environment.NewLine + "(" + (Year + 2) + ")" + Environment.NewLine + "(Proposed)" + Environment.NewLine + "(7)";
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
            txtGovernor.Value = GlobalFunctions.getOfficeHead(GovernorsOfficeID);
            txtGovernorDesignation.Value = GlobalFunctions.getOfficeHeadDesignation(GovernorsOfficeID);
            QRCode.Value = GlobalFunctions.QRCodeValue(PrintedBy, ComputerIP);
            #endregion
        }
        public double getPSBudgetYear(int AccountID, int OfficeID, int propYear1)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                OfficeAdmin_Layer OfficeAdmin_Layer = new OfficeAdmin_Layer();
                SqlCommand com = new SqlCommand(@"dbo.sp_bms_getComputationTotal " + AccountID + "," + OfficeAdmin_Layer.getPmisOfficeID(OfficeID) + "," + propYear1 + ", 0, 0,0, 0," + Account.UserInfo.eid + "", con);
                con.Open();
                return Convert.ToDouble(com.ExecuteScalar());
            }
        }
    }
}