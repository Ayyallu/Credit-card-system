using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using CC_Ingenio.Manager.Contracts;


using CC_Ingenio.DataTranfer.ContactInfo;
using CC_Ingenio.Repository.ContactRepo;


using CC_Ingenio.DataTranfer;

using CC_Ingenio.Manager.Conversion;
using CT = CC_Ingenio.Core.ContactRepo.ContactAggregate;
using CC_Ingenio.Core.ContactRepo.AddressAggregate;
using CC_Ingenio.Core.ContactRepo.ContactAddressAggregate;

namespace CC_Ingenio.Manager
{
    public class ContactManager : IContactManager
    {

        #region Global Variables

        ContactRepository objContactRepository;
        AddressRepository objAddressRepository;
        ContactAddressRepository objContactAddressRepository;

        #endregion


        #region Constructor

        public ContactManager(ContactRepository AContactRepository,
            AddressRepository AAddressRepository,
            ContactAddressRepository AContactAddressRepository)
        {
            if (AContactRepository == null)
                throw new ArgumentNullException("AContactRepository");

            if (AAddressRepository == null)
                throw new ArgumentNullException("AAddressRepository");

            if (AContactAddressRepository == null)
                throw new ArgumentNullException("AContactAddressRepository");

            objContactRepository = AContactRepository;
            objAddressRepository = AAddressRepository;
            objContactAddressRepository = AContactAddressRepository;
        }
        #endregion

        #region Interface implementation
        /// <summary>
        /// Get all Profiles
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="APageCount"></param>
        /// <returns></returns>
        public List<ContactDTO> FindContacts(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("Messages.warning_InvalidArgumentForFindProfiles");

            
            //recover profiles in paged fashion
            var contacts = objContactRepository.GetPaged<int>(APageIndex, APageCount, x => x.ContactId, false);
            /*var addressTypes = _addressTypeRepository.GetAll().ToList();
            var phoneTypes = _phoneTypeRepository.GetAll().ToList();
            */
            if (contacts != null
                &&
                contacts.Any())
            {
                List<ContactDTO> lstContactDTO = new List<ContactDTO>();
                foreach (var contact in contacts)
                {
                    lstContactDTO.Add(Conversion.Mapping.ContactToContactDTO(contact));
                }
                return lstContactDTO;
            }
            else // no data
                return new List<ContactDTO>();
        }

        /// <summary>
        /// Delete profile
        /// </summary>
        /// <param name="profileId"></param>
        public void DeleteContact(int AContactId)
        {
            var contact = objContactRepository.Get(AContactId);

            if (contact != null) //if profile exist
            {
                //Delete all Address associate with this contact 
                List<ContactAddress> lstContactAddress = contact.ContactAddress.ToList();
                foreach (ContactAddress objContactAddress in lstContactAddress)
                {
                    this.DeleteContactAddress(objContactAddress);
                }



                objContactRepository.Remove(contact);

                //commit changes
                objContactRepository.UnitOfWork.Commit();
            }
            else
            {
                //the customer not exist, cannot remove
                //LoggerFactory.CreateLog().LogWarning(Messages.warning_CannotRemoveNonExistingProfile);
             }
        }

        /// <summary>
        /// Find Profile by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContactDTO FindContactById(int id)
        {

            //recover orders
            var contact = objContactRepository.Get(id);


            if (contact != null)
            {
                return Conversion.Mapping.ContactToContactDTO(contact);
            }
            else //no data
                return new ContactDTO();

        }

        /// <summary>
        /// Get all initialization data for Contact page
        /// </summary>
        /// <returns></returns>
        /*public ContactForm InitializePageData()
        {
            var addressTypes = _addressTypeRepository.GetAll().ToList();
            var phoneTypes = _phoneTypeRepository.GetAll().ToList();

            ContactForm objContactForm = new ContactForm();
            objContactForm.lstAddressTypeDTO = Conversion.Mapping.AddressTypeToAddressTypeDTO(addressTypes);
            objContactForm.lstPhoneTypeDTO = Conversion.Mapping.PhoneTypeToPhoneTypeDTO(phoneTypes);

            return objContactForm;
        }*/

