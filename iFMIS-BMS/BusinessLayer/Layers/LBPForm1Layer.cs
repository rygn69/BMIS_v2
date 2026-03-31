using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers
{
    public class LBPForm1Layer
    {
        public string Generate1SemActualYear(int YearOf) {
            try
            {
                DataTable OfficeIDList = new DataTable();
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select OfficeID from tbl_R_BMSOffices where PMISOfficeID != 0", con);
                    con.Open();
                    OfficeIDList.Load(com.ExecuteReader());
                }
                for (int x = 0; x < OfficeIDList.Rows.Count; x++)
                {
                    
                    using (SqlConnection con = new SqlConnection(Common.MyConn()))
                    {
                        SqlCommand com = new SqlCommand(@"sp_BMS_generateCurrentYearObligatedData " + Convert.ToInt32(OfficeIDList.Rows[x][0]) + "," + YearOf + "", con);
                        con.Open();
                        com.ExecuteNonQuery();
                    }
                }
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public IEnumerable<DropDownListModel> getFundType()
        {
            List<DropDownListModel> ddlItemsList = new List<DropDownListModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select FundID,FundName from tbl_R_BMSFunds where FundID in(1,5)", con);
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
        public IEnumerable<DropDownListModel> getParticulars(int FundType, int YearOf)
        {
            List<DropDownListModel> ddlItemsList = new List<DropDownListModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select SeriesID, Particular from tbl_R_BMSLBPForm1Data where ActionCode = 1 and YearOf = "+YearOf +" and FundType = "+ FundType+"", con);
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
        public IEnumerable<LbpForm1Model> getLbpForm1Data(int? FundType, int? YearOf)
        {
            List<LbpForm1Model> List = new List<LbpForm1Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select SeriesID,Concat((select dbo.fn_BMS_GetIndent(ItemLevel)),RowNo,Particular) as Particular,AccountCode,
                                                IncomeClassification,PastYear,CurrentYear,BudgetYear,isnull(OrderNo,0),isnull(isBold,0),isnull(isUseAmount,0),isnull(hasFooterTotal,0),isnull(isUseTotalInGraph,0)
                                                from tbl_R_BMSLBPForm1Data WHERE ActionCode = 1 and YearOf = " + YearOf + " and FundType = " + FundType + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    LbpForm1Model Item = new LbpForm1Model();
                    Item.SeriesID = Convert.ToInt32(reader.GetValue(0));
                    Item.Particular = reader.GetValue(1).ToString();
                    Item.AccountCode = reader.GetValue(2).ToString();
                    Item.IncomeClassification = reader.GetValue(3).ToString();
                    Item.PastYear = reader.GetValue(4) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatString(Convert.ToDouble(reader.GetValue(4)));
                    Item.CurrentYear = reader.GetValue(5) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatString(Convert.ToDouble(reader.GetValue(5)));
                    Item.BudgetYear = reader.GetValue(6) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatString(Convert.ToDouble(reader.GetValue(6)));
                    Item.OrderNo = Convert.ToInt32(reader.GetValue(7));
                    Item.isBold = Convert.ToInt32(reader.GetValue(8));
                    Item.useAmount = Convert.ToInt32(reader.GetValue(9));
                    Item.hasFooterTotal = Convert.ToInt32(reader.GetValue(10));
                    Item.useInGraph = Convert.ToInt32(reader.GetValue(11));

                    List.Add(Item);

                }
            }
            return List;
        }
        public string RemoveItem(int SeriesID) {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Update tbl_R_BMSLBPForm1Data set ActionCode = 0 where SeriesID = " + SeriesID + "", con);
                    con.Open();
                    com.ExecuteNonQuery();
                }
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string getNewOrderNo(int FundType,int YearOf)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Select Top 1 isnull(OrderNo,0) + 1 from tbl_R_BMSLBPForm1Data where ActionCode = 1 and FundType = " + FundType + " and YearOf = " + YearOf + " Order By OrderNo Desc", con);
                    con.Open();
                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception)
            {
                return "1";
            }
        }
        public LbpForm1Model GetSelectedDataForEdit(int SeriesID)
        {
            LbpForm1Model SelectedItemData = new LbpForm1Model();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select isnull(MainSeriesID,0),RowNo,Particular,AccountCode,
                                                IncomeClassification,PastYear,CurrentYear,BudgetYear,isnull(OrderNo,0),isnull(isBold,0),isnull(isUseAmount,0),isnull(hasFooterTotal,0),isnull(isUseTotalInGraph,0),Obligated
                                                from tbl_R_BMSLBPForm1Data WHERE SeriesID = " + SeriesID+"", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    SelectedItemData.SeriesID = Convert.ToInt32(reader.GetValue(0));
                    SelectedItemData.RowNo = reader.GetValue(1).ToString();
                    SelectedItemData.Particular = reader.GetValue(2).ToString();
                    SelectedItemData.AccountCode = reader.GetValue(3).ToString();
                    SelectedItemData.IncomeClassification = reader.GetValue(4).ToString();
                    SelectedItemData.PastYear = reader.GetValue(5) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatStringNoSymbol(Convert.ToDouble(reader.GetValue(5)));
                    SelectedItemData.CurrentYear = reader.GetValue(6) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatStringNoSymbol(Convert.ToDouble(reader.GetValue(6)));
                    SelectedItemData.BudgetYear = reader.GetValue(7) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatStringNoSymbol(Convert.ToDouble(reader.GetValue(7)));
                    SelectedItemData.OrderNo = Convert.ToInt32(reader.GetValue(8));
                    SelectedItemData.isBold = Convert.ToInt32(reader.GetValue(9).ToString());
                    SelectedItemData.useAmount = Convert.ToInt32(reader.GetValue(10));
                    SelectedItemData.hasFooterTotal = Convert.ToInt32(reader.GetValue(11));
                    SelectedItemData.useInGraph = Convert.ToInt32(reader.GetValue(12).ToString());
                    SelectedItemData.CurrentYearActual = reader.GetValue(13) == DBNull.Value ? "" : GlobalFunctions.CurrencyFormatStringNoSymbol(Convert.ToDouble(reader.GetValue(13)));
                    
                }

            }
            return SelectedItemData;
        }
        public string SaveNewParticular(int FundType, int YearOf, string MainGroup, string RowNo, string Particular, string AccountCode, string IncomeClassification,
            string PastYear, string CurrentYear, string BudgetYear, int OrderNo, string isBold, string isUseAmount, string HasFooterTotal, string UseTotalinGraph,string txtCurrentYearActual)
        {
            var itemLevel = MainGroup == "NULL" ? "0" : "(select ItemLevel + 1 from tbl_R_BMSLBPForm1Data where SeriesID = " + MainGroup + ")";
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"insert into tbl_R_BMSLBPForm1Data 
                                                   (MainSeriesID,ItemLevel,RowNo,Particular,AccountCode,IncomeClassification,PastYear,CurrentYear,
                                                   BudgetYear,OrderNo,ActionCode,YearOf,FundType,isUseAmount,isBold,hasFooterTotal,isUseTotalInGraph,Obligated)
                                                    values(" + MainGroup + "," + itemLevel +" ,'"+RowNo
                                                    +"','"+Particular+"','"+AccountCode+"','"+IncomeClassification+"',"+PastYear+","+CurrentYear+","+BudgetYear+","+OrderNo+",1,"+YearOf
                                                    +","+FundType+","+isUseAmount+","+isBold+","+HasFooterTotal+","+UseTotalinGraph+","+ txtCurrentYearActual + ")", con);
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
        public string UpdateParticular(int SeriesID,int FundType, int YearOf, string MainGroup, string RowNo, string Particular, string AccountCode, string IncomeClassification,
            string PastYear, string CurrentYear, string BudgetYear, int OrderNo, string isBold, string isUseAmount, string HasFooterTotal, string UseTotalinGraph, string txtCurrentYearActual)
        {
            var itemLevel = MainGroup == "NULL" ? "0" : "(select ItemLevel + 1 from tbl_R_BMSLBPForm1Data where SeriesID = " + MainGroup + ")";
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Update tbl_R_BMSLBPForm1Data 
                                                   set MainSeriesID = " + MainGroup + ",ItemLevel = " + itemLevel + ",RowNo ='" + RowNo + "',Particular = '" + Particular + "',AccountCode ='" + AccountCode
                                                  +"',IncomeClassification ='"+IncomeClassification+"',PastYear ="+PastYear+",CurrentYear="+CurrentYear
                                                  +",BudgetYear = "+BudgetYear+",OrderNo = "+OrderNo+",isUseAmount = "+isUseAmount+",isBold = "+isBold+",hasFooterTotal = "+HasFooterTotal
                                                  +",isUseTotalInGraph = "+UseTotalinGraph+ ",Obligated=" + txtCurrentYearActual + " where SeriesID = " + SeriesID+ "", con);
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