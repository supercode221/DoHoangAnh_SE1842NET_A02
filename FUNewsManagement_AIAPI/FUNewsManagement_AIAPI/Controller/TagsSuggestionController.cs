using FUNewsManagement_AIAPI.Models;
using FUNewsManagement_AIAPI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagement_AIAPI.Controller
{
    [Route("api/ai")]
    [ApiController]
    public class TagsSuggestionController : ControllerBase
    {
        private readonly IGeminiService _geminiService;
        private readonly ILogger<TagsSuggestionController> _logger;
        private const int MaxContentLength = 10000;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);

        public TagsSuggestionController(
            IGeminiService geminiService,
            ILogger<TagsSuggestionController> logger)
        {
            _geminiService = geminiService;
            _logger = logger;
        }

        [HttpPost("suggest-tags")]
        public async Task<IActionResult> SuggestTags([FromBody] TagSuggestionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return BadRequest(new { error = "Article content cannot be empty" });
            }

            if (request.Content.Length > MaxContentLength)
            {
                return BadRequest(new
                {
                    error = $"Content exceeds maximum length of {MaxContentLength} characters",
                    currentLength = request.Content.Length,
                    maxLength = MaxContentLength
                });
            }

            try
            {
                var tags = await _geminiService.GenerateTagsAsync(request.Content);

                if (tags.Count == 0)
                {
                    _logger.LogWarning("No tags generated.");
                    return Ok(new TagSuggestionResponse
                    {
                        Tags = new List<string>(),
                        Message = "No suitable tags could be generated"
                    });
                }

                return Ok(new TagSuggestionResponse
                {
                    Tags = tags,
                    Cached = false,
                    Message = "Tags generated successfully"
                });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to connect to Gemini API");
                return StatusCode(503, new
                {
                    error = "AI service temporarily unavailable. Please try again later."
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Failed to process AI response");
                return StatusCode(500, new
                {
                    error = "Failed to generate tags. Please try again."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while generating tags");
                return StatusCode(500, new
                {
                    error = "An unexpected error occurred. Please try again later."
                });
            }
        }
    }
}
