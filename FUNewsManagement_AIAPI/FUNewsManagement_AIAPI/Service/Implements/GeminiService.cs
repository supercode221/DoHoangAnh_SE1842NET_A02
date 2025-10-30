using System.Text;
using System.Text.Json;
using FUNewsManagement_AIAPI.Models;
using FUNewsManagement_AIAPI.Service.Interfaces;

namespace FUNewsManagement_AIAPI.Service.Implements
{
    public class GeminiService : IGeminiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;
        private readonly ILogger<GeminiService> _logger;
        private const int MaxContentLength = 3000;
        private const int MaxRetries = 2;

        public GeminiService(
            IHttpClientFactory httpClientFactory,
            IConfiguration config,
            ILogger<GeminiService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = config["Gemini:ApiKey"]
                ?? throw new InvalidOperationException("Gemini API key not configured");
            _logger = logger;
        }

        public async Task<List<string>> GenerateTagsAsync(string content)
        {
            var prompt = BuildPrompt(content);

            for (int attempt = 0; attempt <= MaxRetries; attempt++)
            {
                try
                {
                    var responseText = await CallGeminiApiAsync(prompt);
                    var tags = ExtractTagsFromResponse(responseText);

                    if (tags.Count > 0)
                    {
                        _logger.LogInformation(
                            "Successfully generated {Count} tags.",
                            tags.Count);
                        return tags;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex,
                        "Attempt {Attempt} failed to generate tags", attempt + 1);

                    if (attempt == MaxRetries)
                        throw;

                    await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)));
                }
            }

            return new List<string>();
        }

        private string BuildPrompt(string content)
        {
            var truncatedContent = content.Length > MaxContentLength
                ? content.Substring(0, MaxContentLength) + "..."
                : content;

            return $@"You are an expert tag suggestion engine for a news platform.

Analyze the following article and generate relevant tags.

REQUIREMENTS:
- Generate between 5 and 10 tags
- Each tag must be 1-3 words maximum
- Tags should capture main topics, themes, entities, and categories
- Prioritize specific, actionable tags over generic ones
- Return ONLY a valid JSON array of strings with no additional text

ARTICLE:
Content: {truncatedContent}

RESPONSE FORMAT (return exactly this structure):
[""tag1"", ""tag2"", ""tag3""]";
        }

        private async Task<string> CallGeminiApiAsync(string prompt)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30);

            var requestBody = new GeminiRequest
            {
                Contents = new List<GeminiContent>
                {
                    new GeminiContent
                    {
                        Parts = new List<GeminiPart>
                        {
                            new GeminiPart { Text = prompt }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(requestBody, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash-exp:generateContent?key={_apiKey}";

            var response = await httpClient.PostAsync(url, requestContent);
            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Gemini API returned error: {StatusCode} - {Response}",
                    response.StatusCode, responseText);
                throw new HttpRequestException(
                    $"Gemini API error: {response.StatusCode}");
            }

            return responseText;
        }

        private List<string> ExtractTagsFromResponse(string responseText)
        {
            try
            {
                using var doc = JsonDocument.Parse(responseText);
                var textContent = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString() ?? string.Empty;

                return ParseTags(textContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to parse Gemini response: {Response}", responseText);
                throw new InvalidOperationException("Failed to parse AI response", ex);
            }
        }

        private List<string> ParseTags(string text)
        {
            text = text.Trim();

            if (text.StartsWith("```json", StringComparison.OrdinalIgnoreCase))
            {
                var endIndex = text.LastIndexOf("```");
                if (endIndex > 7)
                    text = text.Substring(7, endIndex - 7).Trim();
            }
            else if (text.StartsWith("```"))
            {
                var endIndex = text.LastIndexOf("```");
                if (endIndex > 3)
                    text = text.Substring(3, endIndex - 3).Trim();
            }

            try
            {
                var tags = JsonSerializer.Deserialize<List<string>>(text);
                if (tags != null && tags.Count > 0)
                {
                    return CleanAndValidateTags(tags);
                }
            }
            catch
            {
                _logger.LogWarning("Direct JSON parsing failed, trying fallback parsing");
            }

            return ParseTagsFallback(text);
        }

        private List<string> ParseTagsFallback(string text)
        {
            var tags = text
                .Replace("[", "")
                .Replace("]", "")
                .Replace("\"", "")
                .Replace("'", "")
                .Split(new[] { ',', '\n', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .ToList();

            return CleanAndValidateTags(tags);
        }

        private List<string> CleanAndValidateTags(List<string> tags)
        {
            return tags
                .Select(t => t.Trim())
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Where(t => t.Length >= 2 && t.Length <= 50)
                .Where(t => t.Split(' ').Length <= 3)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Take(10)
                .ToList();
        }
    }
}
