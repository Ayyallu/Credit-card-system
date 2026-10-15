using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class CollectionChargeSearch
    {
        public int? Page { get; set; }

        public string ProductCode { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime EffectiveDate { get; set; }
        
        public IQueryable<CollectionChargeDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }
    }
}