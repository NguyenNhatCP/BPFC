using System;
using System.IO;

namespace BPFC_System
{
    internal class UniqueFileNameGenerator
    {
        public static string GenerateFileName()
        {
            // Lấy thời gian hiện tại
            DateTime currentTime = DateTime.Now;

            // Lấy tên của tháng hiện tại bằng tiếng Anh
            string monthName = currentTime.ToString("MMMM", new System.Globalization.CultureInfo("en-US"));

            // Lấy tuần của tháng hiện tại
            int weekOfMonth = (int)Math.Ceiling((double)currentTime.Day / 7);

            // Tạo tên tệp dựa trên cấu trúc
            string fileNameBase = $"Random re-check BPFC compliance report {monthName} - W{weekOfMonth}";
            string fileName = $"{fileNameBase}.xlsx";

            // Kiểm tra nếu tệp đã tồn tại
            int fileNumber = 1;
            while (File.Exists(fileName))
            {
                // Nếu tệp đã tồn tại, thêm hậu tố số thứ tự vào
                fileName = $"{fileNameBase} ({fileNumber}).xlsx";
                fileNumber++;
            }

            return fileName;
        }
    }
}
