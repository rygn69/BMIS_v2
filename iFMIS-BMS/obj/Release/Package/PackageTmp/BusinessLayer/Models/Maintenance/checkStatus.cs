using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.Maintenance
{
    public class checkStatus
    {
        public int status_id { get; set; }
        public int UserTypeID { get; set; }
        public string UserTypeDesc { get; set; }
    }
}