using System;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization;
using SFA.DAS.EmployerFinance.Hashing;
using SFA.DAS.EmployerFinance.Web.Authentication;
using SFA.DAS.EmployerFinance.Web.Authorization;
using SFA.DAS.EmployerFinance.Web.RouteValues;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerFinance.UnitTests.Web.Authorization
{
    [TestFixture]
    [Parallelizable]
    public class AuthorizationContextProviderTests : FluentTest<AuthorizationContextProviderTestsFixture>
    {
        [Test]
        public void GetAuthorizationContext_WhenAccountIdExistsAndIsValidAndUserIsAuthenticatedAndUserRefIsValidAndUserEmailIsValid_ThenShouldReturnAuthroizationContextWithAccountIdAndUserRefValues()
        {
            Test(f => f.SetValidAccountId().SetValidUserRef().SetValidUserEmail(), f => f.GetAuthorizationContext(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Get<long?>("AccountId").Should().Be(f.AccountId);
                r.Get<Guid?>("UserRef").Should().Be(f.UserRef);
            });
        }
        
        [Test]
        public void GetAuthorizationContext_WhenAccountIdDoesNotExistAndUserIsNotAuthenticated_ThenShouldReturnAuthroizationContextWithoutAccountIdAndUserRefValues()
        {
            Test(f => f.SetUnauthenticatedUser(), f => f.GetAuthorizationContext(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Get<long?>("AccountId").Should().BeNull();
                r.Get<Guid?>("UserRef").Should().BeNull();
            });
        }
        
        [Test]
        public void GetAuthorizationContext_WhenAccountIdExistsAndIsInvalid_ThenShouldThrowUnauthorizedAccessException()
        {
            TestException(f => f.SetInvalidAccountId(), f => f.GetAuthorizationContext(), (f, r) => r.Should().Throw<UnauthorizedAccessException>());
        }
        
        [Test]
        public void GetAuthorizationContext_WhenUserIsAuthenticatedAndUserRefIsInvalid_ThenShouldThrowUnauthorizedAccessException()
        {
            TestException(f => f.SetValidAccountId().SetInvalidUserRef(), f => f.GetAuthorizationContext(), (f, r) => r.Should().Throw<UnauthorizedAccessException>());
        }
        
        [Test]
        public void GetAuthorizationContext_WhenUserIsAuthenticatedAndUserEmailIsInvalid_ThenShouldThrowUnauthorizedAccessException()
        {
            TestException(f => f.SetValidAccountId().SetValidUserRef().SetInvalidUserEmail(), f => f.GetAuthorizationContext(), (f, r) => r.Should().Throw<UnauthorizedAccessException>());
        }
    }

    public class AuthorizationContextProviderTestsFixture
    {
        public IAuthorizationContextProvider AuthorizationContextProvider { get; set; }
        public Mock<IHttpContextAccessor> HttpContextAccessor { get; set; }
        public Mock<IRoutingFeature> RoutingFeature { get; set; }
        public Mock<IHashingService> HashingService { get; set; }
        public Mock<IAuthenticationService> AuthenticationService { get; set; }
        public string AccountHashedId { get; set; }
        public long AccountId { get; set; }
        public Guid UserRef { get; set; }
        public string UserRefClaimValue { get; set; }
        public string UserEmail { get; set; }

        public AuthorizationContextProviderTestsFixture()
        {
            HttpContextAccessor = new Mock<IHttpContextAccessor>();
            RoutingFeature = new Mock<IRoutingFeature>();
            HashingService = new Mock<IHashingService>();
            AuthenticationService = new Mock<IAuthenticationService>();
            
            HttpContextAccessor.Setup(c => c.HttpContext.Features[typeof(IRoutingFeature)]).Returns(RoutingFeature.Object);
            RoutingFeature.Setup(f => f.RouteData).Returns(new RouteData());
            
            AuthorizationContextProvider = new AuthorizationContextProvider(HttpContextAccessor.Object, HashingService.Object, AuthenticationService.Object);
        }

        public IAuthorizationContext GetAuthorizationContext()
        {
            return AuthorizationContextProvider.GetAuthorizationContext();
        }

        public AuthorizationContextProviderTestsFixture SetValidAccountId()
        {
            AccountHashedId = "ABC";
            AccountId = 123;

            var routeData = new RouteData();
            var accountId = AccountId;
            
            routeData.Values[RouteValueKeys.AccountHashedId] = AccountHashedId;
            
            RoutingFeature.Setup(f => f.RouteData).Returns(routeData);
            HashingService.Setup(h => h.TryDecodeValue(AccountHashedId, out accountId)).Returns(true);
            
            return this;
        }

        public AuthorizationContextProviderTestsFixture SetInvalidAccountId()
        {
            AccountHashedId = "AAA";

            var routeData = new RouteData();
            var accountId = 0L;
            
            routeData.Values[RouteValueKeys.AccountHashedId] = AccountHashedId;
            
            RoutingFeature.Setup(f => f.RouteData).Returns(routeData);
            HashingService.Setup(h => h.TryDecodeValue(AccountHashedId, out accountId)).Returns(false);
            
            return this;
        }

        public AuthorizationContextProviderTestsFixture SetValidUserRef()
        {
            UserRef = Guid.NewGuid();
            UserRefClaimValue = UserRef.ToString();
            
            var userRefClaimValue = UserRefClaimValue;
            
            AuthenticationService.Setup(a => a.IsUserAuthenticated()).Returns(true);
            AuthenticationService.Setup(a => a.TryGetUserClaimValue(DasClaimTypes.Id, out userRefClaimValue)).Returns(true);
            
            return this;
        }

        public AuthorizationContextProviderTestsFixture SetInvalidUserRef()
        {
            UserRefClaimValue = "BBB";
            
            var userRefClaimValue = UserRefClaimValue;
            
            AuthenticationService.Setup(a => a.IsUserAuthenticated()).Returns(true);
            AuthenticationService.Setup(a => a.TryGetUserClaimValue(DasClaimTypes.Id, out userRefClaimValue)).Returns(true);
            
            return this;
        }

        public AuthorizationContextProviderTestsFixture SetValidUserEmail()
        {
            UserEmail = "foo@bar.com";
            
            var userEmailClaimValue = UserEmail;
            
            AuthenticationService.Setup(a => a.IsUserAuthenticated()).Returns(true);
            AuthenticationService.Setup(a => a.TryGetUserClaimValue(DasClaimTypes.Email, out userEmailClaimValue)).Returns(true);
            
            return this;
        }

        public AuthorizationContextProviderTestsFixture SetInvalidUserEmail()
        {
            UserEmail = null;
            
            var userEmailClaimValue = UserEmail;
            
            AuthenticationService.Setup(a => a.IsUserAuthenticated()).Returns(true);
            AuthenticationService.Setup(a => a.TryGetUserClaimValue(DasClaimTypes.Email, out userEmailClaimValue)).Returns(false);
            
            return this;
        }

        public AuthorizationContextProviderTestsFixture SetUnauthenticatedUser()
        {
            AuthenticationService.Setup(a => a.IsUserAuthenticated()).Returns(false);
            
            return this;
        }
    }
}