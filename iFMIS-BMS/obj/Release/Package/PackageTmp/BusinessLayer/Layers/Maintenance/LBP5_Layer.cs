using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers.Maintenance
{
    public class LBP5_Layer
    {
        public string SumbitOBJ(string ObjName, int? OrderBy)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var OfficeID = @Account.UserInfo.Department;
                var Name = ObjName.ToString();
                String ObjectivesName = Name.Replace("'", "''");
                SqlCommand com = new SqlCommand(@"Insert into tbl_R_LBP5_Objectives values('" + ObjectivesName + "', '" + OrderBy + "', '" + OfficeID + "',  YEAR(GETDATE())+1 , 1)", con);
                con.Open();
                com.ExecuteNonQuery();
                return "success";
            }
        }
        public string SumbitFS(string FSName, int? OrderBy)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var OfficeID = @Account.UserInfo.Department;
                var Name = FSName.ToString();
                String FunctionalName = Name.Replace("'", "''");
                SqlCommand com = new SqlCommand(@"Insert into tbl_R_LBP5_FS values('" + FunctionalName + "', '" + OrderBy + "', '" + OfficeID + "',  YEAR(GETDATE())+1 , 1)", con);
                con.Open();
                com.ExecuteNonQuery();
                return "success";
            }
        }
        public string SumbitMFO(string ObjName, double? MFOCost, int? OrderBy)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var OfficeID = @Account.UserInfo.Department;
                var Cost = MFOCost == 0 || MFOCost == null ? 0 : MFOCost;
                var Name = ObjName.ToString();
                String FunctionalName = Name.Replace("'", "''");
                SqlCommand com = new SqlCommand(@"Insert into tbl_R_LBP5_PPA_MFO values('" + FunctionalName + "', '" + OfficeID + "',  YEAR(GETDATE())+1 , 1, '" + Cost + "', '"+OrderBy+"')", con);
                con.Open();
                com.ExecuteNonQuery();
                return "success";
            }
        }
        public string SumbitMFOBreakdown(string PPA_CodeRef, string PPADescription, double? AccountList, string PPA_TARGET, string PPA_Output_Indicator, string From, string To, int? Send_MFO_ID, int? PPA_DescriptionID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                var Name = PPADescription.ToString();
                String PPAMFOName = Name.Replace("'", "''");
                var OutputText = PPA_Output_Indicator.ToString();
                String PPAOutputText = OutputText.Replace("'", "''");
                var TargetText = PPA_TARGET.ToString();
                String PPATargetText = TargetText.Replace("'", "''");
                SqlCommand com = new SqlCommand(@"INSERT into tbl_R_LBP5_PPA_Denomination values('" + Send_MFO_ID + "', '" + PPA_CodeRef + "', '" + (PPA_DescriptionID == null ? PPAMFOName : null) + "', '" + AccountList + "', '" + PPAOutputText + "', '" + PPATargetText + "', '" + From + "', '" + To + "', YEAR(GETDATE())+1, 1, '" + (PPA_DescriptionID == 0 ? 0 : PPA_DescriptionID) + "', '" + (PPA_DescriptionID == 0 ? 0 : PPA_DescriptionID) + "')", con);
                con.Open();
                com.ExecuteNonQuery();
                if (PPA_DescriptionID == null)
                {
                    SqlCommand select = new SqlCommand(@"SELECT PPA_ID FROM tbl_R_LBP5_PPA_Denomination WHERE PPA_Description = '" + PPAMFOName + "' and PPA_MFO_ID = '" + Send_MFO_ID + "'", con);
                    var select_ppa_id = select.ExecuteScalar();
                    SqlCommand update = new SqlCommand(@"UPDATE tbl_R_LBP5_PPA_Denomination set Sub_PPA = '" + select_ppa_id + "' WHERE PPA_ID = '" + select_ppa_id + "'", con);
                    //con.Open();
                    update.ExecuteNonQuery();
                }
                return "success";
            }
        }
        public string ViewFSContent(int? YearDate, int? OfficeIDParam)
        {
            var FSDescription = "";
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT TOP 1 isnull(FS_Description, ' ') FROM tbl_R_LBP5_FS WHERE OfficeID = '" + OfficeIDParam + "' and TransactionYear = '" + YearDate + "' and ActionCode = 1 ORDER BY FS_ID DESC", con);
                con.Open();
                FSDescription = com.ExecuteScalar().ToString();
                return FSDescription;
            }
        }
        public string FSUpdate(string LBP5_FS, int? IDparam)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"INSERT into tbl_R_LBP5_FS values ('" + LBP5_FS + "', '" + IDparam + "', YEAR(GETDATE())+1, 1)", con);
                con.Open();
                com.ExecuteNonQuery();
                return "success";
            }
        }
        public string DeleteOBJ(int? OBJ_ID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"UPDATE tbl_R_LBP5_Objectives set ActionCode = 2 WHERE OBJ_ID = '" + OBJ_ID + "' and TransactionYear = YEAR(GETDATE())+1", con);
                con.Open();
                com.ExecuteNonQuery();
                return "success";
            }
        }
        public string DeleteFS(int? FS_ID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"UPDATE tbl_R_LBP5_FS set ActionCode = 2 WHERE FS_ID = '" + FS_ID + "' and TransactionYear = YEAR(GETDATE())+1", con);
                con.Open();
                com.ExecuteNonQuery();
                return "success";
            }
        }
        public string DeleteMFO(int? PPA_MFO_ID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"UPDATE tbl_R_LBP5_PPA_MFO set ActionCode = 2 WHERE PPA_MFO_ID = '" + PPA_MFO_ID + "' and TransactionYear = YEAR(GETDATE())+1", con);
                con.Open();
                com.ExecuteNonQuery();
                return "success";
            }
        }
        public string DELETE_MFO_Breakdown(int? PPA_ID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"UPDATE tbl_R_LBP5_PPA_Denomination set ActionCode = 2 WHERE PPA_ID = '" + PPA_ID + "' and TransactionYear = YEAR(GETDATE())+1", con);
                con.Open();
                com.ExecuteNonQuery();
                return "success";
            }
        }
        public string SaveEditOBJ(string ObjNameEDIT, string OrderBy, int ObjID)
        {
            String PPAObjName = ObjNameEDIT.Replace("'", "''");
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"UPDATE tbl_R_LBP5_Objectives set OBJ_OrderBy = '" + OrderBy + "', OBJ_Description = '" + PPAObjName + "' WHERE OBJ_ID = '" + ObjID + "' and TransactionYear = YEAR(GETDATE())+1", con);
                con.Open();
                com.ExecuteNonQuery();
                return "success";
            }
        }
        public string SaveEditFS(string FSNameEDIT, string OrderBy, int FSID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                String PPAFSName = FSNameEDIT.Replace("'", "''");
                SqlCommand com = new SqlCommand(@"UPDATE tbl_R_LBP5_FS set FS_OrderBy = '" + OrderBy + "', FS_Description = '" + PPAFSName + "' WHERE FS_ID = '" + FSID + "' and TransactionYear = YEAR(GETDATE())+1", con);
                con.Open();
                com.ExecuteNonQuery();
                return "success";
            }
        }
        public string SaveEditMFO(int? MFOID, string MFONameEdit, double? MFO_COST, int? OrderBy)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                String PPAMFOName = MFONameEdit.Replace("'", "''");
                SqlCommand com = new SqlCommand(@"UPDATE tbl_R_LBP5_PPA_MFO set PPA_Description = '" + PPAMFOName + "', PPA_MFO_Cost = '" + MFO_COST + "', PPA_OrderBy = '" + OrderBy + "' WHERE PPA_MFO_ID = '" + MFOID + "' and TransactionYear = YEAR(GETDATE())+1", con);
                con.Open();
                com.ExecuteNonQuery();
                return "success";
            }
        }
        public string SaveEditBreakdown(int PPA_ID, string PPA_CodeRef, string PPADescription, int? AccountList, string PPA_TARGET, string PPA_Output_Indicator, string From, string To, int Send_MFO_ID, int? PPA_DescriptionID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                String PPADescName = PPADescription.Replace("'", "''");
                String PPATargetName = PPA_TARGET.Replace("'", "''");
                String PPAOutputName = PPA_Output_Indicator.Replace("'", "''");
                //SqlCommand select = new SqlCommand(@"SELECT PPA_Description FROM tbl_R_LBP5_PPA_Denomination WHERE PPA_ID = '" + PPA_ID + "'", con);
                con.Open();
                //var result_select = select.ExecuteScalar().ToString();

                SqlCommand com = new SqlCommand(@"UPDATE tbl_R_LBP5_PPA_Denomination set PPA_CodeRef = '" + PPA_CodeRef + "', PPA_Description = '" + PPADescName + "', Cost = '" + AccountList + "', PPA_Output_Indicator = '" + PPAOutputName + "' , PPA_Target = '" + PPATargetName + "' , PPA_Implement_FROM = '" + From + "' , PPA_Implement_TO = '" + To + "', TransactionYear = YEAR(GETDATE())+1 WHERE  PPA_ID = '" + PPA_ID + "' and PPA_MFO_ID = '" + Send_MFO_ID + "'", con);

                com.ExecuteNonQuery();

                return "success";
            }
        }
        public void UpdateFSOrder(IEnumerable<lbp5> FS)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                foreach (lbp5 FSorder in FS)
                {
                    var userID = Account.UserInfo.eid;
                    SqlCommand com = new SqlCommand(@"UPDATE tbl_R_LBP5_FS set FS_OrderBy = '"+FSorder.FS_OrderBy+"' WHERE FS_ID = '"+FSorder.FS_ID+"'", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public void UpdateOBJOrder(IEnumerable<lbp5> Objectives)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                foreach (lbp5 OBJorder in Objectives)
                {
                    var userID = Account.UserInfo.eid;
                    SqlCommand com = new SqlCommand(@"UPDATE tbl_R_LBP5_Objectives set OBJ_OrderBy = '" + OBJorder.OBJ_OrderBy + "' WHERE OBJ_ID = '" + OBJorder.OBJ_ID + "'", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        public void UpdateOrder(IEnumerable<lbp5> Breakdown)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                foreach (lbp5 BreakDownOrder in Breakdown)
                {
                    var userID = Account.UserInfo.eid;
                    SqlCommand com = new SqlCommand(@"UPDATE tbl_R_LBP5_PPA_Denomination set OrderBy = '" + BreakDownOrder.PPA_OrderBy + "' WHERE PPA_ID = '" + BreakDownOrder.PPA_ID + "'", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        public void UpdateMFOOrder(IEnumerable<lbp5> MFOBreakdown)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                foreach (lbp5 MFOBreakDownOrder in MFOBreakdown)
                {
                    var userID = Account.UserInfo.eid;
                    SqlCommand com = new SqlCommand(@"UPDATE tbl_R_LBP5_PPA_mfo set PPA_OrderBy = '" + MFOBreakDownOrder.PPA_MFOOrderBy + "' WHERE PPA_MFO_ID = '" + MFOBreakDownOrder.PPA_MFO_ID + "'", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        public string CopyLBP5Data(int OfficeID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"sp_bms_CopyLBP5Data " + DateTime.Now.Year + "," + (DateTime.Now.Year + 1) + "," + OfficeID + "", con);
                    con.Open();
                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}