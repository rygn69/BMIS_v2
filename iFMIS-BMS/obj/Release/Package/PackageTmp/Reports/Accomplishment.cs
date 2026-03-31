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
    /// Summary description for Report1.
    /// </summary>
    public partial class Accomplishment : Telerik.Reporting.Report
    {
        public Accomplishment(int? year, int? month_, int? month_To)
        {
         
            InitializeComponent();

            DataTable dt3 = new DataTable();

            var dte = "";
           
           
            dte = DateTime.Now.ToString();
           
            barcode1.Value = FUNCTION.GeneratePISControl();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"IFMIS.dbo.[sp_BMS_Accomplishment] "+ year + "," + month_ + "," + month_To + "", con);
                com.CommandTimeout = 0;
                con.Open();
                dt3.Load(com.ExecuteReader());

                this.table1.DataSource = dt3;
            }

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                //SqlCommand com = new SqlCommand(@"SELECT CONCAT((SELECT DATENAME(month, DATEADD(month, '" + month_ + "'-1, CAST('2008-01-01' AS datetime)))), " +
                //                               " ' - ', (SELECT DATENAME(month, DATEADD(month, '" + month_To + "'-1, CAST('2008-01-01' AS datetime))))) + ' ' + '" + year + "'", con);

                SqlCommand com = new SqlCommand(@"SELECT case when '" + month_ + "' =  '" + month_To + "' then (SELECT DATENAME(month, DATEADD(month,  '" + month_To + "'-1, CAST('2008-01-01' AS datetime)))) " +
                                                "else  CONCAT((SELECT DATENAME(month, DATEADD(month, '" + month_ + "'-1, CAST('2008-01-01' AS datetime)))), " +
                                                " ' - ', (SELECT DATENAME(month, DATEADD(month,  '" + month_To + "'-1, CAST('2008-01-01' AS datetime))))) end + ' ' + '" + year + "'", con);
                con.Open();
                if (month_To == 1)
                {
                    TXT_for_the.Value = "For the period of January " + year;
                }
                else
                {
                    TXT_for_the.Value = "For the period of " + com.ExecuteScalar().ToString();
                }

            }
            dte = DateTime.Now.ToString();
            textBox2.Value =  dte;

        }
    }
}