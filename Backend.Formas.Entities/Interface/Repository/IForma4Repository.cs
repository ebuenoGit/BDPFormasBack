
using Backend.Formas.Entities.DAO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Repository
{
    public interface IForma4Repository
    {
        Task<Formc4> SaveFormac4(Formc4 data);
        Task<bool> SaveFormaDetail(List<Formc4netvolumedetail> data);
        Task<bool> SaveFormac4Total(List<Formc4totalvolumedetail> data);
        Task<Concreteform> SaveConcret(Concreteform concre);
        Task<Concreteform> GetConcreForm(Guid id);
        Task<Formc4> GetFormC4(Guid id);
        Task<bool> Delete(Guid id);
    }
}