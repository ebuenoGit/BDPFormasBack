using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.Responses;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Business
{
    public interface IFormaC7Business
    {
        Task<ResponseBase<RequestFormas<RequestFormaC7>>> LoadFile(List<RequestFormaC7> data, Stream _file, string fileName, string getUserId);
        Task<ResponseBase<dynamic>> Create(List<ResponseFormaC7> data, string GetUserId, string userName);
    }
}
