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
    using iFMIS_BMS.Base;
    using iFMIS_BMS.Classes;
    using System.Configuration;

    /// <summary>
    /// Summary description for LBP2New.
    /// </summary>
    public partial class LBP2New : Telerik.Reporting.Report
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
                return "NO SIGNATORY FOUND";
            }
        }
        public LBP2New(int OfficeID, int YearOf,int ReportTypeID,long eid,int reloadlbp2)
        {
            InitializeComponent();
            OfficeAdmin_Layer OfficeAdmin_Layer = new OfficeAdmin_Layer();
            var ReportType = "";
            var OldDataTableCount = 0;
            DataTable dt = new DataTable();
            if (OfficeID == 0)
            {
                DataTable OfficeIDList = new DataTable();
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select a.OfficeID,a.OfficeName,a.fundID,b.FundName from tbl_R_BMSOffices as a 
                                                    LEFT JOIN tbl_R_BMSFunds as b on b.FundCode = a.FundID
                                                    where a.PMISOfficeID != 0 order by a.fundID, isnull(a.OrderNo,999999)", con);
                    con.Open();
                    OfficeIDList.Load(com.ExecuteReader());
                }
                dt.Columns.Add("OfficeName");
                dt.Columns.Add("OfficeID");
                dt.Columns.Add("FundTypeID");
                dt.Columns.Add("FundType");
                
                    ReportType = ReportTypeID == 2 ? "Proposed" : "Approved";
                    for (int x = 0; x < OfficeIDList.Rows.Count; x++)
                    {
                        using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        {
                            var Query = "";
                            if (ReportTypeID == 3)
                            {
                                Query = @"sp_BMS_Get_LBP2Report " + Convert.ToInt32(OfficeIDList.Rows[x][0]) + ", " + YearOf + ", 3," + eid + ",1," + reloadlbp2 + "";
                            }
                            else
                            {
                                Query = @"sp_BMS_Get_LBP2Report " + Convert.ToInt32(OfficeIDList.Rows[x][0]) + ", " + YearOf + ", " + ReportTypeID + ","+ eid + ",1," + reloadlbp2 + "";
                            }
                            
                            SqlCommand com = new SqlCommand(Query, con);
                            com.CommandTimeout = 0;
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
                        var Query = "";
                        if (ReportTypeID == 3)
                        {
                            Query = @"sp_BMS_Get_LBP2Report " + OfficeID + ", " + YearOf + ", 3," + eid + ",1," + reloadlbp2 + "";
                        }
                        else
                        {
                            Query = @"sp_BMS_Get_LBP2Report " + OfficeID + ", " + YearOf + ", " + ReportTypeID + "," + eid + ",1,1";
                        }
                        
                        SqlCommand com = new SqlCommand(Query, con);
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
                            dt.Rows[i].SetField("BudgetYearAmount", Convert.ToDouble(OfficeAdmin_Layer.setProposalAllotedAmount(Convert.ToInt32(dt.Rows[i]["AccountID"]), OfficeID, YearOf, Convert.ToInt32(dt.Rows[i]["OOEID"]), Convert.ToInt32(dt.Rows[i]["ProgramID"]), Convert.ToInt32(dt.Rows[i]["AccountID"]))));
                        }
                    }
                }
                else
                {
                    ReportType = ReportTypeID == 2 ? "Proposed" : "Approved";
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        var Query = "";
                        if (ReportTypeID == 3)
                        {
                            Query = @"sp_BMS_Get_LBP2Report " + OfficeID + ", " + YearOf + ", 3," + eid + ",1," + reloadlbp2 + "";
                        }
                        else
                        {
                            Query = @"sp_BMS_Get_LBP2Report " + OfficeID + ", " + YearOf + ", " + ReportTypeID + "," + eid + ",1,1";
                        }
                        SqlCommand com = new SqlCommand(Query, con);
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
                        }
                    }
                }
            }
            this.ReportParameters["Signatory1"].Value = getSignatory(OfficeID == 0 ? 21 : OfficeID);
            this.ReportParameters["Signatory2"].Value = GlobalFunctions.getReportTextBoxValue(4, 5);//getSignatory(21);
            this.ReportParameters["Signatory3"].Value = GlobalFunctions.getReportTextBoxValue(2,3);
            this.ReportParameters["ReportType"].Value = ReportType + ") CY " + YearOf;

            string _lgu = "";
            DataTable _tbllgu = new DataTable();
            _lgu = "SELECT [lgu] FROM [IFMIS].[dbo].[tbl_R_BMSLgu] where actioncode = 1";

            _tbllgu = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _lgu).Tables[0];
            if (_tbllgu.Rows.Count > 0)
            {
                this.ReportParameters["LGU"].Value = _tbllgu.Rows[0][0].ToString();
            }
            else
            {
                this.ReportParameters["LGU"].Value = "Provincial Government of Agusan del Sur";

            }
            //this.ReportParameters["LGU"].Value = "Province of Agusan del Sur";

            this.ReportParameters["OfficeID"].Value = OfficeID;
            textBox3.Value = GlobalFunctions.getReportTextBoxValue(2, 1);
        //    htmlTextBox1.Value = GlobalFunctions.getReportTextBoxValue(2, 2);
            this.table2.DataSource = dt;
            textBox33.Value = "Current Year (Estimate) CY " + (YearOf - 1);
            textBox48.Value = "Current Year (Estimate) CY " + (YearOf - 1);
            textBox30.Value = "Past Year(CY " + (YearOf - 2) + ")";
            textBox47.Value = "Past Year(CY " + (YearOf - 2) + ")";
            barcode1.Value = FUNCTION.GeneratePISControl();
            GlobalFunctions.QR_globalstr = barcode1.Value;

            DataTable reportlog = new DataTable();
            using (SqlConnection conUser = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@" exec [sp_BMS_ReportLog] 14," + Account.UserInfo.eid + ",'LBPF2','" + barcode1.Value + "'", conUser);
                conUser.Open();
                reportlog.Load(com.ExecuteReader());

            }

            DataTable reportlog2 = new DataTable();
            using (SqlConnection conUpdateRep = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec [sp_BMS_LBPReport_Update] "+ YearOf + ","+ OfficeID + ","+ ReportTypeID + ",'" + barcode1.Value + "'", conUpdateRep);
                conUpdateRep.Open();
                reportlog2.Load(com.ExecuteReader());

            }
            
        }
    }
}