using Backend.Formas.Entities.DAO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using Backend.Formas.Entities.DTO.Validaciones;
using Backend.Formas.Entities.Responses;

namespace Backend.Formas.Entities.Interface.Repository
{
    public interface IForma9MasivaRepository
    {
        Task<bool> CreateForm(Form _form);
        Task<Form9Masiva> CreateForm9(Form9Masiva _form9Masiva);
        Task<Concreteform> CreateConcreteform(Concreteform _form9Masiva);
        Task<bool> CreateForm9Detail(List<Form9Masivadetail> _formDetail);
        Task<bool> CreateForm9TotalVolumenDetail(Form9Masivatotalvolumedetail _form9Total);
        Task<Form9Masiva> GetForm(Guid? id);
        Task<Concreteform> GetFormConcreted(Guid? id);
        Task<List<Form9Masivadetail>> GetFormDetail(Guid? id);
        Task<List<Form9Masivatotalvolumedetail>> GetFormTotal(Guid? id);
        Task<bool> CreateAcumulados(List<Acumulados> data);
        Task Delete(Guid id);
        string Guardasqlforma9(string Sql_Json);
        Task<string> GuardaBdpforma9(string Sql_Json);
        string Observacionforma9(int pAnio, int PMes, int PTodo);
        string StringBetween(string Source, string Start, string End);
        Task<string> rep_consultaValidaAcumlaforma9(int pAnio, int pMes, string GetUserId);
        Task<ResponseBase<string>> SqlValidarFormas(string cformas);

    }
}