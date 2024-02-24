using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Linq;
using Logistics.Dynamics365.Core.Models;
using Microsoft.Xrm.Sdk.Query;

namespace Logistics.Dynamics365.Plugins.Ambiente1.Repositório
{
    public class RepositorioProduto
    {
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;
        private readonly IPluginExecutionContext _context;
        private CrmServiceClient _crmServiceClient;

        public RepositorioProduto(IOrganizationService service, ITracingService tracingService, IPluginExecutionContext context)
        {
            _service = service;
            _tracingService = tracingService;
            _context = context;
            tracingService.Trace("Iniciando a inicialização do RepositorioProduto.");

            try
            {
                _crmServiceClient = Connections.GetEnvironment2(service);
                tracingService.Trace("Conexão com o Ambiente 2 estabelecida com sucesso.");
            }
            catch (Exception ex)
            {
                tracingService.Trace("Erro ao inicializar o repositório de produtos: {0}", ex.ToString());
                tracingService.Trace("Detalhes do contexto: Depth={0}, Message={1}, OperationId={2}",
                                     context.Depth, context.MessageName, context.OperationId);
                throw;
            }
        }

        public void SincronizarComAmbiente2(Entity produto)
        {
            try
            {
                if (!produto.Contains("productnumber"))
                {
                    if (produto.Id != Guid.Empty)
                    {
                        Entity retrievedProduct = _service.Retrieve(produto.LogicalName, produto.Id, new ColumnSet("productnumber"));
                        produto["productnumber"] = retrievedProduct.GetAttributeValue<string>("productnumber");
                    }

                    if (!produto.Contains("productnumber"))
                    {
                        throw new InvalidPluginExecutionException("O campo 'productnumber' é obrigatório e não foi encontrado.");
                    }
                }

                Entity productToSync = new Entity("product");

                productToSync["productnumber"] = produto.GetAttributeValue<string>("productnumber");
                productToSync["lgs_idprodutoambiente1"] = produto.Id.ToString();
                productToSync["lgs_permitir_criacao"] = true;

                UpdateAttributeIfPresent(produto, productToSync, "name");
                UpdateAttributeIfPresent(produto, productToSync, "defaultuomscheduleid", true);
                UpdateAttributeIfPresent(produto, productToSync, "defaultuomid", true);
                UpdateAttributeIfPresent(produto, productToSync, "quantitydecimal");
                UpdateAttributeIfPresent(produto, productToSync, "description");
                UpdateAttributeIfPresent(produto, productToSync, "validfromdate");
                UpdateAttributeIfPresent(produto, productToSync, "validtodate");
                UpdateAttributeIfPresent(produto, productToSync, "pricelevelid", true);
                UpdateAttributeIfPresent(produto, productToSync, "subjectid", true);

                QueryExpression existingProductQuery = new QueryExpression("product")
                {
                    ColumnSet = new ColumnSet("productid"),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                {
                    new ConditionExpression("productnumber", ConditionOperator.Equal, productToSync["productnumber"])
                }
                    }
                };

                var existingProducts = _crmServiceClient.RetrieveMultiple(existingProductQuery);

                if (existingProducts.Entities.Any())
                {
                    var existingProduct = existingProducts.Entities.First();
                    productToSync.Id = existingProduct.Id;
                    _crmServiceClient.Update(productToSync);
                }
                else
                {
                    _crmServiceClient.Create(productToSync);
                }
            }
            catch (Exception ex)
            {
                _tracingService.Trace("Erro ao sincronizar produto com Ambiente 2: {0}", ex.ToString());
                throw;
            }
        }

        private void UpdateAttributeIfPresent(Entity source, Entity target, string attributeName, bool isEntityReference = false)
        {
            if (source.Contains(attributeName))
            {
                if (isEntityReference)
                {
                    target[attributeName] = source.GetAttributeValue<EntityReference>(attributeName);
                }
                else
                {
                    target[attributeName] = source[attributeName];
                }
            }
        }
        public void DeletarProdutoNoAmbiente2(Guid produtoId)
        {
            try
            {
                QueryExpression query = new QueryExpression("product")
                {
                    ColumnSet = new ColumnSet("productid"),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                {
                    new ConditionExpression("lgs_idprodutoambiente1", ConditionOperator.Equal, produtoId.ToString())
                }
                    }
                };

                var existingProducts = _crmServiceClient.RetrieveMultiple(query);

                if (existingProducts.Entities.Any())
                {
                    var produtoParaDeletarId = existingProducts.Entities.First().Id;
                    _crmServiceClient.Delete("product", produtoParaDeletarId);
                    _tracingService.Trace($"Tentativa de deleção: Produto com ID {produtoId} encontrado no Ambiente 2 e será deletado.");
                }
                else
                {
                    _tracingService.Trace($"Falha na deleção: Produto com ID {produtoId} não encontrado no Ambiente 2.");
                    return; 
                }
            }
            catch (Exception ex)
            {
                _tracingService.Trace($"Erro ao tentar deletar produto no Ambiente 2: {ex.ToString()}");
                throw;
            }
        }

    }



}
