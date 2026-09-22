using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.Interface.Repository;
using Backend.Formas.Entities.Responses;
using Backend.Formas.Repositories.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
//using Form = Backend.Formas.Entities.DAO.Form;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Formas.Repositories.DataBase
{
    public class FormaC4MasivaRepository : IFormaC4MasivaRepository
    {
        private readonly Context.EcoOracleContext OracleContext;

        public FormaC4MasivaRepository(Context.EcoOracleContext oracleContext)
        {
            OracleContext = oracleContext;
        }

        private readonly FormasContext _formas;

        public FormaC4MasivaRepository(FormasContext formas)
        {
            _formas = formas;
        }

        private readonly IBaseRepository<BodyContenteDTOC4Masiva> RepositoryFormC4;

        public FormaC4MasivaRepository(
              IBaseRepository<BodyContenteDTOC4Masiva> repositoryFormC4,
              Context.EcoOracleContext oracleContext,
              FormasContext formas
          )
        {
            RepositoryFormC4 = repositoryFormC4;
            OracleContext = oracleContext;
            _formas = formas;
        }


        public async Task<BodyContenteDTOC4Masiva> CreateFormC4Masiva(BodyContenteDTOC4Masiva _formC4Masiva)
        {
            await RepositoryFormC4.AddAsync(_formC4Masiva);
            return _formC4Masiva;
        }
        /*
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


        public async Task<Form9Masiva> GetForm(Guid? id)
        {
            return await RepositoryForm9.GetAsync(predicate: f => f.Form9id == id);
        }

        public async Task<List<Form9Masivadetail>> GetFormDetail(Guid? id)
        {
            var data = await RepositoryForm9Detail.GetAllAsync(predicate: f => f.Formid == id);
            return data.ToList();
        }


        public async Task<Concreteform> GetFormConcreted(Guid? id)
        {
            var data = await RepositoryConcreteForm.GetAsync(predicate: x => x.Formid == id.Value);
            return data;
        }

        public async Task Delete(Guid id)
        {
            await RepositoryConcreteForm.DeleteAsync(predicate: x => x.Formid == id);
            await RepositoryForm9Detail.DeleteAsync(predicate: x => x.Formid == id);
            await RepositoryForm9totalvolumedetail.DeleteAsync(predicate: x => x.Formid == id);
            await RepositoryForm9.DeleteAsync(predicate: x => x.Form9id == id);
            //await RepositoryAcumulado.DeleteAsync(predicate: x => x.Id_Forma == id);
        }

*/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Sql_Json"></param>
        /// <returns></returns>
        public string Guardasqlforma4(string Sql_Json)
        { string Respuesta = "Sql_Respuesta:1;";
            SqlConnection sqlCon;
            sqlCon = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
            try
            {
                sqlCon.Open();
                SqlCommand sql_cmnd = new SqlCommand("FOXT.Guarda_FormaC4", sqlCon);
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

        public string Guardasqlforma4Muni(string Sql_Json)
        {
            string Respuesta = "Sql_Respuesta:1;";
            SqlConnection sqlCon;
            sqlCon = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
            try
            {
                sqlCon.Open();
                SqlCommand sql_cmnd = new SqlCommand("FOXT.Guarda_FormaC4_Muni", sqlCon);
                sql_cmnd.CommandType = CommandType.StoredProcedure;
                sql_cmnd.Parameters.AddWithValue("@pJson", SqlDbType.NVarChar).Value = Sql_Json;
                sql_cmnd.ExecuteNonQuery();
                sqlCon.Close();
            }
            catch (Exception ex)
            {
                //   result = e.Message;
                Respuesta = "Sql_Respuesta:0;" + ex + ";";
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
        public string GuardaBdpforma4(string Sql_Json)
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

    

        /*
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

        public string rep_consultaValidaAcumlaformaC49(int pAnio, int pMes, string GetUserId)
        {
            string Respuesta = "";
            string resultadoTMP = string.Empty;
            DateTime date = new DateTime(pAnio, pMes, 1).AddMonths(1).AddDays(-1);
            try
            {
                using OracleConnection con = new OracleConnection(OracleContext.ConnectionString);
                using (OracleCommand cmd = new OracleCommand("ADMONFUNC.PKG_MBDP_FORMAS_ASOC.P_COMPARATIVOACUMULADO"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();

                    cmd.Parameters.Add("Pr_x_Period ", OracleDbType.Date).Value = date;
                    cmd.Parameters.Add("Pr_x_UserId ", OracleDbType.Varchar2).Value = GetUserId;
                    cmd.Parameters.Add("Pr_c_resultado", OracleDbType.Clob).Direction = ParameterDirection.Output;
                    cmd.ExecuteReader();
                    var tempClob = (OracleClob)cmd.Parameters["un_resultado"].Value;
                    StreamReader streamreader = new StreamReader(tempClob, Encoding.Unicode);
                    resultadoTMP = streamreader.ReadToEnd();
                    con.Close();


                    bool lError = resultadoTMP.Contains("Error");
                    if (lError == false)
                    {
                        Respuesta = "";
                    }
                    else
                    {
                        Respuesta =  resultadoTMP ;
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Respuesta = "Problamas conectandose a BDP " + ex.Message;
            }
            return Respuesta;
        }
        */
        string IFormaC4MasivaRepository.StringBetween(string Source, string Start, string End)
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
        public string ConsultarformaC4(int pAnio, int PMes, int PTodo)
        {
            SqlConnection sqlCon;
            sqlCon = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
            String json = string.Empty;
            try
            {
                sqlCon.Open();
                SqlCommand sql_cmnd = new SqlCommand("FOXT.Observaciones_FormaC4", sqlCon);
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

        public string ConsultarformaC4Muni(int pAnio, int PMes, int PTodo)
        {
            SqlConnection sqlCon;
            sqlCon = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
            String json = string.Empty;
            try
            {
                sqlCon.Open();
                SqlCommand sql_cmnd = new SqlCommand("FOXT.Observaciones_FormaC4Muni", sqlCon);
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