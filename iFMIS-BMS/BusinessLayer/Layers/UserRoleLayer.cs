using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Connector;
using System.Data.SqlClient;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;
using System.Data;
using System.Collections;

namespace iFMIS_BMS.BusinessLayer.Layers
{
    public class UserRoleLayer
    {

        public IEnumerable<UserRolesModel> UserMenu(string eid, int OfficeID, int UserTypeID)
        {
            List<UserRolesModel> UserMenuList = new List<UserRolesModel>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select a.* from tbl_R_BMSUserMenu as a
                                                LEFT JOIN tbl_R_BMSUsers as b on b.IFMISOfficeID = a.OfficeID  and a.UserID = b.eid
                                                where a.OfficeID = " + OfficeID + " and a.UserID = " + eid + " and b.UserTypeID = " + UserTypeID + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    UserRolesModel usr = new UserRolesModel();
                    usr.UserMenuID = Convert.ToInt32(reader.GetValue(0));
                    usr.UserID = Convert.ToInt32(reader.GetValue(1));
                    usr.MenuID = Convert.ToInt32(reader.GetValue(2));

                    UserMenuList.Add(usr);
                }

            }
            return UserMenuList;
        }
        public IEnumerable<UserRolesModel> UserTypeMenu(string UserTypeID)
        {
            List<UserRolesModel> UserMenuList = new List<UserRolesModel>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select MenuID from tbl_R_BMSUserTypeRole where UserTypeID='" + UserTypeID + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    UserRolesModel usr = new UserRolesModel();
                    usr.MenuID = reader.GetInt32(0);

                    UserMenuList.Add(usr);
                }

            }
            return UserMenuList;
        }
        public string SaveUserMenu(int[] MenuID, string eid, string OfficeID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                foreach (var item in MenuID)
                {
                    SqlCommand com = new SqlCommand("insert into tbl_R_BMSUserMenu(UserID, MenuID, OfficeID) values(" + eid + "," + item + "," + OfficeID + ")", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
                }
                return "1";
            }
        }
        public string SaveUser(string UserTypeID, string eid, string OfficeID, int[] MenuIDs, string Mode)
        {
            var DistictIDs = new ArrayList(MenuIDs.Distinct().ToArray());
            if (Mode == "Add")
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_DeleteUserAccount] " + eid + ","+ OfficeID + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    con.Close();

                    SqlCommand ComInsertuser = new SqlCommand("insert into tbl_R_BMSUsers (eid,UserTypeID,IFMISOfficeID) values(" + eid + "," + UserTypeID + "," + OfficeID + ")", con);
                    try
                    {
                        con.Open();
                        ComInsertuser.ExecuteNonQuery();
                        foreach (var ID in DistictIDs)
                        {
                            SqlCommand comInsertMenu = new SqlCommand("insert into tbl_R_BMSUserMenu(UserID, MenuID, OfficeID) values(" + eid + "," + ID + "," + OfficeID + ")", con);
                            comInsertMenu.ExecuteNonQuery();
                        }
                        return "1";
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }    
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    //try
                    //{
                    //    foreach (var ID in DistictIDs)
                    //    {
                    //        SqlCommand comInsertMenu = new SqlCommand("insert into tbl_R_BMSUserMenu(UserID, MenuID, OfficeID) values(" + eid + "," + ID + "," + OfficeID + ")", con);
                    //        comInsertMenu.ExecuteNonQuery();
                    //    }
                    //    return "2";
                    //}
                    //catch (Exception ex)
                    //{
                    //    return ex.Message;
                    //}
                    return "2";
                }    
            }
            
        }
        public void DeleteUserMenu(string eid, string OfficeID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("delete from tbl_R_BMSUserMenu where UserID='" + eid + "' and OfficeID = '" + OfficeID + "'", con);
                con.Open();
                com.ExecuteNonQuery();
            }
        }
        public string deleteUser(string eid, string OfficeID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("delete from tbl_R_BMSUsers where eid='" + eid + "' and IFMISOfficeID = '" + OfficeID + "'", con);
                try
                {
                    con.Open();
                    com.ExecuteNonQuery();
                    return "1";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public string UpdateUser(string UserTypeID, string eid,string OfficeID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("update tbl_R_BMSUsers set UserTypeID=" + UserTypeID + ", IFMISOfficeID=" + OfficeID + " where eid=" + eid, con);
                try
                {
                    con.Open();
                    com.ExecuteNonQuery();
                    return "1";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public AdditionalRulesModel AdditionalRules(string UserTypeID)
        {
            AdditionalRulesModel AdditionalRulesModel = new AdditionalRulesModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select CanReviewPS, CanReviewMOOE, CanReviewCO, CanReviewFE from tbl_R_BMSUserTypes where UserTypeID='" + UserTypeID + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AdditionalRulesModel.canReviewPS = reader.GetInt16(0);
                    AdditionalRulesModel.canReviewMOOE = reader.GetInt16(1);
                    AdditionalRulesModel.canReviewCO = reader.GetInt16(2);
                    AdditionalRulesModel.canReviewFE = reader.GetInt16(3);

                }

            }
            return AdditionalRulesModel;
        }
        public string UpdateUsertypeRule(int[] MenuIDs, int UsertypeID)
        {
            AdditionalRulesModel AdditionalRulesModel = new AdditionalRulesModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                try
                {
                    SqlCommand com = new SqlCommand(@"delete from tbl_R_BMSUserTypeRole where UserTypeID=" + UsertypeID+"", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
                    foreach (var MenuID in MenuIDs)
                    {
                        
                        SqlCommand com2 = new SqlCommand(@"insert into tbl_R_BMSUserTypeRole values('" + UsertypeID + "','"+MenuID+"')", con);
                        con.Open();
                        com2.ExecuteNonQuery();
                        con.Close();
                    }
                    return "1";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }   
            }
           
        }
        public string UpdateUserType(int UsertypeID, int PS, int MOOE, int CO, int FE)
        {
            AdditionalRulesModel AdditionalRulesModel = new AdditionalRulesModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                try
                {
                   
                        SqlCommand com = new SqlCommand(@"Update tbl_R_BMSUserTypes set CanReviewPS=" + PS + ",CanReviewMOOE=" + MOOE 
                            + ", CanReviewCO=" + CO + ",CanReviewFE=" + FE + " where UserTypeID=" + UsertypeID + "", con);
                        con.Open();
                        com.ExecuteNonQuery();
                    return "1";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }

        }

        public IEnumerable<UserOfficeRole> UserOffice(int? eid)
        {
            List<UserOfficeRole> OfficeUserList = new List<UserOfficeRole>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT a.UserID,a.eid, CONCAT(b.OfficeName,' (',REPLACE(b.OfficeAbbrivation, ' ', ''),')'),c.UserTypeDesc,b.OfficeID,a.UserTypeID FROM dbo.tbl_R_BMSUsers as a inner join dbo.tbl_R_BMSOffices as b on a.IFMISOfficeID = b.OfficeID inner join dbo.tbl_R_BMSUserTypes as c on a.UserTypeID = c.UserTypeID where a.eid =" + eid, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    UserOfficeRole Trunds = new UserOfficeRole();
                    Trunds.User_ID = Convert.ToInt32(reader.GetValue(0));
                    Trunds.eid = Convert.ToInt32(reader.GetValue(1));
                    Trunds.Office_Name = reader.GetValue(2).ToString();
                    Trunds.UserTypeDesc = reader.GetValue(3).ToString();
                    Trunds.IFMISOfficeID= Convert.ToInt32(reader.GetValue(4));
                    Trunds.UserTypeID = Convert.ToInt32(reader.GetValue(5));

                    OfficeUserList.Add(Trunds);
                }

            }
            return OfficeUserList;
        }

        public IEnumerable<UserOfficeRole> UserOfficeNO()
        {
            List<UserOfficeRole> OfficeUserList = new List<UserOfficeRole>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (Account.UserInfo.eid == 1299) {
                    SqlCommand com = new SqlCommand(@"SELECT eid,lname,fname,passcode,[depthead],[deptheadposition]  FROM ifmis.dbo.tbl_T_BMSNO_user where isnull(blocked,0)=0 order by lname", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        UserOfficeRole Trunds = new UserOfficeRole();

                        Trunds.eid = Convert.ToInt32(reader.GetValue(0));
                        Trunds.lname = reader.GetValue(1).ToString();
                        Trunds.fname = reader.GetValue(2).ToString();
                        Trunds.pcode = reader.GetValue(3).ToString();
                        Trunds.depthead = reader.GetValue(4).ToString();
                        Trunds.deptheadposition = reader.GetValue(5).ToString();

                        OfficeUserList.Add(Trunds);
                    }
                }
                else
                {
                    SqlCommand com = new SqlCommand(@"SELECT eid,lname,fname,passcode,[depthead],[deptheadposition]  FROM ifmis.dbo.tbl_T_BMSNO_user where eid=" + Account.UserInfo.eid + " and isnull(blocked,0)=0 order by lname", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        UserOfficeRole Trunds = new UserOfficeRole();

                        Trunds.eid = Convert.ToInt32(reader.GetValue(0));
                        Trunds.lname = reader.GetValue(1).ToString();
                        Trunds.fname = reader.GetValue(2).ToString();
                        Trunds.pcode = reader.GetValue(3).ToString();
                        Trunds.depthead = reader.GetValue(4).ToString();
                        Trunds.deptheadposition = reader.GetValue(5).ToString();

                        OfficeUserList.Add(Trunds);
                    }
                }

            }
            return OfficeUserList;
        }

        public UserOfficeRole EditTrunds(int? User_ID)
        {
            UserOfficeRole OUList = new UserOfficeRole();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"SELECT UserID,eid,UserTypeID,IFMISOfficeID FROM dbo.tbl_R_BMSUsers where UserID =" + User_ID + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    OUList.User_ID = Convert.ToInt32(reader.GetValue(0));
                    OUList.eid = Convert.ToInt32(reader.GetValue(1));
                    OUList.UserTypeID = Convert.ToInt32(reader.GetValue(2));
                    OUList.IFMISOfficeID = Convert.ToInt32(reader.GetValue(3));
                }
                return OUList;
            }
        }


        public string UpdateOU(UserOfficeRole OU)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(" update dbo.tbl_R_BMSUsers set UserTypeID = " + OU.UserTypeID + ", IFMISOfficeID = " + OU.IFMISOfficeID + " where UserID =" + OU.User_ID + "", con);
                try
                {
                    con.Open();
                    com.ExecuteNonQuery();
                    return "1";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }


        public string RemoveUser(int? User_ID, int? eid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"sp_Delete_user " + User_ID + "," + eid + "", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    return "1";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        
        public string RemoveUserNO(int? eid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"update [IFMIS].[dbo].[tbl_T_BMSNO_user] set blocked=1 where eid=" + eid + "", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    return "1";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public IEnumerable<UserDetails_Model> UserDetails()
        {
            List<UserDetails_Model> TrundsList = new List<UserDetails_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select a.UserID, d.UserTypeDesc, b.eid, b.emailaddress, b.passcode, c.EmpName ,a.IFMISOfficeID, c.swipeid, e.OfficeName
                                                  from [dbo].[tbl_R_BMSUsers] as a
                                                  LEFT JOIN [pmis].[dbo].[vwLoginParameter] as b
                                                  ON a.eid = b.eid 
                                                  LEFT JOIN [pmis].[dbo].[vwMergeAllEmployee] as c
                                                  on a.eid = c.eid
                                                  LEFT JOIN [dbo].[tbl_R_BMSUserTypes] as d
                                                  on a.UserTypeID = d.UserTypeID 
                                                  INNER JOIN [dbo].[tbl_R_BMSOffices] as e
                                                  on a.IFMISOfficeID = e.OfficeID", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    UserDetails_Model User = new UserDetails_Model();
                    User.UserID = reader.GetInt32(0);
                    User.UserTypeDesc = reader.GetValue(1).ToString();
                    User.eid = reader.GetInt32(2);
                    User.emailaddress = reader.GetValue(3).ToString();
                    User.passcode = reader.GetValue(4).ToString();
                    User.EmpName = reader.GetValue(5).ToString();
                    User.IFMISOfficeID = reader.GetInt32(6);
                    User.swipeid = reader.GetInt32(7);
                    User.OfficeName = reader.GetValue(8).ToString();


                    TrundsList.Add(User);
                }

            }
            return TrundsList;
        }

    }
}