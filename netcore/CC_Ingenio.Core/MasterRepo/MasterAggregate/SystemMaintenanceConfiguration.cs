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
      Author				:	Smrithy
      Date					:	03-06-2014
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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_SystemMaintenanceConfiguration")]
    public class SystemMaintenanceConfiguration
    {
        [Key]
        public string BaseCurrency { get; set; }
        public int  WeeklyGlobalCount { get; set; }
        public int WeeklySetUp { get; set; }
        public int CashLimit { get; set; }
        public int CreditLimit { get; set; }
        public string DefaultBoost { get; set; }
        public int ReissueDamaged { get; set; }
        public int ClosedDamaged { get; set; }
        public string DefaultMerchantChargeType { get; set; }
        public string DefaultChargeTypeDescrption { get; set; }
        public int MinimumDueMerchantCode { get; set; }
        public int NumberOfSegments { get; set; }
        public string ApprovalStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
    }
}
