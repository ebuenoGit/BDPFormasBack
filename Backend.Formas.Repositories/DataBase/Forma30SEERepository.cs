using Backend.Formas.Entities.Interface.Repository;
using Backend.Formas.Entities.Models;
using Backend.Formas.Repositories.Context;
using Backend.Formas.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Backend.Formas.Repositories.DataBase
{
    public class Forma30SEERepository : IForma30SEERepository
    {
        private readonly FormasContext _formas;

        public Forma30SEERepository(FormasContext formas)
        {
            _formas = formas;
        }

        public async Task<DataTable> consultarCargueForma9(DateTime fechaForma9, string forma)
        {
            try
            {
                SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                SqlCommand command = new SqlCommand("FOXT.SP_ConsultarCargueForma9")
                {
                    Connection = conection
                };
                conection.Open();
                SqlParameter Parameter1 = new SqlParameter("@fecha", SqlDbType.Date);
                SqlParameter Parameter2 = new SqlParameter("@forma", SqlDbType.VarChar);
                Parameter1.Value = fechaForma9;
                Parameter2.Value = forma;
                SqlParameter[] parameters = { Parameter1, Parameter2 };
                command.Parameters.AddRange(parameters);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                DataTable DT = new DataTable();
                sqlDataAdapter.SelectCommand = command;
                sqlDataAdapter.Fill(DT);
                List<dynamic> LstResultadoTabla = ConvertTypes.ToDynamic(DT);
                conection.Close();
                //var LstResultado = true;
                return await Task.Run(() => DT);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GuardarCabecera(BDConcreteForm Cabecera)
        {
            try
            {
                SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                SqlCommand command = new SqlCommand("FOXT.SP_GuardarCabeceraForma30")
                {
                    Connection = conection
                };
                conection.Open();
                SqlParameter Parameter1 = new SqlParameter("@concreteformid", SqlDbType.UniqueIdentifier);
                SqlParameter Parameter2 = new SqlParameter("@maincampid", SqlDbType.Decimal);
                SqlParameter Parameter3 = new SqlParameter("@company", SqlDbType.VarChar);
                SqlParameter Parameter4 = new SqlParameter("@contract", SqlDbType.VarChar);
                SqlParameter Parameter5 = new SqlParameter("@battery", SqlDbType.VarChar);
                SqlParameter Parameter6 = new SqlParameter("@tank", SqlDbType.VarChar);
                SqlParameter Parameter7 = new SqlParameter("@month", SqlDbType.Decimal);
                SqlParameter Parameter8 = new SqlParameter("@year", SqlDbType.Decimal);
                SqlParameter Parameter9 = new SqlParameter("@explotationmodality", SqlDbType.VarChar);
                SqlParameter Parameter10 = new SqlParameter("@annotations", SqlDbType.VarChar);
                SqlParameter Parameter11 = new SqlParameter("@version", SqlDbType.Decimal);
                SqlParameter Parameter12 = new SqlParameter("@currentstate", SqlDbType.UniqueIdentifier);
                SqlParameter Parameter13 = new SqlParameter("@generationflag", SqlDbType.Decimal);
                SqlParameter Parameter14 = new SqlParameter("@campid", SqlDbType.Decimal);
                SqlParameter Parameter15 = new SqlParameter("@pdenid", SqlDbType.VarChar);
                SqlParameter Parameter16 = new SqlParameter("@formid", SqlDbType.UniqueIdentifier);
                SqlParameter Parameter17 = new SqlParameter("@generationjobid", SqlDbType.Decimal);
                SqlParameter Parameter18 = new SqlParameter("@iqistatus", SqlDbType.Decimal);
                SqlParameter Parameter19 = new SqlParameter("@usersigning", SqlDbType.VarChar);
                SqlParameter Parameter20 = new SqlParameter("@minrepsigning", SqlDbType.VarChar);
                SqlParameter Parameter21 = new SqlParameter("@formname", SqlDbType.VarChar);

                Parameter1.Value = Cabecera.concreteformid;
                Parameter2.Value = Cabecera.maincampid;
                Parameter3.Value = Cabecera.company;
                Parameter4.Value = Cabecera.contract;
                Parameter5.Value = Cabecera.battery;
                Parameter6.Value = Cabecera.tank;
                Parameter7.Value = Cabecera.month;
                Parameter8.Value = Cabecera.year;
                Parameter9.Value = Cabecera.explotationmodality;
                Parameter10.Value = Cabecera.annotations;
                Parameter11.Value = Cabecera.version;
                Parameter12.Value = Cabecera.currentstate;
                Parameter13.Value = Cabecera.generationflag;
                Parameter14.Value = Cabecera.campid;
                Parameter15.Value = Cabecera.pdenid;
                Parameter16.Value = Cabecera.formid;
                Parameter17.Value = Cabecera.generationjobid;
                Parameter18.Value = Cabecera.iqistatus;
                Parameter19.Value = Cabecera.usersigning;
                Parameter20.Value = Cabecera.minrepsigning;
                Parameter21.Value = Cabecera.formname;

                SqlParameter[] parameters = { Parameter1, Parameter2, Parameter3, Parameter4, Parameter5, Parameter6, Parameter7, Parameter8, Parameter9, Parameter10, Parameter11, Parameter12, Parameter13, Parameter14, Parameter15, Parameter16, Parameter17, Parameter18, Parameter19, Parameter20, Parameter21 };
                command.Parameters.AddRange(parameters);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                DataTable DT = new DataTable();
                sqlDataAdapter.SelectCommand = command;
                sqlDataAdapter.Fill(DT);
                List<dynamic> LstResultadoTabla = ConvertTypes.ToDynamic(DT);
                conection.Close();
                return DT;//await Task.Run(() => DT);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GuardarForma30BD(BDForma30 dat)
        {
            try
            {
                SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                SqlCommand command = new SqlCommand("FOXT.SP_GuardarForma30")
                {
                    Connection = conection
                };
                conection.Open();
                SqlParameter Parameter1 = new SqlParameter("@form30id", SqlDbType.UniqueIdentifier);
                SqlParameter Parameter2 = new SqlParameter("@operador", SqlDbType.VarChar);
                SqlParameter Parameter3 = new SqlParameter("@contrato", SqlDbType.VarChar);
                SqlParameter Parameter4 = new SqlParameter("@date", SqlDbType.Date);
                SqlParameter Parameter5 = new SqlParameter("@uid", SqlDbType.VarChar);
                SqlParameter Parameter6 = new SqlParameter("@lastModified", SqlDbType.Decimal);
                SqlParameter Parameter7 = new SqlParameter("@lastModifiedDate", SqlDbType.DateTime);
                SqlParameter Parameter8 = new SqlParameter("@name", SqlDbType.VarChar);
                SqlParameter Parameter9 = new SqlParameter("@size", SqlDbType.Decimal);
                SqlParameter Parameter10 = new SqlParameter("@type", SqlDbType.VarChar);
                SqlParameter Parameter11 = new SqlParameter("@percent", SqlDbType.Decimal);
                SqlParameter Parameter12 = new SqlParameter("@originFileObj_uid", SqlDbType.VarChar);
                SqlParameter Parameter13 = new SqlParameter("@operador_id", SqlDbType.VarChar);
                SqlParameter Parameter14 = new SqlParameter("@contrato_id", SqlDbType.VarChar);
                SqlParameter Parameter15 = new SqlParameter("@usuario", SqlDbType.VarChar);
                


                Parameter1.Value = dat.form30id;
                Parameter2.Value = dat.operador;
                Parameter3.Value = dat.contrato;
                Parameter4.Value = dat.date;
                Parameter5.Value = dat.uid;
                Parameter6.Value = dat.lastModified;
                Parameter7.Value = dat.lastModifiedDate;
                Parameter8.Value = dat.name;
                Parameter9.Value = dat.size;
                Parameter10.Value = dat.type;
                Parameter11.Value = dat.percent;
                Parameter12.Value = dat.originFileObj_uid;
                Parameter13.Value = dat.operador_id;
                Parameter14.Value = dat.contrato_id;
                Parameter15.Value = dat.usuario;

                SqlParameter[] parameters = { Parameter1, Parameter2, Parameter3, Parameter4, Parameter5, Parameter6, Parameter7, Parameter8, Parameter9, Parameter10, Parameter11, Parameter12, Parameter13, Parameter14,Parameter15 };
                command.Parameters.AddRange(parameters);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                DataTable DT = new DataTable();
                sqlDataAdapter.SelectCommand = command;
                sqlDataAdapter.Fill(DT);
                List<dynamic> LstResultadoTabla = ConvertTypes.ToDynamic(DT);
                conection.Close();
                return DT;//await Task.Run(() => DT);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GuardarForma30BD(BDForma30Detail dat)
        {
            try
            {
                SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                SqlCommand command = new SqlCommand("FOXT.SP_GuardarForma30Detail")
                {
                    Connection = conection
                };
                conection.Open();
                SqlParameter Parameter1 = new SqlParameter("@form30detail", SqlDbType.UniqueIdentifier);
                SqlParameter Parameter2 = new SqlParameter("@formid", SqlDbType.UniqueIdentifier);
                SqlParameter Parameter3 = new SqlParameter("@campo", SqlDbType.VarChar);
                SqlParameter Parameter4 = new SqlParameter("@gasFormacionKPC", SqlDbType.Decimal);
                SqlParameter Parameter5 = new SqlParameter("@contenidoPropano", SqlDbType.Decimal);
                SqlParameter Parameter6 = new SqlParameter("@contenidoButano", SqlDbType.Decimal);
                SqlParameter Parameter7 = new SqlParameter("@contenidoGasolinaNatural", SqlDbType.Decimal);
                SqlParameter Parameter8 = new SqlParameter("@gasFormacionProcesado", SqlDbType.Decimal);
                SqlParameter Parameter9 = new SqlParameter("@consumoEnCampo", SqlDbType.Decimal);
                SqlParameter Parameter10 = new SqlParameter("@generacionElectrica", SqlDbType.Decimal);
                SqlParameter Parameter11 = new SqlParameter("@otros", SqlDbType.Decimal);
                SqlParameter Parameter12 = new SqlParameter("@quemadoAire", SqlDbType.Decimal);
                SqlParameter Parameter13 = new SqlParameter("@usadoBombeoNeumatico", SqlDbType.Decimal);
                SqlParameter Parameter14 = new SqlParameter("@inyectadoYacimiento", SqlDbType.Decimal);
                SqlParameter Parameter15 = new SqlParameter("@productosObtenidosGasTotalGasProcesadoPlanta", SqlDbType.Decimal);
                SqlParameter Parameter16 = new SqlParameter("@productosObtenidosGasPropano", SqlDbType.Decimal);
                SqlParameter Parameter17 = new SqlParameter("@productosObtenidosGasButano", SqlDbType.Decimal);
                SqlParameter Parameter18 = new SqlParameter("@productosObtenidosGasGasolina", SqlDbType.Decimal);
                SqlParameter Parameter19 = new SqlParameter("@productosObtenidosGasTransformadoGasolinaNaturalPropanosButanos", SqlDbType.Decimal);
                SqlParameter Parameter20 = new SqlParameter("@productosGasProcesadoConsumoEnCampo", SqlDbType.Decimal);
                SqlParameter Parameter21 = new SqlParameter("@productosGasProcesadoGasoductosUrbanos", SqlDbType.Decimal);
                SqlParameter Parameter22 = new SqlParameter("@productosGasProcesadoGeneracionElectrica", SqlDbType.Decimal);
                SqlParameter Parameter23 = new SqlParameter("@productosGasProcesadoOtros", SqlDbType.Decimal);
                SqlParameter Parameter24 = new SqlParameter("@productosGasProcesadoQuemadoAire", SqlDbType.Decimal);
                SqlParameter Parameter25 = new SqlParameter("@productosGasProcesadoUsadoBombeoNeumatico", SqlDbType.Decimal);
                SqlParameter Parameter26 = new SqlParameter("@productosGasProcesadoInyectadoYacimiento", SqlDbType.Decimal);
                SqlParameter Parameter27 = new SqlParameter("@productosGasProcesadoObservaciones", SqlDbType.VarChar);
                SqlParameter Parameter28 = new SqlParameter("@REAL_TP", SqlDbType.VarChar);
                SqlParameter Parameter29 = new SqlParameter("@REAL", SqlDbType.VarChar);
                SqlParameter Parameter30 = new SqlParameter("@REAL_BAS", SqlDbType.VarChar);
                SqlParameter Parameter31 = new SqlParameter("@REAL_INC", SqlDbType.VarChar);
                SqlParameter Parameter32 = new SqlParameter("@GAS_PROCESADO_TP", SqlDbType.VarChar);
                SqlParameter Parameter33 = new SqlParameter("@GAS_QUEMADO_TP", SqlDbType.VarChar);
                SqlParameter Parameter34 = new SqlParameter("@GLP_TP", SqlDbType.VarChar);
                SqlParameter Parameter35 = new SqlParameter("@GLP", SqlDbType.VarChar);
                SqlParameter Parameter36 = new SqlParameter("@PROPANO_TP", SqlDbType.VarChar);
                SqlParameter Parameter37 = new SqlParameter("@BUTANO_TP", SqlDbType.VarChar);
                SqlParameter Parameter38 = new SqlParameter("@GASOLINA_TP", SqlDbType.VarChar);
                SqlParameter Parameter39 = new SqlParameter("@APIASOL_TP", SqlDbType.VarChar);
                SqlParameter Parameter40 = new SqlParameter("@APIASOL", SqlDbType.VarChar);
                SqlParameter Parameter41 = new SqlParameter("@CONDENSADO_TP", SqlDbType.VarChar);
                SqlParameter Parameter42 = new SqlParameter("@CONDENSADO", SqlDbType.VarChar);
                SqlParameter Parameter43 = new SqlParameter("@CONSUMOS_TP", SqlDbType.VarChar);
                SqlParameter Parameter44 = new SqlParameter("@GASODUCTOS_URBANOS_TP", SqlDbType.VarChar);
                SqlParameter Parameter45 = new SqlParameter("@GASODUCTOS_URBANOS", SqlDbType.VarChar);
                SqlParameter Parameter46 = new SqlParameter("@GENERACION_ELECTRICA_TP", SqlDbType.VarChar);
                SqlParameter Parameter47 = new SqlParameter("@OTRAS_VENTAS_TP", SqlDbType.VarChar);
                SqlParameter Parameter48 = new SqlParameter("@GAS_TRANFERENCIA_TP", SqlDbType.VarChar);
                SqlParameter Parameter49 = new SqlParameter("@GAS_TRANFERENCIA", SqlDbType.VarChar);
                SqlParameter Parameter50 = new SqlParameter("@GAS_NEUMATICO_TP", SqlDbType.VarChar);
                SqlParameter Parameter51 = new SqlParameter("@GAS_INYECTADO_TP", SqlDbType.VarChar);
                SqlParameter Parameter52 = new SqlParameter("@TOTAL_PROC_PLANTA_TP", SqlDbType.VarChar);
                SqlParameter Parameter53 = new SqlParameter("@GP_TRATOTAL_PROC_PLANTA_TP", SqlDbType.VarChar);
                SqlParameter Parameter54 = new SqlParameter("@GP_TRATOTAL_PROC_PLANTA", SqlDbType.VarChar);
                SqlParameter Parameter55 = new SqlParameter("@GP_CONSUMOS_TP", SqlDbType.VarChar);
                SqlParameter Parameter56 = new SqlParameter("@GP_GASODUCTOS_URBANOS_TP", SqlDbType.VarChar);
                SqlParameter Parameter57 = new SqlParameter("@GP_GENERACION_ELECTRICA_TP", SqlDbType.VarChar);
                SqlParameter Parameter58 = new SqlParameter("@GP_OTRAS_VENTAS_TP", SqlDbType.VarChar);
                SqlParameter Parameter59 = new SqlParameter("@GP_NEUMATICO_TP", SqlDbType.VarChar);
                SqlParameter Parameter60 = new SqlParameter("@GP_QUEMADO_TP", SqlDbType.VarChar);
                SqlParameter Parameter61 = new SqlParameter("@GP_INYECTADO_TP", SqlDbType.VarChar);
                SqlParameter Parameter62 = new SqlParameter("@REPRESENTA_OPER", SqlDbType.VarChar);
                SqlParameter Parameter63 = new SqlParameter("@REPRESENTA_ANH", SqlDbType.VarChar);
                SqlParameter Parameter64 = new SqlParameter("@IDFORMA", SqlDbType.VarChar);
                SqlParameter Parameter65 = new SqlParameter("@campo_id", SqlDbType.VarChar);
                SqlParameter Parameter66 = new SqlParameter("@pden_id", SqlDbType.VarChar);
                SqlParameter Parameter67 = new SqlParameter("@volume_date", SqlDbType.VarChar);
                SqlParameter Parameter68 = new SqlParameter("@usuario", SqlDbType.VarChar);

                Parameter1.Value = dat.form30detail;
                Parameter2.Value = dat.formid;
                Parameter3.Value = dat.campo;
                Parameter4.Value = dat.gasFormacionKPC;
                Parameter5.Value = dat.contenidoPropano;
                Parameter6.Value = dat.contenidoButano;
                Parameter7.Value = dat.contenidoGasolinaNatural;
                Parameter8.Value = dat.gasFormacionProcesado;
                Parameter9.Value = dat.consumoEnCampo;
                Parameter10.Value = dat.generacionElectrica;
                Parameter11.Value = dat.otros;
                Parameter12.Value = dat.quemadoAire;
                Parameter13.Value = dat.usadoBombeoNeumatico;
                Parameter14.Value = dat.inyectadoYacimiento;
                Parameter15.Value = dat.productosObtenidosGasTotalGasProcesadoPlanta;
                Parameter16.Value = dat.productosObtenidosGasPropano;
                Parameter17.Value = dat.productosObtenidosGasButano;
                Parameter18.Value = dat.productosObtenidosGasGasolina;
                Parameter19.Value = dat.productosObtenidosGasTransformadoGasolinaNaturalPropanosButanos;
                Parameter20.Value = dat.productosGasProcesadoConsumoEnCampo;
                Parameter21.Value = dat.productosGasProcesadoGasoductosUrbanos;
                Parameter22.Value = dat.productosGasProcesadoGeneracionElectrica;
                Parameter23.Value = dat.productosGasProcesadoOtros;
                Parameter24.Value = dat.productosGasProcesadoQuemadoAire;
                Parameter25.Value = dat.productosGasProcesadoUsadoBombeoNeumatico;
                Parameter26.Value = dat.productosGasProcesadoInyectadoYacimiento;
                Parameter27.Value = dat.productosGasProcesadoObservaciones??"";
                Parameter28.Value = dat.REAL_TP;
                Parameter29.Value = dat.REAL;
                Parameter30.Value = dat.REAL_BAS;
                Parameter31.Value = dat.REAL_INC;
                Parameter32.Value = dat.GAS_PROCESADO_TP;
                Parameter33.Value = dat.GAS_QUEMADO_TP;
                Parameter34.Value = dat.GLP_TP;
                Parameter35.Value = dat.GLP;
                Parameter36.Value = dat.PROPANO_TP;
                Parameter37.Value = dat.BUTANO_TP;
                Parameter38.Value = dat.GASOLINA_TP;
                Parameter39.Value = dat.APIASOL_TP;
                Parameter40.Value = dat.APIASOL;
                Parameter41.Value = dat.CONDENSADO_TP;
                Parameter42.Value = dat.CONDENSADO;
                Parameter43.Value = dat.CONSUMOS_TP;
                Parameter44.Value = dat.GASODUCTOS_URBANOS_TP;
                Parameter45.Value = dat.GASODUCTOS_URBANOS;
                Parameter46.Value = dat.GENERACION_ELECTRICA_TP;
                Parameter47.Value = dat.OTRAS_VENTAS_TP;
                Parameter48.Value = dat.GAS_TRANFERENCIA_TP;
                Parameter49.Value = dat.GAS_TRANFERENCIA;
                Parameter50.Value = dat.GAS_NEUMATICO_TP;
                Parameter51.Value = dat.GAS_INYECTADO_TP;
                Parameter51.Value = dat.TOTAL_PROC_PLANTA_TP;
                Parameter53.Value = dat.GP_TRATOTAL_PROC_PLANTA_TP;
                Parameter54.Value = dat.GP_TRATOTAL_PROC_PLANTA;
                Parameter55.Value = dat.GP_CONSUMOS_TP;
                Parameter56.Value = dat.GP_GASODUCTOS_URBANOS_TP;
                Parameter57.Value = dat.GP_GENERACION_ELECTRICA_TP;
                Parameter58.Value = dat.GP_OTRAS_VENTAS_TP;
                Parameter59.Value = dat.GP_NEUMATICO_TP;
                Parameter60.Value = dat.GP_QUEMADO_TP;
                Parameter61.Value = dat.GP_INYECTADO_TP;
                Parameter62.Value = dat.REPRESENTA_OPER;
                Parameter63.Value = dat.REPRESENTA_ANH;
                Parameter64.Value = dat.IDFORMA;
                Parameter65.Value = dat.campo_id;
                Parameter66.Value = dat.pden_id;
                Parameter67.Value = dat.volume_date;
                Parameter68.Value = dat.usuario;

                SqlParameter[] parameters = { Parameter1, Parameter2, Parameter3, Parameter4, Parameter5, Parameter6, Parameter7, Parameter8, Parameter9, Parameter10,
                    Parameter11, Parameter12, Parameter13, Parameter14,Parameter15, Parameter16, Parameter17, Parameter18, Parameter19,Parameter20,
                    Parameter21, Parameter22, Parameter23, Parameter24, Parameter25, Parameter26, Parameter27, Parameter28,Parameter29,Parameter30,
                    Parameter31, Parameter32, Parameter33, Parameter34, Parameter35, Parameter36, Parameter37, Parameter38, Parameter39,Parameter40,
                    Parameter41, Parameter42, Parameter43, Parameter44, Parameter45, Parameter46, Parameter47, Parameter48, Parameter49,Parameter50,
                    Parameter51, Parameter52, Parameter53, Parameter54, Parameter55, Parameter56, Parameter57, Parameter58, Parameter59,Parameter60,
                    Parameter61, Parameter62, Parameter63, Parameter64, Parameter65, Parameter66, Parameter67,Parameter68};
                command.Parameters.AddRange(parameters);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                DataTable DT = new DataTable();
                sqlDataAdapter.SelectCommand = command;
                sqlDataAdapter.Fill(DT);
                List<dynamic> LstResultadoTabla = ConvertTypes.ToDynamic(DT);
                conection.Close();
                return DT;//await Task.Run(() => DT);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable guardarAprobacion(AprobacionCarga dat)
        {
            try
            {
                SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                SqlCommand command = new SqlCommand("FOXT.SP_GuardarAprobacion30")
                {
                    Connection = conection
                };
                conection.Open();
                SqlParameter Parameter1 = new SqlParameter("@ID_Forma", SqlDbType.UniqueIdentifier);
                SqlParameter Parameter2 = new SqlParameter("@FechaForma", SqlDbType.VarChar);
                SqlParameter Parameter3 = new SqlParameter("@Usuario", SqlDbType.VarChar);
                SqlParameter Parameter4 = new SqlParameter("@UsuarioNombre", SqlDbType.VarChar);
                SqlParameter Parameter5 = new SqlParameter("@FechaCarga", SqlDbType.DateTime);
                SqlParameter Parameter6 = new SqlParameter("@Estado", SqlDbType.UniqueIdentifier);
                SqlParameter Parameter7 = new SqlParameter("@ComparativoAgua", SqlDbType.Decimal);
                SqlParameter Parameter8 = new SqlParameter("@ComparativoGas", SqlDbType.Decimal);
                SqlParameter Parameter9 = new SqlParameter("@ComparativoCrudo", SqlDbType.Decimal);
                SqlParameter Parameter10 = new SqlParameter("@FechaActualizacion", SqlDbType.DateTime);
                SqlParameter Parameter11 = new SqlParameter("@UsuarioAprobador", SqlDbType.VarChar);
                SqlParameter Parameter12 = new SqlParameter("@UsuarioNombreAprobador", SqlDbType.VarChar);
                SqlParameter Parameter13 = new SqlParameter("@FormTypeID", SqlDbType.UniqueIdentifier);
                SqlParameter Parameter14 = new SqlParameter("@FormEntidad", SqlDbType.VarChar);
                SqlParameter Parameter15 = new SqlParameter("@FormaName", SqlDbType.VarChar);
                SqlParameter Parameter16 = new SqlParameter("@urlForma", SqlDbType.VarChar);
                SqlParameter Parameter17 = new SqlParameter("@operador", SqlDbType.VarChar);
                SqlParameter Parameter18 = new SqlParameter("@contrato", SqlDbType.VarChar);
                SqlParameter Parameter19 = new SqlParameter("@campo", SqlDbType.VarChar);


                Parameter1.Value = dat.ID_Forma;
                Parameter2.Value = dat.FechaForma;
                Parameter3.Value = dat.Usuario;
                Parameter4.Value = dat.UsuarioNombre;
                Parameter5.Value = dat.FechaCarga;
                Parameter6.Value = dat.Estado;
                Parameter7.Value = dat.ComparativoAgua;
                Parameter8.Value = dat.ComparativoGas;
                Parameter9.Value = dat.ComparativoCrudo;
                Parameter10.Value = dat.FechaActualizacion;
                Parameter11.Value = dat.UsuarioAprobador;
                Parameter12.Value = dat.UsuarioNombreAprobador;
                Parameter13.Value = dat.FormTypeID;
                Parameter14.Value = dat.FormEntidad;
                Parameter15.Value = dat.FormaName;
                Parameter16.Value = dat.urlForma;
                Parameter17.Value = dat.operador;
                Parameter18.Value = dat.contrato;
                Parameter19.Value = dat.campo;

                SqlParameter[] parameters = { Parameter1, Parameter2, Parameter3, Parameter4, Parameter5, Parameter6, Parameter7, Parameter8, Parameter9, Parameter10, Parameter11, Parameter12, Parameter13, Parameter14, Parameter15, Parameter16, Parameter17, Parameter18, Parameter19 };
                command.Parameters.AddRange(parameters);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                DataTable DT = new DataTable();
                sqlDataAdapter.SelectCommand = command;
                sqlDataAdapter.Fill(DT);
                List<dynamic> LstResultadoTabla = ConvertTypes.ToDynamic(DT);
                conection.Close();
                return DT;//await Task.Run(() => DT);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public Task<DataTable> ConsultarCabeceraForma30(Guid? id)
        {
            try
            {
                SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                SqlCommand command = new SqlCommand("FOXT.SP_ConsultarCabeceraForma30")
                {
                    Connection = conection
                };
                conection.Open();
                SqlParameter Parameter1 = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                {
                    Value = id
                };

                SqlParameter[] parameters = { Parameter1 };
                command.Parameters.AddRange(parameters);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                DataTable DT = new DataTable();
                sqlDataAdapter.SelectCommand = command;
                sqlDataAdapter.Fill(DT);
                List<dynamic> LstResultadoTabla = ConvertTypes.ToDynamic(DT);
                conection.Close();
                return Task.Run(() => DT);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<DataTable> ConsultarCabeceraForma30Reg(Guid? id)
        {
            try
            {
                SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                SqlCommand command = new SqlCommand("FOXT.SP_ConsultarForma30Reg")
                {
                    Connection = conection
                };
                conection.Open();
                SqlParameter Parameter1 = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                {
                    Value = id
                };

                SqlParameter[] parameters = { Parameter1 };
                command.Parameters.AddRange(parameters);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                DataTable DT = new DataTable();
                sqlDataAdapter.SelectCommand = command;
                sqlDataAdapter.Fill(DT);
                List<dynamic> LstResultadoTabla = ConvertTypes.ToDynamic(DT);
                conection.Close();
                return Task.Run(() => DT);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<DataTable> ConsultarCabeceraForma30Det(Guid? id)
        {
            try
            {
                SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                SqlCommand command = new SqlCommand("FOXT.SP_ConsultarForma30Det")
                {
                    Connection = conection
                };
                conection.Open();
                SqlParameter Parameter1 = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                {
                    Value = id
                };

                SqlParameter[] parameters = { Parameter1 };
                command.Parameters.AddRange(parameters);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                DataTable DT = new DataTable();
                sqlDataAdapter.SelectCommand = command;
                sqlDataAdapter.Fill(DT);
                List<dynamic> LstResultadoTabla = ConvertTypes.ToDynamic(DT);
                conection.Close();
                return Task.Run(() => DT);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<DataTable> ConsultarPermisoCargua(string forma, DateTime fechaForma, string operador, string contrato, string usuario)
        {
            try
            {
                SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                SqlCommand command = new SqlCommand("FOXT.SP_ConsultarPermisosCargueForma")
                {
                    Connection = conection
                };
                conection.Open();
                SqlParameter Parameter1 = new SqlParameter("@forma", SqlDbType.VarChar);
                SqlParameter Parameter2 = new SqlParameter("@fechaForma", SqlDbType.Date);
                SqlParameter Parameter3 = new SqlParameter("@operador", SqlDbType.VarChar);
                SqlParameter Parameter4 = new SqlParameter("@contrato", SqlDbType.VarChar);
                SqlParameter Parameter5 = new SqlParameter("@usuario", SqlDbType.VarChar);

                Parameter1.Value = forma;
                Parameter2.Value = fechaForma;
                Parameter3.Value = operador;
                Parameter4.Value = contrato;
                Parameter5.Value = usuario;

                SqlParameter[] parameters = { Parameter1, Parameter2, Parameter3, Parameter4, Parameter5 };
                command.Parameters.AddRange(parameters);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                DataTable DT = new DataTable();
                sqlDataAdapter.SelectCommand = command;
                sqlDataAdapter.Fill(DT);
                List<dynamic> LstResultadoTabla = ConvertTypes.ToDynamic(DT);
                conection.Close();
                return Task.Run(() => DT);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable ConsultarNotificacion(decimal id)
        {
            try
            {
                SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                SqlCommand command = new SqlCommand("FOXT.SP_ConsultarNotificacionFormas")
                {
                    Connection = conection
                };
                conection.Open();
                SqlParameter Parameter1 = new SqlParameter("@id", SqlDbType.Decimal)
                {
                    Value = id
                };

                SqlParameter[] parameters = { Parameter1 };
                command.Parameters.AddRange(parameters);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                DataTable DT = new DataTable();
                sqlDataAdapter.SelectCommand = command;
                sqlDataAdapter.Fill(DT);
                List<dynamic> LstResultadoTabla = ConvertTypes.ToDynamic(DT);
                conection.Close();
                return DT;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<DataTable> ConsultarEncabezado(decimal id)
        {
            try
            {
                SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                SqlCommand command = new SqlCommand("FOXT.SP_ConsultarCabeceraFormas")
                {
                    Connection = conection
                };
                conection.Open();

                SqlParameter Parameter1 = new SqlParameter("@id", SqlDbType.Decimal)
                {
                    Value = id
                };
                SqlParameter[] parameters = { Parameter1 };
                command.Parameters.AddRange(parameters);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                DataTable DT = new DataTable();
                sqlDataAdapter.SelectCommand = command;
                sqlDataAdapter.Fill(DT);
                conection.Close();
                return await Task.Run(() => DT);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<DataTable> ConsultarDetalle(decimal id)
        {
            try
            {
                SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                SqlCommand command = new SqlCommand("FOXT.SP_ConsultarDetalleForma20")
                {
                    Connection = conection
                };
                conection.Open();
                SqlParameter Parameter1 = new SqlParameter("@id", SqlDbType.Decimal)
                {
                    Value = id
                };
                SqlParameter[] parameters = { Parameter1 };
                command.Parameters.AddRange(parameters);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                DataTable DT = new DataTable();
                sqlDataAdapter.SelectCommand = command;
                sqlDataAdapter.Fill(DT);
                conection.Close();
                return await Task.Run(() => DT);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<DataTable> ConsultarpreCargua(string forma, DateTime fecha)
        {
            try
            {
                SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                SqlCommand command = new SqlCommand("FOXT.SP_VALIDA_PRECARGA")
                {
                    Connection = conection
                };
                conection.Open();
                SqlParameter Parameter1 = new SqlParameter("@FORMA", SqlDbType.VarChar);
                SqlParameter Parameter2 = new SqlParameter("@FECHAFORMA", SqlDbType.Date);

                Parameter1.Value = forma;
                Parameter2.Value = fecha;

                SqlParameter[] parameters = { Parameter1, Parameter2 };
                command.Parameters.AddRange(parameters);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                DataTable DT = new DataTable();
                sqlDataAdapter.SelectCommand = command;
                sqlDataAdapter.Fill(DT);
                conection.Close();
                return await Task.Run(() => DT);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
