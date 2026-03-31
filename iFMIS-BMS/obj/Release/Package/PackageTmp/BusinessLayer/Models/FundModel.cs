using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class FundModel
    {
        public int FundID { get; set; }
        public int FundCode { get; set; }
        public int FundFlag { get; set; }
        public string FundName { get; set; }
        public string FundMedium { get; set; }
    }
}