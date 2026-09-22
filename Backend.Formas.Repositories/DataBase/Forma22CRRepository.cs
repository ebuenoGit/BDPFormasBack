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
    public class Forma22CRRepository : IForma22CRRepository
    {
        private readonly FormasContext _formas;

        public Forma22CRRepository(FormasContext formas)
        {
            _formas = formas;
        }

        public async Task<DataTable> guardarCabecera(string form_id, infoForm22CR cab, string usuario)
        {
            try
            {
                SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                SqlCommand command = new SqlCommand("FOXT.SP_Guardar_Cab_Forma22")
                {
                    Connection = conection
                };
                conection.Open();
                command.Parameters.AddWithValue("@form_id", Guid.Parse(form_id));
                command.Parameters.AddWithValue("@compania_id", cab.compania_id);
                command.Parameters.AddWithValue("@compania", cab.compania);
                command.Parameters.AddWithValue("@formacion", cab.formacion);
                command.Parameters.AddWithValue("@contrato_id", cab.contrato_id);
                command.Parameters.AddWithValue("@contrato", cab.contrato);
                command.Parameters.AddWithValue("@bloque", cab.bloque);
                command.Parameters.AddWithValue("@campo_id", cab.campo_id);
                command.Parameters.AddWithValue("@campo", cab.campo);
                command.Parameters.AddWithValue("@yacimiento", cab.yacimiento);
                command.Parameters.AddWithValue("@estructura", cab.estructura);
                command.Parameters.AddWithValue("@mes", cab.mes);
                command.Parameters.AddWithValue("@anio", cab.anio);
                command.Parameters.AddWithValue("@pden_id", cab.pden_id);
                command.Parameters.AddWithValue("@usuario", usuario);
                command.Parameters.AddWithValue("@yacimiento_id", cab.yacimiento);


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

        public async Task<DataTable> guardarDetalle(string form_id, string id_detalle, dataForm22CR det, string usuario)
        {
            try
            {
                SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                SqlCommand command = new SqlCommand("FOXT.SP_Guardar_Det_Forma22")
                {
                    Connection = conection
                };
                conection.Open();
                command.Parameters.AddWithValue("@id_cabecera", Guid.Parse(id_detalle));
                command.Parameters.AddWithValue("@form_id", Guid.Parse(form_id));
                command.Parameters.AddWithValue("@Pozo", det.Pozo);
                command.Parameters.AddWithValue("@mes", det.mes);
                command.Parameters.AddWithValue("@petroleoProducidoMensual", det.petroleoProducidoMensual);
                command.Parameters.AddWithValue("@petroleoProducidoAcumulado", det.petroleoProducidoAcumulado);
                command.Parameters.AddWithValue("@aguaInyectadoMensual", det.aguaInyectadoMensual);
                command.Parameters.AddWithValue("@aguaInyectadoAcumulado", det.aguaInyectadoAcumulado);
                command.Parameters.AddWithValue("@aguaProducidoMensual", det.aguaProducidoMensual);
                command.Parameters.AddWithValue("@aguaProducidoAcumulado", det.aguaProducidoAcumulado);
                command.Parameters.AddWithValue("@gasInyectadoMensual", det.gasInyectadoMensual);
                command.Parameters.AddWithValue("@gasInyectadoAcumulado", det.gasInyectadoAcumulado);
                command.Parameters.AddWithValue("@gasProducidoMensual", det.gasProducidoMensual);
                command.Parameters.AddWithValue("@gasProducidoAcumulado", det.gasProducidoAcumulado);
                command.Parameters.AddWithValue("@presionFondo", det.presionFondo);
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
