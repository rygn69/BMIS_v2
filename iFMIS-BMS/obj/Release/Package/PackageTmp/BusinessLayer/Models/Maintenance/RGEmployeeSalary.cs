using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models.Maintenance
{
    public class RGEmployeeSalary
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public decimal BudgetSalary { get; set; }
        public decimal AppointmentSalary { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string MI { get; set; }
    }
}