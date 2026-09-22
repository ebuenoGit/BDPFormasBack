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
    public class Forma17Repository : IForma17Repository
    {
        private readonly FormasContext _formas;

        public Forma17Repository(FormasContext formas)
        {
            _formas = formas;
        }

        public async Task<DataTable> guardarCabecera(string form_id, InfoForma17CR cab, string usuario)
        {
            try
            {
                SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                SqlCommand command = new SqlCommand("FOXT.SP_Guardar_Cab_Forma17")
                {
                    Connection = conection
                };
                conection.Open();
                command.Parameters.AddWithValue("@form_id", Guid.Parse(form_id));
                command.Parameters.AddWithValue("@concesion", cab.concesion);
                command.Parameters.AddWithValue("@operador_id", cab.operador_id);
                command.Parameters.AddWithValue("@operador", cab.operador);
                command.Parameters.AddWithValue("@contrato_id", cab.contrato_id);
                command.Parameters.AddWithValue("@contrato", cab.contrato);
                command.Parameters.AddWithValue("@campo_id", cab.campo_id);
                command.Parameters.AddWithValue("@campo", cab.campo);
                command.Parameters.AddWithValue("@estructura", cab.estructura);
                command.Parameters.AddWithValue("@formacion", cab.formacion);
                command.Parameters.AddWithValue("@bloque", cab.bloque);
                command.Parameters.AddWithValue("@yacimiento", cab.yacimiento);
                command.Parameters.AddWithValue("@mes", cab.mes);
                command.Parameters.AddWithValue("@anio", cab.anio);
                command.Parameters.AddWithValue("@usuario", usuario);

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

        public async Task<DataTable> guardarDetalle(string form_id, string id_detalle, dataForma17CR det, string usuario)
        {
            try
            {
                SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                SqlCommand command = new SqlCommand("FOXT.SP_Guardar_Det_Forma17")
                {
                    Connection = conection
                };
                conection.Open();
                command.Parameters.AddWithValue("@id_cabecera", Guid.Parse(id_detalle));
                command.Parameters.AddWithValue("@form_id", Guid.Parse(form_id));
                command.Parameters.AddWithValue("@pozo", det.pozo);
                command.Parameters.AddWithValue("@diasEnElMes", det.diasEnElMes);
                command.Parameters.AddWithValue("@diasAcumulados", det.diasAcumulados);
                command.Parameters.AddWithValue("@produccionGasMCPDiaria", det.produccionGasMCPDiaria);
                command.Parameters.AddWithValue("@produccionGasMCPMensual", det.produccionGasMCPMensual);
                command.Parameters.AddWithValue("@produccionGasMCPAcumulada", det.produccionGasMCPAcumulada);
                command.Parameters.AddWithValue("@produccionAguaMensual", det.produccionAguaMensual);
                command.Parameters.AddWithValue("@produccionAguaAcumulada", det.produccionAguaAcumulada);
                command.Parameters.AddWithValue("@estadoPozosFinalMes", det.estadoPozosFinalMes);
                command.Parameters.AddWithValue("@usuario", usuario);
                command.Parameters.AddWithValue("@pden_id", det.pden_id);

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
