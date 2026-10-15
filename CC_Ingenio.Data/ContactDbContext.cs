using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOD = Contact.Models;
using System.Data.Entity;
using Contact.Repository;

namespace Contact.Data
{
    public class ContactDbContext : DbContext, IContactDbContext 
    {


        public IDbSet<TEntity> GetDbSet<TEntity>() where TEntity : class
        {
            return Set<TEntity>();
        }

        public ContactDbContext()
        {
           
        }
   
        public ContactDbContext(string AConnectionString)
            : base(AConnectionString)
        { 
        
        }

        public DbSet<MOD.Contact> Contact { get; set; }
        public DbSet<MOD.Address> Address { get; set; }


     
    }
}
