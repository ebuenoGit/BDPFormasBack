using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.Models;
using Backend.Formas.Entities.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Business
{
    public interface IForma30SEEBusiness
    {
        Task<ResponseBase<RequestFormas<fileJSON>>> InsertForma30(DateTime date, string json, string forma, string Usuario);

        Task<ResponseBase<List<dynamic>>> ValidarJsonBDP(DateTime date, Stream fileStream, string FileName, string forma, string fileJSON, string GetUserId, string nombre, string email);
    }
}
