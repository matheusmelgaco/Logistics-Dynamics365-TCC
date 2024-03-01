using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Linq;
using Logistics.Dynamics365.Core.Models;
using Microsoft.Xrm.Sdk.Query;
using System.Text;

namespace Logistics.Dynamics365.Plugins.Ambiente1.Repositório
{
    public class RepositorioOportunidade
    {
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;
        private readonly IPluginExecutionContext _context;
        private CrmServiceClient _crmServiceClient;

        public RepositorioOportunidade(IOrganizationService service, ITracingService tracingService, IPluginExecutionContext context)
        {
            _service = service;
            _tracingService = tracingService;
            _context = context;
            tracingService.Trace("Iniciando a inicialização do RepositorioOportunidade.");

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
        public string GerarIdentificadorUnico()
        {
            string prefixo = "OPP-";
            int numeroSequencial = GerarNumeroSequencial();
            string sufixoAmbienteEAlfanumerico = "A1" + GerarSufixoAlfanumerico();

            string identificador = $"{prefixo}{numeroSequencial}-{sufixoAmbienteEAlfanumerico}";

            while (VerificarIdentificadorExiste(identificador))
            {
                numeroSequencial = GerarNumeroSequencial();
                sufixoAmbienteEAlfanumerico = "A1" + GerarSufixoAlfanumerico();
                identificador = $"{prefixo}{numeroSequencial}-{sufixoAmbienteEAlfanumerico}";
            }

            return identificador;
        }



        private bool VerificarIdentificadorExiste(string identificador)
        {
            QueryExpression query = new QueryExpression("opportunity")
            {
                ColumnSet = new ColumnSet("lgs_opportunitynumber"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("lgs_opportunitynumber", ConditionOperator.Equal, identificador)
                    }
                }
            };

            var result = _service.RetrieveMultiple(query);
            return result.Entities.Any();
        }

        private int GerarNumeroSequencial()
        {
            string entityName = "lgs_sequencialcounter";
            string fieldName = "lgs_value";

            QueryExpression query = new QueryExpression(entityName)
            {
                ColumnSet = new ColumnSet(fieldName)
            };
            EntityCollection results = _service.RetrieveMultiple(query);

            if (results.Entities.Count > 0)
            {
                Entity counterEntity = results.Entities[0];
                int currentValue = counterEntity.GetAttributeValue<int>(fieldName);

                int newValue = currentValue + 1;
                counterEntity[fieldName] = newValue;

                _service.Update(counterEntity);

                return newValue;
            }
            else
            {
                throw new InvalidPluginExecutionException("Registro do contador sequencial não encontrado.");
            }
        }


        private string GerarSufixoAlfanumerico()
        {
            var random = new Random();
            var letras = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var numeros = "0123456789";

            var sufixo = new StringBuilder();
            sufixo.Append(letras[random.Next(letras.Length)]); 
            sufixo.Append(numeros[random.Next(numeros.Length)]);

            return sufixo.ToString();
        }


