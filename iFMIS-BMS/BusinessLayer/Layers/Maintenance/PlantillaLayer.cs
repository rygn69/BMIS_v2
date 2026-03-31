using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation;
using iFMIS_BMS.BusinessLayer.Models;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;
using iFMIS_BMS.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers.Maintenance
{
    public class PlantillaLayer
    {
        //public string BudgetYear = (Convert.ToInt32(GetTransactionYear.TransactionYear()) - 1).ToString();
        //temporary update- dynamic used of plantilla year using dropdown- xXx - 10/16/2019
        //public string PlantillaYear = GetTransactionYear.TransactionYear();
        
        public IEnumerable<PlantillaModel> grPlantillaList(int OfficeID=0, int PlanYear=0)
        {
            OfficeAdmin_Layer OfficeAdmin_Layer = new OfficeAdmin_Layer();

            var OfficeIDParam = 0;
            var MotherOfficeID = 0;
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT isnull(MainOfficeID_PMIS,0) 
                FROM [dbo].[tbl_R_BMSMainAndSubOffices] where SubOfficeID_IFMIS =" + OfficeID + "", con);
                con.Open();
                MotherOfficeID = Convert.ToInt32(com.ExecuteScalar());
            }
            if (MotherOfficeID == 0)
            {
                OfficeIDParam = OfficeAdmin_Layer.getPmisOfficeID(OfficeID);
            }
            else
            {
                OfficeIDParam = MotherOfficeID;
            }
            List<PlantillaModel> PlantillaList = new List<PlantillaModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"dbo.sp_get_data_from_Plantilla " + PlanYear + ", " + OfficeIDParam + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                var CurrentGroup = "";
                var GroupOrder = 0;
                var PlantillaOrder = 0;
                while (reader.Read())
                {
                    if (CurrentGroup != reader.GetValue(1).ToString())
                    {
                        GroupOrder = GroupOrder + 1;
                        CurrentGroup = reader.GetValue(1).ToString();
                        PlantillaOrder = 0;
                    }
                    PlantillaOrder = PlantillaOrder + 1;
                    PlantillaModel Plantilla = new PlantillaModel();
                    Plantilla.SeriesID = Convert.ToInt64(reader.GetValue(0));
                    Plantilla.DivName = reader.GetValue(1).ToString();
                    Plantilla.ItemNo = reader.GetValue(2).ToString();
                    Plantilla.ItemNoNew = reader.GetValue(3).ToString();
                    Plantilla.Position = reader.GetValue(4).ToString();
                    Plantilla.sg = reader.GetValue(5).ToString();
                    Plantilla.StepNew = Convert.ToInt32(reader.GetValue(6));
                    Plantilla.EmployeeName = reader.GetValue(7).ToString();
                    Plantilla.AppointmentDate = reader.GetValue(8).ToString();
                    Plantilla.ProposedSalary = Convert.ToDouble(reader.GetValue(9));
                    Plantilla.YearlySalary = Convert.ToDouble(reader.GetValue(10));
                    Plantilla.ForStepIncrement = reader.GetInt32(11);
                    Plantilla.GroupOrder = GroupOrder;
                    Plantilla.PlantillaOrdering = PlantillaOrder;
                    Plantilla.step = Convert.ToInt32(reader.GetValue(12));
                    Plantilla.oldSG = Convert.ToInt32(reader.GetValue(13));

                    PlantillaList.Add(Plantilla);
                }
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select distinct a.ProposedItemID as 'ProposedItemID(0)',case when a.DivisionID = 0 then 'No Division'else b.rowno +' ' +  b.DivName end as 'DivName(1)',c.Pos_name as 'Position(2)',a.AppointmentDateEffectivity as 'Effectivity(3)', 
                                                isnull(pmis.dbo.sclr_fnc_get_salary(c.sg,1,(select aeffectivity from pmis.dbo.EDGE_tblEffectivityDate where yearof = " + PlanYear + " )),0) as 'Salary(4)'," + ""
                                                + " isnull(pmis.dbo.sclr_fnc_get_salary(c.sg,1,(select aeffectivity from pmis.dbo.EDGE_tblEffectivityDate where yearof = " + PlanYear + " )) * " + ""
                                                + " (13 - DATEPART(mm,a.AppointmentDateEffectivity)),0) as 'YearlySalary(5)', c.sg as 'sg(6)',case when a.ActionCode = 1 then 2 else 4 end as 'Indicator(7)' from tbl_R_BMSProposedNewItem as a " + ""
                                                + " inner join pmis.dbo.EDGE_tblPlantillaDivision as b on b.DivID = a.DivisionID or a.DivisionID = 0" + ""
                                                + " inner JOIN pmis.dbo.RefsPositions as c on c.PositionCOde = a.PositionID where a.ActionCode != 0 and a.OfficeID=" + OfficeID + "and a.Yearof = " + PlanYear + " + 1 ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    PlantillaModel Plantilla = new PlantillaModel();
                    Plantilla.SeriesID = Convert.ToInt64(reader.GetValue(0));
                    Plantilla.DivName = reader.GetValue(1).ToString();
                    Plantilla.ItemNo = "";
                    Plantilla.ItemNoNew = "";
                    Plantilla.Position = reader.GetValue(2).ToString();
                    Plantilla.EmployeeName = "(For Funding)";
                    Plantilla.AppointmentDate = reader.GetDateTime(3).ToShortDateString();
                    Plantilla.sg = reader.GetValue(6).ToString();
                    Plantilla.oldSG = Convert.ToInt32(reader.GetValue(6));
                    Plantilla.step = 1;
                    Plantilla.StepNew = 1;
                    Plantilla.ProposedSalary = Convert.ToDouble(reader.GetValue(4));
                    Plantilla.YearlySalary = Convert.ToDouble(reader.GetValue(5));
                    Plantilla.ForStepIncrement = Convert.ToInt32(reader.GetValue(7));
                    Plantilla.GroupOrder = PlantillaList.Count(go => go.DivName == reader.GetValue(1).ToString()) != 0 ?
                        PlantillaList.Where(go => go.DivName == reader.GetValue(1).ToString()).ElementAt(0).GroupOrder : PlantillaList.Count() != 0 ? PlantillaList.Select(pl => pl.GroupOrder).ElementAt(PlantillaList.Count - 1) + 1 : 0;
                    Plantilla.PlantillaOrdering = PlantillaList.Count(go => go.DivName == reader.GetValue(1).ToString()) != 0 ? PlantillaList.Where(go => go.DivName == reader.GetValue(1).ToString()).OrderByDescending(po => po.PlantillaOrdering).ElementAt(0).PlantillaOrdering + 1 : 1;
                    
                    PlantillaList.Add(Plantilla);
                }
            }
