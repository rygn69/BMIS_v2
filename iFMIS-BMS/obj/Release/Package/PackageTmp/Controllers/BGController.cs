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

namespace iFMIS_BMS.Controllers
{
    public class BGController : Controller
    {
         
        public PartialViewResult pv_BGAPA()
        {
            return PartialView("pv_BGAPA");
        }


        public JsonResult Programs_read([DataSourceRequest] DataSourceRequest request, int prog_ID, int propYear, int office_ID)
        {
            BGAPALayer el = new BGAPALayer();
            var lst = el.Programss(prog_ID,propYear,office_ID);

            return Json(lst.ToDataSourceResult(request));
        }


        public JsonResult _update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<AccountsModel> Accounts)
        {
            BGAPALayer el = new BGAPALayer();
            try
            {
                el.UpdateAccount(Accounts);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(Accounts.ToDataSourceResult(request, ModelState));
        }
    }
}