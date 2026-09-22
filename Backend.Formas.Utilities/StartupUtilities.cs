using Backend.Formas.Utilities.Telemetry;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Formas
{
    /// <summary>
    ///     Startup Utilities
    /// </summary>
    public static class StartupUtilities
    {
        /// <summary>
        ///     Adds the utilities.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="instrumentationKey">The instrumentation key.</param>
        public static void AddUtilities(this IServiceCollection services, string instrumentationKey)
        {
            services.AddApplicationInsightsTelemetry(instrumentationKey);

            services.AddTransient<Utilities.SendMail.ISendMailService, Utilities.SendMail.SendMailService>();
            services.AddTransient<ITelemetryException, TelemetryException>();

            services.AddSingleton<ITelemetryInitializer, TelemetryInitializer>();
        }
    }
}