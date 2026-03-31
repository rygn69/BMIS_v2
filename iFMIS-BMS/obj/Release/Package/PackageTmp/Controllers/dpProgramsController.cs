using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iFMIS_BMS.BusinessLayer.Layers;

namespace iFMIS_BMS.Controllers
{
    public class dpProgramsController : Controller
    {
        public JsonResult GetPrograms(int? propYear)
        {
            dpPrograms_Layer ddl = new dpPrograms_Layer();
            var lst = ddl.Appointments(propYear);

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
    }
}