using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class TestFactory
    {

        public static Test  CreateTest(string ACode, string AName,                       
                       string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AApprovedBy = null)
        {
            Test objTest = new Test()
            {
               
                Code = ACode,
                Name = AName,
                
                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                LastUpdatedOn = DateTime.Now,
                CreatedOn = DateTime.Now,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };

            return objTest;
        }
    }
}
