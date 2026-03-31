using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class UserLogInfo
    {
        public int UserLogID { get; set; }
        public string Date { get; set; }
        public string IP_Address { get; set; }
        public string PC_Name { get; set; }
        public string Time_in { get; set; }
        public string Time_out { get; set; }

        public string lgu_abbr { get; set; }
        public string lgu_site { get; set; }
        public string lgu_province { get; set; }
        public string lgu { get; set; }
        public string tempcolor { get; set; }
    }
}