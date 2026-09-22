using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.Models;
using Backend.Formas.Entities.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Business
{
    public interface IFormas15CRBusiness
    {
        Task<ResponseBase<RequestFormas<Forma15CR>>> ValidateForm15CR(DateTime date, string forma, string fileJson, string usuario);

        Task<ResponseBase<List<dynamic>>> InsertForma15(Stream fileStream, DateTime date, string forma, string fileJSON, string GetUserId, string nombre, string email, string fileName);
    }
}
