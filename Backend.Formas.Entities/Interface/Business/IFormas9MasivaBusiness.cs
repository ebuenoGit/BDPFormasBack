using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.DTO.Validaciones;
using Backend.Formas.Entities.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Business
{
    public interface IFormas9MasivaBusiness
    {
        Task<ResponseBase<Forma9MasivaStructureDTO>> UploadFile(DateTime date, Stream files, string fileName, string getUserId);
        Task<ResponseBase<dynamic>> Create(RequestCreateForma9Masiva files, DateTime date, string GetUserId, string nameUser);
        Task<string> GuardarFormaPPDM(List<DetalleJsonF9Masiva> Datos);
        Task<string> consultaforma9(int pAnio, int PMes, int PTodo);
        Task<string>  consultaValidaAcumlaforma9(int pAnio, int pMes, string GetUserId);
        string StringBetween(string cSource, string Start, string End);
    }
}