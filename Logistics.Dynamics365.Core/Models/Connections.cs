using Microsoft.Xrm.Tooling.Connector;

namespace Logistics.Dynamics365.Core.Models
{
    public class Connections
    {
        public static CrmServiceClient GetEnviroment2()
        {

            var user = "GlobalLogistics@LogisticsTCC.onmicrosoft.com";
            var password = "Equipe4tcc@";
            var url = "https://org81f34f5c.crm2.dynamics.com/";

            CrmServiceClient crmServiceClient = new CrmServiceClient("AuthType=Office365; Url=" + url + "; Username=" + user + "; Password=" + password);

            return crmServiceClient;
        }
    }
}
