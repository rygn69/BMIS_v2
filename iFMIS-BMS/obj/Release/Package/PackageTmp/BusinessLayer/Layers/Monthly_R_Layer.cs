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
    public class Monthly_R_Layer
    {
        Int64 release_Ay = 0;
        Int64 release_float_id_float = 0;
        int FMISOfficeCode_float = 0;
        double Amount_float = 0;
        int MonthOf_float = 0;
        int YearOf_float = 0;
        string DateTimeEntered_float;
        int batch_float = 0;


        //realignment edit_____
        int FromOfficeCodess = 0;
        int FromProgramCodess = 0;
        int FromAccountCodess = 0;
        int ToOfficeCodess = 0;
        int ToProgramCodess = 0;
        int ToAccountCodess = 0;
        double AmountCodess = 0;
        string UserIDCodess;
        string DateTimeEnteredCodess;
        int yearOfCodess = 0;


        //fmis____SUPP

        Int64 SBCodess_OLD = 0;
       // Int64 SBCodess_EDIT_OLD = 0;
        int FMISOfficeCode_SUP = 0;
        int FMISProgramCode_SUP = 0;
        int FMISAccountCode_SUP = 0;
        int OOECode_SUP = 0;
        string Description_SUP;
        double Amount_SUP = 0;
        int MonthOf_SUP = 0;
        int YearOf_SUP = 0;
       // int UserID_SUP = 0;
        string DateTimeEntered_SUP;

        //fmis_legalCode
        Int64 LegalCode_ = 0;
        string LegalDescription_;
        string LegalDescriptionOther;
        Int32 LegalbasisID = 0;


        Int64 SBCode_edit = 0;
        Int64 SBCodess = 0;
        string Descriptionss;
        Int64 SBCodess_empty = 0;
        string Descriptionss_empty;
       // Int64 release_float_ids = 0;
        Int64 release_float_idsEE = 0;
        
        Int64 incomerelease_float_ids = 0;
        Int64 subsidyrelease_float_ids = 0;

        //FLOAT______________
        int FMISOfficeCode_FLOAT = 0;
        int FMISProgramCode_FLOAT = 0;
        int FMISAccountCode_FLOAT = 0;
        double AmountPS_FLOAT = 0;
        double AmountMOOE_FLOAT = 0;
        double AmountCO_FLOAT = 0;
        double BalancePS_FLOAT = 0;
        double BalanceMOOE_FLOAT = 0;
        double BalanceCO_FLOAT = 0;
        int Batch_FLOAT = 0;
        int WithSubsidyFlag_FLOAT = 0;
        int MonthOf_FLOAT = 0;
        int YearOf_FLOAT = 0;
        string UserID_FLOAT;
        string DateTimeEntered_FLOAT;
        string DateReleased_FLOAT;


        // INC

        int FMISOfficeCode_INC = 0;
        double Amount_INC = 0;
        int MonthOf_INC = 0;
        int YearOf_INC = 0;
        string DateTimeEntered_INC;

        //Release
        int FMISOfficeCode_Rel = 0;
        int FMISProgramCode_Rel = 0;
        int FMISAccountCode_Rel = 0;
        double Amount_PS_Rel = 0;
        double Amount_MOOE_Rel = 0;
        double Amount_CO_Rel = 0;
        int MonthOf_Rel = 0;
        int YearOf_Rel = 0;
        string DateTimeEntered_Rel;



            
        public IEnumerable<MonthlyR_Model> Monthly_Release_GF(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_)
        {
            List<MonthlyR_Model> MRsList_GF = new List<MonthlyR_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"sp_MonthlyRelease_view " + office_id + "," + program_id + "," + ooe_id + "," + account_id + ",12," + numeric_ + "," + year_ + "", con);
               // SqlCommand com = new SqlCommand(@"sp_MonthlyRelease_view '4','4','1','299','9','1','2016'", con);

                con.Open();
                com.CommandTimeout = 0;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    MonthlyR_Model Mos = new MonthlyR_Model();
                    Mos.monthly_id = reader.GetInt32(0);
                    Mos.subjects = reader.GetValue(1).ToString();
                    Mos.ps = Convert.ToDouble(reader.GetValue(2));
                    Mos.mooe = Convert.ToDouble(reader.GetValue(3));
                    Mos.co = Convert.ToDouble(reader.GetValue(4));


                    MRsList_GF.Add(Mos);
                //} con.Close();
                
                ////SqlCommand com2 = new SqlCommand(@"sp_MonthlyRelease_view '" + office_id + "','" + program_id + "','" + ooe_id + "','" + account_id + "','" + month_ + "','" + numeric_ + "','" + year_ + "'", con);
                // SqlCommand com2 = new SqlCommand(@"sp_MonthlyRelease_view '65','70','1','299','9','1','2016'", con);
                // con.Open();
                //SqlDataReader reader2 = com2.ExecuteReader();
                //while (reader2.Read())
                //{
                //    MonthlyR_Model Mos2 = new MonthlyR_Model();
                //    Mos2.monthly_id = reader2.GetInt32(0);
                //    Mos2.subjects = reader2.GetValue(1).ToString();
                //    Mos2.ps = Convert.ToDouble(reader2.GetValue(2));
                //    Mos2.mooe = Convert.ToDouble(reader2.GetValue(3));
                //    Mos2.co = Convert.ToDouble(reader2.GetValue(4));


                //    MRsList_GF.Add(Mos2);
                }
            }
            return MRsList_GF;
        }

        public IEnumerable<MonthlyR_Model> Monthly_Release_EEqwgsgrsgrerer(int? Year)
        {
            List<MonthlyR_Model> MRsList_EE = new List<MonthlyR_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select something", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    MonthlyR_Model Trunds = new MonthlyR_Model();
                    Trunds.monthly_id = reader.GetInt32(0);
                    Trunds.subjects = reader.GetValue(2).ToString();
                    Trunds.ps = Convert.ToDouble(reader.GetValue(3));
                    Trunds.mooe = Convert.ToDouble(reader.GetValue(3));
                    Trunds.co = Convert.ToDouble(reader.GetValue(3));
                    Trunds.subsidy = Convert.ToDouble(reader.GetValue(3));
                    Trunds.income = Convert.ToDouble(reader.GetValue(3));


                    MRsList_EE.Add(Trunds);
                }

            }
            return MRsList_EE;
        }



        public string save_reserve(int? office_id, int? program_id, int? ooe_id, int? account_id, decimal percent, double money, int? year_)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("sp_MonthlyRelease_view '65','70','1','299','9','1','2016'", con);
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

        public string save_reserve_null(int? office_id, int? program_id, int? ooe_id, int? account_id, decimal percent, double money, int? year_, int? reserve_flag)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_Reserve set ActionCode = 3 ,DateTimeEntered = CONCAT(DateTimeEntered,' , ', format(GetDate(),'M/d/yyy hh:mm:ss tt')) , userID = CONCAT(userID,' , ', '" + Account.UserInfo.eid.ToString() + "') where FMISOfficeCode = " + office_id + " and FMISProgramCode = " + program_id + " and FMISAccountCode = " + account_id + " and YearOf = " + year_ + " and ActionCode = 1", con);
                    con.Open();
                    com.ExecuteNonQuery();
                  
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"update fmis.dbo.tblBMS_Reserve set ActionCode = 3 ,DateTimeEntered = CONCAT(DateTimeEntered COLLATE Latin1_General_CI_AS,' , ' COLLATE Latin1_General_CI_AS, format(GetDate(),'M/d/yyy hh:mm:ss tt') COLLATE Latin1_General_CI_AS) , userID = CONCAT(userID COLLATE Latin1_General_CI_AS,' , ' COLLATE Latin1_General_CI_AS, '" + Account.UserInfo.eid.ToString() + "' COLLATE Latin1_General_CI_AS) where OfficeCode = " + office_id + " and ProgCode = " + program_id + " and AccountCode = " + account_id + " and YearOf = " + year_ + " and ActionCode = 1", con);
                    con.Open();
                    com.ExecuteNonQuery();

                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("  insert into fmis.dbo.tblBMS_Reserve (OfficeCode ,ProgCode ,AccountCode ,ReservePercent ,ActionCode ,UserID ,DateTimeEntered ,YearOf ,Amount ,AmountFlag) " +
                                "values ('" + office_id + "','" + program_id + "','" + account_id + "','" + percent + "',1,'" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),'" + year_ + "','" + money + "','" + reserve_flag + "')", con);
                    con.Open();
                    com.ExecuteNonQuery();
                   
                }
                
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("  insert into IFMIS.dbo.tbl_R_BMS_Reserve (FMISOfficeCode,FMISProgramCode,FMISAccountCode,ReservePercent,ActionCode,UserID,DateTimeEntered,YearOf,Amount,reserve_flag) " +
                                "values ('" + office_id + "','" + program_id + "','" + account_id + "','" + percent + "',1,'" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),'" + year_ + "','" + money + "','" + reserve_flag + "')", con);
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

        //int reserve_opis = 0;
        //int reserve_program = 0;
        int reserve_accounts = 0;
        //decimal reserve_percent = 0;
        //double reserve_money = 0;
        //int reserve_year_ = 0;
        //int reserve_reserve_flag = 0;

        public string save_reserve_acc(int? office_id, int? program_id, int? ooe_id, int? account_id, decimal percent, double money, int? year_, int? reserve_flag)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_Reserve set ActionCode = 3 ,DateTimeEntered = CONCAT(DateTimeEntered,' , ', format(GetDate(),'M/d/yyy hh:mm:ss tt')) , userID = CONCAT(userID,' , ', '" + Account.UserInfo.eid.ToString() + "') where FMISOfficeCode = " + office_id + " and FMISProgramCode = " + program_id + " and YearOf = " + year_ + " and ActionCode = 1", con);
                    con.Open();
                    com.ExecuteNonQuery();

                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"update fmis.dbo.tblBMS_Reserve set ActionCode = 3 ,DateTimeEntered = CONCAT(DateTimeEntered COLLATE Latin1_General_CI_AS,' , ' COLLATE Latin1_General_CI_AS, format(GetDate(),'M/d/yyy hh:mm:ss tt') COLLATE Latin1_General_CI_AS) , userID = CONCAT(userID COLLATE Latin1_General_CI_AS,' , ' COLLATE Latin1_General_CI_AS, '" + Account.UserInfo.eid.ToString() + "' COLLATE Latin1_General_CI_AS) where OfficeCode = " + office_id + " and ProgCode = " + program_id + "  and YearOf = " + year_ + " and ActionCode = 1", con);
                    con.Open();
                    com.ExecuteNonQuery();

                }

                List<Monthly_reserve_Model> ok2 = new List<Monthly_reserve_Model>();
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"SELECT distinct AccountID  FROM IFMIS.dbo.tbl_R_BMSProgramAccounts where AccountYear = '" + year_ + "' and ProgramID = '" + program_id + "' and ObjectOfExpendetureID = '" + ooe_id + "' and ActionCode=1", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();

                   

                    while (reader.Read())
                    {
                        Monthly_reserve_Model reserves = new Monthly_reserve_Model();
                        reserves.reserve_accounts = reader.GetInt32(0);
                        ok2.Add(reserves);


                        reserve_accounts = reserves.reserve_accounts;
                        Reserver_Accounts(office_id,  program_id,  ooe_id,   percent,  money, year_, reserve_flag);

                    }
                    con.Close();


                    return "1";
                }


            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        private void Reserver_Accounts(int? office_id, int? program_id, int? ooe_id, decimal percent, double money, int? year_, int? reserve_flag)
        {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("insert into fmis.dbo.tblBMS_Reserve (OfficeCode ,ProgCode ,AccountCode ,ReservePercent ,ActionCode ,UserID ,DateTimeEntered ,YearOf ,Amount ,AmountFlag) " +
                                "values ('" + office_id + "','" + program_id + "','" + reserve_accounts + "','" + percent + "',1,'" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),'" + year_ + "','" + money + "','" + reserve_flag + "')", con);
                    con.Open();
                    com.ExecuteNonQuery();

                }
                

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("  insert into IFMIS.dbo.tbl_R_BMS_Reserve (FMISOfficeCode,FMISProgramCode,FMISAccountCode,ReservePercent,ActionCode,UserID,DateTimeEntered,YearOf,Amount,reserve_flag) " +
                                "values ('" + office_id + "','" + program_id + "','" + reserve_accounts + "','" + percent + "',1,'" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),'" + year_ + "','" + money + "','" + reserve_flag + "')", con);
                    con.Open();
                    com.ExecuteNonQuery();
                  
                }

        }

        //public string Reserver_Accounts()
        //{
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(Common.MyConn()))
        //        {
        //            SqlCommand com = new SqlCommand("  insert into IFMIS.dbo.tbl_R_BMS_Reserve (FMISOfficeCode,FMISProgramCode,FMISAccountCode,ReservePercent,ActionCode,UserID,DateTimeEntered,YearOf,Amount,reserve_flag) " +
        //                        "values ('" + office_id + "','" + program_id + "','" + account_id + "','" + percent + "',1,'" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),'" + year_ + "','" + money + "','" + reserve_flag + "')", con);
        //            con.Open();
        //            com.ExecuteNonQuery();
        //            return "1";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message.ToString();
        //    }
        //}
        public Monthly_reserve_Model edit_reserve(int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_)
        {
            Monthly_reserve_Model Float = new Monthly_reserve_Model();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT reserve_id , FMISOfficeCode , FMISProgramCode , FMISAccountCode , ReservePercent , YearOf , Amount , reserve_flag
                                FROM IFMIS.dbo.tbl_R_BMS_Reserve where FMISOfficeCode = " + office_id + " and FMISProgramCode = " + program_id + " and FMISAccountCode = " + account_id + " and YearOf = " + year_ + " and ActionCode = 1", con);

                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    
                    Float.reserve_id = reader.GetInt64(0);
                    Float.FMISOfficeCode = reader.GetInt32(1);
                    Float.FMISProgramCode = reader.GetInt32(2);
                    Float.FMISAccountCode = reader.GetInt32(3);
                    Float.ReservePercent = reader.GetValue(4) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(4));
                    Float.YearOf = reader.GetInt32(5);
                    Float.Amount = reader.GetValue(6) is DBNull? 0 : Convert.ToDouble(reader.GetValue(6));
                    Float.reserve_flag = reader.GetInt32(7);

                }
            }
            return Float;
        }
        public string Available(int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_available_amount] '" + office_id + "','" + program_id + "',0,'" + account_id + "','" + year_ + "'", con);
              //  SqlCommand com = new SqlCommand(@"sp_MonthlyRelease_available_amount 4,4,1,401,2016", con);
                con.Open();
                return com.ExecuteScalar().ToString();


            }
        }
        
        public string Available_balat(int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_available_amount_balat] '" + office_id + "','" + program_id + "',0,'" + account_id + "','" + year_ + "'", con);
                //  SqlCommand com = new SqlCommand(@"sp_MonthlyRelease_available_amount 4,4,1,401,2016", con);
                con.Open();
                return com.ExecuteScalar().ToString();


            }
        }
        public string Edit_Release_(int? release_id)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@" select format(sum(AmountPS+AmountMOOE+AmountCO),'N2') FROM [IFMIS].[dbo].[tbl_R_BMS_Release] where release_id = " + release_id + "", con);
                //  SqlCommand com = new SqlCommand(@"sp_MonthlyRelease_available_amount 4,4,1,401,2016", con);
                con.Open();
                return com.ExecuteScalar().ToString();


            }
        }

        public string supp_total(int? year_)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_Total_Reverse] '" + year_ + "'", con);
                //  SqlCommand com = new SqlCommand(@"sp_MonthlyRelease_available_amount 4,4,1,401,2016", con);
                con.Open();
                return com.ExecuteScalar().ToString();


            }
        }
        public string ToBeRealign( int? year_)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {


                SqlCommand com = new SqlCommand(@"IFMIS.dbo.sp_MonthlyRelease_RealignmentSummary '" + year_ + "',1", con);
                con.Open();
                return com.ExecuteScalar().ToString();
            }
        }
