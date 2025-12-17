using BusBookingSystem.Application.DTOs.Response;
using BusBookingSystem.Core.Entities;

namespace BusBookingSystem.Application.Mappers
{
    public static class CompanyMapper
    {
        public static CompanyDto ToDto(this Company company)
        {
            return new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Phone = company.Phone,
                Email = company.Email,
                Address = company.Address,
                CreatedDate = company.CreatedDate,
                UpdatedDate = company.UpdatedDate
            };
        }

        public static IEnumerable<CompanyDto> ToDto(this IEnumerable<Company> companies)
        {
            return companies.Select(company => company.ToDto());
        }
    }
}

