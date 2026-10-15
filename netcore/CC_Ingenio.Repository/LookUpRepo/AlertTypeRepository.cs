using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.Core.LookUpRepo;
using CC_Ingenio.Data;

namespace CC_Ingenio.Repository.LookUpRepo
{
    public class AlertTypeRepository: GenericRepository<AlertType>,IAlertTypeRepository
    {
        public AlertTypeRepository(UnitOfWork AUnitOfWork)
            : base(AUnitOfWork) 
        {

        }
    }
}
