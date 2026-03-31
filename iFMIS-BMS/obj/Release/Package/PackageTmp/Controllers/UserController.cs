using Kendo.Mvc.UI;
using iFMIS_BMS.BusinessLayer.Layers;
using iFMIS_BMS.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iFMIS_BMS.Reports;
using Kendo.Mvc.Extensions;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using iFMIS_BMS.BusinessLayer.Connector;

namespace iFMIS_BMS.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            var status = "";
            MenuLayer Menu = new MenuLayer();
            var mnu = Menu.Menu(Account.UserInfo.eid.ToString(), Account.UserInfo.Department.ToString(),Account.UserInfo.UserTypeDesc.ToString());
            
            foreach (var item in mnu)
            {
                if (item.MenuID == 2003)
                {
                    status = "Authorized";
                }
            }
            if (status == "Authorized")
            {
                return View();
            }
            else
            {
                return View("_UnAuthorizedAccess");
            }
        }

        public ActionResult User_NationalOffice()
        {
            return View("UserNO");
        }
        public JsonResult ddlEmployee()
        {
            DropDownLayer ddl = new DropDownLayer();
            var lst = ddl.Users();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlEmployeeForOrdinanceAttendance()
        {
            DropDownLayer ddl = new DropDownLayer();
            var lst = ddl.ddlEmployeeForOrdinanceAttendance();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlOrdinanceAttendanceDesignation()
        {
            DropDownLayer ddl = new DropDownLayer();
            var lst = ddl.ddlOrdinanceAttendanceDesignation();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult ddlEmployeeCascade(int OfficeID)
        {
            DropDownLayer ddl = new DropDownLayer();
            var lst = ddl.UsersPerOffice(OfficeID);

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlUserType()
        {
            DropDownLayer ddl = new DropDownLayer();
            var lst = ddl.UserType();

            return Json(lst, JsonRequestBehavior.AllowGet);
        } 








        public JsonResult ddlOffice(int? eid)
        {
            DropDownLayer ddl = new DropDownLayer();
            var lst = ddl.OfficesUser(eid);

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlOfficeUP()
        {
            DropDownLayer ddl = new DropDownLayer();
            var lst = ddl.Offices();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ddlOfficeWithAll()
        {
            DropDownLayer ddl = new DropDownLayer();
            var lst = ddl.ddlOfficeWithAll();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public int CheckOpis(string eid,int OfficeID)
        {
            DropDownLayer ddl = new DropDownLayer();

            int usr = ddl.checkOPISUser(eid);

            if (usr!=OfficeID)
            {
                return usr = 0;
            }
            else
            {
                return usr;
            }
        }





        public int CheckIFMISUsers(string eid)
        {
            DropDownLayer ddl = new DropDownLayer();

            int usr = ddl.checkIFMISUser(eid);

            if (usr == 0)
            {
                return usr=0;
            }
            else
            {
                return usr;
            }
        }
        public int getOffice(string eid)
        {
            DropDownLayer ddl = new DropDownLayer();
            int usr = ddl.checkIFMISUser(eid);
           return usr;
        }
        public JsonResult GetUserMenu(string eid, int OfficeID, int UserTypeID)
        {
            UserRoleLayer UsrMenu = new UserRoleLayer();
            var mnu = UsrMenu.UserMenu(eid, OfficeID, UserTypeID);
            List<int> MenuIds = new List<int>();
            foreach (var u in mnu)
            {
                MenuIds.Add(u.MenuID);
            }
            return Json(MenuIds, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUserTypeMenu(string UserTypeID)
        {
            UserRoleLayer UsrMenu = new UserRoleLayer();
            var mnu = UsrMenu.UserTypeMenu(UserTypeID);
            List<int> MenuIds = new List<int>();
            foreach (var u in mnu)
            {
                MenuIds.Add(u.MenuID);
            }
            return Json(MenuIds, JsonRequestBehavior.AllowGet);
        }
        public string SaveUserMenu(int[] MenuID, string eid, string OfficeID)
        {
            UserRoleLayer SaveMenu = new UserRoleLayer();
            return SaveMenu.SaveUserMenu(MenuID, eid, OfficeID);
        }
        public string SaveUser(string UserTypeID, string eid, string OfficeID, int[] MenuIDs, string Mode)
        {
            
            UserRoleLayer Save = new UserRoleLayer();
            return Save.SaveUser(UserTypeID, eid, OfficeID, MenuIDs, Mode);
        }
        public void DeleteUserMenu(string eid, string OfficeID)
        {
            UserRoleLayer UsrRoleLayer = new UserRoleLayer();
            UsrRoleLayer.DeleteUserMenu(eid, OfficeID);
        }
        public string UpdateUser(string UserTypeID, string eid, string OfficeID)
        {
            UserRoleLayer Update = new UserRoleLayer();
            return Update.UpdateUser(UserTypeID, eid, OfficeID);
        }
        public string RemoveUser(string eid, string IFMISOfficeID)
        {
            UserRoleLayer UsrRoleLayer = new UserRoleLayer();
            UsrRoleLayer.DeleteUserMenu(eid, IFMISOfficeID);
            return UsrRoleLayer.deleteUser(eid, IFMISOfficeID);
        }


        public PartialViewResult pv_AddOfficeViews(int? User_ID=0, int? eid=0,int? officeid=0,int? UserTypeID=0)
        {
            Session["User_ID"] = User_ID;
            Session["eid"] = eid;
            Session["officeid"] = officeid;
            Session["UserTypeID"] = UserTypeID;
            return PartialView("pv_AddOfficeViews");
        }

        public JsonResult ddlEmployee2(int? eid)
        {
            DropDownLayer ddl = new DropDownLayer();
            var lst = ddl.Users2(eid);

            return Json(lst, JsonRequestBehavior.AllowGet);
        }


        public JsonResult LoadUserOffice([DataSourceRequest] DataSourceRequest request, int? eid)

        {
            if (eid == null)
            {
                eid = 0;
                UserRoleLayer NewIS = new UserRoleLayer();
                var lst = NewIS.UserOffice(eid);

                return Json(lst.ToDataSourceResult(request));
            }
            else
            {
                UserRoleLayer NewIS = new UserRoleLayer();
                var lst = NewIS.UserOffice(eid);

                return Json(lst.ToDataSourceResult(request));
            }
        }
        public JsonResult LoadUserOfficeNO([DataSourceRequest] DataSourceRequest request)

        {
           
                UserRoleLayer NewIS = new UserRoleLayer();
                var lst = NewIS.UserOfficeNO();

                return Json(lst.ToDataSourceResult(request));
            
        }

        public PartialViewResult EditUser(int? User_ID)
        {
            UserRoleLayer NewTrunds = new UserRoleLayer();
            return PartialView("pv_UpdateOUViews", NewTrunds.EditTrunds(User_ID));
        }

        [HttpPost]
        public string UpdateOU(UserOfficeRole OU)
        {
            UserRoleLayer el = new UserRoleLayer();

            return el.UpdateOU(OU);
        }



        public int UserID(string eid)
        {
            DropDownLayer ddl = new DropDownLayer();
            int usr = ddl.userid(eid);
            return usr;
        }


        public string RemoveUserNew(int? User_ID, int? eid)
        {
            UserRoleLayer NewIS = new UserRoleLayer();
            var lst = NewIS.RemoveUser(User_ID, eid);
            return lst;
        }
        public string RemoveUserNewNO( int? eid)
        {
            UserRoleLayer NewIS = new UserRoleLayer();
            var lst = NewIS.RemoveUserNO( eid);
            return lst;
        }
        public string EnableSubmit(int? enablesubmit = 0)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var data = "";
                SqlCommand com = new SqlCommand(@"Update [ifmis].[dbo].[tbl_R_BMSubmitbtn] set [EnableSubmit]=" + enablesubmit + " ", con);
                con.Open();
                data = Convert.ToString(com.ExecuteScalar());
                return data;
            }
        }

        public string EnableSubmitWFP(int? enablesubmit = 0)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var data = "";
                SqlCommand com = new SqlCommand(@"Update [ifmis].[dbo].[tbl_R_BMSWFPSubmit] set [submit]=" + enablesubmit + " where yearof=2025 ", con);
                con.Open();
                data = Convert.ToString(com.ExecuteScalar());
                return data;
            }
        }
        public string EnableExecution(int? enablexecution=0)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var data = "";
                SqlCommand com = new SqlCommand(@"Update [ifmis].[dbo].[tbl_R_BMSMenu] set [ActionCode]=" + enablexecution + " where MenuID=13062 ", con);
                con.Open();
                data = Convert.ToString(com.ExecuteScalar());
                return data;
            }
        }
        public string UpdateUserPwd(int? eid=0,string origpwd="", string oldpwd="",string newpwd="",string pwdverify="", string firstname = "", string lastname = "", string officehead = "", string officeheadposition = "")
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var data = "";
                var stat = "";
                if (origpwd != oldpwd)
                {
                    data = "Incorrect password! Please try again.";
                }
                else if (newpwd != pwdverify)
                {
                    data = "Password doesn't match!";
                }
                else{
                    SqlCommand com = new SqlCommand(@"Update [ifmis].[dbo].[tbl_T_BMSNO_user] set [passcode]='"+ pwdverify + "',[depthead]='"+ officehead + "',[deptheadposition]='"+ officeheadposition.Replace("'","''").ToString() + "',[fname]='"+ firstname + "',[lname]='"+ lastname + "',[username]='"+ firstname.ToLower() + "' + '.' + '"+ lastname.ToLower() + "' + '@pgas.ph' where [eid]=" + eid + " and isnull([blocked],0)=0", con);
                    con.Open();
                    stat = Convert.ToString(com.ExecuteScalar());
                    data = "success";
                }
                return data;
            }
        }
        public string AddUserNO(string newpwd = "", string firstname = "", string lastname = "", string officehead = "", string officeheadposition = "")
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var data = "";
                SqlCommand com = new SqlCommand(@"exec sp_BMS_UserNatOffice '" + newpwd + "','" + firstname + "','" + lastname + "','" + officehead + "','" + officeheadposition.Replace("'", "''").ToString() + "'", con);
                con.Open();
                data = Convert.ToString(com.ExecuteScalar());
                return data;
            }
        }
    }
}