        public void IntegrarComAmbiente2(Entity oportunidade)
        {
            try
            {
                Entity opportunityToSync = new Entity("opportunity");

                if (oportunidade.Contains("parentcontactid"))
                {
                    _tracingService.Trace("Iniciando a verificação ou criação do contato no Ambiente 2.");

                    EntityReference contatoRef = oportunidade.GetAttributeValue<EntityReference>("parentcontactid");

                    Entity contato = _service.Retrieve(contatoRef.LogicalName, contatoRef.Id, new ColumnSet("lgs_cpf", "firstname", "lastname"));
                   
                    string cpf = contato.Contains("lgs_cpf") ? contato.GetAttributeValue<string>("lgs_cpf") : string.Empty;
                    string contactName = contato.Contains("firstname") ? contato.GetAttributeValue<string>("firstname") : string.Empty;
                    string contactLastName = contato.Contains("lastname") ? contato.GetAttributeValue<string>("lastname") : string.Empty;
                    
                    if (string.IsNullOrWhiteSpace(cpf))
                    {
                        _tracingService.Trace("CPF não encontrado para o contato no Ambiente 1. Não é possível verificar ou criar no Ambiente 2.");
                        throw new InvalidPluginExecutionException("CPF é obrigatório para verificar ou criar o contato no Ambiente 2.");
                    }
                    _tracingService.Trace($"Tentando encontrar ou criar contato no Ambiente 2 com CPF: {cpf} e nome: {contactName}");

                   var parentContactId = VerificarOuCriarContatoAmbiente2(_crmServiceClient, cpf, contactName,contactLastName, contatoRef.Id);
                    _tracingService.Trace($"Contato verificado ou criado com sucesso no Ambiente 2. ID: {parentContactId}");
                    opportunityToSync["parentcontactid"] = new EntityReference("contact", parentContactId);

                }
                else
                {
                    if (opportunityToSync.Contains("parentcontactid"))
                    {
                        opportunityToSync.Attributes.Remove("parentcontactid");
                    }
                }

                if (oportunidade.Contains("parentaccountid"))
                {
                    _tracingService.Trace("Iniciando a verificação ou criação da conta no Ambiente 2.");

                    EntityReference contaRef = oportunidade.GetAttributeValue<EntityReference>("parentaccountid");
                    Entity conta = _service.Retrieve(contaRef.LogicalName, contaRef.Id, new ColumnSet("lgs_cnpj", "name"));
                    string cnpj = conta.Contains("lgs_cnpj") ? conta.GetAttributeValue<string>("lgs_cnpj") : string.Empty;
                    string accountName = conta.Contains("name") ? conta.GetAttributeValue<string>("name") : string.Empty;

                    if (string.IsNullOrWhiteSpace(cnpj))
                    {
                        _tracingService.Trace("CNPJ não encontrado para a conta no Ambiente 1. Não é possível verificar ou criar no Ambiente 2.");
                        throw new InvalidPluginExecutionException("CNPJ é obrigatório para verificar ou criar a conta no Ambiente 2.");
                    }

                    _tracingService.Trace($"Tentando encontrar ou criar conta no Ambiente 2 com CNPJ: {cnpj} e nome: {accountName}");

                    var parentAccountId = VerificarOuCriarContaAmbiente2(_crmServiceClient, cnpj, accountName, contaRef.Id);

                    _tracingService.Trace($"Conta verificada ou criada com sucesso no Ambiente 2. ID: {parentAccountId}");

                    opportunityToSync["parentaccountid"] = new EntityReference("account", parentAccountId);
                }
                else
                {
                    // Se a referência da conta foi removida no Ambiente 1, verifique e remova no Ambiente 2
                    if (opportunityToSync.Contains("parentaccountid"))
                    {
                        opportunityToSync.Attributes.Remove("parentaccountid");
                    }
                }


                if (!oportunidade.Contains("lgs_opportunitynumber"))
                {
                    if (_context.MessageName.ToLower() == "update")
                    {
                        Entity existingOpportunity = _service.Retrieve(
                            oportunidade.LogicalName,
                            oportunidade.Id,
                            new ColumnSet("lgs_opportunitynumber")
                        );

                        if (existingOpportunity != null && existingOpportunity.Contains("lgs_opportunitynumber"))
                        {
                            oportunidade["lgs_opportunitynumber"] = existingOpportunity.GetAttributeValue<string>("lgs_opportunitynumber");
                        }
                        else
                        {
                            _tracingService.Trace("O campo 'lgs_opportunitynumber' não foi encontrado no registro existente.");
                            throw new InvalidPluginExecutionException("O campo 'ID da Oportunidade' é obrigatório e não foi encontrado no registro existente.");
                        }
                    }
                    else
                    {
                        throw new InvalidPluginExecutionException("O campo 'ID da Oportunidade' é obrigatório e não foi encontrado.");
                    }
                }
          
                

                opportunityToSync["lgs_opportunitynumber"] = oportunidade.GetAttributeValue<string>("lgs_opportunitynumber");
                opportunityToSync["lgs_id_oportunidade_ambiente1"] = oportunidade.Id.ToString();
               
                opportunityToSync["lgs_integrada"] = true;


                UpdateAttributeIfPresent(oportunidade, opportunityToSync, "name");
                UpdateAttributeIfPresent(oportunidade, opportunityToSync, "lgs_opportunitynumber");
                UpdateAttributeIfPresent(oportunidade, opportunityToSync, "budgetamount");
                UpdateAttributeIfPresent(oportunidade, opportunityToSync, "transactioncurrencyid");
                UpdateAttributeIfPresent(oportunidade, opportunityToSync, "purchasetimeframe");
                UpdateAttributeIfPresent(oportunidade, opportunityToSync, "purchaseprocess");
                UpdateAttributeIfPresent(oportunidade, opportunityToSync, "msdyn_forecastcategory");
                UpdateAttributeIfPresent(oportunidade, opportunityToSync, "description");
                UpdateAttributeIfPresent(oportunidade, opportunityToSync, "currentsituation");
                UpdateAttributeIfPresent(oportunidade, opportunityToSync, "customerneed");
                UpdateAttributeIfPresent(oportunidade, opportunityToSync, "proposedsolution");
                UpdateAttributeIfPresent(oportunidade, opportunityToSync, "estimatedvalue");
                UpdateAttributeIfPresent(oportunidade, opportunityToSync, "pricelevelid");
                UpdateAttributeIfPresent(oportunidade, opportunityToSync, "isrevenuesystemcalculated");
                UpdateAttributeIfPresent(oportunidade, opportunityToSync, "totallineitemamount");
                UpdateAttributeIfPresent(oportunidade, opportunityToSync, "discountpercentage");
                UpdateAttributeIfPresent(oportunidade, opportunityToSync, "discountamount");
                UpdateAttributeIfPresent(oportunidade, opportunityToSync, "totalamountlessfreight");
                UpdateAttributeIfPresent(oportunidade, opportunityToSync, "freightamount");
                UpdateAttributeIfPresent(oportunidade, opportunityToSync, "totaltax");
                UpdateAttributeIfPresent(oportunidade, opportunityToSync, "totalamount");               


                QueryExpression query = new QueryExpression("opportunity")
                {
                    ColumnSet = new ColumnSet("lgs_opportunitynumber"),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                {
                    new ConditionExpression("lgs_opportunitynumber", ConditionOperator.Equal, opportunityToSync["lgs_opportunitynumber"])
                }
                    }
                };

                var existingOpportunities = _crmServiceClient.RetrieveMultiple(query);

                if (existingOpportunities.Entities.Any())
                {
                    var existingOpportunity = existingOpportunities.Entities.First();
                    opportunityToSync.Id = existingOpportunity.Id;
                    _crmServiceClient.Update(opportunityToSync);
                }
                else
                {
                    _crmServiceClient.Create(opportunityToSync);
                }
            }
            catch (Exception ex)
            {
                _tracingService.Trace("Erro ao sincronizar oportunidade com Ambiente 2: {0}", ex.ToString());
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
        public void DeletarOportunidadeAmbiente2(Guid oportunidadeId)
        {
            try
            {
                QueryExpression query = new QueryExpression("opportunity")
                {
                    ColumnSet = new ColumnSet("opportunityid"),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                {
                    new ConditionExpression("lgs_id_oportunidade_ambiente1", ConditionOperator.Equal, oportunidadeId.ToString())
                }
                    }
                };

                var existingProducts = _crmServiceClient.RetrieveMultiple(query);

                if (existingProducts.Entities.Any())
                {
                    var produtoParaDeletarId = existingProducts.Entities.First().Id;
                    _crmServiceClient.Delete("opportunity", produtoParaDeletarId);
                    _tracingService.Trace($"Tentativa de deleção: Produto com ID {oportunidadeId} encontrado no Ambiente 2 e será deletado.");
                }
                else
                {
                    _tracingService.Trace($"Falha na deleção: Produto com ID {oportunidadeId} não encontrado no Ambiente 2.");
                    return;
                }
            }
            catch (Exception ex)
            {
                _tracingService.Trace($"Erro ao tentar deletar Oportunidade no Ambiente 2: {ex.ToString()}");
                throw;
            }
        }

