/*All rights reserved.  No part of this program or publication may be reproduced,
transcribed, stored in a retrieval system, or translated into any language or computer
language, or in any form or by any means, electronic, mechanical, magnetic, optical,
chemical, biological or otherwise, without the prior written permission of :


      GEMINI SOFTWARE SOLUTIONS PRIVATE LTD.,
      NILA, TECHNOPARK CAMPUS, TRIVANDRUM, INDIA.

      - - - - - - - - - - - - - - - - - - - - -
      File Name : StatementGroupingFactory.cs
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
    public static class StatementGroupingFactory
    {
        public static StatementGrouping CreateStatementGrouping(int AStatementGroupingId, string AProductCode, string ANetworkCode,
                string ATransactionCode, string AGroupCode, string AGroupCodeDescription, string ASubGroupCode, string ASubGroupCodeDescription,
                string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AOrganizationCode, string AApprovedBy = null) 
        {
            StatementGrouping objStatementGrouping = new StatementGrouping() 
            {
                StatementGroupingId = AStatementGroupingId,
                ProductCode = AProductCode,
                NetworkCode = ANetworkCode,
                TransactionCode = ATransactionCode,
                GroupCode = AGroupCode,
                GroupCodeDescription = AGroupCodeDescription,
                SubGroupCode = ASubGroupCode,
                SubGroupCodeDescription = ASubGroupCodeDescription,
                OrganizationCode = AOrganizationCode,

                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };
            return objStatementGrouping;
        }
    }
}
