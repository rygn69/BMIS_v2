using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class OfficesModel
    {
        public string OfficeID { get; set; }
        public string OfficeName { get; set; }
        public int UserTypeID { get; set; }
        public string FundType { get; set; }
        public string SubOffice { get; set; }
    }
}