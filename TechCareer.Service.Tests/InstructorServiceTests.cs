using Moq;
using AutoMapper;
using TechCareer.Service.Concretes;
using TechCareer.Service.Abstracts;
using TechCareer.Models.Entities;
using TechCareer.Models.Dtos.Instructors.Request;
using TechCareer.Models.Dtos.Instructors.Response;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Service.Rules;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;
using Core.CrossCuttingConcerns.Exceptions.ExceptionTypes;
using FluentValidation;

namespace TechCareer.Tests
{
    public class InstructorServiceTests
    {
        private Mock<IInstructorRepository> _mockInstructorRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<InstructorBusinessRules> _mockInstructorBusinessRules;
        private InstructorService _instructorService;

        [SetUp]
        public void Setup()
        {
            _mockInstructorRepository = new Mock<IInstructorRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockInstructorBusinessRules = new Mock<InstructorBusinessRules>();

            _instructorService = new InstructorService(
                _mockInstructorRepository.Object,
                _mockInstructorBusinessRules.Object,
                _mockMapper.Object
            );
        }

        // CreateAsync metodu için test
        [Test]
        public async Task CreateAsync_ShouldReturnCreatedResponse_WhenInstructorIsAdded()
        {
            // Arrange
            var request = new CreateInstructorRequest("Test Instructor", "Test About");
            var instructor = new Instructor { Id = Guid.NewGuid(), Name = "Test Instructor", About = "Test About" };
            var instructorDto = new CreateInstructorResponse(instructor.Id, instructor.Name, instructor.About, DateTime.Now);

            _mockInstructorRepository.Setup(repo => repo.AddAsync(It.IsAny<Instructor>())).ReturnsAsync(instructor);
            _mockMapper.Setup(mapper => mapper.Map<CreateInstructorResponse>(It.IsAny<Instructor>())).Returns(instructorDto);

            // Act
            var result = await _instructorService.CreateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(instructorDto, result.Data);
        }

        // UpdateAsync methodu için test
        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedResponse_WhenInstructorExists()
        {
            // Arrange
            var request = new UpdateInstructorRequest(Guid.NewGuid(), "Updated Instructor", "Updated About");
            var instructor = new Instructor { Id = request.Id, Name = "Updated Instructor", About = "Updated About" };
            var instructorDto = new UpdateInstructorResponse(instructor.Id, instructor.Name, instructor.About, DateTime.Now);

            _mockInstructorRepository.Setup(repo => repo.GetAsync(It.IsAny<Func<Instructor, bool>>())).ReturnsAsync(instructor);
            _mockInstructorRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Instructor>())).Returns(Task.CompletedTask);
            _mockMapper.Setup(mapper => mapper.Map<UpdateInstructorResponse>(It.IsAny<Instructor>())).Returns(instructorDto);

