using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PPCourse
{
/* resultado:
  versao1
  n = 1 | tamanho = 1000000 | range = 1000 |  Tempo = 83 ms
  n = 2 | tamanho = 1000000 | range = 1000 |  Tempo = 66 ms
  n = 4 | tamanho = 1000000 | range = 1000 |  Tempo = 66 ms
  n = 8 | tamanho = 1000000 | range = 1000 |  Tempo = 150 ms
  n = 16 | tamanho = 1000000 | range = 1000 |  Tempo = 128 ms
  n = 32 | tamanho = 1000000 | range = 1000 |  Tempo = 239 ms
  n = 20 | tamanho = 1000000 | range = 1000 |  Tempo = 247 ms
  n = 20 | tamanho = 1000000 | range = 10000 |  Tempo = 163 ms
  n = 20 | tamanho = 1000000 | range = 100000 |  Tempo = 173 ms
  n = 20 | tamanho = 1000000 | range = 1000 |  Tempo = 239 ms
  n = 20 | tamanho = 10000000 | range = 1000 |  Tempo = 603 ms
  n = 20 | tamanho = 100000000 | range = 1000 |  Tempo = 3371 ms

  versao2
  n = 1 | tamanho = 1000000 | range = 1000 |  Tempo = 81 ms
  n = 2 | tamanho = 1000000 | range = 1000 |  Tempo = 32 ms
  n = 4 | tamanho = 1000000 | range = 1000 |  Tempo = 65 ms
  n = 8 | tamanho = 1000000 | range = 1000 |  Tempo = 68 ms
  n = 16 | tamanho = 1000000 | range = 1000 |  Tempo = 134 ms
  n = 32 | tamanho = 1000000 | range = 1000 |  Tempo = 199 ms
  n = 20 | tamanho = 1000000 | range = 1000 |  Tempo = 148 ms
  n = 20 | tamanho = 1000000 | range = 10000 |  Tempo = 110 ms
  n = 20 | tamanho = 1000000 | range = 100000 |  Tempo = 366 ms
  n = 20 | tamanho = 1000000 | range = 1000 |  Tempo = 252 ms
  n = 20 | tamanho = 10000000 | range = 1000 |  Tempo = 523 ms
  n = 20 | tamanho = 100000000 | range = 1000 |  Tempo = 2657 ms

Análise de Desempenho das Versões

Versão 1: Threads Atualizando a Estrutura Compartilhada

1. Escalabilidade com Aumento de `n` (Número de Threads):
   - `n = 1` a `n = 4`: O tempo diminui ao passar de uma única thread para duas threads, mas não há mais melhorias ao aumentar para quatro threads. Isso sugere que, até quatro threads, o paralelismo oferece benefícios, mas além disso, o ganho é limitado.
   - `n = 8` e além: O tempo de execução aumenta significativamente à medida que mais threads são adicionadas, o que indica contenção na estrutura compartilhada (`ConcurrentDictionary`). Muitas threads competindo para acessar e atualizar a mesma estrutura de dados causam atrasos.

2. Efeito do Range:
   - Aumento do `range` (de 1000 para 100000): A execução com `range` maior reduz ligeiramente o tempo, sugerindo que uma maior dispersão dos números resulta em menos contenção (menos colisões nas chaves do dicionário).
   - Quando `tamanho` aumenta: O tempo aumenta proporcionalmente, o que é esperado, pois mais dados precisam ser processados.

Versão 2: Usando Contadores Privados

1. Escalabilidade com Aumento de `n`:
   - `n = 1` a `n = 2`: O tempo reduz significativamente quando o número de threads aumenta para dois, o que demonstra que os contadores privados oferecem vantagens no paralelismo.
   - `n = 4` a `n = 32`: Como na versão 1, o tempo aumenta a partir de um certo número de threads (n > 2), mas os aumentos são menos drásticos. Isso indica que os contadores privados reduzem a contenção, mas ainda há sobrecarga na fase de fusão dos resultados.

2. Efeito do Range:
   - Com `range` maior: Como na versão 1, há alguma melhoria na execução com `range` maior, pois menos colisões ocorrem.
   - Quando `tamanho` aumenta: Assim como na versão 1, o tempo de execução aumenta proporcionalmente com o tamanho do vetor.

Comparação entre as Versões

- Performance Geral: A versão 2, que usa contadores privados, geralmente se comporta melhor com múltiplas threads comparado à versão 1. Isso é esperado, pois o uso de contadores privados evita a alta contenção que ocorre na versão 1.
  
- Eficiência do Paralelismo: Para ambas as versões, adicionar mais threads além de um certo ponto (`n > 2` ou `n > 4`) começa a diminuir a eficiência ou mesmo a piorar o desempenho, devido a sobrecargas como contenção, sincronização e fusão dos resultados.
*/


    public partial class Lab1
    {
        private static int[] quantidadesDeThreads = { 1, 2, 4, 8, 16, 32 };
        private static int[] rangesAleatorio = { 1000, 10000, 100000 };
        private static int[] tamanhos = { 1000000, 10000000, 100000000 };
        private static Stopwatch cronometro;
        private static double tempoDecorridoMs;

        static void exercicio3()
        {
            benchmark(versao1);
            benchmark(versao2);
            benchmark3();
        }

        private static void benchmark3()
        {
            cronometro = Stopwatch.StartNew();
            versao3();
            cronometro.Stop();

            tempoDecorridoMs = cronometro.ElapsedMilliseconds;
            Console.WriteLine($"Tempo = {tempoDecorridoMs} ms");
        }

        private static void benchmark(Action<int, int, int> benchmarkFunction)
        {
            int numeroDeThreads = Environment.ProcessorCount;
            Console.WriteLine(benchmarkFunction.Method.Name);
            foreach (int n in quantidadesDeThreads)
            {
                doSubBenchmark(benchmarkFunction, n, 1000, 1000000);
            }
            foreach (int range in rangesAleatorio)
            {
                doSubBenchmark(benchmarkFunction, numeroDeThreads, range, 1000000);
            }
            foreach (int tamanho in tamanhos)
            {
                doSubBenchmark(benchmarkFunction, numeroDeThreads, 1000, tamanho);
            }
        }

        private static void doSubBenchmark(Action<int, int, int> benchmarkFunction, int numeroDeThreads, int range, int tamanho)
        {
            cronometro = Stopwatch.StartNew();
            benchmarkFunction(numeroDeThreads, tamanho, range);
            cronometro.Stop();

            tempoDecorridoMs = cronometro.ElapsedMilliseconds;
            Console.WriteLine($"n = {numeroDeThreads} | tamanho = {tamanho} | range = {range} |  Tempo = {tempoDecorridoMs} ms");
        }

        private static void versao1(int n, int tamanhoDoVetor, int rangeAleatorio)
        {
            int numeroDeThreads = n;
            int[] numeros = new int[tamanhoDoVetor];
            Random rand = new Random();

            // Gera números aleatórios
            for (int i = 0; i < tamanhoDoVetor; i++)
            {
                numeros[i] = rand.Next(0, rangeAleatorio);
            }

            ConcurrentDictionary<int, int> dicionarioCompartilhado = new ConcurrentDictionary<int, int>();
            int tamanhoDaParticao = tamanhoDoVetor / numeroDeThreads;
            Thread[] threads = new Thread[numeroDeThreads];

            for (int i = 0; i < numeroDeThreads; i++)
            {
                int inicio = i * tamanhoDaParticao;
                int fim = (i == numeroDeThreads - 1) ? tamanhoDoVetor : inicio + tamanhoDaParticao;

                threads[i] = new Thread(() =>
                {
                    for (int j = inicio; j < fim; j++)
                    {
                        dicionarioCompartilhado.AddOrUpdate(numeros[j], 1, (chave, valorAntigo) => valorAntigo + 1);
                    }
                });

                threads[i].Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            // Exibe parte do resultado
            //foreach (var kvp in dicionarioCompartilhado.Take(10))
            //{
            //    Console.WriteLine($"Número {kvp.Key}: {kvp.Value} ocorrências");
            //}
        }

        private static void versao2(int numeroDeThreads, int tamanhoDoVetor, int rangeAleatorio)
        {
            int[] numeros = new int[tamanhoDoVetor];
            Random rand = new Random();

            // Gera números aleatórios
            for (int i = 0; i < tamanhoDoVetor; i++)
            {
                numeros[i] = rand.Next(0, rangeAleatorio);
            }

            Dictionary<int, int>[] dicionariosLocais = new Dictionary<int, int>[numeroDeThreads];
            int tamanhoDaParticao = tamanhoDoVetor / numeroDeThreads;
            Thread[] threads = new Thread[numeroDeThreads];

            for (int i = 0; i < numeroDeThreads; i++)
            {
                int indiceThread = i;
                dicionariosLocais[indiceThread] = new Dictionary<int, int>();

                int inicio = indiceThread * tamanhoDaParticao;
                int fim = (indiceThread == numeroDeThreads - 1) ? tamanhoDoVetor : inicio + tamanhoDaParticao;

                threads[indiceThread] = new Thread(() =>
                {
                    for (int j = inicio; j < fim; j++)
                    {
                        if (dicionariosLocais[indiceThread].ContainsKey(numeros[j]))
                        {
                            dicionariosLocais[indiceThread][numeros[j]]++;
                        }
                        else
                        {
                            dicionariosLocais[indiceThread][numeros[j]] = 1;
                        }
                    }
                });

                threads[indiceThread].Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            // Mescla os dicionários locais
            Dictionary<int, int> dicionarioFinal = new Dictionary<int, int>();
            foreach (var dicionarioLocal in dicionariosLocais)
            {
                foreach (var kvp in dicionarioLocal)
                {
                    if (dicionarioFinal.ContainsKey(kvp.Key))
                    {
                        dicionarioFinal[kvp.Key] += kvp.Value;
                    }
                    else
                    {
                        dicionarioFinal[kvp.Key] = kvp.Value;
                    }
                }
            }

            // Exibe parte do resultado
            //foreach (var kvp in dicionarioFinal.Take(10))
            //{
            //    Console.WriteLine($"Número {kvp.Key}: {kvp.Value} ocorrências");
            //}
        }

        private static void versao3()
        {
            int numeroDeThreads = Environment.ProcessorCount;
            int tamanhoDoVetor = 1000000;
            int[] numeros = new int[tamanhoDoVetor];
            Random rand = new Random();

            // Gera números aleatórios
            for (int i = 0; i < tamanhoDoVetor; i++)
            {
                numeros[i] = rand.Next(0, 1000);
            }

            int tamanhoDaParticao = tamanhoDoVetor / numeroDeThreads;
            Dictionary<int, int>[] dicionariosLocais = new Dictionary<int, int>[numeroDeThreads];

            Parallel.For(0, numeroDeThreads, i =>
            {
                dicionariosLocais[i] = new Dictionary<int, int>();
                int inicio = i * tamanhoDaParticao;
                int fim = (i == numeroDeThreads - 1) ? tamanhoDoVetor : inicio + tamanhoDaParticao;

                for (int j = inicio; j < fim; j++)
                {
                    if (dicionariosLocais[i].ContainsKey(numeros[j]))
                    {
                        dicionariosLocais[i][numeros[j]]++;
                    }
                    else
                    {
                        dicionariosLocais[i][numeros[j]] = 1;
                    }
                }
            });

            // Fase de redução: Agregação dos resultados de cada thread
            Dictionary<int, int> dicionarioFinal = new Dictionary<int, int>();

            foreach (var dicionarioLocal in dicionariosLocais)
            {
                foreach (var kvp in dicionarioLocal)
                {
                    if (dicionarioFinal.ContainsKey(kvp.Key))
                    {
                        dicionarioFinal[kvp.Key] += kvp.Value;
                    }
                    else
                    {
                        dicionarioFinal[kvp.Key] = kvp.Value;
                    }
                }
            }

            // Exibe parte do resultado
            //foreach (var kvp in dicionarioFinal.Take(10))
            //{
            //    Console.WriteLine($"Número {kvp.Key}: {kvp.Value} ocorrências");
            //}
        }
    }
}
