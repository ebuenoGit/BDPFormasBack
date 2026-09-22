
using Backend.Formas.API.Controllers.Base;
using Backend.Formas.BusinessRules;
using Backend.Formas.BusinessRules.Aprobaciones;
using Backend.Formas.Entities.Constants;
using Backend.Formas.Entities.DTOI;
using Backend.Formas.Entities.Interface.Business;
using Backend.Formas.Entities.Interface.Repository;
using Backend.Formas.Repositories.Base;
using Backend.Formas.Repositories.DataBase;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Backend.Formas.API.Services
{

    /// <summary>
    /// Clase de extensión para configurar todos los servicios de la aplicación.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Agrega todos los servicios y la configuración de la aplicación.
        /// </summary>
        /// <param name="services">La colección de servicios para registrar.</param>
        /// <param name="configuration">La configuración de la aplicación.</param>
        /// <param name="environment">El entorno de hosting web.</param>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            // Configuración de Mvc con Newtonsoft.Json y el filtro de controlador
            services.AddControllers(options =>
            {
                options.Filters.Add(new BaseControllerUtilities(configuration));
            }).AddNewtonsoftJson(opts =>
            {
                opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            // Agrega las extensiones de servicios de tu proyecto
            services.AddSwagger();
            services.AddJwt(configuration);
            services.AddUtilities(configuration.GetValue<string>(KeyVault.InstrumentationKey));
            services.AddRepositories(configuration, environment.IsDevelopment());
            services.AddGrpcClient(configuration);

            // Registra los servicios para inyección de dependencias
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            // Registros de servicios ITransient
            services.AddTransient<IImportarExcel, ImportarExcel>();
            services.AddTransient<IFormas4Business, Forma4Business>();
            services.AddTransient<IFormas9Business, Forma9Business>();
            services.AddTransient<IFormas9MasivaBusiness, Forma9MasivaBusiness>();
            services.AddTransient<IForma30Business, Forma30Business>();
            services.AddTransient<IForma9Repository, Forma9Repository>();
            services.AddTransient<IForma9MasivaRepository, Forma9MasivaRepository>();
            services.AddTransient<IFormasC4MasivaBusiness, FormaC4MasivaBusiness>();
            services.AddTransient<IFormaC4MasivaRepository, FormaC4MasivaRepository>();
            services.AddTransient<IAdministracionFormas, AdministracionFormas>();
            services.AddTransient<IAdministracionFormasBusiness, AdministrarFormasBusiness>();
            services.AddTransient<IForma15Business, Forma15Bussiness>();
            services.AddTransient<IForm16Business, Form16Business>();
            services.AddTransient<IForma16Repository, Forma16Repository>();
            services.AddTransient<IFormaValidate, FormasValidate>();
            services.AddTransient<IForma4Repository, Forma4Repository>();
            services.AddTransient<IForma20Business, Forma20Business>();
            services.AddTransient<IForma20Repository, Forma20Repository>();
            services.AddTransient<IForma21Business, Forma21Business>();
            services.AddTransient<IForma21Repository, Formas21Repository>();
            services.AddTransient<IForma23Business, Forma23Business>();
            services.AddTransient<IForma23Repository, Forma23Repository>();
            services.AddTransient<IForma30Repository, Forma30Repository>();
            services.AddTransient<IForma30SEEBusiness, Forma30SEEBusiness>();
            services.AddTransient<IForma30SEERepository, Forma30SEERepository>();
            services.AddTransient<IForma22CRBusiness, Forma22CRBusiness>();
            services.AddTransient<IForma22CRRepository, Forma22CRRepository>();
            services.AddTransient<IFormaCuadro1ABusiness, FormaCuadro1ABusiness>();
            services.AddTransient<IFormaCuadro1ARepository, FormaCuadro1ARepository>();
            services.AddTransient<IConsultaFormasBusiness, ConsultaFormasBusiness>();
            services.AddTransient<IFormas17Business, Formas17Business>();
            services.AddTransient<IForma17Repository, Forma17Repository>();
            services.AddTransient<IFormaC7Business, FormaC7Business>();
            services.AddTransient<IFormaC7Repository, FormaC7Repository>();
            services.AddTransient<IFormas15CRBusiness, Formas15CRBusiness>();
            services.AddTransient<IForma15CRRepository, Forma15CRRepository>();
            services.AddTransient<IFormaC30MasivaRepository, FormaC30MasivaRepository>();
            services.AddTransient<IFormasC30MasivaBusiness, FormaC30MasivaBusiness>();
            services.AddTransient<IC4, C4>();
            services.AddTransient<IC1, C1>();
            services.AddTransient<IC7, C7>();
            services.AddTransient<IForma16, BusinessRules.Aprobaciones.Forma16>();
            services.AddTransient<IForma23Aprobacion, BusinessRules.Aprobaciones.Forma23Aprobacion>();
            services.AddTransient<IFormaAprobacion<Aprobacion<Detalle21Aprobacion>>, Forma21Aprobacion>();
            services.AddTransient<IFormaAprobacion<Aprobacion<Detalle15Aprobacion>>, Forma15CR>();
            services.AddTransient<IFormaAprobacion<Aprobacion<Detalle22Aprobacion>>, Forma22Aprobacion>();
            services.AddTransient<IFormaAprobacion<Aprobacion<Detalle17Aprobacion>>, Forma17Aprobacion>();
            services.AddTransient<IReporteBusiness, ReporteBusiness>();
            services.AddTransient<IOficializarFormas, OficializarFormas>();

            return services;
        }
    }
}
