#Not currently compatible with Powershell Core (https://stackoverflow.com/questions/52114946/is-it-possible-to-install-the-pki-module-on-powershell-core)
#Use legacy Powershell instead 
#Requires -RunAsAdministrator

$certpwd = ConvertTo-SecureString -String password -Force -AsPlainText

dotnet dev-certs https --trust

#pre core localhost cert install...
#Import-PfxCertificate -FilePath $PSScriptRoot\localhost.pfx -CertStoreLocation cert://LocalMachine/My -Password $certpwd -Exportable
#Import-PfxCertificate -FilePath $PSScriptRoot\localhost.pfx -CertStoreLocation cert://LocalMachine/Root -Password $certpwd -Exportable

Import-PfxCertificate -FilePath $PSScriptRoot\DasAmlCert.pfx -CertStoreLocation cert://LocalMachine/My -Password $certpwd -Exportable
Import-PfxCertificate -FilePath $PSScriptRoot\DasAmlCert.pfx -CertStoreLocation cert://LocalMachine/Root -Password $certpwd -Exportable

$idppwd = ConvertTo-SecureString -String idsrv3test -Force -AsPlainText

Import-PfxCertificate -FilePath $PSScriptRoot\DasIDPCert.pfx -CertStoreLocation cert://CurrentUser/My -Password $idppwd -Exportable