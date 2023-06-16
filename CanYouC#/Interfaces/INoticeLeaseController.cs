using CanYouC_.Models;

namespace CanYouC_.Interfaces
{
    public interface INoticeLeaseController
    {
        IEnumerable<NoticeLeaseSchedule> GetNoticeLeases();
        IEnumerable<NoticeLeaseSchedule> GetNoticeLeaseFromRawSchedules();
    }
}
