using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iFMIS_BMS.BusinessLayer.Models;
using System.Data.SqlClient;
using iFMIS_BMS.BusinessLayer.Connector;



namespace iFMIS_BMS.BusinessLayer.Layers
{
    public class LoginLayer
    {
        public UserModel User(string completeEmail, string passcode, string ip, string hostname)
        {
            UserModel usr = new UserModel();
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"exec sp_BMS_Login  '" + completeEmail + "','" + passcode + "',0", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        usr.UserTypeDesc = reader.GetString(0);
                        usr.eid = reader.GetInt64(1);
                        usr.emailaddress = reader.GetString(2);
                        usr.Passcode = reader.GetString(3);
                        usr.empName = reader.GetString(4);
                        usr.Department = reader.GetValue(5).ToString();
                        usr.imgName = reader.GetValue(6).ToString() + ".jpg";
                        usr.OfficeName = reader.GetString(7);
                        usr.UserTypeID = Convert.ToInt32(reader.GetValue(8));
                        usr.lgu = Convert.ToInt32(reader.GetValue(9));
                    }
                    reader.Close();


                   
                        SqlCommand com2 = new SqlCommand(@"SELECT count(IFMISOfficeID) as ok from dbo.tbl_R_BMSUsers where eid =" + usr.eid + "", con);
                       
                        SqlDataReader readers = com2.ExecuteReader();
                        while (readers.Read())
                        {
                            usr.Countopis = readers.GetInt32(0);


                        }
                        readers.Close();

                    



