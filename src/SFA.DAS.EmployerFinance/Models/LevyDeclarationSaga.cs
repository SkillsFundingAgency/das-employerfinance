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
        public AccountPayeScheme AccountPayeSchemeHighWaterMark { get; private set; }
        public long? AccountPayeSchemeHighWaterMarkId { get; private set; }
        public AccountPayeScheme AccountPayeScheme { get; private set; }
        public long? AccountPayeSchemeId { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime? Updated { get; private set; }
        public int ImportPayeSchemeLevyDeclarationsTasksCount { get; private set; }
        public int ImportPayeSchemeLevyDeclarationsTasksFinishedCount { get; private set; }
        public int ImportPayeSchemeLevyDeclarationsTasksErroredCount { get; private set; }
        public bool IsStage1Complete => ImportPayeSchemeLevyDeclarationsTasksFinishedCount == ImportPayeSchemeLevyDeclarationsTasksCount;
        public int UpdateAccountTransactionBalancesTasksCount { get; private set; }
        public int UpdateAccountTransactionBalancesTasksFinishedCount { get; private set; }
        public int UpdateAccountTransactionBalancesTasksErroredCount { get; private set; }
        public bool IsStage2Complete => UpdateAccountTransactionBalancesTasksFinishedCount == UpdateAccountTransactionBalancesTasksCount;
        public bool IsComplete { get; private set; }
        public static TimeSpan Timeout => TimeSpan.FromMinutes(2);

        public LevyDeclarationSaga(DateTime payrollPeriod, IReadOnlyCollection<AccountPayeScheme> accountPayeSchemes)
        {
            Type = LevyDeclarationSagaType.Planned;
            PayrollPeriod = payrollPeriod;
            Created = DateTime.UtcNow;
            AccountPayeSchemeHighWaterMarkId = accountPayeSchemes.Max(aps => aps.Id);
            ImportPayeSchemeLevyDeclarationsTasksCount = accountPayeSchemes.Select(aps => aps.EmployerReferenceNumber).Count();
            UpdateAccountTransactionBalancesTasksCount = accountPayeSchemes.Select(aps => aps.AccountId).Distinct().Count();
            
            Publish(() => new StartedProcessingLevyDeclarationsEvent(Id, PayrollPeriod, AccountPayeSchemeHighWaterMarkId.Value,Created));
        }

        public LevyDeclarationSaga(DateTime payrollPeriod, AccountPayeScheme accountPayeScheme)
        {
            Type = LevyDeclarationSagaType.AdHoc;
            PayrollPeriod = payrollPeriod;
            Created = DateTime.UtcNow;
            AccountPayeSchemeId = accountPayeScheme.Id;
            ImportPayeSchemeLevyDeclarationsTasksCount = 1;
            UpdateAccountTransactionBalancesTasksCount = 1;
            
            Publish(() => new StartedProcessingLevyDeclarationsAdHocEvent(Id, PayrollPeriod, AccountPayeSchemeId.Value, Created));
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

            if (!IsStage1Complete)
            {
                UpdateStage1Progress(tasks.Where(t => t.Type == LevyDeclarationSagaTaskType.ImportPayeSchemeLevyDeclarations));
            }
            else
            {
                UpdateStage2Progress(tasks.Where(t => t.Type == LevyDeclarationSagaTaskType.UpdateAccountTransactionBalances));
            }
        }

        private void UpdateStage1Progress(IEnumerable<LevyDeclarationSagaTask> tasks)
        {
            foreach (var task in tasks)
            {
                if (task.Finished != null)
                {
                    ImportPayeSchemeLevyDeclarationsTasksFinishedCount++;
                }
                else if (task.Errored != null)
                {
                    ImportPayeSchemeLevyDeclarationsTasksErroredCount++;
                }
            }
            
            Updated = DateTime.UtcNow;

            if (IsStage1Complete)
            {
                Publish(() => new UpdatedLevyDeclarationSagaProgressEvent(Id));
            }

            EnsureImportPayeSchemeLevyDeclarationsTaskCountsBalance();
        }

        private void UpdateStage2Progress(IEnumerable<LevyDeclarationSagaTask> tasks)
        {
            foreach (var task in tasks)
            {
                if (task.Finished != null)
                {
                    UpdateAccountTransactionBalancesTasksFinishedCount++;
                }
                else if (task.Errored != null)
                {
                    UpdateAccountTransactionBalancesTasksErroredCount++;
                }
            }
            
            Updated = DateTime.UtcNow;

            if (IsStage2Complete)
            {
                IsComplete = true;

                switch (Type)
                {
                    case LevyDeclarationSagaType.Planned:
                        Publish(() => new FinishedProcessingLevyDeclarationsEvent(Id, PayrollPeriod, AccountPayeSchemeHighWaterMarkId.Value, Updated.Value));
                        break;
                    case LevyDeclarationSagaType.AdHoc:
                        Publish(() => new FinishedProcessingLevyDeclarationsAdHocEvent(Id, PayrollPeriod, AccountPayeSchemeId.Value, Updated.Value));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            EnsureUpdateAccountTransactionBalancesTaskCountsBalance();
        }

        private void EnsureImportPayeSchemeLevyDeclarationsTaskCountsBalance()
        {
            if (ImportPayeSchemeLevyDeclarationsTasksFinishedCount + ImportPayeSchemeLevyDeclarationsTasksErroredCount > ImportPayeSchemeLevyDeclarationsTasksCount)
            {
                throw new InvalidOperationException($"Requires {nameof(LevyDeclarationSagaTaskType.ImportPayeSchemeLevyDeclarations)} task counts balance");
            }
        }

        private void EnsureUpdateAccountTransactionBalancesTaskCountsBalance()
        {
            if (UpdateAccountTransactionBalancesTasksFinishedCount + UpdateAccountTransactionBalancesTasksErroredCount > UpdateAccountTransactionBalancesTasksCount)
            {
                throw new InvalidOperationException($"Requires {nameof(LevyDeclarationSagaTaskType.UpdateAccountTransactionBalances)} task counts balance");
            }
        }
    }
}