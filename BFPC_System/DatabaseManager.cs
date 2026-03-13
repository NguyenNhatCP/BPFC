using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BPFC_System.frmBpfc;


namespace BPFC_System
{
    public class Globals
    {
        public static string Username { get; set; }
    }
    public class DatabaseManager : IDisposable
    {
        private readonly string connectionString;
        private SqlConnection connection;
        private bool disposed = false;

        public DatabaseManager(string connectionString)
        {
            this.connectionString = connectionString;
            this.connection = new SqlConnection(connectionString);
        }
        private int AddPlantToDatabase(string plantName, int numberOfLines, SqlConnection connection, SqlTransaction transaction)
        {
            string plantQuery = "INSERT INTO Plant (PlantName, NumberOfLines) VALUES (@PlantName, @NumberOfLines); SELECT SCOPE_IDENTITY();";

            using (SqlCommand cmdPlant = new SqlCommand(plantQuery, connection, transaction))
            {
                cmdPlant.Parameters.AddWithValue("@PlantName", plantName);
                cmdPlant.Parameters.AddWithValue("@NumberOfLines", numberOfLines);

                return Convert.ToInt32(cmdPlant.ExecuteScalar());
            }
        }

        public void DeleteArticleToCreate(string articleName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM ArticleToCreate WHERE ArticleName = @ArticleName", connection))
                {
                    command.Parameters.AddWithValue("@ArticleName", articleName);
                    command.ExecuteNonQuery();
                }
            }
        }


