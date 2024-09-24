using System;
using System.Threading;
using System.Threading.Tasks;

namespace PPCourse
{
    public class Lab3_Exercicio5
    {
        private static int vagasDisponiveis;
        private static AutoResetEvent vagaLiberada = new AutoResetEvent(true);

        public static async Task Run()
        {
            string[] entrada = Console.ReadLine().Split();
            int capacidade = int.Parse(entrada[0]);
            int totalVeiculos = int.Parse(entrada[1]);
            vagasDisponiveis = capacidade;

            for (int i = 1; i <= totalVeiculos; i++)
            {
                int id = i;
                Task.Run(() => Veiculo(id));
            }

            Console.ReadLine();
        }

        static async Task Veiculo(int id)
        {
            Random random = new Random();
            Console.WriteLine($"Veículo {id} esperando para entrar...");

            vagaLiberada.WaitOne();

            lock (vagaLiberada)
            {
                vagasDisponiveis--;
                Console.WriteLine($"Evento: Veículo entrou. Vagas disponíveis: {vagasDisponiveis}");
                Console.WriteLine($"Veículo {id} estacionou.");
            }

            await Task.Delay(random.Next(2000, 5000));

            lock (vagaLiberada)
            {
                vagasDisponiveis++;
                Console.WriteLine($"Evento: Veículo saiu. Vagas disponíveis: {vagasDisponiveis}");
            }

            vagaLiberada.Set();
            Console.WriteLine($"Veículo {id} saiu.");
        }
    }
}
