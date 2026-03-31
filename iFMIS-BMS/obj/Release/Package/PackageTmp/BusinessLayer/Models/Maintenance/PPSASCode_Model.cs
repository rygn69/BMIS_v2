using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.Maintenance
{
    public class PPSASCode_Model
    {
        public int Years { get; set; }
        public int? TransactionYear { get; set; }
        public string AccountName { get; set; }
        public string PPSASCode { get; set; }
        public int id { get; set; }
        public int AccountCode { get; set; }
        public int? FMISAccountCode { get; set; }
        public string ChildAccountCode { get; set; }
        public string PPASeriesCode { get; set; }
        public string PPAChildPPSASCode { get; set; }
    }
}