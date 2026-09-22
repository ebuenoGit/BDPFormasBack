using Backend.Formas.Entities.DAO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using Backend.Formas.Entities.DTO.Validaciones;
using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.Responses;

namespace Backend.Formas.Entities.Interface.Repository
{
    public interface IFormaC4MasivaRepository
    {
        Task<BodyContenteDTOC4Masiva> CreateFormC4Masiva(BodyContenteDTOC4Masiva _formC4Masiva);

        public string GuardaBdpforma4(string Sql_Json);
        public string Guardasqlforma4(string Sql_Json);
        public string Guardasqlforma4Muni(string Sql_Json);
        string StringBetween(string Source, string Start, string End);
        string ConsultarformaC4(int pAnio, int PMes, int PTodo);
        string ConsultarformaC4Muni(int pAnio, int PMes, int PTodo);
        Task<ResponseBase<string>> SqlValidarFormas(string cformas);
    }
}