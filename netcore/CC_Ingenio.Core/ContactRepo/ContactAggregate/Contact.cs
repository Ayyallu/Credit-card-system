using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CC_Ingenio.Core.ContactRepo.ContactAddressAggregate;
namespace CC_Ingenio.Core.ContactRepo.ContactAggregate
{
    public partial class Contact 
    {
        public Contact()
        {
            this.ContactAddress = new HashSet<ContactAddress>();
        }


        #region Model
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContactId { get; set; }
        [Required(ErrorMessage = "Please enter mobile no"), StringLength(15, ErrorMessage = "Mobile should be less than 15 characters")]
        public string MobileNo { get; set; }
        public string HomePhoneNo { get; set; }
        public string OfficePhoneNo { get; set; }
        public string Type { get; set; }
        public string OrganizationName { get; set; }
        [Required(ErrorMessage = "Please enter first name"), StringLength(25, ErrorMessage = "First name should be less than 25 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter last name"), StringLength(25, ErrorMessage = "Last name should be less than 25 characters"), Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string SSN { get; set; }

     
        public virtual ICollection<ContactAddress> ContactAddress { get; set; }
        #endregion

    }
}
