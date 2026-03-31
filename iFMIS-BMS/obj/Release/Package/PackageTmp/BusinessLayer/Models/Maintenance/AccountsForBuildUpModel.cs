using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.Maintenance
{
    public class AccountsForBuildUpModel
    {
        public int AccountID { get; set; }
        public int OrderNo { get; set; }
        public string AccountName { get; set; }
        public string OfficeName { get; set; }
    }
}