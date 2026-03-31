using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class MonthlyR_Model
    {
        public int monthly_id { get; set; }
        public string subjects { get; set; }
        public double ps { get; set; }
        public double mooe { get; set; }
        public double co { get; set; }
        public double subsidy { get; set; }
        public double income { get; set; }
        public int type_desu { get; set; }

        
    }
}