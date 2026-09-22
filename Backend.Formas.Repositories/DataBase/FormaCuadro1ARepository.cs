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
    public class FormaCuadro1ARepository : IFormaCuadro1ARepository
    {
        private readonly FormasContext _formas;

        public FormaCuadro1ARepository(FormasContext formas)
        {
            _formas = formas;
        }

        public async Task<DataTable> guardarCabecera(cabeCuadro1 cab)
        {
            try
            {
                SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                SqlCommand command = new SqlCommand("FOXT.SP_Guardar_Cab_Cuadro1")
                {
                    Connection = conection
                };
                conection.Open();
                command.Parameters.AddWithValue("@form_id", cab.form_id);
                command.Parameters.AddWithValue("@mes", cab.mes);
                command.Parameters.AddWithValue("@anio", cab.anio);
                command.Parameters.AddWithValue("@compania_id", cab.compania_id);
                command.Parameters.AddWithValue("@compania", cab.compania);
                command.Parameters.AddWithValue("@contrato_id", cab.contrato_id);
                command.Parameters.AddWithValue("@contrato", cab.contrato);
                command.Parameters.AddWithValue("@campo_id", cab.campo_id);
                command.Parameters.AddWithValue("@campo", cab.campo);
                command.Parameters.AddWithValue("@lugar", cab.lugar);
                command.Parameters.AddWithValue("@tanque", cab.tanque);
                command.Parameters.AddWithValue("@bateria", cab.bateria);
                command.Parameters.AddWithValue("@usuario", cab.usuario);
                command.Parameters.AddWithValue("@pden_id_tanque", cab.PDEN_ID_TANQUE);
                command.Parameters.AddWithValue("@pden_id_bateria", cab.PDEN_ID_TANQUE);

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

        public async Task<DataTable> guardarDetalle(detaCuadro1 det)
        {
            try
            {
                SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                SqlCommand command = new SqlCommand("FOXT.SP_Guardar_Det_Cuadro1")
                {
                    Connection = conection
                };
                conection.Open();
                command.Parameters.AddWithValue("@id_cabecera", det.id_cabecera);
                command.Parameters.AddWithValue("@form_id", det.form_id);
                command.Parameters.AddWithValue("@dias", det.dias);
                command.Parameters.AddWithValue("@medidaMM", det.medidaMM);
                command.Parameters.AddWithValue("@aforoBLS", det.aforoBLS);
                command.Parameters.AddWithValue("@tempF", det.tempF);
                command.Parameters.AddWithValue("@factorTemp", det.factorTemp);
                command.Parameters.AddWithValue("@BLS60F", det.BLS60F);
                command.Parameters.AddWithValue("@BSW", det.BSW);
                command.Parameters.AddWithValue("@factorBSW", det.factorBSW);
                command.Parameters.AddWithValue("@CTSH", det.CTSH);
                command.Parameters.AddWithValue("@tempAmb", det.tempAmb);
                command.Parameters.AddWithValue("@BLSNetos", det.BLSNetos);
                command.Parameters.AddWithValue("@transfBLS", det.transfBLS);
                command.Parameters.AddWithValue("@recibidoBLS", det.recibidoBLS);
                command.Parameters.AddWithValue("@entregaBLS", det.entregaBLS);
                command.Parameters.AddWithValue("@API60F", det.API60F);
                command.Parameters.AddWithValue("@GE", det.GE);
                command.Parameters.AddWithValue("@netosGE", det.netosGE);
                command.Parameters.AddWithValue("@salBTB", det.salBTB);
                command.Parameters.AddWithValue("@usuario", det.usuario);
                command.Parameters.AddWithValue("@pden_tanque", det.PDEN_ID_TANQUE);
                command.Parameters.AddWithValue("@pden_bateria", det.PDEN_ID_TANQUE);

                command.Parameters.AddWithValue("@trasRecibido", det.ECP_BALANCE_RECEIVED);
                command.Parameters.AddWithValue("@TrasEnvio", det.ECP_BALANCE_SENT);
                command.Parameters.AddWithValue("@movIntraRecibido", det.ECP_INTRADIARY_MOV_RECEIVED);
                command.Parameters.AddWithValue("@movIntraEnvio", det.ECP_INTRADIARY_MOV_SENT);

                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                DataTable DT = new DataTable();
                sqlDataAdapter.SelectCommand = command;
                sqlDataAdapter.Fill(DT);
                List<dynamic> LstResultadoTabla = ConvertTypes.ToDynamic(DT);
                conection.Close();
                return await Task.Run(() => DT);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<DataTable> guardarTotal(string id_cabecera, string forma_id, totalFormaCuadro1A tot, string GetUserId)
        {
            try
            {
                SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                SqlCommand command = new SqlCommand("FOXT.SP_Guardar_Tot_Cuadro1")
                {
                    Connection = conection
                };
                conection.Open();
                command.Parameters.AddWithValue("@id_cabecera", Guid.Parse(id_cabecera));
                command.Parameters.AddWithValue("@form_id", Guid.Parse(forma_id));
                command.Parameters.AddWithValue("@BLS60F", tot.BLS60F);
                command.Parameters.AddWithValue("@BLSNetos", tot.BLSNetos);
                command.Parameters.AddWithValue("@recibidoBLS", tot.recibidoBLS);
                command.Parameters.AddWithValue("@entregaBLS", tot.entregaBLS);
                command.Parameters.AddWithValue("@usuario", GetUserId);

                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                DataTable DT = new DataTable();
                sqlDataAdapter.SelectCommand = command;
                sqlDataAdapter.Fill(DT);
                List<dynamic> LstResultadoTabla = ConvertTypes.ToDynamic(DT);
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
