using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.Responses;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Business
{
    public interface IForma20Business
    {
        Task<ResponseBase<RequestFormas<Forma20Request>>> LoadFile(List<Forma20Request> json, Stream fileStream, string fileName, string getUserId);
        Task<ResponseBase<RequestFormas<Forma20Request>>> Create(List<Forma20Request> data, string getUserId, string getNames);
    }
}
