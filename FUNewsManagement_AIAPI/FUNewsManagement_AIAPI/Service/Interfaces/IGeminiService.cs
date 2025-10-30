namespace FUNewsManagement_AIAPI.Service.Interfaces
{
    public interface IGeminiService
    {
        Task<List<string>> GenerateTagsAsync(string content);
    }
}
