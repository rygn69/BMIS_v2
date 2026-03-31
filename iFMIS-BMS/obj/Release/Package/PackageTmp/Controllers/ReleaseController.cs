using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iFMIS_BMS.Reports;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Layers;
using iFMIS_BMS.Reports.Design;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using iFMIS_BMS.BusinessLayer.Connector;
using System.Web.Script.Serialization;
using System.IO;
using iFMIS_BMS.Base;

using iFMIS_BMS.BusinessLayer.Layers.Grid;
using iFMIS_BMS.BusinessLayer.Layers.Maintenance;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;
using iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation;

namespace iFMIS_BMS.Controllersf
{
    public class ReleaseController : Controller
    {
        [Authorize]
        public ActionResult MonthlyRelease()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc.ToString());
            foreach (var item in mnu)
            {
                if (item.MenuID == 6)
                {
                    status = "Authorized";
                }
            }

            if (status == "Authorized")
            {
                @ViewBag.MonthVaue = Menu.MonthValue();
                return View("pv_MonthlyRelease");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }

        public PartialViewResult ChangeGrid(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_, int? classtype)
        {
            Session["office_id"] = office_id;
            Session["program_id"] = program_id;
            Session["ooe_id"] = ooe_id;
            Session["account_id"] = account_id;
            Session["month_"] = month_;
            Session["numeric_"] = numeric_;
            Session["year_"] = year_;
            Session["classtype"] = classtype;
            //Session["Year_Of"] = Year_Of;
            if (classtype == 1) {
                return PartialView("pv_GeneralFund");
            }
            else if (classtype == 3)
            {
                return PartialView("pv_EconomicEnterprise");
            }
            else
            {
                return PartialView("pv_GeneralFund");
            }
            
            
        }

        public PartialViewResult ChangeGrid_supplement(int? sup_class)
        {

            if (sup_class == 1)
            {
                return PartialView("pv_Supplement_add");
            }
            else if (sup_class == 2)
            {
                return PartialView("pv_Supplement_tansf");
            }
            else if (sup_class == 4)
            {
                return PartialView("pv_Supplement_add");
            }
            else
            {
                return PartialView("pv_Supplement_add");
            }
        }

        public JsonResult LoadMonthlyRelease_General([DataSourceRequest] DataSourceRequest request, int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_)
        {
            Monthly_R_Layer ViewGF = new Monthly_R_Layer();
            var lst = ViewGF.Monthly_Release_GF(office_id, program_id, ooe_id, account_id, month_, numeric_, year_);

            return Json(lst.ToDataSourceResult(request));

        }
        public JsonResult LoadMonthlyRelease_Economicrtyrty([DataSourceRequest] DataSourceRequest request, int? Year)
        {
            Monthly_R_Layer ViewEE = new Monthly_R_Layer();
            var lst = ViewEE.Monthly_Release_EEqwgsgrsgrerer(Year);

            return Json(lst.ToDataSourceResult(request));
        }

        public PartialViewResult Reserve(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_)
        {
            Session["office_id"] = office_id;
            Session["program_id"] = program_id;
            Session["ooe_id"] = ooe_id;
            Session["account_id"] = account_id;
            Session["month_"] = month_;
            Session["numeric_"] = numeric_;
            Session["year_"] = year_;
           

            return PartialView("pv_Reserve_nulls");

        }

        public PartialViewResult RealignFrom(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_)
        {
            


            return PartialView("pv_RealignFrom_null");

        }
        public PartialViewResult RealignFrom_EE(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_)
        {



            return PartialView("pv_EE_RealignFrom");

        }

