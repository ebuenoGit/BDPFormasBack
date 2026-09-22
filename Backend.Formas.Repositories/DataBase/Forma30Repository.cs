using Backend.Formas.Entities.Interface.Repository;
using Backend.Formas.Entities.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Backend.Formas.Repositories.DataBase
{
    public class Forma30Repository : IForma30Repository
    {
        private readonly Context.EcoOracleContext OracleContext;

        public Forma30Repository(Context.EcoOracleContext oracleContext)
        {
            OracleContext = oracleContext;
        }
        public Task<bool> CreaForma30(ParametrosCreaForma30 id)
        {
            using OracleConnection connection = new OracleConnection(OracleContext.ConnectionString);
            connection.Open();
            OracleCommand command = connection.CreateCommand();
            OracleTransaction transaction;
            transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

            List<ParametrosForma30> lstRegistros = new List<ParametrosForma30>();
            lstRegistros = id.registros;
            OracleParameter oracleParameter0 = new OracleParameter("I_ANNO", OracleDbType.Int32);
            OracleParameter oracleParameter1 = new OracleParameter("I_MES", OracleDbType.Int32);
            oracleParameter0.Value = id.anno;
            oracleParameter1.Value = id.mes;


            command.Transaction = transaction;
            command.CommandText = "admonfunc.PKG_MBDP_CARGUE_FORMA.FORMA30_NW";
            command.CommandType = CommandType.StoredProcedure;
            bool resultado = true;
            try
            {
                foreach (ParametrosForma30 item in lstRegistros)
                {
                    OracleParameter oracleParameter2 = new OracleParameter("I_CAMPOCONTRA_ID", OracleDbType.Int32);
                    OracleParameter oracleParameter3 = new OracleParameter("I_REAL_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter4 = new OracleParameter("I_REAL", OracleDbType.Double);

                    oracleParameter2.Value = item.pden_id;
                    if (string.IsNullOrEmpty(item.real_tp))
                    {
                        oracleParameter3.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter3.Value = item.real_tp;
                    }
                    if (item.real.Equals(null))
                    {
                        oracleParameter4.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter4.Value = item.real;
                    }

                    OracleParameter oracleParameter5 = new OracleParameter("I_GAS_PROCESADO_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter6 = new OracleParameter("I_GAS_PROCESADO", OracleDbType.Double);
                    OracleParameter oracleParameter7 = new OracleParameter("I_GAS_QUEMADO_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter8 = new OracleParameter("I_GAS_QUEMADO", OracleDbType.Double);
                    OracleParameter oracleParameter9 = new OracleParameter("I_GLP_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter10 = new OracleParameter("I_GLP", OracleDbType.Double);


                    if (string.IsNullOrEmpty(item.gas_procesado_tp))
                    {
                        oracleParameter5.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter5.Value = item.gas_procesado_tp;
                    }
                    if (item.gas_procesado.Equals(null))
                    {
                        oracleParameter6.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter6.Value = item.gas_procesado;
                    }


                    if (string.IsNullOrEmpty(item.gas_quemado_tp))
                    {
                        oracleParameter7.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter7.Value = item.gas_quemado_tp;
                    }
                    if (item.gas_quemado.Equals(null))
                    {
                        oracleParameter8.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter8.Value = item.gas_quemado;
                    }


                    if (string.IsNullOrEmpty(item.glp_tp))
                    {
                        oracleParameter9.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter9.Value = item.glp_tp;
                    }
                    if (item.glp.Equals(null))
                    {
                        oracleParameter10.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter10.Value = item.glp;
                    }
                    OracleParameter oracleParameter11 = new OracleParameter("I_PROPANO_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter12 = new OracleParameter("I_PROPANO", OracleDbType.Double);
                    OracleParameter oracleParameter13 = new OracleParameter("I_BUTANO_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter14 = new OracleParameter("I_BUTANO", OracleDbType.Double);
                    OracleParameter oracleParameter15 = new OracleParameter("I_GASOLINA_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter16 = new OracleParameter("I_GASOLINA", OracleDbType.Double);
                    if (string.IsNullOrEmpty(item.propano_tp))
                    {
                        oracleParameter11.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter11.Value = item.propano_tp;
                    }
                    if (item.propano.Equals(null))
                    {
                        oracleParameter12.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter12.Value = item.propano;
                    }

                    if (string.IsNullOrEmpty(item.butano_tp))
                    {
                        oracleParameter13.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter13.Value = item.butano_tp;
                    }
                    if (item.butano.Equals(null))
                    {
                        oracleParameter14.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter14.Value = item.butano;
                    }


                    if (string.IsNullOrEmpty(item.gasolina_tp))
                    {
                        oracleParameter15.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter15.Value = item.gasolina_tp;
                    }
                    if (item.gasolina.Equals(null))
                    {
                        oracleParameter16.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter16.Value = item.gasolina;
                    }
                    OracleParameter oracleParameter17 = new OracleParameter("I_APIASOL_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter18 = new OracleParameter("I_APIASOL", OracleDbType.Double);
                    OracleParameter oracleParameter19 = new OracleParameter("I_CONDENSADO_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter20 = new OracleParameter("I_CONDENSADO", OracleDbType.Double);
                    OracleParameter oracleParameter21 = new OracleParameter("I_CONSUMOS_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter22 = new OracleParameter("I_CONSUMOS", OracleDbType.Double);
                    if (string.IsNullOrEmpty(item.apiasol_tp))
                    {
                        oracleParameter17.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter17.Value = item.apiasol_tp;
                    }
                    if (item.apiasol.Equals(null))
                    {
                        oracleParameter18.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter18.Value = item.apiasol;
                    }


                    if (string.IsNullOrEmpty(item.condensado_tp))
                    {
                        oracleParameter19.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter19.Value = item.condensado_tp;
                    }
                    if (item.condensado.Equals(null))
                    {
                        oracleParameter20.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter20.Value = item.condensado;
                    }


                    if (string.IsNullOrEmpty(item.consumos_tp))
                    {
                        oracleParameter21.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter21.Value = item.consumos_tp;
                    }
                    if (item.consumos.Equals(null))
                    {
                        oracleParameter22.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter22.Value = item.consumos;
                    }
                    OracleParameter oracleParameter23 = new OracleParameter("I_GASODUCTOS_URBANOS_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter24 = new OracleParameter("I_GASODUCTOS_URBANOS", OracleDbType.Double);
                    OracleParameter oracleParameter25 = new OracleParameter("I_GENERACION_ELECTRICA_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter26 = new OracleParameter("I_GENERACION_ELECTRICA", OracleDbType.Double);
                    OracleParameter oracleParameter27 = new OracleParameter("I_OTRAS_VENTAS_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter28 = new OracleParameter("I_OTRAS_VENTAS", OracleDbType.Double);
                    if (string.IsNullOrEmpty(item.gasoductos_urbanos_tp))
                    {
                        oracleParameter23.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter23.Value = item.gasoductos_urbanos_tp;
                    }
                    if (item.gasoductos_urbanos.Equals(null))
                    {
                        oracleParameter24.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter24.Value = item.gasoductos_urbanos;
                    }

                    if (string.IsNullOrEmpty(item.generacion_elect_tp))
                    {
                        oracleParameter25.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter25.Value = item.generacion_elect_tp;
                    }
                    if (item.generacion_elect.Equals(null))
                    {
                        oracleParameter26.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter26.Value = item.generacion_elect;
                    }


                    if (string.IsNullOrEmpty(item.otras_ventas_tp))
                    {
                        oracleParameter27.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter27.Value = item.otras_ventas_tp;
                    }
                    if (item.otras_ventas.Equals(null))
                    {
                        oracleParameter28.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter28.Value = item.otras_ventas;
                    }
                    OracleParameter oracleParameter29 = new OracleParameter("I_GAS_TRANFERENCIA_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter30 = new OracleParameter("I_GAS_TRANFERENCIA", OracleDbType.Double);
                    OracleParameter oracleParameter31 = new OracleParameter("I_GAS_NEUMATICO_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter32 = new OracleParameter("I_GAS_NEUMATICO", OracleDbType.Double);
                    OracleParameter oracleParameter33 = new OracleParameter("I_GAS_INYECTADO_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter34 = new OracleParameter("I_GAS_INYECTADO", OracleDbType.Double);
                    if (string.IsNullOrEmpty(item.gas_transfer_tp))
                    {
                        oracleParameter29.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter29.Value = item.gas_transfer_tp;
                    }
                    if (item.gas_transfer.Equals(null))
                    {
                        oracleParameter30.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter30.Value = item.gas_transfer;
                    }

                    if (string.IsNullOrEmpty(item.gas_neumatico_tp))
                    {
                        oracleParameter31.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter31.Value = item.gas_neumatico_tp;
                    }
                    if (item.gas_neumatico.Equals(null))
                    {
                        oracleParameter32.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter32.Value = item.gas_neumatico;
                    }


                    if (string.IsNullOrEmpty(item.gas_inyectado_tp))
                    {
                        oracleParameter33.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter33.Value = item.gas_inyectado_tp;
                    }
                    if (item.gas_inyectado.Equals(null))
                    {
                        oracleParameter34.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter34.Value = item.gas_inyectado;
                    }
                    OracleParameter oracleParameter35 = new OracleParameter("I_TOTAL_PROC_PLANTA_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter36 = new OracleParameter("I_TOTAL_PROC_PLANTA", OracleDbType.Double);
                    OracleParameter oracleParameter37 = new OracleParameter("I_GP_TRATOTAL_PROC_PLANTA_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter38 = new OracleParameter("I_GP_TRATOTAL_PROC_PLANTA", OracleDbType.Double);
                    OracleParameter oracleParameter39 = new OracleParameter("I_GP_TRANSFORMADO_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter40 = new OracleParameter("I_GP_TRANSFORMADO", OracleDbType.Double);


                    if (string.IsNullOrEmpty(item.total_procesado_planta_tp))
                    {
                        oracleParameter35.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter35.Value = item.total_procesado_planta_tp;
                    }
                    if (item.total_procesado_planta.Equals(null))
                    {
                        oracleParameter36.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter36.Value = item.total_procesado_planta;
                    }


                    if (string.IsNullOrEmpty(item.gp_tratotal_proc_planta_tp))
                    {
                        oracleParameter37.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter37.Value = item.gp_tratotal_proc_planta_tp;
                    }
                    if (item.gp_tratotal_proc_planta.Equals(null))
                    {
                        oracleParameter38.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter38.Value = item.gp_tratotal_proc_planta;
                    }


                    if (string.IsNullOrEmpty(item.gp_transformado_tp))
                    {
                        oracleParameter39.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter39.Value = item.gp_transformado_tp;
                    }
                    if (item.gp_transformado.Equals(null))
                    {
                        oracleParameter40.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter40.Value = item.gp_transformado;
                    }
                    OracleParameter oracleParameter41 = new OracleParameter("I_GP_CONSUMOS_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter42 = new OracleParameter("I_GP_CONSUMOS", OracleDbType.Double);
                    OracleParameter oracleParameter43 = new OracleParameter("I_GP_GASODUCTOS_URBANOS_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter44 = new OracleParameter("I_GP_GASODUCTOS_URBANOS", OracleDbType.Double);
                    OracleParameter oracleParameter45 = new OracleParameter("I_GP_GENERACION_ELECTRICA_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter46 = new OracleParameter("I_GP_GENERACION_ELECTRICA", OracleDbType.Double);


                    if (string.IsNullOrEmpty(item.gp_consumos_tp))
                    {
                        oracleParameter41.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter41.Value = item.gp_consumos_tp;
                    }
                    if (item.gp_consumos.Equals(null))
                    {
                        oracleParameter42.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter42.Value = item.gp_consumos;
                    }


                    if (string.IsNullOrEmpty(item.gp_gasoductos_urbanos_tp))
                    {
                        oracleParameter43.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter43.Value = item.gp_gasoductos_urbanos_tp;
                    }
                    if (item.gp_gasoductos_urbanos.Equals(null))
                    {
                        oracleParameter44.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter44.Value = item.gp_gasoductos_urbanos;
                    }


                    if (string.IsNullOrEmpty(item.gp_generacion_elect_tp))
                    {
                        oracleParameter45.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter45.Value = item.gp_generacion_elect_tp;
                    }
                    if (item.gp_generacion_elect.Equals(null))
                    {
                        oracleParameter46.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter46.Value = item.gp_generacion_elect;
                    }
                    OracleParameter oracleParameter47 = new OracleParameter("I_GP_OTRAS_VENTAS_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter48 = new OracleParameter("I_GP_OTRAS_VENTAS", OracleDbType.Double);
                    OracleParameter oracleParameter49 = new OracleParameter("I_GP_NEUMATICO_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter50 = new OracleParameter("I_GP_NEUMATICO", OracleDbType.Double);
                    OracleParameter oracleParameter51 = new OracleParameter("I_GP_QUEMADO_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter52 = new OracleParameter("I_GP_QUEMADO", OracleDbType.Double);
                    OracleParameter oracleParameter53 = new OracleParameter("I_GP_INYECTADO_TP", OracleDbType.Varchar2);
                    OracleParameter oracleParameter54 = new OracleParameter("I_GP_INYECTADO", OracleDbType.Double);

                    if (string.IsNullOrEmpty(item.gp_otras_ventas_tp))
                    {
                        oracleParameter47.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter47.Value = item.gp_otras_ventas_tp;
                    }
                    if (item.gp_otras_ventas.Equals(null))
                    {
                        oracleParameter48.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter48.Value = item.gp_otras_ventas;
                    }

                    if (string.IsNullOrEmpty(item.gp_neumatico_tp))
                    {
                        oracleParameter49.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter49.Value = item.gp_neumatico_tp;
                    }
                    if (item.gp_neumatico.Equals(null))
                    {
                        oracleParameter50.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter50.Value = item.gp_neumatico;
                    }


                    if (string.IsNullOrEmpty(item.gp_quemado_tp))
                    {
                        oracleParameter51.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter51.Value = item.gp_quemado_tp;
                    }
                    if (item.gp_quemado.Equals(null))
                    {
                        oracleParameter52.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter52.Value = item.gp_quemado;
                    }

                    if (string.IsNullOrEmpty(item.gp_inyectado_tp))
                    {
                        oracleParameter53.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter53.Value = item.gp_inyectado_tp;
                    }
                    if (item.gp_inyectado.Equals(null))
                    {
                        oracleParameter54.Value = DBNull.Value;
                    }
                    else
                    {
                        oracleParameter54.Value = item.gp_inyectado;
                    }
                    OracleParameter oracleParameter55 = new OracleParameter("IID", OracleDbType.Int32);
                    OracleParameter oracleParameter56 = new OracleParameter("IERROR", OracleDbType.Varchar2, 300);
                    oracleParameter55.Value = 0;
                    oracleParameter56.Value = "Ok";
                    oracleParameter55.Direction = ParameterDirection.InputOutput;
                    oracleParameter56.Direction = ParameterDirection.InputOutput;


                    OracleParameter[] oracleParameters = { oracleParameter0, oracleParameter1, oracleParameter2, oracleParameter3, oracleParameter4, oracleParameter5, oracleParameter6, oracleParameter7, oracleParameter8, oracleParameter9, oracleParameter10,
                                                   oracleParameter11, oracleParameter12, oracleParameter13, oracleParameter14, oracleParameter15, oracleParameter16, oracleParameter17, oracleParameter18, oracleParameter19, oracleParameter20,
                                                   oracleParameter21, oracleParameter22, oracleParameter23, oracleParameter24, oracleParameter25, oracleParameter26, oracleParameter27, oracleParameter28, oracleParameter29, oracleParameter30,
                                                   oracleParameter31, oracleParameter32, oracleParameter33, oracleParameter34, oracleParameter35, oracleParameter36, oracleParameter37, oracleParameter38, oracleParameter39, oracleParameter40,
                                                   oracleParameter41, oracleParameter42, oracleParameter43, oracleParameter44, oracleParameter45, oracleParameter46, oracleParameter47, oracleParameter48, oracleParameter49, oracleParameter50,
                                                   oracleParameter51, oracleParameter52, oracleParameter53, oracleParameter54, oracleParameter55, oracleParameter56};

                    command.Parameters.Clear();
                    command.Parameters.AddRange(oracleParameters);
                    command.Connection = connection;
                    command.ExecuteNonQuery();

                    string salida = oracleParameter56.Value.ToString();
                    string entrada = "INSERTADO EXITOSAMENTE EL REGISTRO DE F30/";
                    bool result = salida.Equals(entrada);

                    if (!result)
                    {
                        resultado = false;
                    }
                }
                if (resultado)
                {
                    transaction.Commit();
                    connection.Close();
                }
                else
                {
                    transaction.Rollback();
                    connection.Close();
                }

                return Task.Run(() => resultado);
            }
            catch (Exception)
            {
                transaction.Rollback();
                return Task.Run(() => false);
            }

        }

        public Task<bool> PvsCrea(ParametrosPvsCrea id, string p_code)
        {
            using OracleConnection connection = new OracleConnection(OracleContext.ConnectionString);
            OracleCommand command = new OracleCommand("admonfunc.PKG_MBDP_CARGUE_FORMA.FORMA30_PVS_CREA")
            {
                CommandType = CommandType.StoredProcedure
            };

            OracleParameter oracleParameter0 = new OracleParameter("I_PDEN_ID", OracleDbType.Varchar2);
            OracleParameter oracleParameter1 = new OracleParameter("I_VOL_METH", OracleDbType.Varchar2);
            OracleParameter oracleParameter2 = new OracleParameter("I_ACT_TYPE", OracleDbType.Varchar2);
            OracleParameter oracleParameter3 = new OracleParameter("I_PER_TYPE", OracleDbType.Varchar2);
            OracleParameter oracleParameter4 = new OracleParameter("I_VOL_DATE", OracleDbType.Varchar2);
            OracleParameter oracleParameter5 = new OracleParameter("I_VOLUMEN", OracleDbType.Double);
            OracleParameter oracleParameter6 = new OracleParameter("I_USER", OracleDbType.Varchar2);
            oracleParameter0.Value = id.pden_id;
            oracleParameter1.Value = id.vol_meth;
            oracleParameter2.Value = id.act_type;
            oracleParameter3.Value = id.per_type;
            oracleParameter4.Value = id.vol_date.ToString("yyyyMMdd");
            oracleParameter5.Value = id.volumen;
            oracleParameter6.Value = p_code;

            oracleParameter4.Value = DBNull.Value;
            OracleParameter[] oracleParameters = { oracleParameter0, oracleParameter1, oracleParameter2, oracleParameter3, oracleParameter4, oracleParameter5, oracleParameter6 };
            command.Parameters.AddRange(oracleParameters);

            command.Connection = connection;
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            var lstResultado = true;
            return Task.Run(() => lstResultado);
        }

        public Task<bool> PvsoCrea(ParametrosPvsoCrea id, string p_code)
        {
            using OracleConnection connection = new OracleConnection(OracleContext.ConnectionString);
            OracleCommand command = new OracleCommand("admonfunc.PKG_MBDP_CARGUE_FORMA.FORMA30_PVSO_CREA")
            {
                CommandType = CommandType.StoredProcedure
            };

            OracleParameter oracleParameter0 = new OracleParameter("I_PDEN_ID", OracleDbType.Varchar2);
            OracleParameter oracleParameter1 = new OracleParameter("I_VOL_METH", OracleDbType.Varchar2);
            OracleParameter oracleParameter2 = new OracleParameter("I_ACT_TYPE", OracleDbType.Varchar2);
            OracleParameter oracleParameter3 = new OracleParameter("I_PRO_TYPE", OracleDbType.Varchar2);
            OracleParameter oracleParameter4 = new OracleParameter("I_PER_TYPE", OracleDbType.Varchar2);
            OracleParameter oracleParameter5 = new OracleParameter("I_VOL_DATE", OracleDbType.Varchar2);
            OracleParameter oracleParameter6 = new OracleParameter("I_VOLUMEN", OracleDbType.Double);
            OracleParameter oracleParameter7 = new OracleParameter("I_USER", OracleDbType.Varchar2);

            oracleParameter0.Value = id.pden_id;
            oracleParameter1.Value = id.vol_meth;
            oracleParameter2.Value = id.act_type;
            oracleParameter3.Value = id.pro_type;
            oracleParameter4.Value = id.per_type;
            oracleParameter5.Value = id.vol_date.ToString("yyyyMMdd");
            oracleParameter6.Value = id.volumen;
            oracleParameter7.Value = p_code;

            oracleParameter4.Value = DBNull.Value;
            OracleParameter[] oracleParameters = { oracleParameter0, oracleParameter1, oracleParameter2, oracleParameter3, oracleParameter4, oracleParameter5, oracleParameter6, oracleParameter7 };
            command.Parameters.AddRange(oracleParameters);

            command.Connection = connection;
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            var lstResultado = true;
            return Task.Run(() => lstResultado);
        }
    }
}
