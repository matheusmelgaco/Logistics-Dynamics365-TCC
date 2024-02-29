using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Linq;

namespace Logistics.Dynamics365.Core.Models
{
    public static class Dynamics1Connections
    {
        public static CrmServiceClient GetEnvironment1()
        {
            // Aqui, você usaria as configurações diretamente, já que não temos um serviço IOrganizationService pré-existente.
            // Supondo que você tenha as informações de URL, usuário e senha disponíveis para se conectar ao Dynamics 365.
            string url = "https://org51c55b59.crm2.dynamics.com/"; // Substitua pela URL real do seu Dynamics 365
            string user = "GlobalLogistics@LogisticsTCC.onmicrosoft.com"; // Substitua pelo usuário
            string password = "Equipe4tcc@"; // Substitua pela senha

            CrmServiceClient crmServiceClient = new CrmServiceClient($"AuthType=Office365; Url={url}; Username={user}; Password={password}");
            return crmServiceClient;
        }
    }
}
