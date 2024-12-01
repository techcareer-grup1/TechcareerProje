using AutoMapper;
using Core.Persistence.Extensions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Models.Dtos.Categories.Requests;
using TechCareer.Models.Dtos.Categories.Responses;
using TechCareer.Models.Entities;
using TechCareer.Service.Concretes;

namespace TechCareer.Tests
{
    [TestFixture]
    public class CategoryServiceTests
    {
        private Mock<ICategoryRepository> _mockCategoryRepository;
        private IMapper _mapper;
        private CategoryService _categoryService;

        [SetUp]
        public void Setup()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile(new CategoryMapper()));
            _mapper = config.CreateMapper();

            _categoryService = new CategoryService(_mockCategoryRepository.Object, _mapper);
        }

        [Test]
        public async Task AddAsync_ShouldAddCategory_WhenValidDto()
        {
            // Arrange
            var categoryDto = new CategoryAddRequestDto { Name = "Test Category" };
            var categoryEntity = new Category("Test Category");

            _mockCategoryRepository.Setup(repo => repo.AddAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);

            // Act
            await _categoryService.AddAsync(categoryDto);

            // Assert
            _mockCategoryRepository.Verify(repo => repo.AddAsync(It.Is<Category>(c => c.Name == "Test Category")), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateCategory_WhenValidDto()
        {
            // Arrange
            var categoryDto = new CategoryUpdateRequestDto { Id = 1, Name = "Updated Category" };
            var categoryEntity = new Category(1, "Updated Category");

            _mockCategoryRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);

            // Act
            await _categoryService.UpdateAsync(categoryDto);

            // Assert
            _mockCategoryRepository.Verify(repo => repo.UpdateAsync(It.Is<Category>(c => c.Name == "Updated Category" && c.Id == 1)), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteCategory_WhenCategoryExists()
        {
            // Arrange
            var categoryId = 1;
            var categoryEntity = new Category(categoryId, "Test Category");

            _mockCategoryRepository.Setup(repo => repo.GetAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Category, bool>>>())).ReturnsAsync(categoryEntity);
            _mockCategoryRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Category>(), It.IsAny<bool>())).Returns(Task.CompletedTask);

            // Act
            await _categoryService.DeleteAsync(categoryId);

            // Assert
            _mockCategoryRepository.Verify(repo => repo.DeleteAsync(It.Is<Category>(c => c.Id == categoryId), true), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_ShouldThrowBusinessException_WhenCategoryNotFound()
        {
            // Arrange
            var categoryId = 999; // Mevcut olmayan kategori ID'si
            _mockCategoryRepository.Setup(repo => repo.GetAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Category, bool>>>())).ReturnsAsync((Category)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<BusinessException>(async () => await _categoryService.DeleteAsync(categoryId));
            Assert.AreEqual("İgili id ye göre kategori bulunamadı.", ex.Message);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnListOfCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category(1, "Category 1"),
                new Category(2, "Category 2")
            };
            _mockCategoryRepository.Setup(repo => repo.GetListAsync(It.IsAny<bool>())).ReturnsAsync(categories);

            // Act
            var result = await _categoryService.GetAllAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Category 1", result[0].Name);
            Assert.AreEqual("Category 2", result[1].Name);
        }

        [Test]
        public async Task GetAllPaginateAsync_ShouldReturnPaginatedCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category(1, "Category 1"),
                new Category(2, "Category 2")
            };
            var paginate = new Paginate<Category>(categories, 1, 10, 2);
            _mockCategoryRepository.Setup(repo => repo.GetPaginateAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(paginate);

            // Act
            var result = await _categoryService.GetAllPaginateAsync(1, 10);

            // Assert
            Assert.AreEqual(2, result.Items.Count);
            Assert.AreEqual("Category 1", result.Items[0].Name);
            Assert.AreEqual("Category 2", result.Items[1].Name);
        }
    }
}

