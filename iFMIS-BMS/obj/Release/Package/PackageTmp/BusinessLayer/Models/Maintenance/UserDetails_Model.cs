using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.Maintenance
{
    public class UserDetails_Model
    {

        public int UserID { get; set; }
        public string UserTypeDesc { get; set; }
        public int eid { get; set; }
        public string emailaddress { get; set; }
        public string passcode { get; set; }
        public string EmpName { get; set; }
        public int IFMISOfficeID { get; set; }
        public int swipeid { get; set; }
        public string OfficeName { get; set; }
    }
}