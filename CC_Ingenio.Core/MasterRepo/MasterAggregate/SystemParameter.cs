/*All rights reserved.  No part of this program or publication may be reproduced,
transcribed, stored in a retrieval system, or translated into any language or computer
language, or in any form or by any means, electronic, mechanical, magnetic, optical,
chemical, biological or otherwise, without the prior written permission of :


      GEMINI SOFTWARE SOLUTIONS PRIVATE LTD.,
      NILA, TECHNOPARK CAMPUS, TRIVANDRUM, INDIA.

      - - - - - - - - - - - - - - - - - - - - -
      File Name : 
      - - - - - - - - - - - - - - - - - - - - -

      System				:  	CC_Ingenio
      Module				:  	
      Author				:	
      Date					:	
      Function				:	
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

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
   
        [Table("Mt_SystemParameter")]
        public class SystemParameter
        {

            #region Model

            [Key]
            [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
            public int ParameterId { get; set; }
            public string ParameterName { get; set; }
            public string OrganizationCode { get; set; }
            public string ParameterValue { get; set; }
            public string Description { get; set; }
            public string FunctionName { get; set; }
            public string DefaultValue { get; set; }
            public string LowValue { get; set; }
            public string HighValue { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public string ApprovedBy { get; set; }
            public DateTime? ApprovedOn { get; set; }
            public string ApprovalStatus { get; set; }
            public string LastUpdatedBy { get; set; }
            public DateTime LastUpdatedOn { get; set; }
            public string QueueReferenceNo { get; set; }
          

            #endregion
        }

    
}
