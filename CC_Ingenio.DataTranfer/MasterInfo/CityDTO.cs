/*All rights reserved.  No part of this program or publication may be reproduced,
transcribed, stored in a retrieval system, or translated into any language or computer
language, or in any form or by any means, electronic, mechanical, magnetic, optical,
chemical, biological or otherwise, without the prior written permission of :


      GEMINI SOFTWARE SOLUTIONS PRIVATE LTD.,
      NILA, TECHNOPARK CAMPUS, TRIVANDRUM, INDIA.

      - - - - - - - - - - - - - - - - - - - - -
      File Name : CityDTO.cs
      - - - - - - - - - - - - - - - - - - - - -

      System				:  	CC_Ingenio
      Module				:  	Data Transfer
      Author				:	Bijily
      Date					:	17/03/2014
      Function				:	Data Transfer Object for City
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
    public class CityDTO
    {
        
        [Display(Name = "CityCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_CHARS, ErrorMessageResourceName = "MsgErrSpecialCharacterNotAllow", ErrorMessageResourceType = typeof(Resources))]
        public string CityCode { get; set; }

        [Display(Name = "CityName", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessageResourceName = "MsgErrMaximumLength", ErrorMessageResourceType = typeof(Resources))]
        public string CityName { get; set; }
        public string OrganizationCode { get; set; }
        public string CountryCode { get; set; }
        public string RegionCode { get; set; }
        public string CityCategoryCode { get; set; }
        public string HubCode { get; set; }
        public string SpokeCode { get; set; }
        public string CommunaCode { get; set; }
        public string CommunaName { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatusDesc { get; set; }

        public string CountryDesc { get; set; }
        public string RegionDesc { get; set; }
        public string CityCategoryDesc { get; set; }
        public string HubDesc { get; set; }
        public string SpokeDesc { get; set; }
        public string CommunaDesc { get; set; }
    }

    public class CityNewDTO
    {
        public string CountryCode { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string CommunaCode { get; set; }
        public string CommunaName { get; set; }
        public string RegionCode { get; set; }
    }
}
