using Logistics.Dynamics365.Core.Models;
using Logistics.Dynamics365.Plugins.Ambiente1.Repositório;
using Microsoft.Xrm.Sdk;
using System;

namespace Logistics.Dynamics365.Plugins.Ambiente1.Gerenciadores
{
    public class GerenciadorProdutos
    {
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;
        private readonly IPluginExecutionContext _context;
        private readonly RepositorioProduto _repositorioProduto;

        public GerenciadorProdutos(IOrganizationService service, ITracingService tracingService, IPluginExecutionContext context)
        {
            _service = service;
            _tracingService = tracingService;
            _context = context;
            _repositorioProduto = new RepositorioProduto(service, tracingService, context);
        }

        public void ProcessarSincronização()
        {
            try
            {
                Entity produto = null;
                switch (_context.MessageName.ToLower())
                {
                    case "create":
                    case "update":
                        if (_context.InputParameters.Contains("Target") && _context.InputParameters["Target"] is Entity entity)
                        {
                            produto = entity;
                            _repositorioProduto.SincronizarComAmbiente2(produto);
                            _tracingService.Trace("Sincronização do produto concluída.");
                        }
                        break;
                    case "delete":
                        if (_context.InputParameters.Contains("Target") && _context.InputParameters["Target"] is EntityReference produtoRef)
                        {
                            Guid produtoId = produtoRef.Id;
                            _repositorioProduto.DeletarProdutoNoAmbiente2(produtoId);
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
                throw new InvalidPluginExecutionException($"Ocorreu um erro em SincronizarProdutoPlugin: {ex.Message}", ex);
            }
        }

    }
}
