using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.Base
{
    class dbConnect
    {
        OleDbConnection _OleDbConn = new OleDbConnection();

        //public string _strDataSource, _strDatabaseName, _strUsername, _strPassword;


        //public string strDataSource()
        //{
        //    return ".";
        //}
        //public string strDatabaseName()
        //{
        //    return "pmis";
        //}

        //public string strUsername()
        //{
        //    return "sa";
        //}
        //public string strPassword()
        //{
        //    return "flamex";
        //}

        



        public string strConnSetting()
        {

            //_strDataSource = ".";
            //_strDatabaseName = "pmis";
            //_strUsername ="sa";
            //_strPassword = "flamex";

            //string ConnectionString = "Provider=SQLOLEDB.1;" +
            //                            "Data Source=" + _strDataSource + ";" +
            //                            "Initial Catalog=" + _strDatabaseName + ";" +
            //                            "User ID=" + _strUsername + ";" +
            //                            "Password=" + _strPassword + ";";

            string ConnectionString = ConfigurationManager.ConnectionStrings["adodb"].ToString();

            return ConnectionString;

        }


        public bool dbConnection()
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["adodb"].ToString();
               // _OleDbConn.ConnectionString = "Provider=SQLOLEDB.1;" +
                                              //"Data Source=" + _strDataSource + ";" +
                                              //"Initial Catalog=" + _strDatabaseName + ";" +
                                              //"User ID=" + _strUsername + ";" +
                                              //"Password=" + _strPassword + ";";

                _OleDbConn.ConnectionString = ConnectionString;



                _OleDbConn.Open();

                return true;
            }

            catch (Exception e)
            {
                
                return false;
            }
        }


       




    }

   
}