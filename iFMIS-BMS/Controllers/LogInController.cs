using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration;
using iFMIS_BMS.BusinessLayer.Layers;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using iFMIS_BMS.BusinessLayer.Models;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Data.SqlClient;
using iFMIS_BMS.BusinessLayer.Connector;


namespace iFMIS_BMS.Controllers
{
    public class LogInController : Controller
    {
       
        // GET: LogIn
        public ActionResult Index()
        {
         
            //if (Request.Cookies["User"] == null)
            //{
            //    var c = new HttpCookie("User");
            //    c.Expires = DateTime.Now.AddDays(-1);
            //    Response.Cookies.Add(c);
            //}
            return View();
        }
        public string Logout()
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                string UserEmail =  Account.UserInfo.emailaddress.ToString();
                SqlCommand query_time = new SqlCommand(@"SELECT SUBSTRING(CONVERT(varchar, getdate(), 109), 13, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 16, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 19, 2) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 100), 18, 2)", con);
                con.Open();
                SqlDataReader reader_time = query_time.ExecuteReader();
                var timeDate = "";
                while (reader_time.Read())
                {
                    timeDate = reader_time.GetString(0);
                }
                reader_time.Close();

                SqlCommand query_signout = new SqlCommand(@"UPDATE tbl_R_BMSUserLOG set Time_OUT = '" + timeDate + "' where User_Email ='" + UserEmail + "' and Date = Convert(date, getdate()) and Time_OUT = ''", con);
                
                query_signout.ExecuteNonQuery();            
            }
            FormsAuthentication.SignOut();
            return ("1");
        }

        public string CheckParameter(string completeEmail, string passcode, string ip, string hostname)
        {
            LoginLayer LogIn = new LoginLayer();
            UserModel User = LogIn.User(completeEmail, passcode, ip, hostname);

            if (User.Countopis == 1)
            {
                if (User.emailaddress == completeEmail && User.Passcode == passcode)
                {
                    FormsAuthentication.SetAuthCookie(User.eid.ToString(), true);
                    Response.SetAuthCookie<UserModel>(User.eid.ToString(), true, User);
                    return ("Success");
                }
                else if (User.emailaddress == "ServerErrorLog-in")
                {
                    return ("ServerErrorLog-in");
                }
                else
                {
                    return ("0");
                }

            }
            else
            {
                if (User.emailaddress == completeEmail && User.Passcode == passcode)
                {
                    return User.eid.ToString();
                }
                else if (User.emailaddress == "ServerErrorLog-in")
                {
                    return ("ServerErrorLog-in");
                }
                else
                {
                    return ("0");
                }
            }
        }
        //public string _MyInfo()
        //{
        //    return (Account.UserInfo.empName);
        //}
        public string _MyImage()
        {
            //HttpWebResponse response = null;
            //var request = (HttpWebRequest)WebRequest.Create("http://121.97.216.184:82/idprod/PGAS/" + Account.UserInfo.imgName);
            //request.Method = "HEAD";
            //try
            //{
            //    response = (HttpWebResponse)request.GetResponse();
            //    return ("http://121.97.216.184:82/idprod/PGAS/" + Account.UserInfo.imgName);
            //}
            //catch (WebException)
            //{
                /* A WebException will be thrown if the status of the response is not `200 OK` */
                return ("/IFMIS_BMS/Content/images/no_avatar_available.jpg");
            //}            
        }
        public JsonResult LoginHistory([DataSourceRequest] DataSourceRequest request, int? UserID)
        {
            LoginLayer el = new LoginLayer();
            var lst = el.grLoginHistory(UserID);
            return Json(lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }



        public string CheckParameterOF(string completeEmail, string passcode, string ip, string hostname, string OfficeID, int? userID)
        {
            LoginLayer LogIn = new LoginLayer();
            UserModel User = LogIn.UserOF(completeEmail, passcode, ip, hostname,OfficeID);

            if (User.emailaddress == completeEmail && User.Passcode == passcode)
            {
                FormsAuthentication.SetAuthCookie(User.eid.ToString(), true);
                Response.SetAuthCookie<UserModel>(User.eid.ToString(), true, User);
                return ("1");
            }
            else if (User.emailaddress == "ServerErrorLog-in")
            {
                return ("ServerErrorLog-in");
            }
            else
            {
                return ("0");
            }
        }

        public JsonResult ddlOfficeOF(int? eid)
        {
            DropDownLayer ddl = new DropDownLayer();
            var lst = ddl.OfficesOF(eid);

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        //public string loginfooteraddress()
        //{
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(Common.MyConn()))
        //        {
        //            var data = "";
        //            SqlCommand com = new SqlCommand("Select top 1 [lgu] from [IFMIS].[dbo].[tbl_R_BMSLgu] where actioncode=1 order by [id]", con);
        //            con.Open();
        //            data = Convert.ToString(com.ExecuteScalar());
        //            return data;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message.ToString();
        //    }
        //}

        public JsonResult loginfooteraddress()
        {

            UserLogInfo data = new UserLogInfo();
            DataTable _dt = new DataTable();
            string _sqlQuery = "";
           
            _sqlQuery = "Select top 1 [lgu],province,lgu_site,lgu_abbr from [IFMIS].[dbo].[tbl_R_BMSLgu] where actioncode=1 order by [id]";
            
            _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
            try
            {
                data.lgu = Convert.ToString(_dt.Rows[0][0]);
                data.lgu_province = Convert.ToString(_dt.Rows[0][1]);
                data.lgu_site = Convert.ToString(_dt.Rows[0][2]);
                data.lgu_abbr = Convert.ToString(_dt.Rows[0][3]);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult gettemplatecolor_login()
        {

            UserLogInfo data = new UserLogInfo();
            DataTable _dt = new DataTable();
            string _sqlQuery = "";

            _sqlQuery = "SELECT [Color] FROM [IFMIS].[dbo].[tbl_R_BMS_TemplateColor] where Actioncode=1";

            _dt = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlQuery).Tables[0];
            try
            {
                data.tempcolor = Convert.ToString(_dt.Rows[0][0]);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public string gettemplatecolor()
        {
            DataTable color_id = new DataTable();
            var tempcolor = "";
            string _sqlprep = "SELECT [Color] FROM [IFMIS].[dbo].[tbl_R_BMS_TemplateColor] where Actioncode=1";
            color_id = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), System.Data.CommandType.Text, _sqlprep).Tables[0];
            if (color_id.Rows.Count == 1)
            {
                tempcolor = Convert.ToString(color_id.Rows[0][0]);
            }
            else
            {
                tempcolor = "#1fa67a";
            }
            return tempcolor;

        }

    }
}