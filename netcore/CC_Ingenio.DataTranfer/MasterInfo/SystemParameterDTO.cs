/*All rights reserved.  No part of this program or publication may be reproduced,
transcribed, stored in a retrieval system, or translated into any language or computer
language, or in any form or by any means, electronic, mechanical, magnetic, optical,
chemical, biological or otherwise, without the prior written permission of :


      GEMINI SOFTWARE SOLUTIONS PRIVATE LTD.,
      NILA, TECHNOPARK CAMPUS, TRIVANDRUM, INDIA.

      - - - - - - - - - - - - - - - - - - - - -
      File Name : SystemParameterDTO.cs
      - - - - - - - - - - - - - - - - - - - - -

      System				:  	CC_Ingenio
      Module				:  	SystemParameterDTO
      Author				:	Amal
      Date					:	12/03/2014
      Function				:	Data Transfer Object for SystemParameter
      Modification History	:   
     

      Label			Date		Name		Change description
      - - - - -	   - - - - 		- - - - - 	- - - - - - - - - - - - - - - - 
 * 
 * 
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.Common.Utils;
using CC_Ingenio_Resources;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class SystemParameterDTO
    {
        [Required]
        public int ParameterId { get; set; }
        [Display(Name = "ParameterName", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessageResourceName = "MsgErrMaximumLength", ErrorMessageResourceType = typeof(Resources))]
        public string ParameterName { get; set; }

        [Display(Name = "ParameterValue", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        //[Display(Name="Parameter Value")]
        //[Required(ErrorMessage = "Please enter Parameter Value"), Range(1, 99)]
        //[RegularExpression(@"^(([0-9]{1}\d{0,2},(\d{3},)*\d{3})|([0-9]{1}\d{0,}))$", ErrorMessage = "Invalid Value")]
        public string ParameterValue { get; set; }

        public string OrganizationCode { get; set; }

        [Display(Name = "Meaning", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string Description { get; set; }

        //[Display(Name="Default Value")]
        //[RegularExpression(@"^(([0-9]{1}\d{0,2},(\d{3},)*\d{3})|([0-9]{1}\d{0,}))$", ErrorMessage = "Invalid Default Value")]
        public string DefaultValue { get; set; }
        public string Function { get; set; }
        //[Display(Name = "Low Value")]
        //[RegularExpression(@"^(([0-9]{1}\d{0,2},(\d{3},)*\d{3})|([0-9]{1}\d{0,}))$", ErrorMessage = "Invalid Low Value")]
        public string LowValue { get; set; }
        //[Display(Name = "High Value")]
        //[RegularExpression(@"^(([0-9]{1}\d{0,2},(\d{3},)*\d{3})|([0-9]{1}\d{0,}))$", ErrorMessage = "Invalid High Value")]
        public string HighValue { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatusDesc { get; set; }
        public string QueueReferenceNo { get; set; }

    }
}
