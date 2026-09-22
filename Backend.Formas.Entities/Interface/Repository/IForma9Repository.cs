using Backend.Formas.Entities.DAO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Repository
{
    public interface IForma9Repository
    {
        Task<bool> CreateForm(Form _form);
        Task<Form9> CreateForm9(Form9 _form9);
        Task<Concreteform> CreateConcreteform(Concreteform _form9);
        Task<bool> CreateForm9Detail(List<Form9detail> _formDetail);
        Task<bool> CreateForm9TotalVolumenDetail(Form9totalvolumedetail _form9Total);
        Task<Form9> GetForm(Guid? id);
        Task<Concreteform> GetFormConcreted(Guid? id);
        Task<List<Form9detail>> GetFormDetail(Guid? id);
        Task<List<Form9totalvolumedetail>> GetFormTotal(Guid? id);
        Task<bool> CreateAcumulados(List<Acumulados> data);
        Task Delete(Guid id);
    }
}