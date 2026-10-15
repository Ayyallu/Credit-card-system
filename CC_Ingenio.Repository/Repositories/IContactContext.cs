using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
namespace Contact.Repository.Repositories
{
    public interface IContactContext : IUnitofWork
    {
        DbSet<Contact> Contact { get; set; }
    }
}
