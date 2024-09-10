using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PPCourse
{
    public class Lab2_Exercicio4
    {
        /**
            Tempo com Tasks: 37 ms
            Tempo com Parallel.For: 0 ms
            Tempo com Parallel.ForEach: 12 ms

            Parallel.For parece ser a abordagem mais rápida neste cenário, possivelmente devido à sua eficiência em gerenciar iterações em paralelo para tarefas simples.
            Parallel.ForEach teve um desempenho razoável e pode ser uma boa escolha para operações em coleções.
            Tasks teve um desempenho inferior, possivelmente devido ao overhead associado à criação e gerenciamento de muitas tarefas.
         */

        public static void Run()
        {
            string[] arquivos = new[] { "Lab2/Ex4/t1.in", "Lab2/Ex4/t2.in"  };
            foreach (var arquivo in arquivos)
            {
                RunFile(arquivo);
            }
        }

        private static void RunFile(string arquivo)
        {
            int[] numeros;

            try
            {
                string[] linhas = File.ReadAllLines(arquivo);
                int N = int.Parse(linhas[0]);
                numeros = linhas[1].Split(' ').Select(int.Parse).ToArray();

                if (numeros.Length != N)
                {
                    throw new Exception("O número de inteiros não corresponde ao valor de N.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao ler o arquivo: " + e.Message);
                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            ProgramaComTasks.Executar(numeros);
            stopwatch.Stop();
            Console.WriteLine($"Tempo com Tasks: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            ProgramaComParallelFor.Executar(numeros);
            stopwatch.Stop();
            Console.WriteLine($"Tempo com Parallel.For: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            ProgramaComParallelForEach.Executar(numeros);
            stopwatch.Stop();
            Console.WriteLine($"Tempo com Parallel.ForEach: {stopwatch.ElapsedMilliseconds} ms");
        }
    }

    class ProgramaComTasks
    {
        public static void Executar(int[] numeros)
        {
            List<Task> tasks = new List<Task>();

            foreach (int k in numeros)
            {
                tasks.Add(Task.Run(() => Utils.ImprimirPrimos(k)));
            }

            Task.WaitAll(tasks.ToArray());
        }
    }

    class ProgramaComParallelFor
    {
        public static void Executar(int[] numeros)
        {
            Parallel.For(0, numeros.Length, i =>
            {
                Utils.ImprimirPrimos(numeros[i]);
            });
        }
    }

    class ProgramaComParallelForEach
    {
        public static void Executar(int[] numeros)
        {
            Parallel.ForEach(numeros, k =>
            {
                Utils.ImprimirPrimos(k);
            });
        }
    }

    static class Utils
    {
        public static void ImprimirPrimos(int k)
        {
            Console.WriteLine($"Primos entre 1 e {k}:");
            for (int i = 2; i <= k; i++)
            {
                if (EhPrimo(i))
                    Console.Write(i + " ");
            }
            Console.WriteLine();
        }

        public static bool EhPrimo(int numero)
        {
            if (numero < 2) return false;
            for (int i = 2; i <= Math.Sqrt(numero); i++)
            {
                if (numero % i == 0) return false;
            }
            return true;
        }
    }
}
