using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Expressions;
namespace CC_Ingenio.Core
{
    public interface IGenericRepository<TEntity>  :IDisposable
        where TEntity :class
    {
        /*IQueryable<TEntity> Select();
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Where(Func<TEntity,bool> predicate);
        int ExecuteSP<TEntity>(string AProcedureName, IList<TEntity> AParamsList);
        TEntity GetSingle(Func<TEntity, bool> predicate);
        TEntity GetFirst(Func<TEntity, bool> predicate);


        void Add(TEntity entity);
        void Delete(TEntity entity);
        void Attach(TEntity entity);

        void Dispose(bool disposing);*/

        IUnitofWork UnitOfWork{ get; }

        void Add(TEntity AItem);
        void Remove(TEntity AItem);
        void Modify(TEntity AItem);
        void TrackItem(TEntity AItem);
        void Merge(TEntity APersistedItem, TEntity ACurrentItem);
        

        TEntity Get(int AUniqueId);
        TEntity Get(string ACode);

        IQueryable<TEntity> GetAll();
        IEnumerable<TEntity> AllMatching(Expression<Func<TEntity,bool>> APredicate);

        IEnumerable<TEntity> GetPaged<Property>(int APageIndex, int APageCount,
            Expression<Func<TEntity, Property>> AOrderByExpression, bool AIsAscending);

        IEnumerable<TEntity> GetFiltered(int APageNo, int APageCount, Expression<Func<TEntity, bool>> AFilter);
        IEnumerable<TEntity> GetFiltered(int APageNo, int APageCount, Expression<Func<TEntity, bool>> AFilter, object[] AIncludes); //Expression<Func<TEntity, object>>[]

        /*void UpdateCollection(List<TEntity> AUpdatedEntity, List<TEntity> ACurrentEntity, TEntity AEntity);*/
        


        int GetRecordCount();
        int GetRecordCount(Expression<Func<TEntity,bool>> APredicate);




    }
}
