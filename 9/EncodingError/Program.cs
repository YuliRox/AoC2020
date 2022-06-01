using System;
using System.IO;
using System.Linq;

namespace EncodingError
{
    class Program
    {
        public static string _inputPath = "C:\\Users\\sschuhma\\Documents\\Projekte\\Spielprojekte\\AdventOfCode\\9\\EncodingError\\input.txt";

        public static int preambleLength = 25;
        static void Main(string[] args)
        {
            var xmasData = InputReader.Read(_inputPath);
            var decryptor = new XmasDecryptor(preambleLength, xmasData);
            var range = Enumerable.Range(preambleLength + 1, xmasData.Count()).ToArray();

            foreach (var index in range)
            {
                if(!decryptor.IsValidNumber(index)){
                    Console.WriteLine($"First invalid Number found at position {index}!");
                    return;
                }
            }
        }
    }

    public static class InputReader
    {
        public static long[] Read(string inputfile)
        {
            return File.ReadAllLines(inputfile).Select(rawNumber => long.Parse(rawNumber)).ToArray();
        }
    }

    public class XmasDecryptor
    {
        private readonly int preambleLength;
        private readonly long[] data;

        public XmasDecryptor(int preambleLength, long[] data)
        {
            this.preambleLength = preambleLength;
            this.data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public long[] GetPreambleNumbers(int index)
        {
            var start = 0;
            if (index >= preambleLength)
            {
                start = index - preambleLength;
            }
            return data.Skip(start).Take(preambleLength).ToArray();
        }

        public bool IsValidNumber(int index)
        {
            if (index <= preambleLength)
            {
                throw new ArgumentException("index must be greater then preamble");
            }

            var validationNumber = data[index];
            Console.Write($"Checking if Number {validationNumber} is valid: ");
            var precedingNumbersWindow = GetPreambleNumbers(index);

            foreach (var windowNumber in precedingNumbersWindow)
            {
                var result = validationNumber - windowNumber;

                if (precedingNumbersWindow.Contains(result))
                {
                    Console.WriteLine(" Yes!");
                    return true;
                }

            }

            Console.WriteLine(" No!");
            return false;
        }
    }
}
