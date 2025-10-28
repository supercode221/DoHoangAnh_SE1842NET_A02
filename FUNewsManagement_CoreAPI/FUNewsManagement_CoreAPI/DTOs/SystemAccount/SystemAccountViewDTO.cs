namespace FUNewsManagement_CoreAPI.BLL.DTOs.SystemAccount
{
    public class SystemAccountViewDTO
    {
        public short AccountId { get; set; }

        public string? AccountName { get; set; }

        public string? AccountEmail { get; set; }

        public int? AccountRole { get; set; }
    }
}
