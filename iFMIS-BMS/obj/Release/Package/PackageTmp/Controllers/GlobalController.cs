using iFMIS_BMS.BusinessLayer.Connector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iFMIS_BMS.Controllers
{
    public class GlobalController : Controller
    {
        // GET: Global
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public string CurrentDate(){
            var returnDate = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var statement = "SELECT dbo.fn_BMS_DateString()";
                SqlCommand query = new SqlCommand(statement, con);
                con.Open();
                returnDate = Convert.ToString(query.ExecuteScalar());
            }
            return returnDate;
        }
        public int CurrentYear()
        {
            var returnDate = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var statement = "SELECT year(getdate())";
                SqlCommand query = new SqlCommand(statement, con);
                con.Open();
                returnDate = Convert.ToInt32(query.ExecuteScalar());
            }
            return returnDate;
        }
        public string CurrentMonth()
        {
            var returnMonth = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var statement = "SELECT month(getdate())";
                SqlCommand query = new SqlCommand(statement, con);
                con.Open();
                returnMonth = Convert.ToString(query.ExecuteScalar());
            }
            return returnMonth;
        }
        public int NextYear()
        {
            var returnDate = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var statement = "select top 1 trnYear from  [IFMIS].[dbo].[tbl_R_BMSTransYear_Release] order by [trnYear] desc";
                SqlCommand query = new SqlCommand(statement, con);
                con.Open();
                returnDate = Convert.ToInt32(query.ExecuteScalar());
            }
            return returnDate;
        }
    }
}