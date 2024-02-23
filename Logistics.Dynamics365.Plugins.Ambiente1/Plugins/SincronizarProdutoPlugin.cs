using Logistics.Dynamics365.Plugins.Ambiente1.Gerenciadores;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Dynamics365.Plugins.Ambiente1.Plugins
{
    public class SincronizarProdutoPlugin : IPlugin

    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            Entity produto = (Entity)context.InputParameters["Target"];

            GerenciadorProdutos gerenciadorProdutos = new GerenciadorProdutos(service, tracingService, context);

            try
            {
                gerenciadorProdutos.SincronizarProduto(produto);
            }
            catch (Exception ex)
            {
                tracingService.Trace("Erro: {0}", ex.ToString());
                throw new InvalidPluginExecutionException("Ocorreu um erro em SincronizarProdutoPlugin.", ex);
            }
        }
    }
}
