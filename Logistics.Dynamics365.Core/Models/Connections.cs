using Microsoft.Xrm.Tooling.Connector;
using System;
using System.IO;
using System.Text.Json;

namespace Logistics.Dynamics365.Core.Models
{
    public class DynamicsConnectionConfig
    {
        public string Url { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
    public class Connections
    {
        private static DynamicsConnectionConfig LoadDynamicsConfig()
        {
            string fileName = "credentials.json";
            string jsonString = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<DynamicsConnectionConfig>(jsonString);
        }

        public static CrmServiceClient GetEnvironment2()
        {
            var config = LoadDynamicsConfig();
            if (config == null)
            {
                throw new InvalidOperationException("Falha ao carregar a configuração do Dynamics.");
            }

            CrmServiceClient crmServiceClient = new CrmServiceClient($"AuthType=Office365; Url={config.Url}; Username={config.User}; Password={config.Password}");

            return crmServiceClient;
        }
    }
}
