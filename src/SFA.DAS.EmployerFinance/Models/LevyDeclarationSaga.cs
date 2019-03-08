using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerFinance.Messages.Events;

namespace SFA.DAS.EmployerFinance.Models
{
    public class LevyDeclarationSaga : Entity
    {
        public virtual int Id { get; private set; }
        public virtual LevyDeclarationSagaType Type { get; private set; }
        public virtual DateTime PayrollPeriod { get; private set; }
        public virtual AccountPayeScheme AccountPayeSchemeHighWaterMark { get; private set; }
        public virtual long? AccountPayeSchemeHighWaterMarkId { get; private set; }
        public virtual AccountPayeScheme AccountPayeScheme { get; private set; }
        public virtual long? AccountPayeSchemeId { get; private set; }
        public virtual DateTime Created { get; private set; }
        public virtual DateTime? Updated { get; private set; }
        public virtual int ImportPayeSchemeLevyDeclarationsTasksCount { get; private set; }
        public virtual int ImportPayeSchemeLevyDeclarationsTasksCompleteCount { get; private set; }
        public virtual bool IsStage1Complete => ImportPayeSchemeLevyDeclarationsTasksCompleteCount == ImportPayeSchemeLevyDeclarationsTasksCount;
        public virtual int UpdateAccountTransactionBalancesTasksCount { get; private set; }
        public virtual int UpdateAccountTransactionBalancesTasksCompleteCount { get; private set; }
        public virtual bool IsStage2Complete => UpdateAccountTransactionBalancesTasksCompleteCount == UpdateAccountTransactionBalancesTasksCount;
        public virtual bool IsComplete { get; private set; }
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

        internal LevyDeclarationSaga()
        {
        }
        
        public virtual void UpdateProgress(IReadOnlyCollection<LevyDeclarationSagaTask> tasks)
        {
            if (IsComplete)
            {
                return;
            }

            if (!IsStage1Complete)
            {
                UpdateStage1Progress(tasks);
            }
            else
            {
                UpdateStage2Progress(tasks);
            }
        }

        private void UpdateStage1Progress(IEnumerable<LevyDeclarationSagaTask> tasks)
        {
            ImportPayeSchemeLevyDeclarationsTasksCompleteCount = tasks.Count(t => t.Type == LevyDeclarationSagaTaskType.ImportPayeSchemeLevyDeclarations);
            Updated = DateTime.UtcNow;

            if (IsStage1Complete)
            {
                Publish(() => new UpdatedLevyDeclarationSagaProgressEvent(Id));
            }

            EnsureImportPayeSchemeLevyDeclarationsTaskCountsBalance();
        }

        private void UpdateStage2Progress(IEnumerable<LevyDeclarationSagaTask> tasks)
        {
            UpdateAccountTransactionBalancesTasksCompleteCount = tasks.Count(t => t.Type == LevyDeclarationSagaTaskType.UpdateAccountTransactionBalances);
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
            if (ImportPayeSchemeLevyDeclarationsTasksCompleteCount > ImportPayeSchemeLevyDeclarationsTasksCount)
            {
                throw new InvalidOperationException($"Requires {nameof(LevyDeclarationSagaTaskType.ImportPayeSchemeLevyDeclarations)} task counts balance");
            }
        }

        private void EnsureUpdateAccountTransactionBalancesTaskCountsBalance()
        {
            if (UpdateAccountTransactionBalancesTasksCompleteCount > UpdateAccountTransactionBalancesTasksCount)
            {
                throw new InvalidOperationException($"Requires {nameof(LevyDeclarationSagaTaskType.UpdateAccountTransactionBalances)} task counts balance");
            }
        }
    }
}