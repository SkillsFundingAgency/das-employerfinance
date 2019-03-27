using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerFinance.UnitTests.Models
{
    [TestFixture]
    [Parallelizable]
    public class UserTests : FluentTest<UserTestsFixture>
    {
        [Test]
        public void Update_WhenPropertiesBeingUpdatedAreSameAsCurrentProperties_ThenUpdatedTimeShouldNotChange()
        {
            Test(f => f.UpdateDefaultingToSame(), f => f.User.Updated.Should().BeNull());
        }
        
        [TestCase(null, null, "lastName")]
        [TestCase(null, "firstName", null)]
        [TestCase(null, "firstName", "lastName")]
        [TestCase("email", null, null)]
        [TestCase("email", null, "lastName")]
        [TestCase("email", "firstName", null)]
        [TestCase("email", "firstName", "lastName")]
        public void Update_WhenPropertiesBeingUpdatedAreDifferentToCurrentProperties_ThenPropertiesShouldBeUpdatesAndUpdatedTimeSet(string email, string firstName, string lastName)
        {
            Test(f => f.UpdateDefaultingToSame(email, firstName, lastName), f => f.AssertUpdatedDefaultingToOriginalValues(email, firstName, lastName));
        }
    }

    public class UserTestsFixture
    {
        public User User { get; set; }
        public User OriginalUser { get; set; }
        private readonly Fixture _fixture;

        public UserTestsFixture()
        {
            _fixture = new Fixture();
            User = _fixture.Create<User>();
            OriginalUser = CloneUser(User);
        }
        
        public User CloneUser(User user)
        {
            var clonedUser = JsonConvert.DeserializeObject<User>(JsonConvert.SerializeObject(user));
            clonedUser.Updated = User.Updated;
            return clonedUser;
        }

        public void UpdateDefaultingToSame(string email = null, string firstName = null, string lastName = null)
        {
            User.Update(email ?? User.Email, firstName ?? User.FirstName, lastName ?? User.LastName);
        }

        public void AssertUpdatedDefaultingToOriginalValues(string expectedEmail = null, string expectedFirstName = null, string expectedLastName = null)
        {
            User.Email.Should().Be(expectedEmail ?? OriginalUser.Email);
            User.FirstName.Should().Be(expectedFirstName ?? OriginalUser.FirstName);
            User.LastName.Should().Be(expectedLastName ?? OriginalUser.LastName);
            User.Updated.Should().NotBeNull();
        }
    }
}