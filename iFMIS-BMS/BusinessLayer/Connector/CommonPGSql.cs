using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace iFMIS_BMS.BusinessLayer.Connector
{
    //public class CommonPGSql
    //{
    //}
    using System.Configuration;

    public static class CommonPGSql
    {
        // This gets your PostgreSQL connection string from Web.config or App.config
        public static string MyPostgreSqlConn()
        {
            // Make sure you have the name "PostgreSqlConn" in your config file
            return ConfigurationManager.ConnectionStrings["ePS"].ConnectionString;
        }
    }
}