using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Layers;
using iFMIS_BMS.BusinessLayer.Models;
using System.Data.SqlClient;

namespace iFMIS_BMS.BusinessLayer.Layers
{
    public class Trunds_Layer
    {
        public string AddNewTrunds(int? Trans_ID, string Trans_name, int? Trunds_Amount, int? Year)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("insert into tbl_R_BMSTrundsAmount (Trans_ID,Trans_Amount,OfficeID,Year) values('" + Trans_ID + "','" + Trunds_Amount + "','" + Account.UserInfo.Department + "','" + Year + "')", con);
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


        public string AddNewTrundsName(int? Trans_ID, string Trans_name, int? Trunds_Amount, int? Year)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("insert into [IFMIS].[dbo].[tbl_R_BMSTrunds] (Trans_name) values ('" + Trans_name + "')", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    SqlCommand com2 = new SqlCommand("insert into [IFMIS].[dbo].[tbl_R_BMSTrundsAmount] (Trans_ID,Trans_Amount,OfficeID,Year) values ((select Trans_ID FROM [IFMIS].[dbo].[tbl_R_BMSTrunds] where Trans_name like '" + Trans_name + "'),'" + Trunds_Amount + "','" + Account.UserInfo.Department + "','" + Year + "')", con);
                    
                    com2.ExecuteNonQuery();
                   
                    return "1";
                }

            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }


        //public string AddNewTrundsName(int? Trans_ID, string Trans_name, int? Trunds_Amount, int? Year)
        //{
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(Common.MyConn()))
        //        {
        //            SqlCommand com = new SqlCommand("insert into [IFMIS].[dbo].[tbl_R_BMSTrunds] (Trans_name) values ('" + Trans_name + "')", con);
        //            con.Open();
        //            com.ExecuteNonQuery();
        //            SqlCommand com2 = new SqlCommand("insert into [IFMIS].[dbo].[tbl_R_BMSTrundsAmount] (Trans_ID,Trans_Amount,OfficeID,Year) values ((select Trans_ID FROM [IFMIS].[dbo].[tbl_R_BMSTrunds] where Trans_name like '" + Trans_name + "'),'" + Trunds_Amount + "','" + Account.UserInfo.Department + "','" + Year + "')", con);

        //            com2.ExecuteNonQuery();

        //            return "1";
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message.ToString();
        //    }
        //}



        public IEnumerable<TransFundsModel> Trunds(int? Year)
        {
            List<TransFundsModel> TrundsList = new List<TransFundsModel>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT a.TransAmount_ID,a.Trans_ID,b.Trans_name,a.Trans_Amount,a.OfficeID FROM dbo.tbl_R_BMSTrundsAmount as a 
                                                    inner join dbo.tbl_R_BMSTrunds as b on a.Trans_ID = b.Trans_ID
                                                     where OfficeID = " + Account.UserInfo.Department + " and a.Year='" + Year + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    TransFundsModel Trunds = new TransFundsModel();
                    Trunds.TransAmount_ID = reader.GetInt32(0);
                    Trunds.Trans_ID = reader.GetInt32(1);
                    Trunds.Trans_name = reader.GetValue(2).ToString();
                    Trunds.Trans_Amount = Convert.ToDouble(reader.GetValue(3));
                    Trunds.OfficeID = reader.GetInt32(4);


                    TrundsList.Add(Trunds);
                }

            }
            return TrundsList;
        }


        public TransFundsModel EditTrunds(int? TransAmount_ID)
        {
            TransFundsModel TrundsList = new TransFundsModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"SELECT a.TransAmount_ID,a.Trans_ID , b.Trans_name,a.Trans_Amount,a.Year FROM dbo.tbl_R_BMSTrundsAmount as a 
                                                                inner join dbo.tbl_R_BMSTrunds as b on a.Trans_ID = b.Trans_ID
                                                                 where a.TransAmount_ID =" + TransAmount_ID + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    TrundsList.TransAmount_ID = reader.GetInt32(0);
                    TrundsList.Trans_ID = reader.GetInt32(1);
                    TrundsList.Trans_name = reader.GetValue(2).ToString();
                    TrundsList.Trans_Amount = Convert.ToDouble(reader.GetValue(3));
                    TrundsList.Year = reader.GetInt32(4);
                }
                return TrundsList;
            }
        }


        public string UpdateTrunds(TransFundsModel tra)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(" update dbo.tbl_R_BMSTrundsAmount set Trans_ID = " + tra.Trans_ID + ", Trans_Amount = " + tra.Trans_Amount + " where TransAmount_ID = " + tra.TransAmount_ID + "and Year =" + tra.Year + "", con);
                try
                {
                    con.Open();
                    com.ExecuteNonQuery();
                    SqlCommand com2 = new SqlCommand("  update dbo.tbl_R_BMSTrunds set Trans_name = '" + tra.Trans_name + "' where Trans_ID ='" + tra.Trans_ID + "'", con);
                      com2.ExecuteNonQuery();
                    return "1";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public IEnumerable<TransFundsModel> TrundsType()
        {
            List<TransFundsModel> TrustFund = new List<TransFundsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT Trans_ID,Trans_name FROM [IFMIS].[dbo].[tbl_R_BMSTrunds]", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    TransFundsModel trust_list = new TransFundsModel();
                    trust_list.Trans_ID = Convert.ToInt32(reader.GetValue(0));
                    trust_list.Trans_name = reader.GetString(1);
                    TrustFund.Add(trust_list);
                }
            }
            return TrustFund;
        }


        public string RemoveTrunds(int? TransAmount_ID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("Delete from dbo.tbl_R_BMSTrundsAmount where TransAmount_ID ='" + TransAmount_ID + "' and OfficeID = '" + Account.UserInfo.Department + "'", con);
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
        public string RemoveGrants(int? grants_id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("Delete from dbo.tbl_R_BMSgrants where grants_id ='" + grants_id + "' and OfficeID = '" + Account.UserInfo.Department + "'", con);
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
        public string RemoveOthers(int? other_id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("Delete from dbo.tbl_R_BMSothers where other_id ='" + other_id + "' and OfficeID = '" + Account.UserInfo.Department + "'", con);
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


        public string RemoveNon(int? trnno, int? YearN)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_SOONONoffice set DateInOut = CONCAT(DateInOut,' , ', GETDATE()), ActionCode = '2' where trnno = '" + trnno + "' and officeiD = '" + Account.UserInfo.Department + "' and Yearof = '" + YearN + "' and ActionCode = 1", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    return "success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }


    }
}