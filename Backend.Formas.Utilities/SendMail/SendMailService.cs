namespace Backend.Formas.Utilities.SendMail
{
    using Backend.Formas.Entities.Responses;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Backend.Componentes.Grpc;
    using Grpc.Net.Client;

    public class SendMailService : ISendMailService
    {
        #region Attributes

        /// <summary>
        /// The client
        /// </summary>
        private readonly Notification.NotificationClient Client;

        /// <summary>
        /// The telemetry exception
        /// </summary>
        private readonly Telemetry.ITelemetryException TelemetryException;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SendMailService" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="telemetryException">The telemetry exception.</param>
        public SendMailService(IConfiguration configuration,
                               Telemetry.ITelemetryException telemetryException)
        {
            TelemetryException = telemetryException;

            try
            {
                var target = $"{configuration.GetValue<string>(Entities.Constants.KeyVault.GRPNotification)}:50051";
                var channel = GrpcChannel.ForAddress(target);
                Client = new Notification.NotificationClient(channel);
            }
            catch (Exception exc)
            {
                TelemetryException.RegisterException(exc);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sends the email asynchronous.
        /// </summary>
        /// <param name="emailInfo">The email information.</param>
        /// <returns></returns>
        public async Task<ResponseBase<bool>> SendEmailAsync(Entities.Services.EmailInfo emailInfo)
        {
            ResponseBase<bool> result;
            try
            {
                if (Client == null)
                {
                    result = new ResponseBase<bool>()
                    {
                        Code = (int)HttpStatusCode.InternalServerError,
                        Message = "No se ha configurado el Cliente de GRPC",
                        Data = false
                    };
                    return result;
                }

                var dataSend = new SendEmailRequest
                {
                    Subject = emailInfo.Subject,
                    Body = string.Empty,
                    Template = "740359C3-30D5-4E89-9930-D332F38D8320",
                    Origin = Telemetry.TelemetryException.NameAPI,
                };

                if (emailInfo.To != null)
                    dataSend.To.AddRange(emailInfo.To);

                if (emailInfo.CC != null)
                    dataSend.Comm.AddRange(emailInfo.CC);

                dataSend.Dictionary.Add("body", emailInfo.Body);
                dataSend.Dictionary.Add("head", emailInfo.Styles);

                var reply = await Client.SendEmailAsync(dataSend);

                return new ResponseBase<bool>()
                {
                    Code = reply.Success ? 200 : 400,
                    Message = reply.Success ? "Correo enviado correctamente" : "Error al enviar correo",
                    Data = reply.Success
                };
            }
            catch (Exception ex)
            {
                TelemetryException.RegisterException(ex);

                result = new ResponseBase<bool>()
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Message = ex.Message,
                    Data = false
                };
            }

            return result;
        }
        #endregion
    }
}