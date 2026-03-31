using iFMIS_BMS.BusinessLayer.Layers;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using iFMIS_BMS.BusinessLayer.Connector;
using System.Data.SqlClient;
using System.Data;
using iFMIS_BMS.Base;
using System.Configuration;
using iFMIS_BMS.BusinessLayer.Models.DashBoard;
using iFMIS_BMS.BusinessLayer.Models;
using eams.Base;

namespace iFMIS_BMS.Controllers
{
    [Authorize]

    public class DashBoardController : Controller
    {
        // GET: DashBoard
        clsDBConnect db = new clsDBConnect();

        //[Authorize]
        public ActionResult Index()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc.ToString());
            foreach (var item in mnu)
            {
                if (item.MenuID == 11018)
                {
                    status = "Authorized";
                }
            }

            if (status == "Authorized")
            {

                var data = 0;
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                   
                    SqlCommand com = new SqlCommand(@"select [dashboardefault] from [IFMIS].[dbo].[tbl_R_BMSUserDashboard] where [officeid]=" + Account.UserInfo.Department + " and [usereid]=" + Account.UserInfo.eid + "", con);
                    con.Open();
                    data = Convert.ToInt16(com.ExecuteScalar());
                }
                if (data == 1)
                {

                    return View();
                }
                else
                {
                    return View("Utilization");
                }
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }
         public JsonResult ProjectedVsProposed(int? BudgetYear=0)
         {
             DashboardLayer ddl = new DashboardLayer();
             var lst = ddl.ProjectedVsProposed(BudgetYear);
             return Json(lst, JsonRequestBehavior.AllowGet);
         }
         public JsonResult OfficesWithProposal(int? BudgetYear=0)
         {
             DashboardLayer ddl = new DashboardLayer();
             var lst = ddl.OfficesWithProposal(BudgetYear);
             return Json(lst, JsonRequestBehavior.AllowGet);
         }
        
         public JsonResult TotalPSMooeCA(int? BudgetYear=0)
         {
             DashboardLayer ddl = new DashboardLayer();
             var lst = ddl.TotalPSMooeCA(BudgetYear);
             return Json(lst, JsonRequestBehavior.AllowGet);
         }
         public JsonResult TotalPSMooe(int? BudgetYear=0)
         {
             DashboardLayer ddl = new DashboardLayer();
             var lst = ddl.TotalPSMooe(BudgetYear);
             return Json(lst, JsonRequestBehavior.AllowGet);
         }
         public JsonResult TotalAllOffices(int? BudgetYear=0)
         {
             DashboardLayer ddl = new DashboardLayer();
             var lst = ddl.TotalAllOffices(BudgetYear);
             return Json(lst, JsonRequestBehavior.AllowGet);
         }
         public JsonResult SourceOfFunds(int? BudgetYear=0)
         {
             DashboardLayer ddl = new DashboardLayer();
             var lst = ddl.TotalSourceOfFunds(BudgetYear);
             return Json(lst, JsonRequestBehavior.AllowGet);
         }
         public PartialViewResult ShowOfficeListWindow(string ViewType)
        {
            ViewBag.ViewType = ViewType;
            return PartialView("pvOfficeListWindow");
        }
         public JsonResult grOfficeListData([DataSourceRequest] DataSourceRequest request, string gridData,int YearOf)
         {
             DashboardLayer Layer = new DashboardLayer();
             var lst = Layer.grOfficeListData(gridData, YearOf);
             return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
         }

         public JsonResult TotalPSMooeCAConsolidated(int? BudgetYear = 0)
         {
             DashboardLayer ddl = new DashboardLayer();
             var lst = ddl.TotalPSMooeCAConsolidated(BudgetYear);
             return Json(lst, JsonRequestBehavior.AllowGet);
         }
         public JsonResult TotalFilledVacantPosition(int? BudgetYear = 0)
         {
             DashboardLayer ddl = new DashboardLayer();
             var lst = ddl.TotalFilledVacantPosition(BudgetYear);
             return Json(lst, JsonRequestBehavior.AllowGet);
         }
        public JsonResult AppropriationSumm(int? BudgetYear = 0)
        {
            DashboardLayer ddl = new DashboardLayer();
            var lst = ddl.AppropriationSumm(BudgetYear);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FinUtilization(int? BudgetYear = 0,int? fundsource=0)
        {
            DashboardLayer ddl = new DashboardLayer();
            var lst = ddl.FinUtilization(BudgetYear, fundsource);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getutilizationsummary(int? year = 0,int? fundsource=0)
        {
            DashBoardModel data = new DashBoardModel();
            DataTable _dt = new DataTable();
            if (fundsource == 1)
            {
                string _sqlQuery = "SELECT sum(appropriation), sum(allotment) ,sum(pre_obligation),sum(obligation),sum(disbursement),sum(utilisation),format(min([datetime]),'M/d/yyyy hh:mm:ss tt')  FROM [IFMIS].[dbo].[tbl_T_BMSUtilization_Dashboard] where yearof=" + year + " and office is not null and accountid !=0 ";
                _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
                try
                {
                    //        preobligationlabel { get; set; }
                    //public decimal obligationlabel { get; set; }
                    //public decimal disbursementlabel { get; set; }
                    //public decimal consumatedlabel { get; set; }
                    data.appropriationlabel = Convert.ToDecimal(_dt.Rows[0][0]);
                    data.allotmentlabel = Convert.ToDecimal(_dt.Rows[0][1]);
                    data.preobligationlabel = Convert.ToDecimal(_dt.Rows[0][2]);
                    data.obligationlabel = Convert.ToDecimal(_dt.Rows[0][3]);
                    data.disbursementlabel = Convert.ToDecimal(_dt.Rows[0][4]);
                    data.consumatedlabel = Convert.ToDecimal(_dt.Rows[0][5]);
                    data.dateandtime = Convert.ToString(_dt.Rows[0][6]);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.Message);
                }
            }
            else
            {
                if (fundsource == 101)
                {
                    string _sqlQuery = "SELECT sum(appropriation), sum(allotment) ,sum(pre_obligation),sum(obligation),sum(disbursement),sum(utilisation),format(min([datetime]),'M/d/yyyy hh:mm:ss tt')  FROM [IFMIS].[dbo].[tbl_T_BMSUtilization_Dashboard] where yearof=" + year + " and fundid=" + fundsource + " and programid != 218 and accountid not in (59410,59802) and office is not null and accountid !=0 ";
                    _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
                    try
                    {
                        //        preobligationlabel { get; set; }
                        //public decimal obligationlabel { get; set; }
                        //public decimal disbursementlabel { get; set; }
                        //public decimal consumatedlabel { get; set; }
                        data.appropriationlabel = Convert.ToDecimal(_dt.Rows[0][0]);
                        data.allotmentlabel = Convert.ToDecimal(_dt.Rows[0][1]);
                        data.preobligationlabel = Convert.ToDecimal(_dt.Rows[0][2]);
                        data.obligationlabel = Convert.ToDecimal(_dt.Rows[0][3]);
                        data.disbursementlabel = Convert.ToDecimal(_dt.Rows[0][4]);
                        data.consumatedlabel = Convert.ToDecimal(_dt.Rows[0][5]);
                        data.dateandtime = Convert.ToString(_dt.Rows[0][6]);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(String.Empty, ex.Message);
                    }
                }
                else if (fundsource == 127)
                {
                    string _sqlQuery = "SELECT sum(appropriation), sum(allotment) ,sum(pre_obligation),sum(obligation),sum(disbursement),sum(utilisation),format(min([datetime]),'M/d/yyyy hh:mm:ss tt')  FROM [IFMIS].[dbo].[tbl_T_BMSUtilization_Dashboard] where yearof=" + year + " and fundid=101 and programid = 218 and office is not null and accountid !=0 ";
                    _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
                    try
                    {
                        //        preobligationlabel { get; set; }
                        //public decimal obligationlabel { get; set; }
                        //public decimal disbursementlabel { get; set; }
                        //public decimal consumatedlabel { get; set; }
                        data.appropriationlabel = Convert.ToDecimal(_dt.Rows[0][0]);
                        data.allotmentlabel = Convert.ToDecimal(_dt.Rows[0][1]);
                        data.preobligationlabel = Convert.ToDecimal(_dt.Rows[0][2]);
                        data.obligationlabel = Convert.ToDecimal(_dt.Rows[0][3]);
                        data.disbursementlabel = Convert.ToDecimal(_dt.Rows[0][4]);
                        data.consumatedlabel = Convert.ToDecimal(_dt.Rows[0][5]);
                        data.dateandtime = Convert.ToString(_dt.Rows[0][6]);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(String.Empty, ex.Message);
                    }
                }
                else if (fundsource == 200)
                {
                    string _sqlQuery = "SELECT sum(appropriation), sum(allotment) ,sum(pre_obligation),sum(obligation),sum(disbursement),sum(utilisation),format(min([datetime]),'M/d/yyyy hh:mm:ss tt')  FROM [IFMIS].[dbo].[tbl_T_BMSUtilization_Dashboard] where yearof=" + year + " and fundid=101 and programid = 211  and accountid in (59410,59802) and office is not null ";
                    _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
                    try
                    {
                        //        preobligationlabel { get; set; }
                        //public decimal obligationlabel { get; set; }
                        //public decimal disbursementlabel { get; set; }
                        //public decimal consumatedlabel { get; set; }
                        data.appropriationlabel = Convert.ToDecimal(_dt.Rows[0][0]);
                        data.allotmentlabel = Convert.ToDecimal(_dt.Rows[0][1]);
                        data.preobligationlabel = Convert.ToDecimal(_dt.Rows[0][2]);
                        data.obligationlabel = Convert.ToDecimal(_dt.Rows[0][3]);
                        data.disbursementlabel = Convert.ToDecimal(_dt.Rows[0][4]);
                        data.consumatedlabel = Convert.ToDecimal(_dt.Rows[0][5]);
                        data.dateandtime = Convert.ToString(_dt.Rows[0][6]);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(String.Empty, ex.Message);
                    }
                }
                else
                {
                    string _sqlQuery = "SELECT sum(appropriation), sum(allotment) ,sum(pre_obligation),sum(obligation),sum(disbursement),sum(utilisation),format(min([datetime]),'M/d/yyyy hh:mm:ss tt')  FROM [IFMIS].[dbo].[tbl_T_BMSUtilization_Dashboard] where yearof=" + year + " and fundid=" + fundsource + " and office is not null and accountid !=0 ";
                    _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
                    try
                    {
                        //        preobligationlabel { get; set; }
                        //public decimal obligationlabel { get; set; }
                        //public decimal disbursementlabel { get; set; }
                        //public decimal consumatedlabel { get; set; }
                        data.appropriationlabel = Convert.ToDecimal(_dt.Rows[0][0]);
                        data.allotmentlabel = Convert.ToDecimal(_dt.Rows[0][1]);
                        data.preobligationlabel = Convert.ToDecimal(_dt.Rows[0][2]);
                        data.obligationlabel = Convert.ToDecimal(_dt.Rows[0][3]);
                        data.disbursementlabel = Convert.ToDecimal(_dt.Rows[0][4]);
                        data.consumatedlabel = Convert.ToDecimal(_dt.Rows[0][5]);
                        data.dateandtime = Convert.ToString(_dt.Rows[0][6]);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(String.Empty, ex.Message);
                    }
                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UtilizationDenom(int? year = 0, string status = "",int? fundsource=0)
        {
            zLineUtility_Layer ddl = new zLineUtility_Layer();
            var lst = ddl.UtilizationDenom(year, status, fundsource);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult UtilizationPV(int? year = 0, string status = "",int? fundsource=0)
        {
            // ViewBag.ViewType = ViewType;
            Session["status"] = status;
            Session["fundsource"] = fundsource;
            return PartialView("pv_utilizationbreakdown");
        }
        
        public string ReloadUtilization(int? year=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [sp_bms_appropriationutilizationAll_Dashboard] "+ year + ",12", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public JsonResult UtilizationDetail([DataSourceRequest] DataSourceRequest request, int? year, int? mode,int? officeid,int? fundsource=0)
        {
            List<DashBoardModel> realign_List = new List<DashBoardModel>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec [sp_bms_appropriationutilizationAll_Detail] " + year + "," + mode + ","+ officeid + ","+ fundsource + "", con);

                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    DashBoardModel real = new DashBoardModel();
                    real.OfficeID = reader.GetInt32(0);
                    real.office = reader.GetValue(1).ToString();
                    real.appropriation = Convert.ToDouble(reader.GetValue(2));
                    real.allotment = Convert.ToDouble(reader.GetValue(3));
                    real.preobligation = Convert.ToDouble(reader.GetValue(4));
                    real.obligation = Convert.ToDouble(reader.GetValue(5));
                    real.disbursement = Convert.ToDouble(reader.GetValue(6));
                    real.accounted = Convert.ToDouble(reader.GetValue(7));
                    real.program = reader.GetValue(15).ToString();
                    real.account = reader.GetValue(17).ToString();
                    real.yearof=reader.GetInt32(18);
                    real.fundid = reader.GetInt32(19);
                    realign_List.Add(real);
                }
            }
            return Json(realign_List.ToDataSourceResult(request));
        }
        public PartialViewResult UtilizationDetailPerOffice(string fundname="", int? year=0,string status="",int? fundsource=0)
        {
            Session["fundname"] = fundname;
            Session["status"] = status;
            Session["year"] = year;
            Session["fundsource"] = fundsource;
            return PartialView("pv_utilizationbreakdownPerOffice");
        }
        public JsonResult UtilizationDetailOffice(int? year = 0, string status = "",string fundname="", int? fundsource = 0)
        {
            zLineUtility_Layer ddl = new zLineUtility_Layer();
            var lst = ddl.UtilizationDetailOffice(year, status, fundname, fundsource);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public double UtilizationPerOfficeSummary(int? year=0, string status="",string fundname="")
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = 0.00;
                    SqlCommand com = new SqlCommand(@"exec [IFMIS].[dbo].[sp_BMS_Utilization_DenominationPerOffice_summary] " + year + ",'" + status + "','" + fundname + "'", con);
                    con.Open();
                    data = Convert.ToDouble(com.ExecuteScalar());
                    return data;
                }
            }
            catch //(Exception ex)
            {
                return 0;
            }
        }
        public ActionResult FundSource([DataSourceRequest]DataSourceRequest request)
        {
            
                string SQL = "";
                SQL = "SELECT [FundCode],[FundMedium] FROM [IFMIS].[dbo].[tbl_R_BMSFunds] where [ActionCode]=1 union select 1,'ALL' union Select 200,'ESMF'  order by FundCode";
                DataTable dt = SQL.DataSet();
                var result = new ContentResult();
                result.Content = SerializeDT.DataTableToJSON(dt);
                result.ContentType = "application/json";
                return result;
            
        }
        public ActionResult AllotmentClass([DataSourceRequest]DataSourceRequest request)
        {

            string SQL = "";
            SQL = "SELECT [OOEID],[OOEAbrevation] FROM [IFMIS].[dbo].[tbl_R_BMSObjectOfExpenditure] union select 0 as ooeid,'All' as [OOEAbrevation]  order by [OOEID]";
            DataTable dt = SQL.DataSet();
            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;

        }
        public JsonResult utilizationpercategory([DataSourceRequest] DataSourceRequest request, int? year, int? percent, int? selcat,int? allclass,int? officeid)
        {
            List<DashBoardModel> realign_List = new List<DashBoardModel>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec [sp_BMS_UtilizationPerOOE] " + year + "," + percent + "," + selcat + ","+ allclass + ","+ officeid + "", con);

                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    DashBoardModel real = new DashBoardModel();
                    real.OfficeID = reader.GetInt32(0);
                    real.office = reader.GetValue(1).ToString();
                    real.accountid = Convert.ToInt32(reader.GetValue(2));
                    real.account = reader.GetValue(3).ToString();
                    real.utilize = Convert.ToDouble(reader.GetValue(4));
                    real.appropriation = Convert.ToDouble(reader.GetValue(5));
                    real.obligation = Convert.ToDouble(reader.GetValue(6));
                    real.unutilize = Convert.ToDouble(reader.GetValue(7));
                    real.num = Convert.ToInt32(reader.GetValue(8));
                    real.yearof = Convert.ToInt32(reader.GetValue(9));
                    real.prcent = Convert.ToInt32(reader.GetValue(10));
                    real.catid = Convert.ToInt32(reader.GetValue(11));
                    real.perclass = Convert.ToInt32(reader.GetValue(12));
                    real.peroffice = Convert.ToInt32(reader.GetValue(13));
                    realign_List.Add(real);
                }
            }
            return Json(realign_List.ToDataSourceResult(request));
        }
        public PartialViewResult utilizationperclass(int? classid = 0,int? percategory=0)
        {   
            if (percategory != 1) // per office
            {
                Session["office"] = classid;
                Session["allclass"] = 0;
            }
            else //per class
            {
                Session["allclass"] = classid;
                Session["office"] = 0;
            }
            return PartialView("pv_utilizationdetail");
        }

        public PartialViewResult utilizationperclass_pro(int? classid = 0, int? percategory = 0)
        {
            if (percategory != 1) // per office
            {
                Session["office"] = classid;
                Session["allclass"] = 0;
            }
            else //per class
            {
                Session["allclass"] = classid;
                Session["office"] = 0;
            }
            return PartialView("pv_utilizationdetail_pro");
        }
        public string dloadprocurementitem(int? year=0)
        {
            try
            {
                DataTable dt ;
                if (year >= 2022)
                {
                    //dt = db.execQuery("select a.transno, c.fmisid, e.programid, e.accountid, e.subaccountid, excessid, b.item, b.description, b.unit, b.unitcost, b.quantity, b.unitcost * quantity as amount  from t_pr a " +
                    //                                "join t_pr_items b on b.prid = a.prid " +
                    //                                "join a_office c on c.officeid = a.officeid " +
                    //                                "join t_pr_object d on d.prid = a.prid " +
                    //                                "join t_ppmp_object e on e.ppid = d.ppid " +
                    //                                "join t_pr_status f on f.prid = a.prid " +
                    //                                "where a.cyear = " + year + " and a.isactive = '1' and f.submitted = 1 and f.bacapproved = 1 and e.fundid != 302");
                    dt = db.execQuery("select a.transno, c.fmisid, e.programid, e.accountid, e.subaccountid, excessid, b.item, b.description, b.unit, b.unitcost, b.quantity, b.unitcost * quantity as amount, " +
                                           " case when g.prid > 0 then 1 else 0 end as earmarked from t_pr a " +
                                        "join t_pr_items b on b.prid = a.prid " +
                                        "join a_office c on c.officeid = a.officeid " +
                                        "join t_pr_object d on d.prid = a.prid " +
                                        "join t_ppmp_object e on e.ppid = d.ppid " +
                                        "join t_pr_status f on f.prid = a.prid " +
                                        "left join (select prid from( " +
                                        "select a.prid, row_number() over(partition by prid order by a.statusid) as rw from t_pr_log a " +
                                        "join l_status b on b.statusid = a.statusid " +
                                        "where a.statusid = 33 ) a where rw = 1) g on g.prid = a.prid " +
                                        "where a.cyear = " + year + " and a.isactive = '1' and f.submitted = 1 and f.bacapproved = 1 and e.fundid != 302");
                }
                else
                {
                    dt = db.execQuery("select a.transno, c.fmisid, e.programid, e.accountid, e.subaccountid, excessid, b.item, b.description, b.unit, b.unitcost, b.quantity, b.unitcost * quantity as amount  from t_pr a " +
                                                    "join t_pr_items b on b.prid = a.prid " +
                                                    "join a_office c on c.officeid = a.officeid " +
                                                    "join t_pr_object d on d.prid = a.prid " +
                                                    "join t_ppmp_object e on e.ppid = d.ppid " +
                                                    "join t_pr_status f on f.prid = a.prid " +
                                                    "where a.cyear = " + year + " and a.isactive = '1' and f.submitted = 1 and e.fundid != 302");
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand updatePPMPStats = new SqlCommand(@"UPDATE tbl_T_BMSProcurementItem set ActionCode = 2,[datetimetrans]=[datetimetrans] + ',' + format(getdate(),'MM/dd/yyyy hh:mm:ss tt'),[userid]=[UserID] +','+ '" + Account.UserInfo.eid.ToString() + "' WHERE year = '" + year + "' and ActionCode = 1", con);
                    con.Open();
                    updatePPMPStats.ExecuteNonQuery();
                }

                using (SqlConnection con2 = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand updatepro = new SqlCommand(@"UPDATE [IFMIS].[dbo].[tbl_T_BMSNonProcurement] set ActionCode = 2 WHERE [yearof] = " + year + " and ActionCode = 1", con2);
                    con2.Open();
                    updatepro.ExecuteNonQuery();

                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ppmp dataTable = new ppmp();

                    dataTable.transnopr = dt.Rows[i]["transno"].ToString();
                    dataTable.OfficeID = dt.Rows[i]["fmisid"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["fmisid"]);
                    dataTable.ProgramID = dt.Rows[i]["programid"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["programid"]);
                    dataTable.AccountID = dt.Rows[i]["accountid"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["accountid"]);
                    dataTable.SubAccountID = dt.Rows[i]["subaccountid"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["subaccountid"]);
                    dataTable.ExcessID = dt.Rows[i]["excessid"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["excessid"]);
                    dataTable.itemname = dt.Rows[i]["item"].ToString().Replace("'", "''");
                    dataTable.description = dt.Rows[i]["description"].ToString().Replace("'", "''");
                    dataTable.unit = dt.Rows[i]["unit"].ToString();
                    dataTable.unitcost = Convert.ToDouble(dt.Rows[i]["unitcost"]);
                    dataTable.qty = Convert.ToDouble(dt.Rows[i]["quantity"]);
                    dataTable.amount = Convert.ToDouble(dt.Rows[i]["amount"]);
                    dataTable.earmark= Convert.ToInt32(dt.Rows[i]["earmarked"]); 

                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand insertItem = new SqlCommand(@"insert into [IFMIS].[dbo].[tbl_T_BMSProcurementItem] ([transno],[officeid],[programid],[accountid],[subaccountid],[excessid],[item],[description],[unit],[unitcost],[qty],[amount],[actioncode],datetimetrans,userid,year,[earmark]) 
                        values('" + dataTable.transnopr + "'," + dataTable.OfficeID + ", " + dataTable.ProgramID + "," + dataTable.AccountID + "," + dataTable.SubAccountID
                            + "," + dataTable.ExcessID + ",'" + dataTable.itemname + "','" + dataTable.description + "','" + dataTable.unit + "'," + dataTable.unitcost + "," + dataTable.qty + "," + dataTable.amount + ",1,format(getdate(),'MM/dd/yyyy hh:mm:ss tt'),'" + Account.UserInfo.eid.ToString() + "'," + year + ","+ dataTable.earmark + ") ", con);
                        con.Open();
                        insertItem.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return "success";
            }
            catch //(Exception ex)
            {
                return "Something went wrong!";
            }
           
        }
        public JsonResult utilizationperprocure([DataSourceRequest] DataSourceRequest request, int? year, int? percent, int? selcat, int? allclass, int? officeid)
        {
            List<DashBoardModel> realign_List = new List<DashBoardModel>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec [sp_BMS_UtilizationProcurement] " + year + "," + percent + "," + selcat + "," + allclass + "," + officeid + "", con);

                con.Open();
                com.CommandTimeout = 0;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    DashBoardModel real = new DashBoardModel(); 
                    real.OfficeID = reader.GetInt32(0);
                    real.office = reader.GetValue(1).ToString();
                    real.accountid = Convert.ToInt32(reader.GetValue(3));
                    real.account = reader.GetValue(2).ToString();
                    real.amount_pr = Convert.ToDouble(reader.GetValue(4));
                    real.utilize_pr = Convert.ToDouble(reader.GetValue(5));
                    real.amount_ob = Convert.ToDouble(reader.GetValue(6));
                    real.utilize_ob = Convert.ToDouble(reader.GetValue(7));
                    real.appropriation_pr = Convert.ToDouble(reader.GetValue(8));
                    real.appropriation_ob = Convert.ToDouble(reader.GetValue(9));
                    real.num = Convert.ToInt32(reader.GetValue(10));
                    real.yearof = Convert.ToInt32(reader.GetValue(11));
                    real.prcent = Convert.ToInt32(reader.GetValue(12));
                    real.catid = Convert.ToInt32(reader.GetValue(13));
                    real.perclass = Convert.ToInt32(reader.GetValue(14));
                    real.peroffice = Convert.ToInt32(reader.GetValue(15));
                    realign_List.Add(real);
                }
            }
            return Json(realign_List.ToDataSourceResult(request));
        }
    }
}