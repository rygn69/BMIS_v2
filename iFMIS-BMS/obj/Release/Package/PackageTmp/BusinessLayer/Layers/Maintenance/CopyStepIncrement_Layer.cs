using iFMIS_BMS.BusinessLayer.Connector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers.Maintenance
{
    public class CopyStepIncrement_Layer
    {
        public string CopyStepIncrement()
        {
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                //SqlCommand query_truncate = new SqlCommand(@"TRUNCATE TABLE ifmis.dbo.tbl_R_BMSStepIncrement", con);
               
               // query_truncate.ExecuteNonQuery();
                SqlCommand query_copyStepIncrement = new SqlCommand(@"Insert into dbo.tbl_R_BMSStepIncrement SELECT * , GETDATE() , 1 FROM  pmis.dbo.vw_RGPermanentAndCasual", con);
                con.Open();
                query_copyStepIncrement.ExecuteNonQuery();
            }
            
            return "success";
        }
    }
}