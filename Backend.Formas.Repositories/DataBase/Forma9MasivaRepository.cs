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
using Rhyous.EasyXml;
using System.IO;
using System.Text;

using Newtonsoft.Json.Linq;
using Backend.Formas.Entities.Responses;

namespace Backend.Formas.Repositories.DataBase
{
    public class Forma9MasivaRepository : IForma9MasivaRepository
    {
        private readonly Context.EcoOracleContext OracleContext;

        public Forma9MasivaRepository(Context.EcoOracleContext oracleContext)
        {
            OracleContext = oracleContext;
        }

        private readonly FormasContext _formas;

        public Forma9MasivaRepository(FormasContext formas)
        {
            _formas = formas;
        }
        private readonly IBaseRepository<Concreteform> RepositoryConcreteForm;
        private readonly IBaseRepository<Form> RepositoryForm;
        private readonly IBaseRepository<Form9Masiva> RepositoryForm9;
        private readonly IBaseRepository<Form9Masivadetail> RepositoryForm9Detail;
        private readonly IBaseRepository<Form9Masivatotalvolumedetail> RepositoryForm9totalvolumedetail;
        private readonly IBaseRepository<Acumulados> RepositoryAcumulado;

        public Forma9MasivaRepository(IBaseRepository<Form> repositoryForm,
              IBaseRepository<Form9Masiva> repositoryForm9,
              IBaseRepository<Form9Masivadetail> repositoryForm9Detail,
              IBaseRepository<Form9Masivatotalvolumedetail> repositoryForm9totalvolumedetail,
              IBaseRepository<Concreteform> respositoryConcretForm,
              IBaseRepository<Acumulados> respositoryAcumulado,
              Context.EcoOracleContext oracleContext,
              FormasContext formas
          )
        {
            RepositoryForm = repositoryForm;
            RepositoryForm9 = repositoryForm9;
            RepositoryForm9Detail = repositoryForm9Detail;
            RepositoryForm9totalvolumedetail = repositoryForm9totalvolumedetail;
            RepositoryConcreteForm = respositoryConcretForm;
            RepositoryAcumulado = respositoryAcumulado;
            OracleContext = oracleContext;
            _formas = formas;
        }

        public async Task<bool> CreateForm(Form _form)
        {
            await RepositoryForm.AddAsync(_form);
            return true;
        }

        public async Task<Form9Masiva> CreateForm9(Form9Masiva _form9Masiva)
        {
            await RepositoryForm9.AddAsync(_form9Masiva);
            return _form9Masiva;
        }

        public async Task<Concreteform> CreateConcreteform(Concreteform _concrete)
        {
            await RepositoryConcreteForm.AddAsync(_concrete);
            return _concrete;
        }

        public async Task<bool> CreateForm9Detail(List<Form9Masivadetail> _form9Detail)
        {
            await RepositoryForm9Detail.AddAsync(_form9Detail);
            return true;
        }

        public async Task<bool> CreateForm9TotalVolumenDetail(Form9Masivatotalvolumedetail _form9Total)
        {
            await RepositoryForm9totalvolumedetail.AddAsync(_form9Total);
            return true;
        }

        public async Task<Form9Masiva> GetForm(Guid? id)
        {
            return await RepositoryForm9.GetAsync(predicate: f => f.Form9id == id);
        }

        public async Task<List<Form9Masivadetail>> GetFormDetail(Guid? id)
        {
            var data = await RepositoryForm9Detail.GetAllAsync(predicate: f => f.Formid == id);
            return data.ToList();
        }

        public async Task<List<Form9Masivatotalvolumedetail>> GetFormTotal(Guid? id)
        {
            var data = await RepositoryForm9totalvolumedetail.GetAllAsync(predicate: x => x.Formid == id);
            return data.ToList();
        }

        public async Task<Concreteform> GetFormConcreted(Guid? id)
        {
            var data = await RepositoryConcreteForm.GetAsync(predicate: x => x.Formid == id.Value);
            return data;
        }

