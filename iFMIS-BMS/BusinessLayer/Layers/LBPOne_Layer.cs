using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Layers;
using iFMIS_BMS.BusinessLayer.Models;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace iFMIS_BMS.BusinessLayer.Layers
{
    public class LBPOne_Layer
    {

        int Fund_Desc_IDss = 0;
        int Eco_Desc_IDss = 0;
        int Type_Desc_IDss = 0;
        int Sub1_Desc_IDss = 0;
        int Sub2_Desc_IDss = 0;
        int Type_IDss = 0;
        int Sub1_IDss = 0;
        int Fund_IDss = 0;
        int Eco_IDss = 0;
        int Eco_Type_IDss = 0;
        int EE_Sub1_IDss = 0;
        int Class_IDss = 0;
        
        //int Type_IDss = 0;

        public IEnumerable<LBPOne_SourceFundsModel> LoadSF(int? Year_of)
        {


            List<LBPOne_SourceFundsModel> SourceFundList = new List<LBPOne_SourceFundsModel>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                LBPOne_SourceFundsModel FirstList = new LBPOne_SourceFundsModel();
                FirstList.Fund_ID = 0;
                FirstList.Fund_Desc = "I. Beginning Cash Balance";
                //Sfunds.grants_Amount = Convert.ToDouble(reader.GetValue(2));
                FirstList.Date_Of = "0";
                FirstList.Action_Code = 0;
                FirstList.Year_Of = 1;
                SourceFundList.Add(FirstList);
                LBPOne_SourceFundsModel FirstListII = new LBPOne_SourceFundsModel();
                FirstListII.Fund_ID = 0;
                FirstListII.Fund_Desc = "II. Receipts";
                //Sfunds.grants_Amount = Convert.ToDouble(reader.GetValue(2));
                FirstListII.Date_Of = "0";
                FirstListII.Action_Code = 0;
                FirstListII.Year_Of = 1;
                SourceFundList.Add(FirstListII);


                SqlCommand com = new SqlCommand(@"SELECT a.Fund_ID,a.Fund_Desc,sum(d.Year1_Amount),sum(d.Year2_Amount),sum(d.Year3_Amount),a.Year_Of FROM IFMIS.dbo.tbl_R_BMS_A_SourceFunds as a
                                                inner join IFMIS.dbo.tbl_R_BMS_A_TypeFunds as b on a.Fund_ID = b.Fund_ID
                                                inner join IFMIS.dbo.tbl_R_BMS_A_Sub1 as c on c.Type_ID = b.Type_ID
                                                inner join IFMIS.dbo.tbl_R_BMS_A_Sub2 as d on d.Sub1_ID = c.Sub1_ID
                                                 where  a.Year_of = '" + Year_of + "' and ((b.Action_Code = 1 or b.Action_Code = 0) and (c.Action_Code = 1 or c.Action_Code = 0) and (d.Action_Code = 1 or d.Action_Code = 0) and (a.Action_Code = 1 or a.Action_Code = 0))  group by a.Fund_ID,a.Fund_Desc,a.Year_Of", con);
                 con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    LBPOne_SourceFundsModel Sfunds = new LBPOne_SourceFundsModel();
                    Sfunds.Fund_ID = reader.GetInt32(0);
                    Sfunds.Fund_Desc = "<span style='margin-left: 15px;'>" + reader.GetValue(1).ToString() + "</span>";
                    Sfunds.Year1_Amount = Convert.ToDouble(reader.GetValue(2));
                    Sfunds.Year2_Amount = Convert.ToDouble(reader.GetValue(3));
                    Sfunds.Year3_Amount = Convert.ToDouble(reader.GetValue(4));
                    Sfunds.Year_Of = reader.GetInt32(5);
                    Sfunds.Difference =  Convert.ToDouble(reader.GetValue(4)) - Convert.ToDouble(reader.GetValue(3));
                    SourceFundList.Add(Sfunds);
                }

            }
            return SourceFundList;
        }


        public IEnumerable<LBPOne_SourceFundsModel> SFtype()
        {
            List<LBPOne_SourceFundsModel> SourceFund = new List<LBPOne_SourceFundsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT Fund_Desc_ID,Fund_Desc FROM IFMIS.dbo.tbl_R_BMS_B_SFUNDS_List", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    LBPOne_SourceFundsModel SF_list = new LBPOne_SourceFundsModel();
                    SF_list.Fund_Desc_ID = reader.GetInt32(0);
                    SF_list.Fund_Desc =reader.GetValue(1).ToString();
                    //Sfunds.grants_Amount = Convert.ToDouble(reader.GetValue(2));
                    //SF_list.Date_Of = reader.GetValue(2).ToString();
                    //SF_list.Action_Code = reader.GetInt32(3);
                    //SF_list.Year_Of = reader.GetInt32(4);
                    SourceFund.Add(SF_list);
                }
            }
            return SourceFund;
        }



        public string AddNewTrunds(int? Fund_Desc_ID, string Fund_Desc, int? Year_of)
        {
            try
            {
                List<LBPOne_IDs> ok = new List<LBPOne_IDs>();
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_SourceFunds (Fund_Desc,Date_Of,Action_Code,Year_Of,Fund_Desc_ID) values ('" + Fund_Desc.Replace("'", "''") + "',GETDATE(),1,'" + Year_of + "'," + Fund_Desc_ID + ")", con);
                    con.Open();
                    com.ExecuteNonQuery();


                    SqlCommand com469 = new SqlCommand(@"SELECT top 1 Fund_ID FROM IFMIS.dbo.tbl_R_BMS_A_SourceFunds where Fund_Desc = '" + Fund_Desc.Replace("'", "''") + "' order by Fund_ID desc ", con);
                    SqlDataReader reader69 = com469.ExecuteReader();
                    while (reader69.Read())
                    {
                        LBPOne_IDs SF_list69 = new LBPOne_IDs();
                        SF_list69.Fund_ID = reader69.GetInt32(0);


                        ok.Add(SF_list69);
                        Fund_IDss = SF_list69.Fund_ID;
                    }
                    con.Close();



                    //insert dum type
                    SqlCommand com23 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_TypeFunds (Type_Desc,Fund_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Type_Desc_ID) values ('dum','" + Fund_IDss + "',0,0,0,'dum',0,0,0)", con);
                    con.Open();
                    com23.ExecuteNonQuery();

                    SqlCommand com47 = new SqlCommand(@"SELECT top 1 Type_ID FROM IFMIS.dbo.tbl_R_BMS_A_TypeFunds where Type_Desc like '%dum%' order by Type_ID desc", con);
                    SqlDataReader reader7 = com47.ExecuteReader();
                    while (reader7.Read())
                    {
                        LBPOne_IDs SF_list = new LBPOne_IDs();
                        SF_list.Type_ID = reader7.GetInt32(0);


                        ok.Add(SF_list);
                        Type_IDss = SF_list.Type_ID;
                    }
                    con.Close();

                    //insert to sub1 with type_id

                    SqlCommand com27 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_Sub1 (Sub1_Desc,Type_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Sub1_Desc_ID ) values ('dum'," + Type_IDss + ",0,0,0,'dum',0,0,0)", con);
                    con.Open();
                    com27.ExecuteNonQuery();

                    SqlCommand comsubID = new SqlCommand(@"SELECT  Sub1_ID FROM IFMIS.dbo.tbl_R_BMS_A_Sub1 where Type_ID=" + Type_IDss + "", con);
                    SqlDataReader readersubID = comsubID.ExecuteReader();
                    while (readersubID.Read())
                    {
                        LBPOne_IDs SF_list = new LBPOne_IDs();
                        SF_list.Sub1_ID = readersubID.GetInt32(0);

                        ok.Add(SF_list);
                        Sub1_IDss = SF_list.Sub1_ID;
                    }
                    con.Close();


                    SqlCommand com231 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_Sub2 (Sub2_Desc,Sub1_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Sub2_Desc_ID) values ('dum'," + Sub1_IDss + ",0,0,0,'dum',0,0,0)", con);
                    con.Open();
                    com231.ExecuteNonQuery();





                    return "1";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }



        public string AddNewTrundsName(int? Fund_Desc_ID, string Fund_Desc, int? Year_of)
        {
            try
            {
                List<LBPOne_IDs> ok = new List<LBPOne_IDs>();
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_B_SFUNDS_List (Fund_Desc,Date_of) values ('" + Fund_Desc.Replace("'", "''") + "',GETDATE())", con);
                    con.Open();
                    com.ExecuteNonQuery();

                    SqlCommand com4 = new SqlCommand(@"SELECT Fund_Desc_ID FROM IFMIS.dbo.tbl_R_BMS_B_SFUNDS_List where Fund_Desc = '" + Fund_Desc.Replace("'", "''") + "'", con);
                    SqlDataReader reader = com4.ExecuteReader();
                    while (reader.Read())
                    {
                        LBPOne_IDs SF_list = new LBPOne_IDs();
                        SF_list.Fund_Desc_ID = reader.GetInt32(0);
                       
                    
                        ok.Add(SF_list);
                        Fund_Desc_IDss = SF_list.Fund_Desc_ID;
                    }
                    con.Close();


                    SqlCommand com2 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_SourceFunds (Fund_Desc,Date_Of,Action_Code,Year_Of,Fund_Desc_ID) values ('" + Fund_Desc.Replace("'", "''") + "',GETDATE(),1,'" + Year_of + "'," + Fund_Desc_IDss + ")", con);
                    con.Open();
                    com2.ExecuteNonQuery();

                    //get ID for dum type
                    SqlCommand com469 = new SqlCommand(@"SELECT top 1 Fund_ID FROM IFMIS.dbo.tbl_R_BMS_A_SourceFunds where Fund_Desc = '" + Fund_Desc.Replace("'", "''") + "' order by Fund_ID desc ", con);
                    SqlDataReader reader69 = com469.ExecuteReader();
                    while (reader69.Read())
                    {
                        LBPOne_IDs SF_list69 = new LBPOne_IDs();
                        SF_list69.Fund_ID = reader69.GetInt32(0);


                        ok.Add(SF_list69);
                        Fund_IDss = SF_list69.Fund_ID;
                    }
                    con.Close();



                    //insert dum type
                    SqlCommand com23 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_TypeFunds (Type_Desc,Fund_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Type_Desc_ID) values ('dum','" + Fund_IDss + "',0,0,0,'dum',0,0,0)", con);
                    con.Open();
                    com23.ExecuteNonQuery();

                    SqlCommand com47 = new SqlCommand(@"SELECT top 1 Type_ID FROM IFMIS.dbo.tbl_R_BMS_A_TypeFunds where Type_Desc like '%dum%' order by Type_ID desc", con);
                    SqlDataReader reader7 = com47.ExecuteReader();
                    while (reader7.Read())
                    {
                        LBPOne_IDs SF_list = new LBPOne_IDs();
                        SF_list.Type_ID = reader7.GetInt32(0);


                        ok.Add(SF_list);
                        Type_IDss = SF_list.Type_ID;
                    }
                    con.Close();

                    //insert to sub1 with type_id

                    SqlCommand com27 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_Sub1 (Sub1_Desc,Type_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Sub1_Desc_ID ) values ('dum'," + Type_IDss + ",0,0,0,'dum',0,0,0)", con);
                    con.Open();
                    com27.ExecuteNonQuery();

                    SqlCommand comsubID = new SqlCommand(@"SELECT  Sub1_ID FROM IFMIS.dbo.tbl_R_BMS_A_Sub1 where Type_ID=" + Type_IDss + "", con);
                    SqlDataReader readersubID = comsubID.ExecuteReader();
                    while (readersubID.Read())
                    {
                        LBPOne_IDs SF_list = new LBPOne_IDs();
                        SF_list.Sub1_ID = readersubID.GetInt32(0);

                        ok.Add(SF_list);
                        Sub1_IDss = SF_list.Sub1_ID;
                    }
                    con.Close();


                    SqlCommand com231 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_Sub2 (Sub2_Desc,Sub1_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Sub2_Desc_ID) values ('dum'," + Sub1_IDss + ",0,0,0,'dum',0,0,0)", con);
                    con.Open();
                    com231.ExecuteNonQuery();


                    return "1";
                }

            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }



        public IEnumerable<LBPOne_TypeFundsModel> ReadTF(int? Fund_ID, int? Year_of)
        {


            List<LBPOne_TypeFundsModel> TF_list = new List<LBPOne_TypeFundsModel>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT a.Type_ID ,a.Type_Desc,sum(c.Year1_Amount),sum(c.Year2_Amount),sum(c.Year3_Amount),a.Year_Of FROM IFMIS.dbo.tbl_R_BMS_A_TypeFunds as a
                                                        inner join IFMIS.dbo.tbl_R_BMS_A_Sub1 as b on a.Type_ID = b.Type_ID
                                                        inner join IFMIS.dbo.tbl_R_BMS_A_Sub2 as c on b.Sub1_ID = c.Sub1_ID
                                                         where a.Fund_ID = " + Fund_ID + " and a.Year_of = " + Year_of + " and a.Action_Code = 1 and ((b.Action_Code = 1 or b.Action_Code = 0) and (c.Action_Code = 1 or c.Action_Code = 0)) group by a.Type_ID,a.Type_ID ,a.Type_Desc,a.Year_Of", con);
                 con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    LBPOne_TypeFundsModel Tfunds = new LBPOne_TypeFundsModel();
                    Tfunds.Type_ID = reader.GetInt32(0);
                    Tfunds.Type_Desc = reader.GetValue(1).ToString();
                    //Tfunds.Fund_ID = reader.GetInt32(2);
                    Tfunds.Year1_Amount = Convert.ToDouble(reader.GetValue(2));
                    Tfunds.Year2_Amount = Convert.ToDouble(reader.GetValue(3));
                    Tfunds.Year3_Amount = Convert.ToDouble(reader.GetValue(4));
                    //Tfunds.Date_Of = reader.GetValue(6).ToString();
                    //Tfunds.Action_Code = reader.GetInt32(7);
                    Tfunds.Year_Of = reader.GetInt32(5);
                    Tfunds.Difference = Convert.ToDouble(reader.GetValue(4)) - Convert.ToDouble(reader.GetValue(3)) ;

                    TF_list.Add(Tfunds);
                }

            }
            return TF_list;
        }

        public IEnumerable<LBPOne_Sub2Model> readSub(int? Type_ID, int? Year_Of)
        {


            List<LBPOne_Sub2Model> TF_list = new List<LBPOne_Sub2Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT a.Sub2_ID,a.Sub2_Desc,a.Sub1_ID,a.Year1_Amount,a.Year2_Amount,a.Year3_Amount,a.Date_Of,a.Action_Code,a.Year_Of,b.Sub1_Desc 
                                                    FROM IFMIS.dbo.tbl_R_BMS_A_Sub2 as a inner join IFMIS.dbo.tbl_R_BMS_A_Sub1 as b on a.Sub1_ID = b.Sub1_ID where b.Type_ID = '" + Type_ID + "' and b.Year_of = '" + Year_Of + "' and a.Action_Code = 1 and b.Action_Code = 1 order by a.Sub1_ID  ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    LBPOne_Sub2Model Tfunds = new LBPOne_Sub2Model();
                    Tfunds.Sub2_ID = reader.GetInt32(0);
                    Tfunds.Sub2_Desc = reader.GetValue(1).ToString();
                    Tfunds.Sub1_ID = reader.GetInt32(2);
                    Tfunds.Year1_Amount = Convert.ToDouble(reader.GetValue(3));
                    Tfunds.Year2_Amount = Convert.ToDouble(reader.GetValue(4));
                    Tfunds.Year3_Amount = Convert.ToDouble(reader.GetValue(5));
                    Tfunds.Date_Of = reader.GetValue(6).ToString();
                    Tfunds.Action_Code = reader.GetInt32(7);
                    Tfunds.Year_Of = reader.GetInt32(8);
                    Tfunds.Sub1_Desc = reader.GetValue(9).ToString();
                    Tfunds.Difference = Convert.ToDouble(reader.GetValue(5)) - Convert.ToDouble(reader.GetValue(4)) ;

                    TF_list.Add(Tfunds);
                }

            }
            return TF_list;
        }





        public IEnumerable<LBPOne_TypeFundsModel> RType()
        {
            List<LBPOne_TypeFundsModel> Rtypes = new List<LBPOne_TypeFundsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT Type_Desc_ID,Type_Desc FROM IFMIS.dbo.tbl_R_BMS_B_STYPES_List", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    LBPOne_TypeFundsModel RT_list = new LBPOne_TypeFundsModel();
                    RT_list.Type_Desc_ID = reader.GetInt32(0);
                    RT_list.Type_Desc = reader.GetValue(1).ToString();
                    //Sfunds.grants_Amount = Convert.ToDouble(reader.GetValue(2));
                    //SF_list.Date_Of = reader.GetValue(2).ToString();
                    //SF_list.Action_Code = reader.GetInt32(3);
                    //SF_list.Year_Of = reader.GetInt32(4);
                    Rtypes.Add(RT_list);
                }
            }
            return Rtypes;
        }





        public string addRType(int? Type_Desc_ID, string Type_Desc, int? Fund_ID, int? Year_of)
        {
            try
            {
                List<LBPOne_IDs> ok = new List<LBPOne_IDs>();
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                   //insert to type
                    SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_TypeFunds (Type_Desc,Fund_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Type_Desc_ID) values ('" + Type_Desc.Replace("'", "''") + "','" + Fund_ID + "',0,0,0,GETDATE(),1,'" + Year_of + "','" + Type_Desc_ID + "')", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    //get type_id
                    SqlCommand com4 = new SqlCommand(@"SELECT Type_ID FROM IFMIS.dbo.tbl_R_BMS_A_TypeFunds where Type_Desc = '" + Type_Desc.Replace("'", "''") + "' and Fund_ID =" + Fund_ID + " and Year_Of =" + Year_of + " and Type_Desc_ID = " + Type_Desc_ID + "", con);

                    SqlDataReader reader = com4.ExecuteReader();
                    while (reader.Read())
                    {
                        LBPOne_IDs SF_list = new LBPOne_IDs();
                        SF_list.Type_ID = reader.GetInt32(0);


                        ok.Add(SF_list);
                        Type_IDss = SF_list.Type_ID;
                    }
                    con.Close();

                    //insert to sub1 with type_id

                    SqlCommand com2 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_Sub1 (Sub1_Desc,Type_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Sub1_Desc_ID ) values ('dum',"+Type_IDss+",0,0,0,'dum',0,0,0)", con);
                    con.Open();
                    com2.ExecuteNonQuery();

                    SqlCommand comsubID = new SqlCommand(@"SELECT  Sub1_ID FROM IFMIS.dbo.tbl_R_BMS_A_Sub1 where Type_ID=" + Type_IDss + "", con);
                    SqlDataReader readersubID = comsubID.ExecuteReader();
                    while (readersubID.Read())
                    {
                        LBPOne_IDs SF_list = new LBPOne_IDs();
                        SF_list.Sub1_ID = readersubID.GetInt32(0);

                        ok.Add(SF_list);
                        Sub1_IDss = SF_list.Sub1_ID;
                    }
                    con.Close();


                    SqlCommand com23 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_Sub2 (Sub2_Desc,Sub1_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Sub2_Desc_ID) values ('dum'," + Sub1_IDss + ",0,0,0,'dum',0,0,0)", con);
                    con.Open();
                    com23.ExecuteNonQuery();


                    return "1";

                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }


        //add type
         public string addRTypeName(int? Type_Desc_ID, string Type_Desc, int? Fund_ID, int? Year_of)
         {
             try
             {
                 List<LBPOne_IDs> ok = new List<LBPOne_IDs>();
                 using (SqlConnection con = new SqlConnection(Common.MyConn()))
                 {
                     SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_B_STYPES_List (Type_Desc,Date_of) values ('" + Type_Desc.Replace("'", "''") + "',GETDATE())", con);
                     con.Open();
                     com.ExecuteNonQuery();

                     SqlCommand com4 = new SqlCommand(@"SELECT Type_Desc_ID FROM IFMIS.dbo.tbl_R_BMS_B_STYPES_List where Type_Desc = '" + Type_Desc.Replace("'", "''") + "'", con);

                     SqlDataReader reader = com4.ExecuteReader();
                     while (reader.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.Type_Desc_ID = reader.GetInt32(0);


                         ok.Add(SF_list);
                         Type_Desc_IDss = SF_list.Type_Desc_ID;
                     }
                     con.Close();


                     SqlCommand com2 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_TypeFunds (Type_Desc,Fund_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Type_Desc_ID) values ('" + Type_Desc.Replace("'", "''") + "','" + Fund_ID + "',0,0,0,GETDATE(),1,'" + Year_of + "','" + Type_Desc_IDss + "')", con);
                     con.Open();
                     com2.ExecuteNonQuery();

                     SqlCommand com47 = new SqlCommand(@"SELECT Type_ID FROM IFMIS.dbo.tbl_R_BMS_A_TypeFunds where Type_Desc = '" + Type_Desc.Replace("'", "''") + "'", con);

                     SqlDataReader reader7 = com47.ExecuteReader();
                     while (reader7.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.Type_ID = reader7.GetInt32(0);


                         ok.Add(SF_list);
                         Type_IDss = SF_list.Type_ID;
                     }
                     con.Close();

                     //insert to sub1 with type_id

                     SqlCommand com27 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_Sub1 (Sub1_Desc,Type_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Sub1_Desc_ID ) values ('dum'," + Type_IDss + ",0,0,0,'dum',0,0,0)", con);
                     con.Open();
                     com27.ExecuteNonQuery();

                     SqlCommand comsubID = new SqlCommand(@"SELECT  Sub1_ID FROM IFMIS.dbo.tbl_R_BMS_A_Sub1 where Type_ID=" + Type_IDss + "", con);
                     SqlDataReader readersubID = comsubID.ExecuteReader();
                     while (readersubID.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.Sub1_ID = readersubID.GetInt32(0);

                         ok.Add(SF_list);
                         Sub1_IDss = SF_list.Sub1_ID;
                     }
                     con.Close();


                     SqlCommand com23 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_Sub2 (Sub2_Desc,Sub1_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Sub2_Desc_ID) values ('dum'," + Sub1_IDss + ",0,0,0,'dum',0,0,0)", con);
                     con.Open();
                     com23.ExecuteNonQuery();



                     return "1";
                 }

             }
             catch (Exception ex)
             {
                 return ex.Message.ToString();
             }

         }




         public IEnumerable<LBPOne_SUB1Model> SubCombo()
         {
             List<LBPOne_SUB1Model> Subname = new List<LBPOne_SUB1Model>();
             using (SqlConnection con = new SqlConnection(Common.MyConn()))
             {
                 SqlCommand com = new SqlCommand(@"SELECT Sub1_Desc_ID,Sub1_Desc FROM IFMIS.dbo.tbl_R_BMS_B_SUB1_List", con);
                 con.Open();
                 SqlDataReader reader = com.ExecuteReader();
                 while (reader.Read())
                 {
                     LBPOne_SUB1Model sub_list = new LBPOne_SUB1Model();
                     sub_list.Sub1_Desc_ID = reader.GetInt32(0);
                     sub_list.Sub1_Desc = reader.GetValue(1).ToString();
                     //Sfunds.grants_Amount = Convert.ToDouble(reader.GetValue(2));
                     //SF_list.Date_Of = reader.GetValue(2).ToString();
                     //SF_list.Action_Code = reader.GetInt32(3);
                     //SF_list.Year_Of = reader.GetInt32(4);
                     Subname.Add(sub_list);
                 }
             }
             return Subname;
         }


         public IEnumerable<LBPOne_Sub2Model> SubCombo2()
         {
             List<LBPOne_Sub2Model> Subname2 = new List<LBPOne_Sub2Model>();
             using (SqlConnection con = new SqlConnection(Common.MyConn()))
             {
                 SqlCommand com = new SqlCommand(@"SELECT Sub2_Desc_ID,Sub2_Desc FROM IFMIS.dbo.tbl_R_BMS_B_SUB2_List", con);
                 con.Open();
                 SqlDataReader reader = com.ExecuteReader();
                 while (reader.Read())
                 {
                     LBPOne_Sub2Model sub_list2 = new LBPOne_Sub2Model();
                     sub_list2.Sub2_Desc_ID = reader.GetInt32(0);
                     sub_list2.Sub2_Desc = reader.GetValue(1).ToString();
                     //Sfunds.grants_Amount = Convert.ToDouble(reader.GetValue(2));
                     //SF_list.Date_Of = reader.GetValue(2).ToString();
                     //SF_list.Action_Code = reader.GetInt32(3);
                     //SF_list.Year_Of = reader.GetInt32(4);
                     Subname2.Add(sub_list2);
                 }
             }
             return Subname2;
         }



         public string old1new1(int? Sub1_Desc_ID, string Sub1_Desc, int? Sub2_Desc_ID, string Sub2_Desc, double Past_year, double Current_year, double Budget_year, int? Type_ID, int? Year_Of)
         {
             try
             {
                 List<LBPOne_IDs> ok = new List<LBPOne_IDs>();
                 using (SqlConnection con = new SqlConnection(Common.MyConn()))
                 {

                     //new sub 2

                     SqlCommand comsub2 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_B_SUB2_List (Sub2_Desc,Date_of) values ('" + Sub2_Desc.Replace("'", "''") + "',GETDATE())", con);
                     con.Open();
                     comsub2.ExecuteNonQuery();

                     SqlCommand comsub22 = new SqlCommand(@"SELECT Sub2_Desc_ID FROM IFMIS.dbo.tbl_R_BMS_B_SUB2_List where Sub2_Desc='" + Sub2_Desc.Replace("'", "''") + "'", con);
                     SqlDataReader readersub2 = comsub22.ExecuteReader();
                     while (readersub2.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.Sub2_Desc_ID = readersub2.GetInt32(0);


                         ok.Add(SF_list);
                         Sub2_Desc_IDss = SF_list.Sub2_Desc_ID;
                     }
                     con.Close();

                     SqlCommand com2 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_Sub1 (Sub1_Desc,Type_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Sub1_Desc_ID ) values ('" + Sub1_Desc.Replace("'", "''") + "','" + Type_ID + "',0,0,0,GETDATE(),1,'" + Year_Of + "','" + Sub1_Desc_ID + "')", con);
                     con.Open();
                     com2.ExecuteNonQuery();

                     SqlCommand comsubID = new SqlCommand(@"SELECT  Sub1_ID FROM IFMIS.dbo.tbl_R_BMS_A_Sub1 where Sub1_Desc='" + Sub1_Desc.Replace("'", "''") + "' and Type_ID = " + Type_ID + " and Action_Code = 1 and Year_Of = " + Year_Of + "", con);
                     SqlDataReader readersubID = comsubID.ExecuteReader();
                     while (readersubID.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.Sub1_ID = readersubID.GetInt32(0);

                         ok.Add(SF_list);
                         Sub1_IDss = SF_list.Sub1_ID;
                     }
                     con.Close();
                     //insert SUB 2 with SUB1_ID
                     SqlCommand com23 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_Sub2 (Sub2_Desc,Sub1_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Sub2_Desc_ID) values ('" + Sub2_Desc.Replace("'", "''") + "','" + Sub1_IDss + "','" + Past_year + "','" + Current_year + "','" + Budget_year + "',GETDATE(),1,'" + Year_Of + "','" + Sub2_Desc_IDss + "')", con);
                     con.Open();
                     com23.ExecuteNonQuery();



                     return "1";
                 }
             }
             catch (Exception ex)
             {
                 return ex.Message.ToString();
             }
         }
         public string new1old2(int? Sub1_Desc_ID, string Sub1_Desc, int? Sub2_Desc_ID, string Sub2_Desc, double Past_year, double Current_year, double Budget_year, int? Type_ID, int? Year_Of)
         {
             try
             {
                 List<LBPOne_IDs> ok = new List<LBPOne_IDs>();
                 using (SqlConnection con = new SqlConnection(Common.MyConn()))
                 {
                     //new sub 1
                     SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_B_SUB1_List (Sub1_Desc,Date_of) values ('" + Sub1_Desc.Replace("'", "''") + "',GETDATE())", con);
                     con.Open();
                     com.ExecuteNonQuery();

                     SqlCommand com4 = new SqlCommand(@"SELECT Sub1_Desc_ID FROM IFMIS.dbo.tbl_R_BMS_B_SUB1_List where Sub1_Desc='" + Sub1_Desc.Replace("'", "''") + "'", con);
                     SqlDataReader reader = com4.ExecuteReader();
                     while (reader.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.Sub1_Desc_ID = reader.GetInt32(0);


                         ok.Add(SF_list);
                         Sub1_Desc_IDss = SF_list.Sub1_Desc_ID;
                     }
                     con.Close();
                     //insert SUB 1
                     SqlCommand com2 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_Sub1 (Sub1_Desc,Type_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Sub1_Desc_ID ) values ('" + Sub1_Desc.Replace("'", "''") + "','" + Type_ID + "',0,0,0,GETDATE(),1,'" + Year_Of + "','" + Sub1_Desc_IDss + "')", con);
                     con.Open();
                     com2.ExecuteNonQuery();
                     //get SUB1_ID
                     SqlCommand comsubID = new SqlCommand(@"SELECT  Sub1_ID FROM IFMIS.dbo.tbl_R_BMS_A_Sub1 where Sub1_Desc='" + Sub1_Desc.Replace("'", "''") + "' and Type_ID = " + Type_ID + " and Action_Code = 1 and Year_Of = " + Year_Of + "", con);
                     SqlDataReader readersubID = comsubID.ExecuteReader();
                     while (readersubID.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.Sub1_ID = readersubID.GetInt32(0);

                         ok.Add(SF_list);
                         Sub1_IDss = SF_list.Sub1_ID;
                     }
                     con.Close();
                     //insert SUB 2 with SUB1_ID
                     SqlCommand com23 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_Sub2 (Sub2_Desc,Sub1_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Sub2_Desc_ID) values ('" + Sub2_Desc.Replace("'", "''") + "','" + Sub1_IDss + "','" + Past_year + "','" + Current_year + "','" + Budget_year + "',GETDATE(),1,'" + Year_Of + "','" + Sub2_Desc_ID + "')", con);
                     con.Open();
                     com23.ExecuteNonQuery();


                  
                     return "1";
                 }
             }
             catch (Exception ex)
             {
                 return ex.Message.ToString();
             }
         }


        //add subs
         public string new1new2(int? Sub1_Desc_ID, string Sub1_Desc, int? Sub2_Desc_ID, string Sub2_Desc, double Past_year, double Current_year, double Budget_year, int? Type_ID, int? Year_Of)
         {
             try
             {
                 List<LBPOne_IDs> ok = new List<LBPOne_IDs>();
                 using (SqlConnection con = new SqlConnection(Common.MyConn()))
                 {
                     //new sub 1
                     SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_B_SUB1_List (Sub1_Desc,Date_of) values ('" + Sub1_Desc.Replace("'", "''") + "',GETDATE())", con);
                     con.Open();
                     com.ExecuteNonQuery();

                     SqlCommand com4 = new SqlCommand(@"SELECT Sub1_Desc_ID FROM IFMIS.dbo.tbl_R_BMS_B_SUB1_List where Sub1_Desc='" + Sub1_Desc.Replace("'", "''") + "'", con);
                     SqlDataReader reader = com4.ExecuteReader();
                     while (reader.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.Sub1_Desc_ID = reader.GetInt32(0);


                         ok.Add(SF_list);
                         Sub1_Desc_IDss = SF_list.Sub1_Desc_ID;
                     }
                     con.Close();

                     //new sub 2

                     SqlCommand comsub2 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_B_SUB2_List (Sub2_Desc,Date_of) values ('" + Sub2_Desc.Replace("'", "''") + "',GETDATE())", con);
                     con.Open();
                     comsub2.ExecuteNonQuery();

                     SqlCommand comsub22 = new SqlCommand(@"SELECT Sub2_Desc_ID FROM IFMIS.dbo.tbl_R_BMS_B_SUB2_List where Sub2_Desc='" + Sub2_Desc.Replace("'", "''") + "'", con);
                     SqlDataReader readersub2 = comsub22.ExecuteReader();
                     while (readersub2.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.Sub2_Desc_ID = readersub2.GetInt32(0);


                         ok.Add(SF_list);
                         Sub2_Desc_IDss = SF_list.Sub2_Desc_ID;
                     }
                     con.Close();



                     //insert SUB 1
                     SqlCommand com2 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_Sub1 (Sub1_Desc,Type_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Sub1_Desc_ID ) values ('" + Sub1_Desc.Replace("'", "''") + "','" + Type_ID + "',0,0,0,GETDATE(),1,'" + Year_Of + "','" + Sub1_Desc_IDss + "')", con);
                     con.Open();
                     com2.ExecuteNonQuery();
                     //get Sub 1 ID
                     SqlCommand comsubID = new SqlCommand(@"SELECT  Sub1_ID FROM IFMIS.dbo.tbl_R_BMS_A_Sub1 where Sub1_Desc='" + Sub1_Desc.Replace("'", "''") + "' and Type_ID = " + Type_ID + " and Action_Code = 1 and Year_Of = " + Year_Of + "", con);
                     SqlDataReader readersubID = comsubID.ExecuteReader();
                     while (readersubID.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.Sub1_ID = readersubID.GetInt32(0);
                         
                         ok.Add(SF_list);
                         Sub1_IDss = SF_list.Sub1_ID;
                     }
                     con.Close();
                     //insert SUB 2 with SUB1_ID
                     SqlCommand com23 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_Sub2 (Sub2_Desc,Sub1_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Sub2_Desc_ID) values ('" + Sub2_Desc.Replace("'", "''") + "','" + Sub1_IDss + "','" + Past_year + "','" + Current_year + "','" + Budget_year + "',GETDATE(),1,'" + Year_Of + "','" + Sub2_Desc_IDss + "')", con);
                     con.Open();
                     com23.ExecuteNonQuery();

                     //SqlCommand com4499 = new SqlCommand(@"SELECT Sub1_ID from IFMIS.dbo.tbl_R_BMS_A_Sub1 where Sub1_Desc='" + Sub1_Desc.Replace("'", "''") + "' and Type_ID='" + Type_ID + "' and Sub1_Desc_ID = '" + Sub1_Desc_IDss + "'", con);
                    
                     //SqlDataReader reader222 = com4499.ExecuteReader();
                     //while (reader222.Read())
                     //{
                     //    LBPOne_IDs SF_list = new LBPOne_IDs();
                     //    SF_list.Sub1_ID = reader222.GetInt32(0);


                     //    ok.Add(SF_list);
                     //    Sub1_IDss = SF_list.Sub1_ID;
                     //}
                     //con.Close();


                     //SqlCommand com66 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_B_SUB2_List (Sub2_Desc,Date_of) values ('" + Sub1_Desc.Replace("'", "''") + "',GETDATE())", con);
                     //con.Open();
                     //com66.ExecuteNonQuery();


                     //SqlCommand com466 = new SqlCommand(@"SELECT Sub2_Desc_ID FROM IFMIS.dbo.tbl_R_BMS_B_SUB2_List where Sub2_Desc = '" + Sub1_Desc.Replace("'", "''") + "'", con);
                     
                     //SqlDataReader reader66 = com466.ExecuteReader();
                     //while (reader66.Read())
                     //{
                     //    LBPOne_IDs SF_list = new LBPOne_IDs();
                     //    SF_list.Sub2_Desc_ID = reader66.GetInt32(0);


                     //    ok.Add(SF_list);
                     //    Sub2_Desc_IDss = SF_list.Sub2_Desc_ID;
                     //}
                     //con.Close();

                     //SqlCommand com442 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_Sub2 (Sub2_Desc,Sub1_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Sub2_Desc_ID) values ('" + Sub1_Desc.Replace("'", "''") + "','" + Sub1_IDss + "','" + Past_year + "','" + Current_year + "','" + Budget_year + "',GETDATE(),1,'" + Year_Of + "','" + Sub2_Desc_IDss + "')", con);
                     //con.Open();
                     //com442.ExecuteNonQuery();


                     return "1";
                 }

             }
             catch (Exception ex)
             {
                 return ex.Message.ToString();
             }

         }

         public string old1old1(int? Sub1_Desc_ID, string Sub1_Desc, int? Sub2_Desc_ID, string Sub2_Desc, double Past_year, double Current_year, double Budget_year, int? Type_ID, int? Year_Of)
         {
             try
             {
                 List<LBPOne_IDs> ok = new List<LBPOne_IDs>();
                 using (SqlConnection con = new SqlConnection(Common.MyConn()))
                 {

                     SqlCommand com2 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_Sub1 (Sub1_Desc,Type_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Sub1_Desc_ID ) values ('" + Sub1_Desc.Replace("'", "''") + "','" + Type_ID + "',0,0,0,GETDATE(),1,'" + Year_Of + "','" + Sub1_Desc_ID + "')", con);
                     con.Open();
                     com2.ExecuteNonQuery();
                     SqlCommand comsubID = new SqlCommand(@"SELECT  Sub1_ID FROM IFMIS.dbo.tbl_R_BMS_A_Sub1 where Sub1_Desc='" + Sub1_Desc.Replace("'", "''") + "' and Type_ID = " + Type_ID + " and Action_Code = 1 and Year_Of = " + Year_Of + "", con);
                     SqlDataReader readersubID = comsubID.ExecuteReader();
                     while (readersubID.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.Sub1_ID = readersubID.GetInt32(0);

                         ok.Add(SF_list);
                         Sub1_IDss = SF_list.Sub1_ID;
                     }
                     con.Close();
                     SqlCommand com23 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_Sub2 (Sub2_Desc,Sub1_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Sub2_Desc_ID) values ('" + Sub2_Desc.Replace("'", "''") + "','" + Sub1_IDss + "','" + Past_year + "','" + Current_year + "','" + Budget_year + "',GETDATE(),1,'" + Year_Of + "','" + Sub2_Desc_ID + "')", con);
                     con.Open();
                     com23.ExecuteNonQuery();

                     return "1";
                 }
             }
             catch (Exception ex)
             {
                 return ex.Message.ToString();
             }
         }

         public string RemoveSF(int? Fund_ID, int? Year_of)
         {
             try
             {
                 using (SqlConnection con = new SqlConnection(Common.MyConn()))
                 {
                     SqlCommand com = new SqlCommand("update [IFMIS].[dbo].[tbl_R_BMS_A_Sub2] set Date_Of = CONCAT(Date_Of,' , ', GETDATE()) , Action_Code = 2 where Sub2_ID in (select Sub2_ID   FROM [IFMIS].[dbo].[tbl_R_BMS_A_Sub2] where Sub1_ID in (select Sub1_ID   FROM [IFMIS].[dbo].[tbl_R_BMS_A_Sub1] where Type_ID in (select Type_ID  FROM [IFMIS].[dbo].[tbl_R_BMS_A_TypeFunds] where Fund_ID = '"+Fund_ID+"' and Year_Of = '"+Year_of+"')))", con);
                     con.Open();
                     com.ExecuteNonQuery();

                     SqlCommand com2 = new SqlCommand("update [IFMIS].[dbo].[tbl_R_BMS_A_Sub1] set Date_Of = CONCAT(Date_Of,' , ', GETDATE()) , Action_Code = 2 where Type_ID in (select Type_ID  FROM [IFMIS].[dbo].[tbl_R_BMS_A_TypeFunds] where Fund_ID = '" + Fund_ID + "' and Year_Of = '" + Year_of + "')", con);
                     com2.ExecuteNonQuery();

                     SqlCommand com3 = new SqlCommand("update [IFMIS].[dbo].[tbl_R_BMS_A_TypeFunds] set Date_Of = CONCAT(Date_Of,' , ', GETDATE()) , Action_Code = 2 where Fund_ID = '" + Fund_ID + "' and Year_Of = '" + Year_of + "'", con);
                     com3.ExecuteNonQuery();

                     SqlCommand com4 = new SqlCommand("update [IFMIS].[dbo].[tbl_R_BMS_A_SourceFunds] set Date_Of = CONCAT(Date_Of,' , ', GETDATE()) , Action_Code = 2 where Fund_ID = '" + Fund_ID + "' and Year_Of = '" + Year_of + "'", con);
                     com4.ExecuteNonQuery();


                     return "success";
                 }
             }
             catch (Exception ex)
             {
                 return ex.Message.ToString();
             }
         }

         public string RemoveTY(int? Type_ID, int? Year_of)
         {
             try
             {
                 using (SqlConnection con = new SqlConnection(Common.MyConn()))
                 {
                     SqlCommand com = new SqlCommand("update [IFMIS].[dbo].[tbl_R_BMS_A_Sub2] set Date_Of = CONCAT(Date_Of,' , ', GETDATE()) , Action_Code = 2 where Sub2_ID in (select Sub2_ID   FROM [IFMIS].[dbo].[tbl_R_BMS_A_Sub2] where Sub1_ID in (select Sub1_ID   FROM [IFMIS].[dbo].[tbl_R_BMS_A_Sub1] where Type_ID = '" + Type_ID + "'))", con);
                     con.Open();
                     com.ExecuteNonQuery();

                     SqlCommand com2 = new SqlCommand("update [IFMIS].[dbo].[tbl_R_BMS_A_Sub1] set Date_Of = CONCAT(Date_Of,' , ', GETDATE()) , Action_Code = 2 where Type_ID = '" + Type_ID + "'", con);
                     com2.ExecuteNonQuery();

                     SqlCommand com3 = new SqlCommand("update [IFMIS].[dbo].[tbl_R_BMS_A_TypeFunds] set Date_Of = CONCAT(Date_Of,' , ', GETDATE()) , Action_Code = 2 where Type_ID = '" + Type_ID + "'", con);
                     
                     com3.ExecuteNonQuery();

                    
                     return "success";
                 }
             }
             catch (Exception ex)
             {
                 return ex.Message.ToString();
             }
         }


         public LBPOne_Sub2Model editSubs(int? Sub2_ID, int? Year_of)
         {
             LBPOne_Sub2Model TrundsList = new LBPOne_Sub2Model();
             using (SqlConnection con = new SqlConnection(Common.MyConn()))
             {

                 SqlCommand com = new SqlCommand(@"SELECT a.Sub2_ID,a.Sub2_Desc,a.Sub1_ID,a.Year1_Amount,a.Year2_Amount,a.Year3_Amount,a.Date_Of,a.Action_Code,a.Year_Of,b.Sub1_Desc 
                                                    FROM IFMIS.dbo.tbl_R_BMS_A_Sub2 as a inner join IFMIS.dbo.tbl_R_BMS_A_Sub1 as b on a.Sub1_ID = b.Sub1_ID where a.Sub2_ID = '" + Sub2_ID + "' and b.Year_of = '" + Year_of + "' order by a.Sub1_ID", con);
                 con.Open();
                 SqlDataReader reader = com.ExecuteReader();
                 while (reader.Read())
                 {
                     TrundsList.Sub2_ID = reader.GetInt32(0);
                     TrundsList.Sub2_Desc = reader.GetValue(1).ToString();
                     TrundsList.Sub1_ID = reader.GetInt32(2);
                     TrundsList.Year1_Amount = Convert.ToDouble(reader.GetValue(3));
                     TrundsList.Year2_Amount = Convert.ToDouble(reader.GetValue(4));
                     TrundsList.Year3_Amount = Convert.ToDouble(reader.GetValue(5));
                     TrundsList.Date_Of = reader.GetValue(6).ToString();
                     TrundsList.Action_Code = reader.GetInt32(7);
                     TrundsList.Year_Of = reader.GetInt32(8);
                     TrundsList.Sub1_Desc = reader.GetValue(9).ToString();
                 }
                 return TrundsList;
             }
         }





         public string UpdateSub( int? Sub2_ID, string Sub1_Desc, string Sub2_Desc, double Past_year, double Current_year, double Budget_year, int? Type_ID, int? Year_Of)
         {
             try
             {

                 using (SqlConnection con = new SqlConnection(Common.MyConn()))
                 {



                     SqlCommand com2 = new SqlCommand("  update [IFMIS].[dbo].[tbl_R_BMS_A_Sub2] set Sub2_Desc = '" + Sub2_Desc.Replace("'", "''") + "' , Year1_Amount= " + Past_year + " ,Year2_Amount = " + Current_year + " ,Year3_Amount = " + Budget_year + " where Sub2_ID = " + Sub2_ID + " and Year_Of = " + Year_Of + "", con);
                     con.Open();
                     com2.ExecuteNonQuery();

                     return "1";
                 }
             }
             catch (Exception ex)
             {
                 return ex.Message.ToString();
             }
         }


         public string RemoveSubs(int? Sub2_ID,  int? Year_Of)
         {
             try
             {
                 using (SqlConnection con = new SqlConnection(Common.MyConn()))
                 {
                     SqlCommand com = new SqlCommand("update [IFMIS].[dbo].[tbl_R_BMS_A_Sub2] set Date_Of = CONCAT(Date_Of,' , ', GETDATE()) , Action_Code = 2 where Sub2_ID = " + Sub2_ID + " and Year_Of = "+Year_Of+"", con);
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



         public IEnumerable<LBPone_dllEcoEnter_Model> EEtype()
         {
             List<LBPone_dllEcoEnter_Model> SourceFund = new List<LBPone_dllEcoEnter_Model>();
             using (SqlConnection con = new SqlConnection(Common.MyConn()))
             {
                 SqlCommand com = new SqlCommand(@"SELECT Eco_Desc_ID,Eco_Desc FROM IFMIS.dbo.tbl_R_BMS_B_EEdesc", con);
                 con.Open();
                 SqlDataReader reader = com.ExecuteReader();
                 while (reader.Read())
                 {
                     LBPone_dllEcoEnter_Model SF_list = new LBPone_dllEcoEnter_Model();
                     SF_list.Eco_Desc_ID = reader.GetInt32(0);
                     SF_list.Eco_Desc = reader.GetValue(1).ToString();

                     SourceFund.Add(SF_list);
                 }
             }
             return SourceFund;
         }






        //add new Economic Enterprise ... new COMBO


         public string AddNewParticular(int? Eco_Desc_ID, string Eco_Desc, int? Year_of)
         {
             try
             {
                 List<LBPOne_IDs> ok = new List<LBPOne_IDs>();
                 using (SqlConnection con = new SqlConnection(Common.MyConn()))
                 {

                     SqlCommand com2 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_EEdesc (Eco_Desc_ID,Date_Of,Action_Code,Year_Of,Class_ID,Account_Code,Eco_Desc) values (" + Eco_Desc_ID + ",GETDATE(),1,'" + Year_of + "','','','" + Eco_Desc.Replace("'", "''") + "')", con);
                     con.Open();
                     com2.ExecuteNonQuery();

                     //get ID for dum type
                     SqlCommand com469 = new SqlCommand(@"SELECT top 1 Eco_ID  FROM IFMIS.dbo.tbl_R_BMS_A_EEdesc where Eco_Desc = '" + Eco_Desc.Replace("'", "''") + "' order by Eco_ID  desc ", con);
                     SqlDataReader reader69 = com469.ExecuteReader();
                     while (reader69.Read())
                     {
                         LBPOne_IDs SF_list69 = new LBPOne_IDs();
                         SF_list69.Eco_ID = reader69.GetInt32(0);


                         ok.Add(SF_list69);
                         Eco_IDss = SF_list69.Eco_ID;
                     }
                     con.Close();



                     //insert dum type
                     SqlCommand com23 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_EEType (Eco_Type_Desc_ID,Eco_ID,Date_Of,Action_Code,Year_Of,Eco_Type_Desc) values (0," + Eco_IDss + ",'date dum',0,0,'dum')", con);
                     con.Open();
                     com23.ExecuteNonQuery();

                     SqlCommand com47 = new SqlCommand(@"SELECT top 1 Eco_Type_ID FROM IFMIS.dbo.tbl_R_BMS_A_EEType where Eco_Type_Desc like '%dum%' order by Eco_Type_ID desc", con);
                     SqlDataReader reader7 = com47.ExecuteReader();
                     while (reader7.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.Eco_Type_ID = reader7.GetInt32(0);


                         ok.Add(SF_list);
                         Eco_Type_IDss = SF_list.Eco_Type_ID;
                     }
                     con.Close();

                     //insert to sub1 with type_id

                     SqlCommand com27 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_EESub1 (EE_Sub1_Desc_ID,Eco_Type_ID,Date_Of,Action_Code,Year_Of,EE_Sub1_Desc) values (0," + Eco_Type_IDss + ",'dum date',0,0,'dum')", con);
                     con.Open();
                     com27.ExecuteNonQuery();

                     SqlCommand comsubID = new SqlCommand(@"SELECT  EE_Sub1_ID FROM IFMIS.dbo.tbl_R_BMS_A_EESub1 where Eco_Type_ID = " + Eco_Type_IDss + "", con);
                     SqlDataReader readersubID = comsubID.ExecuteReader();
                     while (readersubID.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.EE_Sub1_ID = readersubID.GetInt32(0);

                         ok.Add(SF_list);
                         EE_Sub1_IDss = SF_list.EE_Sub1_ID;
                     }
                     con.Close();


                     SqlCommand com231 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_EESub2 (EE_Sub2_Desc_ID,EE_Sub1_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Account_Code,Class_ID,EE_Sub2_Desc) values (0," + EE_Sub1_IDss + ",0.0,0.0,0.0,'dum date',0,0,'dum acc',0,'dum')", con);
                     con.Open();
                     com231.ExecuteNonQuery();




                     return "1";
                 }
             }
             catch (Exception ex)
             {
                 return ex.Message.ToString();
             }
         }

        //Add just add existing shits

         public string AddNewParticularName(int? Eco_Desc_ID, string Eco_Desc, int? Year_of)
         {
             try
             {
                 List<LBPOne_IDs> ok = new List<LBPOne_IDs>();
                 using (SqlConnection con = new SqlConnection(Common.MyConn()))
                 {
                     SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_B_EEdesc (Eco_Desc ,Date_of) values ('" + Eco_Desc.Replace("'", "''") + "',GETDATE())", con);
                     con.Open();
                     com.ExecuteNonQuery();

                     SqlCommand com4 = new SqlCommand(@"SELECT Eco_Desc_ID FROM IFMIS.dbo.tbl_R_BMS_B_EEdesc where Eco_Desc = '" + Eco_Desc.Replace("'", "''") + "'", con);
                     SqlDataReader reader = com4.ExecuteReader();
                     while (reader.Read())
                     {
                         LBPOne_IDs EE_list = new LBPOne_IDs();
                         EE_list.Eco_Desc_ID = reader.GetInt32(0);


                         ok.Add(EE_list);
                         Eco_Desc_IDss = EE_list.Eco_Desc_ID;
                     }
                     con.Close();


                     SqlCommand com2 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_EEdesc (Eco_Desc_ID,Date_Of,Action_Code,Year_Of,Class_ID,Account_Code,Eco_Desc) values (" + Eco_Desc_IDss + ",GETDATE(),1,'" + Year_of + "','','','" + Eco_Desc.Replace("'", "''") + "')", con);
                     con.Open();
                     com2.ExecuteNonQuery();

                     //get ID for dum type
                     SqlCommand com469 = new SqlCommand(@"SELECT top 1 Eco_ID  FROM IFMIS.dbo.tbl_R_BMS_A_EEdesc where Fund_Desc = '" + Eco_Desc.Replace("'", "''") + "' order by Eco_ID  desc ", con);
                     SqlDataReader reader69 = com469.ExecuteReader();
                     while (reader69.Read())
                     {
                         LBPOne_IDs SF_list69 = new LBPOne_IDs();
                         SF_list69.Eco_ID = reader69.GetInt32(0);


                         ok.Add(SF_list69);
                         Eco_IDss = SF_list69.Eco_ID;
                     }
                     con.Close();



                     //insert dum type
                     SqlCommand com23 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_EEType (Eco_Type_Desc_ID,Eco_ID,Date_Of,Action_Code,Year_Of,Eco_Type_Desc) values (0," + Eco_IDss + ",'date dum',0,0,'dum')", con);
                     con.Open();
                     com23.ExecuteNonQuery();

                     SqlCommand com47 = new SqlCommand(@"SELECT top 1 Eco_Type_ID FROM IFMIS.dbo.tbl_R_BMS_A_EEType where Eco_Type_Desc like '%dum%' order by Eco_Type_ID desc", con);
                     SqlDataReader reader7 = com47.ExecuteReader();
                     while (reader7.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.Eco_Type_ID = reader7.GetInt32(0);


                         ok.Add(SF_list);
                         Eco_Type_IDss = SF_list.Eco_Type_ID;
                     }
                     con.Close();

                     //insert to sub1 with type_id

                     SqlCommand com27 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_EESub1 (EE_Sub1_Desc_ID,Eco_Type_ID,Date_Of,Action_Code,Year_Of,EE_Sub1_Desc) values (0," + Eco_Type_IDss + ",'dum date',0,0,'dum')", con);
                     con.Open();
                     com27.ExecuteNonQuery();

                     SqlCommand comsubID = new SqlCommand(@"SELECT  EE_Sub1_ID FROM IFMIS.dbo.tbl_R_BMS_A_EESub1 where Eco_Type_ID = " + Eco_Type_IDss + "", con);
                     SqlDataReader readersubID = comsubID.ExecuteReader();
                     while (readersubID.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.EE_Sub1_ID = readersubID.GetInt32(0);

                         ok.Add(SF_list);
                         EE_Sub1_IDss = SF_list.EE_Sub1_ID;
                     }
                     con.Close();


                     SqlCommand com231 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_EESub2 (EE_Sub2_Desc_ID,EE_Sub1_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Account_Code,Class_ID,EE_Sub2_Desc) values (0," + EE_Sub1_IDss + ",0.0,0.0,0.0,'dum date',0,0,'dum acc',0,'dum')", con);
                     con.Open();
                     com231.ExecuteNonQuery();


                     return "1";
                 }

             }
             catch (Exception ex)
             {
                 return ex.Message.ToString();
             }
         }






        // Read Grid Economic Enterprise

         public IEnumerable<LBPone_EEdescModel> LoadEE(int? Year_of)
         {


             List<LBPone_EEdescModel> eSourceFundList = new List<LBPone_EEdescModel>();

             using (SqlConnection con = new SqlConnection(Common.MyConn()))
             {

                 LBPone_EEdescModel eFirstList = new LBPone_EEdescModel();
                 eFirstList.Eco_ID = 0;
                 eFirstList.Eco_Desc = "I. Beginning Cash Balance";
                 eFirstList.Date_Of = "0";
                 eFirstList.Action_Code = 0;
                 eFirstList.Year_Of = 1;
                 eFirstList.Account_Code = "";
                 eFirstList.Classi_ = "";
                 eSourceFundList.Add(eFirstList);

                 LBPone_EEdescModel FirstLista = new LBPone_EEdescModel();
                 FirstLista.Eco_ID = 0;
                 FirstLista.Eco_Desc = "<span style='margin-left: 23px;'>    a.  Provincial Learning Center </span>";
                 FirstLista.Date_Of = "0";
                 FirstLista.Action_Code = 0;
                 FirstLista.Year_Of = 1;
                 FirstLista.Account_Code = "";
                 FirstLista.Classi_ = "NR";
                 eSourceFundList.Add(FirstLista);

                 LBPone_EEdescModel FirstListb = new LBPone_EEdescModel();
                 FirstListb.Eco_ID = 0;
                 FirstListb.Eco_Desc = "<span style='margin-left: 23px;'>    b.  Agricultural Resource Center </span>";
                 FirstListb.Date_Of = "0";
                 FirstListb.Action_Code = 0;
                 FirstListb.Year_Of = 1;
                 FirstListb.Account_Code = "";
                 FirstListb.Classi_ = "NR";
                 eSourceFundList.Add(FirstListb);

                 LBPone_EEdescModel FirstListc = new LBPone_EEdescModel();
                 FirstListc.Eco_ID = 0;
                 FirstListc.Eco_Desc = "<span style='margin-left: 23px;'>    a.  Provincial Learning Center </span>";
                 FirstListc.Date_Of = "0";
                 FirstListc.Action_Code = 0;
                 FirstListc.Year_Of = 1;
                 FirstListc.Account_Code = "";
                 FirstListc.Classi_ = "NR";
                 eSourceFundList.Add(FirstListc);

                 LBPone_EEdescModel eFirstListII = new LBPone_EEdescModel();
                 eFirstListII.Eco_ID = 0;
                 eFirstListII.Eco_Desc = "II. Receipts";
                 eFirstListII.Date_Of = "0";
                 eFirstListII.Action_Code = 0;
                 eFirstListII.Year_Of = 1;
                 eFirstListII.Account_Code = "";
                 eFirstListII.Classi_ = "";
                 eSourceFundList.Add(eFirstListII);


                 SqlCommand com = new SqlCommand(@"SELECT a.Eco_ID,a.Eco_Desc,sum(d.Year1_Amount),sum(d.Year2_Amount),sum(d.Year3_Amount),a.Year_Of FROM IFMIS.dbo.tbl_R_BMS_A_EEdesc as a
                                                inner join IFMIS.dbo.tbl_R_BMS_A_EEType as b on a.Eco_ID = b.Eco_ID
                                                inner join IFMIS.dbo.tbl_R_BMS_A_EESub1 as c on c.Eco_Type_ID = b.Eco_Type_ID
                                                inner join IFMIS.dbo.tbl_R_BMS_A_EESub2 as d on d.EE_Sub1_ID = c.EE_Sub1_ID
                                                 where  a.Year_of = '" + Year_of + "' and ((b.Action_Code = 1 or b.Action_Code = 0) and (c.Action_Code = 1 or c.Action_Code = 0) and (d.Action_Code = 1 or d.Action_Code = 0) and (a.Action_Code = 1 or a.Action_Code = 0))  group by a.Eco_ID,a.Eco_Desc,a.Year_Of", con);
                 con.Open();
                 SqlDataReader ereader = com.ExecuteReader();
                 while (ereader.Read())
                 {
                     LBPone_EEdescModel eSfunds = new LBPone_EEdescModel();
                     eSfunds.Eco_ID = ereader.GetInt32(0);
                     eSfunds.Eco_Desc = "<span style='margin-left: 15px;'>" + ereader.GetValue(1).ToString() + "</span>";
                     eSfunds.Year1_AmountEE = Convert.ToDouble(ereader.GetValue(2));
                     eSfunds.Year2_AmountEE = Convert.ToDouble(ereader.GetValue(3));
                     eSfunds.Year3_AmountEE = Convert.ToDouble(ereader.GetValue(4));
                     eSfunds.Year_Of = ereader.GetInt32(5);
                     eSfunds.DifferenceEE = Convert.ToDouble(ereader.GetValue(4)) - Convert.ToDouble(ereader.GetValue(3));
                     eSourceFundList.Add(eSfunds);
                 }

             }
             return eSourceFundList;
         }




         public IEnumerable<LBPone_EETypeModel> ReadEETF(int? Eco_ID, int? Year_of)
         {


             List<LBPone_EETypeModel> TF_list = new List<LBPone_EETypeModel>();

             using (SqlConnection con = new SqlConnection(Common.MyConn()))
             {
                 SqlCommand com = new SqlCommand(@"SELECT a.Eco_Type_ID ,a.Eco_Type_Desc,sum(c.Year1_Amount),sum(c.Year2_Amount),sum(c.Year3_Amount),a.Year_Of FROM IFMIS.dbo.tbl_R_BMS_A_EEType as a
                                                        inner join IFMIS.dbo.tbl_R_BMS_A_EESub1 as b on a.Eco_Type_ID = b.Eco_Type_ID
                                                        inner join IFMIS.dbo.tbl_R_BMS_A_EESub2 as c on b.EE_Sub1_ID = c.EE_Sub1_ID
                                                         where a.Eco_ID = " + Eco_ID + " and a.Year_of = '" + Year_of + "' and a.Action_Code = 1 and ((b.Action_Code = 1 or b.Action_Code = 0) and (c.Action_Code = 1 or c.Action_Code = 0)) group by a.Eco_Type_ID ,a.Eco_Type_Desc,a.Year_Of", con);
                 con.Open();
                 SqlDataReader reader = com.ExecuteReader();
                 while (reader.Read())
                 {
                     LBPone_EETypeModel Tfunds = new LBPone_EETypeModel();
                     Tfunds.Eco_Type_ID = reader.GetInt32(0);
                     Tfunds.Eco_Type_Desc = reader.GetValue(1).ToString();
                     Tfunds.Year1_AmountEE = Convert.ToDouble(reader.GetValue(2));
                     Tfunds.Year2_AmountEE = Convert.ToDouble(reader.GetValue(3));
                     Tfunds.Year3_AmountEE = Convert.ToDouble(reader.GetValue(4));
                     Tfunds.Year_Of = reader.GetInt32(5);
                     Tfunds.DifferenceEE = Convert.ToDouble(reader.GetValue(4)) - Convert.ToDouble(reader.GetValue(3));

                     TF_list.Add(Tfunds);
                 }

             }
             return TF_list;
         }


         public IEnumerable<LBPone_EESub2Model> readPartSub(int? Eco_Type_ID, int? Year_Of)
         {


             List<LBPone_EESub2Model> TF_list = new List<LBPone_EESub2Model>();

             using (SqlConnection con = new SqlConnection(Common.MyConn()))
             {
                 SqlCommand com = new SqlCommand(@"SELECT a.EE_Sub2_ID,a.EE_Sub2_Desc,a.EE_Sub1_ID,a.Year1_Amount,a.Year2_Amount,a.Year3_Amount,a.Date_Of,a.Action_Code,a.Year_Of,b.EE_Sub1_Desc 
                                                    FROM IFMIS.dbo.tbl_R_BMS_A_EESub2 as a inner join IFMIS.dbo.tbl_R_BMS_A_EESub1 as b on a.EE_Sub1_ID = b.EE_Sub1_ID where b.Eco_Type_ID = '" + Eco_Type_ID + "' and b.Year_of = '" + Year_Of + "' and a.Action_Code = 1 and b.Action_Code = 1 order by a.EE_Sub1_ID ", con);
                 con.Open();
                 SqlDataReader reader = com.ExecuteReader();
                 while (reader.Read())
                 {
                     LBPone_EESub2Model Tfunds = new LBPone_EESub2Model();
                     Tfunds.EE_Sub2_ID = reader.GetInt32(0);
                     Tfunds.EE_Sub2_Desc = reader.GetValue(1).ToString();
                     Tfunds.EE_Sub1_ID = reader.GetInt32(2);
                     Tfunds.Year1_AmountEE = Convert.ToDouble(reader.GetValue(3));
                     Tfunds.Year2_AmountEE = Convert.ToDouble(reader.GetValue(4));
                     Tfunds.Year3_AmountEE = Convert.ToDouble(reader.GetValue(5));
                     Tfunds.Date_Of = reader.GetValue(6).ToString();
                     Tfunds.Action_Code = reader.GetInt32(7);
                     Tfunds.Year_Of = reader.GetInt32(8);
                     Tfunds.EE_Sub1_Desc = reader.GetValue(9).ToString();
                     Tfunds.DifferenceEE = Convert.ToDouble(reader.GetValue(5)) - Convert.ToDouble(reader.GetValue(4));

                     TF_list.Add(Tfunds);
                 }

             }
             return TF_list;
         }


         public IEnumerable<LBPone_dllEcoEnter_Model> RTypeEE()
         {
             List<LBPone_dllEcoEnter_Model> Rtypes = new List<LBPone_dllEcoEnter_Model>();
             using (SqlConnection con = new SqlConnection(Common.MyConn()))
             {
                 SqlCommand com = new SqlCommand(@"SELECT Eco_Type_Desc_ID,Eco_Type_Desc FROM IFMIS.dbo.tbl_R_BMS_B_EEType", con);
                 con.Open();
                 SqlDataReader reader = com.ExecuteReader();
                 while (reader.Read())
                 {
                     LBPone_dllEcoEnter_Model RT_list = new LBPone_dllEcoEnter_Model();
                     RT_list.Eco_Type_Desc_ID = reader.GetInt32(0);
                     RT_list.Eco_Type_Desc = reader.GetValue(1).ToString();
                   
                     Rtypes.Add(RT_list);
                 }
             }
             return Rtypes;
         }




         public string addRTypeEE(int? Eco_Type_Desc_ID, string Eco_Type_Desc, int? Eco_ID, int? Year_of)
         {
             try
             {
                 List<LBPOne_IDs> ok = new List<LBPOne_IDs>();
                 using (SqlConnection con = new SqlConnection(Common.MyConn()))
                 {
                     SqlCommand com2 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_EEType (Eco_Type_Desc_ID,Eco_ID,Date_Of,Action_Code,Year_Of,Eco_Type_Desc) values ('" + Eco_Type_Desc_ID + "','" + Eco_ID + "',GETDATE(),1,'" + Year_of + "','" + Eco_Type_Desc.Replace("'", "''") + "')", con);
                     con.Open();
                     com2.ExecuteNonQuery();

                     SqlCommand com47 = new SqlCommand(@"SELECT top 1 Eco_Type_ID FROM IFMIS.dbo.tbl_R_BMS_A_EEType where Eco_Type_Desc = '" + Eco_Type_Desc.Replace("'", "''") + "' order by Eco_Type_ID desc", con);

                     SqlDataReader reader7 = com47.ExecuteReader();
                     while (reader7.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.Eco_Type_ID = reader7.GetInt32(0);


                         ok.Add(SF_list);
                         Eco_Type_IDss = SF_list.Eco_Type_ID;
                     }
                     con.Close();

                     //insert to sub1 with type_id


                     SqlCommand com27 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_EESub1 (EE_Sub1_Desc_ID,Eco_Type_ID,Date_Of,Action_Code,Year_Of,EE_Sub1_Desc) values (0," + Eco_Type_IDss + ",'dum date',0,0,'dum')", con);
                     con.Open();
                     com27.ExecuteNonQuery();

                     SqlCommand comsubID = new SqlCommand(@"SELECT  EE_Sub1_ID FROM IFMIS.dbo.tbl_R_BMS_A_EESub1 where Eco_Type_ID = " + Eco_Type_IDss + "", con);
                     SqlDataReader readersubID = comsubID.ExecuteReader();
                     while (readersubID.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.EE_Sub1_ID = readersubID.GetInt32(0);

                         ok.Add(SF_list);
                         EE_Sub1_IDss = SF_list.EE_Sub1_ID;
                     }
                     con.Close();


                     SqlCommand com231 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_EESub2 (EE_Sub2_Desc_ID,EE_Sub1_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Account_Code,Class_ID,EE_Sub2_Desc) values (0," + EE_Sub1_IDss + ",0.0,0.0,0.0,'dum date',0,0,'dum acc',0,'dum')", con);
                     con.Open();
                     com231.ExecuteNonQuery();



                     return "1";

                 }
             }
             catch (Exception ex)
             {
                 return ex.Message.ToString();
             }
         }


         //add type
         public string addRTypeEEName(int? Eco_Type_Desc_ID, string Eco_Type_Desc, int? Eco_ID, int? Year_of)
         {
             try
             {
                 List<LBPOne_IDs> ok = new List<LBPOne_IDs>();
                 using (SqlConnection con = new SqlConnection(Common.MyConn()))
                 {
                     SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_B_EEType (Eco_Type_Desc,Date_of) values ('" + Eco_Type_Desc.Replace("'", "''") + "',GETDATE())", con);
                     con.Open();
                     com.ExecuteNonQuery();

                     SqlCommand com4 = new SqlCommand(@"SELECT Eco_Type_Desc_ID FROM IFMIS.dbo.tbl_R_BMS_B_EEType where Eco_Type_Desc = '" + Eco_Type_Desc.Replace("'", "''") + "'", con);

                     SqlDataReader reader = com4.ExecuteReader();
                     while (reader.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.Type_Desc_ID = reader.GetInt32(0);


                         ok.Add(SF_list);
                         Type_Desc_IDss = SF_list.Type_Desc_ID;
                     }
                     con.Close();


                     SqlCommand com2 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_EEType (Eco_Type_Desc_ID,Eco_ID,Date_Of,Action_Code,Year_Of,Eco_Type_Desc) values ('" + Type_Desc_IDss + "','" + Eco_ID + "',GETDATE(),1,'" + Year_of + "','" + Eco_Type_Desc.Replace("'", "''") + "')", con);
                     con.Open();
                     com2.ExecuteNonQuery();

                     SqlCommand com47 = new SqlCommand(@"SELECT top 1 Eco_Type_ID FROM IFMIS.dbo.tbl_R_BMS_A_EEType where Eco_Type_Desc = '" + Eco_Type_Desc.Replace("'", "''") + "' order by Eco_Type_ID desc", con);

                     SqlDataReader reader7 = com47.ExecuteReader();
                     while (reader7.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.Eco_Type_ID = reader7.GetInt32(0);


                         ok.Add(SF_list);
                         Eco_Type_IDss = SF_list.Eco_Type_ID;
                     }
                     con.Close();

                     //insert to sub1 with type_id


                     SqlCommand com27 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_EESub1 (EE_Sub1_Desc_ID,Eco_Type_ID,Date_Of,Action_Code,Year_Of,EE_Sub1_Desc) values (0," + Eco_Type_IDss + ",'dum date',0,0,'dum')", con);
                     con.Open();
                     com27.ExecuteNonQuery();

                     SqlCommand comsubID = new SqlCommand(@"SELECT  EE_Sub1_ID FROM IFMIS.dbo.tbl_R_BMS_A_EESub1 where Eco_Type_ID = " + Eco_Type_IDss + "", con);
                     SqlDataReader readersubID = comsubID.ExecuteReader();
                     while (readersubID.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.EE_Sub1_ID = readersubID.GetInt32(0);

                         ok.Add(SF_list);
                         EE_Sub1_IDss = SF_list.EE_Sub1_ID;
                     }
                     con.Close();


                     SqlCommand com231 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_EESub2 (EE_Sub2_Desc_ID,EE_Sub1_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Account_Code,Class_ID,EE_Sub2_Desc) values (0," + EE_Sub1_IDss + ",0.0,0.0,0.0,'dum date',0,0,'dum acc',0,'dum')", con);
                     con.Open();
                     com231.ExecuteNonQuery();




                     return "1";
                 }

             }
             catch (Exception ex)
             {
                 return ex.Message.ToString();
             }

         }


        //_____________________________________________________ subs EE __________________________________________


         public IEnumerable<LBPone_dllEcoEnter_Model> SubComboEE()
         {
             List<LBPone_dllEcoEnter_Model> Subname = new List<LBPone_dllEcoEnter_Model>();
             using (SqlConnection con = new SqlConnection(Common.MyConn()))
             {
                 SqlCommand com = new SqlCommand(@"SELECT EE_Sub1_Desc_ID,EE_Sub1_Desc FROM IFMIS.dbo.tbl_R_BMS_B_EESubs1", con);
                 con.Open();
                 SqlDataReader reader = com.ExecuteReader();
                 while (reader.Read())
                 {
                     LBPone_dllEcoEnter_Model sub_list = new LBPone_dllEcoEnter_Model();
                     sub_list.EE_Sub1_Desc_ID = reader.GetInt32(0);
                     sub_list.EE_Sub1_Desc = reader.GetValue(1).ToString();

                     Subname.Add(sub_list);
                 }
             }
             return Subname;
         }


         public IEnumerable<LBPone_dllEcoEnter_Model> SubCombo2EE()
         {
             List<LBPone_dllEcoEnter_Model> Subname2 = new List<LBPone_dllEcoEnter_Model>();
             using (SqlConnection con = new SqlConnection(Common.MyConn()))
             {
                 SqlCommand com = new SqlCommand(@"SELECT EE_Sub2_Desc_ID,EE_Sub2_Desc FROM IFMIS.dbo.tbl_R_BMS_B_EESub2", con);
                 con.Open();
                 SqlDataReader reader = com.ExecuteReader();
                 while (reader.Read())
                 {
                     LBPone_dllEcoEnter_Model sub_list2 = new LBPone_dllEcoEnter_Model();
                     sub_list2.EE_Sub2_Desc_ID = reader.GetInt32(0);
                     sub_list2.EE_Sub2_Desc = reader.GetValue(1).ToString();

                     Subname2.Add(sub_list2);
                 }
             }
             return Subname2;
         }
         public IEnumerable<LBPone_dllEcoEnter_Model> SubCombo3EE()
         {
             List<LBPone_dllEcoEnter_Model> Subname3 = new List<LBPone_dllEcoEnter_Model>();
             using (SqlConnection con = new SqlConnection(Common.MyConn()))
             {
                 SqlCommand com = new SqlCommand(@"SELECT Class_ID,Class_Desc FROM IFMIS.dbo.tbl_R_BMS_B_EEClassi", con);
                 con.Open();
                 SqlDataReader reader = com.ExecuteReader();
                 while (reader.Read())
                 {
                     LBPone_dllEcoEnter_Model sub_list3 = new LBPone_dllEcoEnter_Model();
                     sub_list3.Class_ID = reader.GetInt32(0);
                     sub_list3.Class_Desc = reader.GetValue(1).ToString();

                     Subname3.Add(sub_list3);
                 }
             }
             return Subname3;
         }



         public string old1new1EE(int? EE_Sub1_Desc_ID, string EE_Sub1_Desc, int? EE_Sub2_Desc_ID, string EE_Sub2_Desc, double Past_year, double Current_year, double Budget_year, int? Eco_Type_ID, int? Year_Of, int? Class_ID, string Class_Desc, string Account_Code)
         {
             try
             {
                 List<LBPOne_IDs> ok = new List<LBPOne_IDs>();
                 using (SqlConnection con = new SqlConnection(Common.MyConn()))
                 {
                     if (Class_ID == null || Class_ID == 0)
                     {

                         SqlCommand comclass = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_B_EEClassi (Class_Desc) values ('" + Class_Desc.Replace("'", "''") + "'", con);
                         con.Open();
                         comclass.ExecuteNonQuery();

                         SqlCommand comclass2 = new SqlCommand(@"SELECT top 1 Class_ID FROM IFMIS.dbo.tbl_R_BMS_B_EEClassi where Class_Desc = '" + Class_Desc.Replace("'", "''") + "' order by Class_ID desc", con);
                         SqlDataReader readerClass = comclass2.ExecuteReader();
                         while (readerClass.Read())
                         {
                             LBPOne_IDs Class_list = new LBPOne_IDs();
                             Class_list.Class_ID = readerClass.GetInt32(0);


                             ok.Add(Class_list);
                             Class_IDss = Class_list.Class_ID;
                         }
                         con.Close();

                     }
                     else if (Class_ID != null || Class_ID != 0)
                     {

                         SqlCommand comclass2 = new SqlCommand(@"SELECT Class_ID FROM IFMIS.dbo.tbl_R_BMS_B_EEClassi where Class_ID = '" + Class_ID + "'", con);
                         con.Open();
                         SqlDataReader readerClass = comclass2.ExecuteReader();
                         while (readerClass.Read())
                         {
                             LBPOne_IDs Class_list = new LBPOne_IDs();
                             Class_list.Class_ID = readerClass.GetInt32(0);


                             ok.Add(Class_list);
                             Class_IDss = Class_list.Class_ID;
                         }
                         con.Close();

                     }
                     //new sub 2

                     SqlCommand comsub2 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_B_EESub2 (EE_Sub2_Desc,Date_of) values ('" + EE_Sub2_Desc.Replace("'", "''") + "',GETDATE())", con);
                     con.Open();
                     comsub2.ExecuteNonQuery();

                     SqlCommand comsub22 = new SqlCommand(@"SELECT top 1 EE_Sub2_Desc_ID FROM IFMIS.dbo.tbl_R_BMS_B_EESub2 where EE_Sub2_Desc = '" + EE_Sub2_Desc.Replace("'", "''") + "' order by EE_Sub2_Desc_ID desc", con);
                     SqlDataReader readersub2 = comsub22.ExecuteReader();
                     while (readersub2.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.Sub2_Desc_ID = readersub2.GetInt32(0);


                         ok.Add(SF_list);
                         Sub2_Desc_IDss = SF_list.Sub2_Desc_ID;
                     }
                     con.Close();
                     SqlCommand com2 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_EESub1 (EE_Sub1_Desc_ID,Eco_Type_ID,Date_Of,Action_Code,Year_Of,EE_Sub1_Desc) values  ('" + EE_Sub1_Desc_ID + "','" + Eco_Type_ID + "',GETDATE(),1,'" + Year_Of + "','" + EE_Sub1_Desc.Replace("'", "''") + "')", con);
                     con.Open();
                     com2.ExecuteNonQuery();
                     //get Sub 1 ID
                     SqlCommand comsubID = new SqlCommand(@"SELECT top 1 EE_Sub1_ID FROM IFMIS.dbo.tbl_R_BMS_A_EESub1 where EE_Sub1_Desc='" + EE_Sub1_Desc.Replace("'", "''") + "' and Eco_Type_ID = " + Eco_Type_ID + " and Action_Code = 1 and Year_Of = " + Year_Of + " and EE_Sub1_Desc_ID = " + EE_Sub1_Desc_ID + " order by EE_Sub1_ID desc", con);
                     SqlDataReader readersubID = comsubID.ExecuteReader();
                     while (readersubID.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.Sub1_ID = readersubID.GetInt32(0);

                         ok.Add(SF_list);
                         Sub1_IDss = SF_list.Sub1_ID;
                     }
                     con.Close();
                     //insert SUB 2 with SUB1_ID
                     SqlCommand com23 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_EESub2 (EE_Sub2_Desc_ID,EE_Sub1_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Account_Code,Class_ID,EE_Sub2_Desc) values ('" + Sub2_Desc_IDss + "','" + Sub1_IDss + "','" + Past_year + "','" + Current_year + "','" + Budget_year + "',GETDATE(),1,'" + Year_Of + "','" + Account_Code + "','" + Class_IDss + "','" + EE_Sub2_Desc.Replace("'", "''") + "')", con);
                     con.Open();
                     com23.ExecuteNonQuery();



                     return "1";
                 }
             }
             catch (Exception ex)
             {
                 return ex.Message.ToString();
             }
         }
         public string new1old2EE(int? EE_Sub1_Desc_ID, string EE_Sub1_Desc, int? EE_Sub2_Desc_ID, string EE_Sub2_Desc, double Past_year, double Current_year, double Budget_year, int? Eco_Type_ID, int? Year_Of, int? Class_ID, string Class_Desc, string Account_Code)
         {
             try
             {
                 List<LBPOne_IDs> ok = new List<LBPOne_IDs>();
                 using (SqlConnection con = new SqlConnection(Common.MyConn()))
                 {
                     if (Class_ID == null || Class_ID == 0)
                     {

                         SqlCommand comclass = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_B_EEClassi (Class_Desc) values ('" + Class_Desc.Replace("'", "''") + "'", con);
                         con.Open();
                         comclass.ExecuteNonQuery();

                         SqlCommand comclass2 = new SqlCommand(@"SELECT top 1 Class_ID FROM IFMIS.dbo.tbl_R_BMS_B_EEClassi where Class_Desc = '" + Class_Desc.Replace("'", "''") + "' order by Class_ID desc", con);
                         SqlDataReader readerClass = comclass2.ExecuteReader();
                         while (readerClass.Read())
                         {
                             LBPOne_IDs Class_list = new LBPOne_IDs();
                             Class_list.Class_ID = readerClass.GetInt32(0);


                             ok.Add(Class_list);
                             Class_IDss = Class_list.Class_ID;
                         }
                         con.Close();

                     }
                     else if (Class_ID != null || Class_ID != 0)
                     {

                         SqlCommand comclass2 = new SqlCommand(@"SELECT Class_ID FROM IFMIS.dbo.tbl_R_BMS_B_EEClassi where Class_ID = '" + Class_ID + "'", con);
                         con.Open();
                         SqlDataReader readerClass = comclass2.ExecuteReader();
                         while (readerClass.Read())
                         {
                             LBPOne_IDs Class_list = new LBPOne_IDs();
                             Class_list.Class_ID = readerClass.GetInt32(0);


                             ok.Add(Class_list);
                             Class_IDss = Class_list.Class_ID;
                         }
                         con.Close();

                     }
                     //new sub 1
                     //new sub 1
                     SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_B_EESubs1 (EE_Sub1_Desc,Date_of) values ('" + EE_Sub1_Desc.Replace("'", "''") + "',GETDATE())", con);
                     con.Open();
                     com.ExecuteNonQuery();

                     SqlCommand com4 = new SqlCommand(@"SELECT top 1 EE_Sub1_Desc_ID FROM IFMIS.dbo.tbl_R_BMS_B_EESubs1 where EE_Sub1_Desc = '" + EE_Sub1_Desc.Replace("'", "''") + "' order by EE_Sub1_Desc_ID desc", con);
                     SqlDataReader reader = com4.ExecuteReader();
                     while (reader.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.Sub1_Desc_ID = reader.GetInt32(0);


                         ok.Add(SF_list);
                         Sub1_Desc_IDss = SF_list.Sub1_Desc_ID;
                     }
                     con.Close();

                     //insert SUB 1
                     SqlCommand com2 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_EESub1 (EE_Sub1_Desc_ID,Eco_Type_ID,Date_Of,Action_Code,Year_Of,EE_Sub1_Desc) values  ('" + Sub1_Desc_IDss + "','" + Eco_Type_ID + "',GETDATE(),1,'" + Year_Of + "','" + EE_Sub1_Desc.Replace("'", "''") + "')", con);
                     con.Open();
                     com2.ExecuteNonQuery();
                     //get Sub 1 ID
                     SqlCommand comsubID = new SqlCommand(@"SELECT top 1 EE_Sub1_ID FROM IFMIS.dbo.tbl_R_BMS_A_EESub1 where EE_Sub1_Desc='" + EE_Sub1_Desc.Replace("'", "''") + "' and Eco_Type_ID = " + Eco_Type_ID + " and Action_Code = 1 and Year_Of = " + Year_Of + " and EE_Sub1_Desc_ID = " + Sub1_Desc_IDss + " order by EE_Sub1_ID desc", con);
                     SqlDataReader readersubID = comsubID.ExecuteReader();
                     while (readersubID.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.Sub1_ID = readersubID.GetInt32(0);

                         ok.Add(SF_list);
                         Sub1_IDss = SF_list.Sub1_ID;
                     }
                     con.Close();
                     //insert SUB 2 with SUB1_ID
                     SqlCommand com23 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_EESub2 (EE_Sub2_Desc_ID,EE_Sub1_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Account_Code,Class_ID,EE_Sub2_Desc) values ('" + EE_Sub2_Desc_ID + "','" + Sub1_IDss + "','" + Past_year + "','" + Current_year + "','" + Budget_year + "',GETDATE(),1,'" + Year_Of + "','" + Account_Code + "','" + Class_IDss + "','" + EE_Sub2_Desc.Replace("'", "''") + "')", con);
                     con.Open();
                     com23.ExecuteNonQuery();



                     return "1";
                 }
             }
             catch (Exception ex)
             {
                 return ex.Message.ToString();
             }
         }


         //add subs
         public string new1new2EE(int? EE_Sub1_Desc_ID, string EE_Sub1_Desc, int? EE_Sub2_Desc_ID, string EE_Sub2_Desc, double Past_year, double Current_year, double Budget_year, int? Eco_Type_ID, int? Year_Of, int? Class_ID, string Class_Desc, string Account_Code)
         {
             try
             {
                 List<LBPOne_IDs> ok = new List<LBPOne_IDs>();
                 using (SqlConnection con = new SqlConnection(Common.MyConn()))
                 {
                     //new sub 1
                     SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_B_EESubs1 (EE_Sub1_Desc,Date_of) values ('" + EE_Sub1_Desc.Replace("'", "''") + "',GETDATE())", con);
                     con.Open();
                     com.ExecuteNonQuery();

                     SqlCommand com4 = new SqlCommand(@"SELECT top 1 EE_Sub1_Desc_ID FROM IFMIS.dbo.tbl_R_BMS_B_EESubs1 where EE_Sub1_Desc = '" + EE_Sub1_Desc.Replace("'", "''") + "' order by EE_Sub1_Desc_ID desc", con);
                     SqlDataReader reader = com4.ExecuteReader();
                     while (reader.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.Sub1_Desc_ID = reader.GetInt32(0);


                         ok.Add(SF_list);
                         Sub1_Desc_IDss = SF_list.Sub1_Desc_ID;
                     }
                     con.Close();

                     //new sub 2

                     SqlCommand comsub2 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_B_EESub2 (EE_Sub2_Desc,Date_of) values ('" + EE_Sub2_Desc.Replace("'", "''") + "',GETDATE())", con);
                     con.Open();
                     comsub2.ExecuteNonQuery();

                     SqlCommand comsub22 = new SqlCommand(@"SELECT top 1 EE_Sub2_Desc_ID FROM IFMIS.dbo.tbl_R_BMS_B_EESub2 where EE_Sub2_Desc = '" + EE_Sub2_Desc.Replace("'", "''") + "' order by EE_Sub2_Desc_ID desc", con);
                     SqlDataReader readersub2 = comsub22.ExecuteReader();
                     while (readersub2.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.Sub2_Desc_ID = readersub2.GetInt32(0);


                         ok.Add(SF_list);
                         Sub2_Desc_IDss = SF_list.Sub2_Desc_ID;
                     }
                     con.Close();



                     //new Class
                     if (Class_ID == null || Class_ID == 0) {

                         SqlCommand comclass = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_B_EEClassi (Class_Desc) values ('" + Class_Desc.Replace("'", "''") + "'", con);
                         con.Open();
                         comclass.ExecuteNonQuery();

                         SqlCommand comclass2 = new SqlCommand(@"SELECT top 1 Class_ID FROM IFMIS.dbo.tbl_R_BMS_B_EEClassi where Class_Desc = '" + Class_Desc.Replace("'", "''") + "' order by Class_ID desc", con);
                         SqlDataReader readerClass = comclass2.ExecuteReader();
                         while (readerClass.Read())
                         {
                             LBPOne_IDs Class_list = new LBPOne_IDs();
                             Class_list.Class_ID = readerClass.GetInt32(0);


                             ok.Add(Class_list);
                             Class_IDss = Class_list.Class_ID;
                         }
                         con.Close();

                     }
                     else if (Class_ID != null || Class_ID != 0)
                     {

                         SqlCommand comclass2 = new SqlCommand(@"SELECT Class_ID FROM IFMIS.dbo.tbl_R_BMS_B_EEClassi where Class_ID = '" + Class_ID + "'", con);
                         con.Open();
                         SqlDataReader readerClass = comclass2.ExecuteReader();
                         while (readerClass.Read())
                         {
                             LBPOne_IDs Class_list = new LBPOne_IDs();
                             Class_list.Class_ID = readerClass.GetInt32(0);


                             ok.Add(Class_list);
                             Class_IDss = Class_list.Class_ID;
                         }
                         con.Close();

                     }

                    


                     //insert SUB 1
                     SqlCommand com2 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_EESub1 (EE_Sub1_Desc_ID,Eco_Type_ID,Date_Of,Action_Code,Year_Of,EE_Sub1_Desc) values  ('" + Sub1_Desc_IDss + "','" + Eco_Type_ID + "',GETDATE(),1,'" + Year_Of + "','" + EE_Sub1_Desc.Replace("'", "''") + "')", con);
                     con.Open();
                     com2.ExecuteNonQuery();
                     //get Sub 1 ID
                     SqlCommand comsubID = new SqlCommand(@"SELECT top 1 EE_Sub1_ID FROM IFMIS.dbo.tbl_R_BMS_A_EESub1 where EE_Sub1_Desc='" + EE_Sub1_Desc.Replace("'", "''") + "' and Eco_Type_ID = " + Eco_Type_ID + " and Action_Code = 1 and Year_Of = " + Year_Of + " and EE_Sub1_Desc_ID = " + Sub1_Desc_IDss + " order by EE_Sub1_ID desc", con);
                     SqlDataReader readersubID = comsubID.ExecuteReader();
                     while (readersubID.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.Sub1_ID = readersubID.GetInt32(0);

                         ok.Add(SF_list);
                         Sub1_IDss = SF_list.Sub1_ID;
                     }
                     con.Close();
                     //insert SUB 2 with SUB1_ID
                     SqlCommand com23 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_EESub2 (EE_Sub2_Desc_ID,EE_Sub1_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Account_Code,Class_ID,EE_Sub2_Desc) values ('" + Sub2_Desc_IDss + "','" + Sub1_IDss + "','" + Past_year + "','" + Current_year + "','" + Budget_year + "',GETDATE(),1,'" + Year_Of + "','"+Account_Code+"','"+ Class_IDss +"','" + EE_Sub2_Desc.Replace("'", "''") + "')", con);
                     con.Open();
                     com23.ExecuteNonQuery();

                   

                     return "1";
                 }

             }
             catch (Exception ex)
             {
                 return ex.Message.ToString();
             }

         }

         public string old1old1EE(int? EE_Sub1_Desc_ID, string EE_Sub1_Desc, int? EE_Sub2_Desc_ID, string EE_Sub2_Desc, double Past_year, double Current_year, double Budget_year, int? Eco_Type_ID, int? Year_Of, int? Class_ID, string Class_Desc, string Account_Code)
         {
             try
             {
                 List<LBPOne_IDs> ok = new List<LBPOne_IDs>();
                 using (SqlConnection con = new SqlConnection(Common.MyConn()))
                 {

                     if (Class_ID == null || Class_ID == 0)
                     {

                         SqlCommand comclass = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_B_EEClassi (Class_Desc) values ('" + Class_Desc.Replace("'", "''") + "'", con);
                         con.Open();
                         comclass.ExecuteNonQuery();

                         SqlCommand comclass2 = new SqlCommand(@"SELECT top 1 Class_ID FROM IFMIS.dbo.tbl_R_BMS_B_EEClassi where Class_Desc = '" + Class_Desc.Replace("'", "''") + "' order by Class_ID desc", con);
                         SqlDataReader readerClass = comclass2.ExecuteReader();
                         while (readerClass.Read())
                         {
                             LBPOne_IDs Class_list = new LBPOne_IDs();
                             Class_list.Class_ID = readerClass.GetInt32(0);


                             ok.Add(Class_list);
                             Class_IDss = Class_list.Class_ID;
                         }
                         con.Close();

                     }
                     else if (Class_ID != null || Class_ID != 0)
                     {

                         SqlCommand comclass2 = new SqlCommand(@"SELECT Class_ID FROM IFMIS.dbo.tbl_R_BMS_B_EEClassi where Class_ID = '" + Class_ID + "'", con);
                         con.Open();
                         SqlDataReader readerClass = comclass2.ExecuteReader();
                         while (readerClass.Read())
                         {
                             LBPOne_IDs Class_list = new LBPOne_IDs();
                             Class_list.Class_ID = readerClass.GetInt32(0);


                             ok.Add(Class_list);
                             Class_IDss = Class_list.Class_ID;
                         }
                         con.Close();

                     }

                     SqlCommand com2 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_EESub1 (EE_Sub1_Desc_ID,Eco_Type_ID,Date_Of,Action_Code,Year_Of,EE_Sub1_Desc) values  ('" + EE_Sub1_Desc_ID + "','" + Eco_Type_ID + "',GETDATE(),1,'" + Year_Of + "','" + EE_Sub1_Desc.Replace("'", "''") + "')", con);
                     con.Open();
                     com2.ExecuteNonQuery();
                     //get Sub 1 ID
                     SqlCommand comsubID = new SqlCommand(@"SELECT top 1 EE_Sub1_ID FROM IFMIS.dbo.tbl_R_BMS_A_EESub1 where EE_Sub1_Desc='" + EE_Sub1_Desc.Replace("'", "''") + "' and Eco_Type_ID = " + Eco_Type_ID + " and Action_Code = 1 and Year_Of = " + Year_Of + " and EE_Sub1_Desc_ID = " + EE_Sub1_Desc_ID + " order by EE_Sub1_ID desc", con);
                     SqlDataReader readersubID = comsubID.ExecuteReader();
                     while (readersubID.Read())
                     {
                         LBPOne_IDs SF_list = new LBPOne_IDs();
                         SF_list.Sub1_ID = readersubID.GetInt32(0);

                         ok.Add(SF_list);
                         Sub1_IDss = SF_list.Sub1_ID;
                     }
                     con.Close();
                     //insert SUB 2 with SUB1_ID
                     SqlCommand com23 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_A_EESub2 (EE_Sub2_Desc_ID,EE_Sub1_ID,Year1_Amount,Year2_Amount,Year3_Amount,Date_Of,Action_Code,Year_Of,Account_Code,Class_ID,EE_Sub2_Desc) values ('" + EE_Sub2_Desc_ID + "','" + Sub1_IDss + "','" + Past_year + "','" + Current_year + "','" + Budget_year + "',GETDATE(),1,'" + Year_Of + "','" + Account_Code + "','" + Class_IDss + "','" + EE_Sub2_Desc.Replace("'", "''") + "')", con);
                     con.Open();
                     com23.ExecuteNonQuery();

                     return "1";
                 }
             }
             catch (Exception ex)
             {
                 return ex.Message.ToString();
             }
         }

         public LBPone_EESub2Model editSubsEE(int? EE_Sub2_ID, int? Year_of)
         {
             LBPone_EESub2Model TrundsList = new LBPone_EESub2Model();
             using (SqlConnection con = new SqlConnection(Common.MyConn()))
             {

                 SqlCommand com = new SqlCommand(@"SELECT a.EE_Sub2_ID,a.EE_Sub1_ID,a.Year1_Amount,a.Year2_Amount,a.Year3_Amount,a.Date_Of,a.Action_Code,a.Year_Of,a.Account_Code,a.Class_ID,a.EE_Sub2_Desc , b.EE_Sub1_Desc
                                                    FROM IFMIS.dbo.tbl_R_BMS_A_EESub2 as a inner join IFMIS.dbo.tbl_R_BMS_A_EESub1 as b on a.EE_Sub1_ID = b.EE_Sub1_ID
                                                    where a.EE_Sub2_ID = '" + EE_Sub2_ID + "' and a.Action_Code = 1 and a.Year_Of = '" + Year_of + "'", con);
                 con.Open();
                 SqlDataReader reader = com.ExecuteReader();
                 while (reader.Read())
                 {
                     TrundsList.EE_Sub2_ID = reader.GetInt32(0);
                     TrundsList.EE_Sub1_ID = reader.GetInt32(1);
                     TrundsList.Year1_AmountEE = Convert.ToDouble(reader.GetValue(2));
                     TrundsList.Year2_AmountEE = Convert.ToDouble(reader.GetValue(3));
                     TrundsList.Year3_AmountEE = Convert.ToDouble(reader.GetValue(4));
                     TrundsList.Date_Of = reader.GetValue(5).ToString();
                     TrundsList.Action_Code = reader.GetInt32(6);
                     TrundsList.Year_Of = reader.GetInt32(7);
                     TrundsList.Account_Code = reader.GetValue(8).ToString();
                     TrundsList.Class_ID = reader.GetInt32(9);
                     TrundsList.EE_Sub2_Desc = reader.GetValue(10).ToString();
                     TrundsList.EE_Sub1_Desc = reader.GetValue(11).ToString();
                 }
                 return TrundsList;
             }
         }


         public string UpdateSubEE(int? EE_Sub2_ID, string EE_Sub1_Desc, string EE_Sub2_Desc, double Year1_AmountEE, double Year2_AmountEE, double Year3_AmountEE, int? Eco_Type_ID, int? Year_Of, string Account_Code, int? Class_ID)
         {
             try
             {

                 using (SqlConnection con = new SqlConnection(Common.MyConn()))
                 {



                     SqlCommand com2 = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_A_EESub2 set EE_Sub2_Desc = '" + EE_Sub2_Desc + "' , Year1_Amount = '" + Year1_AmountEE + "',Year2_Amount = '" + Year2_AmountEE + "',Year3_Amount = '" + Year3_AmountEE + "', " +
                                                        "Date_Of = CONCAT(Date_Of,' , ', GETDATE()) , Action_Code = 1, Account_Code = '" + Account_Code + "',Class_ID = '" + Class_ID + "' " +
                                                       "where EE_Sub2_ID = '" + EE_Sub2_ID + "' and Year_Of = '"+ Year_Of +"' and Action_Code = 1", con);
                     con.Open();
                     com2.ExecuteNonQuery();

                     return "1";
                 }
             }
             catch (Exception ex)
             {
                 return ex.Message.ToString();
             }
         }


         public string RemoveTYEE(int? Eco_Type_ID, int? Year_Of)
         {
             try
             {
                 using (SqlConnection con = new SqlConnection(Common.MyConn()))
                 {
                     SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_A_EESub2 set Date_Of = CONCAT(Date_Of,' , ', GETDATE()) , Action_Code = 2 where EE_Sub1_ID in (select EE_Sub1_ID FROM IFMIS.dbo.tbl_R_BMS_A_EESub1 WHERE Eco_Type_ID = '" + Eco_Type_ID + "') and Year_Of = '" + Year_Of + "' and Action_Code = 1", con);
                     con.Open();
                     com.ExecuteNonQuery();

                     SqlCommand com2 = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_A_EESub1 set Date_Of = CONCAT(Date_Of,' , ', GETDATE()) , Action_Code = 2 WHERE Eco_Type_ID = '" + Eco_Type_ID + "' and Year_Of = '" + Year_Of + "' and Action_Code = 1", con);
                     com2.ExecuteNonQuery();

                     SqlCommand com3 = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_A_EEType set Date_Of = CONCAT(Date_Of,' , ', GETDATE()) , Action_Code = 2 where Eco_Type_ID = '" + Eco_Type_ID + "' and Year_Of = '" + Year_Of + "' and Action_Code = 1", con);

                     com3.ExecuteNonQuery();


                     return "success";
                 }
             }
             catch (Exception ex)
             {
                 return ex.Message.ToString();
             }
         }


         public string RemoveSubsEE(int? EE_Sub2_ID, int? Year_Of)
         {
             try
             {
                 using (SqlConnection con = new SqlConnection(Common.MyConn()))
                 {
                     SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_A_EESub2 set Date_Of = CONCAT(Date_Of,' , ', GETDATE()) , Action_Code = 2 where EE_Sub2_ID = '" + EE_Sub2_ID + "' and Year_Of = '" + Year_Of + "' and Action_Code = 1", con);
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


         public string RemoveSFEE(int? Eco_ID, int? Year_Of)
         {
             try
             {
                 using (SqlConnection con = new SqlConnection(Common.MyConn()))
                 {
                     SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_A_EESub2 set Date_Of = CONCAT(Date_Of,' , ', GETDATE()) , Action_Code = 2 where EE_Sub1_ID in (select EE_Sub1_ID FROM IFMIS.dbo.tbl_R_BMS_A_EESub1 WHERE Eco_Type_ID in (SELECT Eco_Type_ID FROM IFMIS.dbo.tbl_R_BMS_A_EEType where Eco_ID = '" + Eco_ID + "')) and Year_Of = '" + Year_Of + "' and Action_Code = 1", con);
                     con.Open();
                     com.ExecuteNonQuery();

                     SqlCommand com2 = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_A_EESub1 set Date_Of = CONCAT(Date_Of,' , ', GETDATE()) , Action_Code = 2 WHERE Eco_Type_ID in (SELECT Eco_Type_ID FROM IFMIS.dbo.tbl_R_BMS_A_EEType where Eco_ID = '" + Eco_ID + "') and Year_Of = '" + Year_Of + "' and Action_Code = 1", con);
                     com2.ExecuteNonQuery();

                     SqlCommand com3 = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_A_EEType set Date_Of = CONCAT(Date_Of,' , ', GETDATE()) , Action_Code = 2 where Eco_Type_ID in (SELECT Eco_Type_ID FROM IFMIS.dbo.tbl_R_BMS_A_EEType where Eco_ID = '" + Eco_ID + "') and Year_Of = '" + Year_Of + "' and Action_Code = 1", con);
                     com3.ExecuteNonQuery();

                     SqlCommand com4 = new SqlCommand("SELECT Eco_ID FROM IFMIS.dbo.tbl_R_BMS_A_EEdesc where Eco_ID = '" + Eco_ID + "' and Year_Of = '" + Year_Of + "' and Action_Code = 1", con);
                     com4.ExecuteNonQuery();


                     return "success";
                 }
             }
             catch (Exception ex)
             {
                 return ex.Message.ToString();
             }
         }
         public string GenerateForm1Data(int YearOf)
         {
             using (SqlConnection con = new SqlConnection(Common.MyConn()))
             {
                 SqlCommand com = new SqlCommand("sp_BMS_GenerateLBPForm1Data " + (YearOf - 1) + "," + YearOf + "", con);
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

    }
}