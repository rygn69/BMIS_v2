using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

namespace iFMIS_BMS.BusinessLayer.Connector
{
    public class Common
    {
        public static string MyConn()
        {
            string path = System.Web.HttpContext.Current.Server.MapPath("~/XML/cString.xml");
            var Constring = "";
            XmlDocument xd = new XmlDocument();
            xd.Load(path);
            XmlNodeList IDListNode = xd.SelectNodes("/Table/constring");

            foreach (XmlNode item in IDListNode)
            {
                Constring = item.SelectSingleNode("ConName").InnerText;
            }
            return Constring;
        }
        public static string MyConn2()
        {
            string path = System.Web.HttpContext.Current.Server.MapPath("~/XML/cString2.xml");
            var Constring = "";
            XmlDocument xd = new XmlDocument();
            xd.Load(path);
            XmlNodeList IDListNode = xd.SelectNodes("/Table/constring");

            foreach (XmlNode item in IDListNode)
            {
                Constring = item.SelectSingleNode("ConName2").InnerText;
            }
            return Constring;
        }
        public static string MyConn3()
        {
            string path = System.Web.HttpContext.Current.Server.MapPath("~/XML/cString3.xml");
            var Constring = "";
            XmlDocument xd = new XmlDocument();
            xd.Load(path);
            XmlNodeList IDListNode = xd.SelectNodes("/Table/constring");

            foreach (XmlNode item in IDListNode)
            {
                Constring = item.SelectSingleNode("ConName2").InnerText;
            }
            return Constring;
        }
        public static bool IsServerConnected()
        {
            using (var con = new SqlConnection(Common.MyConn()))
            {
                try
                {
                    con.Open();
                    con.Close();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }
    }
}