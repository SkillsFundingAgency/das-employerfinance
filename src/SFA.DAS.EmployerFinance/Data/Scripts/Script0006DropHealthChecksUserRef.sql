DROP INDEX [dbo].[HealthChecks].[IX_HealthChecks_UserRef]
ALTER TABLE [dbo].[HealthChecks] DROP CONSTRAINT [FK_HealthChecks_Users_UserRef]
ALTER TABLE [dbo].[HealthChecks] DROP COLUMN [UserRef]