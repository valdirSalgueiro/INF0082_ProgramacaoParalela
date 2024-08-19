using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PPCourse
{
    partial class Program
    {
        private static void exercicio1_2()
        {
            string[] tcs = { "t01.in", "t02.in", "t03.in", "t04.in" };
            foreach (string caminho in tcs)
            {
                string[] linhas = File.ReadAllLines(Path.Combine("Ex1", caminho));

                // A primeira linha contém o número de frases
                int N = int.Parse(linhas[0]);

                // Armazena as frases a partir da segunda linha
                string[] frases = new string[N];
                Array.Copy(linhas, 1, frases, 0, N);

                IniciarThreads(N, frases);
            }
        }

        private static void exercicio1_1()
        {
            // Ler o valor de N da entrada
            int N = int.Parse(Console.ReadLine());

            // Criar um array de strings de tamanho N
            string[] frases = new string[N];

            // Ler as N frases da entrada e salvá-las no array
            for (int i = 0; i < N; i++)
            {
                frases[i] = Console.ReadLine();
            }

            IniciarThreads(N, frases);
        }

        private static void IniciarThreads(int N, string[] frases)
        {
            Thread[] threads = new Thread[N];
            for (int i = 0; i < N; i++)
            {
                int index = i;
                threads[i] = new Thread(() => ImprimirMensagem(frases[index]));
                threads[i].Start();
            }

            foreach (Thread t in threads)
            {
                t.Join();
            }
        }

        private static void ImprimirMensagem(string frase)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"Thread {threadId}: {frase}");

        }
    }
}