        public Guid VerificarOuCriarContatoAmbiente2(CrmServiceClient service, string cpf, string contactName,string contactLastName, Guid contactIdAmbiente1)
        {
            QueryExpression query = new QueryExpression("contact")
            {
                ColumnSet = new ColumnSet("contactid", "lgs_cpf", "lgs_id_contato_ambiente1"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                new ConditionExpression("lgs_cpf", ConditionOperator.Equal, cpf),
                new ConditionExpression("lgs_id_contato_ambiente1", ConditionOperator.Equal, contactIdAmbiente1.ToString())
            }
                }
            };

            EntityCollection result = service.RetrieveMultiple(query);

            if (result.Entities.Count > 0)
            {
                return result.Entities[0].Id;
            }
            else
            {
                Entity newContact = new Entity("contact");
                newContact["firstname"] = contactName;
                newContact["lastname"] = contactLastName;
                newContact["lgs_cpf"] = cpf;
                newContact["lgs_id_contato_ambiente1"] = contactIdAmbiente1.ToString();

                return service.Create(newContact);
            }
        }
        public Guid VerificarOuCriarContaAmbiente2(CrmServiceClient service, string cnpj, string accountName, Guid accountIdAmbiente1)
        {
            QueryExpression query = new QueryExpression("account")
            {
                ColumnSet = new ColumnSet("accountid", "lgs_cnpj", "lgs_id_conta_ambiente1"), 
                Criteria = new FilterExpression
                {
                    Conditions =
            {
                new ConditionExpression("lgs_cnpj", ConditionOperator.Equal, cnpj),
                new ConditionExpression("lgs_id_conta_ambiente1", ConditionOperator.Equal, accountIdAmbiente1.ToString())
            }
                }
            };

            EntityCollection result = service.RetrieveMultiple(query);

            if (result.Entities.Count > 0)
            {
                return result.Entities[0].Id;
            }
            else
            {
                Entity newAccount = new Entity("account");
                newAccount["name"] = accountName; 
                newAccount["lgs_cnpj"] = cnpj; 
                newAccount["lgs_id_conta_ambiente1"] = accountIdAmbiente1.ToString(); 

                return service.Create(newAccount);
            }
        }







    }



}
