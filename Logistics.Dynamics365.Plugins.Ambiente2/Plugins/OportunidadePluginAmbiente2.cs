using Logistics.Dynamics365.Plugins.Ambiente2.Gerenciadores;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


            GerenciadorOportunidadeAmbiente2 gerenciadorOportunidade = new GerenciadorOportunidadeAmbiente2(service, tracingService, context);

            try
            {
                gerenciadorOportunidade.GerarID();
            }
            catch (Exception ex)
            {
                tracingService.Trace("Erro: {0}", ex.ToString());
                throw new InvalidPluginExecutionException("Ocorreu um erro em ProdutoPlugin.", ex);
            }
        }
    }
}
