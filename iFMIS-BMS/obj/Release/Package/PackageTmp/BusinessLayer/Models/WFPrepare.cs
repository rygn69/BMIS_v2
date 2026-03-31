using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class WFPrepare
    {
        public int wfpid { get; set; }
        public int officeid { get; set; }
        public int programid { get; set; }
        public long accountid { get; set; }
        public string accountname { get; set; }
        public string account { get; set; }
        public int activityid { get; set; }
        public string activityname { get; set; }
        public string specificactivity { get; set; }
        public string particular { get; set; }

        public string unit { get; set; }
        public string weight { get; set; }
        public double m1 { get; set; }
        public double m2 { get; set; }
        public double m3 { get; set; }
        public double m4 { get; set; }
        public double m5 { get; set; }
        public double m6 { get; set; }
        public double m7 { get; set; }
        public double m8 { get; set; }
        public double m9 { get; set; }
        public double m10 { get; set; }
        public double m11 { get; set; }
        public double m12 { get; set; }
        public double totalqty { get; set; }
        public int noofdays { get; set; }
        public double amount { get; set; }
        public double totalamount { get; set; }
        public string physicaltarget { get; set; }
        public string devtindicator { get; set; }
        public long responsibleperson { get; set; }
        public string datetimeentered { get; set; }
        public string remarks { get; set; }
        public int subppaid { get; set; }
        public string description { get; set; }
        public int isPPMP { get; set; }
        public int nonofficeid { get; set; }
        public int nonofficeidparent { get; set; }
        public int fundid { get; set; }
        public int kpmid { get; set; }
        public int fundreqid { get; set; }
        public int breakdownid { get; set; }
        public string fund { get; set; }
        public int locationid { get; set; }
        public int barangayid { get; set; }
        public string barangay { get; set; }
        public int municipalid { get; set; }
        public string municipal { get; set; }
        public string account_parent { get; set; }
        public string office { get; set; }
        public int yearof { get; set; }
        public string ooe { get; set; }
        public int ooeid { get; set; }
        public string completiondate { get; set; }
        public int withapprove { get; set; }
        public string officeabbr { get; set; }
        public int actioncode { get; set; }
        public string firstqtr {get; set;}
        public string secondqtr { get; set; }
        public string thirdqtr { get; set; }
        public string fourthqtr { get; set; }
        public int itemid { get; set; }
        public string itemname { get; set; }
        public double unitcost { get; set; }
        public string prepareuser { get; set; }
        public string status { get; set; }
        public int docid { get; set; }
        public int gov_sig { get; set; }
        public string returnby { get; set; }
        public string subppaname { get; set; }
        public int revno { get; set; }
        public string wfpno { get; set; }
        public string activityfrom { get; set; }
        public string activityto { get; set; }
        public int pullitem { get; set; }
        public string pullitem_desc { get; set; }
        public int specid { get; set; }
        public string project { get; set; }
        public int procurement { get; set; }
        public string program { get; set; }
        public int isupplemental { get; set; }
        public int groupid { get; set; }
        public int isassessed { get; set; }
        public double proposeamount { get; set; }
        public double slashamount { get; set; }
        public double diffamount { get; set; }
        public int qty { get; set; }
        public int mon { get; set; }
        public string item { get; set; }
        public int chkbox { get; set; }
        public int withpr { get; set; }
        public int withad { get; set; }
        public double firstmon { get; set; }
        public double secondmon { get; set; }
        public double thirdmon { get; set; }
        public double fourthmon { get; set; }
        public string indicator { get; set; }
        public string physical1 { get; set; }
        public string physical2 { get; set; }
        public string physical3 { get; set; }
        public string physical4 { get; set; }
        public int fpay { get; set; }
        public string arono { get; set; }
        public int trnno { get; set; }
        public string aipcode { get; set; }
        public string ppsas { get; set; }
        public int docstatusid { get; set; }
        public int qty2 { get; set; }
    }
}