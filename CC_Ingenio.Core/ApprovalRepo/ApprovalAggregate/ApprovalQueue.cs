/*All rights reserved.  No part of this program or publication may be reproduced,
transcribed, stored in a retrieval system, or translated into any language or computer
language, or in any form or by any means, electronic, mechanical, magnetic, optical,
chemical, biological or otherwise, without the prior written permission of :


      GEMINI SOFTWARE SOLUTIONS PRIVATE LTD.,
      NILA, TECHNOPARK CAMPUS, TRIVANDRUM, INDIA.

      - - - - - - - - - - - - - - - - - - - - -
      File Name : ApprovalQueue.cs
      - - - - - - - - - - - - - - - - - - - - -

      System				:  	CC_Ingenio
      Module				:  	model - Approval Queue
      Author				:	Amal    
      Date					:	11.03.2014
      Function				:	model for approval queue
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

using CC_Ingenio.Core.ApprovalRepo.ApprovalContextAggregate;
using CC_Ingenio.Core.LookUpRepo;

namespace CC_Ingenio.Core.ApprovalRepo.ApprovalAggregate
{
    [Table("CT_ApprovalQueue")]
    public class ApprovalQueue
    {
        [Key]
        public int RequestId { get; set; }
        [ForeignKey("ApprovalContext")]
        public string ContextCode { get; set; }
        public string ApprovalDescription { get; set; }
        public string OrganizationCode { get; set; }
        public string RequestedBy { get; set; }
        public DateTime RequestedOn { get; set; }
        public string ReturnURL { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }

       
        public virtual ApprovalQueueContext ApprovalContext { get; set; }

        /*public ApprovalQueue()
        {
            this.ApprovalContext = new ApprovalReasonContext();
        }*/


    }
}
