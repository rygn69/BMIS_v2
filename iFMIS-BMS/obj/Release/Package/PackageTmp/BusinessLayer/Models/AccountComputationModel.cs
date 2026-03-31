using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class AccountComputationModel
    {
        public int AccountCode { get; set; }
        public string AccountName { get; set; }
        public double Amount { get; set; }
        public int Month { get; set; }
        public double Percentage { get; set; }
        public Int16 isRoundOf { get; set; }
        public double MaxAmount { get; set; }
        public Int32 EmployeeType { get; set; }
        public double ComputedAmount { get; set; }
        public int noOfEmployees { get; set; }
        public double totalBasicSalary { get; set; }
        public Int64 ComputationID { get; set; }
        public string Period { get; set; }
        public string RoundedOff { get; set; }
        public string EmpType { get; set; }
        public string strAmount { get; set; }
    }
}