        public async Task<bool> CreateAcumulados(List<Acumulados> data)
        {
            try
            {

                await RepositoryAcumulado.AddAsync(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return true;
        }

        public async Task Delete(Guid id)
        {
            await RepositoryConcreteForm.DeleteAsync(predicate: x => x.Formid == id);
            await RepositoryForm9Detail.DeleteAsync(predicate: x => x.Formid == id);
            await RepositoryForm9totalvolumedetail.DeleteAsync(predicate: x => x.Formid == id);
            await RepositoryForm9.DeleteAsync(predicate: x => x.Form9id == id);
            //await RepositoryAcumulado.DeleteAsync(predicate: x => x.Id_Forma == id);
        }


        public string Guardasqlforma9(string Sql_Json)
        {
            string Respuesta = "Sql_Respuesta:0;";
            SqlConnection sqlCon;
            sqlCon = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
            try
            {
                sqlCon.Open();
                SqlCommand sql_cmnd = new SqlCommand("FOXT.Guarda_Forma9", sqlCon);
                sql_cmnd.CommandType = CommandType.StoredProcedure;
                sql_cmnd.CommandTimeout = (int)TimeSpan.FromMinutes(60).TotalSeconds;
                sql_cmnd.Parameters.AddWithValue("@pJson", SqlDbType.NVarChar).Value = Sql_Json;
                sql_cmnd.ExecuteNonQuery();
                sqlCon.Close();
            }
            catch (Exception ex)
            {
                //   result = e.Message;
                Respuesta = "Sql_Respuesta:2;" + ex + ";";
                throw new Exception(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
            return Respuesta;
        }


        public async Task<string> GuardaBdpforma9(string Sql_Json)
        {
            string Respuesta = "BDP_Respuesta:2;";
            string resultadoTMP;
            using OracleConnection con = new OracleConnection(OracleContext.ConnectionString);
            try
            {
                using OracleCommand cmd = new OracleCommand("ADMONFUNC.PKG_MBDP_FORMAS_ASOC.p_selector_formas");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = (int)TimeSpan.FromMinutes(60).TotalSeconds;
                cmd.Connection = con;
                if (con.State == ConnectionState.Closed)
                    con.Open();

                //OracleContext.SetConnectionGlobalization(con);
                cmd.Parameters.Add("un_StrJson", OracleDbType.Clob).Value = Sql_Json;
                cmd.Parameters.Add("un_resultado", OracleDbType.Clob).Direction = ParameterDirection.Output;
                await cmd.ExecuteReaderAsync();
                var tempClob = (OracleClob)cmd.Parameters["un_resultado"].Value;
                resultadoTMP = tempClob.Value.ToString();
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
                Respuesta = "BDP_Respuesta:2;" + ex + ".;";
                //Console.WriteLine(ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con.Dispose();
            }
            return Respuesta;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pAnio"></param>
        /// <param name="PMes"></param>
        /// <param name="PTodo"></param>
        /// <returns></returns>
        public string Observacionforma9(int pAnio, int PMes, int PTodo)
        {
            SqlConnection sqlCon;
            sqlCon = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
            String json = string.Empty;
            try
            {
                sqlCon.Open();
                SqlCommand sql_cmnd = new SqlCommand("FOXT.Observaciones_Forma9", sqlCon);
                sql_cmnd.CommandType = CommandType.StoredProcedure;

                sql_cmnd.Parameters.AddWithValue("@pAnio", SqlDbType.Int).Value = pAnio;
                sql_cmnd.Parameters.AddWithValue("@pMes", SqlDbType.Int).Value = PMes;
                sql_cmnd.Parameters.AddWithValue("@pTodo", SqlDbType.Int).Value = PTodo;
                sql_cmnd.Parameters.Add("@jsonOutput", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                sql_cmnd.CommandTimeout = (int)TimeSpan.FromMinutes(30).TotalSeconds;
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

        public async Task<string> rep_consultaValidaAcumlaforma9(int pAnio, int pMes, string GetUserId)
        {
            string Respuesta = "";
            string resultadoTMP;
            DateTime date = new DateTime(pAnio, pMes, 1).AddMonths(1).AddDays(-1);
            try
            {
                using OracleConnection con = new OracleConnection(OracleContext.ConnectionString);
                using OracleCommand cmd = new OracleCommand("ADMONFUNC.PKG_MBDP_FORMAS_ASOC.P_COMPARATIVOACUMULADO");

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                await con.OpenAsync();
                try
                {
                    //OracleContext.SetConnectionGlobalization(con);
                    cmd.Parameters.Add("Pr_x_Period", OracleDbType.Date).Value = date;
                    cmd.Parameters.Add("Pr_x_UserId", OracleDbType.Varchar2).Value = GetUserId; //"C9123782";// 
                    cmd.Parameters.Add("Pr_c_resultado", OracleDbType.Clob).Direction = ParameterDirection.Output;
                    cmd.CommandTimeout = (int)TimeSpan.FromMinutes(60).TotalSeconds;

                    await cmd.ExecuteReaderAsync();
                    var tempClob = (OracleClob)cmd.Parameters["Pr_c_resultado"].Value;
                    resultadoTMP = tempClob.Value.ToString();
                    //StreamReader streamreader = new StreamReader(tempClob, Encoding.Unicode);
                    //resultadoTMP = streamreader.ReadToEnd();

                    //con.Close();
                    await con.CloseAsync();
                    await con.DisposeAsync();
                    //resultadoTMP = "[{\"GERENCIA\":\"GDT\",\"ID_CAMPO\":\"0051\",\"CAMPO\":\"CASTILLA NORTE\",\"ANIO\":\"2020\",\"MES\":\"03\",\"UWI\":\"CASD0002\",\"POZO\":\"CASTILLA DISPOSAL-02\",\"PDEN_ID\":\"106472\",\"PDEN_NAME\":\"CASTILLA DISPOSAL-02:1:FM_SNFERNAND\",\"PETROLEOMENSUAL\":0,\"PETROLEOACUM\":166.4331,\"PETROLEOACUMANT\":166.4331,\"PETROLEODIFERENCIA\":0,\"GASMENSUAL\":0,\"GASACUM\":0,\"GASACUMANT\":0,\"GASDIFERENCIA\":0,\"AGUAMENSUAL\":0,\"AGUAACUM\":19177.3482,\"AGUAACUMANT\":19177.3482,\"AGUADIFERENCIA\":0},{\"GERENCIA\":\"GDT\",\"ID_CAMPO\":\"0049\",\"CAMPO\":\"CASTILLA\",\"ANIO\":\"2020\",\"MES\":\"03\",\"UWI\":\"CAS360\",\"POZO\":\"CASTILLA-360\",\"PDEN_ID\":\"129799\",\"PDEN_NAME\":\"CASTILLA-360:1:K1\",\"PETROLEOMENSUAL\":1115.67,\"PETROLEOACUM\":260382.44,\"PETROLEOACUMANT\":259266.77,\"PETROLEODIFERENCIA\":0,\"GASMENSUAL\":0,\"GASACUM\":0,\"GASACUMANT\":0,\"GASDIFERENCIA\":0,\"AGUAMENSUAL\":7879.35,\"AGUAACUM\":788475.99,\"AGUAACUMANT\":780596.64,\"AGUADIFERENCIA\":0},{\"GERENCIA\":\"GDT\",\"ID_CAMPO\":\"0049\",\"CAMPO\":\"CASTILLA\",\"ANIO\":null,\"MES\":null,\"UWI\":\"CAST18DIS\",\"POZO\":\"CASTILLA DISPOSAL-18\",\"PDEN_ID\":\"129233\",\"PDEN_NAME\":\"CASTILLA DISPOSAL-18:1:K2\",\"PETROLEOMENSUAL\":null,\"PETROLEOACUM\":null,\"PETROLEOACUMANT\":null,\"PETROLEODIFERENCIA\":0,\"GASMENSUAL\":null,\"GASACUM\":null,\"GASACUMANT\":null,\"GASDIFERENCIA\":0,\"AGUAMENSUAL\":null,\"AGUAACUM\":null,\"AGUAACUMANT\":null,\"AGUADIFERENCIA\":0}]";
                    //Source.Contains(Start)
                    bool lError = resultadoTMP.Contains("Error");
                    if (lError)
                    {
                        Respuesta = "{\"data\":}";
                    }
                    else
                    {
                        Respuesta = "{\"data\":" + resultadoTMP + "}";
                    }
                }
                catch (Exception ex)
                {
                    //Respuesta = ex ;
                    Console.WriteLine(ex.Message);
                    Respuesta = "No se puede conectar a BDP " + ex.Message;
                }
            }
            catch (Exception ex)
            {
                //Respuesta = ex ;
                Console.WriteLine(ex.Message);
                Respuesta = "Problemas conectandose a BDP " + ex.Message;
            }
            return Respuesta;
        }

        string IForma9MasivaRepository.StringBetween(string Source, string Start, string End)

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
                    Console.WriteLine("{0} Exception Resultado en Guardado.", e);
                }

                return result;
            }
            return result;
        }

        public async Task<ResponseBase<string>> SqlValidarFormas(string cformas)
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
                        con.Open(); ;

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