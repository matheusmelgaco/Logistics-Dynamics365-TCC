using Microsoft.Xrm.Sdk;
using System;

namespace Logistics.Dynamics365.Plugins.Ambiente2.Plugins
{
    public class BloquearCriaçãoProdutoPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity produto)
            {
                string messageName = context.MessageName.ToLower();
                if ((messageName == "create" || messageName == "delete") && produto.LogicalName == "product")
                {
                    try
                    {
                        if (messageName == "create" && (!produto.Contains("lgs_permitir_criacao") || !(bool)produto["lgs_permitir_criacao"]))
                        {
                            throw new InvalidPluginExecutionException("A criação direta de produtos não é permitida no Ambiente 2.");
                        }
                        else if (messageName == "delete")
                        {
                            throw new InvalidPluginExecutionException("A exclusão de produtos não é permitida no Ambiente 2.");
                        }
                    }
                    catch (Exception ex)
                    {
                        tracingService.Trace("Erro ao bloquear operação de produto: {0}", ex.ToString());
                        throw;
                    }
                }
            }
        }
    }
}
