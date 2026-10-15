using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate
{
    public class NotepadEntryFactory
    {
        public static NotepadEntry CreateNotepadEntry(string AMessageType,string AMessageDescription,
            int ACustomerId, string ACardNumber,string ACreatedBy,string ALastUpdatedBy)
        {
            NotepadEntry objNotepadEntry = new NotepadEntry() { 
                CustomerId = ACustomerId,
                CardNumber = ACardNumber,
                MessageType = AMessageType,
                MessageDescription = AMessageDescription,
                MessageStatus = "O",//O- Status 'Open'
                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate
            };
            return objNotepadEntry;
        }
    }
}
