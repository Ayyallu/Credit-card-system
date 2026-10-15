/*All rights reserved.  No part of this program or publication may be reproduced,
transcribed, stored in a retrieval system, or translated into any language or computer
language, or in any form or by any means, electronic, mechanical, magnetic, optical,
chemical, biological or otherwise, without the prior written permission of :


      GEMINI SOFTWARE SOLUTIONS PRIVATE LTD.,
      NILA, TECHNOPARK CAMPUS, TRIVANDRUM, INDIA.

      - - - - - - - - - - - - - - - - - - - - -
      File Name : BlockCodeDTO.cs
      - - - - - - - - - - - - - - - - - - - - -

      System				:  	CC_Ingenio
      Module				:  	Data Transfer
      Author				:	Bijily
      Date					:	20/03/2014
      Function				:	Data Transfer Object for Block Code
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
using CC_Ingenio.Common.Utils;
using CC_Ingenio_Resources;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class BlockCodeDTO
    {

        [Display(Name ="BlockCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_CHARS, ErrorMessageResourceName = "MsgErrSpecialCharacterNotAllow", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessageResourceName = "MsgErrMaximumLength", ErrorMessageResourceType = typeof(Resources))]
        public string Code { get; set; }

        public string OrganizationCode { get; set; }

        [Display(Name ="Description", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(100, ErrorMessageResourceName = "MsgErrMaximumLength", ErrorMessageResourceType = typeof(Resources))]
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public bool IsAccountBlocked { get; set; }
        public bool IsAccountBlockedForFinancialReasons { get; set; }
        public bool IsAccountBlockedForNonFinancialReasons { get; set; }
        
        [Display(Name ="FinancialHierarchy", ResourceType = typeof(Resources))]
        //[RegularExpression(@"^[ ]$", ErrorMessage = "White Space is not allowed")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Utils.MSG_ALLOWED_NOSPACE)] 
        public decimal FinancialHierarchy { get; set; }
        [Display(Name ="NonFinancialHierarchy", ResourceType = typeof(Resources))]
        public decimal NonFinancialHierarchy { get; set; }
        public bool IsCardBlocked { get; set; }
        public decimal CardBlockHierarchy { get; set; }
        public bool IsPermanantBlock { get; set; }
        public bool PlasticRecoveryNeeded { get; set; }
        public bool InstallmentClose { get; set; }
        public bool Unblock { get; set; }

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
