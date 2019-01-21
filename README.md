# SFA.DAS.EmployerFinance

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status/Manage%20Apprenticeships/das-employerfinance?branchName=master)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=1212?branchName=master)

#### Requirements

#### Setup

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

The `das.css` file under `wwwroot/css` is generated from the sass file `Styles\das.scss`, ~~which imports the GOV.UK Frontend's sass~~ and the `govuk-frontend.css` file is generated from `govuk-frontend.scss`, which imports the GOV.UK Frontend's sass.

To generate new versions of the css files after updating our own `Styles\das.scss` file, or updating the `govuk-frontend` node package, follow one of the following guides...

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