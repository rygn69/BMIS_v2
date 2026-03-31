using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iFMIS_BMS.BusinessLayer.Layers;

namespace iFMIS_BMS.Controllers
{
    public class dpOfficeController : Controller
    {
        public JsonResult GetOffice()
        {
            DropDownLayer ddl = new DropDownLayer();
            var lst = ddl.Offices();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOffice2()
        {
            dpOfficeLayer ddl = new dpOfficeLayer();
            var lst = ddl.Offices2();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOffice_AIPVerse(int? regularaipid, int? yearof)
        {
            DropDownLayer ddl = new DropDownLayer();
            var lst = ddl.GetOffice_AIPVerse(regularaipid, yearof);

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
    }
}