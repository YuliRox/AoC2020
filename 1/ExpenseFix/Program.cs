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
                //          |func1: _=> inputNumbers // nimm für jede zahl wieder die komplette liste an zahlen
                //                            | func2: x aktuelle zahl, y wird für alle zahlen aus der zweiten liste durchiteriert
                .SelectMany(_ => inputNumbers, (x, y) => new
                {
                    A = x,
                    B = y,
                    Add = x + y,
                    Multiple = x * y
                });

            var cartesianAddThree = inputNumbers
                .SelectMany(_ => inputNumbers, (x, y) => new
                {
                    A = x,
                    B = y
                })
                .SelectMany(_ => inputNumbers, (x, y) => new
                {
                    A = x.A,
                    B = x.B,
                    C = y,
                    Add = x.A + x.B + y,
                    Multiple = x.A * x.B * y
                });

            var resultSet = cartesianAddTwo
            .Where(x => x.Add == 2020)
            .ToList();

            Console.WriteLine($"--- Part One ---");
            foreach (var result in resultSet)
            {
                Console.WriteLine($"{result.A}*{result.B}={result.Multiple}");
            }

            var resultSet2 = cartesianAddThree
            .Where(x => x.Add == 2020)
            .ToList();
            Console.WriteLine($"--- Part Two ---");
            foreach (var result in resultSet2)
            {
                Console.WriteLine($"{result.A}*{result.B}*{result.C}={result.Multiple}");
            }
        }
    }
}
