using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.Maintenance
{
    public class programs
    {
        public int program_id { get; set; }
        public string program_name { get; set; }
        public int fund_id { get; set; }
        public int orderBY { get; set; }
        public int OfficeProgramID { get; set; }
        public string OfficeName { get; set; }
        public int office_id { get; set; }

    }
}