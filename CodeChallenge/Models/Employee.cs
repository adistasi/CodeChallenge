using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Models
{
    public class Employee
    {
        private List<Employee> _directReports;

        public String EmployeeId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Position { get; set; }
        public String Department { get; set; }
        
        // Making this change due to the behavior of loading an array from JSON in C# (Wouldn't be necessary if loading from a DB)
        // Without this, no DirectReports would result in a `null` value, this makes it result in an empty list
        public List<Employee> DirectReports { 
            get { return _directReports ?? (_directReports = new List<Employee>()); }
            set { _directReports = value; } 
        }
    }
}
