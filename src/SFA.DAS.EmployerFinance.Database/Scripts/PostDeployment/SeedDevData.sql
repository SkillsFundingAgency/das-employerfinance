IF (NOT EXISTS (SELECT 1 FROM [dbo].[Accounts] WHERE Id = 1))
BEGIN
    INSERT INTO [dbo].[Accounts] ([Id], [HashedId], [PublicHashedId], [Name], [Created]) VALUES (1, 'JRML7V', 'LDMVWV', 'Foobar', GETUTCDATE())
END