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
    public class Forma15CRRepository : IForma15CRRepository
    {
        private readonly FormasContext _formas;

        public Forma15CRRepository(FormasContext formas)
        {
            _formas = formas;
        }

        public async Task<DataTable> guardarCabecera(string form_id, infoForma15CR cab, string usuario)
        {
            try
            {
                SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                SqlCommand command = new SqlCommand("FOXT.SP_Guardar_Cab_Forma15")
                {
                    Connection = conection
                };
                conection.Open();
                command.Parameters.AddWithValue("@form_id", Guid.Parse(form_id));
                command.Parameters.AddWithValue("@compania_id", cab.compania_id);
                command.Parameters.AddWithValue("@compania", cab.compania);
                command.Parameters.AddWithValue("@concesion", cab.concesion);
                command.Parameters.AddWithValue("@contrato_id", cab.contrato_id);
                command.Parameters.AddWithValue("@contrato", cab.contrato);
                command.Parameters.AddWithValue("@campo_id", cab.campo_id);
                command.Parameters.AddWithValue("@campo", cab.campo);
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

        public async Task<DataTable> guardarDetalle(string form_id, string id_detalle, dataForma15CR det, string usuario)
        {
            try
            {
                SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                SqlCommand command = new SqlCommand("FOXT.SP_Guardar_Det_Forma15")
                {
                    Connection = conection
                };
                conection.Open();
                command.Parameters.AddWithValue("@id_cabecera", Guid.Parse(id_detalle));
                command.Parameters.AddWithValue("@form_id", Guid.Parse(form_id));
                command.Parameters.AddWithValue("@valInyecPozo", det.valInyecPozo);
                command.Parameters.AddWithValue("@valInyecFormacionProductora", det.valInyecFormacionProductora);
                command.Parameters.AddWithValue("@valInyecMetodoProduccion", det.valInyecMetodoProduccion);
                command.Parameters.AddWithValue("@valInyecPresionInyeccion", det.valInyecPresionInyeccion);
                command.Parameters.AddWithValue("@valInyecCiclo", det.valInyecCiclo);
                command.Parameters.AddWithValue("@valInyecDiasMes", det.valInyecDiasMes);
                command.Parameters.AddWithValue("@valInyecDiasAcumulados", det.valInyecDiasAcumulados);
                command.Parameters.AddWithValue("@valInyecLibrasMes", det.valInyecLibrasMes);
                command.Parameters.AddWithValue("@valInyecLibrasAcumulados", det.valInyecLibrasAcumulados);
                command.Parameters.AddWithValue("@valInyecBTUMes", det.valInyecBTUMes);
                command.Parameters.AddWithValue("@valInyecBTUAcumulados", det.valInyecBTUAcumulados);
                command.Parameters.AddWithValue("@valInyecCalidadVapor", det.valInyecCalidadVapor);
                command.Parameters.AddWithValue("@produccionPetroleoBlsNetosMensual", det.produccionPetroleoBlsNetosMensual);
                command.Parameters.AddWithValue("@produccionPetroleoBlsNetosAcumulado", det.produccionPetroleoBlsNetosAcumulado);
                command.Parameters.AddWithValue("@produccionAguaBlsMensual", det.produccionAguaBlsMensual);
                command.Parameters.AddWithValue("@produccionAguaBlsAcumulado", det.produccionAguaBlsAcumulado);
                command.Parameters.AddWithValue("@usuario", usuario);
                command.Parameters.AddWithValue("@pden_id", det.PDEN_ID);

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
