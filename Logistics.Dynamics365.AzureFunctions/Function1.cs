using System.Net;
using System.IO;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Tooling.Connector; 
using Logistics.Dynamics365.Core.Models;
using Microsoft.Xrm.Sdk;
using System.Threading.Tasks;
using System; 

namespace Logistics.Dynamics365.AzureFunctions
{

    public class RequestData
    {
        public string Operation { get; set; } 
        public string EntityName { get; set; } 
        public Guid? EntityId { get; set; }
        public dynamic Attributes { get; set; } 
    }
    public class Function1
    {
        private readonly IOrganizationService _service;

        private readonly ILogger _logger;

        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
        }

        [Function("Function1")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonSerializer.Deserialize<RequestData>(requestBody);

            var crmServiceClient = Dynamics1Connections.GetEnvironment1();

            if (crmServiceClient != null && crmServiceClient.IsReady)
            {
                try
                {
                    Guid recordId;
                    Entity entity = new Entity(data.EntityName);

                    switch (data.Operation.ToLower())
                    {
                        case "create":
                            foreach (var attribute in data.Attributes)
                            {
                                entity[attribute.Key] = attribute.Value;
                            }
                            recordId = crmServiceClient.Create(entity);
                            _logger.LogInformation($"Registro criado com sucesso. ID: {recordId}");
                            break;

                        case "update":
                            if (data.EntityId.HasValue)
                            {
                                entity.Id = data.EntityId.Value;
                                foreach (var attribute in data.Attributes)
                                {
                                    entity[attribute.Key] = attribute.Value;
                                }
                                crmServiceClient.Update(entity);
                                _logger.LogInformation($"Registro atualizado com sucesso. ID: {entity.Id}");
                            }
                            else
                            {
                                _logger.LogError("ID da entidade não fornecido para operação de atualização.");
                            }
                            break;

                        case "delete":
                            if (data.EntityId.HasValue)
                            {
                                crmServiceClient.Delete(data.EntityName, data.EntityId.Value);
                                _logger.LogInformation($"Registro deletado com sucesso. ID: {data.EntityId.Value}");
                            }
                            else
                            {
                                _logger.LogError("ID da entidade não fornecido para operação de exclusão.");
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Erro ao processar a operação '{data.Operation}': {ex.Message}");
                }
            }
            else
            {
                _logger.LogError("Falha na conexão com o Dynamics 365.");
            }


            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString("Operação realizada com sucesso no Dynamics 365!");

            return response;
        }

    }
}
