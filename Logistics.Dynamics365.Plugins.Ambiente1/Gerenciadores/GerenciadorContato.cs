using Microsoft.Xrm.Sdk;
using Logistics.Dynamics365.Plugins.Ambiente1.Repositório;
using System;

namespace Logistics.Dynamics365.Plugins.Ambiente1.Gerenciadores
{
    public class GerenciadorContato
    {
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;
        private readonly IPluginExecutionContext _context;

        public GerenciadorContato(IOrganizationService service, ITracingService tracingService, IPluginExecutionContext context)
        {
            _service = service;
            _tracingService = tracingService;
            _context = context;
        }

        public void ValidarDuplicidadeCPF()
        {
            if (_context.InputParameters.Contains("Target") && _context.InputParameters["Target"] is Entity)
            {
                Entity entidade = (Entity)_context.InputParameters["Target"];
                if (entidade.LogicalName != "contact") return;

                string cpf = entidade.Attributes.Contains("lgs_cpf") ? entidade.Attributes["lgs_cpf"].ToString() : string.Empty;

                if (!string.IsNullOrEmpty(cpf))
                {
                    Guid recordId = entidade.Id; 
                    bool existe = RepositorioContato.VerificarDuplicidadeCPF(_service, cpf, recordId);

                    if (existe)
                    {
                        throw new InvalidPluginExecutionException("Um registro com este CPF já existe no sistema.");
                    }
                }
            }
        }
    }
}
