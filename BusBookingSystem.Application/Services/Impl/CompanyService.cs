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
            // DTO'yu Entity'ye çevir
            var newCompany = new Company
            {
                Name = companyDto.Name,
                Phone = companyDto.Phone,
                Email = companyDto.Email,
                Address = companyDto.Address,
                // CreatedDate otomatik atanıyor (BaseEntity'den)
            };

            // Veritabanına ekle
            await _context.Companies.AddAsync(newCompany);

            // Değişiklikleri kaydet
            await _context.SaveChangesAsync();

            // Oluşturulan company'yi DTO olarak döndür
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

