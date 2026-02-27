using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BPFC_System
{
    internal class ExcelExportRandomCheck
    {
        public void SaveExcel(DataTable dt, string filePath)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Data detail");

                    int startColumn = 6;

                    MergeAndFillMachineNames(worksheet, startColumn);
                    MergeAndFillDataNames(worksheet, startColumn);
                    MergeAndFillTopHeader(worksheet);

                    AddHeaders(worksheet, dt);
                    AddData(worksheet, dt);

                    ApplyAdditionalFormatting(worksheet, dt);
                    ApplyFailFormatting(worksheet, dt);

                    // Điều chỉnh độ rộng của tất cả các cột theo nội dung
                    worksheet.Cells.AutoFitColumns();

                    // Đặt kích thước tối thiểu cho tất cả các cột là 100 pixel
                    SetMinimumColumnWidth(worksheet, 100);

                    // Thêm sheet "Result" và xuất dữ liệu
                    AddResultSheet(package, dt);

                    // Tạo tên tệp duy nhất
                    string uniqueFilePath = GetUniqueFilePath(filePath);

                    var fi = new FileInfo(uniqueFilePath);
                    package.SaveAs(fi);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            MessageBox.Show("File saved successfully!");
        }

        private void SetMinimumColumnWidth(ExcelWorksheet worksheet, int minWidthInPixels)
        {
            double minWidthInCharacters = minWidthInPixels / 7.0;
            for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
            {
                if (worksheet.Column(col).Width < minWidthInCharacters)
                {
                    worksheet.Column(col).Width = minWidthInCharacters;
                }
            }
        }

        private string GetUniqueFilePath(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);

            int counter = 1;
            string uniqueFilePath = filePath;

            while (File.Exists(uniqueFilePath))
            {
                string numberedFileName = $"{fileNameWithoutExtension} ({counter}){extension}";
                uniqueFilePath = Path.Combine(directory, numberedFileName);
                counter++;
            }

            return uniqueFilePath;
        }

        private void MergeAndFillMachineNames(ExcelWorksheet worksheet, int startColumn)
        {
            string[] machineNames = { "Máy 1", "Máy 2", "Máy 3" };
            Color[] machineColors = { Color.AliceBlue, Color.LightGreen, Color.Orange };

            for (int i = 0; i < machineNames.Length; i++)
            {
                MergeAndFillRow(worksheet, 1, startColumn + i * 12, startColumn + (i + 1) * 12 - 1, machineNames[i], machineColors[i]);
            }
        }

        private void MergeAndFillDataNames(ExcelWorksheet worksheet, int startColumn)
        {
            string[,] dataNames = {
                { "Thời gian", "Nhiệt độ", "Hóa chất" },
                { "Thời gian", "Nhiệt độ", "Hóa chất" },
                { "Thời gian", "Nhiệt độ", "Hóa chất" }
            };
            Color[] dataColors = { Color.LightYellow, Color.LightBlue, Color.LightGray };

            for (int i = 0; i < dataNames.GetLength(0); i++)
            {
                for (int j = 0; j < dataNames.GetLength(1); j++)
                {
                    MergeAndFillRow(worksheet, 2, startColumn + (i * 12) + j * 4, startColumn + (i * 12) + (j + 1) * 4 - 1, dataNames[i, j], dataColors[j]);
                }
            }
        }

        private void MergeAndFillTopHeader(ExcelWorksheet worksheet)
        {
            var mergeRange = worksheet.Cells["A1:E2"];
            mergeRange.Merge = true;

            mergeRange.Value = "Biểu kiểm tra TUÂN THỦ QUY TRÌNH BPFC\nBPFC compliance checksheet";

            mergeRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            mergeRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            mergeRange.Style.WrapText = true;
            mergeRange.Style.Font.Size = 16;
            mergeRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
        }

        private void AddHeaders(ExcelWorksheet worksheet, DataTable dt)
        {
            string[] headerNames = {
                "Ngày\nDate", "Chuyền\nLine", "Art\nArticle", "Hình thể\nModel", "Bộ vị\nComponent",
                "Tiêu chuẩn\nStandard", "QIP Báo cáo\nQIP Report", "ME Kiểm tra\nME Recheck", "Kết quả\nResult",
                "Tiêu chuẩn\nStandard", "QIP Báo cáo\nQIP Report", "ME Kiểm tra\nME Recheck", "Kết quả\nResult",
                "Tiêu chuẩn\nStandard", "QIP Báo cáo\nQIP Report", "ME Kiểm tra\nME Recheck", "Kết quả\nResult",
                "Tiêu chuẩn\nStandard", "QIP Báo cáo\nQIP Report", "ME Kiểm tra\nME Recheck", "Kết quả\nResult",
                "Tiêu chuẩn\nStandard", "QIP Báo cáo\nQIP Report", "ME Kiểm tra\nME Recheck", "Kết quả\nResult",
                "Tiêu chuẩn\nStandard", "QIP Báo cáo\nQIP Report", "ME Kiểm tra\nME Recheck", "Kết quả\nResult",
                "Tiêu chuẩn\nStandard", "QIP Báo cáo\nQIP Report", "ME Kiểm tra\nME Recheck", "Kết quả\nResult",
                "Tiêu chuẩn\nStandard", "QIP Báo cáo\nQIP Report", "ME Kiểm tra\nME Recheck", "Kết quả\nResult",
                "Tiêu chuẩn\nStandard", "QIP Báo cáo\nQIP Report", "ME Kiểm tra\nME Recheck", "Kết quả\nResult"
            };

            Color[] colors = { Color.LightYellow, Color.LightBlue, Color.LightGray };
            int colorIndex = 0;

            for (int i = 0; i < headerNames.Length; i++)
            {
                worksheet.Cells[3, i + 1].Value = headerNames[i];
                worksheet.Cells[3, i + 1].Style.Font.Bold = true;
                worksheet.Cells[3, i + 1].Style.Font.Size = 12;
                worksheet.Cells[3, i + 1].Style.WrapText = true;
                worksheet.Cells[3, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[3, i + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[3, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                if (i >= 5)
                {
                    if ((i - 5) % 4 == 0 && (i - 5) > 0)
                    {
                        colorIndex = (colorIndex + 1) % colors.Length;
                    }
                    worksheet.Cells[3, i + 1].Style.Fill.BackgroundColor.SetColor(colors[colorIndex]);
                }
                else
                {
                    worksheet.Cells[3, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                }

                worksheet.Cells[3, i + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[3, i + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[3, i + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[3, i + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            }
        }

        private void AddData(ExcelWorksheet worksheet, DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var dataRow = dt.Rows[i];
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    var cell = worksheet.Cells[i + 4, j + 1];
                    cell.Value = dataRow[j]?.ToString();
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
            }

            for (int col = 1; col <= 4; col++)
            {
                for (int row = 4; row < dt.Rows.Count + 3; row += 2)
                {
                    var mergeRange = worksheet.Cells[row, col, row + 1, col];
                    mergeRange.Merge = true;
                    mergeRange.Value = worksheet.Cells[row, col].Value;
                    mergeRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    mergeRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[row + 1, col].Value = null;
                }
            }
        }
        private void ApplyFailFormatting(ExcelWorksheet worksheet, DataTable dt)
        {
            int[] failColumns = { 9, 13, 17, 21, 25, 29, 33, 37, 41 };

            foreach (int column in failColumns)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // Kiểm tra giá trị trong cột có chứa "FAIL" không
                    var cell = worksheet.Cells[i + 4, column];
                    if (cell.Value != null && cell.Value.ToString().Contains("FAIL"))
                    {
                        // Nếu ô chứa "FAIL", tô màu nền và đặt màu chữ là màu đỏ
                        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cell.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                        cell.Style.Font.Color.SetColor(Color.Red);
                    }
                }
            }
        }


        private void ApplyAdditionalFormatting(ExcelWorksheet worksheet, DataTable dt)
        {
            var dataRange = worksheet.Cells[4, 1, dt.Rows.Count + 3, dt.Columns.Count];
            dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            Color[] alternatingColors = { Color.AliceBlue, Color.White };
            int colorIndex = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = worksheet.Cells[i + 4, 1, i + 4, dt.Columns.Count];
                row.Style.Fill.PatternType = ExcelFillStyle.Solid;

                row.Style.Fill.BackgroundColor.SetColor(alternatingColors[colorIndex]);

                if ((i + 1) % 2 == 0)
                {
                    colorIndex = (colorIndex + 1) % alternatingColors.Length;
                }
            }
        }

        private void MergeAndFillRow(ExcelWorksheet worksheet, int row, int startColumn, int endColumn, string value, Color color)
        {
            var mergeRange = worksheet.Cells[row, startColumn, row, endColumn];
            mergeRange.Merge = true;
            mergeRange.Value = value;
            mergeRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            mergeRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            mergeRange.Style.Fill.BackgroundColor.SetColor(color);
            mergeRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            mergeRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            mergeRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            mergeRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            mergeRange.Style.Font.Size = 16;
            mergeRange.Style.Font.Bold = true;
        }
        private void AddResultSheet(ExcelPackage package, DataTable dt)
        {
            var resultSheet = package.Workbook.Worksheets.Add("Result");

            string[] resultHeaders = { "Date", "Line", "Article", "Model", "Máy oven 1\nHeating 1", "Máy oven 2\nHeating 2", "Máy oven 3\nHeating 3", "Lý do\nReason" };

            for (int i = 0; i < resultHeaders.Length; i++)
            {
                resultSheet.Cells[1, i + 1].Value = resultHeaders[i];
                resultSheet.Cells[1, i + 1].Style.Font.Bold = true;
                resultSheet.Cells[1, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                resultSheet.Cells[1, i + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                resultSheet.Cells[1, i + 1].Style.WrapText = true;
            }

            // Dictionary để lưu trữ các giá trị duy nhất của cột "Date" và hàng tương ứng
            Dictionary<string, int> uniqueDateRows = new Dictionary<string, int>();

            // Duyệt qua các giá trị trong cột "Date" và ghi lại giá trị đầu tiên của mỗi nhóm vào Dictionary
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string currentDateValue = dt.Rows[i]["Date"]?.ToString();

                if (!uniqueDateRows.ContainsKey(currentDateValue))
                {
                    // Nếu giá trị Date chưa tồn tại trong Dictionary, thêm vào
                    uniqueDateRows.Add(currentDateValue, i + 2); // Dòng bắt đầu từ 2 (dòng header là 1)
                }
            }

            // Duyệt qua các phần tử trong Dictionary và gộp các ô tương ứng trong cột "Date"
            foreach (var kvp in uniqueDateRows)
            {
                int startRow = kvp.Value;
                int endRow = startRow;

                // Tìm dòng cuối cùng của nhóm
                while (endRow < dt.Rows.Count + 2 && dt.Rows[endRow - 2]["Date"].ToString() == kvp.Key)
                {
                    endRow++;
                }

                // Gộp các ô trong nhóm và điền giá trị đầu tiên vào ô gộp
                var mergeRange = resultSheet.Cells[startRow, 1, endRow - 1, 1];
                mergeRange.Merge = true;
                mergeRange.Value = kvp.Key;
                mergeRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                mergeRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                // Gán giá trị "Matching" cho các ô sau khi merge trong các cột "Máy oven 1\nHeating 1", "Máy oven 2\nHeating 2", "Máy oven 3\nHeating 3"
                for (int col = 5; col <= 7; col++)
                {
                    for (int row = startRow; row < endRow; row++)
                    {
                        resultSheet.Cells[row, col].Value = "Matching";
                    }
                }
            }

            int currentRow = 2; // Dòng bắt đầu từ 2 (dòng header là 1)

            // Ghi các giá trị còn lại từ DataTable vào sheet "Result"
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                resultSheet.Cells[currentRow, 2].Value = dt.Rows[i]["Line"]?.ToString();
                resultSheet.Cells[currentRow, 3].Value = dt.Rows[i]["Article"]?.ToString();
                resultSheet.Cells[currentRow, 4].Value = dt.Rows[i]["Model"]?.ToString();

                // Tăng chỉ số hàng lên 1 để chuyển đến dòng tiếp theo
                currentRow++;
            }

            // Merge các ô của các cột từ 2 đến 8
            for (int col = 2; col <= 8; col++)
            {
                for (int row = 2; row < dt.Rows.Count + 2; row += 2)
                {
                    if (row + 1 <= dt.Rows.Count + 2) // Kiểm tra để tránh trường hợp dòng cuối cùng ít hơn 2 dòng
                    {
                        var mergeRange = resultSheet.Cells[row, col, row + 1, col];
                        mergeRange.Merge = true;
                        mergeRange.Value = resultSheet.Cells[row, col].Value;
                        mergeRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        mergeRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        resultSheet.Cells[row + 1, col].Value = "Matching"; 
                    }
                }
            }

            // Auto fit columns và đặt kích thước tối thiểu cho các cột
            resultSheet.Cells.AutoFitColumns();
            SetMinimumColumnWidth(resultSheet, 100);

            // Áp dụng định dạng cho sheet "Result"
            ApplyResultSheetFormatting(resultSheet, dt.Rows.Count, resultHeaders.Length);
        }

        private void ApplyResultSheetFormatting(ExcelWorksheet resultSheet, int rowCount, int columnCount)
        {
            var headerRange = resultSheet.Cells[1, 1, 1, columnCount];
            headerRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            headerRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            headerRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            headerRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

            var dataRange = resultSheet.Cells[2, 1, rowCount + 1, columnCount];
            dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            Color[] alternatingColors = { Color.White, Color.AliceBlue };
            int colorIndex = 0;

            for (int i = 0; i < rowCount; i++)
            {
                var row = resultSheet.Cells[i + 2, 1, i + 2, columnCount];
                row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                row.Style.Fill.BackgroundColor.SetColor(alternatingColors[colorIndex]);

                if ((i + 1) % 2 == 0)
                {
                    colorIndex = (colorIndex + 1) % alternatingColors.Length;
                }
            }
        }
    }
}
