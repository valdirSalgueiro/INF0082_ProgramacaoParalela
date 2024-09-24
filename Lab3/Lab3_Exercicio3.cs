using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PPCourse
{
    public class Lab3_Exercicio3
    {

        public static async Task Run()
        {
            string[] entrada = Console.ReadLine().Split();
            int P = int.Parse(entrada[0]); 
            int C = int.Parse(entrada[1]); 

            var pedidos = new List<(string prato, int tempo)>();

            for (int i = 0; i < P; i++)
            {
                string[] pedido = Console.ReadLine().Split(',');
                string prato = pedido[0].Trim();
                int tempo = int.Parse(pedido[1].Trim());
                pedidos.Add((prato, tempo));
            }

            var semaforo = new SemaphoreSlim(C);

            var tarefas = new List<Task>();
            foreach (var pedido in pedidos)
            {
                tarefas.Add(PrepararPedido(semaforo, pedido.prato, pedido.tempo));
            }

            await Task.WhenAll(tarefas);
            Console.WriteLine("Todos os pedidos foram preparados!");
        }

        static async Task PrepararPedido(SemaphoreSlim semaforo, string prato, int tempo)
        {
            await semaforo.WaitAsync();

            try
            {
                Console.WriteLine($"O prato '{prato}' começou a ser preparado.");
                await Task.Delay(tempo * 1000);
                Console.WriteLine($"O prato '{prato}' está pronto.");
            }
            finally
            {
                semaforo.Release();
            }
        }
    }
}
