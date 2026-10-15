using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.Data.Contracts;
using CC_Ingenio.Core;
using Microsoft.EntityFrameworkCore;
using CC_Ingenio.Common.Logging;
using System.Linq.Expressions;
namespace CC_Ingenio.Data
{
    public class GenericRepository< TEntity> : IGenericRepository<TEntity> 
    
        where TEntity : class
    {
        #region Members
        
        protected IQueryableUnitOfWork iqUnitOfWork;
        
        #endregion

        #region Constructor
        
        
        public GenericRepository(IQueryableUnitOfWork AUnitOfWork)
        {
            if (AUnitOfWork == (IUnitofWork)null)
                throw new ArgumentNullException("unitOfWork");

            iqUnitOfWork = AUnitOfWork;
        }
        
        #endregion

        #region IRepository Members

        public IUnitofWork UnitOfWork
        {
            get { return iqUnitOfWork; }
        }

        public virtual void Add(TEntity AEntity)
        {
            if (AEntity != (TEntity)null)
                GetSet().Add(AEntity);
            else
            {
                LoggerFactory.CreateLog().LogError("Cannot add null entity", 100, 2);
            }
        }

        public virtual void Remove(TEntity AItem)
        {
            if (AItem != (TEntity)null)
            {
                //attach item if not exist
                iqUnitOfWork.Attach(AItem);

                //set as "removed"
                GetSet().Remove(AItem);
            }
            else
            {
                
                LoggerFactory.CreateLog().LogError("Cannot remove null entity", 100, 2);
            }
        }

        public virtual void TrackItem(TEntity AEntity)
        {
            if (AEntity != (TEntity)null)
                iqUnitOfWork.Attach(AEntity);
            else
            {
                LoggerFactory.CreateLog().LogError("Cannot remove null entity", 100, 2);
            }
        }

        public virtual void Modify(TEntity AEntity)
        {
            if (AEntity != (TEntity)null)
                iqUnitOfWork.SetModified(AEntity);
            else
            {
                LoggerFactory.CreateLog().LogError("Cannot remove null entity", 100, 2);
            }
        }

        public virtual TEntity Get(int AUniqueId)
        {
            if (AUniqueId != 0)
                return GetSet().Find(AUniqueId);
            else
                return null;
        }
        public virtual TEntity Get(string ACode)
        {
            if (!string.IsNullOrWhiteSpace(ACode))
                return GetSet().Find(ACode);
            else
                return null;
        }

        public virtual TEntity Get(params object[] AKeys)
        {
            if (AKeys != null)
                return GetSet().Find(AKeys);
            else
                return null;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            
            return GetSet().AsNoTracking().AsQueryable();
        }

        public virtual IEnumerable<TEntity> AllMatching(Expression<Func<TEntity, bool>> APredicate)
        {
            return GetSet().AsNoTracking().Where(APredicate);
        }

        public virtual IEnumerable<TEntity> GetPaged<TProperty>(int APageIndex, int APageCount,
            Expression<Func<TEntity, TProperty>> AOrderByExpression, bool AIsAscending)
        {
            var set = GetSet().AsNoTracking();

            if (AIsAscending)
            {
                return set.OrderBy(AOrderByExpression)
                          .Skip(APageCount * APageIndex)
                          .Take(APageCount);
            }
            else
            {
                return set.OrderByDescending(AOrderByExpression)
                          .Skip(APageCount * APageIndex)
                          .Take(APageCount);
            }
        }

        public virtual int GetRecordCount()
        {
            var set = GetSet().AsNoTracking();
            return set.Count();
        }
        public virtual int GetRecordCount(Expression<Func<TEntity,bool>> APredicate)
        {
            var set = GetSet().AsNoTracking();
            return set.Where(APredicate).Count();
        }

        public virtual IEnumerable<TEntity> GetFiltered(int APageNo, int APageCount,  Expression<Func<TEntity, bool>> AFilter)
        {

            return GetSet()
                .AsNoTracking()

                .Where(AFilter)
                            .OrderBy(AFilter)
                            .Skip(APageNo * APageCount)
                            .Take(APageCount);
        }


        public virtual IEnumerable<TEntity> GetFiltered(int APageNo, int APageCount, Expression<Func<TEntity, bool>> AFilter, object[] AIncludes) 
        {
            IQueryable<TEntity> query = GetSet().AsNoTracking();
            if (AIncludes != null)
            {
                /*query = AIncludes.Aggregate(query,
                 (current, include) => current.Include(include));*/
                foreach (string item in AIncludes)
                {
                    query = query.Include(item);
                }      
                
            }
            return query.Where(AFilter)
                            .OrderBy(AFilter)
                            .Skip(APageNo * APageCount)
                            .Take(APageCount);
        }


        public virtual void Merge(TEntity APersisted, TEntity ACurrent)
        {
            
            iqUnitOfWork.ApplyCurrentValues(APersisted, ACurrent);
        }

        /*public virtual void UpdateCollection(List<TEntity> AUpdatedEntityList, List<TEntity> ACurrentEntityList, TEntity AEntity)
        {
            if (AUpdatedEntityList == null && ACurrentEntityList == null)
                return;
            
            #region if user has deleted all the existing records, then remove that one from the model and db
            if( AUpdatedEntityList == null && ACurrentEntityList != null)
            {
                foreach (var item in ACurrentEntityList)
                {
                    GetSet().Remove(item);
                }
            }
            #endregion

            #region If user has added a new record which was not in the model/db
            if (AUpdatedEntityList != null && ACurrentEntityList == null)
            {
                foreach (var item in AUpdatedEntityList)
                {
                    GetSet().Add(item);
                }
            }
            #endregion

            #region If the user has deleted/modified a record
            if (AUpdatedEntityList != null && ACurrentEntityList != null)
            {
                foreach (var item in AUpdatedEntityList)
                { 
                    
                }
            }
            #endregion
        }*/
        #endregion

        /* public IQueryable<TEntity> Select()
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
            
        }*/


        #region IDisposable members
        
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
        
        #endregion

        #region Private Methods
        DbSet<TEntity> GetSet()
        {
            return iqUnitOfWork.CreateSet<TEntity>();
        }
        #endregion




    }
}
