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
    using System.Configuration;
    using System.Linq;
    using iFMIS_BMS.Classes;

    public partial class LBP2NewConsolidated_Original81822 : Telerik.Reporting.Report
    {
        public string getOfficeName(int OfficeID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Select OfficeName From tbl_R_BMSOffices Where OfficeID = " + OfficeID + "", con);
                    con.Open();
                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string getSignatory(int OfficeID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select OfficeHead FROM pmis.dbo.OfficeDescription WHERE OfficeID =
                                                      (select b.PMISOfficeID from dbo.tbl_R_BMSOffices as b where b.OfficeID = " + OfficeID + ") ", con);
                    con.Open();
                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
        public LBP2NewConsolidated_Original81822(int OfficeID, int YearOf, int ReportTypeID, int includeLBP1, int pagnoid,long eid,int includezero,int reloadlbp2,int sectoral)
        {
            InitializeComponent();
            OfficeAdmin_Layer OfficeAdmin_Layer = new OfficeAdmin_Layer();
            var ReportType = "";
            var OldDataTableCount = 0;
            //var pagestemp = "";
            DataTable dt = new DataTable();
            if (OfficeID == 0)
            {
                DataTable OfficeIDList = new DataTable();
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    if (sectoral == 1)
                    {
                        SqlCommand com = new SqlCommand(@"select a.OfficeID,a.OfficeName,a.fundID,b.FundName from tbl_R_BMSOffices as a 
                                                    LEFT JOIN tbl_R_BMSFunds as b on b.FundCode = a.FundID
                                                    where isnull(a.PMISOfficeID,0) != 0 and a.fundid <> 201 and a.officeid <> 37 and a.officeid <> 41 and a.officeid <> 38 and a.officeid <> 43 order by a.SectorID,cast(isnull(a.OrderNo,999999) as integer)", con);
                        con.Open();
                        OfficeIDList.Load(com.ExecuteReader());
                    }
                    else
                    {
                        SqlCommand com = new SqlCommand(@"select a.OfficeID,a.OfficeName,a.fundID,b.FundName from tbl_R_BMSOffices as a 
                                                    LEFT JOIN tbl_R_BMSFunds as b on b.FundCode = a.FundID
                                                    where isnull(a.PMISOfficeID,0) != 0 and a.fundid <> 201 and a.officeid <> 37 and a.officeid <> 41 and a.officeid <> 38 and a.officeid <> 43 order by  cast(isnull(a.OrderNo,999999) as integer)", con);
                        con.Open();
                        OfficeIDList.Load(com.ExecuteReader());
                    }
                }
                dt.Columns.Add("OfficeName");
                dt.Columns.Add("OfficeID");
                dt.Columns.Add("FundTypeID");
                dt.Columns.Add("FundType");
                
                    ReportType = ReportTypeID == 2 ? "Proposed" : "Consolidated";
                    for (int x = 0; x < OfficeIDList.Rows.Count; x++)
                    {
                        using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        {
                            SqlCommand com = new SqlCommand(@"sp_BMS_Get_LBP2Report " + Convert.ToInt32(OfficeIDList.Rows[x][0]) + ", " + YearOf + ", " + ReportTypeID + "," + eid + "," + includezero + ","+ reloadlbp2 + "", con);
                            con.Open();
                            com.CommandTimeout = 0;
                            dt.Load(com.ExecuteReader());
                            //var OfficeName = getOfficeName(Convert.ToInt32(OfficeIDList.Rows[x][0]));
                            for (int i = OldDataTableCount; i < dt.Rows.Count; i++)
                            {
                                dt.Rows[i].SetField("OfficeID", Convert.ToInt32(OfficeIDList.Rows[x][0]));
                                dt.Rows[i].SetField("OfficeName", OfficeIDList.Rows[x][1].ToString());
                                dt.Rows[i].SetField("FundTypeID", Convert.ToInt32(OfficeIDList.Rows[x][2]));
                                dt.Rows[i].SetField("FundType", OfficeIDList.Rows[x][3].ToString());

                            }
                            OldDataTableCount = dt.Rows.Count;
                    }
                       
                }
            }
            else
            {
                if (ReportTypeID == 1)
                {
                    ReportType = "Prepared";
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"sp_BMS_Get_LBP2Report "+ OfficeID +", "+ YearOf +", " + ReportTypeID + "", con);
                        con.Open();
                        com.CommandTimeout = 0;
                        dt.Load(com.ExecuteReader());
                        dt.Columns[10].ReadOnly = false;
                        dt.Columns.Add("OfficeID");
                        dt.Columns.Add("OfficeName");
                        dt.Columns.Add("FundTypeID");
                        dt.Columns.Add("FundType");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i].SetField("OfficeID", OfficeID);
                            dt.Rows[i].SetField("OfficeName", getOfficeName(OfficeID));
                            dt.Rows[i].SetField("BudgetYearAmount", Convert.ToDouble(OfficeAdmin_Layer.setProposalAllotedAmount(Convert.ToInt32(dt.Rows[i]["AccountCode"]), OfficeID, YearOf, Convert.ToInt32(dt.Rows[i]["OOEID"]), Convert.ToInt32(dt.Rows[i]["ProgramID"]), Convert.ToInt32(dt.Rows[i]["AccountID"]))));
                        }
                    }
                }
                else
                {
                    //DataTable OfficeIDList = new DataTable();

                    //using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    //{
                    //    SqlCommand com = new SqlCommand(@"select a.OfficeID,a.OfficeName,a.fundID,b.FundName from tbl_R_BMSOffices as a 
                    //                                LEFT JOIN tbl_R_BMSFunds as b on b.FundCode = a.FundID
                    //                                where a.officeid = "+ OfficeID + " order by a.SectorID, isnull(a.OrderNo,999999)", con);
                    //    con.Open();
                    //    OfficeIDList.Load(com.ExecuteReader());
                    //}

                    //DataTable _dt = new DataTable();
                    //string _sqlQuery = "select a.OfficeID,a.OfficeName,a.fundID,b.FundName from tbl_R_BMSOffices as a "+
                    //                    "     LEFT JOIN tbl_R_BMSFunds as b on b.FundCode = a.FundID "+
                    //                    "     where a.officeid = "+ OfficeID + " order by a.fundID, isnull(a.OrderNo,999999)";
                    //_dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery).Tables[0];
                    
                    ReportType = ReportTypeID == 2 ? "Proposed" : "Consolidated";
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"sp_BMS_Get_LBP2Report " + OfficeID + ", " + YearOf + ", " + ReportTypeID + "," + eid + ","+ includezero + "," + reloadlbp2 + "", con);
                        con.Open();
                        com.CommandTimeout = 0;
                        dt.Load(com.ExecuteReader());
                        dt.Columns.Add("OfficeID");
                        dt.Columns.Add("OfficeName");
                        dt.Columns.Add("FundTypeID");
                        dt.Columns.Add("FundType");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i].SetField("OfficeID", OfficeID);
                            dt.Rows[i].SetField("OfficeName", getOfficeName(OfficeID));
                            //dt.Rows[i].SetField("FundTypeID", Convert.ToInt32(_dt.Rows[0]["fundID"].ToString()));
                            //dt.Rows[i].SetField("FundType", _dt.Rows[0]["FundName"].ToString());

                        }
                       

                    }
                    //using (SqlConnection con2 = new SqlConnection(Common.MyConn()))
                    //{
                    //    SqlCommand com2 = new SqlCommand(@"sp_BMS_Get_LBP2Report_Total " + OfficeID + ", " + YearOf + ", " + ReportTypeID + "," + eid + "", con2);
                    //    con2.Open();
                    //    dt.Load(com2.ExecuteReader());
                    //}
                }
            }
            if (includeLBP1 == 0) {
                textBox2.Value ="=PageNumber + "+ pagnoid +" -1 ";
                //"='Page ' + PageNumber + ' of ' + PageCount";
                //"=PageNumber + " + pagnoid +" -1";

            }

            if (OfficeID == 43)
            {
                DataTable dt2 = new DataTable();
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    //SqlCommand com2 = new SqlCommand(@"sp_BMS_Get_LBP2Report_Total " + OfficeID + ", " + YearOf + ", " + ReportTypeID + "", con);
                    //con.Open();
                    //dt2.Load(com2.ExecuteReader());

                    DataTable _dt = new DataTable();
                    string _sqlQuery = "SELECT "+
                                         " format(isnull(sum([PastYearApproved]), 0),'00,000.00') " +
                                         "  ,format(isnull(sum([Current_Year_Obligated]), 0),'00,000.00') " +
                                         "  ,format(isnull(sum([Current_Year_Estimate]), 0),'00,000.00') " +
                                         "  ,format(isnull(sum([CurrentYearApproved]), 0),'00,000.00') " +
                                         "  ,format(isnull(sum([BudgetYearAmount]), 0),'00,000.00') " +
                                         "  ,format(isnull(sum([BudgetYearAmount]), 0) - isnull(sum([CurrentYearApproved]),0),'00,000.00') as [difference] " +
                                      "FROM[IFMIS].[dbo].[tbl_T_BMSLBP2_total] " +
                                      "where userid = "+ eid + " and proyear="+ YearOf  + "";
                    _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery).Tables[0];

                    textBox131.Value ="Total Appropriations - General Fund Proper";
                    textBox131.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox131.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox132.Value = "Total Appropriations - Provincial Government of Agusan del Sur";
                    textBox132.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox132.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox133.Value = _dt.Rows[0][0].ToString();
                    textBox133.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox133.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox134.Value = _dt.Rows[0][0].ToString();
                    textBox134.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox134.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
                    // textBox134.Size = textBox132.Size.Height.ToString();
                    textBox135.Value = _dt.Rows[0][1].ToString();
                    textBox135.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox135.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox136.Value = _dt.Rows[0][1].ToString();
                    textBox136.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox136.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox137.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox137.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox138.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox138.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox139.Value = _dt.Rows[0][2].ToString();
                    textBox139.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox139.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox140.Value = _dt.Rows[0][2].ToString();
                    textBox140.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox140.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox141.Value = _dt.Rows[0][3].ToString();
                    textBox141.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox141.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox142.Value = _dt.Rows[0][3].ToString();
                    textBox142.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox142.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox143.Value = _dt.Rows[0][4].ToString();
                    textBox143.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox143.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox144.Value = _dt.Rows[0][4].ToString();
                    textBox144.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox144.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox145.Value = _dt.Rows[0][5].ToString();
                    textBox145.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox145.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox145.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox146.Value = _dt.Rows[0][5].ToString();
                    textBox146.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox146.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
                    textBox146.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;


                    // this.table1.DataSource = _dt;
                }
            }

            textBox1.Value = "Annual Budget CY " + YearOf + " - LBP Form No. 2";
            this.ReportParameters["Signatory1"].Value = getSignatory(OfficeID);
            this.ReportParameters["Signatory2"].Value = getSignatory(21);
            this.ReportParameters["Signatory3"].Value = GlobalFunctions.getReportTextBoxValue(2, 3);
            this.ReportParameters["ReportType"].Value = ReportType + " CY " + YearOf +"";
            this.ReportParameters["LGU"].Value = "Provincial Government of Agusan del Sur";
            this.ReportParameters["OfficeID"].Value = OfficeID;
            textBox96.Value = "Current Year (Estimate CY " + (YearOf - 1) + ")";
            textBox110.Value = "Current Year (Estimate CY " + (YearOf - 1) + ")";
            textBox52.Value = "Past Year (CY "+ (YearOf - 2) + ")";
            textBox147.Value = "Past Year (CY " + (YearOf - 2) + ")";
            textBox3.Value = GlobalFunctions.getReportTextBoxValue(2, 1);
            htmlTextBox1.Value = GlobalFunctions.getReportTextBoxValue(2, 2);
            this.table2.DataSource = dt;

            barcode1.Value = FUNCTION.GeneratePISControl();
            DataTable reportlog = new DataTable();
            using (SqlConnection conUser = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@" exec [sp_BMS_ReportLog] 14," + eid + ",'LBPF2','"+ barcode1.Value  +"'", conUser);
                conUser.Open();
                reportlog.Load(com.ExecuteReader());

            }

        }
    }
}