            // Act
            var result = await _instructorService.UpdateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(instructorDto, result.Data);
        }

        // DeleteAsync methodu için test
        [Test]
        public async Task DeleteAsync_ShouldReturnSuccess_WhenInstructorExists()
        {
            // Arrange
            var instructorId = Guid.NewGuid();
            var instructor = new Instructor { Id = instructorId, Name = "Instructor to delete", About = "About" };

            _mockInstructorRepository.Setup(repo => repo.GetAsync(It.IsAny<Func<Instructor, bool>>())).ReturnsAsync(instructor);
            _mockInstructorRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Instructor>())).Returns(Task.CompletedTask);

            // Act
            var result = await _instructorService.DeleteAsync(instructorId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Instructor Deleted", result.Message);  // Beklenen değer ile gelen sonuç aynı mı ? 
        }

        // GetByIdAsync methodu için test
        [Test]
        public async Task GetByIdAsync_ShouldReturnInstructor_WhenInstructorExists()
        {
            // Arrange
            var instructorId = Guid.NewGuid();
            var instructor = new Instructor { Id = instructorId, Name = "Test Instructor", About = "Test About" };
            var instructorDto = new InstructorResponse(instructor.Id, instructor.Name, instructor.About, DateTime.Now, DateTime.Now);

            _mockInstructorRepository.Setup(repo => repo.GetAsync(It.IsAny<Func<Instructor, bool>>())).ReturnsAsync(instructor);
            _mockMapper.Setup(mapper => mapper.Map<InstructorResponse>(It.IsAny<Instructor>())).Returns(instructorDto);

            // Act
            var result = await _instructorService.GetByIdAsync(instructorId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(instructorDto, result.Data);
        }

        // GetAllAsync methodu için test
        [Test]
        public async Task GetAllAsync_ShouldReturnListOfInstructors_WhenInstructorsExist()
        {
            // Arrange
            var instructors = new List<Instructor>
            {
                new Instructor { Id = Guid.NewGuid(), Name = "Instructor 1", About = "About 1" },
                new Instructor { Id = Guid.NewGuid(), Name = "Instructor 2", About = "About 2" }
            };
            var instructorDtos = new List<InstructorResponse>
            {
                new InstructorResponse(instructors[0].Id, instructors[0].Name, instructors[0].About, DateTime.Now, DateTime.Now),
                new InstructorResponse(instructors[1].Id, instructors[1].Name, instructors[1].About, DateTime.Now, DateTime.Now)
            };

            _mockInstructorRepository.Setup(repo => repo.GetListAsync()).ReturnsAsync(instructors);
            _mockMapper.Setup(mapper => mapper.Map<List<InstructorResponse>>(It.IsAny<List<Instructor>>())).Returns(instructorDtos);

            // Act
            var result = await _instructorService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(instructorDtos.Count, result.Data.Count);
        }

        // IsInstructorNameUnique rule için test
        [Test]
        public void IsInstructorNameUnique_ShouldThrowBusinessException_WhenInstructorNameAlreadyExists()
        {
            // Arrange
            var instructorBusinessRules = new InstructorBusinessRules();
            bool anyInstructor = true; // bir eğitmenin zaten var olduğu durum

            // Act & Assert
            Assert.Throws<BusinessException>(() => instructorBusinessRules.IsInstructorNameUnique(anyInstructor));
        }

        //  IsInstructorExists rule için test
        [Test]
        public void IsInstructorExists_ShouldThrowNotFoundException_WhenInstructorNotFound()
        {
            // Arrange
            var instructorBusinessRules = new InstructorBusinessRules();
            Instructor? instructor = null; // bir eğitmenin olmadığı durum

            // Act & Assert
            Assert.Throws<NotFoundException>(() => instructorBusinessRules.IsInstructorExists(instructor));
        }

       
        // CreateAsync metodunda geçersiz veri için validation exception (doğrulama hatası) testi
        [Test]
        public async Task CreateAsync_ShouldThrowValidationException_WhenInvalidDataProvided()
        {
            // Arrange
            var invalidRequest = new CreateInstructorRequest("", ""); // Geçersiz veri (boş stringler)
            _mockInstructorRepository.Setup(repo => repo.AddAsync(It.IsAny<Instructor>())).Throws(new ValidationException("Validation failed"));

            // Act & Assert
            var exception = Assert.ThrowsAsync<ValidationException>(async () => await _instructorService.CreateAsync(invalidRequest));
            Assert.AreEqual("Validation failed", exception.Message);
        }

        //UpdateAsync metodunun repository istisnalarını ele alması için test
        [Test]
        public async Task UpdateAsync_ShouldHandleRepositoryException_WhenDatabaseFails()
        {
            // Arrange
            var request = new UpdateInstructorRequest(Guid.NewGuid(), "Updated Instructor", "Updated About");
            var instructor = new Instructor { Id = request.Id, Name = "Updated Instructor", About = "Updated About" };

            _mockInstructorRepository.Setup(repo => repo.GetAsync(It.IsAny<Func<Instructor, bool>>()))
                .Throws(new Exception("Database error")); 

            // Act & Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await _instructorService.UpdateAsync(request));
            Assert.AreEqual("Database error", exception.Message);
        }

        // Eksik eğitmen ile DeleteAsync için test
        [Test]
        public async Task DeleteAsync_ShouldThrowNotFoundException_WhenInstructorDoesNotExist()
        {
            // Arrange
            var instructorId = Guid.NewGuid();
            _mockInstructorRepository.Setup(repo => repo.GetAsync(It.IsAny<Func<Instructor, bool>>())).ReturnsAsync((Instructor)null); 

            // Act & Assert
            var exception = Assert.ThrowsAsync<NotFoundException>(async () => await _instructorService.DeleteAsync(instructorId));
            Assert.AreEqual("Instructor not found", exception.Message);  
        }
    }
}
