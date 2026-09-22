using Backend.Formas.Entities.Constants;
using Microsoft.Extensions.Configuration;

namespace Backend.Formas.Utilities
{
    /// <summary>
    ///     Helper Connection
    /// </summary>
    public static class HelperConnection
    {
        /// <summary>
        ///     Gets the connection SQL.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="isDevelopment">if set to <c>true</c> [is development].</param>
        /// <returns></returns>
        public static string GetConnectionSQL(IConfiguration configuration, bool isDevelopment = false)
        {
            var database = configuration.GetValue<string>(KeyVault.SQLDataBaseFRM);

            if (database.Contains("Server="))
            {
                return database;
            }

            var server =
                configuration.GetValue<string>($"{KeyVault.SQLServer}");//{(isDevelopment ? "DEV" : string.Empty)}");
            var user = configuration.GetValue<string>(KeyVault.SQLUser);
            var pwd = configuration.GetValue<string>(KeyVault.SQLPassword);

            return string.Format(
                "Server={0};Database={1};User ID={2};Password={3};Trusted_Connection=False;Encrypt=True;Persist Security Info=True;",
                server,
                database,
                user,
                pwd);
        }
    }
}