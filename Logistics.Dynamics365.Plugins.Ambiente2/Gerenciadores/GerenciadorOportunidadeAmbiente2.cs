using Logistics.Dynamics365.Core.Models;
using Logistics.Dynamics365.Plugins.Ambiente2.Gerenciadores;
using Logistics.Dynamics365.Plugins.Ambiente2.Repositório;
using Microsoft.Xrm.Sdk;
using System;
using System.Runtime.Remoting.Contexts;

namespace Logistics.Dynamics365.Plugins.Ambiente2.Gerenciadores
{
    public class GerenciadorOportunidadeAmbiente2
    {
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;
        private readonly IPluginExecutionContext _context;
        private readonly RepositorioOportunidadeAmbiente2 _repositorioOportunidade;

        public GerenciadorOportunidadeAmbiente2(IOrganizationService service, ITracingService tracingService, IPluginExecutionContext context)
        {
            _service = service;
            _tracingService = tracingService;
            _context = context;
            _repositorioOportunidade = new RepositorioOportunidadeAmbiente2(service, tracingService, context);
        }

        public void GerarID()
        {
            try
            {

                if (_context.Depth > 1)
                {
                    _tracingService.Trace("Ignorando execução subsequente no mesmo contexto.");
                    return;
                }
                if (_context.MessageName.ToLower() == "create" && _context.InputParameters.Contains("Target") && _context.InputParameters["Target"] is Entity entity)
                {
                    if (DeveGerarIdentificadorUnico(entity))
                    {
                        var identificadorUnico = _repositorioOportunidade.GerarIdentificadorUnico();
                        entity["lgs_opportunitynumber"] = identificadorUnico;
                        _tracingService.Trace($"Identificador único gerado: {identificadorUnico}.");
                        _service.Update(entity);
                    }

                }
            }
            catch (Exception ex)
            {
                _tracingService.Trace($"Erro: {ex.Message}");
                throw new InvalidPluginExecutionException($"Ocorreu um erro em OportunidadePlugin no Ambiente 2: {ex.Message}", ex);
            }
        }
        private bool DeveGerarIdentificadorUnico(Entity oportunidade)
        {
            if (_context.MessageName.ToLower() == "create" && !oportunidade.Contains("lgs_opportunitynumber"))
            {
                return true;
            }
            return false;

        }
    }
}
