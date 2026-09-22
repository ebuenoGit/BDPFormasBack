namespace Backend.Formas.API
{
    using Backend.Formas.Utilities;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Text;

    /// <summary>
    /// Startup JWT
    /// </summary>
    public static class StartupJwtConfiguration
    {
        /// <summary>
        /// Adds the JWT.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>(Entities.Constants.KeyVault.JWTSecret));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.Response.OnStarting(async () =>
                        {
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync((new Entities.Responses.ResponseBase<bool>(System.Net.HttpStatusCode.Unauthorized, "La sesión ha caducado!")).Serialize());
                        });

                        return System.Threading.Tasks.Task.CompletedTask;
                    }
                };

                x.Authority = $"https://login.microsoftonline.com/{configuration.GetValue<string>(Entities.Constants.KeyVault.TenantId)}/v2.0";
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = $"https://sts.windows.net/{configuration.GetValue<string>(Entities.Constants.KeyVault.TenantId)}/",
                    ValidateAudience = true,
                    ValidAudience = $"api://{Environment.GetEnvironmentVariable("ClientId")}",
                    ValidateLifetime = true
                };
            });
        }
    }
}