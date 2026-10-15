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
      Module				:  	Credit SubSegment Parameter model
      Author				:	Smrithy
      Date					:	23/06/2014
      Function				:	Model for CreditSubSegment Parameter
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
using System.Web.Mvc;
using CC_Ingenio.Core.LookUpRepo;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_CreditSubSegment")]
    public class CreditSubSegment
    {
        [Key,Column(Order = 0)]
        [ForeignKey("objCreditSegment")]
        public string CreditSegmentCode { get; set; }
          [Key, Column(Order = 1)]
        [ForeignKey("objCreditSegment")]
        public string OrganizationCode { get; set; }
        [Key, Column(Order = 2)]
        [ForeignKey("objSystemParameter")]
        public int CreditSubSegmentCode { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public virtual CreditSegment objCreditSegment { get; set; }
        public virtual SystemParameter objSystemParameter { get; set; }

    }
}
