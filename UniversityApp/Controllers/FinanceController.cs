using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Repository.IRepository;
using UniversityApp.Services.Interfaces;
using UniversityManagament.Models.Dto;

namespace UniversityApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FinanceController : ControllerBase
{
    private readonly IFinanceService _financeService;
    private readonly IFacultyRepository _facultyRepository;
    private readonly ILogger<FinanceController> _logger;

    public FinanceController(IFinanceService financeService, IFacultyRepository facultyRepository, ILogger<FinanceController> logger)
    {
        _financeService = financeService;
        _facultyRepository = facultyRepository;
        _logger = logger;
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FinanceReadDto>>> GetAllFinances()
    {
        try
        {
            var finances = await _financeService.GetAllFinancesAsync();
            return Ok(finances);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching universities.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        } 
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("{id}")]
    public async Task<ActionResult<FinanceReadDto>> GetFinanceById(Guid id)
    {
        try
        {
            var finance = await _financeService.GetFinanceByIdAsync(id);
            if (finance == null)
            {
                return NotFound($"Finance with id : {id} not found");
            }

            return Ok(finance);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching universities.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        } 
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPost]
    public async Task<ActionResult<FinanceReadDto>> CreateFinance(FinanceCreateDto financeCreateDto)
    {
        try
        {
            if (await _facultyRepository.GetFacultyByIdAsync(financeCreateDto.FacultyId) == null)
                return NotFound($"Faculty with id : {financeCreateDto.FacultyId} not found");

            var financeReadDto = await _financeService.CreateFinanceAsync(financeCreateDto);
            //return CreatedAtAction(nameof(GetFinanceById), new { id = financeReadDto.Id }, financeReadDto);
            return Ok(financeReadDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching universities.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        } 
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFinance(Guid id, FinanceCreateDto financeUpdateDto)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching universities.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        } 
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFinance(Guid id)
    {
        try
        {
            var result = await _financeService.DeleteFinanceAsync(id);
            if (!result)
            {
                return NotFound($"Finance with id : {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching universities.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        } 
    }
}