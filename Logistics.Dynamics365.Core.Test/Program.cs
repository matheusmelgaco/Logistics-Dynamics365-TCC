using System;
using Logistics.Dynamics365.Core.Models;

namespace Logistics.Dynamics365.Core.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var service = Connections.GetEnvironment2();
                if (service.IsReady)
                {
                    Console.WriteLine("Conexão estabelecida com sucesso!");
                    // Aqui você pode fazer operações adicionais usando o serviço CRM, se necessário.
                }
                else
                {
                    Console.WriteLine("Falha ao estabelecer a conexão com o Dynamics.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
            }

            Console.ReadLine(); // Manter a aplicação aberta para visualização do resultado
        }
    }
}
