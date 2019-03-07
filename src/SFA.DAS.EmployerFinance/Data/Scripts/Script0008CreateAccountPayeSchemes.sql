CREATE TABLE [dbo].[AccountPayeSchemes]
(
    [Id] BIGINT NOT NULL IDENTITY,
    [AccountId] BIGINT NOT NULL,
    [EmployerReferenceNumber] VARCHAR(16) NOT NULL,
    [Created] DATETIME2 NOT NULL,
    [Deleted] DATETIME2 NULL,
    CONSTRAINT [PK_AccountPayeSchemes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AccountPayeSchemes_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]),
    CONSTRAINT [UK_AccountPayeSchemes_AccountId_EmployerReferenceNumber] UNIQUE ([AccountId] ASC, [EmployerReferenceNumber] ASC),
    INDEX [IX_AccountPayeSchemes_AccountId] NONCLUSTERED ([AccountId] ASC)
)
GO

IF (
    @@servername NOT LIKE '%-at-%' AND
    @@servername NOT LIKE '%-test-%' AND
    @@servername NOT LIKE '%-test2-%' AND
    @@servername NOT LIKE '%-pp-%' AND
    @@servername NOT LIKE '%-prd-%' AND
    @@servername NOT LIKE '%-mo-%' AND
    @@servername NOT LIKE '%-demo-%' AND
    NOT EXISTS (SELECT 1 FROM [dbo].[AccountPayeSchemes] WHERE Id = 1)
)
BEGIN
    SET IDENTITY_INSERT [dbo].[AccountPayeSchemes] ON
    INSERT INTO [dbo].[AccountPayeSchemes] ([Id], [AccountId], [EmployerReferenceNumber], [Created]) VALUES (1, 1, '222/ZZ00002', GETUTCDATE())
    SET IDENTITY_INSERT [dbo].[AccountPayeSchemes] OFF
END
GO