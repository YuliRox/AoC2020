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
                if (!inputSet.Content.Any())
                {
                    continue;
                }

                Console.WriteLine($"> Part-1 for {inputSet.Name}");

                var value = Part1(inputSet);
                Console.WriteLine($"Jolts: {value}");

                Console.WriteLine($"< Part-1 for {inputSet.Name}");


                Console.WriteLine($"> Part-2 for {inputSet.Name}");

                var value2 = Part2(inputSet);

                Console.WriteLine($"< Part-2 for {inputSet.Name}");
            }
        }

        private static int Part1(PuzzleInput<int> input)
        {

            var adapterBag = new AdapterBag(input.Content);

            return adapterBag.CountJolts(true);
        }

        public class AdapterBag
        {
            public AdapterBag(int[] input)
            {
                Adapters = new int[input.Length + 2];
                SortedAdapters = new int[input.Length + 2];

                Adapters[0] = ChargingOutlet;
                input.CopyTo(Adapters, 1);
                Adapters.CopyTo(SortedAdapters, 0);

                InternalAdapter = Adapters.Max() + 3;
                Adapters[Adapters.Length - 1] = InternalAdapter;
                SortedAdapters[SortedAdapters.Length - 1] = InternalAdapter;
                Array.Sort(SortedAdapters);
            }

            public int[] Jolts { get; private set; } = new int[4];
            public int ChargingOutlet { get; private set; } = 0;
            public int InternalAdapter { get; private set; } = 0;
            public int[] Adapters { get; private set; }
            private int[] SortedAdapters { get; set; }

            public int CountJolts(bool debug = false)
            {

                for (int i = 0; i < SortedAdapters.Length; i++)
                {
                    Console.Write("{0} ", SortedAdapters[i]);
                }
                Console.WriteLine();

                string adapterType = "Current Adapter";
                for (var i = 0; i < SortedAdapters.Length; i++)
                {
                    if (i == SortedAdapters.Length - 1)
                    {
                        continue;
                    }

                    var current = SortedAdapters[i];
                    var next = SortedAdapters[i + 1];
                    var diff = next - current;
                    Jolts[diff] += 1;

                    if (debug)
                    {
                        PrintIteration(i, adapterType, diff, current, next);
                    }

                }

                if (debug)
                {
                    Console.WriteLine($"There are {Jolts[1]} differences of 1 jolt and {Jolts[3]} differences of 3 jolt");
                }

                return Jolts[1] * Jolts[3];

            }

            private void PrintIteration(int i, string adapterType, int diff, int current, int next)
            {

                    if (i == 0)
                    {
                        adapterType = "Charging Outlet";
                    }
                    else if (i == SortedAdapters.Length - 1)
                    {
                        adapterType = "Built-In Adapter";
                    }
                    else
                    {
                        adapterType = "Current Adapter";
                    }

                    Console.WriteLine($"Iteration {i} - Jolt: {diff} - {adapterType}: {current} - Next Adapter: {next}");

            }
        }

        private static int Part2(PuzzleInput<int> input)
        {
            return 0;
        }
    }
}
