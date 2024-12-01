using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TechCareer.Models.Dtos.Events.Request;
using TechCareer.Models.Dtos.Events.Response;
using TechCareer.Models.Entities;
using TechCareer.Service.Concretes;
using TechCareer.Service.Abstracts;
using Core.CrossCuttingConcerns.Responses;
using Core.Security.Repositories.Abstracts;

namespace TechCareer.Tests
{
    [TestFixture]
    public class EventServiceTests
    {
        private Mock<IEventRepository> _mockEventRepository;
        private IMapper _mapper;
        private EventService _eventService;

        [SetUp]
        public void Setup()
        {
            _mockEventRepository = new Mock<IEventRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new EventMapper()));
            _mapper = new Mapper(config);
            _eventService = new EventService(_mockEventRepository.Object, _mapper);
        }

        [Test]
        public async Task CreateEvent_ShouldReturnSuccess_WhenEventIsCreated()
        {
            // Arrange
            var createRequest = new CreateEventRequest("Event Title", "Event Description", "ImageUrl", DateTime.Now, DateTime.Now.AddDays(1), DateTime.Now.AddDays(-1), "Participation Text", 1);
            var eventEntity = new Event("Event Title", "Event Description", "ImageUrl", DateTime.Now, DateTime.Now.AddDays(1), DateTime.Now.AddDays(-1), "Participation Text", 1);
            var eventResponse = new CreateEventResponse(Guid.NewGuid(), "Event Title", "Event Description", "ImageUrl", DateTime.Now, DateTime.Now.AddDays(1), DateTime.Now.AddDays(-1), "Participation Text", 1);

            _mockEventRepository.Setup(x => x.Where(It.IsAny<Func<Event, bool>>())).ReturnsAsync(false);  // Mock unique event check
            _mockEventRepository.Setup(x => x.AddAsync(It.IsAny<Event>())).ReturnsAsync(eventEntity);

            // Act
            var result = await _eventService.CreateAsync(createRequest);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
            Assert.IsTrue(result.Data != null);
            Assert.AreEqual("Event created successfully.", result.Message);
        }

        [Test]
        public async Task CreateEvent_ShouldReturnFailure_WhenEventTitleIsNotUnique()
        {
            // Arrange
            var createRequest = new CreateEventRequest("Event Title", "Event Description", "ImageUrl", DateTime.Now, DateTime.Now.AddDays(1), DateTime.Now.AddDays(-1), "Participation Text", 1);
            _mockEventRepository.Setup(x => x.Where(It.IsAny<Func<Event, bool>>())).ReturnsAsync(true);  // Mock event with same title exists

            // Act
            var result = await _eventService.CreateAsync(createRequest);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual("Event title must be unique.", result.Message);
        }

        [Test]
        public async Task GetEventById_ShouldReturnEvent_WhenEventExists()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var eventEntity = new Event(eventId, "Event Title", "Event Description", "ImageUrl", DateTime.Now, DateTime.Now.AddDays(1), DateTime.Now.AddDays(-1), "Participation Text", 1);
            var eventResponse = new EventResponse(eventEntity.Id, eventEntity.Title, eventEntity.Description, eventEntity.ImageUrl, eventEntity.StartDate, eventEntity.EndDate, eventEntity.ApplicationDeadline, eventEntity.ParticipationText, eventEntity.CategoryId, DateTime.Now, DateTime.Now);

            _mockEventRepository.Setup(x => x.GetAsync(It.IsAny<Func<Event, bool>>())).ReturnsAsync(eventEntity);

            // Act
            var result = await _eventService.GetByIdAsync(eventId);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(eventEntity.Id, result.Data.Id);
            Assert.AreEqual("Event retrieved successfully.", result.Message);
        }

        [Test]
        public async Task GetEventById_ShouldReturnNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            _mockEventRepository.Setup(x => x.GetAsync(It.IsAny<Func<Event, bool>>())).ReturnsAsync((Event)null);

            // Act
            var result = await _eventService.GetByIdAsync(eventId);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Event not found.", result.Message);
        }

        [Test]
        public async Task DeleteEvent_ShouldReturnSuccess_WhenEventIsDeleted()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var eventEntity = new Event(eventId, "Event Title", "Event Description", "ImageUrl", DateTime.Now, DateTime.Now.AddDays(1), DateTime.Now.AddDays(-1), "Participation Text", 1);
            _mockEventRepository.Setup(x => x.GetAsync(It.IsAny<Func<Event, bool>>())).ReturnsAsync(eventEntity);

            // Act
            var result = await _eventService.DeleteAsync(eventId);

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
            Assert.AreEqual("Event deleted successfully.", result.Message);
        }

        [Test]
        public async Task DeleteEvent_ShouldReturnNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            _mockEventRepository.Setup(x => x.GetAsync(It.IsAny<Func<Event, bool>>())).ReturnsAsync((Event)null);

            // Act
            var result = await _eventService.DeleteAsync(eventId);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Event not found.", result.Message);
        }

        [Test]
        public async Task UpdateEvent_ShouldReturnSuccess_WhenEventIsUpdated()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var updateRequest = new UpdateEventRequest(eventId, "Updated Title", "Updated Description", "Updated ImageUrl", DateTime.Now, DateTime.Now.AddDays(1), DateTime.Now.AddDays(-1), "Updated Participation Text", 1);
            var eventEntity = new Event(eventId, "Event Title", "Event Description", "ImageUrl", DateTime.Now, DateTime.Now.AddDays(1), DateTime.Now.AddDays(-1), "Participation Text", 1);
            var updatedEventResponse = new UpdateEventResponse(eventId, "Updated Title", "Updated Description", "Updated ImageUrl", DateTime.Now, DateTime.Now.AddDays(1), DateTime.Now.AddDays(-1), "Updated Participation Text", 1);

            _mockEventRepository.Setup(x => x.GetAsync(It.IsAny<Func<Event, bool>>())).ReturnsAsync(eventEntity);
            _mockEventRepository.Setup(x => x.Where(It.IsAny<Func<Event, bool>>())).ReturnsAsync(false);

            // Act
            var result = await _eventService.UpdateAsync(updateRequest);

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
            Assert.AreEqual("Event updated successfully.", result.Message);
        }

        [Test]
        public async Task UpdateEvent_ShouldReturnNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            var updateRequest = new UpdateEventRequest(Guid.NewGuid(), "Updated Title", "Updated Description", "Updated ImageUrl", DateTime.Now, DateTime.Now.AddDays(1), DateTime.Now.AddDays(-1), "Updated Participation Text", 1);
            _mockEventRepository.Setup(x => x.GetAsync(It.IsAny<Func<Event, bool>>())).ReturnsAsync((Event)null);

            // Act
            var result = await _eventService.UpdateAsync(updateRequest);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Event not found.", result.Message);
        }
    }
}

