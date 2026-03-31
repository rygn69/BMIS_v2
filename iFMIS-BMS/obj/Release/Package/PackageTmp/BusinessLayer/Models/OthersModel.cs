using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class OthersModel
    {
        public int other_id { get; set; }
        public string other_name { get; set; }
        public double other_Amount { get; set; }
        public int OfficeID { get; set; }
        public int Year { get; set; }

    }
}