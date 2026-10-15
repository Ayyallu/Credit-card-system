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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class SystemParameterFactory
    {
        public static SystemParameter CreateSystemParameter(int AParameterId, string AParameterName, string AParameterValue, string ADescription,
            string AFunction, string ADefaultValue, string ALowValue, string AHighValue, string ACreatedBy, string ALastUpdatedBy, 
            string AApprovalStatus,string AOrganizationCode, string AApprovedBy = null)
        {
            SystemParameter objSystemParameter = new SystemParameter()
            {
                ParameterId = AParameterId,
                OrganizationCode = AOrganizationCode,
                ParameterName = AParameterName,
                ParameterValue = AParameterValue,
                Description = ADescription,
                FunctionName = AFunction,
                DefaultValue = ADefaultValue,
                LowValue = ALowValue,
                HighValue = AHighValue,
                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus

            };


            return objSystemParameter;

        }
    }
}
