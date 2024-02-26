using Logistics.Dynamics365.Plugins.Ambiente1.Repositório;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Dynamics365.Plugins.Ambiente1.Gerenciadores
{
    public class GerenciadorContato
    {
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;
        private readonly IPluginExecutionContext _context;

        public GerenciadorContato(IOrganizationService service, ITracingService tracingService, IPluginExecutionContext context)
        {
            _service = service;
            _tracingService = tracingService;
            _context = context;
        }

        public void ValidarDuplicidade(Entity contato)
        {
            if (contato.Contains("lgs_cpf"))
            {
                string cpf = contato["lgs_cpf"].ToString();

                _tracingService.Trace($"CPF: {cpf}");
                var contatos = RepositorioContato.BuscarContatoPorCPF(cpf, _service);
                _tracingService.Trace($"Contatos: {contatos.Entities.Count}");
                bool isUpdate = _context.MessageName.ToLower() == "update";
                bool isDuplicate = contatos.Entities.Count > 0;

                if (isUpdate && isDuplicate)
                {
                    var targetId = contato.Id;
                    isDuplicate = contatos.Entities.Any(e => e.Id != targetId);
                }

                if (isDuplicate)
                {
                    throw new InvalidPluginExecutionException($"CPF {cpf} já cadastrado. Verifique e tente novamente.");
                }
            }
        }
    }
}
