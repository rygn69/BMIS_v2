using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class separate_Model
    {
        public int per_ID { get; set; }
        public string per_Desc { get; set; }

        public int AccountID { get; set; }
        public string AccountName { get; set; }

        public int OOEID { get; set; }
        public string OOEName { get; set; }

    }
}