using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPCourse
{
    public class Lab2_Exercicio1
    {
        /*
         * Os resultados mostram que, para os valores testados de N, a implementação paralela teve um desempenho significativamente pior do que a implementação sequencial. Aqui estão os pontos principais:

        N = 1.000.000:

        Tempo sequencial: 3 ms
        Tempo paralelo: 326 ms
        Speedup: 0,0092 (desaceleração)
        N = 10.000.000:

        Tempo sequencial: 17 ms
        Tempo paralelo: 1.472 ms
        Speedup: 0,0115 (desaceleração)
        N = 100.000.000:

        Tempo sequencial: 176 ms
        Tempo paralelo: 11.935 ms
        Speedup: 0,0147 (desaceleração)
        Conclusão:
        Desempenho: A versão paralela foi consistentemente mais lenta que a sequencial, resultando em um "speedup" menor que 1, o que indica desaceleração.
        Possíveis causas: O overhead associado à criação e sincronização de threads provavelmente superou os benefícios do paralelismo devido ao custo da operação lock e à natureza relativamente simples do cálculo. Em cenários onde o trabalho por thread é muito pequeno, o paralelismo pode não ser vantajoso.
        */

        public static void Run()
        {
            int[] inputs = { 1000000, 10000000, 100000000 };

            foreach (int input in inputs)
            {
                Console.WriteLine($"Número de inteiros: {input}");

                Stopwatch stopwatch = Stopwatch.StartNew();
                long somaSequencial = Sequencial(input);
                stopwatch.Stop();
                long tempoSequencial = stopwatch.ElapsedMilliseconds;
                Console.WriteLine($"Soma dos quadrados (sequencial): {somaSequencial}");
                Console.WriteLine($"Tempo sequencial: {tempoSequencial} ms");

                stopwatch.Restart();
                long somaParalela = Paralela(input);

                stopwatch.Stop();
                long tempoParalelo = stopwatch.ElapsedMilliseconds;
                Console.WriteLine($"Soma dos quadrados (paralela): {somaParalela}");
                Console.WriteLine($"Tempo paralelo: {tempoParalelo} ms");

                double speedup = (double)tempoSequencial / tempoParalelo;
                Console.WriteLine($"Speedup: {speedup}");
            }
        }

        private static long Paralela(int N)
        {
            long somaParalela = 0;
            object lockObj = new object();

            Parallel.For(0, N + 1, i =>
            {
                long quadrado = i * i;
                lock (lockObj)
                {
                    somaParalela += quadrado;
                }
            });
            return somaParalela;
        }

        private static long Sequencial(int N)
        {
            long somaSequencial = 0;
            for (int i = 0; i <= N; i++)
            {
                somaSequencial += i * i;
            }

            return somaSequencial;
        }
    }
}
