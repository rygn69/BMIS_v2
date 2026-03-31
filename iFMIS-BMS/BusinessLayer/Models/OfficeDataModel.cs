using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class OfficeDataModel
    {
        public int PMISOfficeID { get; set; }
        public int IFMISOfficeID { get; set; }
        public string PMISOfficeDesc { get; set; }
        public string IFMISOfficeDesc { get; set; }
    }
}