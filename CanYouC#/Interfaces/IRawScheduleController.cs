using CanYouC_.Models;

namespace CanYouC_.Interfaces
{
    public interface IRawScheduleController
    {
        IEnumerable<RawSchedule> GetRawSchedules();
    }
}
