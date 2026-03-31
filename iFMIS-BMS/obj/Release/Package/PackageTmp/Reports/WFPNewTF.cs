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
    public partial class WFPNewTF : Telerik.Reporting.Report
    {
        public WFPNewTF(int? year=0, int? office=0, int? project_id=0, int? fundsource=0,string OfficeName="",string ProjectName="", string funddescription="",int? repHistory=0,int? prepby=0,int? printstatus=0,int? projectaip=0,string fundsourcename="",int? accountID=0,string accountname="",string  municipal="",string barangay="",string ooeclass="",int? pgas_loc=0,int? approveprint=0,int? programid=0,int? activityid=0,int? fundid=0)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            if (repHistory == 0)
            {
                DataTable _dt3 = new DataTable();
                string _sqlQuery3 = "Select format(getdate(),'dd-MMM-yy') as ServerDate,format(getdate(),'MM/dd/yyyy hh:mm:ss tt') as ServerDate2";
                _dt3 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisqldb"].ToString(), CommandType.Text, _sqlQuery3).Tables[0];

                textBox84.Value = _dt3.Rows[0]["ServerDate2"].ToString();
                textBox14.Value = _dt3.Rows[0]["ServerDate"].ToString();
                textBox11.Value = "Original";
                qrcode.Value = FUNCTION.GeneratePISControl();

                //// FOR wfp RECONSTRUCTION
                //textBox84.Value = "02/18/2025 06:50:52 PM";//_dt3.Rows[0]["ServerDate"].ToString();
                //qrcode.Value = "5RDOSUT0PY";
                //textBox14.Value = "18-Feb-25";
                //// FOR wfp RECONSTRUCTION

                if (printstatus != 3)
                {
                    textBox12.Visible = true;
                    textBox67.Visible = true;
                    textBox23.Visible = true;
                    textBox72.Visible = true;
                    textBox69.Visible = true;
                }
                else
                {
                    textBox12.Visible = false;
                    textBox67.Visible = false;
                    textBox23.Visible = false;
                    textBox72.Visible = false;
                    textBox69.Visible = false;
                }


                GlobalFunctions.QR_globalstr = qrcode.Value;
                if (fundid == 0) // GF
                {
                    if (activityid != 0 && accountID != 2861)
                    {
                        var query = @"exec ifmis.dbo.[sp_bms_WFP_report_grouping_peractivitiy] " + office + "," + year + "," + Account.UserInfo.eid + ",'" + qrcode.Value + "'," + prepby + ",'CYNTHIA P. LUMANTA, CE, EnP','Acting Provincial Government Department Head','JAVE NHORIEL N. BORDAJE, CPA','Provincial Budget Officer','SANTIAGO B. CANE, JR.','Provincial Governor','" + textBox84.Value + "'," + printstatus + "," + accountID + ",'" + municipal + "','" + barangay + "'," + pgas_loc + "," + approveprint + "," + activityid + "";
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
                        table1.DataSource = dt1;
                    }
                    else
                    {
                        var query = @"exec ifmis.dbo.[sp_bms_WFP_report_grouping] " + office + "," + year + "," + Account.UserInfo.eid + ",'" + qrcode.Value + "'," + prepby + ",'CYNTHIA P. LUMANTA, CE, EnP','Acting Provincial Government Department Head','JAVE NHORIEL N. BORDAJE, CPA','Provincial Budget Officer','SANTIAGO B. CANE, JR.','Provincial Governor','" + textBox84.Value + "'," + printstatus + "," + accountID + ",'" + municipal + "','" + barangay + "'," + pgas_loc + "," + approveprint + "";
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
                        table1.DataSource = dt1;
                    }
                }
                else // TRUST fund
                {
                    var query = @"exec ifmis.dbo.[sp_bms_WFP_report_grouping_TF] " + office + "," + year + "," + Account.UserInfo.eid + ",'" + qrcode.Value + "'," + prepby + ",'CYNTHIA P. LUMANTA, CE, EnP','Acting Provincial Government Department Head','JAVE NHORIEL N. BORDAJE, CPA','Provincial Budget Officer','SANTIAGO B. CANE, JR.','Provincial Governor','" + textBox84.Value + "'," + printstatus + "," + accountID + ",'" + municipal + "','" + barangay + "'," + pgas_loc + "," + approveprint + "";
                    //var query = @"exec ifmis.dbo.[sp_bms_WFP_report_grouping_TF_recon] " + office + "," + year + "," + Account.UserInfo.eid + ",'" + qrcode.Value + "'," + prepby + ",'CYNTHIA P. LUMANTA, CE, EnP','Acting Provincial Government Department Head','JAVE NHORIEL N. BORDAJE, CPA','Provincial Budget Officer','SANTIAGO B. CANE, JR.','Provincial Governor','" + textBox84.Value + "'," + printstatus + "," + accountID + ",'" + municipal + "','" + barangay + "'," + pgas_loc + "," + approveprint + "";
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
                    table1.DataSource = dt1;
                }
                DataTable _dtreserve = new DataTable();
                string _sqlreserve = "";

                if (fundid == 0)
                {
                    _sqlreserve = "exec sp_bms_WFP_retention " + office + "," + accountID + "," + year + " ," + printstatus + "," + Account.UserInfo.eid + "";
                    _dtreserve = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlreserve).Tables[0];
                    if (_dtreserve.Rows.Count > 0)
                    {
                        if (_dtreserve.Rows[0][7].ToString() == "1")
                        {
                            textBox132.Value = _dtreserve.Rows[0][0].ToString();
                            textBox157.Value = _dtreserve.Rows[0][1].ToString();
                            // textBox127.Width='2.34in';
                        }
                        else
                        {

                            textBox132.Visible = false;
                            textBox132.CanShrink = true;
                            textBox163.Visible = false;
                            textBox163.CanShrink = true;
                            textBox157.Visible = false;
                            textBox157.CanShrink = true;
                            textBox153.Visible = false;
                            textBox153.CanShrink = true;
                            textBox134.Visible = false;
                            textBox134.CanShrink = true;
                            textBox134.Visible = false;
                            textBox134.CanShrink = true;
                            textBox159.Visible = false;
                            textBox159.CanShrink = true;

                        }
                        textBox145.Value = _dtreserve.Rows[0][2].ToString();
                        textBox147.Value = _dtreserve.Rows[0][3].ToString();
                        textBox149.Value = _dtreserve.Rows[0][4].ToString();
                        textBox152.Value = _dtreserve.Rows[0][5].ToString();
                        textBox155.Value = _dtreserve.Rows[0][6].ToString();

                    }
                }
                else
                {
                    _sqlreserve = "exec sp_bms_WFP_retention_TF " + office + "," + accountID + "," + year + " ," + printstatus + "," + Account.UserInfo.eid + "";
                    _dtreserve = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlreserve).Tables[0];
                    if (_dtreserve.Rows.Count > 0)
                    {
                        textBox132.Visible = false;
                        textBox132.CanShrink = true;
                        textBox163.Visible = false;
                        textBox163.CanShrink = true;
                        textBox157.Visible = false;
                        textBox157.CanShrink = true;
                        textBox153.Visible = false;
                        textBox153.CanShrink = true;
                        textBox134.Visible = false;
                        textBox134.CanShrink = true;
                        textBox134.Visible = false;
                        textBox134.CanShrink = true;
                        textBox159.Visible = false;
                        textBox159.CanShrink = true;

                        textBox145.Value = _dtreserve.Rows[0][2].ToString();
                        textBox147.Value = _dtreserve.Rows[0][3].ToString();
                        textBox149.Value = _dtreserve.Rows[0][4].ToString();
                        textBox152.Value = _dtreserve.Rows[0][5].ToString();
                        textBox155.Value = _dtreserve.Rows[0][6].ToString();
                    }
                }

                DataTable _dtSig = new DataTable();
                DataTable _dtSig2 = new DataTable();
                string _sqlQuery = "";
                string _sqlQuery2 = "";
                //if (prepby != 0)
                //{
                //    _sqlQuery = "select a.EmpNameFull, a.Position,OfficeHead,OfficeDesignation,a.eid,b.OfficeHeadID from [pmis].[dbo].[vwMergeAllEmployee] as a left join " +
                //                   "pmis.dbo.OfficeDescription as b on b.OfficeID = a.Department " +
                //                   "where eid = " + prepby + "";
                //}
                //else
                //{
                //_sqlQuery = "select a.EmpNameFull, a.Position,OfficeHead,OfficeDesignation,a.eid,b.OfficeHeadID from [pmis].[dbo].[vwMergeAllEmployee] as a left join " +
                //                "pmis.dbo.OfficeDescription as b on b.OfficeID = a.Department " +
                //                "where eid in (Select top 1 isnull(userid, 0) from[IFMIS].[dbo].[tbl_T_BMSWFP_DFPPT] as a  where a.officeid = " + office + " and accountid = " + accountID + " and tyear = " + year + " and actioncode = 1 and isnull(submit,0)= 1) ";
                if (fundid == 0) //GF
                {
                    if (accountID == 2861)
                    {
                        _sqlQuery = "exec [dbo].[sp_BMS_WFP_Preparer_PerActivity_officeowner] " + programid + "," + accountID + "," + year + ",23";
                    }
                    else
                    {
                        if (activityid != 0)
                        {
                            _sqlQuery = "exec [dbo].[sp_BMS_WFP_Preparer] " + office + "," + programid + "," + accountID + "," + year + "," + activityid + "";
                        }
                        else
                        {
                            _sqlQuery = "exec [dbo].[sp_BMS_WFP_Preparer_PerActivity] " + programid + "," + accountID + "," + year + "";
                        }
                    }
                }
                else //TRUST FUND
                {
                    _sqlQuery = "exec [dbo].[sp_BMS_WFP_Preparer_TF] " + office + "," + accountID + "," + year + "," + activityid + "";
                }
                //} 

                //string _sqlQuery = "select a.EmpNameFull, a.Position,OfficeHead,OfficeDesignation from [pmis].[dbo].[vwMergeAllEmployee] as a left join "+ 
                //                    "pmis.dbo.OfficeDescription as b on b.OfficeID = a.Department "+ 
                //                    "where eid = "+ prepby  + "";
                _dtSig = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery).Tables[0];
                //if (accountID == 59747)
                //{
                //    textBox12.Value = "LEODILLA C. DINGDING";
                //    textBox67.Value = "RAZEL O. MONTEMOR";
                //    textBox66.Value = "PROVINCIAL GOVERNMENT DEPARTMENT HEAD";
                //    textBox30.Value = "ARMANDO R. GOMEZ";
                //}
                //else if (accountID == 2599)
                //{
                //    textBox12.Value = "ROBBYMELAILEEN P. NATIVIDAD";
                //    textBox67.Value = "REX MEL P. ACEDILLO";
                //}
                //else
                //{

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
                        //textBox66.Value = _dtSig.Rows[0]["OfficeDesignation"].ToString();
                    }
                    //else
                    //{
                    //    _sqlQuery2 = "select a.EmpNameFull, a.Position,OfficeHead,OfficeDesignation,a.eid,b.OfficeHeadID from [pmis].[dbo].[vwMergeAllEmployee] as a left join " +
                    //                    "pmis.dbo.OfficeDescription as b on b.OfficeID = a.Department " +
                    //                    "where eid in (Select top 1 isnull(userid, 0) from[IFMIS].[dbo].[tbl_T_BMSPPMP] as a where a.OfficeID = " + office + " and accountid = " + accountID + " and TransactionYear = " + year + " and actioncode = 1 and isnull(submit,0)= 1)";

                    //    _dtSig2 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["fmisqldb"].ToString(), CommandType.Text, _sqlQuery2).Tables[0];
                    //    if (_dtSig2.Rows.Count > 0)
                    //    {
                    //        textBox12.Value = _dtSig2.Rows[0]["EmpNameFull"].ToString();
                    //        GlobalFunctions.wfppreparer_sign = Convert.ToInt32(_dtSig2.Rows[0]["eid"].ToString());
                    //        GlobalFunctions.wfpdepthead_sign = Convert.ToInt32(_dtSig2.Rows[0]["OfficeHeadID"].ToString());

                    //        textBox67.Value = _dtSig2.Rows[0]["OfficeHead"].ToString();
                    //        textBox66.Value = _dtSig2.Rows[0]["OfficeDesignation"].ToString();
                    //    }
                    //}
               // }
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
                    if (fundid == 0)
                    {
                        DataTable _wfno = new DataTable();
                        string _sqlwfp = "Select top 1 upper([wfpno]) from ifmis.dbo.tbl_T_BMSWFP_xml where [qrcode]='" + qrcode.Value + "' and actioncode=1";
                        _wfno = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlwfp).Tables[0];
                        textBox34.Value = _wfno.Rows[0][0].ToString();
                    }
                    else
                    {
                        DataTable _wfno = new DataTable();
                        string _sqlwfp = "Select top 1 upper([wfpno]) from ifmis.dbo.tbl_T_BMSWFP_xml_TF where [qrcode]='" + qrcode.Value + "' and actioncode=1";
                        _wfno = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlwfp).Tables[0];
                        textBox34.Value = _wfno.Rows[0][0].ToString();
                    }
                }
                if (accountID == 59747)
                {
                    textBox30.Value = "ARMANDO R. GOMEZ, EMDRCM";
                }
                else
                {
                    textBox30.Value = "";
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
                table1.DataSource = dt1;
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
            //if (activityid != 0)
            //{
            //    DataTable dtprogram = new DataTable();
            //    string sqlprogram = "exec sp_bms_WFP_Program_PerActivity " + office + "," + year + "," + accountID + ","+ activityid + "";
            //    dtprogram = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, sqlprogram).Tables[0];
            //    if (dtprogram.Rows.Count > 0)
            //    {
            //        textBox3.Value = "PROGRAM : " + dtprogram.Rows[0]["program"].ToString();
            //        textBox7.Value = "PROJECT TITLE : + " + accountname + "";
            //        //textBox7.Value = "PROJECT TITLE : " + dtprogram.Rows[0]["project"].ToString();
            //    }
            //}
            //else
            //{
                DataTable dtprogram = new DataTable();
                string sqlprogram = "exec sp_bms_WFP_Program_TF " + office + "," + year + "," + accountID + "";
                dtprogram = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, sqlprogram).Tables[0];
                if (dtprogram.Rows.Count > 0)
                {
                    textBox3.Value = "PROGRAM : " + dtprogram.Rows[0]["program"].ToString();
                    textBox7.Value = "PROJECT TITLE : "+dtprogram.Rows[0]["project"].ToString();
                }
            //}
            DataTable dtrevsion = new DataTable();
            string sqlrevision = "exec sp_BMS_WFPRevision " + office + ","+ programid +"," + accountID + "," + year + "";
            dtrevsion = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, sqlrevision).Tables[0];
            if (dtrevsion.Rows.Count > 0)
            {
                textBox1.Value = "WORK AND FINANCIAL PLAN C.Y. " + year + "" + status + " (Revised From " + dtrevsion.Rows[0]["wfpno"].ToString() + ")";
            }
            else
            {
                textBox1.Value = "WORK AND FINANCIAL PLAN C.Y. " + year + "" + status + "";
            }
            textBox2.Value = OfficeName;
            if (fundid == 0)
            {
                textBox4.Value = "FUND SOURCE (specify) : " + fundsourcename + " - " + accountname.Replace("(MOOE)", "").Replace("(CO)","") + " (" + ooeclass + ")";
            }
            else
            {
                textBox4.Value = "FUND SOURCE (specify) : Trust Fund";
            }
            
            
        }
        
    }
}