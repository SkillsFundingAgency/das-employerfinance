CREATE TABLE [dbo].[PayeSchemes]
(
  [Id] BIGINT NOT NULL,
  [EmployerReferenceNumber] VARCHAR(16) NOT NULL,
  [Name] NVARCHAR(60) NULL, -- todo: check source for >60
  [Created] DATETIME2 NOT NULL,
  [Updated] DATETIME2 NULL,
  [Deleted] DATETIME2 NULL,
  CONSTRAINT [PK_PayeSchemes] PRIMARY KEY CLUSTERED ([Id] ASC),
  CONSTRAINT [AK_PayeSchemes_EmployerReferenceNumber] UNIQUE ([EmployerReferenceNumber] ASC)
)
