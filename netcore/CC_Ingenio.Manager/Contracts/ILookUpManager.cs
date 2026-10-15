using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace CC_Ingenio.Manager.Contracts
{
    public interface ILookUpManager
    {
        List<SelectListItem> GetAll<TEntity>(string ASelected, string AContext, bool ASelectOneNeeded = true) where TEntity : class;
        

    }
}
