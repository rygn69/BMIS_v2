namespace Public_TAMS.Reports.Design
{
    //using iFMIS_BMS.BusinessLayer.Connector;
    //using iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for LBP3New_Casual.
    /// </summary>
    public partial class LBP3New_COS : Telerik.Reporting.Report
    {
        public static string MyFormat(double num)
        {
            string format = "{0:N2}";
            if (num < 0)
            {
                return "(" + string.Format(format, Math.Abs(num)) + ")";
            }
            else
            {
                return string.Format(format, Math.Abs(num));
            }

        }
        public string getOfficeName(int OfficeID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["adodb"].ConnectionString))
            {
                SqlCommand com = new SqlCommand(@"Select OfficeName From OfficeDescription Where OfficeID = " + OfficeID + "", con);
                con.Open();
                return com.ExecuteScalar().ToString();
            }
        }

        public int getBMSOfficeID(int OfficeID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["adodb"].ConnectionString))
            {
                SqlCommand com = new SqlCommand(@"select b.OfficeID from IFMIS.dbo.tbl_R_BMSOffices as b where b.PMISOfficeID = @OfficeID", con);
                com.Parameters.AddWithValue("@OfficeID", OfficeID);
                con.Open();
                return 0;//com.ExecuteScalar().IgnoreDBNull(0).ToInt();
            }
        }

        public string getSignatory(int OfficeID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["adodb"].ConnectionString))
            {
                SqlCommand com = new SqlCommand(@"select OfficeHead FROM pmis.dbo.OfficeDescription WHERE OfficeID =
                                                      (select b.PMISOfficeID from IFMIS.dbo.tbl_R_BMSOffices as b where b.OfficeID = " + OfficeID + ") ", con);
                try
                {
                    con.Open();
                    return com.ExecuteScalar().ToString();
                }
                catch (Exception)
                {
                    return "No Signatory";
                }
            }
        }

        public string getReportTextBoxValue(int ReportID, int FieldID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["adodb"].ConnectionString))
                {
                    SqlCommand com = new SqlCommand(@"select [Value] from IFMIS.dbo.tbl_R_BMSReportTextBoxes where ReportID = " + ReportID + " and ActionCode = 1 and FieldID = " + FieldID + "", con);
                    con.Open();
                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        public LBP3New_COS(int OfficeID, int YearOf, int isAllOffice, ref int StartCountRow)
        {
            InitializeComponent();

            DataTable dt = new DataTable();
            //OfficeAdmin_Layer OfficeAdmin_Layer = new OfficeAdmin_Layer();

            dt.Columns.Add("OfficeID");//0
            dt.Columns.Add("OfficeName");//1
            dt.Columns.Add("PositionTitle");//2
            dt.Columns.Add("NameofIncumbent");//3
            dt.Columns.Add("SG");//4
            dt.Columns.Add("DateOfAppointment");//5
            dt.Columns.Add("CurrentSalary");//6
            dt.Columns.Add("FundType");//7
            dt.Columns.Add("Num");//8
            dt.Columns.Add("Charges");//9

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["adodb"].ConnectionString))
            {
                SqlCommand com = new SqlCommand($"select * from nald_vw_cos_plantilla where office_id = @office_id and YearOf = @YearOf order by current_salary desc,position asc", con);
                con.Open();
                com.Parameters.AddWithValue("@office_id", OfficeID);
                com.Parameters.AddWithValue("@YearOf", YearOf);
                com.CommandTimeout = int.MaxValue;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = reader["office_id"].ToString();
                    dr[1] = reader["OfficeName"].ToString();
                    dr[2] = reader["position"].ToString();
                    dr[3] = reader["emp_name"].ToString();
                    dr[4] = reader["SG"].ToString();
                    dr[5] = reader["appointment_date"].ToString();
                //    dr[6] = reader["current_salary"].ToDouble();
                    dr[8] = StartCountRow;
                    dr[9] = reader.GetValue(9).ToString();

                    StartCountRow++;
                    dt.Rows.Add(dr);
                }
            }
            if (isAllOffice == 1)
            {
                //Report.ReportParameters["Signatory5PersonName"].Value = "";//GlobalFunctions.getReportTextBoxValue(3, 3);
            }
            var BMSOfficeID = getBMSOfficeID(OfficeID); //Get BMS OfficeID
            Report.ReportParameters["FormName"].Value = isAllOffice == 2 ? "LBP Form No. 3-A" : "LBP Form No. 3";
            Report.ReportParameters["Annex"].Value = isAllOffice == 2 ? "Annex G" : "Annex F";
            Report.ReportParameters["ReportName"].Value = BMSOfficeID == 1 || BMSOfficeID == 37 || BMSOfficeID == 38 || BMSOfficeID == 41 ? "Contract of Service of Personnel CY <span style='text-decoration: underline'>" + YearOf + "</span>" : "";
            Report.ReportParameters["OfficeName"].Value = getOfficeName(OfficeID).Replace("&", "And");
            Report.ReportParameters["OfficeName"].Text = "Department/Office: <span style='text-decoration: underline'>" + getOfficeName(OfficeID).Replace("&", "And") + "</span>";
            Report.ReportParameters["Signatory1Title"].Value = isAllOffice == 2 ? "Prepared :" : "PREPARED BY :";
            Report.ReportParameters["Signatory2Title"].Value = isAllOffice == 2 ? "Reviewed :" : "REVIEWED BY :";
            Report.ReportParameters["Signatory3Title"].Value = isAllOffice == 2 ? "Approved : " : "APPROVED :";
            Report.ReportParameters["Signatory1PersonName"].Value = isAllOffice == 2 ? getSignatory(OfficeID) : getSignatory(15);
            Report.ReportParameters["Signatory2PersonName"].Value = isAllOffice == 2 ? getSignatory(15) : getSignatory(21);
            // Report.ReportParameters["Signatory3PersonName"].Value = isAllOffice == 2 ? getSignatory(21) : GlobalFunctions.getReportTextBoxValue(3, 3);
            Report.ReportParameters["Signatory1Position"].Value = isAllOffice == 2 ? "Department Head" : "Acting Provincial Human Resource Management Officer";
            Report.ReportParameters["Signatory2Position"].Value = isAllOffice == 2 ? "Human Resource Management Officer" : "Acting Provincial Budget Officer";
            Report.ReportParameters["Signatory3Position"].Value = isAllOffice == 2 ? "Local Chief Executive" : getReportTextBoxValue(3, 4);
            Report.ReportParameters["isAllOffice"].Value = isAllOffice;
            Report.ReportParameters["YearOf"].Value = YearOf;
            Report.ReportParameters["FundType"].Value = BMSOfficeID == 37 || BMSOfficeID == 38 || BMSOfficeID == 41 ? "FUND : ECONOMIC ENTERPRISE" : "FUND : GENERAL FUND";
            Report.ReportParameters["OfficeID"].Value = BMSOfficeID;
            Report.ReportParameters["Country"].Value = BMSOfficeID == 1 || BMSOfficeID == 37 || BMSOfficeID == 38 || BMSOfficeID == 41 ? "Republic of the Philippines" : "";
            Report.ReportParameters["Province"].Value = BMSOfficeID == 1 || BMSOfficeID == 37 || BMSOfficeID == 38 || BMSOfficeID == 41 ? "Province of Agusan del Sur" : "";
            Report.ReportParameters["Government"].Value = BMSOfficeID == 1 || BMSOfficeID == 37 || BMSOfficeID == 38 || BMSOfficeID == 41 ? "Government Center, Patin-Ay Prosperidad" : "";
            Report.ReportParameters["AnnualBudget"].Value = "ANNUAL BUDGET CALENDAR YEAR " + YearOf;
            Report.ReportParameters["hasReportHeader"].Value = BMSOfficeID == 1 || BMSOfficeID == 37 || BMSOfficeID == 38 || BMSOfficeID == 41 ? 0 : 1;
            
            //}
            table1.DataSource = dt;
        }
    }
}