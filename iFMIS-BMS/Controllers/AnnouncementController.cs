using iFMIS_BMS.BusinessLayer.Connector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iFMIS_BMS.Controllers
{
    public class AnnouncementController : Controller
    {
        // GET: Announcement
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public string getAnnouncement() { 
         using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select isnull((select top 1 AnnouncementTitle + '(J$()-XD=3h4N' + Announcement from ifmis.dbo.tbl_R_BMSAnnouncement where ActionCode = 1 and EndTime > GETDATE()),'0')", con);
                con.Open();
                return com.ExecuteScalar().ToString();
            }
        }
    }
}