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
      Module				:  	Billing Cycle Parameter model
      Author				:	Bijily
      Date					:	03/04/2014
      Function				:	Model for Billing Cycle Parameter
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
    [Table("MT_BillingCycleParameter")]
    public class BillingCycleParameter
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int BillingCycleParameterId { get; set; }
        public int BillMonth { get; set; }
        public int BillYear { get; set; }
        [ForeignKey("BillingCycle")]
        public int BillCycleSysParamId { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime StatementDate { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public string OrganizationCode { get; set; }
        public string BillingCycleCode { get; set; }
        public DateTime? BillFrom { get; set; }
        public DateTime? BillTo { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public virtual SystemParameter BillingCycle { get; set; }
    }
}
