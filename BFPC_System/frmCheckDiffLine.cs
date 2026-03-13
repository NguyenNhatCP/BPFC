using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BPFC_System
{
    public partial class frmCheckDiffLine : Form
    {
        private readonly string connectionString = ConfigHelper.GetConnectionString("strCon");
        public frmCheckDiffLine()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            dtpDate.CustomFormat = "dddd dd/MM/yyyy";
            dtpDate.MinDate = new DateTime(1900, 1, 1);
            dtpDate.MaxDate = new DateTime(2100, 12, 31);
            SetInitialDateTimeValue();
        }
        public bool ShowHomeButton
        {
            set { btnHome.Visible = value; }
        }
        private void SetInitialDateTimeValue()
        {
            DateTime today = DateTime.Now;

            // Kiểm tra hôm nay là thứ mấy
            if (today.DayOfWeek == DayOfWeek.Monday)
            {
                // Nếu hôm nay là thứ 2, đặt giá trị thành 2 ngày trước (ngày thứ 7)
                dtpDate.Value = today.AddDays(-2);
            }
            else
            {
                // Ngược lại, đặt giá trị thành 1 ngày trước
                dtpDate.Value = today.AddDays(-1);
            }
        }
        // Trigger comparison when the button is clicked
        private void btnCompare_Click(object sender, EventArgs e)
        {
            DateTime selectedDate = dtpDate.Value.Date;

            _ = LoadAndCompareAllLinesAsync(selectedDate);
        }
        private async Task LoadAndCompareAllLinesAsync(DateTime selectedDate)
        {
            DatabaseManager db = new DatabaseManager(connectionString);
            // 1️⃣ Get all LineIDs and LineNames
            DataTable linesData = await db.GetAllLinesAsync();

            // 2️⃣ Get Article mappings
            DataTable articlesData = await db.GetArticlesDataAsync();

            // 3️⃣ Prepare final result table
            DataTable finalResult = new DataTable();
            finalResult.Columns.Add("LineID", typeof(int));
            finalResult.Columns.Add("LineName", typeof(string));
            finalResult.Columns.Add("ArticlePartID", typeof(int));
            finalResult.Columns.Add("ArticleName", typeof(string));
            finalResult.Columns.Add("Chemical_Article_Exists", typeof(bool));
            finalResult.Columns.Add("Time_Article_Exists", typeof(bool));
            finalResult.Columns.Add("Temperature_Article_Exists", typeof(bool));

            // 4️⃣ Loop through all lines
            foreach (DataRow lineRow in linesData.Rows)
            {
                int lineID = lineRow.Field<int>("LineID");
                string lineName = lineRow.Field<string>("LineName");

                // Fetch per-line data
                DataTable chemicalData = await db.GetDataAsync("ChemicalResults", lineID, selectedDate);
                DataTable timeData = await db.GetDataAsync("TimeResults", lineID, selectedDate);
                DataTable tempData = await db.GetDataAsync("TemperatureResults", lineID, selectedDate);

                // Find differences for this line
                DataTable differences = db.FindDifferences(chemicalData, timeData, tempData, articlesData);

                // Merge into final result, adding LineName
                foreach (DataRow diffRow in differences.Rows)
                {
                    finalResult.Rows.Add(
                        diffRow.Field<int>("LineID"),
                        lineName,
                        diffRow.Field<int>("ArticlePartID"),
                        diffRow.Field<string>("ArticleName"),
                        diffRow.Field<bool>("Chemical_Article_Exists"),
                        diffRow.Field<bool>("Time_Article_Exists"),
                        diffRow.Field<bool>("Temperature_Article_Exists")
                    );
                }
            }

            // 5️⃣ Bind to DataGridView
            dataGridView.DataSource = finalResult;
            // Create a new DataTable to store unique rows
            DataTable uniqueResult = finalResult.Clone(); // same structure

            // Use LINQ to select distinct rows based on LineName + ArticleName
            var distinctRows = finalResult.AsEnumerable()
                .GroupBy(r => new
                {
                    LineName = r.Field<string>("LineName"),
                    ArticleName = r.Field<string>("ArticleName")
                })
                .Select(g => g.First()); // keep the first row in each group

            foreach (var row in distinctRows)
            {
                uniqueResult.ImportRow(row);
            }

            // Bind to DataGridView
            dataGridView.DataSource = uniqueResult;

            // Change the caption names (headers) for columns
            if (dataGridView.Columns.Contains("LineName"))
                dataGridView.Columns["LineName"].HeaderText = "Chuyền";

            if (dataGridView.Columns.Contains("ArticleName"))
                dataGridView.Columns["ArticleName"].HeaderText = "Tên Article";  // Set custom name here

            if (dataGridView.Columns.Contains("Chemical_Article_Exists"))
                dataGridView.Columns["Chemical_Article_Exists"].HeaderText = "Hóa chất";  // Set custom name here

            if (dataGridView.Columns.Contains("Time_Article_Exists"))
                dataGridView.Columns["Time_Article_Exists"].HeaderText = "Thời gian";  // Set custom name here

            if (dataGridView.Columns.Contains("Temperature_Article_Exists"))
                dataGridView.Columns["Temperature_Article_Exists"].HeaderText = "Nhiệt độ";  // Set custom name here

            // Hide the ID columns if you want
            dataGridView.Columns["LineID"].Visible = false;
            dataGridView.Columns["ArticlePartID"].Visible = false;

            // Optional: auto-size columns for readability
            dataGridView.AutoResizeColumns();
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmHome formHome = new frmHome();
            formHome.Show();
        }
    }
}
