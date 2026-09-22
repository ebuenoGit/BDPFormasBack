using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Backend.Formas.API.Controllers.Base
{
    /// <summary>
    /// BaseUtilities
    /// </summary>
    public class BaseControllerUtilities : ActionFilterAttribute
    {
        /// <summary>
        /// Subscription Key.
        /// </summary>
        private string SubscriptionKey { get; set; }

        /// <summary>
        /// API Url.
        /// </summary>
        private string APIUrl { get; set; }

        /// <summary>
        /// TokenValidationFilterAttribute
        /// </summary>
        /// <param name="configuration"></param>
        public BaseControllerUtilities(IConfiguration configuration)
        {
            SubscriptionKey = configuration[Entities.Constants.KeyVault.APISubscription];
            APIUrl = configuration[Entities.Constants.KeyVault.APIUrl];
        }
        /// <summary>
        /// OnActionExecuting
        /// </summary>
        /// <param name="context"></param>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            try
            {
                if (string.IsNullOrEmpty(token)) { context.Result = new UnauthorizedResult(); return; }

                using (var clientHTTP = new HttpClient()
                {
                    BaseAddress = new Uri(APIUrl),
                    Timeout = TimeSpan.FromMinutes(15)
                })
                {
                    clientHTTP.DefaultRequestHeaders.Accept.Clear();
                    clientHTTP.DefaultRequestHeaders.Clear();

                    clientHTTP.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    clientHTTP.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SubscriptionKey);
                    clientHTTP.DefaultRequestHeaders.Add("Authorization", token);
                    var responseMessage = await clientHTTP.GetAsync("bdp-componentes/api/Authentication/validateToken");

                    if (responseMessage.StatusCode.Equals(System.Net.HttpStatusCode.Unauthorized))
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }
                    else if (!responseMessage.StatusCode.Equals(System.Net.HttpStatusCode.OK))
                    {
                        context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.InternalServerError);
                        return;
                    }
                    else
                    {
                        await next();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
