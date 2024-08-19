using System;
using System.Diagnostics;
using System.Threading;

namespace PPCourse
{
    partial class Program
    {
/*
Conclusões dos Resultados

1. Tamanho do Array (N = 100000)
   - Comportamento Observado: À medida que o número de threads aumenta, o tempo de execução também aumenta, e o speedup diminui. O melhor tempo é com uma única thread, e o desempenho piora significativamente com mais threads.
   - Conclusão: Para arrays menores, a sobrecarga associada à criação e gerenciamento de threads supera os benefícios do paralelismo. Isso ocorre porque o trabalho a ser dividido entre as threads é pequeno, e o overhead de sincronização e troca de contexto domina o tempo de execução.

2. Tamanho do Array (N = 100000000)
   - Comportamento Observado: Com um array muito grande, o speedup melhora substancialmente ao utilizar 2 e 4 threads, atingindo o máximo com 4 threads. No entanto, com 8 threads e além, o speedup começa a diminuir.
   - Conclusão: Para arrays grandes, o paralelismo é efetivo até certo ponto, onde a quantidade de trabalho distribuído entre as threads é suficiente para compensar o overhead. Entretanto, quando o número de threads continua a aumentar, o overhead de sincronização começa a superar os benefícios do paralelismo, especialmente devido à contenção e ao aumento da complexidade de gerenciamento de threads.

3. Tamanho do Array (N = 1000)
   - Comportamento Observado: Para um array muito pequeno, o tempo de execução com várias threads é significativamente pior do que com uma única thread. O speedup é muito baixo, indicando uma perda de desempenho considerável com o aumento do número de threads.
   - Conclusão: Para tarefas muito pequenas, o uso de múltiplas threads não é eficiente e pode ser prejudicial. A sobrecarga de gerenciar threads é muito alta em relação ao trabalho real a ser feito, resultando em tempos de execução maiores.
*/
        static void exercicio2()
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
