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
    public class SooNon_Layer
    {
                      
                       int FMISProgramCode = 0;
                       int   OOECode = 0;
                       int  FMISAccountCode = 0;
                       int  Yearof = 0;
                       double AllotedAmount = 0;
        public IEnumerable<SOONon_Model> SOONON(int? propYear)
        {


            List<SOONon_Model> OthersList = new List<SOONon_Model>();

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT distinct a.trnno, a.FMISProgramCode,a.OOECode,a.FMISAccountCode,a.BudgetAcctName,a.Yearof, b.AllotedAmount, c.Amount
                                                  FROM [fmis].[dbo].[tblBMS_AnnualBudget_Account] as a inner join [fmis].[dbo].[tblBMS_AnnualBudget] as b 
                                                  on a.[FmisProgramCode]= b.[FmisProgramCode] and a.FMISAccountCode = b.FMISAccountCode and a.ActionCode = b.ActionCode and a.YearOf = b.YearOf
                                                  inner join fmis.dbo.tblBMS_NonOfficeAppropriation as c on a.FMISAccountCode = c.FmisAccountID
                                                  where a.Yearof = '" + propYear  + "' and a.FMISProgramCode in (SELECT FmisProgramCode  FROM [fmis].[dbo].[tblRefBMS_BudgetProgram] where Yearof = '" + propYear  + "' and ActionCode = 1 and FmisOfficeCode = 43) and a.ActionCode = 1 and c.FmisOfficeID = '" + Account.UserInfo.Department + "' and c.YearOf='" + propYear + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    SOONon_Model Others = new SOONon_Model();
                    Others.trnno = reader.GetInt64(0);
                    Others.FMISProgramCode = reader.GetInt32(1);
                    Others.OOECode = reader.GetInt32(2);
                    Others.FMISAccountCode = reader.GetInt32(3);
                    Others.BudgetAcctName = reader.GetValue(4).ToString();
                    Others.Yearof = reader.GetInt32(5);
                    Others.AllotedAmount = Convert.ToDouble(reader.GetValue(6));
                    Others.SooAmount = Convert.ToDouble(reader.GetValue(7));

                    OthersList.Add(Others);
                }

            }


            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT trnno,FMISProgramCode,OOECode,FMISAccountCode,BudgetAcctName,Yearof,AllotedAmount,SooAmount,ActionCode,DateInOut,OfficeID FROM IFMIS.dbo.tbl_R_BMS_SOONONoffice where OfficeID = '" + Account.UserInfo.Department + "'  and ActionCode = 1 and Yearof= '" + propYear + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    SOONon_Model Others = new SOONon_Model();
                    Others.trnno = reader.GetInt64(0);
                    Others.FMISProgramCode = reader.GetInt32(1);
                    Others.OOECode = reader.GetInt32(2);
                    Others.FMISAccountCode = reader.GetInt32(3);
                    Others.BudgetAcctName = reader.GetValue(4).ToString();
                    Others.Yearof = reader.GetInt32(5);
                    Others.AllotedAmount = Convert.ToDouble(reader.GetValue(6));
                    Others.SooAmount = Convert.ToDouble(reader.GetValue(7));







                    OthersList.Add(Others);
                }

            }




            return OthersList;
        }







        public void SOONONUPS(SOONon_Model product, int? propYear)
        {
            //var entity = new SOONon_Model();
            try
            {
              
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_SOONONoffice set DateInOut = CONCAT(DateInOut,' , ', GETDATE()), ActionCode = '2' where trnno = '" + product.trnno + "' and officeiD = '" + Account.UserInfo.Department + "' and Yearof = '" + product.Yearof + "' and ActionCode = 1", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    SqlCommand com2 = new SqlCommand(" insert into IFMIS.dbo.tbl_R_BMS_SOONONoffice (trnno,FMISProgramCode,OOECode,FMISAccountCode,BudgetAcctName,Yearof,AllotedAmount,SooAmount,ActionCode,DateInOut,OfficeID) values ('" + product.trnno + "','" + product.FMISProgramCode + "','" + product.OOECode + "','" + product.FMISAccountCode + "','" + product.BudgetAcctName + "','" + product.Yearof + "','" + product.AllotedAmount + "',"+product.SooAmount+",1,GETDATE(),'" + Account.UserInfo.Department + "')", con);
                   
                    com2.ExecuteNonQuery();
                    
                }
            }
            catch (Exception)
            {
                
            }
        }


        public IEnumerable<SOONon_Model> NonType(int? propYear)
         {
             List<SOONon_Model> OthersList = new List<SOONon_Model>();
             using (SqlConnection con = new SqlConnection(Common.MyConn()))
             {
                 SqlCommand com = new SqlCommand(@"select trnno,BudgetAcctName from [fmis].[dbo].[tblBMS_AnnualBudget_Account] where Yearof = '" + propYear + "' and FMISProgramCode in (SELECT FmisProgramCode  FROM [fmis].[dbo].[tblRefBMS_BudgetProgram] where Yearof = '" + propYear + "' and ActionCode = 1 and FmisOfficeCode = 43) and ActionCode = 1 order by FMISAccountCode", con);
                 con.Open();
                 SqlDataReader reader = com.ExecuteReader();
                 while (reader.Read())
                 {
                     SOONon_Model Others = new SOONon_Model();
                     Others.trnno = reader.GetInt64(0);
                     Others.BudgetAcctName = reader.GetValue(1).ToString();

                    


                     OthersList.Add(Others);
                 }
             }
             return OthersList;
         }

         public string AddNon(int? trnno, string BudgetAcctName, int? Year)
         {

             try
             {
                 using (SqlConnection con = new SqlConnection(Common.MyConn()))
                 {

                     SqlCommand com2 = new SqlCommand("SELECT a.trnno, a.FMISProgramCode,a.OOECode,a.FMISAccountCode,a.BudgetAcctName,a.Yearof, b.AllotedAmount " +
                                                          "FROM [fmis].[dbo].[tblBMS_AnnualBudget_Account] as a inner join [fmis].[dbo].[tblBMS_AnnualBudget] as b "+
                                                          "on a.[FmisProgramCode]= b.[FmisProgramCode] and a.FMISAccountCode = b.FMISAccountCode and a.ActionCode = b.ActionCode and a.YearOf = b.YearOf " +
                                                          "where a.Yearof = '" + Year + "' and a.FMISProgramCode in (SELECT FmisProgramCode  FROM [fmis].[dbo].[tblRefBMS_BudgetProgram] where Yearof = '" + Year + "' and ActionCode = 1 and FmisOfficeCode = 43) and a.ActionCode = 1 and a.trnno = '" + trnno + "'", con);
                     con.Open();
                     SqlDataReader reader = com2.ExecuteReader();
                     while (reader.Read())
                     {
                         SOONon_Model Others = new SOONon_Model();
                         Others.trnno = reader.GetInt64(0);
                         Others.FMISProgramCode = reader.GetInt32(1);
                         Others.OOECode = reader.GetInt32(2);
                         Others.FMISAccountCode = reader.GetInt32(3);
                         Others.BudgetAcctName = reader.GetValue(4).ToString();
                         Others.Yearof = reader.GetInt32(5);
                         Others.AllotedAmount = Convert.ToDouble(reader.GetValue(6));


                         
                          FMISProgramCode = Others.FMISProgramCode;
                          OOECode = Others.OOECode;
                          FMISAccountCode = Others.FMISAccountCode;
                          Yearof = Others.Yearof;
                          AllotedAmount = Others.AllotedAmount;
                          }
                 }
                 try
                 {
                     using (SqlConnection con = new SqlConnection(Common.MyConn()))
                     {

                         SqlCommand com4 = new SqlCommand("  insert into IFMIS.dbo.tbl_R_BMS_SOONONoffice (trnno,FMISProgramCode,OOECode,FMISAccountCode,BudgetAcctName,Yearof,AllotedAmount,SooAmount,ActionCode,DateInOut,OfficeID) values ('" + trnno + "','" + FMISProgramCode + "','" + OOECode + "','" + FMISAccountCode + "','" + BudgetAcctName + "','" + Year + "','" + AllotedAmount + "',0,1,GETDATE(),'" + Account.UserInfo.Department + "')", con);
                         con.Open();
                         com4.ExecuteNonQuery();
                         return "1";
                     }
                 }
                 catch (Exception ex)
                 {
                     return ex.Message.ToString();
                 }


             }
             catch (Exception ex)
             {
                 return ex.Message.ToString();
             }
         }



         public void SOODESTROY(SOONon_Model product, int? propYear)
         {
             //var entity = new SOONon_Model();
             try
             {

                 using (SqlConnection con = new SqlConnection(Common.MyConn()))
                 {
                     SqlCommand com = new SqlCommand("update IFMIS.dbo.tbl_R_BMS_SOONONoffice set DateInOut = CONCAT(DateInOut,' , ', GETDATE()), ActionCode = '2' where trnno = '" + product.trnno + "' and officeiD = '" + Account.UserInfo.Department + "' and Yearof = '" + product.Yearof + "' and ActionCode = 1", con);
                     con.Open();
                     com.ExecuteNonQuery();
                    
                 }
             }
             catch (Exception)
             {

             }
         }
    }
}