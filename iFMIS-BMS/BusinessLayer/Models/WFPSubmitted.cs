using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class WFPSubmitted
    {
        public long trnno { get; set; }
        public string objectexpenditure { get; set; }
        public double Appropriation { get; set; }
        public double Reserved { get; set; }
        public double Netprogram { get; set; }
        public double Firstmonth { get; set; }
        public double Secondmonth { get; set; }
        public double Thirdmonth { get; set; }
        public string MFO { get; set; }
        public string PTFirstmonth { get; set; }
        public string PTSecondmonth { get; set; }
        public string PTThirdmonth { get; set; }
        public int officeid { get; set; }
        public int programid { get; set; }
        public long accountid { get; set; }
        public string officemfo { get; set; }
        public double AppropriationNet { get; set; }
        public int qtr { get; set; }
        public double firstmonth_rel { get; set; }
        public double secondmonth_rel { get; set; }
        public double thirdmonth_rel { get; set; }
        public long transno { get; set; }
        public string AccountName { get; set; }
        public string remarks { get; set; }
        public string datetimentered { get; set; }
        public double fistamount { get; set; }
        public double secondamount { get; set; }
        public double thirdamount { get; set; }
        public string Qtr { get; set; }
        public string ooe { get; set; }
        
        public double firstmon_release { get; set; }
        public double secondmon_release { get; set; }
        public double thirdmon_release { get; set; }
        public int approve { get; set; }
        public double denoAmount { get; set; }
        public double qty { get; set; }
        public double TotaldenoAmount { get; set; }
        public string particular { get; set; }
        public int mon { get; set; }
        public string officename { get; set; }
        public int userofficeid { get; set; }
        public double totalamount { get; set; }
        public double allotment { get; set; }
        public double balance { get; set; }
        public string submitdatetime { get; set; }
        public double appropriation_whole { get; set; }
        public double reserve { get; set; }
        public int reserveflag { get; set; }
        public int procurement { get; set; }
    }
}