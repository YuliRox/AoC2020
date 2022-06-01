using System;
using Xunit;
using EncodingError;
using System.Linq;

namespace EncodingErrorTest
{
    public class TestReadInput
    {
        public static string _inputPath = "C:\\Users\\sschuhma\\Documents\\Projekte\\Spielprojekte\\AdventOfCode\\9\\EncodingErrorTest\\sample.txt";
        [Fact]
        public void VerifyInputReading()
        {
            var xmasData = InputReader.Read(_inputPath);
            Assert.IsType<long[]>(xmasData);
            Assert.Equal(20, xmasData.Length);
        }

        [Fact]
        public void VerifyPreamble()
        {
            var xmasData = InputReader.Read(_inputPath);
            var decryptor = new XmasDecryptor(5, xmasData);

            long[] startPreamble = { 35, 20, 15, 25, 47 };
            long[] index10Preamble = { 47, 40, 62, 55, 65 };

            Assert.True(startPreamble.SequenceEqual(decryptor.GetPreambleNumbers(0)));
            Assert.True(startPreamble.SequenceEqual(decryptor.GetPreambleNumbers(1)));
            Assert.True(startPreamble.SequenceEqual(decryptor.GetPreambleNumbers(2)));
            Assert.True(startPreamble.SequenceEqual(decryptor.GetPreambleNumbers(3)));
            Assert.True(startPreamble.SequenceEqual(decryptor.GetPreambleNumbers(4)));
            Assert.True(index10Preamble.SequenceEqual(decryptor.GetPreambleNumbers(9)));
        }

        [Fact]
        public void VerifyKnownNumbersValidation(){

            var xmasData = InputReader.Read(_inputPath);
            var decryptor = new XmasDecryptor(5, xmasData);

            var knownTrueNumber = decryptor.IsValidNumber(10);
            Assert.True(knownTrueNumber);

            var knownFalseNumber = decryptor.IsValidNumber(14);
            Assert.False(knownFalseNumber);
        }

        [Fact]
        public void VerifySetOfSummands(){
            var expectedSet = new long[]{15,25,47,40};

            var xmasData = InputReader.Read(_inputPath);
            var decryptor = new XmasDecryptor(5, xmasData);

            var setOfSummands = decryptor.GetSetOfSummands(14);

            Assert.True(expectedSet.SequenceEqual(setOfSummands));

        }

        [Fact]
        public void VerifyMinMax()
        {
            var testSet = new long[] { 15, 25, 47, 40 };

            var minMaxValue = XmasDecryptor.GetMinMaxValue(testSet);

            Assert.Equal(62, minMaxValue);
        }
    }
}
