/*All rights reserved.  No part of this program or publication may be reproduced,
transcribed, stored in a retrieval system, or translated into any language or computer
language, or in any form or by any means, electronic, mechanical, magnetic, optical,
chemical, biological or otherwise, without the prior written permission of :


      GEMINI SOFTWARE SOLUTIONS PRIVATE LTD.,
      NILA, TECHNOPARK CAMPUS, TRIVANDRUM, INDIA.

      - - - - - - - - - - - - - - - - - - - - -
      File Name : ProductDTO.cs
      - - - - - - - - - - - - - - - - - - - - -

      System				:  	CC_Ingenio
      Module				:  	Data Transfer
      Author				:	Bijily
      Date					:	19/03/2014
      Function				:	Data Transfer Object for Product
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
using CC_Ingenio.Common.Utils;
using CC_Ingenio_Resources;


namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class ProductDTO
    {
        [Display(Name = "ProductCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_CHARS, ErrorMessageResourceName = "MsgErrSpecialCharacterNotAllow", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(5, ErrorMessageResourceName = "MsgErrMinimumLengthDigit", ErrorMessageResourceType = typeof(Resources), MinimumLength = 5)]
        public string ProductCode { get; set; }

        [Display(Name = "ProductName", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessageResourceName = "MsgErrMaximumLength", ErrorMessageResourceType = typeof(Resources))]
        public string ProductName { get; set; }

        public string OrganizationCode { get; set; }

        [Display(Name = "ProductCategory", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string ProductCategoryCode { get; set; }

        [Display(Name = "ProductSubCategory", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string ProductSubCategoryCode { get; set; }

        [Display(Name = "ProductClass", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string ProductClassCode { get; set; }

        public string ProductClassIndicator { get; set; }

        [Display(Name = "CardNoPattern", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(5, ErrorMessageResourceName = "MsgErrMaximumLength", ErrorMessageResourceType = typeof(Resources))]
        public string CardNoPattern { get; set; }

        [Display(Name = "CardNoLength", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_NUMERALS, ErrorMessageResourceName = "MsgErrInvalid", ErrorMessageResourceType = typeof(Resources))]
        public decimal CardNoLength { get; set; }

        [Display(Name = "RevolverCharge", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string RevolverChargeFlag { get; set; }

        [Display(Name = "MinimumCreditLine", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [Range(0d, 999999999d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        public decimal MinimumCreditLine { get; set; }

        [Display(Name = "MaximumCreditLine", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [Range(0d, 999999999d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        public decimal MaximumCreditLine { get; set; }

        [Display(Name = "CashLimit", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public decimal CashLimit { get; set; }

        [Display(Name = "Currency", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string CurrencyCode { get; set; }

        [Display(Name = "ProductMemberType", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string ProductMemberTypeCode { get; set; }

        [Display(Name = "BinValue", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_NUMERALS, ErrorMessageResourceName = "MsgErrInvalid", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(6, ErrorMessageResourceName = "MsgErrMinimumLengthDigit", ErrorMessageResourceType = typeof(Resources), MinimumLength = 6)]
        public string BinValue { get; set; }

        [Display(Name = "SubBinValue", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_NUMERALS, ErrorMessageResourceName = "MsgErrInvalid", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(2, ErrorMessageResourceName = "MsgErrMinimumLengthDigit", ErrorMessageResourceType = typeof(Resources), MinimumLength = 2)]
        public string SubBinValue { get; set; }

        [Display(Name = "ProductExpiryMnths", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_NUMERALS, ErrorMessageResourceName = "MsgErrInvalid", ErrorMessageResourceType = typeof(Resources))]
        [Range(1, 100, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        public decimal ProductExpiryInMonths { get; set; }

        [Display(Name = "ProductStatus", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string ProductStatusCode { get; set; }

        [Display(Name = "ServiceCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string ServiceCode { get; set; }

        [Display(Name = "PaymentAllocationType", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string PaymentAllocationType { get; set; }

        [Display(Name = "InterestMethod", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string InterestMethodCode { get; set; }

        public bool CapitalizedInterest { get; set; }

        [Display(Name = "MinimumConsumptionAmount", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [Range(0d, 999999999d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        public decimal MinimumConsumptionAmount { get; set; }

        [Display(Name = "BillingCycle", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public int BillCycleSysParamId { get; set; }

        public string BillingCycleCode { get; set; }

        [Display(Name = "ProductFranchise", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string ProductFranchiseCode { get; set; }

        [Display(Name = "MaximumAddOnCount", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_NUMERALS, ErrorMessageResourceName = "MsgErrInvalid", ErrorMessageResourceType = typeof(Resources))]
        [Range(0, 9)]
        public decimal MaximumAddOnCount { get; set; }

        public bool CanEmbossPhoto { get; set; }

        public bool CanEmbossSignature { get; set; }

        public bool CanEmbossLogo { get; set; }

        public string ImageRefNo { get; set; }

        //[Display(Name = "NumberofAffinity", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_NUMERALS, ErrorMessageResourceName = "MsgErrInvalid", ErrorMessageResourceType = typeof(Resources))]
        public decimal NumberofAffinity { get; set; }

        public bool InterestDiffFlag { get; set; }

        [Display(Name = "InternalScore", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_NUMERALS, ErrorMessageResourceName = "MsgErrInvalid", ErrorMessageResourceType = typeof(Resources))]
        public decimal InternalScore { get; set; }

        [Display(Name = "ExternalScore", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_NUMERALS, ErrorMessageResourceName = "MsgErrInvalid", ErrorMessageResourceType = typeof(Resources))]
        public decimal ExternalScore { get; set; }

        public bool CPVFlag { get; set; }

        [Display(Name = "CombinedScore", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public decimal CombinedScore { get; set; }

        public string ExternalScoreDefinitionId { get; set; }

        [Display(Name = "MinimumCombinedScore", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_NUMERALS, ErrorMessageResourceName = "MsgErrInvalid", ErrorMessageResourceType = typeof(Resources))]
        public decimal MinimumCombinedScore { get; set; }

        [Display(Name = "MinimumCardAdvance", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [Range(0d, 999999999d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        public decimal MinimumCardAdvance { get; set; }

        [Display(Name = "MaximumCardAdvance", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [Range(0d, 999999999d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        public decimal MaximumCardAdvance { get; set; }

        [Display(Name = "InternalScoreDefinitionId", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string InternalScoreDefinitionId { get; set; }

        public bool ScoreFlag { get; set; }

        public bool RenewalScoreFlag { get; set; }

        //[Display(Name = "CPPLine", ResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_ALLOWED_NUMERALS, ErrorMessageResourceName = "MsgErrInvalid", ErrorMessageResourceType = typeof(Resources))]
        public decimal CPPLine { get; set; }

        public bool VerificationNeeded { get; set; }

        public bool IsPreviewNeeded { get; set; }

        public bool MasterVerificationNeeded { get; set; }

        public bool DeDuplicationOption { get; set; }

        public bool BackgroundImageEmbossFlag { get; set; }

        public bool InterestWaiverFlag { get; set; }

        public bool AutoApprovalFlag { get; set; }

        [Display(Name = "Contract", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string ContractCode { get; set; }

        public List<String> PaymentAllocationList { get; set; }
        public List<String> AssignedList { get; set; }

        public List<String> MemberTypeList { get; set; }
        public List<String> MemberTypeAssignedList { get; set; }
        public string LegacyProductCode { get; set; }

 
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovalStatusDesc { get; set; }


        public string CurrencyDesc { get; set; }
        public string BillingCycleDesc { get; set; }
        public string DefaultAffinityDesc { get; set; }
        public string InterestMethodDesc { get; set; }
        public string ProductClassDesc { get; set; }
        public string ProductMemberTypeDesc { get; set; }
        public string ProductStatusDesc { get; set; }
        public string ProductCategoryDesc { get; set; }
        public string ProductSubCategoryDesc { get; set; }
        public string ServiceDesc { get; set; }
        
    }
}
