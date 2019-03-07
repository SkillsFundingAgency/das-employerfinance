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
* from das-employerusers-web (AT):

Invalid redirect_uri: https://financev2.at-eas.apprenticeships.sfa.bis.gov.uk/signin-oidc
 {
  "ClientId": "employerfinancev2",
  "ClientName": "Employer Finance V2",
  "RedirectUri": "https://financev2.at-eas.apprenticeships.sfa.bis.gov.uk/signin-oidc",
  "AllowedRedirectUris": [
    "https://financev2.at-eas.apprenticeships.education.gov.uk"
  ],
  "SubjectId": "unknown",
  "Flow": "AuthorizationCode",
  "RequestedScopes": "",
  "Raw": {
    "client_id": "employerfinancev2",
    "redirect_uri": "https://financev2.at-eas.apprenticeships.sfa.bis.gov.uk/signin-oidc",
    "response_type": "code",
    "scope": "openid profile",
    "response_mode": "form_post",
    "nonce": "636875632222062709.ODhmMDgyYjUtMDliMC00NjE0LThiNjYtZGQ3ZWZiNGE4ZGIwMzQ2YmFkOTQtMjc3OS00Mjc1LTgxMjctNTg5ZTBmODlkMzQ4",
    "state": "CfDJ8INi49ZQOVtDicN2oXvE77IZ4XPfxMfpLruPI-kgI-oI48P_7Mtbp6jRlJIUXXtQQgR4CpKjIDUKX3LQUd7B17OvxSO5pm94ZIOvx0pNVwyJO-lvTtG5kZeIByI5hVuc6krDGH8W18C0DdiM1suwDH9GG2MdPThuSOEAO3voWJJA1T3JwqGBWecFDontXtaWBl8dKDn9GXt4xAccPGXewpz6X6g-8LEJzdsjK1JORP18xkUJ4i0exux9BOpOcGbcGdWgcdmyZ-2u7mjV5u3ff_NRPORQTijsG025i7QO0k-7Pm1yBDmPYvR1kQH0wPswbVH2Upias02oO-WhR8VCyx7nCzZdEOrRNJZonf1Pz2a74kj0JJFAQUiGV2av-iuQ9mSBYyXkZNlhyiJrmt-wF2734fHngn8dXYKAToT-zIVd",
    "x-client-SKU": "ID_NETSTANDARD2_0",
    "x-client-ver": "5.3.0.0"
  }
}



## Useful Links

* [Resolving dependencies during startup](https://stackoverflow.com/questions/32459670/resolving-instances-with-asp-net-core-di)
* [Oidc](https://identityserver4.readthedocs.io/en/latest/quickstarts/3_interactive_login.html)
