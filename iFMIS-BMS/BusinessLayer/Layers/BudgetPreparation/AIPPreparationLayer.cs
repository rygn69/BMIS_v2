using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models.Budget_Preparation;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using iFMIS_BMS.Classes;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;
using iFMIS_BMS.BusinessLayer.Models;

namespace iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation
{
    public class AIPPreparationLayer
    {
        int TransactionYear = 2017;

        public IEnumerable<DropDownListModel> ddlAddAIP_FundingSource()
        {
            List<DropDownListModel> ddlItemsList = new List<DropDownListModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select FundingSource from tbl_R_BMSAIPFundingSource where ActionCode = 1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    DropDownListModel ddlItem = new DropDownListModel();
                    ddlItem.Text = reader.GetValue(0).ToString();

                    ddlItemsList.Add(ddlItem);
                }
            }
            return ddlItemsList;
        }
        public IEnumerable<ddlIAPModel> ddlAIPActivityDescription(int OfficeID)
        {
            List<ddlIAPModel> ddlItemsList = new List<ddlIAPModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT PPA_MFO_ID, PPA_DESCRIPTION
                                                FROM tbl_R_LBP5_PPA_MFO 
                                                WHERE OfficeID = '" + OfficeID 
                                                + "' and TransactionYear = '" + TransactionYear + "' and ActionCode = 1 ORDER BY PPA_OrderBy, PPA_Description", con);

                SqlCommand com2 = new SqlCommand(@"SELECT a.PPA_MFO_ID, b.PPA_Description
                                                FROM tbl_R_LBP5_PPA_MFO as a inner join 
                                                tbl_R_LBP5_PPA_Denomination as b on a.PPA_MFO_ID = b.PPA_MFO_ID and a.ActionCode = b.ActionCOde
                                                and a.TransactionYear = b.TransactionYear
                                                WHERE a.OfficeID = '" + OfficeID
                                                + "' and a.ActionCode = 1 and a.TransactionYear= '" + TransactionYear 
                                                +"' and b.PPA_Description != '' ORDER BY a.PPA_OrderBy, A.PPA_Description", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ddlIAPModel ddlItem = new ddlIAPModel();
                    ddlItem.ddlID = Convert.ToInt32(reader.GetValue(0));
                    ddlItem.ddlDescription = reader.GetValue(1).ToString();

                    ddlItemsList.Add(ddlItem);
                }
                reader.Close();
                SqlDataReader reader2 = com2.ExecuteReader();
                while (reader2.Read())
                {
                    ddlIAPModel ddlItem = new ddlIAPModel();
                    ddlItem.ddlID = Convert.ToInt32(reader2.GetValue(0));
                    ddlItem.ddlDescription = reader2.GetValue(1).ToString();

                    ddlItemsList.Add(ddlItem);
                }

            }
            return ddlItemsList;
        }
        public string SaveAIP(string MotherAIP_ID, int OfficeID, string EmplementingOfficeID, string FundingSource, string AIPRefCode,
          string Description, string StartDate, string CompletionDate, string ExpectedOutput, string PSAmount, string MOOEAmount,
          string COAmount, string ClimateChangeType, string ClimateChangeAmount, string ClimateChangeTypologyCode, int? OrderNo)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"insert into tbl_R_BMSAnnualInvestmentPlan values(" + MotherAIP_ID + "," + OfficeID + "," + EmplementingOfficeID
                        + ",'" + FundingSource + "','" + AIPRefCode + "','" + Description + "','" + StartDate
                        + "','" + CompletionDate + "','" + ExpectedOutput + "'," + PSAmount
                        + "," + MOOEAmount + "," + COAmount
                        + "," + ClimateChangeType + "," + ClimateChangeAmount + ",'" + ClimateChangeTypologyCode
                        + "'," + OrderNo + "," + (DateTime.Now.Year + 1)  + ",1,0)", con);
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
        public string UpdateAIP(string MotherAIP_ID, string EmplementingOfficeID, string FundingSource, string AIPRefCode,
          string Description, string StartDate, string CompletionDate, string ExpectedOutput, string PSAmount, string MOOEAmount,
          string COAmount, string ClimateChangeType, string ClimateChangeAmount, string ClimateChangeTypologyCode, int? OrderNo,int AIPID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Update tbl_R_BMSAnnualInvestmentPlan set MotherAIP_ID = " + MotherAIP_ID + ", EmplementingOfficeID = " + EmplementingOfficeID
                        + ", FundingSource = '" + FundingSource + "', AIPRefCode = '" + AIPRefCode + "',Description = '" + Description + "',StartDate = '" + StartDate
                        + "',CompletionDate = '" + CompletionDate + "',ExpectedOutput = '" + ExpectedOutput + "',PSAmount = " + PSAmount
                        + ",MOOEAmount = " + MOOEAmount + ",COAmount = " + COAmount
                        + ",ClimateChangeType = " + ClimateChangeType + ",ClimateChangeAmount = " + ClimateChangeAmount + ",ClimateChangeTypologyCode ='" + ClimateChangeTypologyCode
                        + "',OrderNo = " + OrderNo + " where AIP_ID = " + AIPID + "", con);
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
        public void NonOfficeToogleAIP(int? AIPID, int? isNonOffice)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Update tbl_R_BMSAnnualInvestmentPlan set isNonOffice = " + isNonOffice + " where AIP_ID = " + AIPID + "", con);
                    con.Open();
                    com.ExecuteNonQuery();
                   // return "1";
                }
            }
            catch (Exception)
            {
                throw;
                //return ex.Message;
            }
        }
        public IEnumerable<ddlIAPModel> ddlAddAIP_ProgramActivities_Main_Data(int OfficeID)
        {
            List<ddlIAPModel> ddlItemsList = new List<ddlIAPModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select aip_ID,Description from tbl_R_BMSAnnualInvestmentPlan 
                                                where ActionCode = 1 and MotherAIP_ID is NULL and 
                                                OfficeID = " + OfficeID + " and Yearof = " + (DateTime.Now.Year + 1) + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ddlIAPModel ddlItem = new ddlIAPModel();
                    ddlItem.ddlID = Convert.ToInt32(reader.GetValue(0));
                    ddlItem.ddlDescription = reader.GetValue(1).ToString();

                    ddlItemsList.Add(ddlItem);
                }
            }
            return ddlItemsList;
        }
        public string getAIPNewOrderNo(int OfficeID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Select top 1 OrderNO from tbl_R_BMSAnnualInvestmentPlan
                                                where ActionCode = 1  and 
                                                OfficeID = " + OfficeID + " and Yearof = " + (DateTime.Now.Year + 1) + " ORDER BY OrderNo Desc", con);
                con.Open();
                var OrderNo = Convert.ToInt32(com.ExecuteScalar()) + 1;
                return OrderNo.ToString();
            }
        }
        public IEnumerable<AIPItemsModel> grdAIPListData(int? OfficeID)
        {
            List<AIPItemsModel> AIPList = new List<AIPItemsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select AIP_ID,a.OrderNo, AIPRefCode,case when MotherAIP_ID is null 
															                                    then '<b>' + Description + '</b>' 
													                                      else 
															                                   Description 
													                                      end as 'Description',
                                                                StartDate,CompletionDate,ExpectedOutput,FundingSource,PSAmount,
                                                                MOOEAmount,COAmount,isnull(PSAmount,0) + isnull(MOOEAmount,0) + isnull(COAmount,0)  as 'TotalAmount',
                                                                ClimateChangeAmount, b.CCCOde,ClimateChangeType,isnull(isNonOffice,0)
                                                                from tbl_R_BMSAnnualInvestmentPlan as a
                                                                Left Join tbl_R_BMSAIPClimateChangeCode as b on b.CCCodeID = a.ClimateChangeTypologyCode and b.ActionCode = 1 
                                                                where a.ActionCode = 1 and OfficeID = " + OfficeID + " and YearOf = " + (DateTime.Now.Year + 1) + " ORDER BY a.OrderNo,AIP_ID", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        AIPItemsModel AIPItem = new AIPItemsModel();
                        AIPItem.AIPID = Convert.ToInt64(reader.GetValue(0));
                        AIPItem.OrderNo = Convert.ToInt32(reader.GetValue(1));
                        AIPItem.AIPRefCode = reader.GetValue(2).ToString();
                        AIPItem.Description = reader.GetValue(3).ToString();
                        AIPItem.StartDate = reader.GetValue(4).ToString();
                        AIPItem.CompletionDate = reader.GetValue(5).ToString();
                        AIPItem.ExpectedOutput = reader.GetValue(6).ToString();
                        AIPItem.FundingSource = reader.GetValue(7).ToString();
                        AIPItem.PSAmount = reader.GetValue(8) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatString(Convert.ToDouble(reader.GetValue(8)));
                        AIPItem.MOOEAmount = reader.GetValue(9) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatString(Convert.ToDouble(reader.GetValue(9)));
                        AIPItem.COAmount = reader.GetValue(10) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatString(Convert.ToDouble(reader.GetValue(10)));
                        AIPItem.TotalAmount = reader.GetValue(8) == DBNull.Value && reader.GetValue(9) == DBNull.Value && reader.GetValue(10) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatString(Convert.ToDouble(reader.GetValue(11)));
                        AIPItem.CCAmount = reader.GetValue(12) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatString(Convert.ToDouble(reader.GetValue(12)));
                        AIPItem.CCTypologyCode = reader.GetValue(13).ToString();
                        AIPItem.CCType = reader.GetValue(14).ToString();
                        AIPItem.isNonOffice = Convert.ToInt32(reader.GetValue(15));

                        AIPList.Add(AIPItem);
                    }
            }


            return AIPList;
        }
        public string getAIPGridFooterTotal(int? OfficeID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Select isnull(SUM(isnull(PSAmount,0) + isnull(MOOEAmount,0) + isnull(COAmount,0)),0) from tbl_R_BMSAnnualInvestmentPlan
                                                where ActionCode = 1  and 
                                                OfficeID = " + OfficeID + " and Yearof = " + (DateTime.Now.Year + 1) + "", con);
                con.Open();
                return com.ExecuteScalar() == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatString(Convert.ToDouble(com.ExecuteScalar()));
            }
        }
        public string UpdateAIPOrderNo(int AIP_ID, int OrderNo)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Update tbl_R_BMSAnnualInvestmentPlan set OrderNo = " + OrderNo + " where AIP_ID = " + AIP_ID + "", con);
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
        public string RemoveAIP(int AIP_ID)
        { 
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Update tbl_R_BMSAnnualInvestmentPlan set ActionCode = 0 where AIP_ID = " + AIP_ID + "", con);
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
        public AIPItemsModel GetAIPDataForEdit(int AIPID)
        {
            AIPItemsModel SelectedAIPData = new AIPItemsModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select MotherAIP_ID,EmplementingOfficeID,AIPRefCode,Description,StartDate,CompletionDate,
                                                FundingSource,PSAmount,MOOEAmount,COAmount,OrderNo,ExpectedOutput,
                                                ClimateChangeType,ClimateChangeAmount,ClimateChangeTypologyCode 
                                                from tbl_R_BMSAnnualInvestmentPlan where AIP_ID = " +  AIPID+ "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    SelectedAIPData.MotherAIPID = reader.GetValue(0).ToString();
                    SelectedAIPData.EmplementingOffice = reader.GetValue(1).ToString();
                    SelectedAIPData.AIPRefCode = reader.GetValue(2).ToString();
                    SelectedAIPData.Description = reader.GetValue(3).ToString();
                    SelectedAIPData.StartDate = reader.GetValue(4).ToString();
                    SelectedAIPData.CompletionDate = reader.GetValue(5).ToString();
                    SelectedAIPData.FundingSource = reader.GetValue(6).ToString();
                    SelectedAIPData.PSAmount = reader.GetValue(7) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatStringNoSymbol(Convert.ToDouble(reader.GetValue(7)));
                    SelectedAIPData.MOOEAmount = reader.GetValue(8) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatStringNoSymbol(Convert.ToDouble(reader.GetValue(8)));
                    SelectedAIPData.COAmount = reader.GetValue(9) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatStringNoSymbol(Convert.ToDouble(reader.GetValue(9)));
                    SelectedAIPData.OrderNo = Convert.ToInt32(reader.GetValue(10));
                    SelectedAIPData.ExpectedOutput = reader.GetValue(11).ToString();
                    SelectedAIPData.CCType = reader.GetValue(12).ToString();
                    SelectedAIPData.CCAmount = reader.GetValue(13) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatStringNoSymbol(Convert.ToDouble(reader.GetValue(13)));
                    SelectedAIPData.CCTypologyCode = reader.GetValue(14).ToString();

                }

            }
            return SelectedAIPData;
        }

        public IEnumerable<ddlIAPModel> Get_DDLAIPYear()
        {
            List<ddlIAPModel> ddlItemsList = new List<ddlIAPModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select DISTINCT YearOf from tbl_R_BMSAnnualInvestmentPlan where ActionCode = 1 ORDER BY yearof desc", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ddlIAPModel ddlItem = new ddlIAPModel();
                    ddlItem.ddlDescription = reader.GetValue(0).ToString();

                    ddlItemsList.Add(ddlItem);
                }
            }
            return ddlItemsList;
        }
        public IEnumerable<ddlIAPModel> ddlCCTypologyCode_Data(int CCType)
        {
            List<ddlIAPModel> ddlItemsList = new List<ddlIAPModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select  CCCodeID,'(' + CCCode + ') - ' + CCDescription 
                                                from tbl_R_BMSAIPClimateChangeCode where ActionCode = 1 and CCType = " + CCType + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ddlIAPModel ddlItem = new ddlIAPModel();
                    ddlItem.ddlID = Convert.ToInt32(reader.GetValue(0));
                    ddlItem.ddlDescription = reader.GetValue(1).ToString();

                    ddlItemsList.Add(ddlItem);
                }
            }
            return ddlItemsList;
        }
        public IEnumerable<ClimateChangeCodesModel> grdCCCOdeListData()
        {
            List<ClimateChangeCodesModel> CCCodeList = new List<ClimateChangeCodesModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select CCCOdeID,OrderNo,CCCode,CCDescription,b.CCTypeDesc from tbl_R_BMSAIPClimateChangeCode as a
                                                LEFT JOIN tbl_R_BMSAIPCCType as b on a.CCType = b.CCTypeID where a.ActionCode = 1", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        ClimateChangeCodesModel CCCodeItem = new ClimateChangeCodesModel();
                        CCCodeItem.CCCodeID = Convert.ToInt32(reader.GetValue(0));
                        CCCodeItem.OrderNo = Convert.ToInt32(reader.GetValue(1));
                        CCCodeItem.CCCode = reader.GetValue(2).ToString();
                        CCCodeItem.CCCDescription = reader.GetValue(3).ToString();
                        CCCodeItem.CCTypeDesc = reader.GetValue(4).ToString();
                        
                        CCCodeList.Add(CCCodeItem);
                    }
            }


            return CCCodeList;
        }
        public string AddNewCCCode(string CCTypologyCode, string Description, int CCType, int OrderNo) 
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Insert into tbl_R_BMSAIPClimateChangeCode values('" + CCTypologyCode.Replace("'", "''") + "','" + Description.Replace("'", "''") + "'," + CCType + "," + OrderNo + ",1)", con);
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
        public string getLatestOrderNoCCCode(int CCTYpe)
        {
            var AdditionalQuery = CCTYpe == 0 ? "" : "and CCType = " + CCTYpe + "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select top 1 OrderNo from tbl_R_BMSAIPClimateChangeCode 
                                                where ActionCode = 1 " + AdditionalQuery + " ORDER BY orderNo desc", con);
                con.Open();
                return (Convert.ToInt32(com.ExecuteScalar()) + 1).ToString();
            }
        }
        public string RemoveCCCode(int CCCodeID) 
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Update tbl_R_BMSAIPClimateChangeCode set ActionCode = 0 where CCCodeID = " + CCCodeID + "", con);
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
        public ClimateChangeCodesModel GetSelectedCCCodeData(int CCCodeID)
        {
            ClimateChangeCodesModel SelectedData = new ClimateChangeCodesModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select CCCode,CCDescription,CCType,OrderNo from 
                                                  tbl_R_BMSAIPClimateChangeCode where ActionCode = 1 and CCCodeID =  " + CCCodeID + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    SelectedData.CCCode = reader.GetValue(0).ToString();
                    SelectedData.CCCDescription = reader.GetValue(1).ToString();
                    SelectedData.CCCodeID =  Convert.ToInt32(reader.GetValue(2));
                    SelectedData.OrderNo = Convert.ToInt32(reader.GetValue(3));
                }

            }
            return SelectedData;
        }
        public string UpdateCCCode(string CCTypologyCode, string Description, int CCType, int OrderNo, int CCCodeID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Update tbl_R_BMSAIPClimateChangeCode 
                                                    set CCCode = '" + CCTypologyCode.Replace("'","''") + "', CCDescription = '" + Description.Replace("'","''") 
                                                    + "', CCType = " + CCType + ", OrderNo = " + OrderNo + " where CCCodeID = " + CCCodeID + "", con);
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
        public string UpdateCCCodeOrderNo(int CCCodeID,int OrderNo)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Update tbl_R_BMSAIPClimateChangeCode set OrderNo = " + OrderNo + " where CCCodeID = " + CCCodeID + "", con);
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
    }
}