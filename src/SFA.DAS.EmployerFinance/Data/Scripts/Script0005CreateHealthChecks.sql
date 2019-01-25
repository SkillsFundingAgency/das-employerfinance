CREATE TABLE [dbo].[HealthChecks]
(
    [Id] INT NOT NULL IDENTITY,
    [UserRef] UNIQUEIDENTIFIER NOT NULL,
    [SentEmployerFinanceApiRequest] DATETIME2 NOT NULL,
    [ReceivedEmployerFinanceApiResponse] DATETIME2 NULL,
    [PublishedEmployerFinanceEvent] DATETIME2 NOT NULL,
    [ReceivedEmployerFinanceEvent] DATETIME2 NULL,
    CONSTRAINT [PK_HealthChecks] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_HealthChecks_Users_UserRef] FOREIGN KEY ([UserRef]) REFERENCES [Users] ([Ref]),
    INDEX [IX_HealthChecks_UserRef] NONCLUSTERED ([UserRef])
)