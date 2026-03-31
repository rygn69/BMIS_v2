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

    /// <summary>
    /// Summary description for AIPPreperationReport.
    /// </summary>
    public partial class AIPPreperationReport : Telerik.Reporting.Report
    {
        public AIPPreperationReport(int OfficeID, string asOfDate, string ComputerIP, int Year,int isNonOffice)
        {
            var PrintedBy = "";
            var PrintedByPosition = "";
            var WhereAndOrderByStatement = "";
            if (OfficeID == 0)
            {
                WhereAndOrderByStatement = "where a.isNonOffice = 0 and  a.ActionCode = 1 and a.YearOf = " + Year + " ORDER BY a.OrderNo,AIP_ID";
            }
            else if (OfficeID == 43) {
                WhereAndOrderByStatement = "where a.isNonOffice = 1 and  a.ActionCode = 1 and a.YearOf = " + Year + " ORDER BY a.OfficeID,a.OrderNo,AIP_ID";
            }
            else
            {
                WhereAndOrderByStatement = "where a.isNonOffice = " + isNonOffice + " and a.OfficeID = " + OfficeID + " and a.ActionCode = 1 and a.YearOf = " + Year + " ORDER BY a.OrderNo,AIP_ID";
            }
            InitializeComponent();
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select a.OfficeID,b.OfficeName,AIPRefCode,
                                                Case when MotherAIP_ID is null then '<b>' + Replace(Description,'&','&amp;') + '</b>' else Replace(Description,'&','&amp;') end as Description,
                                                c.OfficeAbbrivation as ImplementingOffice,a.StartDate,a.CompletionDate,a.ExpectedOutput,
                                                a.FundingSource,NCHAR(8369) + CONVERT(varchar(50), a.PSAmount, 1) as PSAmount,
                                                NCHAR(8369) + CONVERT(varchar(50), a.MOOEAmount, 1) as MOOEAmount,
                                                NCHAR(8369) + CONVERT(varchar(50), a.COAmount, 1) as COAmount,
                                                Case when a.PSAmount is null and a.MOOEAmount is null and a.COAmount is null then '' else 
                                                NCHAR(8369) + Convert(VARCHAR(50), isnull(a.PSAmount,0) + isnull(a.MOOEAmount,0) + isnull(a.COAmount,0),1) end
                                                as TotalAmount,
                                                Case when a.ClimateChangeType = 1  then NCHAR(8369) + CONVERT(varchar(50), a.ClimateChangeAmount, 1)  else NULL end as 'CCAdaptation' ,
                                                Case when a.ClimateChangeType = 2 then NCHAR(8369) + CONVERT(varchar(50), a.ClimateChangeAmount, 1) else NULL end as 'CCMitigation' ,
                                                d.CCCode as 'CCTypologyCode',
                                                    NCHAR(8369) + CONVERT(varchar(50),(select Sum(isnull(PSAmount,0) + isnull(MOOEAmount,0) + isnull(COAmount,0)) 
                                                    from tbl_R_BMSAnnualInvestmentPlan Where OfficeID = a.OfficeID and ActionCode = 1 and YearOf = a.YearOf),1) as 'FooterTotal'
                                                from tbl_R_BMSAnnualInvestmentPlan a
                                                LEFT JOIN tbl_R_BMSOffices as b on b.OfficeID = a.OfficeID
                                                LEFT JOIN tbl_R_BMSOffices as c on c.OfficeID = a.EmplementingOfficeID
                                                LEFT JOIN tbl_R_BMSAIPClimateChangeCode as d on d.CCCodeID = a.ClimateChangeTypologyCode and d.ActionCode = 1 
                                                " + WhereAndOrderByStatement + "", con);
                con.Open();
                dt.Load(com.ExecuteReader());

                SqlCommand com2 = new SqlCommand(@"select top 1 UPPER(Firstname + ' ' + left(MI,1) + '. ' + LASTNAME), Position from pmis.dbo.employee where eid = " + Account.UserInfo.eid + "", con);
                SqlDataReader reader = com2.ExecuteReader();
                reader.Read();
                PrintedBy = reader.GetValue(0).ToString();
                PrintedByPosition = reader.GetValue(1).ToString();
                reader.Close();
              
            }
            this.DataSource = dt;
            AIPBarcode.Value = "SYSTEM GENERATED DOCUMENT" + Environment.NewLine
                                    + "Printed by : " + PrintedBy + Environment.NewLine
                                    + "Print Date : " + DateTime.Now + Environment.NewLine
                                    + "I.P. Address : " + ComputerIP + Environment.NewLine;
            txtPrepairedBy.Value = PrintedBy;
            textBox3.Value = GlobalFunctions.getReportTextBoxValue(1, 1);
            txtPrepairedByPosition.Value = PrintedByPosition;
            txtOfficeHead.Value = GlobalFunctions.getOfficeHead(OfficeID);
            txtOfficeHeadSignatory.Value = GlobalFunctions.getOfficeHeadDesignation(OfficeID);
            ReportParameters["ReportPrintDateParam"].Value = asOfDate;
            ReportParameters["ReportYearOfParam"].Value = (DateTime.Now.Year + 1).ToString();
        }
    }
}