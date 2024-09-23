using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PPCourse
{
    public class Lab3_Exercicio2
    {
        private static double temperatura = 0.0;
        private static ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();

        public static async Task Run()
        {
            string[] entrada = Console.ReadLine().Split();
            int usuarios = int.Parse(entrada[0]);
            int atualizacoes = int.Parse(entrada[1]);

            int[] leituras = new int[usuarios];
            for (int i = 0; i < usuarios; i++)
            {
                leituras[i] = int.Parse(Console.ReadLine());
            }

            var tasks = new Task[usuarios + 1];

            tasks[0] = Task.Run(async () =>
            {
                Random random = new Random();
                for (int i = 0; i < atualizacoes; i++)
                {
                    await Task.Delay(random.Next(500, 1000));
                    double novaTemperatura = random.NextDouble() * 40;

                    rwLock.EnterWriteLock();
                    try
                    {
                        temperatura = novaTemperatura;
                        Console.WriteLine($"[Sensor] Temperatura atualizada: {temperatura:F2}°C");
                    }
                    finally
                    {
                        rwLock.ExitWriteLock();
                    }
                }
            });

            for (int i = 0; i < usuarios; i++)
            {
                int usuario = i;
                tasks[i + 1] = Task.Run(async () =>
                {
                    Random random = new Random();
                    for (int j = 0; j < leituras[usuario]; j++)
                    {
                        await Task.Delay(random.Next(200, 800));

                        rwLock.EnterReadLock();
                        try
                        {
                            Console.WriteLine($"Usuário {usuario + 1}: Temperatura lida: {temperatura:F2}°C");
                        }
                        finally
                        {
                            rwLock.ExitReadLock();
                        }
                    }
                });
            }

            await Task.WhenAll(tasks);
        }
    }
}
