using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC_Ingenio.DataTranfer.ApprovalQueueInfo;
using System.ComponentModel.DataAnnotations;

namespace CC_Ingenio.Web.ViewModel.ApprovalQueue
{
    public class ApprovalQueueSearch
    {
        public int? Page { get; set; }

        public int RequestId { get; set; }
        public string ContextCode { get; set; }
        public string RequestedBy { get; set; }
        public DateTime RequestedOn { get; set; }
        public DateTime RequestedOnTo { get; set; }

        public IQueryable<ApprovalQueueDTO> SearchResults { get; set; }
        public int RecordCount { get; set; }
        public int CurrentPageNumber { get; set; }
        public string SearchButton { get; set; }
 
    }
}




       