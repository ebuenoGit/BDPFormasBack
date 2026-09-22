using System;
using System.Collections.Generic;
using System.Net;

namespace Backend.Formas.Entities.Services
{
    public class ResponseSeguridad
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseSeguridad{T}" /> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        /// <param name="data">The data.</param>
        public ResponseSeguridad(HttpStatusCode code = HttpStatusCode.OK, string message = null, List<UsersInfo> data = default)
        {
            ResponseTime = DateTime.UtcNow.AddHours(-5);
            Code = (int)code;
            Message = message;
            Data = data;
        }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public List<UsersInfo> Data { get; set; }

        /// <summary>
        /// Gets or sets the response time.
        /// </summary>
        /// <value>
        /// The response time.
        /// </value>
        public DateTime ResponseTime { get; set; }
    }
}