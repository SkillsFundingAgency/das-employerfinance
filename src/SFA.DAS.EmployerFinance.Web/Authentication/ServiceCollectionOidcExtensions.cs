using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Web.Cookies;

namespace SFA.DAS.EmployerFinance.Web.Authentication
{
    public static class ServiceCollectionOidcExtensions
    {
        public static IServiceCollection AddOidcAuthentication(this IServiceCollection services, IOidcConfiguration oidcConfig, bool isDevelopment)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                if (!isDevelopment)
                {
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                }

                //todo: once in same branch as error handling, point to forbidden error page
                options.AccessDeniedPath = "/Error/403";
                options.Cookie.Name = CookieNames.Authentication;
            })
            .AddOpenIdConnect(options =>
            {
                options.Authority = oidcConfig.Authority;
                options.MetadataAddress = oidcConfig.MetadataAddress;
                options.ResponseType = "code";
                options.ClientId = oidcConfig.ClientId;
                options.ClientSecret = oidcConfig.ClientSecret;

                options.ClaimActions.MapUniqueJsonKey("sub", "id");
                
                options.Events.OnRemoteFailure = ctx =>
                {
                    if (ctx.Failure.Message.Contains("Correlation failed"))
                    {
                        //todo: logging
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