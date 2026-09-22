// -------------------------------------------------------------------------
// Este es el nuevo Program.cs adaptado al estilo minimal de .NET 9.
// Se han eliminado los métodos Main y CreateHostBuilder para simplificar el código.
// -------------------------------------------------------------------------

using Azure.Identity;

using Backend.Formas.API.Services;
using Backend.Formas.Utilities.Telemetry;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;



var culture = new System.Globalization.CultureInfo("es-CO");
System.Threading.Thread.CurrentThread.CurrentCulture = culture;
System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

// Crea el constructor de la aplicación web, que es el punto de partida para toda la configuración.
var builder = WebApplication.CreateBuilder(args);

// Configuración de la aplicación (anteriormente en CreateHostBuilder.ConfigureAppConfiguration)
// Se accede directamente a builder.Configuration para agregar proveedores de configuración.
// Este bloque de código para Azure Key Vault se ha movido aquí.
builder.Configuration.AddAzureKeyVault(
    new Uri($"https://{System.Environment.GetEnvironmentVariable("KeyVaultUrl")}.vault.azure.net/"),
    new ClientSecretCredential(
        $"a4305987-cf78-4f93-9d64-bf18af65397b",
        System.Environment.GetEnvironmentVariable("ClientId"),
        System.Environment.GetEnvironmentVariable("ClientSecret")
    )
);

// -------------------------------------------------------------------------
// Se llama al método de extensión para configurar todos los servicios,
// manteniendo el archivo Program.cs limpio y modular.
// -------------------------------------------------------------------------
builder.Services.AddApplicationServices(builder.Configuration, builder.Environment);

// Construye la aplicación web.
var app = builder.Build();

// Configuración del pipeline de middleware (anteriormente en Startup.Configure)
// Estos son los mismos middlewares que tenías en tu clase Startup.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors(x => x.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    // Se asume que TelemetryException.NameAPI es una constante o un valor de configuración.
    c.SwaggerEndpoint("/swagger/v1/swagger.json", TelemetryException.NameAPI);
});

app.MapControllers();

// Inicia la aplicación.
app.Run();