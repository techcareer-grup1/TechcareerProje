using Moq;
using NUnit.Framework;
using AutoMapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TechCareer.Service.Concretes;
using TechCareer.Service.Abstracts;
using TechCareer.Models.Dtos.Roles;
using TechCareer.DataAccess.Repositories.Abstracts;
using Core.Security.Entities;
using TechCareer.Service.Rules;
using TechCareer.Service.Constants;
using FluentValidation.TestHelper;
using Core.CrossCuttingConcerns.Exceptions.ExceptionTypes;

namespace TechCareer.Tests
{
    [TestFixture]
    public class OperationClaimServiceTests
    {
        private Mock<IOperationClaimRepository> _operationClaimRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<OperationClaimBusinessRules> _businessRulesMock;
        private IOperationClaimService _operationClaimService;

        [SetUp]
        public void SetUp()
        {
            _operationClaimRepositoryMock = new Mock<IOperationClaimRepository>();
            _mapperMock = new Mock<IMapper>();
            _businessRulesMock = new Mock<OperationClaimBusinessRules>();
            _operationClaimService = new OperationClaimService(
                _operationClaimRepositoryMock.Object,
                _mapperMock.Object,
                _businessRulesMock.Object
            );
        }

        [Test]
        public async Task AddAsync_ShouldReturnOperationClaimResponseDto_WhenValidDtoIsGiven()
        {
            // Arrange
            var addDto = new OperationClaimAddRequestDto { Name = "Admin" };
            var operationClaim = new OperationClaim { Id = 1, Name = "Admin" };
            var responseDto = new OperationClaimResponseDto { Id = 1, Name = "Admin" };

            _businessRulesMock.Setup(b => b.RoleNameIsUnique(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<OperationClaim>(It.IsAny<OperationClaimAddRequestDto>())).Returns(operationClaim);
            _operationClaimRepositoryMock.Setup(r => r.AddAsync(It.IsAny<OperationClaim>())).ReturnsAsync(operationClaim);
            _mapperMock.Setup(m => m.Map<OperationClaimResponseDto>(It.IsAny<OperationClaim>())).Returns(responseDto);

            // Act
            var result = await _operationClaimService.AddAsync(addDto, CancellationToken.None);

            // Assert
            Assert.AreEqual(responseDto.Id, result.Id);
            Assert.AreEqual(responseDto.Name, result.Name);
            _operationClaimRepositoryMock.Verify(r => r.AddAsync(It.IsAny<OperationClaim>()), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnRoleDeletedMessage_WhenValidIdIsGiven()
        {
            // Arrange
            int id = 1;
            var operationClaim = new OperationClaim { Id = 1, Name = "Admin" };

            _businessRulesMock.Setup(b => b.RoleIsPresentCheck(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _operationClaimRepositoryMock.Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<OperationClaim, bool>>>())).ReturnsAsync(operationClaim);
            _operationClaimRepositoryMock.Setup(r => r.DeleteAsync(It.IsAny<OperationClaim>())).Returns(Task.CompletedTask);

            // Act
            var result = await _operationClaimService.DeleteAsync(id, CancellationToken.None);

            // Assert
            Assert.AreEqual(RoleMessages.RoleDeleted, result);
            _operationClaimRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<OperationClaim>()), Times.Once);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnListOfOperationClaimResponseDto()
        {
            // Arrange
            var operationClaims = new List<OperationClaim>
            {
                new OperationClaim { Id = 1, Name = "Admin" },
                new OperationClaim { Id = 2, Name = "User" }
            };
            var responseDtos = new List<OperationClaimResponseDto>
            {
                new OperationClaimResponseDto { Id = 1, Name = "Admin" },
                new OperationClaimResponseDto { Id = 2, Name = "User" }
            };

            _operationClaimRepositoryMock.Setup(r => r.GetListAsync(It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(operationClaims);
            _mapperMock.Setup(m => m.Map<List<OperationClaimResponseDto>>(It.IsAny<List<OperationClaim>>())).Returns(responseDtos);

            // Act
            var result = await _operationClaimService.GetAllAsync(CancellationToken.None);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Admin", result[0].Name);
            _operationClaimRepositoryMock.Verify(r => r.GetListAsync(It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnOperationClaimResponseDto_WhenValidIdIsGiven()
        {
            // Arrange
            int id = 1;
            var operationClaim = new OperationClaim { Id = 1, Name = "Admin" };
            var responseDto = new OperationClaimResponseDto { Id = 1, Name = "Admin" };

            _businessRulesMock.Setup(b => b.RoleIsPresentCheck(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _operationClaimRepositoryMock.Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<OperationClaim, bool>>>())).ReturnsAsync(operationClaim);
            _mapperMock.Setup(m => m.Map<OperationClaimResponseDto>(It.IsAny<OperationClaim>())).Returns(responseDto);

            // Act
            var result = await _operationClaimService.GetByIdAsync(id, CancellationToken.None);

            // Assert
            Assert.AreEqual(responseDto.Id, result.Id);
            Assert.AreEqual(responseDto.Name, result.Name);
            _operationClaimRepositoryMock.Verify(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<OperationClaim, bool>>>()), Times.Once);
        }

     

        // OperationClaimAddValidator Testleri
        [Test]
        public void OperationClaimAddValidator_ShouldHaveValidationError_WhenNameIsEmpty()
        {
            var validator = new OperationClaimAddValidator();
            var result = validator.TestValidate(new OperationClaimAddRequestDto { Name = "" });

            result.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage(RoleMessages.RoleNameNotBeEmpty);
        }

        [Test]
        public void OperationClaimAddValidator_ShouldHaveValidationError_WhenNameIsTooShort()
        {
            var validator = new OperationClaimAddValidator();
            var result = validator.TestValidate(new OperationClaimAddRequestDto { Name = "A" });

            result.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage(RoleMessages.RoleNameMustBeMinRangeMessage);
        }

        [Test]
        public void OperationClaimAddValidator_ShouldNotHaveValidationError_WhenNameIsValid()
        {
            var validator = new OperationClaimAddValidator();
            var result = validator.TestValidate(new OperationClaimAddRequestDto { Name = "Admin" });

            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        // OperationClaimBusinessRules Testleri
        [Test]
        public async Task RoleNameIsUnique_ShouldThrowBusinessException_WhenRoleNameAlreadyExists()
        {
            var name = "Admin";
            _operationClaimRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<OperationClaim, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var exception = Assert.ThrowsAsync<BusinessException>(() => _businessRulesMock.Object.RoleNameIsUnique(name, CancellationToken.None));

            Assert.AreEqual(RoleMessages.RoleNameMustBeUnique, exception.Message);
        }

        [Test]
        public async Task RoleNameIsUnique_ShouldNotThrowException_WhenRoleNameIsUnique()
        {
            var name = "Admin";
            _operationClaimRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<OperationClaim, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            await _businessRulesMock.Object.RoleNameIsUnique(name, CancellationToken.None);
        }

        [Test]
        public async Task RoleIsPresentCheck_ShouldThrowBusinessException_WhenRoleDoesNotExist()
        {
            var id = 1;
            _operationClaimRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<OperationClaim, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var exception = Assert.ThrowsAsync<BusinessException>(() => _businessRulesMock.Object.RoleIsPresentCheck(id, CancellationToken.None));

            Assert.AreEqual(RoleMessages.RoleNotFound, exception.Message);
        }

        [Test]
        public async Task RoleIsPresentCheck_ShouldNotThrowException_WhenRoleExists()
        {
            var id = 1;
            _operationClaimRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<OperationClaim, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await _businessRulesMock.Object.RoleIsPresentCheck(id, CancellationToken.None);
        }
    }
}
