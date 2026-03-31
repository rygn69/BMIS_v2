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
    /// Summary description for SB9.
    /// </summary>
    public partial class SB3 : Telerik.Reporting.Report
    {
        public SB3(int? OfficeID=0,int? year=0,int? mode=0)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            var query = @"exec ifmis.dbo.[sp_BMS_SB3] " + OfficeID + "," + year + ","+ mode + "";
            DataTable dt1 = new DataTable();
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["adodb"].ConnectionString))
            {
                SqlCommand com = new SqlCommand(query, con);
                com.CommandTimeout = 0;
                con.Open();
                dt1.Load(com.ExecuteReader());

            }
            table1.DataSource = dt1;

            if (mode == 1)
            {
                textBox2.Value = "STATEMENT OF SUPPLEMENTAL APPROPRIATION (PROPOSED)";
            }

            qrcode.Value = FUNCTION.GeneratePISControl();
            GlobalFunctions.QR_globalstr = qrcode.Value;

            if (mode == 3) //for digital signature
            {
                var retstr = "";
                DataTable reportlog2 = new DataTable();
                using (SqlConnection conUpdateRep = new SqlConnection(Common.MyConn()))
                {
                    //SqlCommand com = new SqlCommand(@"exec [sp_BMS_LBPReport_Update] " + year + ",0,9,'" + qrcode.Value + "'", conUpdateRep);
                    //conUpdateRep.Open();
                    ////reportlog2.Load(com.ExecuteReader());
                    //retstr= com.ExecuteScalar().ToString();

                    SqlCommand com = new SqlCommand("dbo.[sp_BMS_LBPReport_Update]", conUpdateRep);
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add(new SqlParameter("@yearof", year));
                    com.Parameters.Add(new SqlParameter("@officeid", OfficeID));
                    com.Parameters.Add(new SqlParameter("@typeid", "9"));
                    com.Parameters.Add(new SqlParameter("@qrcode", qrcode.Value));
                    conUpdateRep.Open();
                    retstr = com.ExecuteScalar().ToString();


                }
                DataTable lbp9 = new DataTable();
                string _sqlwfp = "Select top 1 upper([rptno]) from ifmis.dbo.tbl_T_BMSLBPReport_xml where [qrcode]='" + qrcode.Value + "'";
                lbp9 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlwfp).Tables[0];
                textBox40.Value = lbp9.Rows[0][0].ToString();
            }
        }
    }
}