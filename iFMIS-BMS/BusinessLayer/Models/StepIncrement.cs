using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Models
{
    public class StepIncrement
    {
        public Int64 eid { get; set; }
        public string ActualSalary { get; set; }
        public string BudgetSalary { get; set; }
        public Int16 SalaryGrade { get; set; }
        public Int16 Step { get; set; }
        public string StepColumn { get; set; }
        public Int16 SalaryGradeBudgeted { get; set; }
        public Int16 StepBudgeted { get; set; }
    }
}