using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class UserModel
    {
        public string UserTypeDesc { get; set; }
        public Int64 eid { get; set; }
        public string emailaddress  { get; set; }
        public string Passcode { get; set; }
        public string empName { get; set; }
        public string Department { get; set; }
        public string imgName { get; set; }
        public string OfficeName { get; set; }
        public int Countopis { get; set; }
        public int UserTypeID { get; set; }
        public int lgu { get; set; }

    }
}