namespace iFMIS_BMS.Reports
{
    using iFMIS_BMS.BusinessLayer.Connector;
    using iFMIS_BMS.BusinessLayer.Layers.Maintenance;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    /// <summary>
    /// Summary description for OrdinanceReportSecondPage.
    /// </summary>
    public partial class OrdinanceReportSecondPage : Telerik.Reporting.Report
    {
        public OrdinanceReportSecondPage(int year)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            DataTable dt = new DataTable();
            dt.Columns.Add("Name"); //dr[0]
            dt.Columns.Add("Designation");//dr[1]
            dt.Columns.Add("Status");//dr[2]

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select isnull('Hon. ' + b.Firstname + ' ' + left(b.MI,1) +'. ' + b.Lastname +
                                                case when b.Suffix = '' or b.suffix is null then '' else ', ' + b.suffix end,'None') as 'Name',
                                                c.Designation, a.StatusName + ' :'
                                                from tbl_R_BMSOrdinanceSPStatus as a
                                                LEFT JOIN tbl_R_BMSOrdinanceAttendance  as c on c.Status = a.StatusID and a.ActionCode = c.ActionCode 
                                                and c.YearOf = "+ year 
                                                +" LEFT JOIN pmis.dbo.employee as b on c.eid = b.eid ORDER BY a.OrderNo, c.OrderNo", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = reader.GetValue(0).ToString();  //Name
                    dr[1] = reader.GetValue(1).ToString();  //Designation
                    dr[2] = reader.GetValue(2).ToString();  ///Status

                    dt.Rows.Add(dr);
                }
            }
            OrdinanceLayer Layer = new OrdinanceLayer();
            var Header = Layer.GetAttendanceHeader();
            txtReportTittle.Value = Header[0];
            txtDescription.Value = Header[1];
            
            this.DataSource = dt;

            DataTable _dt4 = new DataTable();
            string _sqlQuery4 = "SELECT [lgu],[province],isnull([address],'') address FROM [IFMIS].[dbo].[tbl_R_BMSLgu] where actioncode=1";
            _dt4 = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, _sqlQuery4).Tables[0];
            if (_dt4.Rows.Count > 0)
            {
                pictureBox1.Value = "http://localhost/ifmis_bms/Images/tacurong.png";
                textBox2.Value = _dt4.Rows[0][0].ToString();
                textBox3.Value = _dt4.Rows[0][2].ToString();
            }
        }
    }
}