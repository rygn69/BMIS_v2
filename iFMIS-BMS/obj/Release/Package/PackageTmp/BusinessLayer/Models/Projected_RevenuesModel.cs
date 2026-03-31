using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class Projected_RevenuesModel
    {
        public int Projected_Revenue_ID { get; set; }
        public string Particular { get; set; }
        public double Amount_1 { get; set; }
        public double Amount_2 { get; set; }
        public double Amount_3 { get; set; }
        public double Amount_4 { get; set; }
        public int year_of { get; set; }


    }
}