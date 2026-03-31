using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace iFMIS_BMS.Classes
{
    public class GetTransactionYear
    {
        public static string TransactionYear()
        {
            string path = System.Web.HttpContext.Current.Server.MapPath("~/XML/TransactionYear.xml");
            var TransYear = "";
            XmlDocument xd = new XmlDocument();
            xd.Load(path);
            XmlNodeList IDListNode = xd.SelectNodes("/Table");

            foreach (XmlNode item in IDListNode)
            {
                TransYear = item.SelectSingleNode("TransactionYear").InnerText;
            }
            return TransYear;
        }
    }
}