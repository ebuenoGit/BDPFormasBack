using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Backend.Formas.Utilities.Telemetry
{
    public class TelemetryException : ITelemetryException
    {
        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExceptionService" /> class.
        /// </summary>
        /// <param name="telemetryClient">The telemetry client.</param>
        public TelemetryException(TelemetryClient telemetryClient)
        {
            TelemetryClient = telemetryClient;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Registers the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void RegisterException(Exception exception)
        {
            TelemetryClient.Context.Cloud.RoleName = NameAPI;
            TelemetryClient.TrackException(exception, ToDictionary(exception));
        }

        #endregion

        /// <summary>
        ///     Converts to dictionary.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        private Dictionary<string, string> ToDictionary(Exception e)
        {
            var st = new StackTrace(e, true);
            var frame = st.GetFrame(0);
            var line = frame.GetFileLineNumber();

            var dic = new Dictionary<string, string>
            {
                {"Message", e.Message},
                {"Line", line.ToString()},
                {"StackTrace", e.StackTrace},
                {"InnerException", e.InnerException?.Message},
                {"ToString", e.ToString()}
            };

            return dic;
        }

        #region Attributes

        /// <summary>
        ///     The name API
        /// </summary>
        public const string NameAPI = "BDP-Formas";

        /// <summary>
        ///     The telemetry client
        /// </summary>
        private readonly TelemetryClient TelemetryClient;

        #endregion
    }
}