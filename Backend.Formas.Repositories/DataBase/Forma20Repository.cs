using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.Interface.Repository;
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
    public class Forma20Repository : IForma20Repository
    {
        private readonly IBaseRepository<Forma20Archivo> _FormaArchivo;

        private readonly FormasContext _formas;

        public Forma20Repository(IBaseRepository<Forma20Archivo> FormaArchivo, FormasContext formas)
        {
            _FormaArchivo = FormaArchivo;
            _formas = formas;
        }

        public async Task<DataTable> InsertarCabeceraForma20(Forma20Archivo forArc)
        {
            try
            {
                SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                SqlCommand command = new SqlCommand("FOXT.SP_Guardar_Cabecera_Forma20")
                {
                    Connection = conection
                };
                conection.Open();
                SqlParameter Parameter1 = new SqlParameter("@forma", SqlDbType.VarChar);
                SqlParameter Parameter2 = new SqlParameter("@name", SqlDbType.VarChar);
                SqlParameter Parameter3 = new SqlParameter("@size", SqlDbType.Int);
                SqlParameter Parameter4 = new SqlParameter("@type", SqlDbType.VarChar);
                SqlParameter Parameter5 = new SqlParameter("@uid", SqlDbType.VarChar);
                SqlParameter Parameter6 = new SqlParameter("@User_Created", SqlDbType.VarChar);
                SqlParameter Parameter7 = new SqlParameter("@Date_Created", SqlDbType.DateTime);
                Parameter1.Value = forArc.forma;
                Parameter2.Value = forArc.name;
                Parameter3.Value = forArc.size;
                Parameter4.Value = forArc.type;
                Parameter5.Value = forArc.uid;
                Parameter6.Value = forArc.User_Created;
                Parameter7.Value = forArc.Date_Created;
                SqlParameter[] parameters = { Parameter1, Parameter2, Parameter3, Parameter4, Parameter5, Parameter6, Parameter7 };
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

        public async Task<DataTable> InsertarFileDataForma20(Forma20Datafile fileData)
        {
            try
            {
                SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                SqlCommand command = new SqlCommand("FOXT.SP_Guardar_File_Forma20")
                {
                    Connection = conection
                };
                conection.Open();
                SqlParameter Parameter1 = new SqlParameter("@id", SqlDbType.Int);
                SqlParameter Parameter2 = new SqlParameter("@sheet", SqlDbType.VarChar);
                SqlParameter Parameter3 = new SqlParameter("@compania", SqlDbType.VarChar);
                SqlParameter Parameter4 = new SqlParameter("@operador", SqlDbType.VarChar);
                SqlParameter Parameter5 = new SqlParameter("@campo", SqlDbType.VarChar);
                SqlParameter Parameter6 = new SqlParameter("@estructura", SqlDbType.VarChar);
                SqlParameter Parameter7 = new SqlParameter("@formacion", SqlDbType.VarChar);
                SqlParameter Parameter8 = new SqlParameter("@yacimiento", SqlDbType.VarChar);
                SqlParameter Parameter9 = new SqlParameter("@mes", SqlDbType.VarChar);
                SqlParameter Parameter10 = new SqlParameter("@anio", SqlDbType.VarChar);
                SqlParameter Parameter11 = new SqlParameter("@bloque", SqlDbType.VarChar);
                Parameter1.Value = fileData.id;
                Parameter2.Value = fileData.sheet;
                Parameter3.Value = fileData.compania;
                Parameter4.Value = fileData.operador;
                Parameter5.Value = fileData.campo;
                Parameter6.Value = fileData.estructura;
                Parameter7.Value = fileData.formacion;
                Parameter8.Value = fileData.yacimiento;
                Parameter9.Value = fileData.mes;
                Parameter10.Value = fileData.anio;
                Parameter11.Value = fileData.bloque;
                SqlParameter[] parameters = { Parameter1, Parameter2, Parameter3, Parameter4, Parameter5, Parameter6, Parameter7, Parameter8, Parameter9, Parameter10, Parameter11 };
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

        public async Task<bool> InsertarDataDataForma20(Forma20Data datos)
        {
            try
            {
                SqlConnection conection = new SqlConnection(_formas.Database.GetDbConnection().ConnectionString);
                SqlCommand command = new SqlCommand("FOXT.SP_Guardar_Data_Forma20")
                {
                    Connection = conection
                };
                conection.Open();
                SqlParameter Parameter1 = new SqlParameter("@id", SqlDbType.Int);
                SqlParameter Parameter2 = new SqlParameter("@inyeccionPozo", SqlDbType.VarChar);
                SqlParameter Parameter3 = new SqlParameter("@inyeccionMes", SqlDbType.Int);
                SqlParameter Parameter4 = new SqlParameter("@inyeccionAcumulados", SqlDbType.Int);
                SqlParameter Parameter5 = new SqlParameter("@inyeccionEspesorEfectivoZonaAbiertaPies", SqlDbType.Int);
                SqlParameter Parameter6 = new SqlParameter("@inyeccionPresionMediaInyeccion", SqlDbType.Int);
                SqlParameter Parameter7 = new SqlParameter("@inyeccionVolumenAguaInyMesBls", SqlDbType.Int);
                SqlParameter Parameter8 = new SqlParameter("@inyeccionVolumenAguaInyAcumuladoBls", SqlDbType.Int);
                SqlParameter Parameter9 = new SqlParameter("@inyeccionEstadoPozoFinalMes", SqlDbType.VarChar);
                SqlParameter Parameter10 = new SqlParameter("@produccionPozo", SqlDbType.Int);
                SqlParameter Parameter11 = new SqlParameter("@produccionDiasMes", SqlDbType.Int);
                SqlParameter Parameter12 = new SqlParameter("@produccionDiasAcumulados", SqlDbType.Int);
                SqlParameter Parameter13 = new SqlParameter("@produccionPetroleoMensualBls", SqlDbType.Int);
                SqlParameter Parameter14 = new SqlParameter("@produccionPetroleoAcumuladoBbl", SqlDbType.Int);
                SqlParameter Parameter15 = new SqlParameter("@produccionAguaMensualBls", SqlDbType.Int);
                SqlParameter Parameter16 = new SqlParameter("@produccionAguaAcumuladaBls", SqlDbType.Int);
                SqlParameter Parameter17 = new SqlParameter("@presionFondo", SqlDbType.Int);
                SqlParameter Parameter18 = new SqlParameter("@estadoFinalMes", SqlDbType.VarChar);
                Parameter1.Value = datos.id;
                Parameter2.Value = datos.inyeccionPozo;
                Parameter3.Value = datos.inyeccionMes;
                Parameter4.Value = datos.inyeccionAcumulados;
                Parameter5.Value = datos.inyeccionEspesorEfectivoZonaAbiertaPies;
                Parameter6.Value = datos.inyeccionPresionMediaInyeccion;
                Parameter7.Value = datos.inyeccionVolumenAguaInyMesBls;
                Parameter8.Value = datos.inyeccionVolumenAguaInyAcumuladoBls;
                Parameter9.Value = datos.inyeccionEstadoPozoFinalMes;
                Parameter10.Value = datos.produccionPozo;
                Parameter11.Value = datos.produccionDiasMes;
                Parameter12.Value = datos.produccionDiasAcumulados;
                Parameter13.Value = datos.produccionPetroleoMensualBls;
                Parameter14.Value = datos.produccionPetroleoAcumuladoBbl;
                Parameter15.Value = datos.produccionAguaMensualBls;
                Parameter16.Value = datos.produccionAguaAcumuladaBls;
                Parameter17.Value = datos.presionFondo;
                Parameter18.Value = datos.estadoFinalMes;
                SqlParameter[] parameters = { Parameter1, Parameter2, Parameter3, Parameter4, Parameter5, Parameter6, Parameter7, Parameter8, Parameter9, Parameter10, Parameter11, Parameter12, Parameter13, Parameter14, Parameter15, Parameter16, Parameter17, Parameter18 };
                command.Parameters.AddRange(parameters);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                DataTable DT = new DataTable();
                sqlDataAdapter.SelectCommand = command;
                sqlDataAdapter.Fill(DT);
                List<dynamic> LstResultadoTabla = ConvertTypes.ToDynamic(DT);
                conection.Close();
                var LstResultado = true;
                return await Task.Run(() => LstResultado);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}

