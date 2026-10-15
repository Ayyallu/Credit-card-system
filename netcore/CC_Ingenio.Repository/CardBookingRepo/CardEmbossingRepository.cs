
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CC_Ingenio.Data;
using CC_Ingenio.Core.CardBookingRepo.CardBookingAggregate;

namespace CC_Ingenio.Repository.CardBookingRepo
{
    public class CardEmbossingRepository : GenericRepository<CardEmbossing>, ICardEmbossingRepository
    {
        public CardEmbossingRepository(UnitOfWork AUnitOfWork)
            : base(AUnitOfWork)
        { }
    }
}

