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
    public partial class WFDP : Telerik.Reporting.Report
    {
        public WFDP(int? year=0, string OfficeName = "",int? printstatus=0,int? accountID=0,string accountname="",string ooeclass="",string wfpno="",int? OfficeID=0)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            
                DataTable _dt3 = new DataTable();
                string _sqlQuery3 = "Select format(getdate(),'M/dd/yyyy hh:mm:ss tt') as ServerDate";
                _dt3 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisqldb"].ToString(), CommandType.Text, _sqlQuery3).Tables[0];

                textBox84.Value = _dt3.Rows[0]["ServerDate"].ToString();
                qrcode.Value = FUNCTION.GeneratePISControl();
            
                textBox1.Value = "WORK AND FINANCIAL PLAN C.Y. " + year + " (REVIEWED)";
                
                //transferred here 5/15/2024 - from the bottom

               
                textBox12.Visible = false;
                textBox67.Visible = false;
                textBox23.Visible = false;
                textBox72.Visible = false;
                textBox69.Visible = false;
                

                GlobalFunctions.QR_globalstr = qrcode.Value;
                DataTable _dtreserve = new DataTable();
            
                //textBox132.Visible = false;
                //textBox132.CanShrink = true;
                //textBox163.Visible = false;
                //textBox163.CanShrink = true;
                //textBox163.Value = "";
                //textBox157.Visible = false;
                //textBox157.CanShrink = true;
                //textBox153.Visible = false;
                //textBox153.CanShrink = true;
                //textBox134.Visible = false;
                //textBox134.CanShrink = true;
                //textBox134.Visible = false;
                //textBox134.CanShrink = true;
                //textBox159.Visible = false;
                //textBox159.CanShrink = true;
                //retention -  end


            var query = @"exec ifmis.dbo.[sp_bms_WFDP_report] " + OfficeID + "," + accountID + ",'"+wfpno+"'," + year + "," + Account.UserInfo.eid + "";
                DataTable dt1 = new DataTable();
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["adodb"].ConnectionString))
                {
                    SqlCommand com = new SqlCommand(query, con);
                    com.CommandTimeout = 0;
                    con.Open();
                    dt1.Load(com.ExecuteReader());

                }
                table1.DataSource = dt1;
                
                DataTable _dtSig = new DataTable();
                DataTable _dtSig2 = new DataTable();
                string _sqlQuery = "";
                string _sqlQuery2 = "";
                
                DataTable location = new DataTable();
                string _sqlloc = "SELECT isnull(Stuff((SELECT N'; ' + barangay + ', '+ municipal FROM [tbl_T_BMSWFP_Location] as xyz where xyz.officeid="+ OfficeID + " and xyz.actioncode=1 and xyz.accountID=" + accountID + " FOR XML PATH(''),TYPE).value('text()[1]','varchar(max)'),1,2,N''),0)";
                location = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlloc).Tables[0];
                if (location.Rows[0][0].ToString() != "0")//(location.Rows.Count > 0)
                {
                    textBox60.Value = location.Rows[0][0].ToString(); 
                }
                else
                {
                    textBox60.Value = "Gov. DO Plaza Government Center, Prosperidad, Agusan del Sur";
                }
                
                textBox34.Value = wfpno;
            
                DataTable dtprogram = new DataTable();
                string sqlprogram = "exec sp_bms_WFP_Program " + OfficeID + "," + year + "," + accountID + "";
                dtprogram = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, sqlprogram).Tables[0];
                if (dtprogram.Rows.Count > 0)
                {
                    textBox3.Value = "PROGRAM : " + dtprogram.Rows[0]["program"].ToString();
                    textBox7.Value = "PROJECT TITLE : " + dtprogram.Rows[0]["project"].ToString();
                }
            
                textBox2.Value = OfficeName;
           
                textBox4.Value = "FUND SOURCE (specify) : " + accountname + "  (" + ooeclass + ")";
           
           
        }
        
    }
}