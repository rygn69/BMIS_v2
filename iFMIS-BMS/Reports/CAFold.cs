namespace iFMIS_BMS.Reports
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Linq;
    using System.Configuration;
    using System.Data.SqlTypes;
    using iFMIS_BMS.Base;
    using System.Data;
    using System.Data.SqlClient;
    using iFMIS_BMS.BusinessLayer.Connector;
    using iFMIS_BMS.Classes;
    public partial class CAFold : Telerik.Reporting.Report
    {
        public CAFold(string cafno)
        {
            InitializeComponent();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            barcode1.Value = FUNCTION.GeneratePISControl();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                //SqlCommand com = new SqlCommand(@"exec [sp_BMS_CAFReport] '" + cafno + "'", con);
                //con.Open();
                //dt2.Load(com.ExecuteReader());
                SqlCommand com = new SqlCommand(@"exec [sp_BMS_CAFReport] '" + cafno + "'", con);
                con.Open();
                dt2.Load(com.ExecuteReader());
            }
            this.table1.DataSource = dt2;
        }
    }
}