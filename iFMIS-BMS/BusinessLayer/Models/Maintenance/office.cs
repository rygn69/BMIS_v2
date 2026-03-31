using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class office
    {
        public Int64 office_id { get; set; }
        public string office_name { get; set; }
        public long eid { get; set; }
        public string EmpName { get; set; }
    }
}