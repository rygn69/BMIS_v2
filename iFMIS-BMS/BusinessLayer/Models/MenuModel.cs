using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class MenuModel
    {
        public int MenuID { get; set; }
        public int MenuLevel { get; set; }
        public string MenuName { get; set; }
        public string MenuDescription { get; set; }
        public string MenuIcon { get; set; }
        public string MenuController { get; set; }
        public string MenuAction { get; set; }
        public int MenuOrder { get; set; }
        public int  MenuParent { get; set; }
        public long access { get; set; }
    }
}