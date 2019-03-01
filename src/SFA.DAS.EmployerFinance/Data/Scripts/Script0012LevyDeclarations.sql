CREATE TABLE [dbo].[LevyDeclarations]
(
  --todo: add some of these comment to PR?

  [Id] BIGINT NOT NULL IDENTITY,
  -- nullable: suggest we add LevyDeclarations row for every levy dec received from hmrc,
  -- but only add a transaction row if the id supplied by hmrc ends in 2 (when a valid payrollPeriod and levyDueYTD should be supplied)
  -- (from hmrc id doc: Taking this identifier modulo 10 gives the type of entry: 0, no entry; 1, inactive; 2, levy declaration; 3, ceased.)
  [TransactionId] BIGINT NULL,
  -- optional A unique identifier for the declaration. This will remain consistent from one call to the API to the next so that the client can identify declarations they’ve already retrieved. It is the identifier assigned by the RTI system to the EPS return, so it is possible to cross-reference with HMRC if needed. Dividing this identifier by 10 (ignoring the remainder) gives the identifier assigned by the RTI system to the EPS return, so it is possible to cross-reference with HMRC if needed. Taking this identifier modulo 10 gives the type of entry: 0, no entry; 1, inactive; 2, levy declaration; 3, ceased.
  -- our options: store it as we receive it
  -- split it into 2, EPS return id + Entry Type
  -- store it as we receive it + Entry Type
  --[HmrcId] BIGINT NOT NULL,
  [EpsSubmissionId] INT NULL, -- docs don't say if INT/BIGINT
  -- 0, no entry; 1, inactive; 2, levy declaration; 3, ceased
  [Type] TINYINT NULL,
  --[HmrcSubmissionId] BIGINT NOT NULL,
  -- The time at which the EPS submission that this declaration relates to was received by HMRC. If the backend systems return a bad date that can not be handled this will be set to 1970-01-01T01:00:00.000
  -- we could set this to null if 1970-01-01T01:00:00.000 is supplied. +ve no need for special check against magic value, -ve can't distinguish from db whether we didn't receive this value, or the magic date
  -- q: do we want to see exactly what came from hmrc by looking in the db, or do we use log and/or audit facility for that?
  [SubmissionTime] DATETIME2 NULL,                  
  [EmployerReferenceNumber] VARCHAR(16) NULL,
  [LevyDueYearToDate] MONEY NULL, -- use decimal to reduce space requirements?
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

/*
namespace HMRC.ESFA.Levy.Api.Types
{
    public class Declaration
    {
        /// <summary>
        /// A unique identifier for the declaration. This will remain consistent from one call to the API to the next so that the client can identify declarations they’ve already retrieved. It is the identifier assigned by the RTI system to the EPS return, so it is possible to cross-reference with HMRC if needed. Dividing this identifier by 10 (ignoring the remainder) gives the identifier assigned by the RTI system to the EPS return, so it is possible to cross-reference with HMRC if needed. Taking this identifier modulo 10 gives the type of entry: 0, no entry; 1, inactive; 2, levy declaration; 3, ceased.
        /// </summary>
        public string Id { get; set; }

        public long SubmissionId { get; set; }

        /// <summary>
        /// If present, indicates the date that the payroll scheme was ceased.
        /// </summary>
        public DateTime? DateCeased { get; set; }

        /// <summary>
        /// Indicates the the payroll scheme will be inactive starting from this date. Should always be the 6th of the month of the first inactive payroll period.
        /// </summary>
        public DateTime? InactiveFrom { get; set; }

        /// <summary>
        /// The date after which the payroll scheme will be active again. Should always be the 5th of the month of the last inactive payroll period.
        /// </summary>
        public DateTime? InactiveTo { get; set; }

        /// <summary>
        /// If present, will always have the value true and indicates that no declaration was necessary for this period. This can be interpreted to mean that the YTD levy balance is unchanged from the previous submitted value.
        /// </summary>
        public bool NoPaymentForPeriod { get; set; }

        /// <summary>
        /// The time at which the EPS submission that this declaration relates to was received by HMRC. If the backend systems return a bad date that can not be handled this will be set to 1970-01-01T01:00:00.000.
        /// </summary>
        public DateTime SubmissionTime { get; set; }

        public PayrollPeriod PayrollPeriod { get; set; }

        /// <summary>
        /// The amount of apprenticeship levy that was declared in the payroll month.
        /// </summary>
        [JsonProperty("levyDueYTD")]
        public decimal LevyDueYearToDate { get; set; }

        /// <summary>
        /// The annual amount of apprenticeship levy allowance that has been allocated to this payroll scheme. If absent then the value can be taken as 0. The maximum value in the 2017/18 will be 15,000.
        /// </summary>
        public decimal LevyAllowanceForFullYear { get; set; }

        /// <summary>
        /// Each LevyDeclaration is either standard, LastPaymentBeforeCutoff, or LatePayment (after Cutoff)
        /// </summary>
        public LevyDeclarationSubmissionStatus LevyDeclarationSubmissionStatus { get; set; }
    }
 */
 
 /*
Data from hmrc...

empref        string  optional	The PAYE Reference for the employer. This will be the same as provided in the URL. For example: 123/AB12345.
declarations            array   optional	
    inactiveFrom        string  optional	Indicates the the payroll scheme will be inactive starting from this date. Should always be the 6th of the month of the first inactive payroll period. Must conform to the regular expression ^\d{4}-\d{2}-06$ For example: 2016–06–06.
    submissionTime      string  optional	The time at which the EPS submission that this declaration relates to was received by HMRC. If the backend systems return a bad date that can not be handled this will be set to 1970-01-01T01:00:00.000. For example: 2016–02–21T16:05:23.000.000.
    payrollPeriod       object  optional	
        year            string  optional	The tax year of the payroll period against which the declaration was made. For example: 15-16.
        month           number  optional	The tax month of the payroll period against which the declaration was made. Month 1 is April. For example: 1.
    allowance           number  optional	The annual amount of apprenticeship levy allowance that has been allocated to this payroll scheme. If absent then the value can be taken as 0. The maximum value in the 2017/18 will be 15,000. For example: 15000.
    noPaymentForPeriod  boolean optional	If present, will always have the value true and indicates that no declaration was necessary for this period. This can be interpreted to mean that the YTD levy balance is unchanged from the previous submitted value. For example: true.
    inactiveTo          string  optional	The date after which the payroll scheme will be active again. Should always be the 5th of the month of the last inactive payroll period. Must conform to the regular expression ^\d{4}-\d{2}-05$ For example: 2016–09–05.
    dateCeased          string  optional	If present, indicates the date that the payroll scheme was ceased. Must conform to the regular expression ^\d{4}-\d{2}-\d{2}$ For example: 2016–03–17.
    id                  number  optional	A unique identifier for the declaration. This will remain consistent from one call to the API to the next so that the client can identify declarations they’ve already retrieved. It is the identifier assigned by the RTI system to the EPS return, so it is possible to cross-reference with HMRC if needed. Dividing this identifier by 10 (ignoring the remainder) gives the identifier assigned by the RTI system to the EPS return, so it is possible to cross-reference with HMRC if needed. Taking this identifier modulo 10 gives the type of entry: 0, no entry; 1, inactive; 2, levy declaration; 3, ceased. For example: 12345684.
    levyDueYTD          number  optional	The amount of apprenticeship levy that was declared in the payroll month. For example: 600.20.
 */

/*
Levy Table..

Fields	Definition 	Source	Note
Account ID 	Identity of the employer account	MA	
empRef	Reference number of the PAYE scheme	MA	
ID	A unique identifier for the declaration. 	HMRC	
Submission ID 	HMRC ascribed submission ID	HMRC	
Submission date time 	Date and time of the submission by the employer to HMRC	HMRC	
PayrollYear	Financial year eg 17-18	HMRC	
Payroll Month	The period the PAYE submission releates to 	HMRC	
End of year adjustment	Identifies an end of year adjustment 	HMRC	
EndOfYearAdjustmentAmount	YTD value for the month 12 	HMRC	
LevyDueYTD 	The value of the PAYE schemes pay bill month for that financial year April / March	MA	
English % 	The English % of the pay bill that is paid to English residents	MA	New  - not sure why the EF is not in the current Levy table
CreatedDate	Date Your funds enter into the account	MA	
DateCeased	Date a PAYE is ceased	HMRC	
InactiveFrom	Indicates the the payroll scheme will be inactive starting from this date. Should always be the 6th of the month of the first inactive payroll period.	HMRC	
InactiveTo	The date after which the payroll scheme will be active again. Should always be the 5th of the month of the last inactive payroll period	HMRC	
HmrcSubmissionId	Same as Submission ID	HMRC	
NoPaymentForPeriod	If present, will always have the value true and indicates that no declaration was necessary for this period. This can be interpreted to mean that the YTD levy balance is unchanged from the previous submitted value.	HMRC	
LevyAllowanceForYear	The value of the levy allowance below which you do not paye the levy - currently £15,000	HMRC	

 */
 
 /*
 Transactions table..
 
 Fields	Definition 	Source	
Id	HMRC unique identifier for the submission	HMRC	
AccountId	Identifier of the employers account	MA	
DateCreated	Date entered into the acocunt	MA	
SubmissionId	Same as HMRC submissionID	HMRC	
TransactionDate	Same as submission date	MA	
TransactionType	Tbc	MA	
LevyDeclared	Monthly amount declared 	MA	
EnglishFraction	The English % of the pay bill that is paid to English residents	MA	
English Levy	The Levy declared after multiplication with the English percentage. 	MA	New
Top up 	10% of the value of the Levy after EF	MA	New
Amount	The amount entering the account after the EF and 10% top up "Your funds" in UI.	MA	
Status 	Whether a submission is Accepted / Superseded / Late	MA	New
EmpRef	PAYE reference code	MA	
PeriodEnd		MA	
Ukprn	Provider Identifier	MA	
SfaCoInvestmentAmount	Amount or provider payment funded by government when there's no levy	MA	
EmployerCoInvestmentAmount	Amount or provider payment funded by the employer when there's no levy	MA	
TransferSenderAccountId	Identifier of the sending employer	MA	
TransferSenderAccountName	Name of the sending employer's account	MA	
TransferReceiverAccountId	Identifier of the receiving employer	MA	
TransferReceiverAccountName	Name of the receiving employer's account	MA	
  */