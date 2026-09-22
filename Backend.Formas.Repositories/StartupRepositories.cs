using Backend.Formas.Entities.Interface.Repository;
using Backend.Formas.Repositories.Base;
using Backend.Formas.Repositories.Context;
using Backend.Formas.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Backend.Formas
{
    /// <summary>
    ///     Startup Repositories
    /// </summary>
    public static class StartupRepositories
    {
        /// <summary>
        ///     Adds the repositories.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="isDev">if set to <c>true</c> [is dev].</param>
        public static void AddRepositories(this IServiceCollection services, IConfiguration configuration, bool isDev)
        {
            services.AddDbContext<FormasContext>(options =>
            {
                options.UseSqlServer(HelperConnection.GetConnectionSQL(configuration, isDev), sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(3, TimeSpan.FromSeconds(30), null);
                    sqlOptions.CommandTimeout(60);
                });
            });

            services.AddSingleton<EcoOracleContext>();

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        }
    }
}