CREATE TABLE [dbo].[Transactions]
(
  [Id] BIGINT NOT NULL IDENTITY,
  [AccountId] BIGINT NOT NULL,
  [Type] TINYINT NOT NULL,
  [SubType] TINYINT NOT NULL,
  --payroll month 1 is 6 apr -> 5 may (tax year)
  --payments r01 is august (academic year)
  --return period in payments table 1-14?
  --tax month in levy dec 1-12?
  --year/month or date? probably date in transactions
  [Period] DATE NOT NULL,
  [Amount] MONEY,
  [Balance] MONEY,
  [JobId] UNIQUEIDENTIFIER,
  CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED ([Id] ASC),
  CONSTRAINT [FK_Transactions_Accounts_Id] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id])
)

/* 
existing...

CREATE TABLE [employer_financial].[TransactionLine]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    AccountId BIGINT NOT NULL,
    DateCreated DATETIME NOT NULL,
    SubmissionId BIGINT NULL,
    TransactionDate DATETIME NOT NULL,
    TransactionType TINYINT NOT NULL DEFAULT 0, 
    LevyDeclared DECIMAL(18,4) NULL, 
    Amount DECIMAL(18,4) NOT NULL DEFAULT 0, 
    EmpRef nVarchar(50) null,
    PeriodEnd nVarchar(50) null,
    Ukprn BIGINT null, 
    SfaCoInvestmentAmount DECIMAL(18, 4) NOT NULL DEFAULT 0, 
    EmployerCoInvestmentAmount DECIMAL(18, 4) NOT NULL DEFAULT 0,
    [EnglishFraction] DECIMAL(18, 5) NULL, 
    [TransferSenderAccountId] BIGINT NULL, 
    [TransferSenderAccountName] NVARCHAR(100) NULL,
    [TransferReceiverAccountId] BIGINT NULL, 
    [TransferReceiverAccountName] NVARCHAR(100) NULL
)

also, type lookup.. (required?)

CREATE TABLE [employer_financial].[TransactionLineTypes]
(	
    [TransactionType] TINYINT NOT NULL , 
    [Description] VARCHAR(100) NOT NULL, 
    CONSTRAINT [PK_TransactionLineTypes] PRIMARY KEY ([TransactionType]) 
)
 */