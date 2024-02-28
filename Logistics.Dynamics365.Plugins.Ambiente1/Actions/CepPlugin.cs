using Microsoft.Xrm.Sdk;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Text;

namespace Logistics.Dynamics365.Cep
{
    public class CepPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            var cep = context.InputParameters["cep"] as string;
            var url = "https://viacep.com.br/ws/" + cep + "/json/";


            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            var response = client.DownloadString(url);
            JObject jsonResponse = JObject.Parse(response);

            context.OutputParameters["formatCep"] = jsonResponse["cep"].ToString();
            context.OutputParameters["rua"] = jsonResponse["logradouro"].ToString();
            context.OutputParameters["bairro"] = jsonResponse["bairro"].ToString();
            context.OutputParameters["cidade"] = jsonResponse["localidade"].ToString();
            context.OutputParameters["estado"] = jsonResponse["uf"].ToString();
            context.OutputParameters["ibge"] = jsonResponse["ibge"].ToString();
            context.OutputParameters["ddd"] = jsonResponse["ddd"].ToString();

        }
    }
}
