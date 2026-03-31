using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using iFMIS_BMS.Reports;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Layers;
using iFMIS_BMS.Reports.Design;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Net;
using System.Data.SqlClient;
using System.Data;
using System.Web.Script.Serialization;
using System.Configuration;
using System.IO;
using iFMIS_BMS.Base;
using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation;
using iFMIS_BMS.BusinessLayer.Layers.Maintenance;
using System.Net.Mail;
using RPTIS.BASE;
using eSignature.Class;
using eams.Base;
using Telerik.Reporting;
using SPMS;
//using System.Threading.Tasks;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace iFMIS_BMS.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        // GET: Report

        clsDBConnect db = new clsDBConnect();

        //[Authorize]
        public ActionResult Index()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc.ToString());
            foreach (var item in mnu)
            {
                if (item.MenuID == 4015)
                {
                    status = "Authorized";
                }
            }

            if (status == "Authorized")
            {
                ReportModel ReportModel = new ReportModel();
                ReportModel.OfficeID = Account.UserInfo.Department;
                ReportModel.UserTypeDesc = Account.UserInfo.UserTypeDesc;

                return View("pv_ReportIndex", ReportModel);
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }
        //[HttpGet]
        //public interface IActionResult{}

        [Authorize]
        public ActionResult LBPOne()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc.ToString());
            foreach (var item in mnu)
            {
                if (item.MenuID == 11016)
                {
                    status = "Authorized";
                }
            }
            if (status == "Authorized")
            {
                return View("pv_LBPOne");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }



        [Authorize]
        public ActionResult SOOReport()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc.ToString());
            foreach (var item in mnu)
            {
                if (item.MenuID == 9017)
                {
                    status = "Authorized";
                }
            }

            if (status == "Authorized")
            {
                ReportModel ReportModel = new ReportModel();
                ReportModel.OfficeID = Account.UserInfo.Department;
                ReportModel.UserTypeDesc = Account.UserInfo.UserTypeDesc;
                ReportModel.OfficeName = Account.UserInfo.Department;
                return View("pv_SOOReport", ReportModel);
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }
        public void send_SOO(int OfficeID, int Year, string OfficeName)
        {
            rpt_SOO.OfficeID = OfficeID;
            rpt_SOO.Year = Year;
            rpt_SOO.OfficeName = OfficeName;

        }



        public ActionResult pv_AIPReport()
        {
            ReportModel ReportModel = new ReportModel();
            ReportModel.OfficeID = Account.UserInfo.Department;
            ReportModel.UserTypeDesc = Account.UserInfo.UserTypeDesc;
            return View("pv_AIPReport");
        }
        [Authorize]
        public ActionResult PrintPlantillaReportIndex()
        {
            return View("pv_PlantillaReportIndex");
        }

        public string DownloadPPMPNew(int? officeid = 0, int? transyear = 0)
        {
            //temp hide - xXx - 10/28/2022
            //var offid = Account.UserInfo.Department.ToString();
            //DataTable dt = db.execQuery("select * from ( " +
            //                "select a.isactive, a.cyear, d.fmisid, c.objid, c.programid, c.accountid, b.itemname, b.unit, b.unitcost, b.qty1, b.qty2, b.qty3, b.qty4, (b.qty1 + b.qty2 + b.qty3 + b.qty4) as totalqty, " +
            //                " b.m1, b.m2, b.m3, b.m4, b.m5, b.m6, b.m7, b.m8, b.m9, b.m10, b.m11, b.m12, " +
            //                "(b.unitcost * (b.qty1 + b.qty2 + b.qty3 + b.qty4)) as amount,b.description " +
            //                "from t_ppmp a " +
            //                "join t_ppmp_items_vw b on b.ppid = a.ppid " +
            //                "join t_ppmp_object c on c.ppid = a.ppid " +
            //                "join a_office d on d.officeid = a.officeid " +
            //                "Union All " +
            //                "select a.isactive, a.cyear, d.fmisid, c.objid, c.programid, c.accountid, b.itemname, b.unit, b.unitcost, b.qty1cost as qty1, b.qty2cost as qty2, b.qty3cost as qty3, b.qty4cost as qty4, 0 as totalqty, " +
            //                "0 as m1, 0 as m2, 0 as m3, 0 as m4, 0 as m5, 0 as m6, 0 as m7, 0 as m8, 0 as m9, 0 as m10, 0 as m11, 0 as m12, " +
            //                "(b.qty1cost + b.qty2cost + b.qty3cost + b.qty4cost) as amount,'' as description " +
            //                "from t_ppmp a " +
            //                "join t_ppmp_itemslumpsum_vw b on b.ppid = a.ppid " +
            //                "join t_ppmp_object c on c.ppid = a.ppid " +
            //                "join a_office d on d.officeid = a.officeid " +
            //                ") a where a.isactive = '1' and fmisid = " + officeid + " and cyear = " + transyear + "");

            //using (SqlConnection con = new SqlConnection(Common.MyConn()))
            //{
            //    SqlCommand updatePPMPStats = new SqlCommand(@"UPDATE tbl_T_BMSPPMP set ActionCode = 2,[DateTimeEntered]=[DateTimeEntered] + ',' + format(getdate(),'MM/dd/yyyy hh:mm:ss tt'),[UserID]=[UserID] +','+ '" + Account.UserInfo.eid.ToString() + "' WHERE OfficeID = '" + officeid + "' and TransactionYear = '" + transyear + "' and isPPMP = 1 and ActionCode = 1", con);
            //    con.Open();
            //    updatePPMPStats.ExecuteNonQuery();


            //}

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    ppmp dataTable = new ppmp();
            //    dataTable.itemname = dt.Rows[i]["itemname"].ToString().Replace("'", "''");
            //    dataTable.amount = dt.Rows[i]["amount"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["amount"]);
            //    dataTable.unitcost = Convert.ToDouble(dt.Rows[i]["unitcost"]);
            //    dataTable.unit = dt.Rows[i]["unit"].ToString();
            //    dataTable.description = dt.Rows[i]["description"].ToString().Replace("'", "''");
            //    var qty1 = dt.Rows[i]["qty1"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["qty1"]);
            //    var qty2 = dt.Rows[i]["qty2"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["qty2"]);
            //    var qty3 = dt.Rows[i]["qty3"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["qty3"]);
            //    var qty4 = dt.Rows[i]["qty4"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["qty4"]);
            //    double Total = dt.Rows[i]["totalqty"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["totalqty"]);
            //    var ProgramID = dt.Rows[i]["programid"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["programid"]);
            //    var AccountID = dt.Rows[i]["accountid"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["accountid"]);
            //    var m1 = dt.Rows[i]["unit"].ToString().ToUpper() == "LUMPSUM" ? 1 :Convert.ToDouble(dt.Rows[i]["m1"]);// != 0 ? 1 : 0;
            //    var m2 = Convert.ToDouble(dt.Rows[i]["m2"]);// != 0 ? 1 : 0;
            //    var m3 = Convert.ToDouble(dt.Rows[i]["m3"]);// != 0 ? 1 : 0;
            //    var m4 = Convert.ToDouble(dt.Rows[i]["m4"]); //!= 0 ? 1 : 0;
            //    var m5 = Convert.ToDouble(dt.Rows[i]["m5"]);// != 0 ? 1 : 0;
            //    var m6 = Convert.ToDouble(dt.Rows[i]["m6"]); //!= 0 ? 1 : 0;
            //    var m7 = Convert.ToDouble(dt.Rows[i]["m7"]);// != 0 ? 1 : 0;
            //    var m8 = Convert.ToDouble(dt.Rows[i]["m8"]);// != 0 ? 1 : 0;
            //    var m9 = Convert.ToDouble(dt.Rows[i]["m9"]);// != 0 ? 1 : 0;
            //    var m10 = Convert.ToDouble(dt.Rows[i]["m10"]);// != 0 ? 1 : 0;
            //    var m11 = Convert.ToDouble(dt.Rows[i]["m11"]);// != 0 ? 1 : 0;
            //    var m12 = Convert.ToDouble(dt.Rows[i]["m12"]);// != 0 ? 1 : 0;

            //    using (SqlConnection con = new SqlConnection(Common.MyConn()))
            //    {
            //        SqlCommand insertItem = new SqlCommand(@"insert into tbl_T_BMSPPMP ([DenominationName],[DenominationAmount],[DateTimeEntered],[UserID],[ActionCode],[TransactionYear],[ProgramID],[AccountID],[isPPMP],[QuantityOrPercentage],[TotalAmount],[OfficeID],[Month],[m1],[m2],[m3],[m4],[m5],[m6],[m7],[m8],[m9],[m10],[m11],[m12],[unit],[description]) 
            //            values('" + dataTable.itemname + "'," + (dataTable.unitcost == 0 ? dataTable.amount : dataTable.unitcost) + ", '" + DateTime.Now + "'," + Account.UserInfo.eid
            //            + ",1," + transyear + "," + ProgramID + "," + AccountID + " ,1, " + (Total == 0 ? 0 : Total) + "," + dataTable.amount + "," + officeid + ",1, "+
            //             m1 +","+ m2 + "," + m3 + "," + m4 + "," + m5 + "," + m6 + "," + m7 + "," + m8 + "," + m9 + "," + m10 + "," + m11 + "," + m12 + ",'"+ dataTable.unit + "','"+ dataTable.description + "')", con);
            //        con.Open();
            //        insertItem.ExecuteNonQuery();
            //        con.Close();
            //    }

            //}
            DataTable dt2 = db.execQuery("select a.itemid,a.itemname,0 ,c.itemgroup as maincategory, b.itemgroup as subcategory from l_item as a " +
                                               "join l_item_group as b on a.itemgroupid = b.itemgroupid " +
                                               "join l_item_group as c on c.itemgroupid = b.parentid " +
                                               "where a.isactive = '1' " +
                                               "ORDER BY c.itemgroup");
            for (Int32 x = 0; x < dt2.Rows.Count; x++)
            {
                ppmp dataTable2 = new ppmp();

                ppmp dataTable = new ppmp();
                dataTable.itemid = Convert.ToInt32(dt2.Rows[x]["itemid"]);
                dataTable.itemname = Convert.ToString(dt2.Rows[x]["itemname"]).Replace("'", "''").ToString();
                dataTable.itemgroupname = Convert.ToString(dt2.Rows[x]["maincategory"]).Replace("'", "''").ToString();
                dataTable.itemsubgroupname = Convert.ToString(dt2.Rows[x]["subcategory"]).Replace("'", "''").ToString();

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand insertItem = new SqlCommand(@"insert into [tbl_R_BMS_PPMPCategory] ([item_id],[item],[itemgroup_id],[itemgroup_main],[itemgroup_sub]) 
                        values(" + dataTable.itemid + ",'" + dataTable.itemname + "',0,'" + dataTable.itemgroupname + "','" + dataTable.itemsubgroupname + "')", con);
                    con.Open();
                    insertItem.ExecuteNonQuery();
                    con.Close();
                }
            }
            return "success";
        }
        public ActionResult PrintLBP2New()
        {
            return View("pv_LBP2ReportIndex");
        }
        public ActionResult PrintLBP3New()
        {
            return View("pv_LBP3NewReportIndex");
        }
        [Authorize]
        public ActionResult ReportLBP5()
        {
            return View("pvReportLBP5");
        }
        public ActionResult PrintReport()
        {
            return Redirect("~/Reports/ReportViewer.aspx?rid=1&req=" + DateTime.Now + "");
        }
        public ActionResult PrintReportLBP5()
        {
            return Redirect("~/Reports/ReportViewer.aspx?rid=3&req=" + DateTime.Now + "");
        }
        public void PrintReportData(int OfficeID, string OfficeName, int Year, string ReportID)
        {
            //LBP3.OfficeID = OfficeID;
            //LBP3.OfficeName = OfficeName;
            //LBP3.Year = Year;
            //LBP3.ReportID = ReportID;
        }
        public ActionResult PrintOrdinanceReport()
        {
            return View("pv_OrdinanceReportIndex");
        }
        public void PrintReportDataLBP5(int OfficeID, string OfficeName, int Year, string ReportID)
        {
            LBP5.ReportID = ReportID;
            LBP5.OfficeID = OfficeID;
            LBP5.OfficeNameReport = OfficeName;
            LBP5.Year = Year;
        }
        public ActionResult Print_SOO()
        {
            return Redirect("~/Reports/ReportViewer.aspx?rid=4&req=" + DateTime.Now + "");
        }
        public void SendDataAIP(int OfficeID, int Year)
        {
            rpt_AIP.OfficeID = OfficeID;
            rpt_AIP.Year = Year;
        }
        public ActionResult PrintReportAIP()
        {
            return Redirect("~/Reports/ReportViewer.aspx?rid=2&req=" + DateTime.Now + "");
        }
        public ActionResult pv_OtherSourcesTABSTRIP()
        {
            return View("pv_OtherSourcesTABSTRIP");
        }

        public JsonResult GetTrundsName()
        {
            Trunds_Layer ddl = new Trunds_Layer();
            var lst = ddl.TrundsType();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult pv_AddTrundsViews(int? Year)
        {

            return PartialView("pv_AddTrundsViews");
        }


        public string SaveTrunds(int? Trans_ID, string Trans_name, int? Trunds_Amount, int? Year)
        {
            Trunds_Layer NewTrunds = new Trunds_Layer();
            if (Trans_ID == 0 || Trans_ID == null)
            {
                return NewTrunds.AddNewTrundsName(Trans_ID, Trans_name, Trunds_Amount, Year);
            }
            else
            {
                return NewTrunds.AddNewTrunds(Trans_ID, Trans_name, Trunds_Amount, Year);
            }
        }

        public JsonResult LoadTrunds([DataSourceRequest] DataSourceRequest request, int? Year)
        {
            Trunds_Layer NewIS = new Trunds_Layer();
            var lst = NewIS.Trunds(Year);

            return Json(lst.ToDataSourceResult(request));
        }
        public PartialViewResult EditTrunds(int? TransAmount_ID)
        {
            Trunds_Layer NewTrunds = new Trunds_Layer();
            return PartialView("pv_UpdateTrundsViews", NewTrunds.EditTrunds(TransAmount_ID));
        }

        [HttpPost]
        public string UpdateTrunds(TransFundsModel tra)
        {
            Trunds_Layer el = new Trunds_Layer();

            return el.UpdateTrunds(tra);
        }

        public ActionResult pv_GrantsViews()
        {
            return View("pv_GrantsViews");
        }
        public ActionResult pv_OtherViews()
        {
            return View("pv_OtherViews");
        }

        public PartialViewResult pv_AddGrantsViews(int? Year)
        {

            return PartialView("pv_AddGrantsViews");
        }

        public string SaveGrants(string Grants_Name, int? Grants_Amount, int? Year)
        {
            GrantsLayer NewGrants = new GrantsLayer();
            return NewGrants.AddNewGrants(Grants_Name, Grants_Amount, Year);
        }
        public JsonResult LoadGrants([DataSourceRequest] DataSourceRequest request, int? Year)
        {
            GrantsLayer NewIS = new GrantsLayer();
            var lst = NewIS.Grants(Year);

            return Json(lst.ToDataSourceResult(request));
        }

        public PartialViewResult EditGrants(int? grants_id)
        {
            GrantsLayer NewTrunds = new GrantsLayer();
            return PartialView("pv_UpdateGrantsView", NewTrunds.EditGrants(grants_id));
        }

        [HttpPost]
        public string UpdateGrants(GrantsModel gra)
        {
            GrantsLayer el = new GrantsLayer();

            return el.UpdateGrants(gra);
        }

        public PartialViewResult pv_AddOthersViews(int? Year)
        {

            return PartialView("pv_AddOthersViews");
        }

        public string SaveOthers(string Others_Name, int? Others_Amount, int? Year)
        {
            OthersLayer NewGrants = new OthersLayer();
            return NewGrants.AddNewOthers(Others_Name, Others_Amount, Year);
        }


        public JsonResult LoadOthers([DataSourceRequest] DataSourceRequest request, int? Year)
        {
            OthersLayer NewIS = new OthersLayer();
            var lst = NewIS.Others(Year);

            return Json(lst.ToDataSourceResult(request));
        }



        public PartialViewResult EditOthers(int? other_id)
        {
            OthersLayer NewTrunds = new OthersLayer();
            return PartialView("pv_UpdateOthersView", NewTrunds.EditOthers(other_id));
        }
        [HttpPost]
        public string UpdateOthers(OthersModel oth)
        {
            OthersLayer el = new OthersLayer();

            return el.UpdateOthers(oth);
        }


        public ActionResult pv_TrustViews()
        {
            return View("pv_TrustViews");
        }



        //public ActionResult pv_OtherSourcesTABSTRIP()
        //{
        //    return View("pv_OtherSourcesTABSTRIP");
        //}

        //public JsonResult GetTrundsName()
        //{
        //    Trunds_Layer ddl = new Trunds_Layer();
        //    var lst = ddl.TrundsType();

        //    return Json(lst, JsonRequestBehavior.AllowGet);
        //}


        public string RemoveTrust(int? TransAmount_ID)
        {
            Trunds_Layer NewIS = new Trunds_Layer();
            var lst = NewIS.RemoveTrunds(TransAmount_ID);
            return lst;
        }
        public string RemoveGrants(int? grants_id)
        {
            Trunds_Layer NewIS = new Trunds_Layer();
            var lst = NewIS.RemoveGrants(grants_id);
            return lst;
        }
        public string RemoveOthers(int? other_id)
        {
            Trunds_Layer NewIS = new Trunds_Layer();
            var lst = NewIS.RemoveOthers(other_id);
            return lst;
        }



        public ActionResult SOOnon([DataSourceRequest] DataSourceRequest request, int? propYear)
        {
            SooNon_Layer SooNoN = new SooNon_Layer();
            var lst = SooNoN.SOONON(propYear);

            return Json(lst.ToDataSourceResult(request));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<SOONon_Model> products, int? propYear)
        {
            SooNon_Layer SooNoNUps = new SooNon_Layer();
            if (products != null)
            {
                foreach (var product in products)
                {
                    SooNoNUps.SOONONUPS(product, propYear);
                }
            }

            return Json(products.ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<SOONon_Model> products, int? propYear)
        {
            SooNon_Layer SooNoNUps = new SooNon_Layer();
            if (products.Any())
            {
                foreach (var product in products)
                {
                    SooNoNUps.SOODESTROY(product, propYear);
                }
            }

            return Json(products.ToDataSourceResult(request));
        }
        public PartialViewResult pv_AddNon(int? Year)
        {

            return PartialView("pv_AddNon");
        }
        public JsonResult GetNonNames(int? propYear)
        {
            SooNon_Layer ddl = new SooNon_Layer();
            var lst = ddl.NonType(propYear);

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public string SaveNon(int? trnno, string BudgetAcctName, int? Year)
        {
            SooNon_Layer NewNon = new SooNon_Layer();

            return NewNon.AddNon(trnno, BudgetAcctName, Year);

        }

        public string RemoveNon(int? trnno, int? YearN)
        {
            Trunds_Layer NewIS = new Trunds_Layer();
            var lst = NewIS.RemoveNon(trnno, YearN);
            return lst;
        }

        //public ActionResult LBPOne()
        //{
        //    return View("pv_LBPOne");
        //}
        public JsonResult LoadSF([DataSourceRequest] DataSourceRequest request, int? Year_of)
        {
            LBPOne_Layer LoadSource = new LBPOne_Layer();
            var lst = LoadSource.LoadSF(Year_of);
            return Json(lst.ToDataSourceResult(request));
        }
        public PartialViewResult pv_AddSourceViews(int? Year_of)
        {

            return PartialView("pv_AddSourceViews");
        }

        public JsonResult GetSF()
        {
            LBPOne_Layer ddl = new LBPOne_Layer();
            var lst = ddl.SFtype();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public string SaveSource(int? Fund_Desc_ID, string Fund_Desc, int? Year_of)
        {
            LBPOne_Layer NewTrunds = new LBPOne_Layer();
            if (Fund_Desc_ID == 0 || Fund_Desc_ID == null)
            {
                return NewTrunds.AddNewTrundsName(Fund_Desc_ID, Fund_Desc, Year_of);
            }
            else
            {
                return NewTrunds.AddNewTrunds(Fund_Desc_ID, Fund_Desc, Year_of);
            }
        }

        [HttpGet]
        public PartialViewResult SourceFunds(int? Fund_ID, int? Year_of)
        {
            Session["Fund_ID"] = Fund_ID;
            Session["Year_of"] = Year_of;
            return PartialView("pv_LPBOne_Window");
        }
        //public JsonResult grOfficeAccounts([DataSourceRequest] DataSourceRequest request, int? propYear1, int? officeID, int ProgramID)
        //{
        //    OfficeAdmin_Layer el = new OfficeAdmin_Layer();
        //    var lst = el.grOfficeProgram(Convert.ToInt32(Session["ProgID1"]), propYear1, officeID, ProgramID);
        //    return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}
        public JsonResult ReadTF([DataSourceRequest] DataSourceRequest request, int? Fund_ID, int? Year_of)
        {
            LBPOne_Layer LoadSource = new LBPOne_Layer();
            var lst = LoadSource.ReadTF(Fund_ID, Year_of);
            return Json(lst.ToDataSourceResult(request));
        }
        public JsonResult readSub([DataSourceRequest] DataSourceRequest request, int? Type_ID, int? Year_Of)
        {
            LBPOne_Layer LoadSource = new LBPOne_Layer();
            var lst = LoadSource.readSub(Type_ID, Year_Of);
            return Json(lst.ToDataSourceResult(request));
        }
        public PartialViewResult NewType(int? Fund_ID, int? Year_of)
        {
            return PartialView("pv_NewType");
        }
        public JsonResult RType()
        {
            LBPOne_Layer ddl = new LBPOne_Layer();
            var lst = ddl.RType();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        //add types
        public string Save_RType(int? Type_Desc_ID, string Type_Desc, int? Fund_ID, int? Year_of)
        {
            LBPOne_Layer NewType = new LBPOne_Layer();
            if (Type_Desc_ID == 0 || Type_Desc_ID == null)
            {
                return NewType.addRTypeName(Type_Desc_ID, Type_Desc, Fund_ID, Year_of);
            }
            else
            {
                return NewType.addRType(Type_Desc_ID, Type_Desc, Fund_ID, Year_of);
            }
        }
        public PartialViewResult NewSubbAdd(int? Type_ID, int? Year_Of)
        {
            Session["Type_ID"] = Type_ID;
            Session["Year_Of"] = Year_Of;
            return PartialView("pv_NewSubbAdd");
        }


        public JsonResult SubCombo()
        {
            LBPOne_Layer ddl = new LBPOne_Layer();
            var lst = ddl.SubCombo();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }



        public JsonResult SubCombo2()
        {
            LBPOne_Layer ddl = new LBPOne_Layer();
            var lst = ddl.SubCombo2();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        //add subs
        public string Save_Subs(int? Sub1_Desc_ID, string Sub1_Desc, int? Sub2_Desc_ID, string Sub2_Desc, double Past_year, double Current_year, double Budget_year, int? Type_ID, int? Year_Of)
        {
            LBPOne_Layer newsubs = new LBPOne_Layer();
            //new sub1 new sub 2
            if ((Sub1_Desc_ID == 0 || Sub1_Desc_ID == null) && (Sub2_Desc_ID == 0 || Sub2_Desc_ID == null))
            {
                return newsubs.new1new2(Sub1_Desc_ID, Sub1_Desc, Sub2_Desc_ID, Sub2_Desc, Past_year, Current_year, Budget_year, Type_ID, Year_Of);
            }
            //new sub 1 old sub 2
            else if ((Sub1_Desc_ID == 0 || Sub1_Desc_ID == null) && (Sub2_Desc_ID != 0 || Sub2_Desc_ID != null))
            {
                return newsubs.new1old2(Sub1_Desc_ID, Sub1_Desc, Sub2_Desc_ID, Sub2_Desc, Past_year, Current_year, Budget_year, Type_ID, Year_Of);
            }
            //old sub 1 new sub 2
            else if ((Sub1_Desc_ID != 0 || Sub1_Desc_ID != null) && (Sub2_Desc_ID == 0 || Sub2_Desc_ID == null))
            {
                return newsubs.old1new1(Sub1_Desc_ID, Sub1_Desc, Sub2_Desc_ID, Sub2_Desc, Past_year, Current_year, Budget_year, Type_ID, Year_Of);
            }
            //old sub 1 old sub 2
            else
            {
                return newsubs.old1old1(Sub1_Desc_ID, Sub1_Desc, Sub2_Desc_ID, Sub2_Desc, Past_year, Current_year, Budget_year, Type_ID, Year_Of);
            }
        }
        public string RemoveSF(int? Fund_ID, int? Year_of)
        {
            LBPOne_Layer NewIS = new LBPOne_Layer();
            var lst = NewIS.RemoveSF(Fund_ID, Year_of);
            return lst;
        }

        public string RemoveTY(int? Type_ID, int? Year_of)
        {
            LBPOne_Layer NewIS = new LBPOne_Layer();
            var lst = NewIS.RemoveTY(Type_ID, Year_of);
            return lst;
        }

        public PartialViewResult editSubs(int? Sub2_ID, int? Year_of, int? TypeID)
        {
            Session["TypeID"] = TypeID;
            LBPOne_Layer NewTrunds = new LBPOne_Layer();
            return PartialView("pv_editSubs", NewTrunds.editSubs(Sub2_ID, Year_of));
        }


        public string UpdateSub(int? Sub2_ID, string Sub1_Desc, string Sub2_Desc, double Past_year, double Current_year, double Budget_year, int? Type_ID, int? Year_Of)
        {
            LBPOne_Layer newsubs = new LBPOne_Layer();

            return newsubs.UpdateSub(Sub2_ID, Sub1_Desc, Sub2_Desc, Past_year, Current_year, Budget_year, Type_ID, Year_Of);

        }
        public string RemoveSubs(int? Sub2_ID, int? Year_Of)
        {
            LBPOne_Layer NewIS = new LBPOne_Layer();
            var lst = NewIS.RemoveSubs(Sub2_ID, Year_Of);
            return lst;
        }

        public PartialViewResult pv_AddParticularViews(int? Year_of)
        {

            return PartialView("pv_AddParticularViews");
        }

        public JsonResult GetEE()
        {
            LBPOne_Layer ddl = new LBPOne_Layer();
            var lst = ddl.EEtype();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public string SaveParticular(int? Eco_Desc_ID, string Eco_Desc, int? Year_of)
        {
            LBPOne_Layer NewParti = new LBPOne_Layer();
            if (Eco_Desc_ID == 0 || Eco_Desc_ID == null)
            {
                return NewParti.AddNewParticularName(Eco_Desc_ID, Eco_Desc, Year_of);
            }
            else
            {
                return NewParti.AddNewParticular(Eco_Desc_ID, Eco_Desc, Year_of);
            }
        }
        public JsonResult LoadEE([DataSourceRequest] DataSourceRequest request, int? Year_of)
        {
            LBPOne_Layer LoadParticular = new LBPOne_Layer();
            var lst = LoadParticular.LoadEE(Year_of);
            return Json(lst.ToDataSourceResult(request));
        }
        [HttpGet]
        public PartialViewResult Particulars(int? Eco_ID, int? Year_of)
        {
            Session["Eco_ID"] = Eco_ID;
            Session["Year_of"] = Year_of;
            return PartialView("pv_Particulars_Window");
        }



        public JsonResult ReadEETF([DataSourceRequest] DataSourceRequest request, int? Eco_ID, int? Year_of)
        {
            LBPOne_Layer LoadSource = new LBPOne_Layer();
            var lst = LoadSource.ReadEETF(Eco_ID, Year_of);
            return Json(lst.ToDataSourceResult(request));
        }


        public JsonResult readPartSub([DataSourceRequest] DataSourceRequest request, int? Eco_Type_ID, int? Year_Of)
        {
            LBPOne_Layer LoadSource = new LBPOne_Layer();
            var lst = LoadSource.readPartSub(Eco_Type_ID, Year_Of);
            return Json(lst.ToDataSourceResult(request));
        }


        public PartialViewResult NewTypeEE(int? Fund_ID, int? Year_of)
        {
            return PartialView("pv_NewTypeEE");
        }


        public JsonResult RTypeEE()
        {
            LBPOne_Layer ddl = new LBPOne_Layer();
            var lst = ddl.RTypeEE();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        //add types
        public string Save_RTypeEE(int? Eco_Type_Desc_ID, string Eco_Type_Desc, int? Eco_ID, int? Year_of)
        {
            LBPOne_Layer NewType = new LBPOne_Layer();
            if (Eco_Type_Desc_ID == 0 || Eco_Type_Desc_ID == null)
            {
                return NewType.addRTypeEEName(Eco_Type_Desc_ID, Eco_Type_Desc, Eco_ID, Year_of);
            }
            else
            {
                return NewType.addRTypeEE(Eco_Type_Desc_ID, Eco_Type_Desc, Eco_ID, Year_of);
            }
        }



        //_______________________________________ subs EE _________________________

        public PartialViewResult NewSubbAddEE(int? Eco_Type_ID, int? Year_Of)
        {
            Session["Eco_Type_ID"] = Eco_Type_ID;
            Session["Year_Of"] = Year_Of;
            return PartialView("pv_NewSubbAddEE");
        }


        public JsonResult SubComboEE()
        {
            LBPOne_Layer ddl = new LBPOne_Layer();
            var lst = ddl.SubComboEE();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SubCombo2EE()
        {
            LBPOne_Layer ddl = new LBPOne_Layer();
            var lst = ddl.SubCombo2EE();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SubCombo3EE()
        {
            LBPOne_Layer ddl = new LBPOne_Layer();
            var lst = ddl.SubCombo3EE();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        //add subs
        public string Save_SubsEE(int? EE_Sub1_Desc_ID, string EE_Sub1_Desc, int? EE_Sub2_Desc_ID, string EE_Sub2_Desc, double Past_year, double Current_year, double Budget_year, int? Eco_Type_ID, int? Year_Of, int? Class_ID, string Class_Desc, string Account_Code)
        {
            LBPOne_Layer newsubsEE = new LBPOne_Layer();
            //new sub1 new sub 2
            if ((EE_Sub1_Desc_ID == 0 || EE_Sub1_Desc_ID == null) && (EE_Sub2_Desc_ID == 0 || EE_Sub2_Desc_ID == null))
            {
                return newsubsEE.new1new2EE(EE_Sub1_Desc_ID, EE_Sub1_Desc, EE_Sub2_Desc_ID, EE_Sub2_Desc, Past_year, Current_year, Budget_year, Eco_Type_ID, Year_Of, Class_ID, Class_Desc, Account_Code);
            }
            //new sub 1 old sub 2
            else if ((EE_Sub1_Desc_ID == 0 || EE_Sub1_Desc_ID == null) && (EE_Sub2_Desc_ID != 0 || EE_Sub2_Desc_ID != null))
            {
                return newsubsEE.new1old2EE(EE_Sub1_Desc_ID, EE_Sub1_Desc, EE_Sub2_Desc_ID, EE_Sub2_Desc, Past_year, Current_year, Budget_year, Eco_Type_ID, Year_Of, Class_ID, Class_Desc, Account_Code);
            }
            //old sub 1 new sub 2
            else if ((EE_Sub1_Desc_ID != 0 || EE_Sub1_Desc_ID != null) && (EE_Sub2_Desc_ID == 0 || EE_Sub2_Desc_ID == null))
            {
                return newsubsEE.old1new1EE(EE_Sub1_Desc_ID, EE_Sub1_Desc, EE_Sub2_Desc_ID, EE_Sub2_Desc, Past_year, Current_year, Budget_year, Eco_Type_ID, Year_Of, Class_ID, Class_Desc, Account_Code);
            }
            //old sub 1 old sub 2
            else
            {
                return newsubsEE.old1old1EE(EE_Sub1_Desc_ID, EE_Sub1_Desc, EE_Sub2_Desc_ID, EE_Sub2_Desc, Past_year, Current_year, Budget_year, Eco_Type_ID, Year_Of, Class_ID, Class_Desc, Account_Code);
            }
        }




        public PartialViewResult editSubsEE(int? EE_Sub2_ID, int? Year_of, int? ECO_ID)
        {
            Session["Eco_Type_ID"] = ECO_ID;
            LBPOne_Layer EditSubss = new LBPOne_Layer();
            return PartialView("pv_editSubsEE", EditSubss.editSubsEE(EE_Sub2_ID, Year_of));
        }

        public string UpdateSubEE(int? EE_Sub2_ID, string EE_Sub1_Desc, string EE_Sub2_Desc, double Year1_AmountEE, double Year2_AmountEE, double Year3_AmountEE, int? Eco_Type_ID, int? Year_Of, string Account_Code, int? Class_ID)
        {
            LBPOne_Layer newsubs = new LBPOne_Layer();

            return newsubs.UpdateSubEE(EE_Sub2_ID, EE_Sub1_Desc, EE_Sub2_Desc, Year1_AmountEE, Year2_AmountEE, Year3_AmountEE, Eco_Type_ID, Year_Of, Account_Code, Class_ID);

        }

        public string RemoveTYEE(int? Eco_Type_ID, int? Year_Of)
        {
            LBPOne_Layer NewIS = new LBPOne_Layer();
            var lst = NewIS.RemoveTYEE(Eco_Type_ID, Year_Of);
            return lst;
        }

        public string RemoveSubsEE(int? EE_Sub2_ID, int? Year_Of)
        {
            LBPOne_Layer NewIS = new LBPOne_Layer();
            var lst = NewIS.RemoveSubsEE(EE_Sub2_ID, Year_Of);
            return lst;
        }



        public string RemoveSFEE(int? Eco_ID, int? Year_Of)
        {
            LBPOne_Layer NewIS = new LBPOne_Layer();
            var lst = NewIS.RemoveSFEE(Eco_ID, Year_Of);
            return lst;
        }
        public ActionResult AIPReportIndex()
        {
            return View("pv_AIPReportIndex");
        }
        public ActionResult LBP4NewReportIndex()
        {
            return View("pv_LBP4NewReportIndex");
        }
        public string GenerateForm1Data(int YearOf)
        {
            LBPOne_Layer NewIS = new LBPOne_Layer();
            return NewIS.GenerateForm1Data(YearOf);
        }
        public ActionResult LBPForm1ReportIndex()
        {
            Session["ueid"] = Account.UserInfo.eid;
            return View("pv_LBP1NewReportIndex");
        }
        public ActionResult TwentyUtilization()
        {
            return View("pv_TwenUtilization");
        }
        public ActionResult GetPPAs([DataSourceRequest]DataSourceRequest request, int actualrootppaid = 0, Boolean rootppa = false, int ddlYear = 0)
        {
            //select * from fn_ppas (2018)
            string tempStr = "exec sp_ppas " + actualrootppaid + "," + ddlYear + "," + rootppa + "";
            DataTable dt = tempStr.MemisDataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult NonOffice()
        {
            return View("pv_NonOffice");
        }
        public ActionResult GetOffice([DataSourceRequest]DataSourceRequest request, int selaccountid = 0, long accountid = 0, int yearof = 0, int status = 0, int officeid_temp = 0)
        {
            string tempStr = "";
            if (selaccountid == 1)
            {
                tempStr = "select OfficeID, CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') as offname from tbl_R_BMSOffices order BY officeName ";
            }
            else
            {
                if (status == 0)
                {
                    //tempStr = "SELECT [accountnameid] as OfficeID,[accountname]  as offname FROM [IFMIS].[dbo].[tbl_R_BMSNonOffice] where actioncode=1 and yearof=" + yearof + " and accountid=" + accountid + " and [officeid]=" + officeid_temp + " order by accountname desc ";
                    tempStr = "SELECT accountnameid as OfficeID,[accountname]  as offname " +
                              " FROM[IFMIS].[dbo].[tbl_R_BMSNonOffice] as xxx " +
                              "where xxx.actioncode = 1 and xxx.yearof = " + yearof + " and [officeid]=" + officeid_temp + " and xxx.accountid = " + accountid + " and xxx.excess = 0 " +
                              "and xxx.accountnameid not in (Select top 1 xx.nonofficeidparent from[IFMIS].[dbo].[tbl_R_BMSNonOffice] as xx " +
                              "where xx.nonofficeidparent=xxx.accountnameid and xx.actioncode=1 and [officeid]=" + officeid_temp + " and xx.accountid=" + accountid + " and xx.yearof= " + yearof + ") " +
                              " and xxx.accountnameid not in (Select top 1 xx.nonofficeidmain from [IFMIS].[dbo].[tbl_R_BMSNonOffice] as xx " +
                              "where xx.nonofficeidmain= xxx.accountnameid and xx.actioncode= 1 and [officeid]=" + officeid_temp + " and xx.accountid= " + accountid + " and xx.yearof= " + yearof + " and xx.nonofficeidparent<> 0) " +
                              "order by accountname";
                }
                else
                {
                    tempStr = "SELECT [accountnameid] as OfficeID,[accountname]  as offname FROM [IFMIS].[dbo].[tbl_R_BMSNonOffice] where actioncode=1 and accountid=" + accountid + " and excess=" + status + " order by accountname desc ";
                }
            }
            DataTable dt = tempStr.DataSet();
            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult GetOfficeWFP([DataSourceRequest]DataSourceRequest request, int? officeid = 0, int? fundid = 0)
        {
            var offid = Account.UserInfo.Department.ToString();

            if (fundid == 0) //general fund
            {
                if (Account.UserInfo.UserTypeID >= 4) //lfc / super admin
                {
                    string tempStr = "select OfficeID, CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') as offname from tbl_R_BMSOffices where isnull([PMISOfficeID],0) <> 0 order BY officeName ";
                    DataTable dt = tempStr.DataSet();

                    var result = new ContentResult();
                    result.Content = SerializeDT.DataTableToJSON(dt);
                    result.ContentType = "application/json";
                    return result;
                }
                else
                {
                    string tempStr = "select OfficeID, CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') as offname from tbl_R_BMSOffices where isnull([PMISOfficeID],0) <> 0 and ([OfficeID] ='" + offid + "' " +
                        " or [OfficeID] in (Select a.[officeid] from [IFMIS].[dbo].[tbl_R_BMSDfpptUser] as a where a.[userid] = " + Account.UserInfo.eid + " and isnull(a.actioncode,0)=1)) order BY officeName ";
                    DataTable dt = tempStr.DataSet();

                    var result = new ContentResult();
                    result.Content = SerializeDT.DataTableToJSON(dt);
                    result.ContentType = "application/json";
                    return result;
                }
            }
            else //trust fund
            {
                string tempStr = "select OfficeID, CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') as offname from tbl_R_BMSOffices_TF where isnull([PMISOfficeID],0) <> 0 order BY officeName ";
                DataTable dt = tempStr.DataSet();

                var result = new ContentResult();
                result.Content = SerializeDT.DataTableToJSON(dt);
                result.ContentType = "application/json";
                return result;
            }
        }
        public ActionResult GetOfficeWFPDFPPT([DataSourceRequest]DataSourceRequest request, int? year = 0)
        {
            var offid = Account.UserInfo.Department.ToString();

            string tempStr = "exec ifmis.dbo.sp_BMS_WFPDFPPT_Office " + year + "";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult GetProgram([DataSourceRequest]DataSourceRequest request, int? year = 0, int? office = 0)
        {
            string tempStr = "select Distinct ProgramID,ProgramDescription from tbl_R_BMSOfficePrograms where ProgramYear = " + year + " and OfficeID = " + office + " and ActionCode=1 order by ProgramDescription";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult GetAccount([DataSourceRequest]DataSourceRequest request, int? year = 0, int? program = 0, int? tempradid = 0, int? allsubppa = 0)
        {

            //string tempStr = "SELECT distinct AccountID,AccountName FROM IFMIS.dbo.tbl_R_BMSProgramAccounts where AccountYear = " + year + " and ProgramID = " + program + " and ActionCode=1 order by AccountName";
            string tempStr = "exec sp_BMS_Accountwithsubppa " + program + ", " + year + "," + allsubppa + "";

            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;

        }

        public ActionResult GetAccountPPAS([DataSourceRequest]DataSourceRequest request, int? year = 0, int? program = 0, int? account = 0, int? excessid = 0, int? tempradid = 0)
        {
            //string tempStr = "SELECT distinct AccountID,AccountName FROM IFMIS.dbo.tbl_R_BMSProgramAccounts where AccountYear = " + year + " and ProgramID = " + program + " order by AccountName";
            string tempStr = "exec sp_bms_PPASDropdown " + year + "," + program + ",'" + tempradid + "'";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult GetAccountactual([DataSourceRequest]DataSourceRequest request, int? year = 0, int? program = 0)
        {
            string tempStr = "select AccountID,AccountName from ifmis.dbo.tbl_R_BMSProgramAccounts where ProgramID=" + program + " and AccountYear=" + year + " and ActionCode=1 order by AccountName";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }

        public ActionResult GetSubAccount([DataSourceRequest]DataSourceRequest request, int year = 0, int program = 0, int accountTemp = 0, int exaccount = 0, int exaccountoption = 0, int? allsubppa = 0)
        {

            string tempStr = "exec IFMIS.dbo.[sp_BMS_DropdownNonOffice_Rao] " + program + "," + accountTemp + "," + year + "," + exaccountoption + "," + exaccount + "," + allsubppa + "";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;

        }
        public ActionResult Utilization()
        {
            return View("pv_Disbursement");
        }
        public int getooeid(int? programID = 0, long? accountID = 0, int? year_ = 0)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (year_ >= 2017)
                {
                    SqlCommand com = new SqlCommand(@"Select [ObjectOfExpendetureID]  from [tbl_R_BMSProgramAccounts] where actioncode=1 and programid=" + programID + " and accountid=" + accountID + " and accountyear=" + year_ + "", con);
                    con.Open();
                    return Convert.ToInt32(com.ExecuteScalar().ToString());
                }
                else
                {
                    SqlCommand com = new SqlCommand(@"Select [OOECode]  from [fmis].[dbo].[tblBMS_AnnualBudget_Account] where actioncode=1 and [FMISProgramCode]=" + programID + " and [FMISAccountCode]=" + accountID + " and [YearOf]=" + year_ + "", con);
                    con.Open();
                    return Convert.ToInt32(com.ExecuteScalar().ToString());
                }

            }
        }
        public ActionResult FundUtilization()
        {
            return View("pv_FundUtilization");
        }

        public ActionResult GetExessAccount([DataSourceRequest]DataSourceRequest request, int accountTemp = 0, int year = 0)
        {

            string tempStr = "exec sp_BMS_ExcessAppropriationList ";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;

        }
        public ActionResult PrintWFP()
        {
            return View("pv_WFP");
        }

        public JsonResult GetWFPDetail([DataSourceRequest] DataSourceRequest request, int? office = 0, int? ooeid = 0, int? propyear = 0, int? qtr = 0)
        {
            OthersLayer gad = new OthersLayer();
            var lst = gad.GetWFPDetail(office, ooeid, propyear, qtr);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public string SaveWFPDetails(int? officeid = 0, int? programid = 0, long? accountid = 0, string mfo = "", string firstmon = "", string secondmon = "", string thirdmon = "", int? yearof = 0, string officemfo = "", string firstmonFin = "", string secondmonFin = "", string thirdmonFin = "", int quarter = 0, int mfoid = 0)

        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec dbo.sp_BMS_InsertWFPdetails " + officeid + ", " + programid + "," + accountid + ", '" + mfo.Replace("'", "''") + "','" + firstmon.Replace("'", "''") + "','" + secondmon.Replace("'", "''") + "','" + thirdmon.Replace("'", "''") + "'," + yearof + ",'" + officemfo.Replace("'", "''") + "','" + firstmonFin + "','" + secondmonFin + "','" + thirdmonFin + "'," + Account.UserInfo.eid.ToString() + "," + quarter + "," + mfoid + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public ActionResult GetEmployeePerOffice([DataSourceRequest]DataSourceRequest request)
        {
            //string tempStr = "select OfficeID, CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') as offname from tbl_R_BMSOffices where isnull([PMISOfficeID],0) <> 0 and ([OfficeID] ='" + offid + "' " +
            //       " or [OfficeID] in (Select a.[officeid] from [IFMIS].[dbo].[tbl_R_BMSDfpptUser] as a where a.[userid] = " + Account.UserInfo.eid + " and isnull(a.actioncode,0)=1)) order BY officeName ";

            var offid = Account.UserInfo.Department.ToString();
            string tempStr = "Select [eid],[EmpNameFull] from [pmis].[dbo].[vwMergeAllEmployee] where  Department in (SELECT [PMISOfficeID] FROM [IFMIS].[dbo].[tbl_R_BMSOffices]  where OfficeID='" + Account.UserInfo.Department.ToString() + "' " +
                " or [Department] in (Select a.[officeid] from [IFMIS].[dbo].[tbl_R_BMSDfpptUser] as a where a.[userid] = " + Account.UserInfo.eid + " and isnull(a.actioncode,0)=1)) order by [EmpNameFull]";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public string DownloadPPMP(int officeid, int transyear)
        {
            PPMPdata_Layer getPPMP = new PPMPdata_Layer();
            return getPPMP.DownloadPPMP(officeid, transyear);
        }
        public ActionResult GetReportHistory([DataSourceRequest]DataSourceRequest request, int? officeid = 0, int? yearof = 0, int? qtr = 0)
        {
            var offid = Account.UserInfo.Department.ToString();
            string tempStr = "Select dfpt_id,datetimentered FROM [tbl_T_BMSDFPT_xml] where officeid=" + officeid + " and yearof=" + yearof + " and qtr =" + qtr + " and actioncode=1  and isnull(datetimentered,'') <> '' order by cast(dfpt_id as bigint) desc";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult GetFundingSource([DataSourceRequest]DataSourceRequest request)
        {
            //try
            //{
            //temporary hide while source is not yet uploaded to TFS
            //string tempStr = "exec IFMIS.dbo.[sp_BMS_DropdownNonOffice] " + program + "," + account + "," + year + "";
            string tempStr = "Select * from fn_fundsource()";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
            //}
            //catch
            //{
            //    return Content("0;0");
            //}

        }
        public int getofficeid(int? officeid = 0)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select [PMISOfficeID] from tbl_R_BMSOffices where [OfficeID] =" + officeid + "", con);
                con.Open();
                return Convert.ToInt32(com.ExecuteScalar().ToString());
            }
        }
        public ActionResult LBP6()
        {
            return View("pv_LBP6");
        }
        public ActionResult LBP7()
        {
            return View("pv_LBP7");
        }
        public JsonResult GetAppropriationSummary([DataSourceRequest] DataSourceRequest request, int? yearof = 0, int? monof = 0, int? earmark = 0, int? officeid = 0, int classtype = 0)
        {
            zLineUtility_Layer gad = new zLineUtility_Layer();
            var lst = gad.GetAppropriationSummary(yearof, monof, earmark, officeid);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult AppropriationProportion()
        {
            return View("pv_Appropriatinproportion");
        }
        public JsonResult GetAppProportionDetail([DataSourceRequest] DataSourceRequest request, int? yearof = 0, int? accountid = 0)
        {
            OthersLayer gad = new OthersLayer();
            var lst = gad.GetAppProportionDetail(yearof, accountid);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAppProportionBreakdown([DataSourceRequest] DataSourceRequest request, string offname = "", int? accountid = 0)
        {
            OthersLayer gad = new OthersLayer();
            var lst = gad.GetAppProportionBreakdown(offname, accountid);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetNOAccount([DataSourceRequest]DataSourceRequest request, int? year = 0, int? program = 0, int? account = 0, int? excessid = 0, int? tempradid = 0)
        {

            string tempStr = "select [nonofficeid],nonofficeidparent,[accountname],officeid,programid,accountid,accountnameid from [IFMIS].[dbo].[tbl_R_BMSNonOffice] where [ProgramID]=" + program + " and [accountid]=" + account + " and [YearOf]=" + year + " and actioncode=1 and excess=" + excessid + " order by accountname";
            //string tempStr = "SELECT [nonofficeid],nonofficeidparent,[accountname],officeid,programid,accountid,accountnameid " +
            //                  " FROM[IFMIS].[dbo].[tbl_R_BMSNonOffice] as xxx " +
            //                  "where xxx.actioncode = 1 and xxx.yearof = " + year + " and programid = " + program + " and xxx.accountid = " + account + " and xxx.excess = "+ excessid + " " +
            //                  "and xxx.accountnameid not in (Select top 1 xx.nonofficeidparent from[IFMIS].[dbo].[tbl_R_BMSNonOffice] as xx " +
            //                  "where xx.nonofficeidparent=xxx.accountnameid and xx.actioncode=1 and programid = " + program + " and xx.accountid=" + account + " and xx.yearof= "+ year + ") " +
            //                  " and xxx.accountnameid not in (Select top 1 xx.nonofficeidmain from [IFMIS].[dbo].[tbl_R_BMSNonOffice] as xx " +
            //                  "where xx.nonofficeidmain= xxx.accountnameid and xx.actioncode= 1 and programid = " + program + " and xx.accountid= " + account + " and xx.yearof= " + year + " and xx.nonofficeidparent<> 0) " +
            //                  "order by accountname";

            DataTable dt = tempStr.DataSet();
            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }

        public ActionResult GetNOAccountWFP([DataSourceRequest]DataSourceRequest request, int? year = 0, int? program = 0, int? account = 0, int? excessid = 0, int? tempradid = 0)
        {

            //string tempStr = "select [nonofficeid],nonofficeidparent,[accountname],officeid,programid,accountid,accountnameid from [IFMIS].[dbo].[tbl_R_BMSNonOffice] where [ProgramID]=" + program + " and [accountid]=" + account + " and [YearOf]=" + year + " and actioncode=1 and excess=" + excessid + " order by accountname";
            string tempStr = "";
            if (excessid == 1) //continuing
            {
                tempStr = "SELECT [nonofficeid],nonofficeidparent,[accountname],officeid,programid,accountid,accountnameid " +
                                  " FROM[IFMIS].[dbo].[tbl_R_BMSNonOffice] as xxx " +
                                  "where xxx.actioncode = 1 and xxx.accountid = " + account + " and xxx.excess = " + excessid + " " +
                                  "and xxx.accountnameid not in (Select top 1 xx.nonofficeidparent from[IFMIS].[dbo].[tbl_R_BMSNonOffice] as xx " +
                                  "where xx.nonofficeidparent=xxx.accountnameid and xx.actioncode=1 and xx.accountid=" + account + " ) " +
                                  " and xxx.accountnameid not in (Select top 1 xx.nonofficeidmain from [IFMIS].[dbo].[tbl_R_BMSNonOffice] as xx " +
                                  "where xx.nonofficeidmain= xxx.accountnameid and xx.actioncode= 1 and xx.accountid= " + account + " and xx.nonofficeidparent<> 0) " +
                                  "order by accountname";

                DataTable dt = tempStr.DataSet();
                var result = new ContentResult();
                result.Content = SerializeDT.DataTableToJSON(dt);
                result.ContentType = "application/json";
                return result;
            }
            else
            {
                if (account != 2861)
                {
                    tempStr = "SELECT [nonofficeid],nonofficeidparent,[accountname],officeid,programid,accountid,accountnameid " +
                                      " FROM[IFMIS].[dbo].[tbl_R_BMSNonOffice] as xxx " +
                                      "where xxx.actioncode = 1 and xxx.yearof = " + year + " and programid = " + program + " and xxx.accountid = " + account + " and xxx.excess = " + excessid + " " +
                                      "and xxx.accountnameid not in (Select top 1 xx.nonofficeidparent from[IFMIS].[dbo].[tbl_R_BMSNonOffice] as xx " +
                                      "where xx.nonofficeidparent=xxx.accountnameid and xx.actioncode=1 and programid = " + program + " and xx.accountid=" + account + " and xx.yearof= " + year + ") " +
                                      " and xxx.accountnameid not in (Select top 1 xx.nonofficeidmain from [IFMIS].[dbo].[tbl_R_BMSNonOffice] as xx " +
                                      "where xx.nonofficeidmain= xxx.accountnameid and xx.actioncode= 1 and programid = " + program + " and xx.accountid= " + account + " and xx.yearof= " + year + " and xx.nonofficeidparent<> 0) " +
                                      "order by accountname";
                }
                else
                {
                    tempStr = "SELECT [ppaid] as accountnameid,[description] as accountname FROM [memis].[dbo].[tblPPAs]  where ppayear=" + year + " and withchildppa=0";
                }
                DataTable dt = tempStr.DataSet();
                var result = new ContentResult();
                result.Content = SerializeDT.DataTableToJSON(dt);
                result.ContentType = "application/json";
                return result;
            }
        }

        public ActionResult utilizationAccounted([DataSourceRequest]DataSourceRequest request, int? year = 0, int? month = 0, int? office = 0, int? byaccount = 0)
        {
            string SQL = "";
            SQL = "exec[sp_bms_appropriationutilization_accounted] " + year + "," + month + "," + office + "," + byaccount + ",0";
            DataTable dt = SQL.DataSet();

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;

            //var result = new ContentResult();
            //result.Content = SerializeDT.DataTableToJSON(dt);
            //result.ContentType = "application/json";
            //return result;
        }
        //public JsonResult GetSAAODA(int? yearof = 0, int? office = 0)
        //{
        //    zLineUtility_Layer ddl = new zLineUtility_Layer();
        //    var lst = ddl.GetSAAODA(yearof, office);
        //    return Json(lst, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult GetSAAODA([DataSourceRequest]DataSourceRequest request, int? yearof = 0, int? office = 0, int? month = 0, int? byaccount = 0)
        {
            //zLineUtility_Layer ddl = new zLineUtility_Layer();
            //var lst = ddl.GetSAAODA(yearof, office);
            //return Json(lst, JsonRequestBehavior.AllowGet);

            string SQL = "";
            SQL = "exec[sp_bms_appropriationutilization_accounted_graph] " + yearof + "," + month + "," + office + "," + byaccount + ",0";
            DataTable dt = SQL.DataSet();
            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public string ReloadSAAODA(int? year = 0, int? month = 0, int? office = 0, int? byaccount = 0)

        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [sp_bms_appropriationutilization_accounted] " + year + "," + month + "," + office + "," + byaccount + ",1", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public ActionResult GetAppropriationSummaryEC([DataSourceRequest]DataSourceRequest request, int? yearof = 0, int? monof = 0, int? earmark = 0, int? officeid = 0, int classtype = 0)
        {
            string SQL = "";
            SQL = "exec [sp_Monthly_SAAO_REPORTAll_led] " + officeid + "," + yearof + "," + classtype + "," + monof + "";
            DataTable dt = SQL.DataSet();

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult GetOfficeOtherPPA([DataSourceRequest]DataSourceRequest request)
        {
            string tempStr = "";
            tempStr = "select OfficeID, CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') as offname from tbl_R_BMSOffices where PMISOfficeID is not null order BY officeName ";
            DataTable dt = tempStr.DataSet();
            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult GetSupplemental([DataSourceRequest]DataSourceRequest request)
        {
            var offid = Account.UserInfo.Department.ToString();
            string tempStr = "select distinct isnull(SuplementalNo,0) as SuplementalNo from spms.dbo.spms_tblDepartmentBudgetaryRequirements group by SuplementalNo order by SuplementalNo";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult rephistory([DataSourceRequest]DataSourceRequest request, int? type = 0, int? office = 0, int? year = 0, int? saaotag = 0)
        {
            string tempStr = "";
            tempStr = "exec sp_bms_repotXML " + type + "," + office + "," + year + "," + saaotag + " ";
            DataTable dt = tempStr.DataSet();
            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        //public double totalallocation(int? programid = 0, int? accountid = 0, int? transyear = 0, int? qtr = 0, long? trnno = 0)
        //{
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(Common.MyConn()))
        //        {
        //            var data = 0.00;
        //            SqlCommand com = new SqlCommand(@" select dbo.[fn_BMS_DfpptAllocationTotal] (" + programid + "," + accountid + "," + transyear + "," + qtr + "," + trnno + ")", con);
        //            con.Open();
        //            data = Convert.ToDouble(com.ExecuteScalar());
        //            return data;
        //        }
        //    }
        //    catch 
        //    {
        //        return 0.00;
        //    }
        //}

        public JsonResult totalallocation(int? programid = 0, int? accountid = 0, int? transyear = 0, int? qtr = 0, long? trnno = 0, int? selmon = 0)
        {

            WFPSubmitted data = new WFPSubmitted();
            DataTable _dt = new DataTable();
            string _sqlQuery = "exec sp_BMS_DfpptAllocationDenominationTotal " + programid + "," + accountid + "," + transyear + "," + @qtr + "," + trnno + "," + selmon + "";
            _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
            try
            {
                data.TotaldenoAmount = Convert.ToDouble(_dt.Rows[0][0]); //baliktad and term
                data.totalamount = Convert.ToDouble(_dt.Rows[0][1]);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public string submitdfppt(int? office = 0, int? ooeid = 0, int? tyear = 0, int? qtr = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@" sp_BMS_SubmitDFPPT " + office + "," + ooeid + "," + tyear + "," + qtr + " ", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public PartialViewResult dfpptlist(int? office = 0, int? tyear = 0, int? id = 0, int? ooeid = 0, int? qtr = 0)
        {
            Session["office"] = office;
            Session["tyear"] = tyear;
            Session["id"] = id;
            Session["ooeid"] = ooeid;
            Session["qtr"] = qtr;
            return PartialView("pv_DfpptSubmit");
        }

        //public ActionResult GetDFPPTPlist([DataSourceRequest]DataSourceRequest request, int? office = 0, int? tyear = 0,int? id=0, int? ooeid = 0, int? qtr = 0)
        //{
        //    string SQL = "";
        //    SQL = "exec[sp_BMS_DfpptList] " + office + "," + tyear + ","+ id + ","+ ooeid  + ","+ qtr + "";
        //    DataTable dt = SQL.DataSet();
        //    var serializer = new JavaScriptSerializer();
        //    var result = new ContentResult();
        //    result.Content = serializer.Serialize(dt.ToDataSourceResult(request)); //SerializeDT.DataTableToJSON(dt);
        //    result.ContentType = "application/json";
        //    return result;
        //}

        public JsonResult GetDFPPTPlist([DataSourceRequest]DataSourceRequest request, int? office = 0, int? tyear = 0, int? id = 0, int? ooeid = 0, int? qtr = 0)
        {
            //string SQL = "";
            //SQL = "exec[sp_BMS_DfpptList] " + office + "," + tyear + "," + id + "," + ooeid + "," + qtr + "";
            //DataTable dt = SQL.DataSet();
            //var serializer = new JavaScriptSerializer();
            //var result = new ContentResult();
            //result.Content = serializer.Serialize(dt.ToDataSourceResult(request)); //SerializeDT.DataTableToJSON(dt);
            //result.ContentType = "application/json";
            //return result;
            List<WFPSubmitted> prog = new List<WFPSubmitted>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec[sp_BMS_DfpptList] " + office + "," + tyear + "," + id + "," + ooeid + "," + qtr + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WFPSubmitted emp = new WFPSubmitted();
                    emp.trnno = Convert.ToInt32(reader.GetValue(0));
                    emp.accountid = Convert.ToInt64(reader.GetValue(1));
                    emp.AccountName = reader.GetValue(2).ToString();
                    emp.fistamount = Convert.ToDouble(reader.GetValue(3));
                    emp.secondamount = Convert.ToDouble(reader.GetValue(4));
                    emp.thirdamount = Convert.ToDouble(reader.GetValue(5));
                    emp.datetimentered = reader.GetValue(6).ToString();
                    emp.Qtr = reader.GetValue(7).ToString();
                    emp.ooe = reader.GetValue(8).ToString();
                    emp.firstmon_release = Convert.ToDouble(reader.GetValue(9));
                    emp.secondmon_release = Convert.ToDouble(reader.GetValue(10));
                    emp.thirdmon_release = Convert.ToDouble(reader.GetValue(11));
                    emp.remarks = reader.GetValue(12).ToString();
                    emp.submitdatetime = reader.GetValue(13).ToString();
                    prog.Add(emp);
                }

            }
            return Json(prog.ToDataSourceResult(request));// prog;
        }

        public JsonResult GetDFPPTApprove([DataSourceRequest]DataSourceRequest request, int? office = 0, int? tyear = 0, int? id = 0, int? ooeid = 0, int? qtr = 0)
        {

            List<WFPSubmitted> prog = new List<WFPSubmitted>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec[sp_BMS_DfpptApprove] " + office + "," + tyear + ",1," + ooeid + "," + qtr + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WFPSubmitted emp = new WFPSubmitted();
                    emp.trnno = Convert.ToInt32(reader.GetValue(0));
                    emp.accountid = Convert.ToInt64(reader.GetValue(1));
                    emp.AccountName = reader.GetValue(2).ToString();
                    emp.fistamount = Convert.ToDouble(reader.GetValue(3));
                    emp.secondamount = Convert.ToDouble(reader.GetValue(4));
                    emp.thirdamount = Convert.ToDouble(reader.GetValue(5));
                    emp.datetimentered = reader.GetValue(6).ToString();
                    emp.Qtr = reader.GetValue(7).ToString();
                    emp.ooe = reader.GetValue(8).ToString();
                    emp.firstmon_release = Convert.ToDouble(reader.GetValue(9));
                    emp.secondmon_release = Convert.ToDouble(reader.GetValue(10));
                    emp.thirdmon_release = Convert.ToDouble(reader.GetValue(11));
                    emp.remarks = reader.GetValue(12).ToString();
                    prog.Add(emp);
                }

            }
            return Json(prog.ToDataSourceResult(request));// prog;
        }

        public string Editing_Update()
        {
            return "success";
        }
        public string returndfppt(long? trnno = 0, string remarks = "")
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"update [IFMIS].[dbo].[tbl_T_BMSAllocation] set [submit]=0,remarks='" + remarks.Replace("'", "''") + "' where app_id =" + trnno + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return "success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string disapprovefppt(long? trnno = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"update [IFMIS].[dbo].[tbl_T_BMSAllocation] set [approve]=0,[approvedatetime]=NULL,[approveby]=NULL where app_id =" + trnno + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return "success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public ActionResult GetOfficeWFPSubmit([DataSourceRequest]DataSourceRequest request, int? tyear = 0)
        {

            string tempStr = "select * from dbo.fn_officedffpt (" + tyear + ") order by offname ";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public JsonResult GetDfpptSubmitted([DataSourceRequest] DataSourceRequest request, int? office = 0, int? ooeid = 0, int? propyear = 0, int? qtr = 0)
        {

            List<WFPSubmitted> prog = new List<WFPSubmitted>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec [sp_bms_WFP_submitted] " + office + "," + ooeid + "," + propyear + "," + qtr + ",'','','','','','','','',0,1,0", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WFPSubmitted emp = new WFPSubmitted();
                    emp.trnno = Convert.ToInt32(reader.GetValue(18));
                    emp.objectexpenditure = reader.GetValue(15).ToString();
                    emp.Appropriation = Convert.ToDouble(reader.GetValue(0));
                    emp.Reserved = Convert.ToDouble(reader.GetValue(1));
                    emp.Netprogram = Convert.ToDouble(reader.GetValue(5));
                    emp.Firstmonth = Convert.ToDouble(reader.GetValue(10));
                    emp.Secondmonth = Convert.ToDouble(reader.GetValue(11));
                    emp.Thirdmonth = Convert.ToDouble(reader.GetValue(12));
                    emp.MFO = reader.GetValue(21).ToString();
                    emp.PTFirstmonth = reader.GetValue(22).ToString();
                    emp.PTSecondmonth = reader.GetValue(23).ToString();
                    emp.PTThirdmonth = reader.GetValue(24).ToString();
                    emp.officeid = Convert.ToInt32(reader.GetValue(13));
                    emp.programid = Convert.ToInt32(reader.GetValue(19));
                    emp.accountid = Convert.ToInt64(reader.GetValue(14));
                    emp.officemfo = reader.GetValue(26).ToString();
                    emp.AppropriationNet = Convert.ToDouble(reader.GetValue(5));
                    emp.qtr = Convert.ToInt32(reader.GetValue(28));
                    emp.firstmonth_rel = Convert.ToDouble(reader.GetValue(29));
                    emp.secondmonth_rel = Convert.ToDouble(reader.GetValue(30));
                    emp.thirdmonth_rel = Convert.ToDouble(reader.GetValue(31));
                    emp.transno = Convert.ToInt64(reader.GetValue(32));
                    emp.approve = Convert.ToInt32(reader.GetValue(33));
                    prog.Add(emp);
                }

            }
            return Json(prog.ToDataSourceResult(request));// prog;
        }
        public string Releasedfppt(int? officeid = 0, int? programid = 0, long? accountid = 0, int? firstmon_check = 0, int? secondmon_check = 0, int? thirdmon_check = 0, double firstmonFin = 0.00, double secondmonFin = 0.00, double thirdmonFin = 0.00, int? qtr = 0, int? TYear = 0, int? batchno = 0, int? trnno = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [IFMIS].[dbo].[sp_bms_releasedfppt] " + officeid + "," + programid + "," + accountid + "," + firstmon_check + "," + secondmon_check + "," + thirdmon_check + "," + firstmonFin + "," + secondmonFin + "," + thirdmonFin + "," + qtr + "," + TYear + "," + Account.UserInfo.eid + "," + batchno + "," + trnno + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public PartialViewResult dfpptlistPerOffice(int? office = 0, int? tyear = 0, int? id = 0, int? ooeid = 0, int? qtr = 0)
        {
            Session["office"] = office;
            Session["tyear"] = tyear;
            Session["id"] = id;
            Session["ooeid"] = ooeid;
            Session["qtr"] = qtr;
            return PartialView("pv_DfpptSubmitConsol");
        }
        public PartialViewResult dfpptapprove(int? office = 0, int? tyear = 0, int? id = 0, int? ooeid = 0, int? qtr = 0)
        {
            Session["office"] = office;
            Session["tyear"] = tyear;
            Session["id"] = id;
            Session["ooeid"] = ooeid;
            Session["qtr"] = qtr;
            return PartialView("pv_DfpptApprove");
        }

        public PartialViewResult dfpptapproveoffice(int? office = 0, int? tyear = 0, int? id = 0, int? ooeid = 0, int? qtr = 0)
        {
            Session["office"] = office;
            Session["tyear"] = tyear;
            Session["id"] = id;
            Session["ooeid"] = ooeid;
            Session["qtr"] = qtr;
            return PartialView("pv_DfpptApproveOffice");
        }
        public ActionResult GetReportSupplementalHistory([DataSourceRequest]DataSourceRequest request, int? officeid = 0, int? monthof = 0)
        {
            //  var offid = Account.UserInfo.Department.ToString();
            //string tempStr = "Select [mafid],format([datetime],'MM/dd/yyyy hh:mm:ss tt') as datetime FROM ifmis.dbo.[tbl_T_BMSMAF_xml] where [officeid]=" + officeid + " and year(datetime)=year(getdate()) order by cast(datetime as datetime) desc";
            string tempStr = "Select min([mafid]) as mafid,datetime from (Select  [mafid],(mafno + ' - '+format([datetime],'MM/dd/yyyy hh:mm:ss tt')) as datetime FROM ifmis.dbo.[tbl_T_BMSMAF_xml] where [officeid]=" + officeid + " and year(datetime)=year(getdate())) as xx group by datetime order by mafid";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public string deletedfppt(int? trnno = 0, int? office = 0, long? accountid = 0, int? transyear = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec sp_BMS_DfpptPrepRemove " + trnno + "," + Account.UserInfo.eid + "," + office + "," + accountid + "," + transyear + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string ApproveDFPPT(string[] transno)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("transno");
                var idx = 0;
                foreach (var trnno in transno)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = transno[idx];
                    dt.Rows.Add(dr);
                    idx++;
                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("dbo.sp_BMS_ApproveDFPPT", con);
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add(new SqlParameter("@trnno", dt));
                    com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                    con.Open();

                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public PartialViewResult getppmpdetails(int? office = 0, int? tyear = 0, int? id = 0, int? ooeid = 0, int? qtr = 0, int? accountid = 0)
        {
            Session["office"] = office;
            Session["tyear"] = tyear;
            Session["id"] = id;
            Session["ooeid"] = ooeid;
            Session["qtr"] = qtr;
            Session["accountid"] = accountid;
            return PartialView("pv_DfpptPPMPdetails");
        }
        public JsonResult GetPPMPdenomination([DataSourceRequest]DataSourceRequest request, int? office = 0, int? tyear = 0, int? id = 0, int? ooeid = 0, int? qtr = 0, int? accountid = 0)
        {

            List<WFPSubmitted> prog = new List<WFPSubmitted>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec[sp_BMS_DfpptPPMPdetails] " + office + "," + tyear + "," + id + "," + ooeid + "," + qtr + "," + accountid + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WFPSubmitted emp = new WFPSubmitted();
                    emp.trnno = Convert.ToInt32(reader.GetValue(0));
                    emp.AccountName = reader.GetValue(1).ToString();
                    emp.denoAmount = Convert.ToDouble(reader.GetValue(2));
                    emp.qty = Convert.ToDouble(reader.GetValue(3));
                    emp.TotaldenoAmount = Convert.ToDouble(reader.GetValue(4));

                    prog.Add(emp);
                }

            }
            return Json(prog.ToDataSourceResult(request));// prog;
        }
        public PartialViewResult getppmpdetailsconsol(int? office = 0, int? tyear = 0, int? id = 0, int? ooeid = 0, int? qtr = 0, int? accountid = 0)
        {
            Session["office"] = office;
            Session["tyear"] = tyear;
            Session["id"] = id;
            Session["ooeid"] = ooeid;
            Session["qtr"] = qtr;
            Session["accountid"] = accountid;
            return PartialView("pv_DfpptPPMPConsolidation");
        }
        public ActionResult GetPeformIndicator([DataSourceRequest]DataSourceRequest request, int? office = 0, int? propyear = 0)
        {
            var offid = Account.UserInfo.Department.ToString();
            string tempStr = "exec sp_BMS_PerformanceIndicator " + office + "," + propyear + "";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public int CheckLBP4header(int? officeID = 0, int? Year = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = 0;
                    SqlCommand com = new SqlCommand(@"SELECT count(*) FROM [IFMIS].[dbo].[tbl_R_BMSLBPForm4OtherData] where officeid=" + officeID + " and yearof=" + Year + " and Data_Type in (1,2,3,4) and ActionCode=1", con);
                    con.Open();
                    data = Convert.ToInt16(com.ExecuteScalar());
                    return data;
                }
            }
            catch //(Exception ex)
            {
                return 0;
            }
        }
        public string copylbp4entry(int? officeid = 0, int? transyear = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"sp_BMS_CopyLBP4Entry " + officeid + "," + transyear + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string DownloadAIPdetails(int? officeid = 0, int? transyear = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"sp_BMS_DownloadPPAdetials " + officeid + "," + transyear + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public PartialViewResult editperformanceindic(int? office = 0, int? tyear = 0, long? perindicator = 0, string perindicatorname = "")
        {
            Session["office"] = office;
            Session["tyear"] = tyear;
            Session["perindicator"] = perindicator;
            Session["perindicatorname"] = perindicatorname;

            return PartialView("pv_DfpptEditPerformanceIndic");
        }

        public string saveeditperformance(int? office = 0, int? tyear = 0, long? perindicator = 0, string indicatorename = "")
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"update [IFMIS].[dbo].[tbl_R_BMS_PerformanceIndic] set [kpm]='" + indicatorename + "' where [officeid]=" + office + " and [kpmtrnno]=" + perindicator + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return "success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public PartialViewResult addofficebreakdown(int? office = 0, int? tyear = 0, int? id = 0, int? ooeid = 0, int? qtr = 0, int? programid = 0, long? accountid = 0, int? maintrnno = 0)

        {
            Session["office"] = office;
            Session["tyear"] = tyear;
            Session["id"] = id;
            Session["ooeid"] = ooeid;
            Session["qtr"] = qtr;
            Session["programid"] = programid;
            Session["accountid"] = accountid;
            Session["maintrnno"] = maintrnno;
            return PartialView("pv_DfpptOfficeBreakdown");
        }
        public PartialViewResult addofficebreakdown_consol(int? office = 0, int? tyear = 0, int? id = 0, int? ooeid = 0, int? qtr = 0, int? programid = 0, long? accountid = 0, int? maintrnno = 0)

        {
            Session["office"] = office;
            Session["tyear"] = tyear;
            Session["id"] = id;
            Session["ooeid"] = ooeid;
            Session["qtr"] = qtr;
            Session["programid"] = programid;
            Session["accountid"] = accountid;
            Session["maintrnno"] = maintrnno;
            return PartialView("pv_DfpptOfficeBreakdown_Consolidated");
        }
        public string dfpptaddDenomination(int? officeid = 0, int? tyear = 0, int? id = 0, int? qtr = 0, int? programid = 0, long? accountid = 0, string denomination = "", float qty = 0, double amount = 0, double totalamount = 0, int? trnno = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec sp_BMS_DfpptAddDenomination " + officeid + "," + tyear + "," + id + "," + qtr + "," + programid + "," + accountid + "," + Account.UserInfo.eid + ",'" + denomination.Replace("'", "''") + "'," + qty + "," + amount + "," + totalamount + "," + Account.UserInfo.Department + "," + trnno + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public JsonResult GetDFpptBreakdown([DataSourceRequest]DataSourceRequest request, int? office = 0, int? tyear = 0, int? id = 0, int? ooeid = 0, int? qtr = 0, int? accountid = 0)
        {

            List<WFPSubmitted> prog = new List<WFPSubmitted>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec[sp_BMS_DfpptBreakdown] " + office + "," + accountid + "," + qtr + "," + tyear + "," + id + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WFPSubmitted emp = new WFPSubmitted();
                    emp.trnno = Convert.ToInt32(reader.GetValue(0));
                    emp.particular = reader.GetValue(3).ToString();
                    emp.denoAmount = Convert.ToDouble(reader.GetValue(5));
                    emp.qty = Convert.ToInt32(reader.GetValue(4));
                    emp.TotaldenoAmount = Convert.ToDouble(reader.GetValue(6));
                    emp.officeid = Convert.ToInt32(reader.GetValue(1));
                    emp.accountid = Convert.ToInt64(reader.GetValue(2));
                    emp.mon = Convert.ToInt32(reader.GetValue(8));
                    emp.officename = reader.GetValue(9).ToString();
                    emp.userofficeid = Convert.ToInt32(reader.GetValue(10));
                    emp.totalamount = Convert.ToDouble(reader.GetValue(11));
                    prog.Add(emp);
                }

            }
            return Json(prog.ToDataSourceResult(request));// prog;
        }
        public string dfpptDeleteDenomination(int? trnno = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"update [IFMIS].[dbo].[tbl_T_BMSDfpptDenomination] set actioncode=4,datetimeentered=datetimeentered +','+format(getdate(),'M/dd/yyyy hh:mm:ss tt'),userid=userid +','+ '" + Account.UserInfo.eid.ToString() + "' where demid= " + trnno + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return "success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public ActionResult ProposalSummary()
        {
            return View("pv_ProposalSummary");
        }
        public JsonResult proposal_utilizationsum([DataSourceRequest]DataSourceRequest request, int? officeid = 0, int? proyear = 0)
        {

            List<BudgetControlModel> prog = new List<BudgetControlModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec[sp_BMS_ProposalUtilization] " + officeid + "," + proyear + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    BudgetControlModel emp = new BudgetControlModel();
                    emp.OfficeID = Convert.ToInt32(reader.GetValue(0));
                    emp.OOE_Name = reader.GetValue(2).ToString();
                    emp.ooeid_uti = Convert.ToDouble(reader.GetValue(3));
                    emp.app1 = Convert.ToDecimal(reader.GetValue(4));
                    emp.allot1 = Convert.ToDecimal(reader.GetValue(5));
                    emp.obligate1 = Convert.ToDecimal(reader.GetValue(6));
                    emp.app_per1 = Convert.ToDecimal(reader.GetValue(7));
                    emp.allot_per1 = Convert.ToDecimal(reader.GetValue(8));
                    emp.app2 = Convert.ToDecimal(reader.GetValue(9));
                    emp.allot2 = Convert.ToDecimal(reader.GetValue(10));
                    emp.obligate2 = Convert.ToDecimal(reader.GetValue(11));
                    emp.app_per2 = Convert.ToDecimal(reader.GetValue(12));
                    emp.allot_per2 = Convert.ToDecimal(reader.GetValue(13));
                    emp.app3 = Convert.ToDecimal(reader.GetValue(14));
                    emp.allot3 = Convert.ToDecimal(reader.GetValue(15));
                    emp.obligate3 = Convert.ToDecimal(reader.GetValue(16));
                    emp.app_per3 = Convert.ToDecimal(reader.GetValue(17));
                    emp.allot_per3 = Convert.ToDecimal(reader.GetValue(18));

                    prog.Add(emp);
                }

            }
            return Json(prog.ToDataSourceResult(request));// prog;
        }
        public JsonResult proposal_summary([DataSourceRequest]DataSourceRequest request, int? account = 0, int? tyear = 0)
        {

            List<BudgetControlModel> prog = new List<BudgetControlModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"sp_BMS_ProposeSummaryICTEquipment " + tyear + ",0,0,0," + account + "", con);
                con.Open();
                com.CommandTimeout = 0;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    BudgetControlModel prosum = new BudgetControlModel();
                    prosum.OfficeID = Convert.ToInt32(reader.GetValue(0));
                    prosum.OfficeName = reader.GetValue(1).ToString();
                    prosum.qty2 = Convert.ToDouble(reader.GetValue(3));
                    prosum.qty = Convert.ToDouble(reader.GetValue(2));
                    prosum.Amount = Convert.ToDecimal(reader.GetValue(4));

                    prog.Add(prosum);
                }

            }
            return Json(prog.ToDataSourceResult(request));// prog;
        }
        public PartialViewResult proposal_summary_tabstrip(int? officeid = 0, int? accountid = 0)
        {
            Session["officeid"] = officeid;
            Session["accountid"] = accountid;
            return PartialView("pv_ProposalSummary_tabstrip");
        }

        public JsonResult proposal_summary_detail([DataSourceRequest]DataSourceRequest request, int? office = 0, int? account = 0, int? tyear = 0)
        {

            List<BudgetControlModel> prog = new List<BudgetControlModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"sp_BMS_ProposeSummaryICTEquipment " + tyear + ",1," + office + ",1," + account + "", con);
                con.Open();
                // con.Close();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    BudgetControlModel prosum = new BudgetControlModel();
                    prosum.OfficeID = Convert.ToInt32(reader.GetValue(0));
                    prosum.OfficeName = reader.GetValue(1).ToString();
                    prosum.qty2 = Convert.ToDouble(reader.GetValue(2));
                    prosum.AccountName = reader.GetValue(3).ToString();
                    prosum.Description = reader.GetValue(4).ToString();
                    prosum.Amount = Convert.ToDecimal(reader.GetValue(5));
                    prosum.totalamount = Convert.ToDecimal(reader.GetValue(6));
                    prosum.trnno_id = Convert.ToInt32(reader.GetValue(7));
                    prosum.Remarks = reader.GetValue(8).ToString();
                    prog.Add(prosum);
                }

            }
            return Json(prog.ToDataSourceResult(request));// prog;
        }
        public PartialViewResult proposal_summary_detail_form()
        {
            return PartialView("pv_ProposalSummary_detail");
        }
        public PartialViewResult proposal_summary_existing()
        {
            return PartialView("pv_ProposalSummary_existing");
        }
        public JsonResult UpdateProposed_Summary([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<BudgetControlModel> Denomination)
        {
            HRBudget_Layer DenominationList = new HRBudget_Layer();
            try
            {
                DenominationList.UpdateDenomination_propose(Denomination);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(Denomination.ToDataSourceResult(request, ModelState));
        }

        public JsonResult proposal_summary_exist([DataSourceRequest]DataSourceRequest request, int? office = 0, int? account = 0, int? tyear = 0)
        {

            List<BudgetControlModel> prog = new List<BudgetControlModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"sp_BMS_ProposeSummaryICTEquipment " + tyear + ",1," + office + ",0," + account + "", con);
                con.Open();
                com.CommandTimeout = 0;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    BudgetControlModel prosum = new BudgetControlModel();
                    prosum.OfficeID = Convert.ToInt32(reader.GetValue(0));
                    prosum.OfficeName = reader.GetValue(1).ToString();
                    prosum.qty = Convert.ToDouble(reader.GetValue(3));
                    prosum.Description = reader.GetValue(2).ToString();
                    prosum.enduser = reader.GetValue(4).ToString();
                    prosum.yearacquired = Convert.ToInt32(reader.GetValue(5));
                    prog.Add(prosum);
                }

            }
            return Json(prog.ToDataSourceResult(request));// prog;
        }
        public string Checksavechanges(int? id, double qty = 0.00, double amount = 0.00, string remark = "")
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"select dbo.fn_BMS_CheckSaveChanges (" + id + "," + qty + "," + amount + ",'" + remark.Replace("'", "''") + "')", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch //(Exception ex)
            {
                return "error";
            }
        }
        public ActionResult GetOOEWFP([DataSourceRequest]DataSourceRequest request, int? officeid = 0, int? tyear = 0)
        {

            string tempStr = "select * from fn_BMS_wfpprogram (" + officeid + "," + tyear + ")  order by ProgramID";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult GetaccountWFP([DataSourceRequest]DataSourceRequest request, int? programid = 0, int? tyear = 0, string selname = "", int? ooeid = 0, int? fundid = 0, int? officeid = 0, int? mode = 0)
        {
            var sname = selname.Split('/');
            if (fundid == 0) //gf
            {
                if (mode == 1)
                { //current
                    string tempStr = "SELECT AccountID,AccountName FROM [IFMIS].[dbo].[tbl_R_BMSProgramAccounts] where AccountYear=" + tyear + " and ActionCode=1  and ProgramID=" + programid + " and isnull(AccountName,'') != ''  and ObjectOfExpendetureID=" + ooeid + " order by AccountName";
                    DataTable dt = tempStr.DataSet();

                    var result = new ContentResult();
                    result.Content = SerializeDT.DataTableToJSON(dt);
                    result.ContentType = "application/json";
                    return result;
                }
                else //continuing
                {
                    string tempStr = "SELECT TransactionNo as  AccountID, Account as AccountName FROM ifmis.dbo.fn_BMS_ViewExcessAppropriations() where Account !='' order by trim(Account)";
                    DataTable dt = tempStr.DataSet();

                    var result = new ContentResult();
                    result.Content = SerializeDT.DataTableToJSON(dt);
                    result.ContentType = "application/json";
                    return result;
                }
            }
            else //tf
            {
                //string tempStr = "SELECT AccountID,AccountName FROM [IFMIS].[dbo].[tbl_R_BMSProgramAccounts_TF] where AccountYear=" + tyear + " and ActionCode=1  and [OfficeID]=" + officeid + " order by AccountName";
                string tempStr = "ifmis.dbo.sp_bms_WFPAccount_TF " + officeid + "," + tyear + "";
                DataTable dt = tempStr.DataSet();

                var result = new ContentResult();
                result.Content = SerializeDT.DataTableToJSON(dt);
                result.ContentType = "application/json";
                return result;
            }

        }

        public ActionResult GetWFPooe([DataSourceRequest]DataSourceRequest request, string selname = "")
        {

            string tempStr = "exec sp_BMS_WFP_ooe '" + selname + "'";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
            //}
        }
        public ActionResult GetOfficeActivity([DataSourceRequest]DataSourceRequest request, int? officeid = 0, int? tyear = 0, int? mode_trans = 0, int? accountid = 0,int? fundid=0)
        {
            //string tempStr = "select * from fn_BMS_wfpactivity (" + officeid + "," + tyear + "," + Account.UserInfo.eid + ") order by  initiative";
            if (mode_trans == 1 || fundid == 1) //current
            {
                string tempStr = "exec sp_BMS_WFPActivity " + officeid + "," + tyear + "," + Account.UserInfo.eid + "";
                DataTable dt = tempStr.DataSet();

                var result = new ContentResult();
                result.Content = SerializeDT.DataTableToJSON(dt);
                result.ContentType = "application/json";
                return result;
            }
            else //excess
            {
                string tempStr = "exec sp_BMS_WFPActivity_excess " + officeid + "," + tyear + "," + Account.UserInfo.eid + "," + accountid + "";
                DataTable dt = tempStr.DataSet();

                var result = new ContentResult();
                result.Content = SerializeDT.DataTableToJSON(dt);
                result.ContentType = "application/json";
                return result;
            }
        }
        public JsonResult getwfpappropriation(int? officeid = 0, int? programid = 0, long? accountid = 0, int? tyear = 0, int? fundid = 0, int? mode_trans = 0, int? applied80per = 0, int? supplemetal = 0)
        {

            WFPSubmitted data = new WFPSubmitted();
            DataTable _dt = new DataTable();
            string _sqlQuery = "";
            if (fundid == 0) //gf
            {
                if (mode_trans == 1) //current
                {
                    if (supplemetal == 0)
                    {
                        if (applied80per == 0)
                        {
                            _sqlQuery = "exec [sp_bms_WFP_DFPPT] " + officeid + "," + programid + "," + accountid + "," + tyear + "";
                        }
                        else //wfp percentage
                        {
                            _sqlQuery = "exec [sp_bms_WFP_DFPPT_Percentage] " + officeid + "," + programid + "," + accountid + "," + tyear + "";
                        }
                    }
                    else
                    {
                        _sqlQuery = "exec [sp_bms_WFP_DFPPT_supplemental] " + officeid + "," + programid + "," + accountid + "," + tyear + "";
                    }
                }
                else//continuing
                {
                    _sqlQuery = "exec [sp_bms_WFP_DFPPT_Excess] " + officeid + "," + programid + "," + accountid + "," + tyear + "";
                }
            }
            else//tf
            {
                _sqlQuery = "exec [sp_bms_WFP_DFPPT_TF] " + officeid + "," + programid + "," + accountid + "," + tyear + "";
            }
            _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
            try
            {
                data.AppropriationNet = Convert.ToDouble(_dt.Rows[0][5]);
                data.allotment = Convert.ToDouble(_dt.Rows[0][6]);
                data.balance = Convert.ToDouble(_dt.Rows[0][8]);
                data.appropriation_whole = Convert.ToDouble(_dt.Rows[0][0]);
                data.reserve = Convert.ToDouble(_dt.Rows[0][1]);
                data.reserveflag = Convert.ToInt32(_dt.Rows[0][29]);
                data.procurement = Convert.ToInt32(_dt.Rows[0][30]);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public string SaveWFPActual(int? officeid = 0, int? programid = 0, long? accountid = 0, int? activity = 0, string activityspecific = "", string itemname = "", string unit = "", string weight = "", double? m1 = 0, double? m2 = 0, double? m3 = 0, double? m4 = 0, double? m5 = 0, double? m6 = 0, double? m7 = 0, double? m8 = 0, double? m9 = 0, double? m10 = 0, double? m11 = 0, double? m12 = 0, double totalqty = 0.00, double amount = 0.00, int? days = 0, double totamount = 0.00, string ptarget = "", string indicator = "", long? resperson = 0, int? year = 0, int? wfpid = 0, int? includeppa = 0, string description = "", int? isPPMPTag = 0, int? MotherAccount = 0, string fund = "", int? kpmid = 0, int? fundreqid = 0, int? breakdownid = 0, string dtecomple = "", string firstqtr = "", string secondqtr = "", string thirdqtr = "", string fourthqtr = "", int propoffice = 0, int fundid = 0, int mode_trans = 0, string dtpckerfrom = "", string dtpckerto = "", int applied80per = 0, string project = "", string programexcess = "", int supplemetal = 0, int fpaysupplier = 0)

        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    if (fundid == 0) //gf
                    {
                        if (mode_trans == 1) // current
                        {
                            if (applied80per == 0)
                            {
                                SqlCommand com = new SqlCommand(@"exec dbo.sp_bms_InsertWFP_DFPPT " + officeid + ", " + programid + ", " + accountid + ", " + activity + ", '" + activityspecific.Replace("'", "''").ToString() + "', '" + itemname.Replace("'", "''").ToString() + "', '" + unit.Replace("'", "''").ToString() + "', '" + weight.Replace("'", "''").ToString() + "', " + m1 + ", " + m2 + ", " + m3 + ", " + m4 + ", " + m5 + ", " + m6 + ", " + m7 + ", " + m8 + ", " + m9 + ", " + m10 + ", " + m11 + ", " + m12 + ", " + totalqty + ", " + amount + ", " + days + ", " + totamount + ", '" + ptarget.Replace("'", "''").ToString() + "', '" + indicator.Replace("'", "''").ToString() + "', " + resperson + "," + Account.UserInfo.eid + "," + year + "," + wfpid + "," + includeppa + ",'" + description.Replace("'", "''") + "'," + isPPMPTag + "," + MotherAccount + ",'" + fund + "'," + kpmid + "," + fundreqid + "," + breakdownid + ",'" + dtecomple + "','" + firstqtr.Replace("'", "''").ToString() + "','" + secondqtr.Replace("'", "''").ToString() + "' ,'" + thirdqtr.Replace("'", "''").ToString() + "','" + fourthqtr.Replace("'", "''").ToString() + "'," + propoffice + ",'" + dtpckerfrom + "','" + dtpckerto + "'," + supplemetal + "", con);
                                com.CommandTimeout = 0;
                                con.Open();
                                data = Convert.ToString(com.ExecuteScalar());
                            }
                            else //percentage
                            {
                                SqlCommand com = new SqlCommand(@"exec dbo.sp_bms_InsertWFP_DFPPT_percentage " + officeid + ", " + programid + ", " + accountid + ", " + activity + ", '" + activityspecific.Replace("'", "''").ToString() + "', '" + itemname.Replace("'", "''").ToString() + "', '" + unit.Replace("'", "''").ToString() + "', '" + weight.Replace("'", "''").ToString() + "', " + m1 + ", " + m2 + ", " + m3 + ", " + m4 + ", " + m5 + ", " + m6 + ", " + m7 + ", " + m8 + ", " + m9 + ", " + m10 + ", " + m11 + ", " + m12 + ", " + totalqty + ", " + amount + ", " + days + ", " + totamount + ", '" + ptarget.Replace("'", "''").ToString() + "', '" + indicator.Replace("'", "''").ToString() + "', " + resperson + "," + Account.UserInfo.eid + "," + year + "," + wfpid + "," + includeppa + ",'" + description.Replace("'", "''") + "'," + isPPMPTag + "," + MotherAccount + ",'" + fund + "'," + kpmid + "," + fundreqid + "," + breakdownid + ",'" + dtecomple + "','" + firstqtr.Replace("'", "''").ToString() + "','" + secondqtr.Replace("'", "''").ToString() + "' ,'" + thirdqtr.Replace("'", "''").ToString() + "','" + fourthqtr.Replace("'", "''").ToString() + "'," + propoffice + ",'" + dtpckerfrom + "','" + dtpckerto + "'", con);
                                com.CommandTimeout = 0;
                                con.Open();
                                data = Convert.ToString(com.ExecuteScalar());
                            }
                        }
                        else
                        {
                            SqlCommand com = new SqlCommand(@"exec dbo.sp_bms_InsertWFP_DFPPT_Excess " + officeid + ", " + programid + ", " + accountid + ", " + activity + ", '" + activityspecific.Replace("'", "''").ToString() + "', '" + itemname.Replace("'", "''").ToString() + "', '" + unit.Replace("'", "''").ToString() + "', '" + weight.Replace("'", "''").ToString() + "', " + m1 + ", " + m2 + ", " + m3 + ", " + m4 + ", " + m5 + ", " + m6 + ", " + m7 + ", " + m8 + ", " + m9 + ", " + m10 + ", " + m11 + ", " + m12 + ", " + totalqty + ", " + amount + ", " + days + ", " + totamount + ", '" + ptarget.Replace("'", "''").ToString() + "', '" + indicator.Replace("'", "''").ToString() + "', " + resperson + "," + Account.UserInfo.eid + "," + year + "," + wfpid + "," + includeppa + ",'" + description.Replace("'", "''") + "'," + isPPMPTag + "," + MotherAccount + ",'" + fund + "'," + kpmid + "," + fundreqid + "," + breakdownid + ",'" + dtecomple + "','" + firstqtr.Replace("'", "''").ToString() + "','" + secondqtr.Replace("'", "''").ToString() + "' ,'" + thirdqtr.Replace("'", "''").ToString() + "','" + fourthqtr.Replace("'", "''").ToString() + "'," + propoffice + ",'" + project.Replace("'", "''").ToString() + "','" + programexcess.Replace("'", "''").ToString() + "'", con);
                            com.CommandTimeout = 0;
                            con.Open();
                            data = Convert.ToString(com.ExecuteScalar());
                        }
                    }
                    else
                    {
                        SqlCommand com = new SqlCommand(@"exec dbo.sp_bms_InsertWFP_DFPPT_TF " + officeid + ", " + programid + ", " + accountid + ", " + activity + ", '" + activityspecific.Replace("'", "''").ToString() + "', '" + itemname.Replace("'", "''").ToString() + "', '" + unit.Replace("'", "''").ToString() + "', '" + weight.Replace("'", "''").ToString() + "', " + m1 + ", " + m2 + ", " + m3 + ", " + m4 + ", " + m5 + ", " + m6 + ", " + m7 + ", " + m8 + ", " + m9 + ", " + m10 + ", " + m11 + ", " + m12 + ", " + totalqty + ", " + amount + ", " + days + ", " + totamount + ", '" + ptarget.Replace("'", "''").ToString() + "', '" + indicator.Replace("'", "''").ToString() + "', " + resperson + "," + Account.UserInfo.eid + "," + year + "," + wfpid + "," + includeppa + ",'" + description.Replace("'", "''") + "'," + isPPMPTag + "," + MotherAccount + ",'" + fund + "'," + kpmid + "," + fundreqid + "," + breakdownid + ",'" + dtecomple + "','" + firstqtr.Replace("'", "''").ToString() + "','" + secondqtr.Replace("'", "''").ToString() + "' ,'" + thirdqtr.Replace("'", "''").ToString() + "','" + fourthqtr.Replace("'", "''").ToString() + "'," + propoffice + "," + fpaysupplier + "", con);
                        com.CommandTimeout = 0;
                        con.Open();
                        data = Convert.ToString(com.ExecuteScalar());
                    }
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public JsonResult GetWFPPrepare([DataSourceRequest]DataSourceRequest request, int? office = 0, int? program = 0, long? account = 0, int? year = 0, int? id = 0, int? fundid = 0, int? mode_trans = 0, int? applied80per = 0, int? supplemetal = 0,int wfpsummaryid=0)
        {

            List<WFPrepare> prog = new List<WFPrepare>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (fundid == 0) //gf
                {
                    if (supplemetal == 0)
                    {
                        if (mode_trans == 1)
                        { //current
                            if (applied80per == 0)
                            {
                                if (wfpsummaryid == 0)
                                {
                                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFP_DFPPTPrepare] " + office + "," + program + "," + account + "," + year + "," + id + "", con);
                                    com.CommandTimeout = 0;
                                    con.Open();
                                    SqlDataReader reader = com.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        WFPrepare emp = new WFPrepare();
                                        emp.wfpid = Convert.ToInt32(reader.GetValue(0));
                                        emp.officeid = Convert.ToInt32(reader.GetValue(1));
                                        emp.programid = Convert.ToInt32(reader.GetValue(2));
                                        emp.accountid = Convert.ToInt64(reader.GetValue(3));
                                        emp.accountname = Convert.ToString(reader.GetValue(4));
                                        emp.activityid = Convert.ToInt32(reader.GetValue(5));
                                        emp.activityname = reader.GetValue(6).ToString();
                                        emp.specificactivity = reader.GetValue(7).ToString();
                                        emp.particular = reader.GetValue(8).ToString();
                                        emp.unit = Convert.ToString(reader.GetValue(9));
                                        emp.weight = Convert.ToString(reader.GetValue(10));
                                        emp.m1 = Convert.ToDouble(reader.GetValue(11));
                                        emp.m2 = Convert.ToDouble(reader.GetValue(12));
                                        emp.m3 = Convert.ToDouble(reader.GetValue(13));
                                        emp.m4 = Convert.ToDouble(reader.GetValue(14));
                                        emp.m5 = Convert.ToDouble(reader.GetValue(15));
                                        emp.m6 = Convert.ToDouble(reader.GetValue(16));
                                        emp.m7 = Convert.ToDouble(reader.GetValue(17));
                                        emp.m8 = Convert.ToDouble(reader.GetValue(18));
                                        emp.m9 = Convert.ToDouble(reader.GetValue(19));
                                        emp.m10 = Convert.ToDouble(reader.GetValue(20));
                                        emp.m11 = Convert.ToDouble(reader.GetValue(21));
                                        emp.m12 = Convert.ToDouble(reader.GetValue(22));
                                        emp.totalqty = Convert.ToDouble(reader.GetValue(23));
                                        emp.noofdays = Convert.ToInt32(reader.GetValue(24));
                                        emp.amount = Convert.ToDouble(reader.GetValue(25));
                                        emp.totalamount = Convert.ToDouble(reader.GetValue(26));
                                        emp.physicaltarget = Convert.ToString(reader.GetValue(27));
                                        emp.devtindicator = Convert.ToString(reader.GetValue(28));
                                        emp.responsibleperson = Convert.ToInt64(reader.GetValue(29));
                                        emp.datetimeentered = Convert.ToString(reader.GetValue(30));
                                        emp.remarks = reader.GetValue(31).ToString();
                                        emp.subppaid = Convert.ToInt32(reader.GetValue(32));
                                        emp.description = Convert.ToString(reader.GetValue(33));
                                        emp.isPPMP = Convert.ToInt32(reader.GetValue(34));
                                        emp.nonofficeid = Convert.ToInt32(reader.GetValue(35));
                                        emp.nonofficeidparent = Convert.ToInt32(reader.GetValue(36));
                                        emp.fund = Convert.ToString(reader.GetValue(37));
                                        emp.kpmid = Convert.ToInt32(reader.GetValue(38));
                                        emp.fundreqid = Convert.ToInt32(reader.GetValue(39));
                                        emp.breakdownid = Convert.ToInt32(reader.GetValue(40));
                                        emp.account_parent = Convert.ToString(reader.GetValue(41));
                                        emp.completiondate = Convert.ToString(reader.GetValue(42));
                                        emp.firstqtr = Convert.ToString(reader.GetValue(43));
                                        emp.secondqtr = Convert.ToString(reader.GetValue(44));
                                        emp.thirdqtr = Convert.ToString(reader.GetValue(45));
                                        emp.fourthqtr = Convert.ToString(reader.GetValue(46));
                                        emp.yearof = Convert.ToInt32(reader.GetValue(47));
                                        emp.prepareuser = Convert.ToString(reader.GetValue(48));
                                        emp.subppaname = Convert.ToString(reader.GetValue(49));
                                        emp.wfpno = Convert.ToString(reader.GetValue(50));
                                        emp.activityfrom = Convert.ToString(reader.GetValue(51));
                                        emp.activityto = Convert.ToString(reader.GetValue(52));
                                        emp.specid = Convert.ToInt32(reader.GetValue(53));
                                        emp.pullitem = 0;
                                        emp.pullitem_desc = "";
                                        emp.isupplemental = Convert.ToInt32(reader.GetValue(54));
                                        emp.withpr = Convert.ToInt32(reader.GetValue(55));
                                        emp.withad = Convert.ToInt32(reader.GetValue(56));
                                        emp.fpay = 0;
                                        prog.Add(emp);
                                    }
                                }
                                else //wfp summary revision
                                {
                                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFP_DFPPTRevision] " + office + "," + program + "," + account + "," + year + "," + id + "", con);
                                    com.CommandTimeout = 0;
                                    con.Open();
                                    SqlDataReader reader = com.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        WFPrepare emp = new WFPrepare();
                                        emp.wfpid = Convert.ToInt32(reader.GetValue(0));
                                        emp.officeid = Convert.ToInt32(reader.GetValue(1));
                                        emp.programid = Convert.ToInt32(reader.GetValue(2));
                                        emp.accountid = Convert.ToInt64(reader.GetValue(3));
                                        emp.accountname = Convert.ToString(reader.GetValue(4));
                                        emp.activityid = Convert.ToInt32(reader.GetValue(5));
                                        emp.activityname = reader.GetValue(6).ToString();
                                        emp.specificactivity = reader.GetValue(7).ToString();
                                        emp.particular = reader.GetValue(8).ToString();
                                        emp.unit = Convert.ToString(reader.GetValue(9));
                                        emp.weight = Convert.ToString(reader.GetValue(10));
                                        emp.m1 = Convert.ToDouble(reader.GetValue(11));
                                        emp.m2 = Convert.ToDouble(reader.GetValue(12));
                                        emp.m3 = Convert.ToDouble(reader.GetValue(13));
                                        emp.m4 = Convert.ToDouble(reader.GetValue(14));
                                        emp.m5 = Convert.ToDouble(reader.GetValue(15));
                                        emp.m6 = Convert.ToDouble(reader.GetValue(16));
                                        emp.m7 = Convert.ToDouble(reader.GetValue(17));
                                        emp.m8 = Convert.ToDouble(reader.GetValue(18));
                                        emp.m9 = Convert.ToDouble(reader.GetValue(19));
                                        emp.m10 = Convert.ToDouble(reader.GetValue(20));
                                        emp.m11 = Convert.ToDouble(reader.GetValue(21));
                                        emp.m12 = Convert.ToDouble(reader.GetValue(22));
                                        emp.totalqty = Convert.ToDouble(reader.GetValue(23));
                                        emp.noofdays = Convert.ToInt32(reader.GetValue(24));
                                        emp.amount = Convert.ToDouble(reader.GetValue(25));
                                        emp.totalamount = Convert.ToDouble(reader.GetValue(26));
                                        emp.physicaltarget = Convert.ToString(reader.GetValue(27));
                                        emp.devtindicator = Convert.ToString(reader.GetValue(28));
                                        emp.responsibleperson = Convert.ToInt64(reader.GetValue(29));
                                        emp.datetimeentered = Convert.ToString(reader.GetValue(30));
                                        emp.remarks = reader.GetValue(31).ToString();
                                        emp.subppaid = Convert.ToInt32(reader.GetValue(32));
                                        emp.description = Convert.ToString(reader.GetValue(33));
                                        emp.isPPMP = Convert.ToInt32(reader.GetValue(34));
                                        emp.nonofficeid = Convert.ToInt32(reader.GetValue(35));
                                        emp.nonofficeidparent = Convert.ToInt32(reader.GetValue(36));
                                        emp.fund = Convert.ToString(reader.GetValue(37));
                                        emp.kpmid = Convert.ToInt32(reader.GetValue(38));
                                        emp.fundreqid = Convert.ToInt32(reader.GetValue(39));
                                        emp.breakdownid = Convert.ToInt32(reader.GetValue(40));
                                        emp.account_parent = Convert.ToString(reader.GetValue(41));
                                        emp.completiondate = Convert.ToString(reader.GetValue(42));
                                        emp.firstqtr = Convert.ToString(reader.GetValue(43));
                                        emp.secondqtr = Convert.ToString(reader.GetValue(44));
                                        emp.thirdqtr = Convert.ToString(reader.GetValue(45));
                                        emp.fourthqtr = Convert.ToString(reader.GetValue(46));
                                        emp.yearof = Convert.ToInt32(reader.GetValue(47));
                                        emp.prepareuser = Convert.ToString(reader.GetValue(48));
                                        emp.subppaname = Convert.ToString(reader.GetValue(49));
                                        emp.wfpno = Convert.ToString(reader.GetValue(50));
                                        emp.activityfrom = Convert.ToString(reader.GetValue(51));
                                        emp.activityto = Convert.ToString(reader.GetValue(52));
                                        emp.specid = Convert.ToInt32(reader.GetValue(53));
                                        emp.pullitem = 0;
                                        emp.pullitem_desc = "";
                                        emp.isupplemental = Convert.ToInt32(reader.GetValue(54));
                                        emp.withpr = Convert.ToInt32(reader.GetValue(55));
                                        emp.withad = Convert.ToInt32(reader.GetValue(56));
                                        emp.fpay = 0;
                                        prog.Add(emp);
                                    }
                                }
                            }
                            else
                            {
                                SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFP_DFPPTPrepare_percentage] " + office + "," + program + "," + account + "," + year + "," + id + "", con);
                                con.Open();
                                com.CommandTimeout = 0;
                                SqlDataReader reader = com.ExecuteReader();
                                while (reader.Read())
                                {
                                    WFPrepare emp = new WFPrepare();
                                    emp.wfpid = Convert.ToInt32(reader.GetValue(0));
                                    emp.officeid = Convert.ToInt32(reader.GetValue(1));
                                    emp.programid = Convert.ToInt32(reader.GetValue(2));
                                    emp.accountid = Convert.ToInt64(reader.GetValue(3));
                                    emp.accountname = Convert.ToString(reader.GetValue(4));
                                    emp.activityid = Convert.ToInt32(reader.GetValue(5));
                                    emp.activityname = reader.GetValue(6).ToString();
                                    emp.specificactivity = reader.GetValue(7).ToString();
                                    emp.particular = reader.GetValue(8).ToString();
                                    emp.unit = Convert.ToString(reader.GetValue(9));
                                    emp.weight = Convert.ToString(reader.GetValue(10));
                                    emp.m1 = Convert.ToDouble(reader.GetValue(11));
                                    emp.m2 = Convert.ToDouble(reader.GetValue(12));
                                    emp.m3 = Convert.ToDouble(reader.GetValue(13));
                                    emp.m4 = Convert.ToDouble(reader.GetValue(14));
                                    emp.m5 = Convert.ToDouble(reader.GetValue(15));
                                    emp.m6 = Convert.ToDouble(reader.GetValue(16));
                                    emp.m7 = Convert.ToDouble(reader.GetValue(17));
                                    emp.m8 = Convert.ToDouble(reader.GetValue(18));
                                    emp.m9 = Convert.ToDouble(reader.GetValue(19));
                                    emp.m10 = Convert.ToDouble(reader.GetValue(20));
                                    emp.m11 = Convert.ToDouble(reader.GetValue(21));
                                    emp.m12 = Convert.ToDouble(reader.GetValue(22));
                                    emp.totalqty = Convert.ToDouble(reader.GetValue(23));
                                    emp.noofdays = Convert.ToInt32(reader.GetValue(24));
                                    emp.amount = Convert.ToDouble(reader.GetValue(25));
                                    emp.totalamount = Convert.ToDouble(reader.GetValue(26));
                                    emp.physicaltarget = Convert.ToString(reader.GetValue(27));
                                    emp.devtindicator = Convert.ToString(reader.GetValue(28));
                                    emp.responsibleperson = Convert.ToInt64(reader.GetValue(29));
                                    emp.datetimeentered = Convert.ToString(reader.GetValue(30));
                                    emp.remarks = reader.GetValue(31).ToString();
                                    emp.subppaid = Convert.ToInt32(reader.GetValue(32));
                                    emp.description = Convert.ToString(reader.GetValue(33));
                                    emp.isPPMP = Convert.ToInt32(reader.GetValue(34));
                                    emp.nonofficeid = Convert.ToInt32(reader.GetValue(35));
                                    emp.nonofficeidparent = Convert.ToInt32(reader.GetValue(36));
                                    emp.fund = Convert.ToString(reader.GetValue(37));
                                    emp.kpmid = Convert.ToInt32(reader.GetValue(38));
                                    emp.fundreqid = Convert.ToInt32(reader.GetValue(39));
                                    emp.breakdownid = Convert.ToInt32(reader.GetValue(40));
                                    emp.account_parent = Convert.ToString(reader.GetValue(41));
                                    emp.completiondate = Convert.ToString(reader.GetValue(42));
                                    emp.firstqtr = Convert.ToString(reader.GetValue(43));
                                    emp.secondqtr = Convert.ToString(reader.GetValue(44));
                                    emp.thirdqtr = Convert.ToString(reader.GetValue(45));
                                    emp.fourthqtr = Convert.ToString(reader.GetValue(46));
                                    emp.yearof = Convert.ToInt32(reader.GetValue(47));
                                    emp.prepareuser = Convert.ToString(reader.GetValue(48));
                                    emp.subppaname = Convert.ToString(reader.GetValue(49));
                                    emp.wfpno = Convert.ToString(reader.GetValue(50));
                                    emp.activityfrom = Convert.ToString(reader.GetValue(51));
                                    emp.activityto = Convert.ToString(reader.GetValue(52));
                                    emp.pullitem = Convert.ToInt32(reader.GetValue(53));
                                    emp.pullitem_desc = Convert.ToString(reader.GetValue(54));
                                    emp.isupplemental = 0;
                                    emp.withpr = 0;
                                    emp.withad = 0;
                                    emp.fpay = 0;
                                    prog.Add(emp);
                                }
                            }
                        }
                        else //excess
                        {
                            SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFP_DFPPTPrepare_Excess] " + office + "," + account + "," + year + "," + id + "", con);
                            con.Open();
                            com.CommandTimeout = 0;
                            SqlDataReader reader = com.ExecuteReader();
                            while (reader.Read())
                            {
                                WFPrepare emp = new WFPrepare();
                                emp.wfpid = Convert.ToInt32(reader.GetValue(0));
                                emp.officeid = Convert.ToInt32(reader.GetValue(1));
                                emp.programid = Convert.ToInt32(reader.GetValue(2));
                                emp.accountid = Convert.ToInt64(reader.GetValue(3));
                                emp.accountname = Convert.ToString(reader.GetValue(4));
                                emp.activityid = Convert.ToInt32(reader.GetValue(5));
                                emp.activityname = reader.GetValue(6).ToString();
                                emp.specificactivity = reader.GetValue(7).ToString();
                                emp.particular = reader.GetValue(8).ToString();
                                emp.unit = Convert.ToString(reader.GetValue(9));
                                emp.weight = Convert.ToString(reader.GetValue(10));
                                emp.m1 = Convert.ToDouble(reader.GetValue(11));
                                emp.m2 = Convert.ToDouble(reader.GetValue(12));
                                emp.m3 = Convert.ToDouble(reader.GetValue(13));
                                emp.m4 = Convert.ToDouble(reader.GetValue(14));
                                emp.m5 = Convert.ToDouble(reader.GetValue(15));
                                emp.m6 = Convert.ToDouble(reader.GetValue(16));
                                emp.m7 = Convert.ToDouble(reader.GetValue(17));
                                emp.m8 = Convert.ToDouble(reader.GetValue(18));
                                emp.m9 = Convert.ToDouble(reader.GetValue(19));
                                emp.m10 = Convert.ToDouble(reader.GetValue(20));
                                emp.m11 = Convert.ToDouble(reader.GetValue(21));
                                emp.m12 = Convert.ToDouble(reader.GetValue(22));
                                emp.totalqty = Convert.ToDouble(reader.GetValue(23));
                                emp.noofdays = Convert.ToInt32(reader.GetValue(24));
                                emp.amount = Convert.ToDouble(reader.GetValue(25));
                                emp.totalamount = Convert.ToDouble(reader.GetValue(26));
                                emp.physicaltarget = Convert.ToString(reader.GetValue(27));
                                emp.devtindicator = Convert.ToString(reader.GetValue(28));
                                emp.responsibleperson = Convert.ToInt64(reader.GetValue(29));
                                emp.datetimeentered = Convert.ToString(reader.GetValue(30));
                                emp.remarks = reader.GetValue(31).ToString();
                                emp.subppaid = Convert.ToInt32(reader.GetValue(32));
                                emp.description = Convert.ToString(reader.GetValue(33));
                                emp.isPPMP = Convert.ToInt32(reader.GetValue(34));
                                emp.nonofficeid = Convert.ToInt32(reader.GetValue(35));
                                emp.nonofficeidparent = Convert.ToInt32(reader.GetValue(36));
                                emp.fund = Convert.ToString(reader.GetValue(37));
                                emp.kpmid = Convert.ToInt32(reader.GetValue(38));
                                emp.fundreqid = Convert.ToInt32(reader.GetValue(39));
                                emp.breakdownid = Convert.ToInt32(reader.GetValue(40));
                                emp.account_parent = Convert.ToString(reader.GetValue(41));
                                emp.completiondate = Convert.ToString(reader.GetValue(42));
                                emp.firstqtr = Convert.ToString(reader.GetValue(43));
                                emp.secondqtr = Convert.ToString(reader.GetValue(44));
                                emp.thirdqtr = Convert.ToString(reader.GetValue(45));
                                emp.fourthqtr = Convert.ToString(reader.GetValue(46));
                                emp.yearof = Convert.ToInt32(reader.GetValue(47));
                                emp.prepareuser = Convert.ToString(reader.GetValue(48));
                                emp.subppaname = Convert.ToString(reader.GetValue(49));
                                emp.wfpno = Convert.ToString(reader.GetValue(50));
                                emp.project = Convert.ToString(reader.GetValue(51));
                                emp.program = Convert.ToString(reader.GetValue(52));
                                emp.isupplemental = 0;
                                emp.withpr = 0;
                                emp.withad = 0;
                                emp.fpay = 0;
                                prog.Add(emp);
                            }
                        }
                    }
                    else
                    { // supplemental
                        SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFP_DFPPTPrepare_supplemental] " + office + "," + program + "," + account + "," + year + "," + id + "", con);
                        con.Open();
                        com.CommandTimeout = 0;
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            WFPrepare emp = new WFPrepare();
                            emp.wfpid = Convert.ToInt32(reader.GetValue(0));
                            emp.officeid = Convert.ToInt32(reader.GetValue(1));
                            emp.programid = Convert.ToInt32(reader.GetValue(2));
                            emp.accountid = Convert.ToInt64(reader.GetValue(3));
                            emp.accountname = Convert.ToString(reader.GetValue(4));
                            emp.activityid = Convert.ToInt32(reader.GetValue(5));
                            emp.activityname = reader.GetValue(6).ToString();
                            emp.specificactivity = reader.GetValue(7).ToString();
                            emp.particular = reader.GetValue(8).ToString();
                            emp.unit = Convert.ToString(reader.GetValue(9));
                            emp.weight = Convert.ToString(reader.GetValue(10));
                            emp.m1 = Convert.ToDouble(reader.GetValue(11));
                            emp.m2 = Convert.ToDouble(reader.GetValue(12));
                            emp.m3 = Convert.ToDouble(reader.GetValue(13));
                            emp.m4 = Convert.ToDouble(reader.GetValue(14));
                            emp.m5 = Convert.ToDouble(reader.GetValue(15));
                            emp.m6 = Convert.ToDouble(reader.GetValue(16));
                            emp.m7 = Convert.ToDouble(reader.GetValue(17));
                            emp.m8 = Convert.ToDouble(reader.GetValue(18));
                            emp.m9 = Convert.ToDouble(reader.GetValue(19));
                            emp.m10 = Convert.ToDouble(reader.GetValue(20));
                            emp.m11 = Convert.ToDouble(reader.GetValue(21));
                            emp.m12 = Convert.ToDouble(reader.GetValue(22));
                            emp.totalqty = Convert.ToDouble(reader.GetValue(23));
                            emp.noofdays = Convert.ToInt32(reader.GetValue(24));
                            emp.amount = Convert.ToDouble(reader.GetValue(25));
                            emp.totalamount = Convert.ToDouble(reader.GetValue(26));
                            emp.physicaltarget = Convert.ToString(reader.GetValue(27));
                            emp.devtindicator = Convert.ToString(reader.GetValue(28));
                            emp.responsibleperson = Convert.ToInt64(reader.GetValue(29));
                            emp.datetimeentered = Convert.ToString(reader.GetValue(30));
                            emp.remarks = reader.GetValue(31).ToString();
                            emp.subppaid = Convert.ToInt32(reader.GetValue(32));
                            emp.description = Convert.ToString(reader.GetValue(33));
                            emp.isPPMP = Convert.ToInt32(reader.GetValue(34));
                            emp.nonofficeid = Convert.ToInt32(reader.GetValue(35));
                            emp.nonofficeidparent = Convert.ToInt32(reader.GetValue(36));
                            emp.fund = Convert.ToString(reader.GetValue(37));
                            emp.kpmid = Convert.ToInt32(reader.GetValue(38));
                            emp.fundreqid = Convert.ToInt32(reader.GetValue(39));
                            emp.breakdownid = Convert.ToInt32(reader.GetValue(40));
                            emp.account_parent = Convert.ToString(reader.GetValue(41));
                            emp.completiondate = Convert.ToString(reader.GetValue(42));
                            emp.firstqtr = Convert.ToString(reader.GetValue(43));
                            emp.secondqtr = Convert.ToString(reader.GetValue(44));
                            emp.thirdqtr = Convert.ToString(reader.GetValue(45));
                            emp.fourthqtr = Convert.ToString(reader.GetValue(46));
                            emp.yearof = Convert.ToInt32(reader.GetValue(47));
                            emp.prepareuser = Convert.ToString(reader.GetValue(48));
                            emp.subppaname = Convert.ToString(reader.GetValue(49));
                            emp.wfpno = Convert.ToString(reader.GetValue(50));
                            emp.activityfrom = Convert.ToString(reader.GetValue(51));
                            emp.activityto = Convert.ToString(reader.GetValue(52));
                            emp.specid = Convert.ToInt32(reader.GetValue(53));
                            emp.pullitem = 0;
                            emp.pullitem_desc = "";
                            emp.isupplemental = Convert.ToInt32(reader.GetValue(54));
                            emp.withpr = 0;
                            emp.withad = 0;
                            emp.fpay = 0;
                            prog.Add(emp);
                        }
                    }
                }
                else
                {
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFP_DFPPTPrepare_TF] " + office + "," + account + "," + year + "," + id + "", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        WFPrepare emp = new WFPrepare();
                        emp.wfpid = Convert.ToInt32(reader.GetValue(0));
                        emp.officeid = Convert.ToInt32(reader.GetValue(1));
                        emp.programid = Convert.ToInt32(reader.GetValue(2));
                        emp.accountid = Convert.ToInt64(reader.GetValue(3));
                        emp.accountname = Convert.ToString(reader.GetValue(4));
                        emp.activityid = Convert.ToInt32(reader.GetValue(5));
                        emp.activityname = reader.GetValue(6).ToString();
                        emp.specificactivity = reader.GetValue(7).ToString();
                        emp.particular = reader.GetValue(8).ToString();
                        emp.unit = Convert.ToString(reader.GetValue(9));
                        emp.weight = Convert.ToString(reader.GetValue(10));
                        emp.m1 = Convert.ToDouble(reader.GetValue(11));
                        emp.m2 = Convert.ToDouble(reader.GetValue(12));
                        emp.m3 = Convert.ToDouble(reader.GetValue(13));
                        emp.m4 = Convert.ToDouble(reader.GetValue(14));
                        emp.m5 = Convert.ToDouble(reader.GetValue(15));
                        emp.m6 = Convert.ToDouble(reader.GetValue(16));
                        emp.m7 = Convert.ToDouble(reader.GetValue(17));
                        emp.m8 = Convert.ToDouble(reader.GetValue(18));
                        emp.m9 = Convert.ToDouble(reader.GetValue(19));
                        emp.m10 = Convert.ToDouble(reader.GetValue(20));
                        emp.m11 = Convert.ToDouble(reader.GetValue(21));
                        emp.m12 = Convert.ToDouble(reader.GetValue(22));
                        emp.totalqty = Convert.ToDouble(reader.GetValue(23));
                        emp.noofdays = Convert.ToInt32(reader.GetValue(24));
                        emp.amount = Convert.ToDouble(reader.GetValue(25));
                        emp.totalamount = Convert.ToDouble(reader.GetValue(26));
                        emp.physicaltarget = Convert.ToString(reader.GetValue(27));
                        emp.devtindicator = Convert.ToString(reader.GetValue(28));
                        emp.responsibleperson = Convert.ToInt64(reader.GetValue(29));
                        emp.datetimeentered = Convert.ToString(reader.GetValue(30));
                        emp.remarks = reader.GetValue(31).ToString();
                        emp.subppaid = Convert.ToInt32(reader.GetValue(32));
                        emp.description = Convert.ToString(reader.GetValue(33));
                        emp.isPPMP = Convert.ToInt32(reader.GetValue(34));
                        emp.nonofficeid = Convert.ToInt32(reader.GetValue(35));
                        emp.nonofficeidparent = Convert.ToInt32(reader.GetValue(36));
                        emp.fund = Convert.ToString(reader.GetValue(37));
                        emp.kpmid = Convert.ToInt32(reader.GetValue(38));
                        emp.fundreqid = Convert.ToInt32(reader.GetValue(39));
                        emp.breakdownid = Convert.ToInt32(reader.GetValue(40));
                        emp.account_parent = Convert.ToString(reader.GetValue(41));
                        emp.completiondate = Convert.ToString(reader.GetValue(42));
                        emp.firstqtr = Convert.ToString(reader.GetValue(43));
                        emp.secondqtr = Convert.ToString(reader.GetValue(44));
                        emp.thirdqtr = Convert.ToString(reader.GetValue(45));
                        emp.fourthqtr = Convert.ToString(reader.GetValue(46));
                        emp.yearof = Convert.ToInt32(reader.GetValue(47));
                        emp.prepareuser = Convert.ToString(reader.GetValue(48));
                        emp.subppaname = Convert.ToString(reader.GetValue(49));
                        emp.wfpno = Convert.ToString(reader.GetValue(50));
                        emp.withpr = 0;
                        emp.withad = 0;
                        emp.fpay = Convert.ToInt32(reader.GetValue(51));
                        prog.Add(emp);
                    }
                }

            }
            return Json(prog.ToDataSourceResult(request));// prog;
        }

        public JsonResult GetWFPofficestatus([DataSourceRequest]DataSourceRequest request, int? office = 0, int? program = 0, long? account = 0, int? year = 0, int? id = 0)
        {

            List<WFPrepare> prog = new List<WFPrepare>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFP_status_office] " + office + "," + program + "," + account + "," + year + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WFPrepare emp = new WFPrepare();
                    emp.wfpid = Convert.ToInt32(reader.GetValue(0));
                    emp.officeid = Convert.ToInt32(reader.GetValue(1));
                    emp.programid = Convert.ToInt32(reader.GetValue(2));
                    emp.accountid = Convert.ToInt64(reader.GetValue(3));
                    emp.accountname = Convert.ToString(reader.GetValue(4));
                    emp.activityid = Convert.ToInt32(reader.GetValue(5));
                    emp.activityname = reader.GetValue(6).ToString();
                    emp.specificactivity = reader.GetValue(7).ToString();
                    emp.particular = reader.GetValue(8).ToString();
                    emp.unit = Convert.ToString(reader.GetValue(9));
                    emp.weight = Convert.ToString(reader.GetValue(10));
                    emp.m1 = Convert.ToDouble(reader.GetValue(11));
                    emp.m2 = Convert.ToDouble(reader.GetValue(12));
                    emp.m3 = Convert.ToDouble(reader.GetValue(13));
                    emp.m4 = Convert.ToDouble(reader.GetValue(14));
                    emp.m5 = Convert.ToDouble(reader.GetValue(15));
                    emp.m6 = Convert.ToDouble(reader.GetValue(16));
                    emp.m7 = Convert.ToDouble(reader.GetValue(17));
                    emp.m8 = Convert.ToDouble(reader.GetValue(18));
                    emp.m9 = Convert.ToDouble(reader.GetValue(19));
                    emp.m10 = Convert.ToDouble(reader.GetValue(20));
                    emp.m11 = Convert.ToDouble(reader.GetValue(21));
                    emp.m12 = Convert.ToDouble(reader.GetValue(22));
                    emp.totalqty = Convert.ToDouble(reader.GetValue(23));
                    emp.noofdays = Convert.ToInt32(reader.GetValue(24));
                    emp.amount = Convert.ToDouble(reader.GetValue(25));
                    emp.totalamount = Convert.ToDouble(reader.GetValue(26));
                    emp.physicaltarget = Convert.ToString(reader.GetValue(27));
                    emp.devtindicator = Convert.ToString(reader.GetValue(28));
                    emp.responsibleperson = Convert.ToInt64(reader.GetValue(29));
                    emp.datetimeentered = Convert.ToString(reader.GetValue(30));
                    emp.remarks = reader.GetValue(31).ToString();
                    emp.subppaid = Convert.ToInt32(reader.GetValue(32));
                    emp.description = Convert.ToString(reader.GetValue(33));
                    emp.isPPMP = Convert.ToInt32(reader.GetValue(34));
                    emp.nonofficeid = Convert.ToInt32(reader.GetValue(35));
                    emp.nonofficeidparent = Convert.ToInt32(reader.GetValue(36));
                    emp.fund = Convert.ToString(reader.GetValue(37));
                    emp.kpmid = Convert.ToInt32(reader.GetValue(38));
                    emp.fundreqid = Convert.ToInt32(reader.GetValue(39));
                    emp.breakdownid = Convert.ToInt32(reader.GetValue(40));
                    emp.account_parent = Convert.ToString(reader.GetValue(41));
                    emp.completiondate = Convert.ToString(reader.GetValue(42));
                    emp.firstqtr = Convert.ToString(reader.GetValue(43));
                    emp.secondqtr = Convert.ToString(reader.GetValue(44));
                    emp.thirdqtr = Convert.ToString(reader.GetValue(45));
                    emp.fourthqtr = Convert.ToString(reader.GetValue(46));
                    emp.yearof = Convert.ToInt32(reader.GetValue(47));
                    prog.Add(emp);
                }

            }
            return Json(prog.ToDataSourceResult(request));// prog;
        }
        public string deletewfp(int? id = 0, int? year = 0, int? fundid = 0, int? mode_trans = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    if (fundid == 1) //tf
                    {
                        SqlCommand com = new SqlCommand(@"sp_BMS_DeleteWFP_TF " + id + ",'" + Account.UserInfo.eid.ToString() + "'," + year + "", con);
                        con.Open();
                        data = Convert.ToString(com.ExecuteScalar());

                    }
                    else //gf
                    {
                        if (mode_trans == 1) //current
                        {
                            SqlCommand com = new SqlCommand(@"sp_BMS_DeleteWFP " + id + ",'" + Account.UserInfo.eid.ToString() + "'," + year + "", con);
                            con.Open();
                            data = Convert.ToString(com.ExecuteScalar());
                        }
                        else //excess
                        {
                            SqlCommand com = new SqlCommand(@"sp_BMS_DeleteWFP_Excess " + id + ",'" + Account.UserInfo.eid.ToString() + "'," + year + "", con);
                            con.Open();
                            data = Convert.ToString(com.ExecuteScalar());
                        }
                    }
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string retunwfp(int? id = 0, int? year = 0, int? officeid = 0, int? programid = 0, int? accountid = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_ReturnWFP] " + id + ",'" + Account.UserInfo.eid.ToString() + "'," + year + "," + officeid + "," + programid + "," + accountid + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string deletelocation(int? id = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"update [IFMIS].[dbo].[tbl_T_BMSWFP_Location] set actioncode=2,datetimeentered=datetimeentered +','+ format(getdate(),'MM/dd/yyyy hh:mm:ss tt') where loc_id=" + id + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return "success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string submitwfp(int? officeid = 0, int? programid = 0, long? accountid = 0, int? year = 0, int? id = 0, int? fundid = 0, int? mode_trans = 0, int? supplemetalapp = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    if (fundid == 0) //gf
                    {
                        if (mode_trans == 1) //current
                        {
                            SqlCommand com = new SqlCommand(@"exec sp_BMS_WFPNew_Submit " + officeid + "," + programid + "," + accountid + "," + year + "," + id + "," + Account.UserInfo.eid + "," + supplemetalapp + "", con);
                            con.Open();
                            data = Convert.ToString(com.ExecuteScalar());
                        }
                        else
                        {
                            SqlCommand com = new SqlCommand(@"exec sp_BMS_WFPNew_Submit_Excess " + officeid + "," + accountid + "," + year + "," + id + "," + Account.UserInfo.eid + "", con);
                            con.Open();
                            data = Convert.ToString(com.ExecuteScalar());
                        }
                    }
                    else //tf
                    {
                        SqlCommand com = new SqlCommand(@"exec sp_BMS_WFPNew_Submit_TF " + officeid + "," + accountid + "," + year + "," + id + "," + Account.UserInfo.eid + "", con);
                        con.Open();
                        data = Convert.ToString(com.ExecuteScalar());
                    }
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string submitwfpsubppa(int? officeid = 0, int? programid = 0, long? accountid = 0, int? year = 0, int? subppaid = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFPNew_Submit_PerSubPpa] " + officeid + "," + programid + "," + accountid + "," + year + "," + subppaid + "," + Account.UserInfo.eid + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());

                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public PartialViewResult wfpsubmitted(int? office = 0, int? program = 0, long? account = 0, int? tyear = 0, int? fundid = 0, int? mode_trans = 0)
        {
            Session["office"] = office;
            Session["program"] = program;
            Session["account"] = account;
            Session["tyear"] = tyear;
            Session["fundid"] = fundid;
            Session["mode_trans"] = mode_trans;
            return PartialView("pv_WFPSubmitted");
            //  return PartialView("pv_WFPSubmitted_Tab");
        }

        public PartialViewResult wfpsubmittedv2()
        {
            return PartialView("pv_WFPSubmitted");
        }

        public PartialViewResult wfpsubmitted_tab()
        {
            return PartialView("pv_WFPSubmitted_Summary");
        }

        public string returnwfp(int? trnno = 0, string remarks = "", int? fundid = 0, int? mode_trans = 0, int? isPPMP = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    if (fundid == 0)
                    {
                        if (mode_trans == 1)
                        {
                            var data = "";
                            SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFPreturn] " + trnno + ",'" + remarks.Replace("'", "''") + "'," + Account.UserInfo.eid + "", con);
                            con.Open();
                            data = Convert.ToString(com.ExecuteScalar());
                        }
                        else
                        {
                            var data = "";
                            SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFPreturn_Excess] " + trnno + ",'" + remarks.Replace("'", "''") + "'," + isPPMP + "", con);
                            con.Open();
                            data = Convert.ToString(com.ExecuteScalar());
                        }

                    }
                    else
                    {
                        var data = "";
                        SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFPreturn_TF] " + trnno + ",'" + remarks.Replace("'", "''") + "'," + isPPMP + "", con);
                        con.Open();
                        data = Convert.ToString(com.ExecuteScalar());
                    }
                    return "success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string returnreviewedwfp(int? trnno = 0, string remarks = "", int? fundid = 0, int? mode_trans = 0, int? yearof = 0, int? isPPMP = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    if (fundid == 0)
                    {
                        if (mode_trans == 1)
                        {

                            SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFPreturn_Reviewed] " + trnno + ",'" + remarks.Replace("'", "''") + "'," + Account.UserInfo.eid + "," + isPPMP + "", con);
                            con.Open();
                            data = Convert.ToString(com.ExecuteScalar());
                        }
                        else
                        {
                            // var data = "";
                            SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFPreturn_Excess_Reviewed] " + trnno + ",'" + remarks.Replace("'", "''") + "'," + isPPMP + "," + Account.UserInfo.eid + "", con);
                            con.Open();
                            data = Convert.ToString(com.ExecuteScalar());
                        }

                    }
                    else
                    {
                        //var data = "";
                        SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFPreturn_TF_Review] " + trnno + ",'" + remarks.Replace("'", "''") + "'," + isPPMP + "," + Account.UserInfo.eid + "", con);
                        con.Open();
                        data = Convert.ToString(com.ExecuteScalar());
                    }
                    return data;//"success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string returnwfpaccount(int? programid = 0, int? accountid = 0, int? yearof = 0, string remark = "", int? fundid = 0, int? mode_trans = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    if (fundid == 0) //gf
                    {
                        if (mode_trans == 1) // current
                        {
                            SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFPAccountreturn]  " + programid + ", " + accountid + "," + yearof + ",'" + remark.Replace("'", "''").ToString() + "'," + Account.UserInfo.eid + "", con);
                            con.Open();
                            data = Convert.ToString(com.ExecuteScalar());
                        }
                        else
                        {
                            SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFPAccountreturn_Excess]  " + programid + ", " + accountid + "," + yearof + ",'" + remark.Replace("'", "''").ToString() + "'," + Account.UserInfo.eid + "", con);
                            con.Open();
                            data = Convert.ToString(com.ExecuteScalar());
                        }
                    }
                    else
                    {
                        SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFPAccountreturn_TF]  " + programid + ", " + accountid + "," + yearof + ",'" + remark.Replace("'", "''").ToString() + "'," + Account.UserInfo.eid + "", con);
                        con.Open();
                        data = Convert.ToString(com.ExecuteScalar());
                    }
                    return "success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string deletewfpaccount(int? programid = 0, int? accountid = 0, int? yearof = 0, string remark = "")
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFPAccountreturn]  " + programid + ", " + accountid + "," + yearof + ",'" + remark.Replace("'", "''").ToString() + "' ", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return "success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public PartialViewResult wfpApproved(int? office = 0, int? program = 0, long? account = 0, int? tyear = 0, int? fundid = 0, int? mode_trans = 0)
        {
            Session["office"] = office;
            Session["program"] = program;
            Session["account"] = account;
            Session["tyear"] = tyear;
            Session["fundid"] = fundid;
            Session["mode_trans"] = mode_trans;
            return PartialView("pv_WFPApproved");
        }

        public PartialViewResult wfpApproved_lce(int? office = 0, int? program = 0, long? account = 0, int? tyear = 0, int? fundid = 0, int mode_trans = 0)
        {
            Session["office"] = office;
            Session["program"] = program;
            Session["account"] = account;
            Session["tyear"] = tyear;
            Session["fundid"] = fundid;
            Session["mode_trans"] = mode_trans;
            return PartialView("pv_WFPApproved_LCE");
        }

        public PartialViewResult wfpstatus(int? office = 0, int? tyear = 0, int? fundid = 0)
        {
            Session["office"] = office;
            Session["tyear"] = tyear;
            Session["fundid"] = fundid;
            return PartialView("pv_WFPReturn");
        }

        public PartialViewResult WFPstatus_reviewonly(int? office = 0, int? program = 0, int? account = 0, int? tyear = 0)
        {
            Session["office"] = office;
            Session["tyear"] = tyear;
            Session["program"] = program;
            Session["account"] = account;
            return PartialView("pv_WFPStatus");
            //return PartialView("pv_WFPReturn");
        }
        public PartialViewResult wfplink(int? office = 0, int? program = 0, int? account = 0, int? breakdownid = 0, int? tyear = 0)
        {
            Session["office"] = office;
            Session["program"] = program;
            Session["account"] = account;
            Session["breakdownid"] = breakdownid;
            Session["tyear"] = tyear;

            if (Account.UserInfo.eid == 5580)
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"update [tbl_R_BMS_PPMPItem]  set isactive=8  where isactive=1", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    data = Convert.ToString(com.ExecuteScalar());
                }
                DataTable dt2 = db.execQuery("select a.itemid,a.itemname,a.unitcost,b.unit,a.description from l_item a , l_item_unit b where a.isactive = '1' and a.unitid =b.unitid order by a.itemname");
                for (Int32 x = 0; x < dt2.Rows.Count; x++)
                {
                    //  ppmp dataTable2 = new ppmp();
                    ppmp dataTable = new ppmp();
                    dataTable.itemid = Convert.ToInt32(dt2.Rows[x]["itemid"]);
                    dataTable.itemname = Convert.ToString(dt2.Rows[x]["itemname"]).Replace("'", "''").ToString();
                    dataTable.unitcost = Convert.ToDouble(dt2.Rows[x]["unitcost"]);
                    dataTable.unit = Convert.ToString(dt2.Rows[x]["unit"]).Replace("'", "''").ToString();
                    dataTable.description = Convert.ToString(dt2.Rows[x]["description"]).Replace("'", "''").ToString();


                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand insertItem = new SqlCommand(@"insert into [tbl_R_BMS_PPMPItem] ([ppmpitem_id],[itemname],[unitcost],[unit],[isactive],[description]) 
                        values(" + dataTable.itemid + ",'" + dataTable.itemname + "'," + dataTable.unitcost + ",'" + dataTable.unit + "',1,'" + dataTable.description + "')", con);
                        con.Open();
                        insertItem.ExecuteNonQuery();
                        con.Close();
                    }
                }

            }

            return PartialView("pv_WFPLink");
        }

        public void downloadpritems(int? office = 0, int? program = 0, int? account = 0, int? tyear = 0)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var data = "";
                //   var wfpr = "";
                SqlCommand com = new SqlCommand(@"update [IFMIS].[dbo].[tbl_R_BMS_PRItem]  set isactive=7  where isactive=1 and yearof=" + tyear + " and [officeid]=" + office + "", con);
                con.Open();
                com.CommandTimeout = 0;
                data = Convert.ToString(com.ExecuteScalar());

                //con.Close();
                //SqlCommand com2 = new SqlCommand(@"update [IFMIS].[dbo].[tbl_R_BMS_WFP_PR]  set [actioncode]=7,[dateandtime]= format(getdate(),'MM/dd/yyyy hh:mm:ss tt')  where  [yearof]=" + tyear + " and [actioncode]=1", con);
                //con.Open();
                //com2.CommandTimeout = 0;
                //wfpr = Convert.ToString(com2.ExecuteScalar());

            }
            DataTable dt2 = db.execQuery(@"select d.prid,e.fmisid,c.programid,c.accountid,d.itemid,d.item,d.description,d.unitcost,d.quantity,d.unit  from t_pr  as a 
                                            left join t_pr_object as b on a.prid = b.prid
                                            left join t_ppmp_object as c on b.ppid = c.ppid
                                            left join t_pr_items as d on a.prid = d.prid
                                            left join a_office as e on a.officeid = e.officeid
                                            where a.isactive = '1' and  a.cyear = " + tyear + " and e.fmisid=" + office + "  and COALESCE(c.excessid, 0) = 0 and d.isactive = '1'");
            for (Int32 x = 0; x < dt2.Rows.Count; x++)
            {
                //  ppmp dataTable2 = new ppmp();
                ppmp dataTable = new ppmp();
                dataTable.prid = Convert.ToInt32(dt2.Rows[x]["prid"]);
                dataTable.OfficeID = Convert.ToInt32(dt2.Rows[x]["fmisid"]);
                dataTable.ProgramID = Convert.ToInt32(dt2.Rows[x]["programid"]);
                dataTable.AccountID = Convert.ToInt32(dt2.Rows[x]["accountid"]);
                dataTable.itemid = Convert.ToInt32(dt2.Rows[x]["itemid"]);
                dataTable.itemname = Convert.ToString(dt2.Rows[x]["item"]).Replace("'", "''").ToString();
                dataTable.unitcost = Convert.ToDouble(dt2.Rows[x]["unitcost"]);
                dataTable.unit = Convert.ToString(dt2.Rows[x]["unit"]).Replace("'", "''").ToString();
                dataTable.description = Convert.ToString(dt2.Rows[x]["description"]).Replace("'", "''").ToString();
                dataTable.unitcost = Convert.ToDouble(dt2.Rows[x]["unitcost"]);
                dataTable.qty = Convert.ToDouble(dt2.Rows[x]["quantity"]);
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand insertItem = new SqlCommand(@"insert into [IFMIS].[dbo].[tbl_R_BMS_PRItem] ([pr_id],[officeid],[programid],[accountid],[itemid],[item],[description],[amount],[qty],[unit],[wfpno],isactive,yearof) 
                        values(" + dataTable.prid + ",'" + dataTable.OfficeID + "'," + dataTable.ProgramID + "," + dataTable.AccountID + "," + dataTable.itemid + ",'" + dataTable.itemname + "','" + dataTable.description + "'," + dataTable.unitcost + "," + dataTable.qty + ",'" + dataTable.unit + "','',1," + tyear + ")", con);
                    con.Open();
                    insertItem.ExecuteNonQuery();
                    con.Close();
                }
            }
            //download wfp item with pr
            //DataTable dtpr = db.execQuery(@"Select wfpid,fund_group,wfpyear from t_wfp_details_grouping where wfpyear=" + tyear + "");

            //for (Int32 x = 0; x < dtpr.Rows.Count; x++)
            //{
            //    ppmp dataTablepr = new ppmp();
            //    dataTablepr.wfpid = Convert.ToInt32(dtpr.Rows[x]["wfpid"]);
            //    dataTablepr.fundid = Convert.ToInt32(dtpr.Rows[x]["fund_group"]);
            //    using (SqlConnection con = new SqlConnection(Common.MyConn()))
            //    {
            //        SqlCommand insertItem = new SqlCommand(@"insert into [IFMIS].[dbo].[tbl_R_BMS_WFP_PR] ([accountdenomid_wfp],[fundid],[actioncode],[yearof],[dateandtime]) 
            //            values("+ dataTablepr.wfpid + ","+ dataTablepr.fundid + ",1," + tyear + ",format(getdate(),'MM/dd/yyyy hh:mm:ss tt'))", con);
            //        con.Open();
            //        insertItem.ExecuteNonQuery();
            //        con.Close();
            //    }
            //}
        }
        public string disapprovewfp(long? trnno = 0, int? programid = 0, int? accountid = 0, int? yearof = 0, int? ooeclass = 0, int? fundid = 0, string wfpno = "", int? mode_trans = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    if (fundid == 0)
                    {
                        if (mode_trans == 1)
                        {
                            SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFPdisapprove] " + trnno + "," + programid + "," + accountid + "," + yearof + "," + ooeclass + "," + Account.UserInfo.eid + "", con);
                            con.Open();
                            data = Convert.ToString(com.ExecuteScalar());
                        }
                        else
                        {
                            SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFPdisapprove_Excess] " + trnno + "," + programid + "," + accountid + "," + yearof + "," + ooeclass + "," + Account.UserInfo.eid + "", con);
                            con.Open();
                            data = Convert.ToString(com.ExecuteScalar());
                        }
                    }
                    else
                    {
                        SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFPdisapprove_TF] " + trnno + "," + programid + "," + accountid + "," + yearof + "," + ooeclass + "," + Account.UserInfo.eid + "", con);
                        con.Open();
                        data = Convert.ToString(com.ExecuteScalar());
                    }
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public ActionResult GetWFPReportHistory([DataSourceRequest]DataSourceRequest request, int? officeid = 0, int? yearof = 0)
        {
            var offid = Account.UserInfo.Department.ToString();
            string tempStr = "Select [wfp_id],upper([wfpno]) as datetimentered FROM [tbl_T_BMSWFP_xml] where officeid=" + officeid + " and yearof=" + yearof + " and actioncode=1 and isnull(approveprint,0)=1  order by cast(wfp_id as bigint) desc";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public string WFPreportemp(string[] transno)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("transno");
                var idx = 0;
                foreach (var trnno in transno)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = transno[idx];
                    dt.Rows.Add(dr);
                    idx++;
                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("dbo.[sp_BMSWFP_report_temp]", con);
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add(new SqlParameter("@trnno", dt));
                    com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                    con.Open();

                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string WFPDFPPTreportemp(string[] transno)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("transno");
                var idx = 0;
                foreach (var trnno in transno)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = transno[idx];
                    dt.Rows.Add(dr);
                    idx++;
                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("dbo.[sp_BMSWFPDFPPT_report_temp]", con);
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add(new SqlParameter("@trnno", dt));
                    com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                    con.Open();

                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public ActionResult GetOfficeProject([DataSourceRequest]DataSourceRequest request, int? officeid = 0, int? tyear = 0)
        {
            string tempStr = "select distinct project_id,project from [ifmiS].[dbo].[tbl_R_BMSAIP_SPMS] " +
                               " where ActionCode = 1 and year = " + tyear + "  and pmis_office_id in (Select PMISOfficeID " +
                               "     from ifmis.dbo.tbl_R_BMSOffices where OfficeID = " + officeid + ")";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public string setReleasedfppt(int? officeid = 0, int? programid = 0, long? accountid = 0, double firstmonFin = 0.00, double secondmonFin = 0.00, double thirdmonFin = 0.00, int? qtr = 0, int? TYear = 0, int? trnno = 0, int? id = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [IFMIS].[dbo].[sp_bms_releasedfppt_set] " + officeid + "," + programid + "," + accountid + "," + firstmonFin + "," + secondmonFin + "," + thirdmonFin + "," + qtr + "," + TYear + "," + Account.UserInfo.eid + "," + trnno + "," + id + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public ActionResult dfpptperiod([DataSourceRequest]DataSourceRequest request)
        {
            var offid = Account.UserInfo.Department.ToString();
            if (Account.UserInfo.UserTypeID >= 4)
            {
                string tempStr = "Select [id],[period] FROM [IFMIS].[dbo].[tbl_R_BMSDfpptPeriod] order by id";
                DataTable dt = tempStr.DataSet();

                var result = new ContentResult();
                result.Content = SerializeDT.DataTableToJSON(dt);
                result.ContentType = "application/json";
                return result;
            }
            else
            {
                string tempStr = "Select [id],[period] FROM [IFMIS].[dbo].[tbl_R_BMSDfpptPeriod] where [actioncode]=1 order by id";
                DataTable dt = tempStr.DataSet();

                var result = new ContentResult();
                result.Content = SerializeDT.DataTableToJSON(dt);
                result.ContentType = "application/json";
                return result;
            }
        }
        public ActionResult GetGroupActivity([DataSourceRequest]DataSourceRequest request, int? programid = 0, int? officeid = 0, int? tyear = 0)
        {
            string tempStr = "select * from fn_BMS_wfpactivity (" + officeid + "," + tyear + "," + Account.UserInfo.eid + ") order by  initiative";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult GetFundsource([DataSourceRequest]DataSourceRequest request)
        {
            string tempStr = "SELECT [FundCode],case when fundcode=101 then 'General Fund' else [FundName] end as  [FundName] FROM [IFMIS].[dbo].[tbl_R_BMSFunds] where ActionCode=1";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult GetMunicipality([DataSourceRequest]DataSourceRequest request)
        {
            string tempStr = "SELECT [citymunCode] ,ltrim(right([citymunDesc],len([citymunDesc]) - patindex('%-%',[citymunDesc]))) as Municipality  FROM [adsceir].[dbo].[tbl_l_municipality] where provCode=1603 order by [citymunDesc]";
            DataTable dt = tempStr.DataSetAds();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult GetBarangay([DataSourceRequest]DataSourceRequest request, int? muncode = 0)
        {
            string tempStr = "SELECT [brgyCode] ,upper([brgyDesc]) brgyDesc FROM [adsceir].[dbo].[tbl_l_barangay] where provCode=1603 and citymunCode=" + muncode + " order by brgyDesc";
            DataTable dt = tempStr.DataSetAds();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult AppFund()
        {
            return View("pv_AppropriationFund");
        }
        public string savelocation(int? officeid = 0, int? accountid = 0, int? municipalid = 0, string municipal = "", int? barangayid = 0, string barangay = "", int? yearof = 0)

        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec dbo.sp_bms_wfplocation " + officeid + " , " + accountid + ", " + municipalid + ", '" + municipal.Replace("'", "''").ToString() + "', " + barangayid + ", '" + barangay.Replace("'", "''").ToString() + "'," + yearof + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public JsonResult GetWFPLocation([DataSourceRequest]DataSourceRequest request, int? office = 0, int? account = 0, int? year = 0)
        {

            List<WFPrepare> prog = new List<WFPrepare>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT [loc_id],[barangay_id],barangay,[municipal_id],municipal FROM [IFMIS].[dbo].[tbl_T_BMSWFP_Location] where actioncode=1 and officeid=" + office + " and accountid=" + account + "  and yearof=" + year + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WFPrepare loc = new WFPrepare();
                    loc.locationid = Convert.ToInt32(reader.GetValue(0));
                    loc.barangayid = Convert.ToInt32(reader.GetValue(1));
                    loc.barangay = Convert.ToString(reader.GetValue(2));
                    loc.municipalid = Convert.ToInt32(reader.GetValue(3));
                    loc.municipal = Convert.ToString(reader.GetValue(4));
                    prog.Add(loc);
                }
            }
            return Json(prog.ToDataSourceResult(request));
        }


        public JsonResult GetReponsibleOffice([DataSourceRequest]DataSourceRequest request, int? office = 0, int? account = 0, int? year = 0, int? resid = 0)
        {

            List<WFPrepare> prog = new List<WFPrepare>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (resid == 1)
                {
                    SqlCommand com = new SqlCommand(@"SELECT a.[OfficeID],[OfficeAbbrivation],isnull(b.actioncode,0) as actioncode FROM [IFMIS].[dbo].[tbl_R_BMSOffices] as a " +
                                                        "left join [IFMIS].[dbo].[tbl_R_BMSWFP_Responsible_Office] as b on b.officeid_assign=a.OfficeID and b.actioncode=1 and b.officeid=" + office + " and b.accountid=" + account + " and b.yearof=" + year + " " +
                                                        "where PMISOfficeID is not null and OfficeName not like '%sef-%' and OfficeAbbrivation not like '%sef%' and a.OfficeID not in (43) order by OfficeAbbrivation", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        WFPrepare loc = new WFPrepare();
                        loc.officeid = Convert.ToInt32(reader.GetValue(0));
                        loc.officeabbr = reader.GetValue(1).ToString();
                        loc.actioncode = Convert.ToInt32(reader.GetValue(2));
                        prog.Add(loc);
                    }
                }
                else
                {
                    SqlCommand com = new SqlCommand(@"SELECT a.eid as [OfficeID],a.EmpName as [OfficeAbbrivation],isnull(b.actioncode,0) as actioncode FROM [pmis].[dbo].[vwMergeAllEmployee] as a " +
                                                        "left join ifmis.dbo.tbl_R_BMSOffices as c on c.PMISOfficeID=a.Department " +
                                                        "left join [IFMIS].[dbo].[tbl_R_BMSWFP_Responsible_Office] as b on b.eid_assign=a.eid and b.actioncode=1 and b.officeid=" + office + " and b.accountid=" + account + "  and b.yearof=" + year + " " +
                                                        "where c.OfficeID=" + office + " order by OfficeAbbrivation", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        WFPrepare loc = new WFPrepare();
                        loc.officeid = Convert.ToInt32(reader.GetValue(0));
                        loc.officeabbr = reader.GetValue(1).ToString();
                        loc.actioncode = Convert.ToInt32(reader.GetValue(2));
                        prog.Add(loc);
                    }
                }
            }
            return Json(prog.ToDataSourceResult(request));
        }

        public JsonResult GetWFPConsol([DataSourceRequest]DataSourceRequest request, int? year = 0, int? review = 0, int? showspco = 0, int? fundid = 0, int? mode_trans = 0)
        {

            List<WFPrepare> prog = new List<WFPrepare>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (fundid == 0) //gf
                {
                    if (mode_trans == 1) //current
                    {
                        SqlCommand com = new SqlCommand(@"exec sp_BMS_WFP_Consol " + year + "," + review + "," + showspco + "", con);
                        con.Open();
                        com.CommandTimeout = 0;
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            WFPrepare loc = new WFPrepare();
                            loc.officeid = Convert.ToInt32(reader.GetValue(0));
                            loc.office = Convert.ToString(reader.GetValue(1));
                            loc.programid = Convert.ToInt32(reader.GetValue(2));
                            loc.accountid = Convert.ToInt32(reader.GetValue(3));
                            loc.accountname = Convert.ToString(reader.GetValue(4));
                            loc.datetimeentered = Convert.ToString(reader.GetValue(5));
                            loc.yearof = Convert.ToInt32(reader.GetValue(6));
                            loc.remarks = Convert.ToString(reader.GetValue(7));
                            loc.ooe = Convert.ToString(reader.GetValue(8));
                            loc.ooeid = Convert.ToInt32(reader.GetValue(9));
                            loc.withapprove = Convert.ToInt32(reader.GetValue(10));
                            prog.Add(loc);
                        }
                    }
                    else //continuing
                    {
                        SqlCommand com = new SqlCommand(@"exec sp_BMS_WFP_Consol_Excess " + year + "," + review + "," + showspco + "", con);
                        con.Open();
                        com.CommandTimeout = 0;
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            WFPrepare loc = new WFPrepare();
                            loc.officeid = Convert.ToInt32(reader.GetValue(0));
                            loc.office = Convert.ToString(reader.GetValue(1));
                            loc.programid = Convert.ToInt32(reader.GetValue(2));
                            loc.accountid = Convert.ToInt32(reader.GetValue(3));
                            loc.accountname = Convert.ToString(reader.GetValue(4));
                            loc.datetimeentered = Convert.ToString(reader.GetValue(5));
                            loc.yearof = Convert.ToInt32(reader.GetValue(6));
                            loc.remarks = Convert.ToString(reader.GetValue(7));
                            loc.ooe = Convert.ToString(reader.GetValue(8));
                            loc.ooeid = Convert.ToInt32(reader.GetValue(9));
                            loc.withapprove = Convert.ToInt32(reader.GetValue(10));
                            prog.Add(loc);
                        }
                    }
                }
                else //TRUST FUND   
                {
                    SqlCommand com = new SqlCommand(@"exec sp_BMS_WFP_Consol_TF " + year + "," + review + "," + showspco + "", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        WFPrepare loc = new WFPrepare();
                        loc.officeid = Convert.ToInt32(reader.GetValue(0));
                        loc.office = Convert.ToString(reader.GetValue(1));
                        loc.programid = Convert.ToInt32(reader.GetValue(2));
                        loc.accountid = Convert.ToInt32(reader.GetValue(3));
                        loc.accountname = Convert.ToString(reader.GetValue(4));
                        loc.datetimeentered = Convert.ToString(reader.GetValue(5));
                        loc.yearof = Convert.ToInt32(reader.GetValue(6));
                        loc.remarks = Convert.ToString(reader.GetValue(7));
                        loc.ooe = Convert.ToString(reader.GetValue(8));
                        loc.ooeid = Convert.ToInt32(reader.GetValue(9));
                        loc.withapprove = Convert.ToInt32(reader.GetValue(10));
                        prog.Add(loc);
                    }
                }
            }
            //return Json(prog.ToDataSourceResult(request));
            return Json(prog.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetWFPStatus([DataSourceRequest]DataSourceRequest request, int? office = 0, int? year = 0, int? ooe = 0)
        {

            List<WFPrepare> prog = new List<WFPrepare>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFP_Status] " + office + "," + year + "," + ooe + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WFPrepare loc = new WFPrepare();
                    loc.officeid = Convert.ToInt32(reader.GetValue(0));
                    loc.office = Convert.ToString(reader.GetValue(1));
                    loc.programid = Convert.ToInt32(reader.GetValue(2));
                    loc.accountid = Convert.ToInt32(reader.GetValue(3));
                    loc.accountname = Convert.ToString(reader.GetValue(4));
                    loc.datetimeentered = Convert.ToString(reader.GetValue(5));
                    loc.yearof = Convert.ToInt32(reader.GetValue(6));
                    loc.remarks = Convert.ToString(reader.GetValue(7));
                    loc.ooe = Convert.ToString(reader.GetValue(8));
                    loc.ooeid = Convert.ToInt32(reader.GetValue(9));
                    //   loc.withapprove = Convert.ToInt32(reader.GetValue(10));
                    prog.Add(loc);
                }
            }
            return Json(prog.ToDataSourceResult(request));
        }

        public JsonResult GetWFPReturn([DataSourceRequest]DataSourceRequest request, int? office = 0, int? year = 0, int? fundid = 0)
        {

            List<WFPrepare> prog = new List<WFPrepare>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (fundid == 0)
                {
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFP_Return] " + office + "," + year + "", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        WFPrepare loc = new WFPrepare();
                        loc.officeid = Convert.ToInt32(reader.GetValue(0));
                        loc.office = Convert.ToString(reader.GetValue(1));
                        loc.programid = Convert.ToInt32(reader.GetValue(2));
                        loc.accountid = Convert.ToInt32(reader.GetValue(3));
                        loc.accountname = Convert.ToString(reader.GetValue(4));
                        loc.yearof = Convert.ToInt32(reader.GetValue(5));
                        loc.remarks = Convert.ToString(reader.GetValue(6));
                        loc.status = Convert.ToString(reader.GetValue(7));
                        loc.docid = Convert.ToInt32(reader.GetValue(9));
                        loc.returnby = Convert.ToString(reader.GetValue(10));
                        //   loc.withapprove = Convert.ToInt32(reader.GetValue(10));
                        prog.Add(loc);
                    }
                }
                else
                {
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFP_Return_TF] " + office + "," + year + "", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        WFPrepare loc = new WFPrepare();
                        loc.officeid = Convert.ToInt32(reader.GetValue(0));
                        loc.office = Convert.ToString(reader.GetValue(1));
                        loc.programid = Convert.ToInt32(reader.GetValue(2));
                        loc.accountid = Convert.ToInt32(reader.GetValue(3));
                        loc.accountname = Convert.ToString(reader.GetValue(4));
                        loc.yearof = Convert.ToInt32(reader.GetValue(5));
                        loc.remarks = Convert.ToString(reader.GetValue(6));
                        loc.status = Convert.ToString(reader.GetValue(7));
                        loc.docid = Convert.ToInt32(reader.GetValue(9));
                        loc.returnby = Convert.ToString(reader.GetValue(10));
                        //   loc.withapprove = Convert.ToInt32(reader.GetValue(10));
                        prog.Add(loc);
                    }
                }
            }
            return Json(prog.ToDataSourceResult(request));
        }

        public string GetWFPlinkitem([DataSourceRequest]DataSourceRequest request, int? office = 0, int? year = 0, int? ooe = 0)
        {
            //List<WFPrepare> prog = new List<WFPrepare>();

            //DataTable dt2 = db.execQuery("select a.itemid,a.itemname,0 ,c.itemgroup as maincategory, b.itemgroup as subcategory from l_item as a " +
            //                                   "join l_item_group as b on a.itemgroupid = b.itemgroupid " +
            //                                   "join l_item_group as c on c.itemgroupid = b.parentid " +
            //                                   "where a.isactive = '1' " +
            //                                   "ORDER BY c.itemgroup");

            DataTable dt2 = db.execQuery("select a.itemid,a.itemname,a.unitcost,b.unit from l_item a , l_item_unit b where a.isactive = '1' and a.unitid =b.unitid");
            for (Int32 x = 0; x < dt2.Rows.Count; x++)
            {
                //  ppmp dataTable2 = new ppmp();

                ppmp dataTable = new ppmp();
                dataTable.itemid = Convert.ToInt32(dt2.Rows[x]["itemid"]);
                dataTable.itemname = Convert.ToString(dt2.Rows[x]["itemname"]).Replace("'", "''").ToString();
                dataTable.unitcost = Convert.ToDouble(dt2.Rows[x]["unitcost"]);
                dataTable.unit = Convert.ToString(dt2.Rows[x]["unit"]).Replace("'", "''").ToString();


                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand insertItem = new SqlCommand(@"insert into [tbl_R_BMS_PPMPItem] ([ppmpitem_id],[itemname],[unitcost],[unit],[isactive]) 
                        values(" + dataTable.itemid + ",'" + dataTable.itemname + "'," + dataTable.unitcost + ",'" + dataTable.unit + "',1)", con);
                    con.Open();
                    insertItem.ExecuteNonQuery();
                    con.Close();
                }
            }
            return "success";
        }
        public JsonResult GetWFPrepare([DataSourceRequest]DataSourceRequest request, int? year = 0)
        {

            List<WFPrepare> prog = new List<WFPrepare>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFP_prepare] " + year + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WFPrepare loc = new WFPrepare();
                    loc.officeid = Convert.ToInt32(reader.GetValue(0));
                    loc.office = Convert.ToString(reader.GetValue(1));
                    loc.programid = Convert.ToInt32(reader.GetValue(2));
                    loc.accountid = Convert.ToInt32(reader.GetValue(3));
                    loc.accountname = Convert.ToString(reader.GetValue(4));
                    loc.datetimeentered = Convert.ToString(reader.GetValue(5));
                    loc.yearof = Convert.ToInt32(reader.GetValue(6));
                    loc.remarks = Convert.ToString(reader.GetValue(7));
                    loc.ooe = Convert.ToString(reader.GetValue(8));
                    loc.ooeid = Convert.ToInt32(reader.GetValue(9));
                    prog.Add(loc);
                }
            }
            return Json(prog.ToDataSourceResult(request));
        }
        public string res_office_sel(int? officeid_drp = 0, int? programid = 0, int? accountid = 0, int? officeid_sel = 0, int? tyear = 0, int? trulse = 0, int? resid = 0, int? activity = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFPResponsiblOffice] " + officeid_drp + "," + programid + "," + accountid + "," + officeid_sel + "," + tyear + "," + trulse + "," + resid + "," + activity + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string setreserve(int? officeid = 0, int? programid = 0, int? accountid = 0, int? yearof = 0, int? reserveincludeid = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec dbo.sp_BMS_WFP_ReserveSet " + officeid + ", " + programid + ", " + accountid + ", " + yearof + "," + reserveincludeid + "," + Account.UserInfo.eid + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string ApproveWFP_posting(string[] transno, int? yearof = 0, int? programid = 0, int? accountid = 0, int? ooeclass = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("transno");
                var idx = 0;
                foreach (var trnno in transno)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = transno[idx];
                    dt.Rows.Add(dr);
                    idx++;
                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("dbo.[sp_BMS_ApproveWFP_posting]", con);
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add(new SqlParameter("@trnno", dt));
                    com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                    com.Parameters.Add(new SqlParameter("@yearof", yearof));
                    com.Parameters.Add(new SqlParameter("@programid", programid));
                    com.Parameters.Add(new SqlParameter("@accountid", accountid));
                    com.Parameters.Add(new SqlParameter("@ooe", ooeclass));
                    con.Open();

                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string ReturnWFP_reviewed(string[] transno, string[] wfpno, int? yearof = 0, int? programid = 0, int? accountid = 0, int? ooeclass = 0, int? fundid = 0, int? mode_trans = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("transno");
                dt.Columns.Add("wfpno");
                var idx = 0;
                foreach (var trnno in transno)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = transno[idx];
                    dr[1] = wfpno[idx];
                    dt.Rows.Add(dr);
                    idx++;
                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    if (fundid == 0) //gf
                    {
                        if (mode_trans == 1)
                        {
                            SqlCommand com = new SqlCommand("dbo.[sp_BMS_WFPAccount_return]", con);
                            com.CommandType = System.Data.CommandType.StoredProcedure;
                            com.Parameters.Add(new SqlParameter("@trnno", dt));
                            com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                            com.Parameters.Add(new SqlParameter("@yearof", yearof));
                            com.Parameters.Add(new SqlParameter("@programid", programid));
                            com.Parameters.Add(new SqlParameter("@accountid", accountid));
                            com.Parameters.Add(new SqlParameter("@ooe", ooeclass));
                            con.Open();
                            return com.ExecuteScalar().ToString();
                        }
                        else
                        {
                            SqlCommand com = new SqlCommand("dbo.[sp_BMS_WFPAccount_return_Excess]", con);
                            com.CommandType = System.Data.CommandType.StoredProcedure;
                            com.Parameters.Add(new SqlParameter("@trnno", dt));
                            com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                            com.Parameters.Add(new SqlParameter("@yearof", yearof));
                            com.Parameters.Add(new SqlParameter("@programid", programid));
                            com.Parameters.Add(new SqlParameter("@accountid", accountid));
                            com.Parameters.Add(new SqlParameter("@ooe", ooeclass));
                            con.Open();
                            return com.ExecuteScalar().ToString();
                        }
                    }
                    else //tf
                    {
                        SqlCommand com = new SqlCommand("dbo.[sp_BMS_WFPAccount_return_TF]", con);
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.Parameters.Add(new SqlParameter("@trnno", dt));
                        com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                        com.Parameters.Add(new SqlParameter("@yearof", yearof));
                        com.Parameters.Add(new SqlParameter("@programid", programid));
                        com.Parameters.Add(new SqlParameter("@accountid", accountid));
                        com.Parameters.Add(new SqlParameter("@ooe", ooeclass));
                        con.Open();
                        return com.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public ActionResult GetWFitem([DataSourceRequest]DataSourceRequest request, string selname = "")
        {

            string tempStr = "Select [ppmpitem_id],[itemname]+ ' (' + cast([unitcost] as varchar(450)) + ')' as itemname FROM [IFMIS].[dbo].[tbl_R_BMS_PPMPItem] where [isactive]=1 and [unitcost] > 0 order by itemname";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
            //}
        }
        public string linkitemeproc(int? officeid = 0, int? programid = 0, long? accountid = 0, int? breakdownid = 0, int? tyear = 0, int? itemid = 0)

        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec dbo.sp_BMS_WFP_UpdatePPMPitem " + officeid + ", " + programid + ", " + accountid + "," + breakdownid + "," + tyear + "," + itemid + " ", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public PartialViewResult wfpadditem(int? office = 0, int? program = 0, int? account = 0, int? tyear = 0, int? fundid = 0, int? mode_trans = 0, int? ooeclass = 0)
        {
            Session["office"] = office;
            Session["program"] = program;
            Session["account"] = account;
            Session["tyear"] = tyear;
            Session["fundid"] = fundid;
            Session["mode_trans"] = mode_trans;
            Session["ooeclass"] = ooeclass;
            return PartialView("pv_WFPAddItem");
        }
        public PartialViewResult wfpadditempropose(int? office = 0, int? program = 0, int? account = 0, int? tyear = 0, int? fundid = 0, int? mode_trans = 0)
        {
            Session["office"] = office;
            Session["program"] = program;
            Session["account"] = account;
            Session["tyear"] = tyear;
            Session["fundid"] = fundid;
            Session["mode_trans"] = mode_trans;
            return PartialView("pv_WFPAddItem_Proposal");
        }
        public JsonResult GetPPPMPitem([DataSourceRequest]DataSourceRequest request, int? office = 0, int? program = 0, int? account = 0, int? tyear = 0, int? fundid = 0, int? mode_trans = 0, int? ooeclass = 0)
        {
            List<WFPrepare> prog = new List<WFPrepare>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (fundid == 0) //gf
                {
                    if (mode_trans == 1) //current
                    {
                        SqlCommand com = new SqlCommand(@"exec sp_BMS_WFPppmpitem_ooe " + office + "," + program + "," + account + "," + tyear + "," + ooeclass + "," + Account.UserInfo.eid + "", con);
                        con.Open();
                        com.CommandTimeout = 0;
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            WFPrepare loc = new WFPrepare();
                            loc.itemid = Convert.ToInt32(reader.GetValue(0));
                            loc.itemname = reader.GetValue(1).ToString().Replace("'", "''").ToString();
                            loc.unitcost = Convert.ToDouble(reader.GetValue(2));
                            loc.unit = Convert.ToString(reader.GetValue(3));
                            prog.Add(loc);
                        }
                    }
                    else //excess
                    {
                        SqlCommand com = new SqlCommand(@"exec sp_BMS_WFPppmpitem_Excess " + office + "," + program + "," + account + "," + tyear + "", con);
                        con.Open();
                        com.CommandTimeout = 0;
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            WFPrepare loc = new WFPrepare();
                            loc.itemid = Convert.ToInt32(reader.GetValue(0));
                            loc.itemname = reader.GetValue(1).ToString().Replace("'", "''").ToString();
                            loc.unitcost = Convert.ToDouble(reader.GetValue(2));
                            loc.unit = Convert.ToString(reader.GetValue(3));
                            prog.Add(loc);
                        }
                    }
                }
                else
                {
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFPppmpitem_TF] " + office + "," + program + "," + account + "," + tyear + "", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        WFPrepare loc = new WFPrepare();
                        loc.itemid = Convert.ToInt32(reader.GetValue(0));
                        loc.itemname = reader.GetValue(1).ToString().Replace("'", "''").ToString();
                        loc.unitcost = Convert.ToDouble(reader.GetValue(2));
                        loc.unit = Convert.ToString(reader.GetValue(3));
                        prog.Add(loc);
                    }
                }

            }

            var jsonResult = Json(prog.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            //return Json(prog.ToDataSourceResult(request));
        }

        public JsonResult GetPPPMPitem_Propose([DataSourceRequest]DataSourceRequest request, int? office = 0, int? program = 0, int? account = 0, int? tyear = 0, int? fundid = 0, int? mode_trans = 0)
        {
            List<WFPrepare> prog = new List<WFPrepare>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec sp_BMS_WFPppmpitem_Propose " + office + "," + program + "," + account + "," + tyear + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WFPrepare loc = new WFPrepare();
                    loc.itemid = Convert.ToInt32(reader.GetValue(0));
                    loc.itemname = reader.GetValue(1).ToString().Replace("'", "''").ToString();
                    loc.unitcost = Convert.ToDouble(reader.GetValue(2));
                    loc.unit = Convert.ToString(reader.GetValue(3));
                    prog.Add(loc);
                }
            }
            var jsonResult = Json(prog.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            //return Json(prog.ToDataSourceResult(request));
        }
        public string additemeproc(string[] transno, int? officeid = 0, int? programid = 0, int? accountid = 0, int? tyear = 0, int? fundid = 0, int? mode_trans = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("transno");
                var idx = 0;
                foreach (var trnno in transno)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = transno[idx];
                    dt.Rows.Add(dr);
                    idx++;
                }
                if (fundid == 0) //gf
                {
                    if (mode_trans == 1) //current
                    {
                        using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        {
                            SqlCommand com = new SqlCommand("dbo.[sp_BMS_WFPItemADD]", con);
                            com.CommandType = System.Data.CommandType.StoredProcedure;
                            com.Parameters.Add(new SqlParameter("@trnno", dt));
                            com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                            com.Parameters.Add(new SqlParameter("@officeid", officeid));
                            com.Parameters.Add(new SqlParameter("@programid", programid));
                            com.Parameters.Add(new SqlParameter("@accountid", accountid));
                            com.Parameters.Add(new SqlParameter("@year", tyear));
                            con.Open();

                            return com.ExecuteScalar().ToString();
                        }
                    }
                    else
                    {
                        using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        {
                            SqlCommand com = new SqlCommand("dbo.[sp_BMS_WFPItemADD_Excess]", con);
                            com.CommandType = System.Data.CommandType.StoredProcedure;
                            com.Parameters.Add(new SqlParameter("@trnno", dt));
                            com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                            com.Parameters.Add(new SqlParameter("@officeid", officeid));
                            com.Parameters.Add(new SqlParameter("@programid", programid));
                            com.Parameters.Add(new SqlParameter("@accountid", accountid));
                            com.Parameters.Add(new SqlParameter("@year", tyear));
                            con.Open();

                            return com.ExecuteScalar().ToString();
                        }
                    }
                }
                else //tf
                {
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand("dbo.[sp_BMS_WFPItemADD_TF]", con);
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.Parameters.Add(new SqlParameter("@trnno", dt));
                        com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                        com.Parameters.Add(new SqlParameter("@officeid", officeid));
                        com.Parameters.Add(new SqlParameter("@programid", programid));
                        com.Parameters.Add(new SqlParameter("@accountid", accountid));
                        com.Parameters.Add(new SqlParameter("@year", tyear));
                        con.Open();

                        return com.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string DeleteWFPItem(string[] transno, int? yearof = 0, int? programid = 0, int? accountid = 0, int? fundid = 0, int? mode_trans = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("transno");
                var idx = 0;
                foreach (var trnno in transno)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = transno[idx];
                    dt.Rows.Add(dr);
                    idx++;
                }
                if (fundid == 0) //gf
                {
                    if (mode_trans == 1) //current
                    {
                        using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        {

                            SqlCommand com = new SqlCommand(@"[sp_BMS_DeleteWFP_Items]", con);
                            com.CommandType = System.Data.CommandType.StoredProcedure;
                            com.Parameters.Add(new SqlParameter("@trnno", dt));
                            com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                            com.Parameters.Add(new SqlParameter("@programid", programid));
                            com.Parameters.Add(new SqlParameter("@accountid", accountid));
                            com.Parameters.Add(new SqlParameter("@year", yearof));
                            con.Open();
                            return com.ExecuteScalar().ToString();

                        }
                    }
                    else //excess
                    {
                        using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        {

                            SqlCommand com = new SqlCommand(@"[sp_BMS_DeleteWFP_Items_Excess]", con);
                            com.CommandType = System.Data.CommandType.StoredProcedure;
                            com.Parameters.Add(new SqlParameter("@trnno", dt));
                            com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                            com.Parameters.Add(new SqlParameter("@programid", programid));
                            com.Parameters.Add(new SqlParameter("@accountid", accountid));
                            com.Parameters.Add(new SqlParameter("@year", yearof));
                            con.Open();
                            return com.ExecuteScalar().ToString();

                        }
                    }
                }
                else //tf
                {
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {

                        SqlCommand com = new SqlCommand(@"[sp_BMS_DeleteWFP_Items_TF]", con);
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.Parameters.Add(new SqlParameter("@trnno", dt));
                        com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                        com.Parameters.Add(new SqlParameter("@programid", programid));
                        com.Parameters.Add(new SqlParameter("@accountid", accountid));
                        com.Parameters.Add(new SqlParameter("@year", yearof));
                        con.Open();
                        return com.ExecuteScalar().ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public ActionResult GetProOffice([DataSourceRequest]DataSourceRequest request)
        {
            var offid = Account.UserInfo.Department.ToString();

            string tempStr = "select OfficeID, REPLACE(OfficeAbbrivation, ' ', '') as offname from tbl_R_BMSOffices order BY OfficeAbbrivation";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        byte[] fileData = null;
        string filePathbin = "";
        public string ApproveWFP(string[] transno, int? yearof = 0, int? programid = 0, int? accountid = 0, int? ooeclass = 0, int? officeid = 0, string officename = "", int? prepby = 0, int? repHistory = 0, int? printstatus = 0, int? projectaip = 0, string fundsource = "", string accountname = "", string municipal = "", string barangay = "", string ooeclassname = "", int pgas_loc = 0, int? activityid = 0, int? fundid = 0, int? reviewpersub = 0, int? subppaid = 0, int? mode_trans = 0, int? specid = 0, int? issupplemetalid = 0)
        {
            var retstr = "";

            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("transno");
                var idx = 0;
                if (accountid != 59787)
                {
                    foreach (var trnno in transno)
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = transno[idx];
                        dt.Rows.Add(dr);
                        idx++;
                    }
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    if (specid != 0)
                    {
                        SqlCommand com = new SqlCommand("dbo.[sp_BMS_ApproveWFP_PerSpecificActivity]", con);
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.Parameters.Add(new SqlParameter("@trnno", dt));
                        com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                        com.Parameters.Add(new SqlParameter("@yearof", yearof));
                        com.Parameters.Add(new SqlParameter("@programid", programid));
                        com.Parameters.Add(new SqlParameter("@accountid", accountid));
                        com.Parameters.Add(new SqlParameter("@ooe", ooeclass));
                        com.Parameters.Add(new SqlParameter("@subppaid", subppaid));
                        com.Parameters.Add(new SqlParameter("@specid", specid));

                        con.Open();
                        retstr = com.ExecuteScalar().ToString();
                    }
                    else
                    {
                        if (reviewpersub == 1 && subppaid != 0 && mode_trans == 1) //per sub-ppa
                        {
                            SqlCommand com = new SqlCommand("dbo.[sp_BMS_ApproveWFP_PerSubPPA]", con);
                            com.CommandType = System.Data.CommandType.StoredProcedure;
                            com.Parameters.Add(new SqlParameter("@trnno", dt));
                            com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                            com.Parameters.Add(new SqlParameter("@yearof", yearof));
                            com.Parameters.Add(new SqlParameter("@programid", programid));
                            com.Parameters.Add(new SqlParameter("@accountid", accountid));
                            com.Parameters.Add(new SqlParameter("@ooe", ooeclass));
                            com.Parameters.Add(new SqlParameter("@subppaid", subppaid));
                            con.Open();
                            retstr = com.ExecuteScalar().ToString();
                        }
                        else
                        {
                            if (fundid == 0) //gf
                            {
                                if (mode_trans == 1)// current
                                {
                                    SqlCommand com = new SqlCommand("dbo.[sp_BMS_ApproveWFP]", con);
                                    com.CommandType = System.Data.CommandType.StoredProcedure;
                                    com.Parameters.Add(new SqlParameter("@trnno", dt));
                                    com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                                    com.Parameters.Add(new SqlParameter("@yearof", yearof));
                                    com.Parameters.Add(new SqlParameter("@programid", programid));
                                    com.Parameters.Add(new SqlParameter("@accountid", accountid));
                                    com.Parameters.Add(new SqlParameter("@ooe", ooeclass));
                                    con.Open();
                                    retstr = com.ExecuteScalar().ToString();
                                }
                                else //excess
                                {
                                    SqlCommand com = new SqlCommand("dbo.[sp_BMS_ApproveWFP_Excess]", con);
                                    com.CommandType = System.Data.CommandType.StoredProcedure;
                                    com.Parameters.Add(new SqlParameter("@trnno", dt));
                                    com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                                    com.Parameters.Add(new SqlParameter("@yearof", yearof));
                                    com.Parameters.Add(new SqlParameter("@officeid", officeid));
                                    com.Parameters.Add(new SqlParameter("@accountid", accountid));
                                    com.Parameters.Add(new SqlParameter("@ooe", ooeclass));
                                    con.Open();
                                    retstr = com.ExecuteScalar().ToString();
                                }
                            }
                            else //trust fund
                            {
                                SqlCommand com = new SqlCommand("dbo.[sp_BMS_ApproveWFP_TF]", con);
                                com.CommandType = System.Data.CommandType.StoredProcedure;
                                com.Parameters.Add(new SqlParameter("@trnno", dt));
                                com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                                com.Parameters.Add(new SqlParameter("@yearof", yearof));
                                com.Parameters.Add(new SqlParameter("@officeid", officeid));
                                com.Parameters.Add(new SqlParameter("@accountid", accountid));
                                com.Parameters.Add(new SqlParameter("@ooe", ooeclass));
                                con.Open();
                                retstr = com.ExecuteScalar().ToString();
                            }
                        }
                    }
                    // SqlConnection connection = new SqlConnection(Common.MyConn());

                    var deviceInfo = new System.Collections.Hashtable();
                    InstanceReportSource rs = new Telerik.Reporting.InstanceReportSource();
                    var reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();
                    var typeReportSource = new Telerik.Reporting.TypeReportSource();
                    if (fundid == 0)
                    {
                        if (mode_trans == 1)// current
                        {
                            rs.ReportDocument = new WFPNew(yearof, officeid, 0, 0, officename, "", "", repHistory, prepby, printstatus, projectaip, "", accountid, accountname, municipal, barangay, ooeclassname, pgas_loc, 1, programid, activityid, fundid, subppaid, specid, issupplemetalid);
                        }
                        else
                        {
                            rs.ReportDocument = new WFPNewExcess(yearof, officeid, 0, 0, officename, "", "", repHistory, prepby, printstatus, projectaip, "", accountid, accountname, municipal, barangay, ooeclassname, pgas_loc, 1, programid, activityid, fundid);
                        }
                    }
                    else
                    {
                        rs.ReportDocument = new WFPNewTF(yearof, officeid, 0, 0, officename, "", "", repHistory, prepby, printstatus, projectaip, "", accountid, accountname, municipal, barangay, ooeclassname, pgas_loc, 1, programid, activityid, fundid);
                    }
                    Telerik.Reporting.Processing.RenderingResult result = reportProcessor.RenderReport("PDF", rs, null);

                    string fileName = "";
                    NetworkCredential credentials = new NetworkCredential(@"pgas.ph\rhayan.gubalane", "DomainUser1");
                    //var credential = new NetworkCredential(username, password);
                    var server_path = networkPath + "\\" + Account.UserInfo.eid;

                    DataTable _wfno = new DataTable();
                    string _sqlwfp = "";
                    string strwfpno = "";
                    string strwfpNOPDF = "";
                    string strwfpnofile = "";
                    string strwfpno_only = "";

                    var regenerate = 0;

                    if (ooeclass != 1) // not include PS
                    {
                        //DataTable _wfno = new DataTable();
                        //string _sqlwfp = "Select upper([wfpno]) from ifmis.dbo.tbl_T_BMSWFP_xml where [qrcode]='" + GlobalFunctions.QR_globalstr + "' and isnull(approveprint,0)=1";
                        if (fundid == 0) // GF
                        {
                            if (mode_trans == 1)// current
                            {
                                _sqlwfp = "exec sp_bms_WFP_TotalAmount " + officeid + "," + accountid + ",'" + GlobalFunctions.QR_globalstr + "'," + yearof + "";
                            }
                            else
                            {
                                _sqlwfp = "exec sp_bms_WFP_TotalAmount_Excess " + officeid + "," + accountid + ",'" + GlobalFunctions.QR_globalstr + "'," + yearof + "";
                            }
                        }
                        else //TRUST FUND
                        {
                            _sqlwfp = "exec sp_bms_WFP_TotalAmount_TF " + officeid + "," + accountid + ",'" + GlobalFunctions.QR_globalstr + "'," + yearof + "";
                        }
                        _wfno = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlwfp).Tables[0];
                        strwfpno = _wfno.Rows[0][0].ToString() + ".pdf";
                        strwfpNOPDF = "'" + _wfno.Rows[0][0].ToString() + "'";
                        strwfpno_only = _wfno.Rows[0][0].ToString();
                        strwfpnofile = _wfno.Rows[0][1].ToString();
                        //var regenerate = 0;
                        if (!result.HasErrors)
                        {

                            fileName = strwfpno;
                            //fileName = "WFP-25-4427.pdf";
                            //string path = "C:\\Users\\admin\\Documents\\Public\\WFP";// System.IO.Path.GetTempPath(); //LOCAL Connection
                            string path = networkPath;// System.IO.Path.GetTempPath();
                            string filePath = System.IO.Path.Combine(path, fileName);

                            //to be used for NAS only
                            //using (new ConnectToSharedFolder(networkPath, credentials))
                            //{
                            //to be used for NAS only
                            //string nas = @"\\192.168.2.210\pgas_attachment\bms\WFP";
                            //string networkPath2 = @"\\192.168.2.210\pgas_attachment\bms\WFP\" + fileName + "";

                            //temporary save to local computer
                            string nas = @"r:\Web Application\iFMIS-BMS_publish";
                            string networkPath2 = @"r:\Web Application\iFMIS-BMS_publish\" + fileName + "";
                            //temporary save to local computer
                            try
                            {
                                if (Directory.Exists(nas))
                                {
                                    ViewBag.Result = "NAS Connectivity Test Successful";
                                    if (System.IO.File.Exists(networkPath2))
                                    {
                                        System.IO.File.Delete(networkPath2);

                                        //System.IO.File.CreateDirectory(networkPath2);
                                        var fileList = Directory.GetDirectories(networkPath);
                                        using (System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                                        {
                                            fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
                                        }
                                        regenerate = 1;
                                    }
                                    else
                                    {
                                        //Directory.CreateDirectory(networkPath2);
                                        //path = networkPath2 + "/file.pdf";
                                        //pdfdoc.SaveToFile(path, FileFormat.PDF);
                                        // 
                                        var fileList = Directory.GetDirectories(networkPath);
                                        using (System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                                        {
                                            fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
                                        }
                                        regenerate = 0;
                                    }
                                }
                                else
                                {
                                    con.Close();
                                    ViewBag.Result = "NAS Connectivity Failed";
                                    var wfperr = "";
                                    SqlCommand wfperrep = new SqlCommand(@"insert into [IFMIS].[dbo].[tbl_T_BMSWFP_Errorlog] ([officeid],[accountid],[particular],[userid]) " +
                                                                                           "values(" + officeid + "," + accountid + ",'NAS Connectivity Failed'," + Account.UserInfo.eid + ") ", con);
                                    con.Open();
                                    wfperr = Convert.ToString(wfperrep.ExecuteScalar());
                                    retstr = "ErrorNAS";
                                }

                                //PDF to binary file - START
                                filePathbin = @"r:\Web Application\iFMIS-BMS_publish\" + fileName + "";
                                fileData = System.IO.File.ReadAllBytes(filePathbin); // Convert PDF to binary

                                //using (SqlConnection conn = new SqlConnection(Common.MyConn()))
                                //using (SqlCommand cmd = new SqlCommand("insert into [ifmis].[dbo].[tbl_T_BMSWFP_PDFtoBinary] ([filename],[filedata],[actioncode],[tyear]) VALUES (@FileName, @FileData,@actioncode,@tyear)", conn))
                                //{
                                //    cmd.Parameters.AddWithValue("@FileName", fileName);
                                //    cmd.Parameters.AddWithValue("@actioncode", "1");
                                //    cmd.Parameters.AddWithValue("@tyear", yearof);
                                //    cmd.Parameters.Add("@FileData", SqlDbType.VarBinary).Value = fileData;

                                //    conn.Open();
                                //    cmd.ExecuteNonQuery();
                                //}
                                //delete the PDF file
                                if (System.IO.File.Exists(networkPath2))
                                {
                                    System.IO.File.Delete(filePath);
                                    Console.WriteLine("File deleted successfully.");
                                }
                                //pdf to binary file - END
                            }
                            catch (Exception ex)
                            {
                                con.Close();
                                ViewBag.Result = "NAS Connectivity Failed! " + ex.Message;
                                var wfperr = "";
                                SqlCommand wfperrep = new SqlCommand(@"insert into [IFMIS].[dbo].[tbl_T_BMSWFP_Errorlog] ([officeid],[accountid],[particular],[userid]) " +
                                                                                       "values(" + officeid + "," + accountid + ",'" + ex.Message + "'," + Account.UserInfo.eid + ") ", con);
                                con.Open();
                                wfperr = Convert.ToString(wfperrep.ExecuteScalar());
                                retstr = "ErrorNAS";
                            }
                            //}

                            con.Close();
                            //DG Signature --- start --
                            var prep_userid = 0;
                            var prep_dephead = 0;
                            var prep_officeid = 0;
                            var sig_usertype = 0;
                            var sign_eid_gov = 0;

                            DataTable wfpsig_id = new DataTable();
                            string _sqlsign = "SELECT eid FROM  [IFMIS].[dbo].[tbl_R_BMS_WFPsignatory] where orderno=8  and yearof=" + yearof + "";
                            wfpsig_id = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlsign).Tables[0];
                            if (wfpsig_id.Rows.Count > 0)
                            {
                                sign_eid_gov = Convert.ToInt32(wfpsig_id.Rows[0][0]);
                            }

                            if (regenerate == 0)
                            {
                                if (fundid == 0)
                                {
                                    if (mode_trans == 1)
                                    {
                                        if (accountid == 2861)
                                        {
                                            DataTable prep_id = new DataTable();
                                            string _sqlprep = "exec sp_BMS_WFP_Preparer_PerActivity_officeowner " + programid + "," + accountid + "," + yearof + ","+ officeid +"," + activityid + "";
                                            prep_id = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlprep).Tables[0];
                                            if (prep_id.Rows.Count > 0)
                                            {
                                                prep_userid = Convert.ToInt32(prep_id.Rows[0][5]);
                                                prep_dephead = Convert.ToInt32(prep_id.Rows[0][4]);
                                            }
                                        }
                                        else
                                        {
                                            if (activityid == 0)
                                            {
                                                DataTable prep_id = new DataTable();
                                                string _sqlprep = "exec sp_BMS_WFP_Preparer_PerActivity " + programid + "," + accountid + "," + yearof + "";
                                                prep_id = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlprep).Tables[0];
                                                if (prep_id.Rows.Count > 0)
                                                {
                                                    prep_userid = Convert.ToInt32(prep_id.Rows[0][5]);
                                                    prep_dephead = Convert.ToInt32(prep_id.Rows[0][4]);
                                                    sig_usertype = Convert.ToInt32(prep_id.Rows[0][9]);
                                                    prep_officeid = Convert.ToInt32(prep_id.Rows[0][3]);
                                                }
                                            }
                                            else
                                            {
                                                DataTable prep_id = new DataTable();
                                                string _sqlprep = "exec sp_BMS_WFP_Preparer " + officeid + ", " + programid + "," + accountid + "," + yearof + "," + activityid + "";
                                                prep_id = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlprep).Tables[0];
                                                if (prep_id.Rows.Count > 0)
                                                {
                                                    prep_userid = Convert.ToInt32(prep_id.Rows[0][5]);
                                                    prep_dephead = Convert.ToInt32(prep_id.Rows[0][4]);
                                                    sig_usertype = Convert.ToInt32(prep_id.Rows[0][9]);
                                                    prep_officeid = Convert.ToInt32(prep_id.Rows[0][3]);
                                                }
                                            }
                                        }
                                    }
                                    else // excess
                                    {
                                        DataTable prep_id = new DataTable();
                                        string _sqlprep = "exec sp_BMS_WFP_Preparer_TF " + officeid + "," + accountid + "," + yearof + "," + activityid + "";
                                        prep_id = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlprep).Tables[0];
                                        if (prep_id.Rows.Count > 0)
                                        {
                                            prep_userid = Convert.ToInt32(prep_id.Rows[0][5]);
                                            prep_dephead = Convert.ToInt32(prep_id.Rows[0][4]);
                                            sig_usertype = Convert.ToInt32(prep_id.Rows[0][9]);
                                        }
                                    }
                                }
                                else
                                {
                                    DataTable prep_id = new DataTable();
                                    string _sqlprep = "exec sp_BMS_WFP_Preparer_TF " + officeid + "," + accountid + "," + yearof + "," + activityid + "";
                                    prep_id = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlprep).Tables[0];
                                    if (prep_id.Rows.Count > 0)
                                    {
                                        prep_userid = Convert.ToInt32(prep_id.Rows[0][5]);
                                        prep_dephead = Convert.ToInt32(prep_id.Rows[0][4]);
                                        sig_usertype = Convert.ToInt32(prep_id.Rows[0][9]);
                                    }
                                }
                                var data2 = "";
                                var datareview = "";
                                var recomapproval2 = 0;
                                if (fundid == 0)//gf
                                {
                                    recomapproval2 = 2635;
                                }
                                else //tf
                                {
                                    recomapproval2 = 628;
                                }
                                var str_random = RandomString(10);

                                //Check if wfp to be reviewed by other end-user - start
                                string wfpreview = "";
                                var review_eid = 0;
                                DataTable _wfpreview = new DataTable();

                                wfpreview = "Select reviewby_eid FROM  ifmis.dbo.tbl_R_BMSWFP_ReviewOffice WHERE  (actioncode = 1) AND (yearof = " + yearof + ") AND (officeid = " + officeid + ")";
                                _wfpreview = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, wfpreview).Tables[0];
                                if (_wfpreview.Rows.Count > 0)
                                {
                                    review_eid = _wfpreview.Rows[0][0].ToInt();

                                }
                                //Check if wfp has to be reviewed by other end-user - end
                                if (review_eid != 0)
                                {
                                    //SqlCommand com2 = new SqlCommand(@"insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                    //                                       //"values('"+ strwfpno + "',1,NULL,'bms/WFP','WFP - " + yearof + "',1,'5582,5582,5582,5582,5582',getdate(),5582,'','" + str_random + "',1,11,11) ", con);
                                    //                                       "values('" + strwfpno + "',1,NULL,'bms/WFP','" + strwfpnofile.Replace("'", "''").ToString() + "',1,'" + 5580 + "',getdate()," + prep_userid + ",'','" + str_random + "',1,11,11) ", con);
                                    //con.Open();
                                    //data2 = Convert.ToString(com2.ExecuteScalar());

                                    using (SqlCommand cmd = new SqlCommand("insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                                                      "values(@doc_name,@doc_type,@doc_attachement,@location,@doc_description,@doc_status_id,@doc_designated,@doc_datetime,@doc_eid,@doc_datetime_update,@doc_code,@doc_signatory_type,@doc_is,@doc_type_id) ", con))
                                    {
                                        cmd.Parameters.AddWithValue("@doc_name", fileName);
                                        cmd.Parameters.AddWithValue("@doc_type", "0");
                                        cmd.Parameters.AddWithValue("@location", "'bms/WFP'");
                                        cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                        cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                        cmd.Parameters.AddWithValue("@doc_designated", prep_userid + ",1604,59,344880," + sign_eid_gov);
                                        cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@doc_eid", prep_userid);
                                        cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                        cmd.Parameters.AddWithValue("@doc_code", str_random);
                                        cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                        cmd.Parameters.AddWithValue("@doc_is", "11");
                                        cmd.Parameters.AddWithValue("@doc_type_id", "11");
                                        cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                                        con.Open();
                                        cmd.ExecuteNonQuery();
                                    }

                                    con.Close();
                                    for (int i = 0; i < _wfpreview.Rows.Count; i++)
                                    {
                                        if (officeid == 19) //ppdo
                                        {
                                            SqlCommand com3 = new SqlCommand(@"insert into [ifmis].[dbo].[tbl_R_BMSWFP_Review] (  officeid, programid, accountid, wfpno, reviewby_eid, signatories, actioncode, yearof,orderno) " +
                                                                               //"values('"+ strwfpno + "',1,NULL,'bms/WFP','WFP - " + yearof + "',1,'5582,5582,5582,5582,5582',getdate(),5582,'','" + str_random + "',1,11,11) ", con);
                                                                               "values(" + officeid + "," + programid + "," + accountid + ",'" + strwfpno + "'," + _wfpreview.Rows[i][0].ToInt() + ",'59," + prep_dephead + "," + recomapproval2 + ",344880," + sign_eid_gov + "',1," + yearof + ", (" + i + "+ 1)) ", con);
                                            con.Open();
                                            datareview = Convert.ToString(com3.ExecuteScalar());

                                        }
                                        else if (officeid == 63) //pdrrm
                                        {
                                            SqlCommand com3 = new SqlCommand(@"insert into [ifmis].[dbo].[tbl_R_BMSWFP_Review] (  officeid, programid, accountid, wfpno, reviewby_eid, signatories, actioncode, yearof,orderno) " +
                                                                               //"values('"+ strwfpno + "',1,NULL,'bms/WFP','WFP - " + yearof + "',1,'5582,5582,5582,5582,5582',getdate(),5582,'','" + str_random + "',1,11,11) ", con);
                                                                               "values(" + officeid + "," + programid + "," + accountid + ",'" + strwfpno + "'," + _wfpreview.Rows[i][0].ToInt() + ",'1604,59," + recomapproval2 + ",344880," + sign_eid_gov + "',1," + yearof + ", (" + i + "+ 1)) ", con);
                                            con.Open();
                                            datareview = Convert.ToString(com3.ExecuteScalar());
                                        }
                                        else
                                        {
                                            //if (fundid == 0) //gf
                                            //{
                                            if ((prep_officeid == 1 && prep_dephead != 19) || officeid == 1) //PGO with national offices
                                            {

                                                SqlCommand com3 = new SqlCommand(@"insert into [ifmis].[dbo].[tbl_R_BMSWFP_Review] (  officeid, programid, accountid, wfpno, reviewby_eid, signatories, actioncode, yearof,orderno) " +
                                                                               //"values('"+ strwfpno + "',1,NULL,'bms/WFP','WFP - " + yearof + "',1,'5582,5582,5582,5582,5582',getdate(),5582,'','" + str_random + "',1,11,11) ", con);
                                                                               "values(" + officeid + "," + programid + "," + accountid + ",'" + strwfpno + "'," + _wfpreview.Rows[i][0].ToInt() + ",'19,59," + recomapproval2 + ",344880," + sign_eid_gov + "',1," + yearof + ", (" + i + "+ 1)) ", con);
                                                con.Open();
                                                datareview = Convert.ToString(com3.ExecuteScalar());

                                            }
                                            else
                                            {
                                                if (officeid == 26 && fundid != 0) //dopm -add preparer countersign -TRUST FUND
                                                {
                                                    SqlCommand com3 = new SqlCommand(@"insert into [ifmis].[dbo].[tbl_R_BMSWFP_Review] (  officeid, programid, accountid, wfpno, reviewby_eid, signatories, actioncode, yearof,orderno) " +
                                                                                   //"values('"+ strwfpno + "',1,NULL,'bms/WFP','WFP - " + yearof + "',1,'5582,5582,5582,5582,5582',getdate(),5582,'','" + str_random + "',1,11,11) ", con);
                                                                                   "values(" + officeid + "," + programid + "," + accountid + ",'" + strwfpno + "'," + _wfpreview.Rows[i][0].ToInt() + ",'" + prep_dephead + ",59," + recomapproval2 + ",344880," + sign_eid_gov + "',1," + yearof + ", (" + i + "+ 1)) ", con);
                                                    con.Open();
                                                    datareview = Convert.ToString(com3.ExecuteScalar());
                                                }
                                                else
                                                {
                                                    SqlCommand com3 = new SqlCommand(@"insert into [ifmis].[dbo].[tbl_R_BMSWFP_Review] (  officeid, programid, accountid, wfpno, reviewby_eid, signatories, actioncode, yearof,orderno) " +
                                                                                   //"values('"+ strwfpno + "',1,NULL,'bms/WFP','WFP - " + yearof + "',1,'5582,5582,5582,5582,5582',getdate(),5582,'','" + str_random + "',1,11,11) ", con);
                                                                                   "values(" + officeid + "," + programid + "," + accountid + ",'" + strwfpno + "'," + _wfpreview.Rows[i][0].ToInt() + ",'" + prep_dephead + ",59," + recomapproval2 + ",344880," + sign_eid_gov + "',1," + yearof + ", (" + i + "+ 1)) ", con);
                                                    con.Open();
                                                    datareview = Convert.ToString(com3.ExecuteScalar());
                                                }
                                            }
                                        }
                                        con.Close();
                                    }

                                }
                                else
                                {
                                    if (programid == 218)
                                    {
                                        //SqlCommand com2 = new SqlCommand(@"insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                        //                                   //"values('"+ strwfpno + "',1,NULL,'bms/WFP','WFP - " + yearof + "',1,'5582,5582,5582,5582,5582',getdate(),5582,'','" + str_random + "',1,11,11) ", con);
                                        //                                   "values('" + strwfpno + "',1,NULL,'bms/WFP','" + strwfpnofile.Replace("'", "''").ToString() + "',1,'" + prep_userid + ",1604,59," + recomapproval2 + ",344880,"+ sign_eid_gov +"',getdate()," + prep_userid + ",'','" + str_random + "',1,11,11) ", con);
                                        //con.Open();
                                        //data2 = Convert.ToString(com2.ExecuteScalar());

                                        using (SqlCommand cmd = new SqlCommand("insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                                                      "values(@doc_name,@doc_type,@doc_attachement,@location,@doc_description,@doc_status_id,@doc_designated,@doc_datetime,@doc_eid,@doc_datetime_update,@doc_code,@doc_signatory_type,@doc_is,@doc_type_id) ", con))
                                        {
                                            cmd.Parameters.AddWithValue("@doc_name", fileName);
                                            cmd.Parameters.AddWithValue("@doc_type", "0");
                                            cmd.Parameters.AddWithValue("@location", "'bms/WFP'");
                                            cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                            cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                            cmd.Parameters.AddWithValue("@doc_designated", prep_userid + ",1604,59," + recomapproval2 + ",344880," + sign_eid_gov);
                                            cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                            cmd.Parameters.AddWithValue("@doc_eid", prep_userid);
                                            cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                            cmd.Parameters.AddWithValue("@doc_code", str_random);
                                            cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                            cmd.Parameters.AddWithValue("@doc_is", "11");
                                            cmd.Parameters.AddWithValue("@doc_type_id", "11");
                                            cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                                            con.Open();
                                            cmd.ExecuteNonQuery();
                                        }

                                    }
                                    else
                                    {
                                        if (officeid == 19) //ppdo
                                        {
                                            //SqlCommand com2 = new SqlCommand(@"insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                            //                                   //"values('"+ strwfpno + "',1,NULL,'bms/WFP','WFP - " + yearof + "',1,'5582,5582,5582,5582,5582',getdate(),5582,'','" + str_random + "',1,11,11) ", con);
                                            //                                   "values('" + strwfpno + "',1,NULL,'bms/WFP','" + strwfpnofile.Replace("'", "''").ToString() + "',1,'" + prep_userid + ",59," + prep_dephead + "," + recomapproval2 + ",344880,"+ sign_eid_gov +"',getdate()," + prep_userid + ",'','" + str_random + "',1,11,11) ", con);
                                            //con.Open();
                                            //data2 = Convert.ToString(com2.ExecuteScalar());

                                            using (SqlCommand cmd = new SqlCommand("insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                                                     "values(@doc_name,@doc_type,@doc_attachement,@location,@doc_description,@doc_status_id,@doc_designated,@doc_datetime,@doc_eid,@doc_datetime_update,@doc_code,@doc_signatory_type,@doc_is,@doc_type_id) ", con))
                                            {
                                                cmd.Parameters.AddWithValue("@doc_name", fileName);
                                                cmd.Parameters.AddWithValue("@doc_type", "0");
                                                cmd.Parameters.AddWithValue("@location", "'bms/WFP'");
                                                cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                                cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                                cmd.Parameters.AddWithValue("@doc_designated", prep_userid + ",59," + recomapproval2 + ",344880," + sign_eid_gov);
                                                cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                                cmd.Parameters.AddWithValue("@doc_eid", prep_userid);
                                                cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                                cmd.Parameters.AddWithValue("@doc_code", str_random);
                                                cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                                cmd.Parameters.AddWithValue("@doc_is", "11");
                                                cmd.Parameters.AddWithValue("@doc_type_id", "11");
                                                cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                                                con.Open();
                                                cmd.ExecuteNonQuery();
                                            }
                                        }
                                        else if (officeid == 63) //pdrrm
                                        {
                                            //SqlCommand com2 = new SqlCommand(@"insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                            //                                       //"values('"+ strwfpno + "',1,NULL,'bms/WFP','WFP - " + yearof + "',1,'5582,5582,5582,5582,5582',getdate(),5582,'','" + str_random + "',1,11,11) ", con);
                                            //                                       "values('" + strwfpno + "',1,NULL,'bms/WFP','" + strwfpnofile.Replace("'", "''").ToString() + "',1,'" + prep_userid + ",1604,59," + recomapproval2 + ",344880,"+ sign_eid_gov +"',getdate()," + prep_userid + ",'','" + str_random + "',1,11,11) ", con);
                                            //con.Open();
                                            //data2 = Convert.ToString(com2.ExecuteScalar());

                                            using (SqlCommand cmd = new SqlCommand("insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                                                    "values(@doc_name,@doc_type,@doc_attachement,@location,@doc_description,@doc_status_id,@doc_designated,@doc_datetime,@doc_eid,@doc_datetime_update,@doc_code,@doc_signatory_type,@doc_is,@doc_type_id) ", con))
                                            {
                                                cmd.Parameters.AddWithValue("@doc_name", fileName);
                                                cmd.Parameters.AddWithValue("@doc_type", "0");
                                                cmd.Parameters.AddWithValue("@location", "'bms/WFP'");
                                                cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                                cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                                cmd.Parameters.AddWithValue("@doc_designated", prep_userid + ",365263,1604,59," + recomapproval2 + ",344880," + sign_eid_gov);
                                                cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                                cmd.Parameters.AddWithValue("@doc_eid", prep_userid);
                                                cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                                cmd.Parameters.AddWithValue("@doc_code", str_random);
                                                cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                                cmd.Parameters.AddWithValue("@doc_is", "11");
                                                cmd.Parameters.AddWithValue("@doc_type_id", "11");
                                                cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                                                con.Open();
                                                cmd.ExecuteNonQuery();
                                            }
                                        }
                                        else
                                        {
                                            //if (fundid == 0) //gf
                                            //{
                                            if (officeid == 1) //PGO with national offices
                                            {
                                                if (prep_dephead == 303955)
                                                {
                                                    //SqlCommand com2 = new SqlCommand(@"insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                    //                                      //"values('"+ strwfpno + "',1,NULL,'bms/WFP','WFP - " + yearof + "',1,'5582,5582,5582,5582,5582',getdate(),5582,'','" + str_random + "',1,11,11) ", con);
                                                    //                                      "values('" + strwfpno + "',1,NULL,'bms/WFP','" + strwfpnofile.Replace("'", "''").ToString() + "',1,'" + prep_userid + ",19,59," + recomapproval2 + ",344880," + sign_eid_gov + "',getdate()," + prep_userid + ",'','" + str_random + "',1,11,11) ", con);


                                                    //con.Open();
                                                    //data2 = Convert.ToString(com2.ExecuteScalar());

                                                    using (SqlCommand cmd = new SqlCommand("insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                                                    "values(@doc_name,@doc_type,@doc_attachement,@location,@doc_description,@doc_status_id,@doc_designated,@doc_datetime,@doc_eid,@doc_datetime_update,@doc_code,@doc_signatory_type,@doc_is,@doc_type_id) ", con))
                                                    {
                                                        cmd.Parameters.AddWithValue("@doc_name", fileName);
                                                        cmd.Parameters.AddWithValue("@doc_type", "0");
                                                        cmd.Parameters.AddWithValue("@location", "'bms/WFP'");
                                                        cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                                        cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                                        cmd.Parameters.AddWithValue("@doc_designated", prep_userid + ",0,303955,59," + recomapproval2 + ",344880," + sign_eid_gov);
                                                        cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                                        cmd.Parameters.AddWithValue("@doc_eid", prep_userid);
                                                        cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                                        cmd.Parameters.AddWithValue("@doc_code", str_random);
                                                        cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                                        cmd.Parameters.AddWithValue("@doc_is", "11");
                                                        cmd.Parameters.AddWithValue("@doc_type_id", "11");
                                                        cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                                                        con.Open();
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }
                                                else if (programid == 1 || programid == 106 || programid == 218 || programid == 217 || programid == 216)
                                                {
                                                    using (SqlCommand cmd = new SqlCommand("insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                                                       "values(@doc_name,@doc_type,@doc_attachement,@location,@doc_description,@doc_status_id,@doc_designated,@doc_datetime,@doc_eid,@doc_datetime_update,@doc_code,@doc_signatory_type,@doc_is,@doc_type_id) ", con))
                                                    {
                                                        cmd.Parameters.AddWithValue("@doc_name", fileName);
                                                        cmd.Parameters.AddWithValue("@doc_type", "0");
                                                        cmd.Parameters.AddWithValue("@location", "'bms/WFP'");
                                                        cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                                        cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                                        cmd.Parameters.AddWithValue("@doc_designated", prep_userid + ",0,303955,59," + recomapproval2 + ",344880," + sign_eid_gov);
                                                        cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                                        cmd.Parameters.AddWithValue("@doc_eid", prep_userid);
                                                        cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                                        cmd.Parameters.AddWithValue("@doc_code", str_random);
                                                        cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                                        cmd.Parameters.AddWithValue("@doc_is", "11");
                                                        cmd.Parameters.AddWithValue("@doc_type_id", "11");
                                                        cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                                                        con.Open();
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }
                                                else
                                                {
                                                    //SqlCommand com2 = new SqlCommand(@"insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                    //                                   "values('" + strwfpno + "',1,NULL,'bms/WFP','" + strwfpnofile.Replace("'", "''").ToString() + "',1,'" + prep_userid + "," + prep_dephead + ",19,59," + recomapproval2 + ",344880," + sign_eid_gov + "',getdate()," + prep_userid + ",'','" + str_random + "',1,11,11) ", con);
                                                    //con.Open();
                                                    //data2 = Convert.ToString(com2.ExecuteScalar());


                                                    using (SqlCommand cmd = new SqlCommand("insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                                                       "values(@doc_name,@doc_type,@doc_attachement,@location,@doc_description,@doc_status_id,@doc_designated,@doc_datetime,@doc_eid,@doc_datetime_update,@doc_code,@doc_signatory_type,@doc_is,@doc_type_id) ", con))
                                                    {
                                                        cmd.Parameters.AddWithValue("@doc_name", fileName);
                                                        cmd.Parameters.AddWithValue("@doc_type", "0");
                                                        cmd.Parameters.AddWithValue("@location", "'bms/WFP'");
                                                        cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                                        cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                                        cmd.Parameters.AddWithValue("@doc_designated", prep_userid + "," + prep_dephead + ",303955,59," + recomapproval2 + ",344880," + sign_eid_gov);
                                                        cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                                        cmd.Parameters.AddWithValue("@doc_eid", prep_userid);
                                                        cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                                        cmd.Parameters.AddWithValue("@doc_code", str_random);
                                                        cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                                        cmd.Parameters.AddWithValue("@doc_is", "11");
                                                        cmd.Parameters.AddWithValue("@doc_type_id", "11");
                                                        cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                                                        con.Open();
                                                        cmd.ExecuteNonQuery();
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                if (officeid == 26 && fundid != 0) //dopm -add preparer countersign -TRUST FUND
                                                {
                                                    //SqlCommand com2 = new SqlCommand(@"insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                    //                                   //"values('"+ strwfpno + "',1,NULL,'bms/WFP','WFP - " + yearof + "',1,'5582,5582,5582,5582,5582',getdate(),5582,'','" + str_random + "',1,11,11) ", con);
                                                    //                                   "values('" + strwfpno + "',1,NULL,'bms/WFP','" + strwfpnofile.Replace("'", "''").ToString() + "',1,'56," + prep_userid + "," + prep_dephead + ",59," + recomapproval2 + ",344880,"+ sign_eid_gov +"',getdate()," + prep_userid + ",'','" + str_random + "',1,11,11) ", con);
                                                    //con.Open();
                                                    //data2 = Convert.ToString(com2.ExecuteScalar());

                                                    using (SqlCommand cmd = new SqlCommand("insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                                                    "values(@doc_name,@doc_type,@doc_attachement,@location,@doc_description,@doc_status_id,@doc_designated,@doc_datetime,@doc_eid,@doc_datetime_update,@doc_code,@doc_signatory_type,@doc_is,@doc_type_id) ", con))
                                                    {
                                                        cmd.Parameters.AddWithValue("@doc_name", fileName);
                                                        cmd.Parameters.AddWithValue("@doc_type", "0");
                                                        cmd.Parameters.AddWithValue("@location", "'bms/WFP'");
                                                        cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                                        cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                                        cmd.Parameters.AddWithValue("@doc_designated", prep_userid + "," + prep_dephead + ",59," + recomapproval2 + ",344880," + sign_eid_gov);
                                                        cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                                        cmd.Parameters.AddWithValue("@doc_eid", prep_userid);
                                                        cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                                        cmd.Parameters.AddWithValue("@doc_code", str_random);
                                                        cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                                        cmd.Parameters.AddWithValue("@doc_is", "11");
                                                        cmd.Parameters.AddWithValue("@doc_type_id", "11");
                                                        cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                                                        con.Open();
                                                        cmd.ExecuteNonQuery();
                                                    }

                                                }
                                                else if (officeid == 49 || officeid == 57 || officeid == 14) //vice gov, sp, sec
                                                {
                                                    using (SqlCommand cmd = new SqlCommand("insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                                                    "values(@doc_name,@doc_type,@doc_attachement,@location,@doc_description,@doc_status_id,@doc_designated,@doc_datetime,@doc_eid,@doc_datetime_update,@doc_code,@doc_signatory_type,@doc_is,@doc_type_id) ", con))
                                                    {
                                                        cmd.Parameters.AddWithValue("@doc_name", fileName);
                                                        cmd.Parameters.AddWithValue("@doc_type", "0");
                                                        cmd.Parameters.AddWithValue("@location", "'bms/WFP'");
                                                        cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                                        cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                                        cmd.Parameters.AddWithValue("@doc_designated", prep_userid + ",354982," + prep_dephead + ",59," + recomapproval2 + ",344880," + sign_eid_gov);
                                                        cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                                        cmd.Parameters.AddWithValue("@doc_eid", prep_userid);
                                                        cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                                        cmd.Parameters.AddWithValue("@doc_code", str_random);
                                                        cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                                        cmd.Parameters.AddWithValue("@doc_is", "11");
                                                        cmd.Parameters.AddWithValue("@doc_type_id", "11");
                                                        cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                                                        con.Open();
                                                        cmd.ExecuteNonQuery();
                                                    }

                                                }
                                                else
                                                {
                                                    //SqlCommand com2 = new SqlCommand(@"insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                    //                                   //"values('"+ strwfpno + "',1,NULL,'bms/WFP','WFP - " + yearof + "',1,'5582,5582,5582,5582,5582',getdate(),5582,'','" + str_random + "',1,11,11) ", con);
                                                    //                                   "values('" + strwfpno + "',1,NULL,'bms/WFP','" + strwfpnofile.Replace("'", "''").ToString() + "',1,'" + prep_userid + "," + prep_dephead + ",59," + recomapproval2 + ",344880,"+ sign_eid_gov +"',getdate()," + prep_userid + ",'','" + str_random + "',1,11,11) ", con);
                                                    //con.Open();
                                                    //data2 = Convert.ToString(com2.ExecuteScalar());

                                                    using (SqlCommand cmd = new SqlCommand("insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                                                      "values(@doc_name,@doc_type,@doc_attachement,@location,@doc_description,@doc_status_id,@doc_designated,@doc_datetime,@doc_eid,@doc_datetime_update,@doc_code,@doc_signatory_type,@doc_is,@doc_type_id) ", con))
                                                    {
                                                        cmd.Parameters.AddWithValue("@doc_name", fileName);
                                                        cmd.Parameters.AddWithValue("@doc_type", "0");
                                                        cmd.Parameters.AddWithValue("@location", "'bms/WFP'");
                                                        cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                                        cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                                        cmd.Parameters.AddWithValue("@doc_designated", prep_userid + "," + prep_dephead + ",59," + recomapproval2 + ",344880," + sign_eid_gov);
                                                        cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                                        cmd.Parameters.AddWithValue("@doc_eid", prep_userid);
                                                        cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                                        cmd.Parameters.AddWithValue("@doc_code", str_random);
                                                        cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                                        cmd.Parameters.AddWithValue("@doc_is", "11");
                                                        cmd.Parameters.AddWithValue("@doc_type_id", "11");
                                                        cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                                                        con.Open();
                                                        cmd.ExecuteNonQuery();
                                                    }

                                                }
                                            }
                                            //}
                                            //else
                                            //{
                                            //    SqlCommand com2 = new SqlCommand(@"insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                            //                                       //"values('"+ strwfpno + "',1,NULL,'bms/WFP','WFP - " + yearof + "',1,'5582,5582,5582,5582,5582',getdate(),5582,'','" + str_random + "',1,11,11) ", con);
                                            //                                       "values('" + strwfpno + "',1,NULL,'bms/WFP_TF','" + strwfpnofile.Replace("'", "''").ToString() + "',1,'" + prep_userid + "," + prep_dephead + ",59,2635,344880,"+ sign_eid_gov +"',getdate()," + prep_userid + ",'','" + str_random + "',1,11,11) ", con);
                                            //    con.Open();
                                            //    data2 = Convert.ToString(com2.ExecuteScalar());
                                            //}
                                        }

                                    }
                                }
                                con.Close();
                                var opndocid = 0;
                                var doc_sign = "";
                                var strconcat = "";
                                var email = "";
                                var fullname = "";

                                DataTable doc_attach = new DataTable();
                                string _sqldoc = "Select [doc_id],doc_designated from [bacpdfsign].[dbo].document_attach where doc_name='" + strwfpno + "'";
                                doc_attach = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqldoc).Tables[0];
                                opndocid = Convert.ToInt32(doc_attach.Rows[0][0]);
                                doc_sign = doc_attach.Rows[0][1].ToString();
                                var doc_sign_split = doc_sign.Split(',');

                                if (fundid == 0) //gfsdf
                                {
                                    var data3 = "";
                                    con.Close();

                                    if ((prep_officeid == 1 && prep_dephead != 19) || officeid == 1) //for national office or pgas
                                    {
                                        SqlCommand com3 = new SqlCommand(@"exec [bacpdfsign].[dbo].sp_bms_wfpsignatory '" + strwfpno_only + "'," + opndocid + ",'" + str_random + "'," + programid + "," + accountid + "," + yearof + "," + sig_usertype + "", con);
                                        con.Open();
                                        data3 = Convert.ToString(com3.ExecuteScalar());
                                    }
                                    else
                                    {
                                        SqlCommand com3 = new SqlCommand(@"exec [bacpdfsign].[dbo].sp_bms_wfpsignatory '" + strwfpno_only + "'," + opndocid + ",'" + str_random + "'," + programid + "," + accountid + "," + yearof + ",0", con);
                                        con.Open();
                                        data3 = Convert.ToString(com3.ExecuteScalar());
                                    }
                                    //for (int i = 0; i < doc_sign_split.Length; i++)
                                    //{
                                    //    var data3 = "";
                                    //    con.Close();
                                    //    if (mode_trans == 1) //current
                                    //    {
                                    //        if (officeid == 32 && i == 0)
                                    //        {
                                    //            SqlCommand com3 = new SqlCommand(@"insert into [bacpdfsign].[dbo].[document_signatories] ([doc_id],[sig_code],[sig_eid],[sig_status],[sig_order],[sig_remarks],[sig_query_signed],[sig_query_return],[sig_user_type],sig_level) " +
                                    //                                                   "values(" + opndocid + ",'" + str_random + "',2764,0," + (i + 1) + ",'','','exec ifmis.dbo.[sp_BMS_WFPAccountreturn_dgsign] " + programid + "," + accountid + "," + yearof + ",2764," + strwfpNOPDF.Replace("'", "''").ToString() + "',0,1) ", con);
                                    //            //SqlCommand com3 = new SqlCommand(@"exec sp_bms_wfpsignatory " + opndocid + ",'" + str_random + "',2764,0," + (i + 1) + ",'','','exec ifmis.dbo.[sp_BMS_WFPAccountreturn_dgsign] " + programid + "," + accountid + "," + yearof + ",2764," + strwfpNOPDF.Replace("'", "''").ToString() + "',0,1) ", con);
                                    //            con.Open();
                                    //            data3 = Convert.ToString(com3.ExecuteScalar());
                                    //        }
                                    //        else
                                    //        {
                                    //            //sig_usertype

                                    //            if (i == 0 || (prep_officeid == 1 && prep_dephead != 19 && i <= 1)) //for national office or pgas
                                    //            {
                                    //                SqlCommand com3 = new SqlCommand(@"insert into [bacpdfsign].[dbo].[document_signatories] ([doc_id],[sig_code],[sig_eid],[sig_status],[sig_order],[sig_remarks],[sig_query_signed],[sig_query_return],[sig_user_type]) " +
                                    //                                                   "values(" + opndocid + ",'" + str_random + "'," + doc_sign_split[i] + ",0," + (i + 1) + ",'','','exec ifmis.dbo.[sp_BMS_WFPAccountreturn_dgsign] " + programid + "," + accountid + "," + yearof + "," + doc_sign_split[i] + "," + strwfpNOPDF.Replace("'", "''").ToString() + "'," + sig_usertype + ") ", con);
                                    //                con.Open();
                                    //                data3 = Convert.ToString(com3.ExecuteScalar());
                                    //            }
                                    //            else
                                    //            {
                                    //                SqlCommand com3 = new SqlCommand(@"insert into [bacpdfsign].[dbo].[document_signatories] ([doc_id],[sig_code],[sig_eid],[sig_status],[sig_order],[sig_remarks],[sig_query_signed],[sig_query_return],[sig_user_type]) " +
                                    //                                                   "values(" + opndocid + ",'" + str_random + "'," + doc_sign_split[i] + ",0," + (i + 1) + ",'','','exec ifmis.dbo.[sp_BMS_WFPAccountreturn_dgsign] " + programid + "," + accountid + "," + yearof + "," + doc_sign_split[i] + "," + strwfpNOPDF.Replace("'", "''").ToString() + "',0) ", con);
                                    //                con.Open();
                                    //                data3 = Convert.ToString(com3.ExecuteScalar());
                                    //            }
                                    //        }
                                    //    }
                                    //    else //excess
                                    //    {
                                    //        if (officeid == 32 && i == 1)
                                    //        {
                                    //            SqlCommand com3 = new SqlCommand(@"insert into [bacpdfsign].[dbo].[document_signatories] ([doc_id],[sig_code],[sig_eid],[sig_status],[sig_order],[sig_remarks],[sig_query_signed],[sig_query_return],[sig_user_type],sig_level) " +
                                    //                                                   "values(" + opndocid + ",'" + str_random + "',2764,0," + (i + 1) + ",'','','exec ifmis.dbo.[sp_BMS_WFPAccountreturn_dgsign_excess] " + officeid + "," + accountid + "," + yearof + ",2764," + strwfpNOPDF.Replace("'", "''").ToString() + "',0,1) ", con);
                                    //            con.Open();
                                    //            data3 = Convert.ToString(com3.ExecuteScalar());
                                    //        }
                                    //        else
                                    //        {
                                    //            //sig_usertype
                                    //            if (i == 0 || (prep_officeid == 1 && prep_dephead != 19 && i <= 1)) //for national office or pgas
                                    //            {
                                    //                SqlCommand com3 = new SqlCommand(@"insert into [bacpdfsign].[dbo].[document_signatories] ([doc_id],[sig_code],[sig_eid],[sig_status],[sig_order],[sig_remarks],[sig_query_signed],[sig_query_return],[sig_user_type]) " +
                                    //                                                   "values(" + opndocid + ",'" + str_random + "'," + doc_sign_split[i] + ",0," + (i + 1) + ",'','','exec ifmis.dbo.[sp_BMS_WFPAccountreturn_dgsign_excess] " + officeid + "," + accountid + "," + yearof + "," + doc_sign_split[i] + "," + strwfpNOPDF.Replace("'", "''").ToString() + "'," + sig_usertype + ") ", con);
                                    //                con.Open();
                                    //                data3 = Convert.ToString(com3.ExecuteScalar());
                                    //            }
                                    //            else
                                    //            {
                                    //                SqlCommand com3 = new SqlCommand(@"insert into [bacpdfsign].[dbo].[document_signatories] ([doc_id],[sig_code],[sig_eid],[sig_status],[sig_order],[sig_remarks],[sig_query_signed],[sig_query_return],[sig_user_type]) " +
                                    //                                                   "values(" + opndocid + ",'" + str_random + "'," + doc_sign_split[i] + ",0," + (i + 1) + ",'','','exec ifmis.dbo.[sp_BMS_WFPAccountreturn_dgsign_excess] " + officeid + "," + accountid + "," + yearof + "," + doc_sign_split[i] + "," + strwfpNOPDF.Replace("'", "''").ToString() + "',0) ", con);
                                    //                con.Open();
                                    //                data3 = Convert.ToString(com3.ExecuteScalar());
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                }
                                else //tf
                                {
                                    for (int i = 0; i < doc_sign_split.Length; i++)
                                    {
                                        var data3 = "";
                                        con.Close();
                                        if (officeid == 32 && i == 1)
                                        {
                                            SqlCommand com3 = new SqlCommand(@"insert into [bacpdfsign].[dbo].[document_signatories] ([doc_id],[sig_code],[sig_eid],[sig_status],[sig_order],[sig_remarks],[sig_query_signed],[sig_query_return],[sig_user_type],sig_level) " +
                                                                                   "values(" + opndocid + ",'" + str_random + "',2764,0," + (i + 1) + ",'','','exec ifmis.dbo.[sp_BMS_WFPAccountreturn_dgsign_tf] " + officeid + "," + accountid + "," + yearof + ",2764," + strwfpNOPDF.Replace("'", "''").ToString() + "',0,1) ", con);
                                            con.Open();
                                            data3 = Convert.ToString(com3.ExecuteScalar());
                                        }
                                        else
                                        {
                                            //sig_usertype
                                            if (i == 0) //for national office or pgas
                                            {
                                                SqlCommand com3 = new SqlCommand(@"insert into [bacpdfsign].[dbo].[document_signatories] ([doc_id],[sig_code],[sig_eid],[sig_status],[sig_order],[sig_remarks],[sig_query_signed],[sig_query_return],[sig_user_type]) " +
                                                                                   "values(" + opndocid + ",'" + str_random + "'," + doc_sign_split[i] + ",0," + (i + 1) + ",'','','exec ifmis.dbo.[sp_BMS_WFPAccountreturn_dgsign_tf] " + officeid + "," + accountid + "," + yearof + "," + doc_sign_split[i] + "," + strwfpNOPDF.Replace("'", "''").ToString() + "'," + sig_usertype + ") ", con);
                                                con.Open();
                                                data3 = Convert.ToString(com3.ExecuteScalar());
                                            }
                                            else
                                            {
                                                SqlCommand com3 = new SqlCommand(@"insert into [bacpdfsign].[dbo].[document_signatories] ([doc_id],[sig_code],[sig_eid],[sig_status],[sig_order],[sig_remarks],[sig_query_signed],[sig_query_return],[sig_user_type]) " +
                                                                                   "values(" + opndocid + ",'" + str_random + "'," + doc_sign_split[i] + ",0," + (i + 1) + ",'','','exec ifmis.dbo.[sp_BMS_WFPAccountreturn_dgsign_tf] " + officeid + "," + accountid + "," + yearof + "," + doc_sign_split[i] + "," + strwfpNOPDF.Replace("'", "''").ToString() + "',0) ", con);
                                                con.Open();
                                                data3 = Convert.ToString(com3.ExecuteScalar());
                                            }
                                        }
                                    }
                                }
                                con.Close();
                                SqlCommand opnstrconcat = new SqlCommand(@"Select concat(" + opndocid + ",',','" + str_random + "',',','" + strwfpno + "',',',0,',,'," + prep_userid + ",',',0)", con);
                                con.Open();
                                strconcat = Convert.ToString(opnstrconcat.ExecuteScalar());

                                var strencryted = Rijndael.Encrypt(strconcat);

                                string pageurl = "https://pgas.ph/dgsign/blank/jklsdhfikujdfhfgiuodfhf?data=" + Server.UrlEncode(strencryted) + "";

                                DataTable doc_signature = new DataTable();
                                string _docsign = "Select upper([username]) username,b.Firstname + ' '+ left(b.mi,1) + '. ' + b.Lastname + ' ' + isnull(b.Suffix,'') as employee from [pmis].[dbo].[eportalUser] as a inner join pmis.dbo.employee as b on b.eid=a.eid where a.[eid]=" + prep_userid + "";
                                doc_signature = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _docsign).Tables[0];
                                if (doc_signature.Rows.Count > 0)
                                {
                                    email = doc_signature.Rows[0][0].ToString();
                                    fullname = doc_signature.Rows[0][1].ToString();
                                    var fullname_split = fullname.Split('@');

                                    sendViaEmailOTP(email, fullname_split[0], pageurl, 0);
                                }
                                //Response.Write(String.Format("window.open('{0}','_blank')", ResolveUrl(pageurl)));

                                //DG Signature --- end --
                            }
                            //com.ExecuteScalar().ToString();
                        }
                        else
                        {
                            var wfperr = "";
                            SqlCommand wfperrep = new SqlCommand(@"insert into [IFMIS].[dbo].[tbl_T_BMSWFP_Errorlog] ([officeid],[accountid],[particular],[userid]) " +
                                                                                   "values(" + officeid + "," + accountid + ",'" + result.HasErrors + "'," + Account.UserInfo.eid + ") ", con);
                            con.Open();
                            wfperr = Convert.ToString(wfperrep.ExecuteScalar());
                        }
                    }
                    con.Close();
                    var chkdocattach = 0;
                    SqlCommand opndocattach = new SqlCommand(@"Select  count([doc_name]) as docattach FROM [bacpdfsign].[dbo].[document_attach] where doc_NAME = '" + strwfpno + "'", con);
                    con.Open();
                    chkdocattach = Convert.ToInt32(opndocattach.ExecuteScalar());
                    if (chkdocattach == 0)
                    {
                        var reviewret = "";
                        con.Close();
                        SqlCommand revret = new SqlCommand(@"exec ifmis.dbo.sp_BMS_WFP_returnreview '" + strwfpno + "'," + yearof + "", con);
                        con.Open();
                        reviewret = Convert.ToString(revret.ExecuteScalar());
                        return retstr = "ErrorNAS";
                    }
                    else
                    {
                        return retstr;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        //includeLBP1:includeLBP1,pagnoi$("#pagnoid").val(),includeLBP2_SP:includeLBP2_SP,eiuserid,includezero:includezero,lbp2export:lbp2export,reloadlbp2:reloadlbp2,sectoral:sectoral,includeCOE:includeCOE,eco:eco 
        public string dgsignreport(int YearOf = 0, int OfficeID = 0, int ReportTypeID = 0, int docuid = 0, int reloadlbp4 = 0, int includeLBP1 = 0, int pagnoid = 0, int includeLBP2_SP = 0, long eid = 0, int includezero = 0, int lbp2export = 0, int reloadlbp2 = 0, int sectoral = 0, string includeCOE = "", int eco = 0, string purpose = "", string note = "", int sort_ = 0)
        {
            DataTable _wfno = new DataTable();
            string strwfpno = "";
            string strwfpnofile = "";
            string _sqlwfp = "";
            string pageurl = "";
            int officeheadid = 0;
            var retstr = "";
            var opndocid = 0;
            var strconcat = "";
            var doctypeid = 0;
            int proposaluserid = 0;
            var str_random = RandomString(10);
            string strarono = "";
            string strfileonly = "";
            var maf_userid = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (docuid != 14 && docuid != 12 && docuid != 13)
                {
                    if (docuid != 6 && docuid != 7 && docuid != 9) //LBP FORM 2 / 4
                    {
                        ReportTypeID = ReportTypeID == 4 ? 3 : ReportTypeID;
                        SqlCommand com = new SqlCommand("dbo.[sp_BMS_LBPReport]", con);
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                        com.Parameters.Add(new SqlParameter("@yearof", YearOf));
                        com.Parameters.Add(new SqlParameter("@officeid", OfficeID));
                        com.Parameters.Add(new SqlParameter("@typeid", ReportTypeID));
                        con.Open();
                        retstr = com.ExecuteScalar().ToString();
                    }
                    else if (docuid == 7) //LBP FORM 7
                    {
                        SqlCommand com = new SqlCommand("dbo.[sp_BMS_LBPReport]", con);
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                        com.Parameters.Add(new SqlParameter("@yearof", YearOf));
                        com.Parameters.Add(new SqlParameter("@officeid", "0"));
                        com.Parameters.Add(new SqlParameter("@typeid", docuid));
                        con.Open();
                        retstr = com.ExecuteScalar().ToString();
                    }
                    else if (docuid == 9) //Supplemental No. 3
                    {
                        SqlCommand com = new SqlCommand("dbo.[sp_BMS_LBPReport]", con);
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                        com.Parameters.Add(new SqlParameter("@yearof", YearOf));
                        com.Parameters.Add(new SqlParameter("@officeid", OfficeID));
                        com.Parameters.Add(new SqlParameter("@typeid", docuid));
                        con.Open();
                        retstr = com.ExecuteScalar().ToString();
                    }
                    //else if (docuid == 11 || docuid == 12) //ARO / SAAO
                    //{
                    //    SqlCommand com = new SqlCommand("dbo.[sp_BMS_LBPReport]", con);
                    //    com.CommandType = System.Data.CommandType.StoredProcedure;
                    //    com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                    //    com.Parameters.Add(new SqlParameter("@yearof", YearOf));
                    //    com.Parameters.Add(new SqlParameter("@officeid", OfficeID));
                    //    com.Parameters.Add(new SqlParameter("@typeid", docuid));
                    //    con.Open();
                    //    retstr = com.ExecuteScalar().ToString();
                    //}

                    else //LBP FORM 1
                    {
                        SqlCommand com = new SqlCommand("dbo.[sp_BMS_LBPReport]", con);
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                        com.Parameters.Add(new SqlParameter("@yearof", YearOf));
                        com.Parameters.Add(new SqlParameter("@officeid", OfficeID));
                        com.Parameters.Add(new SqlParameter("@typeid", docuid));
                        con.Open();
                        retstr = com.ExecuteScalar().ToString();
                    }
                }
                var deviceInfo = new System.Collections.Hashtable();
                InstanceReportSource rs = new Telerik.Reporting.InstanceReportSource();
                var reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();
                var typeReportSource = new Telerik.Reporting.TypeReportSource();

                //rs.ReportDocument = new WFPNew(yearof, officeid, 0, 0, officename, "", "", repHistory, prepby, printstatus, projectaip, "", accountid, accountname, municipal, barangay, ooeclassname, pgas_loc, 1, programid, activityid, fundid, subppaid);
                if ((ReportTypeID == 1 || ReportTypeID == 2 || ReportTypeID == 3 || ReportTypeID == 4) && docuid != 6 && docuid != 7 && docuid != 9 && docuid != 14 && docuid != 12 && docuid != 13)
                {
                    doctypeid = 68;
                    if (ReportTypeID == 1 || ReportTypeID == 2)
                    {
                        rs.ReportDocument = new LBP2New(OfficeID, YearOf, ReportTypeID, Account.UserInfo.eid, 1);
                    }
                    else //consolidated
                    {
                        ReportTypeID = ReportTypeID == 4 ? 3 : ReportTypeID;
                        rs.ReportDocument = new LBP2NewConsolidated(OfficeID, YearOf, ReportTypeID, includeLBP1, pagnoid, eid, includezero, reloadlbp2, sectoral, eco);
                    }
                }
                else if (docuid == 7)
                {
                    rs.ReportDocument = new LBPF7(YearOf);
                    doctypeid = 1;
                }
                else if (docuid == 9)
                {
                    rs.ReportDocument = new SB3(OfficeID, YearOf, eco);
                    doctypeid = 1;
                }
                else if (ReportTypeID == 5 && docuid != 6) //lbp form 4
                {
                    doctypeid = 69;
                    rs.ReportDocument = new LBP4New(OfficeID, YearOf, 0, reloadlbp4);
                }
                else if (docuid == 6) //lbp form 1
                {
                    doctypeid = 70;
                    rs.ReportDocument = new LBPForm1New(YearOf, ReportTypeID, Account.UserInfo.eid);
                }
                else if (docuid == 14) //ARO
                {
                    doctypeid = 71;
                    rs.ReportDocument = new ARO(OfficeID, ReportTypeID, YearOf, reloadlbp4, includeLBP1, sort_, note, includeLBP2_SP, purpose, lbp2export, 1, 0, "", 0, eco, includeCOE);
                }
                else if (docuid == 12) //SAAO
                {
                    doctypeid = 72;
                    rs.ReportDocument = new SAAO(OfficeID, 1, reloadlbp4, YearOf, 0, 1, 0, 0);
                }
                else if (docuid == 13) //MAF-realign ; sort_ = mode
                {
                    doctypeid = 73;
                    rs.ReportDocument = new MAF(0, 0, YearOf, note, OfficeID, sort_, 0, 1, 1);
                }
                Telerik.Reporting.Processing.RenderingResult result = reportProcessor.RenderReport("PDF", rs, null);
                string fileName = "";
                //NetworkCredential credentials = new NetworkCredential(@"pgas.ph\reykirby.lumanta", "kirbygwapo123");
                NetworkCredential credentials = new NetworkCredential(@"pgas.ph\rhayan.gubalane", "DomainUser1");
                //var credential = new NetworkCredential(username, password);
                var server_path = networkPath + "\\" + Account.UserInfo.eid;
                ReportTypeID = docuid == 6 ? 6 : ReportTypeID;

                if (docuid != 14 && docuid != 12 && docuid != 13) // LBP FORms 
                {
                    _sqlwfp = "select rptno,upper([rptno] + ' (' + b.OfficeAbbrivation + ')') as Office from [tbl_T_BMSLBPReport_xml] as a  left join ifmis.dbo.tbl_r_bmsoffices as b on b.officeid = a.officeid where [yearof]=" + YearOf + " and a.[officeid]=" + OfficeID + " and typeid =" + ReportTypeID + " and isnull([qrcode],'')= '" + GlobalFunctions.QR_globalstr + "'";

                    _wfno = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlwfp).Tables[0];
                    strwfpno = _wfno.Rows[0][0].ToString() + ".pdf";
                    strfileonly = _wfno.Rows[0][0].ToString();
                    strwfpnofile = _wfno.Rows[0][1].ToString();
                }
                else if (docuid == 12) //SAAO
                {
                    _sqlwfp = "select controlno,upper(controlno + ' (' + b.OfficeAbbrivation + ')') as Office from ifmis.dbo.tbl_T_BMSReportXML as a  left join ifmis.dbo.tbl_r_bmsoffices as b on b.officeid = a.office where  [yearof]=" + YearOf + " and a.[office]=" + OfficeID + " and isnull([qrcode],'')= '" + GlobalFunctions.QR_globalstr + "'";

                    _wfno = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlwfp).Tables[0];
                    strwfpno = _wfno.Rows[0][0].ToString() + ".pdf";
                    strfileonly = _wfno.Rows[0][0].ToString();
                    strwfpnofile = _wfno.Rows[0][1].ToString();
                }
                else if (docuid == 13) //MAF - realign
                {
                    _sqlwfp = "select distinct mafno,upper(mafno + ' (' + b.OfficeAbbrivation + ')') as Office from ifmis.dbo.[tbl_T_BMSMAF_xml] as a left join ifmis.dbo.tbl_r_bmsoffices as b on b.officeid = a.officeid where a.[officeid]=" + OfficeID + " and isnull([qrcode],'')= '" + GlobalFunctions.QR_globalstr + "'";

                    _wfno = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlwfp).Tables[0];
                    strwfpno = _wfno.Rows[0][0].ToString() + ".pdf";
                    strfileonly = _wfno.Rows[0][0].ToString();
                    strwfpnofile = _wfno.Rows[0][1].ToString();
                }
                else // ARO
                {
                    _sqlwfp = "select arono,upper([arono] + ' (' + b.OfficeAbbrivation + ')') as Office from [tbl_T_BMSARO_xml] as a  left join ifmis.dbo.tbl_r_bmsoffices as b on b.officeid = a.office where qrcode= '" + GlobalFunctions.QR_globalstr + "'  and actioncode=1";

                    _wfno = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlwfp).Tables[0];
                    strwfpno = _wfno.Rows[0][0].ToString() + ".pdf";
                    strwfpnofile = _wfno.Rows[0][1].ToString();
                    strarono = _wfno.Rows[0][0].ToString();
                    strfileonly = _wfno.Rows[0][0].ToString();
                }
                var data2 = "";
                if (!result.HasErrors)
                {
                    fileName = strwfpno;
                    //fileName = "wfp-23-1981.pdf";
                    //string path = "C:\\Users\\admin\\Documents\\Public\\WFP";// System.IO.Path.GetTempPath(); //LOCAL Connection
                    string path = networkPath_report;// System.IO.Path.GetTempPath();
                    string filePath = System.IO.Path.Combine(path, fileName);

                    //using (new ConnectToSharedFolder(networkPath_report, credentials))

                    //use for nas server
                    //using (new conection_shared_data(networkPath_report, credentials))
                    //{

                    //string networkPath2 = @"\\192.168.2.210\pgas_attachment\bms\Report\" + fileName + "";
                    string networkPath2 = @"r:\Web Application\iFMIS-BMS_publish\" + fileName + "";
                    if (System.IO.File.Exists(networkPath2))
                    {
                        System.IO.File.Delete(networkPath2);

                        //System.IO.File.CreateDirectory(networkPath2);
                        var fileList = Directory.GetDirectories(networkPath_report);
                        using (System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                        {
                            fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
                        }
                    }
                    else
                    {
                        var fileList = Directory.GetDirectories(networkPath_report);
                        using (System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                        {
                            fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
                        }
                    }
                    //PDF to binary file - START
                    filePathbin = @"r:\Web Application\iFMIS-BMS_publish\" + fileName + "";
                    fileData = System.IO.File.ReadAllBytes(filePathbin); // Convert PDF to binary
                                                                         //using (SqlConnection conn = new SqlConnection(Common.MyConn()))
                                                                         //using (SqlCommand cmd = new SqlCommand("insert into [ifmis].[dbo].[tbl_T_BMSWFP_PDFtoBinary] ([filename],[filedata],[actioncode],[tyear]) VALUES (@FileName, @FileData,@actioncode,@tyear)", conn))
                                                                         //{
                                                                         //    cmd.Parameters.AddWithValue("@FileName", fileName);
                                                                         //    cmd.Parameters.AddWithValue("@actioncode", "1");
                                                                         //    cmd.Parameters.AddWithValue("@tyear", YearOf);
                                                                         //    cmd.Parameters.Add("@FileData", SqlDbType.VarBinary).Value = fileData;

                    //    conn.Open();
                    //    cmd.ExecuteNonQuery();
                    //}
                    //PDF to binary file - END

                    //delete the PDF file
                    if (System.IO.File.Exists(networkPath2))
                    {
                        System.IO.File.Delete(filePath);
                        Console.WriteLine("File deleted successfully.");
                    }
                    //pdf to binary file - END

                    //}
                    con.Close();
                    SqlCommand comoffice = new SqlCommand(@"select OfficeHeadid FROM pmis.dbo.OfficeDescription WHERE OfficeID =
                                                      (select b.PMISOfficeID from ifmis.dbo.tbl_R_BMSOffices as b where b.OfficeID = " + OfficeID + ") ", con);
                    con.Open();
                    officeheadid = Convert.ToInt32(comoffice.ExecuteScalar());

                    if (docuid != 7 && docuid != 9)
                    {
                        con.Close();
                        SqlCommand propstr = new SqlCommand(@"select dbo.[fn_BMS_ProposalUser] (" + OfficeID + "," + YearOf + ") ", con);
                        con.Open();
                        proposaluserid = Convert.ToInt32(propstr.ExecuteScalar());
                    }
                    //select dbo.[fn_BMS_ProposalUser](4,2025)

                    var sign_eid_gov = 0;

                    DataTable wfpsig_id = new DataTable();
                    string _sqlsign = "SELECT distinct eid FROM  [IFMIS].[dbo].[tbl_R_BMS_WFPsignatory] where orderno=8 and yearof=" + YearOf + "";
                    wfpsig_id = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlsign).Tables[0];
                    if (wfpsig_id.Rows.Count > 0)
                    {
                        sign_eid_gov = Convert.ToInt32(wfpsig_id.Rows[0][0]);
                    }


                    if ((ReportTypeID == 1 || ReportTypeID == 2 || ReportTypeID == 3 || ReportTypeID == 4) && docuid != 6 && docuid != 7 && docuid != 9 && docuid != 14 && docuid != 12 && docuid != 13) // lbp form 2
                    {
                        con.Close();
                        string repsig = "";
                        DataTable repsig_id = new DataTable();
                        string _sqlsignatory = "SELECT [signatory] from [IFMIS].[dbo].[tbl_R_BMS_ReportSignatory] where [sigid]=2";
                        repsig_id = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlsignatory).Tables[0];
                        if (repsig_id.Rows.Count > 0)
                        {
                            repsig = Convert.ToString(repsig_id.Rows[0][0]);
                        }
                        using (SqlCommand cmd = new SqlCommand("insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                                                      "values(@doc_name,@doc_type,@doc_attachement,@location,@doc_description,@doc_status_id,@doc_designated,@doc_datetime,@doc_eid,@doc_datetime_update,@doc_code,@doc_signatory_type,@doc_is,@doc_type_id) ", con))
                        {
                            //consolidation
                            if (OfficeID == 14 || OfficeID == 49 || OfficeID == 57)
                            {
                                cmd.Parameters.AddWithValue("@doc_name", fileName);
                                cmd.Parameters.AddWithValue("@doc_type", "0");
                                cmd.Parameters.AddWithValue("@location", "'bms/Report'");
                                cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                cmd.Parameters.AddWithValue("@doc_designated", "354982," + officeheadid + "," + repsig);
                                cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                cmd.Parameters.AddWithValue("@doc_eid", "354982");
                                cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                cmd.Parameters.AddWithValue("@doc_code", str_random);
                                cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                cmd.Parameters.AddWithValue("@doc_is", "11");
                                cmd.Parameters.AddWithValue("@doc_type_id", doctypeid);
                                cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@doc_name", fileName);
                                cmd.Parameters.AddWithValue("@doc_type", "0");
                                cmd.Parameters.AddWithValue("@location", "'bms/Report'");
                                cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                cmd.Parameters.AddWithValue("@doc_designated", officeheadid + "," + repsig);
                                cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                cmd.Parameters.AddWithValue("@doc_eid", officeheadid);
                                cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                cmd.Parameters.AddWithValue("@doc_code", str_random);
                                cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                cmd.Parameters.AddWithValue("@doc_is", "11");
                                cmd.Parameters.AddWithValue("@doc_type_id", doctypeid);
                                cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;
                            }
                            //preparation
                            //cmd.Parameters.AddWithValue("@doc_name", fileName);
                            //cmd.Parameters.AddWithValue("@doc_type", "0");
                            //cmd.Parameters.AddWithValue("@location", "'bms/Report'");
                            //cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                            //cmd.Parameters.AddWithValue("@doc_status_id", "1");
                            //cmd.Parameters.AddWithValue("@doc_designated", officeheadid);
                            //cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                            //cmd.Parameters.AddWithValue("@doc_eid", officeheadid);
                            //cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                            //cmd.Parameters.AddWithValue("@doc_code", str_random);
                            //cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                            //cmd.Parameters.AddWithValue("@doc_is", "11");
                            //cmd.Parameters.AddWithValue("@doc_type_id", doctypeid);
                            //cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else if (docuid == 7 || docuid == 9) // lbp form 7 or supplemental budget 3
                    {
                        con.Close();
                        //SqlCommand com2 = new SqlCommand(@"insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                        //                                                       "values('" + strwfpno + "',1,NULL,'bms/Report','" + strwfpnofile.Replace("'", "''").ToString() + "',1,'2635," + sign_eid_gov + "',getdate()," + officeheadid + ",'','" + str_random + "',1,11," + doctypeid + ") ", con);
                        //con.Open();
                        //data2 = Convert.ToString(com2.ExecuteScalar());
                        if (docuid == 9) //supplemental budget 3
                        {
                            officeheadid = 2635; //pbo pgdh
                        }
                        using (SqlCommand cmd = new SqlCommand("insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                                                     "values(@doc_name,@doc_type,@doc_attachement,@location,@doc_description,@doc_status_id,@doc_designated,@doc_datetime,@doc_eid,@doc_datetime_update,@doc_code,@doc_signatory_type,@doc_is,@doc_type_id) ", con))
                        {
                            cmd.Parameters.AddWithValue("@doc_name", fileName);
                            cmd.Parameters.AddWithValue("@doc_type", "0");
                            cmd.Parameters.AddWithValue("@location", "'bms/Report'");
                            cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                            cmd.Parameters.AddWithValue("@doc_status_id", "1");
                            cmd.Parameters.AddWithValue("@doc_designated", officeheadid + ",344880," + sign_eid_gov);
                            cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                            cmd.Parameters.AddWithValue("@doc_eid", officeheadid);
                            cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                            cmd.Parameters.AddWithValue("@doc_code", str_random);
                            cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                            cmd.Parameters.AddWithValue("@doc_is", "11");
                            cmd.Parameters.AddWithValue("@doc_type_id", doctypeid);
                            cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                            con.Open();
                            cmd.ExecuteNonQuery();
                        }

                    }
                    else if (ReportTypeID == 5 && docuid != 6) // lbp form 4
                    {
                        con.Close();
                        string repsig = "";
                        DataTable repsig_id = new DataTable();
                        string _sqlsignatory = "SELECT [signatory] from [IFMIS].[dbo].[tbl_R_BMS_ReportSignatory] where [sigid]=1";
                        repsig_id = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlsignatory).Tables[0];
                        if (repsig_id.Rows.Count > 0)
                        {
                            repsig = Convert.ToString(repsig_id.Rows[0][0]);
                        }
                        using (SqlCommand cmd = new SqlCommand("insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                                                     "values(@doc_name,@doc_type,@doc_attachement,@location,@doc_description,@doc_status_id,@doc_designated,@doc_datetime,@doc_eid,@doc_datetime_update,@doc_code,@doc_signatory_type,@doc_is,@doc_type_id) ", con))
                        {
                            //consolidation
                            if (OfficeID == 14 || OfficeID == 49 || OfficeID == 57)
                            {
                                cmd.Parameters.AddWithValue("@doc_name", fileName);
                                cmd.Parameters.AddWithValue("@doc_type", "0");
                                cmd.Parameters.AddWithValue("@location", "'bms/Report'");
                                cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                cmd.Parameters.AddWithValue("@doc_designated", "354982," + officeheadid + "," + repsig);
                                cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                cmd.Parameters.AddWithValue("@doc_eid", "354982");
                                cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                cmd.Parameters.AddWithValue("@doc_code", str_random);
                                cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                cmd.Parameters.AddWithValue("@doc_is", "11");
                                cmd.Parameters.AddWithValue("@doc_type_id", doctypeid);
                                cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@doc_name", fileName);
                                cmd.Parameters.AddWithValue("@doc_type", "0");
                                cmd.Parameters.AddWithValue("@location", "'bms/Report'");
                                cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                cmd.Parameters.AddWithValue("@doc_designated", officeheadid + "," + repsig);
                                cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                cmd.Parameters.AddWithValue("@doc_eid", officeheadid);
                                cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                cmd.Parameters.AddWithValue("@doc_code", str_random);
                                cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                cmd.Parameters.AddWithValue("@doc_is", "11");
                                cmd.Parameters.AddWithValue("@doc_type_id", doctypeid);
                                cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                                //preparation
                                //cmd.Parameters.AddWithValue("@doc_name", fileName);
                                //cmd.Parameters.AddWithValue("@doc_type", "0");
                                //cmd.Parameters.AddWithValue("@location", "'bms/Report'");
                                //cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                //cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                //cmd.Parameters.AddWithValue("@doc_designated", officeheadid);
                                //cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                //cmd.Parameters.AddWithValue("@doc_eid", officeheadid);
                                //cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                //cmd.Parameters.AddWithValue("@doc_code", str_random);
                                //cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                //cmd.Parameters.AddWithValue("@doc_is", "11");
                                //cmd.Parameters.AddWithValue("@doc_type_id", doctypeid);
                                //cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    else if (docuid == 14) // ARO
                    {
                        var aroprepid = 0;
                        DataTable arosig_id = new DataTable();
                        string _sqlarosign = "select [usereid] from [IFMIS].[dbo].[tbl_T_BMSARO_xml] where [arono] = '" + strarono + "'";
                        arosig_id = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlarosign).Tables[0];
                        if (arosig_id.Rows.Count > 0)
                        {
                            aroprepid = Convert.ToInt32(arosig_id.Rows[0][0]);
                        }

                        // aroprepid = 2635;
                        if (OfficeID == 14 || OfficeID == 57 || OfficeID == 49)
                        {
                            sign_eid_gov = 7240;
                        }
                        if (aroprepid == 7043)
                        {
                            con.Close();
                            using (SqlCommand cmd = new SqlCommand("insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                                                         "values(@doc_name,@doc_type,@doc_attachement,@location,@doc_description,@doc_status_id,@doc_designated,@doc_datetime,@doc_eid,@doc_datetime_update,@doc_code,@doc_signatory_type,@doc_is,@doc_type_id) ", con))
                            {
                                cmd.Parameters.AddWithValue("@doc_name", fileName);
                                cmd.Parameters.AddWithValue("@doc_type", "0");
                                cmd.Parameters.AddWithValue("@location", "'bms/Report'");
                                cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                cmd.Parameters.AddWithValue("@doc_designated", aroprepid + ",314227,2635,344880," + sign_eid_gov);
                                cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                cmd.Parameters.AddWithValue("@doc_eid", aroprepid);
                                cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                cmd.Parameters.AddWithValue("@doc_code", str_random);
                                cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                cmd.Parameters.AddWithValue("@doc_is", "11");
                                cmd.Parameters.AddWithValue("@doc_type_id", "115");
                                cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            con.Close();
                            using (SqlCommand cmd = new SqlCommand("insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                                                         "values(@doc_name,@doc_type,@doc_attachement,@location,@doc_description,@doc_status_id,@doc_designated,@doc_datetime,@doc_eid,@doc_datetime_update,@doc_code,@doc_signatory_type,@doc_is,@doc_type_id) ", con))
                            {
                                cmd.Parameters.AddWithValue("@doc_name", fileName);
                                cmd.Parameters.AddWithValue("@doc_type", "0");
                                cmd.Parameters.AddWithValue("@location", "'bms/Report'");
                                cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                cmd.Parameters.AddWithValue("@doc_designated", aroprepid + ",314227,2635,344880," + sign_eid_gov);
                                cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                cmd.Parameters.AddWithValue("@doc_eid", aroprepid);
                                cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                cmd.Parameters.AddWithValue("@doc_code", str_random);
                                cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                cmd.Parameters.AddWithValue("@doc_is", "11");
                                cmd.Parameters.AddWithValue("@doc_type_id", "115");
                                cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    else if (docuid == 12) // saao
                    {
                        //officeheadid = 5580;
                        con.Close();
                        using (SqlCommand cmd = new SqlCommand("insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                                                  "values(@doc_name,@doc_type,@doc_attachement,@location,@doc_description,@doc_status_id,@doc_designated,@doc_datetime,@doc_eid,@doc_datetime_update,@doc_code,@doc_signatory_type,@doc_is,@doc_type_id) ", con))
                        {
                            cmd.Parameters.AddWithValue("@doc_name", fileName);
                            cmd.Parameters.AddWithValue("@doc_type", "0");
                            cmd.Parameters.AddWithValue("@location", "'bms/Report'");
                            cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                            cmd.Parameters.AddWithValue("@doc_status_id", "1");
                            cmd.Parameters.AddWithValue("@doc_designated", Account.UserInfo.eid + ",314227,2635");
                            cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                            cmd.Parameters.AddWithValue("@doc_eid", Account.UserInfo.eid);
                            cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                            cmd.Parameters.AddWithValue("@doc_code", str_random);
                            cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                            cmd.Parameters.AddWithValue("@doc_is", "11");
                            cmd.Parameters.AddWithValue("@doc_type_id", "119");
                            cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else if (docuid == 13) // maf
                    {
                        // officeheadid = 5580;
                        //NOTE : reloadlbp4 = end-user id

                        var mafprepid = 0;
                        DataTable mafsig_id = new DataTable();
                        string _sqlmafsign = "select distinct userid FROM [IFMIS].[dbo].[tbl_T_BMSMAF_xml]  where mafno + '.pdf'= '" + fileName + "'";
                        mafsig_id = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlmafsign).Tables[0];
                        if (mafsig_id.Rows.Count > 0)
                        {
                            mafprepid = Convert.ToInt32(mafsig_id.Rows[0][0]);
                        }

                        if (OfficeID == 49 || OfficeID == 57 || OfficeID == 14) //vice gov, sp, secretary
                        {
                            con.Close();
                            using (SqlCommand cmd = new SqlCommand("insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                                                      "values(@doc_name,@doc_type,@doc_attachement,@location,@doc_description,@doc_status_id,@doc_designated,@doc_datetime,@doc_eid,@doc_datetime_update,@doc_code,@doc_signatory_type,@doc_is,@doc_type_id) ", con))
                            {
                                cmd.Parameters.AddWithValue("@doc_name", fileName);
                                cmd.Parameters.AddWithValue("@doc_type", "0");
                                cmd.Parameters.AddWithValue("@location", "'bms/Report'");
                                cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                cmd.Parameters.AddWithValue("@doc_designated", reloadlbp4 + ",354982," + officeheadid + "," + mafprepid + ",2635,344880," + sign_eid_gov);
                                cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                cmd.Parameters.AddWithValue("@doc_eid", reloadlbp4);
                                cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                cmd.Parameters.AddWithValue("@doc_code", str_random);
                                cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                cmd.Parameters.AddWithValue("@doc_is", "11");
                                cmd.Parameters.AddWithValue("@doc_type_id", "118");
                                cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }
                        //365263
                        else if (OfficeID==63)
                        {
                            con.Close();
                            using (SqlCommand cmd = new SqlCommand("insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                                                      "values(@doc_name,@doc_type,@doc_attachement,@location,@doc_description,@doc_status_id,@doc_designated,@doc_datetime,@doc_eid,@doc_datetime_update,@doc_code,@doc_signatory_type,@doc_is,@doc_type_id) ", con))
                            {
                                cmd.Parameters.AddWithValue("@doc_name", fileName);
                                cmd.Parameters.AddWithValue("@doc_type", "0");
                                cmd.Parameters.AddWithValue("@location", "'bms/Report'");
                                cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                cmd.Parameters.AddWithValue("@doc_designated", reloadlbp4 + ",365263," + officeheadid + "," + mafprepid + ",2635,344880," + sign_eid_gov);
                                cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                cmd.Parameters.AddWithValue("@doc_eid", reloadlbp4);
                                cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                cmd.Parameters.AddWithValue("@doc_code", str_random);
                                cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                cmd.Parameters.AddWithValue("@doc_is", "11");
                                cmd.Parameters.AddWithValue("@doc_type_id", "118");
                                cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            con.Close();
                            using (SqlCommand cmd = new SqlCommand("insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                                                      "values(@doc_name,@doc_type,@doc_attachement,@location,@doc_description,@doc_status_id,@doc_designated,@doc_datetime,@doc_eid,@doc_datetime_update,@doc_code,@doc_signatory_type,@doc_is,@doc_type_id) ", con))
                            {
                                cmd.Parameters.AddWithValue("@doc_name", fileName);
                                cmd.Parameters.AddWithValue("@doc_type", "0");
                                cmd.Parameters.AddWithValue("@location", "'bms/Report'");
                                cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                cmd.Parameters.AddWithValue("@doc_designated", reloadlbp4 + "," + officeheadid + "," + mafprepid + ",2635,344880," + sign_eid_gov);
                                cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                cmd.Parameters.AddWithValue("@doc_eid", reloadlbp4);
                                cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                cmd.Parameters.AddWithValue("@doc_code", str_random);
                                cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                cmd.Parameters.AddWithValue("@doc_is", "11");
                                cmd.Parameters.AddWithValue("@doc_type_id", "118");
                                cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    else //lbp form 1
                    {
                        officeheadid = 2764; // alvin elorde
                        con.Close();
                        //SqlCommand com2 = new SqlCommand(@"insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                        //                                                       "values('" + strwfpno + "',1,NULL,'bms/Report','" + strwfpnofile.Replace("'", "''").ToString() + "',1,'" + proposaluserid + "," + officeheadid + "',getdate()," + proposaluserid + ",'','" + str_random + "',1,11," + doctypeid + ") ", con);
                        //con.Open();
                        //data2 = Convert.ToString(com2.ExecuteScalar());

                        using (SqlCommand cmd = new SqlCommand("insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                                                     "values(@doc_name,@doc_type,@doc_attachement,@location,@doc_description,@doc_status_id,@doc_designated,@doc_datetime,@doc_eid,@doc_datetime_update,@doc_code,@doc_signatory_type,@doc_is,@doc_type_id) ", con))
                        {
                            cmd.Parameters.AddWithValue("@doc_name", fileName);
                            cmd.Parameters.AddWithValue("@doc_type", "0");
                            cmd.Parameters.AddWithValue("@location", "'bms/Report'");
                            cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                            cmd.Parameters.AddWithValue("@doc_status_id", "1");
                            cmd.Parameters.AddWithValue("@doc_designated", proposaluserid + "," + officeheadid);
                            cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                            cmd.Parameters.AddWithValue("@doc_eid", proposaluserid);
                            cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                            cmd.Parameters.AddWithValue("@doc_code", str_random);
                            cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                            cmd.Parameters.AddWithValue("@doc_is", "11");
                            cmd.Parameters.AddWithValue("@doc_type_id", doctypeid);
                            cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    DataTable doc_attach = new DataTable();

                    var doc_sign = "";
                    string _sqldoc = "Select [doc_id],doc_designated from [bacpdfsign].[dbo].document_attach where doc_name='" + strwfpno + "'";
                    doc_attach = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqldoc).Tables[0];
                    opndocid = Convert.ToInt32(doc_attach.Rows[0][0]);
                    doc_sign = doc_attach.Rows[0][1].ToString();
                    var doc_sign_split = doc_sign.Split(',');

                    var datarep = "";
                    con.Close();
                    SqlCommand com3 = new SqlCommand(@"exec [bacpdfsign].[dbo].sp_bms_wfpsignatory '" + strfileonly + "'," + opndocid + ",'" + str_random + "',0,0," + YearOf + ",0", con);
                    con.Open();
                    datarep = Convert.ToString(com3.ExecuteScalar());

                    //for (int i = 0; i < doc_sign_split.Length; i++)
                    //{
                    //    var data3 = "";
                    //    con.Close();

                    //    SqlCommand com3 = new SqlCommand(@"insert into [bacpdfsign].[dbo].[document_signatories] ([doc_id],[sig_code],[sig_eid],[sig_status],[sig_order],[sig_remarks],[sig_query_signed],[sig_query_return],[sig_user_type],sig_level) " +
                    //                                            "values(" + opndocid + ",'" + str_random + "'," + doc_sign_split[i] + ",0," + (i + 1) + ",'','','',0,1) ", con);
                    //    con.Open();
                    //    data3 = Convert.ToString(com3.ExecuteScalar());

                    //}
                    con.Close();
                    SqlCommand opnstrconcat = new SqlCommand(@"Select concat(" + opndocid + ",',','" + str_random + "',',','" + strwfpno + "',',',0,',,'," + officeheadid + ",',',0)", con);
                    con.Open();
                    strconcat = Convert.ToString(opnstrconcat.ExecuteScalar());

                    var strencryted = Rijndael.Encrypt(strconcat);

                    pageurl = "https://pgas.ph/dgsign/blank/jklsdhfikujdfhfgiuodfhf?data=" + Server.UrlEncode(strencryted) + "";

                    var data4 = "";
                    con.Close();
                    SqlCommand comupdate = new SqlCommand(@"Update ifmis.dbo.[tbl_T_BMSLBPReport_xml] set pageurl='" + pageurl + "' where [yearof]=" + YearOf + " and [officeid]=" + OfficeID + " and typeid =" + ReportTypeID + " and isnull([qrcode],'')='" + GlobalFunctions.QR_globalstr + "'", con);
                    con.Open();
                    data4 = Convert.ToString(comupdate.ExecuteScalar());

                    DataTable doc_signature = new DataTable();
                    var email = "";
                    var fullname = "";
                    if (docuid == 11) //ARO -SMS
                    {
                        string _docsign = "Select upper([username]) username,b.Firstname + ' '+ left(b.mi,1) + '. ' + b.Lastname + ' ' + isnull(b.Suffix,'') as employee,c.[Cellphoneno] from [pmis].[dbo].[eportalUser] as a inner join pmis.dbo.employee as b on b.eid=a.eid inner join [pmis].[dbo].[EmployeeFullName] as c on c.eid=b.eid where a.[eid]=" + officeheadid + "";
                        doc_signature = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _docsign).Tables[0];
                        email = doc_signature.Rows[0][0].ToString();
                        fullname = doc_signature.Rows[0][1].ToString();
                        var fullname_split = fullname.Split('@');
                        var smsresult = "";

                        var msg = "Good Day " + fullname_split[0] + ". Allotment Release Order (ARO) document is waiting for your signature in the Dgsign App. You may access it at " + pageurl + ". \n\nThis is a system-generated message. Please do not reply.";
                        con.Close();
                        ////exec[pmis].[dbo].[nald_sp_api_sms_sender]  @recipient, @message, @isid
                        SqlCommand comSMS = new SqlCommand(@"exec [pmis].[dbo].[nald_sp_api_sms_sender] '" + doc_signature.Rows[0][2].ToString() + "','" + msg + "', 1414 ", con);
                        con.Open();
                        smsresult = Convert.ToString(comSMS.ExecuteScalar());
                    }
                    if (docuid == 12) //SAAO -SMS
                    {
                        string _docsign = "Select upper([username]) username,b.Firstname + ' '+ left(b.mi,1) + '. ' + b.Lastname + ' ' + isnull(b.Suffix,'') as employee,c.[Cellphoneno] from [pmis].[dbo].[eportalUser] as a inner join pmis.dbo.employee as b on b.eid=a.eid inner join [pmis].[dbo].[EmployeeFullName] as c on c.eid=b.eid where a.[eid]=" + officeheadid + "";
                        doc_signature = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _docsign).Tables[0];
                        email = doc_signature.Rows[0][0].ToString();
                        fullname = doc_signature.Rows[0][1].ToString();
                        var fullname_split = fullname.Split('@');
                        var smsresult = "";

                        var msg = "Good Day " + fullname_split[0] + ". Allotment Release Order (SAAO) document is waiting for your signature in the Dgsign App. You may access it at " + pageurl + ". \n\nThis is a system-generated message. Please do not reply.";
                        con.Close();
                        ////exec[pmis].[dbo].[nald_sp_api_sms_sender]  @recipient, @message, @isid
                        SqlCommand comSMS = new SqlCommand(@"exec [pmis].[dbo].[nald_sp_api_sms_sender] '" + doc_signature.Rows[0][2].ToString() + "','" + msg + "', 1414 ", con);
                        con.Open();
                        smsresult = Convert.ToString(comSMS.ExecuteScalar());
                    }
                    if (docuid == 13) //MAF -SMS
                    {
                        string _docsign = "Select upper([username]) username,b.Firstname + ' '+ left(b.mi,1) + '. ' + b.Lastname + ' ' + isnull(b.Suffix,'') as employee,c.[Cellphoneno] from [pmis].[dbo].[eportalUser] as a inner join pmis.dbo.employee as b on b.eid=a.eid inner join [pmis].[dbo].[EmployeeFullName] as c on c.eid=b.eid where a.[eid]=" + officeheadid + "";
                        doc_signature = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _docsign).Tables[0];
                        email = doc_signature.Rows[0][0].ToString();
                        fullname = doc_signature.Rows[0][1].ToString();
                        var fullname_split = fullname.Split('@');
                        var smsresult = "";

                        var msg = "Good Day " + fullname_split[0] + ". Modification Advice Form (MAF) document is waiting for your signature in the Dgsign App. You may access it at " + pageurl + ". \n\nThis is a system-generated message. Please do not reply.";
                        con.Close();
                        ////exec[pmis].[dbo].[nald_sp_api_sms_sender]  @recipient, @message, @isid
                        SqlCommand comSMS = new SqlCommand(@"exec [pmis].[dbo].[nald_sp_api_sms_sender] '" + doc_signature.Rows[0][2].ToString() + "','" + msg + "', 1414 ", con);
                        con.Open();
                        comSMS.CommandTimeout = 0;
                        smsresult = Convert.ToString(comSMS.ExecuteScalar());
                    }
                    else if (docuid == 9) //SUPPLEMENTAL BUDGET 3 -SMS
                    {
                        string _docsign = "Select upper([username]) username,b.Firstname + ' '+ left(b.mi,1) + '. ' + b.Lastname + ' ' + isnull(b.Suffix,'') as employee,c.[Cellphoneno] from [pmis].[dbo].[eportalUser] as a inner join pmis.dbo.employee as b on b.eid=a.eid inner join [pmis].[dbo].[EmployeeFullName] as c on c.eid=b.eid where a.[eid]=" + officeheadid + "";
                        doc_signature = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _docsign).Tables[0];
                        email = doc_signature.Rows[0][0].ToString();
                        fullname = doc_signature.Rows[0][1].ToString();
                        var fullname_split = fullname.Split('@');
                        var smsresult = "";

                        var msg = "Good Day " + fullname_split[0] + ". Supplemental Budget No. 3 (SB3) document is waiting for your signature in the Dgsign App. You may access it at " + pageurl + ". \n\nThis is a system-generated message. Please do not reply.";
                        con.Close();
                        ////exec[pmis].[dbo].[nald_sp_api_sms_sender]  @recipient, @message, @isid
                        SqlCommand comSMS = new SqlCommand(@"exec [pmis].[dbo].[nald_sp_api_sms_sender] '" + doc_signature.Rows[0][2].ToString() + "','" + msg + "', 1414 ", con);
                        con.Open();
                        smsresult = Convert.ToString(comSMS.ExecuteScalar());
                    }
                    else if (docuid == 11 || docuid == 2) //lbp 2 or 4 -SMS
                    {
                        string _docsign = "";
                        if (OfficeID == 49 || OfficeID == 57 || OfficeID == 14) //vice gov, sp, secretary
                                                                                //354982 - EBORAH MAE -vg counter sign
                        {
                            _docsign = "Select upper([username]) username,b.Firstname + ' '+ left(b.mi,1) + '. ' + b.Lastname + ' ' + isnull(b.Suffix,'') as employee,c.[Cellphoneno] from [pmis].[dbo].[eportalUser] as a inner join pmis.dbo.employee as b on b.eid=a.eid inner join [pmis].[dbo].[EmployeeFullName] as c on c.eid=b.eid where a.[eid]=354982";
                        }
                        else
                        {
                            _docsign = "Select upper([username]) username,b.Firstname + ' '+ left(b.mi,1) + '. ' + b.Lastname + ' ' + isnull(b.Suffix,'') as employee,c.[Cellphoneno] from [pmis].[dbo].[eportalUser] as a inner join pmis.dbo.employee as b on b.eid=a.eid inner join [pmis].[dbo].[EmployeeFullName] as c on c.eid=b.eid where a.[eid]=" + officeheadid + "";
                        }
                        //string _docsign = "Select upper([username]) username,b.Firstname + ' '+ left(b.mi,1) + '. ' + b.Lastname + ' ' + isnull(b.Suffix,'') as employee,c.[Cellphoneno] from [pmis].[dbo].[eportalUser] as a inner join pmis.dbo.employee as b on b.eid=a.eid inner join [pmis].[dbo].[EmployeeFullName] as c on c.eid=b.eid where a.[eid]=" + officeheadid + "";
                        doc_signature = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _docsign).Tables[0];
                        email = doc_signature.Rows[0][0].ToString();
                        fullname = doc_signature.Rows[0][1].ToString();
                        var fullname_split = fullname.Split('@');
                        var smsresult = "";

                        var msg = "Good Day " + fullname_split[0] + ". A Local Budget Preparation Form 2 or 4 (LBP Form) is pending your signature in the Dgsign App. You may access it at " + pageurl + ". \n\nThis is a system-generated message. Please do not reply.";
                        con.Close();
                        ////exec[pmis].[dbo].[nald_sp_api_sms_sender]  @recipient, @message, @isid
                        SqlCommand comSMS = new SqlCommand(@"exec [pmis].[dbo].[nald_sp_api_sms_sender] '" + doc_signature.Rows[0][2].ToString() + "','" + msg + "', 1414 ", con);
                        con.Open();
                        smsresult = Convert.ToString(comSMS.ExecuteScalar());
                    }
                    else
                    {
                        if (docuid != 7)
                        {
                            string _docsign = "Select upper([username]) username,b.Firstname + ' '+ left(b.mi,1) + '. ' + b.Lastname + ' ' + isnull(b.Suffix,'') as employee from [pmis].[dbo].[eportalUser] as a inner join pmis.dbo.employee as b on b.eid=a.eid where a.[eid]=" + officeheadid + "";
                            doc_signature = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _docsign).Tables[0];

                            email = doc_signature.Rows[0][0].ToString();
                            fullname = doc_signature.Rows[0][1].ToString();
                            var fullname_split = fullname.Split('@');

                            sendViaEmailOTP(email, fullname_split[0], pageurl, ReportTypeID);
                        }
                        else
                        {
                            string _docsign = "Select upper([username]) username,b.Firstname + ' '+ left(b.mi,1) + '. ' + b.Lastname + ' ' + isnull(b.Suffix,'') as employee from [pmis].[dbo].[eportalUser] as a inner join pmis.dbo.employee as b on b.eid=a.eid where a.[eid]=2635";
                            doc_signature = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _docsign).Tables[0];

                            email = doc_signature.Rows[0][0].ToString();
                            fullname = doc_signature.Rows[0][1].ToString();
                            var fullname_split = fullname.Split('@');

                            sendViaEmailOTP(email, fullname_split[0], pageurl, ReportTypeID);
                        }
                    }

                }
                return pageurl;
            }
        }
        public string sendViaEmailOTP(string email, string fullname, string pageurl, int docuid)
        {
            try
            {
                string topics = "DIGITAL SIGNATURE";
                string fors = "for request of Digital Signature";
                string OTS = RandomString(5);
                string docname = "";
                if (docuid == 0)
                {
                    docname = "Work and Financial Plan (WFP)";
                }
                else if (docuid == 1 || docuid == 2 || docuid == 3 || docuid == 4)
                {
                    docname = "Local Budget Preparation (LBP) Form 2";
                }
                else if (docuid == 5)
                {
                    docname = "Local Budget Preparation (LBP) Form 4";
                }
                else if (docuid == 6)
                {
                    docname = "Local Budget Preparation (LBP) Form 1";
                }
                else if (docuid == 7)
                {
                    docname = "Local Budget Preparation (LBP) Form 7";
                }
                string[] ots_public = { OTS, fullname, fors, topics, pageurl, docname };

                string body = PartialView("_pv_OTP_mail_detail", ots_public).ConvertToString(ControllerContext);

                MailMessage message = new MailMessage();
                message.From = new MailAddress("isadmin@pgas.ph", "DigiSign");
                message.To.Add(new MailAddress(email));
                message.Subject = "DIGITAL SIGNATURE";
                message.Body = @"" + body;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient();
                client.Host = "mail.pgas.ph";
                client.EnableSsl = true;
                client.UseDefaultCredentials = true;
                client.Credentials = new System.Net.NetworkCredential("isadmin@pgas.ph", "EE942086-5ED5-464B-A709-1A5FD0EF23C8");
                client.Send(message);
                return OTS;
            }
            catch (Exception ec)
            {
                return "69";
            }
        }

        public string ReturnWFP_submitted(string[] transno, string[] isPPMP, string[] programid , string[] accountid, string[] ooeclass,string[] remarkval, int? yearof = 0, int? fundid = 0,int? programidv2=0, int? accountidv2=0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("transno");
                dt.Columns.Add("isPPMP");
                dt.Columns.Add("programid");
                dt.Columns.Add("accountid");
                dt.Columns.Add("ooeclass");
                dt.Columns.Add("remarkval");
                var idx = 0;
                var strname = "";
                foreach (var trnno in transno)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = transno[idx];
                    dr[1] = isPPMP[idx];
                    dr[2] = programid[idx];
                    dr[3] = accountid[idx];
                    dr[4] = ooeclass[idx];
                    dr[5] = remarkval[idx];
                    dt.Rows.Add(dr);
                    idx++;
                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    if (fundid == 0)
                    {
                        strname = "dbo.[sp_BMS_WFPSubmitted_return]";
                    }
                    else
                    {
                        strname = "dbo.[sp_BMS_WFPSubmitted_return_TF]";
                    }
                    SqlCommand com = new SqlCommand("" + strname + "", con);
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add(new SqlParameter("@trnno", dt));
                    com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                    com.Parameters.Add(new SqlParameter("@yearof", yearof));
                    //com.Parameters.Add(new SqlParameter("@programid", programid));
                    //com.Parameters.Add(new SqlParameter("@accountid", accountid));
                    //com.Parameters.Add(new SqlParameter("@ooe", ooeclass));
                    con.Open();

                    //send sms //=======//=======//=======//=======//=======//==========


                    //SMS- TEMPORAry disabled
                    var smsresult = "";
                    DataTable _wfno = new DataTable();
                    string _sqlwfp = "select [EmpNameFull] from [pmis].[dbo].[vwMergeAllEmployee_Modified] where [eid]=" + Account.UserInfo.eid + "";
                    _wfno = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlwfp).Tables[0];
                    string empnamereturn = _wfno.Rows[0][0].ToString();

                    DataTable _tmpaccount = new DataTable();
                    string tempaccount = "";
                    if (fundid == 0)
                    {
                        string _sqlwfp2 = "select AccountName from ifmis.dbo.tbl_R_BMSProgramAccounts where AccountYear=" + yearof + " and ActionCode=1 and ProgramID = " + programidv2 + " and AccountID=" + accountidv2 + "";
                        _tmpaccount = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlwfp2).Tables[0];
                        tempaccount = _tmpaccount.Rows[0][0].ToString();

                        DataTable _tmpuser = new DataTable();
                        string _sqlwfp3 = "exec [dbo].[sp_BMS_WFP_Preparer_PerActivity] " + programidv2 + "," + accountidv2 + "," + yearof + "";
                        _tmpuser = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlwfp3).Tables[0];
                        if (_tmpuser.Rows.Count > 0)
                        {
                            string tempusername = _tmpuser.Rows[0][7].ToString();
                            string tempcpno = _tmpuser.Rows[0][10].ToString();

                            var msg = "Good day, " + tempusername + ". The Work and Financial Plan (WFP) for " + tempaccount + " has been returned by " + empnamereturn + ". \n\nThis is a system-generated message. Please do not reply.";
                            con.Close();
                            ////exec[pmis].[dbo].[nald_sp_api_sms_sender]  @recipient, @message, @isid
                            SqlCommand comSMS = new SqlCommand(@"exec [pmis].[dbo].[nald_sp_api_sms_sender] '" + tempcpno + "','" + msg.Replace("'","").ToString() + "', 1414 ", con);
                            con.Open();
                            smsresult = Convert.ToString(comSMS.ExecuteScalar());
                            
                        }  
                    }

                    //SMS

                    //send sms//=======//=======//=======//=======//=======//==========

                    return com.ExecuteScalar().ToString();
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //public static async System.Threading.Tasks.Task<string> send(string text, string receiver, string isid="1414")
        //{

        //    var values = new Dictionary<string, string>
        //      {
        //          { "text", text },
        //          { "destination", receiver },
        //          { "isid", isid }
        //      };

        //    var content = new FormUrlEncodedContent(values);

        //    using (var client = new HttpClient())
        //    {
        //        var response = await client.PostAsync("https://serve.pgas.ph/smsapi/api/Send", content);
        //        var responseString = await response.Content.ReadAsStringAsync();
        //        return responseString;
        //    }
        //}
        //private static Task client;
        //public static Tasks client { get; private set; }
        //public static async System.Threading.Tasks.Task<string> send(string text, string receiver, string isid)
        //{

        //    var values = new Dictionary<string, string>
        //      {
        //          { "text", text },
        //          { "destination", receiver },
        //          { "isid", isid }
        //      };

        //    var content = new FormUrlEncodedContent(values);

        //    System.Threading.Tasks.Task client = null;
        //    //var response = await client.PostAsync("https://serve.pgas.ph/smsapi/api/Send", content);
        //    var response = await client.PostAsync("https://serve.pgas.ph/smsapi/api/Send", content);

        //    var responseString = await response.Content.ReadAsStringAsync();

        //    return responseString;
        //}
        private object ResolveUrl(string pageurl)
        {
            throw new NotImplementedException();
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        //    private HttpClient httpClient = new HttpClient();

        //public string networkPath = @"\\192.168.2.210\pgas_attachment\bms\WFP";
        public string networkPath = @"r:\Web Application\iFMIS-BMS_publish";
        //public string networkPath_report = @"\\192.168.2.210\pgas_attachment\bms\Report";
        public string networkPath_report = @"r:\Web Application\iFMIS-BMS_publish";
        private Task<string> SmsHelper;

        //private static readonly System.Threading.Tasks.Task client;


        //   private static readonly Task client;
        //    public System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> PostAsync(string? requestUri, System.Net.Http.HttpContent? content);
        //public static Task client { get; private set; }

        //private static Task client;

        public string GenerateReportPDF(int? officeid = 0, int? yearof = 0, string officename = "", int? prepby = 0, int? repHistory = 0, int? printstatus = 0, int? projectaip = 0, string fundsource = "", int? accountID = 0, string accountname = "", string municipal = "", string barangay = "", string ooeclass = "", int pgas_loc = 0)
        {
            SqlConnection connection = new SqlConnection(Common.MyConn());

            var deviceInfo = new System.Collections.Hashtable();
            InstanceReportSource rs = new Telerik.Reporting.InstanceReportSource();
            var reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();
            var typeReportSource = new Telerik.Reporting.TypeReportSource();
            rs.ReportDocument = new WFPNew(yearof, officeid, 0, 0, officename, "", "", repHistory, prepby, printstatus, projectaip, "", accountID, accountname, municipal, barangay, ooeclass, pgas_loc, 1);

            Telerik.Reporting.Processing.RenderingResult result = reportProcessor.RenderReport("PDF", rs, deviceInfo);

            string fileName = "";
            NetworkCredential credentials = new NetworkCredential(@"pgas.ph\rhayan.gubalane", "DomainUser1");
            //   var credential = new NetworkCredential(username, password);
            var server_path = networkPath + "\\" + Account.UserInfo.eid;

            DataTable _wfno = new DataTable();
            string _sqlwfp = "Select upper([wfpno]) from ifmis.dbo.tbl_T_BMSWFP_xml where [qrcode]='" + GlobalFunctions.QR_globalstr + "' and isnull(approveprint,0)=1";
            _wfno = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlwfp).Tables[0];
            string strwfpno = _wfno.Rows[0][0].ToString();

            if (!result.HasErrors)
            {
                //fileName = result.DocumentName + "." + result.Extension;
                fileName = strwfpno + "." + result.Extension;
                //string path = "C:\\Users\\admin\\Documents\\Public\\WFP";// System.IO.Path.GetTempPath(); //local connection
                string path = networkPath;// System.IO.Path.GetTempPath();
                string filePath = System.IO.Path.Combine(path, fileName);

                //using (System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                //{
                //    fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
                //}
                using (new ConnectToSharedFolder(networkPath, credentials))
                {
                    var fileList = Directory.GetDirectories(networkPath);
                    using (System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                    {
                        fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
                    }
                }
            }

            return "success";
        }
        public PartialViewResult wfpstatus_review(int? office = 0, int? program = 0, long? account = 0, int? tyear = 0, int? fundid = 0, int? mode_trans = 0)
        {
            return PartialView("pv_WFPreview_StatusTab");
        }

        public PartialViewResult wfpstatus_review_regular()
        {
            return PartialView("pv_WFPreview_status");
        }

        public PartialViewResult wfpstatus_review_reroute()
        {
            return PartialView("pv_WFPreroute");
        }
        public PartialViewResult wfprevision(int? office = 0, int? program = 0, long? account = 0, int? tyear = 0, int? fundid = 0)
        {
            Session["office"] = office;
            Session["program"] = program;
            Session["account"] = account;
            Session["tyear"] = tyear;
            Session["fundid"] = fundid;
            return PartialView("pv_WFPRevision");
        }

        public PartialViewResult wfpritem(int? office = 0, int? program = 0, long? account = 0, int? tyear = 0, int? fundid = 0)
        {
            Session["office"] = office;
            Session["program"] = program;
            Session["account"] = account;
            Session["tyear"] = tyear;
            Session["fundid"] = fundid;
            return PartialView("pv_WFPRItem");
        }

        public PartialViewResult wfpdfppt(int? tyear = 0)
        {
            Session["tyear"] = tyear;
            return PartialView("pv_WFPDFPPT_Tab");
        }
        public JsonResult GetWFPStatus_review([DataSourceRequest]DataSourceRequest request, int? year = 0, int? review = 0, int? showspco = 0, int? officeid = 0, int? fundid = 0, int? mode_trans = 0)
        {
            List<WFPrepare> prog = new List<WFPrepare>();
            if (Account.UserInfo.UserTypeID >= 4) //lfc / super admin
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    if (fundid == 0) //gf
                    {
                        if (mode_trans == 1) // current
                        {
                            SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFP_statusreview] " + year + "," + showspco + "," + officeid + "", con);
                            con.Open();
                            com.CommandTimeout = 0;
                            SqlDataReader reader = com.ExecuteReader();
                            while (reader.Read())
                            {
                                WFPrepare loc = new WFPrepare();
                                loc.officeid = Convert.ToInt32(reader.GetValue(0));
                                loc.office = Convert.ToString(reader.GetValue(1));
                                loc.programid = Convert.ToInt32(reader.GetValue(2));
                                loc.accountid = Convert.ToInt32(reader.GetValue(3));
                                loc.accountname = Convert.ToString(reader.GetValue(4));
                                loc.datetimeentered = Convert.ToString(reader.GetValue(5));
                                loc.yearof = Convert.ToInt32(reader.GetValue(6));
                                loc.remarks = Convert.ToString(reader.GetValue(7));
                                loc.ooe = Convert.ToString(reader.GetValue(8));
                                loc.ooeid = Convert.ToInt32(reader.GetValue(9));
                                loc.withapprove = Convert.ToInt32(reader.GetValue(10));
                                loc.status = Convert.ToString(reader.GetValue(11));
                                loc.gov_sig = Convert.ToInt32(reader.GetValue(12));
                                loc.docid = Convert.ToInt32(reader.GetValue(13));
                                loc.wfpno = Convert.ToString(reader.GetValue(14));
                                loc.account = Convert.ToString(reader.GetValue(15));
                                prog.Add(loc);
                            }
                        }
                        else
                        {
                            SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFP_statusreview_Excess] " + year + "," + showspco + "," + officeid + "", con);
                            con.Open();
                            com.CommandTimeout = 0;
                            SqlDataReader reader = com.ExecuteReader();
                            while (reader.Read())
                            {
                                WFPrepare loc = new WFPrepare();
                                loc.officeid = Convert.ToInt32(reader.GetValue(0));
                                loc.office = Convert.ToString(reader.GetValue(1));
                                loc.programid = Convert.ToInt32(reader.GetValue(2));
                                loc.accountid = Convert.ToInt32(reader.GetValue(3));
                                loc.accountname = Convert.ToString(reader.GetValue(4));
                                loc.datetimeentered = Convert.ToString(reader.GetValue(5));
                                loc.yearof = Convert.ToInt32(reader.GetValue(6));
                                loc.remarks = Convert.ToString(reader.GetValue(7));
                                loc.ooe = Convert.ToString(reader.GetValue(8));
                                loc.ooeid = Convert.ToInt32(reader.GetValue(9));
                                loc.withapprove = Convert.ToInt32(reader.GetValue(10));
                                loc.status = Convert.ToString(reader.GetValue(11));
                                loc.gov_sig = Convert.ToInt32(reader.GetValue(12));
                                loc.docid = Convert.ToInt32(reader.GetValue(13));
                                loc.wfpno = "";
                                loc.account = "";
                                prog.Add(loc);
                            }
                        }
                    }
                    else //tf
                    {
                        SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFP_statusreview_TF] " + year + "," + showspco + "," + officeid + "", con);
                        con.Open();
                        com.CommandTimeout = 0;
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            WFPrepare loc = new WFPrepare();
                            loc.officeid = Convert.ToInt32(reader.GetValue(0));
                            loc.office = Convert.ToString(reader.GetValue(1));
                            loc.programid = Convert.ToInt32(reader.GetValue(2));
                            loc.accountid = Convert.ToInt32(reader.GetValue(3));
                            loc.accountname = Convert.ToString(reader.GetValue(4));
                            loc.datetimeentered = Convert.ToString(reader.GetValue(5));
                            loc.yearof = Convert.ToInt32(reader.GetValue(6));
                            loc.remarks = Convert.ToString(reader.GetValue(7));
                            loc.ooe = Convert.ToString(reader.GetValue(8));
                            loc.ooeid = Convert.ToInt32(reader.GetValue(9));
                            loc.withapprove = Convert.ToInt32(reader.GetValue(10));
                            loc.status = Convert.ToString(reader.GetValue(11));
                            loc.gov_sig = Convert.ToInt32(reader.GetValue(12));
                            loc.docid = Convert.ToInt32(reader.GetValue(13));
                            loc.wfpno = "";
                            loc.account = "";
                            prog.Add(loc);
                        }
                    }
                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    if (fundid == 0) //GF
                    {
                        //SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFP_statusreview] " + year + "," + showspco + "," + Account.UserInfo.Department + "", con);
                        //con.Open();
                        //SqlDataReader reader = com.ExecuteReader();
                        //while (reader.Read())
                        //{
                        //    WFPrepare loc = new WFPrepare();
                        //    loc.officeid = Convert.ToInt32(reader.GetValue(0));
                        //    loc.office = Convert.ToString(reader.GetValue(1));
                        //    loc.programid = Convert.ToInt32(reader.GetValue(2));
                        //    loc.accountid = Convert.ToInt32(reader.GetValue(3));
                        //    loc.accountname = Convert.ToString(reader.GetValue(4));
                        //    loc.datetimeentered = Convert.ToString(reader.GetValue(5));
                        //    loc.yearof = Convert.ToInt32(reader.GetValue(6));
                        //    loc.remarks = Convert.ToString(reader.GetValue(7));
                        //    loc.ooe = Convert.ToString(reader.GetValue(8));
                        //    loc.ooeid = Convert.ToInt32(reader.GetValue(9));
                        //    loc.withapprove = Convert.ToInt32(reader.GetValue(10));
                        //    loc.status = Convert.ToString(reader.GetValue(11));
                        //    loc.gov_sig = Convert.ToInt32(reader.GetValue(12));
                        //    loc.docid = Convert.ToInt32(reader.GetValue(13));
                        //    prog.Add(loc);
                        //}
                        if (mode_trans == 1) // current
                        {
                            SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFP_statusreview] " + year + "," + showspco + "," + officeid + "", con);
                            con.Open();
                            SqlDataReader reader = com.ExecuteReader();
                            while (reader.Read())
                            {
                                WFPrepare loc = new WFPrepare();
                                loc.officeid = Convert.ToInt32(reader.GetValue(0));
                                loc.office = Convert.ToString(reader.GetValue(1));
                                loc.programid = Convert.ToInt32(reader.GetValue(2));
                                loc.accountid = Convert.ToInt32(reader.GetValue(3));
                                loc.accountname = Convert.ToString(reader.GetValue(4));
                                loc.datetimeentered = Convert.ToString(reader.GetValue(5));
                                loc.yearof = Convert.ToInt32(reader.GetValue(6));
                                loc.remarks = Convert.ToString(reader.GetValue(7));
                                loc.ooe = Convert.ToString(reader.GetValue(8));
                                loc.ooeid = Convert.ToInt32(reader.GetValue(9));
                                loc.withapprove = Convert.ToInt32(reader.GetValue(10));
                                loc.status = Convert.ToString(reader.GetValue(11));
                                loc.gov_sig = Convert.ToInt32(reader.GetValue(12));
                                loc.docid = Convert.ToInt32(reader.GetValue(13));
                                loc.wfpno = Convert.ToString(reader.GetValue(14));
                                loc.account = Convert.ToString(reader.GetValue(15));
                                prog.Add(loc);
                            }
                        }
                        else
                        {
                            SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFP_statusreview_Excess] " + year + "," + showspco + "," + officeid + "", con);
                            con.Open();
                            com.CommandTimeout = 0;
                            SqlDataReader reader = com.ExecuteReader();
                            while (reader.Read())
                            {
                                WFPrepare loc = new WFPrepare();
                                loc.officeid = Convert.ToInt32(reader.GetValue(0));
                                loc.office = Convert.ToString(reader.GetValue(1));
                                loc.programid = Convert.ToInt32(reader.GetValue(2));
                                loc.accountid = Convert.ToInt32(reader.GetValue(3));
                                loc.accountname = Convert.ToString(reader.GetValue(4));
                                loc.datetimeentered = Convert.ToString(reader.GetValue(5));
                                loc.yearof = Convert.ToInt32(reader.GetValue(6));
                                loc.remarks = Convert.ToString(reader.GetValue(7));
                                loc.ooe = Convert.ToString(reader.GetValue(8));
                                loc.ooeid = Convert.ToInt32(reader.GetValue(9));
                                loc.withapprove = Convert.ToInt32(reader.GetValue(10));
                                loc.status = Convert.ToString(reader.GetValue(11));
                                loc.gov_sig = Convert.ToInt32(reader.GetValue(12));
                                loc.docid = Convert.ToInt32(reader.GetValue(13));
                                loc.wfpno = "";
                                loc.account = "";
                                prog.Add(loc);
                            }
                        }
                    }
                    else //Trust Fund
                    {
                        SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFP_statusreview_TF] " + year + "," + showspco + "," + officeid + "", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            WFPrepare loc = new WFPrepare();
                            loc.officeid = Convert.ToInt32(reader.GetValue(0));
                            loc.office = Convert.ToString(reader.GetValue(1));
                            loc.programid = Convert.ToInt32(reader.GetValue(2));
                            loc.accountid = Convert.ToInt32(reader.GetValue(3));
                            loc.accountname = Convert.ToString(reader.GetValue(4));
                            loc.datetimeentered = Convert.ToString(reader.GetValue(5));
                            loc.yearof = Convert.ToInt32(reader.GetValue(6));
                            loc.remarks = Convert.ToString(reader.GetValue(7));
                            loc.ooe = Convert.ToString(reader.GetValue(8));
                            loc.ooeid = Convert.ToInt32(reader.GetValue(9));
                            loc.withapprove = Convert.ToInt32(reader.GetValue(10));
                            loc.status = Convert.ToString(reader.GetValue(11));
                            loc.gov_sig = Convert.ToInt32(reader.GetValue(12));
                            loc.docid = Convert.ToInt32(reader.GetValue(13));
                            loc.wfpno = "";
                            loc.account = "";
                            prog.Add(loc);
                        }
                    }
                }
            }
            return Json(prog.ToDataSourceResult(request));
            //var jsonResult = Json(prog.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            //jsonResult.MaxJsonLength = int.MaxValue;
            //return jsonResult;
        }

        public JsonResult GetWFPStatus_reroute([DataSourceRequest]DataSourceRequest request, int? year = 0, int? review = 0, int? showspco = 0, int? officeid = 0, int? fundid = 0, int? mode_trans = 0)
        {
            List<WFPrepare> prog = new List<WFPrepare>();
            if (Account.UserInfo.UserTypeID >= 4) //lfc / super admin
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFP_statusreview_reroute] " + year + "," + officeid + "", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        WFPrepare loc = new WFPrepare();
                        loc.officeid = Convert.ToInt32(reader.GetValue(0));
                        loc.office = Convert.ToString(reader.GetValue(1));
                        loc.programid = Convert.ToInt32(reader.GetValue(2));
                        loc.accountid = Convert.ToInt32(reader.GetValue(3));
                        loc.accountname = Convert.ToString(reader.GetValue(4));
                        loc.datetimeentered = Convert.ToString(reader.GetValue(5));
                        loc.yearof = Convert.ToInt32(reader.GetValue(6));
                        loc.remarks = Convert.ToString(reader.GetValue(7));
                        loc.ooe = Convert.ToString(reader.GetValue(8));
                        loc.ooeid = Convert.ToInt32(reader.GetValue(9));
                        loc.withapprove = Convert.ToInt32(reader.GetValue(10));
                        loc.status = Convert.ToString(reader.GetValue(11));
                        loc.gov_sig = Convert.ToInt32(reader.GetValue(12));
                        loc.docid = Convert.ToInt32(reader.GetValue(13));
                        loc.wfpno = Convert.ToString(reader.GetValue(15));
                        prog.Add(loc);
                    }
                }
            }
            return Json(prog.ToDataSourceResult(request));
        }

        public int getdocid(string docname = "")
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var docid = 0;
                SqlCommand com = new SqlCommand(@"select doc_id from [bacpdfsign].[dbo].[document_attach] where doc_name='" + docname + "'", con);
                con.Open();
                docid = Convert.ToInt32(com.ExecuteScalar());
                return docid;
            }
        }
        public string printdgsign()
        {
            string pageurl = "";
            var str_random = RandomString(10);
            string connectionString = ConfigurationManager.ConnectionStrings["sqldb"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Close();
                //SqlCommand opnstrconcat = new SqlCommand(@"Select concat(" + vwbFormID + ",',','" + str_random + "',',','" + vwbpdf_name_m + "',',',0,',,',344880,',',0)", con);
                SqlCommand opnstrconcat = new SqlCommand(@"Select concat(1273,',','" + str_random + "',',','WFP-23-44.pdf',',',0,',,',344880,',',0)", con);
                con.Open();
                string strconcat = Convert.ToString(opnstrconcat.ExecuteScalar());

                var strencryted = SPMS.Rijndael.Encrypt(strconcat);

                pageurl = "https://pgas.ph/dgsign/blank/jklsdhfikujdfhfgiuodfhf?datas=" + Server.UrlEncode(strencryted) + "";
                return pageurl;
            }
        }
        public string submitwfpitem(string[] transno, int? officeid = 0, int? programid = 0, int? accountid = 0, int? year = 0, int? fundid = 0, int? mode_trans = 0, int? supplemetalapp = 0)
        {
            var retstr = "";
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("transno");
                var idx = 0;
                foreach (var trnno in transno)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = transno[idx];
                    dt.Rows.Add(dr);
                    idx++;
                }
                if (fundid == 0) //gf
                {
                    if (mode_trans == 1) //current
                    {
                        using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        {
                            SqlCommand com = new SqlCommand("dbo.[sp_BMS_WFPsubmititem]", con);
                            com.CommandType = System.Data.CommandType.StoredProcedure;
                            com.Parameters.Add(new SqlParameter("@trnno", dt));
                            com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                            com.Parameters.Add(new SqlParameter("@yearof", year));
                            com.Parameters.Add(new SqlParameter("@programid", programid));
                            com.Parameters.Add(new SqlParameter("@accountid", accountid));
                            com.Parameters.Add(new SqlParameter("@supplemetalapp", supplemetalapp));
                            con.Open();
                            retstr = com.ExecuteScalar().ToString();
                        }
                    }
                    else
                    {
                        using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        {
                            SqlCommand com = new SqlCommand("dbo.[sp_BMS_WFPsubmititem_Excess]", con);
                            com.CommandType = System.Data.CommandType.StoredProcedure;
                            com.Parameters.Add(new SqlParameter("@trnno", dt));
                            com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                            com.Parameters.Add(new SqlParameter("@yearof", year));
                            com.Parameters.Add(new SqlParameter("@officeid", officeid));
                            com.Parameters.Add(new SqlParameter("@accountid", accountid));
                            con.Open();
                            retstr = com.ExecuteScalar().ToString();
                        }
                    }
                }
                else //tf
                {
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand("dbo.[sp_BMS_WFPsubmititem_TF]", con);
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.Parameters.Add(new SqlParameter("@trnno", dt));
                        com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                        com.Parameters.Add(new SqlParameter("@yearof", year));
                        com.Parameters.Add(new SqlParameter("@officeid", officeid));
                        com.Parameters.Add(new SqlParameter("@accountid", accountid));
                        con.Open();
                        retstr = com.ExecuteScalar().ToString();
                    }
                }
                return retstr;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public ActionResult GetOfficeActivity_sub([DataSourceRequest]DataSourceRequest request, int? spec_id = 0)
        {
            //string tempStr = "select * from fn_BMS_wfpactivity (" + officeid + "," + tyear + "," + Account.UserInfo.eid + ") order by  initiative";
            string tempStr = "Select [spec_id] AS initiative_id,[specificactivity] AS initiative from [IFMIS].[dbo].[tbl_R_BMS_WFPspecificactivity] where [actioncode]=1 and [spec_id]=" + spec_id + "";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public JsonResult GetWFPStatus_revision([DataSourceRequest]DataSourceRequest request, int? year = 0, int? officeid = 0, int? fundid = 0)
        {
            List<WFPrepare> prog = new List<WFPrepare>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (fundid == 0) //gf
                {
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFPRevision_count] " + officeid + ",0," + year + "", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        WFPrepare loc = new WFPrepare();
                        loc.officeid = Convert.ToInt32(reader.GetValue(0));
                        loc.office = Convert.ToString(reader.GetValue(1));
                        loc.accountid = Convert.ToInt32(reader.GetValue(2));
                        loc.accountname = Convert.ToString(reader.GetValue(3));
                        loc.revno = Convert.ToInt32(reader.GetValue(4));
                        loc.yearof = Convert.ToInt32(reader.GetValue(5));
                        prog.Add(loc);
                    }
                }
            }
            return Json(prog.ToDataSourceResult(request));
        }
        public JsonResult GetWFPStatus_revision_dtls([DataSourceRequest]DataSourceRequest request, int? year = 0, int? officeid = 0, int? fundid = 0, int? accountid = 0)
        {
            List<WFPrepare> prog = new List<WFPrepare>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (fundid == 0) //gf
                {
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFPRevision_count] " + officeid + "," + accountid + "," + year + "", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        WFPrepare loc = new WFPrepare();
                        loc.office = Convert.ToString(reader.GetValue(0));
                        loc.accountname = Convert.ToString(reader.GetValue(1));
                        loc.wfpno = Convert.ToString(reader.GetValue(2));
                        loc.docid = Convert.ToInt32(reader.GetValue(3));
                        loc.datetimeentered = Convert.ToString(reader.GetValue(4));
                        prog.Add(loc);
                    }
                }
            }
            return Json(prog.ToDataSourceResult(request));
        }
        public int getusertf()
        {

            DataTable prep_id = new DataTable();
            int tfid = 0;
            string _sqlprep = "select [fund_gf],[fund_tf] FROM [IFMIS].[dbo].[tbl_R_BMSWFP_TrustFundUser] where userid = " + Account.UserInfo.eid + " and [actioncode]=1";
            prep_id = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlprep).Tables[0];
            if (prep_id.Rows.Count > 0)
            {
                tfid = Convert.ToInt32(prep_id.Rows[0][0]) + Convert.ToInt32(prep_id.Rows[0][1]);
            }
            else
            {
                tfid = 2;
            }
            return tfid;
        }
        public JsonResult GetWFPApprove([DataSourceRequest]DataSourceRequest request, int? office = 0, int? year = 0, int? fundid = 0, int? mode_trans = 0)
        {

            List<WFPrepare> prog = new List<WFPrepare>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (fundid == 0) //gf
                {
                    if (mode_trans == 1) //current
                    {
                        SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFP_statusreview_approve] " + year + "," + office + "," + fundid + "", con);
                        con.Open();
                        com.CommandTimeout = 0;
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            WFPrepare emp = new WFPrepare();
                            emp.docid = Convert.ToInt32(reader.GetValue(8));
                            emp.officeid = Convert.ToInt32(reader.GetValue(0));
                            emp.office = Convert.ToString(reader.GetValue(1));
                            emp.accountid = Convert.ToInt64(reader.GetValue(3));
                            emp.accountname = Convert.ToString(reader.GetValue(4));
                            emp.yearof = Convert.ToInt32(reader.GetValue(5));
                            emp.status = reader.GetValue(6).ToString();
                            emp.wfpno = Convert.ToString(reader.GetValue(9));
                            prog.Add(emp);
                        }
                    }
                    else
                    {
                        SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFP_statusreview_approve_excess] " + year + "," + office + "," + fundid + "", con);
                        con.Open();
                        com.CommandTimeout = 0;
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            WFPrepare emp = new WFPrepare();
                            emp.docid = Convert.ToInt32(reader.GetValue(8));
                            emp.officeid = Convert.ToInt32(reader.GetValue(0));
                            emp.office = Convert.ToString(reader.GetValue(1));
                            emp.accountid = Convert.ToInt64(reader.GetValue(3));
                            emp.accountname = Convert.ToString(reader.GetValue(4));
                            emp.yearof = Convert.ToInt32(reader.GetValue(5));
                            emp.status = reader.GetValue(6).ToString();
                            emp.wfpno = Convert.ToString(reader.GetValue(9));
                            prog.Add(emp);
                        }
                    }
                }

            }
            return Json(prog.ToDataSourceResult(request));// prog;
        }
        public JsonResult GetLBPDgsign_status([DataSourceRequest]DataSourceRequest request, int? year = 0, int? officeid = 0, int? docuid = 0)
        {
            List<WFPrepare> prog = new List<WFPrepare>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFP_statusreview_lbpreport] " + year + "," + officeid + "," + docuid + "", con);
                con.Open();
                com.CommandTimeout = 0;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WFPrepare loc = new WFPrepare();
                    loc.officeid = Convert.ToInt32(reader.GetValue(0));
                    loc.office = Convert.ToString(reader.GetValue(1));
                    loc.accountname = Convert.ToString(reader.GetValue(2));
                    loc.datetimeentered = Convert.ToString(reader.GetValue(3));
                    loc.yearof = Convert.ToInt32(reader.GetValue(4));
                    loc.status = Convert.ToString(reader.GetValue(5));
                    loc.gov_sig = Convert.ToInt32(reader.GetValue(6));
                    loc.docid = Convert.ToInt32(reader.GetValue(7));
                    prog.Add(loc);
                }
            }
            return Json(prog.ToDataSourceResult(request));
        }
        public PartialViewResult LBPDGSiginstatus(int? office = 0, int? program = 0, int? account = 0, int? tyear = 0, int? docuid = 0)
        {
            Session["office"] = office;
            Session["tyear"] = tyear;
            Session["program"] = program;
            Session["account"] = account;
            Session["docuid"] = docuid;
            return PartialView("pv_LBPStatus");
            //return PartialView("pv_WFPReturn");
        }


        public string additemeproc_proposed(string[] transno, int? officeid = 0, int? programid = 0, int? accountid = 0, int? tyear = 0, int? fundid = 0, int? mode_trans = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("transno");
                var idx = 0;
                foreach (var trnno in transno)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = transno[idx];
                    dt.Rows.Add(dr);
                    idx++;
                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("dbo.[sp_BMS_WFPItemADD_Propose]", con);
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add(new SqlParameter("@trnno", dt));
                    com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                    com.Parameters.Add(new SqlParameter("@officeid", officeid));
                    com.Parameters.Add(new SqlParameter("@programid", programid));
                    com.Parameters.Add(new SqlParameter("@accountid", accountid));
                    com.Parameters.Add(new SqlParameter("@year", tyear));
                    con.Open();

                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string deleteitem_proposed(int? id = 0, int? account = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"[sp_BMS_DeleteProposeItem] " + id + ",'" + Account.UserInfo.eid.ToString() + "'," + account + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public ActionResult GetaccountPropose([DataSourceRequest]DataSourceRequest request, int? programid = 0, int? tyear = 0)
        {
            string tempStr = "SELECT AccountID,AccountName FROM [IFMIS].[dbo].[tbl_R_BMSProgramAccounts] where AccountYear=" + tyear + " and ActionCode=1  and ProgramID=" + programid + " and (accountid in (59369,69880,70390) or AccountName like '%internet subscription%') order by AccountName";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public PartialViewResult vwWFPpreparer(int? id = 0)
        {
            Session["id"] = id;
            return PartialView("pv_WFPreview_preparer");
        }
        public ActionResult GetEmployeelist([DataSourceRequest]DataSourceRequest request, int? natloffice = 0)
        {

            //string tempStr = "select [nonofficeid],nonofficeidparent,[accountname],officeid,programid,accountid,accountnameid from [IFMIS].[dbo].[tbl_R_BMSNonOffice] where [ProgramID]=" + program + " and [accountid]=" + account + " and [YearOf]=" + year + " and actioncode=1 and excess=" + excessid + " order by accountname";
            if (natloffice == 0)
            {
                string tempStr = "Select [eid],[EmpName] from [pmis].[dbo].[vwMergeAllEmployee_Modified] order by [EmpName]";

                DataTable dt = tempStr.DataSet();
                var result = new ContentResult();
                result.Content = SerializeDT.DataTableToJSON(dt);
                result.ContentType = "application/json";
                return result;
            }
            else
            {
                string tempStr = "Select [id] as eid,[last_name] +', ' + [first_name] + ' ' + left([middle_name],1) + '. ' as [EmpName] from [bacpdfsign].[dbo].[accounts_registered] where last_name is not null  order by [last_name]";

                DataTable dt = tempStr.DataSet();
                var result = new ContentResult();
                result.Content = SerializeDT.DataTableToJSON(dt);
                result.ContentType = "application/json";
                return result;
            }
        }
        public string TransferPrep(int? docid = 0, int? eid = 0, int? eid_dh = 0, int? natofficeid = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    if (natofficeid == 0)//pgas employee
                    {
                        SqlCommand com = new SqlCommand(@"sp_bms_WFPTrasnferPreparer " + docid + "," + eid + "," + eid_dh + "", con);
                        con.Open();
                        data = Convert.ToString(com.ExecuteScalar());
                    }
                    else
                    {
                        SqlCommand com = new SqlCommand(@"sp_bms_WFPTrasnferPreparer_National " + docid + "," + eid + "," + eid_dh + "", con);
                        con.Open();
                        data = Convert.ToString(com.ExecuteScalar());
                    }
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public JsonResult GetWFPSubmittedSumm([DataSourceRequest]DataSourceRequest request, int? yearof = 0, int? officeid = 0, int? submit = 0)
        {

            List<WFPrepare> prog = new List<WFPrepare>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFPSummary_Submitted] " + yearof + "," + officeid + "," + submit + "", con);
                con.Open();
                com.CommandTimeout = 0;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WFPrepare emp = new WFPrepare();
                    emp.accountid = Convert.ToInt64(reader.GetValue(0));
                    emp.accountname = Convert.ToString(reader.GetValue(1));
                    emp.datetimeentered = Convert.ToString(reader.GetValue(2));
                    emp.office = Convert.ToString(reader.GetValue(3));
                    emp.amount = Convert.ToDouble(reader.GetValue(4));
                    prog.Add(emp);
                }
            }
            return Json(prog.ToDataSourceResult(request));// prog;
        }
        public int wfplock_epa(int? yearof = 0)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var data = 0;
                SqlCommand com = new SqlCommand(@"SELECT count([wfplockid]) FROM [IFMIS].[dbo].[tbl_R_BMS_WFPQtrLock] where yearof=" + yearof + " and qtr=1 and islock=1", con);
                con.Open();
                data = Convert.ToInt32(com.ExecuteScalar());
                return data;
            }
        }
        public int wfpreroute_return(int? year = 0, string wfpno = "", int? eid = 0)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var data = 0;
                SqlCommand com = new SqlCommand(@"exec sp_BMS_WFP_returnreview_reroute '" + wfpno + "'," + year + "," + eid + "", con);
                con.Open();
                data = Convert.ToInt32(com.ExecuteScalar());
                return data;
            }
        }
        public int wfpreroute_review(string wfpno = "", int? eid = 0)
        {
            var strconcat = "";
            var email = "";
            var fullname = "";
            string pageurl_re = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var data = 0;
                SqlCommand com = new SqlCommand(@"exec sp_BMS_WFP_returnreview_review '" + wfpno + "'," + eid + "", con);
                con.Open();
                data = Convert.ToInt32(com.ExecuteScalar());


                DataTable doc_sig_reroute = new DataTable();
                string _docsign_re = "SELECT b.doc_id,b.sig_eid,a.doc_name FROM  [bacpdfsign].[dbo].[document_attach] as a inner join [bacpdfsign].[dbo].[document_signatories] as b on b.doc_id = a.doc_id where doc_name ='" + wfpno + "' + '.pdf' and b.sig_order = 2";
                doc_sig_reroute = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _docsign_re).Tables[0];

                if (doc_sig_reroute.Rows.Count > 0)
                {
                    var str_random = RandomString(10);

                    con.Close();
                    SqlCommand opnstrconcat_re = new SqlCommand(@"Select concat(" + doc_sig_reroute.Rows[0][0].ToInt() + ",',','" + str_random + "',',','" + doc_sig_reroute.Rows[0][2].ToString() + "' ,',',0,',,'," + doc_sig_reroute.Rows[0][1].ToInt() + ",',',0)", con);
                    con.Open();
                    strconcat = Convert.ToString(opnstrconcat_re.ExecuteScalar());

                    var strencryted = Rijndael.Encrypt(strconcat);

                    pageurl_re = "https://pgas.ph/dgsign/blank/jklsdhfikujdfhfgiuodfhf?data=" + Server.UrlEncode(strencryted) + "";

                    DataTable doc_signature = new DataTable();
                    string _docsign = "Select upper([username]) username,b.Firstname + ' '+ left(b.mi,1) + '. ' + b.Lastname + ' ' + isnull(b.Suffix,'') as employee,c.[Cellphoneno] from [pmis].[dbo].[eportalUser] as a inner join pmis.dbo.employee as b on b.eid=a.eid inner join [pmis].[dbo].[EmployeeFullName] as c on c.eid=b.eid where a.[eid]=" + doc_sig_reroute.Rows[0][1].ToInt() + "";
                    doc_signature = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _docsign).Tables[0];
                    email = doc_signature.Rows[0][0].ToString();
                    fullname = doc_signature.Rows[0][1].ToString();
                    var fullname_split = fullname.Split('@');

                    ////via zimbra
                    sendViaEmailOTP(email, fullname_split[0], pageurl_re, 0);

                    ////SMS- TEMPORAry disabled
                    //var smsresult = "";
                    //var msg = "Good Day "+ fullname_split[0] + ". Work and Financial Plan (WFP) document is waiting for your signature on the Dgsign App. You may access it at "+ pageurl_re + ". \n\nThis is a system-generated message. Please do not reply.";
                    //con.Close();
                    //////exec[pmis].[dbo].[nald_sp_api_sms_sender]  @recipient, @message, @isid
                    //SqlCommand comSMS = new SqlCommand(@"exec [pmis].[dbo].[nald_sp_api_sms_sender] '" + doc_signature.Rows[0][2].ToString()+ "','"+ msg + "', 1414 ", con);
                    //con.Open();
                    //smsresult = Convert.ToString(comSMS.ExecuteScalar());
                    ////SMS

                }
                return data;

            }
        }
        public ActionResult GetWFPStatus_reviewperitm([DataSourceRequest]DataSourceRequest request, int? year = 0, int? officeid = 0, int? reload = 0)
        {
            string SQL = "";
            SQL = "exec sp_BMS_WFP_statusreview_PerItem " + year + "," + officeid + "," + reload + "";
            DataTable dt = SQL.DataSet();

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;

        }
        public string ReloadWFPItems(int? officeid = 0, int? year = 0, int? reload = 0)

        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";

                    SqlCommand com = new SqlCommand(@"exec ifmis.dbo.[sp_BMS_WFP_statusreview_PerItem] " + year + "," + officeid + "," + reload + "", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    data = Convert.ToString(com.ExecuteScalar());

                    return "success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public int lgusystem()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = 0;

                    SqlCommand com = new SqlCommand(@"SELECT count([lgu]) lgu FROM [IFMIS].[dbo].[tbl_R_BMSLgu] where actioncode=1", con);
                    con.Open();
                    data = Convert.ToInt32(com.ExecuteScalar());

                    return data;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public JsonResult GetWFPRItemslist([DataSourceRequest]DataSourceRequest request, int? year = 0, int? officeid = 0, int? fundid = 0)
        {
            List<WFPrepare> prog = new List<WFPrepare>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                //if (fundid == 0) //gf
                //{
                SqlCommand com = new SqlCommand(@"exec ifmis.dbo.sp_BMS_WFP_PRItem " + officeid + "," + year + "," + fundid + "", con);
                con.Open();
                com.CommandTimeout = 0;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WFPrepare loc = new WFPrepare();
                    loc.wfpid = Convert.ToInt32(reader.GetValue(1));
                    loc.office = Convert.ToString(reader.GetValue(3));
                    loc.accountname = Convert.ToString(reader.GetValue(4));
                    loc.itemname = Convert.ToString(reader.GetValue(6));
                    loc.amount = Convert.ToDouble(reader.GetValue(7));
                    loc.wfpno = Convert.ToString(reader.GetValue(8));
                    loc.docid = Convert.ToInt32(reader.GetValue(9));
                    loc.groupid = Convert.ToInt32(reader.GetValue(10));
                    prog.Add(loc);
                }
                //}
            }
            return Json(prog.ToDataSourceResult(request));
        }
        public void downloadwfpritems(int? office = 0, int? program = 0, int? account = 0, int? tyear = 0)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                var wfpr = "";
                SqlCommand com2 = new SqlCommand(@"update [IFMIS].[dbo].[tbl_R_BMS_WFP_PR]  set [actioncode]=7,[dateandtime]= format(getdate(),'MM/dd/yyyy hh:mm:ss tt')  where  [yearof]=" + tyear + " and [actioncode]=1 and officeid=" + office + "", con);
                con.Open();
                com2.CommandTimeout = 0;
                wfpr = Convert.ToString(com2.ExecuteScalar());

            }
            //download wfp item with pr
            DataTable dtpr = db.execQuery(@"Select distinct wfpid,fund_group,wfpyear,officeid,groupid from t_wfp_details_grouping where wfpyear=" + tyear + " and officeid=" + office + "");

            for (Int32 x = 0; x < dtpr.Rows.Count; x++)
            {
                ppmp dataTablepr = new ppmp();
                dataTablepr.wfpid = Convert.ToInt32(dtpr.Rows[x]["wfpid"]);
                dataTablepr.fundid = Convert.ToInt32(dtpr.Rows[x]["fund_group"]);
                dataTablepr.OfficeID = Convert.ToInt32(dtpr.Rows[x]["officeid"]);
                dataTablepr.groupid = Convert.ToInt32(dtpr.Rows[x]["groupid"]);

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand insertItem = new SqlCommand(@"insert into [IFMIS].[dbo].[tbl_R_BMS_WFP_PR] ([accountdenomid_wfp],[fundid],[actioncode],[yearof],[dateandtime],officeid,groupid) 
                        values(" + dataTablepr.wfpid + "," + dataTablepr.fundid + ",1," + tyear + ",format(getdate(),'MM/dd/yyyy hh:mm:ss tt')," + dataTablepr.OfficeID + "," + dataTablepr.groupid + ")", con);
                    con.Open();
                    insertItem.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        public PartialViewResult proposalict(int? tyear = 0)
        {
            Session["tyear"] = tyear;
            //return PartialView("pv_ProposalSummary_report");
            //return PartialView("pv_ProposalSummaryPerOffice");
            return PartialView("pv_ProposalSummary_tabstrip2");
        }
        public JsonResult Getictsummary([DataSourceRequest]DataSourceRequest request, int? tyear = 0)
        {
            List<WFPrepare> prog = new List<WFPrepare>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"exec sp_BMS_ProposalAssessment_ICT " + tyear + "", con);
                con.Open();
                com.CommandTimeout = 0;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WFPrepare loc = new WFPrepare();
                    loc.office = Convert.ToString(reader.GetValue(0));
                    loc.accountname = reader.GetValue(1).ToString().Replace("'", "''").ToString();
                    loc.itemname = reader.GetValue(4).ToString().Replace("'", "''").ToString();
                    loc.proposeamount = Convert.ToDouble(reader.GetValue(5));
                    loc.slashamount = Convert.ToDouble(reader.GetValue(6));
                    loc.diffamount = Convert.ToDouble(reader.GetValue(7));
                    loc.isassessed = Convert.ToInt32(reader.GetValue(8));
                    loc.qty = Convert.ToInt32(reader.GetValue(9));
                    loc.amount = Convert.ToDouble(reader.GetValue(10));
                    loc.qty2 =  Convert.ToInt32(reader.GetValue(11));
                    prog.Add(loc);
                }
            }

            var jsonResult = Json(prog.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            //return Json(prog.ToDataSourceResult(request));
        }
        public JsonResult Getictsummaryperoffice([DataSourceRequest]DataSourceRequest request, int? tyear = 0, int? officeid = 0, int? showtiem = 0)
        {
            if (showtiem == 1) //show all items
            {
                if (officeid == 0)
                {
                    List<WFPrepare> prog = new List<WFPrepare>();
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {

                        SqlCommand com = new SqlCommand(@"exec sp_BMS_ProposalAssessment_ICT_summary_All " + tyear + ",0", con);
                        con.Open();
                        com.CommandTimeout = 0;
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            WFPrepare loc = new WFPrepare();
                            loc.officeid = Convert.ToInt32(reader.GetValue(0));
                            loc.office = Convert.ToString(reader.GetValue(1));
                            loc.proposeamount = Convert.ToDouble(reader.GetValue(2));
                            loc.slashamount = Convert.ToDouble(reader.GetValue(3));
                            loc.diffamount = Convert.ToDouble(reader.GetValue(4));
                            loc.yearof = Convert.ToInt32(reader.GetValue(5));
                            loc.isassessed = Convert.ToInt32(reader.GetValue(6));
                            loc.chkbox = 1;
                            prog.Add(loc);
                        }
                    }

                    var jsonResult = Json(prog.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                    //return Json(prog.ToDataSourceResult(request));
                }
                else
                {
                    List<WFPrepare> prog = new List<WFPrepare>();
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {

                        SqlCommand com = new SqlCommand(@"exec sp_BMS_ProposalAssessment_ICT_summary_All " + tyear + "," + officeid + "", con);
                        con.Open();
                        com.CommandTimeout = 0;
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            WFPrepare loc = new WFPrepare();
                            loc.officeid = Convert.ToInt32(reader.GetValue(0));
                            loc.office = Convert.ToString(reader.GetValue(1));
                            loc.accountname = reader.GetValue(2).ToString().Replace("'", "''").ToString();
                            loc.itemname = reader.GetValue(5).ToString().Replace("'", "''").ToString();
                            loc.proposeamount = Convert.ToDouble(reader.GetValue(6));
                            loc.slashamount = Convert.ToDouble(reader.GetValue(7));
                            loc.diffamount = Convert.ToDouble(reader.GetValue(8));
                            loc.amount = Convert.ToDouble(reader.GetValue(9));
                            loc.qty = Convert.ToInt32(reader.GetValue(10));
                            prog.Add(loc);
                        }
                    }

                    var jsonResult = Json(prog.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                }
            }
            else
            {
                if (officeid == 0)
                {
                    List<WFPrepare> prog = new List<WFPrepare>();
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {

                        SqlCommand com = new SqlCommand(@"exec sp_BMS_ProposalAssessment_ICT_summary " + tyear + ",0", con);
                        con.Open();
                        com.CommandTimeout = 0;
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            WFPrepare loc = new WFPrepare();
                            loc.officeid = Convert.ToInt32(reader.GetValue(0));
                            loc.office = Convert.ToString(reader.GetValue(1));
                            loc.proposeamount = Convert.ToDouble(reader.GetValue(2));
                            loc.slashamount = Convert.ToDouble(reader.GetValue(3));
                            loc.diffamount = Convert.ToDouble(reader.GetValue(4));
                            loc.yearof = Convert.ToInt32(reader.GetValue(5));
                            loc.isassessed = Convert.ToInt32(reader.GetValue(6));
                            loc.chkbox = 0;
                            prog.Add(loc);
                        }
                    }

                    var jsonResult = Json(prog.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                    //return Json(prog.ToDataSourceResult(request));
                }
                else
                {
                    List<WFPrepare> prog = new List<WFPrepare>();
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {

                        SqlCommand com = new SqlCommand(@"exec sp_BMS_ProposalAssessment_ICT_summary " + tyear + "," + officeid + "", con);
                        con.Open();
                        com.CommandTimeout = 0;
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            WFPrepare loc = new WFPrepare();
                            loc.officeid = Convert.ToInt32(reader.GetValue(0));
                            loc.office = Convert.ToString(reader.GetValue(1));
                            loc.accountname = reader.GetValue(2).ToString().Replace("'", "''").ToString();
                            loc.itemname = reader.GetValue(5).ToString().Replace("'", "''").ToString();
                            loc.proposeamount = Convert.ToDouble(reader.GetValue(6));
                            loc.slashamount = Convert.ToDouble(reader.GetValue(7));
                            loc.diffamount = Convert.ToDouble(reader.GetValue(8));
                            loc.amount = Convert.ToDouble(reader.GetValue(9));
                            loc.qty = Convert.ToInt32(reader.GetValue(10));
                            prog.Add(loc);
                        }
                    }

                    var jsonResult = Json(prog.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                }
            }
        }
        public ActionResult Proposal520Percent()
        {
            return View("pv_Proposal520Percent");
        }
        public JsonResult proposalfund([DataSourceRequest]DataSourceRequest request, int? yearof = 0, int? fundid = 0)
        {
            List<WFPrepare> prog = new List<WFPrepare>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"sp_BMS_Proposal520Percent " + yearof + "," + fundid + "", con);
                con.Open();
                com.CommandTimeout = 0;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WFPrepare prosum = new WFPrepare();
                    prosum.office = reader.GetValue(0).ToString();
                    prosum.accountname = reader.GetValue(1).ToString();
                    prosum.item = reader.GetValue(2).ToString();
                    prosum.amount = Convert.ToDouble(reader.GetValue(3));
                    prosum.qty = Convert.ToInt32(reader.GetValue(4));
                    prosum.mon = Convert.ToInt32(reader.GetValue(5));
                    prosum.totalamount = Convert.ToDouble(reader.GetValue(6));

                    prog.Add(prosum);
                }

            }
            return Json(prog.ToDataSourceResult(request));// prog;
        }
        public PartialViewResult proposalperoffice(int? param = 0)
        {
            Session["tyear"] = param;
            return PartialView("pv_ProposalSummaryPerOffice");
        }
        public PartialViewResult proposalperitem(int? param = 0)
        {
            Session["tyear"] = param;
            return PartialView("pv_ProposalSummary_report");
        }
        public JsonResult GetWFPDfppt_list([DataSourceRequest]DataSourceRequest request, int? year = 0, int? officeid = 0, int? qtr = 0, int? mode = 0)
        {
            List<WFPrepare> prog = new List<WFPrepare>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                //if (fundid == 0) //gf
                //{
                SqlCommand com = new SqlCommand(@"sp_BMS_WFPDFPPT_AllQtr " + officeid + "," + year + "," + mode + "", con);
                con.Open();
                com.CommandTimeout = 0;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WFPrepare loc = new WFPrepare();
                    loc.accountname = Convert.ToString(reader.GetValue(12));
                    loc.firstmon = Convert.ToDouble(reader.GetValue(3));
                    loc.secondmon = Convert.ToDouble(reader.GetValue(4));
                    loc.thirdmon = Convert.ToDouble(reader.GetValue(5));
                    loc.fourthmon = Convert.ToDouble(reader.GetValue(6));
                    loc.officeid = Convert.ToInt32(reader.GetValue(0));
                    loc.accountid = Convert.ToInt32(reader.GetValue(2));
                    loc.physical1 = Convert.ToString(reader.GetValue(8));
                    loc.physical2 = Convert.ToString(reader.GetValue(9));
                    loc.physical3 = Convert.ToString(reader.GetValue(10));
                    loc.physical4 = Convert.ToString(reader.GetValue(11));
                    loc.programid = Convert.ToInt32(reader.GetValue(1));
                    loc.accountid = Convert.ToInt32(reader.GetValue(2));
                    loc.yearof = Convert.ToInt32(reader.GetValue(17));
                    prog.Add(loc);
                }
                //}
            }
            return Json(prog.ToDataSourceResult(request));
        }
        public string wfpdfpptsummary(string[] transno, int? officeid = 0, int? qtr = 0, int? yearof = 0, int? mode = 0)
        {
            var retstr = "";
            var wfprevisetag = 0;
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("transno");
                var idx = 0;
                foreach (var trnno in transno)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = transno[idx];
                    dt.Rows.Add(dr);
                    idx++;
                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com = new SqlCommand("dbo.[sp_BMS_ApproveWFPDFPPT_summary]", con);
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add(new SqlParameter("@trnno", dt));
                    com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                    com.Parameters.Add(new SqlParameter("@yearof", yearof));
                    com.Parameters.Add(new SqlParameter("@officeid", officeid));
                    con.Open();
                    retstr = com.ExecuteScalar().ToString();

                    // SqlConnection connection = new SqlConnection(Common.MyConn());

                    var deviceInfo = new System.Collections.Hashtable();
                    InstanceReportSource rs = new Telerik.Reporting.InstanceReportSource();
                    var reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();
                    var typeReportSource = new Telerik.Reporting.TypeReportSource();

                    DataTable _wfprev = new DataTable();
                    string revisetag = "";
                    string _sqlQueryrev = "select dbo.fn_BMS_WFPNo (" + officeid + "," + yearof + ")";
                    _wfprev = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQueryrev).Tables[0];
                    revisetag = _wfprev.Rows[0][0].ToString();
                    if (revisetag != "") // WFP summary revision
                    {
                        wfprevisetag = 1;
                        rs.ReportDocument = new WFPDFPPTQtr_Revise(officeid, yearof, qtr, mode);
                    }
                    else
                    {
                        wfprevisetag = 0;
                        rs.ReportDocument = new WFPDFPPTQtr(officeid, yearof, qtr, mode);
                    }

                    Telerik.Reporting.Processing.RenderingResult result = reportProcessor.RenderReport("PDF", rs, null);

                    string fileName = "";
                    NetworkCredential credentials = new NetworkCredential(@"pgas.ph\rhayan.gubalane", "DomainUser1");
                    //var credential = new NetworkCredential(username, password);
                    var server_path = networkPath + "\\" + Account.UserInfo.eid;

                    DataTable _wfno = new DataTable();
                    string _sqlwfp = "";
                    string strwfpno = "";
                    string strwfpNOPDF = "";
                    string strwfpnofile = "";
                    string strwfpno_only = "";
                    var regenerate = 0;

                    //if (ooeclass != 1) // not include PS
                    //{
                    //DataTable _wfno = new DataTable();
                    //string _sqlwfp = "Select upper([wfpno]) from ifmis.dbo.tbl_T_BMSWFP_xml where [qrcode]='" + GlobalFunctions.QR_globalstr + "' and isnull(approveprint,0)=1";
                    //if (fundid == 0) // GF
                    //{
                    //    if (mode_trans == 1)// current
                    //    {
                    _sqlwfp = "exec sp_bms_WFPDFPPT_TotalAmount " + officeid + ",'" + GlobalFunctions.QR_globalstr + "'," + yearof + "";
                    //    }
                    //    else
                    //    {
                    //        _sqlwfp = "exec sp_bms_WFP_TotalAmount_Excess " + officeid + "," + accountid + ",'" + GlobalFunctions.QR_globalstr + "'," + yearof + "";
                    //    }
                    //}
                    //else //TRUST FUND
                    //{
                    //    _sqlwfp = "exec sp_bms_WFP_TotalAmount_TF " + officeid + "," + accountid + ",'" + GlobalFunctions.QR_globalstr + "'," + yearof + "";
                    //}
                    _wfno = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlwfp).Tables[0];
                    strwfpno = _wfno.Rows[0][0].ToString() + ".pdf";
                    strwfpNOPDF = "'" + _wfno.Rows[0][0].ToString() + "'";
                    strwfpno_only = _wfno.Rows[0][0].ToString();
                    strwfpnofile = _wfno.Rows[0][1].ToString();
                    //var regenerate = 0;
                    if (!result.HasErrors)
                    {
                        fileName = strwfpno;
                        //fileName = "wfp-24-1882.pdf";
                        //string path = "C:\\Users\\admin\\Documents\\Public\\WFP";// System.IO.Path.GetTempPath(); //LOCAL Connection
                        string path = networkPath;// System.IO.Path.GetTempPath();
                        string filePath = System.IO.Path.Combine(path, fileName);

                        //using (new ConnectToSharedFolder(networkPath, credentials))
                        //{
                        //string networkPath2 = @"\\192.168.2.210\pgas_attachment\digital_signature\Form" + FormID + "";
                        //string nas = @"\\192.168.2.210\pgas_attachment\bms\WFP";
                        string nas = @"r:\Web Application\iFMIS-BMS_publish";
                        //string networkPath2 = @"\\192.168.2.210\pgas_attachment\bms\WFP\" + fileName + "";
                        string networkPath2 = @"r:\Web Application\iFMIS-BMS_publish\" + fileName + "";
                        try
                        {
                            //if (Directory.Exists(nas))
                            //{
                            ViewBag.Result = "NAS Connectivity Test Successful";
                            if (System.IO.File.Exists(networkPath2))
                            {
                                System.IO.File.Delete(networkPath2);

                                //System.IO.File.CreateDirectory(networkPath2);
                                var fileList = Directory.GetDirectories(networkPath);
                                using (System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                                {
                                    fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
                                }
                                regenerate = 1;
                            }
                            else
                            {
                                //Directory.CreateDirectory(networkPath2);
                                //path = networkPath2 + "/file.pdf";
                                //pdfdoc.SaveToFile(path, FileFormat.PDF);
                                // 
                                var fileList = Directory.GetDirectories(networkPath);
                                using (System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                                {
                                    fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
                                }
                                regenerate = 0;
                            }
                            //PDF to binary file - START
                            filePathbin = @"r:\Web Application\iFMIS-BMS_publish\" + fileName + "";
                            fileData = System.IO.File.ReadAllBytes(filePathbin); // Convert PDF to binary
                                                                                 //using (SqlConnection conn = new SqlConnection(Common.MyConn()))
                                                                                 //using (SqlCommand cmd = new SqlCommand("insert into [ifmis].[dbo].[tbl_T_BMSWFP_PDFtoBinary] ([filename],[filedata],[actioncode],[tyear]) VALUES (@FileName, @FileData,@actioncode,@tyear)", conn))
                                                                                 //{
                                                                                 //    cmd.Parameters.AddWithValue("@FileName", fileName);
                                                                                 //    cmd.Parameters.AddWithValue("@actioncode", "1");
                                                                                 //    cmd.Parameters.AddWithValue("@tyear", yearof);
                                                                                 //    cmd.Parameters.Add("@FileData", SqlDbType.VarBinary).Value = fileData;

                            //    conn.Open();
                            //    cmd.ExecuteNonQuery();
                            //}
                            //PDF to binary file - END
                            //}
                            //else
                            //{
                            //    con.Close();
                            //    ViewBag.Result = "NAS Connectivity Failed";
                            //    var wfperr = "";
                            //    SqlCommand wfperrep = new SqlCommand(@"insert into [IFMIS].[dbo].[tbl_T_BMSWFP_Errorlog] ([officeid],[accountid],[particular],[userid]) " +
                            //                                                           "values(" + officeid + ",0,'NAS Connectivity Failed'," + Account.UserInfo.eid + ") ", con);
                            //    con.Open();
                            //    wfperr = Convert.ToString(wfperrep.ExecuteScalar());
                            //    retstr = "ErrorNAS";
                            //}
                        }
                        catch (Exception ex)
                        {
                            con.Close();
                            ViewBag.Result = "NAS Connectivity Failed " + ex.Message;
                            var wfperr = "";
                            SqlCommand wfperrep = new SqlCommand(@"insert into [IFMIS].[dbo].[tbl_T_BMSWFP_Errorlog] ([officeid],[accountid],[particular],[userid]) " +
                                                                                   "values(" + officeid + ",0,'" + ex.Message + "'," + Account.UserInfo.eid + ") ", con);
                            con.Open();
                            wfperr = Convert.ToString(wfperrep.ExecuteScalar());
                            retstr = "ErrorNAS";
                        }
                        //}

                        con.Close();
                        //DG Signature --- start --
                        var prep_userid = 0;
                        var prep_dephead = 0;
                        var prep_officeid = 0;
                        var sig_usertype = 0;
                        var sign_eid_gov = 0;

                        DataTable wfpsig_id = new DataTable();
                        string _sqlsign = "SELECT eid FROM  [IFMIS].[dbo].[tbl_R_BMS_WFPsignatory] where orderno=8  and yearof=" + yearof + "";
                        wfpsig_id = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlsign).Tables[0];
                        if (wfpsig_id.Rows.Count > 0)
                        {
                            sign_eid_gov = Convert.ToInt32(wfpsig_id.Rows[0][0]);
                        }

                        if (regenerate == 0)
                        {

                            DataTable prep_id = new DataTable();
                            string _sqlprep = "exec sp_BMS_WFPDFPPT_Preparer_v2 " + officeid + "," + yearof + ",'"+ strwfpno_only + "'";
                            prep_id = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlprep).Tables[0];
                            if (prep_id.Rows.Count > 0)
                            {
                                prep_userid = Convert.ToInt32(prep_id.Rows[0][4]);
                                prep_dephead = Convert.ToInt32(prep_id.Rows[0][3]);
                                sig_usertype = Convert.ToInt32(prep_id.Rows[0][0]);
                                prep_officeid = Convert.ToInt32(prep_id.Rows[0][2]);
                            }

                            var data2 = "";
                            var datareview = "";
                            var recomapproval2 = 0;

                            recomapproval2 = 2635;

                            var str_random = RandomString(10);

                            //Check if wfp to be reviewed by other end-user - start
                            string wfpreview = "";
                            var review_eid = 0;
                            DataTable _wfpreview = new DataTable();

                            //Check if wfp has to be reviewed by other end-user - end

                            ////using NAS - START
                            //SqlCommand com2 = new SqlCommand(@"insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                            //                                    //"values('"+ strwfpno + "',1,NULL,'bms/WFP','WFP - " + yearof + "',1,'5582,5582,5582,5582,5582',getdate(),5582,'','" + str_random + "',1,11,11) ", con);
                            //                                    "values('" + strwfpno + "',0,NULL,'bms/WFP','" + strwfpnofile.Replace("'", "''").ToString() + "',1,'" + prep_userid + "," + prep_dephead + ",59," + recomapproval2 + ",344880," + sign_eid_gov + "',getdate()," + prep_userid + ",'','" + str_random + "',1,11,11) ", con);
                            //con.Open();
                            //data2 = Convert.ToString(com2.ExecuteScalar());
                            ////USING NAS - END
                            //prep_userid = 5580;
                            if (wfprevisetag == 1) // WFP revision
                            {
                                using (SqlCommand cmd = new SqlCommand("insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                                                      "values(@doc_name,@doc_type,@doc_attachement,@location,@doc_description,@doc_status_id,@doc_designated,@doc_datetime,@doc_eid,@doc_datetime_update,@doc_code,@doc_signatory_type,@doc_is,@doc_type_id) ", con))
                                {
                                    if (officeid == 63) //pdrrmo
                                    {
                                        cmd.Parameters.AddWithValue("@doc_name", strwfpno);
                                        cmd.Parameters.AddWithValue("@doc_type", "0");
                                        cmd.Parameters.AddWithValue("@location", "'bms/Report'");
                                        cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                        cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                        cmd.Parameters.AddWithValue("@doc_designated", prep_userid + ",365263," + prep_dephead + "," + recomapproval2 + ",344880");
                                        cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@doc_eid", prep_userid);
                                        cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                        cmd.Parameters.AddWithValue("@doc_code", str_random);
                                        cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                        cmd.Parameters.AddWithValue("@doc_is", "11");
                                        cmd.Parameters.AddWithValue("@doc_type_id", 11);
                                        cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                                        con.Open();
                                        cmd.ExecuteNonQuery();
                                    }
                                    else if (officeid == 1)
                                    {
                                        cmd.Parameters.AddWithValue("@doc_name", strwfpno);
                                        cmd.Parameters.AddWithValue("@doc_type", "0");
                                        cmd.Parameters.AddWithValue("@location", "'bms/Report'");
                                        cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                        cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                        cmd.Parameters.AddWithValue("@doc_designated", prep_userid + ",0,303955," + recomapproval2 + ",344880");
                                        cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@doc_eid", prep_userid);
                                        cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                        cmd.Parameters.AddWithValue("@doc_code", str_random);
                                        cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                        cmd.Parameters.AddWithValue("@doc_is", "11");
                                        cmd.Parameters.AddWithValue("@doc_type_id", 11);
                                        cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                                        con.Open();
                                        cmd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@doc_name", strwfpno);
                                        cmd.Parameters.AddWithValue("@doc_type", "0");
                                        cmd.Parameters.AddWithValue("@location", "'bms/Report'");
                                        cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                        cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                        cmd.Parameters.AddWithValue("@doc_designated", prep_userid + "," + prep_dephead + "," + recomapproval2 + ",344880");
                                        cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@doc_eid", prep_userid);
                                        cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                        cmd.Parameters.AddWithValue("@doc_code", str_random);
                                        cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                        cmd.Parameters.AddWithValue("@doc_is", "11");
                                        cmd.Parameters.AddWithValue("@doc_type_id", 11);
                                        cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                                        con.Open();
                                        cmd.ExecuteNonQuery();
                                    }

                                }
                            }
                            else
                            {
                                using (SqlCommand cmd = new SqlCommand("insert into [bacpdfsign].[dbo].[document_attach] ([doc_name],[doc_type],[doc_attachement],[doc_directory],[doc_description],[doc_status_id],[doc_designated],[doc_datetime],[doc_eid],[doc_datetime_update],[doc_code],[doc_signatory_type],[doc_is],[doc_type_id]) " +
                                                                                      "values(@doc_name,@doc_type,@doc_attachement,@location,@doc_description,@doc_status_id,@doc_designated,@doc_datetime,@doc_eid,@doc_datetime_update,@doc_code,@doc_signatory_type,@doc_is,@doc_type_id) ", con))
                                {
                                    if (officeid == 63) //pdrrmo
                                    {
                                        cmd.Parameters.AddWithValue("@doc_name", strwfpno);
                                        cmd.Parameters.AddWithValue("@doc_type", "0");
                                        cmd.Parameters.AddWithValue("@location", "'bms/Report'");
                                        cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                        cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                        cmd.Parameters.AddWithValue("@doc_designated", prep_userid + ",365263," + prep_dephead + ",59," + recomapproval2 + ",344880," + sign_eid_gov);
                                        cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@doc_eid", prep_userid);
                                        cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                        cmd.Parameters.AddWithValue("@doc_code", str_random);
                                        cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                        cmd.Parameters.AddWithValue("@doc_is", "11");
                                        cmd.Parameters.AddWithValue("@doc_type_id", 11);
                                        cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                                        con.Open();
                                        cmd.ExecuteNonQuery();
                                    }
                                    else if (officeid == 1) //pgo
                                    {
                                        cmd.Parameters.AddWithValue("@doc_name", strwfpno);
                                        cmd.Parameters.AddWithValue("@doc_type", "0");
                                        cmd.Parameters.AddWithValue("@location", "'bms/Report'");
                                        cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                        cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                        cmd.Parameters.AddWithValue("@doc_designated", prep_userid + ",0,303955,59," + recomapproval2 + ",344880," + sign_eid_gov);
                                        cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@doc_eid", prep_userid);
                                        cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                        cmd.Parameters.AddWithValue("@doc_code", str_random);
                                        cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                        cmd.Parameters.AddWithValue("@doc_is", "11");
                                        cmd.Parameters.AddWithValue("@doc_type_id", 11);
                                        cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                                        con.Open();
                                        cmd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@doc_name", strwfpno);
                                        cmd.Parameters.AddWithValue("@doc_type", "0");
                                        cmd.Parameters.AddWithValue("@location", "'bms/Report'");
                                        cmd.Parameters.AddWithValue("@doc_description", strwfpnofile.Replace("'", "''").ToString());
                                        cmd.Parameters.AddWithValue("@doc_status_id", "1");
                                        cmd.Parameters.AddWithValue("@doc_designated", prep_userid + "," + prep_dephead + ",59," + recomapproval2 + ",344880," + sign_eid_gov);
                                        cmd.Parameters.AddWithValue("@doc_datetime", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@doc_eid", prep_userid);
                                        cmd.Parameters.AddWithValue("@doc_datetime_update", "");
                                        cmd.Parameters.AddWithValue("@doc_code", str_random);
                                        cmd.Parameters.AddWithValue("@doc_signatory_type", "1");
                                        cmd.Parameters.AddWithValue("@doc_is", "11");
                                        cmd.Parameters.AddWithValue("@doc_type_id", 11);
                                        cmd.Parameters.Add("@doc_attachement", SqlDbType.VarBinary).Value = fileData;

                                        con.Open();
                                        cmd.ExecuteNonQuery();
                                    }

                                }
                            }
                            con.Close();
                            var opndocid = 0;
                            var doc_sign = "";
                            var strconcat = "";
                            var email = "";
                            var fullname = "";

                            DataTable doc_attach = new DataTable();
                            string _sqldoc = "Select [doc_id],doc_designated from [bacpdfsign].[dbo].document_attach where doc_name='" + strwfpno + "' and doc_status_id=1";
                            doc_attach = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqldoc).Tables[0];
                            opndocid = Convert.ToInt32(doc_attach.Rows[0][0]);
                            doc_sign = doc_attach.Rows[0][1].ToString();
                            var doc_sign_split = doc_sign.Split(',');


                            var data3 = "";
                            con.Close();


                            SqlCommand com3 = new SqlCommand(@"exec [bacpdfsign].[dbo].[sp_bms_wfpdfpptsignatory] '" + strwfpno_only + "'," + opndocid + ",'" + str_random + "'," + yearof + "", con);
                            con.Open();
                            data3 = Convert.ToString(com3.ExecuteScalar());

                            con.Close();
                            SqlCommand opnstrconcat = new SqlCommand(@"Select concat(" + opndocid + ",',','" + str_random + "',',','" + strwfpno + "',',',0,',,'," + prep_userid + ",',',0)", con);
                            con.Open();
                            strconcat = Convert.ToString(opnstrconcat.ExecuteScalar());

                            var strencryted = Rijndael.Encrypt(strconcat);

                            //string pageurl = "https://pgas.ph/dgsign/blank/jklsdhfikujdfhfgiuodfhf?data=" + Server.UrlEncode(strencryted) + "";

                            //DataTable doc_signature = new DataTable();
                            //string _docsign = "Select upper([username]) username,b.Firstname + ' '+ left(b.mi,1) + '. ' + b.Lastname + ' ' + isnull(b.Suffix,'') as employee from [pmis].[dbo].[eportalUser] as a inner join pmis.dbo.employee as b on b.eid=a.eid where a.[eid]=" + prep_userid + "";
                            //doc_signature = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _docsign).Tables[0];
                            //email = doc_signature.Rows[0][0].ToString();
                            //fullname = doc_signature.Rows[0][1].ToString();
                            //var fullname_split = fullname.Split('@');

                            //sendViaEmailOTP(email, fullname_split[0], pageurl, 0);

                            string pageurl = "https://pgas.ph/dgsign/blank/jklsdhfikujdfhfgiuodfhf?data=" + Server.UrlEncode(strencryted) + "";
                            
                            DataTable doc_signature = new DataTable();
                        
                            string _docsign = "Select upper([username]) username,b.Firstname + ' '+ left(b.mi,1) + '. ' + b.Lastname + ' ' + isnull(b.Suffix,'') as employee,c.[Cellphoneno] from [pmis].[dbo].[eportalUser] as a inner join pmis.dbo.employee as b on b.eid=a.eid inner join [pmis].[dbo].[EmployeeFullName] as c on c.eid=b.eid where a.[eid]=" + prep_userid + "";
                            doc_signature = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _docsign).Tables[0];
                            email = doc_signature.Rows[0][0].ToString();
                            fullname = doc_signature.Rows[0][1].ToString();
                            var fullname_split = fullname.Split('@');
                            var smsresult = "";

                            var msg = "Good Day " + fullname_split[0] + ". WFP/DFPPT Summary is waiting for your signature in the Dgsign App. You may access it at " + pageurl + ". \n\nThis is a system-generated message. Please do not reply.";
                            con.Close();
                            ////exec[pmis].[dbo].[nald_sp_api_sms_sender]  @recipient, @message, @isid
                            SqlCommand comSMS = new SqlCommand(@"exec [pmis].[dbo].[nald_sp_api_sms_sender] '" + doc_signature.Rows[0][2].ToString() + "','" + msg + "', 1414 ", con);
                            con.Open();
                            smsresult = Convert.ToString(comSMS.ExecuteScalar());
                            
                        }
                        //com.ExecuteScalar().ToString();
                    }
                    else
                    {
                        con.Close();
                        var wfperr = "";
                        SqlCommand wfperrep = new SqlCommand(@"insert into [IFMIS].[dbo].[tbl_T_BMSWFP_Errorlog] ([officeid],[accountid],[particular],[userid]) " +
                                                                               "values(" + officeid + ",0,'" + result.HasErrors + "'," + Account.UserInfo.eid + ") ", con);
                        con.Open();
                        wfperr = Convert.ToString(wfperrep.ExecuteScalar());
                    }

                    con.Close();
                    var chkdocattach = 0;
                    SqlCommand opndocattach = new SqlCommand(@"Select  count([doc_name]) as docattach FROM [bacpdfsign].[dbo].[document_attach] where doc_NAME = '" + strwfpno + "'", con);
                    con.Open();
                    chkdocattach = Convert.ToInt32(opndocattach.ExecuteScalar());
                    if (chkdocattach == 0)
                    {
                        var reviewret = "";
                        con.Close();
                        SqlCommand revret = new SqlCommand(@"exec ifmis.dbo.sp_BMS_WFP_returnreview '" + strwfpno + "'," + yearof + "", con);
                        con.Open();
                        reviewret = Convert.ToString(revret.ExecuteScalar());
                        return retstr = "ErrorNAS";
                    }
                    else
                    {
                        return retstr;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public ActionResult pv_WFPDFPP_det()
        {
            return PartialView("pv_WFPDFPPT_Detail");
        }
        public ActionResult pv_WFPDFPP_rev()
        {
            return PartialView("pv_WFPDFPPT_Reviewed");
        }
        public JsonResult GetWFPDfppt_reviewed([DataSourceRequest]DataSourceRequest request, int? year = 0, int? officeid = 0, int? qtr = 0, int? mode = 0)
        {
            List<WFPrepare> prog = new List<WFPrepare>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                //if (fundid == 0) //gf
                //{
                SqlCommand com = new SqlCommand(@"ifmis.dbo.sp_BMS_WFPDFPPT_reviewed " + officeid + "," + year + "," + qtr + ",1", con);
                con.Open();
                com.CommandTimeout = 0;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WFPrepare loc = new WFPrepare();
                    loc.accountname = Convert.ToString(reader.GetValue(13));
                    loc.firstmon = Convert.ToDouble(reader.GetValue(3));
                    loc.secondmon = Convert.ToDouble(reader.GetValue(4));
                    loc.thirdmon = Convert.ToDouble(reader.GetValue(5));
                    loc.fourthmon = Convert.ToDouble(reader.GetValue(6));
                    loc.docid = Convert.ToInt32(reader.GetValue(18));
                    loc.programid = Convert.ToInt32(reader.GetValue(1));
                    loc.accountid = Convert.ToInt32(reader.GetValue(2));
                    loc.ooeid = Convert.ToInt32(reader.GetValue(16));
                    loc.wfpno= Convert.ToString(reader.GetValue(12));
                    loc.remarks = "";
                    prog.Add(loc);
                }
                //}
            }
            return Json(prog.ToDataSourceResult(request));
        }

        public DataTable GetDataTableFromAPI(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(json); // Ensure JSON format is compatible
                    return dt;
                }
                else
                {
                    throw new Exception("Failed to retrieve data from API.");
                }
            }
        }
        public void wfpstatus_disbursement(int? office = 0, int? program = 0, int? account = 0, int? tyear = 0) //fetch data fron the API
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var wfpr = "";
                SqlCommand com2 = new SqlCommand(@"update [IFMIS].[dbo].[tbl_R_BMS_WFP_PRDisburse]  set [actioncode]=7,[datetime]=[datetime] +',' + format(getdate(),'MM/dd/yyyy hh:mm:ss tt')  where [actioncode]=1", con);
                con.Open();
                com2.CommandTimeout = 0;
                wfpr = Convert.ToString(com2.ExecuteScalar());
            }

            DataTable dtpr = GetDataTableFromAPI("https://pgas.ph/epslite/public/PR_PER_PPA_Itemized"); // API

            for (Int32 x = 0; x < dtpr.Rows.Count; x++)
            {
                ppmp dataTablepr = new ppmp();
                dataTablepr.prno = Convert.ToString(dtpr.Rows[x][0]);
                dataTablepr.obrno = Convert.ToString(dtpr.Rows[x][1]);
                dataTablepr.subppaid = Convert.ToInt32(dtpr.Rows[x][5]);
                dataTablepr.OfficeID = Convert.ToInt32(dtpr.Rows[x][8]);
                dataTablepr.ProgramID = Convert.ToInt32(dtpr.Rows[x][7]);
                dataTablepr.AccountID = Convert.ToInt32(dtpr.Rows[x][6]);
                dataTablepr.pramount = Convert.ToDouble(dtpr.Rows[x][14]);
                dataTablepr.pramount = Convert.ToDouble(dtpr.Rows[x][14]);
                dataTablepr.wfpid = Convert.ToInt32(dtpr.Rows[x][2]);

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand insertItem = new SqlCommand(@"insert into [IFMIS].[dbo].[tbl_R_BMS_WFP_PRDisburse] ([prno],[obrno],[subppaid],[officeid],programid,[accountid],[pramount],[yearof],[actioncode],wfpdenomid) 
                        values('" + dataTablepr.prno + "','" + dataTablepr.obrno + "'," + dataTablepr.subppaid + "," + dataTablepr.OfficeID + "," + dataTablepr.ProgramID + "," + dataTablepr.AccountID + "," + dataTablepr.pramount + "," + tyear + ",1,'" + dataTablepr.wfpid + "')", con);
                    con.Open();
                    insertItem.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        private IActionResult NotFound()
        {
            throw new NotImplementedException();
        }
        //[HttpGet("/pdf/view/{id}")]
        public ActionResult ShowPDFBin(int id)
        {
            byte[] fileData = GetPdfFromDatabase(id); // replace with your actual method
            string fileName = "wfp_report.pdf";

            return File(fileData, "application/pdf", fileName); // File returns FileContentResult
        }

        private byte[] GetPdfFromDatabase(int id)
        {
            byte[] fileData = null;

            // Update this with your actual connection string
            //string connectionString = "Server=your_server;Database=your_db;Integrated Security=True;";
            string query = "SELECT [filedata] FROM [tbl_T_BMSWFP_PDFtoBinary] WHERE wfpbinid = " + id + "";

            using (SqlConnection conn = new SqlConnection(Common.MyConn()))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                //cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();

                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    fileData = (byte[])result;
                }
            }

            return fileData;
        }
        public int getfpaytag(int? officeid = 0)
        {

            DataTable prep_id = new DataTable();
            int tfid = 0;
            string _sqlprep = "select * FROM [IFMIS].[dbo].[tbl_R_BMS_PayProcurement_Office] where [officeid] = " + officeid + " and [actioncode]=1";
            prep_id = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlprep).Tables[0];
            if (prep_id.Rows.Count > 0)
            {
                tfid = 1;
            }
            else
            {
                tfid = 0;
            }
            return tfid;
        }
        public ActionResult GetSupplier([DataSourceRequest] DataSourceRequest request)
        {
            var offid = Account.UserInfo.Department.ToString();
            string connStr = CommonPGSql.MyPostgreSqlConn(); // your PostgreSQL connection string method

            DataTable dt = new DataTable();
            using (Npgsql.NpgsqlConnection con = new Npgsql.NpgsqlConnection(connStr))
            {
                string query = "SELECT supplierid, upper(name) as name FROM l_supplier_details where supplierid > 0 ORDER BY name;";
                using (Npgsql.NpgsqlCommand cmd = new Npgsql.NpgsqlCommand(query, con))
                {
                    con.Open();
                    using (Npgsql.NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public JsonResult GetAROdetail([DataSourceRequest]DataSourceRequest request, int? year = 0, int? officeid = 0)
        {
            List<WFPrepare> prog = new List<WFPrepare>();
            if (Account.UserInfo.UserTypeID >= 4) //lfc / super admin
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com = new SqlCommand(@"exec [sp_ARODgsign] " + officeid + "," + year + "", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        WFPrepare loc = new WFPrepare();
                        loc.officeid = Convert.ToInt32(reader.GetValue(0));
                        loc.office = Convert.ToString(reader.GetValue(1));
                        loc.arono = Convert.ToString(reader.GetValue(2));
                        loc.docid = Convert.ToInt32(reader.GetValue(3));
                        loc.trnno = Convert.ToInt32(reader.GetValue(4));
                        prog.Add(loc);
                    }

                }
            }
            return Json(prog.ToDataSourceResult(request));
            //var jsonResult = Json(prog.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            //jsonResult.MaxJsonLength = int.MaxValue;
            //return jsonResult;
        }
        public JsonResult getsb3grid([DataSourceRequest]DataSourceRequest request, int? office = 0, int? year = 0, int? mode = 0)
        {

            List<WFPrepare> prog = new List<WFPrepare>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"exec [sp_BMS_SB3] " + office + "," + year + "," + mode + "", con);
                com.CommandTimeout = 0;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WFPrepare emp = new WFPrepare();
                    emp.trnno = Convert.ToInt32(reader.GetValue(12));
                    emp.officeid = Convert.ToInt32(reader.GetValue(0));
                    emp.office = Convert.ToString(reader.GetValue(1));
                    emp.program = Convert.ToString(reader.GetValue(2));
                    emp.accountname = Convert.ToString(reader.GetValue(3));
                    emp.particular = Convert.ToString(reader.GetValue(4));
                    emp.amount = Convert.ToDouble(reader.GetValue(5).ToString());
                    emp.aipcode = Convert.ToString(reader.GetValue(6).ToString());
                    emp.ppsas = reader.GetValue(7).ToString();
                    emp.ooeid = Convert.ToInt32(reader.GetValue(8));
                    emp.ooe = Convert.ToString(reader.GetValue(9));
                    emp.fund = Convert.ToString(reader.GetValue(10));
                    emp.fundid = Convert.ToInt32(reader.GetValue(11));
                    prog.Add(emp);
                }
            }
            return Json(prog.ToDataSourceResult(request));// prog;
        }
        public JsonResult getsb3grid_report([DataSourceRequest]DataSourceRequest request, int? year = 0)
        {

            List<WFPrepare> prog = new List<WFPrepare>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"exec [sp_BMS_SupplementalB3_Report] " + year + "", con);
                com.CommandTimeout = 0;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WFPrepare emp = new WFPrepare();
                    emp.officeid = Convert.ToInt32(reader.GetValue(0));
                    emp.office = Convert.ToString(reader.GetValue(1));
                    emp.wfpno = Convert.ToString(reader.GetValue(2));
                    emp.docstatusid = Convert.ToInt32(reader.GetValue(3));
                    emp.docid = Convert.ToInt32(reader.GetValue(4));
                    emp.trnno = Convert.ToInt32(reader.GetValue(5));
                    prog.Add(emp);
                }
            }
            return Json(prog.ToDataSourceResult(request));// prog;
        }
        public string realignposting(string[] transno, int? Year = 0, int? modeid = 0)
        {
            var retstr = "";
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("transno");
                var idx = 0;
                foreach (var trnno in transno)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = transno[idx];
                    dt.Rows.Add(dr);
                    idx++;

                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand("dbo.[sp_BMS_MAFPosting]", con);
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.Parameters.Add(new SqlParameter("@trnno", dt));
                        com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                        com.Parameters.Add(new SqlParameter("@year", Year));
                        com.Parameters.Add(new SqlParameter("@status", modeid));
                        con.Open();
                        retstr = com.ExecuteScalar().ToString();
                    }
                }
                return retstr;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public ActionResult Accomplishment()
        {
            return View("vw_Accomplishment");
        }
        public string ReturnWFPDFPPT_reviewed(string[] transno, string[] wfpno, string[] programid, string[] accountid, string[] ooeid,string[] remarkval, int? yearof = 0 , int? ooeclass = 0, int? fundid = 0, int? mode_trans = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("transno");
                dt.Columns.Add("wfpno");
                dt.Columns.Add("programid");
                dt.Columns.Add("accountid");
                dt.Columns.Add("ooeid");
                dt.Columns.Add("remarkval");
                var idx = 0;
                foreach (var trnno in transno)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = transno[idx];
                    dr[1] = wfpno[idx];
                    dr[2] = programid[idx];
                    dr[3] = accountid[idx];
                    dr[4] = ooeid[idx];
                    dr[5] = remarkval[idx];
                    dt.Rows.Add(dr);
                    idx++;
                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    
                    SqlCommand com = new SqlCommand("dbo.[sp_BMS_WFPDFPPTAccount_return]", con);
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add(new SqlParameter("@trnno", dt));
                    com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
                    com.Parameters.Add(new SqlParameter("@yearof", yearof));
                    //com.Parameters.Add(new SqlParameter("@programid", programid));
                    //com.Parameters.Add(new SqlParameter("@accountid", accountid));
                    //com.Parameters.Add(new SqlParameter("@ooe", ooeclass));
                    con.Open();
                    return com.ExecuteScalar().ToString();
                    
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public JsonResult GetWFPDfppt_item([DataSourceRequest] DataSourceRequest request, int? officeid, int? programid, int? accountid, int? yearof = 0)
        {
            List<WFPrepare> realign_List = new List<WFPrepare>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@" sp_BMS_WFP_DFPPTRevision "+ officeid + "," + programid + "," + accountid + "," + yearof + ",1", con);

                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WFPrepare real = new WFPrepare();
                    real.wfpid = reader.GetInt32(0);
                    real.activityname = reader.GetValue(6).ToString();
                    real.specificactivity = reader.GetValue(7).ToString();
                    real.particular = reader.GetValue(8).ToString();
                    real.amount = Convert.ToDouble(reader.GetValue(25));
                    real.m1 = Convert.ToDouble(reader.GetValue(11));
                    real.m2 = Convert.ToDouble(reader.GetValue(12));
                    real.m3 = Convert.ToDouble(reader.GetValue(13));
                    real.m4 = Convert.ToDouble(reader.GetValue(14));
                    real.m5 = Convert.ToDouble(reader.GetValue(15));
                    real.m6 = Convert.ToDouble(reader.GetValue(16));
                    real.m7 = Convert.ToDouble(reader.GetValue(17));
                    real.m8 = Convert.ToDouble(reader.GetValue(18));
                    real.m9 = Convert.ToDouble(reader.GetValue(19));
                    real.m10 = Convert.ToDouble(reader.GetValue(20));
                    real.m11 = Convert.ToDouble(reader.GetValue(21));
                    real.m12 = Convert.ToDouble(reader.GetValue(22));
                    real.totalamount = Convert.ToDouble(reader.GetValue(26));
                    realign_List.Add(real);
                }
            }
            return Json(realign_List.ToDataSourceResult(request));
        }
        public string deletelogid(long? id = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"delete [IFMIS].[dbo].[tbl_R_BMSObrLogs] where [trnno]="+ id + " and isnull([OBRNo],'') = '' ", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return "success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public PartialViewResult pv_WFPDetail()
        {
            return PartialView("pv_WFPDetail");
        }
        public PartialViewResult pv_WFPCancel()
        {
            return PartialView("pv_WFPCancel");
        }
        public JsonResult GetWFPCancel([DataSourceRequest]DataSourceRequest request, int? office = 0, long? account = 0, int? year = 0)
        {

            List<WFPrepare> prog = new List<WFPrepare>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
               
                SqlCommand com = new SqlCommand(@"exec [sp_BMS_WFPCancel] " + office + "," + account + "," + year + "", con);
                com.CommandTimeout = 0;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WFPrepare emp = new WFPrepare();
                    emp.wfpid = Convert.ToInt32(reader.GetValue(0));
                    emp.officeid = Convert.ToInt32(reader.GetValue(1));
                    emp.programid = Convert.ToInt32(reader.GetValue(2));
                    emp.accountid = Convert.ToInt64(reader.GetValue(3));
                    emp.specificactivity = reader.GetValue(4).ToString();
                    emp.particular = reader.GetValue(5).ToString();
                    emp.amount = Convert.ToDouble(reader.GetValue(6));
                    emp.yearof = Convert.ToInt32(reader.GetValue(7));
                    emp.isPPMP= Convert.ToInt32(reader.GetValue(8));
                    prog.Add(emp);
                }
            }
            return Json(prog.ToDataSourceResult(request));// prog;
        }
        public string returnwfpitem(int? id=0, int? officeid=0, int? accountid=0, int? yearof=0, int? isPPMP=0)
        {
            try
            {
                var data = "";
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                  
                    SqlCommand com = new SqlCommand(@"sp_BMS_ReturnWFPItem " + id + "," + officeid + "," + accountid + "," + yearof + "," + isPPMP + "," + Account.UserInfo.eid + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public ActionResult AddAccountExempt([DataSourceRequest]DataSourceRequest request, int? officeid = 0, int? tyear = 0)
        {
          
            string tempStr = "SELECT a.AccountID,AccountName FROM [IFMIS].[dbo].[tbl_R_BMSProgramAccounts]  AS a "+
                            "inner join ifmis.dbo.tbl_R_BMSOfficePrograms as b on b.ProgramID = a.ProgramID and b.ActionCode = a.ActionCode and b.ProgramYear = a.AccountYear "+
                            "inner join ifmis.dbo.tbl_R_BMSOffices as c on c.OfficeID = b.OfficeID " +
                            "left join[IFMIS].[dbo].[tbl_R_BMS_WFP_AddAccountItem] as d on d.officeid=c.OfficeID and d.accountid=a.AccountID and d.yearof=a.AccountYear and d.actioncode=1" +
                            "where AccountYear="+ tyear + " and a.ActionCode=1 and isnull(AccountName,'') != ''  " +
                            "and c.OfficeID="+ officeid + " and isnull(d.accountid,0)=0 " +
                            "order by AccountName";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
              

        }
        public JsonResult GetWFPAddExempt([DataSourceRequest]DataSourceRequest request, int? office = 0, int? year = 0)
        {

            List<WFPrepare> prog = new List<WFPrepare>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"exec [sp_BMS_AddAccountItemExempt] " + office + "," + year + "", con);
                com.CommandTimeout = 0;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    WFPrepare emp = new WFPrepare();
                    emp.officeid = Convert.ToInt32(reader.GetValue(0));
                    emp.accountid = Convert.ToInt64(reader.GetValue(1));
                    emp.office = reader.GetValue(2).ToString();
                    emp.accountname = reader.GetValue(3).ToString();
                    emp.yearof = Convert.ToInt32(reader.GetValue(4));
                    prog.Add(emp);
                }
            }
            return Json(prog.ToDataSourceResult(request));// prog;
        }
        public string returnaccount( int? officeid = 0, int? accountid = 0, int? yearof = 0)
        {
            try
            {
                var data = "";
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com = new SqlCommand(@"update [tbl_R_BMS_WFP_AddAccountItem] set actioncode=4,userid=userid + ',' + '"+ Account.UserInfo.eid.ToString() + "',datetime=datetime + ','+ format(getdate(),'MM/dd/yyyy hh:mm:ss tt') where officeid=" + officeid + " and accountid="+ accountid + " and yearof="+ yearof + " and [actioncode]=1", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return "success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string AddAccountItem(int? officeid= 0, int? accountid=0, int? tyear=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                   
                    SqlCommand com = new SqlCommand(@"INSERt into [IFMIS].[dbo].[tbl_R_BMS_WFP_AddAccountItem] ([officeid] ,[accountid],[yearof],[actioncode],[userid] ,[datetime]) values ("+ officeid + ","+ accountid + ","+ tyear + ",1,"+ Account.UserInfo.eid +",format(getdate(),'MM/dd/yyyy hh:mm:ss tt'))", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return "success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}