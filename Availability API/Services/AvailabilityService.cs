using Availability_API.DTOs;
using Newtonsoft.Json;

namespace Availability_API.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        string USER_DATA_LOCATION = @"C:\Users\andre\source\repos\Availability API\Availability API\JSON\user_data.json";

        public IEnumerable<EventDto> GetAvailableTimeSlots(IEnumerable<int> userIds)
        {
            var userData = JsonConvert.DeserializeObject<IEnumerable<UserDto>>(File.ReadAllText(USER_DATA_LOCATION));

            var allUserEvents = userData.Where(u => userIds.Contains(u.user_id)).SelectMany(x => x.Events);
            var allUserWorkingHours = userData.Where(u => userIds.Contains(u.user_id)).Select(x => x.working_hours);

            var aggregatedMeetings = AggregateDurations(allUserEvents);
            var aggregatedWorkingHours = AggregateDurations(allUserWorkingHours);

            return CreateAvailableEvents(aggregatedMeetings, aggregatedWorkingHours.First().Start, aggregatedWorkingHours.First().End);
        }

        private IEnumerable<EventDto> CreateAvailableEvents(IEnumerable<IDuration> aggregatedEvents, DateTime from, DateTime to)
        {
            var availbleEvents = new List<EventDto>();
            var sortedEventArray = aggregatedEvents.OrderBy(e => e.Start).ToArray();

            if (from < sortedEventArray[0].Start)
            {
                availbleEvents.Add(new EventDto()
                {
                    Start = from,
                    End = sortedEventArray[0].Start
                });
            }

            for (int i = 0; i < sortedEventArray.Count() - 1; i++)
            {
                availbleEvents.Add(new EventDto()
                {
                    Start = sortedEventArray[i].End,
                    End = sortedEventArray[i + 1].Start
                });
            }

            if (to > sortedEventArray[sortedEventArray.Count() - 1].End)
            {
                availbleEvents.Add(new EventDto()
                {
                    Start = sortedEventArray[sortedEventArray.Count() - 1].End,
                    End = to
                });
            }

            return availbleEvents;
        }

        private IEnumerable<IDuration> AggregateDurations(IEnumerable<IDuration> durations)
        {
            var newDurations = durations.ToList();

            foreach (var baseDuration in durations)
            {
                foreach (var comparisonDuration in durations)
                {
                    if (baseDuration != comparisonDuration && DoEventsOverlap(baseDuration, comparisonDuration))
                    {
                        newDurations.Remove(baseDuration);
                        newDurations.Remove(comparisonDuration);
                        newDurations.Add(CombineDurations(baseDuration, comparisonDuration));
                        return AggregateDurations(newDurations);
                    }
                }
            }

            return newDurations;
        }

        private IDuration CombineDurations(IDuration durA, IDuration durB)
        {
            return new EventDto
            {
                Start = durA.Start < durB.Start ? durA.Start : durB.Start,
                End = durA.End > durB.End ? durA.End : durB.End
            };
        }

        private bool DoEventsOverlap(IDuration durA, IDuration durB)
        {
            return StartsFirst(durA, durB).End >= StartsLast(durA, durB).Start;

            IDuration StartsFirst(IDuration eventA, IDuration eventB)
            {
                return eventA.Start < eventB.Start ? eventA : eventB;
            }

            IDuration StartsLast(IDuration eventA, IDuration eventB)
            {
                return eventA.Start < eventB.Start ? eventB : eventA;
            }
        }
    }
}
