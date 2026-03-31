namespace iFMIS_BMS.Reports
{
    using iFMIS_BMS.BusinessLayer.Connector;
    using iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for LBP3New.
    /// </summary>
    public partial class LBP3New : Telerik.Reporting.Report
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
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Select OfficeName From tbl_R_BMSOffices Where OfficeID = "+OfficeID+"", con);
                con.Open();
                return com.ExecuteScalar().ToString();
            }
        }
        public string getSignatory(int OfficeID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select OfficeHead FROM pmis.dbo.OfficeDescription WHERE OfficeID =
                                                      (select b.PMISOfficeID from dbo.tbl_R_BMSOffices as b where b.OfficeID = " + OfficeID + ") ", con);
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
        public string getOfficeHeadPosition(int OfficeID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select OfficeDesignation FROM pmis.dbo.OfficeDescription WHERE OfficeID =
                                                      (select b.PMISOfficeID from dbo.tbl_R_BMSOffices as b where b.OfficeID = "+OfficeID+") " , con);
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
        
        public LBP3New(int OfficeID, int Year,int isAllOffice)
        {
            InitializeComponent();

            DataTable dt = new DataTable();
            OfficeAdmin_Layer OfficeAdmin_Layer = new OfficeAdmin_Layer();

            dt.Columns.Add("OlditemNo"); //dr[0]
            dt.Columns.Add("NewItemNo");//dr[1]
            dt.Columns.Add("DivisionName");//dr[2]
            dt.Columns.Add("PositionTitle");//dr[3]
            dt.Columns.Add("oldSG");//dr[4]
            dt.Columns.Add("OldStep");//dr[5]
            dt.Columns.Add("NewStep");//dr[6]
            dt.Columns.Add("DateOfAppointment");//dr[7]
            dt.Columns.Add("OldSalary");//dr[8]
            dt.Columns.Add("NewSalaryNoStep");//dr[9]
            dt.Columns.Add("IncreaseNoStep");//dr[10]
            dt.Columns.Add("NewSalaryWithStep");//dr[12]
            dt.Columns.Add("IncreaseWithStep");//dr[13]
            dt.Columns.Add("EffectivityDate");//dr[14]
            dt.Columns.Add("NewSG");//dr[15]
            dt.Columns.Add("FundType");//dr[16]
            dt.Columns.Add("OfficeName");//dr[17]
            dt.Columns.Add("NameofIncumbent");//dr[18]
            dt.Columns.Add("OfficeID");//dr[19]
            
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"sp_BMS_get_LBP4_Report_LFC " + Year + "," + OfficeID + "", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = reader.GetValue(1).ToString();  //OlditemNo
                        dr[1] = reader.GetValue(2).ToString();  //NewItemNo
                        dr[2] = reader.GetValue(3).ToString();  ///DivisionName
                        dr[3] = reader.GetValue(4).ToString();  //PositionTitle
                        dr[4] = reader.GetValue(6).ToString();  //oldSG
                        dr[5] = reader.GetValue(7).ToString();  //OldStep
                        dr[6] = reader.GetValue(8).ToString();  //NewStep
                        dr[7] = reader.GetValue(9).ToString();  //DateOfAppointment
                        dr[8] = Convert.ToDouble(reader.GetValue(10));  //OldSalary
                        dr[9] = Convert.ToDouble(reader.GetValue(11)); //NewSalaryNoStep
                        dr[10] = Convert.ToDouble(reader.GetValue(13)); //IncreaseNoStep
                        dr[11] = Convert.ToDouble(reader.GetValue(12)); //NewSalaryWithStep
                        dr[12] = Convert.ToDouble(reader.GetValue(14)); //IncreaseWithStep
                        try
                        {
                            dr[13] = reader.GetValue(20).ToString();        //EffectivityDate
                        }
                        catch (Exception)
                        {
                            dr[13] = "";        //EffectivityDate
                        }
                        dr[14] = reader.GetValue(21).ToString(); //NewSG
                        dr[15] = 0; //FundType
                        dr[16] = 0; //OfficeName
                        dr[17] = reader.GetValue(5).ToString(); //NameofIncumbent
                        dr[18] = OfficeID; //OfficeID

                        dt.Rows.Add(dr);
                    }
                }
           
                Report.ReportParameters["FormName"].Value = isAllOffice == 2?"LBP Form No. 3-A":"LBP Form No. 3";
                Report.ReportParameters["Annex"].Value = isAllOffice == 2 ? "Annex G" : "Annex F";
                Report.ReportParameters["ReportName"].Value = isAllOffice == 2 ? "Personnel Schedule FY <span style='text-decoration: underline'>" + Year + "</span>" : OfficeID == 1 || OfficeID == 37 || OfficeID == 38 || OfficeID == 41 ? "Plantilla of Personnel CY <span style='text-decoration: underline'>" + Year + "</span>" : "";
                Report.ReportParameters["OfficeName"].Value = getOfficeName(OfficeID).Replace("&", "And");
                Report.ReportParameters["OfficeName"].Text = "Department/Office: <span style='text-decoration: underline'>" + getOfficeName(OfficeID).Replace("&", "And") + "</span>";
                Report.ReportParameters["Signatory1Title"].Value = isAllOffice == 2 ?"Prepared :":"PREPARED BY :";
                Report.ReportParameters["Signatory2Title"].Value = isAllOffice == 2 ?"Reviewed :":"REVIEWED BY :";
                Report.ReportParameters["Signatory3Title"].Value = isAllOffice == 2 ?"Approved : ":"APPROVED :";
                Report.ReportParameters["Signatory1PersonName"].Value = isAllOffice == 2 ? getSignatory(OfficeID) : getSignatory(OfficeID);
                Report.ReportParameters["Signatory2PersonName"].Value = isAllOffice == 2 ?getSignatory(15) :getSignatory(15);
               // Report.ReportParameters["Signatory3PersonName"].Value = isAllOffice == 2 ? getSignatory(21) : GlobalFunctions.getReportTextBoxValue(3, 3);
                Report.ReportParameters["Signatory1Position"].Value = isAllOffice == 2 ? getOfficeHeadPosition(OfficeID) : getOfficeHeadPosition(OfficeID);
                Report.ReportParameters["Signatory2Position"].Value = isAllOffice == 2 ? "Acting PHRMO" : "Acting PHRMO";
                Report.ReportParameters["Signatory3Position"].Value = isAllOffice == 2 ? "Provincial Governor" : GlobalFunctions.getReportTextBoxValue(3, 4);
                Report.ReportParameters["isAllOffice"].Value = isAllOffice;
                Report.ReportParameters["YearOf"].Value = Year;
                Report.ReportParameters["FundType"].Value = OfficeID == 37 || OfficeID == 38 || OfficeID == 41 ? "FUND : ECONOMIC ENTERPRISE" : "FUND : GENERAL FUND";
                Report.ReportParameters["OfficeID"].Value = OfficeID;
                Report.ReportParameters["Country"].Value = OfficeID == 1 || OfficeID == 37 || OfficeID == 38 || OfficeID == 41 ? "Republic of the Philippines" : "";
                Report.ReportParameters["Province"].Value = OfficeID == 1 || OfficeID == 37 || OfficeID == 38 || OfficeID == 41 ? "Province of Agusan del Sur" : "";
                Report.ReportParameters["Government"].Value = OfficeID == 1 || OfficeID == 37 || OfficeID == 38 || OfficeID == 41 ? "Government Center, Patin-Ay Prosperidad" : "";
                Report.ReportParameters["AnnualBudget"].Value = "ANNUAL BUDGET CALENDAR YEAR "+ Year;
                Report.ReportParameters["hasReportHeader"].Value = OfficeID == 1 || OfficeID == 37 || OfficeID == 38 || OfficeID == 41?0:1;

            if (isAllOffice == 1)
            {
                //Report.ReportParameters["Signatory5PersonName"].Value = "";//GlobalFunctions.getReportTextBoxValue(3, 3);
                textBox14.Visible = true;
                textBox17.Visible = true;
                textBox20.Visible = true;
                textBox15.Visible = false;
                textBox18.Visible = false;
                textBox21.Visible = false;
                textBox16.Visible = false;
                textBox4.Visible = false;
                textBox22.Visible = false;
            }
            else
            {
                textBox14.Visible = true;
                textBox17.Visible = true;
                textBox20.Visible = true;
                textBox15.Visible = true;
                textBox18.Visible = true;
                textBox21.Visible = true;
                textBox16.Visible = true;
                textBox4.Visible = true;
                textBox22.Visible = true;
            }
            //}
            table1.DataSource = dt;
        }
    }
}