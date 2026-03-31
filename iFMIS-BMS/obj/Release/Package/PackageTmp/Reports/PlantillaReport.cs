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
    /// Summary description for PlantillaReport.
    /// </summary>
    public partial class PlantillaReport : Telerik.Reporting.Report
    {
        public static string OfficeHead, Designation;
        public static int OfficeIDVar;
        string OfficeNamevar;
        public static string MyFormat(double num)
        {
            string format = "{0:N2}";
            return string.Format(format, Math.Abs(num));
        }
        public static string GetOfficeHead(string isOfficeHead)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
//                    SqlCommand com = new SqlCommand(@"select CONCAT(b.Firstname,' ', SUBSTRING(b.MI, 1, 1),'. ',b.Lastname, ' ', b.Suffix) as 'Name', CONCAT(c.OfficeAbbr,' - ',d.PosShortName) as 'Office & Position'
//                                                      from pmis.dbo.m_tblOfficeHeadSignatory as a
//                                                      LEFT JOIN pmis.dbo.employee as b on b.eid = a.eid
//                                                      LEFT JOIN pmis.dbo.OfficeDescription as c on c.OfficeID = a.OfficeID
//                                                      LEFT JOIN pmis.dbo.RefsPositions as d on d.PositionCode = b.position_id
//													  where  c.OfficeID =  
//                                                      (select b.PMISOfficeID from dbo.tbl_R_BMSOffices as b where b.OfficeID = " + OfficeIDVar + ") ", con);

                    SqlCommand com = new SqlCommand(@"select OfficeHead, OfficeDesignation FROM pmis.dbo.OfficeDescription WHERE OfficeID =
                                                      (select b.PMISOfficeID from dbo.tbl_R_BMSOffices as b where b.OfficeID = " + OfficeIDVar + ") ", con);
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
        public static string GetBudgetHead(string isOfficeHead)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select CONCAT(b.Firstname,' ', SUBSTRING(b.MI, 1, 1),'. ',b.Lastname, ' ', b.Suffix) as 'Name', CONCAT(c.OfficeAbbr,' - ',d.PosShortName) as 'Office & Position'
                                                      from pmis.dbo.m_tblOfficeHeadSignatory as a
                                                      LEFT JOIN pmis.dbo.employee as b on b.eid = a.eid
                                                      LEFT JOIN pmis.dbo.OfficeDescription as c on c.OfficeID = a.OfficeID
                                                      LEFT JOIN pmis.dbo.RefsPositions as d on d.PositionCode = b.position_id
													  where  c.OfficeID = 
                                                    (select b.PMISOfficeID from dbo.tbl_R_BMSOffices as b where b.OfficeID = 21) ", con);
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
        public PlantillaReport(int OfficeID, string OfficeName, int Year, int IncludeProposed)
        {
            foreach (char text in OfficeName)
            {
                if (text == '&')
                {
                    OfficeNamevar = OfficeNamevar + "&amp;";
                }
                else
                {
                    OfficeNamevar = OfficeNamevar + text;
                }
            }
            InitializeComponent();
            OfficeIDVar = OfficeID;
            DataTable dt = new DataTable();
            OfficeAdmin_Layer OfficeAdmin_Layer = new OfficeAdmin_Layer();

            dt.Columns.Add("OlditemNo"); //dr[0]
            dt.Columns.Add("NewItemNo");//dr[1]
            dt.Columns.Add("DivName");//dr[2]
            dt.Columns.Add("PositionTitle");//dr[3]
            dt.Columns.Add("DateOfAppointment");//dr[4]
            dt.Columns.Add("sgCurrent");//dr[5]
            dt.Columns.Add("StepCurrent");//dr[6]
            dt.Columns.Add("AmountCurrent");//dr[7]
            dt.Columns.Add("StepProposed1");//dr[8]
            dt.Columns.Add("AmountProposed1");//dr[9]
            dt.Columns.Add("StepProposed2");//dr[10]
            dt.Columns.Add("AmountProposed2");//dr[11]
            dt.Columns.Add("StepProposed3");//dr[12]
            dt.Columns.Add("AmountProposed3");//dr[13]
            dt.Columns.Add("Increase");//dr[14]
            dt.Columns.Add("Indicator");//dr[15]
            

            var query = "";

            var MotherOfficeID = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT isnull(MainOfficeID_PMIS,0) 
                FROM [dbo].[tbl_R_BMSMainAndSubOffices] where SubOfficeID_IFMIS =" + OfficeID + "", con);
                con.Open();
                MotherOfficeID = Convert.ToInt32(com.ExecuteScalar());
            }
            if (MotherOfficeID == 0)
            {
                query = "sp_get_PlantillaReport3yearsPlan " + Year + "," + OfficeAdmin_Layer.getPmisOfficeID(OfficeID) + "";
            }
            else
            {
                query = "sp_get_PlantillaReport3yearsPlanSubOffice " + Year + "," + MotherOfficeID + ", " + OfficeID + "";
            }

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = reader.GetValue(1).ToString();
                    dr[1] = reader.GetValue(2).ToString();
                    dr[2] = reader.GetValue(3).ToString();
                    dr[3] = reader.GetValue(4).ToString() + Environment.NewLine + reader.GetValue(5).ToString();
                    dr[4] = reader.GetValue(6).ToString();
                    dr[5] = reader.GetValue(7).ToString();
                    dr[6] = reader.GetValue(8).ToString();
                    dr[7] = Convert.ToDouble(reader.GetValue(12));
                    dr[8] = reader.GetValue(9).ToString();
                    dr[9] = Convert.ToDouble(reader.GetValue(13));
                    dr[10] = reader.GetValue(10).ToString();
                    dr[11] = Convert.ToDouble(reader.GetValue(14));
                    dr[12] = reader.GetValue(11).ToString();
                    dr[13] = Convert.ToDouble(reader.GetValue(15));
                    dr[14] = Convert.ToDouble(Math.Abs(reader.GetDecimal(13) - reader.GetDecimal(12)));
                    dr[15] = 0;


                    dt.Rows.Add(dr);
                }
            }
            if (IncludeProposed == 1)
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select distinct a.ProposedItemID as 'ProposedItemID(0)',case when a.DivisionID = 0 then 'No Division'else b.rowno +' ' +  b.DivName end as 'DivName(1)',c.Pos_name as 'Position(2)',a.AppointmentDateEffectivity as 'Effectivity(3)', 
                                                pmis.dbo.sclr_fnc_get_salary(c.sg,1,(select aeffectivity from pmis.dbo.EDGE_tblEffectivityDate where yearof = " + (Year - 1) + " )) as 'Salary(4)'," + ""
                                                    + " pmis.dbo.sclr_fnc_get_salary(c.sg,1,(select aeffectivity from pmis.dbo.EDGE_tblEffectivityDate where yearof = " + (Year - 1) + " )) * " + ""
                                                    + " (13 - DATEPART(mm,a.AppointmentDateEffectivity)) as 'YearlySalary(5)', c.sg as 'sg(6)',case when a.ActionCode = 1 then 2 else 4 end as 'Indicator(7)' from tbl_R_BMSProposedNewItem as a " + ""
                                                    + " inner join pmis.dbo.EDGE_tblPlantillaDivision as b on b.DivID = a.DivisionID or a.DivisionID = 0" + ""
                                                    + " inner JOIN pmis.dbo.RefsPositions as c on c.PositionCOde = a.PositionID where a.ActionCode != 0 and a.OfficeID=" + OfficeID + "and a.Yearof = " + (Year - 1) + " + 1 ", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = "";
                        dr[1] = "";
                        dr[2] = reader.GetValue(1).ToString();
                        dr[3] = reader.GetValue(2).ToString() + Environment.NewLine + "(For Funding)";
                        dr[4] = reader.GetDateTime(3).ToShortDateString();
                        dr[5] = reader.GetValue(6).ToString();
                        dr[6] = "";
                        dr[7] = Convert.ToDouble(0);
                        dr[8] = "1";
                        dr[9] = Convert.ToDouble(reader.GetValue(5));
                        dr[10] = "1";
                        dr[11] = Convert.ToDouble(reader.GetValue(5));
                        dr[12] = "1";
                        dr[13] = Convert.ToDouble(reader.GetValue(5));
                        dr[14] = Convert.ToDouble(0);
                        dr[15] = 1;

                        dt.Rows.Add(dr);
                    }
                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select a.ProposedItemID, a.AppointmentDateEffectivity,b.CasualRateName,
                                                b.CasualRate * 22 as 'MonthlySalary',b.CasualRate * 22 * (13 - DATEPART(mm,a.AppointmentDateEffectivity)) as 'Anual Salary'  
                                                ,case when a.ActionCode = 1 then 3 else 4 end as 'Indicator(5)' from tbl_R_BMSProposedNewItem as a 
                                                inner JOIN tbl_R_BMSCasualRate as b on b.CasualRateID = a.EmploymentStatusID
                                                where a.PositionID is null and a.DivisionID is null and a.officeID = " + OfficeID + " and a.actioncode != 0 and a.yearof = " + (Year - 1) + " + 1", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = "";
                        dr[1] = "";
                        dr[2] = "Proposed Casual";
                        dr[3] = reader.GetValue(2).ToString() + Environment.NewLine + "(For Funding)";
                        dr[4] = reader.GetDateTime(1).ToShortDateString();
                        dr[5] = "";
                        dr[6] = "";
                        dr[7] = Convert.ToDouble(0);
                        dr[8] = "";
                        dr[9] = Convert.ToDouble(reader.GetValue(4));
                        dr[10] = "";
                        dr[11] = Convert.ToDouble(reader.GetValue(4));
                        dr[12] = "";
                        dr[13] = Convert.ToDouble(reader.GetValue(4));
                        dr[14] = Convert.ToDouble(0);
                        dr[15] = 1;

                        dt.Rows.Add(dr);
                    }
                }    
            }
            

            txtActual.Value = "Actual Salary Effective" + Environment.NewLine + "January 1, " + (Year - 1);
            txtProposal1.Value = "Actual Salary Effective" + Environment.NewLine + "January 1, " + Year;
            txtProposal2.Value = "Actual Salary Effective" + Environment.NewLine + "January 1, " + (Year + 1);
            txtProposal3.Value = "Actual Salary Effective" + Environment.NewLine + "January 1, " + (Year + 2);
            txtYear.Value = "CY " + Year;
            txtYearFooter.Value = "Annual Budget CY" + Year + "...LBP Form No. 4...";
            txtOfficeDepartment.Value = "Office/Department : <b>" + OfficeNamevar + "</b>";
            this.DataSource = dt;
        }
    }
}