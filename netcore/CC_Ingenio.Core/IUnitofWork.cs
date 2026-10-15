using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core
{
    public interface IUnitofWork :IDisposable
    {
       // DbSet<TEntity> Set<TEntity>() where TEntity : class;

        void Commit();
        void CommitAndRefreshChanges();
        void RollbackChanges();
        int SaveChanges();
        void RefreshDataInModel<TEntity>(ref TEntity AItem) where TEntity : class;
        void ReloadAllDataInContext<TEntity>() where TEntity : class;
         IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters) where TEntity : class;
       
        int ExecuteCommand(string sqlCommand, params object[] parameters);
        IEnumerable ExecuteSqlQuery(Type elementType, string sqlQuery, params object[] parameters);
    
    }
}
