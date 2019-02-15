using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using SFA.DAS.Authorization;

namespace SFA.DAS.EmployerFinance.Web.Authorization
{
    public class LocalAuthorizationHandler : IAuthorizationHandler
    {
        public string Namespace => _authorizationHandler.Namespace;
        
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IAuthorizationHandler _authorizationHandler;

        public LocalAuthorizationHandler(IHostingEnvironment hostingEnvironment, IAuthorizationHandler authorizationHandler)
        {
            _hostingEnvironment = hostingEnvironment;
            _authorizationHandler = authorizationHandler;
        }
        
        public Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            return _hostingEnvironment.IsDevelopment()
                ? Task.FromResult(new AuthorizationResult())
                : _authorizationHandler.GetAuthorizationResult(options, authorizationContext);
        }
    }
}