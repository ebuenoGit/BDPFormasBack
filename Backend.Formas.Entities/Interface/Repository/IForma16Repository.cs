using Backend.Formas.Entities.DAO;
using System;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Repository
{
    public interface IForma16Repository
    {
        Task<bool> SaveForm16(Form16 form16);
        Task<bool> SaveForm16Detail(Form16Detail form16);
        Task<Form16> GetForm16(Guid id);
        Task<Form16Detail> GetForm16Detail(Guid id);
        Task<bool> Delete(Guid id);
    }
}