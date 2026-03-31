using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Mvc;
using Npgsql;
using System.Data.SqlClient;
using System.Data;


namespace eams.Base
{

    public class clsDBConnect: Controller
    {
        static DbProviderFactory factory = DbProviderFactories.GetFactory("Npgsql");
        NpgsqlConnection dbconn = new NpgsqlConnection();//.CreateConnection();

        public bool dbConnection()
        {
            try
            {
                dbconn.ConnectionString = ConfigurationManager.ConnectionStrings["ePS"].ConnectionString; //"Server=localhost;Port=5432;User Id=postgres;Password=pgas@1;Database=test;";
                dbconn.Open();

                return true;
            }
            catch (Exception e)
            {
                    
                
                System.Web.HttpContext.Current.Response.Write("Failed to connect: " + e.Message);
                return false;
            }
        }

        public DataTable execQuery(string strquery)
        {
            DataTable dt = new DataTable();
            dbConnection();
            NpgsqlCommand command = new NpgsqlCommand(strquery, dbconn);
            command.CommandTimeout = 6000;
            try
            {
                NpgsqlDataReader rd = command.ExecuteReader();
                dt.Load(rd);
                rd.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                dbconn.Close();
            }

            return dt;
        }


        public void execNonQuery(string strquery)
        {
            DataTable dt = new DataTable();
            dbConnection();
            NpgsqlCommand command = new NpgsqlCommand(strquery, dbconn);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                dbconn.Close();
            }

        }

        public object execScalar(string strquery)
        {
            object obj = new object();
            dbConnection();

            NpgsqlCommand command = new NpgsqlCommand(strquery, dbconn);

            try
            {
                obj = command.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                dbconn.Close();
            }
            return obj;
        }



    }

}
