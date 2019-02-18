CREATE TABLE [dbo].[Payes]
(
  [EmployerReferenceNumber] NVARCHAR(16) NOT NULL PRIMARY KEY,
  [AccessToken] VARCHAR(50) NULL,
  [RefreshToken] VARCHAR(50) NULL,
  [Name] VARCHAR(500) NULL
)
