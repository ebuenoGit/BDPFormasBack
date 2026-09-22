using Backend.Formas.Entities.DAO;
using System;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Repository
{
    public interface IForma23Repository
    {
        Task<bool> SaveForm23(Form23 form23);
        Task<bool> SaveForm23Detail(Form23Detail form23);
        Task<bool> DeleteForm23(Guid id);
        Task<Form23> GetForm23(Guid id);
        Task<Form23Detail> GetForm23Detail(Guid id);
    }
}
