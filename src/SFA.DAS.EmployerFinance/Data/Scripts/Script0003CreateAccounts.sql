CREATE TABLE [dbo].[Accounts]
(                                           
    [Id] BIGINT NOT NULL,
    [HashedId] CHAR(6) NOT NULL,
    [PublicHashedId] CHAR(6) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    [Created] DATETIME2 NOT NULL,
    [Updated] DATETIME2 NULL,
    CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED ([Id] ASC)
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
    NOT EXISTS (SELECT 1 FROM [dbo].[Accounts] WHERE Id = 1)
)
BEGIN
  INSERT INTO [dbo].[Accounts] ([Id], [HashedId], [PublicHashedId], [Name], [Created]) VALUES (1, 'JRML7V', 'LDMVWV', 'Foobar', GETUTCDATE())
END
GO