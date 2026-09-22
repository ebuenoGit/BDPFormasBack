using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.Responses;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Business
{
    public interface IForm16Business
    {
        Task<ResponseBase<RequestFormas<RequestForma16>>> LoadFile(List<RequestForma16> data, Stream _file, string fileName, string getUserId);
        Task<ResponseBase<dynamic>> Create(List<ResponseForma16> data, string GetUserId, string userName);
    }
}