using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact.Repository.Repositories
{
    public class ContactContext : System.Data.Entity.DbContext, IContactContext
    {
        private IContactContext tContext;
        private IGenericRepository<Contact> tCustomerRepository;

        public ContactContext()
        { }
        public ContactContext(string AConnectionstring)
            : base(AConnectionstring)
        {
            tContext = new ContactContext();
            tCustomerRepository = new GenericRepository<IContactContext, Contact>(tContext);
        }
    }
}