        /// <summary>
        /// Add new profile
        /// </summary>
        /// <param name="profileDTO"></param>
        /// <returns></returns>
        public void SaveContactInformation(ContactDTO AContactDTO)
        {
            try
            {
                //if profileDTO data is not valid
                if (AContactDTO == null)
                    throw new ArgumentException("Messages.warning_CannotAddProfileWithNullInformation");

                //Create a new profile entity
                var newProfile = CT.ContactFactory.CreateContact(AContactDTO.FirstName, AContactDTO.LastName,
                     AContactDTO.MobileNo, AContactDTO.HomePhoneNo,
                AContactDTO.OfficePhoneNo, AContactDTO.Type,
                AContactDTO.OrganizationName, AContactDTO.SSN
                    );

                //Save Profile
                newProfile = SaveContact(newProfile);

                //if profileDTO contains any address
                if (AContactDTO.AddressDTO != null)
                {
                    foreach (AddressDTO objAddressDTO in AContactDTO.AddressDTO)
                    {
                       // this.SaveAddress(objAddressDTO, newProfile);
                    }
                }

               
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }


        

        /// <summary>
        /// Update existing profile
        /// </summary>
        /// <param name="id"></param>
        /// <param name="profileDTO"></param>
        public void UpdateContactInformation(int AContactId, ContactDTO AContactDTO)
        {
            //if profileDTO data is not valid
            if (AContactDTO == null)
                throw new ArgumentException("Messages.warning_CannotAddProfileWithNullInformation");

            //Create a new profile entity
            var currentProfile = objContactRepository.Get(AContactId);

            //Assign updated value to existing contact
            var updatedContact = new CT.Contact();
            updatedContact.ContactId = AContactId;
            updatedContact.FirstName = AContactDTO.FirstName;
            updatedContact.LastName = AContactDTO.LastName;
            updatedContact.MobileNo = AContactDTO.MobileNo;
            updatedContact.HomePhoneNo = AContactDTO.HomePhoneNo;
            updatedContact.OfficePhoneNo = AContactDTO.OfficePhoneNo;
            updatedContact.OrganizationName = AContactDTO.OrganizationName;
            updatedContact.SSN = AContactDTO.SSN;
            updatedContact.Type = AContactDTO.Type;
            

            //Update Profile
            updatedContact = this.UpdateContact(currentProfile, updatedContact);

            //Update Address
            List<AddressDTO> lstUpdatedAddressDTO = AContactDTO.AddressDTO;
            List<ContactAddress> lstCurrentAddress = objContactAddressRepository.GetFiltered(1,10,x => x.ContactId.Equals(AContactId)).ToList();

            UpdateAddress(lstUpdatedAddressDTO, lstCurrentAddress, updatedContact);

            
        }
 

        #endregion


        #region Private Methods


        /// <summary>
        /// Delete profile address
        /// </summary>
        /// <param name="profileId"></param>
        public void DeleteContactAddress(ContactAddress AContactAddress)
        {
            var address = objAddressRepository.Get(AContactAddress.AddressId);

            if (address != null) //if address exist
            {
                objContactAddressRepository.Remove(AContactAddress);
                //commit changes
                objContactAddressRepository.UnitOfWork.Commit();

                objAddressRepository.Remove(address);
                //commit changes
                objAddressRepository.UnitOfWork.Commit();
            }
            else
            {
                // CHECK LATER
                //the customer not exist, cannot remove
             //   LoggerFactory.CreateLog().LogWarning(Messages.warning_CannotRemoveNonExistingProfile);
            }
        }


        /// <summary>
        /// Add new address
        /// </summary>
        /// <param name="addressDTO"></param>
        /// <param name="profile"></param>
        void SaveAddress(AddressDTO addressDTO, CT.Contact AContact)
        {
            //if addressDTO data is not valid
            if (addressDTO == null)
                throw new ArgumentException("Messages.warning_CannotAddProfileWithNullInformation");

            //Create a new Address entity
            var newAddress = AddressFactory.CreateAddress(addressDTO.Line1, addressDTO.Line2, addressDTO.City, addressDTO.State, addressDTO.Country);
            //Save new Address
            SaveAddress(newAddress);

            
             
            //Create a new contact Address entity
            var newContactAddress = ContactAddressFactory.ContactAddress(AContact, newAddress);

            //Save new Address
            SaveContactAddress(newContactAddress);
        }

        /// <summary>
        /// Save Address
        /// </summary>
        /// <param name="address"></param>
        Address SaveAddress(Address AAddress)
        {
            // CHECK LATER
            /*var entityValidator = EntityValidatorFactory.CreateValidator();

            if (entityValidator.IsValid(address)) //if entity is valid save
            {*/
                //add address and commit changes
            objAddressRepository.Add(AAddress);
            //objAddressRepository.UnitOfWork.Commit();
            return AAddress;
           /* }
            else //if not valid, throw validation errors
                throw new ApplicationValidationErrorsException(entityValidator.GetInvalidMessages(address));
            * */
        }

        /// <summary>
        /// Save Profile Address
        /// </summary>
        /// <param name="profileAddress"></param>
        void SaveContactAddress(ContactAddress AContactAddress)
        {
            // CHECK LATER
            /*var entityValidator = EntityValidatorFactory.CreateValidator();

            if (entityValidator.IsValid(profileAddress))//if entity is valid save. 
            {*/
                //add profile address and commit changes
            objContactAddressRepository.Add(AContactAddress);
               // objContactAddressRepository.UnitOfWork.Commit();
            /*}
            else // if not valid throw validation errors
                throw new ApplicationValidationErrorsException(entityValidator.GetInvalidMessages(profileAddress));
             */
        }

        void CommitData()
        { 
            
        }

        /// <summary>
        /// Update profile address
        /// </summary>
        /// <param name="lstUpdatedAddressDTO"></param>
        /// <param name="lstCurrentAddress"></param>
        void UpdateAddress(List<AddressDTO> lstUpdatedAddressDTO, List<ContactAddress> lstCurrentAddress, CT.Contact AContact)
        {
            //if addressDTO data is not valid
            if (lstUpdatedAddressDTO == null && lstCurrentAddress == null) return;

            #region If user has deleted all existing address

            if (lstUpdatedAddressDTO == null && lstCurrentAddress != null)
            {
                foreach (ContactAddress contactAddress in lstCurrentAddress)
                {
                    DeleteContactAddress(contactAddress);
                }
                return;
            }

            #endregion If user has deleted all existing address

            #region If user has added new address and there was not any existing address

            if (lstUpdatedAddressDTO != null && lstCurrentAddress == null)
            {
                foreach (AddressDTO addressDTO in lstUpdatedAddressDTO)
                {
                    this.SaveAddress(addressDTO, AContact);
                }
                return;
            }

            #endregion If user has added new address and there was not any existing address

            #region if user has updated or Deleted any record

            List<AddressDTO> lstNewAddress = lstUpdatedAddressDTO;

            //Check if address exist in database
            foreach (ContactAddress contactAddress in lstCurrentAddress)
            {
                AddressDTO objAddressDTO = lstUpdatedAddressDTO.FirstOrDefault(x => x.AddressId.Equals(contactAddress.AddressId));

                if (objAddressDTO != null)
                {
                    Address updatedAddress = new Address();
                    updatedAddress.AddressId = objAddressDTO.AddressId;
                    updatedAddress.Line1 = objAddressDTO.Line1;
                    updatedAddress.Line2 = objAddressDTO.Line2;
                    updatedAddress.City = objAddressDTO.City;
                    updatedAddress.State = objAddressDTO.State;
                    updatedAddress.Country = objAddressDTO.Country;
                    
                    UpdateAddress(contactAddress.Address, updatedAddress);
                    lstNewAddress.Remove(objAddressDTO);
                }
                else
                {
                    DeleteContactAddress(contactAddress);
                }
            }

            //Save new address
            foreach (AddressDTO addressDTO in lstNewAddress)
            {
                this.SaveAddress(addressDTO, AContact);
            }

            #endregion if user has updated or Deleted any record
        }

        /// <summary>
        /// Update existing Address
        /// </summary>
        /// <param name="profile"></param>
        void UpdateAddress(Address ACurrentAddress, Address AUpdatedAddress)
        {
            // CHECK LATER
            /*updatedAddress.Created = currentAddress.Created;
            updatedAddress.CreatedBy = currentAddress.CreatedBy;
            updatedAddress.Updated = DateTime.Now;
            updatedAddress.UpdatedBy = "Updated User";

            var entityValidator = EntityValidatorFactory.CreateValidator();

            if (entityValidator.IsValid(updatedAddress))//if entity is valid save. 
            {*/
                //add address and commit changes
                objAddressRepository.Merge(ACurrentAddress, AUpdatedAddress);
                objAddressRepository.UnitOfWork.Commit();
           /* }
            else // if not valid throw validation errors
                throw new ApplicationValidationErrorsException(entityValidator.GetInvalidMessages(updatedAddress));
      
            */
        }


        /// <summary>
        /// Save Contact
        /// </summary>
        /// <param name="AContact"></param>
        CT.Contact SaveContact(CT.Contact AContact)
        {
            //CHECK LATER
            /*var entityValidator = EntityValidatorFactory.CreateValidator();
            CHECK LATER
            if (entityValidator.IsValid(profile))//if entity is valid save. 
            {*/
            //add profile and commit changes
            objContactRepository.Add(AContact);
            objContactRepository.UnitOfWork.Commit();
            return AContact;
            /*}
            else // if not valid throw validation errors
                throw new ApplicationValidationErrorsException(entityValidator.GetInvalidMessages(profile));*/
        }

        /// <summary>
        /// Update existing Contact
        /// </summary>
        /// <param name="AContact"></param>
        CT.Contact UpdateContact(CT.Contact ACurrentContact, CT.Contact AUpdatedContact)
        {
          //CHECK LATER
            /*AUpdatedContact.Created = ACurrentContact.Created;
            AUpdatedContact.CreatedBy = ACurrentContact.CreatedBy;
            AUpdatedContact.Updated = DateTime.Now;
            AUpdatedContact.UpdatedBy = "Updated User";

            var entityValidator = EntityValidatorFactory.CreateValidator();

            if (entityValidator.IsValid(updatedProfile))//if entity is valid save. 
            {*/
                //add profile and commit changes
                objContactRepository.Merge(ACurrentContact, AUpdatedContact);
                objContactRepository.UnitOfWork.Commit();
                return AUpdatedContact;
           /* }
            else // if not valid throw validation errors
                throw new ApplicationValidationErrorsException(entityValidator.GetInvalidMessages(updatedProfile));
            */
        }

        #endregion
    }
}
