using Availability_API.DTOs;

namespace Availability_API.Services
{
    public interface IAvailabilityService
    {
        IEnumerable<EventDto> GetAvailableTimeSlots(IEnumerable<int> userIds);
    }
}
