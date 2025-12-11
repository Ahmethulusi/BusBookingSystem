// BusBookingSystem.API/Controllers/CompaniesController.cs
using BusBookingSystem.Application.DTOs.Request;
using BusBookingSystem.Application.DTOs.Response;
using BusBookingSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            try
            {
                var companies = await _companyService.GetAllCompaniesAsync();
                return Ok(Response<IEnumerable<CompanyDto>>.Successful(companies));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<IEnumerable<CompanyDto>>.Fail(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            try
            {
                var company = await _companyService.GetCompanyByIdAsync(id);
                if (company == null)
                    return NotFound(Response<CompanyDto>.Fail("Firma bulunamadı"));

                return Ok(Response<CompanyDto>.Successful(company));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<CompanyDto>.Fail(ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDto request)
        {
            try
            {
                var createdCompany = await _companyService.AddCompanyAsync(request);
                return Ok(Response<CompanyDto>.Successful(createdCompany, "Firma başarıyla oluşturulmuştur"));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<CompanyDto>.Fail(ex.Message));
            }
        }
    }
}

