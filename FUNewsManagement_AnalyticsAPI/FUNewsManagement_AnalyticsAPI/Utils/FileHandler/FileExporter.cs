namespace FUNewsManagement_AnalyticsAPI.Utils.FileHandler
{
    public static class FileExporter
    {
        public static async Task<string> ExportToPathAsync(byte[] fileBytes, string fileName, string extension = ".xlsx")
        {
            var baseDir = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName
                          ?? AppContext.BaseDirectory;
            string exportFolder = Path.Combine(baseDir, "Exports");

            Directory.CreateDirectory(exportFolder);

            string filePath = Path.Combine(exportFolder, $"{fileName}_{DateTime.Now:yyyyMMdd_HHmmss}{extension}");

            await File.WriteAllBytesAsync(filePath, fileBytes);

            return filePath;
        }

    }
}
