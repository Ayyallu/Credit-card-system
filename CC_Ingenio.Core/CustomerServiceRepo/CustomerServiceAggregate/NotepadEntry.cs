using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate
{
    [Table("CS_NotepadEntry")]
    public class NotepadEntry
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotepadEntryId {get;set;}
        public int CustomerId {get;set;}
        public string CardNumber { get; set; }
	    public string MessageType {get;set;}
	    public string MessageDescription {get;set;}
        public string MessageStatus { get; set; }
	    public string CreatedBy {get;set;}
	    public DateTime CreatedOn {get;set;}
	    public string LastUpdatedBy {get;set;}
	    public DateTime LastUpdatedOn {get;set;}
    }
}
