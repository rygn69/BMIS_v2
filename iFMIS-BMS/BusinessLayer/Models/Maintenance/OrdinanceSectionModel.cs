using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.Maintenance
{
    public class OrdinanceSectionModel
    {
        public Int64 SectionID { get; set; }
        public string SectionName { get; set; }
        public string SectionDescription { get; set; }
        public int SectionOrder { get; set; }
    }
}