                    if (usr.emailaddress != null && usr.Passcode != null)
                    {
                        SqlCommand query_time = new SqlCommand(@"SELECT SUBSTRING(CONVERT(varchar, getdate(), 109), 13, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 16, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 19, 2) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 100), 18, 2)", con);
                        
                        SqlDataReader reader_time = query_time.ExecuteReader();
                        var timeDate = "";
                        while (reader_time.Read())
                        {
                            timeDate = reader_time.GetString(0);
                        }
                        reader_time.Close();
                        if(ip == "::1"){
                            ip = "localhost";
                        }
                        SqlCommand query_insertUserLog = new SqlCommand(@"Insert into tbl_R_BMSUserLog values('" + usr.emailaddress + "', '" + usr.eid + "', '" + ip + "', '" + hostname + "', Convert(date, getdate()), '" + timeDate + "', '')", con);
                        query_insertUserLog.ExecuteNonQuery();
                    }
                }
                return usr;
            }
            catch (Exception ex)
            {
                usr.emailaddress = ex.Message;
                return usr;
            }
        }



        public UserModel UserOF(string completeEmail, string passcode, string ip, string hostname, string OfficeID)
        {
            UserModel usr = new UserModel();
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    //SqlCommand com = new SqlCommand(@"select d.UserTypeDesc, b.eid, b.emailaddress, b.passcode, 
                    //                              c.EmpName ,a.IFMISOfficeID, c.swipeid, e.OfficeName, d.UserTypeID
                    //                              from tbl_R_BMSUsers as a
                    //                              LEFT JOIN [pmis].[dbo].[vwLoginParameter] as b
                    //                              ON a.eid = b.eid 
                    //                              LEFT JOIN [pmis].[dbo].[vwMergeAllEmployee] as c
                    //                              on a.eid = c.eid
                    //                              LEFT JOIN tbl_R_BMSUserTypes as d
                    //                              on a.UserTypeID = d.UserTypeID 
                    //                              INNER JOIN tbl_R_BMSOffices as e
                    //                              on a.IFMISOfficeID = e.OfficeID
                    //                              where b.emailaddress = '" + completeEmail + "' and b.passcode = '" + passcode + "' and a.IFMISOfficeID='"+ OfficeID +"'", con);
                    SqlCommand com = new SqlCommand(@"exec sp_BMS_Login  '" + completeEmail + "','" + passcode + "',"+ OfficeID  + "", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        usr.UserTypeDesc = reader.GetString(0);
                        usr.eid = reader.GetInt64(1);
                        usr.emailaddress = reader.GetString(2);
                        usr.Passcode = reader.GetString(3);
                        usr.empName = reader.GetString(4);
                        usr.Department = reader.GetValue(5).ToString();
                        usr.imgName = reader.GetValue(6).ToString() + ".jpg";
                        usr.OfficeName = reader.GetString(7);
                        usr.UserTypeID = Convert.ToInt32(reader.GetValue(8));
                        usr.lgu = Convert.ToInt32(reader.GetValue(9));
                    }
                    reader.Close();



                    //SqlCommand com2 = new SqlCommand(@"SELECT count(IFMISOfficeID) as ok from dbo.tbl_R_BMSUsers where eid =" + usr.eid + "", con);

                    //SqlDataReader readers = com2.ExecuteReader();
                    //while (readers.Read())
                    //{
                    //    usr.Countopis = readers.GetInt32(0);


                    //}
                    //readers.Close();





                    if (usr.emailaddress != null && usr.Passcode != null)
                    {
                        SqlCommand query_time = new SqlCommand(@"SELECT SUBSTRING(CONVERT(varchar, getdate(), 109), 13, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 16, 2) + ':' + SUBSTRING(CONVERT(varchar, getdate(), 109), 19, 2) + ' ' + SUBSTRING(CONVERT(varchar, getdate(), 100), 18, 2)", con);

                        SqlDataReader reader_time = query_time.ExecuteReader();
                        var timeDate = "";
                        while (reader_time.Read())
                        {
                            timeDate = reader_time.GetString(0);
                        }
                        reader_time.Close();
                        if (ip == "::1")
                        {
                            ip = "localhost";
                        }
                        SqlCommand query_insertUserLog = new SqlCommand(@"Insert into tbl_R_BMSUserLog values('" + usr.emailaddress + "', '" + usr.eid + "', '" + ip + "', '" + hostname + "', Convert(date, getdate()), '" + timeDate + "', '')", con);
                        query_insertUserLog.ExecuteNonQuery();
                    }
                }
                return usr;
            }
            catch (Exception)
            {
                usr.emailaddress = "ServerErrorLog-in";
                return usr;
            }
        }  
        public IEnumerable<UserLogInfo> grLoginHistory(int? UserID)
        {
            List<UserLogInfo> LoginHistoryList = new List<UserLogInfo>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                UserID = UserID == null ? 0 : UserID;
                SqlCommand query_UserLog = new SqlCommand(@"SELECT dbo.tbl_R_BMSUserLog.[Date],
                            dbo.tbl_R_BMSUserLog.User_IPAddress,
                            dbo.tbl_R_BMSUserLog.User_PCName,
                            dbo.tbl_R_BMSUserLog.Time_IN,
                            dbo.tbl_R_BMSUserLog.Time_OUT,
                            dbo.tbl_R_BMSUserLog.UserLog_ID
                            FROM tbl_R_BMSUserLog where User_ID ='" + UserID + "' ORDER BY UserLog_ID DESC", con);
                con.Open();
                SqlDataReader reader_UserLog = query_UserLog.ExecuteReader();
                while(reader_UserLog.Read())
                {
                    UserLogInfo userloginfo = new UserLogInfo();
                    userloginfo.Date = reader_UserLog.GetString(0);
                    userloginfo.IP_Address = reader_UserLog.GetString(1);
                    userloginfo.PC_Name = reader_UserLog.GetString(2);
                    userloginfo.Time_in = reader_UserLog.GetString(3);
                    userloginfo.Time_out = reader_UserLog.GetString(4);
                    userloginfo.UserLogID = reader_UserLog.GetInt32(5);
                    LoginHistoryList.Add(userloginfo);
                }
            }
            return LoginHistoryList;
        }




    }
}