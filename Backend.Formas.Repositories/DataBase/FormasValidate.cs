using Backend.Formas.Entities.Interface.Repository;
using Backend.Formas.Repositories.Context;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Backend.Formas.Repositories.DataBase
{
    public class FormasValidate : IFormaValidate
    {
        private readonly EcoOracleContext OracleContext;

        private readonly string paqueteConsulta = "ADMONFUNC.PKG_MBDP_CONSULTAS";
        private readonly string paqueteFormas = "ADMONFUNC.PKG_MBDP_FORMAS";

        public FormasValidate(EcoOracleContext oracleContext)
        {
            OracleContext = oracleContext;
        }

        public async Task<List<dynamic>> ValidaOperador(string nombre)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("p_Compania", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("OutError",OracleDbType.Varchar2,ParameterDirection.Output),
                new OracleParameter("Cur_Compania_Nom", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            parameters[0].Direction = ParameterDirection.InputOutput;
            parameters[1].Direction = ParameterDirection.InputOutput;
            parameters[2].Direction = ParameterDirection.Output;

            parameters[0].Value = nombre;
            parameters[1].Value = "OK";

            var ps = $"{paqueteConsulta}.COMPANIA_NOM_LTY";
            var data = await OracleContext.ExecuteProcedure<dynamic>(ps, parameters.ToArray());
            return data;
        }

        public async Task<List<dynamic>> ValidaContrato(string nombre)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("NOM", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("IID", OracleDbType.Decimal, ParameterDirection.Output),
                new OracleParameter("IERROR", OracleDbType.Varchar2, ParameterDirection.Output),
                new OracleParameter("IDATOS", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            parameters[0].Direction = ParameterDirection.InputOutput;
            parameters[1].Direction = ParameterDirection.InputOutput;
            parameters[2].Direction = ParameterDirection.InputOutput;
            parameters[3].Direction = ParameterDirection.Output;


            parameters[0].Value = nombre;
            parameters[1].Value = 0;
            parameters[2].Value = "OK";

            var ps = $"{paqueteConsulta}.CONTRATO_NOM_LTY";
            var data = await OracleContext.ExecuteProcedure<dynamic>(ps, parameters.ToArray());
            return data;
        }

        public async Task<List<dynamic>> ValidaCampo(string campo)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("CAMPO", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("IERROR", OracleDbType.Varchar2, ParameterDirection.Output),
                new OracleParameter("IDATOS", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            parameters[0].Direction = ParameterDirection.InputOutput;
            parameters[1].Direction = ParameterDirection.InputOutput;
            parameters[2].Direction = ParameterDirection.Output;

            parameters[0].Value = campo;
            parameters[1].Value = "OK";

            var ps = $"{paqueteConsulta}.CAMPO_NOM_LTY";
            return await OracleContext.ExecuteProcedure<dynamic>(ps, parameters.ToArray());

        }

        public async Task<List<dynamic>> ValidaCampoContratoOperador(string operador, string campo, string contrato)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("I_OPERADOR", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("I_CAMPO", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("I_CONTRATO", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("IERROR", OracleDbType.Varchar2, ParameterDirection.Output),
                new OracleParameter("IDATOS", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            parameters[0].Direction = ParameterDirection.InputOutput;
            parameters[1].Direction = ParameterDirection.InputOutput;
            parameters[2].Direction = ParameterDirection.InputOutput;
            parameters[3].Direction = ParameterDirection.InputOutput;
            parameters[4].Direction = ParameterDirection.Output;

            parameters[0].Value = operador;
            parameters[1].Value = campo;
            parameters[2].Value = contrato;
            parameters[3].Value = "OK";

            var ps = $"{paqueteConsulta}.OPERADOR_CAMPO_CONTRATO_NOM_LT";
            var data = await OracleContext.ExecuteProcedure<dynamic>(ps, parameters.ToArray());
            return data;

        }

        public async Task<List<dynamic>> ValidaCompaniaCampo(string compania, string campo)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("I_COMPANIA", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("I_CAMPO", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("IERROR", OracleDbType.Varchar2, ParameterDirection.Output),
                new OracleParameter("IDATOS", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            parameters[0].Direction = ParameterDirection.InputOutput;
            parameters[1].Direction = ParameterDirection.InputOutput;
            parameters[2].Direction = ParameterDirection.InputOutput;
            parameters[3].Direction = ParameterDirection.Output;

            parameters[0].Value = compania;
            parameters[1].Value = campo;
            parameters[2].Value = "OK";

            var ps = $"{paqueteConsulta}.COMPANIA_CAMPO_NOM_LT";
            var data = await OracleContext.ExecuteProcedure<dynamic>(ps, parameters.ToArray());
            return data;
        }

        public async Task<List<dynamic>> ValidaCompaniaCampoEstado(string compania, string campo,string fecha)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("I_COMPANIA", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("I_CAMPO", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("I_FECHA", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("IERROR", OracleDbType.Varchar2, ParameterDirection.Output),
                new OracleParameter("IDATOS", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            parameters[0].Direction = ParameterDirection.InputOutput;
            parameters[1].Direction = ParameterDirection.InputOutput;
            parameters[2].Direction = ParameterDirection.InputOutput;
            parameters[3].Direction = ParameterDirection.InputOutput;
            parameters[4].Direction = ParameterDirection.Output;

            parameters[0].Value = compania;
            parameters[1].Value = campo;
            parameters[2].Value = fecha;
            parameters[3].Value = "OK";

            var ps = $"{paqueteConsulta}.COMPANIA_CAMPO_ESTADO";
            var data = await OracleContext.ExecuteProcedure<dynamic>(ps, parameters.ToArray());
            return data;
        }

        public async Task<List<dynamic>> ValidaBloque(string nombre)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("p_Nom_Tip_Contrato", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("CURDATOS", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            parameters[0].Direction = ParameterDirection.InputOutput;
            parameters[1].Direction = ParameterDirection.Output;
            parameters[0].Value = nombre;

            var ps = $"{paqueteConsulta}.TIPO_CONTRATO_NOM_LT";
            var data = await OracleContext.ExecuteProcedure<dynamic>(ps, parameters.ToArray());
            return data;
        }

        public async Task<List<dynamic>> ValidaPozo(string nombre)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("p_Pozo_Uwi", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("OutError", OracleDbType.Varchar2, ParameterDirection.Output),
                new OracleParameter("Cur_Pozo_Uwi", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            parameters[0].Direction = ParameterDirection.InputOutput;
            parameters[1].Direction = ParameterDirection.InputOutput;
            parameters[2].Direction = ParameterDirection.Output;

            parameters[0].Value = nombre;
            parameters[1].Value = "OK";

            var ps = $"{paqueteConsulta}.POZO_UWI_LT";
            var data = await OracleContext.ExecuteProcedure<dynamic>(ps, parameters.ToArray());
            return data;
        }

        public async Task<List<dynamic>> ValidaYacimiento(string nombre)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("p_Nombre_Yac", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("OutError",OracleDbType.Varchar2,ParameterDirection.Output),
                new OracleParameter("CURDATOS", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            parameters[0].Direction = ParameterDirection.InputOutput;
            parameters[1].Direction = ParameterDirection.InputOutput;
            parameters[2].Direction = ParameterDirection.Output;

            parameters[0].Value = nombre;
            parameters[1].Value = "OK";

            var ps = $"{paqueteConsulta}.YACIMIENTO_NOM_LTY";
            var data = await OracleContext.ExecuteProcedure<dynamic>(ps, parameters.ToArray());
            return data;
        }

        public async Task<List<dynamic>> ValidaFormacion(string nombre)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("p_Nombre_Yac", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("OutError",OracleDbType.Varchar2,ParameterDirection.Output),
                new OracleParameter("CURDATOS", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            parameters[0].Direction = ParameterDirection.InputOutput;
            parameters[1].Direction = ParameterDirection.InputOutput;
            parameters[2].Direction = ParameterDirection.Output;

            parameters[0].Value = nombre;
            parameters[1].Value = "OK";

            var ps = $"{paqueteConsulta}.YACIMIENTO_NOM_LTY";
            var data = await OracleContext.ExecuteProcedure<dynamic>(ps, parameters.ToArray());
            return data;
        }

        public async Task<List<dynamic>> ValidaModalidad(string nombre)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("P_MODALIDAD", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("IID", OracleDbType.Decimal, ParameterDirection.Output),
                new OracleParameter("IERROR", OracleDbType.Varchar2, ParameterDirection.Output),
                new OracleParameter("IDATOS", OracleDbType.RefCursor, ParameterDirection.Output),
            };

            parameters[0].Direction = ParameterDirection.InputOutput;
            parameters[1].Direction = ParameterDirection.InputOutput;
            parameters[2].Direction = ParameterDirection.InputOutput;
            parameters[3].Direction = ParameterDirection.Output;

            parameters[0].Value = nombre;
            parameters[1].Value = 0;
            parameters[2].Value = "OK";

            var ps = $"{paqueteConsulta}.MODALIDAD_NOM_LT";
            var data = await OracleContext.ExecuteProcedure<dynamic>(ps, parameters.ToArray());
            return data;
        }

        public async Task<List<dynamic>> ValidaPozoYacimiento(string nombre)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("i_POZO_YACI_NM", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("CURDATOS", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            parameters[0].Direction = ParameterDirection.InputOutput;
            parameters[1].Direction = ParameterDirection.Output;

            parameters[0].Value = nombre;
            parameters[1].Value = "OK";

            var ps = $"{paqueteConsulta}.POZO_YACI_NOM_LT";
            var data = await OracleContext.ExecuteProcedure<dynamic>(ps, parameters.ToArray());
            return data;
        }

        public async Task<List<dynamic>> ValidaFacilidad(string nombre)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("p_Facilidad", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("CURDATOS", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            parameters[0].Direction = ParameterDirection.InputOutput;
            parameters[1].Direction = ParameterDirection.Output;

            parameters[0].Value = nombre;
            parameters[1].Value = "OK";

            var ps = $"{paqueteConsulta}.FACILIDAD_NOM_LT";
            var data = await OracleContext.ExecuteProcedure<dynamic>(ps, parameters.ToArray());
            return data;
        }

        public async Task<List<dynamic>> ValidaCodigoDane(string nombre)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("p_Codigo_Municipio", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("Iid", OracleDbType.Decimal, ParameterDirection.Output),
                new OracleParameter("OutError", OracleDbType.Varchar2, ParameterDirection.Output),
                new OracleParameter("Cur_Municipio", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            parameters[0].Direction = ParameterDirection.InputOutput;
            parameters[1].Direction = ParameterDirection.InputOutput;
            parameters[2].Direction = ParameterDirection.InputOutput;
            parameters[3].Direction = ParameterDirection.Output;

            parameters[0].Value = nombre;
            parameters[1].Value = 0;
            parameters[2].Value = "Ok";


            var ps = $"{paqueteConsulta}.MUNICIPIO_COD_LT";
            var data = await OracleContext.ExecuteProcedure<dynamic>(ps, parameters.ToArray());
            return data;
        }

        public async Task<List<dynamic>> ValidaProduccion(string nombre)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("p_Metodo_Prod", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("CURDATOS", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            parameters[0].Direction = ParameterDirection.InputOutput;
            parameters[1].Direction = ParameterDirection.Output;

            parameters[0].Value = nombre;
            parameters[1].Value = "OK";

            var ps = $"{paqueteConsulta}.METODO_PRODUCCION_NOM_LT";
            var data = await OracleContext.ExecuteProcedure<dynamic>(ps, parameters.ToArray());
            return data;
        }

        public async Task<List<dynamic>> ValidarCargueF9Existencia(int year, int mont, int campo)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("I_AÑO", OracleDbType.Int32, year),
                new OracleParameter("I_MES", OracleDbType.Int32, mont),
                new OracleParameter("I_CAMPO_ID", OracleDbType.Varchar2, campo),
                new OracleParameter("I_FORMA_ID", OracleDbType.Int32, 1),
                new OracleParameter("IID", OracleDbType.RefCursor),
                new OracleParameter("IERROR", OracleDbType.RefCursor)
            };

            var data = await OracleContext.ExecuteProcedure<dynamic>("F9_VALIDA_CARGUE", parameters.ToArray());

            return data;
        }

        public async Task<dynamic> Oficialializar(string json) {

            var parameters = new List<OracleParameter>
            {
                new OracleParameter("un_StrXml", OracleDbType.Clob, ParameterDirection.Input),
                new OracleParameter("un_resultado", OracleDbType.Clob,ParameterDirection.Output)
            };
             
            parameters[0].Direction = ParameterDirection.InputOutput;
            parameters[1].Direction = ParameterDirection.Output;

            parameters[0].Value = json;
            parameters[1].Value = 0;
            var ps = $"{paqueteFormas}.OFICIALIZAR_FORMAS";
            var data = await OracleContext.ExecuteProcedureClob(ps, "un_resultado", parameters.ToArray());
            return data;
        }

        public async Task<dynamic> ConsultarOficilizacion(string fechaPeriodo, string idForma)
        {

            var parameters = new List<OracleParameter>
            {
                new OracleParameter("I_FECHA_PERIODO", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("I_ID_FORMA", OracleDbType.Varchar2,ParameterDirection.Input),
                new OracleParameter("OutError", OracleDbType.Varchar2,ParameterDirection.Output),
                new OracleParameter("IDATOS", OracleDbType.RefCursor,ParameterDirection.Output)
            };

            parameters[0].Direction = ParameterDirection.InputOutput;
            parameters[1].Direction = ParameterDirection.InputOutput;
            parameters[2].Direction = ParameterDirection.InputOutput;
            parameters[3].Direction = ParameterDirection.InputOutput;

            parameters[0].Value = fechaPeriodo;
            parameters[1].Value = idForma;
            parameters[2].Value = "NA";

            var ps = $"{paqueteConsulta}.OFICIALIZA_FORMA_LT_CONTRATO";
            var data = await OracleContext.ExecuteProcedure<dynamic>(ps,parameters.ToArray());
            return data;
        }

        public async Task<List<dynamic>> ValidaForma9Producion(List<dynamic> listData)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("I_MES", OracleDbType.Decimal, ParameterDirection.Input),
                new OracleParameter("I_PDEN_ID_POZOYA", OracleDbType.Decimal, ParameterDirection.Input),
                new OracleParameter("I_PETROLEOMENSUAL", OracleDbType.Decimal, ParameterDirection.Input),
                new OracleParameter("I_PETROLEOACUM", OracleDbType.Decimal, ParameterDirection.Input),
                new OracleParameter("I_AGUAMENSUAL", OracleDbType.Decimal, ParameterDirection.Input),
                new OracleParameter("I_AGUAACUM", OracleDbType.Decimal, ParameterDirection.Input),
                new OracleParameter("I_GASMENSUAL", OracleDbType.Decimal, ParameterDirection.Input),
                new OracleParameter("I_GASACUM", OracleDbType.Decimal, ParameterDirection.Input),

                new OracleParameter("IID", OracleDbType.Decimal, ParameterDirection.Output),
                new OracleParameter("O_PETROLEOACUM", OracleDbType.Decimal, ParameterDirection.Output),
                new OracleParameter("O_AGUAACUM", OracleDbType.Decimal, ParameterDirection.Output),
                new OracleParameter("O_GASACUM", OracleDbType.Decimal, ParameterDirection.Output),
                new OracleParameter("IERROR", OracleDbType.Varchar2, ParameterDirection.Output)
            };

            parameters[0].Direction = ParameterDirection.InputOutput;
            parameters[1].Direction = ParameterDirection.InputOutput;
            parameters[2].Direction = ParameterDirection.InputOutput;
            parameters[3].Direction = ParameterDirection.InputOutput;
            parameters[4].Direction = ParameterDirection.InputOutput;
            parameters[5].Direction = ParameterDirection.InputOutput;
            parameters[6].Direction = ParameterDirection.InputOutput;
            parameters[7].Direction = ParameterDirection.InputOutput;
            parameters[8].Direction = ParameterDirection.InputOutput;
            parameters[9].Direction = ParameterDirection.InputOutput;
            parameters[10].Direction = ParameterDirection.InputOutput;
            parameters[11].Direction = ParameterDirection.InputOutput;
            parameters[12].Direction = ParameterDirection.InputOutput;

            //asignacion de datos 
            parameters[0].Value = listData[0];
            parameters[1].Value = listData[1];
            parameters[2].Value = listData[2];
            parameters[3].Value = listData[3];
            parameters[4].Value = listData[4];
            parameters[5].Value = listData[5];
            parameters[6].Value = listData[6];
            parameters[7].Value = listData[7];

            parameters[8].Value = 0;
            parameters[9].Value = 0;
            parameters[10].Value = 0;
            parameters[11].Value = 0;
            parameters[12].Value = "OK";

            var ps = $"{paqueteFormas}.F9_VALIDA_PRODUCCION";
            var data = await OracleContext.ExecuteProcedure<dynamic>(ps, parameters.ToArray());
            return data;
        }


        public async Task<List<dynamic>> ValidarEstadoPozo(string nombre)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("NOM", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("IID", OracleDbType.Decimal, ParameterDirection.Output),
                new OracleParameter("IERROR", OracleDbType.Varchar2, ParameterDirection.Output),
                new OracleParameter("IDATOS", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            parameters[0].Direction = ParameterDirection.InputOutput;
            parameters[1].Direction = ParameterDirection.InputOutput;
            parameters[2].Direction = ParameterDirection.InputOutput;
            parameters[3].Direction = ParameterDirection.Output;


            parameters[0].Value = nombre;
            parameters[1].Value = 0;
            parameters[2].Value = "OK";

            var ps = $"{paqueteConsulta}.ESTADO_POZO_NOM_LTY";
            var data = await OracleContext.ExecuteProcedure<dynamic>(ps, parameters.ToArray());
            return data;
        }

        public async Task<List<dynamic>> ValidarMethodoProducion(string nombre)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("NOM", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("IID", OracleDbType.Decimal, ParameterDirection.Output),
                new OracleParameter("IERROR", OracleDbType.Varchar2, ParameterDirection.Output),
                new OracleParameter("IDATOS", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            parameters[0].Direction = ParameterDirection.InputOutput;
            parameters[1].Direction = ParameterDirection.InputOutput;
            parameters[2].Direction = ParameterDirection.InputOutput;
            parameters[3].Direction = ParameterDirection.Output;


            parameters[0].Value = nombre;
            parameters[1].Value = 0;
            parameters[2].Value = "OK";

            var ps = $"{paqueteConsulta}.METODO_PRODUCCION_NOM_LTY";
            var data = await OracleContext.ExecuteProcedure<dynamic>(ps, parameters.ToArray());
            return data;
        }
    }
}