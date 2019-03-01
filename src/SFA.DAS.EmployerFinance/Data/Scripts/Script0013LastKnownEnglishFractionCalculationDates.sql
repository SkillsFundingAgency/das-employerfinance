CREATE TABLE [dbo].[LastKnownEnglishFractionCalculationDates]
(
  [Id] BIGINT NOT NULL IDENTITY,
  [EmployerReferenceNumber] VARCHAR(16) NOT NULL,
  [LastKnownCalculationDate] DATE NULL,
  CONSTRAINT [PK_LastKnownEnglishFractionCalculationDates] PRIMARY KEY CLUSTERED ([Id] ASC),
  CONSTRAINT [UK_LastKnownEnglishFractionCalculationDates_EmployerReferenceNumber] UNIQUE ([EmployerReferenceNumber] ASC)
)