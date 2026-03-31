namespace iFMIS_BMS.Reports
{
    using iFMIS_BMS.BusinessLayer.Connector;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Linq;
    using System.Configuration;
    using System.Data.SqlTypes;
    using iFMIS_BMS.Base;
    using iFMIS_BMS.Classes;

    /// <summary>
    /// Summary description for LBEF.
    /// </summary>
    public partial class MAF : Telerik.Reporting.Report
    {
        public MAF(int? programid=0,int? accountID=0,int? yearof=0,string OfficeName="",int? OfficeID=0,int? mode=0,int? approverepHistory = 0,int? source=0,int? previewid=0)
        {
            
            InitializeComponent();


            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();

            DataTable _dt2 = new DataTable();

            if (approverepHistory != 0) {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    if (mode == 1) //supplemental
                    {
                        SqlCommand com = new SqlCommand(@"exec [sp_BMS_MAFXML_read] "+ approverepHistory + ","+ OfficeID + "", con);
                        con.Open();
                        dt2.Load(com.ExecuteReader());
                        this.table2.DataSource = dt2;
                    }
                    else if (mode >= 2) //realignment or reversion
                    {
                        SqlCommand com = new SqlCommand(@"exec [sp_BMS_MAFXML_read] " + approverepHistory + "," + OfficeID + "", con);
                        con.Open();
                        dt2.Load(com.ExecuteReader());
                        this.table1.DataSource = dt2;
                        Int32 reportdificient = 0;
                        reportdificient = Convert.ToInt32(approverepHistory) + 1;
                        using (SqlConnection con2 = new SqlConnection(Common.MyConn()))
                        {
                            SqlCommand com2 = new SqlCommand(@"exec [sp_BMS_MAFXML_read] " + reportdificient + " ," + OfficeID + "", con2);
                            con2.Open();
                            dt3.Load(com2.ExecuteReader());
                            this.table2.DataSource = dt3;
                        }
                    }
                }
                
                string _sqlQuery = "Select [mafno],[certification],format([datetime],'MM/dd/yyyy hh:mm:ss tt'),format([datetime],'MM/dd/yyyy'),a.[mode],a.[office] + ' ('+ rtrim(ltrim(b.[OfficeAbbrivation])) + ')' as office,[prepareby],[preparebyposition],[qrcode] from [IFMIS].[dbo].[tbl_T_BMSMAF_xml] as a left join " +
                                            "[IFMIS].[dbo].[tbl_R_BMSOffices] as b on b.[OfficeID]=a.[officeid] where [mafid]=" + approverepHistory + "";
                dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    textBox6.Value = "MODIFICATION ADVICE FORM (MAF) NO. " + dt.Rows[0][0].ToString();
                    textBox7.Value = dt.Rows[0][1].ToString();
                    textBox23.Value = "Date Printed: " + dt.Rows[0][2].ToString();
                    textBox24.Value = dt.Rows[0][3].ToString();
                    txt_FundType.Value = "General Fund - Annual Budget CY " + yearof;
                    txt_Office.Value = dt.Rows[0][5].ToString();
                    textBox58.Value = dt.Rows[0][6].ToString();
                    textBox57.Value = dt.Rows[0][7].ToString();
                    bcode.Value = dt.Rows[0][8].ToString();
                    if (Convert.ToInt32(dt.Rows[0][4]) == 1)
                    {
                        checkBox1.FalseValue = 1; //supplemental
                        checkBox2.FalseValue = "";
                        checkBox3.FalseValue = "";
                    }
                    else if (Convert.ToInt32(dt.Rows[0][4]) == 2)
                    {
                        checkBox1.FalseValue = "";
                        checkBox2.FalseValue = "";
                        checkBox3.FalseValue = 1; //realignment

                    }
                    else if (Convert.ToInt32(dt.Rows[0][4]) == 3) //reversion
                    {
                        textBox63.Value = "for Reversion";
                        checkBox1.FalseValue = "";
                        checkBox2.FalseValue = 1;//reversion
                        checkBox3.FalseValue = "";

                    }
                }

            }
            else
            {
                string _sqlQuery2 = "exec sp_OfficeHeadPerOffice " + OfficeID + "," + Account.UserInfo.eid + "";
                _dt2 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisqldb"].ToString(), CommandType.Text, _sqlQuery2).Tables[0];

                textBox58.Value = _dt2.Rows[0][0].ToString();
                textBox57.Value = _dt2.Rows[0][1].ToString();

                bcode.Value = FUNCTION.GeneratePISControl();
                GlobalFunctions.QR_globalstr = bcode.Value;

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    if (mode == 1) //supplemental
                    {
                        SqlCommand com = new SqlCommand(@"exec sp_BMS_MAF " + programid + "," + accountID + "," + yearof + "," + OfficeID + "," + Account.UserInfo.eid + ",'" + bcode.Value + "','" + textBox58.Value + "','" + textBox57.Value + "'," + mode + ",0,"+ previewid + "", con);
                        con.Open();
                        dt2.Load(com.ExecuteReader());
                        this.table2.DataSource = dt2;
                    }
                    else if (mode >= 2) //realignment or reversion
                    {
                        //con.Close();

                        SqlCommand com = new SqlCommand(@"exec sp_BMS_MAF " + programid + "," + accountID + "," + yearof + "," + OfficeID + "," + Account.UserInfo.eid + ",'" + bcode.Value + "','" + textBox58.Value + "','" + textBox57.Value + "'," + mode + ","+ source + ", "+ previewid + "", con);
                        con.Open();
                        com.CommandTimeout = 0;
                        dt2.Load(com.ExecuteReader());
                        this.table1.DataSource = dt2;

                        using (SqlConnection con2 = new SqlConnection(Common.MyConn()))
                        {
                            SqlCommand com2 = new SqlCommand(@"exec sp_BMS_MAF " + programid + "," + accountID + "," + yearof + "," + OfficeID + "," + Account.UserInfo.eid + ",'" + bcode.Value + "','" + textBox58.Value + "','" + textBox57.Value + "'," + mode + ",2," + previewid + "", con2);
                            con2.Open();
                            com2.CommandTimeout = 0;
                            dt3.Load(com2.ExecuteReader());
                            this.table2.DataSource = dt3;
                        }
                    }
                    
                }
                //if (mode == 2)
                //{
                //    using (SqlConnection con2 = new SqlConnection(Common.MyConn()))
                //    {
                //        SqlCommand com2 = new SqlCommand(@"exec sp_BMS_MAF " + programid + "," + accountID + "," + yearof + "," + OfficeID + "," + Account.UserInfo.eid + ",'" + bcode.Value + "','" + textBox58.Value + "','" + textBox57.Value + "'," + mode + ",2", con2);
                //        con2.Open();
                //        dt3.Load(com2.ExecuteReader());
                //        this.table2.DataSource = dt3;
                //    }
                //}
                txt_Office.Value = OfficeName;

                string _sqlQuery = "Select [mafno],[certification],format([datetime],'MM/dd/yyyy hh:mm:ss tt'),format([datetime],'MM/dd/yyyy') from [IFMIS].[dbo].[tbl_T_BMSMAF_xml] where [officeid]=" + OfficeID + " and [qrcode]='" + bcode.Value + "'";
                dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    textBox6.Value = "MODIFICATION ADVICE FORM (MAF) NO. " + dt.Rows[0][0].ToString();
                    textBox7.Value = dt.Rows[0][1].ToString();
                    textBox23.Value = "Date Printed: " + dt.Rows[0][2].ToString();
                    textBox24.Value = dt.Rows[0][3].ToString();
                    txt_FundType.Value = "General Fund - Annual Budget CY " + yearof;
                }

                if (mode == 1)
                {
                    checkBox1.FalseValue = 1; //supplemental
                    checkBox2.FalseValue = "";
                    checkBox3.FalseValue = "";
                }
                else if (mode == 2)
                {
                    checkBox1.FalseValue = "";
                    checkBox2.FalseValue = "";
                    checkBox3.FalseValue = 1; //realignment

                }
                else if (mode == 3) //reversion
                {
                    textBox63.Value = "for Reversion";
                    checkBox1.FalseValue = "";
                    checkBox2.FalseValue = 1;//reversion
                    checkBox3.FalseValue = ""; 

                }
            }
            DataTable _dt4 = new DataTable();
            string _sqlQuery4 = "SELECT [lgu],[province],isnull([address],'') address FROM [IFMIS].[dbo].[tbl_R_BMSLgu] where actioncode=1";
            _dt4 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery4).Tables[0];
            if (_dt4.Rows.Count > 0)
            {
                textBox50.Value = _dt4.Rows[0][0].ToString();
                //textBox2.Value = _dt4.Rows[0][1].ToString();
                //textBox3.Value = _dt4.Rows[0][2].ToString();
                textBox48.Value = _dt4.Rows[0][0].ToString() + " - Making your task easier...";
            }
        }
    } 
}