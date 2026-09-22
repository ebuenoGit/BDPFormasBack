using System;

namespace Backend.Formas.Utilities.Telemetry
{
    public interface ITelemetryException
    {
        void RegisterException(Exception exception);
    }
}