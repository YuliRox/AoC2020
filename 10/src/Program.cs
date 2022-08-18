using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shared;

namespace src
{
    class Program
    {
        static void Main(string[] args)
        {

            var inputData = DataLoader.LoadInputData(data => int.Parse(data));

            foreach (var inputSet in inputData)
            {
                Console.WriteLine($"> Part-1 for {inputSet.Name}");

                var value = Part1(inputSet);

                Console.WriteLine($"< Part-1 for {inputSet.Name}");


                Console.WriteLine($"> Part-2 for {inputSet.Name}");

                var value2 = Part2(inputSet);

                Console.WriteLine($"< Part-2 for {inputSet.Name}");
            }
        }

        private static int Part1(PuzzleInput<int> input)
        {
            Console.WriteLine("Original array");
            foreach (int i in input.Content)
            {
                Console.Write(i + " ");
            }

            Array.Sort(input.Content);
            Console.WriteLine("Sorted array");
            foreach (int i in input.Content)
            {
                Console.Write(i + " ");
            }

            return 0;
        }

        private static int Part2(PuzzleInput<int> input)
        {
            return 0;
        }
    }
}
