using Backend.Formas.Repositories.Context;

namespace Backend.Formas.Repositories.DataBase
{
    public class FormasAprobarRepositorybck
    {
        private readonly FormasContext _formas;

        public FormasAprobarRepositorybck(FormasContext formas)
        {
            _formas = formas;
        }

        /* public async Task<DataTable> ConsultarEncabezado(decimal id)
         {
             try
             {
                 SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                 SqlCommand command = new SqlCommand("FOXT.SP_ConsultarCabeceraFormas")
                 {
                     Connection = conection
                 };
                 conection.Open();

                 /*SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
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

         public async Task<DataTable> ConsultarDetalle21(decimal id)
         {
             try
             {
                 SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                 SqlCommand command = new SqlCommand("FOXT.SP_ConsultarDetalleForma21")
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
         }*/
    }
}
