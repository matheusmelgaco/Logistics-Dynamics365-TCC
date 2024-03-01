using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Rest;
using System;

namespace Logistics.Dynamics365.Core.Models
{
    public static class Dynamics1Connections
    {
        public static ServiceClient GetEnvironment1()
        {

            
                string url = "https://org51c55b59.crm2.dynamics.com/";
                string clientId = "218513c1-0226-4a1d-90f0-fa0399eb59ad";
                string clientSecret = "Ywa8Q~kIr3NS8wESPxbdDDAOJREGzxOGWX240cl-";
                string tenantId = "9878ef17-e218-4ce5-991f-4fc691b49ea7";

                string connectionString = $"AuthType=ClientSecret; " +
                                          $"Url={url}; " +
                                          $"ClientId={clientId}; " +
                                          $"ClientSecret={clientSecret}; " +
                                          $"TenantId={tenantId}";

                var serviceClient = new ServiceClient(connectionString);
                return serviceClient;
            }

        }
    }
