using AutoMapper;
using SFA.DAS.EmployerFinance.Application.Queries.GetHealthCheck.Dtos;
using SFA.DAS.EmployerFinance.Models;

namespace SFA.DAS.EmployerFinance.Mappings
{
    public class HealthCheckMappings : Profile
    {
        public HealthCheckMappings()
        {
            CreateMap<HealthCheck, HealthCheckDto>();
        }
    }
}