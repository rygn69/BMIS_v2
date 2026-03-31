using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.Maintenance
{
    public class ClimateChangeCodesModel
    {
        public int CCCodeID { get; set; }
        public int OrderNo { get; set; }
        public string CCCode { get; set; }
        public string CCCDescription { get; set; }
        public string CCTypeDesc { get; set; }
    }
}