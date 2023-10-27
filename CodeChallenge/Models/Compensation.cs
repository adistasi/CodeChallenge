using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        public string CompensationID { get; set; }
        public double Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string EmployeeID { get; set; }
        public Employee Employee { get; set; }
    }
}
