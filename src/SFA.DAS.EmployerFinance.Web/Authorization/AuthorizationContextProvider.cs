using System;
using Microsoft.AspNetCore.Http;
using SFA.DAS.Authorization;
using SFA.DAS.Authorization.EmployerUserRoles;
using SFA.DAS.EmployerFinance.Hashing;
using SFA.DAS.EmployerFinance.Web.Authentication;
using SFA.DAS.EmployerFinance.Web.Extensions;
using SFA.DAS.EmployerFinance.Web.RouteValues;

namespace SFA.DAS.EmployerFinance.Web.Authorization
{
    public class AuthorizationContextProvider : IAuthorizationContextProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHashingService _hashingService;
        private readonly IAuthenticationService _authenticationService;

        public AuthorizationContextProvider(IHttpContextAccessor httpContextAccessor, IHashingService hashingService, IAuthenticationService authenticationService)
        {
            _httpContextAccessor = httpContextAccessor;
            _hashingService = hashingService;
            _authenticationService = authenticationService;
        }
        
        public IAuthorizationContext GetAuthorizationContext()
        {
            var authorizationContext = new AuthorizationContext();
            var accountValues = GetAccountValues();
            var userValues = GetUserValues();
            
            authorizationContext.AddEmployerUserRoleValues(accountValues.Id, userValues.Ref);

            return authorizationContext;
        }
        
        private (string HashedId, long? Id) GetAccountValues()
        {
            if (!_httpContextAccessor.HttpContext.TryGetRouteValue(RouteValueKeys.AccountHashedId, out var accountHashedId))
            {
                return (null, null);
            }
            
            if (!_hashingService.TryDecodeValue(accountHashedId.ToString(), out var accountId))
            {
                throw new UnauthorizedAccessException();
            }

            return (accountHashedId.ToString(), accountId);
        }

        private (Guid? Ref, string Email) GetUserValues()
        {
            if (!_authenticationService.IsUserAuthenticated())
            {
                return (null, null);
            }

            if (!_authenticationService.TryGetUserClaimValue(DasClaimTypes.Id, out var userRefClaimValue))
            {
                throw new UnauthorizedAccessException();
            }

            if (!Guid.TryParse(userRefClaimValue, out var userRef))
            {
                throw new UnauthorizedAccessException();
            }

            if (!_authenticationService.TryGetUserClaimValue(DasClaimTypes.Email, out var userEmail))
            {
                throw new UnauthorizedAccessException();
            }

            return (userRef, userEmail);
        }
    }
}