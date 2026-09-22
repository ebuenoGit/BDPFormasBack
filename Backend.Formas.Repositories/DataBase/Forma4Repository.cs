using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Formas.Repositories.DataBase
{

    public class Forma4Repository : IForma4Repository
    {
        private readonly IBaseRepository<Concreteform> RepositoryConcreteForm;
        private readonly IBaseRepository<Form> RepositoryForm;

        private readonly IBaseRepository<Formc4> RepositoryForm4;

        private readonly IBaseRepository<Formc4netvolumedetail> RepositoryForm4Detail;
        private readonly IBaseRepository<Aprobacioncarga> RepositoryAprobacion;

        private readonly IBaseRepository<Formc4totalvolumedetail> RepositoryForm4totalvolumedetail;

        public Forma4Repository(
            IBaseRepository<Concreteform> repositoryConcreteForm,
            IBaseRepository<Form> repositoryForm,
            IBaseRepository<Formc4> repositoryForm4,
            IBaseRepository<Formc4netvolumedetail> repositoryForm4Detail,
            IBaseRepository<Formc4totalvolumedetail> repositoryForm4totalvolumedetail,
            IBaseRepository<Aprobacioncarga> repositorioAprobacion
        )
        {
            RepositoryConcreteForm = repositoryConcreteForm;
            RepositoryForm = repositoryForm;
            RepositoryForm4 = repositoryForm4;
            RepositoryForm4totalvolumedetail = repositoryForm4totalvolumedetail;
            RepositoryForm4Detail = repositoryForm4Detail;
            RepositoryAprobacion = repositorioAprobacion;
        }

        public async Task<Concreteform> SaveConcret(Concreteform concre)
        {
            await RepositoryConcreteForm.AddAsync(concre);
            return concre;
        }

        public async Task<Formc4> SaveFormac4(Formc4 data)
        {
            await RepositoryForm4.AddAsync(data);
            return data;
        }

        public async Task<bool> SaveFormaDetail(List<Formc4netvolumedetail> data)
        {
            await RepositoryForm4Detail.AddAsync(data);
            return true;
        }

        public async Task<bool> SaveFormac4Total(List<Formc4totalvolumedetail> data)
        {
            await RepositoryForm4totalvolumedetail.AddAsync(data);
            return true;
        }

        public async Task<Concreteform> GetConcreForm(Guid id)
        {
            return await RepositoryConcreteForm.GetAsync(predicate: x => x.Formid == id);
        }

        public async Task<Formc4> GetFormC4(Guid id)
        {
            return await RepositoryForm4.GetAsync(predicate: x => x.Formc4id == id);
        }


        public async Task<bool> Delete(Guid id)
        {
            await RepositoryAprobacion.DeleteAsync(predicate: x => x.IdForma == id);
            await RepositoryConcreteForm.DeleteAsync(predicate: x => x.Formid == id);
            await RepositoryForm4.DeleteAsync(predicate: x => x.Formc4id == id);
            await RepositoryForm4Detail.DeleteAsync(predicate: x => x.Formid == id);
            await RepositoryForm4totalvolumedetail.DeleteAsync(predicate: x => x.Formid == id);

            return true;
        }


    }
}