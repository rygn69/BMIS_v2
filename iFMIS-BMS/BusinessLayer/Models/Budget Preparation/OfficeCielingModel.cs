using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.Budget_Preparation
{
    public class OfficeCielingModel
    {
        public int SeriesID { get; set; }
        public int OfficeID { get; set; }
        public string OfficeName { get; set; }
        public int Percentage { get; set; }
        public double Amount { get; set; }
        public double OriginalAmount { get; set; }
        public double PercentageIncrease { get; set; }
        public int ActionCode { get; set; }
    }
}