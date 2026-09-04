    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Reporting;
using iFMIS_BMS.Reports.Design;
using System.Data;
using System.Data.SqlClient;
using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.Base;

namespace iFMIS_BMS.Reports
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        InstanceReportSource rs = new InstanceReportSource();
        InstanceReportSource rs2 = new InstanceReportSource();
        ReportBook rb = new ReportBook();
        ReportBook rbf4 = new ReportBook();
        protected void Page_Load(object sender, EventArgs e)
        {
            var OfficeID = 0;
            var OfficeName = "";
            var year = 0;
            var year2 = 0;
            var ReportID = "";
            var IncludeProposed = 0;
            var AsOfDate = "";
            var programID = 0;
            var OOE_ID = 0;
            var accountID = 0;
            var classtype = 0;
            var month_ = 0;
            var monthname = "";
            var month_To = 0;
            var batch = 0;
            var sort_ = 0;  
            var note_ ="";
            var air = 0;
            var changed_To = "";
            var Fundtype = 0;
            var SectorID = 0;
            var InclusiveYear = "";
            var ReportTypeID = 0;
            var SAAO_type = 0;
            var isNonOffice = 0;
            var Detail_type = 0;
            var All_Type = 0;
            var realign_type = 0;
            var pgo_ = 0;
            var rootppaid = 0;
            var subppaid = 0;
            var summary =0;
            var program=0;
            var accountTemp = 0;
            var subaccount=0;
            var ProposalYear = 0;
            var pgocntrl = 0;
            var monthof = 0;
            var Disyear = 0;
            var officeID = 0;
            var Dismonth = 0;
            var includex = 0;
            var excessacctval=0;
            var exmonhtofcessacct = "";
            var byYear = 0;
            var monthofname = "";
            var fundtypeid = 0;
            var excesscntrl = 0;
            int exaccount = 0;
            var ooeid = 0;
            var yearof = 0;
            var qtr = 0;
            var mpoffice = "";
            long  empid = 0;
            var empname = "";
            var quarter = 0;
            var repHistory = 0;
            var earmark_type = 0;
            var includeLBP1 = 0;
            var pagnoid = 0;
            var includeLBP2_SP = 0;
            long eid = 0;
            var includezero = 0;
            var expclass = 0;
            var purposeid = "";
            var packet = 0;
            var reporthistory = 0;
            var txtsearch = "";
            var cafno = "";
            var issuedate = "";
            var is_float = 0;
            var certid = "";
            var budgettype = 0;
            var lbp2export = 0;
            var reloadlbp2 = 0;
            var sectoral = 0;
            var excess = 0;
            var accountname = "";
            int commitmentthistory = 0;
            long accountcom = 0;
            var repxmlhistory = 0;
            var saaotag = 0;
            var dateissue = "";
            var allsubppa = 0;
            var prepdfppt = 0;
            var approvedfppt = 0;
            var applynewdate = 0;
            var project_id = 0;
            var fundsource = 0;
            var ProjectName = "";
            var funddescription = "";
            var includeCOE = 0;
            var prepby = 0;
            var printstatus = 0;
            var accountidlist = 0;
            var projectaip = 0;
            var fundsourcename = "";
            var municipal = "";
            var barangay = "";
            var controlno = "";
            var perprogram = 0;
            var mode = 0;
            var source = 0;
            var approverepHistory = 0;
            var eco = 0;
            var ooeclass = "";
            var pgas_loc =0;
            var activityid = 0;
            var fundid = 0;
            var mode_trans = 0;
            var reloadlbp4 = 0;
            var specid = 0;
            int issupplemetalid = 0;
            int previewid = 0;
            int reptype = 0;
            int tyear = 0;
            var office = "";
            var wfpno = "";
            // int[] accountidlist = new int[] { }; 
            // string[] accountidlist= new string[] { };

            string ComputerIP = Request.UserHostAddress == "::1" ? "LocalHost" : Request.UserHostAddress;

            try
            {
                accountidlist = Convert.ToInt32(Request["accountidlist"].ToString()); 
                //accountidlist = new int[] {Convert.ToInt32(Request["accountidlist"].ToString())}; //{ 475, 352 };
                //   accountidlist = new string[] { "475","352" };// { Request["accountidlist"].ToString() };
            }
            catch { }
            try
            {
                SAAO_type = Convert.ToInt32(Request["SAAO_type"].ToString());
            }
            catch { } try
            {
                pgo_ = Convert.ToInt32(Request["pgo_"].ToString());
            }
            catch { }
            try
            {
                realign_type = Convert.ToInt32(Request["realign_type"].ToString());
            }
            catch { }
            try
            {
                Detail_type = Convert.ToInt32(Request["Detail_type"].ToString());
            }
            catch { }
            try
            {
                All_Type = Convert.ToInt32(Request["All_Type"].ToString());
            }
            catch { }
            try
            {
                ReportTypeID = Convert.ToInt32(Request["ReportTypeID"].ToString());
            }
            catch { }
            try
            {
                isNonOffice = Convert.ToInt32(Request["isNonOffice"].ToString());
            }
            catch { }
            try
            {
                OfficeID = Convert.ToInt32(Request["OfficeID"].ToString());
            }
            catch { }
            try
            {
                OfficeName = Request["OfficeName"].ToString();
            }
            catch { }
            try
            {
                year = Convert.ToInt32(Request["Year"].ToString());
            }
            catch { }
             try
            {
                year2 = Convert.ToInt32(Request["Year2"].ToString());
            }
            catch { }
            try
            {
                ReportID = Request["ReportID"].ToString();
            }
            catch { }
            try
            {
                IncludeProposed = Convert.ToInt32(Request["IncludeProposed"].ToString());
            }
            catch { }
            try
            {
                AsOfDate = Request["AsOfDate"].ToString();
            }
            catch { }
            try
            {
                programID = Convert.ToInt32(Request["programID"].ToString());
            }
            catch { }
            try
            {
                OOE_ID = Convert.ToInt32(Request["OOE_ID"].ToString());
            }
            catch { }
            try
            {
                accountID = Convert.ToInt32(Request["accountID"].ToString());
            }
            catch { }
            try
            {
                classtype = Convert.ToInt32(Request["classtype"].ToString());
            }
            catch { }
            try
            {
                month_ = Convert.ToInt32(Request["month_"].ToString());
            }
            catch { }
              try
            {
                monthname = Request["monthname"].ToString();
            }
            catch { }
            
            try
            {
                batch = Convert.ToInt32(Request["batch"].ToString());
            }
            catch { }
            try
            {
                month_To = Convert.ToInt32(Request["month_To"].ToString());
            }
            catch { }
            try
            {
                sort_ = Convert.ToInt32(Request["sort_"].ToString());
            }
            catch { }
            try
            {
                note_ = Request["note_"].ToString();
            }
            catch { }
            try
            {
                air = Convert.ToInt32(Request["air"].ToString());
            }
            catch { }
            try
            {
                changed_To = Request["changed_To"].ToString();
            }
            catch { }
            try
            {
                Fundtype = Convert.ToInt32(Request["Fundtype"].ToString());
            }
            catch { }
            try
            {
                SectorID = Convert.ToInt32(Request["SectorID"].ToString());
            }
            catch { }
            try
            {
                InclusiveYear = Request["InclusiveYear"].ToString();
            }
            catch { }
            try
            {
                rootppaid = Convert.ToInt32(Request["Actualrootppaid"].ToString());
            }
            catch { }
            try
            {
                subppaid = Convert.ToInt32(Request["ppaid"].ToString());
            }
            catch { }
            try
            {
                summary = Convert.ToInt16(Request["reportsum"].ToString());
            }
            catch { }

            try
            {
                program = Convert.ToInt16(Request["program"].ToString());
            }
            catch { }

            try
            {
                accountTemp = Convert.ToInt32(Request["accountTemp"].ToString());
            }
            catch { }

            try
            {
                subaccount = Convert.ToInt16(Request["subaccount"].ToString());
            }
            catch { }
            try
            {
                ProposalYear = Convert.ToInt16(Request["ProposalYear"].ToString());
            }
            catch { }
            try
            {
                pgocntrl = Convert.ToInt16(Request["pgocntrl"].ToString());
            }
            catch { }
            
            try
            {
                monthof = Convert.ToInt16(Request["monthof"].ToString());
            }
            catch { }
            try
            {
                Disyear = Convert.ToInt32(Request["Disyear"].ToString());
            }
            catch { }
            try
            {
                OfficeID = Convert.ToInt16(Request["OfficeID"].ToString());
            }
            catch { }
            try
            {
                Dismonth = Convert.ToInt16(Request["Dismonth"].ToString());
            }
            
            catch { }
            try
            {
                includex = Convert.ToInt16(Request["includex"].ToString());
            }
            catch { }
            try
            {
                excessacctval = Convert.ToInt16(Request["excessacctval"].ToString());
            }
            catch { }
            try
            {
                exmonhtofcessacct = Request["exmonhtofcessacct"].ToString();
            }
                
            catch { }
            
            try
            {
                byYear = Convert.ToInt16(Request["byYear"].ToString());
            }
                
            catch { }
            try 
            {
                monthofname = Request["monthofname"].ToString();
            }
            catch{}
            try
            {
                fundtypeid = Convert.ToInt16(Request["fundtypeid"].ToString());
            }
            catch { }
            try
            {
                excesscntrl = Convert.ToInt16(Request["excesscntrl"].ToString());
            }
            catch { }
            try
            {
                exaccount = Convert.ToInt32(Request["exaccount"].ToString());
            }
            catch { }
            try
            {
                ooeid = Convert.ToInt16(Request["ooeid"].ToString());
            }
            catch { }
            try
            {
                yearof = Convert.ToInt32(Request["yearof"].ToString());
            }
            catch { }
            try
            {
                qtr = Convert.ToInt16(Request["qtr"].ToString());
            }
            catch { }
            
            try
            {
                mpoffice = Request["mpoffice"].ToString();
            }
            catch { }
            try
            {
                empname = Request["empname"].ToString();
            }
            catch { }
            try
            {
                empid = Convert.ToInt64(Request["empid"].ToString());
            }
            catch { }
            try
            {
                quarter = Convert.ToInt16(Request["quarter"].ToString());
            }
            catch { }
            try
            {
                repHistory = Convert.ToInt32(Request["repHistory"].ToString());
            }
            catch { }
            try
            {
                earmark_type = Convert.ToInt16(Request["earmark_type"].ToString());
            }
            catch { }
            try
            {
                includeLBP1 = Convert.ToInt16(Request["includeLBP1"].ToString());
            }
            catch { }
            try {
                pagnoid = Convert.ToInt32(Request["pagnoid"].ToString());
            }
            catch { }
            try
            {
                includeLBP2_SP = Convert.ToInt16(Request["includeLBP2_SP"].ToString());
            }
            catch { }
            try
            {
                eid = Convert.ToInt64(Request["eid"].ToString());
            }
            catch { }
            try
            {
                includezero = Convert.ToInt16(Request["includezero"].ToString());
            }
            catch { }
            try
            {
                expclass = Convert.ToInt16(Request["expclass"].ToString());
            }
            catch { }
            try
            {
                purposeid= Request["purposeid"].ToString();
            }
            catch { }
            try
            {
                packet= Convert.ToInt16(Request["packet"].ToString());
            }
            catch { }
            try
            {
                reporthistory= Convert.ToInt32(Request["reporthistory"].ToString());
            }
            catch { }
            try
            {
                txtsearch =Request["txtsearch"].ToString();
            }
            catch { }
            try
            {
                cafno = Request["cafno"].ToString();
            }
            catch { }
            try
            {
                issuedate = Request["issuedate"].ToString();
            }
            catch { }
            try
            {
                is_float = Convert.ToInt16(Request["is_float"].ToString());
            }
            catch { }
            try
            {
                certid = Request["certid"].ToString();
            }
            catch { }
            try
            {
                budgettype = Convert.ToInt16(Request["budgettype"].ToString());
            }
            catch { }
            
            try
            {
                lbp2export = Convert.ToInt16(Request["lbp2export"].ToString());
            }
            catch { }
            try
            {
                reloadlbp2 = Convert.ToInt16(Request["reloadlbp2"].ToString());
            }
            catch { }
            try
            {
                sectoral = Convert.ToInt16(Request["sectoral"].ToString());
            }
            catch { }
            try
            {
                excess = Convert.ToInt16(Request["excess"].ToString());
            }
            catch { }
            try
            {
                accountname = Request["accountname"].ToString();
            }
            catch { }
            try
            {
                commitmentthistory = Convert.ToInt32(Request["commitmentthistory"].ToString());
            }
            catch { }
            try
            {
                accountcom = Convert.ToInt64(Request["accountcom"].ToString());
            }
            catch { }
            try
            {
                repxmlhistory = Convert.ToInt32(Request["repxmlhistory"].ToString());

            }
            catch { }
            try
            {
                saaotag = Convert.ToInt32(Request["saaotag"].ToString());
            }
            catch { }
            try
            {
                dateissue = Request["dateissue"].ToString();
            }
            catch { }
            try
            {
                allsubppa = Convert.ToInt32(Request["allsubppa"].ToString());
            }
            catch { }
            try
            {
                prepdfppt = Convert.ToInt32(Request["prepdfppt"].ToString());
            }
            catch{ }
            try
            {
                approvedfppt = Convert.ToInt32(Request["approvedfppt"].ToString());
            }
            catch { }
            try
            {
                applynewdate = Convert.ToInt32(Request["applynewdate"].ToString());
            }
            catch { }
            try
            {
                project_id = Convert.ToInt32(Request["project_id"].ToString());
            }
            catch { }
            try
            {
                fundsource = Convert.ToInt32(Request["fundsource"].ToString());
            }
            catch { }
            try
            {
                ProjectName = Request["ProjectName"].ToString();
            }
            catch { }
            try
            {
                funddescription = Request["funddescription"].ToString();
            }
            catch { }
            try
            {
                includeCOE=Convert.ToInt32(Request["includeCOE"].ToString());
            }
            catch { }
            try
            {
                prepby = Convert.ToInt32(Request["prepby"].ToString());
            }
            catch
            {

            }
            try
            {
                printstatus = Convert.ToInt32(Request["printstatus"].ToString());
            }
            catch
            {

            }
            try
            {
                projectaip = Convert.ToInt32(Request["projectaip"].ToString());
            }
            catch { }
            try
            {
                fundsourcename = Request["fundsourcename"].ToString();
            }
            catch { }
            try
            {
                municipal = Request["municipal"].ToString();
            }
            catch { }
            try
            {
                barangay = Request["barangay"].ToString();
            }
            catch { }
            try
            {
                controlno = Request["controlno"].ToString();
            }
            catch { }
            try
            {
                perprogram = Convert.ToInt32(Request["perprogram"].ToString());
            }
            catch { }
            try
            {
                mode = Convert.ToInt16(Request["mode"].ToString());
            }
            catch { }
            try
            {
                source = Convert.ToInt16(Request["source"].ToString());
            }
            catch { }
            try
            {
                approverepHistory= Convert.ToInt32(Request["approverepHistory"].ToString());
            }
            catch { }
            try
            {
                eco = Convert.ToInt32(Request["eco"].ToString());
            }
            catch { }
            try
            {
                ooeclass = Request["ooeclass"].ToString();
            }
            catch { }
            try
            {
                pgas_loc = Convert.ToInt32(Request["pgas_loc"].ToString());
            }
            catch { }
            try
            {
                activityid = Convert.ToInt32(Request["activityid"].ToString());
            }
            catch { }
            try
            {
                fundid = Convert.ToInt32(Request["fundid"].ToString());
            }
            catch { }
            try
            {
                mode_trans = Convert.ToInt32(Request["mode_trans"].ToString());
            }
            catch { }
            try {
                reloadlbp4 = Convert.ToInt32(Request["reloadlbp4"].ToString());
            }
            catch { }
            try
            {
                specid = Convert.ToInt32(Request["reloadlbp4"].ToString());
            }
            catch { }
            try
            {
                issupplemetalid= Convert.ToInt32(Request["issupplemetalid"].ToString());
            }
            catch { }
            try
            {
                previewid = Convert.ToInt32(Request["previewid"].ToString());
            }
            catch { }
            try
            {
                reptype = Convert.ToInt32(Request["reptype"].ToString());
            }
            catch {
            }
            try {
                tyear = Convert.ToInt32(Request["tyear"].ToString());
            }
            catch { }
            try
            {
                office = Convert.ToString(Request["office"].ToString());
            }
            catch { }
            try
            {
                wfpno = Convert.ToString(Request["wfpno"].ToString());
            }
            catch { }
            var rid = Request.QueryString["rid"];
            if (rid == "2")
            {
                if (Account.UserInfo.UserTypeDesc == "Budget In-Charge")
                {
                    rs.ReportDocument = new PlantillaReport(OfficeID, OfficeName, year, IncludeProposed);
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ReportSource = rs;
                }
                else
                {
                    rs.ReportDocument = new LBP4_LFC(OfficeID, OfficeName, year, IncludeProposed, ComputerIP);
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ReportSource = rs;
                }

            }
            else if (rid == "1")
            {
                rs.ReportDocument = new LBP3(OfficeID, OfficeName, year, ReportID, ComputerIP);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }
            else if (rid == "3")
            {
                rs.ReportDocument = new LBP5(year, OfficeID, OfficeName, ComputerIP);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }
            else if (rid == "4")
            {
                rs.ReportDocument = new rpt_SOO(OfficeID, OfficeName, year);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }
            else if (rid == "5")
            {
                rs.ReportDocument = new rpt_AIP();
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }
            else if (rid == "6")
            {
                var yearParam = year- 1;
                var FrontPage = new OrdinanceReportFrontPage(yearParam);
                var SecondPage = new OrdinanceReportSecondPage(yearParam);
                var OrdinanceReport = new OrdinanceReport(yearParam);
                rb.Reports.Add(FrontPage);
                rb.Reports.Add(SecondPage);
                rb.Reports.Add(OrdinanceReport);

                rs.ReportDocument = rb;
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;

            }
            else if (rid == "7")
            {
                rs.ReportDocument = new ProposedPositionReport(year, OfficeID);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }
            else if (rid == "8")
            {
                rs.ReportDocument = new BudgetReport(year, OfficeID, OfficeName, ComputerIP);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }
            else if (rid == "9")
            {
                rs.ReportDocument = new AIPPreperationReport(OfficeID, AsOfDate, ComputerIP, year, isNonOffice);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }
            else if (rid == "69")
            {
                rs.ReportDocument = new RegistryofAllotments(OfficeID, programID, OOE_ID, accountID, classtype, year, sort_, air, ComputerIP, includex, monthof, excessacctval, exmonhtofcessacct, byYear, txtsearch, repxmlhistory, monthname);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }
            else if (rid == "96")
            {
                rs.ReportDocument = new NonOffice(OfficeID, programID, OOE_ID, accountID, classtype, year, sort_, air, ComputerIP);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }
            else if (rid == "10")
            {
                rs.ReportDocument = new ARO(OfficeID, classtype, year, month_, batch, sort_, note_, expclass, purposeid,Fundtype,packet, reporthistory, ComputerIP, is_float, budgettype, dateissue);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }
            else if (rid == "101")
            {
                rs.ReportDocument = new LBEF(OfficeID, classtype, year, month_, batch, sort_, note_, ComputerIP);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }
            else if (rid == "11")
            {
                rs.ReportDocument = new BOI(OfficeID, year, month_, pgo_);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }
            else if (rid == "9678")
            {
                rs.ReportDocument = new BOIperOffice(year, month_, OfficeID, pgo_);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }
            else if (rid == "12")
            {
                rs.ReportDocument = new total_obligation(year, month_, month_To);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }
            else if (rid == "13")
            {
                rs.ReportDocument = new total_release(year, month_, month_To, repxmlhistory);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }
            else if (rid == "14")
            {
                var dtetime = "";
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select format(getdate(),'M/d/yyyy hh:mm:ss tt') ", con);
                    con.Open();
                     dtetime = com.ExecuteScalar().ToString();
                }
                   
                if (SAAO_type == 2)
                {


                    DataTable FundID_ = new DataTable();
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {

                        SqlCommand com = new SqlCommand(@" select FundCode,FundName from IFMIS.dbo.tbl_R_BMSFunds where FundID in (1,2,5,14) and actioncode=1  order by FundCode ", con);
                        con.Open();
                        FundID_.Load(com.ExecuteReader());
                    }
                    for (int a = 0; a <= FundID_.Rows.Count - 1; a++)
                    {

                        //var dtetime=DateTime.Now.ToString();
                        DataTable OfficeID_ = new DataTable();
                        using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        {

                            //SqlCommand com = new SqlCommand(@" select OfficeID,OfficeName from IFMIS.dbo.tbl_R_BMSOffices where PMISOfficeID is not null and OfficeID not in (17,9,64,58,59,60,61,66,72,74,76,78,79,80,81,82,83,84,85,86,87,88,89,90,91,92) and FundID = '" + Convert.ToInt32(FundID_.Rows[a][0]) + "'  order by isnull(OrderNo,999999)", con);
                            SqlCommand com = new SqlCommand(@"exec sp_BMS_OfficeSAAO " + Convert.ToInt32(FundID_.Rows[a][0]) + ","+ year + "", con);
                            con.Open();
                            OfficeID_.Load(com.ExecuteReader());
                        }
                        for (int i = 0; i <= OfficeID_.Rows.Count - 1; i++) 
                        {
                            if (i == 0 && i != OfficeID_.Rows.Count - 1)
                            {
                                var PlantillaReport = new SAAO_start(Convert.ToInt32(OfficeID_.Rows[i][0]), month_, month_To, year, classtype, 1, earmark_type, repxmlhistory, dtetime, saaotag);
                                rb.Reports.Add(PlantillaReport);
                            }
                            else if (i == OfficeID_.Rows.Count - 1)
                            {
                                var PlantillaReport = new SAAO_end(Convert.ToInt32(OfficeID_.Rows[i][0]), month_, month_To, year, classtype, a, earmark_type, repxmlhistory, dtetime, saaotag);
                                rb.Reports.Add(PlantillaReport);
                            }
                            else
                            {
                                var PlantillaReport = new SAAO_mid(Convert.ToInt32(OfficeID_.Rows[i][0]), month_, month_To, year, classtype, 1, earmark_type, repxmlhistory, dtetime, saaotag);
                                rb.Reports.Add(PlantillaReport);
                            }

                        }
                    }
                    rs.ReportDocument = rb;
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ReportSource = rb;
                    
                }
                else
                {

                    rs.ReportDocument = new SAAO(OfficeID, month_, month_To, year, classtype, SAAO_type, earmark_type, repxmlhistory);
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ReportSource = rs;

                }
            }
            else if (rid == "15")
            {
                if (reptype == 1)
                {
                    rs.ReportDocument = new exces_budget(Fundtype, changed_To, month_, month_To, year, air, pgo_, repxmlhistory, tyear);
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ReportSource = rs;
                }
                else
                {
                    rs.ReportDocument = new SAAO_Excess(Fundtype, changed_To, month_, month_To, year, tyear);
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ReportSource = rs;
                    
                }
            }
            else if (rid == "151")
            {
                    rs2.ReportDocument = new SAAO_Excess_Summary(Fundtype, changed_To, month_, month_To, year, tyear);
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ReportSource = rs2;
                
            }
            else if (rid == "16")
            {
                rs.ReportDocument = new LBEF_EE(OfficeID, classtype, year, month_, batch, sort_, note_, ComputerIP);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }
            else if (rid == "17")
            {
                rs.ReportDocument = new ELA_LDIP_Report(SectorID, InclusiveYear);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }
            else if (rid == "18")
            {
                if (OfficeID == 0)
                {
                    int[] OfficesWithSignatory = new int[] { 70, 39, 37, 38, 41 };
                    DataTable OfficeIDList = new DataTable();
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        //SqlCommand com = new SqlCommand(@"select OfficeID from tbl_R_BMSOffices where PMISOfficeID in(select distinct OfficeID from pmis.dbo.EDGE_tblPlantillaDivision) and OfficeID not in (17,9)  order by isnull(OrderNo,999999)", con);
                        SqlCommand com = new SqlCommand(@"select OfficeID,OfficeName from tbl_R_BMSOffices where PMISOfficeID is not null and OfficeID not in (17,9,43,64) and FundID in(101,119)  order by isnull(OrderNo,999999)", con);
                        con.Open();
                        OfficeIDList.Load(com.ExecuteReader());
                    }
                    for (int i = 0; i <= OfficeIDList.Rows.Count - 1; i++)
                    {
                        var PlantillaReport = new LBP3New(Convert.ToInt32(OfficeIDList.Rows[i][0]), year, OfficesWithSignatory.Contains(Convert.ToInt32(OfficeIDList.Rows[i][0])) ? 0 : 2);
                        rb.Reports.Add(PlantillaReport);
                    }
                    rs.ReportDocument = rb;
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ShowExportGroup = false;
                    RV.ReportSource = rb;

                }
                else
                {
                    rs.ReportDocument = new LBP3New(OfficeID, year, 1);
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ShowExportGroup = false;
                    RV.ReportSource = rs;
                }

            }
            else if (rid == "19")
            {
                rs.ReportDocument = new LBP2New(OfficeID, year, ReportTypeID, eid, reloadlbp2);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
            //    RV.ShowExportGroup = false;
                RV.ReportSource = rs;
            }
            else if (rid == "20")
            {
                rs.ReportDocument = new LBPForm1New(year, Fundtype, eid);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }
            else if (rid == "21")
            {
                rs.ReportDocument = new SAAO_detailed(OfficeID, month_, month_To, year, classtype, SAAO_type, Detail_type);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }
            else if (rid == "22")
            {
                rs.ReportDocument = new Realignment(OfficeID, month_, month_To, year, classtype, All_Type, realign_type);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }
            else if (rid == "23")
            {
                if (OfficeID == 0) //all offices
                {
                    DataTable OfficeID_ = new DataTable();
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@" select a.OfficeID,a.OfficeName,a.fundID,b.FundName from tbl_R_BMSOffices as a 
                                                    LEFT JOIN tbl_R_BMSFunds as b on b.FundCode = a.FundID
                                                    where isnull(a.PMISOfficeID,0) != 0 and a.fundid not in (201,0) and a.officeid <> 37 and a.officeid <> 41 and a.officeid <> 38 and a.officeid <> 43 order by  cast(isnull(a.OrderNo,999999) as integer)", con);
                        con.Open();
                        OfficeID_.Load(com.ExecuteReader());

                        for (int i = 0; i <= OfficeID_.Rows.Count - 1; i++)
                        {

                            //var ReportDocument = new LBP4New(Convert.ToInt32(OfficeID_.Rows[i][0]), year, isNonOffice, reloadlbp4);
                            //var ReportDocument = new LBP4PerFund(Convert.ToInt32(OfficeID_.Rows[i][0]), year, isNonOffice, reloadlbp4);
                            var ReportDocument = new LBP4PerFund_GroupActivity_v2(Convert.ToInt32(OfficeID_.Rows[i][0]), year, isNonOffice, reloadlbp4);
                            rbf4.Reports.Add(ReportDocument);
                        }
                       // con.Close();
                    }
                    rs.ReportDocument = rbf4;
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                   // RV.ShowExportGroup = true;
                    RV.ReportSource = rs;
                }
                else
                {
                    //rs.ReportDocument = new LBP4New(OfficeID, year, isNonOffice, reloadlbp4);
                    //rs.ReportDocument = new LBP4PerFund(OfficeID, year, isNonOffice, reloadlbp4);
                    rs.ReportDocument = new LBP4PerFund_GroupActivity_v2(OfficeID, year, isNonOffice, reloadlbp4);
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ReportSource = rs;
                }
            }
            else if (rid == "24")
            {
                //                        if (OfficeID == 0)
                //                        {
                //                            DataTable OfficeIDList = new DataTable();
                //                            using (SqlConnection con = new SqlConnection(Common.MyConn()))
                //                            {
                //                                SqlCommand com = new SqlCommand(@"select a.OfficeID,a.OfficeName,a.fundID,b.FundName from tbl_R_BMSOffices as a 
                //                                                    LEFT JOIN tbl_R_BMSFunds as b on b.FundCode = a.FundID
                //                                                    where a.PMISOfficeID != 0 order by a.fundID, isnull(a.OrderNo,999999)", con);
                //                                con.Open();
                //                                OfficeIDList.Load(com.ExecuteReader());
                //                            }
                //                            for (int x = 0; x < OfficeIDList.Rows.Count; x++)
                //                            {
                //                                var ReportDocument = new LBP2NewConsolidated(Convert.ToInt32(OfficeIDList.Rows[x][0]), year, ReportTypeID);
                //                                rb.Reports.Add(ReportDocument);    
                //                            }
                //                            rs.ReportDocument = rb;
                //                            RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                //                            RV.ReportSource = rs;
                //                        }
                //                        else
                //                        {
                //                            rs.ReportDocument = new LBP2NewConsolidated(OfficeID, year, ReportTypeID);
                //                            RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                //                            RV.ReportSource = rs;
                //                        }
                //temporary hide -xXx
                //Report Book
                if (includeLBP1 == 1 && includeLBP2_SP == 0)
                {
                    var FirstPage = new LBPForm1New(year, 1,eid);
                    rb.Reports.Add(FirstPage);
                    //var SecondPage = new LBP2NewConsolidated_Original(OfficeID, year, ReportTypeID, includeLBP1, pagnoid);
                    if (OfficeID == 0)
                    {
                        DataTable Userid = new DataTable();
                        using (SqlConnection conUser = new SqlConnection(Common.MyConn()))
                        {
                            SqlCommand com = new SqlCommand(@" Delete [IFMIS].[dbo].[tbl_T_BMSLBP2_total] where userid =" + eid + "", conUser);
                            conUser.Open();
                            Userid.Load(com.ExecuteReader());

                        }

                        DataTable OfficeID_ = new DataTable();
                        using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        {
                            if (sectoral == 1)
                            {
                                SqlCommand com = new SqlCommand(@" select a.OfficeID,a.OfficeName,a.fundID,b.FundName from tbl_R_BMSOffices as a 
                                                    LEFT JOIN tbl_R_BMSFunds as b on b.FundCode = a.FundID
                                                    where isnull(a.PMISOfficeID,0) != 0 and a.fundid <> 201 and a.officeid <> 37 and a.officeid <> 41 and a.officeid <> 38 and a.officeid <> 43 order by a.SectorID,cast(isnull(a.OrderNo,999999) as bigint)", con);
                                con.Open();
                                OfficeID_.Load(com.ExecuteReader());
                            }
                            else
                            {
                                SqlCommand com = new SqlCommand(@" select a.OfficeID,a.OfficeName,a.fundID,b.FundName from tbl_R_BMSOffices as a 
                                                    LEFT JOIN tbl_R_BMSFunds as b on b.FundCode = a.FundID
                                                    where isnull(a.PMISOfficeID,0) != 0 and a.fundid <> 201 and a.officeid <> 37 and a.officeid <> 41 and a.officeid <> 38 and a.officeid <> 43 order by cast(isnull(a.OrderNo,999999) as bigint)", con);
                                con.Open();
                                OfficeID_.Load(com.ExecuteReader());
                            }
                        }
                        if (includeCOE == 0)
                        {
                            for (int i = 0; i <= OfficeID_.Rows.Count - 1; i++)
                            {

                                var SecondPage = new LBP2NewConsolidated(Convert.ToInt32(OfficeID_.Rows[i][0]), year, ReportTypeID, includeLBP1, pagnoid, eid, includezero, reloadlbp2, sectoral,eco);
                                rb.Reports.Add(SecondPage);
                            }
                        }
                        else
                        {
                            for (int i = 0; i <= OfficeID_.Rows.Count - 1; i++)
                            {

                                var SecondPage = new LBP2NewConsolidatedCOE(Convert.ToInt32(OfficeID_.Rows[i][0]), year, ReportTypeID, includeLBP1, pagnoid, eid, includezero, reloadlbp2, sectoral);
                                rb.Reports.Add(SecondPage);
                            }
                        }
                    }
                    else
                    {
                        var SecondPage = new LBP2NewConsolidated(OfficeID, year, ReportTypeID, includeLBP1, pagnoid, eid, includezero, reloadlbp2, sectoral, eco);
                        rb.Reports.Add(SecondPage);
                    }
                }
                else if (includeLBP1 == 1 && includeLBP2_SP == 1)
                {
                    var FirstPage = new LBPForm1New(year, 1,eid);
                    var SecondPage = new LBP2NewConsolidatedSP(OfficeID, year, ReportTypeID, includeLBP1, pagnoid, includezero, eid, reloadlbp2);

                    rb.Reports.Add(FirstPage);
                    rb.Reports.Add(SecondPage);
                }
                else if (includeLBP1 == 0 && includeLBP2_SP == 1)
                {
                    var SecondPage = new LBP2NewConsolidatedSP(OfficeID, year, ReportTypeID, includeLBP1, pagnoid, includezero, eid, reloadlbp2);
                    rb.Reports.Add(SecondPage);
                }
                else
                {
                    //rs.ReportDocument = new LBP2NewConsolidated(OfficeID, year, ReportTypeID);
                    //RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    //RV.ReportSource = rs;

                    if (OfficeID == 0 && lbp2export ==0)
                    {
                        DataTable Userid = new DataTable();
                        using (SqlConnection conUser = new SqlConnection(Common.MyConn()))
                        {
                            SqlCommand com = new SqlCommand(@" Delete [IFMIS].[dbo].[tbl_T_BMSLBP2_total] where userid ="+ eid + "", conUser);
                            conUser.Open();
                            Userid.Load(com.ExecuteReader());

                        }
                        
                        DataTable OfficeID_ = new DataTable();
                        using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        {

                            if (sectoral == 1)
                            {
                                SqlCommand com = new SqlCommand(@" select a.OfficeID,a.OfficeName,a.fundID,b.FundName from tbl_R_BMSOffices as a 
                                                    LEFT JOIN tbl_R_BMSFunds as b on b.FundCode = a.FundID
                                                    where isnull(a.PMISOfficeID,0) != 0 and a.fundid <> 201 and a.officeid <> 37 and a.officeid <> 41 and a.officeid <> 38 and a.officeid <> 43 order by  a.SectorID,cast(isnull(a.OrderNo,999999) as integer)", con);
                                con.Open();
                                OfficeID_.Load(com.ExecuteReader());
                            }
                            else if (eco == 1)
                            {
                                SqlCommand com = new SqlCommand(@" select a.OfficeID,a.OfficeName,a.fundID,b.FundName from tbl_R_BMSOffices as a 
                                                    LEFT JOIN tbl_R_BMSFunds as b on b.FundCode = a.FundID
                                                    where a.officeid in (41,38,37) order by cast(isnull(a.OrderNo,999999) as integer)", con);
                                con.Open();
                                OfficeID_.Load(com.ExecuteReader());
                            }
                            else
                            {
                                SqlCommand com = new SqlCommand(@" select a.OfficeID,a.OfficeName,a.fundID,b.FundName from tbl_R_BMSOffices as a 
                                                    LEFT JOIN tbl_R_BMSFunds as b on b.FundCode = a.FundID
                                                    where isnull(a.PMISOfficeID,0) != 0 and a.fundid <> 201 and a.officeid <> 37 and a.officeid <> 41 and a.officeid <> 38 and a.officeid <> 43  and a.FundID=101  order by  cast(isnull(a.OrderNo,999999) as integer)", con);
                                con.Open();
                                OfficeID_.Load(com.ExecuteReader());
                            }
                        }
                        if (includeCOE == 0)
                        {
                            for (int i = 0; i <= OfficeID_.Rows.Count - 1; i++)
                            {

                                var ReportDocument = new LBP2NewConsolidated(Convert.ToInt32(OfficeID_.Rows[i][0]), year, ReportTypeID, includeLBP1, pagnoid, eid, includezero, reloadlbp2, sectoral, eco);
                                rb.Reports.Add(ReportDocument);
                            }
                        }
                        else
                        {
                            for (int i = 0; i <= OfficeID_.Rows.Count - 1; i++)
                            {

                                var ReportDocument = new LBP2NewConsolidatedCOE(Convert.ToInt32(OfficeID_.Rows[i][0]), year, ReportTypeID, includeLBP1, pagnoid, eid, includezero, reloadlbp2, sectoral);
                                rb.Reports.Add(ReportDocument);
                            }
                        }

                    }
                    //changed "if" statement to "else if" -- 9/27/2023 - xXx
                    else if (OfficeID == 0 && lbp2export == 1)
                    {
                        DataTable Userid = new DataTable();
                        using (SqlConnection conUser = new SqlConnection(Common.MyConn()))
                        {
                            SqlCommand com = new SqlCommand(@" Delete [IFMIS].[dbo].[tbl_T_BMSLBP2_total] where userid =" + eid + "", conUser);
                            conUser.Open();
                            Userid.Load(com.ExecuteReader());
                        }
                        if (includeCOE == 0)
                        {
                            var ReportDocument = new LBP2NewConsolidated(OfficeID, year, ReportTypeID, includeLBP1, pagnoid, eid, includezero, reloadlbp2, sectoral, eco);
                            rb.Reports.Add(ReportDocument);
                        }
                        else
                        {
                            var ReportDocument = new LBP2NewConsolidatedCOE(OfficeID, year, ReportTypeID, includeLBP1, pagnoid, eid, includezero, reloadlbp2, sectoral);
                            rb.Reports.Add(ReportDocument);
                        }

                    }

                    else
                    {
                        if (includeCOE == 0)
                        {
                            var SecondPage = new LBP2NewConsolidated(OfficeID, year, ReportTypeID, includeLBP1, pagnoid, eid, includezero, reloadlbp2, sectoral, eco);
                            rb.Reports.Add(SecondPage);
                        }
                        else
                        {
                            var SecondPage = new LBP2NewConsolidatedCOE(OfficeID, year, ReportTypeID, includeLBP1, pagnoid, eid, includezero, reloadlbp2, sectoral);
                            rb.Reports.Add(SecondPage);
                        }   
                    }
                }
                rs.ReportDocument = rb;
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ShowExportGroup = true;
                RV.ReportSource = rs;
            }
            else if (rid == "25")
            {
                rs.ReportDocument = new LBP2NewSummary(OfficeID, year, ReportTypeID);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }

            //   var rootppaid = 0;
            //var subppaid = 0;
            //var summary = 0;
            else if (rid == "26") // 20% Utilization - xXx - 2/22/2018
            {
                if (summary == 1)
                {
                    rs.ReportDocument = new TwentyPercent(year, year2, month_, monthname, rootppaid, subppaid, pgo_, summary, earmark_type);
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ReportSource = rs;
                }
                else
                {
                    rs.ReportDocument = new TwentyPercent_Details(year, year2, month_, monthname, rootppaid, subppaid, pgo_, summary, earmark_type);
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ReportSource = rs;
                }
            }
            else if (rid == "27") // 20% Utilization - xXx - 2/22/2018
            {
                if (excesscntrl == 1)
                {
                    rs.ReportDocument = new NonOfficeSubExcess(program, accountTemp, subaccount, ProposalYear, pgocntrl, monthof, exaccount, earmark_type);
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ReportSource = rs;
                }
                else
                {
                    rs.ReportDocument = new NonOfficeSub(program, accountTemp, subaccount, ProposalYear, pgocntrl, monthof, earmark_type, allsubppa);
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ReportSource = rs;
                }
            }
            else if (rid == "28") // 20% Utilization - xXx - 6/1/2018
            {

                rs.ReportDocument = new Disbursement(Disyear, OfficeID, 0, 0, 0, Dismonth);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }
            else if (rid == "29") // Ldrrmf Utilization - xXx - 9/4/2018
            {

                rs.ReportDocument = new Ldrrmf(year, month_, monthofname, excessacctval);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }
            else if (rid == "30") // Ldrrmf Utilization - xXx - 9/4/2018
            {

                string Unique_Code = UTILITIES.UniqueKey(18);
                rs.ReportDocument = new FundUtilizationNew(year, fundtypeid, Unique_Code);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }
            else if (rid == "31") // wfp  - xXx - 1/9/2019
            {
                
                rs.ReportDocument = new WFP(OfficeID, ooeid, yearof, qtr, mpoffice, empname, empid, repHistory, prepdfppt, approvedfppt, accountidlist);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ShowExportGroup = false;
                RV.ReportSource = rs;
            }
            else if (rid == "32") // lbp6  - xXx - 4/11/2019
            {

                rs.ReportDocument = new LBPF6(yearof);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }
            else if (rid == "33") // lbp7  - xXx - 4/22/2019
            {

                rs.ReportDocument = new LBPF7(yearof);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ReportSource = rs;
            }
            else if (rid == "34") // CAF
            {

                rs.ReportDocument = new CAF(cafno, year, issuedate, applynewdate, certid);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ShowExportGroup = true;
                RV.ReportSource = rs;
            }
            else if (rid == "36") // Commitment
            {
                rs.ReportDocument = new Commitment(OfficeID,program, accountcom, year, excess, accountname, commitmentthistory);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ShowExportGroup = false;
                RV.ReportSource = rs;
            }
            else if (rid == "37" || rid == "44") // WFP
            {
                //if (accountID == 0)
                //{
                //    rs.ReportDocument = new WFPNew(year, OfficeID, project_id, fundsource, OfficeName, ProjectName, funddescription, repHistory, prepby, printstatus, projectaip, fundsourcename, accountID, accountname, municipal, barangay, ooeclass, pgas_loc);
                //    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                //    RV.ShowExportGroup = true;
                //    RV.ReportSource = rs;
                //}
                //else
                //{
                if (rid == "44") //for DFPPT to WFP
                {
                    rs.ReportDocument = new WFDP(year, OfficeName, printstatus, accountID, accountname, ooeclass, wfpno, OfficeID);
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ShowExportGroup = false;
                }
                else
                {
                    if (fundid == 0)
                    { //gf
                        if (mode_trans == 1)
                        {
                            rs.ReportDocument = new WFPNew(year, OfficeID, project_id, fundsource, OfficeName, ProjectName, funddescription, repHistory, prepby, printstatus, projectaip, fundsourcename, accountID, accountname, municipal, barangay, ooeclass, pgas_loc, 0, programID, activityid, fundid, 0, specid, issupplemetalid);
                            RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                            RV.ShowExportGroup = false;
                        }
                        else
                        {
                            rs.ReportDocument = new WFPNewExcess(year, OfficeID, project_id, fundsource, OfficeName, ProjectName, funddescription, repHistory, prepby, printstatus, projectaip, fundsourcename, accountID, accountname, municipal, barangay, ooeclass, pgas_loc, 0, programID, activityid, fundid);
                            RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                            RV.ShowExportGroup = false;
                        }
                    }
                    else //tf
                    {
                        rs.ReportDocument = new WFPNewTF(year, OfficeID, project_id, fundsource, OfficeName, ProjectName, funddescription, repHistory, prepby, printstatus, projectaip, fundsourcename, accountID, accountname, municipal, barangay, ooeclass, pgas_loc, 0, programID, activityid, fundid);
                        RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                        RV.ShowExportGroup = false;
                    }
                    //  RV.ShowPrintButton = false;
                }
                    RV.ReportSource = rs;
            }
            else if (rid == "38")
            {
                if (perprogram == 1)
                {
                    rs.ReportDocument = new Control(OfficeID, year, controlno);
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ShowExportGroup = false;
                    RV.ReportSource = rs;
                }
                else
                {
                    rs.ReportDocument = new ControlPerAccount(OfficeID, year, controlno);
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ShowExportGroup = false;
                    RV.ReportSource = rs;
                }
            }
            else if (rid == "39") // MAF
            {
                rs.ReportDocument = new MAF(programID, accountID, yearof, OfficeName, OfficeID, mode, approverepHistory, source, previewid);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ShowExportGroup = true;
                RV.ReportSource = rs;
            }
            else if (rid == "40") // appropriation per fund(procurement and non-procurement)
            {
                rs.ReportDocument = new AppropriationFund(year);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ShowExportGroup = true;
                RV.ReportSource = rs;
            }
            else if (rid == "41") // appropriation per fund(procurement and non-procurement)
            {
                rs.ReportDocument = new WFPDFPPTQtr(OfficeID, yearof, qtr,mode);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ShowExportGroup = true;
                RV.ReportSource = rs;
            }
            else if (rid == "42") // supplemental budget
            {
                rs.ReportDocument = new SB3(OfficeID, year, mode);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ShowExportGroup = true;
                RV.ReportSource = rs;
            }
            else if (rid == "43") // accomplishment report
            {
                rs.ReportDocument = new Accomplishment(year, month_, month_To);
                RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                RV.ShowExportGroup = true;
                RV.ReportSource = rs;
            }
        }
    }
}
