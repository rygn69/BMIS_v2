using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.Maintenance
{
    public class AccountsToCombineModel
    {
        public int SeriesID { get; set; }
        public int isCombineWithNonOffice { get; set; }
        public string AccountName { get; set; }
    }
}