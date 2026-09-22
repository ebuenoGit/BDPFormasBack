using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
//using Form = Backend.Formas.Entities.DAO.Form;
using System.Data;
using Microsoft.Data.SqlClient;
using Backend.Formas.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text;
using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.Responses;

namespace Backend.Formas.Repositories.DataBase
{
    public class FormaC30MasivaRepository : IFormaC30MasivaRepository
    {
        private readonly Context.EcoOracleContext OracleContext;

        public FormaC30MasivaRepository(Context.EcoOracleContext oracleContext)
        {
            OracleContext = oracleContext;
        }

        private readonly FormasContext _formas;

        public FormaC30MasivaRepository(FormasContext formas)
        {
            _formas = formas;
        }

        private readonly IBaseRepository<BodyContenteDTOC30Masiva> RepositoryFormC30;

        public FormaC30MasivaRepository(
              IBaseRepository<BodyContenteDTOC30Masiva> repositoryFormC30,
              Context.EcoOracleContext oracleContext,
              FormasContext formas
          )
        {
            RepositoryFormC30 = repositoryFormC30;
            OracleContext = oracleContext;
            _formas = formas;
        }


        public async Task<BodyContenteDTOC30Masiva> CreateFormC30Masiva(BodyContenteDTOC30Masiva _formC30Masiva)
        {
            await RepositoryFormC30.AddAsync(_formC30Masiva);
            return _formC30Masiva;
        }
     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Sql_Json"></param>
        /// <returns></returns>
        public string Guardasqlforma30(string Sql_Json)
        { string Respuesta = "Sql_Respuesta:1;";
            SqlConnection sqlCon;
            sqlCon = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
            try
            {
                sqlCon.Open();
                SqlCommand sql_cmnd = new SqlCommand("FOXT.Guarda_FormaC30", sqlCon);
                sql_cmnd.CommandType = CommandType.StoredProcedure;
                sql_cmnd.Parameters.AddWithValue("@pJson", SqlDbType.NVarChar).Value = Sql_Json;
                sql_cmnd.ExecuteNonQuery();
                sqlCon.Close();
            }
            catch (Exception ex)
            {
                //   result = e.Message;
                Respuesta = "Sql_Respuesta:0;" + ex+";";
                throw new Exception(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
            return Respuesta;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="Sql_Json"></param>
        /// <returns></returns>
        public string GuardaBdpforma30(string Sql_Json)
        {
            string Respuesta = "BDP_Respuesta:1;";
            string resultadoTMP;
            try
            {
                using OracleConnection con = new OracleConnection(OracleContext.ConnectionString);
                using OracleCommand cmd = new OracleCommand("ADMONFUNC.PKG_MBDP_FORMAS_ASOC.p_selector_formas");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                con.Open();
                cmd.Parameters.Add("un_StrJson", OracleDbType.Clob).Value = Sql_Json;
                cmd.Parameters.Add("un_resultado", OracleDbType.Clob).Direction = ParameterDirection.Output;
                cmd.ExecuteReader();
                var tempClob = (OracleClob)cmd.Parameters["un_resultado"].Value;
                StreamReader streamreader = new StreamReader(tempClob, Encoding.Unicode);
                resultadoTMP = streamreader.ReadToEnd();
                con.Close();

                bool lError = resultadoTMP.Contains("Error");
                if (lError == false)
                {
                    Respuesta = "BDP_Respuesta:0;";
                }
                else
                {
                    Respuesta = "BDP_Respuesta:1;" + resultadoTMP + ".;";
                }
            }
            catch (Exception ex)
            {
                Respuesta = "BDP_Respuesta:2;"+ex+".;";

            }
            return Respuesta;
        }

    

        string IFormaC30MasivaRepository.StringBetween(string Source, string Start, string End)
        {
            
            string result = "";
            if (Source.Contains(Start) && Source.Contains(End))
            {
                int StartIndex = Source.IndexOf(Start, 0) + Start.Length;
                int EndIndex = Source.IndexOf(End, StartIndex) - StartIndex;
                try
                {
                    result = Source.Substring(StartIndex, EndIndex);
                }
                catch (Exception e)
                {
                    result = "";
                    Console.WriteLine("{0} Exception Resultado Guardado.", e);
                }

                return result;
            }
            return result;
        }
        public string ConsultarformaC30(int pAnio, int PMes, int PTodo)
        {
            SqlConnection sqlCon;
            sqlCon = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
            String json = string.Empty;
            try
            {
                sqlCon.Open();
                SqlCommand sql_cmnd = new SqlCommand("FOXT.Observaciones_FormaC30", sqlCon);
                sql_cmnd.CommandType = CommandType.StoredProcedure;
                sql_cmnd.Parameters.AddWithValue("@pAnio", SqlDbType.Int).Value = pAnio;
                sql_cmnd.Parameters.AddWithValue("@pMes", SqlDbType.Int).Value = PMes;
                sql_cmnd.Parameters.AddWithValue("@pTodo", SqlDbType.Int).Value = PTodo;
                sql_cmnd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                sql_cmnd.ExecuteNonQuery();
                json = sql_cmnd.Parameters["@jsonOutput"].Value.ToString();
                sqlCon.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
            return json;
        }


        public async Task<ResponseBase<string>> SqlValidarFormas30(string cformas)
        {
            var result = new ResponseBase<string>();
            string resultadoTMP = string.Empty;
            string cValor;
            string jsonText = cformas.Replace("{\"FORMAS\":", "").Replace("}}", "}").Replace("[{\"FORMA_CODIGO\"", "{\"FORMA_CODIGO\"").Replace("}]}]", "}]}");
            cValor = jsonText; // pformas.strXml;
            using OracleConnection con = new OracleConnection(OracleContext.ConnectionString);
            try
            {
                using (OracleCommand cmd = new OracleCommand("ADMONFUNC.PKG_MBDP_XFORMAS.p_selector_formas"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    if (con.State == ConnectionState.Closed)
                        con.Open(); 

                    cmd.Parameters.Add("un_StrXml", OracleDbType.Clob).Value = cValor;
                    // pformas.strXml;
                    cmd.Parameters.Add("un_Formato", OracleDbType.Varchar2).Value = "JSON";
                    cmd.Parameters.Add("un_resultado", OracleDbType.Clob).Direction = ParameterDirection.Output;
                    cmd.CommandTimeout = (int)TimeSpan.FromMinutes(30).TotalSeconds;
                    await cmd.ExecuteReaderAsync();

                    var tempClob = (OracleClob)cmd.Parameters["un_resultado"].Value;
                    resultadoTMP = tempClob.Value.ToString();
                    //StreamReader streamreader = new StreamReader(tempClob, Encoding.Unicode);
                    //resultadoTMP = streamreader.ReadToEnd();


                }
                result.Count = 2;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con.Dispose();
            }
            result.Data = resultadoTMP;
            return result; //Task.Run(() => result);
        }

    }
}