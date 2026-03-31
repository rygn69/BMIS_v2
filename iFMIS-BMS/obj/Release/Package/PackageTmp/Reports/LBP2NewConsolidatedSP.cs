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

    public partial class LBP2NewConsolidatedSP : Telerik.Reporting.Report
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
        public LBP2NewConsolidatedSP(int OfficeID, int YearOf, int ReportTypeID, int includeLBP1, int pagnoid,int includezero,long eid,int reloadlbp2)
        {
            InitializeComponent();
            OfficeAdmin_Layer OfficeAdmin_Layer = new OfficeAdmin_Layer();
            var ReportType = "";
            var OldDataTableCount = 0;
            var pagestemp = "";
            DataTable dt = new DataTable();
            if (OfficeID == 0)
            {
                DataTable OfficeIDList = new DataTable();
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select a.OfficeID,a.OfficeName,a.fundID,b.FundName from tbl_R_BMSOffices as a 
                                                    LEFT JOIN tbl_R_BMSFunds as b on b.FundCode = a.FundID
                                                    where isnull(a.PMISOfficeID,0) != 0 and a.fundid <> 201 and a.officeid  not in (41,43,37,38,95,102,101,98,94,100,99,75,77) order by  cast(isnull(a.OrderNo,999999) as integer)", con);
                    con.Open();
                    OfficeIDList.Load(com.ExecuteReader());
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
                            SqlCommand com = new SqlCommand(@"sp_BMS_Get_LBP2Report_SP " + Convert.ToInt32(OfficeIDList.Rows[x][0]) + ", " + YearOf + ", " + ReportTypeID + ","+ includezero + ","+ eid + "," + reloadlbp2  + "", con);
                            con.Open();
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
                        SqlCommand com = new SqlCommand(@"sp_BMS_Get_LBP2Report_SP " + OfficeID +", "+ YearOf +", " + ReportTypeID + "," + includezero + "," + eid + "," + reloadlbp2 + "", con);
                        con.Open();
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
                    ReportType = ReportTypeID == 2 ? "Proposed" : "Consolidated";
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"sp_BMS_Get_LBP2Report_SP " + OfficeID + ", " + YearOf + ", " + ReportTypeID + "," + includezero + "," + eid + "," + reloadlbp2 + "", con);
                        con.Open();
                        dt.Load(com.ExecuteReader());
                        dt.Columns.Add("OfficeID");
                        dt.Columns.Add("OfficeName");
                        dt.Columns.Add("FundTypeID");
                        dt.Columns.Add("FundType");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i].SetField("OfficeID", OfficeID);
                            dt.Rows[i].SetField("OfficeName", getOfficeName(OfficeID));
                            
                        }

                     //   textBox2.Value = "=PageNumber + 1 + ' of ' + PageCount";
                        //textBox2.Value = "=PageNumber + ' of ' + PageCount";

                    }
                }
            }
            if (includeLBP1 == 0) {
                textBox2.Value ="=PageNumber + "+ pagnoid +" -1 ";
                //"='Page ' + PageNumber + ' of ' + PageCount";
                //"=PageNumber + " + pagnoid +" -1";

            }
            textBox1.Value = "Annual Budget CY "+ YearOf + " - LBP Form No. 2";
            this.ReportParameters["Signatory1"].Value = getSignatory(OfficeID == 0 ? 21 : OfficeID);
            this.ReportParameters["Signatory2"].Value = getSignatory(21);
            this.ReportParameters["Signatory3"].Value = GlobalFunctions.getReportTextBoxValue(2,3);
            this.ReportParameters["ReportType"].Value = ReportType;
            this.ReportParameters["LGU"].Value = "Provincial Government of Agusan del Sur";
            this.ReportParameters["OfficeID"].Value = OfficeID;
            textBox3.Value = GlobalFunctions.getReportTextBoxValue(2, 1);
            htmlTextBox1.Value = GlobalFunctions.getReportTextBoxValue(2, 2);
            this.table2.DataSource = dt;

        }
    }
}