using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class GrantsModel
    {

        public int grants_id { get; set; }
        public string grants_name { get; set; }
        public double grants_Amount { get; set; }
        public int OfficeID { get; set; }
        public int Year { get; set; }
    }
}