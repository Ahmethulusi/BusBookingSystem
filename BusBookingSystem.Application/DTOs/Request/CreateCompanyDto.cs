// BusBookingSystem.Application/DTOs/Request/CreateCompanyDto.cs
namespace BusBookingSystem.Application.DTOs.Request
{
    public class CreateCompanyDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}

