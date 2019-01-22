using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.EmployerFinance.Web.Startup
{
#if not_ready_yet
    public static class ServiceCollectionExtensions
    {
        //todo: needs oidc config. config content needs to change. plug into .net core's IConfiguration, rather than autoconfig?
        //todo: use dans azure storage configuration provider (could potentially add the autoconfig ability into it to pick up the connection string from the env variable (das-reservations)
        public static void AddAuthenticationService(this IServiceCollection services, IHostingEnvironment hostingEnvironment)
            //, AuthenticationConfiguration authConfig, IEmployerVacancyClient vacancyClient, IRecruitVacancyClient recruitClient, IHostingEnvironment hostingEnvironment)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme; //todo: das-recruit uses "oidc", AuthenticationScheme == "OpenIdConnect"
            })
//            .AddCookie("Cookies", options =>
//why are we supplying AuthenticationScheme again, use other overload?
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                //options.Cookie.Name = CookieNames.RecruitData;

                if (!hostingEnvironment.IsDevelopment())
                {
                    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); //AuthenticationConfiguration.SessionTimeoutMinutes);
                }

                //todo:
                options.AccessDeniedPath = "/Error/403";
            })
//why are we supplying AuthenticationScheme again, use other overload?
            //.AddOpenIdConnect("oidc", options =>
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.SignInScheme = "Cookies";

                options.Authority = authConfig.Authority;
                options.MetadataAddress = authConfig.MetaDataAddress;
                options.RequireHttpsMetadata = false;
                options.ResponseType = "code";
                options.ClientId = authConfig.ClientId;
                options.ClientSecret = authConfig.ClientSecret;
                options.Scope.Add("profile");

                options.Events.OnTokenValidated = async (ctx) =>
                {
                    await PopulateAccountsClaim(ctx, vacancyClient);
                    await HandleUserSignedIn(ctx, recruitClient);
                };

                options.Events.OnRemoteFailure = ctx =>
                {
                    if (ctx.Failure.Message.Contains("Correlation failed"))
                    {
                        var logger = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>().CreateLogger<Startup>();
                        logger.LogDebug("Correlation Cookie was invalid - probably timed-out");

                        ctx.Response.Redirect("/");
                        ctx.HandleResponse();
                    }

                    return Task.CompletedTask;
                };
            });
        }
    }
#endif
}