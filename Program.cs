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
            RodarLab2();
        }

        private static void RodarLab2()
        {
            Lab2_Exercicio1.Benchmark();
        }
    }
}
