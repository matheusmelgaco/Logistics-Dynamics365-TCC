using Logistics.Dynamics365.Core.Models;
using Logistics.Dynamics365.Plugins.Ambiente1.Repositório;
using Microsoft.Xrm.Sdk;
using System;

namespace Logistics.Dynamics365.Plugins.Ambiente1.Gerenciadores
{
    public class GerenciadorOportunidade
    {
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;
        private readonly IPluginExecutionContext _context;
        private readonly RepositorioOportunidade _repositorioOportunidade;

        public GerenciadorOportunidade(IOrganizationService service, ITracingService tracingService, IPluginExecutionContext context)
        {
            _service = service;
            _tracingService = tracingService;
            _context = context;
            _repositorioOportunidade = new RepositorioOportunidade(service, tracingService, context);
        }

        public void ProcessarIntegracao()
        {
            try
            {
                switch (_context.MessageName.ToLower())
                {
                    case "create":
                    case "update":
                        if (_context.InputParameters.Contains("Target") && _context.InputParameters["Target"] is Entity entity)
                        {

                            if (_context.Depth > 1)
                            {
                                _tracingService.Trace("Ignorando execução subsequente no mesmo contexto.");
                                return;
                            }
                            if (DeveGerarIdentificadorUnico(entity))
                            {
                                var identificadorUnico = _repositorioOportunidade.GerarIdentificadorUnico();
                                entity["lgs_opportunitynumber"] = identificadorUnico; 
                                _tracingService.Trace($"Identificador único gerado: {identificadorUnico}.");
                                _service.Update(entity);
                            }

                            _repositorioOportunidade.IntegrarComAmbiente2(entity);
                            _tracingService.Trace("Sincronização da oportunidade concluída.");
                        }
                        break;
                    case "delete":
                        if (_context.InputParameters.Contains("Target") && _context.InputParameters["Target"] is EntityReference oportunidadeRef)
                        {
                            Guid oportunidadeId = oportunidadeRef.Id;
                            _repositorioOportunidade.DeletarOportunidadeAmbiente2(oportunidadeId);
                            _tracingService.Trace("Produto deletado no Ambiente 2.");
                        }
                        break;
                    default:
                        _tracingService.Trace("Operação não suportada.");
                        break;
                }
            }
            catch (Exception ex)
            {
                _tracingService.Trace($"Erro: {ex.Message}");
                throw new InvalidPluginExecutionException($"Ocorreu um erro em SincronizarOportunidadePlugin: {ex.Message}", ex);
            }
        }

        private bool DeveGerarIdentificadorUnico(Entity oportunidade)
        {
            if(_context.MessageName.ToLower() == "create" && !oportunidade.Contains("lgs_opportunitynumber"))
            {
                return true;
            }   
            return false;
          
        }
    }
}
