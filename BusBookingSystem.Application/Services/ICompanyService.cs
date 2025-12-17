// BusBookingSystem.Application/Services/ICompanyService.cs
using BusBookingSystem.Application.DTOs.Request;
using BusBookingSystem.Application.DTOs.Response;

namespace BusBookingSystem.Application.Services
{
    public interface ICompanyService
    {
        Task<CompanyDto> AddCompanyAsync(CreateCompanyDto companyDto);
        Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync();
        Task<CompanyDto?> GetCompanyByIdAsync(int id);
    }
}

