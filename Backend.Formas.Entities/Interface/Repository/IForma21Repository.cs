using Backend.Formas.Entities.DAO;
using System;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Repository
{
    public interface IForma21Repository
    {
        Task<Form21> SaveForma21(Form21 data);
        Task<bool> SaveForm21InyectionDetail(Form21InyectionDetail form21InyectionDetail);
        Task<bool> SaveForm21ProductionDetail(Form21ProductionDetail form21ProductionDetail);
        Task<bool> SaveForm21TotalVolumenDetail(Form21TotalVolumenDetail form21TotalVolumenDetail);
        Task<Form21> GetForm21(Guid id);
        Task<Form21InyectionDetail> GetForm21InyectionDetail(Guid id);
        Task<Form21ProductionDetail> GetForm21ProductionDetail(Guid id);
        Task<Form21TotalVolumenDetail> GetForm21TotalVolumenDetail(Guid id);
        Task<bool> Delete(Guid id);
    }
}
