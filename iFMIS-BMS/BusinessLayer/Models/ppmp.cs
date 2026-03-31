using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class ppmp
    {
        public string itemname { get; set; }
        public string unit { get; set; }
        public double unitcost { get; set; }
        public double qty { get; set; }
        public int qty1 { get; set; }
        public int qty2 { get; set; }
        public int qty3 { get; set; }
        public int qty4 { get; set; }
        public double amount { get; set; }
        public int OfficeID { get; set; }
        public int ProgramID { get; set; }
        public int AccountID { get; set; }
        public int SubAccountID { get; set; }
        public int ExcessID { get; set; }
        public string returnPPMPitems { get; set; }
        public string returnPPMPLumpSum { get; set; }
        public string returnPPMPNonOffice { get; set; }
        public string returnPPMPNonOfficeLumpSum { get; set; }
        public int transno { get; set; }
        public string description { get; set; }
        public string transnopr { get; set; }
        public int earmark { get; set; }
        public int itemid { get; set; }
        public int itemgroup_id { get; set; }
        public string itemgroupname { get; set; }
        public string itemsubgroupname { get; set; }
        public int prid { get; set; }
        public int wfpid {get;set;}
        public int fundid { get; set; }
        public int groupid { get; set; }
        public string prno { get; set; }
        public string obrno { get; set; }
        public int subppaid { get; set; }
        public double pramount { get; set; }
        public string wfpno { get; set; }
    }
}