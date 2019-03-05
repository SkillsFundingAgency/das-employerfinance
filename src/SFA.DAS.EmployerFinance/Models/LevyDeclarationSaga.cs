using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerFinance.Messages.Events;

namespace SFA.DAS.EmployerFinance.Models
{
    public class LevyDeclarationSaga : Entity
    {
        public int Id { get; private set; }
        public LevyDeclarationSagaType Type { get; private set; }
        public DateTime PayrollPeriod { get; private set; }
        public long HighWaterMarkId { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime? Updated { get; private set; }
        public int ImportPayeSchemeLevyDeclarationsTasksCount { get; private set; }
        public int ImportPayeSchemeLevyDeclarationsTasksCompleteCount { get; private set; }
        public DateTime? ImportPayeSchemeLevyDeclarationsTasksFinished { get; private set; }
        public int UpdateAccountTransactionBalancesTasksCount { get; private set; }
        public int UpdateAccountTransactionBalancesTasksCompleteCount { get; private set; }
        public DateTime? UpdateAccountTransactionBalancesTasksFinished { get; private set; }
        public bool IsComplete { get; private set; }
        public TimeSpan Timeout => TimeSpan.FromMinutes(2);

        public LevyDeclarationSaga(DateTime payrollPeriod, IReadOnlyCollection<AccountPayeScheme> accountPayeSchemes)
        {
            Type = LevyDeclarationSagaType.All;
            PayrollPeriod = payrollPeriod;
            Created = DateTime.UtcNow;
            HighWaterMarkId = accountPayeSchemes.Max(aps => aps.Id);
            ImportPayeSchemeLevyDeclarationsTasksCount = accountPayeSchemes.Select(aps => aps.EmployerReferenceNumber).Distinct().Count();
            UpdateAccountTransactionBalancesTasksCount = accountPayeSchemes.Select(aps => aps.AccountId).Distinct().Count();
            
            Publish(() => new StartedProcessingLevyDeclarationsEvent(Id, PayrollPeriod, Created));
        }

        public LevyDeclarationSaga(DateTime payrollPeriod, AccountPayeScheme accountPayeScheme)
        {
            Type = LevyDeclarationSagaType.AdHoc;
            PayrollPeriod = payrollPeriod;
            Created = DateTime.UtcNow;
            HighWaterMarkId = accountPayeScheme.Id;
            ImportPayeSchemeLevyDeclarationsTasksCount = 1;
            UpdateAccountTransactionBalancesTasksCount = 1;
            
            Publish(() => new StartedProcessingLevyDeclarationsAdHocEvent(Id, PayrollPeriod, HighWaterMarkId, Created));
        }

        private LevyDeclarationSaga()
        {
        }
        
        public void UpdateProgress(IReadOnlyCollection<LevyDeclarationSagaTask> tasks)
        {
            if (IsComplete)
            {
                return;
            }
            
            if (ImportPayeSchemeLevyDeclarationsTasksCompleteCount < ImportPayeSchemeLevyDeclarationsTasksCount)
            {
                ImportPayeSchemeLevyDeclarationsTasksCompleteCount = tasks.Count(t => t.Type == LevyDeclarationSagaTaskType.ImportPayeSchemeLevyDeclarations);
                Updated = DateTime.UtcNow;

                if (ImportPayeSchemeLevyDeclarationsTasksCompleteCount == ImportPayeSchemeLevyDeclarationsTasksCount)
                {
                    ImportPayeSchemeLevyDeclarationsTasksFinished = Updated;
                        
                    Publish(() => new UpdatedLevyDeclarationSagaProgressEvent(Id));
                }
            }
            else
            {
                UpdateAccountTransactionBalancesTasksCompleteCount = tasks.Count(t => t.Type == LevyDeclarationSagaTaskType.UpdateAccountTransactionBalances);
                Updated = DateTime.UtcNow;

                if (UpdateAccountTransactionBalancesTasksCompleteCount == UpdateAccountTransactionBalancesTasksCount)
                {
                    UpdateAccountTransactionBalancesTasksFinished = Updated;
                    IsComplete = true;

                    switch (Type)
                    {
                        case LevyDeclarationSagaType.All:
                            Publish(() => new FinishedProcessingLevyDeclarationsEvent(Id, PayrollPeriod, Updated.Value));
                            break;
                        case LevyDeclarationSagaType.AdHoc:
                            Publish(() => new FinishedProcessingLevyDeclarationsAdHocEvent(Id, PayrollPeriod, HighWaterMarkId, Updated.Value));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
    }
}
