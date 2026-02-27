using System;
using System.IO;
using Newtonsoft.Json.Linq;
namespace BPFC_System
{

    public static class ConfigHelper
    {
        public static string GetConnectionString(string name)
        {
            try
            {
                // Lấy đường dẫn file appsettings.json cạnh file .exe
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

                if (!File.Exists(path)) return null;

                string json = File.ReadAllText(path);
                var obj = JObject.Parse(json);

                return obj["ConnectionStrings"]?[name]?.ToString();
            }
            catch
            {
                return null;
            }
        }
    }
}
