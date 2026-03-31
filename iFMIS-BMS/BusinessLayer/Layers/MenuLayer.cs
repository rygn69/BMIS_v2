using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iFMIS_BMS.BusinessLayer.Models;
using System.Data.SqlClient;
using iFMIS_BMS.BusinessLayer.Connector;

namespace iFMIS_BMS.BusinessLayer.Layers
{
    public class MenuLayer
    {
        public IEnumerable<MenuModel> Menu(string userID,string officeID,string userDesc)
        {
            List<MenuModel> MenuList = new List<MenuModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
//                SqlCommand com = new SqlCommand(@"select distinct a.* from tbl_R_BMSMenu as a LEFT 
//                JOIN tbl_R_BMSUserMenu as b on a.MenuID = b.MenuID 
//                LEFT JOin tbl_R_BMSUsers as c on c.IFMISOfficeID = b.OfficeID and   c.eid = b.UserID
//                where b.UserID = '" + usserID + "'and b.OfficeID = " + officeID 
//                + " and c.UserTypeID = " + Account.UserInfo.UserTypeID + " ORDER BY a.MenuOrder", con);
                SqlCommand com = new SqlCommand(@"select distinct a.* from tbl_R_BMSMenu as a LEFT 
                JOIN tbl_R_BMSUserMenu as b on a.MenuID = b.MenuID 
                where a.ActionCode = 1 and b.UserID = '" + userID + "'and b.OfficeID = " + officeID + " and [ISId]=1 ORDER BY a.MenuOrder", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    
                    MenuModel mnu = new MenuModel();
                    mnu.MenuID = reader.GetInt32(0);
                    mnu.MenuParent = reader.GetInt32(1);
                    mnu.MenuLevel = reader.GetInt32(2);
                    mnu.MenuName = reader.GetString(3);
                    mnu.MenuDescription = reader.GetValue(4).ToString();
                    mnu.MenuIcon = reader.GetString(5);
                    mnu.MenuController = reader.GetValue(6).ToString();
                    mnu.MenuAction = reader.GetValue(7).ToString();

                    MenuList.Add(mnu);
                }
            }
            return MenuList;
        }



        public string MonthValue()
        {
            
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT DATEPART(m, getdate())", con);
                con.Open();
                return com.ExecuteScalar().ToString();


            }
        }
        public IEnumerable<MenuModel> getMenuList(int? User_ID = 0, int? eid = 0)
        {
            List<MenuModel> MenuList = new List<MenuModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                //SqlCommand com = new SqlCommand(@"with cte as(
                //                                select a.MenuID,b.MenuParent as ParentID,
                //                                b.MenuName as MenuParent
                //                                , a.MenuName,
                //                                rn = ROW_NUMBER()OVER(PARTITION BY a.MenuParent ORDER BY b.MenuName, a.MenuOrder desc),a.MenuOrder
                //                                from tbl_R_BMSMenu as a
                //                                LEFT JOIN tbl_R_BMSMenu as b on b.MenuID = a.MenuParent where a.ActionCode=1 and b.ActionCode=1
                //                                )
                //                                select MenuID,ParentID,MenuName from cte where (rn = 1 and MenuID = ParentID) or (menuID != ParentID) order by MenuName", con);
                SqlCommand com = new SqlCommand(@"with cte as(
                                    select distinct a.MenuID,b.MenuParent as ParentID,
                                    b.MenuName as MenuParent
                                    , a.MenuName,isnull(c.menuid,0) as Access,
                                    rn = ROW_NUMBER()OVER(PARTITION BY a.MenuParent ORDER BY b.MenuName, a.MenuOrder desc),a.MenuOrder
                                    from tbl_R_BMSMenu as a
                                    LEFT JOIN tbl_R_BMSMenu as b on b.MenuID = a.MenuParent
	                                left join  tbl_R_BMSUserMenu as c on c.menuid=a.MenuID and c.OfficeID in (select IFMISOfficeID from dbo.tbl_R_BMSUsers where UserID = "+ User_ID + " and eid = " + eid + ") and c.userid=" + eid + ""+
	                                "where a.ActionCode=1 and b.ActionCode=1 "+
                                    ") "+
                                    "select distinct MenuID,ParentID,MenuName,Access from cte where (rn = 1 and MenuID = ParentID) or (menuID != ParentID) order by MenuName", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    
                    MenuModel mnu = new MenuModel();
                    mnu.MenuID = reader.GetInt32(0);
                    mnu.MenuParent = reader.GetInt32(1);
                    mnu.MenuName = reader.GetValue(2).ToString();
                    mnu.access = reader.GetInt64(3);
                    MenuList.Add(mnu);
                }
            }
            return MenuList;
        }
        
    }
}