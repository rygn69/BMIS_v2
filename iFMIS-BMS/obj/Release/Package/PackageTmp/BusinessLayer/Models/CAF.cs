using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class CAFPosting
    {
        public int cafid { get; set; }
        public int officeid { get; set; }
        public int programid { get; set; }
        public int accountid { get; set; }
        public int ppaid { get; set; }
        public int nonofficeid { get; set; }
        public string dscription { get; set; }
        public double amount { get; set; }
        public string cafno { get; set; }
        public string office { get; set; }
        public string account { get; set; }
        public string dateissued { get; set; }
    }
}