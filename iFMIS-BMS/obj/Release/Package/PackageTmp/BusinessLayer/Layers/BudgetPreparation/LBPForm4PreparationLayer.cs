using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Models.Budget_Preparation;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation
{
    public class LBPForm4PreparationLayer
    {
        public string FetchAIPData(int OfficeID) 
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"sp_BMS_FetchAIPData "+ OfficeID +"", con);
                con.Open();
                return com.ExecuteScalar().ToString();
            }
        }
        public IEnumerable<LBPForm4Model> grLBPForm4PPAs(int? OfficeID,int? transyear, long? transno = 0)
        {
            List<LBPForm4Model> List = new List<LBPForm4Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                //SqlCommand com = new SqlCommand(@"select [id],[program],[project],[initiative_id],[initiative],[expenseclass],[psamount],[mooeamount],[coamount],[output],[MFO],[target_indicator],[target],
                //       [initiative_order],[impsched_from],[impsched_to],[aip_id],[aipspms_tag],[initiativesub_parentid],[initiativesub_mainid],[OfficeID],[year] from [IFMIS].[dbo].[vwBMS_AIP] where [OfficeID]= " + OfficeID + " and [year]=" + transyear + "", con);

                SqlCommand com = new SqlCommand(@"exec sp_BMS_LBP4_Grid " + OfficeID + "," + transyear + ",0", con);
                com.CommandTimeout = 0;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    LBPForm4Model Item = new LBPForm4Model();

                    Item.transno = Convert.ToInt32(reader.GetValue(0));
                    //Item.SeriesID = Convert.ToInt64(reader.GetValue(15));
                    //Item.AIPRefCode = reader.GetValue(1).ToString();
                    Item.PPADesc = reader.GetValue(4).ToString();
                    Item.MajorFinalOutput = reader.GetValue(10).ToString();
                    Item.PerformanceIndicator = reader.GetValue(11).ToString();
                    Item.TargetForTheBudgetYear = reader.GetValue(12).ToString();
                    Item.PSAmountStr = reader.GetValue(6) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatString(Convert.ToDouble(reader.GetValue(6)));//Convert.ToDouble(reader.GetValue(6));
                    Item.MOOEAmount = reader.GetValue(7) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatString(Convert.ToDouble(reader.GetValue(7)));
                    Item.COAmount = reader.GetValue(8) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatString(Convert.ToDouble(reader.GetValue(8)));
                    //Item.Total = reader.GetValue(10) == DBNull.Value && reader.GetValue(7) == DBNull.Value && reader.GetValue(8) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatString(Convert.ToDouble(reader.GetValue(9)));
                    //Item.Total = GlobalFunctions.CurrencyFormatString(Convert.ToDouble(reader.GetValue(10)));
                    Item.OrderNo = Convert.ToInt32(reader.GetValue(13));
                    //Item.isBold = Convert.ToInt32(reader.GetValue(11));
                    //Item.isNonOffice = 0;// Convert.ToInt32(reader.GetValue(12));
                    Item.aiptag = Convert.ToInt32(reader.GetValue(17));

                    List.Add(Item);
                }
            }


            return List;
        }
        public string getLBPFORM4GridFooterTotal(int? OfficeID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Select isnull(SUM(isnull(PSAmount,0) + isnull(MOOEAmount,0) + isnull(COAmount,0)),0) from tbl_R_BMSLBPForm4PPAs
                                                where ActionCode = 1  and 
                                                OfficeID = " + OfficeID + " and Yearof = " + (DateTime.Now.Year + 1) + "", con);
                con.Open();
                return com.ExecuteScalar() == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatString(Convert.ToDouble(com.ExecuteScalar()));
            }
        }
        public IEnumerable<DropDownListModel> ddlForm4DataType()
        {
            List<DropDownListModel> ddlItemsList = new List<DropDownListModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Select SeriesID,DataType from tbl_R_BMSLBPForm4DataTypes where ActionCode = 1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    DropDownListModel ddlItem = new DropDownListModel();
                    ddlItem.Value = Convert.ToInt32(reader.GetValue(0));
                    ddlItem.Text = reader.GetValue(1).ToString();

                    ddlItemsList.Add(ddlItem);
                }
            }
            return ddlItemsList;
        }
        public string SaveNewForm4OtherData(int DataType, int OrderNo, string Description, int OfficeID,int yearof) 
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"insert into tbl_R_BMSLBPForm4OtherData values('"+Description+"',"+OrderNo+",1,"+OfficeID+","+ yearof +","+DataType+")", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    return "1";
                }
            }
            catch (Exception ex)
            {
               return ex.Message;
            }
        }
        public string UpdateForm4OtherData(int SeriesID, int DataType, int OrderNo, string Description,int yearof)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Update tbl_R_BMSLBPForm4OtherData set Description = '"+Description+"',OrderNo ="+OrderNo+",Data_Type = "+DataType+" where SeriesID = "+SeriesID+"", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    return "1";
                }
            }
            catch (Exception ex)
            {
               return ex.Message;
            }
        }
        public string DeleteForm4OtherData(int SeriesID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Update tbl_R_BMSLBPForm4OtherData set ActionCode = 0 where SeriesID = " + SeriesID + "", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    return "1";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string GetForm4OtherDataOrderNo(int OfficeID, int DataType)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select top 1 isnull(OrderNo,0) + 1 from tbl_R_BMSLBPForm4OtherData where OfficeID = " + OfficeID + " and Data_Type = " + DataType 
                                                    + " and YearOf = "+ (DateTime.Now.Year + 1) +" and ActionCode = 1 order by OrderNo desc", con);
                    con.Open();
                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception)
            {
                return "1";
            }
            
        }
        public LBPForm4OtherDataModel GetForm4OtherDataSelectedForEdit(int SeriesID)
        {
            LBPForm4OtherDataModel SelectedData = new LBPForm4OtherDataModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select Description,isnull(OrderNo,0),Data_Type
                                                from tbl_R_BMSLBPForm4OtherData where SeriesID = " + SeriesID + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    SelectedData.Description = reader.GetValue(0).ToString();
                    SelectedData.OrderNo = Convert.ToInt32(reader.GetValue(1));
                    SelectedData.DataType = Convert.ToInt32(reader.GetValue(2));
                }
            }
            return SelectedData;
        }
        public LBPForm4Model GetForm4PPASelectedForEdit(int? OfficeID=0,int? transyear=0, long? transno=0)
        {
            LBPForm4Model SelectedData = new LBPForm4Model();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {

                //    SqlCommand com = new SqlCommand(@"select a.AIPRefCode,a.PPADesc,a.MajorFinalOutput,a.PerformanceIndicator,a.TargetForTheBudgetYear,
                //                                    a.PSAmount,a.MOOEAmount,a.COAmount,a.isBold,isnull(a.OrderNo,0),isnull(b.PSAmount,0),isnull(b.MOOEAmount,0),isnull(b.COAmount,0)
                //                                    from  tbl_R_BMSLBPForm4PPAs as a
                //                                    LEFT JOIN tbl_R_BMSAnnualInvestmentPlan as  b on b.AIP_ID = a.AIPID and b.YearOf = a.YearOf and b.ActionCode = a.ActionCode
                //                                    where a.SeriesID = " + SeriesID + "", con);
                //    con.Open();
                //    SqlDataReader reader = com.ExecuteReader();
                //    while (reader.Read())
                //    {
                //        SelectedData.AIPRefCode = reader.GetValue(0).ToString();
                //        SelectedData.PPADesc = reader.GetValue(1).ToString();
                //        SelectedData.MajorFinalOutput = reader.GetValue(2).ToString();
                //        SelectedData.PerformanceIndicator = reader.GetValue(3).ToString();
                //        SelectedData.TargetForTheBudgetYear = reader.GetValue(4).ToString();
                //        SelectedData.PSAmount = reader.GetValue(5) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatStringNoSymbol(Convert.ToDouble(reader.GetValue(5)));
                //        SelectedData.MOOEAmount = reader.GetValue(6) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatStringNoSymbol(Convert.ToDouble(reader.GetValue(6)));
                //        SelectedData.COAmount = reader.GetValue(7) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatStringNoSymbol(Convert.ToDouble(reader.GetValue(7)));
                //        SelectedData.isBold = Convert.ToInt32(reader.GetValue(8));
                //        SelectedData.OrderNo = Convert.ToInt32(reader.GetValue(9));
                //        SelectedData.PSMax = Convert.ToDouble(reader.GetValue(10));
                //        SelectedData.MOOEMax = Convert.ToDouble(reader.GetValue(11));
                //        SelectedData.COMax = Convert.ToDouble(reader.GetValue(12));
                //    }
                //}
                //SqlCommand com = new SqlCommand(@"select [id],[program],[project],[initiative_id],[initiative],[expenseclass],[psamount],[mooeamount],[coamount],[output],[MFO],[target_indicator],[target],
                //       [initiative_order],[impsched_from],[impsched_to],[aip_id],[aipspms_tag],[initiativesub_parentid],[initiativesub_mainid],[OfficeID],[year] from [IFMIS].[dbo].[vwBMS_AIP] where [OfficeID]= " + OfficeID + " and [year]=" + transyear + "", con);



                //SqlCommand com = new SqlCommand(@"select [id],[program],[project],[initiative_id],[initiative],[expenseclass],isnull([psamount],0) + isnull([mooeamount],0) + isnull([coamount],0) as Amount,[output],[MFO],[target_indicator],[target],
                //          [initiative_order],[impsched_from],[impsched_to],[aip_id],[aipspms_tag],[initiativesub_parentid],[initiativesub_mainid],[OfficeID],[year],isbold,parent_id,ChildTotAmount from [IFMIS].[dbo].[vwBMS_AIP] where id=" + transno + "", con);
                SqlCommand com = new SqlCommand(@"exec sp_BMS_LBP4_Grid " + OfficeID + "," + transyear + ","+ transno + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                 //   SelectedData.AIPRefCode = reader.GetValue(1).ToString();
               //     SelectedData.PPADesc = reader.GetValue(4).ToString();
                    SelectedData.MajorFinalOutput = reader.GetValue(8).ToString();
                    SelectedData.PerformanceIndicator = reader.GetValue(9).ToString();
                    SelectedData.TargetForTheBudgetYear = reader.GetValue(10).ToString();
                    SelectedData.PSAmount = Convert.ToDouble(reader.GetValue(6));
              //     SelectedData.MOOEAmount = reader.GetValue(7) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatStringNoSymbol(Convert.ToDouble(reader.GetValue(7)));
               //     SelectedData.COAmount = reader.GetValue(8) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatStringNoSymbol(Convert.ToDouble(reader.GetValue(8)));
                    SelectedData.isBold = Convert.ToInt32(reader.GetValue(20));
                    SelectedData.OrderNo = Convert.ToInt32(reader.GetValue(11));
                    SelectedData.PSMax = Convert.ToDouble(reader.GetValue(6));
                    SelectedData.MOOEMax = Convert.ToDouble(reader.GetValue(6));
                    SelectedData.COMax = Convert.ToDouble(reader.GetValue(6));
                    SelectedData.initiativeid= Convert.ToInt32(reader.GetValue(0));
                    SelectedData.initiative= reader.GetValue(4).ToString();
                    SelectedData.parentaipid = Convert.ToInt32(reader.GetValue(16));
                    SelectedData.parent_trnno = Convert.ToInt64(reader.GetValue(21));
                    SelectedData.target = reader.GetValue(10).ToString();
                    SelectedData.ChildTotAmount= Convert.ToDouble(reader.GetValue(22));
                }
            }
            return SelectedData;
        }
        //public IEnumerable<LBPForm4Model> grLBPForm4PPAs(int? OfficeID, int? transyear)
        //{
        //    List<LBPForm4Model> List = new List<LBPForm4Model>();
        //    using (SqlConnection con = new SqlConnection(Common.MyConn()))
        //    {
        //        SqlCommand com = new SqlCommand(@"select [id],[program],[project],[initiative_id],[initiative],[expenseclass],[psamount],[mooeamount],[coamount],[output],[MFO],[target_indicator],[target],
        //               [initiative_order],[impsched_from],[impsched_to],[aip_id],[aipspms_tag],[initiativesub_parentid],[initiativesub_mainid],[OfficeID],[year] from [IFMIS].[dbo].[vwBMS_AIP] where [OfficeID]= " + OfficeID + " and [year]=" + transyear + "", con);
        //        con.Open();
        //        SqlDataReader reader = com.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            LBPForm4Model Item = new LBPForm4Model();

        //            Item.transno = Convert.ToInt32(reader.GetValue(0));
        //            //Item.SeriesID = Convert.ToInt64(reader.GetValue(15));
        //            //Item.AIPRefCode = reader.GetValue(1).ToString();
        //            Item.PPADesc = reader.GetValue(4).ToString();
        //            Item.MajorFinalOutput = reader.GetValue(10).ToString();
        //            Item.PerformanceIndicator = reader.GetValue(11).ToString();
        //            //Item.TargetForTheBudgetYear = reader.GetValue(5).ToString();
        //            Item.PSAmount = reader.GetValue(6) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatString(Convert.ToDouble(reader.GetValue(6)));
        //            Item.MOOEAmount = reader.GetValue(7) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatString(Convert.ToDouble(reader.GetValue(7)));
        //            Item.COAmount = reader.GetValue(8) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatString(Convert.ToDouble(reader.GetValue(8)));
        //            //Item.Total = reader.GetValue(10) == DBNull.Value && reader.GetValue(7) == DBNull.Value && reader.GetValue(8) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatString(Convert.ToDouble(reader.GetValue(9)));
        //            //Item.Total = GlobalFunctions.CurrencyFormatString(Convert.ToDouble(reader.GetValue(10)));
        //            Item.OrderNo = Convert.ToInt32(reader.GetValue(13));
        //            //Item.isBold = Convert.ToInt32(reader.GetValue(11));
        //            //Item.isNonOffice = 0;// Convert.ToInt32(reader.GetValue(12));
        //            Item.aiptag = Convert.ToInt32(reader.GetValue(17));

        //            List.Add(Item);
        //        }
        //    }


        //    return List;
        //}
        public IEnumerable<LBPForm4OtherDataModel> grLBPForm4OtherData(int? OfficeID,int? transyear)
        {
            List<LBPForm4OtherDataModel> List = new List<LBPForm4OtherDataModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select a.SeriesID,b.DataType,a.Description,a.OrderNo from tbl_R_BMSLBPForm4OtherData as a
                                                LEFT JOIN tbl_R_BMSLBPForm4DataTypes as b on b.SeriesID = a.Data_Type and b.ActionCode = a.ActionCode
                                                where a.ActionCode = 1 and a.OfficeID = " + OfficeID + " and a.YearOf = "+ transyear + " order by a.OrderNo", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    LBPForm4OtherDataModel Item = new LBPForm4OtherDataModel();
                    Item.SeriesID = Convert.ToInt32(reader.GetValue(0));
                    Item.DataTypeDesc = reader.GetValue(1).ToString();
                    Item.Description = reader.GetValue(2).ToString();
                    Item.OrderNo = Convert.ToInt32(reader.GetValue(3));
                   
                    List.Add(Item);
                }
            }


            return List;
        }
        public string UpdateForm4PPA(int SeriesID, int OrderNo, string PPADesc,
                string MajorFinalOutput, string PerformanceIndicator,
                string Target, string PSAmount, string MOOEAmount, string COAmount, string isBold, string RefCode)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Update tbl_R_BMSLBPForm4PPAs set AIPRefCode = '" + RefCode 
                                                    +"',PPADesc = '"+ PPADesc +"',MajorFinalOutput = '"+ MajorFinalOutput 
                                                    +"',PerformanceIndicator='"+PerformanceIndicator+"',TargetForTheBudgetYear = '"+Target
                                                    +"',PSAmount="+PSAmount+",MOOEAmount="+MOOEAmount+",COAmount="+COAmount+",isBold="+isBold
                                                    +",OrderNo = "+OrderNo+" where SeriesID = "+SeriesID+"", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    return "1";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string RemoveLBPForm4PPA(int transno)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    var data = "";
                    SqlCommand com = new SqlCommand(@"exec dbo.sp_BMS_DeletePPAbreakdown " + transno + "", con);
                    con.Open();
                    data = Convert.ToString(com.ExecuteScalar());
                    return data;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}