//            using (SqlConnection con = new SqlConnection(Common.MyConn()))
//            {
//                SqlCommand com = new SqlCommand(@"select a.ProposedItemID, a.AppointmentDateEffectivity,b.CasualRateName,
//                                                b.CasualRate * 22 as 'MonthlySalary',b.CasualRate * 22 * (13 - DATEPART(mm,a.AppointmentDateEffectivity)) as 'Anual Salary'  
//                                                ,case when a.ActionCode = 1 then 3 else 4 end as 'Indicator(5)' from tbl_R_BMSProposedNewItem as a 
//                                                inner JOIN tbl_R_BMSCasualRate as b on b.CasualRateID = a.EmploymentStatusID
//                                                where a.PositionID is null and a.DivisionID is null and a.officeID = " + OfficeID + " and a.actioncode != 0 and a.yearof = " + PlantillaYear + " + 1", con);
//                con.Open();
//                SqlDataReader reader = com.ExecuteReader();
//                while (reader.Read())
//                {
//                    PlantillaModel Plantilla = new PlantillaModel();
//                    Plantilla.SeriesID = Convert.ToInt64(reader.GetValue(0));
//                    Plantilla.DivName = "";
//                    Plantilla.ItemNo = "";
//                    Plantilla.ItemNoNew = "";
//                    Plantilla.Position = "(Proposed Casual) " + reader.GetValue(2).ToString();
//                    Plantilla.EmployeeName = "(For Funding)";
//                    Plantilla.AppointmentDate = reader.GetDateTime(1).ToShortDateString();
//                    Plantilla.sg = "0";
//                    Plantilla.step = 0;
//                    Plantilla.ProposedSalary = Convert.ToDouble(reader.GetValue(3));
//                    Plantilla.YearlySalary = Convert.ToDouble(reader.GetValue(4));
//                    Plantilla.ForStepIncrement = Convert.ToInt32(reader.GetValue(5));
//                    Plantilla.GroupOrder = PlantillaList.Count(go => go.DivName == reader.GetValue(1).ToString()) != 0 ? 
//                        PlantillaList.Where(go => go.DivName == reader.GetValue(1).ToString()).ElementAt(0).GroupOrder : PlantillaList.Count() != 0 ? PlantillaList.Select(pl => pl.GroupOrder).ElementAt(PlantillaList.Count - 1) + 1 : 0;
//                    Plantilla.PlantillaOrdering = PlantillaList.Count(go => go.DivName == reader.GetValue(1).ToString()) != 0 ? PlantillaList.Where(go => go.DivName == reader.GetValue(1).ToString()).OrderByDescending(po => po.PlantillaOrdering).ElementAt(0).PlantillaOrdering + 1 : 1;

