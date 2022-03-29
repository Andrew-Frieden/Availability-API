using Availability_API.DTOs;
using Availability_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Availability_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AvailabilityController : ControllerBase
    {
        private readonly IAvailabilityService m_availabilityService;

        public AvailabilityController(IAvailabilityService availabilityService)
        {
            m_availabilityService = availabilityService;
        }

        [HttpGet(Name = "api/v1/availability")]
        public IEnumerable<EventDto> Get([FromQuery] int[] userIds)
        {
            return m_availabilityService.GetAvailableTimeSlots(userIds.ToList());
        }
    }
}