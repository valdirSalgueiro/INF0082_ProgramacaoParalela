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

        public delegate void ManualDelegate();
        public delegate void TCDelegate();

        static void Main(string[] args)
        {
            Delegate[][] exercicios = new Delegate[5][];

            exercicios[0] = new Delegate[] { new ManualDelegate(stub), new TCDelegate(stub) };
            exercicios[1] = new Delegate[] { new ManualDelegate(tc2), new TCDelegate(stub) };
            //exercicios[2] = new Delegate[] { new ManualDelegate(manual1), new TCDelegate(tc1) };
            for (int i = 0; i < exercicios.Length; i++)
            {
                if (exercicios[i] == null)
                    break;

                Console.WriteLine($"Executando exercício {i + 1}:");

                if (exercicios[i][0] is ManualDelegate manualDelegate)
                {
                    manualDelegate();
                }

                if (exercicios[i][1] is TCDelegate automaticoDelegate)
                {
                    automaticoDelegate();
                }
            }
        }

        private static void stub() { }
    }
}
