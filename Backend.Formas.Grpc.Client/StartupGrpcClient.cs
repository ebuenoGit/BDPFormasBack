using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Backend.Formas
{
    /// <summary>
    /// Startup Grpc Client
    /// </summary>
    public static class StartupGrpcClient
    {
        /// <summary>
        /// Adds the GRPC client.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        public static void AddGrpcClient(this IServiceCollection services, IConfiguration configuration)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            services.AddGrpcClient<Administracion.AdmGrpc.AdmGrpcClient>(c =>
            {
                string target = $"http://{configuration.GetValue<string>($"{Entities.Constants.KeyVault.GRPAdministration}")}:50051";
                c.Address = new Uri(target);
            });

            services.AddGrpcClient<Commons.CommonGrpc.CommonGrpcClient>(c =>
            {
                c.ChannelOptionsActions.Add(action => { action.MaxReceiveMessageSize = 32 * 1024 * 1024; action.MaxSendMessageSize = 32 * 1024 * 1024; });
                string target = $"http://{configuration.GetValue<string>($"{Entities.Constants.KeyVault.GRPCommon}")}:50051";
                c.Address = new Uri(target);
            });

            services.AddGrpcClient<Ppdm.PpdmGrpc.PpdmGrpcClient>(c =>
            {
                string target = $"http://{configuration.GetValue<string>($"{Entities.Constants.KeyVault.GRPPPDM}")}:50051";
                //string target = $"http://localhost:53303";
                c.Address = new Uri(target);
            });
        }
    }
}
