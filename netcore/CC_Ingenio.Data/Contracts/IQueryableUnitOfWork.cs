using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CC_Ingenio.Core;

namespace CC_Ingenio.Data.Contracts
{
    public interface IQueryableUnitOfWork : IUnitofWork, ISql
    {
        DbSet<TEntity> CreateSet<TEntity>() where TEntity : class;
        void Attach<TEntity>(TEntity AItem) where TEntity : class;
        
        void SetModified<TEntity>(TEntity AItem) where TEntity : class;
        void ApplyCurrentValues<TEntity>(TEntity AOriginalItem, TEntity ACurrentIte) where TEntity : class;
    }
}
