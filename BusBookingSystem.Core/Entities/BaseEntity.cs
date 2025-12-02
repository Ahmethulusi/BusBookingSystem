namespace BusBookingSystem.Core.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Postgres için UtcNow daha güvenlidir
        public DateTime? UpdatedDate { get; set; }
    }
}