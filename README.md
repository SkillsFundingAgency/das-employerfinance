# SFA.DAS.EmployerFinance

todo these are different, which 1?

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status/Manage%20Apprenticeships/das-employerfinance?branchName=master)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=1212?branchName=master)
[![Build status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status/Manage%20Apprenticeships/das-employerfinance)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=1183)

#### Requirements

1. Install [Visual Studio] with these workloads:
    * ASP.NET and web development
    * Azure development
    * Data storage and processing
    * .NET Core cross-platform development 
2. Install [Azure Storage Explorer] 
3. Download [Git] and clone the project to your desired local location.

[Azure Storage Explorer]: http://storageexplorer.com
[Visual Studio]: https://www.visualstudio.com
[Git]: https://git-scm.com/

#### Setup

##### Add configuration to Azure Storage Emulator

* Clone the [das-employer-config](https://github.com/SkillsFundingAgency/das-employer-config) repository.
* Clone the [das-employer-config-updater](https://github.com/SkillsFundingAgency/das-employer-config-updater) repository.
* Run Azure Storage Emulator.
* Open the `das-employer-config-updater` solution in Visual Studio.
* Press F5 and follow the instructions to import the config from the directory that you cloned the `das-employer-config repository` to.

> The two repositories above are private. If the links appear to be dead, make sure that you're logged into GitHub with an account that has access to these i.e. that you are part of the Skills Funding Agency Team organization.

##### Add Certificates

Execute DevInstall.ps1 as an admin in a legacy Powershell console (the script is currently not compatible with Powershell Core), to import required certificates into their appropriate store locations. If a dialog prompts you whether to install the dev certificate, click 'Yes'.

#### Development Tasks
 
##### Update GOV.UK Frontend

GOV.UK Frontend is included in the project using npm. (There is also a direct reference in `_Layout.cshtml` to a GOV.UK Frontend asset stored in the CDN.)

First, check to see if there is an update available. To do this, open your favourite shell, change directory to the web project, then execute...

`npm outdated govuk-frontend`

If an update is available, you'll see something like this...

```
Package         Current  Wanted  Latest  Location
govuk-frontend    2.5.0   2.5.1   2.5.1  asp.net
``` 

If no update is available, the command will complete silently.

To update the package, run

`npm update govuk-frontend`

You'll then need to

* regenerate our css file (if you haven't set up your IDE to generate it automatically, you'll need to do this manually, see 'Compile Sass' below)
* run the batch file 'copy-govuk-frontend-js.bat' in the web directory
* build the project (this will incorporate the new css and js into our bundles, and then minify them)

##### Compile Sass

The `das.css` file under `wwwroot/css` is generated from the sass file `content\styles\das.scss`, ~~which imports the GOV.UK Frontend's sass~~ and the `govuk-frontend.css` file is generated from `govuk-frontend.scss`, which imports the GOV.UK Frontend's sass.

To generate new versions of the css files after updating our own `content\styles\das.scss` file, or updating the `govuk-frontend` node package, follow one of the following guides...

###### Visual Studio 2019

In the `SFA.DAS.EmployerFinance.Web` project,

* right click `package.json` and select `Restore Packages`
* right click `gulpfile.js` and select `Task Runner Explorer`

Then, in the Task Runner Explorer window, either double-click on `sass`, or right click `sass`, and select `Run`.

###### Jetbrains Rider

In the `SFA.DAS.EmployerFinance.Web` project,

* right click `package.json` and select `Tools` > `Run 'npm install'`
* right click `gulpfile.js` and select `Tools` > `Show Gulp Tasks`

Then, in the Gulp window that appears, either double-click on `sass`, or right click `sass`, and select `Run sass`.


### OS X Differences

#### Requirements

1. Install [.Net Core]
2. Install a .Net Core IDE (i.e [Jetbrains Rider], [VS Code])

[.Net Core]: https://dotnet.microsoft.com/download
[Jetbrains Rider]: https://www.jetbrains.com/rider
[VS Code]: https://code.visualstudio.com/

#### Setup

##### Add Environment variables

As OS X doesn't have an azure emulator you will need to set the configuration storage string to point to your external azure storage. To set this string just create the following environmental variable:

**APPSETTING_ConfigurationStorageConnectionString --> 'You connection string'**

Your IDE configuration will likely be the place to set any environmental variables. Jetbrains rider has this under the run time configuration settings.

##### Add configuration to Azure Storage Emulator

Currently the das-employer-config-updater does not run under .net core so you will need to import the config values manually.
