using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace Logistics.Dynamics365.Plugins.Ambiente1.Repositório
{
    public static class RepositorioContato
    {
        public static bool VerificarDuplicidadeCPF(IOrganizationService service, string cpf, Guid recordId)
        {
            QueryExpression query = new QueryExpression("contact");
            query.ColumnSet = new ColumnSet("lgs_cpf");
            query.Criteria = new FilterExpression();
            query.Criteria.AddCondition("lgs_cpf", ConditionOperator.Equal, cpf);

            if (recordId != Guid.Empty)
            {
                query.Criteria.AddCondition("contactid", ConditionOperator.NotEqual, recordId);
            }

            EntityCollection results = service.RetrieveMultiple(query);

            return results.Entities.Count > 0;
        }
    }
}
