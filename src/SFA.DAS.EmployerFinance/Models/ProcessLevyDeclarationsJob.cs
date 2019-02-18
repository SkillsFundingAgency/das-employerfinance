using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.NServiceBus.ClientOutbox;

namespace SFA.DAS.EmployerFinance.Models
{
    public class ProcessLevyDeclarationsJob : Entity
    {
        public Guid Id { get; private set; }
        public DateTime PayrollPeriod { get; private set; }
        public AccountPayeScheme AccountPayeScheme { get; private set; }
        public long? AccountPayeSchemeId { get; private set; }
        public int ImportLevyDeclarationsTasksCount { get; private set; }
        public int ImportLevyDeclarationsTasksCompletedCount { get; private set; }
        public int UpdateAccountBalanceTasksCount { get; private set; }
        public int UpdateAccountBalanceTasksCompletedCount { get; private set; }
        public bool IsComplete { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime? Updated { get; private set; }
        public TimeSpan Timeout => TimeSpan.FromMinutes(2);

        public ProcessLevyDeclarationsJob(DateTime payrollPeriod, IReadOnlyCollection<AccountPayeScheme> accountPayeSchemes)
        {
            Id = GuidComb.NewGuidComb();
            PayrollPeriod = payrollPeriod;
            ImportLevyDeclarationsTasksCount = accountPayeSchemes.Select(aps => aps.EmployerReferenceNumber).Distinct().Count();
            UpdateAccountBalanceTasksCount = accountPayeSchemes.Select(aps => aps.AccountId).Distinct().Count();
            Created = DateTime.UtcNow;
        }

        public ProcessLevyDeclarationsJob(DateTime payrollPeriod, AccountPayeScheme accountPayeScheme)
        {
            Id = GuidComb.NewGuidComb();
            PayrollPeriod = payrollPeriod;
            AccountPayeScheme = accountPayeScheme;
            AccountPayeSchemeId = AccountPayeSchemeId;
            ImportLevyDeclarationsTasksCount = 1;
            UpdateAccountBalanceTasksCount = 1;
            Created = DateTime.UtcNow;
        }

        private ProcessLevyDeclarationsJob()
        {
        }
        
        public ProcessLevyDeclarationsJobStateChange UpdateProgress(IReadOnlyCollection<ProcessLevyDeclarationsJobTask> tasks)
        {
            var stateChange = ProcessLevyDeclarationsJobStateChange.None;

            if (!IsComplete)
            {
                if (ImportLevyDeclarationsTasksCompletedCount < ImportLevyDeclarationsTasksCount)
                {
                    ImportLevyDeclarationsTasksCompletedCount = tasks.Count(t => t.Type == ProcessLevyDeclarationsJobTaskType.ImportLevyDeclarations);
                    Updated = DateTime.UtcNow;

                    if (ImportLevyDeclarationsTasksCompletedCount >= ImportLevyDeclarationsTasksCount)
                    {
                        stateChange = ProcessLevyDeclarationsJobStateChange.ImportLevyDeclarationsTasksCompleted;
                    }
                }
                else
                {
                    UpdateAccountBalanceTasksCompletedCount = tasks.Count(t => t.Type == ProcessLevyDeclarationsJobTaskType.UpdateAccountBalance);
                    Updated = DateTime.UtcNow;

                    if (UpdateAccountBalanceTasksCompletedCount >= UpdateAccountBalanceTasksCount)
                    {
                        stateChange = ProcessLevyDeclarationsJobStateChange.UpdateAccountBalanceTasksCompleted;
                        IsComplete = true;
                    }
                }
            }

            return stateChange;
        }
    }
}