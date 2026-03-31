using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.Models
{
	
    public class NonOfficeSubAccount
    {
        public int SPO_ID { get; set; }
        public int nonofficeid { get; set; }
        public int officeid { get; set; }
        public int programid { get; set; }
        public int accountid { get; set; }
        public decimal amount { get; set; }
        public string AccountName { get; set; }
        public int YearOf { get; set; }
        public int ActionCode { get; set; }
        public string DateTimeEntered { get; set; }
        public int UserID { get; set; }
        public  int nonofficeidparent {get;set;}
        public decimal Amount { get; set; }
        public int caf_id { get; set; }
        public int comid { get; set; }
        public string excessaccount { get; set; }
        public string linkaccount { get; set; }
        
    }
}