//                    PlantillaList.Add(Plantilla);
//                }
//         }

            return PlantillaList;
        }
        public IEnumerable<PlantillaModel> grCasualList(int OfficeID,int PlanYear)
        {

            List<PlantillaModel> PlantillaList = new List<PlantillaModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Select a.SeriesID,b.CasualRateName,Cast(isnull(a.SalaryEffectivityDate,'" + (Convert.ToInt32(PlanYear) + 1) + "-01-01') as varchar(max)),(CasualRate * 22), Round((CasualRate * 22) *  "+ ""
                                                + "  (13 - DATEPART(mm,isnull(SalaryEffectivityDate,''))),0), a.EmploymentStatusID ,isnull(c.Lastname +', '+c.Firstname + ' '+left(c.MI,1) + '. ' + isnull(c.Suffix,''),'Vacant') as Name" + ""
                                                +" from tbl_R_BMSVacantAndTransferedCasual as a  " + ""
                                                + " inner join tbl_R_BMSCasualRate as b on a.EmploymentStatusID = b.CasualRateID and a.ActionCode = b.ActionCode and b.BudgetYear = a.Yearof  left join " + ""
                                                + " pmis.dbo.employee as c on c.eid = a.Eid "+ ""
                                                + " where OfficeID = " + OfficeID + " and Yearof = " + PlanYear + " + 1 and a.ActionCode = 1 order by c.Lastname,c.Firstname,b.CasualrateName desc", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    PlantillaModel Plantilla = new PlantillaModel();
                    Plantilla.SeriesID = Convert.ToInt64(reader.GetValue(0));
                    Plantilla.rate = reader.GetValue(1).ToString();
                    Plantilla.AppointmentDate = reader.GetValue(2).ToString();
                    Plantilla.ProposedSalary = Convert.ToDouble(reader.GetValue(3));
                    Plantilla.YearlySalary = Convert.ToDouble(reader.GetValue(4));
                    Plantilla.DivID = Convert.ToInt32(reader.GetValue(5));
                    Plantilla.EmployeeName = reader.GetValue(6).ToString();

                    PlantillaList.Add(Plantilla);
                }
            }

            return PlantillaList;
        }
        public IEnumerable<PlantillaModel> grPlantillaListSubOffice(int OfficeID,int PlantillaYear)
        {
            //dd
            var MotherOfficeID = 0;

            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT isnull(MainOfficeID_PMIS,0) 
                FROM [dbo].[tbl_R_BMSMainAndSubOffices] where SubOfficeID_IFMIS =" + OfficeID + "", con);
                con.Open();
                MotherOfficeID = Convert.ToInt32(com.ExecuteScalar());
            }
            List<PlantillaModel> PlantillaList = new List<PlantillaModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"sp_get_data_transfered_to_subOffice " + PlantillaYear + ", " + MotherOfficeID + "," + OfficeID + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                var CurrentGroup = "";
                var GroupOrder = 0;
                var PlantillaOrder = 0;
                while (reader.Read())
                {
                    if (CurrentGroup != reader.GetValue(1).ToString())
                    {
                        GroupOrder = GroupOrder + 1;
                        CurrentGroup = reader.GetValue(1).ToString();
                        PlantillaOrder = 0;
                    }
                    PlantillaModel Plantilla = new PlantillaModel();
                    Plantilla.SeriesID = Convert.ToInt64(reader.GetValue(0));
                    Plantilla.DivName = reader.GetValue(1).ToString();
                    Plantilla.ItemNo = reader.GetValue(2).ToString();
                    Plantilla.ItemNoNew = reader.GetValue(3).ToString();
                    Plantilla.Position = reader.GetValue(4).ToString();
                    Plantilla.sg = reader.GetValue(5).ToString();
                    Plantilla.step = Convert.ToInt32(reader.GetValue(6));
                    Plantilla.EmployeeName = reader.GetValue(7).ToString();
                    Plantilla.AppointmentDate = reader.GetValue(8).ToString();
                    Plantilla.ProposedSalary = Convert.ToDouble(reader.GetValue(9));
                    Plantilla.YearlySalary = Convert.ToDouble(reader.GetValue(10));
                    Plantilla.ForStepIncrement = reader.GetInt32(11);
                    Plantilla.GroupOrder = GroupOrder;
                    Plantilla.PlantillaOrdering = PlantillaOrder;

                    PlantillaList.Add(Plantilla);
                }
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select a.ProposedItemID as 'ProposedItemID(0)',isnull(b.rowno +' ' +  b.DivName,'No Division') as 'DivName(1)',c.Pos_name as 'Position(2)',a.AppointmentDateEffectivity as 'Effectivity(3)', 
                                                pmis.dbo.sclr_fnc_get_salary(c.sg,1,(select aeffectivity from pmis.dbo.EDGE_tblEffectivityDate where yearof = " + PlantillaYear + " )) as 'Salary(4)'," + ""
                                                + " pmis.dbo.sclr_fnc_get_salary(c.sg,1,(select aeffectivity from pmis.dbo.EDGE_tblEffectivityDate where yearof = " + PlantillaYear + " )) * " + ""
                                                + " (13 - DATEPART(mm,a.AppointmentDateEffectivity)) as 'YearlySalary(5)', c.sg as 'sg(6)',case when a.ActionCode = 1 then 2 else 4 end as 'Indicator(7)' from tbl_R_BMSProposedNewItem as a " + ""
                                                + " left join pmis.dbo.EDGE_tblPlantillaDivision as b on b.DivID = a.DivisionID" + ""
                                                + " left JOIN pmis.dbo.RefsPositions as c on c.PositionCOde = a.PositionID where a.ActionCode != 0 and a.OfficeID=" + OfficeID + "and a.Yearof = " + PlantillaYear + " + 1 ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    PlantillaModel Plantilla = new PlantillaModel();
                    Plantilla.SeriesID = Convert.ToInt64(reader.GetValue(0));
                    Plantilla.DivName = reader.GetValue(1).ToString();
                    Plantilla.ItemNo = "";
                    Plantilla.ItemNoNew = "";
                    Plantilla.Position = reader.GetValue(2).ToString();
                    Plantilla.EmployeeName = "(For Funding)";
                    Plantilla.AppointmentDate = reader.GetDateTime(3).ToShortDateString();
                    Plantilla.sg = reader.GetValue(6).ToString();
                    Plantilla.step = 1;
                    Plantilla.ProposedSalary = Convert.ToDouble(reader.GetValue(4));
                    Plantilla.YearlySalary = Convert.ToDouble(reader.GetValue(5));
                    Plantilla.ForStepIncrement = Convert.ToInt32(reader.GetValue(7));
                    //Plantilla.GroupOrder = PlantillaList.Count(go => go.DivName == reader.GetValue(1).ToString()) != 0 ? PlantillaList.Where(go => go.DivName == reader.GetValue(1).ToString()).ElementAt(0).GroupOrder : 0;
                    Plantilla.GroupOrder = PlantillaList.Count(go => go.DivName == reader.GetValue(1).ToString()) != 0 ?
                        PlantillaList.Where(go => go.DivName == reader.GetValue(1).ToString()).ElementAt(0).GroupOrder : PlantillaList.Count() != 0 ? PlantillaList.Select(pl => pl.GroupOrder).ElementAt(PlantillaList.Count - 1) + 1 : 0;
                    Plantilla.PlantillaOrdering = PlantillaList.Count(go => go.DivName == reader.GetValue(1).ToString()) != 0 ? PlantillaList.Where(go => go.DivName == reader.GetValue(1).ToString()).OrderByDescending(po => po.PlantillaOrdering).ElementAt(0).PlantillaOrdering + 1 : 1;

                    PlantillaList.Add(Plantilla);
                }
            }
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select a.ProposedItemID, a.AppointmentDateEffectivity,b.CasualRateName,
                                                b.CasualRate * 22 as 'MonthlySalary',b.CasualRate * 22 * (13 - DATEPART(mm,a.AppointmentDateEffectivity)) as 'Anual Salary'  
                                                ,case when a.ActionCode = 1 then 3 else 4 end as 'Indicator(5)' from tbl_R_BMSProposedNewItem as a 
                                                inner JOIN tbl_R_BMSCasualRate as b on b.CasualRateID = a.EmploymentStatusID
                                                where a.PositionID is null and a.DivisionID is null and a.officeID = " + OfficeID + " and a.actioncode != 0 and a.yearof = " + PlantillaYear + " + 1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    PlantillaModel Plantilla = new PlantillaModel();
                    Plantilla.SeriesID = Convert.ToInt64(reader.GetValue(0));
                    Plantilla.DivName = "";
                    Plantilla.ItemNo = "";
                    Plantilla.ItemNoNew = "";
                    Plantilla.Position = "(Proposed Casual) " + reader.GetValue(2).ToString();
                    Plantilla.EmployeeName = "(For Funding)";
                    Plantilla.AppointmentDate = reader.GetDateTime(1).ToShortDateString();
                    Plantilla.sg = "0";
                    Plantilla.step = 0;
                    Plantilla.ProposedSalary = Convert.ToDouble(reader.GetValue(3));
                    Plantilla.YearlySalary = Convert.ToDouble(reader.GetValue(4));
                    Plantilla.ForStepIncrement = Convert.ToInt32(reader.GetValue(5));
                    Plantilla.GroupOrder = PlantillaList.Count(go => go.DivName == reader.GetValue(1).ToString()) != 0 ?
                        PlantillaList.Where(go => go.DivName == reader.GetValue(1).ToString()).ElementAt(0).GroupOrder : PlantillaList.Count() != 0 ? PlantillaList.Select(pl => pl.GroupOrder).ElementAt(PlantillaList.Count - 1) + 1 : 0;
                    Plantilla.PlantillaOrdering = PlantillaList.Count(go => go.DivName == reader.GetValue(1).ToString()) != 0 ? PlantillaList.Where(go => go.DivName == reader.GetValue(1).ToString()).OrderByDescending(po => po.PlantillaOrdering).ElementAt(0).PlantillaOrdering + 1 : 1;

                    PlantillaList.Add(Plantilla);
                }
            }

            return PlantillaList;
        }

        public IEnumerable<PlantillaModel> grGetVacantPositions()
        {

            List<PlantillaModel> PlantillaList = new List<PlantillaModel>();
            OfficeAdmin_Layer OfficeAdmin_Layer = new OfficeAdmin_Layer();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select positionCode, Pos_name + ' (' + PosShortname + ')'from  pmis.dbo.refsPositions where sg != 0", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    PlantillaModel Plantilla = new PlantillaModel();
                    Plantilla.PositionID = Convert.ToInt32(reader.GetValue(0)); //PositionCode
                    Plantilla.Position = reader.GetValue(1).ToString();
                    PlantillaList.Add(Plantilla);
                }
            }
            return PlantillaList;
        }

        public string UpdateStepIncrement(int SeriesID, int Step)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Insert into tbl_R_BMSProposedStepIncrement VALUES(" + SeriesID + "," + Step + ",NULL,1)", con);
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
        public string reqTransferToChildOffice(int SeriesID, int OfficeID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"insert into tbl_R_BMSTransferToChildOffice values(" + SeriesID + "," + OfficeID + " )", con);
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
        public string reqTransferToMotherOffice(int SeriesID, int OfficeID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"delete from tbl_R_BMSTransferToChildOffice where PlantillaItemNoID = " + SeriesID + " and ChildOfficeID = " + OfficeID + "", con);
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
        public PlantillaModel GetPositionData(int PositionID, int PlantillaYear)
        {
            PlantillaModel PlantillaModel = new PlantillaModel();
            OfficeAdmin_Layer OfficeAdmin_Layer = new OfficeAdmin_Layer();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select positionCode, Pos_name + ' (' + PosShortname + ')', 
                                                      sg,isnull((select pmis.dbo.sclr_fnc_get_salary(sg,1,(select aeffectivity 
                                                      from pmis.dbo.EDGE_tblEffectivityDate where yearof = " + PlantillaYear + "))),0) as Salary from  pmis.dbo.refsPositions where Positioncode = " + PositionID + " ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    PlantillaModel.PositionID = Convert.ToInt32(reader.GetValue(0));
                    PlantillaModel.Position = reader.GetValue(1).ToString();
                    PlantillaModel.sg = reader.GetValue(2).ToString();
                    PlantillaModel.strSalary = "₱" + string.Format(new System.Globalization.CultureInfo("en-US"), "{0:N2}", Convert.ToDouble(reader.GetValue(3)));

                }
            }
            return PlantillaModel;
        }
        public string savePlantillaItem(int PositionID, string AppointmentDate, int EmploymentStatus, int DivisionID, int OfficeID, double HazardPay, double Subsistence,int PlanYear)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Insert into tbl_R_BMSProposedNewItem values(" + PositionID + ",'" + AppointmentDate + "'," + EmploymentStatus + "," + DivisionID + "," + OfficeID + ", 1," + PlanYear + "," + HazardPay + "," + Subsistence + ")", con);
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
        public string RemovePlantillaItem(int PlantillaItemID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Update tbl_R_BMSProposedNewItem set ActionCode = 0 where ProposedItemID = " + PlantillaItemID + " and ActionCode = 1", con);
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
        public PlantillaModel GetSelectedPlantillaItemData(int ProposedItemID, int PlantillaYear)
        {
            PlantillaModel SelectedItemData = new PlantillaModel();
            OfficeAdmin_Layer OfficeAdmin_Layer = new OfficeAdmin_Layer();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select a.ProposedItemID,c.PositionCode,c.Pos_name, 
                                                b.DivID, a.EmploymentStatusID ,a.AppointmentDateEffectivity , 
                                                isnull(pmis.dbo.sclr_fnc_get_salary(c.sg,1,(select aeffectivity 
                                                from pmis.dbo.EDGE_tblEffectivityDate where yearof = " + PlantillaYear + " )),0) " + ""
                                                +", c.sg,a.HazardPay, a.SubsistenceAndLaundry from tbl_R_BMSProposedNewItem as a  "+""
                                                +"inner join pmis.dbo.EDGE_tblPlantillaDivision as b on "+""
                                                +"b.DivID = a.DivisionID inner JOIN pmis.dbo.RefsPositions as c "+""
                                                +"on c.PositionCOde = a.PositionID where a.ProposedItemID = " + ProposedItemID + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    SelectedItemData.SeriesID = reader.GetInt32(0);
                    SelectedItemData.PositionID = reader.GetInt32(1);
                    SelectedItemData.Position = reader.GetValue(2).ToString();
                    SelectedItemData.DivID = reader.GetInt32(3);
                    SelectedItemData.EmploymentStatusID = reader.GetInt32(4);
                    SelectedItemData.AppointmentDate = reader.GetDateTime(5).ToString("MM/dd/yyyy");
                    SelectedItemData.strSalary = "₱" + string.Format(new System.Globalization.CultureInfo("en-US"), "{0:N2}", Convert.ToDouble(reader.GetValue(6)));
                    SelectedItemData.sg = reader.GetValue(7).ToString();
                    SelectedItemData.HazardPay = Convert.ToDouble(reader.GetValue(8));
                    SelectedItemData.Subsistence = Convert.ToDouble(reader.GetValue(9).ToString());
                }
            }
            return SelectedItemData;
        }
        public PlantillaModel GetSelectedCasualData(int ProposedItemID)
        {
            PlantillaModel SelectedItemData = new PlantillaModel();
            OfficeAdmin_Layer OfficeAdmin_Layer = new OfficeAdmin_Layer();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select a.ProposedItemID, a.AppointmentDateEffectivity,b.CasualRateID,
                                                b.CasualRate * 22 as 'MonthlySalary',b.CasualRate,a.HazardPay,a.SubsistenceAndLaundry
                                                from tbl_R_BMSProposedNewItem as a 
                                                inner JOIN tbl_R_BMSCasualRate as b on b.CasualRateID = a.EmploymentStatusID
                                                where proposedItemID = " + ProposedItemID + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    SelectedItemData.SeriesID = reader.GetInt32(0);
                    SelectedItemData.AppointmentDate = reader.GetDateTime(1).ToString("MM/dd/yyyy");
                    SelectedItemData.EmploymentStatusID = reader.GetInt32(2);
                    SelectedItemData.strSalary = "₱" + string.Format(new System.Globalization.CultureInfo("en-US"), "{0:N2}", Convert.ToDouble(reader.GetValue(3)));
                    SelectedItemData.Salary = Convert.ToDouble(reader.GetValue(4));
                    SelectedItemData.HazardPay = Convert.ToDouble(reader.GetValue(5));
                    SelectedItemData.Subsistence = Convert.ToDouble(reader.GetValue(6));
                }
            }
            return SelectedItemData;
        }
        public string UpdatePlantillaItem(int ProposedItemID, int PositionID, string AppointmentDate, int EmploymentStatus, int DivisionID, double HazardPay, double Subsistence, int PlanYear)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Update tbl_R_BMSProposedNewItem set PositionID = " + PositionID + " ,AppointmentDateEffectivity = '" + AppointmentDate
                                                + "', EmploymentStatusID  = " + EmploymentStatus + ", DivisionID = " + DivisionID + ",HazardPay=" + HazardPay + ",SubsistenceAndLaundry=" + Subsistence + " where ProposedItemID = " + ProposedItemID + " and YearOf ="+ PlanYear + "", con);

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
        public string UpdateCasualItem(int ProposedItemID, string AppointmentDate, int EmploymentStatus,double HazardPay, double Subsistence)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Update tbl_R_BMSProposedNewItem set AppointmentDateEffectivity = '" + AppointmentDate
                                                + "', EmploymentStatusID  = " + EmploymentStatus + ", HazardPay="+HazardPay+",SubsistenceAndLaundry=" + Subsistence + " where ProposedItemID = " + ProposedItemID + "", con);

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
        public IEnumerable<DropDownListModel> ddlHazardComputation()
        {
            List<DropDownListModel> List = new List<DropDownListModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("Select SeriesID,Description from tbl_R_BMSHazardComputation where ActionCode = 1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    DropDownListModel Item = new DropDownListModel();
                    Item.Value = Convert.ToInt32(reader.GetValue(0));
                    Item.Text = reader.GetValue(1).ToString();
                    List.Add(Item);
                }
            }
            return List;
        }
        public PlantillaModel GetSelectedCasualRate(int CasualRateID)
        {
            PlantillaModel PlantillaModel = new PlantillaModel();
            OfficeAdmin_Layer OfficeAdmin_Layer = new OfficeAdmin_Layer();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select CasualRate,CasualRate * 22 FROM tbl_R_BMSCasualRate where CasualRateID = " + CasualRateID + " and ActionCode = 1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    PlantillaModel.Salary = Convert.ToDouble(reader.GetValue(0));
                    PlantillaModel.strSalary = "₱" + string.Format(new System.Globalization.CultureInfo("en-US"), "{0:N2}", Convert.ToDouble(reader.GetValue(1)));

                }
            }
            return PlantillaModel;
        }
        public string SaveNewCasual(int EmploymentStatus, string AppointmentDate, int OfficeID, double HazardPay, double Subsistence, int Quantity, int PlantillaYear)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    for (int i = 1; i <= Quantity; i++)
                    {
                        SqlCommand com = new SqlCommand(@"insert into tbl_R_BMSProposedNewItem values(NULL,'" + AppointmentDate + "'," + EmploymentStatus + ",NULL," + OfficeID + ",'1'," + PlantillaYear + " + 1," + HazardPay + ", " + Subsistence + ")", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                        
                    }
                    return "1";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string reqRemoveCasual(int SeriesID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                        SqlCommand com = new SqlCommand(@"delete from tbl_R_BMSVacantAndTransferedCasual where SeriesID = " + SeriesID + "", con);
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
        public string SaveVacantCasual(int EmploymentStatus, string AppointmentDate, int OfficeID, int Quantity,int PlanYear)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    for (int i = 1; i <= Quantity; i++)
                    { //Update on 3/27/2019 -Used of parameter:plan year
                        SqlCommand com = new SqlCommand(@"insert into tbl_R_BMSVacantAndTransferedCasual([Eid],[EmploymentStatusID],[SalaryEffectivityDate],[ActionCode],[Yearof],[OfficeID],[pos_id],[isProposed],[SeriesID_aip]) values(0,'" + EmploymentStatus + "','" + AppointmentDate + "',1," + PlanYear + " + 1,'" + OfficeID + "',0,1,0)", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();

                    }
                    return "1";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string UpdateCasualVacant(int SeriesID, string AppointmentDate, int CasualRateID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Update tbl_R_BMSVacantAndTransferedCasual set EmploymentStatusID = " + CasualRateID
                                                    + ", SalaryEffectivityDate = '" + AppointmentDate + "' where SeriesID = " + SeriesID + "", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();

                    return "1";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string CheckWithHazardPay(int OfficeID,int PlanYear)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"SELECT ISNULL((select isnull(HazardFormulaUsed,0) 
                                                    FROM tbl_R_BMSOfficesWithHazardAndSubsistence 
                                                    WHERE ActionCode = 1 and IFMISOfficeID = " + OfficeID + " and hasHazard = 1),0)", con);
                        con.Open();
                        return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        
        public string CheckHazardAndSubsistence(int SeriesID, int OfficeID, int isForFunding)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {                                                   //Function Parameters OfficeID, PlantillaItemNo, BudgetYear
                    SqlCommand com = new SqlCommand(@"select dbo.fn_BMS_CheckSubsistenceAndHazardPerPlantillaItem(" + OfficeID + "," + SeriesID + "," + (DateTime.Now.Year + 1) + "," + isForFunding + "," + Account.UserInfo.UserTypeID + ")", con);
                    con.Open();
                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string ReqExcludeOnHazard(int PlantillaItemNo, int isForFunding)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Insert into tbl_R_BMSExcludedOnHazard (PlantillaItemNo,ActionCode,YearOf,isForFunding) 
                                                    values(" + PlantillaItemNo + ",1," + (DateTime.Now.Year + 1) + "," + isForFunding + ")", con);
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
        public string ReqExcludeOnSubsistence(int PlantillaItemNo, int isForFunding)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Insert into tbl_R_BMSExcludedOnSubsistence (PlantillaItemNo,ActionCode,YearOf,isForFunding) 
                                                    values(" + PlantillaItemNo + ",1," + (DateTime.Now.Year + 1) + "," + isForFunding + ")", con);
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
        public string ReqIncludeOnSubsistence(int PlantillaItemNo, int isForFunding)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"delete from tbl_R_BMSExcludedOnSubsistence where PlantillaItemNo = " + PlantillaItemNo + " and ActionCode = 1 and YearOf = " + (DateTime.Now.Year + 1) + " and isForFunding = " + isForFunding + "", con);
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
        public string reqUpdateHazardFormulaUsed(int SeriesID, int OfficeID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Update tbl_R_BMSOfficesWithHazardAndSubsistence set HazardFormulaUsed = " + SeriesID 
                                                    + "  where ActionCode = 1 and IFMISOfficeID = " + OfficeID + " and hasHazard = 1", con);
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
        public string ReqIncludeOnHazard(int PlantillaItemNo, int isForFunding)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"delete from tbl_R_BMSExcludedOnHazard where PlantillaItemNo = " + PlantillaItemNo + " and ActionCode = 1 and YearOf = " + (DateTime.Now.Year + 1) + " and isForFunding = " + isForFunding + "", con);
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