using Logistics.Dynamics365.Plugins.Ambiente1.Gerenciadores;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Dynamics365.Plugins.Ambiente1.Plugins
{
    public class ContatoPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));


            GerenciadorContato gerenciadorContato = new GerenciadorContato(service, tracingService, context);

            try
            {
                gerenciadorContato.ValidarDuplicidadeCPF();
            }
            catch (InvalidPluginExecutionException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                tracingService.Trace("ContaPlugin Exception: {0}", ex.ToString());
                throw new InvalidPluginExecutionException($"Ocorreu um erro em ContaPlugin: {ex.Message}", ex);
            }
        }
    }
}