using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class EmployeeSalaryModel
    {
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }

        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double BasicSalary { get; set; }
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double AmountUsed { get; set; }
        [DisplayFormat(DataFormatString = "₱{0:N2}")]
        [Range(0, 1000000000000)]
        public double GivenAmount { get; set; }
        public string EmployeeStatus { get; set; }

    }
}