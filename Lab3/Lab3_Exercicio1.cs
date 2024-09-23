using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPCourse
{
    public class Lab3_Exercicio1
    {
        private static int totalVotosChapaA = 0;
        private static int totalVotosChapaB = 0;
        private static readonly object lockObject = new object();

        public static async Task Run()
        {
            int R = int.Parse(Console.ReadLine());

            var votos = new List<(int votosChapaA, int votosChapaB)>();

            for (int i = 0; i < R; i++)
            {
                string[] entrada = Console.ReadLine().Split();
                int votosRegiaoA = int.Parse(entrada[0]);
                int votosRegiaoB = int.Parse(entrada[1]);
                votos.Add((votosRegiaoA, votosRegiaoB));
            }

            var tasks = new List<Task>();
            foreach (var (votosChapaA, votosChapaB) in votos)
            {
                tasks.Add(Task.Run(async () =>
                {
                    Random random = new Random();
                    await Task.Delay(random.Next(100, 500)); // Simulação de envio de dados com delay
                    lock (lockObject)
                    {
                        totalVotosChapaA += votosChapaA;
                        totalVotosChapaB += votosChapaB;
                    }
                }));
            }

            await Task.WhenAll(tasks);

            Console.WriteLine($"Chapa A: {totalVotosChapaA} Votos");
            Console.WriteLine($"Chapa B: {totalVotosChapaB} Votos");

            if (totalVotosChapaA > totalVotosChapaB)
                Console.WriteLine("A chapa A venceu a eleição!");
            else if (totalVotosChapaB > totalVotosChapaA)
                Console.WriteLine("A chapa B venceu a eleição!");
            else
                Console.WriteLine("A eleição terminou em empate!");
        }
    }
}