Int64 rel_id = 0;
double totalAmount = 0;
double setamount = 0;
double setamount2 = 0;
Int64 rel_id_old = 0;
double totalAmount_old = 0;
double setamount_old = 0;
double setamount2_old = 0;
        public string save_realign_to(int? office_id_realign, int? program_id_realign, int? account_id_realign, double amount_to_realign, double to_be_realign, int? year_, int? office_id_to_realign, int? program_id_to_realign, int? account_id_to_realign, double to_Amount)
        {
            try
            {
                if (to_Amount == 0)
                {

                    // old database fmis

                    List<Monthly_Realignment_Model> ok_fmis = new List<Monthly_Realignment_Model>();
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"SELECT  trnno,Amount FROM fmis.dbo.tblBMS_TotalRealign where YearOf = '" + year_ + "' and ActionCode = 1", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();


                        while (reader.Read())
                        {
                            Monthly_Realignment_Model Float = new Monthly_Realignment_Model();
                            Float.trnno = reader.GetValue(0) is DBNull ? 0 : reader.GetInt32(0);
                            Float.amount_totalss = reader.GetValue(1) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(1));

                            ok_fmis.Add(Float);

                            rel_id_old = Float.trnno;
                            totalAmount_old = Float.amount_totalss;
                        }
                        con.Close();

                        SqlCommand com2 = new SqlCommand("update fmis.dbo.tblBMS_TotalRealign set ActionCode = 2 ,DateTimeEntered = format(GetDate(),'M/d/yyy hh:mm:ss tt') " +
                                                         " , userID =  '" + Account.UserInfo.eid.ToString() + "' where YearOf = '" + year_ + "'  and ActionCode = 1 and trnno = '" + rel_id_old + "'", con);
                        con.Open();
                        com2.ExecuteNonQuery();
                        con.Close();
                        setamount_old = totalAmount_old + amount_to_realign;
                        SqlCommand com3 = new SqlCommand("insert into fmis.dbo.tblBMS_TotalRealign (UserID,Amount,ActionCode,YearOf,DateTimeEntered) values ('" + Account.UserInfo.eid.ToString() + "','" + setamount_old + "',1,'" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'))", con);
                        con.Open();
                        com3.ExecuteNonQuery();
                        con.Close();
                    }


                    // old database fmis



                    List<Monthly_Realignment_Model> ok = new List<Monthly_Realignment_Model>();
                     using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"SELECT  totalrealign_id,Amount FROM [IFMIS].[dbo].[tbl_R_BMS_TotalRealign] where YearOf = '" + year_ + "' and ActionCode = 1", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();
                        
                           
                        while (reader.Read())
                        {
                             Monthly_Realignment_Model Float = new Monthly_Realignment_Model();
                            Float.total_rel_id = reader.GetValue(0) is DBNull ? 0 : reader.GetInt64(0);
                            Float.amount_totalss = reader.GetValue(1) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(1));

                            ok.Add(Float);

                            rel_id = Float.total_rel_id;
                            totalAmount = Float.amount_totalss;
                        }
                            con.Close();

                            SqlCommand com2 = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_TotalRealign set ActionCode = 4 , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where YearOf = '" + year_ + "'  and ActionCode = 1 and totalrealign_id = '" + rel_id + "'", con);
                            con.Open();
                            com2.ExecuteNonQuery();
                            con.Close();
                            setamount = totalAmount + amount_to_realign;
                            SqlCommand com3 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_TotalRealign (UserID,Amount,ActionCode,YearOf,DateTimeEntered,TOfficeCode ,ToProgramCode ,ToOOECode ,ToAccountCode ,ToAmount) " +
                                                         "values ('" + Account.UserInfo.eid.ToString() + "','" + setamount + "',1,'" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),0,0,0,0,0)", con);
                            con.Open();
                            com3.ExecuteNonQuery();
                            con.Close();
                    }


                  


                     //using (SqlConnection con = new SqlConnection(Common.MyConn()))
                     //{
                     //    SqlCommand com = new SqlCommand("insert into  fmis.dbo.tblBMS_Reallignment (FromOfficeCode ,FromProgCode ,FromAccountCode ,Amount ,UserID ,DateTimeEntered ,ActionCode ,YearOf) " +
                     //    "values ('" + office_id_realign + "','" + program_id_realign + "','" + account_id_realign + "','" + amount_to_realign + "','" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),1,'" + year_ + "')", con);
                     //    con.Open();
                     //    com.ExecuteNonQuery();
                     //    con.Close();
                     //}


                     // old database fmis ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^


                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com1 = new SqlCommand("insert into  fmis.dbo.tblBMS_Reallignment (FromOfficeCode ,FromProgCode ,FromAccountCode ,Amount ,UserID ,DateTimeEntered ,ActionCode ,YearOf) " +
                        "values ('" + office_id_realign + "','" + program_id_realign + "','" + account_id_realign + "','" + amount_to_realign + "','" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),1,'" + year_ + "')", con);
                        con.Open();
                        com1.ExecuteNonQuery();
                        con.Close();
                        SqlCommand com = new SqlCommand("insert into  IFMIS.dbo.tbl_R_BMS_Realignment (FromOfficeCode ,FromProgramCode ,FromAccountCode ,ToOfficeCode ,ToProgramCode ,ToAccountCode ,Amount ,UserID ,DateTimeEntered ,ActionCode ,YearOf) " +
                        "values ('" + office_id_realign + "','" + program_id_realign + "','" + account_id_realign + "',0,0,0,'" + amount_to_realign + "','" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),1,'" + year_ + "')", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        return "1";
                    }


                    }else if(amount_to_realign == 0){


                        //old fmis -----------------

                        List<Monthly_Realignment_Model> ok_fmis2 = new List<Monthly_Realignment_Model>();
                        using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        {
                            SqlCommand com = new SqlCommand(@"SELECT  trnno,Amount FROM fmis.dbo.tblBMS_TotalRealign where YearOf = '" + year_ + "' and ActionCode = 1", con);
                            con.Open();
                            SqlDataReader reader = com.ExecuteReader();


                            while (reader.Read())
                            {
                                Monthly_Realignment_Model Float = new Monthly_Realignment_Model();
                                Float.trnno = reader.GetValue(0) is DBNull ? 0 : reader.GetInt32(0);
                                Float.amount_totalss = reader.GetValue(1) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(1));

                                ok_fmis2.Add(Float);

                                rel_id_old = Float.trnno;
                                totalAmount_old = Float.amount_totalss;
                            }
                            con.Close();

                            //SqlCommand com2 = new SqlCommand("update fmis.dbo.tblBMS_TotalRealign set ActionCode = 2 ,DateTimeEntered = CONCAT(DateTimeEntered COLLATE Latin1_General_CI_AS,' , ' COLLATE Latin1_General_CI_AS, format(GetDate(),'M/d/yyy hh:mm:ss tt') COLLATE Latin1_General_CI_AS) " +
                            //                                  " , userID = CONCAT(userID COLLATE Latin1_General_CI_AS,' , ' COLLATE Latin1_General_CI_AS, '" + Account.UserInfo.eid.ToString() + "' COLLATE Latin1_General_CI_AS) where YearOf = '" + year_ + "'  and ActionCode = 1 and trnno = '" + rel_id_old + "'", con);
                            SqlCommand com2 = new SqlCommand("update fmis.dbo.tblBMS_TotalRealign set ActionCode = 2 ,DateTimeEntered = format(GetDate(),'M/d/yyy hh:mm:ss tt')" +
                                                             " , userID = '" + Account.UserInfo.eid.ToString() + "' where YearOf = '" + year_ + "'  and ActionCode = 1 and trnno = '" + rel_id_old + "'", con);
                            con.Open();
                            com2.ExecuteNonQuery();
                            con.Close();
                            setamount2_old = totalAmount_old - to_Amount;
                            SqlCommand com3 = new SqlCommand("insert into fmis.dbo.tblBMS_TotalRealign (UserID,Amount,ActionCode,YearOf,DateTimeEntered) values ('" + Account.UserInfo.eid.ToString() + "','" + setamount2_old + "',1,'" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'))", con);
                            con.Open();
                            com3.ExecuteNonQuery();

                        }

                     





                        //using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        //{
                        //    SqlCommand com = new SqlCommand(" insert into  fmis.dbo.tblBMS_Reallignment (ToOfficeCode ,ToProgCode ,ToAccountCode ,Amount ,UserID ,DateTimeEntered ,ActionCode ,YearOf) " +
                        //    "values ('" + office_id_to_realign + "','" + program_id_to_realign + "','" + account_id_to_realign + "','" + to_Amount + "','" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),1," + year_ + ")", con);
                        //    con.Open();
                        //    com.ExecuteNonQuery();
                        //    con.Close();
                        //}

                        //old fmis -----------------

                    List<Monthly_Realignment_Model> ok = new List<Monthly_Realignment_Model>();
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"SELECT totalrealign_id,Amount FROM [IFMIS].[dbo].[tbl_R_BMS_TotalRealign] where YearOf = '" + year_ + "' and ActionCode = 1", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();


                        while (reader.Read())
                        {
                            Monthly_Realignment_Model Float = new Monthly_Realignment_Model();
                            Float.total_rel_id = reader.GetValue(0) is DBNull ? 0 : reader.GetInt64(0);
                            Float.amount_totalss = reader.GetValue(1) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(1));

                            ok.Add(Float);

                            rel_id = Float.total_rel_id;
                            totalAmount = Float.amount_totalss;
                        }
                        con.Close();

                        SqlCommand com2 = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_TotalRealign set ActionCode = 4 , DateTimeEntered = CONCAT(DateTimeEntered,' , ', format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where YearOf = '" + year_ + "'  and ActionCode = 1 and totalrealign_id = '" + rel_id + "'", con);
                        con.Open();
                        com2.ExecuteNonQuery();
                        con.Close();
                        setamount2 = totalAmount - to_Amount;
                        SqlCommand com3 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_TotalRealign (UserID,Amount,ActionCode,YearOf,DateTimeEntered,TOfficeCode ,ToProgramCode ,ToOOECode ,ToAccountCode ,ToAmount) " +
                                                     "values ('" + Account.UserInfo.eid.ToString() + "','" + setamount2 + "',1,'" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),'" + office_id_to_realign + "','" + program_id_to_realign + "',0,'" + account_id_to_realign + "','"+ to_Amount +"')", con);
                        con.Open();
                        com3.ExecuteNonQuery();

                    }
                      


                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com1 = new SqlCommand(" insert into  fmis.dbo.tblBMS_Reallignment (ToOfficeCode ,ToProgCode ,ToAccountCode ,Amount ,UserID ,DateTimeEntered ,ActionCode ,YearOf) " +
                        "values ('" + office_id_to_realign + "','" + program_id_to_realign + "','" + account_id_to_realign + "','" + to_Amount + "','" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),1," + year_ + ")", con);
                        con.Open();
                        com1.ExecuteNonQuery();
                        con.Close();
                        SqlCommand com = new SqlCommand("insert into  IFMIS.dbo.tbl_R_BMS_Realignment (FromOfficeCode ,FromProgramCode ,FromAccountCode ,ToOfficeCode ,ToProgramCode ,ToAccountCode ,Amount ,UserID ,DateTimeEntered ,ActionCode ,YearOf) "+
                        "values ('" + office_id_realign + "','" + program_id_realign + "','" + account_id_realign + "','" + office_id_to_realign + "','" + program_id_to_realign + "','" + account_id_to_realign + "','" + to_Amount + "','" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),1," + year_ + ")", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        return "1";
                    }

                }
                else if (amount_to_realign != 0 && to_Amount != 0)
                {

                    List<Monthly_Realignment_Model> ok = new List<Monthly_Realignment_Model>();
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"SELECT  totalrealign_id,Amount FROM [IFMIS].[dbo].[tbl_R_BMS_TotalRealign] where YearOf = '" + year_ + "' and ActionCode = 1", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();


                        while (reader.Read())
                        {
                            Monthly_Realignment_Model Float = new Monthly_Realignment_Model();
                            Float.total_rel_id = reader.GetValue(0) is DBNull ? 0 : reader.GetInt64(0);
                            Float.amount_totalss = reader.GetValue(1) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(1));

                            ok.Add(Float);

                            rel_id = Float.total_rel_id;
                            totalAmount = Float.amount_totalss;
                        }
                        con.Close();

                        SqlCommand com2 = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_TotalRealign set ActionCode = 4 , DateTimeEntered = CONCAT(DateTimeEntered,' , ', format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where totalrealign_id = '" + rel_id + "'", con);
                        con.Open();
                        com2.ExecuteNonQuery();
                        con.Close();
                        setamount = totalAmount + amount_to_realign;
                        SqlCommand com3 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_TotalRealign (UserID,Amount,ActionCode,YearOf,DateTimeEntered,TOfficeCode ,ToProgramCode ,ToOOECode ,ToAccountCode ,ToAmount) " +
                                                     "values ('" + Account.UserInfo.eid.ToString() + "','" + setamount + "',1,'" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),0,0,0,0,0)", con);
                        con.Open();
                        com3.ExecuteNonQuery();
                        con.Close();
                    }
                    List<Monthly_Realignment_Model> ok2 = new List<Monthly_Realignment_Model>();
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"SELECT  totalrealign_id,Amount FROM [IFMIS].[dbo].[tbl_R_BMS_TotalRealign] where YearOf = '" + year_ + "' and ActionCode = 1", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();


                        while (reader.Read())
                        {
                            Monthly_Realignment_Model Float = new Monthly_Realignment_Model();
                            Float.total_rel_id = reader.GetValue(0) is DBNull ? 0 : reader.GetInt64(0);
                            Float.amount_totalss = reader.GetValue(1) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(1));

                            ok2.Add(Float);

                            rel_id = Float.total_rel_id;
                            totalAmount = Float.amount_totalss;
                        }
                        con.Close();

                        SqlCommand com2 = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_TotalRealign set ActionCode = 4 , DateTimeEntered = CONCAT(DateTimeEntered,' , ', format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where YearOf = '" + year_ + "'  and ActionCode = 1 and totalrealign_id = '" + rel_id + "'", con);
                        con.Open();
                        com2.ExecuteNonQuery();
                        con.Close();
                        setamount2 = totalAmount - to_Amount;
                        SqlCommand com3 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_TotalRealign (UserID,Amount,ActionCode,YearOf,DateTimeEntered,TOfficeCode ,ToProgramCode ,ToOOECode ,ToAccountCode ,ToAmount) " +
                                                     "values ('" + Account.UserInfo.eid.ToString() + "','" + setamount2 + "',1,'" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),0,0,0,0,0)", con);
                        con.Open();
                        com3.ExecuteNonQuery();
                        totalAmount = 0;
                        setamount = 0;
                        setamount2 = 0;
                    }

                    List<Monthly_Realignment_Model> ok_fmis = new List<Monthly_Realignment_Model>();
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"SELECT  trnno,Amount FROM fmis.dbo.tblBMS_TotalRealign where YearOf = '" + year_ + "' and ActionCode = 1", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();


                        while (reader.Read())
                        {
                            Monthly_Realignment_Model Float = new Monthly_Realignment_Model();
                            Float.trnno = reader.GetValue(0) is DBNull ? 0 : reader.GetInt32(0);
                            Float.amount_totalss = reader.GetValue(1) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(1));

                            ok_fmis.Add(Float);

                            rel_id_old = Float.trnno;
                            totalAmount_old = Float.amount_totalss;
                        }
                        con.Close();

                        SqlCommand com2 = new SqlCommand("update fmis.dbo.tblBMS_TotalRealign set ActionCode = 2 ,DateTimeEntered =  format(GetDate(),'M/d/yyy hh:mm:ss tt')  " +
                                                         " , userID = '" + Account.UserInfo.eid.ToString() + "'  where YearOf = '" + year_ + "'  and ActionCode = 1 and trnno = '" + rel_id_old + "'", con);
                        con.Open();
                        com2.ExecuteNonQuery();
                        con.Close();
                        setamount_old = totalAmount_old + amount_to_realign;
                        SqlCommand com3 = new SqlCommand("insert into fmis.dbo.tblBMS_TotalRealign (UserID,Amount,ActionCode,YearOf,DateTimeEntered) values ('" + Account.UserInfo.eid.ToString() + "','" + setamount_old + "',1,'" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'))", con);
                        con.Open();
                        com3.ExecuteNonQuery();
                        con.Close();
                    }


                    List<Monthly_Realignment_Model> ok_fmis2 = new List<Monthly_Realignment_Model>();
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"SELECT  trnno,Amount FROM fmis.dbo.tblBMS_TotalRealign where YearOf = '" + year_ + "' and ActionCode = 1", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();


                        while (reader.Read())
                        {
                            Monthly_Realignment_Model Float = new Monthly_Realignment_Model();
                            Float.trnno = reader.GetValue(0) is DBNull ? 0 : reader.GetInt32(0);
                            Float.amount_totalss = reader.GetValue(1) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(1));

                            ok_fmis2.Add(Float);

                            rel_id_old = Float.trnno;
                            totalAmount_old = Float.amount_totalss;
                        }
                        con.Close();

                        SqlCommand com2 = new SqlCommand("update fmis.dbo.tblBMS_TotalRealign set ActionCode = 2 ,DateTimeEntered =  format(GetDate(),'M/d/yyy hh:mm:ss tt') " +
                                                          " , userID = '" + Account.UserInfo.eid.ToString() + "'  where YearOf = '" + year_ + "'  and ActionCode = 1 and trnno = '" + rel_id_old + "'", con);
                        con.Open();
                        com2.ExecuteNonQuery();
                        con.Close();
                        setamount2_old = totalAmount_old - to_Amount;
                        SqlCommand com3 = new SqlCommand("insert into fmis.dbo.tblBMS_TotalRealign (UserID,Amount,ActionCode,YearOf,DateTimeEntered) values ('" + Account.UserInfo.eid.ToString() + "','" + setamount2_old + "',1,'" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'))", con);
                        con.Open();
                        com3.ExecuteNonQuery();

                    }




                    //using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    //{
                    //    SqlCommand com = new SqlCommand(" insert into  fmis.dbo.tblBMS_Reallignment (FromOfficeCode ,FromProgCode ,FromAccountCode ,ToOfficeCode ,ToProgCode ,ToAccountCode ,Amount ,UserID ,DateTimeEntered ,ActionCode ,YearOf) " +
                    //    "values ('" + office_id_realign + "','" + program_id_realign + "','" + account_id_realign + "','" + office_id_to_realign + "','" + program_id_to_realign + "','" + account_id_to_realign + "','" + to_Amount + "','" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),1," + year_ + ")", con);
                    //    con.Open();
                    //    com.ExecuteNonQuery();

                    //}


                    //using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    //{
                    //    SqlCommand com = new SqlCommand("insert into  fmis.dbo.tblBMS_Reallignment (FromOfficeCode ,FromProgCode ,FromAccountCode ,Amount ,UserID ,DateTimeEntered ,ActionCode ,YearOf) " +
                    //    "values ('" + office_id_realign + "','" + program_id_realign + "','" + account_id_realign + "','" + amount_to_realign + "','" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),1,'" + year_ + "')", con);
                    //    con.Open();
                    //    com.ExecuteNonQuery();

                    //}


                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com3 = new SqlCommand("insert into  fmis.dbo.tblBMS_Reallignment (FromOfficeCode ,FromProgCode ,FromAccountCode ,Amount ,UserID ,DateTimeEntered ,ActionCode ,YearOf) " +
                        "values ('" + office_id_realign + "','" + program_id_realign + "','" + account_id_realign + "','" + amount_to_realign + "','" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),1,'" + year_ + "')", con);
                        con.Open();
                        com3.ExecuteNonQuery();
                        con.Close();
                        SqlCommand com5 = new SqlCommand(" insert into  fmis.dbo.tblBMS_Reallignment (FromOfficeCode ,FromProgCode ,FromAccountCode ,ToOfficeCode ,ToProgCode ,ToAccountCode ,Amount ,UserID ,DateTimeEntered ,ActionCode ,YearOf) " +
                        "values ('" + office_id_realign + "','" + program_id_realign + "','" + account_id_realign + "','" + office_id_to_realign + "','" + program_id_to_realign + "','" + account_id_to_realign + "','" + to_Amount + "','" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),1," + year_ + ")", con);
                        con.Open();
                        com5.ExecuteNonQuery();
                        con.Close();
                        SqlCommand com = new SqlCommand("insert into  IFMIS.dbo.tbl_R_BMS_Realignment (FromOfficeCode ,FromProgramCode ,FromAccountCode ,ToOfficeCode ,ToProgramCode ,ToAccountCode ,Amount ,UserID ,DateTimeEntered ,ActionCode ,YearOf) " +
                        "values ('" + office_id_realign + "','" + program_id_realign + "','" + account_id_realign + "',0,0,0,'" + amount_to_realign + "','" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),1," + year_ + ")", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                        SqlCommand com2 = new SqlCommand("insert into  IFMIS.dbo.tbl_R_BMS_Realignment (FromOfficeCode ,FromProgramCode ,FromAccountCode ,ToOfficeCode ,ToProgramCode ,ToAccountCode ,Amount ,UserID ,DateTimeEntered ,ActionCode ,YearOf) " +
                        "values ('" + office_id_realign + "','" + program_id_realign + "','" + account_id_realign + "','" + office_id_to_realign + "','" + program_id_to_realign + "','" + account_id_to_realign + "','" + to_Amount + "','" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),1," + year_ + ")", con);
                        con.Open();
                        com2.ExecuteNonQuery();
                        return "1";
                    }
                }
                else { 
                         return "2";
                    }


            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public string save_realign_to_null(int? office_id_realign_null, int? program_id_realign_null, int? ooe_id_realign_null, int? account_id_realign_null, int? year_, int? office_id_to_realign_null, int? program_id_to_realign_null, int? ooe_id_to_realign_null, int? account_id_to_realign_null)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("sp_MonthlyRelease_view '65','70','1','299','9','1','2016'", con);
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



        public IEnumerable<LegalBasis_Model> LegalBasis()
        {
            List<LegalBasis_Model> legal_basis = new List<LegalBasis_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT Legal_id ,LegalCode ,LegalDescription,YearOf FROM IFMIS.dbo.tbl_R_BMS_LegalBasis where ActionCode = 1 order by Legal_id desc", con);
                

                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    LegalBasis_Model legal_list = new LegalBasis_Model();
                    legal_list.Legal_id = reader.GetInt64(0);
                    legal_list.LegalCode = reader.GetInt64(1);
                    legal_list.LegalDescription = reader.GetValue(2).ToString();
                    legal_list.YearOf = reader.GetInt32(3);
                   


                    legal_basis.Add(legal_list);
                  
                }
            }
            return legal_basis;
        }

        public void legal_Create(LegalBasis_Model legalbasis)
        {
          using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com2 = new SqlCommand(@"insert into fmis.dbo.tblBMS_LegalBasis (LegalCode,LegalDescription,UserID,ActionCode,DateTimeEntered,YearOf)values ('" + legalbasis.LegalCode + "','" + legalbasis.LegalDescription + "','" + Account.UserInfo.eid.ToString() + "',1,format(GetDate(),'M/d/yyy hh:mm:ss tt')," + legalbasis.YearOf + ")", con);
                        con.Open();
                        com2.ExecuteNonQuery();
                        con.Close();
                        SqlCommand com = new SqlCommand(@"  insert into IFMIS.dbo.tbl_R_BMS_LegalBasis (LegalCode,LegalDescription,UserID,ActionCode,DateTimeEntered,YearOf)values ('" + legalbasis.LegalCode + "','" + legalbasis.LegalDescription + "','" + Account.UserInfo.eid.ToString() + "',1,format(GetDate(),'M/d/yyy hh:mm:ss tt')," + legalbasis.YearOf + ")", con);
                        con.Open();
                        com.ExecuteNonQuery();
                    }
        }

        public void legal_Update(LegalBasis_Model legalbasis)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com47 = new SqlCommand(@"SELECT LegalCode ,CONCAT(LegalDescription,' (',REPLACE(LegalCode, ' ', ''),')') FROM IFMIS.dbo.tbl_R_BMS_LegalBasis where ActionCode = 1 and Legal_id =" + legalbasis.Legal_id + "", con);
                con.Open();
                SqlDataReader reader7 = com47.ExecuteReader();
                while (reader7.Read())
                {
                    LegalBasis_Model legalbasis_list = new LegalBasis_Model();
                    legalbasis_list.LegalCode = reader7.GetInt64(0);
                    legalbasis_list.LegalDescription = reader7.GetString(1);

                    LegalCode_ = legalbasis_list.LegalCode;
                    LegalDescription_ = legalbasis_list.LegalDescription;
                }

            }


            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com3 = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_LegalBasis set ActionCode = 2 where LegalCode = '" + LegalCode_ + "' and LegalDescription = '" + LegalDescription_ + "'", con);
                con.Open();
                com3.ExecuteNonQuery();
                con.Close();
                SqlCommand com24 = new SqlCommand(@"insert into fmis.dbo.tblBMS_LegalBasis (LegalCode,LegalDescription,UserID,ActionCode,DateTimeEntered,YearOf)values ('" + legalbasis.LegalCode + "','" + legalbasis.LegalDescription + "','" + Account.UserInfo.eid.ToString() + "',1,format(GetDate(),'M/d/yyy hh:mm:ss tt')," + legalbasis.YearOf + ")", con);
                con.Open();
                com24.ExecuteNonQuery();
                con.Close();

                SqlCommand com = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_LegalBasis set ActionCode = 2 , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), userID = CONCAT(userID,' , ', '" + Account.UserInfo.eid.ToString() + "')  where Legal_id = '" + legalbasis.Legal_id + "'", con);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
                SqlCommand com2 = new SqlCommand(@"insert into IFMIS.dbo.tbl_R_BMS_LegalBasis (LegalCode,LegalDescription,UserID,ActionCode,DateTimeEntered,YearOf)values ('" + legalbasis.LegalCode + "','" + legalbasis.LegalDescription + "','" + Account.UserInfo.eid.ToString() + "',1,format(GetDate(),'M/d/yyy hh:mm:ss tt')," + legalbasis.YearOf + ")", con);
                con.Open();
                com2.ExecuteNonQuery();
            }
        }

        public void legal_Destroy(LegalBasis_Model legalbasis)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com47 = new SqlCommand(@"SELECT LegalCode ,CONCAT(LegalDescription,' (',REPLACE(LegalCode, ' ', ''),')') FROM IFMIS.dbo.tbl_R_BMS_LegalBasis where ActionCode = 1 and Legal_id =" + legalbasis.Legal_id + "", con);
                con.Open();
                SqlDataReader reader7 = com47.ExecuteReader();
                while (reader7.Read())
                {
                    LegalBasis_Model legalbasis_list = new LegalBasis_Model();
                    legalbasis_list.LegalCode = reader7.GetInt64(0);
                    legalbasis_list.LegalDescription = reader7.GetString(1);

                    LegalCode_ = legalbasis_list.LegalCode;
                    LegalDescription_ = legalbasis_list.LegalDescription;
                }

            }



            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com3 = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_LegalBasis set ActionCode = 2 where LegalCode = '" + LegalCode_ + "' and LegalDescription = '" + LegalDescription_ + "'", con);
                con.Open();
                com3.ExecuteNonQuery();
                con.Close();
                SqlCommand com = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_LegalBasis set ActionCode = 4 , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), userID = CONCAT(userID,' , ', '" + Account.UserInfo.eid.ToString() + "')  where Legal_id = '" + legalbasis.Legal_id + "'", con);
                con.Open();
                com.ExecuteNonQuery();
                
            }
        }


        public IEnumerable<LegalBasis_Model> LegalBasis_CB()
        {
            List<LegalBasis_Model> LegalBasisList = new List<LegalBasis_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT LegalCode ,LegalDescription FROM IFMIS.dbo.tbl_R_BMS_LegalBasis where ActionCode = 1 order by Legal_id desc", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    LegalBasis_Model legalbasis_list = new LegalBasis_Model();
                    legalbasis_list.LegalCode = reader.GetInt64(0);
                    legalbasis_list.LegalDescription = reader.GetString(1);

                    LegalBasisList.Add(legalbasis_list);
                }
            }
            return LegalBasisList;
        }

        public string save_supplemement(int? legal_code, int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_, double supplement_amount, int? MonthOf)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com47 = new SqlCommand(@" select isnull((select top 1 SBCode FROM fmis.dbo.tblBMS_SupplementalBudget where ActionCode = 1 order by SBCode desc),0)", con);
                    con.Open();
                    SqlDataReader reader7 = com47.ExecuteReader();
                    while (reader7.Read())
                    {
                        Monthly_Edit_Realign_Model edit_list = new Monthly_Edit_Realign_Model();
                        edit_list.SBCode = reader7.GetInt64(0);


                        SBCodess_OLD = edit_list.SBCode + 1;
                       
                    }

                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com47 = new SqlCommand(@"   select isnull((select top 1 SBCode FROM IFMIS.dbo.tbl_R_BMS_SupplementalBudget where ActionCode = 1 order by SBCode desc),0)", con);
                    con.Open();
                    SqlDataReader reader7 = com47.ExecuteReader();
                    while (reader7.Read())
                    {
                        Monthly_Edit_Realign_Model edit_list = new Monthly_Edit_Realign_Model();
                        edit_list.SBCode = reader7.GetInt64(0);


                        SBCodess = edit_list.SBCode + 1;
                       // SBCodess = SBCodess + 1;

                    }

                    con.Close();

                    SqlCommand com49 = new SqlCommand(@"SELECT AccountName FROM IFMIS.dbo.tbl_R_BMSAccounts where FMISAccountCode = " + account_id + "", con);
                    con.Open();
                    SqlDataReader reader72 = com49.ExecuteReader();
                    while (reader72.Read())
                    {
                        Monthly_Edit_Realign_Model edit_list = new Monthly_Edit_Realign_Model();

                        edit_list.Description_supp = reader72.GetString(0);

                        Descriptionss = edit_list.Description_supp;


                    }

                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com2 = new SqlCommand("insert into fmis.dbo.tblBMS_SupplementalBudget (SBCode,LegalCode,FMISOfficeCode,FMISProgramCode,FMISAccountCode,OOECode,[Description],Amount,MonthOf,YearOf,ActionCode,UserID,DateTimeEntered) " +
                            "values ('" + SBCodess_OLD + "','" + SBCodess_OLD + "','" + office_id + "','" + program_id + "','" + account_id + "','" + ooe_id + "','" + Descriptionss.Replace("'","''") + "','" + supplement_amount + "','" + MonthOf + "','" + year_ + "',1,'" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt')) ", con);
                    con.Open();
                    com2.ExecuteNonQuery();
                    con.Close();
                    SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_SupplementalBudget (SBCode,LegalCode,FMISOfficeCode,FMISProgramCode,FMISAccountCode,OOECode,[Description],Amount,MonthOf,YearOf,ActionCode,UserID,DateTimeEntered) " +
                            "values ('" + SBCodess + "','" + legal_code + "','" + office_id + "','" + program_id + "','" + account_id + "','" + ooe_id + "','" + Descriptionss.Replace("'", "''") + "','" + supplement_amount + "','" + MonthOf + "','" + year_ + "',1,'" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt')) ", con);
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


        Int64 SBCodess_reverse = 0;
        Int64 SBCodess_transfere = 0;
        public string save_supplemement_reverse(int? legal_code, int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_, double supplement_amount, int? MonthOf)
        {
            try
            {
   
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com47 = new SqlCommand(@" select isnull((select top 1 SBCode FROM IFMIS.dbo.tbl_R_BMS_SupplementalReverse where ActionCode = 1 order by SBCode desc),0)", con);
                    con.Open();
                    SqlDataReader reader7 = com47.ExecuteReader();
                    while (reader7.Read())
                    {
                        Monthly_Edit_Realign_Model edit_list = new Monthly_Edit_Realign_Model();
                        edit_list.SBCode = reader7.GetInt64(0);


                        SBCodess_reverse = edit_list.SBCode + 1;
                        // SBCodess = SBCodess + 1;

                    }

                   
                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_SupplementalReverse (SBCode,LegalCode,FMISOfficeCode,FMISProgramCode,FMISAccountCode,MonthOf,YearOf,ActionCode,UserID,DateTimeEntered,Amount,type_) " +
                            "values ('" + SBCodess_reverse + "','" + legal_code + "','" + office_id + "','" + program_id + "','" + account_id + "','" + MonthOf + "','" + year_ + "',1,'" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),'" + supplement_amount + "',1) ", con);
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
     


        public string save_supplemement_transfere(int? legal_code, int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_, double supplement_amount, int? MonthOf)
        {
            try
            {
                
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com47 = new SqlCommand(@"select isnull((select top 1 SBCode FROM IFMIS.dbo.tbl_R_BMS_SupplementalTransfere where ActionCode = 1 order by SBCode desc),0)", con);
                    con.Open();
                    SqlDataReader reader7 = com47.ExecuteReader();
                    while (reader7.Read())
                    {
                        Monthly_Edit_Realign_Model edit_list = new Monthly_Edit_Realign_Model();
                        edit_list.SBCode = reader7.GetInt64(0);


                        SBCodess_transfere = edit_list.SBCode + 1;
                        // SBCodess = SBCodess + 1;

                    }

                
                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_SupplementalTransfere (SBCode,LegalCode,FMISOfficeCode,FMISProgramCode,FMISAccountCode,MonthOf,YearOf,ActionCode,UserID,DateTimeEntered,Amount) " +
                            "values ('" + SBCodess_transfere + "','" + legal_code + "','" + office_id + "','" + program_id + "','" + account_id + "','" + MonthOf + "','" + year_ + "',1,'" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),'" + supplement_amount + "') ", con);
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



        Int64 SBCodess_reverse_Edit = 0;
        Int64 SBCodess_transfere_Edit = 0;
        public string save_supplemement_reverse_Edit(int? legal_code, int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_, double supplement_amount, int? MonthOf, int? supplement_id, string transdate, int? copydatetag)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com47 = new SqlCommand(@" select isnull((select top 1 SBCode FROM IFMIS.dbo.tbl_R_BMS_SupplementalReverse where ActionCode = 1 and supplementalreverse_id = '" + supplement_id + "' order by SBCode desc),0)", con);
                    con.Open();
                    SqlDataReader reader7 = com47.ExecuteReader();
                    while (reader7.Read())
                    {
                        Monthly_Edit_Realign_Model edit_list = new Monthly_Edit_Realign_Model();
                        edit_list.SBCode = reader7.GetInt64(0);


                        SBCodess_reverse_Edit = edit_list.SBCode + 1;
                        // SBCodess = SBCodess + 1;

                    }


                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_SupplementalReverse set ActionCode = 2,DateTimeEntered=DateTimeEntered + ','+ cast(format(GetDate(),'M/d/yyy hh:mm:ss tt') as varchar(50)),UserID=UserID +','+ '" + Account.UserInfo.eid.ToString() + "'  where supplementalreverse_id = '" + supplement_id + "'", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    

                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    if (copydatetag == 0)
                    {
                        SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_SupplementalReverse (SBCode,LegalCode,FMISOfficeCode,FMISProgramCode,FMISAccountCode,MonthOf,YearOf,ActionCode,UserID,DateTimeEntered,Amount,type_) " +
                                "values ('" + SBCodess_reverse_Edit + "','" + legal_code + "','" + office_id + "','" + program_id + "','" + account_id + "','" + MonthOf + "','" + year_ + "',1,'" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),'" + supplement_amount + "',1) ", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        return "1";
                    }
                    else
                    {
                        SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_SupplementalReverse (SBCode,LegalCode,FMISOfficeCode,FMISProgramCode,FMISAccountCode,MonthOf,YearOf,ActionCode,UserID,DateTimeEntered,Amount,type_) " +
                                "values ('" + SBCodess_reverse_Edit + "','" + legal_code + "','" + office_id + "','" + program_id + "','" + account_id + "','" + MonthOf + "','" + year_ + "',1,'" + Account.UserInfo.eid.ToString() + "','"+ transdate +"','" + supplement_amount + "',1) ", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        return "1";
                    }

                }


            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }


        public string save_supplemement_transfere_Edit(int? legal_code, int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_, double supplement_amount, int? MonthOf, int? supplement_id, string transdate, int? copydatetag)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com47 = new SqlCommand(@"select isnull((select top 1 SBCode FROM IFMIS.dbo.tbl_R_BMS_SupplementalTransfere where ActionCode = 1 and supplementaltransfere_id = '" + supplement_id + "' order by SBCode desc),0)", con);
                    con.Open();
                    SqlDataReader reader7 = com47.ExecuteReader();
                    while (reader7.Read())
                    {
                        Monthly_Edit_Realign_Model edit_list = new Monthly_Edit_Realign_Model();
                        edit_list.SBCode = reader7.GetInt64(0);


                        SBCodess_transfere_Edit = edit_list.SBCode + 1;
                        // SBCodess = SBCodess + 1;

                    }


                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com = new SqlCommand(" update IFMIS.dbo.tbl_R_BMS_SupplementalTransfere set ActionCode = 2,DateTimeEntered=DateTimeEntered + ','+ cast(format(GetDate(),'M/d/yyy hh:mm:ss tt') as varchar(50)),[UserID]=[UserID]+','+ '" + Account.UserInfo.eid.ToString() + "' where supplementaltransfere_id = '" + supplement_id + "'", con);
                    con.Open();
                    com.ExecuteNonQuery();
                  

                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    if (copydatetag == 0)
                    {
                        SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_SupplementalTransfere (SBCode,LegalCode,FMISOfficeCode,FMISProgramCode,FMISAccountCode,MonthOf,YearOf,ActionCode,UserID,DateTimeEntered,Amount) " +
                            "values ('" + SBCodess_transfere_Edit + "','" + legal_code + "','" + office_id + "','" + program_id + "','" + account_id + "','" + MonthOf + "','" + year_ + "',1,'" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),'" + supplement_amount + "') ", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        return "1";
                    }
                    else
                    {
                        SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_SupplementalTransfere (SBCode,LegalCode,FMISOfficeCode,FMISProgramCode,FMISAccountCode,MonthOf,YearOf,ActionCode,UserID,DateTimeEntered,Amount) " +
                            "values ('" + SBCodess_transfere_Edit + "','" + legal_code + "','" + office_id + "','" + program_id + "','" + account_id + "','" + MonthOf + "','" + year_ + "',1,'" + Account.UserInfo.eid.ToString() + "','"+ transdate + "','" + supplement_amount + "') ", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        return "1";
                    }

                }


            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }





        public string save_supplemement_empty(int? legal_code, int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_, double supplement_amount, int? MonthOf)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com47 = new SqlCommand(@" SELECT top 1 SBCode FROM IFMIS.dbo.tbl_R_BMS_SupplementalBudget order by SBCode desc", con);
                    con.Open();
                    SqlDataReader reader7 = com47.ExecuteReader();
                    while (reader7.Read())
                    {
                        Monthly_Edit_Realign_Model edit_list = new Monthly_Edit_Realign_Model();
                        edit_list.SBCode = reader7.GetInt64(0) + 1;


                        SBCodess_empty = edit_list.SBCode;
                      
                        
                    }

                    con.Close();

                    SqlCommand com49 = new SqlCommand(@"SELECT AccountName FROM IFMIS.dbo.tbl_R_BMSAccounts where FMISAccountCode = " + account_id + "", con);
                    con.Open();
                    SqlDataReader reader72 = com49.ExecuteReader();
                    while (reader72.Read())
                    {
                        Monthly_Edit_Realign_Model edit_list = new Monthly_Edit_Realign_Model();

                        edit_list.Description_supp = reader72.GetString(0);

                        Descriptionss_empty = edit_list.Description_supp;
                        

                    }

                    con.Close();

                    SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_SupplementalBudget (SBCode,LegalCode,FMISOfficeCode,FMISProgramCode,FMISAccountCode,OOECode,[Description],Amount,MonthOf,YearOf,ActionCode,UserID,DateTimeEntered) "+
                            "values ('" + SBCodess_empty + "','" + legal_code + "','" + office_id + "','" + program_id + "','" + account_id + "','" + ooe_id + "','" + Descriptionss_empty + "','" + supplement_amount + "','" + MonthOf + "','" + year_ + "',1,'" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt')) ", con);
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

        public IEnumerable<Release_Float_Model> Float_Grid_List(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_)
        {

            List<Release_Float_Model> Float_List = new List<Release_Float_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec sp_BMS_ReleaseFloatList " + office_id + "," + program_id + "," + account_id + "," + ooe_id + "," + year_ + "," + Account.UserInfo.eid.ToString() + "", con);
                                     

                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Release_Float_Model Float = new Release_Float_Model();
                    Float.release_float_id = reader.GetInt64(0);
                    Float.amount_description = reader.GetValue(1).ToString();
                    Float.Amount_release = Convert.ToDecimal(reader.GetValue(2));
                    Float.MonthOf = reader.GetValue(3).ToString();
                    Float.YearOf = reader.GetInt32(4);
                    Float.balance_amount = Convert.ToDecimal(reader.GetValue(5));
                    Float.batch = reader.GetInt32(6);
                    Float.Float_Flag = reader.GetInt32(7);
                    Float.gov_com = reader.GetInt32(8);
                    Float.comtag =  reader.GetInt32(9);
                    Float.code = reader.GetValue(10).ToString();
                    Float.officeid = reader.GetInt32(11);
                    Float.programid = reader.GetInt32(12);
                    Float.accountid = reader.GetInt64(13);
                    Float.particular = reader.GetValue(14).ToString();
                    Float.muncode = reader.GetInt64(15);
                    Float.brgycode = reader.GetInt64(16);

                    Float_List.Add(Float);
                   
                }
            }
            return Float_List;
        }

        public IEnumerable<Release_Float_Model> Float_Grid_List2(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_)
        {
            List<Release_Float_Model> Float_List = new List<Release_Float_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec sp_BMS_ReleaseFloatList " + office_id + "," + program_id + "," + account_id + "," + ooe_id + "," + year_ + "," + Account.UserInfo.eid.ToString() + "", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Release_Float_Model Float = new Release_Float_Model();
                    Float.release_float_id = reader.GetInt64(0);
                    Float.amount_description = reader.GetValue(1).ToString();
                    Float.Amount_release = Convert.ToDecimal(reader.GetValue(2));
                    Float.MonthOf = reader.GetValue(3).ToString();
                    Float.YearOf = reader.GetInt32(4);
                    Float.balance_amount = Convert.ToDecimal(reader.GetValue(5));
                    Float.batch = reader.GetInt32(6);
                    Float.Float_Flag = reader.GetInt32(7);
                    Float.gov_com = reader.GetInt32(8);
                    Float.comtag = reader.GetInt32(9);
                    Float.code = reader.GetValue(10).ToString();
                    Float.officeid = reader.GetInt32(11);
                    Float.programid = reader.GetInt32(12);
                    Float.accountid = reader.GetInt64(13);
                    Float.particular = reader.GetValue(14).ToString();
                    Float.muncode = reader.GetInt64(15);
                    Float.brgycode = reader.GetInt64(16);

                    Float_List.Add(Float);

                }
            }
            return Float_List;
        }

        public IEnumerable<Release_Float_Model> Float_Grid_List3(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_)
        {
            List<Release_Float_Model> Float_List = new List<Release_Float_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"exec sp_BMS_ReleaseFloatList "+ office_id + ","+ program_id + ","+ account_id + ","+ ooe_id + ","+ year_ + ","+ Account.UserInfo.eid.ToString() +"", con);
                
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Release_Float_Model Float = new Release_Float_Model();
                    Float.release_float_id = reader.GetInt64(0);
                    Float.amount_description = reader.GetValue(1).ToString();
                    Float.Amount_release = Convert.ToDecimal(reader.GetValue(2));
                    Float.MonthOf = reader.GetValue(3).ToString();
                    Float.YearOf = reader.GetInt32(4);
                    Float.balance_amount = Convert.ToDecimal(reader.GetValue(5));
                    Float.batch = reader.GetInt32(6);
                    Float.Float_Flag = reader.GetInt32(7);
                    Float.gov_com = reader.GetInt32(8);
                    Float.comtag = reader.GetInt32(9);
                    Float.code  = reader.GetValue(10).ToString();
                    Float.officeid = reader.GetInt32(11);
                    Float.programid = reader.GetInt32(12);
                    Float.accountid = reader.GetInt64(13);
                    Float.particular = reader.GetValue(14).ToString();
                    Float.muncode = reader.GetInt64(15);
                    Float.brgycode = reader.GetInt64(16);

                    Float_List.Add(Float);

                }
            }
            return Float_List;
        }


        public Release_Float_Edit Edit_Float_ps(int? release_float_id, int? ooe_id)
        {
            
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"  SELECT a.release_float_id ,a.FMISOfficeCode ,a.FMISProgramCode ,a.AmountPS ,a.MonthOf ,a.YearOf, a.FMISAccountCode ,a.Batch , b.ObjectOfExpendetureID "+
                                                 "FROM IFMIS.dbo.tbl_R_BMS_Release_Float as a left join IFMIS.dbo.tbl_R_BMSProgramAccounts as b on a.FMISAccountCode = b.AccountID "+
                                                 "WHERE a.release_float_id = '" + release_float_id + "' GROUP BY release_float_id,MonthOf,YearOf,Batch,FMISOfficeCode ,FMISProgramCode , FMISAccountCode,b.ObjectOfExpendetureID,a.AmountPS ", con);
                        

                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Release_Float_Edit Float = new Release_Float_Edit();
                    Float.release_float_id = reader.GetInt64(0);
                    Float.FMISOfficeCode = reader.GetInt32(1);
                    Float.FMISProgramCode = reader.GetInt32(2);
                    Float.Amount_release = Convert.ToDecimal(reader.GetValue(3));
                    Float.MonthOf = reader.GetInt32(4);
                    Float.YearOf = reader.GetInt32(5);
                    Float.FMISAccountCode = reader.GetInt32(6);
                    Float.batch = reader.GetInt32(7);
                    Float.ooe_ids = reader.GetInt32(8);
                   
                    return Float;
                }
            }
            return new Release_Float_Edit();
        }


        public Release_Float_Edit Edit_Float_mooe(int? release_float_id, int? ooe_id)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"  SELECT a.release_float_id ,a.FMISOfficeCode ,a.FMISProgramCode ,a.AmountMOOE ,a.MonthOf ,a.YearOf, a.FMISAccountCode ,a.Batch , b.ObjectOfExpendetureID " +
                                                 "FROM IFMIS.dbo.tbl_R_BMS_Release_Float as a left join IFMIS.dbo.tbl_R_BMSProgramAccounts as b on a.FMISAccountCode = b.AccountID " +
                                                 "WHERE a.release_float_id = '" + release_float_id + "' GROUP BY release_float_id,MonthOf,YearOf,Batch,FMISOfficeCode ,FMISProgramCode , FMISAccountCode,b.ObjectOfExpendetureID,a.AmountMOOE ", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Release_Float_Edit Float = new Release_Float_Edit();
                    Float.release_float_id = reader.GetInt64(0);
                    Float.FMISOfficeCode = reader.GetInt32(1);
                    Float.FMISProgramCode = reader.GetInt32(2);
                    Float.Amount_release = Convert.ToDecimal(reader.GetValue(3));
                    Float.MonthOf = reader.GetInt32(4);
                    Float.YearOf = reader.GetInt32(5);
                    Float.FMISAccountCode = reader.GetInt32(6);
                    Float.batch = reader.GetInt32(7);
                    Float.ooe_ids = reader.GetInt32(8);

                    return Float;
                }
            }
            return new Release_Float_Edit();
        }

        public Release_Float_Edit Edit_Float_co(int? release_float_id, int? ooe_id)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"  SELECT a.release_float_id ,a.FMISOfficeCode ,a.FMISProgramCode ,a.AmountCO ,a.MonthOf ,a.YearOf, a.FMISAccountCode ,a.Batch , b.ObjectOfExpendetureID " +
                                                 "FROM IFMIS.dbo.tbl_R_BMS_Release_Float as a left join IFMIS.dbo.tbl_R_BMSProgramAccounts as b on a.FMISAccountCode = b.AccountID " +
                                                 "WHERE a.release_float_id = '" + release_float_id + "' GROUP BY release_float_id,MonthOf,YearOf,Batch,FMISOfficeCode ,FMISProgramCode , FMISAccountCode,b.ObjectOfExpendetureID,a.AmountCO ", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Release_Float_Edit Float = new Release_Float_Edit();
                    Float.release_float_id = reader.GetInt64(0);
                    Float.FMISOfficeCode = reader.GetInt32(1);
                    Float.FMISProgramCode = reader.GetInt32(2);
                    Float.Amount_release = Convert.ToDecimal(reader.GetValue(3));
                    Float.MonthOf = reader.GetInt32(4);
                    Float.YearOf = reader.GetInt32(5);
                    Float.FMISAccountCode = reader.GetInt32(6);
                    Float.batch = reader.GetInt32(7);
                    Float.ooe_ids = reader.GetInt32(8);

                    return Float;
                }
            }
            return new Release_Float_Edit();
        }




        public string FloatDisplay(string[] release_float_id)
        {


            try
            {
                var idx = 0;

                foreach (var item in release_float_id)
                {
                    List<Monthly_SubInc> ok2 = new List<Monthly_SubInc>();
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@" select release_float_id from IFMIS.dbo.tbl_R_BMS_SubsidyRelease_Float  where release_float_id = '" + item + "' and ActionCode = 1", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();


                        while (reader.Read())
                        {
                            Monthly_SubInc Float = new Monthly_SubInc();
                            Float.release_float_id = reader.GetInt64(0);

                            ok2.Add(Float);

                            release_Ay = Float.release_float_id;

                        }
                    }
                    if (release_Ay == 0)
                    {
                        using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        {
                            SqlCommand com4 = new SqlCommand("insert into fmis.dbo.tblBMS_Releases (UserID ,DateTimeEntered ,ActionCode ,FMISOfficeCode ,FMISProgramCode ,AmountPS ,AmountMOOE ,AmountCO ,MonthOf ,YearOf ,DateReleased ,FMISAccountCode ,BalancePS ,BalanceMOOE ,BalanceCO ,Batch ,WithSubsidyFlag,ProjectFlag,OtherFund,PerAccountFlag) " +
                                                            "SELECT UserID ,DateTimeEntered ,ActionCode ,FMISOfficeCode ,FMISProgramCode ,AmountPS ,AmountMOOE ,AmountCO ,MonthOf ,YearOf ,DateReleased ,FMISAccountCode ,BalancePS ,BalanceMOOE ,BalanceCO ,Batch , WithSubsidyFlag,0,'No',1 FROM IFMIS.dbo.tbl_R_BMS_Release_Float where release_float_id = '" + item + "' and ActionCode = 1", con);
                            con.Open();
                            com4.ExecuteNonQuery();

                            con.Close();

                            SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_Release (UserID ,DateTimeEntered ,ActionCode ,FMISOfficeCode ,FMISProgramCode ,AmountPS ,AmountMOOE ,AmountCO ,MonthOf ,YearOf ,DateReleased ,FMISAccountCode ,BalancePS ,BalanceMOOE ,BalanceCO ,Batch ,WithSubsidyFlag,release_float_id)  " +
                                                            "SELECT UserID ,DateTimeEntered ,ActionCode ,FMISOfficeCode ,FMISProgramCode ,AmountPS ,AmountMOOE ,AmountCO ,MonthOf ,YearOf ,DateReleased ,FMISAccountCode ,BalancePS ,BalanceMOOE ,BalanceCO ,Batch , WithSubsidyFlag,release_float_id FROM IFMIS.dbo.tbl_R_BMS_Release_Float where release_float_id = '" + item + "' and ActionCode = 1", con);
                            con.Open();
                            com.ExecuteNonQuery();
                        }

                        using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        {
                            SqlCommand com47 = new SqlCommand(@"SELECT UserID ,DateTimeEntered ,FMISOfficeCode ,FMISProgramCode ,AmountPS ,AmountMOOE ,AmountCO ,MonthOf ,YearOf ,DateReleased ,FMISAccountCode ,BalancePS ,BalanceMOOE ,BalanceCO ,Batch , WithSubsidyFlag FROM IFMIS.dbo.tbl_R_BMS_Release_Float where release_float_id = '" + item + "' and ActionCode = 1", con);
                            con.Open();
                            SqlDataReader reader7 = com47.ExecuteReader();
                            while (reader7.Read())
                            {
                                Monthly_Supplement_Model edit_list = new Monthly_Supplement_Model();
                                edit_list.UserID = reader7.GetValue(0).ToString();
                                edit_list.DateTimeEntered = reader7.GetValue(1) is DBNull ? "" : reader7.GetValue(1).ToString();
                                edit_list.FMISOfficeCode = reader7.GetValue(2) is DBNull ? 0 : reader7.GetInt32(2);
                                edit_list.FMISProgramCode = reader7.GetValue(3) is DBNull ? 0 : reader7.GetInt32(3);
                                edit_list.AmountPS = reader7.GetValue(4) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(4));
                                edit_list.AmountMOOE = reader7.GetValue(5) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(5));
                                edit_list.AmountCO = reader7.GetValue(6) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(6));
                                edit_list.MonthOf = reader7.GetValue(7) is DBNull ? 0 : reader7.GetInt32(7);
                                edit_list.YearOf = reader7.GetValue(8) is DBNull ? 0 : reader7.GetInt32(8);
                                edit_list.DateReleased = reader7.GetValue(9).ToString();
                                edit_list.FMISAccountCode = reader7.GetValue(10) is DBNull ? 0 : reader7.GetInt32(10);
                                edit_list.BalancePS = reader7.GetValue(11) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(11));
                                edit_list.BalanceMOOE = reader7.GetValue(12) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(12));
                                edit_list.BalanceCO = reader7.GetValue(13) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(13));
                                edit_list.Batch = reader7.GetValue(14) is DBNull ? 0 : reader7.GetInt32(14);
                                edit_list.WithSubsidyFlag = reader7.GetValue(15) is DBNull ? 0 : reader7.GetInt32(15);


                                UserID_FLOAT = edit_list.UserID;
                                DateTimeEntered_FLOAT = edit_list.DateTimeEntered;
                                FMISOfficeCode_FLOAT = edit_list.FMISOfficeCode;
                                FMISProgramCode_FLOAT = edit_list.FMISProgramCode;
                                AmountPS_FLOAT = edit_list.AmountPS;
                                AmountMOOE_FLOAT = edit_list.AmountMOOE;
                                AmountCO_FLOAT = edit_list.AmountCO;
                                MonthOf_FLOAT = edit_list.MonthOf;
                                YearOf_FLOAT = edit_list.YearOf;
                                DateReleased_FLOAT = edit_list.DateReleased;
                                FMISAccountCode_FLOAT = edit_list.FMISAccountCode;
                                BalancePS_FLOAT = edit_list.BalancePS;
                                BalanceMOOE_FLOAT = edit_list.BalanceMOOE;
                                BalanceCO_FLOAT = edit_list.BalanceCO;
                                Batch_FLOAT = edit_list.Batch;
                                WithSubsidyFlag_FLOAT = edit_list.WithSubsidyFlag;
                                   
                            }

                        }




                        using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        {

                            SqlCommand com4 = new SqlCommand("update fmis.dbo.tblBMS_Releases_Float set ActionCode = 2 where UserID = '" + UserID_FLOAT + "' and DateTimeEntered = '" + DateTimeEntered_FLOAT + "' and FMISOfficeCode = '" + FMISOfficeCode_FLOAT + "' " +
                                      "and FMISProgramCode = '" + FMISProgramCode_FLOAT + "' and AmountPS = '" + AmountPS_FLOAT + "' and AmountMOOE = '" + AmountMOOE_FLOAT + "' and AmountCO = '" + AmountCO_FLOAT + "' " +
                                      "and MonthOf = '" + MonthOf_FLOAT + "' and YearOf = '" + YearOf_FLOAT + "' and DateReleased = '" + DateReleased_FLOAT + "' and FMISAccountCode = '" + FMISAccountCode_FLOAT + "' " +
                                      "and BalancePS = '" + BalancePS_FLOAT + "' and BalanceMOOE = '" + BalanceMOOE_FLOAT + "' and BalanceCO = '" + BalanceCO_FLOAT + "' and WithSubsidyFlag = '" + WithSubsidyFlag_FLOAT + "' and Batch = '" + Batch_FLOAT + "'", con);
                            con.Open();
                            com4.ExecuteNonQuery();
                            con.Close();

                            SqlCommand com = new SqlCommand(" update IFMIS.dbo.tbl_R_BMS_Release_Float set ActionCode = 2, Float_Flag = '1', DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where release_float_id = '" + item + "'", con);
                            con.Open();
                            com.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        release_Ay = 0;

                    }

                    idx++;

                }
             
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public IEnumerable<Monthly_Realignment_Model> Read_From(int? office_id, int? year_)
        {
            List<Monthly_Realignment_Model> Realign_From = new List<Monthly_Realignment_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"ifmis.dbo.sp_Monthly_Realignment_List '" + office_id + "','" + year_ + "' ", con);

               
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_Realignment_Model From_List = new Monthly_Realignment_Model();
                    From_List.realignment_id = reader.GetInt64(0);
                    From_List.DateTimeEntered = reader.GetValue(1).ToString();
                    From_List.FromAccountName = reader.GetValue(2).ToString();
                    From_List.FromAccountCode = reader.GetInt32(3);
                    From_List.ToAccountName = reader.GetValue(4).ToString();
                    From_List.ToAccountCode = reader.GetInt32(5);
                    From_List.Amount = Convert.ToDouble(reader.GetValue(6));
                    From_List.ToAmount = Convert.ToDouble(reader.GetValue(7));
                    From_List.YearOf = reader.GetInt32(8);
                    From_List.user_name = reader.GetValue(9).ToString();
                    From_List.type_ = reader.GetInt32(10);
                    Realign_From.Add(From_List);

                }
            }
            return Realign_From;
        }


        public IEnumerable<Monthly_Realignment_Model> Read_From_(int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_)
        {
            List<Monthly_Realignment_Model> Realign_From = new List<Monthly_Realignment_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"  select realignment_id,CONVERT(DATE, DateTimeEntered) AS DateTimeEntered,Amount,YearOf  FROM IFMIS.dbo.tbl_R_BMS_Realignment as a " +
                                                " where   a.FromOfficeCode = '" + office_id + "' and a.FromProgramCode = '" + program_id + "' and a.FromAccountCode = '" + account_id + "' and a.YearOf = '" + year_ + "' and a.ActionCode = 1  " +
                                                " and (ToOfficeCode is null or ToOfficeCode = 0) ", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_Realignment_Model From_List = new Monthly_Realignment_Model();
                    From_List.realignment_id = reader.GetInt64(0);
                    From_List.DateTimeEntered = reader.GetValue(1).ToString();
                   
                    From_List.Amount = Convert.ToDouble(reader.GetValue(2));
                    From_List.YearOf = reader.GetInt32(3);



                    Realign_From.Add(From_List);

                }
            }
            return Realign_From;
        }


        public IEnumerable<Monthly_Realignment_Model> Read_To(int? office_id_to, int? program_id_to, int? ooe_id_to, int? account_id_to, int? year_)
        {
            List<Monthly_Realignment_Model> Realign_To = new List<Monthly_Realignment_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"	SELECT a.realignment_id,b.AccountName,a.ToAccountCode,a.Amount,a.YearOf,a.DateTimeEntered,
                                                     CONCAT(Firstname COLLATE Latin1_General_CI_AS,'  ' COLLATE Latin1_General_CI_AS, LastName COLLATE Latin1_General_CI_AS)
	                                                    FROM IFMIS.dbo.tbl_R_BMS_Realignment as a  
	                                                    left join IFMIS.dbo.tbl_R_BMSProgramAccounts as b 
                                                            on a.ToAccountCode = b.AccountID and a.ToProgramCode = b.ProgramID and a.ActionCode =  b.ActionCode and a.YearOf = b.AccountYear
		                                                    left join pmis.dbo.employee as  c on a.UserID = c.eid
                                                where   a.ToOfficeCode = '" + office_id_to + "' and a.ToProgramCode = '" + program_id_to + "' and a.ToAccountCode = '" + account_id_to + "' and a.YearOf = '" + year_ + "' and a.ActionCode = 1 ", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_Realignment_Model To_List = new Monthly_Realignment_Model();
                    To_List.realignment_id = reader.GetInt64(0);
                    To_List.Description = reader.GetValue(1).ToString();
                    To_List.FromAccountCode = reader.GetInt32(2);
                    To_List.Amount = Convert.ToDouble(reader.GetValue(3));
                    To_List.YearOf = reader.GetInt32(4);
                    To_List.DateTimeEntered = reader.GetValue(5).ToString();
                    To_List.user_name = reader.GetValue(6).ToString();


                    Realign_To.Add(To_List);

                }
            }
            return Realign_To;
        }


        //public void realign_to_Update(Monthly_Realignment_Model realing_to)
        //{
        //    using (SqlConnection con = new SqlConnection(Common.MyConn()))
        //    {
        //        SqlCommand com = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_realing_to set ActionCode = 2 , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt'))  where Legal_id =" + realing_to.realignment_id + "", con);
        //        con.Open();
        //        com.ExecuteNonQuery();
        //        con.Close();
        //        SqlCommand com2 = new SqlCommand(@"insert into IFMIS.dbo.tbl_R_BMS_realing_to (LegalCode,LegalDescription,UserID,ActionCode,DateTimeEntered,YearOf) values ('" + realing_to.realignment_id + "','" + realing_to.realignment_id + "','" + Account.UserInfo.eid.ToString() + "',1,format(GetDate(),'M/d/yyy hh:mm:ss tt')," + realing_to.YearOf + ")", con);
        //        con.Open();
        //        com2.ExecuteNonQuery();
        //    }
        //}
        //public void realign_from_Update(Monthly_Realignment_Model realing_from)
        //{
        //    using (SqlConnection con = new SqlConnection(Common.MyConn()))
        //    {
        //        SqlCommand com = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_realing_from set ActionCode = 2 , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt'))  where Legal_id =" + realing_from.realignment_id + "", con);
        //        con.Open();
        //        com.ExecuteNonQuery();
        //        con.Close();
        //        SqlCommand com2 = new SqlCommand(@"insert into IFMIS.dbo.tbl_R_BMS_realing_from (LegalCode,LegalDescription,UserID,ActionCode,DateTimeEntered,YearOf)values ('" + realing_from.realignment_id + "','" + realing_from.realignment_id + "','" + Account.UserInfo.eid.ToString() + "',1,format(GetDate(),'M/d/yyy hh:mm:ss tt')," + realing_from.YearOf + ")", con);
        //        con.Open();
        //        com2.ExecuteNonQuery();
        //    }
        //}


        public string Edit_Realign(int? realignment_id)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select Amount from IFMIS.dbo.tbl_R_BMS_Realignment where realignment_id = " + realignment_id + "", con);
               
                con.Open();
                return com.ExecuteScalar().ToString();


            }
        }


        public Monthly_Realignment_Model Edit_RealignV2(int? realignment_id)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select realignment_id,Amount,FromAccountCode,FromProgramCode from IFMIS.dbo.tbl_R_BMS_Realignment where realignment_id = " + realignment_id + "", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_Realignment_Model Float = new Monthly_Realignment_Model();
                    Float.realignment_id_EDIT = reader.GetInt64(0);
                    Float.Amount_EDIT = Convert.ToDouble(reader.GetValue(1));
                    Float.FromAccountCode = reader.GetInt32(2);
                    Float.FromProgramCode = reader.GetInt32(3);

                    return Float;
                }
            }
            return new Monthly_Realignment_Model();
        }

        public Monthly_Realignment_Model Edit_RealignTo_n(int? realignment_id)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select realignment_id,Amount,ToAccountCode,ToProgramCode from IFMIS.dbo.tbl_R_BMS_Realignment where realignment_id = " + realignment_id + "", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_Realignment_Model Float = new Monthly_Realignment_Model();
                    Float.realignment_id_EDIT = reader.GetInt64(0);
                    Float.Amount_EDIT = Convert.ToDouble(reader.GetValue(1));
                    Float.ToAccountCode = reader.GetInt32(2);
                    Float.ToProgramCode = reader.GetInt32(3);

                    return Float;
                }
            }
            return new Monthly_Realignment_Model();
        }
        Int64 realign_ID_edit = 0;
        double realaign_amount_AV_edit = 0;
        double realaign_amount_AVP_edit = 0;
        double realaign_amount_AVP_edit_last = 0;
        int trno_ = 0;
        double realaign_amount_AV_edit_old = 0;
        double realaign_amount_AVP_edit_old = 0;
        double realaign_amount_AVP_edit_last_old = 0;

        public string save_realign_edit(int? year_, double amount, int? realignment_id, double amount_dum,string transdate)
        {
            try
            {
               //old____
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"SELECT  trnno,Amount FROM fmis.dbo.tblBMS_TotalRealign where YearOf = '" + year_ + "' and ActionCode = 1", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();


                    while (reader.Read())
                    {
                        Monthly_Realignment_Model Float = new Monthly_Realignment_Model();
                        Float.trnno = reader.GetValue(0) is DBNull ? 0 : reader.GetInt32(0);
                        Float.amount_totalss = reader.GetValue(1) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(1));

                        

                        trno_ = Float.trnno;
                        realaign_amount_AV_edit_old = Float.amount_totalss;
                    }

               
                  
                 
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com2 = new SqlCommand("update fmis.dbo.tblBMS_TotalRealign set ActionCode = 2 ,DateTimeEntered =DateTimeEntered +','+  format(GetDate(),'M/d/yyy hh:mm:ss tt') " +
                                    " , userID = '" + Account.UserInfo.eid.ToString() + "'  where YearOf = '" + year_ + "'  and ActionCode = 1 and trnno = '" + trno_ + "'", con);
                    con.Open();
                    com2.ExecuteNonQuery();

                }
                realaign_amount_AVP_edit_old = realaign_amount_AV_edit_old - amount_dum;
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com3 = new SqlCommand("insert into fmis.dbo.tblBMS_TotalRealign (UserID,Amount,ActionCode,YearOf,DateTimeEntered) values ('" + Account.UserInfo.eid.ToString() + "','" + realaign_amount_AVP_edit_old + "',4,'" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'))", con);
                    con.Open();
                    com3.ExecuteNonQuery();


                }
                realaign_amount_AVP_edit_last_old = realaign_amount_AVP_edit_old + amount;
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com3 = new SqlCommand("insert into fmis.dbo.tblBMS_TotalRealign (UserID,Amount,ActionCode,YearOf,DateTimeEntered) values ('" + Account.UserInfo.eid.ToString() + "','" + realaign_amount_AVP_edit_last_old + "',1,'" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'))", con);
                    con.Open();
                    com3.ExecuteNonQuery();
                    realaign_amount_AVP_edit_last_old = 0;
                    realaign_amount_AV_edit_old = 0;
                    realaign_amount_AVP_edit_old = 0;
                }


                //old____

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com47 = new SqlCommand(@"SELECT  totalrealign_id,Amount FROM [IFMIS].[dbo].[tbl_R_BMS_TotalRealign] where YearOf = '" + year_ + "' and ActionCode = 1", con);
                    con.Open();
                    SqlDataReader reader7 = com47.ExecuteReader();
                    while (reader7.Read())
                    {
                        Monthly_Realignment_Model subs_available = new Monthly_Realignment_Model();
                        subs_available.total_rel_id = reader7.GetInt64(0);
                        subs_available.available_amount = Convert.ToDouble(reader7.GetValue(1));

                        realign_ID_edit = subs_available.total_rel_id;
                        realaign_amount_AV_edit = subs_available.available_amount;


                    }

                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com2 = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_TotalRealign set ActionCode = 4 , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where ActionCode = 1 and totalrealign_id = '" + realign_ID_edit + "'", con);
                    con.Open();
                    com2.ExecuteNonQuery();

                }
                realaign_amount_AVP_edit = realaign_amount_AV_edit - amount_dum;
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com3 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_TotalRealign (UserID,Amount,ActionCode,YearOf,DateTimeEntered,TOfficeCode ,ToProgramCode ,ToOOECode ,ToAccountCode ,ToAmount) " +
                                                    "values ('" + Account.UserInfo.eid.ToString() + "','" + realaign_amount_AVP_edit + "',4,'" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),0,0,0,0,0)", con);
                    con.Open();
                    com3.ExecuteNonQuery();

                  
                }
                realaign_amount_AVP_edit_last = realaign_amount_AVP_edit + amount;
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com3 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_TotalRealign (UserID,Amount,ActionCode,YearOf,DateTimeEntered,TOfficeCode ,ToProgramCode ,ToOOECode ,ToAccountCode ,ToAmount) " +
                                                    "values ('" + Account.UserInfo.eid.ToString() + "','" + realaign_amount_AVP_edit_last + "',1,'" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),0,0,0,0,0)", con);
                    con.Open();
                    com3.ExecuteNonQuery();
                    realaign_amount_AVP_edit_last = 0;
                    realaign_amount_AV_edit = 0;
                    realaign_amount_AVP_edit = 0;
                }




                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com47 = new SqlCommand(@"SELECT FromOfficeCode,FromProgramCode,FromAccountCode,ToOfficeCode,ToProgramCode,ToAccountCode,Amount,UserID,DateTimeEntered,YearOf FROM IFMIS.dbo.tbl_R_BMS_Realignment where realignment_id = " + realignment_id + "", con);
                    con.Open();
                    SqlDataReader reader7 = com47.ExecuteReader();
                    while (reader7.Read())
                    {
                        Monthly_Edit_Realign_Model edit_list = new Monthly_Edit_Realign_Model();
                        edit_list.FromOfficeCode = reader7.GetInt32(0);
                        edit_list.FromProgramCode = reader7.GetInt32(1);
                        edit_list.FromAccountCode = reader7.GetInt32(2);
                        edit_list.ToOfficeCode = reader7.GetInt32(3);
                        edit_list.ToProgramCode = reader7.GetInt32(4);
                        edit_list.ToAccountCode = reader7.GetInt32(5);
                        edit_list.AmountCodes = Convert.ToDouble(reader7.GetValue(6));
                        edit_list.UserIDCodes = reader7.GetValue(7).ToString();
                        edit_list.DateTimeEnteredCodes = reader7.GetValue(8).ToString();
                        edit_list.yearOfCodes = reader7.GetInt32(9);



                        FromOfficeCodess = edit_list.FromOfficeCode;
                        FromProgramCodess = edit_list.FromProgramCode;
                        FromAccountCodess = edit_list.FromAccountCode;
                        ToOfficeCodess = edit_list.ToOfficeCode;
                        ToProgramCodess = edit_list.ToProgramCode;
                        ToAccountCodess = edit_list.ToAccountCode;
                        AmountCodess = edit_list.AmountCodes;
                        UserIDCodess =  edit_list.UserIDCodes;
                        DateTimeEnteredCodess = edit_list.DateTimeEnteredCodes;
                        yearOfCodess = edit_list.yearOfCodes;


                        //float AmountCodes = 0;
                        //string UserIDCodes;
                        //string DateTimeEnteredCodes;
                        //int yearOfCodes = 0;
        

                    }

                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"update fmis.dbo.tblBMS_Reallignment set ActionCode = 3 where FromOfficeCode = '" + FromOfficeCodess + "' and FromProgCode = '" + FromProgramCodess + "' and  " +
                      " FromAccountCode = '" + FromAccountCodess + "' and Amount = '" + AmountCodess + "' and UserID = '" + UserIDCodess + "' and DateTimeEntered = '" + DateTimeEnteredCodess + "' and YearOf = '" + yearOfCodess + "' and ActionCode = 1 and (ToAccountCode = 0 or ToAccountCode is null)", con);
                    con.Open();
                    com.ExecuteNonQuery();

                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_Realignment set ActionCode = 3 ,DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')) , userID = CONCAT(userID,' , ', '" + Account.UserInfo.eid.ToString() + "') where realignment_id = " + realignment_id + "", con);
                    con.Open();
                    com.ExecuteNonQuery();

                }
                
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("insert into  fmis.dbo.tblBMS_Reallignment (FromOfficeCode,FromProgCode,FromAccountCode,ToOfficeCode,ToProgCode,ToAccountCode,Amount,UserID,DateTimeEntered,ActionCode,YearOf) " +
                      " values ('" + FromOfficeCodess + "','" + FromProgramCodess + "','" + FromAccountCodess + "','" + ToOfficeCodess + "','" + ToProgramCodess + "','" + ToAccountCodess + "','" + amount + "','" + Account.UserInfo.eid.ToString() + "', '" + transdate + "',1,'" + year_ + "') ", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
                    SqlCommand com2 = new SqlCommand("insert into  IFMIS.dbo.tbl_R_BMS_Realignment (FromOfficeCode,FromProgramCode,FromAccountCode,ToOfficeCode,ToProgramCode,ToAccountCode,Amount,UserID,DateTimeEntered,ActionCode,YearOf) " +
                      " values ('" + FromOfficeCodess + "','" + FromProgramCodess + "','" + FromAccountCodess + "','" + ToOfficeCodess + "','" + ToProgramCodess + "','" + ToAccountCodess + "','" + amount + "','" + Account.UserInfo.eid.ToString() + "', '" + transdate + "',1,'" + year_ + "') ", con);
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



        public string save_realign_edit_TO(int? year_, double amount, int? realignment_id, double amount_dum,string transdate,int? copydate)
        {
            try
            {
                //---------------------- old ----------------------//

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"SELECT  trnno,Amount FROM fmis.dbo.tblBMS_TotalRealign where YearOf = '" + year_ + "' and ActionCode = 1", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();


                    while (reader.Read())
                    {
                        Monthly_Realignment_Model Float = new Monthly_Realignment_Model();
                        Float.trnno = reader.GetValue(0) is DBNull ? 0 : reader.GetInt32(0);
                        Float.amount_totalss = reader.GetValue(1) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(1));



                        trno_ = Float.trnno;
                        realaign_amount_AV_edit_old = Float.amount_totalss;
                    }




                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com2 = new SqlCommand("update fmis.dbo.tblBMS_TotalRealign set ActionCode = 2 ,DateTimeEntered =DateTimeEntered +','+  format(GetDate(),'M/d/yyy hh:mm:ss tt') " +
                                    " , userID = '" + Account.UserInfo.eid.ToString() + "'  where YearOf = '" + year_ + "'  and ActionCode = 1 and trnno = '" + trno_ + "'", con);
                    con.Open();
                    com2.ExecuteNonQuery();

                }
                realaign_amount_AVP_edit_old = realaign_amount_AV_edit_old + amount_dum;
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com3 = new SqlCommand("insert into fmis.dbo.tblBMS_TotalRealign (UserID,Amount,ActionCode,YearOf,DateTimeEntered) values ('" + Account.UserInfo.eid.ToString() + "','" + realaign_amount_AVP_edit_old + "',4,'" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'))", con);
                    con.Open();
                    com3.ExecuteNonQuery();


                }
                realaign_amount_AVP_edit_last_old = realaign_amount_AVP_edit_old - amount;
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com3 = new SqlCommand("insert into fmis.dbo.tblBMS_TotalRealign (UserID,Amount,ActionCode,YearOf,DateTimeEntered) values ('" + Account.UserInfo.eid.ToString() + "','" + realaign_amount_AVP_edit_last_old + "',1,'" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'))", con);
                    con.Open();
                    com3.ExecuteNonQuery();
                    realaign_amount_AVP_edit_last_old = 0;
                    realaign_amount_AV_edit_old = 0;
                    realaign_amount_AVP_edit_old = 0;
                }


                //---------------------- old ----------------------//

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com47 = new SqlCommand(@"SELECT  totalrealign_id,Amount FROM [IFMIS].[dbo].[tbl_R_BMS_TotalRealign] where YearOf = '" + year_ + "' and ActionCode = 1", con);
                    con.Open();
                    SqlDataReader reader7 = com47.ExecuteReader();
                    while (reader7.Read())
                    {
                        Monthly_Realignment_Model subs_available = new Monthly_Realignment_Model();
                        subs_available.total_rel_id = reader7.GetInt64(0);
                        subs_available.available_amount = Convert.ToDouble(reader7.GetValue(1));

                        realign_ID_edit = subs_available.total_rel_id;
                        realaign_amount_AV_edit = subs_available.available_amount;


                    }

                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com2 = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_TotalRealign set ActionCode = 4 , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where ActionCode = 1 and totalrealign_id = '" + realign_ID_edit + "'", con);
                    con.Open();
                    com2.ExecuteNonQuery();

                }
                realaign_amount_AVP_edit = realaign_amount_AV_edit + amount_dum;
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com3 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_TotalRealign (UserID,Amount,ActionCode,YearOf,DateTimeEntered,TOfficeCode ,ToProgramCode ,ToOOECode ,ToAccountCode ,ToAmount) " +
                                                    "values ('" + Account.UserInfo.eid.ToString() + "','" + realaign_amount_AVP_edit + "',4,'" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),0,0,0,0,0)", con);
                    con.Open();
                    com3.ExecuteNonQuery();


                }
                realaign_amount_AVP_edit_last = realaign_amount_AVP_edit - amount;
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com3 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_TotalRealign (UserID,Amount,ActionCode,YearOf,DateTimeEntered,TOfficeCode ,ToProgramCode ,ToOOECode ,ToAccountCode ,ToAmount) " +
                                                    "values ('" + Account.UserInfo.eid.ToString() + "','" + realaign_amount_AVP_edit_last + "',1,'" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),0,0,0,0,0)", con);
                    con.Open();
                    com3.ExecuteNonQuery();
                    realaign_amount_AVP_edit_last = 0;
                    realaign_amount_AV_edit = 0;
                    realaign_amount_AVP_edit = 0;
                }

              
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com47 = new SqlCommand(@"SELECT FromOfficeCode,FromProgramCode,FromAccountCode,ToOfficeCode,ToProgramCode,ToAccountCode,Amount,UserID,DateTimeEntered,YearOf FROM IFMIS.dbo.tbl_R_BMS_Realignment where realignment_id = " + realignment_id + "", con);
                    con.Open();
                    SqlDataReader reader7 = com47.ExecuteReader();
                    while (reader7.Read())
                    {
                        Monthly_Edit_Realign_Model edit_list = new Monthly_Edit_Realign_Model();
                        edit_list.FromOfficeCode = reader7.GetInt32(0);
                        edit_list.FromProgramCode = reader7.GetInt32(1);
                        edit_list.FromAccountCode = reader7.GetInt32(2);
                        edit_list.ToOfficeCode = reader7.GetInt32(3);
                        edit_list.ToProgramCode = reader7.GetInt32(4);
                        edit_list.ToAccountCode = reader7.GetInt32(5);
                        edit_list.AmountCodes = Convert.ToDouble(reader7.GetValue(6));
                        edit_list.UserIDCodes = reader7.GetValue(7).ToString();
                        edit_list.DateTimeEnteredCodes = reader7.GetValue(8).ToString();
                        edit_list.yearOfCodes = reader7.GetInt32(9);



                        FromOfficeCodess = edit_list.FromOfficeCode;
                        FromProgramCodess = edit_list.FromProgramCode;
                        FromAccountCodess = edit_list.FromAccountCode;
                        ToOfficeCodess = edit_list.ToOfficeCode;
                        ToProgramCodess = edit_list.ToProgramCode;
                        ToAccountCodess = edit_list.ToAccountCode;
                        AmountCodess = edit_list.AmountCodes;
                        UserIDCodess =  edit_list.UserIDCodes;
                        DateTimeEnteredCodess = edit_list.DateTimeEnteredCodes;
                        yearOfCodess = edit_list.yearOfCodes;


                        //float AmountCodes = 0;
                        //string UserIDCodes;
                        //string DateTimeEnteredCodes;
                        //int yearOfCodes = 0;
        

                    }

                
                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@" update fmis.dbo.tblBMS_Reallignment set ActionCode = 2,DateTimeEntered=DateTimeEntered +','+ format(GetDate(),'M/d/yyyy hh:mm:ss tt') WHERE YearOf = '" + year_ + "' and ToOfficeCode = '" + ToOfficeCodess + "' and ToProgCode = '" + ToProgramCodess + "' and ToAccountCode = '" + ToAccountCodess + "' and Amount = '" + AmountCodess + "' and ActionCode = 1 and DateTimeEntered ='" + DateTimeEnteredCodess + "'", con);//transdate
                    con.Open();
                    com.ExecuteNonQuery();

                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_Realignment set ActionCode = 3 ,DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')) , userID = CONCAT(userID,' , ', '" + Account.UserInfo.eid.ToString() + "') where realignment_id = " + realignment_id + "", con);
                    con.Open();
                    com.ExecuteNonQuery();

                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    if (copydate == 1)
                    {
                        SqlCommand com4 = new SqlCommand("insert into  fmis.dbo.tblBMS_Reallignment (FromOfficeCode,FromProgCode,FromAccountCode,ToOfficeCode,ToProgCode,ToAccountCode,Amount,UserID,DateTimeEntered,ActionCode,YearOf) " +
                         " values ('" + FromOfficeCodess + "','" + FromProgramCodess + "','" + FromAccountCodess + "','" + ToOfficeCodess + "','" + ToProgramCodess + "','" + ToAccountCodess + "','" + amount + "','" + Account.UserInfo.eid.ToString() + "','" + transdate + "',1,'" + year_ + "') ", con);
                        con.Open();
                        com4.ExecuteNonQuery();
                        con.Close();
                        SqlCommand com = new SqlCommand("insert into  IFMIS.dbo.tbl_R_BMS_Realignment (FromOfficeCode,FromProgramCode,FromAccountCode,ToOfficeCode,ToProgramCode,ToAccountCode,Amount,UserID,DateTimeEntered,ActionCode,YearOf) " +
                        " values ('" + FromOfficeCodess + "','" + FromProgramCodess + "','" + FromAccountCodess + "','" + ToOfficeCodess + "','" + ToProgramCodess + "','" + ToAccountCodess + "','" + amount + "','" + Account.UserInfo.eid.ToString() + "','" + transdate + "',1,'" + year_ + "') ", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        return "1";
                    }
                    else
                    {
                        SqlCommand com4 = new SqlCommand("insert into  fmis.dbo.tblBMS_Reallignment (FromOfficeCode,FromProgCode,FromAccountCode,ToOfficeCode,ToProgCode,ToAccountCode,Amount,UserID,DateTimeEntered,ActionCode,YearOf) " +
                         " values ('" + FromOfficeCodess + "','" + FromProgramCodess + "','" + FromAccountCodess + "','" + ToOfficeCodess + "','" + ToProgramCodess + "','" + ToAccountCodess + "','" + amount + "','" + Account.UserInfo.eid.ToString() + "',format(getdate(),'M/d/yyyy hh:mm:ss tt'),1,'" + year_ + "') ", con);
                        con.Open();
                        com4.ExecuteNonQuery();
                        con.Close();
                        SqlCommand com = new SqlCommand("insert into  IFMIS.dbo.tbl_R_BMS_Realignment (FromOfficeCode,FromProgramCode,FromAccountCode,ToOfficeCode,ToProgramCode,ToAccountCode,Amount,UserID,DateTimeEntered,ActionCode,YearOf) " +
                        " values ('" + FromOfficeCodess + "','" + FromProgramCodess + "','" + FromAccountCodess + "','" + ToOfficeCodess + "','" + ToProgramCodess + "','" + ToAccountCodess + "','" + amount + "','" + Account.UserInfo.eid.ToString() + "',format(getdate(),'M/d/yyyy hh:mm:ss tt'),1,'" + year_ + "') ", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        return "1";
                    }
                }

            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }


        public string Supplement_amount(int? legal_code, int? office_id, int? program_id, int? ooe_id, int? account_id,  int? MonthOf, int? year_)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT Amount FROM IFMIS.dbo.tbl_R_BMS_SupplementalBudget where LegalCode = " + legal_code + " and FMISOfficeCode = " + office_id + " and FMISProgramCode = " + program_id + " and FMISAccountCode = " + account_id + " and OOECode = " + ooe_id + " and MonthOf = " + MonthOf + " and YearOf = " + year_ + " and ActionCode = 1", con);
                con.Open();
                return com.ExecuteScalar().ToString();


            }
        }




        public Monthly_Realignment_Model Supplement_amountV2(int? legal_code, int? office_id, int? program_id, int? ooe_id, int? account_id, int? MonthOf, int? year_)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT supplementalbudget_id, Amount FROM IFMIS.dbo.tbl_R_BMS_SupplementalBudget where LegalCode = '" + legal_code + "' and FMISOfficeCode = '" + office_id + "' and FMISProgramCode = '" + program_id + "' and FMISAccountCode = '" + account_id + "' and OOECode = '" + ooe_id + "' and MonthOf = '" + MonthOf + "' and YearOf = '" + year_ + "' and ActionCode = 1", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_Realignment_Model Float = new Monthly_Realignment_Model();
                    Float.supplementalbudget_id = reader.GetInt64(0);
                    Float.Amount_Sup = Convert.ToDouble(reader.GetValue(1));

                    return Float;
                }
            }
            return new Monthly_Realignment_Model();
        }


        public string save_Floats(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_, double ps_, double mooe_, double co_, double balance_ps, double balance_mooe, double balance_co, int? float_id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@" update IFMIS.dbo.tbl_R_BMS_Release_Float set ActionCode = 3  , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "')  where release_float_id = '" + float_id + "' ", con);
                    con.Open();
                    com.ExecuteNonQuery();

                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com = new SqlCommand("  insert into IFMIS.dbo.tbl_R_BMS_Release_Float (UserID,DateTimeEntered,ActionCode,FMISOfficeCode,FMISProgramCode,AmountPS,AmountMOOE,AmountCO,MonthOf " +
                                                      " ,YearOf,DateReleased,FMISAccountCode,BalancePS,BalanceMOOE,BalanceCO,Batch,Float_Flag,WithSubsidyFlag,PerAccountFlag) values ('" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),1,'" + office_id + "','" + program_id + "' " +
                                                      " ,'" + ps_ + "','" + mooe_ + "','" + co_ + "','" + month_ + "','" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),'" + account_id + "','" + balance_ps + "','" + balance_mooe + "','" + balance_co + "','" + numeric_ + "',0,0,1)", con);
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
        public string save_FloatsV2(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_, double release_ps, double release_mooe, double release_co, double remainPS, double remainMOOE, double remainCO, string date_)
        {
            try
            {
               
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com3 = new SqlCommand("  insert into fmis.dbo.tblBMS_Releases_Float (UserID,DateTimeEntered,ActionCode,FMISOfficeCode,FMISProgramCode,AmountPS,AmountMOOE,AmountCO,MonthOf " +
                                                      " ,YearOf,DateReleased,FMISAccountCode,BalancePS,BalanceMOOE,BalanceCO,Batch,WithSubsidyFlag,ProjectFlag,OtherFund,PerAccountFlag) values ('" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),1,'" + office_id + "','" + program_id + "' " +
                                                      " ,'" + release_ps + "','" + release_mooe + "','" + release_co + "','" + month_ + "','" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),'" + account_id + "','" + remainPS + "','" + remainMOOE + "','" + remainCO + "','" + numeric_ + "',0,0,'No',0)", con);
                    con.Open();
                    com3.ExecuteNonQuery();
                    con.Close();
                    SqlCommand com = new SqlCommand("  insert into IFMIS.dbo.tbl_R_BMS_Release_Float (UserID,DateTimeEntered,ActionCode,FMISOfficeCode,FMISProgramCode,AmountPS,AmountMOOE,AmountCO,MonthOf " +
                                                      " ,YearOf,DateReleased,FMISAccountCode,BalancePS,BalanceMOOE,BalanceCO,Batch,Float_Flag,WithSubsidyFlag,PerAccountFlag) values ('" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),1,'" + office_id + "','" + program_id + "' " +
                                                      " ,'" + release_ps + "','" + release_mooe + "','" + release_co + "','" + month_ + "','" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),'" + account_id + "','" + remainPS + "','" + remainMOOE + "','" + remainCO + "','" + numeric_ + "',0,0,1)", con);
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

        public Monthly_Realignment_Model get_balance(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_MonthlyRelease_Float_release '" + office_id + "','" + program_id + "','" + ooe_id + "','" + account_id + "','" + month_ + "','" + numeric_ + "','" + year_ + "'", con);



                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_Realignment_Model Float = new Monthly_Realignment_Model();

                    Float.balance_amount_ps = reader.GetValue(0) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(0));
                    Float.balance_amount_mooe = reader.GetValue(1) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(1));
                    Float.balance_amount_co = reader.GetValue(2) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(2));
                    Float.available_amount = reader.GetValue(3) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(3));
                   
                    return Float;
                }
            }
            return new Monthly_Realignment_Model();
        }

        public Monthly_Realignment_Model dibay_dibay(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_, double acc_available)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_MonthlyRelease_Divide '" + office_id + "','" + program_id + "','" + ooe_id + "','" + account_id + "','" + month_ + "','" + numeric_ + "','" + year_ + "','" + acc_available + "'", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_Realignment_Model Float = new Monthly_Realignment_Model();
                    Float.debayd_PS = Convert.ToDouble(reader.GetValue(0));
                    Float.debayd_MOOE = Convert.ToDouble(reader.GetValue(1));
                    Float.debayd_CO = Convert.ToDouble(reader.GetValue(2));
                   

                    return Float;
                }
            }
            return new Monthly_Realignment_Model();
        }

        public string Release_(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_, double release_ps, double release_mooe, double release_co, double remainPS, double remainMOOE, double remainCO, string date_)
        {
            try
            {
                
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com = new SqlCommand(@" update IFMIS.dbo.tbl_R_BMS_Release_Float set Float_Flag = 2, ActionCode = 2 ,DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')) , userID = CONCAT(userID,' , ', '" + Account.UserInfo.eid.ToString() + "') " +
                        " where ActionCode = 2 and FMISOfficeCode = '" + office_id + "' and FMISProgramCode = '" + program_id + "' and Float_Flag = 1 and YearOf = '" + year_ + "' and FMISAccountCode = '" + account_id + "'  and MonthOf = '" + month_ + "' ", con);
                    con.Open();
                    com.ExecuteNonQuery();

                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com3 = new SqlCommand("  insert into fmis.dbo.tblBMS_Releases (UserID,DateTimeEntered,ActionCode,FMISOfficeCode,FMISProgramCode,AmountPS,AmountMOOE,AmountCO,MonthOf,YearOf,DateReleased,FMISAccountCode,BalancePS,BalanceMOOE,BalanceCO,Batch,WithSubsidyFlag,ProjectFlag,OtherFund,PerAccountFlag) " +
                                           "values ('" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),1,'" + office_id + "','" + program_id + "','" + release_ps + "', " +
                                                     " '" + release_mooe + "','" + release_co + "','" + month_ + "','" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),'" + account_id + "','" + remainPS + "','" + remainMOOE + "','" + remainCO + "','" + numeric_ + "',0,0,'No',1)", con);
                    con.Open();
                    com3.ExecuteNonQuery();
                    con.Close();
                    SqlCommand com = new SqlCommand("  exec sp_BMS_Release  " +
                                            "" + Account.UserInfo.eid.ToString() + "," + office_id + "," + program_id + "," + release_ps + ", " +
                                                      " " + release_mooe + "," + release_co + "," + month_ + "," + year_ + "," + account_id + "," + remainPS + "," + remainMOOE + "," + remainCO + "," + numeric_ + "", con);
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

        double tots_mount = 0;
        public string Release_EE_(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_, double release_ps, double release_mooe, double release_co, double remainPS, double remainMOOE, double remainCO, int? subsIn, string date_, double income_Available, double subsidy_Available)
        { 
            try 
            {
                 
                tots_mount = release_ps + release_mooe + release_co;


                if (subsIn == 1)
                {
                      if (subsidy_Available < tots_mount)
                    {

                        return "4";
                    }
                      else { 
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com4 = new SqlCommand(@"insert into fmis.dbo.tblBMS_SubsidyRelease (FMISOfficeCode ,Amount ,MonthOf ,YearOf ,UserID ,DateTimeEntered ,ActionCode ,Batch) " +
                                                         " values ('" + office_id + "' ,'" + tots_mount + "' ,'" + month_ + "' ,'" + year_ + "' ,'" + Account.UserInfo.eid.ToString() + "' ,format(GetDate(),'M/d/yyy hh:mm:ss tt') ,1 ,'" + numeric_ + "')", con);
                        con.Open();
                        com4.ExecuteNonQuery();

                        con.Close();
                        SqlCommand com = new SqlCommand(@"insert into IFMIS.dbo.tbl_R_BMS_SubsidyRelease (FMISOfficeCode ,Amount ,MonthOf ,YearOf ,UserID ,DateTimeEntered ,ActionCode ,Batch,release_float_id) " +
                                                         " values ('" + office_id + "' ,'" + tots_mount + "' ,'" + month_ + "' ,'" + year_ + "' ,'" + Account.UserInfo.eid.ToString() + "' ,format(GetDate(),'M/d/yyy hh:mm:ss tt') ,1 ,'" + numeric_ + "',0)", con);
                        con.Open();
                        com.ExecuteNonQuery();

                    }
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com3 = new SqlCommand("  insert into fmis.dbo.tblBMS_Releases (UserID,DateTimeEntered,ActionCode,FMISOfficeCode,FMISProgramCode,AmountPS,AmountMOOE,AmountCO,MonthOf,YearOf,DateReleased,FMISAccountCode,BalancePS,BalanceMOOE,BalanceCO,Batch,WithSubsidyFlag,ProjectFlag,OtherFund,PerAccountFlag) " +
                                                 "values ('" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),1,'" + office_id + "','" + program_id + "','" + release_ps + "', " +
                                                           " '" + release_mooe + "','" + release_co + "','" + month_ + "','" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),'" + account_id + "','" + remainPS + "','" + remainMOOE + "','" + remainCO + "','" + numeric_ + "','" + subsIn + "',0,'No',1)", con);
                        con.Open();
                        com3.ExecuteNonQuery();
                        con.Close();

                        SqlCommand com = new SqlCommand("  insert into IFMIS.dbo.tbl_R_BMS_Release (UserID,DateTimeEntered,ActionCode,FMISOfficeCode,FMISProgramCode,AmountPS,AmountMOOE,AmountCO,MonthOf,YearOf,DateReleased,FMISAccountCode,BalancePS,BalanceMOOE,BalanceCO,Batch,WithSubsidyFlag,release_float_id,PerAccountFlag) " +
                                                "values ('" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),1,'" + office_id + "','" + program_id + "','" + release_ps + "', " +
                                                          " '" + release_mooe + "','" + release_co + "','" + month_ + "','" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),'" + account_id + "','" + remainPS + "','" + remainMOOE + "','" + remainCO + "','" + numeric_ + "','" + subsIn + "',0,1)", con);
                        con.Open();
                        com.ExecuteNonQuery();

                    }
                      }
                }
                else if (subsIn == 0)
                {
                    if (income_Available < tots_mount) {

                        return "3";
                    }
                   
                    else { 
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com4 = new SqlCommand(@"insert into [fmis].[dbo].[tblBMS_IncomeRelease] (FMISOfficeCode ,Amount ,MonthOf ,YearOf ,UserID ,DateTimeEntered ,ActionCode ,Batch) " +
                                                         " values ('" + office_id + "' ,'" + tots_mount + "' ,'" + month_ + "' ,'" + year_ + "' ,'" + Account.UserInfo.eid.ToString() + "' ,format(GetDate(),'M/d/yyy hh:mm:ss tt') ,1 ,'" + numeric_ + "')", con);
                        con.Open();
                        com4.ExecuteNonQuery();

                        con.Close();
                        SqlCommand com = new SqlCommand(@"insert into IFMIS.dbo.tbl_R_BMS_IncomeRelease (FMISOfficeCode ,Amount ,MonthOf ,YearOf ,UserID ,DateTimeEntered ,ActionCode ,Batch,release_float_id) " +
                                                         " values ('" + office_id + "' ,'" + tots_mount + "' ,'" + month_ + "' ,'" + year_ + "' ,'" + Account.UserInfo.eid.ToString() + "' ,format(GetDate(),'M/d/yyy hh:mm:ss tt') ,1 ,'" + numeric_ + "',0)", con);
                        con.Open();
                        com.ExecuteNonQuery();

                    }
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com3 = new SqlCommand("  insert into fmis.dbo.tblBMS_Releases (UserID,DateTimeEntered,ActionCode,FMISOfficeCode,FMISProgramCode,AmountPS,AmountMOOE,AmountCO,MonthOf,YearOf,DateReleased,FMISAccountCode,BalancePS,BalanceMOOE,BalanceCO,Batch,WithSubsidyFlag,ProjectFlag,OtherFund,PerAccountFlag) " +
                                                 "values ('" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),1,'" + office_id + "','" + program_id + "','" + release_ps + "', " +
                                                           " '" + release_mooe + "','" + release_co + "','" + month_ + "','" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),'" + account_id + "','" + remainPS + "','" + remainMOOE + "','" + remainCO + "','" + numeric_ + "','" + subsIn + "',0,'No',1)", con);
                        con.Open();
                        com3.ExecuteNonQuery();
                        con.Close();

                        SqlCommand com = new SqlCommand("  insert into IFMIS.dbo.tbl_R_BMS_Release (UserID,DateTimeEntered,ActionCode,FMISOfficeCode,FMISProgramCode,AmountPS,AmountMOOE,AmountCO,MonthOf,YearOf,DateReleased,FMISAccountCode,BalancePS,BalanceMOOE,BalanceCO,Batch,WithSubsidyFlag,release_float_id,PerAccountFlag) " +
                                                "values ('" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),1,'" + office_id + "','" + program_id + "','" + release_ps + "', " +
                                                          " '" + release_mooe + "','" + release_co + "','" + month_ + "','" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),'" + account_id + "','" + remainPS + "','" + remainMOOE + "','" + remainCO + "','" + numeric_ + "','" + subsIn + "',0,1)", con);
                        con.Open();
                        com.ExecuteNonQuery();

                    }
                    }
                }
                else
                {
                    return "2";
                }



                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    
                    SqlCommand com = new SqlCommand(@" update IFMIS.dbo.tbl_R_BMS_Release_Float set Float_Flag = 2, ActionCode = 2 ,DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')) , userID = CONCAT(userID,' , ', '" + Account.UserInfo.eid.ToString() + "') " +
                        " where ActionCode = 2 and FMISOfficeCode = '" + office_id + "' and FMISProgramCode = '" + program_id + "' and Float_Flag = 1 and YearOf = '" + year_ + "' and FMISAccountCode = '" + account_id + "'  and MonthOf = '" + month_ + "' ", con);
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

        // public string release_cya()
        //{
        //    try
        //    {
        //        using (SqlConnection con3 = new SqlConnection(Common.MyConn()))
        //        {
        //            SqlCommand com2 = new SqlCommand("  insert into IFMIS.dbo.tbl_R_BMS_Release (UserID,DateTimeEntered,ActionCode,FMISOfficeCode,FMISProgramCode,AmountPS,AmountMOOE,AmountCO,MonthOf,YearOf,DateReleased,FMISAccountCode,BalancePS,BalanceMOOE,BalanceCO,Batch) " +
        //                                                "select UserID,DateTimeEntered,ActionCode,FMISOfficeCode,FMISProgramCode,AmountPS,AmountMOOE,AmountCO,MonthOf,YearOf,DateReleased,FMISAccountCode,BalancePS,BalanceMOOE,BalanceCO,Batch FROM IFMIS.dbo.tbl_R_BMS_Release_Float where release_float_id = '" + release_float_ids + "'", con3);
        //            con3.Open();
        //            com2.ExecuteNonQuery();
                   
        //        }
        //        using (SqlConnection con3 = new SqlConnection(Common.MyConn()))
        //        {
        //            SqlCommand com2 = new SqlCommand("  update IFMIS.dbo.tbl_R_BMS_Release set WithSubsidyFlag = 0 where release_float_id = '" + release_float_ids + "'", con3);
        //            con3.Open();
        //            com2.ExecuteNonQuery();
        //            return "1";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message.ToString();
        //    }
        //}



        public string delete_float(int? release_float_id=0,string code="")
        {
            try
            {
                  using (SqlConnection con = new SqlConnection(Common.MyConn()))
                       {
                           SqlCommand com47 = new SqlCommand(@"SELECT UserID ,DateTimeEntered ,FMISOfficeCode ,FMISProgramCode ,AmountPS ,AmountMOOE ,AmountCO ,MonthOf ,YearOf ,DateReleased ,FMISAccountCode ,BalancePS ,BalanceMOOE ,BalanceCO ,Batch , WithSubsidyFlag FROM IFMIS.dbo.tbl_R_BMS_Release_Float where release_float_id = '" + release_float_id + "' and ActionCode = 1", con);
                           con.Open();
                           SqlDataReader reader7 = com47.ExecuteReader();
                           while (reader7.Read())
                           {
                               Monthly_Supplement_Model edit_list = new Monthly_Supplement_Model();
                               edit_list.UserID = reader7.GetValue(0).ToString();
                               edit_list.DateTimeEntered = reader7.GetValue(1) is DBNull ? "" : reader7.GetValue(1).ToString();
                               edit_list.FMISOfficeCode = reader7.GetValue(2) is DBNull ? 0 : reader7.GetInt32(2);
                               edit_list.FMISProgramCode = reader7.GetValue(3) is DBNull ? 0 : reader7.GetInt32(3);
                               edit_list.AmountPS = reader7.GetValue(4) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(4));
                               edit_list.AmountMOOE = reader7.GetValue(5) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(5));
                               edit_list.AmountCO = reader7.GetValue(6) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(6));
                               edit_list.MonthOf = reader7.GetValue(7) is DBNull ? 0 : reader7.GetInt32(7);
                               edit_list.YearOf = reader7.GetValue(8) is DBNull ? 0 : reader7.GetInt32(8);
                               edit_list.DateReleased = reader7.GetValue(9).ToString();
                               edit_list.FMISAccountCode = reader7.GetValue(10) is DBNull ? 0 : reader7.GetInt32(10);
                               edit_list.BalancePS = reader7.GetValue(11) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(11));
                               edit_list.BalanceMOOE = reader7.GetValue(12) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(12));
                               edit_list.BalanceCO = reader7.GetValue(13) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(13));
                               edit_list.Batch = reader7.GetValue(14) is DBNull ? 0 : reader7.GetInt32(14);
                               edit_list.WithSubsidyFlag = reader7.GetValue(15) is DBNull ? 0 : reader7.GetInt32(15);


                               UserID_FLOAT = edit_list.UserID;
                               DateTimeEntered_FLOAT = edit_list.DateTimeEntered;
                               FMISOfficeCode_FLOAT = edit_list.FMISOfficeCode;
                               FMISProgramCode_FLOAT = edit_list.FMISProgramCode;
                               AmountPS_FLOAT = edit_list.AmountPS;
                               AmountMOOE_FLOAT = edit_list.AmountMOOE;
                               AmountCO_FLOAT = edit_list.AmountCO;
                               MonthOf_FLOAT = edit_list.MonthOf;
                               YearOf_FLOAT = edit_list.YearOf;
                               DateReleased_FLOAT = edit_list.DateReleased;
                               FMISAccountCode_FLOAT = edit_list.FMISAccountCode;
                               BalancePS_FLOAT = edit_list.BalancePS;
                               BalanceMOOE_FLOAT = edit_list.BalanceMOOE;
                               BalanceCO_FLOAT = edit_list.BalanceCO;
                               Batch_FLOAT = edit_list.Batch;
                               WithSubsidyFlag_FLOAT = edit_list.WithSubsidyFlag;



                           }

                       }


                  using (SqlConnection con = new SqlConnection(Common.MyConn()))
                  {

                      SqlCommand com4 = new SqlCommand(@"update fmis.dbo.tblBMS_Releases_Float set ActionCode = 2 where UserID = '" + UserID_FLOAT + "' and DateTimeEntered = '" + DateTimeEntered_FLOAT + "' and FMISOfficeCode = '" + FMISOfficeCode_FLOAT + "' " +
                                "and FMISProgramCode = '" + FMISProgramCode_FLOAT + "' and AmountPS = '" + AmountPS_FLOAT + "' and AmountMOOE = '" + AmountMOOE_FLOAT + "' and AmountCO = '" + AmountCO_FLOAT + "' " +
                                "and MonthOf = '" + MonthOf_FLOAT + "' and YearOf = '" + YearOf_FLOAT + "' and DateReleased = '" + DateReleased_FLOAT + "' and FMISAccountCode = '" + FMISAccountCode_FLOAT + "' " +
                                "and BalancePS = '" + BalancePS_FLOAT + "' and BalanceMOOE = '" + BalanceMOOE_FLOAT + "' and BalanceCO = '" + BalanceCO_FLOAT + "' and WithSubsidyFlag = '" + WithSubsidyFlag_FLOAT + "' and Batch = '" + Batch_FLOAT + "'", con);
                      con.Open();
                      com4.ExecuteNonQuery();
                      con.Close();
                  }
                  using (SqlConnection con = new SqlConnection(Common.MyConn()))
                  {

                      SqlCommand com4 = new SqlCommand(@"update fmis.dbo.tblBMS_Releases set ActionCode = 2 where DateTimeEntered = '" + DateTimeEntered_FLOAT + "' and FMISOfficeCode = '" + FMISOfficeCode_FLOAT + "' " +
                                "and FMISProgramCode = '" + FMISProgramCode_FLOAT + "' and AmountPS = '" + AmountPS_FLOAT + "' and AmountMOOE = '" + AmountMOOE_FLOAT + "' and AmountCO = '" + AmountCO_FLOAT + "' " +
                                "and MonthOf = '" + MonthOf_FLOAT + "' and YearOf = '" + YearOf_FLOAT + "' and DateReleased = '" + DateReleased_FLOAT + "' and FMISAccountCode = '" + FMISAccountCode_FLOAT + "' " +
                                "and BalancePS = '" + BalancePS_FLOAT + "' and BalanceMOOE = '" + BalanceMOOE_FLOAT + "' and BalanceCO = '" + BalanceCO_FLOAT + "' and WithSubsidyFlag = '" + WithSubsidyFlag_FLOAT + "' and Batch = '" + Batch_FLOAT + "'", con);
                      con.Open();
                      com4.ExecuteNonQuery();
                      con.Close();
                  }

                //using (SqlConnection con = new SqlConnection(Common.MyConn()))
                //{

                //    SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_Release set ActionCode = 4  , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "')  where release_float_id = '" + release_float_id + "' ", con);
                //    con.Open();
                //    com.ExecuteNonQuery();
                   
                //}
                //using (SqlConnection con = new SqlConnection(Common.MyConn()))
                //{

                //    SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_Release_Float set ActionCode = 4  , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "')  where release_float_id = '" + release_float_id + "' ", con);
                //    con.Open();
                //    com.ExecuteNonQuery();
                //    return "1";
                //}
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com = new SqlCommand("sp_BMS_CancelReleaseFloat '" + release_float_id + "', '" + Account.UserInfo.eid.ToString() + "','"+ code + "' ", con);
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


        public IEnumerable<Release_Float_Model> release_lists(int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_)
        {



            List<Release_Float_Model> MRsList_GF = new List<Release_Float_Model>();


            if (ooe_id == 1) { 

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT a.release_id,b.AccountName,a.AmountPS - isnull(d.amount,0) as AmountPS ,DateName( month , DateAdd( month , a.MonthOf , 0 ) - 1 ) as MonthOf ,a.YearOf ,a.BalancePS,a.Batch, a.DateReleased ,a.DateTimeEntered,a.MonthOf as RelMonth,e.Lastname + ', '+ e.Firstname + ' '+  isnull(e.Suffix,'') + ' '+ left(e.mi,1) + '. ' as 'enduser' " +
                                                    " FROM IFMIS.dbo.tbl_R_BMS_Release as a " +
                                                    " left join IFMIS.dbo.tbl_R_BMSAccounts as b on a.FMISAccountCode = b.FMISAccountCode " +
									             	" left join IFMIS.dbo.tbl_R_BMSProgramAccounts as c on a.FMISAccountCode = c.AccountID and a.FMISProgramCode = c.ProgramID  " +
                                                    " left join (Select[officeid],[programid],[accountid], sum([amount]) as amount, releaseid  from[IFMIS].[dbo].[tbl_R_BMS_ReleaseReversion] "+
                                                    " where actioncode = 1 and yearof = "+ year_ + " group by[officeid],[programid],[accountid], releaseid) as d on d.officeid = a.FMISOfficeCode and d.programid = a.FMISProgramCode "+
                                                    " and d.accountid = a.FMISAccountCode and d.[releaseid]=a.release_id left join  pmis.dbo.employee as e on e.eid = a.UserID "+
                                                    " where c.ObjectOfExpendetureID = '" + ooe_id + "' and a.FmiSofficeCode = '" + office_id + "' and a.FMISProgramCode = '" + program_id + "' and a.FMISAccountCode = '" + account_id + "'  " +
                                                    " and a.YearOf = '" + year_ + "' and a.ActionCode = 1  " +
                                                    " GROUP BY a.release_id,a.MonthOf,a.YearOf,a.Batch,a.FMISOfficeCode ,a.FMISProgramCode , a.FMISAccountCode,b.AccountName,a.AmountPS ,a.BalancePS,a.DateReleased,a.DateTimeEntered ,d.amount,e.Lastname , e.Firstname ,e.Suffix,e.mi ", con);
               

                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Release_Float_Model Float = new Release_Float_Model();
                    Float.release_id = reader.GetInt64(0);
                    Float.amount_description = reader.GetValue(1).ToString();
                    Float.Amount_release = Convert.ToDecimal(reader.GetValue(2));
                    Float.MonthOf = reader.GetValue(3).ToString();
                    Float.YearOf = reader.GetInt32(4);
                    Float.balance_amount = Convert.ToDecimal(reader.GetValue(5));
                    Float.batch = reader.GetInt32(6);
                    Float.DateReleased = reader.GetValue(7).ToString();
                    Float.DateTimeEntered = reader.GetValue(8).ToString();
                    Float.MonthOf_ = reader.GetInt32(9);
                    Float.enduser=  reader.GetValue(10).ToString();                  
                    MRsList_GF.Add(Float);

                }
            }

            }
            else if (ooe_id == 2) {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"SELECT a.release_id,b.AccountName,a.AmountMOOE - isnull(d.amount,0) as AmountMOOE ,DateName( month , DateAdd( month , a.MonthOf , 0 ) - 1 ) as MonthOf ,a.YearOf ,a.BalanceMOOE,a.Batch, a.DateReleased AS DateReleased, a.DateTimeEntered,a.MonthOf as RelMonth,e.Lastname + ', '+ e.Firstname + ' '+  isnull(e.Suffix,'') + ' '+ left(e.mi,1) + '. ' as 'enduser' " +
                                                        " FROM IFMIS.dbo.tbl_R_BMS_Release as a " +
                                                        " left join IFMIS.dbo.tbl_R_BMSAccounts as b on a.FMISAccountCode = b.FMISAccountCode " +
                                                        " left join IFMIS.dbo.tbl_R_BMSProgramAccounts as c on a.FMISAccountCode = c.AccountID and a.FMISProgramCode = c.ProgramID  " +
                                                        " left join (Select[officeid],[programid],[accountid], sum([amount]) as amount, releaseid  from[IFMIS].[dbo].[tbl_R_BMS_ReleaseReversion] " +
                                                        " where actioncode = 1 and yearof = " + year_ + " group by[officeid],[programid],[accountid], releaseid) as d on d.officeid = a.FMISOfficeCode and d.programid = a.FMISProgramCode " +
                                                        " and d.accountid = a.FMISAccountCode and d.[releaseid]=a.release_id left join  pmis.dbo.employee as e on e.eid = a.UserID" +
                                                        " where c.ObjectOfExpendetureID = '" + ooe_id + "' and a.FmiSofficeCode = '" + office_id + "' and a.FMISProgramCode = '" + program_id + "' and a.FMISAccountCode = '" + account_id + "'  " +
                                                        " and a.YearOf = '" + year_ + "' and a.ActionCode = 1  " +
                                                        " GROUP BY a.release_id,a.MonthOf,a.YearOf,a.Batch,a.FMISOfficeCode ,a.FMISProgramCode , a.FMISAccountCode,b.AccountName,a.AmountMOOE ,a.BalanceMOOE,a.DateReleased,a.DateTimeEntered,d.amount,e.Lastname , e.Firstname ,e.Suffix,e.mi  ", con);


                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Release_Float_Model Float = new Release_Float_Model();
                        Float.release_id = reader.GetInt64(0);
                        Float.amount_description = reader.GetValue(1).ToString();
                        Float.Amount_release = Convert.ToDecimal(reader.GetValue(2));
                        Float.MonthOf = reader.GetValue(3).ToString();
                        Float.YearOf = reader.GetInt32(4);
                        Float.balance_amount = Convert.ToDecimal(reader.GetValue(5));
                        Float.batch = reader.GetInt32(6);
                        Float.DateReleased = reader.GetValue(7).ToString();
                        Float.DateTimeEntered = reader.GetValue(8).ToString();
                        Float.MonthOf_ = reader.GetInt32(9);
                        Float.enduser = reader.GetValue(10).ToString();

                        MRsList_GF.Add(Float);

                    }
                }


            }
            else if (ooe_id == 3)
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"SELECT a.release_id,b.AccountName,a.AmountCO - isnull(d.amount,0) as AmountCO,DateName( month , DateAdd( month , a.MonthOf , 0 ) - 1 ) as MonthOf ,a.YearOf ,a.BalanceCO,a.Batch, a.DateReleased, a.DateTimeEntered,a.MonthOf as RelMonth,e.Lastname + ', '+ e.Firstname + ' '+  isnull(e.Suffix,'') + ' '+ left(e.mi,1) + '. ' as 'enduser' " +
                                                        " FROM IFMIS.dbo.tbl_R_BMS_Release as a " +
                                                        " left join IFMIS.dbo.tbl_R_BMSAccounts as b on a.FMISAccountCode = b.FMISAccountCode " +
                                                        " left join IFMIS.dbo.tbl_R_BMSProgramAccounts as c on a.FMISAccountCode = c.AccountID and a.FMISProgramCode = c.ProgramID  " +
                                                        " left join (Select[officeid],[programid],[accountid], sum([amount]) as amount, releaseid  from[IFMIS].[dbo].[tbl_R_BMS_ReleaseReversion] " +
                                                        " where actioncode = 1 and yearof = " + year_ + " group by[officeid],[programid],[accountid], releaseid) as d on d.officeid = a.FMISOfficeCode and d.programid = a.FMISProgramCode " +
                                                        " and d.accountid = a.FMISAccountCode and d.[releaseid]=a.release_id left join  pmis.dbo.employee as e on e.eid = a.UserID " +
                                                        " where c.ObjectOfExpendetureID = '" + ooe_id + "' and a.FmiSofficeCode = '" + office_id + "' and a.FMISProgramCode = '" + program_id + "' and a.FMISAccountCode = '" + account_id + "'  " +
                                                        " and a.YearOf = '" + year_ + "' and a.ActionCode = 1  " +
                                                        " GROUP BY a.release_id,a.MonthOf,a.YearOf,a.Batch,a.FMISOfficeCode ,a.FMISProgramCode , a.FMISAccountCode,b.AccountName,a.AmountCO ,a.BalanceCO,a.DateReleased,a.DateTimeEntered,d.amount,e.Lastname , e.Firstname ,e.Suffix,e.mi   ", con);


                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Release_Float_Model Float = new Release_Float_Model();
                        Float.release_id = reader.GetInt64(0);
                        Float.amount_description = reader.GetValue(1).ToString();
                        Float.Amount_release = Convert.ToDecimal(reader.GetValue(2));
                        Float.MonthOf = reader.GetValue(3).ToString();
                        Float.YearOf = reader.GetInt32(4);
                        Float.balance_amount = Convert.ToDecimal(reader.GetValue(5));
                        Float.batch = reader.GetInt32(6);
                        Float.DateReleased = reader.GetValue(7).ToString();
                        Float.DateTimeEntered = reader.GetValue(8).ToString();
                        Float.MonthOf_ = reader.GetInt32(9);
                        Float.enduser = reader.GetValue(10).ToString();

                        MRsList_GF.Add(Float);

                    }
                }


            }
            return MRsList_GF;
        }

        public IEnumerable<OfficesModel> Offices_EE()
        {
            List<OfficesModel> OfficeList = new List<OfficesModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select OfficeID, isnull(cast([FunctionID] as varchar(4)),'') + '-' + CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') from tbl_R_BMSOffices where OfficeID in (41,38,37) order BY officeName ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    OfficesModel Office = new OfficesModel();
                    Office.OfficeID = reader.GetValue(0).ToString();
                    Office.OfficeName = reader.GetString(1);

                    OfficeList.Add(Office);
                }
            }
            return OfficeList;
        }

        public IEnumerable<OfficesModel> Offices_SEF()
        {
            List<OfficesModel> OfficeList = new List<OfficesModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (Account.UserInfo.lgu == 0)
                {
                    SqlCommand com = new SqlCommand("select OfficeID,isnull(cast([FunctionID] as varchar(4)),'') + '-' + CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') from tbl_R_BMSOffices where FundID = 201 order BY officeName ", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        OfficesModel Office = new OfficesModel();
                        Office.OfficeID = reader.GetValue(0).ToString();
                        Office.OfficeName = reader.GetString(1);

                        OfficeList.Add(Office);
                    }
                }
                else
                {
                    if (Account.UserInfo.UserTypeID != 1) //access all offices
                    {
                        SqlCommand com = new SqlCommand("select OfficeID,isnull(cast([FunctionID] as varchar(4)),'') + '-' + CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') from tbl_R_BMSOffices where FundID = 201 order BY officeName ", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            OfficesModel Office = new OfficesModel();
                            Office.OfficeID = reader.GetValue(0).ToString();
                            Office.OfficeName = reader.GetString(1);

                            OfficeList.Add(Office);
                        }
                    }
                    else
                    {
                        SqlCommand com = new SqlCommand("select OfficeID,isnull(cast([FunctionID] as varchar(4)),'') + '-' + CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') from tbl_R_BMSOffices where FundID = 201  and " +
                                                          "PMISOfficeID in (select department from pmis.dbo.vwMergeAllEmployee where eid =" + Account.UserInfo.eid + ")  order BY officeName ", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            OfficesModel Office = new OfficesModel();
                            Office.OfficeID = reader.GetValue(0).ToString();
                            Office.OfficeName = reader.GetString(1);

                            OfficeList.Add(Office);
                        }
                    }
                }
            }
            return OfficeList;
        }


        public IEnumerable<OfficesModel> Offices_()
        {
            List<OfficesModel> OfficeList = new List<OfficesModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (Account.UserInfo.lgu == 0)
                {
                    SqlCommand com = new SqlCommand("select OfficeID,isnull(cast([FunctionID] as varchar(4)),'') + '-' + CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') from tbl_R_BMSOffices where OfficeID not in (41,38,37) and PMISOfficeID !=0 order BY officeName ", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        OfficesModel Office = new OfficesModel();
                        Office.OfficeID = reader.GetValue(0).ToString();
                        Office.OfficeName = reader.GetString(1);

                        OfficeList.Add(Office);
                    }
                }
                else //other lgu
                {
                    if (Account.UserInfo.UserTypeID != 1) //access all offices
                    {
                        SqlCommand com = new SqlCommand("select OfficeID, isnull(cast([FunctionID] as varchar(4)),'') + '-' + CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') from tbl_R_BMSOffices where PMISOfficeID !=0 order BY officeName ", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            OfficesModel Office = new OfficesModel();
                            Office.OfficeID = reader.GetValue(0).ToString();
                            Office.OfficeName = reader.GetString(1);

                            OfficeList.Add(Office);
                        }
                    }
                    else
                    {
                        SqlCommand com = new SqlCommand("select OfficeID, isnull(cast([FunctionID] as varchar(4)),'') + '-' + CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') from tbl_R_BMSOffices where PMISOfficeID !=0 and "+
                                                          "(PMISOfficeID in (select department from pmis.dbo.vwMergeAllEmployee where eid =" + Account.UserInfo.eid + ") or officeid in (select [OfficeID] from [IFMIS].[dbo].[tbl_R_BMSUserMenu] where [UserID] =" + Account.UserInfo.eid + " )) order BY officeName ", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            OfficesModel Office = new OfficesModel();
                            Office.OfficeID = reader.GetValue(0).ToString();
                            Office.OfficeName = reader.GetString(1);

                            OfficeList.Add(Office);
                        }
                    }
                }
            }
            return OfficeList;
        }

        public IEnumerable<OfficesModel> GetOfficeAllFunds(int? officeaccounts, int? year)
        {
            List<OfficesModel> OfficeList = new List<OfficesModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select OfficeID, CONCAT(OfficeName,' (',REPLACE(OfficeAbbrivation, ' ', ''),')') from tbl_R_BMSOffices where PMISOfficeID !=0 order BY officeName ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    OfficesModel Office = new OfficesModel();
                    Office.OfficeID = reader.GetValue(0).ToString();
                    Office.OfficeName = reader.GetString(1);

                    OfficeList.Add(Office);
                }
            }
            return OfficeList;
        }

        public IEnumerable<OfficesModel> GetAllAccount(int? officeaccounts, int? year)
        {
            List<OfficesModel> OfficeList = new List<OfficesModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("exec sp_BMS_ProgramAllAccounts "+ year + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    OfficesModel Office = new OfficesModel();
                    Office.OfficeID = reader.GetValue(0).ToString();
                    Office.OfficeName = reader.GetString(1);

                    OfficeList.Add(Office);
                }
            }
            return OfficeList;
        }
        public IEnumerable<ProgramsModel> gPrograms(int? year_, int? office_ID)
        {
            List<ProgramsModel> pross = new List<ProgramsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select Distinct ProgramID,ProgramDescription from tbl_R_BMSOfficePrograms where ProgramYear = '" + year_ + "' and OfficeID = '" + office_ID + "' AND (ActionCode = 1)", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ProgramsModel app = new ProgramsModel();
                    app.ProgramID = reader.GetValue(0).ToString();
                    app.ProgramDescription = reader.GetString(1);

                    pross.Add(app);
                }
            }
            return pross;
        }

        public IEnumerable<Monthly_DD_Model> MOS_accounts(int? propYear, int? ooe_id, int? prog_id)
        {
            List<Monthly_DD_Model> account_list = new List<Monthly_DD_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT distinct AccountID,AccountName FROM IFMIS.dbo.tbl_R_BMSProgramAccounts where AccountYear = '" + propYear + "' and ObjectOfExpendetureID = '" + ooe_id + "' and ProgramID = '" + prog_id + "' AND (ActionCode = 1)", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_DD_Model app = new Monthly_DD_Model();

                    app.account_id = reader.GetInt32(0);
                    app.account_name = reader.GetValue(1).ToString();

                    account_list.Add(app);
                }
            }
            return account_list;
        }
        public IEnumerable<Monthly_DD_Model> _accounts(int? year_, int? prog_id)
        {
            List<Monthly_DD_Model> account_list = new List<Monthly_DD_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT distinct AccountID,AccountName FROM IFMIS.dbo.tbl_R_BMSProgramAccounts where AccountYear = '" + year_ + "'  and ProgramID = '" + prog_id + "' AND (ActionCode = 1)", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_DD_Model app = new Monthly_DD_Model();

                    app.account_id = reader.GetInt32(0);
                    app.account_name = reader.GetValue(1).ToString();

                    account_list.Add(app);
                }
            }
            return account_list;
        }
        public IEnumerable<Monthly_DD_Model> _account_subsidy(int? year_, int? prog_id)
        {
            List<Monthly_DD_Model> account_list = new List<Monthly_DD_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"[dbo].[sp_MonthlyRelease_Account_Subs] '" + prog_id + "','" + year_ + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_DD_Model app = new Monthly_DD_Model();

                    app.account_id = reader.GetInt32(0);
                    app.account_name = reader.GetValue(1).ToString();

                    account_list.Add(app);
                }
            }
            return account_list;
        }
        public IEnumerable<Monthly_Annuals> Subsidy_Grid(int? office_id)
        {
            List<Monthly_Annuals> Float_List = new List<Monthly_Annuals>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT a.Binding_ID ,c.AccountName ,a.YearOf ,b.ProposalAllotedAmount " +
                                                  "FROM IFMIS.dbo.tbl_R_BMS_Binding as a inner join IFMIS.dbo.tbl_T_BMSBudgetProposal as b  " +
                                                 " on a.ProgramCodeBind = b.ProgramID and a.AccountCodeBind = b.AccountID and b.ProposalYear = a.YearOf " +
                                                 "inner join IFMIS.dbo.tbl_R_BMSAccounts as c on b.AccountID = c.FMISAccountCode where OfficeCode = '" + office_id + "' and ActionCode = 1  " +
                                                 "and b.ProposalActionCode = 1 order by a.YearOf desc", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_Annuals Float = new Monthly_Annuals();
                    Float.Binding_ID = reader.GetInt64(0);
                    Float.AccountName = reader.GetValue(1).ToString();
                    Float.YearOf = reader.GetInt32(2);
                    Float.ProposalAllotedAmount = Convert.ToDouble(reader.GetValue(3));


                    Float_List.Add(Float);

                }
            }
            return Float_List;
        }


        public string save_subs(int? office_id, int? office_id_to, int? program_id_to, int? account_id_to, int? year_)
        {
            try
            {
                  using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com2 = new SqlCommand("  insert into fmis.dbo.tblBMS_Binding  (OfficeCode ,OfficeCodeBind ,ProgramCodeBind ,AccountCodeBind ,ActionCode ,UserID ,DateTimeEntered ,YearOf) " +
                                                   " values ('" + office_id + "' ,'" + office_id_to + "' ,'" + program_id_to + "' ,'" + account_id_to + "' ,1 , '" + Account.UserInfo.eid.ToString() + "' ,format(GetDate(),'M/d/yyy hh:mm:ss tt') ,'" + year_ + "')", con);
                    con.Open();
                    com2.ExecuteNonQuery();
                    con.Close();
                    SqlCommand com = new SqlCommand("  insert into IFMIS.dbo.tbl_R_BMS_Binding  (OfficeCode ,OfficeCodeBind ,ProgramCodeBind ,AccountCodeBind ,ActionCode ,UserID ,DateTimeEntered ,YearOf) "+
                                                    " values ('" + office_id + "' ,'" + office_id_to + "' ,'" + program_id_to + "' ,'" + account_id_to + "' ,1 , '" + Account.UserInfo.eid.ToString() + "' ,format(GetDate(),'M/d/yyy hh:mm:ss tt') ,'" + year_ + "')", con);
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


        public string MonthValue()
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT DATEPART(m, getdate())", con);
                con.Open();
                return com.ExecuteScalar().ToString();


            }
        }


        public string delete_subs(int? Binding_ID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_Binding set ActionCode = 2  , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "')  where Binding_ID = '" + Binding_ID + "' ", con);
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

        int? sub_office = 0;
        int? year_subs = 0;
        public Monthly_Annuals Delete_suu(int? Binding_ID)
        {
            Monthly_Annuals subsss = new Monthly_Annuals();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT OfficeCode,YearOf FROM IFMIS.dbo.tbl_R_BMS_Binding where Binding_ID = '" + Binding_ID + "'", con);

                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {

                    subsss.OfficeCode = reader.GetInt32(0);
                    subsss.YearOf = reader.GetInt32(1);
                  sub_office =  subsss.OfficeCode;
                  year_subs = subsss.YearOf;
                }
            }

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT sum(Amount) FROM IFMIS.dbo.tbl_R_BMS_SubsidyRelease where FMISOfficeCode = '" + sub_office + "' and YearOf ='" + year_subs + "' and ActionCode = 1", con);

                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {

                    subsss.ProposalAllotedAmount = Convert.ToDouble(reader.GetValue(0) is DBNull ? 0 : reader.GetValue(0));

                }
            }
            return subsss;
        }

        public string delete_subsidy(int? Binding_ID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_Binding set ActionCode = 2  , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where Binding_ID = '" + Binding_ID + "' ", con);
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

        public IEnumerable<Monthly_Annuals> Income_Grid(int? office_id,int? month_id)
        {
            List<Monthly_Annuals> Subs_List = new List<Monthly_Annuals>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT income_id ,DateName( month , DateAdd( month , MonthOf , 0 ) - 1 ) as MonthOf ,Amount ,YearOf  FROM IFMIS.dbo.tbl_R_BMS_Income where FMISOfficeCode = '" + office_id + "'  and ActionCode = 1 order by YearOf desc", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_Annuals Subs = new Monthly_Annuals();
                    Subs.income_id = reader.GetInt64(0);
                    Subs.MonthOf = reader.GetValue(1).ToString();
                    Subs.Amount_inc = Convert.ToDouble(reader.GetValue(2));
                    Subs.YearOf = reader.GetInt32(3);
                    


                    Subs_List.Add(Subs);

                }
            }
            return Subs_List;
        }



        public string save_incs_edit(int? office_id, int? month_id, double amount_inc, int? year_, int? income_ID)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com47 = new SqlCommand(@"SELECT FMISOfficeCode,MonthOf,Amount,YearOf,DateTimeEntered FROM IFMIS.dbo.tbl_R_BMS_Income where income_id = " + income_ID + "", con);
                    con.Open();
                    SqlDataReader reader7 = com47.ExecuteReader();
                    while (reader7.Read())
                    {
                        Monthly_SubInc edit_list = new Monthly_SubInc();
                        edit_list.FMISOfficeCode = reader7.GetValue(0) is DBNull ? 0 : reader7.GetInt32(0);
                        edit_list.MonthOf = reader7.GetValue(1) is DBNull ? 0 : reader7.GetInt32(1);
                        edit_list.Amount = reader7.GetValue(2) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(2));
                        edit_list.YearOf = reader7.GetValue(3) is DBNull ? 0 : reader7.GetInt32(3);
                        edit_list.DateTimeEntered = reader7.GetValue(4).ToString();

                        FMISOfficeCode_INC = edit_list.FMISOfficeCode;
                        MonthOf_INC = edit_list.MonthOf;
                        Amount_INC = edit_list.Amount;
                        YearOf_INC = edit_list.YearOf;
                        DateTimeEntered_INC = edit_list.DateTimeEntered;
                    }

                }



                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com4 = new SqlCommand("update fmis.dbo.tblBMS_Income set ActionCode = 2 where FMISOfficeCode = '" + FMISOfficeCode_INC + "' and MonthOf = '" + MonthOf_INC + "' and Amount = '" + Amount_INC + "' and YearOf = '" + YearOf_INC + "' and DateTimeEntered = '" + DateTimeEntered_INC + "'", con);
                    con.Open();
                    com4.ExecuteNonQuery();

                    con.Close();

                    SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_Income set ActionCode = 4  , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where income_id = '" + income_ID + "'", con);
                    con.Open();
                    com.ExecuteNonQuery();
                  
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com2 = new SqlCommand("insert into fmis.dbo.tblBMS_Income (FMISOfficeCode ,MonthOf ,Amount ,YearOf ,UserID ,DateTimeEntered ,ActionCode) " +
                                                  " values ('" + office_id + "' ,'" + month_id + "','" + amount_inc + "','" + year_ + "', '" + Account.UserInfo.eid.ToString() + "' ,format(GetDate(),'M/d/yyy hh:mm:ss tt'),1 )", con);
                    con.Open();
                    com2.ExecuteNonQuery();
                    con.Close();
                    SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_Income (FMISOfficeCode ,MonthOf ,Amount ,YearOf ,UserID ,DateTimeEntered ,ActionCode) " +
                                                    " values ('" + office_id + "' ,'" + month_id + "','" + amount_inc + "','" + year_ + "', '" + Account.UserInfo.eid.ToString() + "' ,format(GetDate(),'M/d/yyy hh:mm:ss tt'),1 )", con);
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




        public string FloatDisplay_Sub(string[] release_float_id)
        {
                 
            try
            {
                var idx = 0;

                foreach (var item in release_float_id)
                {

                      // List<Monthly_SubInc> ok2 = new List<Monthly_SubInc>();
                       using (SqlConnection con = new SqlConnection(Common.MyConn()))
                       {
                           SqlCommand com = new SqlCommand(@"select release_float_id,FMISOfficeCode ,AmountPS , AmountMOOE , AmountCO ,MonthOf ,YearOf ,DateTimeEntered , Batch FROM IFMIS.dbo.tbl_R_BMS_Release_Float where release_float_id = '" + item + "' and WithSubsidyFlag = 0", con);
                           con.Open();
                           SqlDataReader reader = com.ExecuteReader();


                           while (reader.Read())
                           {
                               Monthly_SubInc Float = new Monthly_SubInc();
                               Float.release_float_id = reader.GetInt64(0);
                               Float.FMISOfficeCode = reader.GetInt32(1);
                               Float.AmountPS = Convert.ToDouble(reader.GetValue(2));
                               Float.AmountMOOE = Convert.ToDouble(reader.GetValue(3));
                               Float.AmountCO = Convert.ToDouble(reader.GetValue(4));
                               Float.MonthOf = reader.GetInt32(5);
                               Float.YearOf = reader.GetInt32(6);
                               Float.MonthOf_D = reader.GetValue(7).ToString();
                               Float.Batch = reader.GetInt32(8);
                              // ok2.Add(Float);

                               release_float_id_float = Float.release_float_id;
                               FMISOfficeCode_float = Float.FMISOfficeCode;
                               Amount_float = Float.AmountPS + Float.AmountMOOE + Float.AmountCO;
                               MonthOf_float = Float.MonthOf;
                               YearOf_float = Float.YearOf;
                               DateTimeEntered_float = Float.MonthOf_D;
                               batch_float = Float.Batch;

                               Float_Subsss();



                           }
                       }
                       using (SqlConnection con = new SqlConnection(Common.MyConn()))
                       {
                           SqlCommand com4 = new SqlCommand("insert into fmis.dbo.tblBMS_Releases (UserID ,DateTimeEntered ,ActionCode ,FMISOfficeCode ,FMISProgramCode ,AmountPS ,AmountMOOE ,AmountCO ,MonthOf ,YearOf ,DateReleased ,FMISAccountCode ,BalancePS ,BalanceMOOE ,BalanceCO ,Batch ,WithSubsidyFlag,ProjectFlag,OtherFund,PerAccountFlag) " +
                                                            "SELECT UserID ,DateTimeEntered ,ActionCode ,FMISOfficeCode ,FMISProgramCode ,AmountPS ,AmountMOOE ,AmountCO ,MonthOf ,YearOf ,DateReleased ,FMISAccountCode ,BalancePS ,BalanceMOOE ,BalanceCO ,Batch , WithSubsidyFlag,0,'No',1 FROM IFMIS.dbo.tbl_R_BMS_Release_Float where release_float_id = '" + item + "' and ActionCode = 1", con);
                           con.Open();
                           com4.ExecuteNonQuery();

                           con.Close();

                           SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_Release (UserID ,DateTimeEntered ,ActionCode ,FMISOfficeCode ,FMISProgramCode ,AmountPS ,AmountMOOE ,AmountCO ,MonthOf ,YearOf ,DateReleased ,FMISAccountCode ,BalancePS ,BalanceMOOE ,BalanceCO ,Batch ,WithSubsidyFlag,release_float_id)  " +
                                                           "SELECT UserID ,DateTimeEntered ,ActionCode ,FMISOfficeCode ,FMISProgramCode ,AmountPS ,AmountMOOE ,AmountCO ,MonthOf ,YearOf ,DateReleased ,FMISAccountCode ,BalancePS ,BalanceMOOE ,BalanceCO ,Batch , WithSubsidyFlag,release_float_id FROM IFMIS.dbo.tbl_R_BMS_Release_Float where release_float_id = '" + item + "' and ActionCode = 1", con);
                           con.Open();
                           com.ExecuteNonQuery();
                       }
                       using (SqlConnection con = new SqlConnection(Common.MyConn()))
                       {
                           SqlCommand com47 = new SqlCommand(@"SELECT UserID ,DateTimeEntered ,FMISOfficeCode ,FMISProgramCode ,AmountPS ,AmountMOOE ,AmountCO ,MonthOf ,YearOf ,DateReleased ,FMISAccountCode ,BalancePS ,BalanceMOOE ,BalanceCO ,Batch , WithSubsidyFlag FROM IFMIS.dbo.tbl_R_BMS_Release_Float where release_float_id = '" + item + "' and ActionCode = 1", con);
                           con.Open();
                           SqlDataReader reader7 = com47.ExecuteReader();
                           while (reader7.Read())
                           {
                               Monthly_Supplement_Model edit_list = new Monthly_Supplement_Model();
                               edit_list.UserID = reader7.GetValue(0).ToString();
                               edit_list.DateTimeEntered = reader7.GetValue(1) is DBNull ? "" : reader7.GetValue(1).ToString();
                               edit_list.FMISOfficeCode = reader7.GetValue(2) is DBNull ? 0 : reader7.GetInt32(2);
                               edit_list.FMISProgramCode = reader7.GetValue(3) is DBNull ? 0 : reader7.GetInt32(3);
                               edit_list.AmountPS = reader7.GetValue(4) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(4));
                               edit_list.AmountMOOE = reader7.GetValue(5) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(5));
                               edit_list.AmountCO = reader7.GetValue(6) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(6));
                               edit_list.MonthOf = reader7.GetValue(7) is DBNull ? 0 : reader7.GetInt32(7);
                               edit_list.YearOf = reader7.GetValue(8) is DBNull ? 0 : reader7.GetInt32(8);
                               edit_list.DateReleased = reader7.GetValue(9).ToString();
                               edit_list.FMISAccountCode = reader7.GetValue(10) is DBNull ? 0 : reader7.GetInt32(10);
                               edit_list.BalancePS = reader7.GetValue(11) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(11));
                               edit_list.BalanceMOOE = reader7.GetValue(12) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(12));
                               edit_list.BalanceCO = reader7.GetValue(13) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(13));
                               edit_list.Batch = reader7.GetValue(14) is DBNull ? 0 : reader7.GetInt32(14);
                               edit_list.WithSubsidyFlag = reader7.GetValue(15) is DBNull ? 0 : reader7.GetInt32(15);


                               UserID_FLOAT = edit_list.UserID;
                               DateTimeEntered_FLOAT = edit_list.DateTimeEntered;
                               FMISOfficeCode_FLOAT = edit_list.FMISOfficeCode;
                               FMISProgramCode_FLOAT = edit_list.FMISProgramCode;
                               AmountPS_FLOAT = edit_list.AmountPS;
                               AmountMOOE_FLOAT = edit_list.AmountMOOE;
                               AmountCO_FLOAT = edit_list.AmountCO;
                               MonthOf_FLOAT = edit_list.MonthOf;
                               YearOf_FLOAT = edit_list.YearOf;
                               DateReleased_FLOAT = edit_list.DateReleased;
                               FMISAccountCode_FLOAT = edit_list.FMISAccountCode;
                               BalancePS_FLOAT = edit_list.BalancePS;
                               BalanceMOOE_FLOAT = edit_list.BalanceMOOE;
                               BalanceCO_FLOAT = edit_list.BalanceCO;
                               Batch_FLOAT = edit_list.Batch;
                               WithSubsidyFlag_FLOAT = edit_list.WithSubsidyFlag;



                           }

                       }


                        using (SqlConnection con = new SqlConnection(Common.MyConn()))
                        {

                            SqlCommand com4 = new SqlCommand(@"update fmis.dbo.tblBMS_Releases_Float set ActionCode = 2 where UserID = '" + UserID_FLOAT + "' and DateTimeEntered = '" + DateTimeEntered_FLOAT + "' and FMISOfficeCode = '" + FMISOfficeCode_FLOAT + "' " +
                                      "and FMISProgramCode = '" + FMISProgramCode_FLOAT + "' and AmountPS = '" + AmountPS_FLOAT + "' and AmountMOOE = '" + AmountMOOE_FLOAT + "' and AmountCO = '" + AmountCO_FLOAT + "' " +
                                      "and MonthOf = '" + MonthOf_FLOAT + "' and YearOf = '" + YearOf_FLOAT + "' and DateReleased = '" + DateReleased_FLOAT + "' and FMISAccountCode = '" + FMISAccountCode_FLOAT + "' " +
                                      "and BalancePS = '" + BalancePS_FLOAT + "' and BalanceMOOE = '" + BalanceMOOE_FLOAT + "' and BalanceCO = '" + BalanceCO_FLOAT + "' and WithSubsidyFlag = '" + WithSubsidyFlag_FLOAT + "' and Batch = '" + Batch_FLOAT + "'", con);
                            con.Open();
                            com4.ExecuteNonQuery();
                            con.Close();

                            SqlCommand com = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_Release_Float set ActionCode = 2, Float_Flag = 1, DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where release_float_id = '" + item + "'", con);
                            con.Open();
                            com.ExecuteNonQuery();
                        }


                    idx++;

                }

                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public void Float_Subsss()
        {
            
            

                List<Monthly_SubInc> ok2 = new List<Monthly_SubInc>();
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select release_float_id from IFMIS.dbo.tbl_R_BMS_SubsidyRelease_Float  where release_float_id = '" + release_float_id_float + "' and ActionCode in (1,2)", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();


                    while (reader.Read())
                    {
                        Monthly_SubInc Float = new Monthly_SubInc();
                        Float.release_float_id = reader.GetInt64(0);
                       
                        ok2.Add(Float);

                        release_Ay = Float.release_float_id;
                                              
                    }
                }
             
                if (release_Ay == 0)
                {
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com3 = new SqlCommand("insert into  fmis.dbo.tblBMS_SubsidyRelease_Float (FMISOfficeCode ,Amount ,MonthOf ,YearOf ,UserID ,DateTimeEntered ,ActionCode ,Batch) " +
                                                        "values ('" + FMISOfficeCode_float + "' ,'" + Amount_float + "' ,'" + MonthOf_float + "' ,'" + YearOf_float + "' ,'" + Account.UserInfo.eid.ToString() + "' , '" + DateTimeEntered_float + "' ,2 ,'" + batch_float + "')", con);
                        con.Open();
                        com3.ExecuteNonQuery();

                        con.Close();

                        SqlCommand com = new SqlCommand("insert into  IFMIS.dbo.tbl_R_BMS_SubsidyRelease_Float (FMISOfficeCode ,Amount ,MonthOf ,YearOf ,UserID ,DateTimeEntered ,ActionCode ,Batch ,Float_Flag,release_float_id) " +
                                                        "values ('" + FMISOfficeCode_float + "' ,'" + Amount_float + "' ,'" + MonthOf_float + "' ,'" + YearOf_float + "' ,'" + Account.UserInfo.eid.ToString() + "' , '" + DateTimeEntered_float + "' ,2 ,'" + batch_float + "' ,1,'" + release_float_id_float + "')", con);
                        con.Open();
                        com.ExecuteNonQuery();

                    }

                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com4 = new SqlCommand(@"insert into fmis.dbo.tblBMS_SubsidyRelease (FMISOfficeCode ,Amount ,MonthOf ,YearOf ,UserID ,DateTimeEntered ,ActionCode ,Batch) " +
                                                        " values ('" + FMISOfficeCode_float + "' ,'" + Amount_float + "' ,'" + MonthOf_float + "' ,'" + YearOf_float + "' ,'" + Account.UserInfo.eid.ToString() + "' ,'" + DateTimeEntered_float + "' ,'1' ,'" + batch_float + "')", con);
                        con.Open();
                        com4.ExecuteNonQuery();

                        con.Close();

                        SqlCommand com = new SqlCommand(@"insert into IFMIS.dbo.tbl_R_BMS_SubsidyRelease (FMISOfficeCode ,Amount ,MonthOf ,YearOf ,UserID ,DateTimeEntered ,ActionCode ,Batch,release_float_id) " +
                                                         " values ('" + FMISOfficeCode_float + "' ,'" + Amount_float + "' ,'" + MonthOf_float + "' ,'" + YearOf_float + "' ,'" + Account.UserInfo.eid.ToString() + "' ,'" + DateTimeEntered_float + "' ,'1' ,'" + batch_float + "','" + release_float_id_float + "')", con);
                        con.Open();
                        com.ExecuteNonQuery();
                       
                    }
                }else
                {
                    release_Ay = 0;

                }
                
                
            
             
        }



        public string FloatDisplay_Inc(string[] release_float_id)
        {

            try
            {
                var idx = 0;

                foreach (var item in release_float_id)
                {


                    List<Monthly_SubInc> ok2 = new List<Monthly_SubInc>();
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@" select release_float_id,FMISOfficeCode ,AmountPS , AmountMOOE , AmountCO ,MonthOf ,YearOf ,DateTimeEntered , Batch FROM IFMIS.dbo.tbl_R_BMS_Release_Float where release_float_id = '" + item + "' and WithSubsidyFlag = 1", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();


                        while (reader.Read())
                        {
                            Monthly_SubInc Float = new Monthly_SubInc();
                            Float.release_float_id = reader.GetInt64(0);
                            Float.FMISOfficeCode = reader.GetInt32(1);
                            Float.AmountPS = Convert.ToDouble(reader.GetValue(2));
                            Float.AmountMOOE = Convert.ToDouble(reader.GetValue(3));
                            Float.AmountCO = Convert.ToDouble(reader.GetValue(4));
                            Float.MonthOf = reader.GetInt32(5);
                            Float.YearOf = reader.GetInt32(6);
                            Float.MonthOf_D = reader.GetValue(7).ToString();
                            Float.Batch = reader.GetInt32(8);
                            ok2.Add(Float);

                            release_float_id_float = Float.release_float_id;
                            FMISOfficeCode_float = Float.FMISOfficeCode;
                            Amount_float = Float.AmountPS + Float.AmountMOOE + Float.AmountCO;
                            MonthOf_float = Float.MonthOf;
                            YearOf_float = Float.YearOf;
                            DateTimeEntered_float = Float.MonthOf_D;
                            batch_float = Float.Batch;

                            Float_incccc();



                        }
                    }
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com4 = new SqlCommand("insert into fmis.dbo.tblBMS_Releases (UserID ,DateTimeEntered ,ActionCode ,FMISOfficeCode ,FMISProgramCode ,AmountPS ,AmountMOOE ,AmountCO ,MonthOf ,YearOf ,DateReleased ,FMISAccountCode ,BalancePS ,BalanceMOOE ,BalanceCO ,Batch ,WithSubsidyFlag,ProjectFlag,OtherFund,PerAccountFlag) " +
                                 "SELECT UserID ,DateTimeEntered ,ActionCode ,FMISOfficeCode ,FMISProgramCode ,AmountPS ,AmountMOOE ,AmountCO ,MonthOf ,YearOf ,DateReleased ,FMISAccountCode ,BalancePS ,BalanceMOOE ,BalanceCO ,Batch , WithSubsidyFlag,0,'No',1 FROM IFMIS.dbo.tbl_R_BMS_Release_Float where release_float_id = '" + item + "' and ActionCode = 1", con);
                        con.Open();
                        com4.ExecuteNonQuery();

                        con.Close();
                        SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_Release (UserID ,DateTimeEntered ,ActionCode ,FMISOfficeCode ,FMISProgramCode ,AmountPS ,AmountMOOE ,AmountCO ,MonthOf ,YearOf ,DateReleased ,FMISAccountCode ,BalancePS ,BalanceMOOE ,BalanceCO ,Batch ,WithSubsidyFlag,release_float_id)  " +
                                                        "SELECT UserID ,DateTimeEntered ,ActionCode ,FMISOfficeCode ,FMISProgramCode ,AmountPS ,AmountMOOE ,AmountCO ,MonthOf ,YearOf ,DateReleased ,FMISAccountCode ,BalancePS ,BalanceMOOE ,BalanceCO ,Batch , WithSubsidyFlag,release_float_id FROM IFMIS.dbo.tbl_R_BMS_Release_Float where release_float_id = '" + item + "' and ActionCode = 1", con);
                        con.Open();
                        com.ExecuteNonQuery();
                    }
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com47 = new SqlCommand(@"SELECT UserID ,DateTimeEntered ,FMISOfficeCode ,FMISProgramCode ,AmountPS ,AmountMOOE ,AmountCO ,MonthOf ,YearOf ,DateReleased ,FMISAccountCode ,BalancePS ,BalanceMOOE ,BalanceCO ,Batch , WithSubsidyFlag FROM IFMIS.dbo.tbl_R_BMS_Release_Float where release_float_id = '" + item + "' and ActionCode = 1", con);
                        con.Open();
                        SqlDataReader reader7 = com47.ExecuteReader();
                        while (reader7.Read())
                        {
                            Monthly_Supplement_Model edit_list = new Monthly_Supplement_Model();
                            edit_list.UserID = reader7.GetValue(0).ToString();
                            edit_list.DateTimeEntered = reader7.GetValue(1) is DBNull ? "" : reader7.GetValue(1).ToString();
                            edit_list.FMISOfficeCode = reader7.GetValue(2) is DBNull ? 0 : reader7.GetInt32(2);
                            edit_list.FMISProgramCode = reader7.GetValue(3) is DBNull ? 0 : reader7.GetInt32(3);
                            edit_list.AmountPS = reader7.GetValue(4) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(4));
                            edit_list.AmountMOOE = reader7.GetValue(5) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(5));
                            edit_list.AmountCO = reader7.GetValue(6) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(6));
                            edit_list.MonthOf = reader7.GetValue(7) is DBNull ? 0 : reader7.GetInt32(7);
                            edit_list.YearOf = reader7.GetValue(8) is DBNull ? 0 : reader7.GetInt32(8);
                            edit_list.DateReleased = reader7.GetValue(9).ToString();
                            edit_list.FMISAccountCode = reader7.GetValue(10) is DBNull ? 0 : reader7.GetInt32(10);
                            edit_list.BalancePS = reader7.GetValue(11) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(11));
                            edit_list.BalanceMOOE = reader7.GetValue(12) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(12));
                            edit_list.BalanceCO = reader7.GetValue(13) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(13));
                            edit_list.Batch = reader7.GetValue(14) is DBNull ? 0 : reader7.GetInt32(14);
                            edit_list.WithSubsidyFlag = reader7.GetValue(15) is DBNull ? 0 : reader7.GetInt32(15);


                            UserID_FLOAT = edit_list.UserID;
                            DateTimeEntered_FLOAT = edit_list.DateTimeEntered;
                            FMISOfficeCode_FLOAT = edit_list.FMISOfficeCode;
                            FMISProgramCode_FLOAT = edit_list.FMISProgramCode;
                            AmountPS_FLOAT = edit_list.AmountPS;
                            AmountMOOE_FLOAT = edit_list.AmountMOOE;
                            AmountCO_FLOAT = edit_list.AmountCO;
                            MonthOf_FLOAT = edit_list.MonthOf;
                            YearOf_FLOAT = edit_list.YearOf;
                            DateReleased_FLOAT = edit_list.DateReleased;
                            FMISAccountCode_FLOAT = edit_list.FMISAccountCode;
                            BalancePS_FLOAT = edit_list.BalancePS;
                            BalanceMOOE_FLOAT = edit_list.BalanceMOOE;
                            BalanceCO_FLOAT = edit_list.BalanceCO;
                            Batch_FLOAT = edit_list.Batch;
                            WithSubsidyFlag_FLOAT = edit_list.WithSubsidyFlag;



                        }

                    }
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com4 = new SqlCommand(@"update fmis.dbo.tblBMS_Releases_Float set ActionCode = 2 where UserID = '" + UserID_FLOAT + "' and DateTimeEntered = '" + DateTimeEntered_FLOAT + "' and FMISOfficeCode = '" + FMISOfficeCode_FLOAT + "' " +
                                     "and FMISProgramCode = '" + FMISProgramCode_FLOAT + "' and AmountPS = '" + AmountPS_FLOAT + "' and AmountMOOE = '" + AmountMOOE_FLOAT + "' and AmountCO = '" + AmountCO_FLOAT + "' " +
                                     "and MonthOf = '" + MonthOf_FLOAT + "' and YearOf = '" + YearOf_FLOAT + "' and DateReleased = '" + DateReleased_FLOAT + "' and FMISAccountCode = '" + FMISAccountCode_FLOAT + "' " +
                                     "and BalancePS = '" + BalancePS_FLOAT + "' and BalanceMOOE = '" + BalanceMOOE_FLOAT + "' and BalanceCO = '" + BalanceCO_FLOAT + "' and WithSubsidyFlag = '" + WithSubsidyFlag_FLOAT + "' and Batch = '" + Batch_FLOAT + "'", con);
                        con.Open();
                        com4.ExecuteNonQuery();
                        con.Close();

                        SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_Release_Float set ActionCode = 2, Float_Flag = 1, DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where release_float_id = '" + item + "'", con);
                        con.Open();
                        com.ExecuteNonQuery();
                    }

                    idx++;

                }

                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public void Float_incccc()
        {
            

                List<Monthly_SubInc> ok2 = new List<Monthly_SubInc>();
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@" select release_float_id from IFMIS.dbo.tbl_R_BMS_IncomeRelease_Float  where release_float_id = '" + release_float_id_float + "' and ActionCode in (1,2)", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();


                    while (reader.Read())
                    {
                        Monthly_SubInc Float = new Monthly_SubInc();
                        Float.release_float_id = reader.GetInt64(0);

                        ok2.Add(Float);

                        release_Ay = Float.release_float_id;

                    }
                }
         if (release_Ay == 0)
                {

                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {

                        SqlCommand com4 = new SqlCommand("insert into  fmis.dbo.tblBMS_IncomeRelease_Float (FMISOfficeCode ,Amount ,MonthOf ,YearOf ,UserID ,DateTimeEntered ,ActionCode ,Batch) " +
                                                        "values ('" + FMISOfficeCode_float + "' ,'" + Amount_float + "' ,'" + MonthOf_float + "' ,'" + YearOf_float + "' ,'" + Account.UserInfo.eid.ToString() + "' , GETDATE() ,2 ,'" + batch_float + "')", con);
                        con.Open();
                        com4.ExecuteNonQuery();

                        con.Close();

                        SqlCommand com = new SqlCommand("insert into  IFMIS.dbo.tbl_R_BMS_IncomeRelease_Float (FMISOfficeCode ,Amount ,MonthOf ,YearOf ,UserID ,DateTimeEntered ,ActionCode ,Batch ,Float_Flag,release_float_id) " +
                                                        "values ('" + FMISOfficeCode_float + "' ,'" + Amount_float + "' ,'" + MonthOf_float + "' ,'" + YearOf_float + "' ,'" + Account.UserInfo.eid.ToString() + "' , GETDATE() ,2 ,'" + batch_float + "' ,1,'" + release_float_id_float + "')", con);
                        con.Open();
                        com.ExecuteNonQuery();

                    }
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com4 = new SqlCommand(@"insert into fmis.dbo.tblBMS_IncomeRelease (FMISOfficeCode ,Amount ,MonthOf ,YearOf ,UserID ,DateTimeEntered ,ActionCode ,Batch) " +
                                                         " values ('" + FMISOfficeCode_float + "' ,'" + Amount_float + "' ,'" + MonthOf_float + "' ,'" + YearOf_float + "' ,'" + Account.UserInfo.eid.ToString() + "' ,format(GetDate(),'M/d/yyy hh:mm:ss tt') ,'1' ,'" + batch_float + "')", con);
                        con.Open();
                        com4.ExecuteNonQuery();
                        con.Close();
                        SqlCommand com = new SqlCommand(@"insert into IFMIS.dbo.tbl_R_BMS_IncomeRelease (FMISOfficeCode ,Amount ,MonthOf ,YearOf ,UserID ,DateTimeEntered ,ActionCode ,Batch,release_float_id) " +
                                                         " values ('" + FMISOfficeCode_float + "' ,'" + Amount_float + "' ,'" + MonthOf_float + "' ,'" + YearOf_float + "' ,'" + Account.UserInfo.eid.ToString() + "' ,format(GetDate(),'M/d/yyy hh:mm:ss tt') ,'1' ,'" + batch_float + "','" + release_float_id_float + "')", con);
                        con.Open();
                        com.ExecuteNonQuery();
                       
                    }
                }
                else
                {
                    release_Ay = 0;       
                }
           
        }

        double Sub_Total = 0;
        public string save_Floats_Sub(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_, double release_ps, double release_mooe, double release_co, double remainPS, double remainMOOE, double remainCO, int? indicator_, string date_, double subsidy_Available)
        {
            try
            {
                Sub_Total = release_ps + release_mooe + release_co;


                if (subsidy_Available < Sub_Total)
                {

                    return "5";
                }
                else { 
              
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com3 = new SqlCommand("  insert into fmis.dbo.tblBMS_Releases_Float (UserID,DateTimeEntered,ActionCode,FMISOfficeCode,FMISProgramCode,AmountPS,AmountMOOE,AmountCO,MonthOf " +
                                                      " ,YearOf,DateReleased,FMISAccountCode,BalancePS,BalanceMOOE,BalanceCO,Batch,WithSubsidyFlag,ProjectFlag,OtherFund,PerAccountFlag) values ('" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),1,'" + office_id + "','" + program_id + "' " +
                                                      " ,'" + release_ps + "','" + release_mooe + "','" + release_co + "','" + month_ + "','" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),'" + account_id + "','" + remainPS + "','" + remainMOOE + "','" + remainCO + "','" + numeric_ + "','" + indicator_ + "',0,'No',1)", con);
                        con.Open();
                        com3.ExecuteNonQuery();
                        con.Close();

                        SqlCommand com = new SqlCommand("  insert into IFMIS.dbo.tbl_R_BMS_Release_Float (UserID,DateTimeEntered,ActionCode,FMISOfficeCode,FMISProgramCode,AmountPS,AmountMOOE,AmountCO,MonthOf " +
                                                     " ,YearOf,DateReleased,FMISAccountCode,BalancePS,BalanceMOOE,BalanceCO,Batch,Float_Flag,WithSubsidyFlag,PerAccountFlag) values ('" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),1,'" + office_id + "','" + program_id + "' " +
                                                     " ,'" + release_ps + "','" + release_mooe + "','" + release_co + "','" + month_ + "','" + year_ + "','" + year_ + "','" + account_id + "','" + remainPS + "','" + remainMOOE + "','" + remainCO + "','" + numeric_ + "',0,0,1)", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        return "1";
                       

                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        double Inc_Total = 0;
        public string save_Floats_Inc(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_, double release_ps, double release_mooe, double release_co, double remainPS, double remainMOOE, double remainCO, int? indicator_, string date_, double income_Available)
        {
            try
            {
                Inc_Total = release_ps + release_mooe + release_co;


                if (income_Available < Inc_Total)
                {

                    return "5";
                }
                else { 



                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com3 = new SqlCommand("  insert into fmis.dbo.tblBMS_Releases_Float (UserID,DateTimeEntered,ActionCode,FMISOfficeCode,FMISProgramCode,AmountPS,AmountMOOE,AmountCO,MonthOf " +
                                                      " ,YearOf,DateReleased,FMISAccountCode,BalancePS,BalanceMOOE,BalanceCO,Batch,WithSubsidyFlag,ProjectFlag,OtherFund,PerAccountFlag) values ('" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),1,'" + office_id + "','" + program_id + "' " +
                                                      " ,'" + release_ps + "','" + release_mooe + "','" + release_co + "','" + month_ + "','" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),'" + account_id + "','" + remainPS + "','" + remainMOOE + "','" + remainCO + "','" + numeric_ + "','" + indicator_ + "',0,'No',1)", con);
                        con.Open();
                        com3.ExecuteNonQuery();
                        con.Close();
                        SqlCommand com = new SqlCommand("  insert into IFMIS.dbo.tbl_R_BMS_Release_Float (UserID,DateTimeEntered,ActionCode,FMISOfficeCode,FMISProgramCode,AmountPS,AmountMOOE,AmountCO,MonthOf " +
                                                     " ,YearOf,DateReleased,FMISAccountCode,BalancePS,BalanceMOOE,BalanceCO,Batch,Float_Flag,WithSubsidyFlag,PerAccountFlag) values ('" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),1,'" + office_id + "','" + program_id + "' " +
                                                     " ,'" + release_ps + "','" + release_mooe + "','" + release_co + "','" + month_ + "','" + year_ + "','" + year_ + "','" + account_id + "','" + remainPS + "','" + remainMOOE + "','" + remainCO + "','" + numeric_ + "',0,1,1)", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        return "1";

                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public IEnumerable<Monthly_SubInc> Float_Grid_Sub(int? office_id, int? ooe_id, int? month_, int? numeric_, int? year_)
        {
            List<Monthly_SubInc> Float_List = new List<Monthly_SubInc>();
            if (ooe_id == 1) { 

           



            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT a.release_float_id,c.AccountName,(a.AmountPS + a.AmountMOOE + a.AmountCO )as Amount,DateName( month , DateAdd( month , a.MonthOf , 0 ) - 1 ) as MonthOf ,a.YearOf ,(a.BalancePS + a.BalanceMOOE + a.BalanceCO) as Balance ,a.Batch ,a.Float_Flag
                                                     FROM IFMIS.dbo.tbl_R_BMS_Release_Float as a
                                                     left join IFMIS.dbo.tbl_R_BMSProgramAccounts as c on a.FMISAccountCode = c.AccountID and a.FMISProgramCode = c.ProgramID 
                                                     where a.WithSubsidyFlag = 0 and  a.FmiSofficeCode = '" + office_id + "' and a.YearOf = '" + year_ + "' and a.ActionCode = 1  and Float_Flag = 0 " +
                                                     "GROUP BY a.release_float_id,a.MonthOf,a.YearOf,a.Batch,a.FMISOfficeCode ,a.FMISProgramCode , a.FMISAccountCode,c.AccountName,a.Float_Flag,a.AmountPS,a.AmountMOOE,a.AmountCO ,a.BalancePS,a.BalanceMOOE,a.BalanceCO", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_SubInc Float = new Monthly_SubInc();


                    Float.release_float_id = reader.GetInt64(0);
                    Float.amount_description = reader.GetValue(1).ToString();
                    Float.Amount = Convert.ToDouble(reader.GetValue(2));
                    Float.MonthOf_D = reader.GetValue(3).ToString();
                    Float.YearOf = reader.GetInt32(4);
                    Float.balance_amount = Convert.ToDouble(reader.GetValue(5));
                    Float.Batch = reader.GetInt32(6);
                    Float.Float_Flag = reader.GetInt32(7);

                    Float_List.Add(Float);

                }
            }
                
          


            }
            else if (ooe_id == 2) {
                

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"SELECT a.release_float_id,c.AccountName,(a.AmountPS + a.AmountMOOE + a.AmountCO ) as Amount,DateName( month , DateAdd( month , a.MonthOf , 0 ) - 1 ) as MonthOf ,a.YearOf ,(a.BalancePS + a.BalanceMOOE + a.BalanceCO) as Balance ,a.Batch ,a.Float_Flag
                                                     FROM IFMIS.dbo.tbl_R_BMS_Release_Float as a
                                                     left join IFMIS.dbo.tbl_R_BMSProgramAccounts as c on a.FMISAccountCode = c.AccountID and a.FMISProgramCode = c.ProgramID 
                                                     where a.WithSubsidyFlag = 0 and  a.FmiSofficeCode = '" + office_id + "' and a.YearOf = '" + year_ + "' and a.ActionCode = 1  and Float_Flag = 0 " +
                                                     "GROUP BY a.release_float_id,a.MonthOf,a.YearOf,a.Batch,a.FMISOfficeCode ,a.FMISProgramCode , a.FMISAccountCode,c.AccountName,a.Float_Flag,a.AmountPS,a.AmountMOOE,a.AmountCO ,a.BalancePS,a.BalanceMOOE,a.BalanceCO", con);


                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Monthly_SubInc Float = new Monthly_SubInc();


                        Float.release_float_id = reader.GetInt64(0);
                        Float.amount_description = reader.GetValue(1).ToString();
                        Float.Amount = Convert.ToDouble(reader.GetValue(2));
                        Float.MonthOf_D = reader.GetValue(3).ToString();
                        Float.YearOf = reader.GetInt32(4);
                        Float.balance_amount = Convert.ToDouble(reader.GetValue(5));
                        Float.Batch = reader.GetInt32(6);
                        Float.Float_Flag = reader.GetInt32(7);

                        Float_List.Add(Float);

                    }
                }

              

            }
            else if (ooe_id == 3)
            {
              

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"SELECT a.release_float_id,c.AccountName,(a.AmountPS + a.AmountMOOE + a.AmountCO ) as Amount,DateName( month , DateAdd( month , a.MonthOf , 0 ) - 1 ) as MonthOf ,a.YearOf ,(a.BalancePS + a.BalanceMOOE + a.BalanceCO) as Balance ,a.Batch ,a.Float_Flag
                                                     FROM IFMIS.dbo.tbl_R_BMS_Release_Float as a
                                                     left join IFMIS.dbo.tbl_R_BMSProgramAccounts as c on a.FMISAccountCode = c.AccountID and a.FMISProgramCode = c.ProgramID 
                                                     where a.WithSubsidyFlag = 0 and  a.FmiSofficeCode = '" + office_id + "' and a.YearOf = '" + year_ + "' and a.ActionCode = 1  and Float_Flag = 0 " +
                                                     "GROUP BY a.release_float_id,a.MonthOf,a.YearOf,a.Batch,a.FMISOfficeCode ,a.FMISProgramCode , a.FMISAccountCode,c.AccountName,a.Float_Flag,a.AmountPS,a.AmountMOOE,a.AmountCO ,a.BalancePS,a.BalanceMOOE,a.BalanceCO", con);


                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Monthly_SubInc Float = new Monthly_SubInc();


                        Float.release_float_id = reader.GetInt64(0);
                        Float.amount_description = reader.GetValue(1).ToString();
                        Float.Amount = Convert.ToDouble(reader.GetValue(2));
                        Float.MonthOf_D = reader.GetValue(3).ToString();
                        Float.YearOf = reader.GetInt32(4);
                        Float.balance_amount = Convert.ToDouble(reader.GetValue(5));
                        Float.Batch = reader.GetInt32(6);
                        Float.Float_Flag = reader.GetInt32(7);

                        Float_List.Add(Float);

                    }
                }

               

            }


            return Float_List;

        }

        public IEnumerable<Monthly_SubInc> Float_Grid_Inc(int? office_id, int? ooe_id, int? month_, int? numeric_, int? year_)
        {
            List<Monthly_SubInc> Float_List = new List<Monthly_SubInc>();
            if (ooe_id == 1)
            {



                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"SELECT a.release_float_id,c.AccountName,(a.AmountPS + a.AmountMOOE + a.AmountCO ) as Amount ,DateName( month , DateAdd( month , a.MonthOf , 0 ) - 1 ) as MonthOf ,a.YearOf ,(a.BalancePS + a.BalanceMOOE + a.BalanceCO) as Balance ,a.Batch ,a.Float_Flag
                                                     FROM IFMIS.dbo.tbl_R_BMS_Release_Float as a
                                                     left join IFMIS.dbo.tbl_R_BMSProgramAccounts as c on a.FMISAccountCode = c.AccountID and a.FMISProgramCode = c.ProgramID and c.AccountYear=a.YearOf
                                                     where a.WithSubsidyFlag = 1 and  a.FmiSofficeCode = '" + office_id + "' and a.YearOf = '" + year_ + "' and a.ActionCode = 1  and Float_Flag = 0 " +
                                                     "GROUP BY a.release_float_id,a.MonthOf,a.YearOf,a.Batch,a.FMISOfficeCode ,a.FMISProgramCode , a.FMISAccountCode,c.AccountName,a.Float_Flag,a.AmountPS,a.AmountMOOE,a.AmountCO ,a.BalancePS,a.BalanceMOOE,a.BalanceCO", con);


                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Monthly_SubInc Float = new Monthly_SubInc();


                        Float.release_float_id = reader.GetInt64(0);
                        Float.amount_description = reader.GetValue(1).ToString();
                        Float.Amount = Convert.ToDouble(reader.GetValue(2));
                        Float.MonthOf_D = reader.GetValue(3).ToString();
                        Float.YearOf = reader.GetInt32(4);
                        Float.balance_amount = Convert.ToDouble(reader.GetValue(5));
                        Float.Batch = reader.GetInt32(6);
                        Float.Float_Flag = reader.GetInt32(7);

                        Float_List.Add(Float);

                    }
                }




            }
            else if (ooe_id == 2)
            {


                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"SELECT a.release_float_id,c.AccountName,(a.AmountPS + a.AmountMOOE + a.AmountCO ) as Amount ,DateName( month , DateAdd( month , a.MonthOf , 0 ) - 1 ) as MonthOf ,a.YearOf ,(a.BalancePS + a.BalanceMOOE + a.BalanceCO) as Balance ,a.Batch ,a.Float_Flag
                                                     FROM IFMIS.dbo.tbl_R_BMS_Release_Float as a
                                                     left join IFMIS.dbo.tbl_R_BMSProgramAccounts as c on a.FMISAccountCode = c.AccountID and a.FMISProgramCode = c.ProgramID and c.AccountYear=a.YearOf
                                                     where a.WithSubsidyFlag = 1 and  a.FmiSofficeCode = '" + office_id + "' and a.YearOf = '" + year_ + "' and a.ActionCode = 1  and Float_Flag = 0 " +
                                                     "GROUP BY a.release_float_id,a.MonthOf,a.YearOf,a.Batch,a.FMISOfficeCode ,a.FMISProgramCode , a.FMISAccountCode,c.AccountName,a.Float_Flag,a.AmountPS,a.AmountMOOE,a.AmountCO ,a.BalancePS,a.BalanceMOOE,a.BalanceCO", con);


                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Monthly_SubInc Float = new Monthly_SubInc();


                        Float.release_float_id = reader.GetInt64(0);
                        Float.amount_description = reader.GetValue(1).ToString();
                        Float.Amount = Convert.ToDouble(reader.GetValue(2));
                        Float.MonthOf_D = reader.GetValue(3).ToString();
                        Float.YearOf = reader.GetInt32(4);
                        Float.balance_amount = Convert.ToDouble(reader.GetValue(5));
                        Float.Batch = reader.GetInt32(6);
                        Float.Float_Flag = reader.GetInt32(7);

                        Float_List.Add(Float);

                    }
                }



            }
            else if (ooe_id == 3)
            {


                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"SELECT a.release_float_id,c.AccountName,(a.AmountPS + a.AmountMOOE + a.AmountCO ) as Amount ,DateName( month , DateAdd( month , a.MonthOf , 0 ) - 1 ) as MonthOf ,a.YearOf ,(a.BalancePS + a.BalanceMOOE + a.BalanceCO) as Balance ,a.Batch ,a.Float_Flag
                                                     FROM IFMIS.dbo.tbl_R_BMS_Release_Float as a
                                                     left join IFMIS.dbo.tbl_R_BMSProgramAccounts as c on a.FMISAccountCode = c.AccountID and a.FMISProgramCode = c.ProgramID and c.AccountYear=a.YearOf
                                                     where a.WithSubsidyFlag = 1 and  a.FmiSofficeCode = '" + office_id + "' and a.YearOf = '" + year_ + "' and a.ActionCode = 1  and Float_Flag = 0 " +
                                                     "GROUP BY a.release_float_id,a.MonthOf,a.YearOf,a.Batch,a.FMISOfficeCode ,a.FMISProgramCode , a.FMISAccountCode,c.AccountName,a.Float_Flag,a.AmountPS,a.AmountMOOE,a.AmountCO ,a.BalancePS,a.BalanceMOOE,a.BalanceCO", con);


                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Monthly_SubInc Float = new Monthly_SubInc();


                        Float.release_float_id = reader.GetInt64(0);
                        Float.amount_description = reader.GetValue(1).ToString();
                        Float.Amount = Convert.ToDouble(reader.GetValue(2));
                        Float.MonthOf_D = reader.GetValue(3).ToString();
                        Float.YearOf = reader.GetInt32(4);
                        Float.balance_amount = Convert.ToDouble(reader.GetValue(5));
                        Float.Batch = reader.GetInt32(6);
                        Float.Float_Flag = reader.GetInt32(7);

                        Float_List.Add(Float);

                    }
                }



            }


            return Float_List;
        }

        public string Available_Sub(int? office_id, int? year_)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"sp_MonthlyRelease_Sub_Available '" + office_id + "','" + year_ + "'", con);
                
                con.Open();
                return com.ExecuteScalar().ToString();


            }
        }
        public string Available_Inc(int? office_id, int? year_)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"sp_MonthlyRelease_Inc_Available '" + office_id + "','" + year_ + "'", con);

                con.Open();
                return com.ExecuteScalar().ToString();


            }
        }

        public IEnumerable<Monthly_DD_Model> Account_DP(int? propYear, int? prog_id)
        {
            List<Monthly_DD_Model> account_list = new List<Monthly_DD_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT distinct AccountID,AccountName FROM IFMIS.dbo.tbl_R_BMSProgramAccounts where AccountYear = '" + propYear + "' and ProgramID = '" + prog_id + "' and ActionCode = 1 order by AccountID ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_DD_Model app = new Monthly_DD_Model();

                    app.account_id = reader.GetInt32(0);
                    app.account_name = reader.GetValue(1).ToString();

                    account_list.Add(app);
                }
            }
            return account_list;
        }

        public IEnumerable<Monthly_DD_Model> Account_Mos_From(int? office_ID_from, int? prog_id_from, int? year_, int? office_ID_to, int? prog_id_to,  int? account_id_to)
        {
            List<Monthly_DD_Model> account_list = new List<Monthly_DD_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_MonthlyRelease_Accounts '" + office_ID_from + "', '" + prog_id_from + "',  '" + year_ + "', '" + office_ID_to + "', '" + prog_id_to + "',  '" + account_id_to + "' ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_DD_Model app = new Monthly_DD_Model();

                    app.account_id = reader.GetInt32(0);
                    app.account_name = reader.GetValue(1).ToString();

                    account_list.Add(app);
                }
            }
            return account_list;
        }
        public IEnumerable<Monthly_DD_Model> Account_Mos_TO(int? office_ID_from, int? prog_id_from, int? account_id_from, int? year_, int? office_ID_to, int? prog_id_to)
        {
            List<Monthly_DD_Model> account_list = new List<Monthly_DD_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_MonthlyRelease_Accounts_To '" + office_ID_from + "', '" + prog_id_from + "', '" + account_id_from + "', '" + year_ + "', '" + office_ID_to + "', '" + prog_id_to + "' ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_DD_Model app = new Monthly_DD_Model();

                    app.account_id = reader.GetInt32(0);
                    app.account_name = reader.GetValue(1).ToString();

                    account_list.Add(app);
                }
            }
            return account_list;
        }

        public IEnumerable<Monthly_DD_Model> RealignAccount_To(int? office_ID_from, int? prog_id_from, int? account_id_from, int? year_, int? office_ID_to, int? prog_id_to, int? ooe_id_from)
        {
            List<Monthly_DD_Model> account_list = new List<Monthly_DD_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.[sp_MonthlyRelease_Realign_To] '" + office_ID_from + "', '" + prog_id_from + "', '" + account_id_from + "', '" + year_ + "', '" + office_ID_to + "', '" + prog_id_to + "',"+ Account.UserInfo.UserTypeID+","+ ooe_id_from + " ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_DD_Model app = new Monthly_DD_Model();

                    app.account_id = reader.GetInt32(0);
                    app.account_name = reader.GetValue(1).ToString();

                    account_list.Add(app);
                }
            }
            return account_list;
        }

        public IEnumerable<Monthly_DD_Model> ReversionAccount_To(int? office_ID_from, int? prog_id_from, int? account_id_from, int? year_, int? office_ID_to, int? prog_id_to)
        {
            List<Monthly_DD_Model> account_list = new List<Monthly_DD_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.[sp_MonthlyRelease_Reversion_To] '" + office_ID_from + "', '" + prog_id_from + "', '" + account_id_from + "', '" + year_ + "', '" + office_ID_to + "', '" + prog_id_to + "'," + Account.UserInfo.UserTypeID + " ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_DD_Model app = new Monthly_DD_Model();

                    app.account_id = reader.GetInt32(0);
                    app.account_name = reader.GetValue(1).ToString();

                    account_list.Add(app);
                }
            }
            return account_list;
        }
        public IEnumerable<MonthlyR_Model> Monthly_Release_EE(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_, int? type_desu)
        {
            List<MonthlyR_Model> MRsList_GF = new List<MonthlyR_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"sp_MonthlyRelease_view_EE '" + office_id + "','" + program_id + "','" + ooe_id + "','" + account_id + "','" + month_ + "','" + numeric_ + "','" + year_ + "','" + type_desu + "'", con);
               

                con.Open();
                com.CommandTimeout = 0;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    MonthlyR_Model Mos = new MonthlyR_Model();
                    Mos.monthly_id = reader.GetInt32(0);
                    Mos.subjects = reader.GetValue(1).ToString();
                    Mos.ps = Convert.ToDouble(reader.GetValue(2));
                    Mos.mooe = Convert.ToDouble(reader.GetValue(3));
                    Mos.co = Convert.ToDouble(reader.GetValue(4));
                    Mos.subsidy = Convert.ToDouble(reader.GetValue(5));
                    Mos.income = Convert.ToDouble(reader.GetValue(6));
                    Mos.type_desu = reader.GetInt32(7);

                    MRsList_GF.Add(Mos);
                  
                }
            }
            return MRsList_GF;
        }


        public string Release_EE(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_, double release_ps, double release_mooe, double release_co, double total_debayd, double total_debaydm, double total_debaydc)
        {
            try
            {
                List<Monthly_Realignment_Model> ok2 = new List<Monthly_Realignment_Model>();
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"SELECT release_float_id  FROM IFMIS.dbo.tbl_R_BMS_Release_Float WHERE FMISOfficeCode = '" + office_id + "' AND FMISProgramCode = '" + program_id + "' AND YearOf = '" + year_ + "' AND FMISAccountCode = '" + account_id + "' AND Float_Flag = 1 AND ActionCode = 1  and MonthOf = '" + month_ + "'", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();


                    while (reader.Read())
                    {
                        Monthly_Realignment_Model Float = new Monthly_Realignment_Model();
                        Float.release_float_id = reader.GetInt64(0);
                        ok2.Add(Float);


                        release_float_idsEE = Float.release_float_id;
                        release_cyaEE();



                    }
                    con.Close();



                }


                List<Monthly_SubInc> subsidy = new List<Monthly_SubInc>();
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"SELECT subsidyrelease_float_id  FROM IFMIS.dbo.tbl_R_BMS_SubsidyRelease_Float where FMISOfficeCode = '" + office_id + "' and YearOf = '" + year_ + "' and  and MonthOf = '" + month_ + "' and ActionCode = 1 and Float_Flag = 1", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();


                    while (reader.Read())
                    {
                        Monthly_SubInc Float = new Monthly_SubInc();
                        Float.subsidyrelease_float_id = reader.GetInt64(0);
                        subsidy.Add(Float);


                        subsidyrelease_float_ids = Float.subsidyrelease_float_id;
                        subsidy_float_T(office_id, program_id,  ooe_id, account_id, month_, numeric_, year_, release_ps, release_mooe, release_co, total_debayd, total_debaydm, total_debaydc);



                    }
                    con.Close();



                }

                List<Monthly_SubInc> income = new List<Monthly_SubInc>();
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"SELECT incomerelease_float_id FROM IFMIS.dbo.tbl_R_BMS_IncomeRelease_Float where FMISOfficeCode = '" + office_id + "' and YearOf = '" + year_ + "' and  and MonthOf = '" + month_ + "' and ActionCode = 1 and Float_Flag = 1", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();


                    while (reader.Read())
                    {
                        Monthly_SubInc Float = new Monthly_SubInc();
                        Float.incomerelease_float_id = reader.GetInt64(0);
                        income.Add(Float);


                        incomerelease_float_ids = Float.incomerelease_float_id;
                        income_float_T(office_id, program_id, ooe_id, account_id, month_, numeric_, year_, release_ps, release_mooe, release_co, total_debayd, total_debaydm, total_debaydc);



                    }
                    con.Close();



                }



                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {


                    SqlCommand com = new SqlCommand(@" update IFMIS.dbo.tbl_R_BMS_Release_Float set Float_Flag = 2, ActionCode = 2 ,DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')) , userID = CONCAT(userID,' , ', '" + Account.UserInfo.eid.ToString() + "') " +
                        " where ActionCode = 1 and FMISOfficeCode = '" + office_id + "' and FMISProgramCode = '" + program_id + "' and Float_Flag = 1 and YearOf = '" + year_ + "' and FMISAccountCode = '" + account_id + "' ", con);
                    con.Open();
                    com.ExecuteNonQuery();

                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com = new SqlCommand("  insert into tbl_R_BMS_Release (UserID,DateTimeEntered,ActionCode,FMISOfficeCode,FMISProgramCode,AmountPS,AmountMOOE,AmountCO,MonthOf,YearOf,DateReleased,FMISAccountCode,BalancePS,BalanceMOOE,BalanceCO,Batch) " +
                                            "values ('" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),1,'" + office_id + "','" + program_id + "','" + release_ps + "', " +
                                                      "'" + release_mooe + "','" + release_co + "','" + month_ + "','" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),'" + account_id + "','" + total_debayd + "','" + total_debaydm + "','" + total_debaydc + "','" + numeric_ + "')", con);
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

        double sub_av = 0;
        double inc_av = 0;


        private void income_float_T(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_, double release_ps, double release_mooe, double release_co, double total_debayd, double total_debaydm, double total_debaydc)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"insert into IFMIS.dbo.tbl_R_BMS_IncomeRelease (FMISOfficeCode ,Amount ,MonthOf ,YearOf ,UserID ,DateTimeEntered ,ActionCode ,Batch)
                                                SELECT FMISOfficeCode ,Amount ,MonthOf ,YearOf ,UserID ,DateTimeEntered ,ActionCode ,Batch  FROM IFMIS.dbo.tbl_R_BMS_IncomeRelease_Float where incomerelease_float_id = '" + incomerelease_float_ids + "' and ActionCode = 1 and Float_Flag = 1", con);
                con.Open();
                com.ExecuteNonQuery();

            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"", con);
                con.Open();
                com.ExecuteNonQuery();

            }

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_IncomeRelease_Float set ActionCode = 2, Float_Flag = 2, DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where incomerelease_float_id = '" + incomerelease_float_ids + "'", con);
                con.Open();
                com.ExecuteNonQuery();

            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com47 = new SqlCommand(@"sp_MonthlyRelease_Inc_Available '" + office_id + "','" + year_ + "'", con);
                con.Open();
                SqlDataReader reader7 = com47.ExecuteReader();
                while (reader7.Read())
                {
                    Monthly_SubInc subs_available = new Monthly_SubInc();
                    subs_available.inc_available = Convert.ToDouble(reader7.GetValue(0));


                    inc_av = subs_available.inc_available;


                }

            }
        }



        private void subsidy_float_T(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_, double release_ps, double release_mooe, double release_co, double total_debayd, double total_debaydm, double total_debaydc)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"insert into IFMIS.dbo.tbl_R_BMS_SubsidyRelease (FMISOfficeCode ,Amount ,MonthOf ,YearOf ,UserID ,DateTimeEntered ,ActionCode ,Batch)
                                                SELECT FMISOfficeCode ,Amount ,MonthOf ,YearOf ,UserID ,DateTimeEntered ,ActionCode ,Batch FROM IFMIS.dbo.tbl_R_BMS_SubsidyRelease_Float where subsidyrelease_float_id = '" + subsidyrelease_float_ids + "' and ActionCode = 1 and Float_Flag = 1", con);
                con.Open();
                com.ExecuteNonQuery();

            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_SubsidyRelease_Float set ActionCode = 2, Float_Flag = 2, DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where incomerelease_float_id = '" + subsidyrelease_float_ids + "'", con);
                con.Open();
                com.ExecuteNonQuery();

            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com47 = new SqlCommand(@"sp_MonthlyRelease_Sub_Available '" + office_id + "','" + year_ + "'", con);
                con.Open();
                SqlDataReader reader7 = com47.ExecuteReader();
                while (reader7.Read())
                {
                    Monthly_SubInc subs_available = new Monthly_SubInc();
                    subs_available.sub_available = Convert.ToDouble(reader7.GetValue(0));


                    sub_av = subs_available.sub_available;
                   

                }

            }
        }


        public string release_cyaEE()
        {
            try
            {
                using (SqlConnection con3 = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com2 = new SqlCommand("  insert into IFMIS.dbo.tbl_R_BMS_Release (UserID,DateTimeEntered,ActionCode,FMISOfficeCode,FMISProgramCode,AmountPS,AmountMOOE,AmountCO,MonthOf,YearOf,DateReleased,FMISAccountCode,BalancePS,BalanceMOOE,BalanceCO,Batch) " +
                                                        "select UserID,DateTimeEntered,ActionCode,FMISOfficeCode,FMISProgramCode,AmountPS,AmountMOOE,AmountCO,MonthOf,YearOf,DateReleased,FMISAccountCode,BalancePS,BalanceMOOE,BalanceCO,Batch FROM IFMIS.dbo.tbl_R_BMS_Release_Float where release_float_id = '" + release_float_idsEE + "'", con3);
                    con3.Open();
                    com2.ExecuteNonQuery();
                    
                }
                using (SqlConnection con3 = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com2 = new SqlCommand("  update IFMIS.dbo.tbl_R_BMS_Release set WithSubsidyFlag = 0 where release_float_id = '" + release_float_idsEE + "'", con3);
                    con3.Open();
                    com2.ExecuteNonQuery();
                    return "1";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public string delete_float_sub(int? release_float_id)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com47 = new SqlCommand(@"SELECT UserID ,DateTimeEntered ,FMISOfficeCode ,FMISProgramCode ,AmountPS ,AmountMOOE ,AmountCO ,MonthOf ,YearOf ,DateReleased ,FMISAccountCode ,BalancePS ,BalanceMOOE ,BalanceCO ,Batch , WithSubsidyFlag FROM IFMIS.dbo.tbl_R_BMS_Release_Float where release_float_id = '" + release_float_id + "' and ActionCode = 1", con);
                    con.Open();
                    SqlDataReader reader7 = com47.ExecuteReader();
                    while (reader7.Read())
                    {
                        Monthly_Supplement_Model edit_list = new Monthly_Supplement_Model();
                        edit_list.UserID = reader7.GetValue(0).ToString();
                        edit_list.DateTimeEntered = reader7.GetValue(1) is DBNull ? "" : reader7.GetValue(1).ToString();
                        edit_list.FMISOfficeCode = reader7.GetValue(2) is DBNull ? 0 : reader7.GetInt32(2);
                        edit_list.FMISProgramCode = reader7.GetValue(3) is DBNull ? 0 : reader7.GetInt32(3);
                        edit_list.AmountPS = reader7.GetValue(4) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(4));
                        edit_list.AmountMOOE = reader7.GetValue(5) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(5));
                        edit_list.AmountCO = reader7.GetValue(6) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(6));
                        edit_list.MonthOf = reader7.GetValue(7) is DBNull ? 0 : reader7.GetInt32(7);
                        edit_list.YearOf = reader7.GetValue(8) is DBNull ? 0 : reader7.GetInt32(8);
                        edit_list.DateReleased = reader7.GetValue(9).ToString();
                        edit_list.FMISAccountCode = reader7.GetValue(10) is DBNull ? 0 : reader7.GetInt32(10);
                        edit_list.BalancePS = reader7.GetValue(11) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(11));
                        edit_list.BalanceMOOE = reader7.GetValue(12) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(12));
                        edit_list.BalanceCO = reader7.GetValue(13) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(13));
                        edit_list.Batch = reader7.GetValue(14) is DBNull ? 0 : reader7.GetInt32(14);
                        edit_list.WithSubsidyFlag = reader7.GetValue(15) is DBNull ? 0 : reader7.GetInt32(15);


                        UserID_FLOAT = edit_list.UserID;
                        DateTimeEntered_FLOAT = edit_list.DateTimeEntered;
                        FMISOfficeCode_FLOAT = edit_list.FMISOfficeCode;
                        FMISProgramCode_FLOAT = edit_list.FMISProgramCode;
                        AmountPS_FLOAT = edit_list.AmountPS;
                        AmountMOOE_FLOAT = edit_list.AmountMOOE;
                        AmountCO_FLOAT = edit_list.AmountCO;
                        MonthOf_FLOAT = edit_list.MonthOf;
                        YearOf_FLOAT = edit_list.YearOf;
                        DateReleased_FLOAT = edit_list.DateReleased;
                        FMISAccountCode_FLOAT = edit_list.FMISAccountCode;
                        BalancePS_FLOAT = edit_list.BalancePS;
                        BalanceMOOE_FLOAT = edit_list.BalanceMOOE;
                        BalanceCO_FLOAT = edit_list.BalanceCO;
                        Batch_FLOAT = edit_list.Batch;
                        WithSubsidyFlag_FLOAT = edit_list.WithSubsidyFlag;

                     }

                }



                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com4 = new SqlCommand(@"update fmis.dbo.tblBMS_Releases_Float set ActionCode = 2 where UserID = '" + UserID_FLOAT + "' and DateTimeEntered = '" + DateTimeEntered_FLOAT + "' and FMISOfficeCode = '" + FMISOfficeCode_FLOAT + "' " +
                              "and FMISProgramCode = '" + FMISProgramCode_FLOAT + "' and AmountPS = '" + AmountPS_FLOAT + "' and AmountMOOE = '" + AmountMOOE_FLOAT + "' and AmountCO = '" + AmountCO_FLOAT + "' " +
                              "and MonthOf = '" + MonthOf_FLOAT + "' and YearOf = '" + YearOf_FLOAT + "' and DateReleased = '" + DateReleased_FLOAT + "' and FMISAccountCode = '" + FMISAccountCode_FLOAT + "' " +
                              "and BalancePS = '" + BalancePS_FLOAT + "' and BalanceMOOE = '" + BalanceMOOE_FLOAT + "' and BalanceCO = '" + BalanceCO_FLOAT + "' and WithSubsidyFlag = '" + WithSubsidyFlag_FLOAT + "' and Batch = '" + Batch_FLOAT + "'", con);
                    con.Open();
                    com4.ExecuteNonQuery();
                    con.Close();
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com4 = new SqlCommand(@"update fmis.dbo.tblBMS_Releases set ActionCode = 2 where DateTimeEntered = '" + DateTimeEntered_FLOAT + "' and FMISOfficeCode = '" + FMISOfficeCode_FLOAT + "' " +
                              "and FMISProgramCode = '" + FMISProgramCode_FLOAT + "' and AmountPS = '" + AmountPS_FLOAT + "' and AmountMOOE = '" + AmountMOOE_FLOAT + "' and AmountCO = '" + AmountCO_FLOAT + "' " +
                              "and MonthOf = '" + MonthOf_FLOAT + "' and YearOf = '" + YearOf_FLOAT + "' and DateReleased = '" + DateReleased_FLOAT + "' and FMISAccountCode = '" + FMISAccountCode_FLOAT + "' " +
                              "and BalancePS = '" + BalancePS_FLOAT + "' and BalanceMOOE = '" + BalanceMOOE_FLOAT + "' and BalanceCO = '" + BalanceCO_FLOAT + "' and WithSubsidyFlag = '" + WithSubsidyFlag_FLOAT + "' and Batch = '" + Batch_FLOAT + "'", con);
                    con.Open();
                    com4.ExecuteNonQuery();
                    con.Close();
                }



                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com4 = new SqlCommand(@"  update fmis.dbo.tblBMS_SubsidyRelease set ActionCode = 1 where FMISOfficeCode = '" + FMISOfficeCode_FLOAT + "' "
                                                        + " and MonthOf = '" + MonthOf_FLOAT + "' and YearOf = '" + YearOf_FLOAT + "' "
                                                        + " and DateTimeEntered = '" + DateReleased_FLOAT + "' and Batch = '" + Batch_FLOAT + "'", con);
                    con.Open();
                    com4.ExecuteNonQuery();
                    con.Close();
                }


                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com4 = new SqlCommand(@"  update fmis.dbo.tblBMS_SubsidyRelease_Float set ActionCode = 1 where FMISOfficeCode = '" + FMISOfficeCode_FLOAT + "' "
                                                        + " and MonthOf = '" + MonthOf_FLOAT + "' and YearOf = '" + YearOf_FLOAT + "' "
                                                        + " and DateTimeEntered = '" + DateReleased_FLOAT + "' and Batch = '" + Batch_FLOAT + "'", con);
                    con.Open();
                    com4.ExecuteNonQuery();
                    con.Close();
                }






                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_Release set ActionCode = 4, DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where release_float_id = '" + release_float_id + "'", con);
                    con.Open();
                    com.ExecuteNonQuery();

                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_SubsidyRelease_Float set ActionCode = 4, Float_Flag = '4', DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where release_float_id = '" + release_float_id + "'", con);
                    con.Open();
                    com.ExecuteNonQuery();

                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_SubsidyRelease set ActionCode = 4, DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where release_float_id = '" + release_float_id + "'", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    
                }
                  using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_Release_Float set ActionCode = 4, Float_Flag = 4, DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where release_float_id = '" + release_float_id + "'", con);
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

      


        public string delete_float_inc(int? release_float_id)
        {
            try
            {



                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com47 = new SqlCommand(@"SELECT UserID ,DateTimeEntered ,FMISOfficeCode ,FMISProgramCode ,AmountPS ,AmountMOOE ,AmountCO ,MonthOf ,YearOf ,DateReleased ,FMISAccountCode ,BalancePS ,BalanceMOOE ,BalanceCO ,Batch , WithSubsidyFlag FROM IFMIS.dbo.tbl_R_BMS_Release_Float where release_float_id = '" + release_float_id + "' and ActionCode = 1", con);
                    con.Open();
                    SqlDataReader reader7 = com47.ExecuteReader();
                    while (reader7.Read())
                    {
                        Monthly_Supplement_Model edit_list = new Monthly_Supplement_Model();
                        edit_list.UserID = reader7.GetValue(0).ToString();
                        edit_list.DateTimeEntered = reader7.GetValue(1) is DBNull ? "" : reader7.GetValue(1).ToString();
                        edit_list.FMISOfficeCode = reader7.GetValue(2) is DBNull ? 0 : reader7.GetInt32(2);
                        edit_list.FMISProgramCode = reader7.GetValue(3) is DBNull ? 0 : reader7.GetInt32(3);
                        edit_list.AmountPS = reader7.GetValue(4) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(4));
                        edit_list.AmountMOOE = reader7.GetValue(5) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(5));
                        edit_list.AmountCO = reader7.GetValue(6) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(6));
                        edit_list.MonthOf = reader7.GetValue(7) is DBNull ? 0 : reader7.GetInt32(7);
                        edit_list.YearOf = reader7.GetValue(8) is DBNull ? 0 : reader7.GetInt32(8);
                        edit_list.DateReleased = reader7.GetValue(9).ToString();
                        edit_list.FMISAccountCode = reader7.GetValue(10) is DBNull ? 0 : reader7.GetInt32(10);
                        edit_list.BalancePS = reader7.GetValue(11) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(11));
                        edit_list.BalanceMOOE = reader7.GetValue(12) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(12));
                        edit_list.BalanceCO = reader7.GetValue(13) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(13));
                        edit_list.Batch = reader7.GetValue(14) is DBNull ? 0 : reader7.GetInt32(14);
                        edit_list.WithSubsidyFlag = reader7.GetValue(15) is DBNull ? 0 : reader7.GetInt32(15);


                        UserID_FLOAT = edit_list.UserID;
                        DateTimeEntered_FLOAT = edit_list.DateTimeEntered;
                        FMISOfficeCode_FLOAT = edit_list.FMISOfficeCode;
                        FMISProgramCode_FLOAT = edit_list.FMISProgramCode;
                        AmountPS_FLOAT = edit_list.AmountPS;
                        AmountMOOE_FLOAT = edit_list.AmountMOOE;
                        AmountCO_FLOAT = edit_list.AmountCO;
                        MonthOf_FLOAT = edit_list.MonthOf;
                        YearOf_FLOAT = edit_list.YearOf;
                        DateReleased_FLOAT = edit_list.DateReleased;
                        FMISAccountCode_FLOAT = edit_list.FMISAccountCode;
                        BalancePS_FLOAT = edit_list.BalancePS;
                        BalanceMOOE_FLOAT = edit_list.BalanceMOOE;
                        BalanceCO_FLOAT = edit_list.BalanceCO;
                        Batch_FLOAT = edit_list.Batch;
                        WithSubsidyFlag_FLOAT = edit_list.WithSubsidyFlag;

                    }

                }



                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com4 = new SqlCommand(@"update fmis.dbo.tblBMS_Releases_Float set ActionCode = 2 where UserID = '" + UserID_FLOAT + "' and DateTimeEntered = '" + DateTimeEntered_FLOAT + "' and FMISOfficeCode = '" + FMISOfficeCode_FLOAT + "' " +
                              "and FMISProgramCode = '" + FMISProgramCode_FLOAT + "' and AmountPS = '" + AmountPS_FLOAT + "' and AmountMOOE = '" + AmountMOOE_FLOAT + "' and AmountCO = '" + AmountCO_FLOAT + "' " +
                              "and MonthOf = '" + MonthOf_FLOAT + "' and YearOf = '" + YearOf_FLOAT + "' and DateReleased = '" + DateReleased_FLOAT + "' and FMISAccountCode = '" + FMISAccountCode_FLOAT + "' " +
                              "and BalancePS = '" + BalancePS_FLOAT + "' and BalanceMOOE = '" + BalanceMOOE_FLOAT + "' and BalanceCO = '" + BalanceCO_FLOAT + "' and WithSubsidyFlag = '" + WithSubsidyFlag_FLOAT + "' and Batch = '" + Batch_FLOAT + "'", con);
                    con.Open();
                    com4.ExecuteNonQuery();
                    con.Close();
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com4 = new SqlCommand(@"update fmis.dbo.tblBMS_Releases set ActionCode = 2 where DateTimeEntered = '" + DateTimeEntered_FLOAT + "' and FMISOfficeCode = '" + FMISOfficeCode_FLOAT + "' " +
                              "and FMISProgramCode = '" + FMISProgramCode_FLOAT + "' and AmountPS = '" + AmountPS_FLOAT + "' and AmountMOOE = '" + AmountMOOE_FLOAT + "' and AmountCO = '" + AmountCO_FLOAT + "' " +
                              "and MonthOf = '" + MonthOf_FLOAT + "' and YearOf = '" + YearOf_FLOAT + "' and DateReleased = '" + DateReleased_FLOAT + "' and FMISAccountCode = '" + FMISAccountCode_FLOAT + "' " +
                              "and BalancePS = '" + BalancePS_FLOAT + "' and BalanceMOOE = '" + BalanceMOOE_FLOAT + "' and BalanceCO = '" + BalanceCO_FLOAT + "' and WithSubsidyFlag = '" + WithSubsidyFlag_FLOAT + "' and Batch = '" + Batch_FLOAT + "'", con);
                    con.Open();
                    com4.ExecuteNonQuery();
                    con.Close();
                }



                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com4 = new SqlCommand(@"  update fmis.dbo.tblBMS_IncomeRelease set ActionCode = 1 where FMISOfficeCode = '" + FMISOfficeCode_FLOAT + "' "
                                                        + " and MonthOf = '" + MonthOf_FLOAT + "' and YearOf = '" + YearOf_FLOAT + "' "
                                                        + " and DateTimeEntered = '" + DateReleased_FLOAT + "' and Batch = '" + Batch_FLOAT + "'", con);
                    con.Open();
                    com4.ExecuteNonQuery();
                    con.Close();
                }


                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com4 = new SqlCommand(@"  update fmis.dbo.tblBMS_IncomeRelease_Float set ActionCode = 1 where FMISOfficeCode = '" + FMISOfficeCode_FLOAT + "' "
                                                        + " and MonthOf = '" + MonthOf_FLOAT + "' and YearOf = '" + YearOf_FLOAT + "' "
                                                        + " and DateTimeEntered = '" + DateReleased_FLOAT + "' and Batch = '" + Batch_FLOAT + "'", con);
                    con.Open();
                    com4.ExecuteNonQuery();
                    con.Close();
                }





                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_Release set ActionCode = 4, DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where release_float_id = '" + release_float_id + "'", con);
                    con.Open();
                    com.ExecuteNonQuery();

                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_IncomeRelease_Float set ActionCode = 4, Float_Flag = '4', DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where release_float_id = '" + release_float_id + "'", con);
                    con.Open();
                    com.ExecuteNonQuery();

                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_IncomeRelease set ActionCode = 4, DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where release_float_id = '" + release_float_id + "'", con);
                    con.Open();
                    com.ExecuteNonQuery();
                   
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_Release_Float set ActionCode = 4, Float_Flag = 4, DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where release_float_id = '" + release_float_id + "'", con);
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

        public IEnumerable<Monthly_DD_Model> Supplement_Grid(int? LegalCode, int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? year_)
        {

            List<Monthly_DD_Model> Float_List = new List<Monthly_DD_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT a.supplementalbudget_id , b.LegalDescription,a.Amount ,DateName( month , DateAdd( month , a.MonthOf , 0 ) - 1 ) as MonthOf ,a.YearOf "+
                                                  " FROM IFMIS.dbo.tbl_R_BMS_SupplementalBudget as a inner join  IFMIS.dbo.tbl_R_BMS_LegalBasis as b "+
                                                  " on a.LegalCode = b.LegalCode and a.ActionCode = b.ActionCode and a.YearOf = b.YearOf where a.FMISOfficeCode = '" + office_id + "' and a.FMISProgramCode = '" + program_id + "' and a.OOECode = '" + ooe_id + "' " +
                                                  "  and a.FMISAccountCode = '" + account_id + "' and a.YearOf = '" + year_ + "' and a.ActionCode = 1", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_DD_Model Float = new Monthly_DD_Model();
                    Float.supplementalbudget_id = reader.GetInt64(0);
                    Float.LegalDescription = reader.GetValue(1).ToString();
                    Float.Amount = Convert.ToDouble(reader.GetValue(2));
                    Float.MonthOf = reader.GetValue(3).ToString();
                    Float.YearOf = reader.GetInt32(4);
                   

                    Float_List.Add(Float);

                }
            }
            return Float_List;
        }


        public IEnumerable<Monthly_Supplement_Model> Supplement_Grid_transf(int? LegalCode = 0, int? office_id = 0, int? program_id = 0, int? ooe_id = 0, int? account_id = 0, int? month_ = 0, int? year_ = 0)
        {

            List<Monthly_Supplement_Model> Float_List = new List<Monthly_Supplement_Model>();

            if (account_id != 0 && program_id !=0)
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@" select distinct a.supplementaltransfere_id,a.Amount ,DateName( month , DateAdd( month , a.MonthOf , 0 ) - 1 ) as MonthOf 
                                                                  ,a.YearOf,b.OfficeAbbrivation,c.AccountName,a.[DateTimeEntered] FROM IFMIS.dbo.tbl_R_BMS_SupplementalTransfere as a
                                                                  left join IFMIS.dbo.tbl_R_BMSOffices as b
                                                                  on a.FMISOfficeCode = b.OfficeID
                                                                  left join IFMIS.dbo.tbl_R_BMSProgramAccounts as c
                                                                  on a.FMISProgramCode = c.ProgramID and a.FMISAccountCode = c.AccountID 
                                                                  and a.ActionCode = c.ActionCode and a.YearOf = c.AccountYear " +
                                                                    "where a.FMISProgramCode=" + program_id + " and a.FMISAccountCode=" + account_id + " and  a.YearOf = '" + year_ + "' and a.ActionCode = 1 and c.actioncode=1" +
                                                                    "order by a.supplementaltransfere_id", con);


                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Monthly_Supplement_Model Float = new Monthly_Supplement_Model();
                        Float.supplementaltransfere_id = reader.GetInt64(0);
                        Float.Amount = Convert.ToDouble(reader.GetValue(1));
                        Float.MonthOf_ = reader.GetValue(2).ToString();
                        Float.YearOf = reader.GetInt32(3);
                        Float.OfficeAbbrivation = reader.GetValue(4) is DBNull ? "" : reader.GetValue(4).ToString();
                        Float.AccountName = reader.GetValue(5) is DBNull ? "" : reader.GetValue(5).ToString();
                        Float.DateTimeEntered = reader.GetValue(6).ToString();

                        Float_List.Add(Float);

                    }
                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@" select distinct a.supplementaltransfere_id,a.Amount ,DateName( month , DateAdd( month , a.MonthOf , 0 ) - 1 ) as MonthOf 
                                                                  ,a.YearOf,b.OfficeAbbrivation,c.AccountName,a.[DateTimeEntered] FROM IFMIS.dbo.tbl_R_BMS_SupplementalTransfere as a
                                                                  left join IFMIS.dbo.tbl_R_BMSOffices as b
                                                                  on a.FMISOfficeCode = b.OfficeID
                                                                  left join IFMIS.dbo.tbl_R_BMSProgramAccounts as c
                                                                  on a.FMISProgramCode = c.ProgramID and a.FMISAccountCode = c.AccountID 
                                                                  and a.YearOf = c.AccountYear " +
                                                                    "where a.YearOf = '" + year_ + "' and a.ActionCode = 1 and c.actioncode=1 " +
                                                                    "order by a.supplementaltransfere_id", con);


                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Monthly_Supplement_Model Float = new Monthly_Supplement_Model();
                        Float.supplementaltransfere_id = reader.GetInt64(0);
                        Float.Amount = Convert.ToDouble(reader.GetValue(1));
                        Float.MonthOf_ = reader.GetValue(2).ToString();
                        Float.YearOf = reader.GetInt32(3);
                        Float.OfficeAbbrivation = reader.GetValue(4) is DBNull ? "" : reader.GetValue(4).ToString();
                        Float.AccountName = reader.GetValue(5) is DBNull ? "" : reader.GetValue(5).ToString();
                        Float.DateTimeEntered = reader.GetValue(6).ToString();

                        Float_List.Add(Float);

                    }
                }
            }
            return Float_List;
            
        }
        public IEnumerable<Monthly_DD_Model> Supplement_Grid_rever(int? LegalCode = 0, int? office_id = 0, int? program_id = 0, int? ooe_id = 0, int? account_id = 0, int? month_ = 0, int? year_ = 0)
        {

            List<Monthly_DD_Model> Float_List = new List<Monthly_DD_Model>();

            if (account_id != 0 && program_id !=0)
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@" select a.supplementalreverse_id,a.Amount ,DateName( month , DateAdd( month , a.MonthOf , 0 ) - 1 ) as MonthOf 
                                                                  ,a.YearOf,b.OfficeAbbrivation,c.AccountName,a.type_,a.DateTimeEntered FROM IFMIS.dbo.tbl_R_BMS_SupplementalReverse as a
                                                                  left join IFMIS.dbo.tbl_R_BMSOffices as b
                                                                  on a.FMISOfficeCode = b.OfficeID
                                                                  left join IFMIS.dbo.tbl_R_BMSProgramAccounts as c
                                                                  on a.FMISProgramCode = c.ProgramID and a.FMISAccountCode = c.AccountID 
                                                                  and a.YearOf = c.AccountYear " +
                                                                      "where a.FMISProgramCode = "+ program_id + " and a.FMISAccountCode ="+ account_id + " and a.YearOf = '" + year_ + "' and a.ActionCode = 1 and c.actioncode=1" +
                                                                      "order by a.supplementalreverse_id", con);


                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Monthly_DD_Model Float = new Monthly_DD_Model();
                        Float.supplementalreverse_id = reader.GetInt64(0);
                        Float.Amount = Convert.ToDouble(reader.GetValue(1));
                        Float.MonthOf = reader.GetValue(2).ToString();
                        Float.YearOf = reader.GetInt32(3);
                        Float.OfficeAbbrivation = reader.GetValue(4) is DBNull ? "" : reader.GetValue(4).ToString();
                        Float.AccountName = reader.GetValue(5) is DBNull ? "" : reader.GetValue(5).ToString();
                        Float.type_ = reader.GetInt32(6);
                        Float.dtetime = reader.GetValue(7).ToString();
                        Float_List.Add(Float);

                    }
                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@" select a.supplementalreverse_id,a.Amount ,DateName( month , DateAdd( month , a.MonthOf , 0 ) - 1 ) as MonthOf 
                                                                  ,a.YearOf,b.OfficeAbbrivation,c.AccountName,a.type_,a.DateTimeEntered FROM IFMIS.dbo.tbl_R_BMS_SupplementalReverse as a
                                                                  left join IFMIS.dbo.tbl_R_BMSOffices as b
                                                                  on a.FMISOfficeCode = b.OfficeID
                                                                  left join IFMIS.dbo.tbl_R_BMSProgramAccounts as c
                                                                  on a.FMISProgramCode = c.ProgramID and a.FMISAccountCode = c.AccountID 
                                                                  and a.YearOf = c.AccountYear " +
                                                                      "where a.YearOf = '" + year_ + "' and a.ActionCode = 1  and c.actioncode=1" +
                                                                      "order by a.supplementalreverse_id", con);


                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Monthly_DD_Model Float = new Monthly_DD_Model();
                        Float.supplementalreverse_id = reader.GetInt64(0);
                        Float.Amount = Convert.ToDouble(reader.GetValue(1));
                        Float.MonthOf = reader.GetValue(2).ToString();
                        Float.YearOf = reader.GetInt32(3);
                        Float.OfficeAbbrivation = reader.GetValue(4) is DBNull ? "" : reader.GetValue(4).ToString();
                        Float.AccountName = reader.GetValue(5) is DBNull ? "" : reader.GetValue(5).ToString();
                        Float.type_ = reader.GetInt32(6);
                        Float.dtetime = reader.GetValue(7).ToString();
                        Float_List.Add(Float);

                    }
                }
            }
            return Float_List;
        }

        public string dedet_income(int? income_id)
        {
                
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@" SELECT Amount FROM IFMIS.dbo.tbl_R_BMS_Income where income_id = '" + income_id + "' and ActionCode = 1", con);
                con.Open();
                return com.ExecuteScalar().ToString();


            }
        }
        Int64 realign_ID_del = 0;
        double realaign_amount_EE_ = 0;
        double realaign_amount_EE_AV = 0;
        double realaign_amount_EE_AVP = 0; 
        public string delete_Realaign(int? realignment_id, int? year_)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com47 = new SqlCommand(@"SELECT Amount FROM IFMIS.dbo.tbl_R_BMS_Realignment where realignment_id = '" + realignment_id + "'", con);
                    con.Open();
                    SqlDataReader reader7 = com47.ExecuteReader();
                    while (reader7.Read())
                    {
                        Monthly_Realignment_Model subs_available = new Monthly_Realignment_Model();
                        subs_available.Amount = Convert.ToDouble(reader7.GetValue(0));


                        realaign_amount_EE_ = subs_available.Amount;


                    }

                }

                

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com47 = new SqlCommand(@"SELECT  totalrealign_id,Amount FROM [IFMIS].[dbo].[tbl_R_BMS_TotalRealign] where YearOf = '" + year_ + "' and ActionCode = 1", con);
                    con.Open();
                    SqlDataReader reader7 = com47.ExecuteReader();
                    while (reader7.Read())
                    {
                        Monthly_Realignment_Model subs_available = new Monthly_Realignment_Model();
                        subs_available.total_rel_id = reader7.GetInt64(0);
                        subs_available.available_amount = Convert.ToDouble(reader7.GetValue(1));

                        realign_ID_del = subs_available.total_rel_id;
                        realaign_amount_EE_AV = subs_available.available_amount;


                    }

                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com2 = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_TotalRealign set ActionCode = 4 , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where ActionCode = 1 and totalrealign_id = '" + realign_ID_del + "'", con);
                    con.Open();
                    com2.ExecuteNonQuery();

                }
                realaign_amount_EE_AVP = realaign_amount_EE_AV - realaign_amount_EE_;
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com3 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_TotalRealign (UserID,Amount,ActionCode,YearOf,DateTimeEntered,TOfficeCode ,ToProgramCode ,ToOOECode ,ToAccountCode ,ToAmount) " +
                                                    "values ('" + Account.UserInfo.eid.ToString() + "','" + realaign_amount_EE_AVP + "',1,'" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),0,0,0,0,0)", con);
                    con.Open();
                    com3.ExecuteNonQuery();
                     realaign_amount_EE_ = 0;
                     realaign_amount_EE_AV = 0;
                     realaign_amount_EE_AVP = 0; 
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_Realignment set ActionCode = 4  , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where realignment_id = '" + realignment_id + "' ", con);
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
        public string delete_Realaign_TO(int? realignment_id, int? year_)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com47 = new SqlCommand(@"SELECT Amount FROM IFMIS.dbo.tbl_R_BMS_Realignment where realignment_id = '" + realignment_id + "'", con);
                    con.Open();
                    SqlDataReader reader7 = com47.ExecuteReader();
                    while (reader7.Read())
                    {
                        Monthly_Realignment_Model subs_available = new Monthly_Realignment_Model();
                        subs_available.Amount = Convert.ToDouble(reader7.GetValue(0));


                        realaign_amount_EE_ = subs_available.Amount;


                    }

                }



                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com47 = new SqlCommand(@"SELECT  totalrealign_id,Amount FROM [IFMIS].[dbo].[tbl_R_BMS_TotalRealign] where YearOf = '" + year_ + "' and ActionCode = 1", con);
                    con.Open();
                    SqlDataReader reader7 = com47.ExecuteReader();
                    while (reader7.Read())
                    {
                        Monthly_Realignment_Model subs_available = new Monthly_Realignment_Model();
                        subs_available.total_rel_id = reader7.GetInt64(0);
                        subs_available.available_amount = Convert.ToDouble(reader7.GetValue(1));

                        realign_ID_del = subs_available.total_rel_id;
                        realaign_amount_EE_AV = subs_available.available_amount;


                    }

                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com2 = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_TotalRealign set ActionCode = 4 , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where ActionCode = 1 and totalrealign_id = '" + realign_ID_del + "'", con);
                    con.Open();
                    com2.ExecuteNonQuery();

                }
                realaign_amount_EE_AVP = realaign_amount_EE_AV + realaign_amount_EE_;
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com3 = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_TotalRealign (UserID,Amount,ActionCode,YearOf,DateTimeEntered,TOfficeCode ,ToProgramCode ,ToOOECode ,ToAccountCode ,ToAmount) " +
                                                    "values ('" + Account.UserInfo.eid.ToString() + "','" + realaign_amount_EE_AVP + "',1,'" + year_ + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),0,0,0,0,0)", con);
                    con.Open();
                    com3.ExecuteNonQuery();
                    realaign_amount_EE_ = 0;
                    realaign_amount_EE_AV = 0;
                    realaign_amount_EE_AVP = 0;
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_Realignment set ActionCode = 4  , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where realignment_id = '" + realignment_id + "' ", con);
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
        public IEnumerable<Monthly_reserve_Model> read_reserve(int? office_id,int? account_id, int? year_)
        {
            List<Monthly_reserve_Model> Realign_From = new List<Monthly_reserve_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                //Display reserved appropriations
                //Update on 2/9/2018 - xXx
                  SqlCommand com = new SqlCommand(@" exec sp_BMS_ReadReserve  '" + office_id + "','" + account_id + "','" + year_ + "'", con);
//                SqlCommand com = new SqlCommand(@"SELECT a.reserve_id,c.AccountName ,a.ReservePercent ,YearOf ,isnull(a.Amount,0) as Amount ,reserve_flag,isnull(case when b.ProposalAllotedAmount = 0 then 0 else case when b.ProposalAllotedAmount = 0 then 0 else(b.ProposalAllotedAmount * (a.ReservePercent/100)) end end,0) as percentInMoney,
//                                                 isnull(case when b.ProposalAllotedAmount = 0 then 0 else case when Amount = 0 then 0 else ((Amount/b.ProposalAllotedAmount) * 100) end end,0) as moneyInpercent  FROM IFMIS.dbo.tbl_R_BMS_Reserve AS a 
//                                                 inner join IFMIS.dbo.tbl_R_BMSAccounts as c on a.FMISAccountCode  = c.FMISAccountCode 
//                                                 inner join IFMIS.dbo.tbl_T_BMSBudgetProposal as b on a.FMISProgramCode = b.ProgramID and a.FMISAccountCode = b.AccountID and a.YearOf = b.ProposalYear
//                                                 where ActionCode = 1 and a.FMISOfficeCode = '" + office_id + "' and YearOf = '" + year_ + "' and b.ProposalYear = '" + year_ + "' and b.ProposalActionCode = 1 ", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_reserve_Model reserve_list = new Monthly_reserve_Model();
                    reserve_list.reserve_id = reader.GetInt64(0);
                    reserve_list.account_name = reader.GetValue(1).ToString();
                    reserve_list.ReservePercent = Convert.ToDouble(reader.GetValue(2));
                    reserve_list.YearOf = reader.GetInt32(3);
                    reserve_list.Amount = Convert.ToDouble(reader.GetValue(4));
                    reserve_list.reserve_flag = reader.GetInt32(5);
                    reserve_list.percentInmoney = reader.GetValue(6) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(6));
                    reserve_list.moneyInpercent = reader.GetValue(6) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(7));



                    Realign_From.Add(reserve_list);

                }
            }
            return Realign_From;
        }


        public string delete_reserve(int? reserve_id)
        {
            try
            {
               
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_Reserve set ActionCode = 4  , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where reserve_id = '" + reserve_id + "' ", con);
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


        public Monthly_Supplement_Model Edit_Supplement_(int? supplementalbudget_id)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select supplementalbudget_id,LegalCode,Amount FROM IFMIS.dbo.tbl_R_BMS_SupplementalBudget where supplementalbudget_id = " + supplementalbudget_id + "", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_Supplement_Model Float = new Monthly_Supplement_Model();
                    Float.supplementalbudget_id = reader.GetInt64(0);
                    Float.LegalCode = reader.GetInt64(1);
                    Float.Amount = Convert.ToDouble(reader.GetValue(2));

                    return Float;
                }
            }
            return new Monthly_Supplement_Model();  
        }
        public Monthly_Supplement_Model Edit_Supplement_Reverse(int? supplementalreverse_id)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"  select distinct a.supplementalreverse_id, a.LegalCode, a.Amount, a.FMISOfficeCode, a.FMISProgramCode, b.ObjectOfExpendetureID
                                                      , a.FMISAccountCode, a.MonthOf FROM IFMIS.dbo.tbl_R_BMS_SupplementalReverse as a
                                                      inner join IFMIS.dbo.tbl_R_BMSProgramAccounts as b
                                                      on a.FMISProgramCode = b.ProgramID and a.FMISAccountCode = b.AccountID and a.YearOf = b.AccountYear and a.ActionCode = a.ActionCode
                                                      where supplementalreverse_id = " + supplementalreverse_id + "", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_Supplement_Model Float = new Monthly_Supplement_Model();
                    Float.supplementalbudget_id = reader.GetInt64(0);
                    Float.LegalCode = reader.GetInt64(1);
                    Float.Amount = Convert.ToDouble(reader.GetValue(2));
                    Float.FMISOfficeCode = reader.GetValue(3) is DBNull ? 0 : reader.GetInt32(3);
                    Float.FMISProgramCode = reader.GetValue(4) is DBNull ? 0 : Convert.ToInt32(reader.GetValue(4));
                    Float.OOECode = reader.GetValue(5) is DBNull ? 0 : Convert.ToInt32(reader.GetValue(5));
                    Float.FMISAccountCode = reader.GetValue(6) is DBNull ? 0 : Convert.ToInt32(reader.GetValue(6));
                    Float.MonthOf = reader.GetValue(7) is DBNull ? 0 : Convert.ToInt32(reader.GetValue(7));

                    return Float;
                }
            }
            return new Monthly_Supplement_Model();
        }


        public Release_Float_Edit _Obli_Remain(int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_)
        {

            using (SqlConnection con = new SqlConnection(Common .MyConn()))
            {
                SqlCommand com = new SqlCommand(@"IFMIS.dbo.sp_MonthlyRelease_Total_Obligate '" + office_id + "','" + program_id + "','" + ooe_id + "','" + account_id + "','" + year_ + "'", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Release_Float_Edit Float = new Release_Float_Edit();
                    Float.AmountRelease = Convert.ToDouble(reader.GetValue(0));
                    Float.AmountObligate = Convert.ToDouble(reader.GetValue(1));
                    Float.AmountRemain = Convert.ToDouble(reader.GetValue(2));


                    return Float;
                }
            }
            return new Release_Float_Edit();
        }
        public Monthly_Supplement_Model Edit_Supplement_Transfere(int? supplementaltransfere_id)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select distinct a.supplementaltransfere_id, a.LegalCode, a.Amount, a.FMISOfficeCode, a.FMISProgramCode, b.ObjectOfExpendetureID
                                                      , a.FMISAccountCode, a.MonthOf FROM IFMIS.dbo.tbl_R_BMS_SupplementalTransfere as a
                                                      inner join IFMIS.dbo.tbl_R_BMSProgramAccounts as b
                                                      on a.FMISProgramCode = b.ProgramID and a.FMISAccountCode = b.AccountID and a.YearOf = b.AccountYear
                                                      where supplementaltransfere_id = " + supplementaltransfere_id + " and a.ActionCode=1 and b.ActionCode=1", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_Supplement_Model Float = new Monthly_Supplement_Model();
                    Float.supplementalbudget_id = reader.GetInt64(0);
                    Float.LegalCode = reader.GetInt64(1);
                    Float.Amount = Convert.ToDouble(reader.GetValue(2));
                    Float.FMISOfficeCode = reader.GetValue(3) is DBNull ? 0 : reader.GetInt32(3);
                    Float.FMISProgramCode = reader.GetValue(4) is DBNull ? 0 : Convert.ToInt32(reader.GetValue(4));
                    Float.OOECode = reader.GetValue(5) is DBNull ? 0 : Convert.ToInt32(reader.GetValue(5));
                    Float.FMISAccountCode = reader.GetValue(6) is DBNull ? 0 : Convert.ToInt32(reader.GetValue(6));
                    Float.MonthOf = reader.GetValue(7) is DBNull ? 0 : Convert.ToInt32(reader.GetValue(7));
                    return Float;
                }
            }
            return new Monthly_Supplement_Model();
        }
        public string delete_Supplement_(int? supplementalbudget_id)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com47 = new SqlCommand(@"select FMISOfficeCode,FMISProgramCode,FMISAccountCode,OOECode,[Description],Amount,MonthOf,YearOf,DateTimeEntered FROM IFMIS.dbo.tbl_R_BMS_SupplementalBudget where supplementalbudget_id = " + supplementalbudget_id + "", con);
                    con.Open();
                    SqlDataReader reader7 = com47.ExecuteReader();
                    while (reader7.Read())
                    {
                        Monthly_Supplement_Model edit_list = new Monthly_Supplement_Model();
                        edit_list.FMISOfficeCode = reader7.GetValue(0) is DBNull ? 0 : reader7.GetInt32(0);
                        edit_list.FMISProgramCode = reader7.GetValue(1) is DBNull ? 0 : Convert.ToInt32(reader7.GetValue(1));
                        edit_list.FMISAccountCode = reader7.GetValue(2) is DBNull ? 0 : Convert.ToInt32(reader7.GetValue(2));
                        edit_list.OOECode = reader7.GetValue(3) is DBNull ? 0 : Convert.ToInt32(reader7.GetValue(3));
                        edit_list.Description = reader7.GetValue(4) is DBNull ? "" : reader7.GetValue(4).ToString();
                        edit_list.Amount = reader7.GetValue(5) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(5));
                        edit_list.MonthOf = reader7.GetValue(6) is DBNull ? 0 : Convert.ToInt32(reader7.GetValue(6));
                        edit_list.YearOf = reader7.GetValue(7) is DBNull ? 0 : Convert.ToInt32(reader7.GetValue(7));
                        edit_list.DateTimeEntered = reader7.GetValue(8).ToString();

                        FMISOfficeCode_SUP = edit_list.FMISOfficeCode;
                        FMISProgramCode_SUP = edit_list.FMISProgramCode;
                        FMISAccountCode_SUP = edit_list.FMISAccountCode;
                        OOECode_SUP = edit_list.OOECode;
                        Description_SUP = edit_list.Description;
                        Amount_SUP = edit_list.Amount;
                        MonthOf_SUP = edit_list.MonthOf;
                        YearOf_SUP = edit_list.YearOf;
                        DateTimeEntered_SUP = edit_list.DateTimeEntered;
                    }

                }


                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com2 = new SqlCommand("update fmis.dbo.tblBMS_SupplementalBudget set ActionCode = 4 where FMISOfficeCode = '" + FMISOfficeCode_SUP + "' and ActionCode = 1 " +
                        " and FMISProgramCode = '" + FMISProgramCode_SUP + "' and FMISAccountCode = '" + FMISAccountCode_SUP + "' and OOECode = '" + OOECode_SUP + "' and Description = '" + Description_SUP + "'" +
                        " and Amount = '" + Amount_SUP + "' and MonthOf = '" + MonthOf_SUP + "' and YearOf = '" + YearOf_SUP + "' and DateTimeEntered = '" + DateTimeEntered_SUP + "'", con);
                    con.Open();
                    com2.ExecuteNonQuery();
                    con.Close();

                    SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_SupplementalBudget set ActionCode = 4  , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where supplementalbudget_id = '" + supplementalbudget_id + "' ", con);
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
        public string delete_Supplement_Reverse(int? supplementalreverse_id)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("  update IFMIS.dbo.tbl_R_BMS_SupplementalReverse set ActionCode = 4 where supplementalreverse_id = '" + supplementalreverse_id + "'", con);
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
        public string delete_Supplement_Transfere(int? supplementaltransfere_id)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("UPDATE IFMIS.dbo.tbl_R_BMS_SupplementalTransfere SET ActionCode = 4 where supplementaltransfere_id = '" + supplementaltransfere_id + "' ", con);
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

       // FMISOfficeCode,FMISProgramCode,FMISAccountCode,OOECode,[Description],Amount,MonthOf,YearOf,UserID,DateTimeEntered
        //SBCodess_EDIT_OLD 
        // FMISOfficeCode_SUP 
        // FMISProgramCode_SUP 
        // FMISAccountCode_SUP 
        // OOECode_SUP 
        // Description_SUP 
        // Amount_SUP 
        // MonthOf_SUP 
        // YearOf_SUP 
        // UserID_SUP 
        // DateTimeEntered_SUP

        public string edit_supplemement(int? legal_code, int? office_id, int? program_id, int? ooe_id, int? account_id, int? year_, double supplement_amount, int? MonthOf, int? supplement_id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com47 = new SqlCommand(@"select FMISOfficeCode,FMISProgramCode,FMISAccountCode,OOECode,[Description],Amount,MonthOf,YearOf,DateTimeEntered FROM IFMIS.dbo.tbl_R_BMS_SupplementalBudget where supplementalbudget_id = " + supplement_id + "", con);
                    con.Open();
                    SqlDataReader reader7 = com47.ExecuteReader();
                    while (reader7.Read())
                    {
                        Monthly_Supplement_Model edit_list = new Monthly_Supplement_Model();
                         edit_list.FMISOfficeCode = reader7.GetValue(0) is DBNull ? 0 : reader7.GetInt32(0);
                         edit_list.FMISProgramCode = reader7.GetValue(1) is DBNull ? 0 : Convert.ToInt32(reader7.GetValue(1));
                         edit_list.FMISAccountCode = reader7.GetValue(2) is DBNull ? 0 : Convert.ToInt32(reader7.GetValue(2));
                         edit_list.OOECode = reader7.GetValue(3) is DBNull ? 0 : Convert.ToInt32(reader7.GetValue(3));
                         edit_list.Description = reader7.GetValue(4) is DBNull ? "" : reader7.GetValue(4).ToString();
                         edit_list.Amount = reader7.GetValue(5) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(5));
                         edit_list.MonthOf = reader7.GetValue(6) is DBNull ? 0 : Convert.ToInt32(reader7.GetValue(6));
                         edit_list.YearOf = reader7.GetValue(7) is DBNull ? 0 : Convert.ToInt32(reader7.GetValue(7));
                         edit_list.DateTimeEntered = reader7.GetValue(8).ToString();

                        FMISOfficeCode_SUP = edit_list.FMISOfficeCode;
                        FMISProgramCode_SUP = edit_list.FMISProgramCode;
                        FMISAccountCode_SUP =  edit_list.FMISAccountCode;
                        OOECode_SUP = edit_list.OOECode;
                        Description_SUP = edit_list.Description;
                        Amount_SUP = edit_list.Amount;
                        MonthOf_SUP = edit_list.MonthOf;
                        YearOf_SUP = edit_list.YearOf;
                        DateTimeEntered_SUP = edit_list.DateTimeEntered;
                    }

                }

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com47 = new SqlCommand(@"select SBCode FROM fmis.dbo.tblBMS_SupplementalBudget where FMISOfficeCode = " + FMISOfficeCode_SUP + " and ActionCode = 1 " +
                        " and FMISProgramCode = '" + FMISProgramCode_SUP + "' and FMISAccountCode = '" + FMISAccountCode_SUP + "' and OOECode = '" + OOECode_SUP + "' and Description = '" + Description_SUP + "'" +
                        " and Amount = '" + Amount_SUP + "' and MonthOf = '" + MonthOf_SUP + "' and YearOf = '" + YearOf_SUP + "' and DateTimeEntered = '" + DateTimeEntered_SUP + "'", con);
                    con.Open();
                    SqlDataReader reader7 = com47.ExecuteReader();  
                    while (reader7.Read())
                    {
                        Monthly_Edit_Realign_Model edit_list = new Monthly_Edit_Realign_Model();
                        edit_list.SBCode = reader7.GetInt64(0);


                        SBCodess_OLD = edit_list.SBCode;


                    }

                }
                
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com47 = new SqlCommand(@" select SBCode FROM IFMIS.dbo.tbl_R_BMS_SupplementalBudget where supplementalbudget_id = " + supplement_id + "", con);
                    con.Open();
                    SqlDataReader reader7 = com47.ExecuteReader();
                    while (reader7.Read())
                    {
                        Monthly_Edit_Realign_Model edit_list = new Monthly_Edit_Realign_Model();
                        edit_list.SBCode = reader7.GetInt64(0);


                        SBCode_edit = edit_list.SBCode;
                      

                    }

                    con.Close();

                    SqlCommand com49 = new SqlCommand(@"SELECT AccountName FROM IFMIS.dbo.tbl_R_BMSAccounts where FMISAccountCode = " + account_id + "", con);
                    con.Open();
                    SqlDataReader reader72 = com49.ExecuteReader();
                    while (reader72.Read())
                    {
                        Monthly_Edit_Realign_Model edit_list = new Monthly_Edit_Realign_Model();

                        edit_list.Description_supp = reader72.GetString(0);

                        Descriptionss = edit_list.Description_supp;


                    }

                    con.Close();
                                                       
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com2 = new SqlCommand("insert into fmis.dbo.tblBMS_SupplementalBudget (SBCode,LegalCode,FMISOfficeCode,FMISProgramCode,FMISAccountCode,OOECode,[Description],Amount,MonthOf,YearOf,ActionCode,UserID,DateTimeEntered) " +
                            "values ('" + SBCodess_OLD + "','" + SBCodess_OLD + "','" + office_id + "','" + program_id + "','" + account_id + "','" + ooe_id + "','" + Descriptionss.Replace("'","''") + "','" + supplement_amount + "','" + MonthOf + "','" + year_ + "',1,'" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt')) ", con);
                    con.Open();
                    com2.ExecuteNonQuery();
                    con.Close();
                    SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_SupplementalBudget (SBCode,LegalCode,FMISOfficeCode,FMISProgramCode,FMISAccountCode,OOECode,[Description],Amount,MonthOf,YearOf,ActionCode,UserID,DateTimeEntered) " +
                            "values ('" + SBCode_edit + "','" + legal_code + "','" + office_id + "','" + program_id + "','" + account_id + "','" + ooe_id + "','" + Descriptionss.Replace("'", "''") + "','" + supplement_amount + "','" + MonthOf + "','" + year_ + "',1,'" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt')) ", con);
                    con.Open();
                    com.ExecuteNonQuery();
                }
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com2 = new SqlCommand("update fmis.dbo.tblBMS_SupplementalBudget set ActionCode = 4 where FMISOfficeCode = '" + FMISOfficeCode_SUP + "' and ActionCode = 1 " +
                        " and FMISProgramCode = '" + FMISProgramCode_SUP + "' and FMISAccountCode = '" + FMISAccountCode_SUP + "' and OOECode = '" + OOECode_SUP + "' and Description = '" + Description_SUP + "'" +
                        " and Amount = '" + Amount_SUP + "' and MonthOf = '" + MonthOf_SUP + "' and YearOf = '" + YearOf_SUP + "' and DateTimeEntered = '" + DateTimeEntered_SUP + "'", con);
                    con.Open();
                    com2.ExecuteNonQuery();
                    con.Close();

                    SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_SupplementalBudget set ActionCode = 4  , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where supplementalbudget_id = '" + supplement_id + "' ", con);
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



        //public string save_incs(int? office_id, int? month_id, double amount_inc, int? year_)
        //{
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(Common.MyConn()))
        //        {
        //            SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_Income (FMISOfficeCode,MonthOf,Amount,YearOf,UserID,DateTimeEntered,ActionCode) " +
        //                                           " values ('" + office_id + "' ,'" + month_id + "','" + amount_inc + "','" + year_ + "', '" + Account.UserInfo.eid.ToString() + "' ,format(GetDate(),'M/d/yyy hh:mm:ss tt'),1 )", con);
        //            con.Open();
        //            com.ExecuteNonQuery();
        //            con.Close();

        //            SqlCommand com2 = new SqlCommand("insert into fmis.dbo.tblBMS_Income (FMISOfficeCode ,MonthOf ,Amount ,YearOf ,UserID ,DateTimeEntered ,ActionCode) " +
        //                                           " values ('" + office_id + "' ,'" + month_id + "','" + amount_inc + "','" + year_ + "', '" + Account.UserInfo.eid.ToString() + "' ,format(GetDate(),'M/d/yyy hh:mm:ss tt'),1 )", con);
        //            con.Open();
        //            com2.ExecuteNonQuery();
        //            return "1";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message.ToString();
        //    }
        //}

        public string cake_inc_Save(int? office_id, int? month_id, double amount_inc, int? year_) {
            try {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_Income (FMISOfficeCode,MonthOf,Amount,YearOf,UserID,DateTimeEntered,ActionCode) " +
                                                   " values ('" + office_id + "' ,'" + month_id + "','" + amount_inc + "','" + year_ + "', '" + Account.UserInfo.eid.ToString() + "' ,format(GetDate(),'M/d/yyy hh:mm:ss tt'),1 )", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();

                    SqlCommand com2 = new SqlCommand("insert into fmis.dbo.tblBMS_Income (FMISOfficeCode ,MonthOf ,Amount ,YearOf ,UserID ,DateTimeEntered ,ActionCode) " +
                                                   " values ('" + office_id + "' ,'" + month_id + "','" + amount_inc + "','" + year_ + "', '" + Account.UserInfo.eid.ToString() + "' ,format(GetDate(),'M/d/yyy hh:mm:ss tt'),1 )", con);
                    con.Open();
                    com2.ExecuteNonQuery();

                    return "1";
                }
            } catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        
        
        }

        public string Get_amount_Release(int? release_id, int? ooe_id)
        {
            if(ooe_id == 1){
             using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select AmountPS from IFMIS.dbo.tbl_R_BMS_Release where release_id = " + release_id + "", con);
                    
                con.Open();
                return com.ExecuteScalar().ToString();


            }
            } else if(ooe_id == 2) {
             using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select AmountMOOE from IFMIS.dbo.tbl_R_BMS_Release where release_id = " + release_id + "", con);
                    
                con.Open();
                return com.ExecuteScalar().ToString();


            }
            } else if(ooe_id == 3) {
             using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select AmountCO from IFMIS.dbo.tbl_R_BMS_Release where release_id = " + release_id + "", con);
                    
                con.Open();
                return com.ExecuteScalar().ToString();


            }
            }else {
            return "0";
            }
           
        }




        public string remove_Release_(int? release_id, int? ooe_id)
        {
            try
            {


                if (ooe_id == 1)
                {


                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com47 = new SqlCommand(@"select FMISOfficeCode,FMISProgramCode,FMISAccountCode,MonthOf,YearOf,DateTimeEntered,AmountPS from IFMIS.dbo.tbl_R_BMS_Release where release_id = '" + release_id + "' ", con);
                        con.Open();
                        SqlDataReader reader7 = com47.ExecuteReader();
                        while (reader7.Read())
                        {
                            Release_Float_Model edit_list = new Release_Float_Model();
                            edit_list.FMISOfficeCode = reader7.GetValue(0) is DBNull ? 0 : reader7.GetInt32(0);
                            edit_list.FMISProgramCode = reader7.GetValue(1) is DBNull ? 0 : reader7.GetInt32(1);
                            edit_list.FMISAccountCode = reader7.GetValue(2) is DBNull ? 0 : reader7.GetInt32(2);
                            edit_list.MonthOf_ = reader7.GetValue(3) is DBNull ? 0 : reader7.GetInt32(3);
                            edit_list.YearOf = reader7.GetValue(4) is DBNull ? 0 : reader7.GetInt32(4);
                            edit_list.DateTimeEntered = reader7.GetValue(5).ToString();
                            edit_list.AmountPS = reader7.GetValue(6) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(6));

                            FMISOfficeCode_Rel = edit_list.FMISOfficeCode;
                            FMISProgramCode_Rel = edit_list.FMISProgramCode;
                            FMISAccountCode_Rel = edit_list.FMISAccountCode;
                            MonthOf_Rel = edit_list.MonthOf_;
                            YearOf_Rel = edit_list.YearOf;
                            DateTimeEntered_Rel = edit_list.DateTimeEntered;
                            Amount_PS_Rel = edit_list.AmountPS;

                        }

                    }


                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com2 = new SqlCommand("update fmis.dbo.tblBMS_Releases set ActionCode = 4 where FMISOfficeCode = '" + FMISOfficeCode_Rel + "' and ActionCode = 1 " +
                            " and FMISProgramCode = '" + FMISProgramCode_Rel + "' and FMISAccountCode = '" + FMISAccountCode_Rel + "' " +
                            " and AmountPS = '" + Amount_PS_Rel + "' and MonthOf = '" + MonthOf_Rel + "' and YearOf = '" + YearOf_Rel + "' and DateTimeEntered = '" + DateTimeEntered_Rel + "'", con);
                        con.Open();
                        com2.ExecuteNonQuery();
                        con.Close();

                        SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_Release set ActionCode = 4  , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where release_id = '" + release_id + "' ", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        return "1";
                    }

                }
                else if (ooe_id == 2)
                {

                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com47 = new SqlCommand(@"select FMISOfficeCode,FMISProgramCode,FMISAccountCode,MonthOf,YearOf,DateTimeEntered,AmountMOOE from IFMIS.dbo.tbl_R_BMS_Release where release_id = '" + release_id + "' ", con);
                        con.Open();
                        SqlDataReader reader7 = com47.ExecuteReader();
                        while (reader7.Read())
                        {
                            Release_Float_Model edit_list = new Release_Float_Model();
                            edit_list.FMISOfficeCode = reader7.GetValue(0) is DBNull ? 0 : reader7.GetInt32(0);
                            edit_list.FMISProgramCode = reader7.GetValue(1) is DBNull ? 0 : reader7.GetInt32(1);
                            edit_list.FMISAccountCode = reader7.GetValue(2) is DBNull ? 0 : reader7.GetInt32(2);
                            edit_list.MonthOf_ = reader7.GetValue(3) is DBNull ? 0 : reader7.GetInt32(3);
                            edit_list.YearOf = reader7.GetValue(4) is DBNull ? 0 : reader7.GetInt32(4);
                            edit_list.DateTimeEntered = reader7.GetValue(5).ToString();
                            edit_list.AmountMOOE = reader7.GetValue(6) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(6));

                            FMISOfficeCode_Rel = edit_list.FMISOfficeCode;
                            FMISProgramCode_Rel = edit_list.FMISProgramCode;
                            FMISAccountCode_Rel = edit_list.FMISAccountCode;
                            MonthOf_Rel = edit_list.MonthOf_;
                            YearOf_Rel = edit_list.YearOf;
                            DateTimeEntered_Rel = edit_list.DateTimeEntered;
                            Amount_MOOE_Rel = edit_list.AmountMOOE;

                        }

                    }


                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com2 = new SqlCommand("update fmis.dbo.tblBMS_Releases set ActionCode = 4 where FMISOfficeCode = '" + FMISOfficeCode_Rel + "' and ActionCode = 1 " +
                            " and FMISProgramCode = '" + FMISProgramCode_Rel + "' and FMISAccountCode = '" + FMISAccountCode_Rel + "' " +
                            " and AmountMOOE = '" + Amount_MOOE_Rel + "' and MonthOf = '" + MonthOf_Rel + "' and YearOf = '" + YearOf_Rel + "' and DateTimeEntered = '" + DateTimeEntered_Rel + "'", con);
                        con.Open();
                        com2.ExecuteNonQuery();
                        con.Close();

                        SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_Release set ActionCode = 4  , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where release_id = '" + release_id + "' ", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        return "1";
                    }



                }
                else if (ooe_id == 3)
                {

                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com47 = new SqlCommand(@"select FMISOfficeCode,FMISProgramCode,FMISAccountCode,MonthOf,YearOf,DateTimeEntered,AmountCO from IFMIS.dbo.tbl_R_BMS_Release where release_id = '" + release_id + "' ", con);
                        con.Open();
                        SqlDataReader reader7 = com47.ExecuteReader();
                        while (reader7.Read())
                        {
                            Release_Float_Model edit_list = new Release_Float_Model();
                            edit_list.FMISOfficeCode = reader7.GetValue(0) is DBNull ? 0 : reader7.GetInt32(0);
                            edit_list.FMISProgramCode = reader7.GetValue(1) is DBNull ? 0 : reader7.GetInt32(1);
                            edit_list.FMISAccountCode = reader7.GetValue(2) is DBNull ? 0 : reader7.GetInt32(2);
                            edit_list.MonthOf_ = reader7.GetValue(3) is DBNull ? 0 : reader7.GetInt32(3);
                            edit_list.YearOf = reader7.GetValue(4) is DBNull ? 0 : reader7.GetInt32(4);
                            edit_list.DateTimeEntered = reader7.GetValue(5).ToString();
                            edit_list.AmountCO = reader7.GetValue(6) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(6));

                            FMISOfficeCode_Rel = edit_list.FMISOfficeCode;
                            FMISProgramCode_Rel = edit_list.FMISProgramCode;
                            FMISAccountCode_Rel = edit_list.FMISAccountCode;
                            MonthOf_Rel = edit_list.MonthOf_;
                            YearOf_Rel = edit_list.YearOf;
                            DateTimeEntered_Rel = edit_list.DateTimeEntered;
                            Amount_CO_Rel = edit_list.AmountCO;

                        }

                    }


                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com2 = new SqlCommand("update fmis.dbo.tblBMS_Releases set ActionCode = 4 where FMISOfficeCode = '" + FMISOfficeCode_Rel + "' and ActionCode = 1 " +
                            " and FMISProgramCode = '" + FMISProgramCode_Rel + "' and FMISAccountCode = '" + FMISAccountCode_Rel + "' " +
                            " and AmountCO = '" + Amount_CO_Rel + "' and MonthOf = '" + MonthOf_Rel + "' and YearOf = '" + YearOf_Rel + "' and DateTimeEntered = '" + DateTimeEntered_Rel + "'", con);
                        con.Open();
                        com2.ExecuteNonQuery();
                        con.Close();

                        SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_Release set ActionCode = 4  , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where release_id = '" + release_id + "' ", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        return "1";
                    }


                }
                else
                {
                    return "100";
                }



            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }


        public string get_Expenses( int? program_id, int? account_id, int? year_)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select COALESCE(SUM(Amount), 0) as amount from IFMIS.dbo.vwBMS_TotalControlALL where ProgramID = '" + program_id + "' and Budget_AcctCharge =  '" + account_id + "' and YearOf = '" + year_ + "'", con);
                con.Open();
                return com.ExecuteScalar().ToString();


            }
        }

        public string Get_Release_(int? office_id, int? program_id, int? account_id, int? year_)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT COALESCE(sum(AmountPS + AmountMOOE + AmountCO), 0) as total_release FROM IFMIS.dbo.tbl_R_BMS_Release where FMISOfficeCode = '" + office_id + "' and FMISProgramCode= '" + program_id + "' and FMISAccountCode = '" + account_id + "' and YearOf = '" + year_ + "' and ActionCode = 1", con);
               
                con.Open();
                return com.ExecuteScalar().ToString();


            }
        }

        public string Get_ReleaseAcc(int? office_id, int? program_id, int? account_id, int? year_,int? month_)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT COALESCE(sum(AmountPS + AmountMOOE + AmountCO), 0) as total_release FROM IFMIS.dbo.tbl_R_BMS_Release where FMISOfficeCode = '" + office_id + "' and FMISProgramCode= '" + program_id + "' and FMISAccountCode = '" + account_id + "' and YearOf = '" + year_ + "' and ActionCode = 1 and [MonthOf] <= "+ month_  + "", con);

                con.Open();
                return com.ExecuteScalar().ToString();


            }
        }

        public string Available_INC(int? office_id, int? year_)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_available_amount_INCOME] '" + office_id + "','" + year_ + "'", con);
                //  SqlCommand com = new SqlCommand(@"sp_MonthlyRelease_available_amount 4,4,1,401,2016", con);
                con.Open();
                return com.ExecuteScalar().ToString();


            }
        }

        public string income_amount_(int? income_id)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@" select Amount From IFMIS.dbo.tbl_R_BMS_Income where income_id = '" + income_id + "'", con);
                con.Open();
                return com.ExecuteScalar().ToString();


            }
        }

        public string delete_Income_(int? income_id)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com47 = new SqlCommand(@"SELECT FMISOfficeCode,MonthOf,Amount,YearOf,DateTimeEntered FROM IFMIS.dbo.tbl_R_BMS_Income where income_id = " + income_id + "", con);
                    con.Open();
                    SqlDataReader reader7 = com47.ExecuteReader();
                    while (reader7.Read())
                    {
                        Monthly_SubInc edit_list = new Monthly_SubInc();
                        edit_list.FMISOfficeCode = reader7.GetValue(0) is DBNull ? 0 : reader7.GetInt32(0);
                        edit_list.MonthOf = reader7.GetValue(1) is DBNull ? 0 : reader7.GetInt32(1);
                        edit_list.Amount = reader7.GetValue(2) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(2));
                        edit_list.YearOf = reader7.GetValue(3) is DBNull ? 0 : reader7.GetInt32(3);
                        edit_list.DateTimeEntered = reader7.GetValue(4).ToString();

                        FMISOfficeCode_INC = edit_list.FMISOfficeCode;
                        MonthOf_INC = edit_list.MonthOf;
                        Amount_INC = edit_list.Amount;
                        YearOf_INC = edit_list.YearOf;
                        DateTimeEntered_INC = edit_list.DateTimeEntered;
                    }

                }


              using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com4 = new SqlCommand("update fmis.dbo.tblBMS_Income set ActionCode = 2 where FMISOfficeCode = '" + FMISOfficeCode_INC + "' and MonthOf = '" + MonthOf_INC + "' and Amount = '" + Amount_INC + "' and YearOf = '" + YearOf_INC + "' and DateTimeEntered = '" + DateTimeEntered_INC + "'", con);
                    con.Open();
                    com4.ExecuteNonQuery();

                    con.Close();

                    SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_Income set ActionCode = 4  , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where income_id = '" + income_id + "'", con);
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



        public string remove_Release_EE(int? release_id, int? ooe_id, int? subsIn)
        {
            try
            {


                if (ooe_id == 1)
                {


                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com47 = new SqlCommand(@"select FMISOfficeCode,FMISProgramCode,FMISAccountCode,MonthOf,YearOf,DateTimeEntered,AmountPS from IFMIS.dbo.tbl_R_BMS_Release where release_id = '" + release_id + "' ", con);
                        con.Open();
                        SqlDataReader reader7 = com47.ExecuteReader();
                        while (reader7.Read())
                        {
                            Release_Float_Model edit_list = new Release_Float_Model();
                            edit_list.FMISOfficeCode = reader7.GetValue(0) is DBNull ? 0 : reader7.GetInt32(0);
                            edit_list.FMISProgramCode = reader7.GetValue(1) is DBNull ? 0 : reader7.GetInt32(1);
                            edit_list.FMISAccountCode = reader7.GetValue(2) is DBNull ? 0 : reader7.GetInt32(2);
                            edit_list.MonthOf_ = reader7.GetValue(3) is DBNull ? 0 : reader7.GetInt32(3);
                            edit_list.YearOf = reader7.GetValue(4) is DBNull ? 0 : reader7.GetInt32(4);
                            edit_list.DateTimeEntered = reader7.GetValue(5).ToString();
                            edit_list.AmountPS = reader7.GetValue(6) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(6));

                            FMISOfficeCode_Rel = edit_list.FMISOfficeCode;
                            FMISProgramCode_Rel = edit_list.FMISProgramCode;
                            FMISAccountCode_Rel = edit_list.FMISAccountCode;
                            MonthOf_Rel = edit_list.MonthOf_;
                            YearOf_Rel = edit_list.YearOf;
                            DateTimeEntered_Rel = edit_list.DateTimeEntered;
                            Amount_PS_Rel = edit_list.AmountPS;

                        }

                    }

                            if (subsIn == 1) {

                                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                                {
                                    SqlCommand com = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_SubsidyRelease set ActionCode = 4, DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where FMISOfficeCode = '" + FMISOfficeCode_Rel + "' "
                                                                        + " and MonthOf = '" + MonthOf_Rel + "' and YearOf = '" + YearOf_Rel + "' "
                                                                        + " and DateTimeEntered = '" + DateTimeEntered_Rel + "'", con);
                                    con.Open();
                                    com.ExecuteNonQuery();

                                }



                                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                                {

                                    SqlCommand com4 = new SqlCommand(@"  update fmis.dbo.tblBMS_SubsidyRelease set ActionCode = 1 where FMISOfficeCode = '" + FMISOfficeCode_Rel + "' "
                                                                        + " and MonthOf = '" + MonthOf_Rel + "' and YearOf = '" + YearOf_Rel + "' "
                                                                        + " and DateTimeEntered = '" + DateTimeEntered_Rel + "'", con);
                                    con.Open();
                                    com4.ExecuteNonQuery();
                                    con.Close();
                                }

                                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                                {
                                    SqlCommand com2 = new SqlCommand("update fmis.dbo.tblBMS_Releases set ActionCode = 4 where FMISOfficeCode = '" + FMISOfficeCode_Rel + "' and ActionCode = 1 " +
                                        " and FMISProgramCode = '" + FMISProgramCode_Rel + "' and FMISAccountCode = '" + FMISAccountCode_Rel + "' " +
                                        " and AmountPS = '" + Amount_PS_Rel + "' and MonthOf = '" + MonthOf_Rel + "' and YearOf = '" + YearOf_Rel + "' and DateTimeEntered = '" + DateTimeEntered_Rel + "'", con);
                                    con.Open();
                                    com2.ExecuteNonQuery();
                                    con.Close();

                                    SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_Release set ActionCode = 4  , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where release_id = '" + release_id + "' ", con);
                                    con.Open();
                                    com.ExecuteNonQuery();
                                    return "1";
                                }
                            
                            }
                            else if (subsIn == 0) {

                                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                                {
                                    SqlCommand com = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_IncomeRelease set ActionCode = 4, DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where FMISOfficeCode = '" + FMISOfficeCode_Rel + "' "
                                                                        + " and MonthOf = '" + MonthOf_Rel + "' and YearOf = '" + YearOf_Rel + "' "
                                                                        + " and DateTimeEntered = '" + DateTimeEntered_Rel + "'", con);
                                    con.Open();
                                    com.ExecuteNonQuery();

                                }
                                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                                {

                                    SqlCommand com4 = new SqlCommand(@"  update fmis.dbo.tblBMS_IncomeRelease set ActionCode = 1 where FMISOfficeCode = '" + FMISOfficeCode_Rel + "' "
                                                                        + " and MonthOf = '" + MonthOf_Rel + "' and YearOf = '" + YearOf_Rel + "' "
                                                                        + " and DateTimeEntered = '" + DateTimeEntered_Rel + "'", con);
                                    con.Open();
                                    com4.ExecuteNonQuery();
                                    con.Close();
                                }

                                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                                {
                                    SqlCommand com2 = new SqlCommand("update fmis.dbo.tblBMS_Releases set ActionCode = 4 where FMISOfficeCode = '" + FMISOfficeCode_Rel + "' and ActionCode = 1 " +
                                        " and FMISProgramCode = '" + FMISProgramCode_Rel + "' and FMISAccountCode = '" + FMISAccountCode_Rel + "' " +
                                        " and AmountPS = '" + Amount_PS_Rel + "' and MonthOf = '" + MonthOf_Rel + "' and YearOf = '" + YearOf_Rel + "' and DateTimeEntered = '" + DateTimeEntered_Rel + "'", con);
                                    con.Open();
                                    com2.ExecuteNonQuery();
                                    con.Close();

                                    SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_Release set ActionCode = 4  , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where release_id = '" + release_id + "' ", con);
                                    con.Open();
                                    com.ExecuteNonQuery();
                                    return "1";
                                }
                            }
                            else {
                                return "2";
                            }

                  

                }
                else if (ooe_id == 2)
                {

                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com47 = new SqlCommand(@"select FMISOfficeCode,FMISProgramCode,FMISAccountCode,MonthOf,YearOf,DateTimeEntered,AmountMOOE from IFMIS.dbo.tbl_R_BMS_Release where release_id = '" + release_id + "' ", con);
                        con.Open();
                        SqlDataReader reader7 = com47.ExecuteReader();
                        while (reader7.Read())
                        {
                            Release_Float_Model edit_list = new Release_Float_Model();
                            edit_list.FMISOfficeCode = reader7.GetValue(0) is DBNull ? 0 : reader7.GetInt32(0);
                            edit_list.FMISProgramCode = reader7.GetValue(1) is DBNull ? 0 : reader7.GetInt32(1);
                            edit_list.FMISAccountCode = reader7.GetValue(2) is DBNull ? 0 : reader7.GetInt32(2);
                            edit_list.MonthOf_ = reader7.GetValue(3) is DBNull ? 0 : reader7.GetInt32(3);
                            edit_list.YearOf = reader7.GetValue(4) is DBNull ? 0 : reader7.GetInt32(4);
                            edit_list.DateTimeEntered = reader7.GetValue(5).ToString();
                            edit_list.AmountMOOE = reader7.GetValue(6) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(6));

                            FMISOfficeCode_Rel = edit_list.FMISOfficeCode;
                            FMISProgramCode_Rel = edit_list.FMISProgramCode;
                            FMISAccountCode_Rel = edit_list.FMISAccountCode;
                            MonthOf_Rel = edit_list.MonthOf_;
                            YearOf_Rel = edit_list.YearOf;
                            DateTimeEntered_Rel = edit_list.DateTimeEntered;
                            Amount_MOOE_Rel = edit_list.AmountMOOE;

                        }

                    }

                                if (subsIn == 1)
                                {

                                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                                    {
                                        SqlCommand com = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_SubsidyRelease set ActionCode = 4, DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where FMISOfficeCode = '" + FMISOfficeCode_Rel + "' "
                                                                            + " and MonthOf = '" + MonthOf_Rel + "' and YearOf = '" + YearOf_Rel + "' "
                                                                            + " and DateTimeEntered = '" + DateTimeEntered_Rel + "'", con);
                                        con.Open();
                                        com.ExecuteNonQuery();

                                    }

                                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                                    {

                                        SqlCommand com4 = new SqlCommand(@"  update fmis.dbo.tblBMS_SubsidyRelease set ActionCode = 1 where FMISOfficeCode = '" + FMISOfficeCode_Rel + "' "
                                                                            + " and MonthOf = '" + MonthOf_Rel + "' and YearOf = '" + YearOf_Rel + "' "
                                                                            + " and DateTimeEntered = '" + DateTimeEntered_Rel + "'", con);
                                        con.Open();
                                        com4.ExecuteNonQuery();
                                        con.Close();
                                    }

                                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                                    {
                                        SqlCommand com2 = new SqlCommand("update fmis.dbo.tblBMS_Releases set ActionCode = 4 where FMISOfficeCode = '" + FMISOfficeCode_Rel + "' and ActionCode = 1 " +
                                            " and FMISProgramCode = '" + FMISProgramCode_Rel + "' and FMISAccountCode = '" + FMISAccountCode_Rel + "' " +
                                            " and AmountMOOE = '" + Amount_MOOE_Rel + "' and MonthOf = '" + MonthOf_Rel + "' and YearOf = '" + YearOf_Rel + "' and DateTimeEntered = '" + DateTimeEntered_Rel + "'", con);
                                        con.Open();
                                        com2.ExecuteNonQuery();
                                        con.Close();

                                        SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_Release set ActionCode = 4  , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where release_id = '" + release_id + "' ", con);
                                        con.Open();
                                        com.ExecuteNonQuery();
                                        return "1";
                                    }
                                }
                                else if (subsIn == 0)
                                {
                                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                                    {
                                        SqlCommand com = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_IncomeRelease set ActionCode = 4, DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where FMISOfficeCode = '" + FMISOfficeCode_Rel + "' "
                                                                            + " and MonthOf = '" + MonthOf_Rel + "' and YearOf = '" + YearOf_Rel + "' "
                                                                            + " and DateTimeEntered = '" + DateTimeEntered_Rel + "'", con);
                                        con.Open();
                                        com.ExecuteNonQuery();

                                    }
                                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                                    {

                                        SqlCommand com4 = new SqlCommand(@"  update fmis.dbo.tblBMS_IncomeRelease set ActionCode = 1 where FMISOfficeCode = '" + FMISOfficeCode_Rel + "' "
                                                                            + " and MonthOf = '" + MonthOf_Rel + "' and YearOf = '" + YearOf_Rel + "' "
                                                                            + " and DateTimeEntered = '" + DateTimeEntered_Rel + "'", con);
                                        con.Open();
                                        com4.ExecuteNonQuery();
                                        con.Close();
                                    }

                                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                                    {
                                        SqlCommand com2 = new SqlCommand("update fmis.dbo.tblBMS_Releases set ActionCode = 4 where FMISOfficeCode = '" + FMISOfficeCode_Rel + "' and ActionCode = 1 " +
                                            " and FMISProgramCode = '" + FMISProgramCode_Rel + "' and FMISAccountCode = '" + FMISAccountCode_Rel + "' " +
                                            " and AmountMOOE = '" + Amount_MOOE_Rel + "' and MonthOf = '" + MonthOf_Rel + "' and YearOf = '" + YearOf_Rel + "' and DateTimeEntered = '" + DateTimeEntered_Rel + "'", con);
                                        con.Open();
                                        com2.ExecuteNonQuery();
                                        con.Close();

                                        SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_Release set ActionCode = 4  , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where release_id = '" + release_id + "' ", con);
                                        con.Open();
                                        com.ExecuteNonQuery();
                                        return "1";
                                    }
                                }
                                else
                                {
                                    return "2";
                                }
                   



                }
                else if (ooe_id == 3)
                {

                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com47 = new SqlCommand(@"select FMISOfficeCode,FMISProgramCode,FMISAccountCode,MonthOf,YearOf,DateTimeEntered,AmountCO from IFMIS.dbo.tbl_R_BMS_Release where release_id = '" + release_id + "' ", con);
                        con.Open();
                        SqlDataReader reader7 = com47.ExecuteReader();
                        while (reader7.Read())
                        {
                            Release_Float_Model edit_list = new Release_Float_Model();
                            edit_list.FMISOfficeCode = reader7.GetValue(0) is DBNull ? 0 : reader7.GetInt32(0);
                            edit_list.FMISProgramCode = reader7.GetValue(1) is DBNull ? 0 : reader7.GetInt32(1);
                            edit_list.FMISAccountCode = reader7.GetValue(2) is DBNull ? 0 : reader7.GetInt32(2);
                            edit_list.MonthOf_ = reader7.GetValue(3) is DBNull ? 0 : reader7.GetInt32(3);
                            edit_list.YearOf = reader7.GetValue(4) is DBNull ? 0 : reader7.GetInt32(4);
                            edit_list.DateTimeEntered = reader7.GetValue(5).ToString();
                            edit_list.AmountCO = reader7.GetValue(6) is DBNull ? 0 : Convert.ToDouble(reader7.GetValue(6));

                            FMISOfficeCode_Rel = edit_list.FMISOfficeCode;
                            FMISProgramCode_Rel = edit_list.FMISProgramCode;
                            FMISAccountCode_Rel = edit_list.FMISAccountCode;
                            MonthOf_Rel = edit_list.MonthOf_;
                            YearOf_Rel = edit_list.YearOf;
                            DateTimeEntered_Rel = edit_list.DateTimeEntered;
                            Amount_CO_Rel = edit_list.AmountCO;

                        }

                    }

                                if (subsIn == 1)
                                {

                                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                                    {
                                        SqlCommand com = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_SubsidyRelease set ActionCode = 4, DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where FMISOfficeCode = '" + FMISOfficeCode_Rel + "' "
                                                                            + " and MonthOf = '" + MonthOf_Rel + "' and YearOf = '" + YearOf_Rel + "' "
                                                                            + " and DateTimeEntered = '" + DateTimeEntered_Rel + "'", con);
                                        con.Open();
                                        com.ExecuteNonQuery();

                                    }


                                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                                    {

                                        SqlCommand com4 = new SqlCommand(@"  update fmis.dbo.tblBMS_SubsidyRelease set ActionCode = 1 where FMISOfficeCode = '" + FMISOfficeCode_Rel + "' "
                                                                            + " and MonthOf = '" + MonthOf_Rel + "' and YearOf = '" + YearOf_Rel + "' "
                                                                            + " and DateTimeEntered = '" + DateTimeEntered_Rel + "'", con);
                                        con.Open();
                                        com4.ExecuteNonQuery();
                                        con.Close();
                                    }

                                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                                    {
                                        SqlCommand com2 = new SqlCommand("update fmis.dbo.tblBMS_Releases set ActionCode = 4 where FMISOfficeCode = '" + FMISOfficeCode_Rel + "' and ActionCode = 1 " +
                                            " and FMISProgramCode = '" + FMISProgramCode_Rel + "' and FMISAccountCode = '" + FMISAccountCode_Rel + "' " +
                                            " and AmountCO = '" + Amount_CO_Rel + "' and MonthOf = '" + MonthOf_Rel + "' and YearOf = '" + YearOf_Rel + "' and DateTimeEntered = '" + DateTimeEntered_Rel + "'", con);
                                        con.Open();
                                        com2.ExecuteNonQuery();
                                        con.Close();

                                        SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_Release set ActionCode = 4  , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where release_id = '" + release_id + "' ", con);
                                        con.Open();
                                        com.ExecuteNonQuery();
                                        return "1";
                                    }
                                }
                                else if (subsIn == 0)
                                {

                                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                                    {
                                        SqlCommand com = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_IncomeRelease set ActionCode = 4, DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where FMISOfficeCode = '" + FMISOfficeCode_Rel + "' "
                                                                            + " and MonthOf = '" + MonthOf_Rel + "' and YearOf = '" + YearOf_Rel + "' "
                                                                            + " and DateTimeEntered = '" + DateTimeEntered_Rel + "'", con);
                                        con.Open();
                                        com.ExecuteNonQuery();

                                    }
                                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                                    {

                                        SqlCommand com4 = new SqlCommand(@"  update fmis.dbo.tblBMS_IncomeRelease set ActionCode = 1 where FMISOfficeCode = '" + FMISOfficeCode_Rel + "' "
                                                                            + " and MonthOf = '" + MonthOf_Rel + "' and YearOf = '" + YearOf_Rel + "' "
                                                                            + " and DateTimeEntered = '" + DateTimeEntered_Rel + "'", con);
                                        con.Open();
                                        com4.ExecuteNonQuery();
                                        con.Close();
                                    }

                                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                                    {
                                        SqlCommand com2 = new SqlCommand("update fmis.dbo.tblBMS_Releases set ActionCode = 4 where FMISOfficeCode = '" + FMISOfficeCode_Rel + "' and ActionCode = 1 " +
                                            " and FMISProgramCode = '" + FMISProgramCode_Rel + "' and FMISAccountCode = '" + FMISAccountCode_Rel + "' " +
                                            " and AmountCO = '" + Amount_CO_Rel + "' and MonthOf = '" + MonthOf_Rel + "' and YearOf = '" + YearOf_Rel + "' and DateTimeEntered = '" + DateTimeEntered_Rel + "'", con);
                                        con.Open();
                                        com2.ExecuteNonQuery();
                                        con.Close();

                                        SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_Release set ActionCode = 4  , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), UserID = CONCAT(UserID,' , ', '" + Account.UserInfo.eid.ToString() + "') where release_id = '" + release_id + "' ", con);
                                        con.Open();
                                        com.ExecuteNonQuery();
                                        return "1";
                                    }
                                }
                                else
                                {
                                    return "2";
                                }
                  


                }
                else
                {
                    return "100";
                }



            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public string get_AccountName(int? program_id, int? ooe_id, int? account_id, int? year_)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT distinct AccountName FROM IFMIS.dbo.tbl_R_BMSProgramAccounts where AccountYear = '" + year_ + "' and ObjectOfExpendetureID = '" + ooe_id + "' and ProgramID = '" + program_id + "' and AccountID = '" + account_id + "' and actioncode = 1", con);
                con.Open();
                return com.ExecuteScalar().ToString();


            }
        }
        public string get_AccountName_realignment(int? program_id, int? account_id, int? year_)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT distinct AccountName FROM IFMIS.dbo.tbl_R_BMSProgramAccounts where AccountYear = '" + year_ + "' and ProgramID = '" + program_id + "' and AccountID = '" + account_id + "' and actioncode = 1", con);
                con.Open();
                return com.ExecuteScalar().ToString();


            }
        }

        public IEnumerable<Projected_RevenuesModel> Projected_Revenues()
        {
            List<Projected_RevenuesModel> legal_basis = new List<Projected_RevenuesModel>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT Particular ,Amount_1 ,Amount_2 ,Amount_3 ,Amount_4 FROM IFMIS.dbo.tbl_R_BMS_Projected_Revenues where year_of = 2017 and action_code = 1 and type_of = 1", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Projected_RevenuesModel PR = new Projected_RevenuesModel();
                    PR.Particular = reader.GetValue(0).ToString();
                    PR.Amount_1 = reader.GetValue(1) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(1));
                    PR.Amount_2 = reader.GetValue(2) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(2));
                    PR.Amount_3 = reader.GetValue(3) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(3));
                    PR.Amount_4 = reader.GetValue(4) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(4));



                    legal_basis.Add(PR);

                }
            }
            return legal_basis;
        }

        public IEnumerable<Projected_RevenuesModel> Projected_Revenues_2()
        {
            List<Projected_RevenuesModel> legal_basis = new List<Projected_RevenuesModel>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT Particular ,Amount_1 ,Amount_2 ,Amount_3 ,Amount_4 FROM IFMIS.dbo.tbl_R_BMS_Projected_Revenues where year_of = 2017 and action_code = 1 and type_of = 2", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Projected_RevenuesModel PR = new Projected_RevenuesModel();
                    PR.Particular = reader.GetValue(0).ToString();
                    PR.Amount_1 = reader.GetValue(1) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(1));
                    PR.Amount_2 = reader.GetValue(2) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(2));
                    PR.Amount_3 = reader.GetValue(3) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(3));
                    PR.Amount_4 = reader.GetValue(4) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(4));



                    legal_basis.Add(PR);

                }
            }
            return legal_basis;
        }

        public void PR_Create(Projected_RevenuesModel pr)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com2 = new SqlCommand(@" insert into IFMIS.dbo.tbl_R_BMS_Projected_Revenues values ('" + pr.Particular + "'," + pr.Amount_1 + "," + pr.Amount_2 + "," + pr.Amount_3 + "," + pr.Amount_4 + ",2017,1,69)", con);
                con.Open();
                com2.ExecuteNonQuery();
                con.Close();
                //SqlCommand com = new SqlCommand(@"  insert into IFMIS.dbo.tbl_R_BMS_LegalBasis (LegalCode,LegalDescription,UserID,ActionCode,DateTimeEntered,YearOf)values ('" + legalbasis.LegalCode + "','" + legalbasis.LegalDescription + "','" + Account.UserInfo.eid.ToString() + "',1,format(GetDate(),'M/d/yyy hh:mm:ss tt')," + legalbasis.YearOf + ")", con);
                //con.Open();
                //com.ExecuteNonQuery();
            }
        }

        public void PR_Create_2(Projected_RevenuesModel pr)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com2 = new SqlCommand(@" insert into IFMIS.dbo.tbl_R_BMS_Projected_Revenues values ('" + pr.Particular + "'," + pr.Amount_1 + "," + pr.Amount_2 + "," + pr.Amount_3 + "," + pr.Amount_4 + ",2017,1,69)", con);
                con.Open();
                com2.ExecuteNonQuery();
                con.Close();
                //SqlCommand com = new SqlCommand(@"  insert into IFMIS.dbo.tbl_R_BMS_LegalBasis (LegalCode,LegalDescription,UserID,ActionCode,DateTimeEntered,YearOf)values ('" + legalbasis.LegalCode + "','" + legalbasis.LegalDescription + "','" + Account.UserInfo.eid.ToString() + "',1,format(GetDate(),'M/d/yyy hh:mm:ss tt')," + legalbasis.YearOf + ")", con);
                //con.Open();
                //com.ExecuteNonQuery();
            }
        }




        public void pr_Update(Projected_RevenuesModel pr)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com2 = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_Projected_Revenues set action_code = 3 where Projected_Revenue_ID = '" + pr.Projected_Revenue_ID + "'", con);
                con.Open();
                com2.ExecuteNonQuery();
                con.Close();
                SqlCommand com = new SqlCommand(@"insert into IFMIS.dbo.tbl_R_BMS_Projected_Revenues values ('" + pr.Particular + "'," + pr.Amount_1 + "," + pr.Amount_2 + "," + pr.Amount_3 + "," + pr.Amount_4 + ",2017,1,69)", con);
                con.Open();
                com.ExecuteNonQuery();
            }
        }

        public void pr_Update_2(Projected_RevenuesModel pr)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com2 = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_Projected_Revenues set action_code = 3 where Projected_Revenue_ID = '" + pr.Projected_Revenue_ID + "'", con);
                con.Open();
                com2.ExecuteNonQuery();
                con.Close();
                SqlCommand com = new SqlCommand(@"insert into IFMIS.dbo.tbl_R_BMS_Projected_Revenues values ('" + pr.Particular + "'," + pr.Amount_1 + "," + pr.Amount_2 + "," + pr.Amount_3 + "," + pr.Amount_4 + ",2017,1,69)", con);
                con.Open();
                com.ExecuteNonQuery();
            }
        }

        public void pr_Destroy(Projected_RevenuesModel pr)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com2 = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_Projected_Revenues set action_code = 3 where Projected_Revenue_ID = '" + pr.Projected_Revenue_ID + "'", con);
                con.Open();
                com2.ExecuteNonQuery();
              
            }
        }

        public void pr_Destroy_2(Projected_RevenuesModel pr)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com2 = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_Projected_Revenues set action_code = 3 where Projected_Revenue_ID = '" + pr.Projected_Revenue_ID + "'", con);
                con.Open();
                com2.ExecuteNonQuery();

            }
        }



        //public Monthly_Realignment_Model numeric_desuni(int? office_id, int? program_id, int? ooe_id, int? account_id, int? month_, int? numeric_, int? year_)
        //{

        //    using (SqlConnection con = new SqlConnection(Common.MyConn()))
        //    {
        //        SqlCommand com = new SqlCommand(@"dbo.sp_MonthlyRelease_Float_release '" + office_id + "','" + program_id + "','" + ooe_id + "','" + account_id + "','" + month_ + "','" + numeric_ + "','" + year_ + "'", con);



        //        con.Open();
        //        SqlDataReader reader = com.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            Monthly_Realignment_Model Float = new Monthly_Realignment_Model();

        //            Float.balance_amount_ps = reader.GetValue(0) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(0));
        //            Float.balance_amount_mooe = reader.GetValue(1) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(1));
        //            Float.balance_amount_co = reader.GetValue(2) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(2));
        //            Float.available_amount = reader.GetValue(3) is DBNull ? 0 : Convert.ToDouble(reader.GetValue(3));

        //            return Float;
        //        }
        //    }
        //    return new Monthly_Realignment_Model();
        //}

        public string numeric_desuni(int? office_id, int? program_id, int? account_id, int? year_, int? month_)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"select isnull((SELECT top 1 Batch FROM [IFMIS].[dbo].[tbl_R_BMS_Release] where FMISOfficeCode = '" + office_id + "' and yearof = '" + year_ + "' and ActionCode = 1 and MonthOf = '" + month_ + "' order by Batch desc),0) as Batch", con);
              
                con.Open();
                return com.ExecuteScalar().ToString();


            }
        }
        public string numeric_desuniNon(int? office_id, int? program_id, int? account_id, int? year_, int? month_)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"select isnull((SELECT top 1 Batch FROM [IFMIS].[dbo].[tbl_R_BMS_Release] where FMISOfficeCode = 43 and yearof = '" + year_ + "' and ActionCode = 1 and MonthOf = '" + month_ + "' order by release_id desc),0) as Batch", con);

                con.Open();
                return com.ExecuteScalar().ToString();


            }
        }
        public string getMax_desu(int? office_id, int? month_, int? year_)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                SqlCommand com = new SqlCommand(@"select Top 1 Batch FROM IFMIS.dbo.tbl_R_BMS_Release where FMISOfficeCode = 4 and YearOf = 2017 and MonthOf = 3 and actioncode = 1 order by Batch desc", con);

                con.Open();
                return com.ExecuteScalar().ToString();


            }
        }

        public IEnumerable<FundModel> FundType_Desu()
        {
            List<FundModel> pross = new List<FundModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select FundFlag , CONCAT(FundName, ' ' , '(' +FundMedium +')') as FundName FROM [IFMIS].[dbo].[tbl_R_BMSFunds] where FundFlag not in (4,5)", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    FundModel app = new FundModel();
                    app.FundFlag = reader.GetValue(0) is DBNull ? 0 : reader.GetInt32(0);
                    app.FundName = reader.GetString(1);

                    pross.Add(app);
                }
            }
            return pross;
        }


        public IEnumerable<Release_Float_Edit> Changed_To(int? fund_type)
        {
            List<Release_Float_Edit> pross = new List<Release_Float_Edit>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select [TransactionNo], [Account] FROM [IFMIS].[dbo].[tbl_T_BMSExcessAppropriation] where FundFlag = '" + fund_type + "' and ActionCode=1  order by [TransactionNo] desc", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Release_Float_Edit app = new Release_Float_Edit();
                    app.TransactionNo = reader.GetString(0);
                    app.Account_Name = reader.GetString(1);

                    pross.Add(app);
                }
            }
            return pross;
        }



        public string addNewSupplement(string supplement_souce, int? month_, int? year_, double source_amount)
        {
            try
            {


                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com2 = new SqlCommand("insert into [IFMIS].[dbo].[tbl_R_BMS_SupplementalSource] ([supplement_souce] ,[ActionCode] ,[DateTimeEntered]) values ('" + supplement_souce + "',1,GETDATE())", con);
                    con.Open();
                    com2.ExecuteNonQuery();
                    con.Close();
                    SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_SupplementalReverse (SBCode,LegalCode,FMISOfficeCode,FMISProgramCode,FMISAccountCode,MonthOf,YearOf,ActionCode,UserID,DateTimeEntered,Amount,type_) " +
                                             "values ('0','0','0','0','0','" + month_ + "','" + year_ + "',1,'" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),'" + source_amount + "',2) ", con);
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
        public string addSupplement_Amount(int? supplement_source_ID, string supplement_souce, int? month_, int? year_, double source_amount)
        {
            try
            {


                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {

                    SqlCommand com = new SqlCommand("insert into IFMIS.dbo.tbl_R_BMS_SupplementalReverse (SBCode,LegalCode,FMISOfficeCode,FMISProgramCode,FMISAccountCode,MonthOf,YearOf,ActionCode,UserID,DateTimeEntered,Amount,type_) " +
                                            "values ('0','0','0','0','0','" + month_ + "','" + year_ + "',1,'" + Account.UserInfo.eid.ToString() + "',format(GetDate(),'M/d/yyy hh:mm:ss tt'),'" + source_amount + "',2) ", con);
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
        public IEnumerable<LBPOne_SourceFundsModel> GetSourcesSupplement()
        {
            List<LBPOne_SourceFundsModel> SourceFund = new List<LBPOne_SourceFundsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT supplement_source_ID,supplement_souce FROM IFMIS.dbo.tbl_R_BMS_SupplementalSource WHERE ActionCode = 1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    LBPOne_SourceFundsModel SF_list = new LBPOne_SourceFundsModel();
                    SF_list.supplement_source_ID = reader.GetInt64(0);
                   
                    SF_list.supplement_souce = reader.GetValue(1).ToString();
                  
                    SourceFund.Add(SF_list);
                }
            }
            return SourceFund;
        }


        public string release_edit__(int? ID_, double EDIT_Amount, int? ooe_id, int progid, long accountid, int yearof, string DateEntered, int EDIT_Batch, string releasedate,int? relcopydatetag)
        {
            try
            {
                if (ooe_id == 1 || ooe_id == 2 || ooe_id == 3) { 
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {

                        SqlCommand com = new SqlCommand("exec sp_BMS_ReleaseUpdate "+ ID_ + ","+ EDIT_Amount + ","+ ooe_id + ","+ progid  + ","+ accountid + ","+ yearof + ",'"+ DateEntered + "',"+ Account.UserInfo.eid.ToString() + ","+ EDIT_Batch + ",'"+ releasedate + "',"+ relcopydatetag + "", con);
                        con.Open();
                        com.ExecuteNonQuery();
                    
                        return "1";
                    }
                }
                else {
                    return "2";
                }
                
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public IEnumerable<LegalBasis_Model> LegalBasisRelRev()
        {
            List<LegalBasis_Model> legal_basis = new List<LegalBasis_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT [Legal_id],[LegalCode] ,LegalDescription,YearOf FROM IFMIS.dbo.[tbl_R_BMS_LegalBasisRR] where ActionCode = 1 order by Legal_id desc", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    LegalBasis_Model legal_list = new LegalBasis_Model();
                    legal_list.Legal_id = reader.GetInt64(0);
                    legal_list.LegalCode = reader.GetInt64(1);
                    legal_list.LegalDescription = reader.GetValue(2).ToString();
                    legal_list.YearOf = reader.GetInt32(3);



                    legal_basis.Add(legal_list);

                }
            }
            return legal_basis;
        }
        public void legalRR_Create(LegalBasis_Model legalbasis)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                //SqlCommand com2 = new SqlCommand(@"insert into fmis.dbo.tbl_R_BMS_LegalBasisRR (LegalCode,LegalDescription,UserID,ActionCode,DateTimeEntered,YearOf)values ('" + legalbasis.LegalCode + "','" + legalbasis.LegalDescription + "','" + Account.UserInfo.eid.ToString() + "',1,format(GetDate(),'M/d/yyy hh:mm:ss tt')," + legalbasis.YearOf + ")", con);
                //con.Open();
                //com2.ExecuteNonQuery();
                con.Close();
                SqlCommand com = new SqlCommand(@"exec LegalBasis_ReleaseRev " + legalbasis.LegalCode + ",'" + legalbasis.LegalDescription + "'," + Account.UserInfo.eid.ToString() + "," + legalbasis.YearOf + "", con);
                con.Open();
                com.ExecuteNonQuery();
            }
        }
        public void legalRR_Update(LegalBasis_Model legalbasis)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com47 = new SqlCommand(@"SELECT [LegalCode] ,CONCAT(LegalDescription,' (',REPLACE(LegalCode, ' ', ''),')'),LegalDescription,[LegalBasisID] FROM IFMIS.dbo.tbl_R_BMS_LegalBasisRR where ActionCode = 1 and Legal_id =" + legalbasis.Legal_id + "", con);
                con.Open();
                SqlDataReader reader7 = com47.ExecuteReader();
                while (reader7.Read())
                {
                    LegalBasis_Model legalbasis_list = new LegalBasis_Model();
                    legalbasis_list.LegalCode = reader7.GetInt64(0);
                    legalbasis_list.LegalDescription = reader7.GetString(1);
                    legalbasis_list.LegalDescriptionOther = reader7.GetString(2);
                    legalbasis_list.LegalbasisID= reader7.GetInt32(3);

                    LegalCode_ = legalbasis_list.LegalCode;
                    LegalDescription_ = legalbasis_list.LegalDescription;
                    LegalDescriptionOther = legalbasis_list.LegalDescriptionOther;
                    LegalbasisID = legalbasis_list.LegalbasisID;
                }

            }


            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com3 = new SqlCommand(@"update FMIS.dbo.tbl_R_BMS_LegalBasisRR set ActionCode = 2 where [LegalCode] = " + LegalCode_ + " and LegalDescription = '" + LegalDescriptionOther + "' and actioncode=1", con);
                con.Open();
                com3.ExecuteNonQuery();
                con.Close();
                SqlCommand com24 = new SqlCommand(@"insert into fmis.dbo.tbl_R_BMS_LegalBasisRR (LegalCode,LegalDescription,UserID,ActionCode,DateTimeEntered,YearOf)values (" + legalbasis.LegalCode + ",'" + legalbasis.LegalDescription + "','" + Account.UserInfo.eid.ToString() + "',1,format(GetDate(),'M/d/yyy hh:mm:ss tt')," + legalbasis.YearOf + ")", con);
                con.Open();
                com24.ExecuteNonQuery();
                con.Close();

                SqlCommand com = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_LegalBasisRR set ActionCode = 2 , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), userID = CONCAT(userID,' , ', '" + Account.UserInfo.eid.ToString() + "')  where Legal_id = '" + legalbasis.Legal_id + "'", con);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
                SqlCommand com2 = new SqlCommand(@"insert into IFMIS.dbo.tbl_R_BMS_LegalBasisRR (LegalCode,LegalDescription,UserID,ActionCode,DateTimeEntered,YearOf,[LegalBasisID])values (" + legalbasis.LegalCode + ",'" + legalbasis.LegalDescription + "','" + Account.UserInfo.eid.ToString() + "',1,format(GetDate(),'M/d/yyy hh:mm:ss tt')," + legalbasis.YearOf + ","+ LegalbasisID + ")", con);
                con.Open();
                com2.ExecuteNonQuery();
            }

        }
        public void legalRR_Destroy(LegalBasis_Model legalbasis)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com47 = new SqlCommand(@"SELECT LegalCode ,CONCAT(LegalDescription,' (',REPLACE(LegalCode, ' ', ''),')'),LegalDescription FROM IFMIS.dbo.tbl_R_BMS_LegalBasisRR where ActionCode = 1 and Legal_id =" + legalbasis.Legal_id + "", con);
                con.Open();
                SqlDataReader reader7 = com47.ExecuteReader();
                while (reader7.Read())
                {
                    LegalBasis_Model legalbasis_list = new LegalBasis_Model();
                    legalbasis_list.LegalCode = reader7.GetInt64(0);
                    legalbasis_list.LegalDescription = reader7.GetString(1);
                    legalbasis_list.LegalDescriptionOther = reader7.GetString(2);

                    LegalCode_ = legalbasis_list.LegalCode;
                    LegalDescription_ = legalbasis_list.LegalDescription;
                    LegalDescriptionOther = legalbasis_list.LegalDescriptionOther;
                }

            }



            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com3 = new SqlCommand(@"update FMIS.dbo.tbl_R_BMS_LegalBasisRR set ActionCode = 4 where LegalCode = '" + LegalCode_ + "' and LegalDescription = '" + LegalDescriptionOther + "'", con);
                con.Open();
                com3.ExecuteNonQuery();
                con.Close();
                SqlCommand com = new SqlCommand(@"update IFMIS.dbo.tbl_R_BMS_LegalBasisRR set ActionCode = 4 , DateTimeEntered = CONCAT(DateTimeEntered,' , ',format(GetDate(),'M/d/yyy hh:mm:ss tt')), userID = CONCAT(userID,' , ', '" + Account.UserInfo.eid.ToString() + "')  where Legal_id = '" + legalbasis.Legal_id + "'", con);
                con.Open();
                com.ExecuteNonQuery();

            }
        }
        public IEnumerable<LegalBasis_Model> LegalBasis_RR()
        {
            List<LegalBasis_Model> LegalBasisList = new List<LegalBasis_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT LegalBasisID ,LegalDescription FROM IFMIS.dbo.tbl_R_BMS_LegalBasisRR where ActionCode = 1 order by Legal_id desc", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    LegalBasis_Model legalbasis_list = new LegalBasis_Model();
                    legalbasis_list.LegalbasisID = reader.GetInt32(0);
                    legalbasis_list.LegalDescription = reader.GetString(1);

                    LegalBasisList.Add(legalbasis_list);
                }
            }
            return LegalBasisList;
        }
        public IEnumerable<Monthly_DD_Model> ReleaseReversionGrid(int? year_=0,int? office=0, long? account = 0,int? reltag=0)
        {

            List<Monthly_DD_Model> Float_List = new List<Monthly_DD_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                //SqlCommand com = new SqlCommand(@" select a.[release_id] as [tbl_revid],isnull([AmountPS],0) +isnull([AmountMOOE],0)+isnull([AmountCO],0) as Amount,a.monthof,a.YearOf,c.AccountName,[DateReleased] as datetimentered FROM IFMIS.dbo.[tbl_R_BMS_Release] as a
                //                                            left join IFMIS.dbo.tbl_R_BMSOffices as b
                //                                            on a.[FMISOfficeCode] = b.OfficeID
                //                                            left join IFMIS.dbo.tbl_R_BMSProgramAccounts as c
                //                                            on a.[FMISProgramCode] = c.ProgramID and a.[FMISAccountCode] = c.AccountID 
                //                                            and a.ActionCode = c.ActionCode and a.YearOf = c.AccountYear " +
                //                                           " where a.YearOf = "+ year_ + " and a.ActionCode = 1 and a.[FMISOfficeCode] = "+ office + " and a.[FMISAccountCode] = "+ account + " "+
                //                                           " order by a.[release_id]", con);
                SqlCommand com = new SqlCommand(@" exec [sp_BMS_ReleaseReversion_Read] "+ office + ","+ account  + ","+ year_ + ","+ reltag + "", con);


                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_DD_Model Float = new Monthly_DD_Model();
                    Float.releasereversion_id = reader.GetInt64(0);
                    Float.Amount = Convert.ToDouble(reader.GetValue(1));
                    Float.MonthOf = reader.GetValue(2).ToString();
                    Float.YearOf = reader.GetInt32(3);
                    Float.AccountName = reader.GetValue(4) is DBNull ? "" : reader.GetValue(4).ToString();
                    Float.dtetime = reader.GetValue(5).ToString();
                    Float.reltag= reader.GetInt32(6);
                    Float.legalbasisid = reader.GetInt32(7);
                    Float.isfloat = reader.GetInt32(8);
                    Float_List.Add(Float);

                }
            }
            return Float_List;
        }
        public string subpparealignAvailable(int? office_id=0, int? program_id=0, int? ooe_id=0, int? account_id=0, int? year_=0,int? subppaid=0)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (account_id != 0)
                {
                    if (subppaid == 0)
                    {
                        //SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_available_amount] '" + office_id + "','" + program_id + "',0,'" + account_id + "','" + year_ + "'", con);
                        SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_availablewithwfp] '" + office_id + "','" + program_id + "',0,'" + account_id + "','" + year_ + "'", con);
                        con.Open();
                        return com.ExecuteScalar().ToString();
                    }
                    else
                    {
                        SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_BMS_RealignSubPPABalance] " + office_id + "," + program_id + "," + account_id + "," + year_ + "," + subppaid + "", con);
                        //  SqlCommand com = new SqlCommand(@"sp_MonthlyRelease_available_amount 4,4,1,401,2016", con);
                        con.Open();
                        return com.ExecuteScalar().ToString();
                    }
                }

            }
            return "";
        }

        public string subppareversionAvailable(int? office_id = 0, int? program_id = 0, int? ooe_id = 0, int? account_id = 0, int? year_ = 0, int? subppaid = 0)
        {

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                if (account_id != 0)
                {
                    if (subppaid == 0)
                    {
                        SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_MonthlyRelease_available_amount] '" + office_id + "','" + program_id + "',0,'" + account_id + "','" + year_ + "'", con);
                        //  SqlCommand com = new SqlCommand(@"sp_MonthlyRelease_available_amount 4,4,1,401,2016", con);
                        con.Open();
                        return com.ExecuteScalar().ToString();
                    }
                    else
                    {
                        SqlCommand com = new SqlCommand(@"[IFMIS].[dbo].[sp_BMS_ReversionSubPPABalance] " + office_id + "," + program_id + "," + account_id + "," + year_ + "," + subppaid + "", con);
                        //  SqlCommand com = new SqlCommand(@"sp_MonthlyRelease_available_amount 4,4,1,401,2016", con);
                        con.Open();
                        return com.ExecuteScalar().ToString();
                    }
                }

            }
            return "";
        }
        public IEnumerable<Monthly_DD_Model> Account_RealignFrom(int? office_ID_from, int? prog_id_from, int? year_, int? office_ID_to, int? prog_id_to, int? account_id_to, int? ooe_id_from, int? realignid)
        {
            List<Monthly_DD_Model> account_list = new List<Monthly_DD_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.[sp_MonthlyRealignment_Accounts] " + office_ID_from + ", " + prog_id_from + ",  " + year_ + ", " + office_ID_to + ", " + prog_id_to + ",  " + account_id_to + ","+ ooe_id_from + ","+ realignid + " ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Monthly_DD_Model app = new Monthly_DD_Model();

                    app.account_id = reader.GetInt32(0);
                    app.account_name = reader.GetValue(1).ToString();

                    account_list.Add(app);
                }
            }
            return account_list;
        }
    }
}