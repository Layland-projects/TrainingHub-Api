using TrainingHub.Infrastructure.Abstractions;

namespace TrainingHub.Infrastructure.Implementations.Mock
{
    public class MockTimestampService : ITimestampService
    {
        public DateTime GetDateTime()
        {
            return DateTime.Now;
        }

        public DateTimeOffset GetDateTimeOffset()
        {
            return DateTimeOffset.Now;
        }

        public DateTime GetUTCDateTime()
        {
            return DateTime.UtcNow;
        }

        public DateTimeOffset GetUTCDateTimeOffset()
        {
            return DateTimeOffset.UtcNow;
        }
    }
}
