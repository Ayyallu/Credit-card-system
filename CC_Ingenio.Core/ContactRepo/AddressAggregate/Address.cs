using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CC_Ingenio.Core.ContactRepo.ContactAddressAggregate;

namespace CC_Ingenio.Core.ContactRepo.AddressAggregate
{
    public partial class Address
    {
        public Address()
        {
            this.ContactAddress = new HashSet<ContactAddress>();
        }

        #region Model
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public virtual ICollection<ContactAddress> ContactAddress { get; set; } 
        #endregion
    }
}
