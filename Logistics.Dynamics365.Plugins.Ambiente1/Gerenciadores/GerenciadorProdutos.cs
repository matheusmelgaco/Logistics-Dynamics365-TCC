using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Dynamics365.Plugins.Ambiente1.Gerenciadores
{
    public class GerenciadorProdutos
    {
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;
        private readonly IPluginExecutionContext _context;

        public GerenciadorProdutos(IOrganizationService service, ITracingService tracingService, IPluginExecutionContext context)
        {
            _service = service;
            _tracingService = tracingService;
            _context = context;

        }

        public void SincronizarProduto(Entity produto)
        {
            // Lógica de sincronização
        }
    }
}
