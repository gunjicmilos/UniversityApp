using Microsoft.AspNetCore.Mvc;
using UniversityManagament.Services;

namespace UniversityManagament.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatBotController : ControllerBase
{
    private readonly AiService _openAIService;
    private readonly ILogger<ChatBotController> _logger;

    public ChatBotController(AiService openAIService, ILogger<ChatBotController> logger)
    {
        _openAIService = openAIService;
        _logger = logger;
    }

    [HttpPost("ask")]
    public async Task<IActionResult> AskQuestion([FromBody] QuestionRequest request)
    {
        var answer = await _openAIService.GetAnswerAsync(request.Question);
        if (answer != null)
        {
            return Ok(new { answer });
        }

        _logger.LogError("Unable to get answer from OpenAI.");
        return BadRequest("Unable to get answer from OpenAI.");
    }
}

public class QuestionRequest
{
    public string Question { get; set; }
}