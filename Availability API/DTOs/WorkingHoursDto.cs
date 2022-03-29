using Availability_API.DTOs;

namespace Availability_API
{
    public class WorkingHoursDto : IDuration
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}