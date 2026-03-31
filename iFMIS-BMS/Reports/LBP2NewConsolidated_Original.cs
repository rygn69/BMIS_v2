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

    public partial class LBP2NewConsolidated_Original : Telerik.Reporting.Report
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
        public LBP2NewConsolidated_Original(int OfficeID, int YearOf, int ReportTypeID, int includeLBP1, int pagnoid)
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
                    SqlCommand com = new SqlCommand(@"select a.OfficeID,a.OfficeName,a.fundID,b.FundName from tbl_R_BMSOffices as a 
                                                    LEFT JOIN tbl_R_BMSFunds as b on b.FundCode = a.FundID
                                                    where isnull(a.PMISOfficeID,0) != 0 and a.fundid <> 201 and a.officeid <> 37 and a.officeid <> 41 and a.officeid <> 38 order by a.fundID, isnull(a.OrderNo,999999)", con);
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
                            SqlCommand com = new SqlCommand(@"sp_BMS_Get_LBP2Report " + Convert.ToInt32(OfficeIDList.Rows[x][0]) + ", " + YearOf + ", " + ReportTypeID + "", con);
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
                        SqlCommand com = new SqlCommand(@"sp_BMS_Get_LBP2Report "+ OfficeID +", "+ YearOf +", " + ReportTypeID + "", con);
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
                    DataTable OfficeIDList = new DataTable();

                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"select a.OfficeID,a.OfficeName,a.fundID,b.FundName from tbl_R_BMSOffices as a 
                                                    LEFT JOIN tbl_R_BMSFunds as b on b.FundCode = a.FundID
                                                    where a.officeid = "+ OfficeID + " order by a.fundID, isnull(a.OrderNo,999999)", con);
                        con.Open();
                        OfficeIDList.Load(com.ExecuteReader());
                    }

                    //DataTable _dt = new DataTable();
                    //string _sqlQuery = "select a.OfficeID,a.OfficeName,a.fundID,b.FundName from tbl_R_BMSOffices as a "+
                    //                    "     LEFT JOIN tbl_R_BMSFunds as b on b.FundCode = a.FundID "+
                    //                    "     where a.officeid = "+ OfficeID + " order by a.fundID, isnull(a.OrderNo,999999)";
                    //_dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery).Tables[0];
                    
                    ReportType = ReportTypeID == 2 ? "Proposed" : "Consolidated";
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"sp_BMS_Get_LBP2Report " + OfficeID + ", " + YearOf + ", " + ReportTypeID + "", con);
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
                            //dt.Rows[i].SetField("FundTypeID", Convert.ToInt32(_dt.Rows[0]["fundID"].ToString()));
                            //dt.Rows[i].SetField("FundType", _dt.Rows[0]["FundName"].ToString());

                        }
                    }
                }
            }
            if (includeLBP1 == 0) {
                textBox2.Value ="=PageNumber + "+ pagnoid +" -1 ";
                //"='Page ' + PageNumber + ' of ' + PageCount";
                //"=PageNumber + " + pagnoid +" -1";

            }
            textBox1.Value = "Annual Budget CY " + YearOf + " - LBP Form No. 2";
            this.ReportParameters["Signatory1"].Value = getSignatory(OfficeID);
            this.ReportParameters["Signatory2"].Value = getSignatory(21);
            this.ReportParameters["Signatory3"].Value = GlobalFunctions.getReportTextBoxValue(2, 3);
            this.ReportParameters["ReportType"].Value = ReportType;
            this.ReportParameters["LGU"].Value = "Provincial Government of Agusan del Sur";
            this.ReportParameters["OfficeID"].Value = OfficeID;
            textBox3.Value = GlobalFunctions.getReportTextBoxValue(2, 1);
            htmlTextBox1.Value = GlobalFunctions.getReportTextBoxValue(2, 2);
            this.table2.DataSource = dt;

        }
    }
}