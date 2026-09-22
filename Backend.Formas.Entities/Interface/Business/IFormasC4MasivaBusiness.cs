using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.DTO.Validaciones;
using Backend.Formas.Entities.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Business
{
    public interface IFormasC4MasivaBusiness
    {
        Task<ResponseBase<FormaC4MasivaStructureDTO>> UploadFile(DateTime date, Stream files, string fileName, string getUserId);
        Task<ResponseBase<dynamic>> Create(RequestCreateFormaC4Masiva files, DateTime date, string GetUserId, string nameUser);
        Task<string> GuardarFormaC4PPDM(List<DetalleJsonC4Masiva> Data);
        Task<string> GuardarFormaC4PPDMuni(List<DetalleJsonC4Municipio> Data);
        string StringBetween(string result, string v1, string v2);
        Task<string> consultaformaC4(int pAnio, int PMes, int PTodo); 
        Task<string> consultaformaC4Muni(int pAnio, int PMes, int PTodo);
    }

}