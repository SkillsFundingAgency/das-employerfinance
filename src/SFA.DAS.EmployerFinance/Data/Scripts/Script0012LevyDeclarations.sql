CREATE TABLE [dbo].[LevyDeclarations]
(
  [Id] BIGINT NOT NULL IDENTITY,
  [TransactionId] BIGINT NOT NULL,
  [HmrcSubmissionId] INT NOT NULL, ---todo: check type
  [SubmissionDate] DATETIME2 NOT NULL,
  [EmployerReferenceNumber] VARCHAR(16) NOT NULL,
  [LevyDueYearToDate] MONEY NOT NULL, -- use decimal to reduce space requirements?
  CONSTRAINT [PK_LevyDeclarations] PRIMARY KEY CLUSTERED ([Id] ASC),
  CONSTRAINT [FK_LevyDeclarations_Transactions_Id] FOREIGN KEY ([TransactionId]) REFERENCES [Transactions] ([Id]),
  CONSTRAINT [UK_LevyDeclarations_HmrcSubmissionId] UNIQUE ([HmrcSubmissionId] ASC)
)

--existing..
/*
CREATE TABLE [employer_financial].[LevyDeclaration]
(
  [Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
  [AccountId] BIGINT NOT NULL DEFAULT 0,
  [EmpRef] NVARCHAR(50) NOT NULL,
  [LevyDueYTD] DECIMAL(18, 4) NULL DEFAULT 0,
  [LevyAllowanceForYear] DECIMAL(18, 4) NULL DEFAULT 0,
  [SubmissionDate] DATETIME NULL,
  [SubmissionId] BIGINT NOT NULL DEFAULT 0,       -- think this is the original hmrc non-unique id. do we need it now that we have the unique HmrcSubmissionId
  [PayrollYear] NVARCHAR(10) NULL,                -- do we want, for ease of display?
  [PayrollMonth] TINYINT NULL,                    -- think this will be useful, even though we could calculate this from the period column in transactions table
  [CreatedDate] DATETIME NOT NULL DEFAULT GetDate(),
  [EndOfYearAdjustment] BIT NOT NULL DEFAULT 0,
  [EndOfYearAdjustmentAmount] DECIMAL(18,4) NULL,   -- is this separate amount from amount, or can we just re-use amount?
  [DateCeased] DATETIME NULL,
  [InactiveFrom] DATETIME NULL,
  [InactiveTo] DATETIME NULL,
  [HmrcSubmissionId] BIGINT NULL,
  [NoPaymentForPeriod] BIT DEFAULT 0
)
*/