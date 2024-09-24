using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PPCourse
{
    public class Lab3_Exercicio4
    {
        public static async Task Run()
        {
            string[] entrada = Console.ReadLine().Split();
            int N = int.Parse(entrada[0]); 
            int V = int.Parse(entrada[1]); 
            int E = int.Parse(entrada[2]); 

            var filaDePedidos = new ConcurrentQueue<int>();
            var random = new Random(); 

            var tarefasVendedores = new Task[V];
            int pedidosCriados = 0; 
            object lockObj = new object();

            for (int i = 0; i < V; i++)
            {
                int vendedorId = i + 1;
                tarefasVendedores[i] = Task.Run(async () =>
                {
                    while (true)
                    {
                        await Task.Delay(random.Next(500, 1500));
                        int pedidoId;

                        lock (lockObj)
                        {
                            if (pedidosCriados >= N) break;
                            pedidoId = ++pedidosCriados;
                        }

                        filaDePedidos.Enqueue(pedidoId);
                        Console.WriteLine($"Vendedor {vendedorId}: Pedido #{pedidoId:D3} criado.");
                    }
                });
            }

            var tarefasEntregadores = new Task[E];
            for (int i = 0; i < E; i++)
            {
                int entregadorId = i + 1;
                tarefasEntregadores[i] = Task.Run(async () =>
                {
                    while (true)
                    {
                        if (filaDePedidos.TryDequeue(out int pedidoId))
                        {
                            Console.WriteLine($"Entregador {entregadorId}: Pedido #{pedidoId:D3} entregue.");
                            await Task.Delay(random.Next(500, 1000));
                        }
                        else
                        {
                            if (pedidosCriados >= N && filaDePedidos.IsEmpty)
                            {
                                break;
                            }
                            await Task.Delay(100);
                        }
                    }
                });
            }

            await Task.WhenAll(tarefasVendedores);
            await Task.WhenAll(tarefasEntregadores);
            Console.WriteLine("Todas as entregas foram concluídas!");
        }
    }
}
