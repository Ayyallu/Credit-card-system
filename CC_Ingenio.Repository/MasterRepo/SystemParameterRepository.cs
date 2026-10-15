using CC_Ingenio.Core.MasterRepo.MasterAggregate;
using CC_Ingenio.Data;
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

namespace CC_Ingenio.Repository.MasterRepo
{
    

    public class SystemParameterRepository : GenericRepository<SystemParameter>, ISystemParameterRepository
    {
        public SystemParameterRepository(UnitOfWork AUnitOfWork)
            : base(AUnitOfWork)
        { }
    }
}
