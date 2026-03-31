using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class UserMenuModel
    {
        public int User_ID { get; set; }
        public int eid { get; set; }
        public string Office_Name { get; set; }
        public string UserTypeDesc { get; set; }
        public int UserTypeID { get; set; }
        public int IFMISOfficeID { get; set; }

    }
}