using JL.PersistModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    // Интерфейс работы с сущностями БД по принципу CRUD
    public interface IRepository<TModel> : IDisposable where TModel : class, IPersist
    {
        // CREATE
        TModel Insert(TModel persist);

        // READ
        IEnumerable<TModel> GetAll();
        IQueryable<TModel> Get();
        TModel GetById(int id);

        // UPDATE
        TModel Update(TModel persist);
        
        // DELETE
        void Delete(TModel persist);
        void DeleteById(int id);
        

        void SaveChanges();
    }
}
