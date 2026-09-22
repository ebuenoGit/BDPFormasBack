using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.DTO.Validaciones;
using Backend.Formas.Entities.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Business
{
    public interface IFormasC30MasivaBusiness
    {
        Task<ResponseBase<FormaC30MasivaStructureDTO>> UploadFile(DateTime date, Stream files, string fileName, string getUserId);
        Task<ResponseBase<dynamic>> Create(RequestCreateFormaC30Masiva files, DateTime date, string GetUserId, string nameUser);
        Task<string> GuardarFormaC30PPDM(List<DetalleJsonC30Masiva> Data);
        string StringBetween(string result, string v1, string v2);
        Task<string> consultaformaC30(int pAnio, int PMes, int PTodo); 
    }

}