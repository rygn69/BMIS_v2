using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class PMISUserModel
    {
        public Int64 eid { get; set; }
        public string empName { get; set; }
        public string UserTypeID { get; set; }
        public string IFMISOfficeID { get; set; }
        public int PMISOfficeID { get; set; }
    }
}