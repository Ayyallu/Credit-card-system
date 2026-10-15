using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Data.Contracts
{
    public interface ISql
    {
        IEnumerable<TEntity> ExecuteQuery<TEntity>(string ASqlQuery, params object[] AParameters) where TEntity : class; 
        int ExecuteCommand(string ASqlCommand, params object[] AParameters);

    }
}
