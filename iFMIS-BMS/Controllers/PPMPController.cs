using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iFMIS_BMS.PPNP;
using System.Data;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using iFMIS_BMS.BusinessLayer.Layers.Maintenance;

namespace iFMIS_BMS.Controllers
{
    public class PPMPController : Controller
    {
        // GET: PPMP
        serviceSoapClient PPMPdata = new serviceSoapClient();
        public ActionResult Index()
        {   
            return View("vwPPMP");
        }
        public JsonResult PPMP_DataTable([DataSourceRequest] DataSourceRequest request, int year = 2017, int officeid = 72, int programid = 4, int accountid = 969)
        {
            PPMPdata_Layer item_list = new PPMPdata_Layer();
            var data_layer = item_list.PPMP_data(year, officeid, programid, accountid);
            return Json(data_layer.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        //public DataTable PPMP_data(int year = 2016, int officeid = 72, int programid = 4, int accountid = 969)
        //{
        //    var dt = PPMPdata.PPMPItems(year, officeid, programid, accountid);
        //    dt.TableName = "myTable";
        //    return dt;
        //}
    }
}