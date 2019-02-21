/*
Script to populate Account/Legal Entity data for employer finance v2 from MA source data

Instructions for use:
1. Think about obtaining a prod db backup for sourcing the data, since it kills the db
2. Turn on SQL CMD mode
3. Execute this script against MA employer_account database
4. Execute the resulting script against EmployerFinanceV2 database

Expectations:
~30 minutes on employer_account database
~20 seconds on employer finance v2 db
~15k+ Accounts created
~27k+ AccountLegalEntities created
*/

SET NOCOUNT ON

declare @MAXINSERT int = 1000 --insert values() batch size (cannot be more than 1000)

--Some table var declarations
print 'declare @Accounts table ([Id] bigint,[HashedId] char(6),[PublicHashedId] char(6),[Name] nvarchar(100),[Created] datetime2)'
print 'declare @AccountLegalEntities table ([Id] bigint,[PublicHashedId] char(6),[AccountId] bigint,[Name] nvarchar(100),[Created] datetime2)'

BEGIN TRY

  --Accounts
  select
      case (ROW_NUMBER() OVER (ORDER BY a.Id) % @MAXINSERT) when 1 then 'insert into @Accounts ([Id],[HashedId],[PublicHashedId],[Name],[Created]) values' + char(13) + char(10) else '' end +
      ' (' + convert(varchar,[Id]) + ', ' + '''' + convert(varchar,[HashedId]) + '''' + ', ' + '''' + convert(varchar,[PublicHashedId]) + '''' + ', ' + '''' + replace([Name],'''','''''') + '''' + ', ' + '''' + convert(varchar,[CreatedDate],121) + '''' + ')' +
      case when ((ROW_NUMBER() OVER (ORDER BY a.Id) % @MAXINSERT = 0) OR (ROW_NUMBER() OVER (ORDER BY a.Id) = (select count(1) from [employer_account].[Account] where HashedId is not null and PublicHashedId is not null))) then '' else ',' end
  from
    [employer_account].[Account] a
  where HashedId is not null
    and PublicHashedId is not null
  order by a.Id asc


  --AccountLegalEntities
  select
      case (ROW_NUMBER() OVER (ORDER BY ale.Id) % @MAXINSERT) when 1 then 'insert into @AccountLegalEntities ([Id],[PublicHashedId],[AccountId],[Name],[Created]) values' + char(13) + char(10) else '' end +
      ' (' + convert(varchar,ale.[Id]) + ', '
      + '''' + ale.[PublicHashedId] + '''' + ', '
      + convert(varchar,ale.[AccountId]) + ', '
      + '''' + replace(ale.[Name],'''','''''') + '''' + ','
      + '''' + convert(varchar,[Created],121) + ''''
      + ')'  +
      case when
             ((ROW_NUMBER() OVER (ORDER BY ale.Id) % @MAXINSERT = 0)
               OR (ROW_NUMBER() OVER (ORDER BY ale.Id) = (select count(1) from [employer_account].[AccountLegalEntity] where PublicHashedId is not null and Deleted is null)))
             then '' else ',' end
  from [employer_account].[AccountLegalEntity] ale
         join [employer_account].[LegalEntity] le on le.Id = ale.LegalEntityId
  where ale.PublicHashedId is not null
    and ale.Deleted is null
  order by ale.Id asc

  --Final inserts
  print '
	BEGIN TRANSACTION

	insert into Accounts ([Id], [HashedId], [PublicHashedId], [Name], [Created])
	select a.[Id], a.[HashedId], a.[PublicHashedId], a.[Name], a.[Created] from @Accounts a
	left join Accounts e on e.[Id] = a.[Id]
	where e.[Id] is null --skip existing
	print ''Inserted '' + convert(varchar,@@ROWCOUNT) + '' Accounts''
	'

  print '
	insert into AccountLegalEntities([Id],[PublicHashedId],[AccountId],[Name], [Created])
	select ale.[Id], ale.[PublicHashedId], ale.[AccountId], ale.[Name], ale.[Created] 
	from @AccountLegalEntities ale
	left join AccountLegalEntities e on e.[Id] = ale.[AccountLegalEntityId]
	where e.[Id] is null --skip existing
	print ''Inserted '' + convert(varchar,@@ROWCOUNT) + '' AccountLegalEntities''
	print ''Completed''


	COMMIT TRANSACTION
	'

END TRY
BEGIN CATCH
  PRINT 'Problem, (blank public hashed Id?)'
END CATCH
