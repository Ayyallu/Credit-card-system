/*All rights reserved.  No part of this program or publication may be reproduced,
transcribed, stored in a retrieval system, or translated into any language or computer
language, or in any form or by any means, electronic, mechanical, magnetic, optical,
chemical, biological or otherwise, without the prior written permission of :


      GEMINI SOFTWARE SOLUTIONS PRIVATE LTD.,
      NILA, TECHNOPARK CAMPUS, TRIVANDRUM, INDIA.

      - - - - - - - - - - - - - - - - - - - - -
      File Name : InterestMasterDTO.cs
      - - - - - - - - - - - - - - - - - - - - -

      System				:  	CC_Ingenio
      Module				:  	Data Transfer
      Author				:	Bijily
      Date					:	04/04/2014
      Function				:	Data Transfer Object for Interest Master
      Modification History	:   
     

      Label			Date		Name		Change description
      - - - - -	   - - - - 		- - - - - 	- - - - - - - - - - - - - - - - 
 * 
 * 
*/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio_Resources;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class InterestMasterDTO
    {
        [Required]
        public int InterestMasterId { get; set; }

        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "ProductCode", ResourceType = typeof(Resources))]
        public string ProductCode { get; set; }

        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "CustomerSegment", ResourceType = typeof(Resources))]     
        public string CustomerSegmentCode { get; set; }

        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "CustomerSubSegment", ResourceType = typeof(Resources))] 
        public string CustomerSubSegmentCode { get; set; }

        public DateTime EffectiveDate { get; set; }
        public string MaxInterestDefinitionFor { get; set; }
        public string OrganizationCode { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatusDesc { get; set; }

        public string ProductName { get; set; }
        public string CustomerSegmentDesc { get; set; }
        public string CustomerSubSegmentDesc { get; set; }

        public List<InterestMasterDetailsDTO> InterestMasterDetailsDTO { get; set; }

        public InterestMasterDTO()
        {
            InterestMasterDetailsDTO = new List<InterestMasterDetailsDTO>();
        }

    }
}
