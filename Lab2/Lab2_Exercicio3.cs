using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PPCourse
{
    public class Lab2_Exercicio3
    {
        public static void Run()
        {
            string[] arquivos = new[] { "Lab2/Ex3/t1.in", "Lab2/Ex3/t2.in", "Lab2/Ex3/t3.in"};
            foreach (var arquivo in arquivos)
            {
                RunFile(arquivo);
            }

        }

        private static void RunFile(string nomeArquivoEntrada)
        {
            string[] linhasEntrada;
            try
            {
                linhasEntrada = File.ReadAllLines(nomeArquivoEntrada);
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao ler o arquivo de entrada: " + e.Message);
                return;
            }

            string nomeArquivoTexto = linhasEntrada[0];

            int quantidadePalavras = int.Parse(linhasEntrada[1]);

            string[] palavras = new string[quantidadePalavras];
            for (int i = 0; i < quantidadePalavras; i++)
            {
                palavras[i] = linhasEntrada[i + 2].ToLower();
            }

            string texto;
            try
            {
                texto = File.ReadAllText("Lab2/Ex3/" + nomeArquivoTexto).ToLower();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao ler o arquivo de texto: " + e.Message);
                return;
            }

            var contador = new ConcurrentDictionary<string, int>();

            Parallel.ForEach(palavras, palavra =>
            {
                var textoFiltrado = texto.Split(new[] { ' ', '.', ',', ';', '!', '?', '\r', '\n', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                int contagem = textoFiltrado.Count(x => x == palavra);
                contador[palavra] = contagem;
            });

            foreach (var palavra in palavras)
            {
                Console.WriteLine($"A palavra '{palavra}' aparece {contador[palavra]} vez(es) no texto.");
            }
        }
    }
}
