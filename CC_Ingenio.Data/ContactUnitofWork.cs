using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contact.Repository;
namespace Contact.Data
{
    public class ContactUnitofWork :IUnitofWork
    {
        private ContactDbContext dbContext = null;

        public ContactUnitofWork()
        {
            dbContext = new ContactDbContext();
        }

        public void Commit()
        {
            dbContext.SaveChanges();
        }

      

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
