using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarations;
using SFA.DAS.EmployerFinance.Services;

namespace SFA.DAS.EmployerFinance.Jobs.ScheduledJobs
{
    public class ProcessLevyDeclarationsJob
    {
        private readonly IMessageSession _messageSession;
        private readonly IDateTimeService _dateTimeService;
        private readonly ILogger _logger;

        public ProcessLevyDeclarationsJob(IMessageSession messageSession, IDateTimeService dateTimeService, ILogger logger)
        {
            _messageSession = messageSession;
            _dateTimeService = dateTimeService;
            _logger = logger;
        }

        public Task Run([TimerTrigger("0 0 15 20 * *")] TimerInfo timer)
        {
            var now = _dateTimeService.UtcNow;
            var today = now.Date;
            var month = new DateTime(today.Year, today.Month, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var payrollPeriod = month.AddMonths(-1);
            var command = new ProcessLevyDeclarationsCommand(payrollPeriod);
            var task = _messageSession.Send(command);

            _logger.LogInformation($"Sent '{nameof(ProcessLevyDeclarationsCommand)}' with '{nameof(ProcessLevyDeclarationsCommand.PayrollPeriod)}' value '{command.PayrollPeriod:MM yyyy}'");
            
            return task;
        }
    }
}