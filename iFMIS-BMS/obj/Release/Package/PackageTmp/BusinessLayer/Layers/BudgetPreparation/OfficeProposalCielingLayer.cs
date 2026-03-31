using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models.Budget_Preparation;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation
{
    public class OfficeProposalCielingLayer
    {
        public IEnumerable<OfficeCielingModel> grOfficeCieling(int isNonOffice)
        {
            List<OfficeCielingModel> List = new List<OfficeCielingModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"sp_BMS_GetPreparationCieling " + (DateTime.Now.Year + 1) + "," + DateTime.Now.Year + ",0," + isNonOffice + ",0", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    OfficeCielingModel Item = new OfficeCielingModel();
                    Item.SeriesID = Convert.ToInt32(reader.GetValue(0));
                    Item.OfficeName = reader.GetValue(2).ToString();
                    Item.Percentage = Convert.ToInt32(reader.GetValue(3));
                    Item.Amount = Convert.ToDouble(reader.GetValue(4));
                    Item.OriginalAmount = Convert.ToDouble(reader.GetValue(5));
                    Item.PercentageIncrease = Convert.ToDouble(reader.GetValue(6));
                    Item.ActionCode = Convert.ToInt32(reader.GetValue(7));
                    List.Add(Item);

                }
            }
            return List;
        }
        public string UpdateOfficeCielingPercentage(int SeriesID, int Percentage)
        {
            try
            {
                double PercentValue = Convert.ToDouble(Percentage) / Convert.ToDouble(100);
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand("Update tbl_R_BMSPreparationCieling set CielingPercentage = " + PercentValue + " where SeriesID = " + SeriesID + "", con);
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
        public double getTotalProposedPerOffice(int OfficeID, int YearOf,int ProgramID, string ProgramDescription)
        {
            try
            {
                var Result = 0.0;
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    if (ProgramDescription == "Special Projects/Activities")
                    {
                        SqlCommand com = new SqlCommand(@"Select isnull([TotalProposeSProject],0) from tbl_R_BMSPreparationCieling where OfficeID = " + OfficeID + " and YearOf = " + YearOf + " and ActionCode=1", con);
                        con.Open();
                        Result = Convert.ToDouble(com.ExecuteScalar());
                    }
                    else
                    {
                        SqlCommand com = new SqlCommand(@"Select isnull(TotalProposed,0) from tbl_R_BMSPreparationCieling where OfficeID = " + OfficeID + " and YearOf = " + YearOf + " and ActionCode=1", con);
                        con.Open();
                        Result = Convert.ToDouble(com.ExecuteScalar());
                    }
                }
                return Result;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public string UpdateTotalProposed(int OfficeID, int YearOf, int ProgramID,string ProgramDescription)
        {
            try
            {
            var Result = 0.0;
            int OfficeIDParam = OfficeID;
            int isTransfered = 0;
            int UserID = Convert.ToInt32(Account.UserInfo.eid);
            GetPMISOfficeID(ref OfficeIDParam, ref isTransfered);
            if (ProgramID == 43 && Account.UserInfo.UserTypeDesc == "Budget In-Charge")
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"select sum(isnull(TotalAmount,0))  FROM tbl_T_BMSAccountDenomination as a WHERE a.ProgramID = 43 
					and a.ActionCode = 1 and a.TransactionYear = " + YearOf + " and OfficeID = " + OfficeID + "", con);
                    con.Open();
                    Result = Convert.ToDouble(com.ExecuteScalar());
                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                        //SqlCommand com = new SqlCommand(@"select isnull(c.AccountCode,0),a.ObjectOfExpendetureID,b.ProgramID,a.AccountID 
                        //                            from tbl_R_BMSProgramAccounts as a 
                        //                            LEFT JOIN tbl_R_BMSOfficePrograms as b on b.ProgramID = a.ProgramID 
                        //                            and a.ActionCode = b.ActionCode and a.AccountYear = b.ProgramYear
                        //                            LEFT JOIN tbl_R_BMSAccounts as c on c.FMISAccountCode = a.AccountID and a.ActionCode = c.Active
                        //                            where a.ActionCode = 1 and b.OfficeID = " + OfficeID + " and a.AccountYEar = " + YearOf + "", con);
                    SqlCommand com = new SqlCommand(@"exec [sp_BMS_TotalProposeAmount] " + OfficeID + ","+ YearOf + ","+ ProgramID + "", con);
                     con.Open();
                    Result = Convert.ToDouble(com.ExecuteScalar());
                    //SqlDataReader reader = com.ExecuteReader();
                    //while (reader.Read())
                    //{
                    //    Result = Result + ExecuteProcedure(Convert.ToInt32(reader.GetValue(0)), OfficeIDParam, YearOf,
                    //                        Convert.ToInt32(reader.GetValue(1)), Convert.ToInt32(reader.GetValue(2)), Convert.ToInt32(reader.GetValue(3)),
                    //                        UserID, OfficeID, isTransfered);
                    //}
                }
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                    //if (ProgramDescription != "Contractual/Statutory Obligations" && ProgramDescription != "Reserve")
                    //{
                        if (ProgramDescription == "Special Projects/Activities")
                        {
                            SqlCommand com = new SqlCommand(@"Update tbl_R_BMSPreparationCieling set [TotalProposeSProject] = " + Result + " where OfficeID = " + OfficeID + " and YearOf = " + YearOf + "", con);
                            con.Open();
                            com.ExecuteNonQuery();
                        }
                        else
                        {
                            SqlCommand com = new SqlCommand(@"Update tbl_R_BMSPreparationCieling set TotalProposed = " + Result + " where OfficeID = " + OfficeID + " and YearOf = " + YearOf + "", con);
                            con.Open();
                            com.ExecuteNonQuery();
                        }
                    //}
            }
            return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public double ExecuteProcedure(int AccountCode, int PMISOfficeID, int YearOf,int OOEID, int ProgramID,int FMISAccountCode,int UserID,int IFMISOfficeID,int isTransfered)
        {
            var Query = "";
            if (isTransfered == 1)
            {
                Query = @"EXEC sp_BMS_getComputation_Transfered " + AccountCode + "," + PMISOfficeID + "," + YearOf + "," + OOEID
                    + "," + ProgramID + ",0," + IFMISOfficeID + "," + FMISAccountCode + "," + UserID + "";
            }
            else
            {
                Query = @"EXEC sp_bms_getComputationTotal " + AccountCode + "," + PMISOfficeID + "," + YearOf + "," + OOEID
                    + "," + ProgramID + ",0," + FMISAccountCode + "," + UserID + "";
            }

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(Query, con);
                com.CommandTimeout = 0;
                con.Open();
                return com.ExecuteScalar() == null ? 0.0 : Convert.ToDouble(com.ExecuteScalar());
            }
        }
        public void GetPMISOfficeID(ref int OfficeIDParam, ref int isTransfred)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select top 1 pmisOfficeID,case when OfficeID in(select subOfficeID_Ifmis 
                                                 from tbl_R_BMSMainAndSubOffices where subOfficeID_Ifmis = " + OfficeIDParam + ") then 1 else 0 end from tbl_R_BMSOffices where OfficeID =" + OfficeIDParam + " ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                reader.Read();
                OfficeIDParam = Convert.ToInt32(reader.GetValue(0));
                isTransfred = Convert.ToInt32(reader.GetValue(1));
            }
        }
        public double getOfficeTotalCieling(int OfficeID, int YearOf, int ProgramID)
        {
            var isNonOffice = ProgramID == 43 && Account.UserInfo.UserTypeDesc == "Budget In-Charge" ? 1 : 0;
           
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"sp_BMS_GetPreparationCieling " + YearOf + "," + (YearOf - 1) + "," + OfficeID + "," + isNonOffice + ","+ ProgramID + "", con);
                con.Open();
                return com.ExecuteScalar() == DBNull.Value ? 0 : Convert.ToDouble(com.ExecuteScalar());
                //return Convert.ToDouble(com.ExecuteScalar());
            }
        }
        public string getOffiCeiling(int OfficeID,int proyear)
        {
           
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select top 1 ActionCode from tbl_R_BMSPreparationCieling where OfficeID=" + OfficeID + " and [YearOf]="+ proyear + "", con);
                con.Open();
                return com.ExecuteScalar().ToString();
            }
        }
        public string UpdateCeilingStatus(int SeriesID, int ActionCode)
        {
           
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Update tbl_R_BMSPreparationCieling set ActionCode = " + (ActionCode == 0 ?  1 : 0) + " where SeriesID = " + SeriesID + "", con);
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