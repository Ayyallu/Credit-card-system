/*All rights reserved.  No part of this program or publication may be reproduced,
transcribed, stored in a retrieval system, or translated into any language or computer
language, or in any form or by any means, electronic, mechanical, magnetic, optical,
chemical, biological or otherwise, without the prior written permission of :


      GEMINI SOFTWARE SOLUTIONS PRIVATE LTD.,
      NILA, TECHNOPARK CAMPUS, TRIVANDRUM, INDIA.

      - - - - - - - - - - - - - - - - - - - - -
      File Name : PlanMaster.cs
      - - - - - - - - - - - - - - - - - - - - -

      System				:  	CC_Ingenio
      Module				:  	PlanMaster model
      Author				:	Smrithy  
      Date					:	16.04.2014
      Function				:	model for PlanMaster master
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

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
     [Table("Mt_PlanMaster")]

    public class PlanMaster
    {
       #region Model

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlanID { get; set; }
        //[ForeignKey("CreditSegment"),Column (Order = 0)]
        public string CreditSegmentCode { get; set; }
        //[ForeignKey("CreditSegment"),Column (Order = 1)]           
        public string OrganizationCode { get; set; }
        //[ForeignKey("Product")]
        public string ProductCode { get; set; }
        //[ForeignKey("CustomerSegment"), Column(Order = 0)]
        public string CustomerSegmentCode { get; set; }
        //[ForeignKey("CustomerSegment"), Column(Order = 0)]
        public string CustomerSubSegmentCode { get; set; }
        //[ForeignKey("Transaction")]
        public string TransactionCode { get; set; }
        public decimal CreditLimitFrom { get; set; }
        public decimal CreditLimitTo { get; set; }
        //public decimal TenureFrom { get; set; }
        public decimal TenureTo { get; set; }
        public DateTime ValidFromDate { get; set; }
        //public DateTime ValidToDate { get; set; }
        public decimal SBIFInterest { get; set; }
        public decimal ExpectedInterestRate { get; set; }
        public decimal DeferredPayment { get; set; }
        public string InterestCalculation { get; set; }
        public string FirstBilling { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatus { get; set; }

        [ForeignKey("CreditSegmentCode, OrganizationCode")]
        public virtual CreditSegment CreditSegment { get; set; }
        public virtual Product Product { get; set; }
        [ForeignKey("CustomerSegmentCode, OrganizationCode")]
        public virtual CustomerSegment CustomerSegment { get; set; }
        [ForeignKey("CustomerSubSegmentCode, OrganizationCode")]
        public virtual CustomerSubSegment CustomerSubSegment { get; set; }
        public virtual Transaction Transaction { get; set; }

       #endregion
         
    }
}
