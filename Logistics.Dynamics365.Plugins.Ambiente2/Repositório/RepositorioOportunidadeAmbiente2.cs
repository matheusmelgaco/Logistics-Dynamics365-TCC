using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Linq;
using Logistics.Dynamics365.Core.Models;
using Microsoft.Xrm.Sdk.Query;
using System.Text;

namespace Logistics.Dynamics365.Plugins.Ambiente2.Repositório
{
    public class RepositorioOportunidadeAmbiente2
    {
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;
        private readonly IPluginExecutionContext _context;

        public RepositorioOportunidadeAmbiente2(IOrganizationService service, ITracingService tracingService, IPluginExecutionContext context)
        {
            _service = service;
            _tracingService = tracingService;
            _context = context;
            tracingService.Trace("Iniciando a inicialização do RepositorioOportunidade.");
        }
        public string GerarIdentificadorUnico()
        {
            string prefixo = "OPP-";
            int numeroSequencial = GerarNumeroSequencial();
            string sufixoAmbienteEAlfanumerico = "A2" + GerarSufixoAlfanumerico(); 

            string identificador = $"{prefixo}{numeroSequencial}-{sufixoAmbienteEAlfanumerico}";

            while (VerificarIdentificadorExiste(identificador))
            {
                numeroSequencial = GerarNumeroSequencial();
                sufixoAmbienteEAlfanumerico = "A2" + GerarSufixoAlfanumerico();
                identificador = $"{prefixo}{numeroSequencial}-{sufixoAmbienteEAlfanumerico}";
            }

            return identificador;
        }


        private bool VerificarIdentificadorExiste(string identificador)
        {
            QueryExpression query = new QueryExpression("opportunity")
            {
                ColumnSet = new ColumnSet("lgs_opportunitynumber"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("lgs_opportunitynumber", ConditionOperator.Equal, identificador)
                    }
                }
            };

            var result = _service.RetrieveMultiple(query);
            return result.Entities.Any();
        }

        private int GerarNumeroSequencial()
        {
            string entityName = "lgs_sequencialcounter";
            string fieldName = "lgs_value";

            QueryExpression query = new QueryExpression(entityName)
            {
                ColumnSet = new ColumnSet(fieldName)
            };
            EntityCollection results = _service.RetrieveMultiple(query);

            if (results.Entities.Count > 0)
            {
                Entity counterEntity = results.Entities[0];
                int currentValue = counterEntity.GetAttributeValue<int>(fieldName);

                int newValue = currentValue + 1;
                counterEntity[fieldName] = newValue;

                _service.Update(counterEntity);

                return newValue;
            }
            else
            {
                throw new InvalidPluginExecutionException("Registro do contador sequencial não encontrado.");
            }
        }


        private string GerarSufixoAlfanumerico()
        {
            var random = new Random();
            var letras = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var numeros = "0123456789";

            var sufixo = new StringBuilder();
            sufixo.Append(letras[random.Next(letras.Length)]); 
            sufixo.Append(numeros[random.Next(numeros.Length)]); 

            return sufixo.ToString();
        }


    }



}
