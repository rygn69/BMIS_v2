using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.Maintenance
{
    public class OrdinanceSpecialProvisionsModel
    {
        public Int64 SeriesID { get; set; }
        public string OfficeName { get; set; }
        public string OrderNo { get; set; }
        public string Description { get; set; }
        public int OfficeOrder { get; set; }
    }
}