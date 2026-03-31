using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.Base
{
    public class SAAODA
    {
        public long trnno { get; set; }
        public int ooeid { get; set; }
        public string office { get; set; }
        public string program { get; set; }
        public string AccountName { get; set; }
        public decimal Appropriation { get; set; }
        public decimal Allotment { get; set; }
        public decimal Obligation { get; set; }
        public decimal Disbursed { get; set; }
        public decimal Accounted { get; set; }

    }
}