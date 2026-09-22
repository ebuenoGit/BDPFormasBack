using Backend.Formas.Entities.Responses;
using Backend.Formas.Entities.Services;
using System.Threading.Tasks;

namespace Backend.Formas.Utilities.SendMail
{
    public interface ISendMailService
    {
        /// <summary>
        /// Sends the email asynchronous.
        /// </summary>
        /// <param name="emailInfo">The email information.</param>
        /// <returns></returns>
        Task<ResponseBase<bool>> SendEmailAsync(EmailInfo emailInfo);
    }
}