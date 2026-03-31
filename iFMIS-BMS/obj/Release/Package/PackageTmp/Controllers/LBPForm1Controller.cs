using iFMIS_BMS.BusinessLayer.Layers;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace iFMIS_BMS.Controllers
{
    public class LBPForm1Controller : Controller
    {
        // GET: LBPForm1
        public ActionResult Index()
        {
            return View();
        }
        public string Generate1SemActualYear(int YearOf) {
            LBPForm1Layer LBPForm1Layer = new LBPForm1Layer();
            return LBPForm1Layer.Generate1SemActualYear(YearOf);
        }
        public ActionResult LoadForm1Index()
        {
            return View("pvConfigureLBPForm1Index");
        }
        public JsonResult getFundType([DataSourceRequest] DataSourceRequest request)
        {
            LBPForm1Layer Layer = new LBPForm1Layer();
            var lst = Layer.getFundType();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getParticulars([DataSourceRequest] DataSourceRequest request, int FundType, int YearOf)
        {
            LBPForm1Layer Layer = new LBPForm1Layer();
            var lst = Layer.getParticulars(FundType, YearOf);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult getLbpForm1Data([DataSourceRequest] DataSourceRequest request, int? FundType, int? YearOf)
        {
            LBPForm1Layer Layer = new LBPForm1Layer();
            var lst = Layer.getLbpForm1Data(FundType, YearOf);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public string RemoveItem(int SeriesID) {
            LBPForm1Layer Layer = new LBPForm1Layer();
            return Layer.RemoveItem(SeriesID);
        }
        public PartialViewResult ShowNewItemWindow(string Mode,string SeriesID)
        {
            ViewBag.Mode = Mode + "," + SeriesID;
            return PartialView("pvAddParticularWindow");
        }
        public JsonResult GetSelectedDataForEdit(int SeriesID)
        {
            LBPForm1Layer Layer = new LBPForm1Layer();
            var lst = Layer.GetSelectedDataForEdit(SeriesID);
            List<string> SelectedItemData = new List<string>();
            SelectedItemData.Add(lst.SeriesID.ToString()); //[0]
            SelectedItemData.Add(lst.RowNo);//[1]
            SelectedItemData.Add(lst.Particular);//[2]
            SelectedItemData.Add(lst.AccountCode);//[3]
            SelectedItemData.Add(lst.IncomeClassification);//[4]
            SelectedItemData.Add(lst.PastYear);//[5]
            SelectedItemData.Add(lst.CurrentYear);//[6]
            SelectedItemData.Add(lst.BudgetYear);//[7]
            SelectedItemData.Add(lst.OrderNo.ToString());//[8]
            SelectedItemData.Add(lst.isBold.ToString());//[9]
            SelectedItemData.Add(lst.useAmount.ToString());//[10]
            SelectedItemData.Add(lst.hasFooterTotal.ToString());//[11]
            SelectedItemData.Add(lst.useInGraph.ToString());//[12]
            SelectedItemData.Add(lst.CurrentYearActual);//[6]
            return Json(SelectedItemData, JsonRequestBehavior.AllowGet);
        }
        public string getNewOrderNo(int FundType,int YearOf) {
            LBPForm1Layer Layer = new LBPForm1Layer();
            return Layer.getNewOrderNo(FundType, YearOf);   
        }
        public string SaveNewParticular(int FundType,int YearOf,string MainGroup,string RowNo, string  Particular, string AccountCode,string IncomeClassification,
            string PastYear,string CurrentYear, string BudgetYear ,int OrderNo,bool isBold,bool isUseAmount, bool HasFooterTotal, bool UseTotalinGraph,string txtCurrentYearActual) {
                LBPForm1Layer Layer = new LBPForm1Layer();
                return Layer.SaveNewParticular(FundType,YearOf,MainGroup == "" ? "NULL" : MainGroup ,RowNo.Replace("'","''"),
                    Particular.Replace("'","''"),AccountCode.Replace("'","''"),IncomeClassification.Replace("'","''"),
                    PastYear == ""?"NULL":PastYear.Replace(",",""),CurrentYear == ""?"NULL":CurrentYear.Replace(",",""),BudgetYear == ""?"NULL":BudgetYear.Replace(",","") ,
                    OrderNo,isBold == true ? "1":"NULL",isUseAmount == true ?"1":"0",HasFooterTotal == true?"1":"NULL",UseTotalinGraph == true ?"1":"NULL", txtCurrentYearActual == "" ? "NULL" : txtCurrentYearActual.Replace(",", ""));   
        }
        public string UpdateParticular(int SeriesID,int FundType, int YearOf, string MainGroup, string RowNo, string Particular, string AccountCode, string IncomeClassification,
            string PastYear,string CurrentYear, string BudgetYear ,int OrderNo,bool isBold,bool isUseAmount, bool HasFooterTotal, bool UseTotalinGraph, string txtCurrentYearActual) {
                LBPForm1Layer Layer = new LBPForm1Layer();
                return Layer.UpdateParticular(SeriesID, FundType, YearOf, MainGroup == "" ? "NULL" : MainGroup, RowNo.Replace("'", "''"),
                    Particular.Replace("'","''"),AccountCode.Replace("'","''"),IncomeClassification.Replace("'","''"),
                    PastYear == ""?"NULL":PastYear.Replace(",",""),CurrentYear == ""?"NULL":CurrentYear.Replace(",",""),BudgetYear == ""?"NULL":BudgetYear.Replace(",","") ,
                    OrderNo,isBold == true ? "1":"NULL",isUseAmount == true ?"1":"0",HasFooterTotal == true?"1":"NULL",UseTotalinGraph == true ?"1":"NULL", txtCurrentYearActual == "" ? "NULL" : txtCurrentYearActual.Replace(",", ""));   
        }
        
        
    }
}