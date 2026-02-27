using BPFC_System;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;

namespace BPFC_System
{
    public partial class frmSelectPlant : DevExpress.XtraEditors.XtraForm
    {
        private string connectionString;
        private DatabaseManager dbManager;

        public frmSelectPlant()
        {
            InitializeComponent();
            connectionString = ConfigHelper.GetConnectionString("strCon");
            dbManager = new DatabaseManager(connectionString);
            LoadPlantNames();
        }

        private void LoadPlantNames()
        {
            List<string> plantNames = dbManager.GetPlantNames();

            for (int i = 0; i < plantNames.Count; i++)
            {
                plantNames[i] = "Xưởng " + plantNames[i];
            }

            plantNames.Sort();
            cbxPlantName.Items.Clear();
            cbxPlantName.Items.AddRange(plantNames.ToArray());
            cbxPlantName.SelectedIndex = -1;
        }

        private void frmSelectPlant_Load(object sender, System.EventArgs e)
        {
            LoadPlantNames();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private bool plantFormOpened = false;

        public bool PlantFormOpened
        {
            get { return plantFormOpened; }
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (cbxPlantName.SelectedItem != null)
            {
                string selectedPlantName = cbxPlantName.SelectedItem.ToString().Substring(6);

                if (!string.IsNullOrEmpty(selectedPlantName))
                {
                    frmPlant formPlant = new frmPlant();
                    formPlant.lblHeader.Text = "XƯỞNG " + selectedPlantName;

                    this.Close();

                    plantFormOpened = true;

                    formPlant.Show();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn xưởng trước khi mở.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