        public List<string> LoadProductionLines(string plantName)
        {
            List<string> lines = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT LineName FROM ProductionLines WHERE PlantID IN (SELECT PlantID FROM Plant WHERE PlantName = @SelectedPlant AND IsActive = @IsActive)", connection))
            {
                cmd.Parameters.AddWithValue("@SelectedPlant", plantName);
                cmd.Parameters.AddWithValue("@IsActive" ,true);
                connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string lineName = reader["LineName"].ToString();
                        lines.Add(lineName);
                    }
                }
            }

            return lines;
        }

        public List<string> GetArticleNamesToCreate()
        {
            List<string> articleNames = new List<string>();

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (SqlCommand command = new SqlCommand("SELECT ArticleName FROM ArticleToCreate", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string articleName = reader.GetString(0);
                        articleNames.Add(articleName);
                    }
                }
            }

            return articleNames;
        }

        public void SaveArticleToCreate(string articleName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO ArticleToCreate (ArticleName) VALUES (@ArticleName)", connection))
                {
                    command.Parameters.AddWithValue("@ArticleName", articleName);
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool DeletePlantAndLines(string plantName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    DeleteProductionLines(plantName, connection, transaction);
                    DeletePlant(plantName, connection, transaction);

                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public class ListViewItemComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                ListViewItem itemX = (ListViewItem)x;
                ListViewItem itemY = (ListViewItem)y;

                string[] partsX = itemX.Text.Split('L');
                string[] partsY = itemY.Text.Split('L');

                // Chuyển đổi phần số thành số nguyên để so sánh
                int numberX = int.Parse(partsX[1]);
                int numberY = int.Parse(partsY[1]);

                return numberX.CompareTo(numberY);
            }
        }

        public bool IsPlantNameExists(string plantName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Plant WHERE PlantName = @PlantName", connection))
            {
                cmd.Parameters.AddWithValue("@PlantName", plantName);
                connection.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        public bool AddPlant(string plantName, int numberOfLines)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    int plantID = AddPlantToDatabase(plantName, numberOfLines, connection, transaction);

                    int lineCount = 1;
                    for (int i = 1; i <= numberOfLines; i++)
                    {
                        if (lineCount == 4 || lineCount == 14)
                        {
                            lineCount++;
                        }

                        string lineName = $"{plantName}L{lineCount}".Replace(" ", "");
                        AddProductionLineRecord(plantID, lineName, connection, transaction);

                        lineCount++;
                    }

                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        private void AddProductionLineRecord(int plantID, string lineName, SqlConnection connection, SqlTransaction transaction)
        {
            string lineQuery = "INSERT INTO ProductionLines (PlantID, LineName) VALUES (@PlantID, @LineName);";

            using (SqlCommand cmdLine = new SqlCommand(lineQuery, connection, transaction))
            {
                cmdLine.Parameters.AddWithValue("@PlantID", plantID);
                cmdLine.Parameters.AddWithValue("@LineName", lineName);
                cmdLine.ExecuteNonQuery();
            }
        }


        private void DeleteProductionLines(string plantName, SqlConnection connection, SqlTransaction transaction)
        {
            string deleteLinesQuery = "DELETE FROM ProductionLines WHERE PlantID = (SELECT PlantID FROM Plant WHERE PlantName = @PlantName)";

            using (SqlCommand cmd = new SqlCommand(deleteLinesQuery, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@PlantName", plantName);
                cmd.ExecuteNonQuery();
            }
        }

        private void DeletePlant(string plantName, SqlConnection connection, SqlTransaction transaction)
        {
            string deletePlantQuery = "DELETE FROM Plant WHERE PlantName = @PlantName";

            using (SqlCommand cmd = new SqlCommand(deletePlantQuery, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@PlantName", plantName);
                cmd.ExecuteNonQuery();
            }
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    connection.Dispose();
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public Dictionary<DateTime, string> GetLineResultForDateAndLineName(string lineName, DateTime startDate, DateTime endDate)
        {
            Dictionary<DateTime, string> lineResults = new Dictionary<DateTime, string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT ReportDate, LineResult FROM DailyReport WHERE LineName = @LineName AND ReportDate >= @StartDate AND ReportDate <= @EndDate";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@LineName", lineName);
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime reportDate = reader.GetDateTime(reader.GetOrdinal("ReportDate"));
                            string lineResult = reader["LineResult"].ToString();

                            lineResults.Add(reportDate, lineResult);
                        }
                    }
                }
            }

            return lineResults;
        }

        public void SaveOrUpdateResultsToDatabase(DataTable dataTable, DateTime reportDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (DataRow row in dataTable.Rows)
                {
                    string plantName = row["PlantName"].ToString();
                    string lineName = row["LineName"].ToString();

                    // Kiểm tra xem giá trị có phải là DBNull không
                    int lineID = DBNull.Value.Equals(row["LineID"]) ? 0 : Convert.ToInt32(row["LineID"]);

                    // Kiểm tra xem bản ghi có tồn tại chưa
                    if (!CheckRecordExists(plantName, lineName, reportDate))
                    {
                        // Nếu không tồn tại, thực hiện lưu mới
                        SaveResultsToDatabase(row, reportDate);
                    }
                    else
                    {
                        // Nếu tồn tại, kiểm tra giá trị có thay đổi không
                        if (CheckValuesChanged(row, reportDate))
                        {
                            // Nếu có thay đổi, thực hiện cập nhật
                            UpdateResults(row, reportDate);
                        }
                    }
                }
            }
        }


        private bool CheckValuesChanged(DataRow row, DateTime reportDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Temp, Time, Chemical, LineResult FROM DailyReport " +
                               "WHERE PlantName = @PlantName AND LineName = @LineName AND CONVERT(date, ReportDate) = @ReportDate";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@PlantName", row["PlantName"]);
                    cmd.Parameters.AddWithValue("@LineName", row["LineName"]);
                    cmd.Parameters.AddWithValue("@ReportDate", reportDate.Date);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Kiểm tra giá trị có thay đổi không
                            string temp = row["FinalTempResult"].ToString();
                            string time = row["FinalTimeResult"].ToString();
                            string chemical = row["FinalChemicalResult"].ToString();
                            string lineResult = row["LineResult"].ToString();

                            if (temp != reader["Temp"].ToString() || time != reader["Time"].ToString() ||
                                chemical != reader["Chemical"].ToString() || lineResult != reader["LineResult"].ToString())
                            {
                                return true; 
                            }
                        }
                    }
                }
            }

            return false;
        }


        private void SaveResultsToDatabase(DataRow row, DateTime reportDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string insertQuery = "INSERT INTO DailyReport (PlantName, LineName, Temp, Time, Chemical, LineResult, ReportDate, RecordedAt) " +
                                    "VALUES (@PlantName, @LineName, @Temp, @Time, @Chemical, @LineResult, @ReportDate, @RecordedAt)";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@PlantName", row["PlantName"]);
                    command.Parameters.AddWithValue("@LineName", row["LineName"]);
                    command.Parameters.AddWithValue("@Temp", row["FinalTempResult"]);
                    command.Parameters.AddWithValue("@Time", row["FinalTimeResult"]);
                    command.Parameters.AddWithValue("@Chemical", row["FinalChemicalResult"]);
                    command.Parameters.AddWithValue("@LineResult", row["LineResult"]);
                    command.Parameters.AddWithValue("@ReportDate", reportDate.Date);
                    command.Parameters.AddWithValue("@RecordedAt", DateTime.Now);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void UpdateResults(DataRow row, DateTime reportDate)
        {
            // Thực hiện logic cập nhật vào cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string updateQuery = "UPDATE DailyReport SET Temp = @Temp, Time = @Time, Chemical = @Chemical, LineResult = @LineResult, ReportDate = @ReportDate " +
                                    "WHERE PlantName = @PlantName AND LineName = @LineName AND CONVERT(date, ReportDate) = @ReportDate";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Temp", row["FinalTempResult"]);
                    command.Parameters.AddWithValue("@Time", row["FinalTimeResult"]);
                    command.Parameters.AddWithValue("@Chemical", row["FinalChemicalResult"]);
                    command.Parameters.AddWithValue("@LineResult", row["LineResult"]);
                    command.Parameters.AddWithValue("@PlantName", row["PlantName"]);
                    command.Parameters.AddWithValue("@LineName", row["LineName"]);
                    command.Parameters.AddWithValue("@ReportDate", reportDate);
                    command.Parameters.AddWithValue("@RecordedAt", DateTime.Now);

                    command.ExecuteNonQuery();
                }
            }
        }

            private bool CheckRecordExists(string plantName, string lineName, DateTime reportDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM DailyReport WHERE PlantName = @PlantName AND LineName = @LineName AND CONVERT(date, ReportDate) = @ReportDate";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@PlantName", plantName);
                    cmd.Parameters.AddWithValue("@LineName", lineName);
                    cmd.Parameters.AddWithValue("@ReportDate", reportDate.Date);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }
        public void AddDepartment(string departmentName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Tạo một đối tượng SqlCommand để thực hiện truy vấn chèn dữ liệu
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Department (DeptName) VALUES (@DeptName)", connection))
                    {
                        // Thêm tham số cho truy vấn để tránh tấn công SQL Injection
                        cmd.Parameters.AddWithValue("@DeptName", departmentName);

                        // Thực thi truy vấn
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm bộ phận: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void DeleteProductionLine(string lineName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("DELETE FROM ProductionLines WHERE LineName = @LineName", connection))
            {
                cmd.Parameters.AddWithValue("@LineName", lineName);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateLineName(string oldLineName, string newLineName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("UPDATE ProductionLines SET LineName = @NewLineName WHERE LineName = @OldLineName", connection))
            {
                cmd.Parameters.AddWithValue("@NewLineName", newLineName);
                cmd.Parameters.AddWithValue("@OldLineName", oldLineName);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<int> GetProductionLineIDs(string selectedPlantName)
        {
            List<int> lineIDs = new List<int>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("SELECT LineID FROM ProductionLines WHERE PlantID = (SELECT PlantID FROM Plant WHERE PlantName = @PlantName) AND IsActive = @IsActive", connection))
                {
                    cmd.Parameters.AddWithValue("@PlantName", selectedPlantName);
                    cmd.Parameters.AddWithValue("@IsActive", true);
                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int lineId = Convert.ToInt32(reader["LineID"]);
                            lineIDs.Add(lineId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu cần
                MessageBox.Show("Lỗi khi tải danh sách xưởng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return lineIDs;
        }

        public List<string> GetPlantNames()
        {
            List<string> plantNames = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("SELECT PlantName FROM Plant", connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string plantName = reader["PlantName"].ToString();
                            plantNames.Add(plantName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu cần
                MessageBox.Show("Lỗi khi tải danh sách xưởng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return plantNames;
        }

        public Dictionary<string, int> GetArticlePartIDs(string articleName)
        {
            var partIds = new Dictionary<string, int>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
            SELECT ap.PartName, ap.PartID 
            FROM Articles a
            JOIN ArticleParts ap ON a.ArticleID = ap.ArticleID
            WHERE a.ArticleName = @ArticleName";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ArticleName", articleName);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            partIds[reader["PartName"].ToString()] = Convert.ToInt32(reader["PartID"]);
                        }
                    }
                }
            }
            return partIds;
        }
        public List<string> GetDepartments()
        {
            List<string> departments = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Tạo một đối tượng SqlCommand để truy vấn danh sách bộ phận từ cơ sở dữ liệu
                    using (SqlCommand cmd = new SqlCommand("SELECT DeptName FROM Department", connection))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string departmentName = reader["DeptName"].ToString();
                            departments.Add(departmentName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                MessageBox.Show("Lỗi khi truy vấn danh sách bộ phận: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return departments;
        }

        public bool DeleteDepartment(string departmentName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Tạo một đối tượng SqlCommand để thực hiện truy vấn xóa dữ liệu
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Department WHERE DeptName = @DeptName", connection))
                    {
                        // Thêm tham số cho truy vấn để tránh tấn công SQL Injection
                        cmd.Parameters.AddWithValue("@DeptName", departmentName);

                        // Thực thi truy vấn
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Trả về true nếu có ít nhất một hàng bị ảnh hưởng (bộ phận đã được xóa)
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                Console.WriteLine("Lỗi khi xóa bộ phận: " + ex.Message);
                return false;
            }
        }

        public bool IsDepartmentExists(string departmentName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Tạo một đối tượng SqlCommand để kiểm tra sự tồn tại của bộ phận
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Department WHERE DeptName = @DeptName", connection))
                    {
                        cmd.Parameters.AddWithValue("@DeptName", departmentName);

                        int count = (int)cmd.ExecuteScalar();

                        // Nếu số lượng lớn hơn 0, bộ phận đã tồn tại
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý các trường hợp ngoại lệ
                Console.WriteLine("Lỗi: " + ex.Message);
                return false;
            }
        }


        public bool CheckLineExists(string lineName, DateTime selectedDate)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT COUNT(1) 
            FROM ArticleOfLines 
            WHERE LineName = @LineName AND CONVERT(DATE, ReportDate) = @ReportDate";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@LineName", lineName);
                cmd.Parameters.AddWithValue("@ReportDate", selectedDate.Date);

                conn.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        public void UpdateArticleNameForLine(string articleName, string lineName, DateTime selectedDate)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            UPDATE ArticleOfLines 
            SET ArticleName = @ArticleName 
            WHERE LineName = @LineName AND CONVERT(DATE, ReportDate) = @ReportDate";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ArticleName", articleName);
                cmd.Parameters.AddWithValue("@LineName", lineName);
                cmd.Parameters.AddWithValue("@ReportDate", selectedDate.Date);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void InsertArticleOfLine(string articleName, string lineName, DateTime ReportDate)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            INSERT INTO ArticleOfLines (ArticleName, LineName, ReportDate) 
            VALUES (@ArticleName, @LineName, @ReportDate)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ArticleName", articleName);
                cmd.Parameters.AddWithValue("@LineName", lineName);
                cmd.Parameters.AddWithValue("@ReportDate", ReportDate);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public bool HasReachedDataLimitForTimeResults(int lineId, DateTime selectedDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"SELECT COUNT(*) FROM TimeResults 
                         WHERE LineID = @LineID AND 
                               PartName IN ('Outsole', 'Upper') AND 
                               CAST(ReportDate AS DATE) = CAST(@SelectedDate AS DATE)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LineID", lineId);
                    command.Parameters.AddWithValue("@SelectedDate", selectedDate.Date);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count >= 2;
                }
            }
        }


        public bool HasReachedDataLimitForTempResults(int lineId, DateTime selectedDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"SELECT COUNT(*) FROM TemperatureResults
                         WHERE LineID = @LineID AND 
                               PartName IN ('Outsole', 'Upper') AND 
                               CAST(ReportDate AS DATE) = CAST(@SelectedDate AS DATE)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LineID", lineId);
                    command.Parameters.AddWithValue("@SelectedDate", selectedDate.Date);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count >= 2;
                }
            }
        }
        public void UpdateTemperatureResults(int lineId, Dictionary<string, int> partIds, Dictionary<string, string[]> tempResults, DateTime reportDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var entry in tempResults)
                {
                    string partName = entry.Key;
                    string[] values = entry.Value;

                    // Lấy ArticlePartID từ Dictionary partIds
                    int articlePartId = partIds.ContainsKey(partName) ? partIds[partName] : 0;

                    // Chuyển đổi StdTemp_X từ chuỗi sang float (loại bỏ " ±5" nếu có)
                    float stdTemp1 = string.IsNullOrEmpty(values[1]) ? 0.0f : float.Parse(values[1].Replace(" ±5", ""));
                    float stdTemp2 = string.IsNullOrEmpty(values[4]) ? 0.0f : float.Parse(values[4].Replace(" ±5", ""));
                    float stdTemp3 = string.IsNullOrEmpty(values[7]) ? 0.0f : float.Parse(values[7].Replace(" ±5", ""));
                    float stdTempHeat = string.IsNullOrEmpty(values[10]) ? 0.0f : float.Parse(values[10].Replace(" ±5", ""));

                    // Tạo danh sách các phần SET để tránh lỗi dấu phẩy
                    List<string> setClauses = new List<string>();
                    setClauses.Add("ArticlePartID = @ArticlePartID");

                    if (!string.IsNullOrEmpty(values[0]))
                        setClauses.Add("ActualTemp_1 = @ActualTemp1");

                    if (!string.IsNullOrEmpty(values[1]))
                        setClauses.Add("StandardTemp_1 = @StdTemp1");

                    setClauses.Add("Result_1 = @Result1");

                    if (!string.IsNullOrEmpty(values[3]))
                        setClauses.Add("ActualTemp_2 = @ActualTemp2");

                    if (!string.IsNullOrEmpty(values[4]))
                        setClauses.Add("StandardTemp_2 = @StdTemp2");

                    setClauses.Add("Result_2 = @Result2");

                    if (!string.IsNullOrEmpty(values[6]))
                        setClauses.Add("ActualTemp_3 = @ActualTemp3");

                    if (!string.IsNullOrEmpty(values[7]))
                        setClauses.Add("StandardTemp_3 = @StdTemp3");

                    setClauses.Add("Result_3 = @Result3");

                    if (!string.IsNullOrEmpty(values[9]))
                        setClauses.Add("ActualTemp_Heat = @ActualTempHeat");

                    if (!string.IsNullOrEmpty(values[10]))
                        setClauses.Add("StandardTemp_Heat = @StdTempHeat");

                    setClauses.Add("Result_Heat = @ResultHeat");

                    string query = $"UPDATE TemperatureResults SET {string.Join(", ", setClauses)} " +
                                   "WHERE LineID = @LineID AND Partname = @Partname AND CAST(ReportDate AS DATE) = CAST(@ReportDate AS DATE)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LineID", lineId);
                        command.Parameters.AddWithValue("@Partname", partName);
                        command.Parameters.AddWithValue("@ArticlePartID", articlePartId == 0 ? (object)DBNull.Value : articlePartId);

                        if (!string.IsNullOrEmpty(values[0]))
                            command.Parameters.AddWithValue("@ActualTemp1", float.Parse(values[0]));

                        if (!string.IsNullOrEmpty(values[1]))
                            command.Parameters.AddWithValue("@StdTemp1", stdTemp1);

                        command.Parameters.AddWithValue("@Result1", string.IsNullOrEmpty(values[2]) ? (object)DBNull.Value : values[2]);

                        if (!string.IsNullOrEmpty(values[3]))
                            command.Parameters.AddWithValue("@ActualTemp2", float.Parse(values[3]));

                        if (!string.IsNullOrEmpty(values[4]))
                            command.Parameters.AddWithValue("@StdTemp2", stdTemp2);

                        command.Parameters.AddWithValue("@Result2", string.IsNullOrEmpty(values[5]) ? (object)DBNull.Value : values[5]);

                        if (!string.IsNullOrEmpty(values[6]))
                            command.Parameters.AddWithValue("@ActualTemp3", float.Parse(values[6]));

                        if (!string.IsNullOrEmpty(values[7]))
                            command.Parameters.AddWithValue("@StdTemp3", stdTemp3);

                        command.Parameters.AddWithValue("@Result3", string.IsNullOrEmpty(values[8]) ? (object)DBNull.Value : values[8]);

                        if (!string.IsNullOrEmpty(values[9]))
                            command.Parameters.AddWithValue("@ActualTempHeat", float.Parse(values[9]));

                        if (!string.IsNullOrEmpty(values[10]))
                            command.Parameters.AddWithValue("@StdTempHeat", stdTempHeat);

                        command.Parameters.AddWithValue("@ResultHeat", string.IsNullOrEmpty(values[11]) ? (object)DBNull.Value : values[11]);

                        command.Parameters.AddWithValue("@ReportDate", reportDate.Date);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }


        public void UpdateTimeResults(int lineId, Dictionary<string, int> partIds, Dictionary<string, string[]> timeResults, DateTime reportDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var entry in timeResults)
                {
                    string partName = entry.Key;
                    string[] values = entry.Value;
                    int articlePartId = partIds.ContainsKey(partName) ? partIds[partName] : 0;

                    string query = @"
UPDATE TimeResults
SET ArticlePartID = @ArticlePartID,
    ActualTime_1 = @ActualTime1,
    StandardTime_1 = @StdTime1,
    Result_1 = @Result1,
    
    ActualTime_2 = @ActualTime2,
    StandardTime_2 = @StdTime2,
    Result_2 = @Result2,

    ActualTime_3 = @ActualTime3,
    StandardTime_3 = @StdTime3,
    Result_3 = @Result3,

    ActualTime_Heat = @ActualTimeHeat,
    StandardTime_Heat = @StdTimeHeat,
    Result_Heat = @ResultHeat
WHERE LineID = @LineID AND Partname = @Partname AND CAST(ReportDate AS DATE) = CAST(@ReportDate AS DATE)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LineID", lineId);
                        command.Parameters.AddWithValue("@Partname", partName);
                        command.Parameters.AddWithValue("@ArticlePartID", articlePartId == 0 ? (object)DBNull.Value : articlePartId);
                        command.Parameters.AddWithValue("@ReportDate", reportDate.Date);

                        // Hàm helper nhỏ để chuyển chuỗi null hoặc rỗng thành DBNull.Value
                        object GetDbValue(string s) => string.IsNullOrWhiteSpace(s) ? (object)DBNull.Value : s;

                        // Time 1
                        command.Parameters.AddWithValue("@ActualTime1", GetDbValue(values[0]));
                        command.Parameters.AddWithValue("@StdTime1", GetDbValue(values[1]));
                        command.Parameters.AddWithValue("@Result1", GetDbValue(values[2]));

                        // Time 2
                        command.Parameters.AddWithValue("@ActualTime2", GetDbValue(values[3]));
                        command.Parameters.AddWithValue("@StdTime2", GetDbValue(values[4]));
                        command.Parameters.AddWithValue("@Result2", GetDbValue(values[5]));

                        // Time 3
                        command.Parameters.AddWithValue("@ActualTime3", GetDbValue(values[6]));
                        command.Parameters.AddWithValue("@StdTime3", GetDbValue(values[7]));
                        command.Parameters.AddWithValue("@Result3", GetDbValue(values[8]));

                        // Heat
                        command.Parameters.AddWithValue("@ActualTimeHeat", GetDbValue(values[9]));
                        command.Parameters.AddWithValue("@StdTimeHeat", GetDbValue(values[10]));
                        command.Parameters.AddWithValue("@ResultHeat", GetDbValue(values[11]));

                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private object ParseNullableFloat(string input)
        {
            if (float.TryParse(input, out float result))
                return result;
            return DBNull.Value;
        }

        public void SaveAllResults(string articleName, string lineName, Dictionary<string, string[]> timeResults, Dictionary<string, string[]> tempResults, int userId, DateTime reportDate)
        {
            var partIds = GetArticlePartIDs(articleName);
            int lineId = GetLineID(lineName);

            // Lưu dữ liệu thời gian cho Outsole và Upper
            SaveTimeResults(lineId, partIds, timeResults, userId, reportDate);

            // Lưu dữ liệu nhiệt độ cho Outsole và Upper
            SaveTemperatureResults(lineId, partIds, tempResults, userId, reportDate);
        }

        public void SaveTimeResults(int lineId, Dictionary<string, int> partIds, Dictionary<string, string[]> timeResults, int userId, DateTime reportDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Lưu dữ liệu cho Outsole nếu key "Outsole" tồn tại
                if (partIds.ContainsKey("Outsole") && timeResults.ContainsKey("Outsole"))
                {
                    SaveTimeResultsForPart(connection, lineId, partIds["Outsole"], "Outsole", timeResults["Outsole"], userId, reportDate);
                }

                // Lưu dữ liệu cho Upper nếu key "Upper" tồn tại
                if (partIds.ContainsKey("Upper") && timeResults.ContainsKey("Upper"))
                {
                    SaveTimeResultsForPart(connection, lineId, partIds["Upper"], "Upper", timeResults["Upper"], userId, reportDate);
                }
            }
        }

        public void SaveTemperatureResults(int lineId, Dictionary<string, int> partIds, Dictionary<string, string[]> tempResults, int userId, DateTime reportDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Lưu dữ liệu cho Outsole nếu key "Outsole" tồn tại
                if (partIds.ContainsKey("Outsole") && tempResults.ContainsKey("Outsole"))
                {
                    SaveTemperatureResultsForPart(connection, lineId, partIds["Outsole"], "Outsole", tempResults["Outsole"], userId, reportDate);
                }

                // Lưu dữ liệu cho Upper nếu key "Upper" tồn tại
                if (partIds.ContainsKey("Upper") && tempResults.ContainsKey("Upper"))
                {
                    SaveTemperatureResultsForPart(connection, lineId, partIds["Upper"], "Upper", tempResults["Upper"], userId, reportDate);
                }
            }
        }
        private void SaveTimeResultsForPart(SqlConnection connection, int lineId, int articlePartId, string partName, string[] timeResults, int userId, DateTime reportDate)
        {
            string query = @"INSERT INTO TimeResults 
(LineID, ArticlePartID, Partname, ActualTime_1, StandardTime_1, Result_1, 
 ActualTime_2, StandardTime_2, Result_2, ActualTime_3, StandardTime_3, Result_3, 
 ActualTime_Heat, StandardTime_Heat, Result_Heat, RecordedBy, ReportDate)
VALUES 
(@LineID, @ArticlePartID, @Partname, @ActualTime1, @StdTime1, @Result1, 
 @ActualTime2, @StdTime2, @Result2, @ActualTime3, @StdTime3, @Result3, 
 @ActualTimeHeat, @StdTimeHeat, @ResultHeat, @RecordedBy, @ReportDate)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LineID", lineId);
                command.Parameters.AddWithValue("@ArticlePartID", articlePartId);
                command.Parameters.AddWithValue("@Partname", partName);

                // Chuyển trực tiếp string hoặc DBNull.Value nếu null hoặc trắng
                command.Parameters.AddWithValue("@ActualTime1", string.IsNullOrWhiteSpace(timeResults[0]) ? (object)DBNull.Value : timeResults[0]);
                command.Parameters.AddWithValue("@StdTime1", string.IsNullOrWhiteSpace(timeResults[1]) ? (object)DBNull.Value : timeResults[1]);
                command.Parameters.AddWithValue("@Result1", string.IsNullOrWhiteSpace(timeResults[2]) ? (object)DBNull.Value : timeResults[2]);

                command.Parameters.AddWithValue("@ActualTime2", string.IsNullOrWhiteSpace(timeResults[3]) ? (object)DBNull.Value : timeResults[3]);
                command.Parameters.AddWithValue("@StdTime2", string.IsNullOrWhiteSpace(timeResults[4]) ? (object)DBNull.Value : timeResults[4]);
                command.Parameters.AddWithValue("@Result2", string.IsNullOrWhiteSpace(timeResults[5]) ? (object)DBNull.Value : timeResults[5]);

                command.Parameters.AddWithValue("@ActualTime3", string.IsNullOrWhiteSpace(timeResults[6]) ? (object)DBNull.Value : timeResults[6]);
                command.Parameters.AddWithValue("@StdTime3", string.IsNullOrWhiteSpace(timeResults[7]) ? (object)DBNull.Value : timeResults[7]);
                command.Parameters.AddWithValue("@Result3", string.IsNullOrWhiteSpace(timeResults[8]) ? (object)DBNull.Value : timeResults[8]);

                command.Parameters.AddWithValue("@ActualTimeHeat", string.IsNullOrWhiteSpace(timeResults[9]) ? (object)DBNull.Value : timeResults[9]);
                command.Parameters.AddWithValue("@StdTimeHeat", string.IsNullOrWhiteSpace(timeResults[10]) ? (object)DBNull.Value : timeResults[10]);
                command.Parameters.AddWithValue("@ResultHeat", string.IsNullOrWhiteSpace(timeResults[11]) ? (object)DBNull.Value : timeResults[11]);

                command.Parameters.AddWithValue("@RecordedBy", userId);
                command.Parameters.AddWithValue("@ReportDate", reportDate.Date);

                command.ExecuteNonQuery();
            }
        }
        private void SaveTemperatureResultsForPart(SqlConnection connection, int lineId, int articlePartId, string partName, string[] tempResults, int userId, DateTime reportDate)
        {
            string query = @"INSERT INTO TemperatureResults (LineID, ArticlePartID, Partname, ActualTemp_1, StandardTemp_1, Result_1, ActualTemp_2, StandardTemp_2, Result_2, ActualTemp_3, StandardTemp_3, Result_3, ActualTemp_Heat, StandardTemp_Heat, Result_Heat, RecordedBy, ReportDate)
    VALUES (@LineID, @ArticlePartID, @Partname, @ActualTemp1, @StdTemp1, @Result1, @ActualTemp2, @StdTemp2, @Result2, @ActualTemp3, @StdTemp3, @Result3, @ActualTempHeat, @StdTempHeat, @ResultHeat, @RecordedBy, @ReportDate)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LineID", lineId);
                command.Parameters.AddWithValue("@ArticlePartID", articlePartId);
                command.Parameters.AddWithValue("@Partname", partName);

                // Chuyển đổi và kiểm tra giá trị trước khi thêm vào Parameters
                if (float.TryParse(tempResults[0], out float actualTemp1))
                    command.Parameters.AddWithValue("@ActualTemp1", actualTemp1);
                else
                    command.Parameters.Add("@ActualTemp1", SqlDbType.Float).Value = DBNull.Value;

                // Remove " ±5" from StandardTemp_1
                string standardTemp1 = tempResults[1]?.Replace(" ±5", string.Empty);
                if (float.TryParse(standardTemp1, out float stdTemp1))
                    command.Parameters.AddWithValue("@StdTemp1", stdTemp1);
                else
                    command.Parameters.Add("@StdTemp1", SqlDbType.Float).Value = DBNull.Value;

                command.Parameters.AddWithValue("@Result1", tempResults[2] != null ? (object)tempResults[2] : DBNull.Value);

                if (float.TryParse(tempResults[3], out float actualTemp2))
                    command.Parameters.AddWithValue("@ActualTemp2", actualTemp2);
                else
                    command.Parameters.Add("@ActualTemp2", SqlDbType.Float).Value = DBNull.Value;

                // Remove " ±5" from StandardTemp_2
                string standardTemp2 = tempResults[4]?.Replace(" ±5", string.Empty);
                if (float.TryParse(standardTemp2, out float stdTemp2))
                    command.Parameters.AddWithValue("@StdTemp2", stdTemp2);
                else
                    command.Parameters.Add("@StdTemp2", SqlDbType.Float).Value = DBNull.Value;

                command.Parameters.AddWithValue("@Result2", tempResults[5] != null ? (object)tempResults[5] : DBNull.Value);

                if (float.TryParse(tempResults[6], out float actualTemp3))
                    command.Parameters.AddWithValue("@ActualTemp3", actualTemp3);
                else
                    command.Parameters.Add("@ActualTemp3", SqlDbType.Float).Value = DBNull.Value;

                // Remove " ±5" from StandardTemp_3
                string standardTemp3 = tempResults[7]?.Replace(" ±5", string.Empty);
                if (float.TryParse(standardTemp3, out float stdTemp3))
                    command.Parameters.AddWithValue("@StdTemp3", stdTemp3);
                else
                    command.Parameters.Add("@StdTemp3", SqlDbType.Float).Value = DBNull.Value;

                command.Parameters.AddWithValue("@Result3", tempResults[8] != null ? (object)tempResults[8] : DBNull.Value);


                if (float.TryParse(tempResults[9], out float actualTempHeat))
                    command.Parameters.AddWithValue("@ActualTempHeat", actualTempHeat);
                else
                    command.Parameters.Add("@ActualTempHeat", SqlDbType.Float).Value = DBNull.Value;
                string standardTempHeat = tempResults[10]?.Replace(" ±5", string.Empty);
                if (float.TryParse(standardTempHeat, out float stdTempHeat))
                    command.Parameters.AddWithValue("@StdTempHeat", stdTempHeat);
                else
                    command.Parameters.Add("@StdTempHeat", SqlDbType.Float).Value = DBNull.Value;

                command.Parameters.AddWithValue("@ResultHeat", tempResults[11] != null ? (object)tempResults[11] : DBNull.Value);

                command.Parameters.AddWithValue("@RecordedBy", userId);
                command.Parameters.AddWithValue("@ReportDate", reportDate.Date);

                command.ExecuteNonQuery();
            }
        }
        private void AddFloatParameter(SqlCommand command, string paramName, string[] source, int index)
        {
            if (index >= source.Length || !float.TryParse(source[index], out float value))
                command.Parameters.Add(paramName, SqlDbType.Float).Value = DBNull.Value;
            else
                command.Parameters.Add(paramName, SqlDbType.Float).Value = value;
        }

        private void AddStringParameter(SqlCommand command, string paramName, string[] tempResults, int index)
        {
            if (tempResults.Length > index && !string.IsNullOrEmpty(tempResults[index]))
            {
                command.Parameters.AddWithValue(paramName, tempResults[index]);
            }
            else
            {
                command.Parameters.Add(paramName, SqlDbType.NVarChar).Value = DBNull.Value;
            }
        }

        public int GetLineID(string lineName)
        {
            if (string.IsNullOrEmpty(lineName))
            {
                return -1; 
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT LineID FROM ProductionLines WHERE LineName = @LineName";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LineName", lineName);
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    return -1;
                }
            }
        }

        public bool HasReachedDataLimitForChemicalresults(int lineId, DateTime selectedDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"SELECT COUNT(*) FROM ChemicalResults 
                         WHERE LineID = @LineID AND 
                               PartName IN ('Outsole', 'Upper') AND 
                               CAST(ReportDate AS DATE) = CAST(@SelectedDate AS DATE)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LineID", lineId);
                    command.Parameters.AddWithValue("@SelectedDate", selectedDate.Date);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count >= 2;
                }
            }
        }


        public void UpdateChemicalResults(int lineId, Dictionary<string, int> partIds, Dictionary<string, string[]> chemicalResults, DateTime reportDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var entry in chemicalResults)
                {
                    string partName = entry.Key;
                    string[] values = entry.Value;

                    int articlePartId = partIds.ContainsKey(partName) ? partIds[partName] : 0;

                    string query = $@"UPDATE ChemicalResults
                SET ArticlePartID = @ArticlePartID,

                    ActualChemical_1 = @ActualChemical1,
                    StandardChemical_1 = @StdChemical1,
                    Result_1 = @Result1,

                    ActualChemical_2 = @ActualChemical2,
                    StandardChemical_2 = @StdChemical2,
                    Result_2 = @Result2,

                    ActualChemical_3 = @ActualChemical3,
                    StandardChemical_3 = @StdChemical3,
                    Result_3 = @Result3

                WHERE LineID = @LineID AND Partname = @Partname AND ReportDate = @ReportDate";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LineID", lineId);
                        command.Parameters.AddWithValue("@Partname", partName);
                        command.Parameters.AddWithValue("@ArticlePartID", (object)articlePartId ?? DBNull.Value);

                        command.Parameters.AddWithValue("@ActualChemical1", (object)values[0] ?? DBNull.Value);
                        command.Parameters.AddWithValue("@StdChemical1", (object)values[1] ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Result1", (object)values[2] ?? DBNull.Value);

                        command.Parameters.AddWithValue("@ActualChemical2", (object)values[3] ?? DBNull.Value);
                        command.Parameters.AddWithValue("@StdChemical2", (object)values[4] ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Result2", (object)values[5] ?? DBNull.Value);

                        command.Parameters.AddWithValue("@ActualChemical3", (object)values[6] ?? DBNull.Value);
                        command.Parameters.AddWithValue("@StdChemical3", (object)values[7] ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Result3", (object)values[8] ?? DBNull.Value);

                        command.Parameters.AddWithValue("@ReportDate", reportDate);

                        // Build the SQL with parameters replaced for logging
                        var debugSql = query;
                        foreach (SqlParameter param in command.Parameters)
                        {
                            string value = (param.Value == DBNull.Value || param.Value == null) ? "NULL" : $"'{param.Value.ToString()}'";
                            debugSql = debugSql.Replace(param.ParameterName, value);
                        }
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public void SaveChemicalResults(int lineId, Dictionary<string, int> partIds, Dictionary<string, string[]> chemicalResults, DateTime reportDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                if (partIds.ContainsKey("Outsole") && chemicalResults.ContainsKey("Outsole"))
                {
                    SaveChemicalResultsForPart(connection, lineId, partIds["Outsole"], "Outsole", chemicalResults["Outsole"], reportDate);
                }

                if (partIds.ContainsKey("Upper") && chemicalResults.ContainsKey("Upper"))
                {
                    SaveChemicalResultsForPart(connection, lineId, partIds["Upper"], "Upper", chemicalResults["Upper"], reportDate);
                }
            }
        }

        private void SaveChemicalResultsForPart(SqlConnection connection, int lineId, int articlePartId, string partName, string[] chemicalResults, DateTime reportDate)
        {
            string query = @"INSERT INTO ChemicalResults (LineID, ArticlePartID, Partname, ActualChemical_1, StandardChemical_1, Result_1, ActualChemical_2, StandardChemical_2, Result_2, ActualChemical_3, StandardChemical_3, Result_3, ReportDate)
                      VALUES (@LineID, @ArticlePartID, @Partname, @ActualChemical1, @StdChemical1, @Result1, @ActualChemical2, @StdChemical2, @Result2, @ActualChemical3, @StdChemical3, @Result3, @ReportDate)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LineID", lineId);
                command.Parameters.AddWithValue("@ArticlePartID", (object)articlePartId ?? DBNull.Value);
                command.Parameters.AddWithValue("@Partname", partName);

                command.Parameters.AddWithValue("@ActualChemical1", (object)chemicalResults[0] ?? DBNull.Value);
                command.Parameters.AddWithValue("@StdChemical1", (object)chemicalResults[1] ?? DBNull.Value);
                command.Parameters.AddWithValue("@Result1", (object)chemicalResults[2] ?? DBNull.Value);

                command.Parameters.AddWithValue("@ActualChemical2", (chemicalResults.Length > 3) ? (object)chemicalResults[3] ?? DBNull.Value : DBNull.Value);
                command.Parameters.AddWithValue("@StdChemical2", (chemicalResults.Length > 4) ? (object)chemicalResults[4] ?? DBNull.Value : DBNull.Value);
                command.Parameters.AddWithValue("@Result2", (chemicalResults.Length > 5) ? (object)chemicalResults[5] ?? DBNull.Value : DBNull.Value);

                command.Parameters.AddWithValue("@ActualChemical3", (chemicalResults.Length > 6) ? (object)chemicalResults[6] ?? DBNull.Value : DBNull.Value);
                command.Parameters.AddWithValue("@StdChemical3", (chemicalResults.Length > 7) ? (object)chemicalResults[7] ?? DBNull.Value : DBNull.Value);
                command.Parameters.AddWithValue("@Result3", (chemicalResults.Length > 8) ? (object)chemicalResults[8] ?? DBNull.Value : DBNull.Value);

                command.Parameters.AddWithValue("@ReportDate", reportDate);

                // Build the SQL with parameters replaced for logging
                var debugSql = query;
                foreach (SqlParameter param in command.Parameters)
                {
                    string value = (param.Value == DBNull.Value || param.Value == null) ? "NULL" : $"'{param.Value.ToString()}'";
                    debugSql = debugSql.Replace(param.ParameterName, value);
                }

                command.ExecuteNonQuery();
            }
        }

        // Tính toán mã băm SHA-256
        public string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Thêm người dùng mới vào cơ sở dữ liệu
        public void AddUser(string username, string hashedPassword, string employeeName, string department, int employeeID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO dbo.Users (Username, Password, EmployeeName, Department, EmployeeID, IsActive) VALUES (@Username, @Password, @EmployeeName, @Department, @EmployeeID, 1)", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", hashedPassword);
                    command.Parameters.AddWithValue("@EmployeeName", employeeName);
                    command.Parameters.AddWithValue("@Department", department);
                    command.Parameters.AddWithValue("@EmployeeID", employeeID);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Kiểm tra đăng nhập
        public bool CheckLogin(string username, string hashedPassword)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT IsActive FROM dbo.Users WHERE Username=@Username AND Password=@Password", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", hashedPassword);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bool isActive = reader.GetBoolean(0);
                            return isActive;
                        }
                        else
                        {
                            // Không tìm thấy tài khoản
                            return false;
                        }
                    }
                }
            }
        }

        public bool IsActive(string username)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT IsActive FROM dbo.Users WHERE Username=@Username", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bool isActive = reader.GetBoolean(0);
                            return isActive;
                        }
                        else
                        {
                            // Không tìm thấy tài khoản
                            return false;
                        }
                    }
                }
            }
        }

        // Kiểm tra sự tồn tại của người dùng
        public bool UserExists(string username)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT COUNT(1) FROM dbo.Users WHERE Username=@Username", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count == 1;
                }
            }
        }


        // Xác minh người dùng
        public bool ValidateUser(string username, string password)
        {
            string hashedPassword = ComputeHash(password);
            return CheckLogin(username, hashedPassword);
        }

        public bool GetUserIsActiveStatus(string username)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT IsActive FROM Users WHERE Username = @Username";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return (bool)result;
                    }
                }
            }

            return false;
        }

        // Thêm phương thức kích hoạt tài khoản người dùng
        public bool ActivateUserAccount(string username)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE Users SET IsActive = 1 WHERE Username = @Username";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
                return false;
            }
        }
        public bool ChangePassword(string username, string newPassword)
        {

            // Kiểm tra tính hợp lệ của newPassword (thêm kiểm tra theo yêu cầu của bạn)
            if (!IsNewPasswordValid(newPassword))
            {
                return false; // Mật khẩu mới không hợp lệ
            }

            // Thực hiện cập nhật mật khẩu mới trong cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("UPDATE dbo.Users SET Password=@Password WHERE Username=@Username", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", ComputeHash(newPassword));

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0; // Trả về true nếu cập nhật thành công
                }
            }
        }

        // Thay đổi mật khẩu người dùng
        public bool ChangePassword(string username, string currentPassword, string newPassword)
        {
            // Kiểm tra xem username và currentPassword có hợp lệ
            if (!CheckLogin(username, ComputeHash(currentPassword)))
            {
                return false; // Mật khẩu cũ không đúng
            }

            // Kiểm tra tính hợp lệ của newPassword (thêm kiểm tra theo yêu cầu của bạn)
            if (!IsNewPasswordValid(newPassword))
            {
                return false; // Mật khẩu mới không hợp lệ
            }

            // Thực hiện cập nhật mật khẩu mới trong cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("UPDATE dbo.Users SET Password=@Password WHERE Username=@Username", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", ComputeHash(newPassword));

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0; // Trả về true nếu cập nhật thành công
                }
            }
        }

        // Kiểm tra tính hợp lệ của mật khẩu mới
        public bool IsNewPasswordValid(string newPassword)
        {
            // Độ dài tối thiểu
            if (newPassword.Length < 5)
            {
                return false;
            }
            return true;
        }

        // Mở form tương ứng dựa trên bộ phận của người dùng
        public void OpenCorrespondingForm(string username)
        {
            string department = GetDepartmentFromDatabase(username);

            Form formToOpen = null;
            switch (department.ToLowerInvariant())
            {
                case "ce":
                    formToOpen = new frmBpfc();
                    break;
                case "me":
                    formToOpen = new frmAdmin();
                    break;
                case "warehouse":
                    formToOpen = new frmWarehouse();
                    break;
                default:
                    if (department.ToLowerInvariant().Contains("plant"))
                    {
                        formToOpen = new frmPlant();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy thông tin bộ phận tương ứng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
            }

            if (formToOpen != null)
            {
                // Thiết lập giá trị UserName cho biến toàn cục
                Globals.Username = username;

                // Thiết lập giá trị UserName cho từng form cụ thể
                if (formToOpen is frmBpfc bpfcForm)
                {
                    bpfcForm.UserName = Globals.Username;
                }
                if (formToOpen is frmPlant plantForm)
                {
                    plantForm.UserName = Globals.Username;
                    department = department.Replace("Plant", "XƯỞNG");
                    plantForm.lblHeader.Text = department;
                }
                if (formToOpen is frmWarehouse warehouseForm)
                {
                    warehouseForm.UserName = Globals.Username;
                }
                if (formToOpen is frmAdmin adminForm)
                {
                    adminForm.UserName = Globals.Username;
                }

                formToOpen.Show();
            }
        }
        public string GetDepartmentFromDatabase(string username)
        {
            string department = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Department FROM Users WHERE Username = @Username";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            department = reader["Department"].ToString();
                        }
                    }
                }
            }
            return department;
        }

        public bool ArticleExists(string article)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT COUNT(1) FROM dbo.Articles WHERE ArticleName=@ArticleName", connection))
                {
                    command.Parameters.AddWithValue("@ArticleName", article);

                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }
        public enum ArticleExistenceAction
        {
            ViewArticleInfo,
            HighlightTextBox,
            None
        }

        public ArticleExistenceAction CheckArticleExistence(string article)
        {
            if (ArticleExists(article))
            {
                return ArticleExistenceAction.ViewArticleInfo;
            }
            else
            {
                BPFC_System.frmBpfc formBpfc = System.Windows.Forms.Application.OpenForms.OfType<BPFC_System.frmBpfc>().FirstOrDefault();
                if (formBpfc != null)
                {
                    TextBox txtModel = formBpfc.Controls.Find("txtModel", true).FirstOrDefault() as TextBox;
                    if (txtModel != null)
                    {
                        HighlightTextBoxForShortDuration(txtModel);

                        txtModel.Focus();
                    }
                }
                return ArticleExistenceAction.HighlightTextBox;
            }
        }

        public void HighlightTextBoxForShortDuration(TextBox textBox)
        {
            textBox.BackColor = System.Drawing.Color.LightSteelBlue;


            Timer highlightTimer = new Timer();
            highlightTimer.Interval = 1000;
            highlightTimer.Tick += (sender, e) =>
            {
                textBox.BackColor = System.Drawing.Color.White;
                highlightTimer.Stop();
            };
            highlightTimer.Start();
        }

        public void SaveArticleData(ArticleData data, int createdByUserId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Lưu vào bảng Articles
                string sqlArticle = "INSERT INTO Articles (ArticleName, Model, CreatedBy) VALUES (@ArticleName, @Model, @CreatedBy)";
                using (SqlCommand cmdArticle = new SqlCommand(sqlArticle, connection))
                {
                    cmdArticle.Parameters.AddWithValue("@ArticleName", data.ArticleName);
                    cmdArticle.Parameters.AddWithValue("@Model", data.Model);
                    cmdArticle.Parameters.AddWithValue("@CreatedBy", createdByUserId);
                    cmdArticle.ExecuteNonQuery();
                }

                // Lấy ID của Article vừa thêm
                string sqlGetId = "SELECT IDENT_CURRENT('Articles')";
                SqlCommand cmdGetId = new SqlCommand(sqlGetId, connection);
                int articleId = Convert.ToInt32(cmdGetId.ExecuteScalar());

                // Lưu thông tin Upper và Outsole vào ArticleParts
                SaveArticlePart(connection, articleId, "Upper", data, createdByUserId);
                SaveArticlePart(connection, articleId, "Outsole", data, createdByUserId);
            }
        }

        public void SaveArticlePart(SqlConnection connection, int articleId, string partName, ArticleData data, int createdByUserId)
        {
            // Tiếp tục lưu thông tin vào bảng ArticleParts
            string sqlPart = "INSERT INTO ArticleParts (ArticleID, PartName, StandardTemp_1, StandardTime_1, StandardChemical_1, StandardTemp_2, StandardTime_2, StandardChemical_2, StandardTemp_3, StandardTime_3, StandardChemical_3, CreatedBy) VALUES (@ArticleID, @PartName, @StandardTemp1, @StandardTime1, @StandardChemical1, @StandardTemp2, @StandardTime2, @StandardChemical2, @StandardTemp3, @StandardTime3, @StandardChemical3, @CreatedBy)";
            using (SqlCommand cmdPart = new SqlCommand(sqlPart, connection))
            {
                cmdPart.Parameters.AddWithValue("@ArticleID", articleId);
                cmdPart.Parameters.AddWithValue("@PartName", partName);

                cmdPart.Parameters.AddWithValue("@StandardTemp1", partName == "Upper" ? (object)data.Temp1Upper : (object)data.Temp1Outsole);
                cmdPart.Parameters.AddWithValue("@StandardTime1", partName == "Upper" ? (object)data.Time1Upper : (object)data.Time1Outsole);
                cmdPart.Parameters.AddWithValue("@StandardChemical1", partName == "Upper" ? (object)data.Chemical1Upper : (object)data.Chemical1Outsole);

                cmdPart.Parameters.AddWithValue("@StandardTemp2", partName == "Upper" ? (object)data.Temp2Upper : (object)data.Temp2Outsole);
                cmdPart.Parameters.AddWithValue("@StandardTime2", partName == "Upper" ? (object)data.Time2Upper : (object)data.Time2Outsole);
                cmdPart.Parameters.AddWithValue("@StandardChemical2", partName == "Upper" ? (object)data.Chemical2Upper : (object)data.Chemical2Outsole);

                cmdPart.Parameters.AddWithValue("@StandardTemp3", partName == "Upper" ? (object)data.Temp3Upper : (object)data.Temp3Outsole);
                cmdPart.Parameters.AddWithValue("@StandardTime3", partName == "Upper" ? (object)data.Time3Upper : (object)data.Time3Outsole);
                cmdPart.Parameters.AddWithValue("@StandardChemical3", partName == "Upper" ? (object)data.Chemical3Upper : (object)data.Chemical3Outsole);

                // Bổ sung giá trị mặc định cho các tham số nếu chúng là null
                cmdPart.Parameters["@StandardTemp1"].Value = cmdPart.Parameters["@StandardTemp1"].Value ?? DBNull.Value;
                cmdPart.Parameters["@StandardTime1"].Value = cmdPart.Parameters["@StandardTime1"].Value ?? DBNull.Value;
                cmdPart.Parameters["@StandardChemical1"].Value = cmdPart.Parameters["@StandardChemical1"].Value ?? DBNull.Value;

                cmdPart.Parameters["@StandardTemp2"].Value = cmdPart.Parameters["@StandardTemp2"].Value ?? DBNull.Value;
                cmdPart.Parameters["@StandardTime2"].Value = cmdPart.Parameters["@StandardTime2"].Value ?? DBNull.Value;
                cmdPart.Parameters["@StandardChemical2"].Value = cmdPart.Parameters["@StandardChemical2"].Value ?? DBNull.Value;

                cmdPart.Parameters["@StandardTemp3"].Value = cmdPart.Parameters["@StandardTemp3"].Value ?? DBNull.Value;
                cmdPart.Parameters["@StandardTime3"].Value = cmdPart.Parameters["@StandardTime3"].Value ?? DBNull.Value;
                cmdPart.Parameters["@StandardChemical3"].Value = cmdPart.Parameters["@StandardChemical3"].Value ?? DBNull.Value;

                cmdPart.Parameters.AddWithValue("@CreatedBy", createdByUserId);

                cmdPart.ExecuteNonQuery();
            }
        }

        public int GetUserIdByUsername(string username)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT UserID FROM dbo.Users WHERE Username=@Username", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        return -1;
                    }
                }
            } 
        }


        public List<string> GetActivity(string department, DateTime timestamp)
        {
            List<string> descriptions = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT Description FROM UserActivities WHERE Department = @Department AND CONVERT(DATE, Timestamp) = @Timestamp ORDER BY Timestamp DESC", connection))
                {
                    command.Parameters.AddWithValue("@Department", department);
                    command.Parameters.AddWithValue("@Timestamp", timestamp.Date);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string description = reader.GetString(0);
                            descriptions.Add(description);
                        }
                    }
                }
            }

            return descriptions;
        } 
        public void LogCEActivity(string username, string articleName,  string actionType, string department)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO UserActivities (UserID, ActionType, Description, Timestamp, Department, ArticleName) VALUES (@UserID, @ActionType, @Description, @Timestamp, @Department, @ArticleName)", connection))
                {
                    int userId = GetUserIdByUsername(username);
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.Parameters.AddWithValue("@ActionType", actionType);
                    command.Parameters.AddWithValue("@Description", $" {actionType} Article {articleName} bởi {username}");
                    command.Parameters.AddWithValue("@Timestamp", DateTime.Now);
                    command.Parameters.AddWithValue("@Department", department);
                    command.Parameters.AddWithValue("@ArticleName", articleName);

                    command.ExecuteNonQuery();
                }
            }
        }


        public void LogArticleActivity(string username,string reportDate, string lineName, string articleName, string result, string actionType, string department, DateTime selectedDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO UserActivities (UserID, ActionType, Description, Timestamp, Department, ArticleName, ReportDate) VALUES (@UserID, @ActionType, @Description, @Timestamp, @Department, @ArticleName, @ReportDate)", connection))
                {
                    int userId = GetUserIdByUsername(username);
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.Parameters.AddWithValue("@ActionType", actionType);
                    command.Parameters.AddWithValue("@Description", $"{reportDate}: {lineName} {actionType} Article {articleName} - {result} bởi {username}");
                    command.Parameters.AddWithValue("@Timestamp", DateTime.Now);
                    command.Parameters.AddWithValue("@Department", department);
                    command.Parameters.AddWithValue("@ArticleName", articleName);
                    command.Parameters.AddWithValue("@ReportDate", selectedDate);

                    command.ExecuteNonQuery();
                }
            }
        }

        public ArticleData GetArticleData(string articleName)
        {
            ArticleData articleData = new ArticleData();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT a.Model, a.CreatedAt, u.Username, u.Department, u.EmployeeName " +
                          "FROM Articles a " +
                          "INNER JOIN Users u ON a.CreatedBy = u.UserID " +
                          "WHERE a.ArticleName = @ArticleName";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@ArticleName", articleName);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            articleData.Model = reader["Model"].ToString();
                            articleData.CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
                            string createdBy = reader["Username"].ToString();
                            string department = reader["Department"].ToString();
                            string employeeName = reader["EmployeeName"].ToString();
                            articleData.CreatedBy = $"{createdBy} - {employeeName} - {department}";
                        }
                    }
                }
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT * FROM ArticleParts WHERE ArticleID = (SELECT ArticleID FROM Articles WHERE ArticleName = @ArticleName)";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@ArticleName", articleName);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string partName = reader["PartName"].ToString();

                            if (partName == "Upper")
                            {
                                articleData.TempHeatUpper = GetNullableFloat(reader, "StandardTemp_Heat");
                                articleData.Temp1Upper = GetNullableFloat(reader, "StandardTemp_1");
                                articleData.Time1Upper = reader["StandardTime_1"].ToString();
                                articleData.TimeHeatUpper = reader["StandardTime_Heat"].ToString();
                                articleData.Chemical1Upper = reader["StandardChemical_1"].ToString();

                                articleData.Temp2Upper = GetNullableFloat(reader, "StandardTemp_2");
                                articleData.Time2Upper = reader["StandardTime_2"].ToString();
                                articleData.Chemical2Upper = reader["StandardChemical_2"].ToString();

                                articleData.Temp3Upper = GetNullableFloat(reader, "StandardTemp_3");
                                articleData.Time3Upper = reader["StandardTime_3"].ToString();
                                articleData.Chemical3Upper = reader["StandardChemical_3"].ToString();
                            }
                            else if (partName == "Outsole")
                            {
                                articleData.Temp1Outsole = GetNullableFloat(reader, "StandardTemp_1");
                                articleData.Time1Outsole = reader["StandardTime_1"].ToString();
                                articleData.Chemical1Outsole = reader["StandardChemical_1"].ToString();

                                articleData.Temp2Outsole = GetNullableFloat(reader, "StandardTemp_2");
                                articleData.Time2Outsole = reader["StandardTime_2"].ToString();
                                articleData.Chemical2Outsole = reader["StandardChemical_2"].ToString();

                                articleData.Temp3Outsole = GetNullableFloat(reader, "StandardTemp_3");
                                articleData.Time3Outsole = reader["StandardTime_3"].ToString();
                                articleData.Chemical3Outsole = reader["StandardChemical_3"].ToString();
                            }
                        }
                    }
                }
            }

            return articleData;
        }

        private float? GetNullableFloat(SqlDataReader reader, string columnName)
        {
            if (reader[columnName] is DBNull)
            {
                return null;
            }

            if (float.TryParse(reader[columnName].ToString(), out float result))
            {
                return result;
            }

            return null;
        }

        public void UpdateArticleData(string articleName, ArticleData data, int updatedByUserId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                if (ArticleExists(articleName))
                {
                    string updateArticleQuery = "UPDATE Articles SET Model = @Model, UpdatedBy = @UpdatedBy, UpdatedAt = GETDATE() WHERE ArticleName = @ArticleName";
                    using (SqlCommand cmdUpdateArticle = new SqlCommand(updateArticleQuery, connection))
                    {
                        cmdUpdateArticle.Parameters.AddWithValue("@Model", data.Model);
                        cmdUpdateArticle.Parameters.AddWithValue("@UpdatedBy", updatedByUserId);
                        cmdUpdateArticle.Parameters.AddWithValue("@ArticleName", articleName);

                        cmdUpdateArticle.ExecuteNonQuery();
                    }

                    string getArticleIdQuery = "SELECT ArticleID FROM Articles WHERE ArticleName = @ArticleName";
                    using (SqlCommand cmdGetArticleId = new SqlCommand(getArticleIdQuery, connection))
                    {
                        cmdGetArticleId.Parameters.AddWithValue("@ArticleName", articleName);
                        int articleId = Convert.ToInt32(cmdGetArticleId.ExecuteScalar());

                        // Cập nhật thông tin Upper và Outsole trong bảng ArticleParts
                        UpdateArticlePart(connection, articleId, "Upper", data, updatedByUserId);
                        UpdateArticlePart(connection, articleId, "Outsole", data, updatedByUserId);
                    }
                }
                else
                {
                    MessageBox.Show("Article không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        public void UpdateArticlePart(SqlConnection connection, int articleId, string partName, ArticleData newData, int updatedByUserId)
        {
            // Cập nhật thông tin trong bảng ArticleParts
            string updatePartQuery = "UPDATE ArticleParts SET StandardTemp_1 = @StandardTemp1, StandardTime_1 = @StandardTime1, StandardChemical_1 = @StandardChemical1, " +
                                     "StandardTemp_2 = @StandardTemp2, StandardTime_2 = @StandardTime2, StandardChemical_2 = @StandardChemical2, " +
                                     "StandardTemp_3 = @StandardTemp3, StandardTime_3 = @StandardTime3, StandardChemical_3 = @StandardChemical3, " +
                                     "UpdatedBy = @UpdatedBy, UpdatedAt = GETDATE() " +
                                     "WHERE ArticleID = @ArticleID AND PartName = @PartName";

            using (SqlCommand cmdUpdatePart = new SqlCommand(updatePartQuery, connection))
            {
                cmdUpdatePart.Parameters.AddWithValue("@ArticleID", articleId);
                cmdUpdatePart.Parameters.AddWithValue("@PartName", partName);

                cmdUpdatePart.Parameters.AddWithValue("@StandardTemp1", partName == "Upper" ? (object)newData.Temp1Upper : (object)newData.Temp1Outsole);
                cmdUpdatePart.Parameters.AddWithValue("@StandardTime1", partName == "Upper" ? (object)newData.Time1Upper : (object)newData.Time1Outsole);
                cmdUpdatePart.Parameters.AddWithValue("@StandardChemical1", partName == "Upper" ? (object)newData.Chemical1Upper : (object)newData.Chemical1Outsole);

                cmdUpdatePart.Parameters.AddWithValue("@StandardTemp2", partName == "Upper" ? (object)newData.Temp2Upper : (object)newData.Temp2Outsole);
                cmdUpdatePart.Parameters.AddWithValue("@StandardTime2", partName == "Upper" ? (object)newData.Time2Upper : (object)newData.Time2Outsole);
                cmdUpdatePart.Parameters.AddWithValue("@StandardChemical2", partName == "Upper" ? (object)newData.Chemical2Upper : (object)newData.Chemical2Outsole);

                cmdUpdatePart.Parameters.AddWithValue("@StandardTemp3", partName == "Upper" ? (object)newData.Temp3Upper : (object)newData.Temp3Outsole);
                cmdUpdatePart.Parameters.AddWithValue("@StandardTime3", partName == "Upper" ? (object)newData.Time3Upper : (object)newData.Time3Outsole);
                cmdUpdatePart.Parameters.AddWithValue("@StandardChemical3", partName == "Upper" ? (object)newData.Chemical3Upper : (object)newData.Chemical3Outsole);

                // Bổ sung giá trị mặc định cho các tham số nếu chúng là null
                cmdUpdatePart.Parameters["@StandardTemp1"].Value = cmdUpdatePart.Parameters["@StandardTemp1"].Value ?? DBNull.Value;
                cmdUpdatePart.Parameters["@StandardTime1"].Value = cmdUpdatePart.Parameters["@StandardTime1"].Value ?? DBNull.Value;
                cmdUpdatePart.Parameters["@StandardChemical1"].Value = cmdUpdatePart.Parameters["@StandardChemical1"].Value ?? DBNull.Value;

                cmdUpdatePart.Parameters["@StandardTemp2"].Value = cmdUpdatePart.Parameters["@StandardTemp2"].Value ?? DBNull.Value;
                cmdUpdatePart.Parameters["@StandardTime2"].Value = cmdUpdatePart.Parameters["@StandardTime2"].Value ?? DBNull.Value;
                cmdUpdatePart.Parameters["@StandardChemical2"].Value = cmdUpdatePart.Parameters["@StandardChemical2"].Value ?? DBNull.Value;

                cmdUpdatePart.Parameters["@StandardTemp3"].Value = cmdUpdatePart.Parameters["@StandardTemp3"].Value ?? DBNull.Value;
                cmdUpdatePart.Parameters["@StandardTime3"].Value = cmdUpdatePart.Parameters["@StandardTime3"].Value ?? DBNull.Value;
                cmdUpdatePart.Parameters["@StandardChemical3"].Value = cmdUpdatePart.Parameters["@StandardChemical3"].Value ?? DBNull.Value;

                cmdUpdatePart.Parameters.AddWithValue("@UpdatedBy", updatedByUserId);

                cmdUpdatePart.ExecuteNonQuery();
            }
        }
        public void UpdateArticlePart_Heat(string articleName, double? standardTemp, string standardTime)
        {
            string partName = "Upper";
            int? articleId = null;

            using (SqlConnection conn = new SqlConnection(connectionString)) 
            {
                conn.Open();

                // Lấy ArticleID từ ArticleName
                string getIdQuery = "SELECT ArticleID FROM Articles WHERE ArticleName = @ArticleName";
                using (SqlCommand getIdCmd = new SqlCommand(getIdQuery, conn))
                {
                    getIdCmd.Parameters.AddWithValue("@ArticleName", articleName);
                    object result = getIdCmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        articleId = Convert.ToInt32(result);
                    }
                }

                if (!articleId.HasValue)
                {
                    throw new Exception("ArticleID not found for ArticleName: " + articleName);
                }

                // Cập nhật dữ liệu
                string updatePartQuery =
                    "UPDATE ArticleParts SET StandardTemp_Heat = @StandardTempHeat, StandardTime_Heat = @StandardTimeHeat " +
                    "WHERE ArticleID = @ArticleID AND PartName = @PartName";

                using (SqlCommand cmdUpdatePart = new SqlCommand(updatePartQuery, conn))
                {
                    cmdUpdatePart.Parameters.AddWithValue("@ArticleID", articleId.Value);
                    cmdUpdatePart.Parameters.AddWithValue("@PartName", partName);

                    object tempValue = standardTemp.HasValue ? (object)standardTemp.Value : DBNull.Value;
                    object timeValue = string.IsNullOrWhiteSpace(standardTime) ? (object)DBNull.Value : standardTime;

                    cmdUpdatePart.Parameters.AddWithValue("@StandardTempHeat", tempValue);
                    cmdUpdatePart.Parameters.AddWithValue("@StandardTimeHeat", timeValue);

                    cmdUpdatePart.ExecuteNonQuery();
                }
            }
        }
        // Fetch all LineIDs from the database
        public async Task<DataTable> GetAllLinesAsync()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT LineID, LineName FROM ProductionLines";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    DataTable dt = new DataTable();
                    await Task.Run(() => adapter.Fill(dt));
                    return dt;
                }
            }
        }
        public async Task<DataTable> GetArticlesDataAsync()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT 
                p.PartID,
                a.ArticleName
            FROM ArticleParts p
            JOIN Articles a ON a.ArticleID = p.ArticleID";

                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    DataTable dt = new DataTable();
                    await Task.Run(() => adapter.Fill(dt));
                    return dt;
                }
            }
        }
        // Fetch data from a table based on the selected LineID and date
        public async Task<DataTable> GetDataAsync(string tableName, int lineID, DateTime selectedDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = $@"
                    SELECT * 
                    FROM {tableName}
                    WHERE LineID = @LineID AND CONVERT(DATE, ReportDate) = @ReportDate";

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.Parameters.AddWithValue("@LineID", lineID);
                adapter.SelectCommand.Parameters.AddWithValue("@ReportDate", selectedDate);

                DataTable dataTable = new DataTable();
                await Task.Run(() => adapter.Fill(dataTable));
                return dataTable;
            }
        }

        // Find differences in articles between the three datasets
        public DataTable FindDifferences(DataTable chemicalData, DataTable timeData, DataTable temperatureData, DataTable articlesData)
        {
            // merged by ArticlePartID and LineID
            var merged = from chemical in chemicalData.AsEnumerable()
                         join time in timeData.AsEnumerable()
                         on new { LineID = chemical.Field<int>("LineID"), ArticlePartID = chemical.Field<int>("ArticlePartID") }
                         equals new { LineID = time.Field<int>("LineID"), ArticlePartID = time.Field<int>("ArticlePartID") }
                         into timeJoin
                         from time in timeJoin.DefaultIfEmpty()
                         join temp in temperatureData.AsEnumerable()
                         on new { LineID = chemical.Field<int>("LineID"), ArticlePartID = chemical.Field<int>("ArticlePartID") }
                         equals new { LineID = temp.Field<int>("LineID"), ArticlePartID = temp.Field<int>("ArticlePartID") }
                         into tempJoin
                         from temp in tempJoin.DefaultIfEmpty()
                         select new
                         {
                             LineID = chemical.Field<int>("LineID"),
                             ArticlePartID = chemical.Field<int>("ArticlePartID"),
                             Chemical_Article_Exists = true,
                             Time_Article_Exists = time != null,
                             Temperature_Article_Exists = temp != null
                         };

            // Convert to DataTable
            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("LineID", typeof(int));
            resultTable.Columns.Add("ArticlePartID", typeof(int));
            resultTable.Columns.Add("ArticleName", typeof(string));
            resultTable.Columns.Add("Chemical_Article_Exists", typeof(bool));
            resultTable.Columns.Add("Time_Article_Exists", typeof(bool));
            resultTable.Columns.Add("Temperature_Article_Exists", typeof(bool));

            foreach (var row in merged)
            {
                // Lookup ArticleName from articlesData table
                var articleRow = articlesData.AsEnumerable()
                    .FirstOrDefault(a => a.Field<int>("PartID") == row.ArticlePartID);

                string articleName = articleRow != null ? articleRow.Field<string>("ArticleName") : row.ArticlePartID.ToString();

                // Only show differences
                if (!row.Time_Article_Exists || !row.Temperature_Article_Exists)
                {
                    resultTable.Rows.Add(
                        row.LineID,
                        row.ArticlePartID,
                        articleName,
                        row.Chemical_Article_Exists,
                        row.Time_Article_Exists,
                        row.Temperature_Article_Exists
                    );
                }
            }

            return resultTable;
        }
    }
}
