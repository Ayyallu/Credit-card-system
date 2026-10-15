using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact.Repository
{
    public class GenericRepository<TContext, TEntity> :IGenericRepository<TEntity>
        where TContext : IUnitofWork
        where TEntity : class
    {

        protected TContext tContext;

        protected GenericRepository(TContext AContext)
        {
            tContext = AContext;
        }

        public IQueryable<TEntity> Select()
        {
            return tContext.Set<TEntity>().AsQueryable();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return tContext.Set<TEntity>().AsEnumerable();
        }

        public IEnumerable<TEntity> Where(Func<TEntity, bool> APredicate)
        {
            return tContext.Set<TEntity>().Where(APredicate);
        }

        public TEntity GetSingle(Func<TEntity, bool> APredicate)
        {
            return tContext.Set<TEntity>().Single(APredicate);
                
        }

        public TEntity GetFirst(Func<TEntity, bool> APredicate)
        {
            return tContext.Set<TEntity>().First(APredicate);

        }

        public void Add(TEntity AEntity)
        {
            if (AEntity == null)
                throw new ArgumentException("Cannot add a null entity");
            tContext.Set<TEntity>().Add(AEntity);
        }

        public void Delete(TEntity AEntity)
        {
            if (AEntity == null)
                throw new ArgumentException("Cannot delete a null entity");
            tContext.Set<TEntity>().Remove(AEntity);
        }

        public void Attach(TEntity AEntity)
        {
            if (AEntity == null)
                throw new ArgumentException("Cannot attach a null entity");
            tContext.Set<TEntity>().Attach(AEntity);
        }

        private bool disposedValue;
        public void Dispose(bool ADisposing)
        {
            if (!this.disposedValue)
            {
                if (ADisposing)
                {
                    // dispose managed state here if required
                }
                // dispose unmanaged objects and set large fields to null
            }
            this.disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }






    }
}
