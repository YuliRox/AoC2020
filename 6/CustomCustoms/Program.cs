using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace CustomCustoms
{
    public static class Program
    {
        static void Main(string[] _)
        {
            var ir = new InputReader();
            var customsQuestions = ir.ReadInput("input.txt");

            var yesCount = 0;
            var stopwatch = new Stopwatch();
            var stopwatch2 = new Stopwatch();

            foreach (var group in customsQuestions)
            {

                stopwatch.Start();
                var distinctYes = group.PositiveQuestions
                    .SelectMany(x => x)
                    .GroupBy(x => x)
                    .Select(x => new {x.Key, Count = x.Count()})
                    .Count(x => x.Count == group.PositiveQuestions.Length);
                stopwatch.Stop();
                yesCount += distinctYes;
            }

            Console.WriteLine($"Distinct Solution: {stopwatch.Elapsed}");


            /*foreach (var group in customsQuestions)
            {
                stopwatch2.Start();
                var distinctYes2 = group.PositiveQuestions
                    .SelectMany(x => x)
                    .GroupBy(x => x)
                    .ToHashSet().Count;
                stopwatch2.Stop();
            }
            Console.WriteLine($"HashSet Solution: {stopwatch2.Elapsed}");*/

            Console.WriteLine($"Sum of all yes per Group: {yesCount}");
        }
    }

    internal class InputReader
    {
        public CustomsQuestionnaireGroup[] ReadInput(string input)
        {
            using var textStream = new StreamReader(input);
            return ReadQuestionnaires(textStream);
        }

        private CustomsQuestionnaireGroup[] ReadQuestionnaires(StreamReader textStream)
        {
            var questionnaires = new List<CustomsQuestionnaireGroup>();
            CustomsQuestionnaireGroup? newCQGroup;

            while ((newCQGroup = ReadCustomsQuestionnaireGroup(textStream)) != null){
                questionnaires.Add(newCQGroup);
            }
            return questionnaires.ToArray();
        }

        private CustomsQuestionnaireGroup? ReadCustomsQuestionnaireGroup(StreamReader textStream)
        {
            string line;
            var positiveQuestions = new List<string>();

            while(!string.IsNullOrWhiteSpace(line = textStream.ReadLine()))
            {
                var cleanedLine = string.Concat(line.OrderBy(x => x).Distinct());
                positiveQuestions.Add(cleanedLine);
            }

            if (positiveQuestions.Count == 0){ return null; }

            return new CustomsQuestionnaireGroup(){ PositiveQuestions = positiveQuestions.ToArray()};

        }
    }

    internal class CustomsQuestionnaireGroup
    {
        public string[] PositiveQuestions {get; set;}
    }
}
