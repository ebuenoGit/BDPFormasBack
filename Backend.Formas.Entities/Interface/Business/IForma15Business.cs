using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.Responses;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Business
{
    public interface IForma15Business
    {
        Task<ResponseBase<ResponseForma15>> LoadFile(DateTime date, Stream _file);
        Task<ResponseBase<dynamic>> Create(ResponseForma15 data, DateTime date);
    }
}