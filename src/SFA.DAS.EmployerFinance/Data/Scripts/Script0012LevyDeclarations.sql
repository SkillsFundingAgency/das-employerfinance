CREATE TABLE [dbo].[LevyDeclarations]
(
  [Id] BIGINT NOT NULL IDENTITY,
  [TransactionId] BIGINT NOT NULL,
  [HmrcSubmissionId] BIGINT NOT NULL,
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

/*
 namespace HMRC.ESFA.Levy.Api.Types
{
  public class Declaration
  {
    public string Id { get; set; }
    public long SubmissionId { get; set; }
    public DateTime? DateCeased { get; set; }
    public DateTime? InactiveFrom { get; set; }
    public DateTime? InactiveTo { get; set; }
    public bool NoPaymentForPeriod { get; set; }
    public DateTime SubmissionTime { get; set; }
    public PayrollPeriod PayrollPeriod { get; set; }
    [JsonProperty("levyDueYTD")]
    public Decimal LevyDueYearToDate { get; set; }
    public Decimal LevyAllowanceForFullYear { get; set; }
    public LevyDeclarationSubmissionStatus LevyDeclarationSubmissionStatus { get; set; }
  }
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
