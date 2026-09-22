using Backend.Formas.Entities.Constants;
using Backend.Formas.Utilities;
using Backend.Formas.Utilities.Telemetry;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Backend.Formas.Repositories.Context
{
    /// <summary>
    ///     Eco Oracle Context
    /// </summary>
    public class EcoOracleContext
    {
        /// <summary>
        ///     The connection string
        /// </summary>
        public readonly string ConnectionString;

        /// <summary>
        ///     The telemetry exception
        /// </summary>
        private readonly ITelemetryException TelemetryException;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EcoOracleContext" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="telemetryException">The telemetry exception.</param>
        public EcoOracleContext(IConfiguration configuration,
            ITelemetryException telemetryException)
        {
            ConnectionString = configuration.GetValue<string>(KeyVault.ORACLEConnection);
            TelemetryException = telemetryException;
        }

        /// <summary>
        ///     Executes the procedure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public async Task<List<T>> ExecuteProcedure<T>(string name, params OracleParameter[] parameters)
        {
            using var oracleConnection = new OracleConnection(ConnectionString);
            try
            {
                using var oracleCommand = new OracleCommand(name)
                {
                    Connection = oracleConnection,
                    CommandType = CommandType.StoredProcedure
                };

                await oracleConnection.OpenAsync();

                if (parameters != null && parameters.Any())
                {
                    oracleCommand.Parameters.AddRange(parameters);
                }

                using OracleDataAdapter oracleDataAdapter = new OracleDataAdapter
                {
                    SelectCommand = oracleCommand
                };

                DataTable dataTable = new DataTable();
                oracleDataAdapter.Fill(dataTable);

                var lstRespuesta = dataTable.ToDynamic<T>();

                return lstRespuesta;
            }
            catch (Exception exc)
            {
                TelemetryException.RegisterException(exc);
                return null;
            }
            finally
            {
                if (oracleConnection != null && oracleConnection.State == ConnectionState.Open)
                {
                    await oracleConnection.CloseAsync();
                }
            }
        }

        /// <summary>
        ///     Executes the procedure non query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public async Task<int> ExecuteProcedureNonQuery<T>(string name, params OracleParameter[] parameters)
        {
            using var oracleConnection = new OracleConnection(ConnectionString);
            try
            {
                using var oracleCommand = new OracleCommand(name)
                {
                    Connection = oracleConnection,
                    CommandType = CommandType.StoredProcedure
                };

                await oracleConnection.OpenAsync();

                if (parameters != null && parameters.Count() > 0)
                {
                    oracleCommand.Parameters.AddRange(parameters);
                }

                var result = await oracleCommand.ExecuteNonQueryAsync();

                return result;
            }
            catch (Exception exc)
            {
                TelemetryException.RegisterException(exc);
                return 0;
            }
            finally
            {
                if (oracleConnection != null && oracleConnection.State == ConnectionState.Open)
                {
                    await oracleConnection.CloseAsync();
                }
            }
        }

        /// <summary>
        /// Executes the procedure clob.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="parameterOut">The parameter out.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public async Task<string> ExecuteProcedureClob(string name, string parameterOut, params OracleParameter[] parameters)
        {
            using OracleConnection oracleConnection = new OracleConnection(ConnectionString);
            try
            {
                using OracleCommand oracleCommand = new OracleCommand(name)
                {
                    Connection = oracleConnection,
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = (int)TimeSpan.FromMinutes(120).TotalSeconds
                };

                if (parameters != null && parameters.Any())
                {
                    oracleCommand.Parameters.AddRange(parameters);
                }

                await oracleConnection.OpenAsync();

                await oracleCommand.ExecuteReaderAsync();

                var tempClob = (OracleClob)oracleCommand.Parameters[parameterOut].Value;
                using StreamReader streamreader = new StreamReader(tempClob, Encoding.Unicode);
                var resultadoTMP = streamreader.ReadToEnd();

                return resultadoTMP;
            }
            catch (Exception exc)
            {
                TelemetryException.RegisterException(exc);

                throw new ArgumentException(exc.Message, exc);
            }
            finally
            {
                if (oracleConnection != null)
                {
                    await oracleConnection.CloseAsync();
                    await oracleConnection.DisposeAsync();
                }
            }
        }
    }
}