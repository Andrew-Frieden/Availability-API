namespace Availability_API.DTOs
{
    public class UserDto
    {
        public int user_id { get; set; }
        public WorkingHoursDto working_hours {get;set;}
        public IEnumerable<EventDto> Events { get; set; }
    }

    public class EventDto : IDuration
    {
        public int Id { get; set; }
        public DateTime Start { get; set; } 
        public DateTime End { get; set; }
    }

    public interface IDuration
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
