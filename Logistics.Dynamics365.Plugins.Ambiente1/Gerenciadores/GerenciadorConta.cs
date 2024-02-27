using Microsoft.Xrm.Sdk;
using Logistics.Dynamics365.Plugins.Ambiente1.Repositório;
using System;
using System.Linq;

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

        public void ValidarDuplicidade()
        {
            if (_context.InputParameters.Contains("Target") && _context.InputParameters["Target"] is Entity)
            {
                Entity entidade = (Entity)_context.InputParameters["Target"];
                if (entidade.LogicalName != "account") return;

                string cnpj = entidade.Attributes.Contains("lgs_cnpj") ? entidade.Attributes["lgs_cnpj"].ToString() : string.Empty;

                if (!string.IsNullOrEmpty(cnpj))
                {
                    Guid recordId = entidade.Id;
                    bool existe = RepositorioConta.VerificarDuplicidadeCNPJ(_service, cnpj, recordId);

                    if (existe)
                    {
                        throw new InvalidPluginExecutionException($"CNPJ {cnpj} já cadastrado. Verifique e tente novamente.");
                    }
                }
            }
        }
    }
}


