using iFMIS_BMS.BusinessLayer.Connector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.Classes
{
    public class AdditionalRulesCaseStatement
    {
        Int32 CanReviewPS, CanReviewMOOE, CanReviewCO, CanReviewFE;
        string caseStatement;
        public string CaseStatement()
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
                {
                    SqlCommand query_time = new SqlCommand(@"Select CanReviewPS, CanReviewMOOE, CanReviewCO, CanReviewFE from tbl_R_BMSUserTypes where UserTypeID=3", con);
                    con.Open();
                    SqlDataReader reader = query_time.ExecuteReader();
                    while (reader.Read())
                    {
                        CanReviewPS = reader.GetInt16(0);
                        CanReviewMOOE = reader.GetInt16(1);
                        CanReviewCO = reader.GetInt16(2);
                        CanReviewFE = reader.GetInt16(3);
                    }
                    #region Logic for HRMO Approval dynamic
                    if (CanReviewPS == 1 && CanReviewMOOE == 1 && CanReviewCO == 1 && CanReviewFE == 1)
                    {
                        caseStatement = "2";
                    }
                    else if (CanReviewPS == 1 && CanReviewMOOE == 0 && CanReviewCO == 0 && CanReviewFE == 0)
                    {
                        caseStatement = "(Case When (f.OOEAbrevation = 'PS') then 2 else 1 end)";
                    }
                    else if (CanReviewPS == 1 && CanReviewMOOE == 1 && CanReviewCO == 0 && CanReviewFE == 0)
                    {
                        caseStatement = "(Case When (f.OOEAbrevation = 'PS' OR f.OOEAbrevation = 'MOOE' ) then 2 else 1 end)";
                    }
                    else if (CanReviewPS == 1 && CanReviewMOOE == 1 && CanReviewCO == 1 && CanReviewFE == 0)
                    {
                        caseStatement = "(Case When (f.OOEAbrevation = 'FE') then 1 else 2 end)";
                    }
                    else if (CanReviewPS == 0 && CanReviewMOOE == 0 && CanReviewCO == 0 && CanReviewFE == 0)
                    {
                        caseStatement = "1";
                    }
                    else if (CanReviewPS == 0 && CanReviewMOOE == 1 && CanReviewCO == 1 && CanReviewFE == 1)
                    {
                        caseStatement = "(Case When (f.OOEAbrevation = 'PS') then 1 else 2 end)";
                    }
                    else if (CanReviewPS == 0 && CanReviewMOOE == 0 && CanReviewCO == 1 && CanReviewFE == 1)
                    {
                        caseStatement = "(Case When (f.OOEAbrevation = 'CO' OR f.OOEAbrevation = 'FE') then 2 else 1 end)";
                    }
                    else if (CanReviewPS == 0 && CanReviewMOOE == 0 && CanReviewCO == 0 && CanReviewFE == 1)
                    {
                        caseStatement = "(Case When (f.OOEAbrevation = 'FE') then 2 else 1 end)";
                    }
                    else if (CanReviewPS == 1 && CanReviewMOOE == 0 && CanReviewCO == 1 && CanReviewFE == 1)
                    {
                        caseStatement = "(Case When (f.OOEAbrevation = 'MOOE') then 1 else 2 end)";
                    }
                    else if (CanReviewPS == 1 && CanReviewMOOE == 0 && CanReviewCO == 0 && CanReviewFE == 1)
                    {
                        caseStatement = "(Case When (f.OOEAbrevation = 'PS' OR f.OOEAbrevation = 'FE') then 2 else 1 end)";
                    }
                    else if (CanReviewPS == 1 && CanReviewMOOE == 1 && CanReviewCO == 0 && CanReviewFE == 1)
                    {
                        caseStatement = "(Case When (f.OOEAbrevation = 'CO') then 1 else 2 end)";
                    }
                    else if (CanReviewPS == 1 && CanReviewMOOE == 0 && CanReviewCO == 1 && CanReviewFE == 0)
                    {
                        caseStatement = "(Case When (f.OOEAbrevation = 'PS' OR f.OOEAbrevation = 'CO') then 2 else 1 end)";
                    }
                    else if (CanReviewPS == 0 && CanReviewMOOE == 1 && CanReviewCO == 0 && CanReviewFE == 0)
                    {
                        caseStatement = "(Case When (f.OOEAbrevation = 'MOOE') then 2 else 1 end)";
                    }
                    else if (CanReviewPS == 0 && CanReviewMOOE == 1 && CanReviewCO == 1 && CanReviewFE == 0)
                    {
                        caseStatement = "(Case When (f.OOEAbrevation = 'MOOE' OR f.OOEAbrevation = 'CO') then 2 else 1 end)";
                    }
                    else if (CanReviewPS == 0 && CanReviewMOOE == 1 && CanReviewCO == 0 && CanReviewFE == 1)
                    {
                        caseStatement = "(Case When (f.OOEAbrevation = 'MOOE' OR f.OOEAbrevation = 'FE') then 2 else 1 end)";
                    }
                    else if (CanReviewPS == 0 && CanReviewMOOE == 0 && CanReviewCO == 1 && CanReviewFE == 0)
                    {
                        caseStatement = "(Case When (f.OOEAbrevation = 'CO') then 2 else 1 end)";
                    } 
                    #endregion
                }
            return caseStatement;
        }
    }
}