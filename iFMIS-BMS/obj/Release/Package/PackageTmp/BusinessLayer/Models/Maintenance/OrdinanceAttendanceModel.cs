using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.Maintenance
{
    public class OrdinanceAttendanceModel
    {
        public int SeriesID { get; set; }
        public int GroupOrderNo { get; set; }
        public int OrderNo { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Status { get; set; }
    }
}