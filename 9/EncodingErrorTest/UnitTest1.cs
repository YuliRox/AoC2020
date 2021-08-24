using System;
using Xunit;
using EncodingError;
using System.Linq;

namespace EncodingErrorTest
{
    public class TestReadInput
    {
        [Fact]
        public void VerifyInputReading()
        {
            var xmasData = InputReader.Read("C:\\Users\\sschuhma.SVA\\Documents\\Projekte\\Spielprojekte\\AdventOfCode\\9\\EncodingErrorTest\\sample.txt");
            Assert.IsType<int[]>(xmasData);
            Assert.Equal(20, xmasData.Length);
        }

        [Fact]
        public void VerifyPreamble()
        {
            var xmasData = InputReader.Read("C:\\Users\\sschuhma.SVA\\Documents\\Projekte\\Spielprojekte\\AdventOfCode\\9\\EncodingErrorTest\\sample.txt");
            var decryptor = new XmasDecryptor(5, xmasData);

            int[] startPreamble = { 35, 20, 15, 25, 47 };
            int[] index10Preamble = { 47, 40, 62, 55, 65 };

            Assert.True(startPreamble.SequenceEqual(decryptor.GetPreambleNumbers(0)));
            Assert.True(startPreamble.SequenceEqual(decryptor.GetPreambleNumbers(1)));
            Assert.True(startPreamble.SequenceEqual(decryptor.GetPreambleNumbers(2)));
            Assert.True(startPreamble.SequenceEqual(decryptor.GetPreambleNumbers(3)));
            Assert.True(startPreamble.SequenceEqual(decryptor.GetPreambleNumbers(4)));
            Assert.True(index10Preamble.SequenceEqual(decryptor.GetPreambleNumbers(9)));
        }
    }
}
