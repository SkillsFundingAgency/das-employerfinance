# das-employerfinance

[![Build status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status/Manage%20Apprenticeships/das-employerfinance)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=1183)

#### Requirements

1. Install [Visual Studio] with these workloads:
    * ASP.NET and web development
    * Azure development
    * Data storage and processing
    * .NET Core cross-platform development 
2. Install [Azure Storage Explorer] (if required for local storage access as Azure portal now has an online version)
3. Download Git and clone the project to your desired local location. You can download the zip version alternativey.

[Azure Storage Explorer]: http://storageexplorer.com
[Visual Studio]: https://www.visualstudio.com

#### Setup

##### Add configuration to Azure Storage Emulator

* Clone the [das-employer-config](https://github.com/SkillsFundingAgency/das-employer-config) repository.
* Clone the [das-employer-config-updater](https://github.com/SkillsFundingAgency/das-employer-config-updater) repository.
* Run Azure Storage Emulator.
* Open the `das-employer-config-updater` solution in Visual Studio.
* Press F5 and follow the instructions to import the config from the directory that you cloned the `das-employer-config repository` to.

> The two repositories above are private. If the links appear to be dead make sure that you are logged into GitHub with an account that has access to these i.e. that you are part of the Skills Funding Agency Team organization.



### OS X Differences

#### Requirements

1. Install [.Net Core] ()
2. Install a .Net Core IDE (i.e [Jetbrains Rider], [VS Code])

[.Net Core]: https://dotnet.microsoft.com/download
[Jetbrains Rider]: https://www.jetbrains.com/rider
[VS Code]: https://code.visualstudio.com/

#### Setup

##### Add Environment variables

As OS X doesn't have an azure emmulator you will need to set the configuration storage string to point to your external azure storage. To set this string just create the following environmental variable:

**APPSETTING_ConfigurationStorageConnectionString --> 'You connection string'**

Your IDE configuration will likely be the place to set any environmental variables. Jetbrains rider has this under the run time configuration settings.


##### Add configuration to Azure Storage Emulator

Currently the das-employer-config-updater does not run under .net core so you will need to import the config values manually
