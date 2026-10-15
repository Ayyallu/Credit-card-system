using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Common.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class CreditSegmentSearch
    {
        public int? Page { get; set; }

        public string CardSegmentCode { get; set; }

        public string CardSegmentName { get; set; }

       // [DataType(DataType.Date)]
       // [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
       
        public DateTime EffectiveFromDate { get; set; }

      //  [DataType(DataType.Date)]
       // [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]

        public DateTime EffectiveToDate { get; set; }

        public IQueryable<CreditSegmentDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }

        public bool SearchDeleted { get; set; }

    }
}