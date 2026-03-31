using iFMIS_BMS.BusinessLayer.Layers.BudgetControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iFMIS_BMS.Controllers
{
    [Authorize]
    public class OBRController : Controller
    {
        // GET: OBR
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult CheckInOBR(int? FundID, string UserInTimeStamp, int? UserID, string RefNo, int isPastYear,string cttsno,string obrno,int? chkcafoa,int? tyear,int? officeassign)
        {
            OBR_Layer data = new OBR_Layer();
            var lst = data.CheckInOBR(FundID, UserInTimeStamp, UserID, RefNo, isPastYear, cttsno, obrno, chkcafoa, tyear, officeassign);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
    }
}