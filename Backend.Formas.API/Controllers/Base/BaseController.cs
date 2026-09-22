using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.RegularExpressions;

namespace Backend.Formas.API.Controllers.Base
{
    /// <summary>
    ///     Base Controller
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [Authorize]
    public class BaseController : ControllerBase
    {
        /// <summary>
        ///     Gets the get email.
        /// </summary>
        /// <value>
        ///     The get email.
        /// </value>
        internal string GetEmail => new[] { "name", "upn", "email" }.Select(type => HttpContext.User?.Claims.FirstOrDefault(c => c.Type.Contains(type))?.Value).FirstOrDefault(val => Regex.IsMatch(val ?? "", @"^[^@\s]+@[^@\s]+\.[^@\s]+$"));

        /// <summary>
        ///     Gets the get user identifier.
        /// </summary>
        /// <value>
        ///     The get user identifier.
        /// </value>
        internal string GetUserId => HttpContext.Request.Headers["Register"].ToString() ?? string.Empty;

        /// <summary>
        ///     Gets the get names.
        /// </summary>
        /// <value>
        ///     The get names.
        /// </value>
        internal string GetNames => Regex.Replace(HttpContext.User?.Claims.FirstOrDefault(c => c.Type == "name")?.Value, @"\s*\([^)]*\)", "").Trim()
            ?? $"{HttpContext.User?.Claims.FirstOrDefault(c => c.Type.Contains("given_name"))?.Value} {HttpContext.User?.Claims.FirstOrDefault(c => c.Type.Contains("surname"))?.Value}";

        /// <summary>
        ///     Gets the get role identifier.
        /// </summary>
        /// <value>
        ///     The get role identifier.
        /// </value>
        internal System.Collections.Generic.List<string> GetRolesIds()
        {
            var roles = HttpContext.Request.Headers["User-Rol"].ToString();
            if (string.IsNullOrEmpty(roles))
            {
                return new System.Collections.Generic.List<string>();
            }
            else
            {
                return roles.Split(',').ToList();
            }
        }
    }
}