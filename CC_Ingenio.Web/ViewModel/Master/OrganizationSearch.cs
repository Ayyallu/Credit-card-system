using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Common.Utils;
using System.ComponentModel.DataAnnotations;

namespace CC_Ingenio.Web.ViewModel.Master
{
    public class OrganizationSearch
    {

        public int? Page { get; set; }

        public string OrganizationCode { get; set; }

        public string OrganizationDescription { get; set; }

        public DateTime EffectiveFromDate { get; set; }

        public DateTime EffectiveToDate { get; set; }

        public IQueryable<OrganizationDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }

        public string ApprovalReasonCode { get; set; }

        public string ApprovalRemarks { get; set; }

        public ApprovalStatus ApprovalType { get; set; }

    }
}