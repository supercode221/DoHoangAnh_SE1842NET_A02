using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace FUNewsManagement_AnalyticsAPI.Utils.EPPlus
{
    public static class ExcelUtils
    {
        public static byte[] ExportToExcel(DataTable table)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Data");
                worksheet.Cells["A1"].LoadFromDataTable(table, true);
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                using (var range = worksheet.Cells[1, 1, 1, table.Columns.Count])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                return package.GetAsByteArray();
            }
        }
    }
}
