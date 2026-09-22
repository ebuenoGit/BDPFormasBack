using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.Models;
using Backend.Formas.Entities.Responses;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Business
{
    public interface IForma30Business
    {
        Task<ResponseBase<ResponseForma30>> LoadFile(DateTime date, Stream fileStream, string fileName);
        Task<ResponseBase<dynamic>> Create(dynamic data, DateTime date);
        Task<ResponseBase<bool>> CreaForma30(ParametrosCreaForma30 id);
        Task<ResponseBase<bool>> PvsCrea(ParametrosPvsCrea id, string p_code);
        Task<ResponseBase<bool>> PvsoCrea(ParametrosPvsoCrea id, string p_code);
    }
}