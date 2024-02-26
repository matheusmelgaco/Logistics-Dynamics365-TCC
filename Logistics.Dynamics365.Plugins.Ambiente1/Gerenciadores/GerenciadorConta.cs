using Logistics.Dynamics365.Plugins.Ambiente1.Repositório;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;

namespace Logistics.Dynamics365.Plugins.Ambiente1.Gerenciadores
{
    public class GerenciadorConta
    {
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;
        private readonly IPluginExecutionContext _context;

        public GerenciadorConta(IOrganizationService service, ITracingService tracingService, IPluginExecutionContext context)
        {
            _service = service;
            _tracingService = tracingService;
            _context = context;

        }

        public void ValidarDuplicidade(Entity conta)
        {

            if (conta.Contains("lgs_cnpj"))
            {
                string cnpj = conta["lgs_cnpj"].ToString();

                _tracingService.Trace($"CNPJ: {cnpj}");
                var contas = RepositorioConta.BuscarContaPorCNPJ(cnpj, _service);
                _tracingService.Trace($"Contas: {contas.Entities.Count}");
                bool isUpdate = _context.MessageName.ToLower() == "update";
                bool isDuplicate = contas.Entities.Count > 0;

                if (isUpdate && isDuplicate)
                {
                    var targetId = conta.Id;
                    isDuplicate = contas.Entities.Any(e => e.Id != targetId);
                }

                if (isDuplicate)
                {
                    throw new InvalidPluginExecutionException($"CNPJ {cnpj} já cadastrado. Verifique e tente novamente.");
                }

            }
        }
    }
}
