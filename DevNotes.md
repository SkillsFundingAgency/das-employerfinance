# Developer Notes

## CDN

Once the cdn has gained certain features, we should be able to serve some content direct from the CDN. (We already pick up images and fonts from it.)

We need the files in the CDN to include a version in their name (or path), and we need minified versions in there. We could then serve GOV.UK's (and when it's ready common DAS and our site's specific) css and js from the CDN.

We might also want to bundle some or all of the GOV.UK / common DAS / site specific artifacts together, but only really if we can populate/update the cdn as part of the release pipeline. We should aim to not go above the [concurrent number of downloads per server](https://stackoverflow.com/questions/985431/max-parallel-http-connections-in-a-browser) for modern browsers, to keep page latency low when artifacts are not cached. To help with that aim we might want to combine GOV.UK's images into a sprite sheet.

## To Do

* GDPR / CheckConsentNeeded?
* [Scope our node package](https://docs.npmjs.com/misc/scope) to Esfa?
* Move npm & gulp configuration to the project root?
* For federated sign-out, looks like we might be able to play with the cookie domain, path etc. and have 1 cookie, so any sub-site auto signs out of main site! see https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-2.2
* authentication: ios on safari and same-site cookies, see https://brockallen.com/2019/01/11/same-site-cookies-asp-net-core-and-external-authentication-providers/
^^ check fix
* enabling debug logging in at revealed the following little gem. is it an issue?
Execution plan of action filters (in the following order): Microsoft.AspNetCore.Mvc.Internal.ControllerActionFilter (Order: -2147483648), Microsoft.AspNetCore.Mvc.ModelBinding.UnsupportedContentTypeFilter (Order: -3000), SFA.DAS.EmployerFinance.Web.Filters.UrlsViewBagFilter
* response header contains `X-Powered-By: ASP.NET`, thought we'd blatted that already

## Useful Links

* [Resolving dependencies during startup](https://stackoverflow.com/questions/32459670/resolving-instances-with-asp-net-core-di)
* [Oidc](https://identityserver4.readthedocs.io/en/latest/quickstarts/3_interactive_login.html)
