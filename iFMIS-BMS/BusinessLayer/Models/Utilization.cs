using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class Utilization
    {
        public int ProposalID { get; set; }
        public int AccountID { get; set; }
        public int AccountCode { get; set; }
        public string AccountName { get; set; }
        public double Actual5 { get; set; }
        public double Approve5 { get; set; }
        public double Actual4 { get; set; }
        public double Approve4 { get; set; }
        public double Actual3 { get; set; }
        public double Approve3 { get; set; }
        public double Actual2 { get; set; }
        public double Approve2 { get; set; }
        public double Actual1 { get; set; }
        public double Approve1 { get; set; }
        public double ProposalAmount { get; set; }
        public int ProgramID { get; set; }
        public int ooeidpie { get; set; }
        public string LegendPiePSMooeCA { get; set; }
        public double TotalPSMooeCA { get; set; }
       
    }
    //public class UtilizationGraph
    //{
    //    public int officeid { get; set; }
    //    public int programid { get; set; }
    //    public int accountid { get; set; }
    //    public int ooeidpie { get; set; }
    //    public string LegendPiePSMooeCA { get; set; }
    //    public double TotalPSMooeCA { get; set; }
    //    public double psamount { get; set; }
    //    public double mooeamount { get; set; }
    //    public double coamount { get; set; }
    //    public string psname { get; set; }
    //    public string mooename { get; set; }
    //    public string coname { get; set; }

    //}
}