using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPCourse
{
    public class Lab2_Exercicio2
    {
        public static void Run()
        {
            Console.WriteLine("Insira o valor de N:");
            int N = int.Parse(Console.ReadLine());

            double[] vetor = new double[N];
            Random random = new Random();
            for (int i = 0; i < N; i++)
            {
                vetor[i] = random.NextDouble() * 100; 
            }

            Task<double> mediaTask = Task.Run(() =>
            {
                return vetor.Average();
            });

            Task<double> medianaTask = Task.Run(() =>
            {
                var sortedVetor = vetor.OrderBy(x => x).ToArray();
                if (N % 2 == 0)
                {
                    return (sortedVetor[N / 2 - 1] + sortedVetor[N / 2]) / 2.0;
                }
                else
                {
                    return sortedVetor[N / 2];
                }
            });

            Task<double> varianciaTask = mediaTask.ContinueWith(mediaAntecedente =>
            {
                double media = mediaAntecedente.Result;
                return vetor.Select(x => Math.Pow(x - media, 2)).Average();
            });

            Task<double> desvioPadraoTask = varianciaTask.ContinueWith(varianciaAntecedente =>
            {
                return Math.Sqrt(varianciaAntecedente.Result);
            });

            Task.WaitAll(mediaTask, medianaTask, varianciaTask, desvioPadraoTask);

            Console.WriteLine($"Média: {mediaTask.Result}");
            Console.WriteLine($"Mediana: {medianaTask.Result}");
            Console.WriteLine($"Variância: {varianciaTask.Result}");
            Console.WriteLine($"Desvio Padrão: {desvioPadraoTask.Result}");
        }
    }
}
