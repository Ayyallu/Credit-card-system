/*All rights reserved.  No part of this program or publication may be reproduced,
transcribed, stored in a retrieval system, or translated into any language or computer
language, or in any form or by any means, electronic, mechanical, magnetic, optical,
chemical, biological or otherwise, without the prior written permission of :


      GEMINI SOFTWARE SOLUTIONS PRIVATE LTD.,
      NILA, TECHNOPARK CAMPUS, TRIVANDRUM, INDIA.

      - - - - - - - - - - - - - - - - - - - - -
      File Name : SBIFInterestDTO.cs
      - - - - - - - - - - - - - - - - - - - - -

      System				:  	CC_Ingenio
      Module				:  	Data Transfer
      Author				:	Bijily
      Date					:	03/04/2014
      Function				:	Data Transfer Object for SBIF Interest
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
using CC_Ingenio.Common.Utils;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class SBIFInterestDTO
    {
        [Required]
        public int SBIFInterestId { get; set; }
        public string CustomerSubSegmentCode { get; set; }

        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "EffectiveDate", ResourceType = typeof(Resources))]
        public DateTime EffectiveDate { get; set; }

        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "InterestDefinition", ResourceType = typeof(Resources))]      
        public string MaxInterestDefinitionFor { get; set; }
      

        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "CreditLimitFrom", ResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_NUMERALS, ErrorMessageResourceName = "MsgErrInvalid", ErrorMessageResourceType = typeof(Resources))]
        public decimal CreditLimitFrom { get; set; }


        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "CreditLimitTo", ResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_NUMERALS, ErrorMessageResourceName = "MsgErrInvalid", ErrorMessageResourceType = typeof(Resources))]
        public decimal CreditLimitTo { get; set; }

        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "TenureFrom", ResourceType = typeof(Resources))]
        public int TenureFromMonths { get; set; }

        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "TenureTo", ResourceType = typeof(Resources))]
        public int TenureToMonths { get; set; }

        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources)),Range(0,100)]
        [Display(Name = "InterestRatePercentage", ResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_NUMERALS, ErrorMessageResourceName = "MsgErrInvalid", ErrorMessageResourceType = typeof(Resources))]
        public decimal InterestRatePercent { get; set; }

        public string OrganizationCode { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatusDesc { get; set; }

        public string MaxInterestDefinitionForDesc { get; set; }
        public string TenureFromMonthsDesc { get; set; }
        public string TenureToMonthsDesc { get; set; }

        public List<SBIFInterestDetailsDTO> SBIFInterestDetailsDTO { get; set; }

        public SBIFInterestDTO()
        {
            SBIFInterestDetailsDTO = new List<SBIFInterestDetailsDTO>();
        }

    }
}
