using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerFinance.Configuration;

namespace SFA.DAS.EmployerFinance.Web.Authentication
{
    //todo: options.ClaimActions.MapUniqueJsonKey("sub", "id");
    //todo: move to authentication/extensions? rename ServiceCollectionOidcExtensions?
    //todo: looks might be able to play with the cookie domain, path etc. and have 1 cookie, so any sub-site auto signs out of main site! see https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-2.2
    //todo: ios on safari and same-site cookies, see https://brockallen.com/2019/01/11/same-site-cookies-asp-net-core-and-external-authentication-providers/
    //looks like das-recruit followed this (or earlier guide)... https://identityserver4.readthedocs.io/en/latest/quickstarts/3_interactive_login.html
    public static class ServiceCollectionExtensions
    {
        //https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/additional-claims?view=aspnetcore-2.2
        // reservations hooks into OnTokenValidated, example uses OnPostConfirmationAsync
        public static IServiceCollection AddAndConfigureAuthentication(this IServiceCollection services, IHostingEnvironment hostingEnvironment, IOidcConfiguration oidcConfig)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                if (!hostingEnvironment.IsDevelopment())
                {
                    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); //AuthenticationConfiguration.SessionTimeoutMinutes);
                }

                //todo:
                options.AccessDeniedPath = "/Error/403";
            })
            .AddOpenIdConnect(options =>
            {
                options.Authority = oidcConfig.Authority;
                options.MetadataAddress = oidcConfig.MetadataAddress;
                options.ResponseType = "code";
                options.ClientId = oidcConfig.ClientId;
                options.ClientSecret = oidcConfig.ClientSecret;

                //is this required? try with JwtSecurityTokenHandler.InboundClaimTypeMap.Clear()
                options.ClaimActions.MapUniqueJsonKey("sub", "id");
                
                options.Events.OnRemoteFailure = ctx =>
                {
                    if (ctx.Failure.Message.Contains("Correlation failed"))
                    {
//                        var logger = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>().CreateLogger<Startup>();
//                        logger.LogDebug("Correlation Cookie was invalid - probably timed-out");

                        //todo: what's our homepage? inject EmployerUrls? redirect back to login page with message along the lines of 'for your security you must log in again if you spend too long on the login page'
                        ctx.Response.Redirect("/");
                        ctx.HandleResponse();
                    }

                    return Task.CompletedTask;
                };
            });
            
            return services;
        }
    }
}