        public JsonResult GetOffice(int? office_id)
        {
            DropDownLayer ddl = new DropDownLayer();
            var lst = ddl.reserve_office(office_id);

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        
        public JsonResult GetPrograms2(int? propYear, int? office_ID)
        {
            ddProgramsesLayer ddl = new ddProgramsesLayer();
            var lst = ddl.gPrograms(propYear,office_ID);

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult gProgramsrealign(int? propYear, int? office_ID,int? program_from)
        {
            ddProgramsesLayer ddl = new ddProgramsesLayer();
            var lst = ddl.gProgramsrealign(propYear, office_ID, program_from);

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult gProgramsreversion(int? propYear, int? office_ID, int? program_from)
        {
            ddProgramsesLayer ddl = new ddProgramsesLayer();
            var lst = ddl.gProgramsreversion(propYear, office_ID, program_from);

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        
        public PartialViewResult RealignedTo(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_)
        {
            SessionsK(office_id, program_id, ooe_id, account_id, month_, numeric_, year_);
        

                return PartialView("pv_RealignedTo_null");

        }


        public PartialViewResult Supplemental(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_)
        {
            SessionsK(office_id, program_id, ooe_id, account_id, month_, numeric_, year_);
            Monthly_R_Layer ddl = new Monthly_R_Layer();
            @ViewBag.MonthVaue = ddl.MonthValue();

                return PartialView("pv_Supplemental_null");

 
        }
        public PartialViewResult Supplemental_add(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_)
        {
            SessionsK(office_id, program_id, ooe_id, account_id, month_, numeric_, year_);
            Monthly_R_Layer ddl = new Monthly_R_Layer();
            @ViewBag.MonthVaue = ddl.MonthValue();

            return PartialView("pv_Supplemental_add");
            
        }
        public PartialViewResult Supplemental_subtract(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_)
        {
            SessionsK(office_id, program_id, ooe_id, account_id, month_, numeric_, year_);
            Monthly_R_Layer ddl = new Monthly_R_Layer();
            @ViewBag.MonthVaue = ddl.MonthValue();

            return PartialView("pv_Supplemental_subtract");


        }

        public string Save_Reserve_Null(int? office_id, int? program_id, int? ooe_id, int? account_id, decimal percent, double money, int? year_, int? reserve_flag)
        {
                     Monthly_R_Layer NewType = new Monthly_R_Layer();
                     return NewType.save_reserve_null(office_id, program_id, ooe_id, account_id, percent, money, year_, reserve_flag);
          
        }
        public string Save_Reserve_Acc(int? office_id, int? program_id, int? ooe_id, int? account_id, decimal percent, double money, int? year_, int? reserve_flag)
        {
            Monthly_R_Layer NewType = new Monthly_R_Layer();
            return NewType.save_reserve_acc(office_id, program_id, ooe_id, account_id, percent, money, year_, reserve_flag);

        }


        public JsonResult Edit_Reserve(int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_)
        {
            Monthly_R_Layer Edit_Float = new Monthly_R_Layer();
            var lst = Edit_Float.edit_reserve(office_id, program_id, ooe_id, account_id, year_);

            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        public string Save_Reserve(int? office_id, int? program_id, int? ooe_id, int? account_id, decimal percent, double money, int? year_)
        {
            Monthly_R_Layer NewType = new Monthly_R_Layer();
            return NewType.save_reserve(office_id, program_id, ooe_id, account_id, percent, money, year_);

        }

        public string datass(int? office_id, int? program_id, int? ooe_id, int? account_id,int? year_)
        {
            Monthly_R_Layer datass = new Monthly_R_Layer();

            return datass.Available(office_id, program_id, ooe_id, account_id, year_);
           

        }
        public string datass_balat(int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_)
        {
            Monthly_R_Layer datass = new Monthly_R_Layer();

            return datass.Available_balat(office_id, program_id, ooe_id, account_id, year_);
            
        }
        
        public string supp_total( int? year_)
        {
            Monthly_R_Layer supp_total_data = new Monthly_R_Layer();


            return supp_total_data.supp_total(year_);

        }
        public string thatass( int? year_)
        {
            Monthly_R_Layer thatass = new Monthly_R_Layer();
            return thatass.ToBeRealign( year_);
        }
        public string Save_Realign_To_Null(int? office_id_realign, int? program_id_realign, int? account_id_realign, double amount_to_realign, double to_be_realign, int? year_, int? office_id_to_realign, int? program_id_to_realign, int? account_id_to_realign, double to_Amount)
        {
            Monthly_R_Layer NewType = new Monthly_R_Layer();
            return NewType.save_realign_to(office_id_realign, program_id_realign, account_id_realign, amount_to_realign, to_be_realign, year_, office_id_to_realign, program_id_to_realign, account_id_to_realign, to_Amount);

        }


        public string Save_Supplemental_Null(int? office_id_realign, int? program_id_realign, int? account_id_realign, double amount_to_realign, double to_be_realign, int? year_, int? office_id_to_realign, int? program_id_to_realign, int? account_id_to_realign, double to_Amount)
        {
            Monthly_R_Layer NewType = new Monthly_R_Layer();
            return NewType.save_realign_to(office_id_realign, program_id_realign, account_id_realign, amount_to_realign, to_be_realign, year_, office_id_to_realign, program_id_to_realign, account_id_to_realign, to_Amount);

        }


        public PartialViewResult LegalBasis()
        {

            return PartialView("pv_LegalBasis");
                      
        }
        public PartialViewResult ADD_more_()
        {

            return PartialView("pv_Supplemental_Add_Amount");

        }
        public PartialViewResult add_Notch()
        {

            return PartialView("pv_Add_Notes");

        }
        public JsonResult LegalBasis_Read([DataSourceRequest] DataSourceRequest request)
        {
            Monthly_R_Layer ViewGF = new Monthly_R_Layer();
            var lst = ViewGF.LegalBasis();

            return Json(lst.ToDataSourceResult(request));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LegalBasis_Create([DataSourceRequest] DataSourceRequest request, LegalBasis_Model legalbasis)
        {
            Monthly_R_Layer Legal_Create = new Monthly_R_Layer();
            if (legalbasis != null && ModelState.IsValid)
            {
                Legal_Create.legal_Create(legalbasis);
            }

            return Json(new[] { legalbasis }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LegalBasis_Update([DataSourceRequest] DataSourceRequest request, LegalBasis_Model legalbasis)
        {
            Monthly_R_Layer Legal_Update = new Monthly_R_Layer();
            if (legalbasis != null && ModelState.IsValid)
            {
                Legal_Update.legal_Update(legalbasis);
            }

            return Json(new[] { legalbasis }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LegalBasis_Destroy([DataSourceRequest] DataSourceRequest request, LegalBasis_Model legalbasis)
        {
            Monthly_R_Layer Legal_Destroy = new Monthly_R_Layer();
            if (legalbasis != null)
            {
                Legal_Destroy.legal_Destroy(legalbasis);
            }

            return Json(new[] { legalbasis }.ToDataSourceResult(request, ModelState));
        }


        public JsonResult GetLegalBasis()
        {
            Monthly_R_Layer ddl = new Monthly_R_Layer();
            var lst = ddl.LegalBasis_CB();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }



        public string Save_Supplement(int? legal_code, int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_, double supplement_amount, int? MonthOf)
        {
            Monthly_R_Layer supplement = new Monthly_R_Layer();
            return supplement.save_supplemement(legal_code, office_id, program_id, ooe_id, account_id, year_, supplement_amount, MonthOf);

        }

        public string Save_Supplement_reverse(int? legal_code, int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_, double supplement_amount, int? MonthOf)
        {
            Monthly_R_Layer supplement = new Monthly_R_Layer();
            return supplement.save_supplemement_reverse(legal_code, office_id, program_id, ooe_id, account_id, year_, supplement_amount, MonthOf);

        }
        public string Save_Supplement_transfere(int? legal_code, int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_, double supplement_amount, int? MonthOf)
        {
            Monthly_R_Layer supplement = new Monthly_R_Layer();
            return supplement.save_supplemement_transfere(legal_code, office_id, program_id, ooe_id, account_id, year_, supplement_amount, MonthOf);

        }
        public string Save_Supplement_reverse_Edit(int? legal_code, int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_, double supplement_amount, int? MonthOf, int? supplement_id,string transdate,int? copydatetag)
        {
            Monthly_R_Layer supplement = new Monthly_R_Layer();
            return supplement.save_supplemement_reverse_Edit(legal_code, office_id, program_id, ooe_id, account_id, year_, supplement_amount, MonthOf, supplement_id, transdate, copydatetag);

        }


        public string Save_Supplement_transfere_Edit(int? legal_code, int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_, double supplement_amount, int? MonthOf, int? supplement_id, string transdate, int? copydatetag)
        {
            Monthly_R_Layer supplement = new Monthly_R_Layer();
            return supplement.save_supplemement_transfere_Edit(legal_code, office_id, program_id, ooe_id, account_id, year_, supplement_amount, MonthOf, supplement_id, transdate, copydatetag);

        }
        public string Save_Supplement_empty(int? legal_code, int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_, double supplement_amount, int? MonthOf)
        {
            Monthly_R_Layer supplement = new Monthly_R_Layer();
            return supplement.save_supplemement_empty(legal_code, office_id, program_id, ooe_id, account_id, year_, supplement_amount, MonthOf);

        }



        public PartialViewResult Float(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_)
        {
            Session["office_id"] = office_id;
            Session["program_id"] = program_id;
            Session["ooe_id"] = ooe_id;
            Session["account_id"] = account_id;
            Session["month_"] = month_;
            Session["numeric_"] = numeric_;
            Session["year_"] = year_;

            Monthly_R_Layer ddl = new Monthly_R_Layer();
            @ViewBag.MonthVaue = ddl.MonthValue();


                return PartialView("pv_Float_null");

            

        }

        public PartialViewResult FundGrid_Load()
        {
                return PartialView("pv_FundGrid_Load");
        }

        public JsonResult Load_Float_List([DataSourceRequest] DataSourceRequest request, int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_)
        {
            Monthly_R_Layer ViewGF = new Monthly_R_Layer();
            if (ooe_id == 1) { 
            var lst = ViewGF.Float_Grid_List(office_id, program_id, ooe_id, account_id, month_, numeric_, year_);

            return Json(lst.ToDataSourceResult(request));
            }
            else if (ooe_id == 2)
            { 
                var lst = ViewGF.Float_Grid_List2(office_id, program_id, ooe_id, account_id, month_, numeric_, year_);

            return Json(lst.ToDataSourceResult(request));
            }
            else if (ooe_id == 3)
            {
                var lst = ViewGF.Float_Grid_List3(office_id, program_id, ooe_id, account_id, month_, numeric_, year_);

                return Json(lst.ToDataSourceResult(request));
            }
            else {
                var lst = ViewGF.Float_Grid_List(office_id, program_id, ooe_id, account_id, month_, numeric_, year_);

                return Json(lst.ToDataSourceResult(request));
            }

        }

        public JsonResult Edit_Flaot(int? release_float_id, int? ooe_id)
        {
            Monthly_R_Layer Edit_Float = new Monthly_R_Layer();
            
            if (ooe_id == 1)
            {
                var lst = Edit_Float.Edit_Float_ps(release_float_id, ooe_id);
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            else if (ooe_id == 2)
            {
                var lst = Edit_Float.Edit_Float_mooe(release_float_id, ooe_id);
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            else if (ooe_id == 3)
            {
                var lst = Edit_Float.Edit_Float_co(release_float_id, ooe_id);
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(JsonRequestBehavior.AllowGet);
            }
        }

        public String addFloatD(string[] release_float_id)
        {
            Monthly_R_Layer display_float = new Monthly_R_Layer();
            return display_float.FloatDisplay(release_float_id);
         }

        public PartialViewResult FundGrid_Load2(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_, int? classtype)
        {
            Session["office_id"] = office_id;
            Session["program_id"] = program_id;
            Session["ooe_id"] = ooe_id;
            Session["account_id"] = account_id;
            Session["month_"] = month_;
            Session["numeric_"] = numeric_;
            Session["year_"] = year_;

            return PartialView("pv_FloaGrid_LOAD2");

        }



        public PartialViewResult Realigned_From_EE()
        {

            return PartialView("pv_Realigned_From_EE");

        }

        public PartialViewResult Realigned_From()
        {

            return PartialView("pv_Realigned_From");

        }
        public PartialViewResult Realigned_To()
        {

            return PartialView("pv_Realigned_To");

        }

        public PartialViewResult Realignment_History()
        {

            return PartialView("pv_Realignment_History");

        }
        public PartialViewResult Realigned_To_EE()
        {

            return PartialView("pv_Realigned_To_EE");

        }

        public JsonResult Realign_From([DataSourceRequest] DataSourceRequest request, int? office_id, int? year_)
        {
            Monthly_R_Layer R_From = new Monthly_R_Layer();
            var lst = R_From.Read_From(office_id, year_);

            return Json(lst.ToDataSourceResult(request));

        }
        public JsonResult Realign_From_([DataSourceRequest] DataSourceRequest request, int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_)
        {
            Monthly_R_Layer R_From = new Monthly_R_Layer();
            var lst = R_From.Read_From_(office_id, program_id, ooe_id, account_id, year_);

            return Json(lst.ToDataSourceResult(request));

        }
        public JsonResult Realign_To([DataSourceRequest] DataSourceRequest request, int? office_id_to, int? program_id_to, int? ooe_id_to, int? account_id_to, int? year_)
        {
            Monthly_R_Layer R_To = new Monthly_R_Layer();
            var lst = R_To.Read_To(office_id_to, program_id_to, ooe_id_to, account_id_to, year_);

            return Json(lst.ToDataSourceResult(request));

        }

        //public ActionResult Realign_To_Update([DataSourceRequest] DataSourceRequest request, Monthly_Realignment_Model realing_to)
        //{
        //    Monthly_R_Layer Realign_To_Update = new Monthly_R_Layer();
        //    if (realing_to != null && ModelState.IsValid)
        //    {
        //        Realign_To_Update.realign_to_Update(realing_to);
        //    }

        //    return Json(new[] { realing_to }.ToDataSourceResult(request, ModelState));
        //}
        //public ActionResult Realign_From_Update([DataSourceRequest] DataSourceRequest request, Monthly_Realignment_Model realing_from)
        //{
        //    Monthly_R_Layer Realign_From_Update = new Monthly_R_Layer();
        //    if (realing_from != null && ModelState.IsValid)
        //    {
        //        Realign_From_Update.realign_from_Update(realing_from);
        //    }

        //    return Json(new[] { realing_from }.ToDataSourceResult(request, ModelState));
        //}



        public string Edit_Realign(int? realignment_id)
        {
            Monthly_R_Layer Edit_real = new Monthly_R_Layer();

            return Edit_real.Edit_Realign(realignment_id);


        }

        public JsonResult Edit_RealignV2(int? realignment_id)
        {
            Monthly_R_Layer Edit_reala = new Monthly_R_Layer();
            var lst = Edit_reala.Edit_RealignV2(realignment_id);

            return Json(lst, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Edit_RealignT0_n(int? realignment_id)
        {
            Monthly_R_Layer Edit_reala = new Monthly_R_Layer();
            var lst = Edit_reala.Edit_RealignTo_n(realignment_id);

            return Json(lst, JsonRequestBehavior.AllowGet);

        }
        public string delete_Realaign(int? realignment_id, int? year_)
        {

            Monthly_R_Layer supplement = new Monthly_R_Layer();
            return supplement.delete_Realaign(realignment_id, year_);

        }
        public string delete_Realaign_TO(int? realignment_id, int? year_)
        {

            Monthly_R_Layer supplement = new Monthly_R_Layer();
            return supplement.delete_Realaign_TO(realignment_id, year_);

        }

        public string Save_Realign_Edit(int? year_, double amount, int? realignment_id, double amount_dum,string transdate)
        {
            Monthly_R_Layer save_realign = new Monthly_R_Layer();
            return save_realign.save_realign_edit(year_, amount, realignment_id, amount_dum, transdate);

        }
        public string Save_Realign_Edit_TO(int? year_, double amount, int? realignment_id, double amount_dum, string transdate,int copydate)
        {
            Monthly_R_Layer save_realign = new Monthly_R_Layer();
            return save_realign.save_realign_edit_TO(year_, amount, realignment_id, amount_dum, transdate, copydate);

        }

        public string supplement_amount(int? legal_code, int? office_id, int? program_id, int? ooe_id, int? account_id, int? MonthOf, int? year_)
        {
            Monthly_R_Layer supp = new Monthly_R_Layer();

            return supp.Supplement_amount(legal_code, office_id, program_id, ooe_id, account_id, MonthOf, year_);

        }

        public JsonResult supplement_amountV2(int? legal_code, int? office_id, int? program_id, int? ooe_id, int? account_id, int? MonthOf, int? year_)
        {
            Monthly_R_Layer Edit_reala = new Monthly_R_Layer();
            var lst = Edit_reala.Supplement_amountV2(legal_code, office_id, program_id, ooe_id, account_id, MonthOf, year_);

            return Json(lst, JsonRequestBehavior.AllowGet);

        }


        public string Save_Flaot(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_, double ps_, double mooe_, double co_, double balance_ps, double balance_mooe, double balance_co, int? float_id)
        {
            Monthly_R_Layer supplement = new Monthly_R_Layer();
            return supplement.save_Floats(office_id, program_id, ooe_id, account_id, month_, numeric_, year_, ps_, mooe_, co_, balance_ps, balance_mooe, balance_co, float_id);

        }

        public string Save_FlaotV2(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_, double release_ps, double release_mooe, double release_co, double remainPS, double remainMOOE, double remainCO, string date_)
        {
            Monthly_R_Layer supplement = new Monthly_R_Layer();
            return supplement.save_FloatsV2(office_id, program_id, ooe_id, account_id, month_, numeric_, year_, release_ps, release_mooe, release_co, remainPS, remainMOOE, remainCO, date_);

        }
        public JsonResult get_balance(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_)
        {
            Monthly_R_Layer Edit_reala = new Monthly_R_Layer();
            var lst = Edit_reala.get_balance(office_id, program_id, ooe_id, account_id, month_,numeric_, year_);

            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        public JsonResult debayd_dibay(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_, double acc_available)
        {
            Monthly_R_Layer Edit_reala = new Monthly_R_Layer();
            var lst = Edit_reala.dibay_dibay(office_id, program_id, ooe_id, account_id, month_, numeric_, year_, acc_available);

            return Json(lst, JsonRequestBehavior.AllowGet);

        }


        public string Release_na(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_, double release_ps, double release_mooe, double release_co, double remainPS, double remainMOOE, double remainCO, string date_)
        {
            Monthly_R_Layer supplement = new Monthly_R_Layer();
            return supplement.Release_(office_id, program_id, ooe_id, account_id, month_, numeric_, year_, release_ps, release_mooe, release_co, remainPS, remainMOOE, remainCO, date_);

        }

        public string Release_na_EE(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_, double release_ps, double release_mooe, double release_co, double remainPS, double remainMOOE, double remainCO, int? subsIn, string date_, double income_Available, double subsidy_Available)
        {

            Monthly_R_Layer supplement = new Monthly_R_Layer();
            return supplement.Release_EE_(office_id, program_id, ooe_id, account_id, month_, numeric_, year_, release_ps, release_mooe, release_co, remainPS, remainMOOE, remainCO, subsIn, date_, income_Available, subsidy_Available);


        }

        public string Delete_Flaot(int? release_float_id=0, string code="")
        {

            Monthly_R_Layer supplement = new Monthly_R_Layer();
            return supplement.delete_float(release_float_id, code);

        }



        public PartialViewResult Release_History(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var data = 0;
                SqlCommand com = new SqlCommand(@"select dbo.fn_BMS_AccessSpecialMenu (4,"+ Account.UserInfo.eid.ToString() + ")", con);
                con.Open();
                data = Convert.ToInt32(com.ExecuteScalar());
                Session["reversioallomentlinkid"] = data;
            }

            Session["office_id"] = office_id;
            Session["program_id"] = program_id;
            Session["ooe_id"] = ooe_id;
            Session["account_id"] = account_id;
            Session["month_"] = month_;
            Session["numeric_"] = numeric_;
            Session["year_"] = year_;


            return PartialView("pv_Release_List");



        }





        public JsonResult Release_List([DataSourceRequest] DataSourceRequest request, int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_)
        {
            Monthly_R_Layer ViewGF = new Monthly_R_Layer();
            var lst = ViewGF.release_lists(office_id, program_id, ooe_id, account_id, year_);

            return Json(lst.ToDataSourceResult(request));

        }


        //------------------------------------------ Economic Enterprise --------------------------------------------
        public JsonResult LoadMonthlyRelease_Economic([DataSourceRequest] DataSourceRequest request, int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_, int? type_desu)
        {
            Monthly_R_Layer ViewGF = new Monthly_R_Layer();
            var lst = ViewGF.Monthly_Release_EE(office_id, program_id, ooe_id, account_id, month_, numeric_, year_, type_desu);

            return Json(lst.ToDataSourceResult(request));

        }


        public PartialViewResult Reserve_EE(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_)
        {
            Session["office_id"] = office_id;
            Session["program_id"] = program_id;
            Session["ooe_id"] = ooe_id;
            Session["account_id"] = account_id;
            Session["month_"] = month_;
            Session["numeric_"] = numeric_;
            Session["year_"] = year_;


            return PartialView("pv_EE_Reserve");

        }

        public PartialViewResult RealignedTo_EE(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_)
        {
            SessionsK(office_id, program_id, ooe_id, account_id, month_, numeric_, year_);


            return PartialView("pv_EE_RealignedTo");

        }

        public PartialViewResult Supplemental_EE(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_)
        {
            SessionsK(office_id, program_id, ooe_id, account_id, month_, numeric_, year_);

            Monthly_R_Layer ddl = new Monthly_R_Layer();
            @ViewBag.MonthVaue = ddl.MonthValue();

            return PartialView("pv_EE_Supplemental");


        }




        public PartialViewResult Float_EE_Main()
        {

            return PartialView("pv_Subsidy_EE_Float");

        }

        public ActionResult Float_EE()
        {
            ViewBag.UserType = Account.UserInfo.UserTypeDesc;
            Monthly_R_Layer ddl = new Monthly_R_Layer();
            @ViewBag.MonthVaue = ddl.MonthValue();

            return PartialView("pv_EE_Float", ViewBag.UserType);
        }

        public ActionResult Float_EE_Income()
        {
            ViewBag.UserType = Account.UserInfo.UserTypeDesc;
            Monthly_R_Layer ddl = new Monthly_R_Layer();
            @ViewBag.MonthVaue = ddl.MonthValue();

            return PartialView("pv_Income_EE_Float", ViewBag.UserType);
        }

        public ActionResult Float_EE_Subsidy()
        {
            ViewBag.UserType = Account.UserInfo.UserTypeDesc;
            Monthly_R_Layer ddl = new Monthly_R_Layer();
            @ViewBag.MonthVaue = ddl.MonthValue();

            return PartialView("pv_Subsidy_EE_Float", ViewBag.UserType);
        }


        public PartialViewResult Float_Grid_EE(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_, int? classtype)
        {
            Session["office_id"] = office_id;
            Session["program_id"] = program_id;
            Session["ooe_id"] = ooe_id;
            Session["account_id"] = account_id;
            Session["month_"] = month_;
            Session["numeric_"] = numeric_;
            Session["year_"] = year_;

            return PartialView("pv_EE_Float_Grid");



        }



        public PartialViewResult Annual_EE(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_)
        {
            Session["office_id"] = office_id;
            Session["program_id"] = program_id;
            Session["ooe_id"] = ooe_id;
            Session["account_id"] = account_id;
            Session["month_"] = month_;
            Session["numeric_"] = numeric_;
            Session["year_"] = year_;


            return PartialView("pv_EE_Annual");

        }

        public ActionResult Annual_Subsidy()
        {
            ViewBag.UserType = Account.UserInfo.UserTypeDesc;
            return PartialView("pv_Annual_Subsidy", ViewBag.UserType);
        }

        public ActionResult Annual_Income()
        {
            ViewBag.UserType = Account.UserInfo.UserTypeDesc;
            Monthly_R_Layer ddl = new Monthly_R_Layer();
            @ViewBag.MonthVaue = ddl.MonthValue();
            return PartialView("pv_Annual_Income", ViewBag.UserType);
        }

        // ------------------------------------ OFFICE ---------------------------------------//
        public JsonResult GetOffice_EE()
        {
            Monthly_R_Layer ddl = new Monthly_R_Layer();
            var lst = ddl.Offices_EE();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOffice_()
        {
            Monthly_R_Layer ddl = new Monthly_R_Layer();
            var lst = ddl.Offices_();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOffice_Dynamic(int? classtype_GFEE)
        {
            Monthly_R_Layer ddl = new Monthly_R_Layer();

            if (classtype_GFEE == 2) { 
                var lst = ddl.Offices_();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            else if (classtype_GFEE == 1)
            {
                var lst = ddl.Offices_SEF();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            else {
                var lst = ddl.Offices_EE();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            

          
        }
        // ------------------------------------ OFFICE ---------------------------------------//
        public JsonResult GetOffice_MO(int? classtype)
        {   
            if (classtype == 1) {
                Monthly_R_Layer ddl = new Monthly_R_Layer();

                var lst = ddl.Offices_SEF(); 

                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            else if (classtype == 2 || classtype == 0 || classtype == 8)
            {
                Monthly_R_Layer ddl = new Monthly_R_Layer();

                var lst = ddl.Offices_(); 

                return Json(lst, JsonRequestBehavior.AllowGet);
            }  
            else {
                Monthly_R_Layer ddl = new Monthly_R_Layer();

                var lst = ddl.Offices_EE();

                return Json(lst, JsonRequestBehavior.AllowGet);
            }
           
        }

        public PartialViewResult Float_Grid_EE_Sub(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_, int? classtype)
        {
            Session["office_id"] = office_id;
            Session["program_id"] = program_id;
            Session["ooe_id"] = ooe_id;
            Session["account_id"] = account_id;
            Session["month_"] = month_;
            Session["numeric_"] = numeric_;
            Session["year_"] = year_;

            return PartialView("pv_EE_Float_Sub_Grid");



        }

        public PartialViewResult Float_Grid_EE_Inc(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_, int? classtype)
        {
            Session["office_id"] = office_id;
            Session["program_id"] = program_id;
            Session["ooe_id"] = ooe_id;
            Session["account_id"] = account_id;
            Session["month_"] = month_;
            Session["numeric_"] = numeric_;
            Session["year_"] = year_;

            return PartialView("pv_EE_Float_Inc_Grid");



        }

        public JsonResult GetPrograms(int? year_, int? office_ID)
        {
            Monthly_R_Layer ddl = new Monthly_R_Layer();
            var lst = ddl.gPrograms(year_, office_ID);

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult _Account(int? year_, int? prog_id)
        {
            Monthly_R_Layer ddl = new Monthly_R_Layer();
            var lst = ddl._accounts(year_, prog_id);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult _Account_Subsidy(int? year_, int? prog_id)
        {
            Monthly_R_Layer ddl = new Monthly_R_Layer();
            var lst = ddl._account_subsidy(year_, prog_id);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Subsidy_Grid([DataSourceRequest] DataSourceRequest request, int? office_id)
        {
            Monthly_R_Layer ViewGF = new Monthly_R_Layer();

            var lst = ViewGF.Subsidy_Grid(office_id);

                return Json(lst.ToDataSourceResult(request));
           
        }


        public string Save_Subs(int? office_id, int? office_id_to, int? program_id_to, int? account_id_to, int? year_)
        {
            Monthly_R_Layer subs = new Monthly_R_Layer();
            return subs.save_subs(office_id, office_id_to, program_id_to, account_id_to, year_);

        }


        public string Delete_Subsidy(int? Binding_ID)
        {
            Monthly_R_Layer del_Subsidy = new Monthly_R_Layer();
            return del_Subsidy.delete_subsidy(Binding_ID);

        }

        public JsonResult Delete_Check(int? Binding_ID)
        {
            Monthly_R_Layer _Subsidy = new Monthly_R_Layer();
            var lst = _Subsidy.Delete_suu(Binding_ID);

            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Income_Grid([DataSourceRequest] DataSourceRequest request, int? office_id, int? month_id)
        {
            Monthly_R_Layer ViewGF = new Monthly_R_Layer();

            var lst = ViewGF.Income_Grid(office_id, month_id);

            return Json(lst.ToDataSourceResult(request));

        }

        public string Save_Incs(int? office_id, int? month_id, double amount_inc, int? year_)
        {
            Monthly_R_Layer subs = new Monthly_R_Layer();
            return subs.cake_inc_Save(office_id, month_id, amount_inc, year_);

        }
        public string Save_Incs_Edit(int? office_id, int? month_id, double amount_inc, int? year_, int? income_ID)
        {
            Monthly_R_Layer subs = new Monthly_R_Layer();
            return subs.save_incs_edit(office_id, month_id, amount_inc, year_, income_ID);

        }

        public String addFloat_Sub(string[] release_float_id)
        {
            Monthly_R_Layer display_float = new Monthly_R_Layer();
            return display_float.FloatDisplay_Sub(release_float_id);
        }
        public String addFloat_Inc(string[] release_float_id)
        {
            Monthly_R_Layer display_float = new Monthly_R_Layer();
            return display_float.FloatDisplay_Inc(release_float_id);
        }

        public string Save_Flaot_Sub(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_, double release_ps, double release_mooe, double release_co, double remainPS, double remainMOOE, double remainCO, int? indicator_, string date_, double subsidy_Available)
        {

            Monthly_R_Layer supplement = new Monthly_R_Layer();
            return supplement.save_Floats_Sub(office_id, program_id, ooe_id, account_id, month_, numeric_, year_, release_ps, release_mooe, release_co, remainPS, remainMOOE, remainCO, indicator_, date_, subsidy_Available);

        }
        public string Save_Flaot_Inc(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_, double release_ps, double release_mooe, double release_co, double remainPS, double remainMOOE, double remainCO, int? indicator_, string date_, double income_Available)
        {
            Monthly_R_Layer supplement = new Monthly_R_Layer();
            return supplement.save_Floats_Inc(office_id, program_id, ooe_id, account_id, month_, numeric_, year_, release_ps, release_mooe, release_co, remainPS, remainMOOE, remainCO, indicator_, date_, income_Available);

        }

        public JsonResult Load_Float_Sub([DataSourceRequest] DataSourceRequest request, int? office_id, int? ooe_id, int? month_, int? numeric_, int? year_)
        {
            Monthly_R_Layer ViewGF = new Monthly_R_Layer();

            var lst = ViewGF.Float_Grid_Sub(office_id, ooe_id, month_, numeric_, year_);

                return Json(lst.ToDataSourceResult(request));
           

        }
        public JsonResult Load_Float_Inc([DataSourceRequest] DataSourceRequest request, int? office_id, int? ooe_id, int? month_, int? numeric_, int? year_)
        {
            Monthly_R_Layer ViewGF = new Monthly_R_Layer();

            var lst = ViewGF.Float_Grid_Inc(office_id, ooe_id, month_, numeric_, year_);

            return Json(lst.ToDataSourceResult(request));


        }
        public string sub_available(int? office_id, int? year_)
        {
            Monthly_R_Layer datass = new Monthly_R_Layer();

            return datass.Available_Sub(office_id, year_);
        }

        public string inc_available(int? office_id, int? year_)
        {
            Monthly_R_Layer datass = new Monthly_R_Layer();

            return datass.Available_Inc(office_id, year_);
        }

        public JsonResult Accounts_DD(int? propYear, int? prog_id)
        {
            Monthly_R_Layer ddl = new Monthly_R_Layer();
            var lst = ddl.Account_DP(propYear, prog_id);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Account_Mos_From(int? office_ID_from, int? prog_id_from,  int? year_, int? office_ID_to, int? prog_id_to,  int? account_id_to)
        {
            Monthly_R_Layer ddl = new Monthly_R_Layer();
            var lst = ddl.Account_Mos_From(office_ID_from, prog_id_from, year_, office_ID_to, prog_id_to, account_id_to);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Account_Mos_To(int? office_ID_from, int? prog_id_from,  int? account_id_from, int? year_, int? office_ID_to, int? prog_id_to)
        {
            Monthly_R_Layer ddl = new Monthly_R_Layer();
            var lst = ddl.Account_Mos_TO(office_ID_from, prog_id_from, account_id_from, year_, office_ID_to, prog_id_to);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RealignAccount_To(int? office_ID_from, int? prog_id_from, int? account_id_from, int? year_, int? office_ID_to, int? prog_id_to,int? ooe_id_from)
        {
            Monthly_R_Layer ddl = new Monthly_R_Layer();
            var lst = ddl.RealignAccount_To(office_ID_from, prog_id_from, account_id_from, year_, office_ID_to, prog_id_to, ooe_id_from);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReversionAccount_To(int? office_ID_from, int? prog_id_from, int? account_id_from, int? year_, int? office_ID_to, int? prog_id_to)
        {
            Monthly_R_Layer ddl = new Monthly_R_Layer();
            var lst = ddl.ReversionAccount_To(office_ID_from, prog_id_from, account_id_from, year_, office_ID_to, prog_id_to);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public string Release_naEE(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_, double release_ps, double release_mooe, double release_co, double total_debayd, double total_debaydm, double total_debaydc)
        {
            Monthly_R_Layer supplement = new Monthly_R_Layer();
            return supplement.Release_EE(office_id, program_id, ooe_id, account_id, month_, numeric_, year_, release_ps, release_mooe, release_co, total_debayd, total_debaydm, total_debaydc);

        }

        public string Delete_Flaot_Sub(int? release_float_id)
        {
            Monthly_R_Layer supplement = new Monthly_R_Layer();
            return supplement.delete_float_sub(release_float_id);

        }
        public string Delete_Flaot_Inc(int? release_float_id)
        {
            Monthly_R_Layer supplement = new Monthly_R_Layer();
            return supplement.delete_float_inc(release_float_id);

        }
        public JsonResult Load_Supp([DataSourceRequest] DataSourceRequest request, int? LegalCode, int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? year_)
        {
            Monthly_R_Layer ViewGF = new Monthly_R_Layer();

            var lst = ViewGF.Supplement_Grid(LegalCode, office_id, program_id, ooe_id, account_id, month_, year_);

                return Json(lst.ToDataSourceResult(request));
          
        }

        public JsonResult Load_Supp_transf([DataSourceRequest] DataSourceRequest request,int? LegalCode=0,int? office_id=0,int? program_id=0,int? ooe_id=0, int? account_id=0,int? month_=0, int? year_=0)
        {
            Monthly_R_Layer ViewGF = new Monthly_R_Layer();

            var lst = ViewGF.Supplement_Grid_transf(LegalCode, office_id,program_id, ooe_id, account_id, month_, year_);

            return Json(lst.ToDataSourceResult(request));

        }
        public JsonResult Load_Supp_rever([DataSourceRequest] DataSourceRequest request,int? LegalCode=0,int? office_id=0,int? program_id=0,int? ooe_id=0,int? account_id=0,int? month_=0, int? year_=0)
        {
            Monthly_R_Layer ViewGF = new Monthly_R_Layer();

            var lst = ViewGF.Supplement_Grid_rever(LegalCode, office_id,program_id, ooe_id, account_id, month_, year_);

            return Json(lst.ToDataSourceResult(request));

        }

    

        public string income_dedet(int? income_id)
        {
            Monthly_R_Layer dedet = new Monthly_R_Layer();
            return dedet.dedet_income(income_id);
        }

        public JsonResult read_reserve([DataSourceRequest] DataSourceRequest request, int? office_id, int? account_id,int? year_)
        {
            Monthly_R_Layer R_From = new Monthly_R_Layer();
            var lst = R_From.read_reserve(office_id, account_id, year_);

            return Json(lst.ToDataSourceResult(request));

        }

        public string delete_reserve(int? reserve_id)
        {

            Monthly_R_Layer supplement = new Monthly_R_Layer();
            return supplement.delete_reserve(reserve_id);

        }
        public void SessionsK(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_)
        {

            if (office_id == null || office_id == 0)
            {
                Session["office_id"] = "";
                Session["program_id"] = "";
                Session["ooe_id"] = "";
                Session["account_id"] = "";
                Session["month_"] = month_;
                Session["numeric_"] = numeric_;
                Session["year_"] = year_;
            }
            else if ((office_id != null || office_id != 0) && (program_id == 0 || program_id == null) )
            {
                Session["office_id"] = office_id;
                Session["program_id"] = "";
                Session["ooe_id"] = "";
                Session["account_id"] = "";
                Session["month_"] = month_;
                Session["numeric_"] = numeric_;
                Session["year_"] = year_;
            }
            else if ((office_id != null || office_id != 0) && (program_id != 0 || program_id != null) && (ooe_id == 0 || ooe_id == null))
            {
                Session["office_id"] = office_id;
                Session["program_id"] = program_id;
                Session["ooe_id"] = "";
                Session["account_id"] = "";
                Session["month_"] = month_;
                Session["numeric_"] = numeric_;
                Session["year_"] = year_;
            }
            else if ((office_id != null || office_id != 0) && (program_id != 0 || program_id != null) && (ooe_id != 0 || ooe_id != null) && (account_id == 0 || account_id == null))
            {
                Session["office_id"] = office_id;
                Session["program_id"] = program_id;
                Session["ooe_id"] = ooe_id;
                Session["account_id"] = "";
                Session["month_"] = month_;
                Session["numeric_"] = numeric_;
                Session["year_"] = year_;
            }
            else
            {
                Session["office_id"] = office_id;
                Session["program_id"] = program_id;
                Session["ooe_id"] = ooe_id;
                Session["account_id"] = account_id;
                Session["month_"] = month_;
                Session["numeric_"] = numeric_;
                Session["year_"] = year_;
            }
        }

        public JsonResult Edit_Supplement_(int? supplementalbudget_id)
        {
            Monthly_R_Layer Edit_reala = new Monthly_R_Layer();
            var lst = Edit_reala.Edit_Supplement_(supplementalbudget_id);

            return Json(lst, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Edit_Supplement_reverse(int? supplementalreverse_id)
        {
            Monthly_R_Layer Edit_reala = new Monthly_R_Layer();
            var lst = Edit_reala.Edit_Supplement_Reverse(supplementalreverse_id);

            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        public JsonResult get_Obli_Remain(int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_)
        {
            Monthly_R_Layer Edit_reala = new Monthly_R_Layer();
            var lst = Edit_reala._Obli_Remain(office_id, program_id, ooe_id, account_id, year_);

            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        public string Edit_release(int? release_id)
        {
            Monthly_R_Layer datass = new Monthly_R_Layer();

            return datass.Edit_Release_(release_id);


        }
        public JsonResult Edit_Supplement_transfere(int? supplementaltransfere_id)
        {
            Monthly_R_Layer Edit_reala = new Monthly_R_Layer();
            var lst = Edit_reala.Edit_Supplement_Transfere(supplementaltransfere_id);

            return Json(lst, JsonRequestBehavior.AllowGet);

        }
        public string delete_Supplement(int? supplementalbudget_id)
        {

            Monthly_R_Layer supplement = new Monthly_R_Layer();
            return supplement.delete_Supplement_(supplementalbudget_id);

        }
        public string delete_Supplement_(int? supplementalbudget_id)
        {

            Monthly_R_Layer supplement = new Monthly_R_Layer();
            return supplement.delete_Supplement_(supplementalbudget_id);

        }


        public string delete_Supplement_Reverse(int? supplementalreverse_id)
        {

            Monthly_R_Layer supplement = new Monthly_R_Layer();
            return supplement.delete_Supplement_Reverse(supplementalreverse_id);

        }
        public string delete_Supplement_Transfere(int? supplementaltransfere_id)
        {

            Monthly_R_Layer supplement = new Monthly_R_Layer();
            return supplement.delete_Supplement_Transfere(supplementaltransfere_id);

        }
        public string Edit_Supplement(int? legal_code, int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_, double supplement_amount, int? MonthOf, int? supplement_id)
        {
            Monthly_R_Layer supplement = new Monthly_R_Layer();
            return supplement.edit_supplemement(legal_code, office_id, program_id, ooe_id, account_id, year_, supplement_amount, MonthOf, supplement_id);

        }

        public string amount_Release(int? release_id, int? ooe_id)
        {
            Monthly_R_Layer mount_release = new Monthly_R_Layer();

            return mount_release.Get_amount_Release(release_id, ooe_id);


        }
        public string delete_Release(int? release_id, int? ooe_id)
        {

            Monthly_R_Layer delete_Rel = new Monthly_R_Layer();
            return delete_Rel.remove_Release_(release_id, ooe_id);

        }

        public string get_Expense( int? program_id, int? account_id, int? year_)
        {
            Monthly_R_Layer exp = new Monthly_R_Layer();

            return exp.get_Expenses(program_id, account_id, year_);


        }

        public string get_TotalRelease(int? office_id, int? program_id, int? account_id, int? year_)
        {
            Monthly_R_Layer get_release = new Monthly_R_Layer();

            return get_release.Get_Release_(office_id, program_id, account_id, year_);


        }
        public string get_TotalReleaseAcc(int? office_id, int? program_id, int? account_id, int? year_,int? month_)
        {
            Monthly_R_Layer get_release = new Monthly_R_Layer();

            return get_release.Get_ReleaseAcc(office_id, program_id, account_id, year_, month_);


        }

        public string get_Available_Inc(int? office_id, int? year_)
        {
            Monthly_R_Layer get_inc = new Monthly_R_Layer();

            return get_inc.Available_INC(office_id, year_);


        }
        public string get_incAmount(int? income_id)
        {
            Monthly_R_Layer get_inc = new Monthly_R_Layer();

            return get_inc.income_amount_(income_id);

        }

        public string delete_Income(int? income_id)
        {

            Monthly_R_Layer supplement = new Monthly_R_Layer();
            return supplement.delete_Income_(income_id);

        }

        public string delete_Release_EE(int? release_id, int? ooe_id, int? subsIn)
        {

            Monthly_R_Layer delete_Rel = new Monthly_R_Layer();
            return delete_Rel.remove_Release_EE(release_id, ooe_id, subsIn);

        }

        public string get_AccountName(int? program_id,int? ooe_id, int? account_id, int? year_)
        {
            Monthly_R_Layer exp = new Monthly_R_Layer();

            return exp.get_AccountName(program_id, ooe_id, account_id, year_);


        }
        public string get_AccountName_realignment(int? program_id, int? account_id, int? year_)
        {
            Monthly_R_Layer exp = new Monthly_R_Layer();

            return exp.get_AccountName_realignment(program_id, account_id, year_);


        }
        public ActionResult RegistryofAllotments()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc.ToString());
            foreach (var item in mnu)
            {
                if (item.MenuID == 12028)
                {
                    status = "Authorized";
                }
            }

            if (status == "Authorized")
            {
                @ViewBag.MonthVaue = Menu.MonthValue();
                return View("pv_RegistryofAllotments");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }
        public ActionResult LBEF()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc.ToString());
            foreach (var item in mnu)
            {
                if (item.MenuID == 12029)
                {
                    status = "Authorized";
                }
            }

            if (status == "Authorized")
            {
                @ViewBag.MonthVaue = Menu.MonthValue();
                return View("pv_LBEF");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }

        public ActionResult Breakdown_Obligations ()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc.ToString());
            foreach (var item in mnu)
            {
                if (item.MenuID == 12030)
                {
                    status = "Authorized";
                }
            }

            if (status == "Authorized")
            {
                @ViewBag.MonthVaue = Menu.MonthValue();
                return View("pv_Breakdown_Obligations");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }
        public ActionResult TotalObligation()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc.ToString());
            foreach (var item in mnu)
            {
                if (item.MenuID == 12031)
                {
                    status = "Authorized";
                }
            }

            if (status == "Authorized")
            {
                @ViewBag.MonthVaue = Menu.MonthValue();
                return View("pv_TotalObligation");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }

        public ActionResult TotalRelease()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc.ToString());
            foreach (var item in mnu)
            {
                if (item.MenuID == 12032)
                {
                    status = "Authorized";
                }
            }

            if (status == "Authorized")
            {
                @ViewBag.MonthVaue = Menu.MonthValue();
                return View("pv_TotalRelease");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }


        public ActionResult SAAO()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc.ToString());
            foreach (var item in mnu)
            {
                if (item.MenuID == 12033)
                {
                    status = "Authorized";
                }
            }

            if (status == "Authorized")
            {
                @ViewBag.MonthVaue = Menu.MonthValue();
                return View("pv_SAAO");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }

        public ActionResult SAAOsample()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc.ToString());
            foreach (var item in mnu)
            {
                if (item.MenuID == 12033)
                {
                    status = "Authorized";
                }
            }

            if (status == "Authorized")
            {
                @ViewBag.MonthVaue = Menu.MonthValue();
                return View("pv_SAAO");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }

        public ActionResult Projected_Revenues()
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
                @ViewBag.MonthVaue = Menu.MonthValue();
                return View("pv_Projected_Revenues");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }

        public JsonResult Projectd_Revenues([DataSourceRequest] DataSourceRequest request)
        {
            Monthly_R_Layer PR = new Monthly_R_Layer();
            var lst = PR.Projected_Revenues();

            return Json(lst.ToDataSourceResult(request));

        }

        public JsonResult Projectd_Revenues_2([DataSourceRequest] DataSourceRequest request)
        {
            Monthly_R_Layer PR = new Monthly_R_Layer();
            var lst = PR.Projected_Revenues_2();

            return Json(lst.ToDataSourceResult(request));

        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Projectd_Revenues_Create([DataSourceRequest] DataSourceRequest request, Projected_RevenuesModel pr)
        {
            Monthly_R_Layer PR_Create = new Monthly_R_Layer();
            if (pr != null && ModelState.IsValid)
            {
                PR_Create.PR_Create(pr);
            }

            return Json(new[] { pr }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Projectd_Revenues_Create_2([DataSourceRequest] DataSourceRequest request, Projected_RevenuesModel pr)
        {
            Monthly_R_Layer PR_Create = new Monthly_R_Layer();
            if (pr != null && ModelState.IsValid)
            {
                PR_Create.PR_Create_2(pr);
            }

            return Json(new[] { pr }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Projectd_Revenues_Update([DataSourceRequest] DataSourceRequest request, Projected_RevenuesModel pr)
        {
            Monthly_R_Layer Legal_Update = new Monthly_R_Layer();
            if (pr != null && ModelState.IsValid)
            {
                Legal_Update.pr_Update(pr);
            }

            return Json(new[] { pr }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Projectd_Revenues_Update_2([DataSourceRequest] DataSourceRequest request, Projected_RevenuesModel pr)
        {
            Monthly_R_Layer Legal_Update = new Monthly_R_Layer();
            if (pr != null && ModelState.IsValid)
            {
                Legal_Update.pr_Update_2(pr);
            }

            return Json(new[] { pr }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LegalBasis_Destroy([DataSourceRequest] DataSourceRequest request, Projected_RevenuesModel pr)
        {
            Monthly_R_Layer Legal_Destroy = new Monthly_R_Layer();
            if (pr != null)
            {
                Legal_Destroy.pr_Destroy(pr);
            }

            return Json(new[] { pr }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LegalBasis_Destroy_2([DataSourceRequest] DataSourceRequest request, Projected_RevenuesModel pr)
        {
            Monthly_R_Layer Legal_Destroy = new Monthly_R_Layer();
            if (pr != null)
            {
                Legal_Destroy.pr_Destroy_2(pr);
            }

            return Json(new[] { pr }.ToDataSourceResult(request, ModelState));
        }




        public string numeric_des(int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_, int? month_)
        {
            Monthly_R_Layer datass = new Monthly_R_Layer();

            return datass.numeric_desuni(office_id, program_id, account_id, year_, month_);
            
        }
        public string numeric_desNon(int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_, int? month_)
        {
            Monthly_R_Layer datass = new Monthly_R_Layer();

            return datass.numeric_desuniNon(office_id, program_id, account_id, year_, month_);


        }
        public string getMax(int? office_id, int? month_, int? year_)
        {
            Monthly_R_Layer datass = new Monthly_R_Layer();

            return datass.getMax_desu(office_id, month_, year_);


        }



        public ActionResult BOI()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc.ToString());
            foreach (var item in mnu)
            {
                if (item.MenuID == 12035)
                {
                    status = "Authorized";
                }
            }

            if (status == "Authorized")
            {
                @ViewBag.MonthVaue = Menu.MonthValue();
                return View("pv_BOI_excess");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }
        public ActionResult Realignment_Report()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeDesc.ToString());
            foreach (var item in mnu)
            {
                if (item.MenuID == 12044)
                {
                    status = "Authorized";
                }
            }

            if (status == "Authorized")
            {
                @ViewBag.MonthVaue = Menu.MonthValue();
                return View("pv_Realignment_Report");
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }
        public JsonResult FundType_Desu()
        {
            Monthly_R_Layer ddl = new Monthly_R_Layer();
            var lst = ddl.FundType_Desu();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Changed_To(int? fund_type)
        {
            Monthly_R_Layer ddl = new Monthly_R_Layer();
            var lst = ddl.Changed_To(fund_type);

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public string Save_supplement_source(int? supplement_source_ID, string supplement_souce, int? month_, int? year_, double source_amount)
        {
            Monthly_R_Layer NewTrunds = new Monthly_R_Layer();
            if (supplement_source_ID == 0 || supplement_source_ID == null)
            {
                return NewTrunds.addNewSupplement(supplement_souce, month_, year_, source_amount);
            }
            else
            {
                return NewTrunds.addSupplement_Amount(supplement_source_ID, supplement_souce, month_, year_, source_amount);
            }
        }
        public JsonResult get_Supplement_Source()
        {
            Monthly_R_Layer ddl = new Monthly_R_Layer();
            var lst = ddl.GetSourcesSupplement();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public string Edit_rel(int? ID_, double EDIT_Amount, int? ooe_id, int progid, long accountid, int yearof, string DateEntered,int EDIT_Batch,string releasedate,int? relcopydatetag)
        {
            Monthly_R_Layer NewType = new Monthly_R_Layer();
            return NewType.release_edit__(ID_, EDIT_Amount, ooe_id, progid, accountid, yearof, DateEntered, EDIT_Batch, releasedate, relcopydatetag);

        }
        public ActionResult ReleaseCAF()
        {
            return View("pv_CAF");
        }
        public JsonResult CAFComputation(int? year=0,int? Progid=0, long? accountid=0,long? ppaid=0,int? chargeofficeid=0)
        {

            CurrentControl_Model data = new CurrentControl_Model();
            DataTable _dt = new DataTable();
            string _sqlQuery = "exec sp_BMS_CAPComputation "+ year + ","+ Progid + ","+ accountid + ","+ ppaid + ","+ chargeofficeid + "";
            _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
            try
            {
                data.Appropriation = Convert.ToDouble(_dt.Rows[0][2]);
                data.AllotedAmount = Convert.ToDouble(_dt.Rows[0][3]);
                data.BalanceAllotment = Convert.ToDouble(_dt.Rows[0][4]);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public string InsertCAF(int? office=0,int? program=0,long? account=0,double amount=0,int? month=0,int? year=0 ,long? ppaid=0,int? charge_office=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec dbo.sp_BMS_CAFIssuance " + Account.UserInfo.eid.ToString() + "," + office + ", " + program + "," + account + ", " + amount + "," + month + "," + year + "," + ppaid + "," + charge_office + "", con);
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
       
        public ActionResult ViewCAFtransaction([DataSourceRequest]DataSourceRequest request, int? program = 0, long? account = 0, long? ppaid = 0,int? charge_office=0, int? year = 0,int? caftrans=0)
        {
            string SQL = "";
            SQL = "exec sp_BMS_CAFView "+ program + ","+ account + ","+ ppaid + ","+ charge_office  + ","+ year + ","+ caftrans  + "";
            DataTable dt = SQL.DataSet();

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }
        public string UpdateReleaseCAF(int? transid=0,double relamount=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec dbo.sp_BMS_CAFUpdate " + Account.UserInfo.eid.ToString() + "," + relamount + ", " + transid + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
        public string DeleteReleaseCAF(int? transid = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    //SqlCommand com = new SqlCommand(@"update ifmis.dbo.[tbl_R_BMS_CAF] set  [UserID]=[UserID] + ',' + '" + Account.UserInfo.eid.ToString() + "',actioncode=4,[DateTimeEntered]=[DateTimeEntered] +','+ format(cast(getdate() as datetime),'MM/dd/yyyy hh:mm:ss tt') where [caf_id]=" + transid  + "", con);
                    SqlCommand com = new SqlCommand(@"[sp_BMS_CAFDelete] " + transid + ",'" + Account.UserInfo.eid.ToString() + "'", con);
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
        public ActionResult GetCAFHistory([DataSourceRequest]DataSourceRequest request, int? year = 0,int? epaid=0)
        {
            if (epaid == 0)
            {
                string tempStr = "select cafno FROM   [IFMIS].[dbo].[tbl_R_BMS_CAF] where YearOf=" + year + " and isnull(cafno,'') <> '' and actioncode=1 group by cafno order by cast(right(cafno,len(cafno)-patindex('%-%',cafno)) as int) desc";
                DataTable dt = tempStr.DataSet();
                var result = new ContentResult();
                result.Content = SerializeDT.DataTableToJSON(dt);
                result.ContentType = "application/json";
                return result;
            }
            else
            {
                string tempStr = "select cafno FROM   [IFMIS].[dbo].[tbl_R_BMS_CAF_EPA] where YearOf=" + year + " and isnull(cafno,'') <> '' and actioncode=1 group by cafno order by cast(right(cafno,len(cafno)-patindex('%-%',cafno)) as int) desc";
                DataTable dt = tempStr.DataSet();
                var result = new ContentResult();
                result.Content = SerializeDT.DataTableToJSON(dt);
                result.ContentType = "application/json";
                return result;
            }
        }
        public string GenerateCafNo(int? year=0,int? office=0, int? program=0,long? account=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    //original
                    //SqlCommand com = new SqlCommand(@"exec dbo.sp_BMS_CAFNo " + year +","+ office + ","+ program +","+ account + "," + Account.UserInfo.eid.ToString() + "", con);
                    //change into this
                    SqlCommand com = new SqlCommand(@"exec dbo.[sp_BMS_CAFNo_Generate] " + year + "," + office + "," + program + "," + account + "," + Account.UserInfo.eid.ToString() + "", con);
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
        public JsonResult GetOfficeAllFunds(int? officeaccounts, int? year)
        {
            if (officeaccounts == 0)
            {
                Monthly_R_Layer ddl = new Monthly_R_Layer();
                var lst = ddl.GetAllAccount(officeaccounts, year);
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Monthly_R_Layer ddl = new Monthly_R_Layer();
                var lst = ddl.GetOfficeAllFunds(officeaccounts, year);
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult AddCertStatement(string cafno="",int? year=0,string issuedate="",int? applynewdate=0)
        {
            //@ViewBag.MonthVaue
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec dbo.[sp_BMS_CAFTotalinWords] '" + cafno + "'," + year + ",'" + issuedate + "'," + applynewdate + ",'' ", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    @ViewBag.certificate= data;
                }
            }
            catch (Exception ex)
            {
                @ViewBag.certificate = "";
            }
            return PartialView("pv_CAFCertification");
        }
        public string releasefloat (int officeid, long accountid, int yearof)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec dbo.sp_BMS_ReleaseFloat " + officeid + ", " + accountid + "," + yearof + "," + Account.UserInfo.eid.ToString() + "", con);
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
        public ActionResult ReleaseCom()
        {
            return View("pv_Commitment");
        }
        public JsonResult CommComp(int year, int Progid, long accountid,int excess)
        {

            CurrentControl_Model data = new CurrentControl_Model();
            DataTable _dt = new DataTable();
            string _sqlQuery = "exec [sp_BMS_CommComputation] " + year + "," + Progid + "," + accountid + "," + excess + "";
            _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
            try
            {
                data.Appropriation = Convert.ToDouble(_dt.Rows[0][2]);
                data.AllotedAmount = Convert.ToDouble(_dt.Rows[0][3]);
                data.BalanceAllotment = Convert.ToDouble(_dt.Rows[0][4]);
                data.totreserveapp = Convert.ToDouble(_dt.Rows[0][5]);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public string InsertCommitment(int? office = 0, int? program = 0, long? account = 0, string particular = "", double amount = 0, int? year = 0, int? excess = 0, string transcode="",long? municipal=0,long? barangay=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec dbo.sp_BMS_Commitment " + Account.UserInfo.eid.ToString() + "," + office + ", " + program + "," + account + ",'"+ particular  + "', " + amount + "," + year + "," + excess + ",'"+ transcode + "',"+ municipal + ","+ barangay + "", con);
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
        public ActionResult CommHistory([DataSourceRequest]DataSourceRequest request, int? office = 0, int? program = 0, long? account = 0, int? year = 0, int? excess = 0)
        {
            string SQL = "";
            SQL = "exec sp_BMS_CommitmentHistory " + office + ", " + program + "," + account + "," + year + "," + excess + "";
            DataTable dt = SQL.DataSet();

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }
        public string DeleteComm(int? transid = 0,string transcode="")
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@" sp_BMS_DeleteCommitment " + transid + ",'" + Account.UserInfo.eid.ToString() +"','"+ transcode + "'", con);
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
        public PartialViewResult comdetails(int? officeid=0, int? programid=0, long? accountid=0,string particular="",int? muncode=0, long? brgycode=0, double Amount_release=0,string code="",int? YearOf=0)
        {
            Session["officeid"] = officeid;
            Session["programid"] = programid;
            Session["accountid"] = accountid;
            Session["particular"] = particular;
            Session["muncode"] = muncode;
            Session["brgycode"] = brgycode;
            Session["Amount_release"] = Amount_release;
            Session["code"] = code;
            Session["YearOf"] = YearOf;
            return PartialView("pv_Commitmentdetails");
        }
        public string releasefloatselected(string[] release_float_id)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Releaseid");
             
                var idx = 0;
                foreach (var relid in release_float_id)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = release_float_id[idx];
                    dt.Rows.Add(dr);
                    idx++;
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("dbo.sp_BMS_ReleaseFloatSelected", con);
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add(new SqlParameter("@releaseid", dt));
                    com.Parameters.Add(new SqlParameter("@userid", Account.UserInfo.eid));
                    con.Open();

                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public PartialViewResult ComSummary(int? year_)
        {
            
            Session["year_"] = year_;

            return PartialView("pv_Commitment_Summary");



        }
        //public ActionResult commitment_summary([DataSourceRequest]DataSourceRequest request,int? year_ = 0)
        public JsonResult commitment_summary([DataSourceRequest] DataSourceRequest request, int? year_=0)
        {
         
            List<Release_Float_Model> Float_List = new List<Release_Float_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec [sp_BMS_CommitmentSummary] " + year_ + "", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Release_Float_Model Float = new Release_Float_Model();
                    Float.release_float_id = reader.GetInt64(0);
                    Float.amount_description = reader.GetValue(1).ToString();
                    Float.Amount_release = Convert.ToDecimal(reader.GetValue(2));
                    Float.MonthOf = reader.GetValue(3).ToString();
                    Float.YearOf = reader.GetInt32(4);
                    Float.balance_amount = Convert.ToDecimal(reader.GetValue(5));
                    Float.batch = reader.GetInt32(6);
                    Float.Float_Flag = reader.GetInt32(7);
                    Float.gov_com = reader.GetInt32(8);
                    Float.comtag = reader.GetInt32(9);
                    Float.code = reader.GetValue(10).ToString();
                    Float.officeid = reader.GetInt32(11);
                    Float.programid = reader.GetInt32(12);
                    Float.accountid = reader.GetInt64(13);
                    Float.particular = reader.GetValue(14).ToString();
                    Float.muncode = reader.GetInt64(15);
                    Float.brgycode = reader.GetInt64(16);
                    Float.officename= reader.GetValue(17).ToString();

                    Float_List.Add(Float);

                }
            }
            return Json(Float_List.ToDataSourceResult(request));
        }
        public PartialViewResult releaseforapproval(int? year_)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var data = 0;
                SqlCommand com = new SqlCommand(@" Select count(menu_id) from [IFMIS].[dbo].[tbl_R_BMSUserSpecialMenu] where [specialmenu]=2 and [eid]=" + Account.UserInfo.eid.ToString() + " and [actioncode]=1", con);
                con.Open();
                data = Convert.ToInt32(com.ExecuteScalar());
                Session["userspecialaccess"] = data;
            }
            return PartialView("pv_ReleaseForApproval");
        }
        public JsonResult release_for_approval([DataSourceRequest] DataSourceRequest request, int? year_ = 0,int? programid=0,long? accountid=0,int? allrelease=0)
        {

            List<Release_Float_Model> Float_List = new List<Release_Float_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec [sp_BMS_ReleaseforApproval] " + year_ + ","+ programid  + ","+ accountid + ","+ allrelease  + "", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Release_Float_Model Float = new Release_Float_Model();
                    Float.release_id = reader.GetInt64(0);
                    Float.amount_description = reader.GetValue(1).ToString();
                    Float.Amount_release = Convert.ToDecimal(reader.GetValue(2));
                    Float.MonthOf = reader.GetValue(3).ToString();
                    Float.YearOf = reader.GetInt32(4);
                    Float.batch = reader.GetInt32(6);
                    Float.officename = reader.GetValue(7).ToString();
                    Float.DateReleased= reader.GetValue(8).ToString();
                    Float.ooeid = reader.GetInt32(9);
                    Float_List.Add(Float);

                }
            }
            return Json(Float_List.ToDataSourceResult(request));
        }
        public string releaselected_approve(string[] release_id)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Releaseid");

                var idx = 0;
                foreach (var relid in release_id)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = release_id[idx];
                    dt.Rows.Add(dr);
                    idx++;
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("dbo.[sp_BMS_ReleaseApprove]", con);
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add(new SqlParameter("@releaseid", dt));
                    com.Parameters.Add(new SqlParameter("@userid", Account.UserInfo.eid));
                    con.Open();

                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public PartialViewResult ReleaseReversion()
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var data = "";
                SqlCommand com = new SqlCommand(@" Select format(getdate(),'M/d/yyyy hh:mm:ss tt') as datetime ", con);
                con.Open();
                data = com.ExecuteScalar().ToString();
                Session["dateandtime"] = data;
            }
            return PartialView("pv_ReleaseReversion");
        }
        public double AllotmentBalForReversion(int? office_id=0, int?program_id=0, long? account_id=0,int? year_=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = 0.00;
                    SqlCommand com = new SqlCommand(@" sp_BMS_AllotmentBalance "+ office_id + ","+ account_id + ","+ program_id + ","+ year_ + ",1,0", con);
                    con.Open();
                    data = Convert.ToDouble(com.ExecuteScalar());
                    return data;
                }
            }
            catch 
            {
                return 0;
            }
        }
        public PartialViewResult LegalBasisRelRev()
        {

            return PartialView("pv_LegalBasisReleaseRev");

        }

        public JsonResult LegalBasisRR_Read([DataSourceRequest] DataSourceRequest request)
        {
            Monthly_R_Layer ViewGF = new Monthly_R_Layer();
            var lst = ViewGF.LegalBasisRelRev();

            return Json(lst.ToDataSourceResult(request));

        }
        public ActionResult LegalBasisRR_Create([DataSourceRequest] DataSourceRequest request, LegalBasis_Model legalbasis)
        {
            Monthly_R_Layer Legal_Create = new Monthly_R_Layer();
            if (legalbasis != null && ModelState.IsValid)
            {
                Legal_Create.legalRR_Create(legalbasis);
            }

            return Json(new[] { legalbasis }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult LegalBasisRR_Update([DataSourceRequest] DataSourceRequest request, LegalBasis_Model legalbasis)
        {
            Monthly_R_Layer Legal_Update = new Monthly_R_Layer();
            if (legalbasis != null && ModelState.IsValid)
            {
                Legal_Update.legalRR_Update(legalbasis);
            }

            return Json(new[] { legalbasis }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult LegalBasisRR_Destroy([DataSourceRequest] DataSourceRequest request, LegalBasis_Model legalbasis)
        {
            Monthly_R_Layer Legal_Destroy = new Monthly_R_Layer();
            if (legalbasis != null)
            {
                Legal_Destroy.legalRR_Destroy(legalbasis);
            }

            return Json(new[] { legalbasis }.ToDataSourceResult(request, ModelState));
        }
        public JsonResult GetLegalBasisRR()
        {
            Monthly_R_Layer ddl = new Monthly_R_Layer();
            var lst = ddl.LegalBasis_RR();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReleaseReversionGrid([DataSourceRequest] DataSourceRequest request, int? year_=0,int? office=0, long? account=0,int? reltag=0)
        {
            Monthly_R_Layer ViewGF = new Monthly_R_Layer();

            var lst = ViewGF.ReleaseReversionGrid(year_, office, account, reltag);

            return Json(lst.ToDataSourceResult(request));

        }
        public string InsertReleseReversion(long? legalbasis=0, int? office=0, int? program=0, long? account=0, string dtetime="",double amount=0.00,long? releaseid=0,int? copdtetag=0,int? chksummary=0,int? isfloat=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_ReleaseReversion] " + legalbasis + ","+ office + ","+ program + ","+ account + ",'"+ dtetime  + "'," + Account.UserInfo.eid.ToString() + "," + amount + ","+ releaseid + ","+ copdtetag + ","+ chksummary + ","+ isfloat + "", con);
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
        public string DeleteReleseReversion(int releasereversion_id=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec sp_BMS_ReleaseReversionRemove " + releasereversion_id + "," + Account.UserInfo.eid.ToString() + "", con);
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
        public ActionResult DfpptConsolidation()
        {
            return View("pv_DffptConsolidation");
        }
        public PartialViewResult dfpptrelease(int? office = 0, int? tyear = 0, int? ooeid = 0,int? qtr=0)
        {
            Session["office"] = office;
            Session["tyear"] = tyear;
            Session["ooeid"] = ooeid;
            Session["qtr"] = qtr;
            return PartialView("pv_dfpptrelease");
        }
        public ActionResult GetDFPPTrelease([DataSourceRequest]DataSourceRequest request, int? office = 0, int? tyear = 0, int? ooeid = 0,int? qtr=0)
        {

            string SQL = "";
            SQL = "exec [sp_BMS_DfpptRelease] " + office + "," + tyear + "," + ooeid + ", "+ qtr  + "";
            DataTable dt = SQL.DataSet();
            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request)); //SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public PartialViewResult dfpptrelease_office(int? office = 0, int? tyear = 0, int? ooeid = 0, int? qtr = 0)
        {
            Session["office"] = office;
            Session["tyear"] = tyear;
            Session["ooeid"] = ooeid;
            Session["qtr"] = qtr;
            return PartialView("pv_dfpptrelease_office");
        }
        public ActionResult SupplementalRequest()
        {
            //SessionsK(office_id, program_id, ooe_id, account_id, month_, numeric_, year_);
            //Monthly_R_Layer ddl = new Monthly_R_Layer();
            //@ViewBag.MonthVaue = ddl.MonthValue();
            return View("pv_Supplemental_office");

        }
        public ActionResult GetOfficeSupplement([DataSourceRequest]DataSourceRequest request)
        {
            if (Account.UserInfo.UserTypeID == 4) // system admin
            {
                string SQL = "";
                SQL = "SELECT [OfficeID],[OfficeName] + ' ('+ltrim(rtrim(OfficeAbbrivation)) + ')' as OfficeName FROM [IFMIS].[dbo].[tbl_R_BMSOffices] where PMISOfficeID is not null and FundID <> 201 and OfficeID <> 43 order by OfficeName";
                DataTable dt = SQL.DataSet();
                var result = new ContentResult();
                result.Content = SerializeDT.DataTableToJSON(dt);
                result.ContentType = "application/json";
                return result;
            }
            else
            {
                string SQL = "";
                SQL = "SELECT [OfficeID],[OfficeName] + ' ('+ltrim(rtrim(OfficeAbbrivation)) + ')' as OfficeName FROM [IFMIS].[dbo].[tbl_R_BMSOffices] where officeid=" + Account.UserInfo.Department +"";
                DataTable dt = SQL.DataSet();
                var result = new ContentResult();
                result.Content = SerializeDT.DataTableToJSON(dt);
                result.ContentType = "application/json";
                return result;
            }
        }
        public JsonResult Load_Supp_Req([DataSourceRequest] DataSourceRequest request, int? office_id, int? program_id, int? account_id, int? month_,int? yearof,int? subaccount)
        {

            List<Monthly_DD_Model> Float_List = new List<Monthly_DD_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec sp_BMS_SupplementalGrid "+ office_id + ","+ program_id + "," + account_id + ","+ yearof + ","+ subaccount + ","+ month_ + "", con);
                
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_DD_Model Float = new Monthly_DD_Model();
                    Float.supplementalbudget_id = reader.GetInt64(0);
                    Float.Amount = Convert.ToDouble(reader.GetValue(1));
                    Float.MonthOf = reader.GetValue(2).ToString();
                    Float.YearOf = reader.GetInt32(3);
                    Float.subaccount = Convert.ToInt32(reader.GetValue(4));
                    Float.mon = Convert.ToInt32(reader.GetValue(5));
                    Float.purpose = reader.GetValue(6).ToString();
                    Float.AccountName = reader.GetValue(7).ToString();
                    Float.purposereturn = reader.GetValue(8).ToString();
                    Float.saipno = Convert.ToInt32(reader.GetValue(9));
                    Float_List.Add(Float);
                }
            }
            return Json(Float_List.ToDataSourceResult(request));
        }
        public ActionResult GetProgramsSupple([DataSourceRequest]DataSourceRequest request, int? office_ID=0,int? yearof=0)
        {
            string SQL = "";
            SQL = "select Distinct ProgramID,ProgramDescription from tbl_R_BMSOfficePrograms where ProgramYear =  "+ yearof + " and OfficeID = '" + office_ID + "' and actioncode = 1 order by ProgramDescription";
            DataTable dt = SQL.DataSet();
            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult GetAccountSupple([DataSourceRequest]DataSourceRequest request, int? prog_id=0,long yearof=0)
        {
            string SQL = "";
            SQL = "SELECT [AccountID],[AccountName] FROM [IFMIS].[dbo].[tbl_R_BMSProgramAccounts] where ProgramID=" + prog_id + " and AccountYear= "+ yearof + " and ActionCode=1 order by AccountName";
            DataTable dt = SQL.DataSet();
            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public string SupplementalAvailable(int? office_id, int? program_id, long? account_id,int? yearof,int? subppa)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (subppa == 0)
                {
                    SqlCommand com = new SqlCommand(@"exec [IFMIS].[dbo].[sp_Supplemental_Balance] " + office_id + "," + program_id + "," + account_id + "," + yearof + "", con);
                    con.Open();
                    return com.ExecuteScalar().ToString();
                }
                else
                {
                    SqlCommand com = new SqlCommand(@"exec [IFMIS].[dbo].[sp_BMS_SubAccountRemainingBalance] " + office_id + "," + account_id + "," + program_id + "," + yearof + ","+ subppa + ",0", con);
                    con.Open();
                    return com.ExecuteScalar().ToString();   
                }
            }
        }

        public string save_supplememental_request(int? legal_code=0, int? office_id=0, int? program_id = 0, long? account_id = 0, double supplement_amount = 0.00, int? MonthOf = 0, int? supid = 0, int? yearof = 0, int? subaccount_id = 0,string purpose="",int? saipno =0 )
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    
                    SqlCommand com = new SqlCommand("exec sp_BMS_Supplemental_Request " + legal_code + "," + office_id + "," + program_id + "," + account_id + "," + supplement_amount + "," + MonthOf + "," + Account.UserInfo.eid.ToString() + ","+ supid + ","+ yearof + ","+ subaccount_id + ",'"+ purpose.Replace("'","''").ToString() + "',"+ saipno + "", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    return "1";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public JsonResult Edit_Supplementa_request(int? supplementalbudget_id)
        {

            Monthly_Supplement_Model data = new Monthly_Supplement_Model();
            DataTable _dt = new DataTable();
            string _sqlQuery = "select supplementalbudget_id,LegalCode,Amount FROM IFMIS.dbo.[tbl_R_BMS_SupplementalRequest] where supplementalbudget_id = " + supplementalbudget_id + "";
            _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
            try
            {
                data.supplementalbudget_id = Convert.ToInt64(_dt.Rows[0][0]);
                data.LegalCode = Convert.ToInt64(_dt.Rows[0][1]);
                data.Amount = Convert.ToDouble(_dt.Rows[0][2]);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public string delete_dupplemental_req(int? supid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com = new SqlCommand("update [IFMIS].[dbo].[tbl_R_BMS_SupplementalRequest] set [UserID]=[UserID] + ',' + '" + Account.UserInfo.eid.ToString() + "',[ActionCode]=4,[DateTimeEntered]=[DateTimeEntered] +','+ format(cast(getdate() as datetime),'M/d/yyyy hh:mm:ss tt') where [supplementalbudget_id]=" + supid + "", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    return "1";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        
        public string submit_supplememental_request(int? office_id, int? program_id, long? account_id, int? MonthOf,int? yearof)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com = new SqlCommand("exec sp_BMS_Submit_Request " + office_id + "," + program_id + "," + account_id + "," + MonthOf + "," + Account.UserInfo.eid.ToString() + ","+ yearof + "", con);
                    con.Open();
                    return com.ExecuteScalar().ToString();
                    //com.ExecuteNonQuery();
                    //return "1";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        //public string SupplementalAvailable(int? office_id, int? program_id, long? account_id)
        //{

        //    using (SqlConnection con = new SqlConnection(Common.MyConn()))
        //    {
        //        SqlCommand com = new SqlCommand(@"exec [IFMIS].[dbo].[sp_Supplemental_Balance] " + office_id + "," + program_id + "," + account_id + "", con);
        //        con.Open();
        //        return com.ExecuteScalar().ToString();
        //    }
        //}
        public PartialViewResult supreqsubmitted(int? office=0)
        {
            Session["office"] = office;
            return PartialView("pv_SupplementalReqSubmitted");
        }
        public PartialViewResult supreqsubmittedforapproval(int? office = 0)
        {
            Session["office"] = office;
            return PartialView("pv_SupplementalReqSubmittedForApproval");
        }
        public PartialViewResult supreqforapproval(int? office = 0)
        {
            Session["office"] = office;
            return PartialView("pv_SupplementalReqForApproval");
        }
        public JsonResult GetSupplementlist([DataSourceRequest] DataSourceRequest request, int? office = 0,int? programid=0,int? accountid=0, int? status=0,int? yearof=0,int? monthof=0)
        {

            List<Monthly_DD_Model> Float_List = new List<Monthly_DD_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec [sp_BMS_SupplementalSub_Grid] " + office + ","+ programid + ","+ accountid + ","+ status + ","+ yearof + ","+ monthof + "", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_DD_Model Float = new Monthly_DD_Model();
                    Float.supplementalbudget_id = reader.GetInt64(0);
                    Float.Amount = Convert.ToDouble(reader.GetValue(1));
                    Float.MonthOf = reader.GetValue(2).ToString();
                    Float.YearOf = reader.GetInt32(3);
                    Float.AccountName = reader.GetValue(4).ToString();
                    Float.dtetime = reader.GetValue(5).ToString();
                    Float.subaccount = Convert.ToInt32(reader.GetValue(6));
                    Float.subaccountname = reader.GetValue(7).ToString();
                    Float.AmountApprove = Convert.ToDouble(reader.GetValue(8));
                    Float.status = Convert.ToInt32(reader.GetValue(9));
                    Float.purposereturn= reader.GetValue(10).ToString();
                    Float_List.Add(Float);

                }
            }
            return Json(Float_List.ToDataSourceResult(request));
        }
        public string returnsupplemental(int? trnno=0,string purposereturn="")
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com = new SqlCommand("update [IFMIS].[dbo].[tbl_R_BMS_SupplementalRequest] set [UserID]='" + Account.UserInfo.eid.ToString() + "',[ActionCode]=1,[DateTimeEntered]=left([DateTimeEntered],patindex('%,%',[DateTimeEntered]) -1),[Purpose_return]='"+ purposereturn.Replace("'","''").ToString() + "' where [supplementalbudget_id]=" + trnno + " and [ActionCode]=5", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    return "success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public string disapprovesupplemental(int? trnno = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com = new SqlCommand("update [IFMIS].[dbo].[tbl_R_BMS_SupplementalRequest] set [ActionCode]=5,[ApproveAmount]=NULL where [supplementalbudget_id]=" + trnno + " and [ActionCode]=6", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    return "success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public string approvesupplemental(string[] sup_id,int? legalbasis=0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Releaseid");

                var idx = 0;
                foreach (var relid in sup_id)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = sup_id[idx];
                    dt.Rows.Add(dr);
                    idx++;
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("dbo.[sp_BMS_SupplementalApprove]", con);
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add(new SqlParameter("@supid", dt));
                    com.Parameters.Add(new SqlParameter("@userid", Account.UserInfo.eid));
                    com.Parameters.Add(new SqlParameter("@legalbasis", legalbasis));
                    con.Open();

                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public PartialViewResult supplemantalapproved(int? office = 0)
        {
            Session["office"] = office;
            return PartialView("pv_SupplementalReqApprove");
        }
        public int checksubppa(int? year=0, int? Progid=0, long? accountid=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = 0;
                    SqlCommand com = new SqlCommand(@"Select count([nonofficeid]) from [IFMIS].[dbo].[tbl_R_BMSNonOffice] where [programid]=" + Progid + " and [accountid]="+ accountid  + " and [yearof]="+ year + " and [actioncode]=1", con);
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
        public ActionResult WFP()
        {
            return View("pv_WFPNew");
        }
        public ActionResult WFPConsolidation()
        {
            return View("pv_WFPNewConsol");
        }
        public string approvesupplementalamount(int? office=0,int? programid=0,int? accountid=0,int? yearof=0,int? subaccount=0,int? monthof=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand("exec sp_BMS_SupplementalForApproval " + office + ","+ programid + ","+ accountid + ","+ yearof + ","+ subaccount + ","+ monthof + ","+Account.UserInfo.eid +"", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public ActionResult RealignRequest()
        {
            return View("pv_Realign_Office");
        }
        public ActionResult ReversionRequest()
        {
            return View("pv_Reversion_Office");
        }
        public string Save_Realign_Office(int? office_id_realign, int? program_id_realign, int? account_id_realign,int? subaccountfrom, double amount_to_realign, double to_be_realign, int? year_, int? office_id_to_realign, int? program_id_to_realign, int? account_id_to_realign,int? subaccountto, double to_Amount,int? realignid_tmp,string purposefrom="",string purposeto="",int? saip=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_RealignOffice] "+ office_id_realign + ","+ program_id_realign + ","+ account_id_realign + ","+ subaccountfrom + ","+ amount_to_realign + ",0,"+ office_id_to_realign + ","+ program_id_to_realign + ","+ account_id_to_realign + ","+ subaccountto + ","+ to_Amount + "," + Account.UserInfo.eid + ","+ year_ + ","+ realignid_tmp + ",'"+ purposefrom.Replace("'","''").ToString() + "','" + purposeto.Replace("'", "''").ToString() + "',"+ saip + "", con);
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
        public string Save_Reversion_Office(int? office_id_realign, int? program_id_realign, int? account_id_realign, int? subaccountfrom, double amount_to_realign, double to_be_realign, int? year_, int? office_id_to_realign, int? program_id_to_realign, int? account_id_to_realign, int? subaccountto, double to_Amount, int? realignid_tmp, string purposefrom = "", string purposeto = "",int? saiprev=0,int? surplus=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_ReversionOffice] " + office_id_realign + "," + program_id_realign + "," + account_id_realign + "," + subaccountfrom + "," + amount_to_realign + ",0," + office_id_to_realign + "," + program_id_to_realign + "," + account_id_to_realign + "," + subaccountto + "," + to_Amount + "," + Account.UserInfo.eid + "," + year_ + "," + realignid_tmp + ",'" + purposefrom.Replace("'", "''").ToString() + "','" + purposeto.Replace("'", "''").ToString() + "',"+ saiprev + ","+ surplus + "", con);
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
        public double thatassET(int? year_=0,int? officeid=0,int? account_from=0,int? ooeid=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = 0.00;
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_RealignmentOfficeSummary] " + year_ + "," + officeid + ",1,"+ account_from + ","+ ooeid + "", con);
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
        public double thatassETv2(int? year_ = 0, int? officeid = 0, int? account_from = 0, int? ooeid = 0,int? realignid=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = 0.00;
                    if (realignid == 1) //realignment
                    {
                        SqlCommand com = new SqlCommand(@"exec [sp_BMS_RealignmentOfficeSummary] " + year_ + "," + officeid + ",1," + account_from + "," + ooeid + "", con);
                        con.Open();
                        data = Convert.ToDouble(com.ExecuteScalar());
                    }
                    else
                    { //reversion
                        SqlCommand com = new SqlCommand(@"exec [sp_BMS_ReversionOfficeSummaryV2] " + year_ + "," + officeid + ",1," + 0 + "," + account_from + ","+ ooeid + "", con);
                        con.Open();
                        data = Convert.ToDouble(com.ExecuteScalar());
                    }
                    return data;
                }
            }
            catch //(Exception ex)
            {
                return 0;
            }
        }
        public double thatassRevert(int? year_ = 0, int? officeid = 0,int? revsummaryid=0,int? accountid=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = 0.00;
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_ReversionOfficeSummary] " + year_ + "," + officeid + ",1,"+ revsummaryid + ","+ accountid +"", con);
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
        public JsonResult Load_Realign_Req([DataSourceRequest] DataSourceRequest request, int? office_id, int? program_id, int? account_id,int? yearof, int? subaccount,int? submit,int? approved, int? posted,int? ooe_id_from,int? realignid)
        {

            List<Monthly_DD_Model> realign_List = new List<Monthly_DD_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (realignid == 1)
                {
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_RealignmentGrid] " + office_id + "," + program_id + "," + account_id + "," + yearof + "," + subaccount + "," + submit + "," + approved + "," + posted + "," + ooe_id_from + "", con);

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Monthly_DD_Model real = new Monthly_DD_Model();
                        real.realignmentid = reader.GetInt64(0);
                        real.accountfrom = reader.GetValue(1).ToString();
                        real.ppafrom = reader.GetValue(2).ToString();
                        real.accountto = reader.GetValue(3).ToString();
                        real.ppato = reader.GetValue(4).ToString();
                        real.Amount = Convert.ToDouble(reader.GetValue(5));
                        real.fromprogram = reader.GetInt32(6);
                        real.fromaccount = reader.GetInt32(7);
                        real.toprogram = reader.GetInt32(8);
                        real.toaccount = reader.GetInt32(9);
                        real.subaccount = reader.GetInt32(10);
                        real.tooffice = reader.GetInt32(11);
                        real.fromoffice = reader.GetInt32(12);
                        real.datetimesubmit = reader.GetValue(13).ToString();
                        real.datetimeapprove = reader.GetValue(14).ToString();
                        real.Amount_orig = Convert.ToDouble(reader.GetValue(15));
                        real.purpose = reader.GetValue(16).ToString();
                        real.saipno = reader.GetInt32(17);
                        real.purposereturn = reader.GetValue(18).ToString();
                        real.datetimeposted = reader.GetValue(19).ToString();
                        real.mode = reader.GetValue(20).ToString();
                        real.Amountfrom = Convert.ToDouble(reader.GetValue(21));
                        real.ooeid = 0;
                        real.realign_tag = Convert.ToInt32(realignid);
                        realign_List.Add(real);
                    }
                }
                else
                {
                    SqlCommand com2 = new SqlCommand(@"exec [sp_BMS_ReversionGrid] " + office_id + "," + program_id + "," + account_id + "," + yearof + "," + subaccount + "," + submit + "," + approved + "," + posted + "," + 0 + "," + ooe_id_from + "", con);

                    con.Open();
                    SqlDataReader reader2 = com2.ExecuteReader();
                    while (reader2.Read())
                    {
                        Monthly_DD_Model real = new Monthly_DD_Model();
                        real.realignmentid = reader2.GetInt64(0);
                        real.accountfrom = reader2.GetValue(1).ToString();
                        real.ppafrom = reader2.GetValue(2).ToString();
                        real.accountto = reader2.GetValue(3).ToString();
                        real.ppato = reader2.GetValue(4).ToString();
                        real.AmountApprove = Convert.ToDouble(reader2.GetValue(5));
                        real.fromprogram = reader2.GetInt32(6);
                        real.fromaccount = reader2.GetInt32(7);
                        real.subaccount = reader2.GetInt32(8);
                        real.fromoffice = reader2.GetInt32(9);
                        real.datetimesubmit = reader2.GetValue(10).ToString();
                        real.datetimeapprove = reader2.GetValue(11).ToString();
                        real.Amount = Convert.ToDouble(reader2.GetValue(12));//propose
                        real.purpose = reader2.GetValue(13).ToString();
                        real.tooffice = reader2.GetInt32(15);
                        real.toprogram = reader2.GetInt32(16);
                        real.toaccount = reader2.GetInt32(17);
                        real.saipno = reader2.GetInt32(19);
                        real.purposereturn = reader2.GetValue(18).ToString();
                        real.datetimeposted = reader2.GetValue(21).ToString();
                        real.mode = reader2.GetValue(20).ToString();
                        real.officename = reader2.GetValue(22).ToString();
                        real.posted = reader2.GetInt32(23);
                        real.trnno = reader2.GetInt32(25);
                        real.Amountfrom = Convert.ToDouble(reader2.GetValue(5));
                        real.ooeid = reader2.GetInt32(24);
                        real.realign_tag = Convert.ToInt32(realignid);
                        realign_List.Add(real);
                    }
                }
            }
            return Json(realign_List.ToDataSourceResult(request));
        }
        public JsonResult Load_Realign_Review([DataSourceRequest] DataSourceRequest request, int? office_id,int? yearof, int? submit, int? approved, int? posted, int? ooe_id_from,int? mode)
        {

            List<Monthly_DD_Model> realign_List = new List<Monthly_DD_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (mode == 1) //realign
                {
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_RealignmentGrid_review] " + office_id + "," + yearof + "," + submit + "," + approved + "," + posted + "", con);

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Monthly_DD_Model real = new Monthly_DD_Model();
                        real.realignmentid = reader.GetInt64(0);
                        real.accountfrom = reader.GetValue(1).ToString();
                        real.ppafrom = reader.GetValue(2).ToString();
                        real.accountto = reader.GetValue(3).ToString();
                        real.ppato = reader.GetValue(4).ToString();
                        real.Amount = Convert.ToDouble(reader.GetValue(5));
                        real.fromprogram = reader.GetInt32(6);
                        real.fromaccount = reader.GetInt32(7);
                        real.toprogram = reader.GetInt32(8);
                        real.toaccount = reader.GetInt32(9);
                        real.subaccount = reader.GetInt32(10);
                        real.tooffice = reader.GetInt32(11);
                        real.fromoffice = reader.GetInt32(12);
                        real.datetimesubmit = reader.GetValue(13).ToString();
                        real.datetimeapprove = reader.GetValue(14).ToString();
                        real.Amount_orig = Convert.ToDouble(reader.GetValue(15));
                        real.purpose = reader.GetValue(16).ToString();
                        real.saipno = reader.GetInt32(17);
                        real.purposereturn = reader.GetValue(18).ToString();
                        real.datetimeposted = reader.GetValue(19).ToString();
                        real.mode = reader.GetValue(20).ToString();
                        real.Amountfrom = Convert.ToDouble(reader.GetValue(21));
                        realign_List.Add(real);
                    }
                }
                else
                {
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_ReversionGrid_review] " + office_id + "," + yearof + "," + submit + "," + approved + "," + posted + "", con);

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Monthly_DD_Model real = new Monthly_DD_Model();
                        real.realignmentid = reader.GetInt64(0);
                        real.accountfrom = reader.GetValue(1).ToString();
                        real.ppafrom = reader.GetValue(2).ToString();
                        real.accountto = reader.GetValue(3).ToString();
                        real.ppato = reader.GetValue(4).ToString();
                        real.Amount = Convert.ToDouble(reader.GetValue(5));
                        real.fromprogram = reader.GetInt32(6);
                        real.fromaccount = reader.GetInt32(7);
                        real.toprogram = reader.GetInt32(8);
                        real.toaccount = reader.GetInt32(9);
                        real.subaccount = reader.GetInt32(10);
                        real.tooffice = reader.GetInt32(11);
                        real.fromoffice = reader.GetInt32(12);
                        real.datetimesubmit = reader.GetValue(13).ToString();
                        real.datetimeapprove = reader.GetValue(14).ToString();
                        real.Amount_orig = Convert.ToDouble(reader.GetValue(15));
                        real.purpose = reader.GetValue(16).ToString();
                        real.saipno = reader.GetInt32(17);
                        real.purposereturn = reader.GetValue(18).ToString();
                        real.datetimeposted = reader.GetValue(19).ToString();
                        real.mode = reader.GetValue(20).ToString();
                        real.Amountfrom = Convert.ToDouble(reader.GetValue(21));
                        realign_List.Add(real);
                    }
                }
            }
            return Json(realign_List.ToDataSourceResult(request));
        }
        public JsonResult Load_Realign_ReviewSummary([DataSourceRequest] DataSourceRequest request, int? modeid, int? year)
        {

            List<Monthly_DD_Model> realign_List = new List<Monthly_DD_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec [sp_BMS_MafReview] " + modeid + "," + year + "", con);

                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_DD_Model real = new Monthly_DD_Model();
                    real.trnno = Convert.ToInt32(reader.GetValue(0));
                    real.FMISOfficeCode = Convert.ToInt32(reader.GetValue(1)); 
                    real.officename = reader.GetValue(2).ToString();
                    real.Amount = Convert.ToDouble(reader.GetValue(3));
                    real.datetimesubmit = reader.GetValue(4).ToString();
                    real.YearOf = Convert.ToInt32(reader.GetValue(5));
                    real.purpose = reader.GetValue(6).ToString();
                    real.officefullname = reader.GetValue(7).ToString();
                    real.modev2= Convert.ToInt32(reader.GetValue(8));
                    real.userid = Convert.ToInt64(reader.GetValue(9));
                    realign_List.Add(real);
                }
            }
            return Json(realign_List.ToDataSourceResult(request));
        }
        public string datassV2(int? office_id=0, int? program_id=0, int? ooe_id=0, int? account_id=0, int? year_=0,int? subppaid=0,int? realignid=0)
        {
            Monthly_R_Layer datass = new Monthly_R_Layer();
            if (realignid == 1)
            {
                return datass.subpparealignAvailable(office_id, program_id, ooe_id, account_id, year_, subppaid);
            }
            else //reversion
            {
                return datass.subppareversionAvailable(office_id, program_id, ooe_id, account_id, year_, subppaid);
            }
        }
        public string datassReversion(int? office_id = 0, int? program_id = 0, int? ooe_id = 0, int? account_id = 0, int? year_ = 0, int? subppaid = 0)
        {
            Monthly_R_Layer datass = new Monthly_R_Layer();

            return datass.subppareversionAvailable(office_id, program_id, ooe_id, account_id, year_, subppaid);


        }
        public string DeleteRealignRequest(int? realignmentid=0, int? realign_tag = 0, int? toprogram=0)
        {
            var data = "";
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                   
                    SqlCommand com = new SqlCommand(@"update [IFMIS].[dbo].[tbl_R_BMS_Realignment_Office] set [ActionCode]=4,[DateTimeEntered]=[DateTimeEntered] +','+ format(getdate(),'MM/dd/yyyy hh:mm:ss tt'),[UserID]=[UserID] +','+ '" + Account.UserInfo.eid.ToString() + "' where realignment_id=" + realignmentid + " and actioncode=1", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                   
                    return "success";
                }
            }
            catch //(Exception ex)
            {
                return "";
            }
        }
        public string DeleteReversionRequest(int? realignmentid = 0, int? realign_tag = 0, int? toprogram = 0)
        {
            var data = "";
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    
                    if (toprogram != 0) // Transfer
                    {
                        SqlCommand com = new SqlCommand(@"update [IFMIS].[dbo].[tbl_R_BMS_SupplementalTransfer_Request] set [ActionCode]=4,[DateTimeEntered]=[DateTimeEntered] +','+ format(getdate(),'MM/dd/yyyy hh:mm:ss tt'),[UserID]=[UserID] +','+ '" + Account.UserInfo.eid.ToString() + "' where supplementaltransfere_id=" + realignmentid + " and actioncode=1", con);
                        con.Open();
                        data = Convert.ToString(com.ExecuteScalar());
                    }
                    else
                    {
                        SqlCommand com = new SqlCommand(@"update [IFMIS].[dbo].[tbl_R_BMS_SupplementalReverse_Request] set [ActionCode]=4,[DateTimeEntered]=[DateTimeEntered] +','+ format(getdate(),'MM/dd/yyyy hh:mm:ss tt'),[UserID]=[UserID] +','+ '" + Account.UserInfo.eid.ToString() + "' where supplementalreverse_id=" + realignmentid + " and actioncode=1", con);
                        con.Open();
                        data = Convert.ToString(com.ExecuteScalar());
                    }
                    
                    return "success";
                }
            }
            catch //(Exception ex)
            {
                return "";
            }
        }
        public string submitrealignment(int? office_id=0,int? program_id=0,int? account_id= 0,int? year=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_RealignmentGrid_Submit]  "+ office_id + ","+ program_id + ","+ account_id + ","+ year + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch //(Exception ex)
            {
                return "";
            }
        }
        public PartialViewResult realignsubmitted()
        {
            return PartialView("pv_Realign_Office_Submitted");
        }
        public PartialViewResult realignforapproval()
        {
            return PartialView("pv_Realign_Office_SubmittedForApproval");
        }
        public string realigforapprovalupdate(int? realigid=0,int? accountid_to=0,double amount=0.00)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_RealignForApprovalUpdate]  " + realigid + "," + accountid_to + "," + amount + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch //(Exception ex)
            {
                return "";
            }
        }
        public string realignreturn(int? realignmentid=0, int? fromoffice=0, int? fromprogram=0, int? fromaccount=0, int? tooffice=0, int? toprogram=0, int? toaccount=0, int? subaccount=0, double Amount=0.00,int? year=0,string purposereturn="")
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_RealignmentReturn]  " + realignmentid + "," + fromoffice + ","+ fromprogram + "," + fromaccount + "," + tooffice + "," + toprogram + "," + toaccount + "," + subaccount + "," + Amount + "," + year + ",'"+ purposereturn.Replace("'","''").ToString() + "'", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch //(Exception ex)
            {
                return "";
            }
        }
        public string PostRealignment(string[] realtrans)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("transno");
                var idx = 0;
                foreach (var trnno in realtrans)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = realtrans[idx];
                    dt.Rows.Add(dr);
                    idx++;
                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("dbo.[sp_BMS_RealignmentPosting]", con);
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
        public PartialViewResult realignposted()
        {
            return PartialView("pv_Realign_Office_PostedV2_user");
        }
        public JsonResult Load_Reversion_Req([DataSourceRequest] DataSourceRequest request, int? office_id, int? program_id, int? account_id, int? yearof, int? subaccount, int? submit, int? approved,int? posted,int? revsumm)
        {

            List<Monthly_DD_Model> realign_List = new List<Monthly_DD_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (posted == 0)
                {
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_ReversionGrid] " + office_id + "," + program_id + "," + account_id + "," + yearof + "," + subaccount + "," + submit + "," + approved + "," + posted + "," + revsumm + "", con);

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Monthly_DD_Model real = new Monthly_DD_Model();
                        real.reversion_id = reader.GetInt64(0);
                        real.accountfrom = reader.GetValue(1).ToString();
                        real.ppafrom = reader.GetValue(2).ToString();
                        real.accountto = reader.GetValue(3).ToString();
                        real.ppato = reader.GetValue(4).ToString();
                        real.AmountApprove = Convert.ToDouble(reader.GetValue(5));
                        real.fromprogram = reader.GetInt32(6);
                        real.fromaccount = reader.GetInt32(7);
                        real.subaccount = reader.GetInt32(8);
                        real.fromoffice = reader.GetInt32(9);
                        real.datetimesubmit = reader.GetValue(10).ToString();
                        real.datetimeapprove = reader.GetValue(11).ToString();
                        real.Amount = Convert.ToDouble(reader.GetValue(12));//propose
                        real.purpose = reader.GetValue(13).ToString();
                        real.tooffice = reader.GetInt32(15);
                        real.toprogram = reader.GetInt32(16);
                        real.toaccount = reader.GetInt32(17);
                        real.saipno = reader.GetInt32(19);
                        real.purposereturn = reader.GetValue(18).ToString();
                        real.datetimeposted = reader.GetValue(21).ToString();
                        real.mode = reader.GetValue(20).ToString();
                        real.officename = reader.GetValue(22).ToString();
                        real.posted = reader.GetInt32(23);
                        real.trnno = reader.GetInt32(24);
                        realign_List.Add(real);
                    }
                }
                else //posted
                {
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_ReversionGrid_posted] " + office_id + "," + program_id + "," + account_id + "," + yearof + "," + subaccount + "," + submit + "," + approved + "," + posted + "," + revsumm + "", con);

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Monthly_DD_Model real = new Monthly_DD_Model();
                        real.reversion_id = reader.GetInt64(0);
                        real.accountfrom = reader.GetValue(1).ToString();
                        real.ppafrom = reader.GetValue(2).ToString();
                        real.accountto = reader.GetValue(3).ToString();
                        real.ppato = reader.GetValue(4).ToString();
                        real.AmountApprove = Convert.ToDouble(reader.GetValue(5));
                        real.fromprogram = reader.GetInt32(6);
                        real.fromaccount = reader.GetInt32(7);
                        real.subaccount = reader.GetInt32(8);
                        real.fromoffice = reader.GetInt32(9);
                        real.datetimesubmit = reader.GetValue(10).ToString();
                        real.datetimeapprove = reader.GetValue(11).ToString();
                        real.Amount = Convert.ToDouble(reader.GetValue(12));//propose
                        real.purpose = reader.GetValue(13).ToString();
                        real.tooffice = reader.GetInt32(15);
                        real.toprogram = reader.GetInt32(16);
                        real.toaccount = reader.GetInt32(17);
                        real.saipno = reader.GetInt32(19);
                        real.purposereturn = reader.GetValue(18).ToString();
                        real.datetimeposted = reader.GetValue(21).ToString();
                        real.mode = reader.GetValue(20).ToString();
                        real.officename = reader.GetValue(22).ToString();
                        real.posted = reader.GetInt32(23);
                        real.trnno = 0;
                        realign_List.Add(real);
                    }
                }
            }
            return Json(realign_List.ToDataSourceResult(request));
        }

        public JsonResult Load_Reversion_Surplus([DataSourceRequest] DataSourceRequest request, int? office_id, int? program_id, int? account_id, int? yearof, int? subaccount, int? submit, int? approved, int? posted, int? revsumm)
        {

            List<Monthly_DD_Model> realign_List = new List<Monthly_DD_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
               
                SqlCommand com = new SqlCommand(@"exec [sp_BMS_ReversionGrid_surplus] " + office_id + "," + program_id + "," + account_id + "," + yearof + "," + subaccount + "," + submit + "," + approved + "," + posted + "," + revsumm + "", con);

                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_DD_Model real = new Monthly_DD_Model();
                    real.fromoffice = reader.GetInt32(0);
                    real.Amount = Convert.ToDouble(reader.GetValue(5));
                    real.officename = reader.GetValue(1).ToString();
                    real.AccountName = reader.GetValue(4).ToString();
                    realign_List.Add(real);
                }
            }
            return Json(realign_List.ToDataSourceResult(request));
        }
        //public string DeleteReversionRequest(int? realignmentid = 0, int? toaccount = 0)
        //{
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(Common.MyConn()))
        //        {
        //            var data = "";
        //            if (toaccount == 0) //source appropriation
        //            {
        //                SqlCommand com = new SqlCommand(@"update [IFMIS].[dbo].[tbl_R_BMS_SupplementalReverse_Request] set [ActionCode]=4,[DateTimeEntered]=[DateTimeEntered] +','+ format(getdate(),'MM/dd/yyyy hh:mm:ss tt'),[UserID]=[UserID] +','+ '" + Account.UserInfo.eid.ToString() + "' where supplementalreverse_id=" + realignmentid + " and actioncode=1", con);
        //                con.Open();
        //                data = Convert.ToString(com.ExecuteScalar());
        //            }
        //            else //defficient
        //            {
        //                SqlCommand com2 = new SqlCommand(@"update [IFMIS].[dbo].[tbl_R_BMS_SupplementalTransfer_Request] set [ActionCode]=4,[DateTimeEntered]=[DateTimeEntered] +','+ format(getdate(),'MM/dd/yyyy hh:mm:ss tt'),[UserID]=[UserID] +','+ '" + Account.UserInfo.eid.ToString() + "' where supplementaltransfere_id=" + realignmentid + " and actioncode=1", con);
        //                con.Open();
        //                data = Convert.ToString(com2.ExecuteScalar());
        //            }
        //            return "success";
        //        }
        //    }
        //    catch //(Exception ex)
        //    {
        //        return "";
        //    }
        //}
        public string submitreversion(int? office_id = 0, int? program_id = 0, int? account_id = 0, int? year = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_ReversionGrid_Submit]  " + office_id + "," + program_id + "," + account_id + "," + year + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch //(Exception ex)
            {
                return "";
            }
        }
        public PartialViewResult reversionsubmitted()
        {
            return PartialView("pv_Reversion_Office_Submitted");
        }
        public PartialViewResult reversionForApproval()
        {
            return PartialView("pv_Reversion_Office_SubmittedForApproval");
        }
        public string reversionforapprovalupdate(int? realigid = 0, int? accountid_to = 0, double amount = 0.00)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_ReversionForApprovalUpdate]  " + realigid + "," + accountid_to + "," + amount + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch //(Exception ex)
            {
                return "";
            }
        }
        public string PostReversion(int? officeid=0,int? programid=0,int? accountid=0,int? yearof=0,int? lglbasis=0,int? revsummaryid=0)
        {
            try
            {
                var data = "";
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("dbo.[sp_BMS_ReversionPosting] "+ officeid + ","+ programid + ","+ accountid + ","+ yearof +","+ Account.UserInfo.eid + ","+ lglbasis + ","+ revsummaryid +"", con);
                    //com.CommandType = System.Data.CommandType.StoredProcedure;
                    //com.Parameters.Add(new SqlParameter("@trnno", dt));
                    //com.Parameters.Add(new SqlParameter("@UserID", Account.UserInfo.eid));
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
        public PartialViewResult reversionposted()
        {
            return PartialView("pv_Reversion_Office_Posted");
        }
        public PartialViewResult reversionapprove()
        {
            return PartialView("pv_Reversion_Office_Approved");
        }
        public JsonResult Account_RealignFrom(int? office_ID_from, int? prog_id_from, int? year_, int? office_ID_to, int? prog_id_to, int? account_id_to,int? ooe_id_from,int? realignid)
        {
            Monthly_R_Layer ddl = new Monthly_R_Layer();
            var lst = ddl.Account_RealignFrom(office_ID_from, prog_id_from, year_, office_ID_to, prog_id_to, account_id_to, ooe_id_from, realignid);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public ActionResult saipno([DataSourceRequest]DataSourceRequest request, int year = 0)
        {

            string tempStr = "select distinct SuplementalNo from spms.dbo.spms_tblDepartmentBudgetaryRequirements where isSuplemental = 1 and fiscal_year = "+ year + "";
            DataTable dt = tempStr.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;

        }
        public string Approve_Realign(int? office_id=0, int? program_id=0, int? account_id=0, int? yearof=0, int? submit=0, int? approved=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_RealignmentApproval] " + office_id + "," + program_id + "," + account_id + "," + yearof + "," + submit +"," + approved + ","+ Account.UserInfo.eid +"", con);
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
        public PartialViewResult realignapproved()
        {
            return PartialView("pv_Realign_Office_Forposting_User");
        }
        public string revertreturn(int? realignmentid = 0, int? fromoffice = 0, int? fromprogram = 0, int? fromaccount = 0, int? tooffice = 0, int? toprogram = 0, int? toaccount = 0, int? subaccount = 0, double Amount = 0.00, int? year = 0, string purposereturn = "")
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_ReversionReturn]  " + realignmentid + "," + fromoffice + "," + fromprogram + "," + fromaccount + "," + tooffice + "," + toprogram + "," + toaccount + "," + subaccount + "," + Amount + "," + year + ",'" + purposereturn.Replace("'", "''").ToString() + "'", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch //(Exception ex)
            {
                return "";
            }
        }
        public string Approve_Reversion(int? office_id = 0, int? program_id = 0, int? account_id = 0, int? yearof = 0, int? submit = 0, int? approved = 0,int? revsummaryid=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_ReversionForApproval] " + office_id + "," + program_id + "," + account_id + "," + yearof + "," + submit + "," + approved + "," + Account.UserInfo.eid + ","+ revsummaryid + "", con);
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
        public string revertdisapprove(int? reversion_id = 0, int? accountto = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    if (accountto != 0)
                    {
                        SqlCommand com = new SqlCommand(@"update [IFMIS].[dbo].[tbl_R_BMS_SupplementalReverse_Request] set approve=0 where actioncode=1 and supplementalreverse_id=" + reversion_id + "", con);
                        con.Open();
                        data = Convert.ToString(com.ExecuteScalar());
                    }
                    else
                    {
                        SqlCommand com = new SqlCommand(@"update [IFMIS].[dbo].[tbl_R_BMS_SupplementalTransfer_Request] set approve=0 where actioncode=1 and supplementaltransfere_id=" + reversion_id + "", con);
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
        public double RevertSumm(int? year_ = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = 0.00;
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_ReversionSummary] " + year_ + "", con);
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
        public string updatesuppamount(int? supid = 0, double amount = 0.00)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand("exec sp_BMS_SupplementalForApprovalUpdate "+ supid + ","+ amount + ","+ Account.UserInfo.eid +"", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public string realigndisapprove(int? realignmentid = 0, int? accountto = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    if (accountto != 0)
                    {
                        SqlCommand com = new SqlCommand(@"update [IFMIS].[dbo].[tbl_R_BMS_Realignment_Office] set approve=0 where actioncode=1 and [realignment_id]=" + realignmentid + "", con);
                        con.Open();
                        data = Convert.ToString(com.ExecuteScalar());
                    }
                    else
                    {
                        SqlCommand com = new SqlCommand(@"update [IFMIS].[dbo].[tbl_R_BMS_Realignment_Office] set approve=0 where actioncode=1 and [realignment_id]=" + realignmentid + "", con);
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
        public PartialViewResult reversionsurplus()
        {
            return PartialView("pv_Reversion_Office_Surplus");
        }
        public PartialViewResult postcaf()
        {
            return PartialView("pv_CAFPosting");

        }
        public PartialViewResult postcaf_forposted()
        {
            return PartialView("pv_CAFPosting_NotPosted");

        }
        public PartialViewResult postcaf_posted()
        {
            return PartialView("pv_CAFPosting_Posted");

        }
        public JsonResult CafUnposted([DataSourceRequest]DataSourceRequest request, int? program = 0, long? account = 0, long? ppaid = 0, int? charge_office = 0, int? year = 0, int? caftrans = 0,int? epaid=0)
        {
            List<CAFPosting> prog = new List<CAFPosting>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (epaid == 0)
                {
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_CAFPosting] " + program + "," + account + "," + ppaid + "," + charge_office + "," + year + "," + caftrans + "", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        CAFPosting loc = new CAFPosting();
                        loc.cafid = Convert.ToInt32(reader.GetValue(0));
                        loc.programid = Convert.ToInt32(reader.GetValue(1));
                        loc.accountid = Convert.ToInt32(reader.GetValue(2));
                        loc.ppaid = Convert.ToInt32(reader.GetValue(3));
                        loc.nonofficeid = Convert.ToInt32(reader.GetValue(4));
                        loc.dscription = reader.GetValue(5).ToString();//Convert.ToString(reader.GetValue(5));
                        loc.amount = Convert.ToDouble(reader.GetValue(6));
                        loc.cafno = reader.GetValue(7).ToString();//Convert.ToString(reader.GetValue(7));
                        loc.office = Convert.ToString(reader.GetValue(8));
                        loc.officeid = Convert.ToInt32(reader.GetValue(9));
                        loc.account = Convert.ToString(reader.GetValue(10));
                        loc.dateissued = Convert.ToString(reader.GetValue(11));
                        prog.Add(loc);
                    }
                }
                else
                {
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_CAFPosting_EPA] " + program + "," + account + "," + ppaid + "," + charge_office + "," + year + "," + caftrans + "", con);
                    con.Open();
                    com.CommandTimeout = 0;
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        CAFPosting loc = new CAFPosting();
                        loc.cafid = Convert.ToInt32(reader.GetValue(0));
                        loc.programid = Convert.ToInt32(reader.GetValue(1));
                        loc.accountid = Convert.ToInt32(reader.GetValue(2));
                        loc.ppaid = Convert.ToInt32(reader.GetValue(3));
                        loc.nonofficeid = Convert.ToInt32(reader.GetValue(4));
                        loc.dscription = reader.GetValue(5).ToString();//Convert.ToString(reader.GetValue(5));
                        loc.amount = Convert.ToDouble(reader.GetValue(6));
                        loc.cafno = reader.GetValue(7).ToString();//Convert.ToString(reader.GetValue(7));
                        loc.office = Convert.ToString(reader.GetValue(8));
                        loc.officeid = Convert.ToInt32(reader.GetValue(9));
                        loc.account = Convert.ToString(reader.GetValue(10));
                        loc.dateissued = Convert.ToString(reader.GetValue(11));
                        prog.Add(loc);
                    }
                }
            }
            return Json(prog.ToDataSourceResult(request));
        }
        public string PostingCafNo(int? cafid = 0, int? officeid = 0, int? accountid = 0, string cafno = "", int? programid = 0, int? year = 0, int? epaid = 0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    if (epaid == 0)
                    {
                        SqlCommand com = new SqlCommand(@"exec dbo.[sp_BMS_CAFNo_Posted] " + cafid + "," + officeid + "," + accountid + ",'" + cafno + "'," + Account.UserInfo.eid.ToString() + "," + programid + "," + year + "", con);
                        con.Open();
                        data = Convert.ToString(com.ExecuteScalar());
                    }
                    else
                    {
                        SqlCommand com = new SqlCommand(@"exec dbo.[sp_BMS_CAFNo_Posted_EPA] ," + officeid + "," + accountid + ",'" + cafno + "'," + Account.UserInfo.eid.ToString() + "," + programid + "," + year + "," + epaid + "", con);
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
        public ActionResult MafReview()
        {
            return View("pv_MAFReview");
        }
        public ActionResult SupplementalBudget()
        {
            return View("vw_supplemental");
        }
        public ActionResult pv_supplementalb3()
        {
            return PartialView("pv_supplementalb3");
        }
        
        public ActionResult pv_supplementalb3_report()
        {
            return PartialView("pv_supplementalb3_dgsign");
        }
        public string mafreturn(int? officeid =0,string purposereturn = "",int? year=0,int mode=0)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_MAFreturn]  " + officeid + "," + year + ",'" + purposereturn.ToString().Replace("'","''") + "'," + mode + ","+ Account.UserInfo.eid +"", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch //(Exception ex)
            {
                return "";
            }
        }
        public PartialViewResult realignforposting()
        {
            return PartialView("pv_Realign_Office_Forposting");
        }

        public PartialViewResult realignForPosted()
        {
            return PartialView("pv_Realign_Office_PostedV2");
        }

        public JsonResult Load_Realign_ForPosting([DataSourceRequest] DataSourceRequest request, int? modeid, int? year,int? statusid,int? officeid)
        {

            List<Monthly_DD_Model> realign_List = new List<Monthly_DD_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec [sp_BMS_MafReview_forposting] " + modeid + "," + year + ","+ statusid + ","+ officeid + "", con);

                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_DD_Model real = new Monthly_DD_Model();
                    real.trnno = Convert.ToInt32(reader.GetValue(0));
                    real.FMISOfficeCode = Convert.ToInt32(reader.GetValue(1));
                    real.officename = reader.GetValue(2).ToString();
                    real.Amount = Convert.ToDouble(reader.GetValue(3));
                    real.datetimesubmit = reader.GetValue(4).ToString();
                    real.YearOf = Convert.ToInt32(reader.GetValue(5));
                    real.purpose = reader.GetValue(6).ToString();
                    real.officefullname = reader.GetValue(7).ToString();
                    real.mafno = reader.GetValue(8).ToString();
                    real.docid = Convert.ToInt64(reader.GetValue(9));
                    realign_List.Add(real);
                }
            }
            return Json(realign_List.ToDataSourceResult(request));
        }
    }
}


  