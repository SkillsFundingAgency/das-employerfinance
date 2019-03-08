using System;

namespace SFA.DAS.EmployerFinance.Models
{
    public class LevyDeclarationSagaTask : Entity
    {
        public virtual long Id { get; private set; }
        public virtual LevyDeclarationSaga Saga { get; private set; }
        public virtual int SagaId { get; private set; }
        public virtual LevyDeclarationSagaTaskType Type { get; private set; }
        public virtual AccountPayeScheme AccountPayeScheme { get; private set; }
        public virtual long? AccountPayeSchemeId { get; private set; }
        public virtual Account Account { get; private set; }
        public virtual long? AccountId { get; private set; }
        public virtual DateTime Started { get; private set; }
        public virtual DateTime? Finished { get; private set; }

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
        }

        internal LevyDeclarationSagaTask()
        {
        }

        public virtual void Start()
        {
            Started = DateTime.UtcNow;
        }

        public virtual void Finish()
        {
            Finished = DateTime.UtcNow;
        }
    }
}