using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Linq;

namespace Logistics.Dynamics365.Core.Models
{
    public static class Connections
    {
        public static CrmServiceClient GetEnvironment2(IOrganizationService service)
        {
            string entityName = "lgs_configuracao_dynamics";
            string urlField = "lgs_url";
            string userField = "lgs_user";
            string passwordField = "lgs_password";

            QueryExpression query = new QueryExpression(entityName)
            {
                ColumnSet = new ColumnSet(new string[] { urlField, userField, passwordField })
            };
         
            EntityCollection results = service.RetrieveMultiple(query);
            if (results.Entities.Count == 0)
            {
                throw new InvalidOperationException("Configurações de conexão com o Dynamics não encontradas.");
            }

            Entity config = results.Entities.First();
            string url = config.GetAttributeValue<string>(urlField);
            string user = config.GetAttributeValue<string>(userField);
            string password = config.GetAttributeValue<string>(passwordField);

            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
            {
                throw new InvalidOperationException("Configurações de conexão com o Dynamics estão incompletas.");
            }

            CrmServiceClient crmServiceClient = new CrmServiceClient($"AuthType=Office365; Url={url}; Username={user}; Password={password}");
            return crmServiceClient;
        }
    }
}
