using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Web.Cookies;

namespace SFA.DAS.EmployerFinance.Web.Startup
{
    public static class AuthenticationStartup
    {
        public static IServiceCollection AddDasOidcAuthentication(this IServiceCollection services, IOidcConfiguration oidcConfiguration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services
                .AddAuthentication(o =>
                {
                    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    o.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie(o =>
                {
                    o.AccessDeniedPath = "/403.html";
                    o.Cookie.Name = CookieNames.Authentication;
                    //todo: see if this fixes auth on Safari
                    o.Cookie.SameSite = SameSiteMode.None;
                    o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    o.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    o.SlidingExpiration = true;
                })
                .AddOpenIdConnect(o =>
                {
                    o.Authority = oidcConfiguration.Authority;
                    o.ClientId = oidcConfiguration.ClientId;
                    o.ClientSecret = oidcConfiguration.ClientSecret;
                    o.MetadataAddress = oidcConfiguration.MetadataAddress;
                    o.ResponseType = "code";
    
                    o.ClaimActions.MapUniqueJsonKey("sub", "id");
                    
                    o.Events.OnRemoteFailure = c =>
                    {
                        if (c.Failure.Message.Contains("Correlation failed"))
                        {
                            // TODO: Logging
                            
                            c.Response.Redirect("/"); // TODO: Confirm correlation failure behaviour
                            c.HandleResponse();
                        }
    
                        return Task.CompletedTask;
                    };
                });
            
            return services;
        }
        
        public static void RequireAuthenticatedUser(this MvcOptions mvcOptions)
        {
            var authorizationPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            var authorizeFilter = new AuthorizeFilter(authorizationPolicy);
            
            mvcOptions.Filters.Add(authorizeFilter);
        }
    }
}