using DAL.Repository;
using JL.PersistModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.DAL.Repository
{
    public class RepositoryBase<TModel> : IRepository<TModel> where TModel : class, IPersist
    {
        private JLContext _context;

        public RepositoryBase(JLContext context)
        {
            _context = context;
        }

        private DbSet<TModel> getDbSet() => _context.Set<TModel>();


        // DELETE
        public void Delete(TModel persist)
        {
            _context.Remove(persist);
        }

        public void DeleteById(int id)
        {
            var persist = getDbSet().Where(x => x.Id == id).FirstOrDefault();
            if (persist != null) _context.Remove(persist);
        }

        // READ
        public IQueryable<TModel> Get()
        {
            IQueryable<TModel> query = _context.Set<TModel>();
            return query;
        }

        public IEnumerable<TModel> GetAll()
        {
            return getDbSet();
        }

        public TModel? GetById(int id)
        {
            return getDbSet().FirstOrDefault(x => x.Id == id);
        }

        // CREATE
        public TModel Insert(TModel persist)
        {
            return _context.Add(persist).Entity;
        }

        
        // UPDATE
        public TModel Update(TModel persist)
        {
            return (TModel)_context.Update(persist).Entity;
        }


        public void SaveChanges()
        {
            _context.SaveChanges();
        }


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
