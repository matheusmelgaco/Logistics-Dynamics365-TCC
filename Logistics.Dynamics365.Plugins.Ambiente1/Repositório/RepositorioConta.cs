using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace Logistics.Dynamics365.Plugins.Ambiente1.Repositório
{
    public static class RepositorioConta
    {
        public static bool VerificarDuplicidadeCNPJ(IOrganizationService service, string cnpj, Guid recordId)
        {
            QueryExpression query = new QueryExpression("account");
            query.ColumnSet = new ColumnSet("lgs_cnpj");
            query.Criteria = new FilterExpression();
            query.Criteria.AddCondition("lgs_cnpj", ConditionOperator.Equal, cnpj);

            // Excluir o registro atual do resultado para atualizações
            if (recordId != Guid.Empty)
            {
                query.Criteria.AddCondition("accountid", ConditionOperator.NotEqual, recordId);
            }

            EntityCollection results = service.RetrieveMultiple(query);

            return results.Entities.Count > 0;
        }
    }
}
