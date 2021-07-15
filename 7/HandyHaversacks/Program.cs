using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace HandyHaversacks
{
    public static class Program
    {
        public static void Main()
        {
            // input2: 1 directly, 6 indirect
            var reader = new InputReader();
            var bags = reader.Read("input.txt");
            var bagChecker = new BagChecker(bags);
            Console.WriteLine($"------------- Done reading {bags.Length} bags -------------");
            var contains = bagChecker.SearchBags();
            Console.Write("\r\n");
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine($"Bags that contain: {contains.Length}");
            foreach (var containedBag in contains)
            {
                Console.Write(containedBag.Name + ", ");
            }
            Console.Write("\r\n");
            Console.WriteLine("------------------------------------------------------------");
            var shinyGoldBag = bags.First(bag => bag.Name == "shiny gold");
            int containedBags = bagChecker.CountContainedBags(shinyGoldBag);
            Console.WriteLine($"{shinyGoldBag.Name} contains {containedBags} other bags");
        }
    }

    public class BagChecker
    {
        private readonly Bag[] bags;

        public BagChecker(Bag[] bags)
        {
            this.bags = bags;
        }

        public Bag[] SearchBags()
        {
            return bags.Where(bag => ContainsShiny(bag)).ToArray();
        }

        public bool ContainsShiny(Bag bag)
        {
            Console.Write(".");
            if (bag.Name == "shiny gold")
            {
                return false;
            }
            else if (bag.Content == null)
            {
                return false;
            }
            else if (bag.Content.Any(content => content.Bag.Name == "shiny gold"))
            {
                return true;
            }
            else
            {
                foreach (var content in bag.Content)
                {
                    var result = ContainsShiny(content.Bag);
                    if (result)
                    {
                        return result;
                    }
                }
            }

            return false;

        }

        internal int CountContainedBags(Bag bag)
        {
            var count = 0;

            if (bag.Content == null)
            {
                return 0;
            }

            foreach (var content in bag.Content)
            {
                count += content.Amount;
                if (content.Amount != 0)
                {
                    count += content.Amount * CountContainedBags(content.Bag);
                }
            }

            return count;
        }
    }

    public static class LinqExtension
    {
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> items, int maxItems)
        {
            return items.Select((item, inx) => new { item, inx })
                        .GroupBy(x => x.inx / maxItems)
                        .Select(g => g.Select(x => x.item));
        }


    }

    public class InputReader
    {

        private readonly Dictionary<string, Bag> listOfBags = new();
        public Bag AddBag(string rawType, string rawColor)
        {
            var key = rawType + " " + rawColor;
            var contains = listOfBags.TryGetValue(key, out var bag);

            if (!contains)
            {
                bag = new Bag(rawType, rawColor);
                listOfBags.Add(key, bag);
            }

            return bag;

        }
        public Bag[] Read(string inputfile)
        {
            var rgx = new Regex("(,|\\.|contain\\s|\\s?bags?)");

            var input = File.ReadAllLines(inputfile);


            return input.Select(line =>
            {
                var strippedLine = rgx.Replace(line, String.Empty).TrimEnd();
                var rawFields = strippedLine.Trim().Split(' ');
                // dotted blue bags contain 3 wavy bronze bags, 5 clear tomato bags.
                //"dotted blue 3 wavy bronze 5 clear tomato"

                var bag = AddBag(rawFields[0], rawFields[1]);

                if (rawFields[2] != "no")
                {
                    var bagcontent = new List<BagContent>();
                    foreach (var content in rawFields[2..].Batch(3))
                    {
                        var bc = content.ToArray();
                        var containedBag = AddBag(bc[1], bc[2]);
                        var bagContent = new BagContent(int.Parse(bc[0]), containedBag);
                        bagcontent.Add(bagContent);
                    }
                    bag.Content = bagcontent.ToArray();
                }
                return bag;

            }).ToArray();

        }

    }

    public class Bag
    {
        public string Name { get; set; }
        public BagContent[] Content { get; set; }

        public Bag(string type, string color)
        {
            this.Name = type + " " + color;
        }
    }

    public readonly struct BagContent
    {
        public int Amount { get; }
        public Bag Bag { get; }

        public BagContent(int amount, Bag bag)
        {
            this.Amount = amount;
            this.Bag = bag;
        }
    }
}
