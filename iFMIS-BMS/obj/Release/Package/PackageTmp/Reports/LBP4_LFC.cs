namespace iFMIS_BMS.Reports
{
    using iFMIS_BMS.BusinessLayer.Connector;
    using iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation;
    using iFMIS_BMS.BusinessLayer.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for LBP4_LFC.
    /// </summary>
    public partial class LBP4_LFC : Telerik.Reporting.Report
    {
        public static string OfficeHead, Designation;
        public static int OfficeIDVar;
        string OfficeNamevar;
        public static string MyFormat(double num)
        {
            string format = "{0:N2}";
            if (num < 0)
            {
                return  "("+ string.Format(format, Math.Abs(num)) + ")";
            }
            else
            {
                return string.Format(format, Math.Abs(num));
            }
            
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

        public LBP4_LFC(int OfficeID, string OfficeName, int Year, int IncludeProposed, string ComputerIP)
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
            dt.Columns.Add("sgCurrent");//dr[4]
            dt.Columns.Add("StepCurrent");//dr[5]
            dt.Columns.Add("StepNew");//dr[6]
            dt.Columns.Add("DateOfAppointment");//dr[7]
            dt.Columns.Add("AmountCurrent");//dr[8]
            dt.Columns.Add("AmountProposed");//dr[9]
            dt.Columns.Add("Increase");//dr[10]
            dt.Columns.Add("OldRate");//dr[11]
            dt.Columns.Add("NewRate");//dr[12]
            dt.Columns.Add("ProposedAnnual");//dr[13]
            dt.Columns.Add("OfficeTotal");//dr[14]
            dt.Columns.Add("AmountProposedWithStep");//dr[15]
            dt.Columns.Add("IncreaseWithStep");//dr[16]
            dt.Columns.Add("NewRateWithStep");//dr[17]
            dt.Columns.Add("Indicator");//dr[18]
            dt.Columns.Add("StepDate");//dr[19]
            dt.Columns.Add("sgNew");//dr[20]
            dt.Columns.Add("FundType");//dr[21]
            dt.Columns.Add("OfficeName");//dr[22]

            if (OfficeID != 0)
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"sp_BMS_get_LBP4_Report_LFC " + Year + "," + OfficeID + "", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = reader.GetValue(1).ToString();  //Old Item no
                        dr[1] = reader.GetValue(2).ToString();  //New item no
                        dr[2] = reader.GetValue(3).ToString();  /// Divname
                        dr[3] = reader.GetValue(4).ToString() + Environment.NewLine + reader.GetValue(5).ToString();// Position and Name
                        dr[4] = reader.GetValue(6).ToString();  //SG
                        dr[5] = reader.GetValue(7).ToString();  //StepCurrent
                        dr[6] = reader.GetValue(8).ToString();  //StepNew
                        dr[7] = reader.GetValue(9).ToString();  //Date of Appointment
                        dr[8] = Convert.ToDouble(reader.GetValue(10));  //Current Amount
                        dr[9] = Convert.ToDouble(reader.GetValue(11));  //proposed Amount
                        dr[10] = Convert.ToDouble(reader.GetValue(13)); // Increase
                        dr[11] = Convert.ToDouble(reader.GetValue(16)); //  OldRate
                        dr[12] = Convert.ToDouble(reader.GetValue(17)); //  NewRate
                        dr[13] = Convert.ToDouble(reader.GetValue(15)); // Proposed Annual
                        dr[14] = "Total " + OfficeNamevar;              // Office fotter total
                        dr[15] = Convert.ToDouble(reader.GetValue(12)); //  Proposed Amount With Step
                        dr[16] = Convert.ToDouble(reader.GetValue(14)); // Increase With Step
                        dr[17] = Convert.ToDouble(reader.GetValue(18)); // New Rate With Step
                        dr[18] = reader.GetValue(19).ToString();        // Indicator
                        try
                        {
                            dr[19] = reader.GetValue(20).ToString();        // StepDate
                        }
                        catch (Exception)
                        {
                            dr[19] = "";        // StepDate
                        }
                        dr[20] = reader.GetValue(21).ToString(); //SgNew
                        dr[21] = 0; //FundType
                        dr[22] = 0; //OfficeName


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
                                                        + " inner JOIN pmis.dbo.RefsPositions as c on c.PositionCOde = a.PositionID where a.ActionCode != 0 and a.OfficeID=" + OfficeID + "and a.Yearof = " + Year + " order by c.sg desc", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            DataRow dr = dt.NewRow();
                            dr[0] = "";  //Old Item no
                            dr[1] = "";  //New item no
                            dr[2] = reader.GetValue(1).ToString();  /// Divname
                            dr[3] = reader.GetValue(2).ToString() + Environment.NewLine + "(For Funding)";// Position and Name
                            dr[4] = reader.GetValue(6).ToString();  //SG
                            dr[5] = "1";  //Step
                            dr[6] = "1";  //StepNew
                            dr[7] = reader.GetDateTime(3).ToShortDateString();  //Date of Appointment
                            dr[8] = Convert.ToDouble(0);  //Current Amount
                            dr[9] = Convert.ToDouble(reader.GetValue(5));  //proposed Amount
                            dr[10] = Convert.ToDouble(reader.GetValue(5)); // Increase
                            dr[11] = Convert.ToDouble(0); //  OldRate
                            dr[12] = Convert.ToDouble(reader.GetValue(4)); //  NewRate
                            dr[13] = Convert.ToDouble(reader.GetValue(5)); // Proposed Annual
                            dr[14] = "Total " + OfficeNamevar;              // Office fotter total
                            dr[15] = Convert.ToDouble(0); //  Proposed Amount With Step
                            dr[16] = Convert.ToDouble(0); // Increase With Step
                            dr[17] = Convert.ToDouble(0); // New Rate With Step
                            dr[18] = 1;        // Indicator
                            dr[19] = "";        // StepDate
                            dr[21] = 0; //FundType
                            dr[22] = 0; //OfficeName
                            dt.Rows.Add(dr);
                        }
                    }
                }
                txtOfficeDepartment.Value = "Office/Department : <b>" + OfficeNamevar + "</b>";
            }
            else
            {
                List<OfficesModel> OfficesList = new List<OfficesModel>();
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select OfficeID, OfficeName + ' (' + REPLACE(OfficeAbbrivation, ' ', '') + ')', 
                                                    Case when OfficeID in(37,38,41) then 'Economic Enterprise' else 'General Fund' 
                                                    end from tbl_R_BMSOffices where PMISOfficeID != 0 ORDER BY OfficeName", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        OfficesModel Office = new OfficesModel();
                        Office.OfficeID = reader.GetValue(0).ToString();
                        Office.OfficeName = reader.GetValue(1).ToString();
                        Office.FundType = reader.GetValue(2).ToString();

                        OfficesList.Add(Office);
                    }
                }
                foreach (var item in OfficesList)
                {
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"sp_BMS_get_LBP4_Report_LFC " + Year + "," + item.OfficeID + "", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();                        
                        while (reader.Read())
                        {
                            DataRow dr = dt.NewRow();
                            dr[0] = reader.GetValue(1).ToString();  //Old Item no
                            dr[1] = reader.GetValue(2).ToString();  //New item no
                            dr[2] = reader.GetValue(3).ToString();  /// Divname
                            dr[3] = reader.GetValue(4).ToString() + Environment.NewLine + reader.GetValue(5).ToString();// Position and Name
                            dr[4] = reader.GetValue(6).ToString();  //SG
                            dr[5] = reader.GetValue(7).ToString();  //StepCurrent
                            dr[6] = reader.GetValue(8).ToString();  //StepNew
                            dr[7] = reader.GetValue(9).ToString();  //Date of Appointment
                            dr[8] = Convert.ToDouble(reader.GetValue(10));  //Current Amount
                            dr[9] = Convert.ToDouble(reader.GetValue(11));  //proposed Amount
                            dr[10] = Convert.ToDouble(reader.GetValue(13)); // Increase
                            dr[11] = Convert.ToDouble(reader.GetValue(16)); //  OldRate
                            dr[12] = Convert.ToDouble(reader.GetValue(17)); //  NewRate
                            dr[13] = Convert.ToDouble(reader.GetValue(15)); // Proposed Annual
                            dr[14] = "Total " + OfficeNamevar;              // Office fotter total
                            dr[15] = Convert.ToDouble(reader.GetValue(12)); //  Proposed Amount With Step
                            dr[16] = Convert.ToDouble(reader.GetValue(14)); // Increase With Step
                            dr[17] = Convert.ToDouble(reader.GetValue(18)); // New Rate With Step
                            dr[18] = reader.GetValue(19).ToString();        // Indicator
                            try
                            {
                                dr[19] = reader.GetValue(20).ToString();        // StepDate
                            }
                            catch (Exception)
                            {
                                dr[19] = "";        // StepDate
                            }
                            dr[20] = reader.GetValue(21).ToString(); //SgNew
                            dr[21] = item.FundType; //FundType
                            dr[22] = item.OfficeName; //OfficeName


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
                                                            + " inner JOIN pmis.dbo.RefsPositions as c on c.PositionCOde = a.PositionID where a.ActionCode != 0 and a.OfficeID=" + item.OfficeID + "and a.Yearof = " + Year+ " order by c.sg desc", con);
                            con.Open();
                            SqlDataReader reader = com.ExecuteReader();
                            while (reader.Read())
                            {
                                DataRow dr = dt.NewRow();
                                dr[0] = "";  //Old Item no
                                dr[1] = "";  //New item no
                                dr[2] = reader.GetValue(1).ToString();  /// Divname
                                dr[3] = reader.GetValue(2).ToString() + Environment.NewLine + "(For Funding)";// Position and Name
                                dr[4] = reader.GetValue(6).ToString();  //SG
                                dr[5] = "1";  //Step
                                dr[6] = "1";  //StepNew
                                dr[7] = reader.GetDateTime(3).ToShortDateString();  //Date of Appointment
                                dr[8] = Convert.ToDouble(0);  //Current Amount
                                dr[9] = Convert.ToDouble(reader.GetValue(5));  //proposed Amount
                                dr[10] = Convert.ToDouble(reader.GetValue(5)); // Increase
                                dr[11] = Convert.ToDouble(0); //  OldRate
                                dr[12] = Convert.ToDouble(reader.GetValue(4)); //  NewRate
                                dr[13] = Convert.ToDouble(reader.GetValue(5)); // Proposed Annual
                                dr[14] = "Total " + OfficeNamevar;              // Office fotter total
                                dr[15] = Convert.ToDouble(0); //  Proposed Amount With Step
                                dr[16] = Convert.ToDouble(0); // Increase With Step
                                dr[17] = Convert.ToDouble(0); // New Rate With Step
                                dr[18] = 1;        // Indicator
                                dr[19] = "";        // StepDate
                                dr[21] = item.FundType; //FundType
                                dr[22] = item.OfficeName; //OfficeName
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                }
                txtOfficeDepartment.Value = "All Offices";
            }

            this.DataSource = dt;
            txt1stTranche.Value = "Proposed Salary Effective" + Environment.NewLine + "January 1, " + (Year - 1);
            txt2ndTranche.Value = "Proposed Salary Effective" + Environment.NewLine + "January 1, " + Year;
            txtYearFooter.Value = "Annual Budget CY"+Year+"...LBP Form No. 4...";
            txtYear.Value = "CY " + Year;
            
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
            txtOfficehead.Value = OfficeID == 0 ? PrintedBy : GlobalFunctions.getOfficeHead(OfficeID);
            txtDesignation.Value = OfficeID == 0 ? PrintedByPosition : GlobalFunctions.getOfficeHeadDesignation(OfficeID);
            txtBudgetHead.Value = GlobalFunctions.getOfficeHead(BudgetOfficeID);
            txtBudgetHeadDesignation.Value = GlobalFunctions.getOfficeHeadDesignation(BudgetOfficeID);
            txtGovernor.Value = GlobalFunctions.getOfficeHead(GovernorsOfficeID);
            txtGovernorDesignation.Value = GlobalFunctions.getOfficeHeadDesignation(GovernorsOfficeID);
            QRCode.Value = GlobalFunctions.QRCodeValue(PrintedBy, ComputerIP);
        }
    }
}