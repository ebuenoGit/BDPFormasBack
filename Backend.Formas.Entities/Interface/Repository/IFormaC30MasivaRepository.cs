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
    public interface IFormaC30MasivaRepository
    {
        Task<BodyContenteDTOC30Masiva> CreateFormC30Masiva(BodyContenteDTOC30Masiva _formC30Masiva);

        public string GuardaBdpforma30(string Sql_Json);
        public string Guardasqlforma30(string Sql_Json);
        string StringBetween(string Source, string Start, string End);
        string ConsultarformaC30(int pAnio, int PMes, int PTodo);
        Task<ResponseBase<string>> SqlValidarFormas30(string cformas);
    }
}