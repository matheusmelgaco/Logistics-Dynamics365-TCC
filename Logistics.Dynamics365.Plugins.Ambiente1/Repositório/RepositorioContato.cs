using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Dynamics365.Plugins.Ambiente1.Repositório
{
    public static class RepositorioContato
    {
        public static EntityCollection BuscarContatoPorCPF(string cpf, IOrganizationService service)
        {
            QueryExpression query = new QueryExpression("contact");
            query.ColumnSet = new ColumnSet("contactid", "fullname");
            query.Criteria.AddCondition("lgs_cpf", ConditionOperator.Equal, cpf);

            return service.RetrieveMultiple(query);
        }
    }
}
