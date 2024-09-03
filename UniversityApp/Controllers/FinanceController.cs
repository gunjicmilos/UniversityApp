using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Repository.IRepository;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services;
using UniversityManagament.Services.Interfaces;

namespace UniversityManagament.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FinanceController : ControllerBase
{
    private readonly IFinanceService _financeService;
    private readonly IFacultyRepository _facultyRepository;

    public FinanceController(IFinanceService financeService, IFacultyRepository facultyRepository)
    {
        _financeService = financeService;
        _facultyRepository = facultyRepository;
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FinanceReadDto>>> GetAllFinances()
    {
        var finances = await _financeService.GetAllFinancesAsync();
        return Ok(finances);
    }

    [Authorize(Policy = "FinancePolicy")]
    [HttpGet("{id}")]
    public async Task<ActionResult<FinanceReadDto>> GetFinanceById(Guid id)
    {
        var finance = await _financeService.GetFinanceByIdAsync(id);
        if (finance == null)
        {
            return NotFound($"Finance with id : {id} not found");
        }
        return Ok(finance);
    }

    [Authorize(Policy = "FinancePolicy")]
    [HttpPost]
    public async Task<ActionResult<FinanceReadDto>> CreateFinance(FinanceCreateDto financeCreateDto)
    {
        if (await _facultyRepository.GetFacultyByIdAsync(financeCreateDto.FacultyId) == null)
            return NotFound($"Faculty with id : {financeCreateDto.FacultyId} not found");
        
        var financeReadDto = await _financeService.CreateFinanceAsync(financeCreateDto);
        return CreatedAtAction(nameof(GetFinanceById), new { id = financeReadDto.Id }, financeReadDto);
    }

    [Authorize(Policy = "FinancePolicy")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFinance(Guid id, FinanceCreateDto financeUpdateDto)
    {
        if (await _facultyRepository.GetFacultyByIdAsync(financeUpdateDto.FacultyId) == null)
            return NotFound($"Faculty with id : {financeUpdateDto.FacultyId} not found");
        
        var result = await _financeService.UpdateFinanceAsync(id, financeUpdateDto);
        if (!result)
        {
            return NotFound($"Finance with id : {id} not found");
        }
        return NoContent();
    }

    [Authorize(Policy = "FinancePolicy")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFinance(Guid id)
    {
        var result = await _financeService.DeleteFinanceAsync(id);
        if (!result)
        {
            return NotFound($"Finance with id : {id} not found");
        }
        return NoContent();
    }
}