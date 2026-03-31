using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.Maintenance
{
    public class createNewAccount
    {
        public int text_office_id { get; set; }
        public int program_id { get; set; }
        public int ooe_id { get; set; }
        public string ThirdLevelGroup { get; set; }
        public string fund_id { get; set; }
        public string account_name { get; set; }
        public int AccountCode { get; set; }
        public string child_series_no { get; set; }
        public int orderBy { get; set; }
        public int ammount_money { get; set; }
        public string AccountName { get; set; }
        public int AccountID { get; set; }
    }
}