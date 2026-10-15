/*All rights reserved.  No part of this program or publication may be reproduced,
transcribed, stored in a retrieval system, or translated into any language or computer
language, or in any form or by any means, electronic, mechanical, magnetic, optical,
chemical, biological or otherwise, without the prior written permission of :


      GEMINI SOFTWARE SOLUTIONS PRIVATE LTD.,
      NILA, TECHNOPARK CAMPUS, TRIVANDRUM, INDIA.

      - - - - - - - - - - - - - - - - - - - - -
      File Name : StatementGrouping.cs
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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using CC_Ingenio.Core.LookUpRepo;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_StatementGrouping")]
    public class StatementGrouping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatementGroupingId { get; set; }
        //[Key,Column(Order=1)]
        //[ForeignKey("Product")]
        public string ProductCode { get; set; }
        //[Key, Column(Order = 2)]
        //[ForeignKey("Network")]
        public string NetworkCode { get; set; }
        //[Key, Column(Order = 3)]
        //[ForeignKey("Transaction")]
        public string TransactionCode { get; set; }
        public string GroupCode { get; set; }
        public string GroupCodeDescription { get; set; }
        public string SubGroupCode { get; set; }
        public string SubGroupCodeDescription { get; set; }
        //[ForeignKey("Product") , Column (Order = 0)]
        public string OrganizationCode { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public virtual Product Product { get; set; }
        public virtual Network Network { get; set; }
        public virtual Transaction Transaction { get; set; }
    }
}
