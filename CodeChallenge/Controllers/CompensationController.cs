using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/compensation")]
    public class CompensationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        /// <summary>
        /// POST to save a compensation object
        /// Requires an corresponding Employee (matched by ID)
        /// </summary>
        /// <param name="compensation">Compensation object containing EmployeeID (String) (to attach to Employee), Salary (Double), and EffectiveDate (DateString)</param>
        /// <returns>CreatedAtRoute() with object or BadRequest if the connected employee doesn't exist</returns>
        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received employee create request for Employee ID '{compensation.EmployeeID}'");
            
            // Retrieve employee based off of inputted ID to ensure validity, error out if not present
            _logger.LogDebug($"Attempting to fetch Employee for Employee ID '{compensation.EmployeeID}'");
            Employee compEmployee = _compensationService.FetchEmployee(compensation.EmployeeID);

            if (compEmployee == null)
                return BadRequest($"No Employee with ID {compensation.EmployeeID}");

            // Attach Employee Object to return structure
            compensation.Employee = compEmployee;
            _compensationService.Create(compensation);

            return CreatedAtRoute("getCompensationByEmployeeID", new { employeeID = compensation.EmployeeID }, compensation);
        }

        /// <summary>
        /// GET to retrieve a compensation object given the associated EmployeeID
        /// </summary>
        /// <param name="employeeID">String EmployeeID for the employee assoicated with the compensation</param>
        /// <returns>OK() containing compensation model or NotFound() if an invalid employee ID is supplied</returns>
        [HttpGet("{employeeID}", Name = "getCompensationByEmployeeID")]
        public IActionResult GetCompensationByEmployeeID(String employeeID)
        {
            _logger.LogDebug($"Received employee get request for '{employeeID}'");

            var compensation = _compensationService.GetByEmployeeId(employeeID);

            if (compensation == null)
                return NotFound();

            // Attach Employee Object for Return
            compensation.Employee = _compensationService.FetchEmployee(compensation.EmployeeID);


            return Ok(compensation);
        }
    }
}
