# das-employerfinance

[![Build status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status/Manage%20Apprenticeships/das-employerfinance)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=1183)

#### Requirements

[Windows]

1. Install [Visual Studio] with these workloads:
    * ASP.NET and web development
    * Azure development
    * Data storage and processing
    * .NET Core cross-platform development 
2. Install [Azure Storage Explorer] (if required for local storage access as Azure portal now has an online version)
3. Download Git and clone the project to your desired local location. You can download the zip version alternativey.


[OS X]

1. Install .Net Core (https://dotnet.microsoft.com/download)
2. Install a .Net Core IDE (i.e [Jetbrains Rider], [VS Code])
3. Install [Azure Storage Explorer] (if required for local storage access as Azure portal now has an online version)
4. Download Git and clone the project to your desired local location. You can download the zip version alternativey.


#### Setup

##### Add Environment variables

[OS X]
[Not using local azure emulator]

You will need to set the configuration storage string that is used in development as this defaults to the local azure enumlator if it is not set. To set the connection string you need to add the following to you environmental variables:

**APPSETTING_ConfigurationStorageConnectionString --> 'You connection string'**

In windows this is just the normal environmental settings. In OS X this is likely set in your IDE configuration. Jetbrains rider has this under the run time configuration settings.

