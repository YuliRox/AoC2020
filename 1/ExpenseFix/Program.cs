using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseFix
{
    public static class Program
    {
        public static async Task Main(string[] _)
        {
            var inputLines = await File.ReadAllLinesAsync("../input.txt");
            var inputNumbers = inputLines.Select(int.Parse).ToArray();

            var cartesianAddTwo = inputNumbers
                .SelectMany(_ => inputNumbers, (x, y) => new
                {
                    A = x,
                    B = y,
                    Add = x + y,
                    Multiple = x * y
                });
            var resultSet = cartesianAddTwo
            .Where(x => x.Add == 2020)
            .ToList();

            foreach (var result in resultSet)
            {
                Console.WriteLine($"{result.A}*{result.B}={result.Multiple}");
            }
        }
    }
}
