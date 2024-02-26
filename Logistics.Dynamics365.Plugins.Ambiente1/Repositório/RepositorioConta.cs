using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Dynamics365.Plugins.Ambiente1.Repositório
{
    public class RepositorioConta
    {
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;
        private readonly IPluginExecutionContext _context;

        public RepositorioConta(IOrganizationService service, ITracingService tracingService, IPluginExecutionContext context)
        {
            _service = service;
            _tracingService = tracingService;
            _context = context;

        }


        public static EntityCollection BuscarContaPorCNPJ(string cnpj, IOrganizationService service)
        {
            QueryExpression query = new QueryExpression("account");
            query.ColumnSet = new ColumnSet("accountid", "name");
            query.Criteria.AddCondition("lgs_cnpj", ConditionOperator.Equal, cnpj);


            return service.RetrieveMultiple(query);
        }

    }
}
