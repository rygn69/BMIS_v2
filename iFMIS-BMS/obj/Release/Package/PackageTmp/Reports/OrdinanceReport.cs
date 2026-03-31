namespace iFMIS_BMS.Reports
{
    using iFMIS_BMS.BusinessLayer.Connector;
    using iFMIS_BMS.Classes;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Xml;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for OrdinanceReport.
    /// </summary>
    public partial class OrdinanceReport : Telerik.Reporting.Report
    {
        //public static int year = DateTime.Now.Year;
        public static int year;
        public OrdinanceReport(int YearParam)
        {
            year = YearParam;
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            DataTable dt = new DataTable();

            dt.Columns.Add("Description"); //dr[0]
            dt.Columns.Add("Amount"); //dr[1]
            dt.Columns.Add("FirstGroup"); //dr[2]
            dt.Columns.Add("SecondGroup"); //dr[3]
            dt.Columns.Add("SubGroup"); //dr[4]
            dt.Columns.Add("MainGroup"); //dr[5]
            dt.Columns.Add("SectionGroup"); //dr[6]
            dt.Columns.Add("Stringyear");//dr[7]
            dt.Columns.Add("MainGroupFooter"); //dr[8]
            dt.Columns.Add("currentyearapproved");//dr[9]
            dt.Columns.Add("currentyearobligation");//dr[9]
            dt.Columns.Add("pastyearapproved");//dr[9]

            DataRow dr1 = dt.NewRow();
            dr1[2] = 0;  //FirstGroup
            dr1[3] = 0;  // SecondGroup
            dr1[4] = 0;  //SubGroup
            dr1[5] = 0;  //MainGroup
            dr1[6] = 0;  //SectionGroup
            dr1[7] = ConvertNumberToWords.NumberToWords(year).ToUpper();
            dr1[8] = 0;
            dr1[9] = 0;
            dr1[10] = 0;
            dr1[11] = 0;

            dt.Rows.Add(dr1);
            

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_BMS_OrdinanceReport " + (year + 1) + "", con);
                com.CommandTimeout = 0;
                con.Open();
                //SqlDataReader reader = com.ExecuteReader();
                dt.Load(com.ExecuteReader());
            }
            this.DataSource = dt;
            txtSeriesOf.Value = "Series of " + year.ToString();
            textBox17.Value = "(CY " + (year + 1) + ")";
            string path = System.Web.HttpContext.Current.Server.MapPath("~/XML/OrdinanceReportSignatory.xml");
            XmlDocument xd = new XmlDocument();
            xd.Load(path);
            XmlNodeList IDListNode = xd.SelectNodes("/Table/Signatory");

            foreach (XmlNode item in IDListNode)
            {
                var SignatoryID = item.SelectSingleNode("SignatoryID").InnerText;
                switch (SignatoryID)
                {
                    case "1":
                        txtSignatory1.Value = item.SelectSingleNode("SignatoryName").InnerText;
                        break;
                    case "2":
                        txtSignatory2.Value = item.SelectSingleNode("SignatoryName").InnerText;
                        break;
                    case "3":
                        txtattested1.Value = item.SelectSingleNode("SignatoryName").InnerText;
                        break;
                    case "4":
                        txtattested2.Value = item.SelectSingleNode("SignatoryName").InnerText;
                        break;
                    case "5":
                        txtattested3.Value = item.SelectSingleNode("SignatoryName").InnerText;
                        break;
                    case "6":
                        txtApproved1.Value = item.SelectSingleNode("SignatoryName").InnerText;
                        break;
                    case "7":
                        txtApproved2.Value = item.SelectSingleNode("SignatoryName").InnerText;
                        break;
                }

            }
            textBox17.Value = "CY "+ (YearParam + 1) + "";
            textBox19.Value = "Current Year (Estimate CY "+ YearParam + " )";
            textBox15.Value = "(CY " + (YearParam-1) + " Actual)";
        }
        public static string getOfficeSpecialProvision(string OfficeID)
        {
            var SpecialProvisions = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select SpecialProvisionOrder,SpecialProvisionDescription from dbo.tbl_R_BMSOrdinanceSpecialProvision where OfficeID = '" + OfficeID + "' and ActionCode = 1 and YearOf = " + year + " order by SpecialProvisionOrder", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    SpecialProvisions = SpecialProvisions + reader.GetValue(0).ToString() + ") " + reader.GetValue(1).ToString() + "<br/><br/>";
                }
            }
            if (SpecialProvisions == "")
            {
                return "0";
            }
            else
            {
                return SpecialProvisions;    
            }   
            
        }
        public static string getMainOfficeSpecialProvision(string OfficeID)
        {
            var SpecialProvisions = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select SpecialProvisionOrder,SpecialProvisionDescription from dbo.tbl_R_BMSOrdinanceMainOfficeSpecialProvision where OfficeID = '" + OfficeID + "' and ActionCode = 1 and YearOf = " + year + " order by SpecialProvisionOrder", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    SpecialProvisions = SpecialProvisions + reader.GetValue(0).ToString() + ") " + reader.GetValue(1).ToString() + "<br/><br/>";
                }
            }
            if (SpecialProvisions == "")
            {
                return "0";
            }
            else
            {
                return SpecialProvisions;
            }

        }
        public static string getAuthor()
        {
            var Author = "";
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select Upper(b.Firstname + ' ' + Left(b.MI,1) + '. ' + b.Lastname + 
                                                case when b.Suffix = '' or b.Suffix = NULL then '' else ', ' + b.suffix  END) from tbl_R_BMSOrdinanceAuthor as a 
                                                LEFT JOIN pmis.dbo.employee as b on a.eid = b.eid
                                                where a.Yearof = " + year + " and a.ActionCode = 1", con);
                    con.Open();
                    Author = com.ExecuteScalar().ToString();
                }
                if (Author == "")
                {
                    return "No Author Please Go To Configuration Settings And Configure The Author";
                }
                else
                {
                    return "HON. " + Author;
                }
            }
            catch (Exception)
            {
                return "No Author Please Go To Configuration Settings And Configure The Author";
            }
        }
    }
}