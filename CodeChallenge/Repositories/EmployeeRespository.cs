using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;
using System.Collections;

namespace CodeChallenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            return _employeeContext.Employees.Include(e => e.DirectReports).SingleOrDefault(e => e.EmployeeId == id);
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }

        /// <summary>
        /// Retrieve the Reporting structure for a given Employee
        /// </summary>
        /// <param name="employeeID">ID String for the Employee</param>
        /// <returns>ReportingStructure object containing full Employee Hierarchy & Count of Reports</returns>
        public ReportingStructure GetReportingStructureById(string employeeID)
        {
            Employee employee = GetById(employeeID);
            BuildReportHeirarchy(employee);

            ReportingStructure reportingStructure = new ReportingStructure()
            {
                Employee = employee,
                NumberOfReports = GetReportCountForEmployee(employee),
            };

            return reportingStructure;

        }

        /// <summary>
        /// Internal Helper to update the Employee Ojbect/Tree by setting the Nested Reports
        /// This is done because EF's default behavior is not to eager load nested objects more than 1 deep
        /// </summary>
        /// <param name="upperEmployee">Employee Object - The top level employee that we are generating the tree for</param>
        private void BuildReportHeirarchy(Employee upperEmployee)
        {
            if (upperEmployee != null)
            {
                /*
                 * We Need to re-retrieve the Direct Report based off of the ID to load their Reports
                 * This is an intentional decision based on the Eager Loading behavior of this Nested Structure (in that it doesn't do nested eager loading)
                 * An argument could be made for implementing a solution as described here - https://michaelceber.medium.com/implementing-a-recursive-projection-query-in-c-and-entity-framework-core-240945122be6
                 * At this data volume and for the purposes of this demo, this approach is sufficient.
                 * Please see the ReadMe for more detailed info on this
                 */
                foreach (Employee dr in upperEmployee.DirectReports)
                {
                    Employee reportingEmployee = GetById(dr.EmployeeId);
                    dr.DirectReports = reportingEmployee.DirectReports;
                }
            }
        }

        /// <summary>
        /// Internal Helper to do a recursive count of entire reporting structure tree and return the full count of direct reports for one employee
        /// </summary>
        /// <param name="employee">The employee object for the top level employee</param>
        /// <returns>Integer count of # of Employees in the passed in Employee's report structure</returns>
        private int GetReportCountForEmployee(Employee employee)
        {
            if(employee == null)
                return 0;

            int count = employee.DirectReports.Count;

            foreach (Employee directReport in employee.DirectReports)
            {
                count += GetReportCountForEmployee(directReport);
            }

            return count;
        }
    }
}
