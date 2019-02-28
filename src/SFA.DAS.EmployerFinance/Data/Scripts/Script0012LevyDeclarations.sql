CREATE TABLE [dbo].[LevyDeclarations]
(
  [Id] BIGINT NOT NULL IDENTITY,
  [TransactionId] BIGINT NOT NULL,
  [HmrcSubmissionId] INT NOT NULL, ---todo: check type
  [YearToDate] MONEY NOT NULL,
  CONSTRAINT [PK_LevyDeclarations] PRIMARY KEY CLUSTERED ([Id] ASC),
  CONSTRAINT [FK_LevyDeclarations_Transactions_Id] FOREIGN KEY ([TransactionId]) REFERENCES [Transactions] ([Id]),
  CONSTRAINT [UK_LevyDeclarations_HmrcSubmissionId] UNIQUE ([HmrcSubmissionId] ASC)
)