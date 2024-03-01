using Microsoft.Xrm.Sdk;
using System;

namespace Logistics.Dynamics365.Plugins.Ambiente2.Plugins
{
    public class OportunidadePluginAmbiente2 : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity oportunidade)
            {
                string messageName = context.MessageName.ToLower();
                if ((messageName == "create" || messageName == "delete") && oportunidade.LogicalName == "opportunity")
                {
                    try
                    {
                        if (messageName == "create" && (!oportunidade.Contains("lgs_integrada") || !(bool)oportunidade["lgs_integrada"]))
                        {
                            throw new InvalidPluginExecutionException("A criação direta de oportunidades não é permitida no Ambiente 2.");
                        }
                        else if (messageName == "delete")
                        {
                            throw new InvalidPluginExecutionException("A exclusão de oportunidades não é permitida no Ambiente 2.");
                        }
                    }
                    catch (Exception ex)
                    {
                        tracingService.Trace("Erro ao bloquear operação de oportunidade: {0}", ex.ToString());
                        throw;
                    }
                }
            }
        }
    }
}
