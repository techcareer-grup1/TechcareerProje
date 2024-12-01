using Moq;
using NUnit.Framework;
using TechCareer.Service.Concretes;
using TechCareer.Service.Abstracts;
using Core.Security.Entities;
using Core.Security.JWT;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace TechCareer.Tests
{
    [TestFixture]
    public class UserWithTokenServiceTests
    {
        private Mock<ITokenHelper> _tokenHelperMock;
        private Mock<IUserOperationClaimRepository> _userOperationClaimRepositoryMock;
        private UserWithTokenService _userWithTokenService;

        [SetUp]
        public void Setup()
        {
            _tokenHelperMock = new Mock<ITokenHelper>();
            _userOperationClaimRepositoryMock = new Mock<IUserOperationClaimRepository>();
            _userWithTokenService = new UserWithTokenService(_tokenHelperMock.Object, _userOperationClaimRepositoryMock.Object);
        }

        [Test]
        public async Task CreateAccessToken_ShouldReturnAccessToken_WhenValidUser()
        {
            // Arrange
            var user = new User { Id = 1, Email = "test@test.com", FirstName = "John", LastName = "Doe" };
            var operationClaims = new List<OperationClaim>
            {
                new OperationClaim { Id = 1, Name = "Admin" },
                new OperationClaim { Id = 2, Name = "User" }
            };

           
            _userOperationClaimRepositoryMock.Setup(repo => repo.Query())
                .Returns(new List<UserOperationClaim>
                {
                    new UserOperationClaim { UserId = 1, OperationClaimId = 1, OperationClaim = new OperationClaim { Id = 1, Name = "Admin" } },
                    new UserOperationClaim { UserId = 1, OperationClaimId = 2, OperationClaim = new OperationClaim { Id = 2, Name = "User" } }
                }.AsQueryable().BuildMockDbSet().Object);

           
            var expectedToken = new AccessToken { Token = "validToken", Expiration = DateTime.Now.AddHours(1) };
            _tokenHelperMock.Setup(helper => helper.CreateToken(user, operationClaims)).Returns(expectedToken);

            // Act
            var result = await _userWithTokenService.CreateAccessToken(user);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedToken.Token, result.Token);
            Assert.AreEqual(expectedToken.Expiration, result.Expiration);
        }
    }
}
