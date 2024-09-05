namespace UniversityManagament.Services;

public class AiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ILogger<AiService> _logger;

    public AiService(HttpClient httpClient, IConfiguration configuration, ILogger<AiService> logger)
    {
        _httpClient = httpClient;
        _apiKey = configuration["OpenAI:ApiKey"];
        _logger = logger;
    }

    public async Task<string> GetAnswerAsync(string question)
    {
        var request = new
        {
            model = "gpt-3.5-turbo",
            prompt = question,
            max_tokens = 150
        };

        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

        try
        {
            var response = await _httpClient.PostAsJsonAsync("https://api.openai.com/v1/completions", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
                return result.choices[0].text.Trim();
            }

            var errorResponse = await response.Content.ReadAsStringAsync();
            _logger.LogError($"OpenAI API request failed with status code {response.StatusCode}: {errorResponse}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while calling OpenAI API");
        }

        return null;
    }
}

public class OpenAIResponse
{
    public Choice[] choices { get; set; }
}

public class Choice
{
    public string text { get; set; }
}