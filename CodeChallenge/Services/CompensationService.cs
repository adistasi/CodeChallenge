using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
        private readonly ILogger<CompensationService> _logger;

        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository)
        {
            _compensationRepository = compensationRepository;
            _logger = logger;
        }

        public Compensation Create(Compensation compensation)
        {
            if(compensation != null)
            {
                _compensationRepository.Add(compensation);
                _compensationRepository.SaveAsync().Wait();
            }

            return compensation;
        }

        public Employee FetchEmployee(String employeeID) {
            if (!String.IsNullOrEmpty(employeeID))
            {
                return _compensationRepository.GetEmployee(employeeID);
            }

            return null;
        }

        public Compensation GetByEmployeeId(String employeeID)
        {
            if(!String.IsNullOrEmpty(employeeID))
            {
                return _compensationRepository.GetByEmployeeId(employeeID);
            }

            return null;
        }
    }
}
