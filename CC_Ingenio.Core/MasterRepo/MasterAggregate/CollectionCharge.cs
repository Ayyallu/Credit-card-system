/*All rights reserved.  No part of this program or publication may be reproduced,
transcribed, stored in a retrieval system, or translated into any language or computer
language, or in any form or by any means, electronic, mechanical, magnetic, optical,
chemical, biological or otherwise, without the prior written permission of :


      GEMINI SOFTWARE SOLUTIONS PRIVATE LTD.,
      NILA, TECHNOPARK CAMPUS, TRIVANDRUM, INDIA.

      - - - - - - - - - - - - - - - - - - - - -
      File Name : CollectionCharge.cs
      - - - - - - - - - - - - - - - - - - - - -

      System				:  	CC_Ingenio
      Module				:  	Collection Charge model
      Author				:	Bijily
      Date					:	08/04/2014
      Function				:	Model for Collection Charge
      Modification History	:   
     

      Label			Date		Name		Change description
      - - - - -	   - - - - 		- - - - - 	- - - - - - - - - - - - - - - - 
 * 
 * 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CC_Ingenio.Core.LookUpRepo;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_CollectionCharge")]
    public class CollectionCharge
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CollectionChargeId { get; set; }
        public string ProductCode { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string OrganizationCode { get; set; }
        public string CurrencyCode { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public virtual Product Product { get; set; }
        public virtual List<CollectionChargeDetails> CollectionChargeDetails { get; set; }

        public CollectionCharge()
        {
            CollectionChargeDetails = new List<CollectionChargeDetails>();
        }

    }
}
