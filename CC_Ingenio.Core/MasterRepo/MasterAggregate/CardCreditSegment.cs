/*All rights reserved.  No part of this program or publication may be reproduced,
transcribed, stored in a retrieval system, or translated into any language or computer
language, or in any form or by any means, electronic, mechanical, magnetic, optical,
chemical, biological or otherwise, without the prior written permission of :


      GEMINI SOFTWARE SOLUTIONS PRIVATE LTD.,
      NILA, TECHNOPARK CAMPUS, TRIVANDRUM, INDIA.

      - - - - - - - - - - - - - - - - - - - - -
      File Name : BillingCycleParameter.cs
      - - - - - - - - - - - - - - - - - - - - -

      System				:  	CC_Ingenio
      Module				:  	Card Credit Segment Model
      Author				:	Smrithy
      Date					:	19/06/2014
      Function				:	Model for Card Credit Segment
      Modification History	:   
     

      Label			Date		Name		Change description
      - - - - -	   - - - - 		- - - - - 	- - - - - - - - - - - - - - - - 
 * 
 * 
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.Core.LookUpRepo;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_CardCreditSegment")]
    public class CardCreditSegment
    {
        [Key, Column(Order = 0)]
        public string CreditSegmentCode { get; set; }
        [Key, Column(Order = 1)]
        public string CardNumber { get; set; }
        [Column(Order = 2)]
        public string CustomerSegmentCode { get; set; }
        [Column(Order = 3)]
        public string CustomerSubSegmentCode { get; set; }
        [Key, Column(Order = 4)]
        public string OrganizationCode { get; set; }
        
        public string RUT { get; set; }   
        public string ProductCode { get; set; }      
        public decimal CreditValue { get; set; }
        public decimal OTB { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public decimal Paid { get; set; }
        public string CreditSegmentStatus { get; set; }
       
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        [ForeignKey("CustomerSegmentCode, OrganizationCode")]
        public virtual CustomerSegment objCustomerSegment { get; set; }
        [ForeignKey("CustomerSubSegmentCode, OrganizationCode")]
        public virtual CustomerSubSegment objCustomerSubSegment { get; set; }
        public virtual Organization objOrganization { get; set; }
        [ForeignKey("CreditSegmentCode, OrganizationCode")]
        public virtual CreditSegment objCreditSegment { get; set; }
        public virtual Product objProduct { get; set; } 
       
    }
}

