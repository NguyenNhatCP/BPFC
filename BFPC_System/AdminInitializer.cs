using BPFC_System;
using System;

namespace BPFC_System
{
    public class AdminInitializer : IDisposable
    {
        private string connectionString;

        public AdminInitializer(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void InitializeAdminAccount()
        {
            DatabaseManager dbManager = new DatabaseManager(connectionString);

            if (!dbManager.UserExists("Administrator"))
            {
                string adminPassword = "admin";

                string hashedPassword = dbManager.ComputeHash(adminPassword);

                dbManager.AddUser("Administrator", hashedPassword, "Administrator", "ME", 0);
            }
        }

        public void Dispose()
        {
        }
    }
}