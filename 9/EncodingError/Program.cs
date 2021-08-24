using System;
using System.IO;
using System.Linq;

namespace EncodingError
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public static class InputReader
    {
        public static int[] Read (string inputfile){
            return File.ReadAllLines(inputfile).Select(rawNumber => int.Parse(rawNumber)).ToArray();
        }
    }

    public class XmasDecryptor {
        private readonly int preambleLength;
        private readonly int[] data;

        public XmasDecryptor(int preambleLength, int[] data)
        {
            this.preambleLength = preambleLength;
            this.data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public int[] GetPreambleNumbers(int index){
            var start = 0;
            if (index >= preambleLength){
                start = index - preambleLength;
            }
            return data.Skip(start).Take(preambleLength).ToArray();
        }
    }
}
