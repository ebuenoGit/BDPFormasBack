using Backend.Formas.Entities.DAO;
using System;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Repository
{
    public interface IFormaC7Repository
    {
        Task<bool> SaveFormC7(FormC7 formC7);
        Task<bool> SaveFormC7Detail(FormC7Detail formC7dDetail);
        Task<FormC7> GetFormC7(Guid id);
        Task<FormC7Detail> GetFormC7Detail(Guid id);
        Task<bool> Delete(Guid id);
    }
}
