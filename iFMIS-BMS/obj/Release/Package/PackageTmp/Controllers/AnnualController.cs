using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Layers;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;

namespace iFMIS_BMS.Controllers
{    
    [Authorize]
    public class AnnualController : Controller
    {
        public ActionResult Index1()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(), Account.UserInfo.UserTypeID.ToString());
            foreach (var item in mnu)
            {
                if (item.MenuID == 4)
                {
                    status = "Authorized";
                }
            }
            if (status == "Authorized")
            {
                if (Account.UserInfo.UserTypeDesc == "Budget In-Charge")
                {
                    return View("pv_Home");
                }
                else
                {
                    //  return RedirectToAction("pv_Home", "Annual");
                    return View("pv_Home3");
                }
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }
        public PartialViewResult newAccounts()
        {
            return PartialView("vwNewAccounts");
        }
        public PartialViewResult SuggestedAccounts(int OfficeID, int ProgramID)
        {
            programs data = new programs
            {
                program_id = ProgramID,
                office_id = OfficeID
            };
            return PartialView("vwSuggestedAccounts", data);
        }
        public JsonResult Programs_read1([DataSourceRequest] DataSourceRequest request, int? propYear1)
        {
            home1_Layer el = new home1_Layer();
            var lst = el.gPrograms( propYear1);

            return Json(lst.ToDataSourceResult(request));
        }



        public ActionResult pv_Home()
        {
            return View("pv_Home");
        }


        public JsonResult Home_read([DataSourceRequest] DataSourceRequest request)
        {
            Home_Layer el = new Home_Layer();
            var lst = el.Programss();

            return Json(lst.ToDataSourceResult(request));
        }

        [HttpGet]
        public PartialViewResult _edithomeprog(int ProgramID)
        {
            Session["ProgID1"] = ProgramID;
            return PartialView("pv_Home2");
        }



        public PartialViewResult pv_Home2()
        {
            return PartialView("pv_Home2");
        }


        public JsonResult Prog_read1([DataSourceRequest] DataSourceRequest request, int? propYear1)
        {
            EditHome2_Layer el = new EditHome2_Layer();
            var lst = el.Programss(Convert.ToInt32(Session["ProgID1"]), propYear1);

            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

         [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult _updateHome2([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<AccountsModel> Accounts)
        {
            EditHome2_Layer el = new EditHome2_Layer();
            try
            {
                el.UpdateAccountHome2(Accounts);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(Accounts.ToDataSourceResult(request, ModelState));
        }

        //this is for Budget and HR

        [HttpGet]
        public PartialViewResult _editProg(int ProgramID)
         {
             //DropDownLayer ddl = new DropDownLayer();
             //ViewData["Account"] = ddl.acc();
             //ViewData["Funds"] = ddl.funds();
             //ViewData["Ooe"] = ddl.OOE();
             //ViewData["Years"] = ddl.Year();
            Session["ProgID2"] = ProgramID;
            
            return PartialView("pv_Home4");
        }


        public ActionResult pv_Home3()
        {
            return View("pv_Home3");
        }

        public PartialViewResult pv_Home4()
        {

            return PartialView("pv_Home4");
        }

        public JsonResult Programs_read2([DataSourceRequest] DataSourceRequest request, int? off_ID, int? propYear)
        {
            HRbgtAP_Layer el = new HRbgtAP_Layer();
            var lst = el.gPrograms(off_ID, propYear);

            return Json(lst.ToDataSourceResult(request));
        }


        public JsonResult Prog_read([DataSourceRequest] DataSourceRequest request, int? off_ID, int? propYear)
        {
            EditHome4_Layer el = new EditHome4_Layer();
            var lst = el.Programss(Convert.ToInt32(Session["ProgID2"]), off_ID, propYear);

            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }



        public ActionResult _updateHome4([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<AccountsModel> ABAccounts)
        {
            EditHome4_Layer el = new EditHome4_Layer();
            try
            {
                el.UpdateAccountHome4(ABAccounts);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(ABAccounts.ToDataSourceResult(request, ModelState));
        }
       

    }
}