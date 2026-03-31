using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iFMIS_BMS.BusinessLayer.Layers;

namespace iFMIS_BMS.Controllers
{
    public class PYearController : Controller
    {
        public JsonResult GetPropYear()
        {
            dp_PorposalYearLayer ddl = new dp_PorposalYearLayer();
            var lst = ddl.ProposalYears();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        //DropDown for Homess

        public JsonResult GetPropYear2()
        {
            dp_PorposalYearLayer ddl = new dp_PorposalYearLayer();
            var lst = ddl.ProposalYears2();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPropYear3()
        {
            dp_PorposalYearLayer ddl = new dp_PorposalYearLayer();
            var lst = ddl.ProposalYears3();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult MAFYear()
        {
            dp_PorposalYearLayer ddl = new dp_PorposalYearLayer();
            var lst = ddl.MAFYear();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPropYear4()
        {
            dp_PorposalYearLayer ddl = new dp_PorposalYearLayer();
            var lst = ddl.ProposalYears4();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult WFPYears()
        {
            dp_PorposalYearLayer ddl = new dp_PorposalYearLayer();
            var lst = ddl.WFPYears();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Tyear_ict()
        {
            dp_PorposalYearLayer ddl = new dp_PorposalYearLayer();
            var lst = ddl.Tyear_ict();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UtilizationYear()
        {
            dp_PorposalYearLayer ddl = new dp_PorposalYearLayer();
            var lst = ddl.UtilizationYear();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PPAYear()
        {
            dp_PorposalYearLayer ddl = new dp_PorposalYearLayer();
            var lst = ddl.PPAYear();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult trustYear()
        {
            dp_PorposalYearLayer ddl = new dp_PorposalYearLayer();
            var lst = ddl.TRyear();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }


        public JsonResult boi_year()
        {
            dp_PorposalYearLayer ddl = new dp_PorposalYearLayer();
            var lst = ddl.boi_yearss();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProposalYear()
        {
            dp_PorposalYearLayer ddl = new dp_PorposalYearLayer();
            var lst = ddl.GetProposalYear();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPropYearFund()
        {
            dp_PorposalYearLayer ddl = new dp_PorposalYearLayer();
            var lst = ddl.GetPropYearFund();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetWFPYear()
        {
            dp_PorposalYearLayer ddl = new dp_PorposalYearLayer();
            var lst = ddl.GetWFPYear();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
    }
}