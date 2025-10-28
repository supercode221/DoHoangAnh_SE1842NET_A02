namespace FUNewsManagement_CoreAPI.BLL.Models
{
    public static class TokenStore
    {
        public static Dictionary<string, string> RefreshTokens = new(); // email → refresh token
    }
}
