using Moq;
using NUnit.Framework;
using TechCareer.Service.Concretes;
using TechCareer.Service.Abstracts;
using TechCareer.DataAccess.Repositories.Abstracts;
using Core.Security.Entities;
using TechCareer.Service.Rules;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Threading;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.TestHelper;
using TechCareer.Service.Validations.Users;

namespace TechCareer.Service.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<UserBusinessRules> _mockUserBusinessRules;
        private UserService _userService;

        [SetUp]
        public void SetUp()
        {
            // Mocking dependencies
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUserBusinessRules = new Mock<UserBusinessRules>(_mockUserRepository.Object);

            // Mock'lanmış bağımlılıklar ile UserService'i başlattık
            _userService = new UserService(_mockUserRepository.Object, _mockUserBusinessRules.Object);
        }

        [Test]
        public async Task AddAsync_ShouldAddUser_WhenValid()
        {
            var user = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Status = true
            };

            _mockUserBusinessRules.Setup(x => x.UserEmailShouldNotExistsWhenInsert(It.IsAny<string>())).Returns(Task.CompletedTask);
            _mockUserRepository.Setup(x => x.AddAsync(It.IsAny<User>())).ReturnsAsync(user);

            var result = await _userService.AddAsync(user);

            Assert.IsNotNull(result);
            Assert.AreEqual(user.Email, result.Email);
            _mockUserRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateUser_WhenValid()
        {
            var user = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Status = true
            };

            _mockUserBusinessRules.Setup(x => x.UserEmailShouldNotExistsWhenUpdate(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            _mockUserRepository.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(user);

            var result = await _userService.UpdateAsync(user);

            Assert.IsNotNull(result);
            Assert.AreEqual(user.Email, result.Email);
            _mockUserRepository.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteUser_WhenValid()
        {
            var user = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Status = true
            };

            _mockUserRepository.Setup(x => x.DeleteAsync(It.IsAny<User>(), It.IsAny<bool>())).ReturnsAsync(user);

            var result = await _userService.DeleteAsync(user, false);

            Assert.IsNotNull(result);
            Assert.AreEqual(user.Id, result.Id);
            _mockUserRepository.Verify(x => x.DeleteAsync(It.IsAny<User>(), It.IsAny<bool>()), Times.Once);
        }

        [Test]
        public async Task GetAsync_ShouldReturnUser_WhenUserExists()
        {
            var user = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Status = true
            };

            _mockUserRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);

            var result = await _userService.GetAsync(u => u.Id == 1);

            Assert.IsNotNull(result);
            Assert.AreEqual(user.Id, result?.Id);
            _mockUserRepository.Verify(x => x.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetListAsync_ShouldReturnUsers_WhenUsersExist()
        {
            var users = new List<User>
            {
                new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Status = true },
                new User { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", Status = true }
            };

            _mockUserRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(users);

            var result = await _userService.GetListAsync(u => u.Status == true);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            _mockUserRepository.Verify(x => x.GetListAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UserEmailShouldNotExistsWhenInsert_ShouldThrowBusinessException_WhenEmailExists()
        {
            _mockUserRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>())).ReturnsAsync(true);

            var exception = Assert.ThrowsAsync<BusinessException>(() => _mockUserBusinessRules.Object.UserEmailShouldNotExistsWhenInsert("existing.email@example.com"));
            Assert.AreEqual(AuthMessages.UserMailAlreadyExists, exception.Message);
        }

        [Test]
        public async Task UserEmailShouldNotExistsWhenUpdate_ShouldThrowBusinessException_WhenEmailExists()
        {
            _mockUserRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>())).ReturnsAsync(true);

            var exception = Assert.ThrowsAsync<BusinessException>(() => _mockUserBusinessRules.Object.UserEmailShouldNotExistsWhenUpdate(1, "existing.email@example.com"));
            Assert.AreEqual(AuthMessages.UserMailAlreadyExists, exception.Message);
        }

        [Test]
        public void LoginValidator_ShouldHaveValidationError_WhenEmailIsInvalid()
        {
            var validator = new LoginValidator();
            var result = validator.TestValidate(new UserForLoginDto { Email = "invalid-email" });

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Test]
        public void LoginValidator_ShouldNotHaveValidationError_WhenEmailIsValid()
        {
            var validator = new LoginValidator();
            var result = validator.TestValidate(new UserForLoginDto { Email = "valid.email@example.com" });

            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }
    }
}
