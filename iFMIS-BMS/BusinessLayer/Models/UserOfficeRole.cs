using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class UserOfficeRole
    {
        public int User_ID { get; set; }
        public int eid { get; set; }
        public string Office_Name { get; set; }
        public string UserTypeDesc { get; set; }
        public int UserTypeID { get; set; }
        public int IFMISOfficeID { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string pcode { get; set; }
        public string depthead { get; set; }
        public string deptheadposition { get; set; }
    }
}