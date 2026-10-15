using CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate;
using CC_Ingenio.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Repository.CustomerServiceRepo
{
    public class CardClosureRepository : GenericRepository<CardClosure>, ICardClosureRepository
    {
        public CardClosureRepository(UnitOfWork AUnitOfWork) : base(AUnitOfWork) { }
    }
}
