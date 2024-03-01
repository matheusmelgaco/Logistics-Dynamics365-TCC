using System;
using Microsoft.Xrm.Sdk;
using Logistics.Dynamics365.Plugins.Ambiente1.Gerenciadores;

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

                GerenciadorConta gerenciadorConta = new GerenciadorConta(service, tracingService, context);

                try
                {
                    gerenciadorConta.ValidarDuplicidade();
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


