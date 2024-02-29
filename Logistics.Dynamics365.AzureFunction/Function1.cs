using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Logistics.Dynamics365.Core.Models;
using Microsoft.Xrm.Sdk;
using System.Threading.Tasks;

namespace Logistics.Dynamics365.AzureFunctions
{
    public class AccountData
    {
        public string Operation { get; set; }
        public string EntityName { get; set; } = "account";
        public Guid? EntityId { get; set; }
        public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();
    }


    public class Function1
    {
        private readonly ILogger _logger;

        // The constructor has been updated to use dependency injection for the ILogger
        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
        }
        [Function("Function1")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestData req,
            FunctionContext executionContext)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonSerializer.Deserialize<AccountData>(requestBody);

            var serviceClient = Dynamics1Connections.GetEnvironment1(); 

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            if (serviceClient != null && serviceClient.IsReady)
            {
                try
                {
                    Guid recordId;
                    Entity entity = new Entity(data.EntityName);

                    switch (data.Operation.ToLower())
                    {
                        case "create":
                        case "update":
                            foreach (var attribute in data.Attributes)
                            {
                                entity[attribute.Key] = attribute.Value;
                            }
                            if (data.Operation.ToLower() == "create")
                            {
                                recordId = serviceClient.Create(entity);
                                _logger.LogInformation($"Account created successfully. ID: {recordId}");
                            }
                            else
                            {
                                if (data.EntityId.HasValue)
                                {
                                    entity.Id = data.EntityId.Value;
                                    serviceClient.Update(entity);
                                    _logger.LogInformation($"Account updated successfully. ID: {entity.Id}");
                                }
                                else
                                {
                                    throw new InvalidOperationException("Entity ID is required for update operations.");
                                }
                            }
                            break;

                        case "delete":
                            if (data.EntityId.HasValue)
                            {
                                serviceClient.Delete(data.EntityName, data.EntityId.Value);
                                _logger.LogInformation($"Account deleted successfully. ID: {data.EntityId.Value}");
                            }
                            else
                            {
                                throw new InvalidOperationException("Entity ID is required for delete operations.");
                            }
                            break;

                        default:
                            throw new InvalidOperationException("Unsupported operation.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error processing operation '{data.Operation}': {ex.Message}");
                    response = req.CreateResponse(HttpStatusCode.InternalServerError);
                    await response.WriteStringAsync($"An error occurred: {ex.Message}");
                    return response;
                }
            }
            else
            {
                _logger.LogError("Failed to connect to Dynamics 365.");
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteStringAsync("Failed to connect to Dynamics 365.");
                return response;
            }

            await response.WriteStringAsync($"Operation '{data.Operation}' performed successfully on Dynamics 365!");
            return response;
        }

    }
}
