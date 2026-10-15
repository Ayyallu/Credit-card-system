/*All rights reserved.  No part of this program or publication may be reproduced,
transcribed, stored in a retrieval system, or translated into any language or computer
language, or in any form or by any means, electronic, mechanical, magnetic, optical,
chemical, biological or otherwise, without the prior written permission of :


      GEMINI SOFTWARE SOLUTIONS PRIVATE LTD.,
      NILA, TECHNOPARK CAMPUS, TRIVANDRUM, INDIA.

      - - - - - - - - - - - - - - - - - - - - -
      File Name : OrganizationDTO.cs
      - - - - - - - - - - - - - - - - - - - - -

      System				:  	CC_Ingenio
      Module				:  	Data Transfer
      Author				:	Bijily
      Date					:	19/03/2014
      Function				:	Data Transfer Object for Organization
      Modification History	:   
     

      Label			Date		Name		Change description
      - - - - -	   - - - - 		- - - - - 	- - - - - - - - - - - - - - - - 

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CC_Ingenio.Common.Utils;
using CC_Ingenio_Resources;
namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class OrganizationDTO
    {
        [Display(Name = "OrganizationCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_CHARS, ErrorMessageResourceName = "MsgErrSpecialCharacterNotAllow", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(5, ErrorMessageResourceName = "MsgErrMaximumLength", ErrorMessageResourceType = typeof(Resources))]
        public string OrganizationCode { get; set; }

        [Display(Name = "BusinessName", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessageResourceName = "MsgErrMaximumLength", ErrorMessageResourceType = typeof(Resources))]
        public string OrganizationDescription { get; set; }

        [Display(Name = "EffectiveFromDate", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public DateTime EffectiveFromDate { get; set; }

        [Display(Name = "EffectiveToDate", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public DateTime EffectiveToDate { get; set; }

        [Display(Name = "ShortName", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(10, ErrorMessageResourceName = "MsgErrMaximumLength", ErrorMessageResourceType = typeof(Resources))]
        public string OrganizationShortName { get; set; }

        //[Required(ErrorMessage = "Please select Logo"), StringLength(50, ErrorMessage = "File name should be less than 50 characters")]
        public string OrganizationLogo { get; set; }
        public string OrganizationTypeCode { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatusDesc { get; set; }
    }
}
