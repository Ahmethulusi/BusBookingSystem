using BusBookingSystem.Core.Helpers;

namespace BusBookingSystem.Core.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTimeHelper.GetTurkeyTimeNow();
        public DateTime? UpdatedDate { get; set; }
    }
}