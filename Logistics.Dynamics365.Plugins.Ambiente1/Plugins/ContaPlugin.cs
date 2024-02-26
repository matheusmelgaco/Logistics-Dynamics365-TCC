using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logistics.Dynamics365.Plugins.Ambiente1.Gerenciadores;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Logistics.Dynamics365.Plugins.Ambiente1
{
    public class ContaPlugin : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            Entity conta = (Entity)context.InputParameters["Target"];

            GerenciadorConta gerenciadorConta = new GerenciadorConta(service, tracingService, context);

            try
            {
                gerenciadorConta.ValidarDuplicidade(conta);
            }
            catch (Exception ex)
            {
                tracingService.Trace("Erro: {0}", ex.ToString());
                throw new InvalidPluginExecutionException("Ocorreu um erro em ContaPlugin.", ex);
            }


        }
    }


}

