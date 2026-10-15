/*All rights reserved.  No part of this program or publication may be reproduced,
transcribed, stored in a retrieval system, or translated into any language or computer
language, or in any form or by any means, electronic, mechanical, magnetic, optical,
chemical, biological or otherwise, without the prior written permission of :


      GEMINI SOFTWARE SOLUTIONS PRIVATE LTD.,
      NILA, TECHNOPARK CAMPUS, TRIVANDRUM, INDIA.

      - - - - - - - - - - - - - - - - - - - - -
      File Name : FinancialAccountDTO.cs
      - - - - - - - - - - - - - - - - - - - - -

      System				:  	CC_Ingenio
      Module				:  	Data Transfer
      Author				:	Bijily
      Date					:	03/04/2014
      Function				:	Data Transfer Object for Financial Account
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
using CC_Ingenio_Resources;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class FinancialAccountDTO
    {
        [Required]
        public int FinancialAccountId { get; set; }

        [Display(Name = "Branch", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(5, ErrorMessageResourceName = "MsgErrValueShouldBeLessThan", ErrorMessageResourceType = typeof(Resources))]
        public string BranchCode { get; set; }

        [Display(Name = "ModuleName", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessageResourceName = "MsgErrValueShouldBeLessThan", ErrorMessageResourceType = typeof(Resources))]
        public string ModuleName { get; set; }

        [Display(Name = "Product", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string ProductCode { get; set; }

        [Display(Name = "FinancialActivity", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public int TransactionSubCodeId { get; set; }

        [Display(Name = "DebitAccountNumber", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessageResourceName = "MsgErrValueShouldBeLessThan", ErrorMessageResourceType = typeof(Resources))]
        public string DebitAccountNr { get; set; }

        [Display(Name = "CreditAccountNumber", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessageResourceName = "MsgErrValueShouldBeLessThan", ErrorMessageResourceType = typeof(Resources))]
        public string CreditAccountNr { get; set; }

        public bool IsDebitConsolidated { get; set; }
        public bool IsCreditConsolidated { get; set; }
        //[Required(ErrorMessage = "Please enter Debit Account Code"), StringLength(25, ErrorMessage = "Debit Account Code should be less than 25 characters")]
        public string DebitAccountCode { get; set; }
        //[Required(ErrorMessage = "Please enter Credit Account Code"), StringLength(25, ErrorMessage = "Credit Account Code should be less than 25 characters")]
        public string CreditAccountCode { get; set; }
        //[Required(ErrorMessage = "Please enter Debit SubTransaction Code"), StringLength(5, ErrorMessage = "Debit SubTransaction Code should be less than 5 characters")]
        public string DebitSubTransactionCode { get; set; }
        //[Required(ErrorMessage = "Please enter Credit SubTransaction Code"), StringLength(5, ErrorMessage = "Credit SubTransaction Code should be less than 5 characters")]
        public string CreditSubTransactionCode { get; set; }

        [Display(Name = "DebitNarration", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessageResourceName = "MsgErrValueShouldBeLessThan", ErrorMessageResourceType = typeof(Resources))]
        public string DebitNarration { get; set; }       

        [Display(Name = "CreditNarration", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessageResourceName = "MsgErrValueShouldBeLessThan", ErrorMessageResourceType = typeof(Resources))]
        public string CreditNarration { get; set; }

        public DateTime? EffectiveDate { get; set; }
        public string OrganizationCode { get; set; }
        [Display(Name = "PortFolio", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string Portfolio { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatusDesc { get; set; }

        public string BranchDesc { get; set; }
        public string ProductName { get; set; }
        public string FinancialActivityDesc { get; set; }
    }
}
