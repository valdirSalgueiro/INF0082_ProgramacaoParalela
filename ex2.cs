using System;
using System.Diagnostics;
using System.Threading;

namespace PPCourse
{
    partial class Program
    {
        static void tc2()
        {
            Console.Write("Digite o tamanho do array (N): ");
            // Ler N da entrada
            int N = int.Parse(Console.ReadLine());

            int[] sequence = new int[N];

            // Inicializar o array com inteiros aleatórios
            Random rand = new Random();
            for (int i = 0; i < N; i++)
            {
                sequence[i] = rand.Next(1, 100);
            }

            // Valores de M (número de threads) a serem testados
            int[] quantidadesDeThreads = { 1, 2, 4, 8, 16, 32 };

            // Variável para armazenar o tempo de execução com 1 thread
            double tempoComUmaThread = 0;

            foreach (int M in quantidadesDeThreads)
            {
                // Executa a soma usando M threads e mede o tempo
                var cronometro = Stopwatch.StartNew();
                int somaTotal = SomaParalela(sequence, M);
                cronometro.Stop();

                double tempoDecorridoMs = cronometro.ElapsedMilliseconds;

                Console.WriteLine($"M = {M} | Soma Total = {somaTotal} | Tempo = {tempoDecorridoMs} ms");

                if (M == 1)
                {
                    tempoComUmaThread = tempoDecorridoMs; // Tempo com 1 thread
                }
                else
                {
                    // Calcula o speedup em relação à execução com 1 thread
                    double speedup = tempoComUmaThread / tempoDecorridoMs;
                    Console.WriteLine($"Speedup com {M} threads: {speedup:F2}");
                }

                Console.WriteLine();
            }
        }

        static int SomaParalela(int[] array, int quantidadeDeThreads)
        {
            int tamanhoPorThread = array.Length / quantidadeDeThreads;
            int elementosRestantes = array.Length % quantidadeDeThreads;
            int somaTotal = 0;

            Thread[] threads = new Thread[quantidadeDeThreads];
            int[] somasParciais = new int[quantidadeDeThreads];

            for (int i = 0; i < quantidadeDeThreads; i++)
            {
                int inicio = i * tamanhoPorThread;
                int fim = (i == quantidadeDeThreads - 1) ? inicio + tamanhoPorThread + elementosRestantes : inicio + tamanhoPorThread;

                int indiceThread = i;
                threads[i] = new Thread(() =>
                {
                    somasParciais[indiceThread] = SomaSegmentoArray(array, inicio, fim);
                });
                threads[i].Start();
            }

            // Aguarda a conclusão de todas as threads
            for (int i = 0; i < quantidadeDeThreads; i++)
            {
                threads[i].Join();
                somaTotal += somasParciais[i];
            }

            return somaTotal;
        }

        static int SomaSegmentoArray(int[] array, int inicio, int fim)
        {
            int soma = 0;
            for (int i = inicio; i < fim; i++)
            {
                soma += array[i];
            }
            return soma;
        }
    }
}
