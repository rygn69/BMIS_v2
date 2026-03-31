using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers
{
    public class CurrentControl_Model
    {
        public int ConnectionStatus { get; set; }
        public string MTitle { get; set; }
        public string MBody { get; set; }
        public string MType { get; set; }
        public double Amount { get; set; }
        public double AllotedAmount { get; set; }
        public double ObligatedAmount { get; set; }
        public double BalanceAllotment { get; set; }
        public int ActionCode { get; set; }
        public string ORNumber { get; set; }
        public double Appropriation { get; set; }
        public bool enablefunction { get; set; }
        public int beginyear { get; set; }
        public double totreserveapp { get; set; }
        public int subppatag { get; set; }
        public int first { get;set;}
        public int second { get; set; }
        public int third { get; set; }
        public int fourth { get; set; }
    }
}