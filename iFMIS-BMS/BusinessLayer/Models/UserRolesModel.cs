using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class UserRolesModel
    {
        public int RoleID { get; set; }
        public int UserID { get; set; }
        public int MenuID { get; set; }
        public int UserMenuID { get; set; }

    }
}