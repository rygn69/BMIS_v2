using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.Maintenance
{
    public class OrdinanceDescriptionModel
    {
        public int DescriptionID { get; set; }
        public string OfficeName { get; set; }
        public string OfficeDescription { get; set; }
        public int OfficeOrder { get; set; }
        public int OfficeID { get; set; }
    }
}