using System;
using SFA.DAS.EmployerFinance.Extensions;

namespace SFA.DAS.EmployerFinance.Models
{
    public class LevyDeclarationSagaTask : Entity
    {
        public long Id { get; private set; }
        public LevyDeclarationSaga Saga { get; private set; }
        public int SagaId { get; private set; }
        public LevyDeclarationSagaTaskType Type { get; private set; }
        public AccountPayeScheme AccountPayeScheme { get; private set; }
        public long? AccountPayeSchemeId { get; private set; }
        public Account Account { get; private set; }
        public long? AccountId { get; private set; }
        public DateTime Started { get; private set; }
        public DateTime? Finished { get; private set; }
        public DateTime? Errored { get; private set; }
        public string ErrorMessage { get; private set; }

        public static LevyDeclarationSagaTask CreateImportPayeSchemeLevyDeclarationsTask(int sagaId, long accountPayeSchemeId)
        {
            return new LevyDeclarationSagaTask(sagaId, LevyDeclarationSagaTaskType.ImportPayeSchemeLevyDeclarations, accountPayeSchemeId, null);
        }
        
        public static LevyDeclarationSagaTask CreateUpdateAccountTransactionBalancesTask(int sagaId, long accountId)
        {
            return new LevyDeclarationSagaTask(sagaId, LevyDeclarationSagaTaskType.UpdateAccountTransactionBalances, null, accountId);
        }
        
        private LevyDeclarationSagaTask(int sagaId, LevyDeclarationSagaTaskType type, long? accountPayeSchemeId, long? accountId)
        {
            SagaId = sagaId;
            Type = type;
            AccountPayeSchemeId = accountPayeSchemeId;
            AccountId = accountId;
            Started = DateTime.UtcNow;
        }

        private LevyDeclarationSagaTask()
        {
        }

        public void Finish()
        {
            EnsureNotFinished();
            EnsureNotErrored();
            
            Finished = DateTime.UtcNow;
        }

        public void Error(Exception ex)
        {
            EnsureNotFinished();
            EnsureNotErrored();
            
            Errored = DateTime.UtcNow;
            ErrorMessage = ex.GetAggregateMessage();
        }

        private void EnsureNotFinished()
        {
            if (Finished != null)
            {
                throw new InvalidOperationException("Requires task is not finished");
            }
        }

        private void EnsureNotErrored()
        {
            if (Errored != null)
            {
                throw new InvalidOperationException("Requires task has not errored");
            }
        }
    }
}