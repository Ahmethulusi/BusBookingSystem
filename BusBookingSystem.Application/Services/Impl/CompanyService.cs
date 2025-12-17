// BusBookingSystem.Application/Services/Impl/CompanyService.cs
using BusBookingSystem.Application.DTOs.Request;
using BusBookingSystem.Application.DTOs.Response;
using BusBookingSystem.Application.Mappers;
using BusBookingSystem.Core.Entities;
using BusBookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BusBookingSystem.Application.Services.Impl
{
    public class CompanyService : ICompanyService
    {
        private readonly AppDbContext _context;

        public CompanyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CompanyDto> AddCompanyAsync(CreateCompanyDto companyDto)
        {
            var newCompany = new Company
            {
                Name = companyDto.Name,
                Phone = companyDto.Phone,
                Email = companyDto.Email,
                Address = companyDto.Address,
            };

            await _context.Companies.AddAsync(newCompany);
            await _context.SaveChangesAsync();

            return newCompany.ToDto();
        }

        public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync()
        {
            var companies = await _context.Companies
                .OrderBy(c => c.Name)
                .ToListAsync();

            return companies.ToDto();
        }

        public async Task<CompanyDto?> GetCompanyByIdAsync(int id)
        {
            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.Id == id);

            return company?.ToDto();
        }
    }
}

