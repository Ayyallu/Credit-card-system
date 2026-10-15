using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC_Ingenio.Common.Utils;
using CC_Ingenio.DataTranfer.UserInfo;

namespace CC_Ingenio.Web.ViewModel.User
{
    public class UserGroupSearch
    {
        //public int? Page { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public List<UserDTO> Users { get; set; }
        public IQueryable<UserGroupDTO> SearchResults { get; set; }

        public string SearchButton { get; set; }
        public string ApprovalReasonCode { get; set; }
        public string ApprovalRemarks { get; set; }
        public ApprovalStatus ApprovalType { get; set; }

    }
}