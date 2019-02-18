using System;

namespace SFA.DAS.EmployerFinance.Models
{
    public class ProcessLevyDeclarationsJobTask : Entity
    {
        public long Id { get; private set; }
        public ProcessLevyDeclarationsJob Job { get; private set; }
        public Guid JobId { get; private set; }
        public ProcessLevyDeclarationsJobTaskType Type { get; private set; }
        public AccountPayeScheme AccountPayeScheme { get; private set; }
        public long? AccountPayeSchemeId { get; private set; }
        public Account Account { get; private set; }
        public long? AccountId { get; private set; }
        public DateTime Started { get; private set; }
        public DateTime Finished { get; private set; }

        public static ProcessLevyDeclarationsJobTask CreateImportLevyDeclarationsTask(Guid jobId, long accountPayeSchemeId)
        {
            return new ProcessLevyDeclarationsJobTask(jobId, ProcessLevyDeclarationsJobTaskType.ImportLevyDeclarations, accountPayeSchemeId, null);
        }
        
        public static ProcessLevyDeclarationsJobTask CreateUpdateAccountBalanceTask(Guid jobId, long accountId)
        {
            return new ProcessLevyDeclarationsJobTask(jobId, ProcessLevyDeclarationsJobTaskType.UpdateAccountBalance, null, accountId);
        }
        
        private ProcessLevyDeclarationsJobTask(Guid jobId, ProcessLevyDeclarationsJobTaskType type, long? accountPayeSchemeId, long? accountId)
        {
            JobId = jobId;
            Type = type;
            AccountPayeSchemeId = accountPayeSchemeId;
            AccountId = accountId;
        }

        private ProcessLevyDeclarationsJobTask()
        {
        }

        public void Start()
        {
            Started = DateTime.UtcNow;
        }

        public void Finish()
        {
            Finished = DateTime.UtcNow;
        }
    }
}