using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contact.Repository;
using System.Data.Entity;
namespace Contact.Data
{
    public interface IContactDbContext : IUnitofWork
    {
        //IDbSet<TEntity> GetDbSet<TEntity>() where TEntity : class;
        DbSet<Contact.Models.Contact> Contact { get; set; }
    }
}
