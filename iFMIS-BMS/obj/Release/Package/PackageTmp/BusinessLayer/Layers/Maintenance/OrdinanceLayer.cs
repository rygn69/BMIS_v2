using iFMIS_BMS.BusinessLayer.Connector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using iFMIS_BMS.BusinessLayer.Models.Maintenance;
using iFMIS_BMS.BusinessLayer.Models;
using System.Xml;

namespace iFMIS_BMS.BusinessLayer.Layers.Maintenance
{
    public class OrdinanceLayer
    {
        public string UpdateAuthor(int eid)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"if EXISTS (select SeriesID from tbl_R_BMSOrdinanceAuthor where yearof = Year(GETDATE())and ActionCode = 1)
                                                BEGIN
                                                Update tbl_R_BMSOrdinanceAuthor set eid ='" + eid + "' where yearof = Year(GETDATE())and ActionCode = 1 "+""
                                                +" END "+""
                                                +" ELSE "+""
                                                +" BEGIN "+""
                                                +" INSERT into tbl_R_BMSOrdinanceAuthor VALUES('" + eid + "',Year(GETDATE()),1) "+""
                                                +" END ", con);
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
        public string SaveSection(string SectionDescription, int SectionOrder, string SectionName)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"insert into tbl_R_BMSOrdinanceSection VALUES('" + SectionName.Replace("'", "''") + "','" + SectionDescription.Replace("'", "''") + "',YEAR(GETDATE()),1,'" + SectionOrder + "')", con);
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
        public string UpdateSection(string SectionDescription, int SectionOrder, string SectionName, int SectionID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Update tbl_R_BMSOrdinanceSection set SectionName = '" + SectionName.Replace("'", "''") + "', SectionDescription ='" + SectionDescription.Replace("'", "''") + "',SectionOrder ='" + SectionOrder + "' where SectionID = " + SectionID + "", con);
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
        
        public string SaveSpecialProvision(int OfficeID, int SpecialProvisionNo, string SpecialProvision)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"insert into tbl_R_BMSOrdinanceSpecialProvision VALUES('" + OfficeID + "','" + SpecialProvision.Replace("'", "''") + "','" + SpecialProvisionNo + "',YEAR(GETDATE()),1)", con);
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
        public string SaveSpecialProvisionMain(int OfficeID, int SpecialProvisionNo, string SpecialProvision)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"insert into tbl_R_BMSOrdinanceMainOfficeSpecialProvision VALUES('" + OfficeID + "','" + SpecialProvision.Replace("'", "''") + "','" + SpecialProvisionNo + "',YEAR(GETDATE()),1)", con);
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
        public string UpdateSpecialProvision(int OfficeID, int SpecialProvisionNo, string SpecialProvision, int SeriesID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Update tbl_R_BMSOrdinanceSpecialProvision Set SpecialProvisionDescription='" + SpecialProvision.Replace("'", "''") + "',SpecialProvisionOrder='" + SpecialProvisionNo + "'where SpecialProvisionID = " + SeriesID + "", con);
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
        public string UpdateSpecialProvisionMain(int OfficeID, int SpecialProvisionNo, string SpecialProvision, int SeriesID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Update tbl_R_BMSOrdinanceMainOfficeSpecialProvision Set SpecialProvisionDescription='" + SpecialProvision.Replace("'", "''") + "',SpecialProvisionOrder='" + SpecialProvisionNo + "'where MainOfficeSpecialProvisionID = " + SeriesID + "", con);
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
        
        public string UpdateOfficeDescription(int OfficeID, string OfficeDescription)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"IF EXISTS (SELECT OfficeID from tbl_R_BMSOrdinanceOfficeDescription where OfficeID = "+ OfficeID +" and YearOf = year(GETDATE()) and ActionCode = 1) "+""
                                                +" BEGIN "+""
                                                +" UPDATE tbl_R_BMSOrdinanceOfficeDescription set OfficeDescription = '" + OfficeDescription.Replace("'", "''") + "' where OfficeID = "+ OfficeID +" and YearOf = year(GETDATE()) "+""
                                                +" END "+""
                                                +" ELSE "+""
                                                +" BEGIN "+""
                                                +" INSERT INTO tbl_R_BMSOrdinanceOfficeDescription VALUES(" + OfficeID + ",'" + OfficeDescription.Replace("'", "''") + "',year(GETDATE()),1) "+""
                                                +" END "+""
                                                +"", con);
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
        public string RemoveSection(int SectionID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Update tbl_R_BMSOrdinanceSection set ActionCode = 0 where SectionID = " + SectionID + " ", con);
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
        public string RemoveSpecialProvision(int SeriesID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Update tbl_R_BMSOrdinanceSpecialProvision set ActionCode = 0 where SpecialProvisionID = " + SeriesID + " ", con);
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
        public string RemoveSpecialProvisionMain(int SeriesID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Update tbl_R_BMSOrdinanceMainOfficeSpecialProvision set ActionCode = 0 where MainOfficeSpecialProvisionID = " + SeriesID + " ", con);
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
        
        public string RemoveOfficeDescription(int DescriptionID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Update tbl_R_BMSOrdinanceOfficeDescription set ActionCode = 0 where OfficeDescriptionID = " + DescriptionID + " ", con);
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
        public string getDescriptionSelectedOffice(int OfficeID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT OfficeDescription from tbl_R_BMSOrdinanceOfficeDescription where OfficeID = " + OfficeID + " and YearOf = year(GETDATE()) and ActionCode = 1", con);
                try
                {
                    con.Open();
                    return Convert.ToString(com.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public int GetSpecialProvisionOrderNo(int OfficeID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Select Top 1 SpecialProvisionOrder + 1 from tbl_R_BMSOrdinanceSpecialProvision where ActionCode = 1 and YearOf = Year(GetDate()) and OfficeID = " + OfficeID + " order by SpecialProvisionOrder desc", con);
                try
                {
                    con.Open();
                    var Result = Convert.ToInt32(com.ExecuteScalar());
                    if (Result == 0)
                    {
                        return 1;
                    }
                    else
	                {
                        return Result;
	                }
                }
                catch (Exception)
                {
                    return 1;
                }
            }
        }
        public int GetOrderNoSpecialProvisionMain(int OfficeID)
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Select Top 1 SpecialProvisionOrder + 1 from tbl_R_BMSOrdinanceMainOfficeSpecialProvision where ActionCode = 1 and YearOf = Year(GetDate()) and OfficeID = " + OfficeID + " order by SpecialProvisionOrder desc", con);
                try
                {
                    con.Open();
                    var Result = Convert.ToInt32(com.ExecuteScalar());
                    if (Result == 0)
                    {
                        return 1;
                    }
                    else
	                {
                        return Result;
	                }
                }
                catch (Exception)
                {
                    return 1;
                }
            }
        }
        
        
        public string getOrdinanceAuthor()
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"SELECT eid FROM tbl_R_BMSOrdinanceAuthor where Yearof = year(Getdate()) and ActionCode = 1", con);
                try
                {
                    con.Open();
                    return Convert.ToString(com.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public int getSectionOrder()
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"Select top 1 SectionOrder + 1 from tbl_R_BMSOrdinanceSection where ActionCode = 1 and YearOf = year(getdate()) order by sectionOrder desc", con);
                try
                {
                    con.Open();
                    var Result = Convert.ToInt32(com.ExecuteScalar());
                    if (Result == 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return Result;
                    }
                }
                catch (Exception)
                {
                    return 1;
                }
            }
        }
        
        
        
        public OrdinanceSectionModel getSelectedSectionForEdit(int SectionID)
        {
            OrdinanceSectionModel Section = new OrdinanceSectionModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select SectionID,SectionOrder,SectionDescription,SectionName from tbl_R_BMSOrdinanceSection where SectionID =" + SectionID + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Section.SectionID = Convert.ToInt64(reader.GetValue(0));
                    Section.SectionOrder = Convert.ToInt32(reader.GetValue(1));
                    Section.SectionDescription = reader.GetValue(2).ToString();
                    Section.SectionName = reader.GetValue(3).ToString();
                }
            }
            return Section;
        }
        public OrdinanceSpecialProvisionsModel getSelectedSpecialProvisionForEdit(int SeriesID)
        {
            OrdinanceSpecialProvisionsModel SpecialProvision = new OrdinanceSpecialProvisionsModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select SpecialProvisionID,SpecialProvisionOrder,OfficeID,SpecialProvisionDescription from tbl_R_BMSOrdinanceSpecialProvision where SpecialProvisionID = '" + SeriesID + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    SpecialProvision.SeriesID = Convert.ToInt64(reader.GetValue(0));
                    SpecialProvision.OrderNo = reader.GetValue(1).ToString();
                    SpecialProvision.OfficeName = reader.GetValue(2).ToString();
                    SpecialProvision.Description = reader.GetValue(3).ToString();
                }
            }
            return SpecialProvision;
        }
        public OrdinanceSpecialProvisionsModel getSelectedSpecialProvisionMainForEdit(int SeriesID)
        {
            OrdinanceSpecialProvisionsModel SpecialProvision = new OrdinanceSpecialProvisionsModel();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select MainOfficeSpecialProvisionID,SpecialProvisionOrder,OfficeID,SpecialProvisionDescription 
                                                from tbl_R_BMSOrdinanceMainOfficeSpecialProvision where MainOfficeSpecialProvisionID = " + SeriesID + "", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    SpecialProvision.SeriesID = Convert.ToInt64(reader.GetValue(0));
                    SpecialProvision.OrderNo = reader.GetValue(1).ToString();
                    SpecialProvision.OfficeName = reader.GetValue(2).ToString();
                    SpecialProvision.Description = reader.GetValue(3).ToString();
                }
            }
            return SpecialProvision;
        }
        
        
        
        public IEnumerable<OrdinanceSectionModel> ReadSectionsList()
        {

            List<OrdinanceSectionModel> SectionList = new List<OrdinanceSectionModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select SectionID,SectionOrder,SectionDescription from tbl_R_BMSOrdinanceSection where ActionCode = 1 and YearOf = year(GETDATE()) ORDER BY SectionOrder, SectionID", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    OrdinanceSectionModel Section = new OrdinanceSectionModel();
                    Section.SectionID = Convert.ToInt64(reader.GetValue(0));
                    Section.SectionOrder = Convert.ToInt32(reader.GetValue(1));
                    Section.SectionDescription = reader.GetValue(2).ToString();
                    SectionList.Add(Section);
                }
            }
            return SectionList;
        }

        public IEnumerable<OrdinanceSpecialProvisionsModel> ReadOrdinanceSpecialProvisions()
        {

            List<OrdinanceSpecialProvisionsModel> SpecialProvisionList = new List<OrdinanceSpecialProvisionsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select DISTINCT  isnull(d.SpecialProvisionID,0),d.SpecialProvisionOrder,c.OfficeName,d.SpecialProvisionDescription,e.OrderNo
                                                  from tbl_T_BMSBudgetProposal as a
                                                  INNER JOIN tbl_R_BMSOfficePrograms as b on b.ProgramID = a.ProgramID 
                                                  and a.ProposalActionCode = b.ActionCode and a.ProposalYear = b.ProgramYear
                                                  INNER JOIN tbl_R_BMSOffices as c on c.OfficeID = b.OfficeID
                                                  FULL OUTER JOIN tbl_R_BMSOrdinanceSpecialProvision as d on d.OfficeID = c.OfficeID and d.YearOf = year(GETDATE()) and d.ActionCode = 1
												  INNER JOIN tbl_R_BMSOrdinanceOfficeOrdering as e on e.OfficeID = c.OfficeID
                                                  where a.ProposalYear = year(GETDATE()) + 1 and a.ProposalActionCode = 1 ORDER BY e.OrderNo, c.OfficeName,d.SpecialProvisionOrder", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    OrdinanceSpecialProvisionsModel SpecialProvision = new OrdinanceSpecialProvisionsModel();
                    SpecialProvision.SeriesID = Convert.ToInt64(reader.GetValue(0));
                    SpecialProvision.OrderNo = reader.GetValue(1).ToString();
                    SpecialProvision.OfficeName = reader.GetValue(2).ToString();
                    SpecialProvision.Description = reader.GetValue(3).ToString();
                    SpecialProvision.OfficeOrder = Convert.ToInt32(reader.GetValue(4));
                    SpecialProvisionList.Add(SpecialProvision);
                }
            }
            return SpecialProvisionList;
        }
        public IEnumerable<OrdinanceSpecialProvisionsModel> ReadOrdinanceSpecialProvisionsMainOffice()
        {

            List<OrdinanceSpecialProvisionsModel> SpecialProvisionList = new List<OrdinanceSpecialProvisionsModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select DISTINCT  isnull(d.MainOfficeSpecialProvisionID,0),d.SpecialProvisionOrder,c.OfficeName,d.SpecialProvisionDescription
                                                  from tbl_T_BMSBudgetProposal as a
                                                  INNER JOIN tbl_R_BMSOfficePrograms as b on b.ProgramID = a.ProgramID 
                                                  and a.ProposalActionCode = b.ActionCode and a.ProposalYear = b.ProgramYear
                                                  INNER JOIN tbl_R_BMSOffices as c on c.OfficeID = b.OfficeID
                                                  FULL OUTER JOIN tbl_R_BMSOrdinanceMainOfficeSpecialProvision as d on d.OfficeID = c.OfficeID and d.YearOf = year(GETDATE()) and d.ActionCode = 1
                                                  where a.ProposalYear = year(GETDATE()) + 1 and a.ProposalActionCode = 1 and c.OfficeID in(select MainOfficeID from tbl_R_BMSOrdinanceOfficeGrouping) ORDER BY c.OfficeName,d.SpecialProvisionOrder", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    OrdinanceSpecialProvisionsModel SpecialProvision = new OrdinanceSpecialProvisionsModel();
                    SpecialProvision.SeriesID = Convert.ToInt64(reader.GetValue(0));
                    SpecialProvision.OrderNo = reader.GetValue(1).ToString();
                    SpecialProvision.OfficeName = reader.GetValue(2).ToString();
                    SpecialProvision.Description = reader.GetValue(3).ToString();
                    SpecialProvisionList.Add(SpecialProvision);
                }
            }
            return SpecialProvisionList;
        }
        public IEnumerable<OrdinanceDescriptionModel> ReadOrdinanceDescriptions()
        {

            List<OrdinanceDescriptionModel> DescriptionList = new List<OrdinanceDescriptionModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select  isnull(d.OfficeDescriptionID,0),c.OfficeName,d.OfficeDescription,
                                                isnull(e.OrderNo,(SELECT top 1 OrderNo + 1 from tbl_R_BMSOrdinanceOfficeOrdering ORDER BY OrderNo desc)) as 'Office Order'
                                                ,c.OfficeID
                                                from tbl_T_BMSBudgetProposal as a
                                                INNER JOIN tbl_R_BMSOfficePrograms as b on b.ProgramID = a.ProgramID 
                                                and a.ProposalActionCode = b.ActionCode and a.ProposalYear = b.ProgramYear
                                                INNER JOIN tbl_R_BMSOffices as c on c.OfficeID = b.OfficeID
                                                FULL OUTER JOIN tbl_R_BMSOrdinanceOfficeDescription as d on d.OfficeID = c.OfficeID and d.YearOf = 
                                                year(GETDATE()) and d.ActionCode = 1
                                                FULL OUTER JOIN tbl_R_BMSOrdinanceOfficeOrdering as e on e.OfficeID = c.OfficeID	
                                                where a.ProposalYear = year(GETDATE()) + 1 and a.ProposalActionCode = 1 
                                                GROUP BY isnull(d.OfficeDescriptionID,0),c.OfficeName,d.OfficeDescription,e.OrderNo,c.OfficeID
                                                ORDER BY isnull(e.OrderNo,(SELECT top 1 OrderNo + 1 from tbl_R_BMSOrdinanceOfficeOrdering ORDER BY OrderNo desc))
                                                ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    OrdinanceDescriptionModel Description = new OrdinanceDescriptionModel();
                    Description.DescriptionID = Convert.ToInt32(reader.GetValue(0));
                    Description.OfficeName= reader.GetValue(1).ToString();
                    Description.OfficeDescription = reader.GetValue(2).ToString();
                    Description.OfficeOrder = Convert.ToInt32(reader.GetValue(3));
                    Description.OfficeID = Convert.ToInt32(reader.GetValue(4));
                    DescriptionList.Add(Description);
                }
            }
            return DescriptionList;
        }
        public IEnumerable<OfficesModel> ReadOfficeGrouping()
        {

            List<OfficesModel> OfficeGroupingList = new List<OfficesModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select a.OfficeGroupingID, b.OfficeName as 'Main Office', c.OfficeName as  'Sub Office'
                                                from tbl_R_BMSOrdinanceOfficeGrouping as a 
                                                INNER JOIN tbl_R_BMSOffices as b on a.MainOfficeID = b.OfficeID
                                                INNER JOIN tbl_R_BMSOffices as c on c.OfficeID = a.SubOfficeID
                                                ORDER BY b.OfficeName, c.OfficeName", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    OfficesModel Office = new OfficesModel();
                    Office.OfficeID = reader.GetValue(0).ToString();
                    Office.OfficeName = reader.GetValue(1).ToString();
                    Office.SubOffice = reader.GetValue(2).ToString();
                    OfficeGroupingList.Add(Office);
                }
            }
            return OfficeGroupingList;
        }
        public IEnumerable<OrdinanceAttendanceModel> getAttendanceList()
        {

            List<OrdinanceAttendanceModel> AttendanceList = new List<OrdinanceAttendanceModel>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand(@"select isnull(c.SeriesID,0),isnull('Hon. ' + b.Firstname + ' ' + left(b.MI,1) +'. ' + b.Lastname +
                                                case when b.Suffix = '' or b.suffix is null then '' else ', ' + b.suffix end,'None') as 'Name',
                                                c.Designation, a.StatusName, a.OrderNo, isnull(c.OrderNo,0)
                                                from tbl_R_BMSOrdinanceSPStatus as a
                                                LEFT JOIN tbl_R_BMSOrdinanceAttendance  as c on c.Status = a.StatusID and a.ActionCode = c.ActionCode 
                                                and c.YearOf = year(GETDATE())
                                                LEFT JOIN pmis.dbo.employee as b on c.eid = b.eid
                                                ORDER BY a.OrderNo, c.Orderno", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    OrdinanceAttendanceModel Attendance = new OrdinanceAttendanceModel();
                    Attendance.SeriesID = Convert.ToInt32(reader.GetValue(0));
                    Attendance.Name= reader.GetValue(1).ToString();
                    Attendance.Designation = reader.GetValue(2).ToString();
                    Attendance.Status= reader.GetValue(3).ToString();
                    Attendance.GroupOrderNo = Convert.ToInt32(reader.GetValue(4));
                    Attendance.OrderNo = Convert.ToInt32(reader.GetValue(5));

                    AttendanceList.Add(Attendance);
                }
            }
            return AttendanceList;
        }
        
        public string GroupSection(int? FormID, int SectionID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"IF EXISTS (select SeriesID from tbl_R_BMSOrdinanceGrouping where SectionID = " + SectionID + " and ActionCode = 1 and YearOf = year(GETDATE())) " + ""
                                                    +" BEGIN "+""
                                                    +" UPDATE tbl_R_BMSOrdinanceGrouping SET FormType = " + Convert.ToInt32(FormID) + " where SectionID = " + SectionID + " and ActionCode = 1 and YearOf = year(GETDATE()) " + ""
                                                    +" END "+""
                                                    +" ELSE "+""
                                                    +" BEGIN "+""
                                                    +" INSERT INTO tbl_R_BMSOrdinanceGrouping values(" + SectionID + "," + Convert.ToInt32(FormID) + ",1,year(GETDATE())) " + ""
                                                    +" END "+""
                                                    + " Update tbl_R_BMSOrdinanceGrouping set ActionCode = 0 where SeriesID not in(select * from fn_BMS_GetSectionGroupID())", con);
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
        public string RemoveGroup(int OfficeGroupingID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"delete from tbl_R_BMSOrdinanceOfficeGrouping where OfficeGroupingID = " + OfficeGroupingID + "", con);
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
        
        public string GroupOffice(int MainOfficeID, int SubOfficeID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"IF EXISTS (select OfficeGroupingID from tbl_R_BMSOrdinanceOfficeGrouping where MainOfficeiD = " + MainOfficeID + " and SubOfficeID = " + SubOfficeID + ") " + ""
                                                    + " BEGIN " + ""
                                                    + " SELECT 'EXISTING GROUP' " + ""
                                                    + " END " + ""
                                                    + " ELSE " + ""
                                                    + " BEGIN " + ""
                                                    + " INSERT INTO tbl_R_BMSOrdinanceOfficeGrouping values(" + MainOfficeID + "," + SubOfficeID + ") " + ""
                                                    + " Select '1' " +""
                                                    + " END", con);
                    con.Open();
                    return com.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string UpdateOfficeOrder(int OfficeID, int OrderNo)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"IF EXISTS (select OfficeID from tbl_R_BMSOrdinanceOfficeOrdering where OfficeID = " + OfficeID + ") " + ""
                                                    + " BEGIN " + ""
                                                    + " UPDATE tbl_R_BMSOrdinanceOfficeOrdering SET OrderNo = " + OrderNo + " where OfficeID = " + OfficeID + " " + ""
                                                    + " END " + ""
                                                    + " ELSE " + ""
                                                    + " BEGIN " + ""
                                                    + " INSERT INTO tbl_R_BMSOrdinanceOfficeOrdering values(" + OfficeID + "," + OrderNo + ")" + ""
                                                    + " END", con);
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
        public string CheckSubFormType(int SectionID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Select FormType from tbl_R_BMSOrdinanceGrouping where ActionCode = 1 and Yearof = year(getdate()) and SectionID = " + SectionID + "", con);
                    con.Open();
                    var Result = 0;
                    Result = Convert.ToInt32(com.ExecuteScalar());
                    if (Result == 0)
                    {
                        return "No Group";
                    }
                    else
                    {
                        return Result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
                
        }
        public string CheckMainGroup(int FormID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Select SectionID from tbl_R_BMSOrdinanceGrouping where ActionCode = 1 and Yearof = year(getdate()) and FormType = " + FormID + "", con);
                    con.Open();
                    var Result = 0;
                    Result = Convert.ToInt32(com.ExecuteScalar());
                    if (Result == 0)
                    {
                        return "No Group";
                    }
                    else
                    {
                        return Result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public string AddToAttendance(int eid, string Designation,int Status)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Insert into tbl_R_BMSOrdinanceAttendance values(" + eid + ", '" + Designation.Replace("'", "''") + "'," + Status
                        + ",Year(GetDate()),1,(select top 1 OrderNo + 1 from tbl_R_BMSOrdinanceAttendance where ActionCode = 1 and yearof = year(getdate()) order by OrderNo desc ) )", con);
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
        public string RemoveEmployeeAttendance(int SeriesID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Update tbl_R_BMSOrdinanceAttendance Set ActionCode = 0 where SeriesID = " + SeriesID + "", con);
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
        public string UpdateAttendanceHeader(string ReportTitle, string ReportDescription)
        {
            try
            {
                string path = System.Web.HttpContext.Current.Server.MapPath("~/XML/AttendanceHeader.xml");
                using (XmlWriter writer = XmlWriter.Create(path))
                {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("Header");
                        writer.WriteElementString("ReportTitle", ReportTitle);
                        writer.WriteElementString("ReportDescription", ReportDescription);
                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                }
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public List<string> GetAttendanceHeader()
        {
            List<string> Result = new List<string>();
            string path = System.Web.HttpContext.Current.Server.MapPath("~/XML/AttendanceHeader.xml");
            XmlDocument xd = new XmlDocument();
            xd.Load(path);
            XmlNodeList IDListNode = xd.SelectNodes("/Header");
            Result.Add(IDListNode.Item(0).SelectSingleNode("ReportTitle").InnerText);
            Result.Add(IDListNode.Item(0).SelectSingleNode("ReportDescription").InnerText);
            return Result;
        }
        public List<string> GetReportSignatories()
        {
            List<string> Result = new List<string>();
            string path = System.Web.HttpContext.Current.Server.MapPath("~/XML/OrdinanceReportSignatory.xml");
            XmlDocument xd = new XmlDocument();
            xd.Load(path);
            XmlNodeList IDListNode = xd.SelectNodes("Table/Signatory");
            Result.Add(IDListNode.Item(0).SelectSingleNode("SignatoryName").InnerText);
            Result.Add(IDListNode.Item(1).SelectSingleNode("SignatoryName").InnerText);
            Result.Add(IDListNode.Item(2).SelectSingleNode("SignatoryName").InnerText);
            Result.Add(IDListNode.Item(3).SelectSingleNode("SignatoryName").InnerText);
            Result.Add(IDListNode.Item(4).SelectSingleNode("SignatoryName").InnerText);
            Result.Add(IDListNode.Item(5).SelectSingleNode("SignatoryName").InnerText);
            Result.Add(IDListNode.Item(6).SelectSingleNode("SignatoryName").InnerText);
            
            return Result;
        }
        public string UpdateOrdinanceSignatories(string Signatory1Name, string Signatory1Pos, string Signatory2Name,
            string Signatory2Pos, string Signatory2Rule, string Signatory3Name, string Signatory3Pos)
        {
            try
            {
                string path = System.Web.HttpContext.Current.Server.MapPath("~/XML/OrdinanceReportSignatory.xml");
                using (XmlWriter writer = XmlWriter.Create(path))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Table");

                        writer.WriteStartElement("Signatory");
                        writer.WriteElementString("SignatoryID", "1");
                        writer.WriteElementString("SignatoryName", Signatory1Name);
                        writer.WriteEndElement();

                        writer.WriteStartElement("Signatory");
                        writer.WriteElementString("SignatoryID", "2");
                        writer.WriteElementString("SignatoryName", Signatory1Pos);
                        writer.WriteEndElement();

                        writer.WriteStartElement("Signatory");
                        writer.WriteElementString("SignatoryID", "3");
                        writer.WriteElementString("SignatoryName", Signatory2Name);
                        writer.WriteEndElement();

                        writer.WriteStartElement("Signatory");
                        writer.WriteElementString("SignatoryID", "4");
                        writer.WriteElementString("SignatoryName", Signatory2Pos);
                        writer.WriteEndElement();

                        writer.WriteStartElement("Signatory");
                        writer.WriteElementString("SignatoryID", "5");
                        writer.WriteElementString("SignatoryName", Signatory2Rule);
                        writer.WriteEndElement();

                        writer.WriteStartElement("Signatory");
                        writer.WriteElementString("SignatoryID", "6");
                        writer.WriteElementString("SignatoryName", Signatory3Name);
                        writer.WriteEndElement();

                        writer.WriteStartElement("Signatory");
                        writer.WriteElementString("SignatoryID", "7");
                        writer.WriteElementString("SignatoryName", Signatory3Pos);
                        writer.WriteEndElement();

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string UpdateAttendanceOrder(int seriesID, int OrderNo)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand com = new SqlCommand(@"Update tbl_R_BMSOrdinanceAttendance set OrderNo = " + OrderNo + " where SeriesID = " + seriesID + "", con);
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