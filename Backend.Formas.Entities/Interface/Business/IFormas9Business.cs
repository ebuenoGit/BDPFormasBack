using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.Responses;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Business
{
    public interface IFormas9Business
    {
        Task<ResponseBase<Forma9StructureDTO>> UploadFile(DateTime date, Stream files, string fileName, string getUserId);
        Task<ResponseBase<dynamic>> Create(RequestCreateForma9 files, DateTime date, string GetUserId, string nameUser);
    }
}