using FluentValidation.TestHelper;
using NUnit.Framework;
using Moq;
using AutoMapper;
using TechCareer.Service.Concretes;
using TechCareer.Service.Abstracts;
using TechCareer.Models.Entities;
using TechCareer.Models.Dtos.VideoEducations.Request;
using TechCareer.Models.Dtos.VideoEducations.Response;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Service.Rules;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;
using Core.CrossCuttingConcerns.Responses;
using Core.CrossCuttingConcerns.Exceptions.ExceptionTypes;

namespace TechCareer.Tests
{
    [TestFixture]
    public class VideoEduServiceTests
    {
        private Mock<IVideoEduRepository> _mockVideoEduRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<VideoEduBusinessRules> _mockVideoEduBusinessRules;
        private IVideoEduService _videoEduService;

        [SetUp]
        public void Setup()
        {
            _mockVideoEduRepository = new Mock<IVideoEduRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockVideoEduBusinessRules = new Mock<VideoEduBusinessRules>();

            _videoEduService = new VideoEduService(
                _mockVideoEduRepository.Object,
                _mockVideoEduBusinessRules.Object,
                _mockMapper.Object
            );
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnListOfVideoEdu_WhenVideoEdusExist()
        {
            // Arrange
            var videoEducations = new List<VideoEducation>
            {
                new VideoEducation(1, "Video 1", "Description 1", 10, true, Level.Beginner, "image1.jpg", Guid.NewGuid(), "C#"),
                new VideoEducation(2, "Video 2", "Description 2", 15, false, Level.Intermediate, "image2.jpg", Guid.NewGuid(), "Java")
            };
            var videoEduResponseDtos = new List<VideoEduResponseDto>
            {
                new VideoEduResponseDto("Video 1", "Description 1", 10, true, Level.Beginner, "image1.jpg", videoEducations[0].InstructorId, "C#"),
                new VideoEduResponseDto("Video 2", "Description 2", 15, false, Level.Intermediate, "image2.jpg", videoEducations[1].InstructorId, "Java")
            };

            _mockVideoEduRepository.Setup(repo => repo.GetListAsync()).ReturnsAsync(videoEducations);
            _mockMapper.Setup(mapper => mapper.Map<List<VideoEduResponseDto>>(videoEducations)).Returns(videoEduResponseDtos);

            // Act
            var result = await _videoEduService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(videoEduResponseDtos.Count, result.Data.Count);
            Assert.AreEqual("Video eğitimleri başarıyla listelendi.", result.Message);  // Varsayılan başarı mesajı
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnVideoEdu_WhenVideoEduExists()
        {
            // Arrange
            var videoId = 1;
            var videoEducation = new VideoEducation(videoId, "Video 1", "Description", 10, true, Level.Beginner, "image.jpg", Guid.NewGuid(), "C#");
            var videoEduResponseDto = new VideoEduResponseDto("Video 1", "Description", 10, true, Level.Beginner, "image.jpg", videoEducation.InstructorId, "C#");

            _mockVideoEduRepository.Setup(repo => repo.GetAsync(It.IsAny<Func<VideoEducation, bool>>())).ReturnsAsync(videoEducation);
            _mockMapper.Setup(mapper => mapper.Map<VideoEduResponseDto>(videoEducation)).Returns(videoEduResponseDto);

            // Act
            var result = await _videoEduService.GetByIdAsync(videoId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(videoEduResponseDto, result.Data);
            Assert.AreEqual("Video eğitim başarıyla getirildi.", result.Message);  // Varsayılan başarı mesajı
        }

        [Test]
        public async Task CreateAsync_ShouldReturnCreatedResponse_WhenVideoEduIsCreated()
        {
            // Arrange
            var request = new CreateVideoEduRequestDto("New Video", "Description", 10, true, Level.Beginner, "image.jpg", Guid.NewGuid(), "C#");
            var videoEducation = new VideoEducation("New Video", "Description", 10, true, Level.Beginner, "image.jpg", Guid.NewGuid(), "C#");
            var createVideoEduResponseDto = new CreateVideoEduResponseDto("New Video", DateTime.Now);

            _mockVideoEduBusinessRules.Setup(businessRules => businessRules.IsVideoTitleExist(It.IsAny<string>())).Returns(Task.CompletedTask);
            _mockMapper.Setup(mapper => mapper.Map<VideoEducation>(request)).Returns(videoEducation);
            _mockVideoEduRepository.Setup(repo => repo.AddAsync(videoEducation)).ReturnsAsync(videoEducation);
            _mockMapper.Setup(mapper => mapper.Map<CreateVideoEduResponseDto>(videoEducation)).Returns(createVideoEduResponseDto);

            // Act
            var result = await _videoEduService.CreateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(createVideoEduResponseDto, result.Data);
            Assert.AreEqual("Video eğitim başarıyla eklendi.", result.Message); // Varsayılan başarı mesajı
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnUpdatedResponse_WhenVideoEduIsUpdated()
        {
            // Arrange
            var request = new UpdateVideoEduRequestDto(1, "Updated Video", "Updated Description", 12, true, Level.Intermediate, "updatedimage.jpg", Guid.NewGuid(), "Python");
            var videoEducation = new VideoEducation(1, "Updated Video", "Updated Description", 12, true, Level.Intermediate, "updatedimage.jpg", Guid.NewGuid(), "Python");
            var updateVideoEduResponseDto = new UpdateVideoEduResponseDto("Updated Video", DateTime.Now);

            _mockVideoEduBusinessRules.Setup(businessRules => businessRules.IsVideoEduExist(It.IsAny<int>())).Returns(Task.CompletedTask);
            _mockMapper.Setup(mapper => mapper.Map<VideoEducation>(request)).Returns(videoEducation);
            _mockVideoEduRepository.Setup(repo => repo.UpdateAsync(videoEducation)).Returns(Task.CompletedTask);
            _mockMapper.Setup(mapper => mapper.Map<UpdateVideoEduResponseDto>(videoEducation)).Returns(updateVideoEduResponseDto);

            // Act
            var result = await _videoEduService.UpdateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(updateVideoEduResponseDto, result.Data);
            Assert.AreEqual("Video eğitim başarıyla güncellendi.", result.Message); // Varsayılan başarı mesajı
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnSuccess_WhenVideoEduIsDeleted()
        {
            // Arrange
            var videoId = 1;
            var videoEducation = new VideoEducation(videoId, "Video to delete", "Description", 10, true, Level.Beginner, "image.jpg", Guid.NewGuid(), "Java");

            _mockVideoEduRepository.Setup(repo => repo.GetAsync(It.IsAny<Func<VideoEducation, bool>>())).ReturnsAsync(videoEducation);
            _mockVideoEduRepository.Setup(repo => repo.DeleteAsync(videoEducation)).Returns(Task.CompletedTask);

            // Act
            var result = await _videoEduService.DeleteAsync(videoId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Video eğitim başarıyla silindi.", result.Message);  // Varsayılan başarı mesajı
        }

      

        // CreateVideoEduRequestValidator Testleri
        [Test]
        public void CreateVideoEduRequestValidator_ShouldHaveValidationError_WhenTitleIsEmpty()
        {
            var validator = new CreateVideoEduRequestValidator();
            var result = validator.TestValidate(new CreateVideoEduRequestDto("", "Description", 10, true, Level.Beginner, "image.jpg", Guid.NewGuid(), "C#"));

            result.ShouldHaveValidationErrorFor(x => x.Title).WithErrorMessage("Başlık boş olamaz.");
        }

        [Test]
        public void CreateVideoEduRequestValidator_ShouldHaveValidationError_WhenTitleIsTooLong()
        {
            var validator = new CreateVideoEduRequestValidator();
            var result = validator.TestValidate(new CreateVideoEduRequestDto(new string('A', 201), "Description", 10, true, Level.Beginner, "image.jpg", Guid.NewGuid(), "C#"));

            result.ShouldHaveValidationErrorFor(x => x.Title).WithErrorMessage("Başlık 200 karakterden uzun olamaz.");
        }

        // UpdateVideoEduRequestValidator Testleri
        [Test]
        public void UpdateVideoEduRequestValidator_ShouldHaveValidationError_WhenTitleIsEmpty()
        {
            var validator = new UpdateVideoEduRequestValidator();
            var result = validator.TestValidate(new UpdateVideoEduRequestDto(1, "", "Description", 10, true, Level.Beginner, "image.jpg", Guid.NewGuid(), "C#"));

            result.ShouldHaveValidationErrorFor(x => x.Title).WithErrorMessage("Başlık boş olamaz.");
        }

        [Test]
        public void UpdateVideoEduRequestValidator_ShouldHaveValidationError_WhenIdIsLessThanOrEqualToZero()
        {
            var validator = new UpdateVideoEduRequestValidator();
            var result = validator.TestValidate(new UpdateVideoEduRequestDto(0, "Updated Video", "Description", 10, true, Level.Beginner, "image.jpg", Guid.NewGuid(), "C#"));

            result.ShouldHaveValidationErrorFor(x => x.Id).WithErrorMessage("Id sıfırdan büyük olmalıdır.");
        }

        // VideoEduBusinessRules Testleri
        [Test]
        public async Task IsVideoEduNull_ShouldThrowBusinessException_WhenVideoEduIsNull()
        {
            var videoEdu = (VideoEducation)null;

            Assert.ThrowsAsync<BusinessException>(() => _mockVideoEduBusinessRules.Object.IsVideoEduNull(videoEdu));
        }

        [Test]
        public async Task IsVideoEduExist_ShouldThrowBusinessException_WhenVideoEduExists()
        {
            var videoId = 1;
            _mockVideoEduRepository.Setup(repo => repo.AnyAsync(It.IsAny<Func<VideoEducation, bool>>(), false)).ReturnsAsync(true);

            Assert.ThrowsAsync<BusinessException>(() => _mockVideoEduBusinessRules.Object.IsVideoEduExist(videoId));
        }
    }
}
