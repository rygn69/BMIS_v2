namespace iFMIS_BMS.Reports
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Data;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using iFMIS_BMS.BusinessLayer.Connector;
    using System.Linq;
    using iFMIS_BMS.Base;
    using iFMIS_BMS.Classes;
    using System.Configuration;

    /// <summary>
    /// Summary description for WFP.
    /// </summary>
    public partial class WFPNew_orig : Telerik.Reporting.Report
    {
        public WFPNew_orig(int? year=0, int? office=0, int? project_id=0, int? fundsource=0,string OfficeName="",string ProjectName="", string funddescription="",int? repHistory=0,int? prepby=0,int? printstatus=0,int? projectaip=0,string fundsourcename="",int? accountID=0,string accountname="",string  municipal="",string barangay="",string ooeclass="",int? pgas_loc=0,int? approveprint=0)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            if (repHistory == 0)
            {
                DataTable _dt3 = new DataTable();
                string _sqlQuery3 = "Select format(getdate(),'M/dd/yyyy hh:mm:ss tt') as ServerDate";
                _dt3 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisqldb"].ToString(), CommandType.Text, _sqlQuery3).Tables[0];

                textBox84.Value = _dt3.Rows[0]["ServerDate"].ToString();

                qrcode.Value = FUNCTION.GeneratePISControl();
                GlobalFunctions.QR_globalstr = qrcode.Value;
                var query = @"exec ifmis.dbo.sp_bms_WFP_report " + office + ","+ year + "," + Account.UserInfo.eid + ",'"+ qrcode.Value + "',"+ prepby + ",'CYNTHIA P. LUMANTA, CE, EnP','Acting Provincial Government Department Head','JAVE NHORIEL N. BORDAJE, CPA','Provincial Budget Officer','SANTIAGO B. CANE, JR.','Provincial Governor','" + textBox84.Value + "',"+ printstatus + ","+ accountID + ",'"+ municipal +"','"+barangay +"',"+ pgas_loc + ","+ approveprint + "" ;
                DataTable dt1 = new DataTable();
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["adodb"].ConnectionString))
                {
                    SqlCommand com = new SqlCommand(query, con);
                    //com.Parameters.Add(new SqlParameter("@Office", office));
                    //com.Parameters.Add(new SqlParameter("@Year", year));
                    //com.Parameters.Add(new SqlParameter("@Project", project_id));
                    //com.Parameters.Add(new SqlParameter("@FundSource", fundsource));
                    com.CommandTimeout = 0;
                    con.Open();
                    dt1.Load(com.ExecuteReader());

                }
                table2.DataSource = dt1;

                DataTable _dtSig = new DataTable();
                DataTable _dtSig2 = new DataTable();
                string _sqlQuery = "";
                string _sqlQuery2 = "";
                if (prepby != 0)
                {
                    _sqlQuery = "select a.EmpNameFull, a.Position,OfficeHead,OfficeDesignation,a.eid,b.OfficeHeadID from [pmis].[dbo].[vwMergeAllEmployee] as a left join " +
                                   "pmis.dbo.OfficeDescription as b on b.OfficeID = a.Department " +
                                   "where eid = " + prepby + "";
                }
                else
                {
                    _sqlQuery = "select a.EmpNameFull, a.Position,OfficeHead,OfficeDesignation,a.eid,b.OfficeHeadID from [pmis].[dbo].[vwMergeAllEmployee] as a left join " +
                                    "pmis.dbo.OfficeDescription as b on b.OfficeID = a.Department "+
                                    "where eid in (Select top 1 isnull(userid, 0) from[IFMIS].[dbo].[tbl_T_BMSWFP_DFPPT] as a  where a.officeid = "+ office + " and accountid = "+ accountID + " and tyear = "+ year + " and actioncode = 1 and isnull(submit,0)= 1) ";
                }

                //string _sqlQuery = "select a.EmpNameFull, a.Position,OfficeHead,OfficeDesignation from [pmis].[dbo].[vwMergeAllEmployee] as a left join "+ 
                //                    "pmis.dbo.OfficeDescription as b on b.OfficeID = a.Department "+ 
                //                    "where eid = "+ prepby  + "";
                _dtSig = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["fmisqldb"].ToString(), CommandType.Text, _sqlQuery).Tables[0];
                if (_dtSig.Rows.Count > 0)
                {
                    //if (prepby != 0)
                    //{
                    textBox12.Value = _dtSig.Rows[0]["EmpNameFull"].ToString();
                    GlobalFunctions.wfppreparer_sign = Convert.ToInt32(_dtSig.Rows[0]["eid"].ToString());
                    GlobalFunctions.wfpdepthead_sign = Convert.ToInt32(_dtSig.Rows[0]["OfficeHeadID"].ToString());
                    //       textBox13.Value = _dtSig.Rows[0]["Position"].ToString();
                    //}
                    textBox67.Value = _dtSig.Rows[0]["OfficeHead"].ToString();
                    textBox66.Value = _dtSig.Rows[0]["OfficeDesignation"].ToString();
                }
                else
                {
                    _sqlQuery2 = "select a.EmpNameFull, a.Position,OfficeHead,OfficeDesignation,a.eid,b.OfficeHeadID from [pmis].[dbo].[vwMergeAllEmployee] as a left join " +
                                    "pmis.dbo.OfficeDescription as b on b.OfficeID = a.Department " +
                                    "where eid in (Select top 1 isnull(userid, 0) from[IFMIS].[dbo].[tbl_T_BMSPPMP] as a where a.OfficeID = " + office + " and accountid = " + accountID + " and TransactionYear = " + year + " and actioncode = 1 and isnull(submit,0)= 1)";

                    _dtSig2 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["fmisqldb"].ToString(), CommandType.Text, _sqlQuery2).Tables[0];
                    if (_dtSig2.Rows.Count > 0)
                    {
                        textBox12.Value = _dtSig2.Rows[0]["EmpNameFull"].ToString();
                        GlobalFunctions.wfppreparer_sign = Convert.ToInt32(_dtSig2.Rows[0]["eid"].ToString());
                        GlobalFunctions.wfpdepthead_sign = Convert.ToInt32(_dtSig2.Rows[0]["OfficeHeadID"].ToString());

                        textBox67.Value = _dtSig2.Rows[0]["OfficeHead"].ToString();
                        textBox66.Value = _dtSig2.Rows[0]["OfficeDesignation"].ToString();
                    }
                }
                DataTable location = new DataTable();
                string _sqlloc = "SELECT isnull(Stuff((SELECT N'; ' + barangay + ', '+ municipal FROM [tbl_T_BMSWFP_Location] as xyz where xyz.officeid="+ office  + " and xyz.actioncode=1 and xyz.accountID=" + accountID + " FOR XML PATH(''),TYPE).value('text()[1]','varchar(max)'),1,2,N''),0)";
                location = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlloc).Tables[0];
                if (location.Rows[0][0].ToString() != "0")//(location.Rows.Count > 0)
                {
                    textBox60.Value = location.Rows[0][0].ToString(); 
                }
                else
                {
                    textBox60.Value = "Gov. DO Plaza Government Center, Prosperidad, Agusan del Sur";
                }
                if (approveprint == 1)
                {
                    DataTable _wfno = new DataTable();
                    string _sqlwfp = "Select upper([wfpno]) from ifmis.dbo.tbl_T_BMSWFP_xml where [qrcode]='" + qrcode.Value + "'";
                    _wfno = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlwfp).Tables[0];
                    textBox34.Value = _wfno.Rows[0][0].ToString();
                }

            }
            else //report history
            {
                var query = @"exec ifmis.dbo.[sp_BMS_WFPxml] "+ repHistory + "";
                DataTable dt1 = new DataTable();
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["adodb"].ConnectionString))
                {
                    SqlCommand com = new SqlCommand(query, con);
                    com.Parameters.Add(new SqlParameter("@Office", office));
                    com.Parameters.Add(new SqlParameter("@Year", year));
                    //com.Parameters.Add(new SqlParameter("@Project", project_id));
                    //com.Parameters.Add(new SqlParameter("@FundSource", fundsource));
                    com.CommandTimeout = 0;
                    con.Open();
                    dt1.Load(com.ExecuteReader());

                }
                table2.DataSource = dt1;
                DataTable _dt3 = new DataTable();
                string _sqlQuery3 = "Select * from ifmis.dbo.tbl_T_BMSWFP_xml where [wfp_id]="+ repHistory + " and [actioncode]=1";
                _dt3 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["fmisqldb"].ToString(), CommandType.Text, _sqlQuery3).Tables[0];

                textBox84.Value = _dt3.Rows[0]["datetimentered"].ToString();
                qrcode.Value = _dt3.Rows[0]["qrcode"].ToString();
                textBox12.Value = _dt3.Rows[0]["preparedby"].ToString();
                //textBox13.Value = _dt3.Rows[0]["preparedbyposition"].ToString();
                textBox67.Value = _dt3.Rows[0]["recommendby1"].ToString();
                textBox66.Value = _dt3.Rows[0]["recommendbyposition1"].ToString();
                textBox23.Value = _dt3.Rows[0]["recommendby2"].ToString();
                textBox25.Value = _dt3.Rows[0]["recommendbyposition2"].ToString();
                textBox72.Value = _dt3.Rows[0]["recommendby3"].ToString();
                textBox9.Value = _dt3.Rows[0]["recommendbyposition3"].ToString();
                textBox69.Value = _dt3.Rows[0]["approvedby"].ToString();
                textBox68.Value = _dt3.Rows[0]["approvedbyposition"].ToString();
                printstatus = Convert.ToInt32(_dt3.Rows[0]["submit"].ToString());

                //textBox3.Value = "Project : " + ProjectName;
                //textBox7.Value = "Project Title : ";
            }
            string status = "";
            if (printstatus == 1)
            {
                status = " (PREPARED)";
            }
            else if (printstatus == 2)
            {
                status = " (SUBMITTED)";
            }
            DataTable dtprogram = new DataTable();
            string sqlprogram = "exec sp_bms_WFP_Program " + office + "," + year + "," + accountID + "";
            dtprogram = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, sqlprogram).Tables[0];
            if (dtprogram.Rows.Count > 0)
            {
                textBox3.Value = "PROGRAM : " + dtprogram.Rows[0]["program"].ToString();
                textBox7.Value = "PROJECT TITLE : " + dtprogram.Rows[0]["project"].ToString();
            }
            textBox1.Value = "WORK AND FINANCIAL PLAN "+ year + ""+ status + "";
            textBox2.Value = OfficeName;
            textBox4.Value = "FUND SOURCE (specify) : " + fundsourcename +" - " + accountname + " ("+ ooeclass +")";
            
            
        }
        
    }
}