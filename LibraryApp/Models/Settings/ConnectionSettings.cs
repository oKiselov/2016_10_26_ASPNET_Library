using System.Configuration;

namespace LibraryApp.Models.Settings
{
    public class ConnectionSettings
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }
}
