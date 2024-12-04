using AutoMapper;
using Core.CrossCuttingConcerns.Responses;
using Core.Security.Repositories.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Models.Dtos.Events.Request;
using TechCareer.Models.Dtos.Events.Response;
using TechCareer.Models.Entities;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Validations.Events;
using TechCareer.Service.Validations.Instructors;

namespace TechCareer.Service.Concretes
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public EventService(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }


        [ValidationAspect(typeof(CreateEventRequestValidator))]
        [LoggerAspect]
        [ClearCacheAspect(cacheGroupKey: "GetEvents")]
        [AuthorizeAspect(RolesAuthorizationRequirement: "Admin")]
        public async Task<ReturnModel<CreateEventResponse>> CreateAsync(CreateEventRequest request)
        {
            // Check if event with same title already exists
            bool anyEvent = await _eventRepository.Where(x => x.Title == request.Title).AnyAsync();
            if (anyEvent)
            {
                return ReturnModel<CreateEventResponse>.Fail("Event title must be unique.", HttpStatusCode.BadRequest);
            }

            Event eventEntity = _mapper.Map<Event>(request);
            await _eventRepository.AddAsync(eventEntity);
            CreateEventResponse eventAsDto = _mapper.Map<CreateEventResponse>(eventEntity);

            return ReturnModel<CreateEventResponse>.Success(eventAsDto, "Event created successfully.", HttpStatusCode.Created);
        }

        [ClearCacheAspect(cacheGroupKey: "GetEvents")]
        [LoggerAspect]
        [AuthorizeAspect(RolesAuthorizationRequirement: "Admin")]
        public async Task<ReturnModel> DeleteAsync(Guid id)
        {
            Event? eventEntity = await _eventRepository.GetAsync(x => x.Id == id);
            if (eventEntity == null)
            {
                return ReturnModel.Fail("Event not found.", HttpStatusCode.NotFound);
            }

            await _eventRepository.DeleteAsync(eventEntity);
            return ReturnModel.Success("Event deleted successfully.", HttpStatusCode.NoContent);
        }

        [CacheAspect(cacheKeyTemplate: "GetEventsList", bypassCache: false, cacheGroupKey: "GetEvents")]
        public async Task<ReturnModel<List<EventResponse>>> GetAllAsync()
        {
            List<Event> events = await _eventRepository.GetListAsync();
            List<EventResponse> eventsAsDto = _mapper.Map<List<EventResponse>>(events);

            return ReturnModel<List<EventResponse>>.Success(eventsAsDto, "Events listed successfully.");
        }

        public async Task<ReturnModel<EventResponse>> GetByIdAsync(Guid id)
        {
            Event? eventEntity = await _eventRepository.GetAsync(x => x.Id == id);
            if (eventEntity == null)
            {
                return ReturnModel<EventResponse>.Fail("Event not found.", HttpStatusCode.NotFound);
            }

            EventResponse eventAsDto = _mapper.Map<EventResponse>(eventEntity);
            return ReturnModel<EventResponse>.Success(eventAsDto, "Event retrieved successfully.");
        }

        [ClearCacheAspect(cacheGroupKey: "GetEvents")]
        [LoggerAspect]
        [ValidationAspect(typeof(UpdateEventRequestValidator))]
        [AuthorizeAspect(RolesAuthorizationRequirement: "Admin")]
        public async Task<ReturnModel<UpdateEventResponse>> UpdateAsync(UpdateEventRequest request)
        {
            Event eventEntity = await _eventRepository.GetAsync(x => x.Id == request.Id);
            if (eventEntity == null)
            {
                return ReturnModel<UpdateEventResponse>.Fail("Event not found.", HttpStatusCode.NotFound);
            }

            bool anyEvent = await _eventRepository.Where(x => x.Title == request.Title && x.Id != eventEntity.Id).AnyAsync();
            if (anyEvent)
            {
                return ReturnModel<UpdateEventResponse>.Fail("Event title must be unique.", HttpStatusCode.BadRequest);
            }

            eventEntity = _mapper.Map(request, eventEntity);
            await _eventRepository.UpdateAsync(eventEntity);

            var eventAsDto = _mapper.Map<UpdateEventResponse>(eventEntity);
            return ReturnModel<UpdateEventResponse>.Success(eventAsDto, "Event updated successfully.", HttpStatusCode.NoContent);
        }
    }
}
