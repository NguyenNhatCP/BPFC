using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace BPFC_System
{
    internal class ExcelExporter : IDisposable
    {
        private readonly ExcelPackage excelPackage;
        private ExcelWorksheet worksheet;
        private string filePath;

        private const int StartRowForHeaders = 4;
        private const int StartRowForDailyMonthlyTitle = 1;
        private const int EndRowForDailyMonthlyTitle = 3;

        public ExcelExporter(string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            this.filePath = filePath;
            excelPackage = new ExcelPackage();
        }

        public ExcelPackage GetExcelPackage() => excelPackage;

        public void AddDataTableToSheet(DataTable dataTable, string sheetName)
        {
            if (ShouldIncludeSheet(sheetName))
            {
                worksheet = excelPackage.Workbook.Worksheets.Add(sheetName);
                PopulateSheetWithData(dataTable);
                FormatSheet(worksheet, sheetName);
            }
        }

        private static bool ShouldIncludeSheet(string sheetName)
        {
            return sheetName == "Monthly Report" || sheetName == "Daily Report" || sheetName.StartsWith("Plant");
        }

        private void SetBorderAndGridlines(ExcelWorksheet sheet)
        {
            sheet.View.ShowGridLines = false;

            if (sheet.Dimension != null)
            {
                ExcelRange dataRange = sheet.Cells[sheet.Dimension.Address];

                dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                sheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            for (int row = 1; row <= 4; row++)
            {
                for (int col = 1; col <= sheet.Dimension.End.Column; col++)
                {
                    sheet.Cells[row, col].Style.WrapText = true;
                }
            }
        }

        private void AdjustColumnWidth(ExcelWorksheet sheet, string sheetName)
        {
            int minimumColumnWidth;

            switch (sheetName)
            {
                case "Monthly Report":
                    minimumColumnWidth = 10;
                    break;

                case "Daily Report":
                case string s when s.StartsWith("Plant"):
                    minimumColumnWidth = 15;
                    break;

                default:
                    minimumColumnWidth = 15;
                    break;
            }

            for (int col = sheet.Dimension.Start.Column; col <= sheet.Dimension.End.Column; col++)
            {
                sheet.Column(col).Width = minimumColumnWidth;
            }

            sheet.Cells.AutoFitColumns();

            // Kiểm tra và đặt lại kích thước cột nếu nhỏ hơn giá trị tối thiểu
            for (int col = sheet.Dimension.Start.Column; col <= sheet.Dimension.End.Column; col++)
            {
                double autofitWidth = sheet.Column(col).Width;
                if (autofitWidth < minimumColumnWidth)
                {
                    sheet.Column(col).Width = minimumColumnWidth;
                }
            }
        }

        private void PopulateSheetWithData(DataTable dataTable)
        {
            // Sắp xếp DataTable theo FirstLetter và LineNumber
            DataView dataView = dataTable.DefaultView;
            dataView.Sort = "FirstLetter ASC, LineNumber ASC";
            DataTable sortedDataTable = dataView.ToTable();

            int offset = 0;
            int startRow = StartRowForHeaders;

            for (int i = 0; i < sortedDataTable.Columns.Count; i++)
            {
                string columnName = sortedDataTable.Columns[i].ColumnName;

                if (!columnName.Equals("LineID") && !columnName.Equals("LineNumber") && !columnName.Equals("FirstLetter"))
                {
                    AddHeaderRow(startRow, offset, columnName);
                    AddDataRows(startRow + 1, offset, sortedDataTable.Rows, i);
                    offset++;
                }
            }

            foreach (DataRow row in sortedDataTable.Rows)
            {
                string lineName = row["LineName"].ToString();
                string lineNumber = row["LineNumber"].ToString();

                if (lineName.Equals("Adiracer", StringComparison.OrdinalIgnoreCase) && lineNumber.Contains("2"))
                {
                    Console.WriteLine($"Found matching row: LineName = {lineName}, LineNumber = {lineNumber}");
                }
            }
        }

        private void AddHeaderRow(int startRow, int offset, string columnName)
        {
            ExcelRange headerCell = worksheet.Cells[startRow, offset + 1];
            headerCell.Value = TranslateColumnNameToVietnamese(columnName);
        }

        private void AddDataRows(int startRow, int offset, DataRowCollection rows, int columnIndex)
        {
            int rowCount = rows.Count;
            int col = offset + 1;

            for (int j = 0; j < rowCount; j++)
            {
                ExcelRange dataCell = worksheet.Cells[startRow + j, col];
                dataCell.Value = rows[j][columnIndex];
            }
        }
        public void FormatSheet(ExcelWorksheet sheet, string sheetName)
        {
            if (sheetName == "Monthly Report")
            {
                FormatMonthlyReportSheet(sheet);
                SetBorderAndGridlines(sheet);
                FindAndMergeDuplicateCellsForAllRows(sheet);
            }
            else if (sheetName == "Daily Report")
            {
                FormatDailyReportSheet(sheet);
                SetBorderAndGridlines(sheet);
                FindAndMergeDuplicateCellsForAllRows(sheet);
            }
            else if (sheetName.StartsWith("Plant"))
            {
                ColorAlternate2Rows(sheet);

                FormatPlantSheet(sheet);

                MergeAdjacentCellsInColumns(sheet);

                MergeAndFillHeaders(sheet);

                MergeAndFillMachineNames(sheet);

                MergeAndClearCells(sheet, 2, 1, 3, 4);

                ColorAlternateRows4(sheet);

                ColorAlternateResultCol(sheet);

                AddTitleToPlantSheet(sheet);

                SetBorderAndGridlines(sheet);


            }
            AdjustColumnWidth(sheet, sheetName);
        }
        private void FormatDailyReportSheet(ExcelWorksheet sheet)
        {
            AddTitleToDailySheet(sheet);
            ExcelRange dataRange = sheet.Cells[5, 1, 5, sheet.Dimension.End.Column];
            dataRange.Style.Font.Size = 11;
            AdjustPropertiesRow4Daily(sheet);
            sheet.View.FreezePanes(9, 3);
            ColorAlternate1Rows(sheet);
            MergeAndClearCells(sheet, 4, 1, 8, 1);
            ColorDailyReportCells(sheet);
            ColorRow5BasedOnCondition(sheet);
        }

        private void FormatMonthlyReportSheet(ExcelWorksheet sheet)
        {
            AddTitleToMonthlySheet(sheet);
            ExcelRange dataRange = sheet.Cells[5, 1, 5, sheet.Dimension.End.Column];
            dataRange.Style.Font.Size = 11;
            AdjustPropertiesRow4Monthly(sheet);
            sheet.View.FreezePanes(9, 3);
            ColorAlternate1Rows(sheet);
            MergeAndClearCells(sheet, 4, 1, 8, 1);
            ColorMonthlyReportCells(sheet);
            ColorRow5BasedOnCondition(sheet);
        }

        private void AdjustPropertiesRow4Daily(ExcelWorksheet sheet)
        {
            int lastDataCol = sheet.Dimension.End.Column;

            // Đặt font size cho dòng 4
            sheet.Row(4).Style.Font.Size = 12;
            sheet.Row(5).Style.Font.Size = 12;
            sheet.Row(6).Style.Font.Size = 12;
            sheet.Row(7).Style.Font.Size = 12;
            sheet.Row(8).Style.Font.Size = 12;

            sheet.Row(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            // Đặt màu nền cho dòng 4
            sheet.Cells[4, 2, 4, lastDataCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells[4, 2, 4, lastDataCol].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);

            sheet.Cells[5, 2, 5, lastDataCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells[5, 2, 5, lastDataCol].Style.Fill.BackgroundColor.SetColor(Color.Beige);

            sheet.Cells[7, 2, 7, lastDataCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells[7, 2, 7, lastDataCol].Style.Fill.BackgroundColor.SetColor(Color.Beige);

        }

        private void ColorRow5BasedOnCondition(ExcelWorksheet sheet)
        {
            int lastDataCol = sheet.Dimension.End.Column;

            for (int col = 3; col <= lastDataCol; col++)
            {
                var cellValue = sheet.Cells[5, col].Text;

                // Kiểm tra nếu ô không chứa "100.0%" hoặc "OFF DAY", thì tô màu chữ thành đỏ
                if (!cellValue.Contains("100.0%") && !cellValue.Contains("OFF DAY"))
                {
                    sheet.Cells[5, col].Style.Font.Color.SetColor(Color.Red);
                }
            }
        }
        private void ColorMonthlyReportCells(ExcelWorksheet sheet)
        {
            int lastDataRow = sheet.Dimension.End.Row;
            int lastDataCol = sheet.Dimension.End.Column;

            for (int row = 1; row <= lastDataRow; row++)
            {
                for (int col = 1; col <= lastDataCol; col++)
                {
                    var cellValue = sheet.Cells[row, col].Text;

                    // Kiểm tra nếu ô chứa chữ "OFF DAY" thì đặt màu nền và màu chữ
                    if (cellValue.Contains("OFF DAY"))
                    {
                        sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(Color.Orange);
                        sheet.Cells[row, col].Style.Font.Color.SetColor(Color.Black); 
                    }

                    // Kiểm tra nếu ô chứa chữ "FAIL" thì đặt màu nền và màu chữ
                    if (cellValue.Contains("FAIL"))
                    {
                        sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet.Cells[row, col].Style.Font.Color.SetColor(Color.Red); 
                    }
                }
            }
        }

        private void ColorDailyReportCells(ExcelWorksheet sheet)
        {
            int lastDataRow = sheet.Dimension.End.Row;
            int lastDataCol = sheet.Dimension.End.Column;

            for (int row = 1; row <= lastDataRow; row++)
            {
                for (int col = 1; col <= lastDataCol; col++)
                {
                    var cellValue = sheet.Cells[row, col].Text;

                    if (cellValue.Contains("FAIL"))
                    {
                        sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet.Cells[row, col].Style.Font.Color.SetColor(Color.Red);
                    }
                }
            }
        }

        private void AdjustPropertiesRow4Monthly(ExcelWorksheet sheet)
        {
            int lastDataCol = sheet.Dimension.End.Column;

            sheet.Row(4).Style.Font.Size = 12;
            sheet.Row(5).Style.Font.Size = 12;
            sheet.Row(6).Style.Font.Size = 12;
            sheet.Row(7).Style.Font.Size = 12;
            sheet.Row(8).Style.Font.Size = 12;

            sheet.Row(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            sheet.Cells[4, 2, 4, lastDataCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells[4, 2, 4, lastDataCol].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);

            sheet.Cells[5, 2, 5, lastDataCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells[5, 2, 5, lastDataCol].Style.Fill.BackgroundColor.SetColor(Color.Beige);

            sheet.Cells[7, 2, 7, lastDataCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells[7, 2, 7, lastDataCol].Style.Fill.BackgroundColor.SetColor(Color.Beige);

            for (int col = 1; col <= lastDataCol; col++)
            {
                var cellValue = sheet.Cells[4, col].Text;

                if (cellValue.Contains("-20"))
                {
                    if (DateTime.TryParseExact(cellValue, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateValue))
                    {
                        sheet.Cells[4, col].Value = dateValue.ToString("dd-MM");
                    }
                }
            }
        }

        private void AddTitleToDailySheet(ExcelWorksheet sheet)
        {
            int startColumn = 1;
            int endColumn = 6;
            int startRow = 1;
            int endRow = 3;

            ExcelRange titleRange = sheet.Cells[startRow, startColumn, endRow, endColumn];
            titleRange.Merge = true;
            titleRange.Style.WrapText = true;
            titleRange.Value = "% Tuân thủ BPFC hàng ngày\nDaily BPFC Compliance %";
            titleRange.Style.Font.Size = 24;
            titleRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            titleRange.Style.Fill.SetBackground(Color.AliceBlue);
            titleRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            titleRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Row(startRow).Height = 40;
        }

        private void AddTitleToMonthlySheet(ExcelWorksheet sheet)
        {
            int startColumn = 1;
            int endColumn = 33;
            int startRow = 1;
            int endRow = 3;

            ExcelRange titleRange = sheet.Cells[startRow, startColumn, endRow, endColumn];
            titleRange.Merge = true;
            titleRange.Style.WrapText = true;
            titleRange.Value = "% Tuân thủ BPFC hàng tháng\nMonthly BPFC Compliance %";
            titleRange.Style.Font.Size = 24;
            titleRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            titleRange.Style.Fill.SetBackground(Color.AliceBlue);
            titleRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            titleRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Row(startRow).Height = 40;
        }

        private void ClearValuesBeforeMerge(ExcelWorksheet sheet, int startRow, int endRow, int column)
        {
            for (int row = startRow; row <= endRow; row++)
            {
                var cell = sheet.Cells[row, column];
                cell.Value = null;
            }
        }

        private void MergeAndFillHeaders(ExcelWorksheet sheet)
        {
            int startRow = 3;
            int currentColumn = 5; // Start from column 5
            string[] headerLabels = { "Thời gian sấy\nHeating Time", "Nhiệt độ\nTemperature", "Hóa chất\nChemical" };
            Color[] headerColors = { Color.LightBlue, Color.LightSkyBlue, Color.LightSeaGreen };

            // Manually add 2 fixed header groups from column 5 to 10
            for (int i = 0; i < 2; i++)
            {
                int mergeStart = currentColumn;
                int mergeEnd = currentColumn + 2;

                ExcelRange mergeRange = sheet.Cells[startRow, mergeStart, startRow, mergeEnd];
                mergeRange.Merge = true;
                mergeRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                mergeRange.Style.Fill.BackgroundColor.SetColor(headerColors[i]);
                mergeRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                mergeRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                mergeRange.Style.WrapText = true;

                mergeRange.Value = headerLabels[i];
                mergeRange.Worksheet.Row(startRow).Height = 40;
                mergeRange.Style.Font.Size = 13;

                currentColumn += 3;
            }

            // Now continue from column 11 onward as usual
            int headerIndex = 0; // Start with "Hóa chất\nChemical"
            while (currentColumn <= sheet.Dimension.End.Column)
            {
                int mergeCount = Math.Min(sheet.Dimension.End.Column - currentColumn + 1, 3);
                ExcelRange mergeRange = sheet.Cells[startRow, currentColumn, startRow, currentColumn + mergeCount - 1];
                mergeRange.Merge = true;

                mergeRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                mergeRange.Style.Fill.BackgroundColor.SetColor(headerColors[headerIndex]);
                mergeRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                mergeRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                mergeRange.Style.WrapText = true;

                mergeRange.Value = headerLabels[headerIndex];
                mergeRange.Worksheet.Row(startRow).Height = 40;
                mergeRange.Style.Font.Size = 13;

                currentColumn += mergeCount;
                headerIndex = (headerIndex + 1) % headerLabels.Length;
            }
        }

        private void MergeAndClearCells(ExcelWorksheet sheet, int startRow, int startCol, int endRow, int endCol)
        {
            sheet.Cells[startRow, startCol, endRow, endCol].Clear();

            var mergeRange = sheet.Cells[startRow, startCol, endRow, endCol];
            mergeRange.Merge = true;
            mergeRange.Value = "";
        }

        private void MergeAndFillMachineNames(ExcelWorksheet sheet)
        {
            int startRow = 2;
            int currentColumn = 11;
            string[] machineNames = { "Máy sấy 1\nOven Machine 1", "Máy sấy 2\n Oven Machine 2", "Máy sấy 3\nOven Machine 3" };

            Color[] backgroundColors = { Color.Beige, Color.LightGray, Color.LightSteelBlue };
            int colorIndex = 0;

            // Thêm Heating Machine từ cột 5 đến 10
            ExcelRange heatingRange = sheet.Cells[startRow, 5, startRow, 10];
            heatingRange.Merge = true;
            heatingRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            heatingRange.Style.Fill.BackgroundColor.SetColor(Color.LightYellow);
            heatingRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            heatingRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            heatingRange.Style.Font.Size = 14;
            heatingRange.Value = "Máy định hình nóng\nHeating Machine";
            sheet.Row(startRow).Height = 50;

            for (int i = 0; i < machineNames.Length; i++)
            {
                int mergeCount = Math.Min(sheet.Dimension.End.Column - currentColumn + 1, 9);
                ExcelRange mergeRange = sheet.Cells[startRow, currentColumn, startRow, currentColumn + mergeCount - 1];

                mergeRange.Merge = true;
                mergeRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                mergeRange.Style.Fill.BackgroundColor.SetColor(backgroundColors[colorIndex]);
                mergeRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                mergeRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                mergeRange.Style.Font.Size = 14;
                mergeRange.Value = machineNames[i];
                sheet.Row(startRow).Height = 50;

                colorIndex = (colorIndex + 1) % backgroundColors.Length;
                currentColumn += mergeCount;
            }
        }

        private void ColorAlternateResultCol(ExcelWorksheet sheet)
        {
            int lastDataRow = sheet.Dimension.End.Row;
            int[] targetColumns = new int[] { 7, 10, 13, 16, 19, 22, 25, 28, 31 };

            foreach (int col in targetColumns)
            {
                for (int row = 5; row <= lastDataRow; row++)
                {
                    var cell = sheet.Cells[row, col];

                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cell.Style.Fill.BackgroundColor.SetColor(Color.AntiqueWhite);

                    if (cell.Text == "FAIL")
                    {
                        cell.Style.Font.Color.SetColor(Color.Red);
                    }
                }
            }
        }

        private void ColorAlternateRows4(ExcelWorksheet sheet)
        {
            int lastDataCol = sheet.Dimension.End.Column;

            Color[] fillColors = new Color[] { Color.LightGray, Color.LightYellow, Color.AliceBlue };

            for (int col = 1; col <= lastDataCol; col++)
            {
                Color fillColor = fillColors[col % fillColors.Length];

                sheet.Cells[4, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[4, col].Style.Fill.BackgroundColor.SetColor(fillColor);

                sheet.Cells[4, col].Style.Font.Size = 12;

                sheet.Row(4).Height = 35;
                sheet.Cells[4, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Cells[4, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
        }

        private void FormatPlantSheet(ExcelWorksheet sheet)
        {
            if (sheet.Dimension != null)
            {
                int startRow = 5;
                int startColumn = 1;
                int endRow = sheet.Dimension.End.Row;
                int endColumn = sheet.Dimension.End.Column;

                // Kiểm tra xem startRow, startColumn, endRow, endColumn có giá trị hợp lệ hay không
                if (startRow <= endRow && startColumn <= endColumn)
                {
                    ExcelRange dataRange = sheet.Cells[startRow, startColumn, endRow, endColumn];
                    dataRange.Style.Font.Size = 12;
                    dataRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    sheet.View.FreezePanes(5, 5);


                    string[] columnsToCheck = { "H", "N", "W", "AF" };
                    AddTemperatureToleranceToColumns(sheet, columnsToCheck);

                    string[] columnsToAdd = { "I", "O", "X", "AG" };
                    AddDegreeCelsiusSymbolToColumns(sheet, columnsToAdd);
                }
            }
        }

        private void AddDegreeCelsiusSymbolToColumns(ExcelWorksheet sheet, string[] columnsToAdd)
        {
            int startRow = 5;
            int endRow = sheet.Dimension.End.Row;

            foreach (string column in columnsToAdd)
            {
                // Convert column string to column index using the custom method
                int columnIndex = GetColumnIndexFromColumnName(sheet, column);

                for (int row = startRow; row <= endRow; row++)
                {
                    object cellValue = sheet.Cells[row, columnIndex].Value;

                    if (cellValue != null)
                    {
                        // Check if the cell is not empty before adding degree Celsius symbol
                        string currentValue = cellValue.ToString();
                        if (!string.IsNullOrEmpty(currentValue))
                        {
                            sheet.Cells[row, columnIndex].Value = $"{currentValue} °C";
                        }
                    }
                }
            }
        }

        private int GetColumnIndexFromColumnName(ExcelWorksheet sheet, string columnName)
        {
            int columnIndex = 0;
            int factor = 1;

            for (int i = columnName.Length - 1; i >= 0; i--)
            {
                columnIndex += (columnName[i] - 'A' + 1) * factor;
                factor *= 26;
            }

            return columnIndex;
        }


        private void AddTemperatureToleranceToColumns(ExcelWorksheet sheet, string[] columnsToCheck)
        {
            int startRow = 5;
            int endRow = sheet.Dimension.End.Row;

            foreach (string column in columnsToCheck)
            {
                int columnIndex = GetColumnIndexFromColumnName(sheet, column);

                for (int row = startRow; row <= endRow; row++)
                {
                    object cellValue = sheet.Cells[row, columnIndex].Value;

                    if (cellValue != null)
                    {
                        string currentValue = cellValue.ToString();
                        if (!string.IsNullOrEmpty(currentValue))
                        {
                            sheet.Cells[row, columnIndex].Value = $"{currentValue} ±5°C";
                        }
                    }
                }
            }
        }

        private void AddTitleToPlantSheet(ExcelWorksheet sheet)
        {
            int startColumn = 1;
            int endColumn = sheet.Dimension.End.Column;
            int startRow = 1;
            int endRow = 1;

            ExcelRange titleRange = sheet.Cells[startRow, startColumn, endRow, endColumn];
            titleRange.Merge = true;
            titleRange.Style.WrapText = true;
            titleRange.Value = "Biểu kiểm tra TUÂN THỦ QUY TRÌNH BPFC và máy sấy nóng - " + sheet.Name + "\nBPFC Compliance Checksheet and Heating Machine - " + sheet.Name.Replace("Plant", "Xưởng");
            titleRange.Style.Font.Size = 22;
            titleRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            titleRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            titleRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            titleRange.Style.Fill.BackgroundColor.SetColor(Color.AliceBlue);

            sheet.Row(startRow).Height = 60;
        }

        private void ColorAlternate1Rows(ExcelWorksheet sheet)
        {
            int startRow = 9;
            int lastDataRow = sheet.Dimension.End.Row;
            int lastDataCol = sheet.Dimension.End.Column;

            for (int row = startRow; row <= lastDataRow; row += 2)
            {
                Color fillColor = Color.AliceBlue;

                ExcelRange rowRange = sheet.Cells[row, 1, row, lastDataCol];
                rowRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rowRange.Style.Fill.BackgroundColor.SetColor(fillColor);
            }

            for (int row = startRow + 1; row <= lastDataRow; row += 2)
            {
                Color fillColor = Color.White;

                ExcelRange rowRange = sheet.Cells[row, 1, row, lastDataCol];
                rowRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rowRange.Style.Fill.BackgroundColor.SetColor(fillColor);
            }
        }


        private void ColorAlternate2Rows(ExcelWorksheet sheet)
        {
            if (sheet.Dimension != null)
            {
                int startRow = 5;
                int lastDataRow = sheet.Dimension.End.Row;
                int lastDataCol = sheet.Dimension.End.Column;

                Color fillColor = Color.White;

                for (int row = startRow; row <= lastDataRow; row += 2)
                {
                    if (sheet.Cells[row, 1]?.Value != null)
                    {
                        ExcelRange rowRange = sheet.Cells[row, 1, row + 1, lastDataCol];
                        rowRange.Style.Fill.SetBackground(fillColor);

                        fillColor = (fillColor == Color.White) ? Color.AliceBlue : Color.White;
                    }
                }
            }
            else
            {
                Console.WriteLine("Worksheet dimension is null.");
            }
        }
        public void SaveWorkbookWithUniqueName(string filePath)
        {
            try
            {
                List<string> sheetsToKeep = new List<string> { "Monthly Report", "Daily Report" };

                foreach (ExcelWorksheet sheet in excelPackage.Workbook.Worksheets)
                {
                    if (sheet.Name.StartsWith("Plant"))
                    {
                        sheetsToKeep.Add(sheet.Name);
                    }
                }

                // Xóa các sheet không nằm trong danh sách
                foreach (var sheet in excelPackage.Workbook.Worksheets.ToList())
                {
                    if (!sheetsToKeep.Contains(sheet.Name))
                    {
                        excelPackage.Workbook.Worksheets.Delete(sheet);
                    }
                }

                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists)
                {
                    SaveWorkbookWithUniqueName(fileInfo);
                }
                else
                {
                    excelPackage.SaveAs(fileInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in SaveWorkbookWithUniqueName: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                excelPackage.Dispose();
            }
        }

        private void SaveWorkbookWithUniqueName(FileInfo fileInfo)
        {
            int i = 1;
            string directory = fileInfo.DirectoryName;
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.FullName);
            string fileExtension = fileInfo.Extension;

            string newFileName = $"{fileNameWithoutExtension} ({i}){fileExtension}";
            string newFilePath = Path.Combine(directory, newFileName);

            while (File.Exists(newFilePath))
            {
                i++;
                newFileName = $"{fileNameWithoutExtension} ({i}){fileExtension}";
                newFilePath = Path.Combine(directory, newFileName);
            }

            excelPackage.SaveAs(new FileInfo(newFilePath));
        }

        private void FindAndMergeDuplicateCellsForAllRows(ExcelWorksheet sheet)
        {
            int startRow = 9;
            int lastDataRow = sheet.Dimension.End.Row;

            if (startRow == -1)
            {
                return;
            }

            int row = startRow;
            Color alternateColor = Color.AliceBlue;
            Color currentColor = alternateColor;

            while (row <= lastDataRow)
            {
                var currentCell = sheet.Cells[row, 1];
                var firstCellValue = currentCell.Text;
                int mergeStartRow = row;

                // Tìm tất cả các ô liên tiếp giống nhau
                while (row < lastDataRow && IsCellsEqual(currentCell, sheet.Cells[row + 1, 1]))
                {
                    sheet.Cells[row + 1, 1].Clear(); // Xóa dữ liệu của ô giống nhau
                    row++;
                }

                // Gộp các ô giống nhau lại và điền dữ liệu từ ô đầu tiên
                if (mergeStartRow != row)
                {
                    var mergeRange = sheet.Cells[mergeStartRow, 1, row, 1];
                    mergeRange.Merge = true;
                    mergeRange.Value = firstCellValue;

                    for (int i = mergeStartRow; i <= row; i++)
                    {
                        ExcelRange cell = sheet.Cells[i, 1];

                        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cell.Style.Fill.BackgroundColor.SetColor(currentColor);
                        cell.Style.Font.Size = 13;

                        // Kẻ viền Thick Box cho vùng đã gộp đến cột cuối cùng chứa dữ liệu
                        var outsideBorderRange = sheet.Cells[mergeStartRow, 1, row, sheet.Dimension.End.Column];
                        outsideBorderRange.Style.Border.BorderAround(ExcelBorderStyle.Thick);
                    }

                    currentColor = (currentColor == alternateColor) ? Color.White : alternateColor;

                    row++;
                }
                else
                {
                    row++;
                }
            }
        }

        private Dictionary<string, string> columnTranslations = new Dictionary<string, string>
        {
            { "PlantName", "Xưởng\nPlant" },
            { "LineName", "Chuyền\nLine" },
            { "Kết quả\nResult", "Thời gian\nTime" },
            { "FinalTimeResult", "Thời gian\nTime" },
            { "FinalTempResult", "Nhiệt độ\nTemperature" },
            { "FinalChemicalResult", "Hóa chất\nChemical" },
            { "LineResult", "Kết quả\nResult" },
            { "PartName", "Bộ vị\nComponent" },
            { "Model", "Tên hình thể\nModel" },
            { "ArticleName", "Art\nArticle" },
            { "StandardTemp_1", "Tiêu chuẩn\nStandard" },
            { "ActualTemp_1", "Thực tế\nActual" },
            { "ResultTemp_1", "Kết quả\nResult" },
            { "StandardTemp_2", "Tiêu chuẩn\nStandard" },
            { "ActualTemp_2", "Thực tế\nActual" },
            { "ResultTemp_2", "Kết quả\nResult" },
            { "StandardTemp_3", "Tiêu chuẩn\nStandard" },
            { "ActualTemp_3", "Thực tế\nActual" },
            { "ResultTemp_3", "Kết quả\nResult" },
            { "StandardTime_1", "Tiêu chuẩn\nStandard" },
            { "ActualTime_1", "Thực tế\nActual" },
            { "ResultTime_1", "Kết quả\nResult" },
            { "StandardTime_2", "Tiêu chuẩn\nStandard" },
            { "ActualTime_2", "Thực tế\nActual" },
            { "ResultTime_2", "Kết quả\nResult" },
            { "StandardTime_3", "Tiêu chuẩn\nStandard" },
            { "ActualTime_3", "Thực tế\nActual" },
            { "ResultTime_3", "Kết quả\nResult" },
            { "StandardChemical_1", "Tiêu chuẩn\nStandard" },
            { "ActualChemical_1", "Thực tế\nActual" },
            { "ResultChemical_1", "Kết quả\nResult" },
            { "StandardChemical_2", "Tiêu chuẩn\nStandard" },
            { "ActualChemical_2", "Thực tế\nActual" },
            { "ResultChemical_2", "Kết quả\nResult" },
            { "StandardChemical_3", "Tiêu chuẩn\nStandard" },
            { "ActualChemical_3", "Thực tế\nActual" },
            { "ResultChemical_3", "Kết quả\nResult" },
        };

        private string TranslateColumnNameToVietnamese(string englishColumnName)
        {
            return columnTranslations.TryGetValue(englishColumnName, out var translation) ? translation : englishColumnName;
        }

        private void MergeAdjacentCellsInColumns(ExcelWorksheet sheet)
        {
            int startRow = 5;
            int lastDataRow = sheet.Dimension.End.Row;
            int lastColumn = 3;

            for (int column = 1; column <= lastColumn; column++)
            {
                int mergeStartRow = startRow;
                Color currentRowColor = Color.White;

                for (int row = startRow; row <= lastDataRow; row += 2)
                {
                    if (row + 1 <= lastDataRow)
                    {
                        var currentCell = sheet.Cells[row, column];
                        var nextCell = sheet.Cells[row + 1, column];

                        if (IsCellsEqual(currentCell, nextCell))
                        {
                            ClearValuesBeforeMerge(sheet, mergeStartRow + 1, row + 1, column);

                            var mergeRange = sheet.Cells[mergeStartRow, column, row + 1, column];
                            mergeRange.Merge = true;
                            mergeRange.Value = currentCell.Value;


                            currentRowColor = (currentRowColor == Color.White) ? Color.AliceBlue : Color.White;

                            mergeStartRow = row + 2;
                        }
                        else
                        {
                            mergeStartRow = row + 1;
                        }
                    }
                    else
                    {
                        // Xử lý trường hợp dòng cuối cùng
                        var lastCell = sheet.Cells[lastDataRow, column];
                        ClearValuesBeforeMerge(sheet, mergeStartRow + 1, lastDataRow, column);

                        var lastMergeRange = sheet.Cells[mergeStartRow, column, lastDataRow, column];
                        lastMergeRange.Merge = true;
                        lastMergeRange.Value = lastCell.Value;

                        // Áp dụng định dạng cho ô gộp

                        mergeStartRow = row + 2;
                    }
                }
            }
        }
        private bool IsCellsEqual(ExcelRangeBase cell1, ExcelRangeBase cell2)
        {
            return cell1.Text == cell2.Text;
        }
        public void Dispose()
        {
            excelPackage.Dispose();
        }
    }
}