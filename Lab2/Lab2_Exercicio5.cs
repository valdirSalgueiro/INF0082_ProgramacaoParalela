using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PPCourse
{
    public class Lab2_Exercicio5
    {
        /**
            Tempo de Execução (t1...t5)
            Versão Sequencial:
                1579 ms
                1893 ms
                2547 ms
                4470 ms
                9126 ms

            Parallel.ForEach:
                604 ms
                355 ms
                436 ms
                1681 ms
                4574 ms

            Async/Await:
                243 ms
                344 ms
                588 ms
                870 ms
                1587 ms

            Async/Await oferece o melhor desempenho para operações de I/O, como requisições HTTP, 
                por ser assíncrono e não bloquear a thread enquanto aguarda as respostas.
            Parallel.ForEach é uma boa alternativa para tarefas CPU-bound e pode ser eficiente, 
                mas pode não ter o mesmo nível de eficiência para I/O-bound quando comparado ao Async/Await.
            Versão Sequencial não é recomendada para cenários com grande volume de dados ou operações de I/O, 
                devido ao seu tempo de execução elevado e comportamento bloqueante.
         */

        public static async Task Run()
        {
            string[] arquivos = new[] { "Lab2/Ex5/t1.in", "Lab2/Ex5/t2.in", "Lab2/Ex5/t3.in", "Lab2/Ex5/t4.in", "Lab2/Ex5/t5.in" };
            foreach (var arquivo in arquivos)
            {
                await RunFile(arquivo);
            }
        }

        private static async Task RunFile(string caminhoArquivo)
        {
            List<int> ids = new List<int>();
            try
            {
                ids = LerIdsDoArquivo(caminhoArquivo);
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao ler o arquivo: " + e.Message);
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            ProgramaSequencial.Executar(ids);
            stopwatch.Stop();
            Console.WriteLine($"Tempo com versão Sequencial: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            ProgramaParallel.Executar(ids);
            stopwatch.Stop();
            Console.WriteLine($"Tempo com Parallel.ForEach: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            await ProgramaAsync.Executar(ids);
            stopwatch.Stop();
            Console.WriteLine($"Tempo com Async/Await: {stopwatch.ElapsedMilliseconds} ms");
        }

        static List<int> LerIdsDoArquivo(string caminhoArquivo)
        {
            List<int> ids = new List<int>();
            foreach (var linha in File.ReadLines(caminhoArquivo))
            {
                ids.AddRange(Array.ConvertAll(linha.Split(' '), int.Parse));
            }
            return ids;
        }
    }

    class ProgramaSequencial
    {
        private static readonly HttpClient client = new HttpClient();

        public static void Executar(List<int> ids)
        {
            List<string> resultados = new List<string>();
            foreach (var id in ids)
            {
                var resultado = PokemonUtils.ObterDadosPokemon(id).Result;
                resultados.Add(resultado);
            }

            File.WriteAllLines("pokedex.txt", resultados);
        }
    }

    class ProgramaParallel
    {
        private static readonly HttpClient client = new HttpClient();

        public static void Executar(List<int> ids)
        {
            List<string> resultados = new List<string>();
            Parallel.ForEach(ids, id =>
            {
                var resultado = PokemonUtils.ObterDadosPokemon(id).Result;
                lock (resultados)
                {
                    resultados.Add(resultado);
                }
            });

            File.WriteAllLines("pokedex.txt", resultados);
        }
    }

    class ProgramaAsync
    {
        public static async Task Executar(List<int> ids)
        {
            List<Task<string>> tarefas = new List<Task<string>>();
            foreach (var id in ids)
            {
                tarefas.Add(PokemonUtils.ObterDadosPokemon(id));
            }

            var resultados = await Task.WhenAll(tarefas);
            File.WriteAllLines("pokedex.txt", resultados);
        }
    }

    static class PokemonUtils
    {
        private static readonly HttpClient client = new HttpClient();
        public static async Task<string> ObterDadosPokemon(int id)
        {
            string url = $"https://pokeapi.co/api/v2/pokemon/{id}";
            string resposta = await client.GetStringAsync(url);
            var json = JsonDocument.Parse(resposta);

            string nome = json.RootElement.GetProperty("name").GetString();
            var tipos = json.RootElement.GetProperty("types").EnumerateArray();
            List<string> tiposList = new List<string>();

            foreach (var tipo in tipos)
            {
                string tipoNome = tipo.GetProperty("type").GetProperty("name").GetString();
                tiposList.Add(tipoNome);
            }

            return $"{nome},{string.Join(",", tiposList)}";
        }
    }
}
