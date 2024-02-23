using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logistics.Dynamics365.Gerenciadores;
using Microsoft.Xrm.Sdk;

namespace Logistics.Dynamics365.Plugins
{
    public class SincronizarProdutoPlugin : IPlugin

    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            GerenciadorProdutos gerenciadorProduto = new GerenciadorProdutos(service, tracingService, context);    

            try { 
            
            
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException("Ocorreu um erro em SincronizarProdutoPlugin.", ex);
            }
        }
        }
    }

