using Moq;
using NUnit.Framework;
using TechCareer.API.Controllers;
using TechCareer.Service.Abstracts;
using Core.Security.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Core.Security.Entities;
using TechCareer.Service.Constants;

namespace TechCareer.Tests
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<IAuthService> _authServiceMock;
        private AuthController _controller;

        [SetUp]
        public void Setup()
        {
            _authServiceMock = new Mock<IAuthService>();
            _controller = new AuthController(_authServiceMock.Object);
        }

        [Test]
        public async Task Login_ShouldReturnAccessToken_WhenValidCredentials()
        {
            // Arrange
            var loginDto = new UserForLoginDto { Email = "test@test.com", Password = "password123" };
            var token = new AccessToken { Token = "validToken" };

            _authServiceMock.Setup(service => service.LoginAsync(loginDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(token);

            // Act
            var result = await _controller.Login(loginDto, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(token, okResult.Value);
        }

        [Test]
        public async Task Login_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var loginDto = new UserForLoginDto { Email = "nonexistent@test.com", Password = "password123" };

            _authServiceMock.Setup(service => service.LoginAsync(loginDto, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception(AuthMessages.UserDontExists));

            // Act
            var result = await _controller.Login(loginDto, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual(AuthMessages.UserDontExists, notFoundResult.Value);
        }

        [Test]
        public async Task GetAllUsers_ShouldReturnPaginatedUsers()
        {
            // Arrange
            var paginateResult = new Paginate<UserResponseDto>
            {
                Items = new List<UserResponseDto>
                {
                    new UserResponseDto { Email = "test1@test.com" },
                    new UserResponseDto { Email = "test2@test.com" }
                },
                TotalCount = 2
            };

            _authServiceMock.Setup(service => service.GetAllPaginateAsync(1, 10))
                .ReturnsAsync(paginateResult);

            // Act
            var result = await _controller.GetAllUsers(1, 10);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var returnedData = okResult.Value as Paginate<UserResponseDto>;
            Assert.AreEqual(2, returnedData.Items.Count);
        }
    }
}
