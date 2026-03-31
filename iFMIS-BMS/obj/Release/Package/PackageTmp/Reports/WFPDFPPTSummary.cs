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
    /// Summary description for Report1.
    /// </summary>
    public partial class WFPDFPPTSummary : Telerik.Reporting.Report
    {
        //function ImgError(image)
        //{
        //    var local = 0
        //    var imageFilename = $(image).attr('alt')

        //    var safeUrl = "https://pgas.ph/hris/content/images/photos/" + @Account.UserInfo.eid + ".png" + "?timestamp=" + new Date().getTime();
        //    var errorUrl = "@Url.Content("~/ Content / images / no_avatar_available.jpg")" + "?timestamp=" + new Date().getTime();
        //    $.LoadImage(safeUrl, errorUrl, image);
        //}

        //$.LoadImage = function(safeUrl, errorUrl, image)
        //{
        //    var img = new Image();
        //    img.src = safeUrl;

        //    img.onload = function() {
        //        image.src = safeUrl
        //    }
        //    img.onerror = function() {
        //        image.src = errorUrl
        //    }
        //}
        public WFPDFPPTSummary(int? OfficeID=0, int? yearof=0, int? qtr=0,int? mode=0)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            textBox1.Value = "C.Y. " + yearof;
            //pictureBox1.Value = "http://10.10.51.111/hris/Content/Images/signature/blank-signature.png";
            
                //DataTable _dt = new DataTable();
                //string _sqlQuery1 = "Select [Position] from [vwMergeAllEmployee] where eid=" + empid + "";
                //_dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisqldb"].ToString(), CommandType.Text, _sqlQuery1).Tables[0];

                //textBox65.Value = _dt.Rows[0]["Position"].ToString();
                ////txt_account.Value = _dt.Rows[0]["AccountName"].ToString();

                DataTable _dt2 = new DataTable();
                string _sqlQuery2 = "exec ifmis.dbo.sp_BMS_WFPDFPPT_Preparer " + OfficeID + ","+ yearof + "";
                _dt2 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisqldb"].ToString(), CommandType.Text, _sqlQuery2).Tables[0];

                textBox67.Value = _dt2.Rows[0][7].ToString();
                textBox66.Value = _dt2.Rows[0][5].ToString();
                textBox71.Value = _dt2.Rows[0][9].ToString(); 
                //textBox64.Value = empname;

                DataTable _dt3 = new DataTable();
                string _sqlQuery3 = "Select format(getdate(),'M/dd/yyyy hh:mm:ss tt') as ServerDate";
                _dt3 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisqldb"].ToString(), CommandType.Text, _sqlQuery3).Tables[0];

                textBox84.Value = _dt3.Rows[0]["ServerDate"].ToString();
                qrcode.Value = FUNCTION.GeneratePISControl();
                GlobalFunctions.QR_globalstr = qrcode.Value;
               
                 textBox34.Value = "Detailed Financial and Physical Performance Target";

            DataTable _dtsign = new DataTable();
            string _sqlQuery6 = "SELECT top 1 [approveby],isnull([approvedatetime],format(getdate(),'MM/dd/yyyy hh:mm:ss tt')) [approvedatetime] FROM [IFMIS].[dbo].[tbl_T_BMSAllocation] where OfficeID=" + OfficeID + " and year=" + yearof + " and qtr=" + qtr + " and actioncode=1 and approve=1 order by cast([approvedatetime] as datetime) desc";
            _dtsign = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery6).Tables[0];
            if (_dtsign.Rows.Count != 0)
            {
                var safeUrl = "https://pgas.ph/hris/Content/Images/signature/" + Account.UserInfo.eid + ".png";// + "?timestamp=" + new Date().getTime();
                pictureBox1.Value = safeUrl;
               
            }


            if (qtr == 1)
            {
                textBox31.Value = "FIRST QUARTER";
                textBox11.Value = "January";
                textBox19.Value = "February";
                textBox22.Value = "March";
                textBox28.Value = textBox11.Value;
                textBox26.Value = textBox19.Value;
                textBox29.Value = textBox22.Value;
            }
            else if (qtr == 2)
            {
                textBox31.Value = "SECOND QUARTER";
                textBox11.Value = "April";
                textBox19.Value = "May";
                textBox22.Value = "June";
                textBox28.Value = textBox11.Value;
                textBox26.Value = textBox19.Value;
                textBox29.Value = textBox22.Value;
            }
            else if (qtr == 3)
            {
                textBox31.Value = "THIRD QUARTER";
                textBox11.Value = "July";
                textBox19.Value = "August";
                textBox22.Value = "September";
                textBox28.Value = textBox11.Value;
                textBox26.Value = textBox19.Value;
                textBox29.Value = textBox22.Value;
            }
            else
            {
                textBox31.Value = "FOURTH QUARTER";
                textBox11.Value = "October";
                textBox19.Value = "November";
                textBox22.Value = "December";
                textBox28.Value = textBox11.Value;
                textBox26.Value = textBox19.Value;
                textBox29.Value = textBox22.Value;
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("ooename");              //dr[0]
            dt.Columns.Add("accountname");              //dr[1]
            dt.Columns.Add("Amount_Appropriation");         //dr[2]
            dt.Columns.Add("Amount_Reserve");      //dr[3]
            dt.Columns.Add("appropriation_net");
            dt.Columns.Add("first");
            dt.Columns.Add("second");
            dt.Columns.Add("third");
            dt.Columns.Add("mfo");
            dt.Columns.Add("physicaltargetfirst");
            dt.Columns.Add("physicaltargetsecond");
            dt.Columns.Add("physicaltargethird");
            dt.Columns.Add("ooe");
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
           
                SqlCommand com = new SqlCommand(@"ifmis.dbo.[sp_BMS_WFPDFPPTApprove_summary]", con);
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.Parameters.Add(new SqlParameter("@OfficeID", OfficeID));
                com.Parameters.Add(new SqlParameter("@yearof", yearof));
                com.Parameters.Add(new SqlParameter("@qtr", qtr));
                com.Parameters.Add(new SqlParameter("@mode", mode));
                com.Parameters.Add(new SqlParameter("@userid", Account.UserInfo.eid));
                com.Parameters.Add(new SqlParameter("@qrcode", qrcode.Value));

                con.Open();
                
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = reader.GetValue(15).ToString();
                    dr[1] = reader.GetValue(14).ToString();
                    dr[2] = reader.GetValue(0).ToString();
                    dr[3] = reader.GetValue(1).ToString();
                    dr[4] = reader.GetValue(2).ToString();
                    dr[5] = reader.GetValue(6).ToString();
                    dr[6] = reader.GetValue(7).ToString();
                    dr[7] = reader.GetValue(8).ToString();
                    dr[8] =reader.GetValue(9).ToString();
                    dr[9] =  reader.GetValue(10).ToString();
                    dr[10] = ""; //reader.GetValue(23).ToString();
                    dr[11] = ""; //reader.GetValue(24).ToString();
                    dr[12] = reader.GetValue(17).ToString();
                    dt.Rows.Add(dr);
                }
                
            }
            table1.DataSource = dt;

            DataTable _dt9 = new DataTable();
            string _sqlQuerymfo = "select dbo.[fn_BMS_WFPDFPPT_MFO] (" + OfficeID + "," + yearof + ")";
            _dt9 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuerymfo).Tables[0];

            textBox70.Value = _dt9.Rows[0][0].ToString();

            DataTable _wfno = new DataTable();
            string _sqlwfp = "Select top 1 upper([wfpno]),format(cast(datetimentered as datetime),'MM/dd/yyyy hh:mm:ss tt') from ifmis.dbo.tbl_T_BMSWFP_xml where [qrcode]='" + qrcode.Value + "' and actioncode=1";
            _wfno = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlwfp).Tables[0];
            if (_wfno.Rows.Count > 0)
            {
                textBox73.Value = _wfno.Rows[0][0].ToString();
                textBox76.Value = _wfno.Rows[0][1].ToString();
            }
        }
        
    }
}