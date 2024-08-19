using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PPCourse
{
    partial class Program
    {
        public delegate void ExercicioDelegate();

        static void Main(string[] args)
        {
            Delegate[] exercicios = new Delegate[4];

            exercicios[0] = new ExercicioDelegate(exercicio1_1);
            exercicios[1] = new ExercicioDelegate(exercicio1_2);
            exercicios[2] = new ExercicioDelegate(exercicio2);
            exercicios[3] = new ExercicioDelegate(exercicio3);
            for (int i = 0; i < exercicios.Length; i++)
            {
                if (exercicios[i] == null)
                    break;

                Console.WriteLine($"Executando exercício {exercicios[i].Method.Name}:");

                if (exercicios[i] is ExercicioDelegate exercicioDelegate)
                {
                    exercicioDelegate();
                }
            }
        }

        private static void stub() { }
    }
}
