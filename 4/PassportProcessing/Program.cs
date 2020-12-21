using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace PassportProcessing
{
    public static class Program
    {
        private static void Main(string[] _)
        {

            var ir = new InputReader();
            var passports = ir.ReadInput("input");

            var validPassports = passports.Count(passport => passport.IsValid());
            Console.WriteLine($"Valid Passports: {validPassports}");

        }
    }

    public class InputReader
    {

        public Passport[] ReadInput(string input)
        {
            using var textStream = new StreamReader(input);

            return ReadPassports(textStream);

        }

        public Passport[] ReadPassports(StreamReader sr)
        {
            var passports = new List<Passport>();
            Passport? newPassport;

            while ((newPassport = ReadPassport(sr)) != null)
            {
                passports.Add(newPassport.Value);
            }

            return passports.ToArray();

        }

        public Passport? ReadPassport(StreamReader sr)
        {
            string line;
            var rawPassport = new StringBuilder();
            while (!String.IsNullOrWhiteSpace(line = sr.ReadLine()))
            {
                rawPassport.Append(line.TrimEnd()).Append(" ");
            }

            if (rawPassport.Length <= 1)
            {
                return null;
            }

            var passportValue = rawPassport.ToString().TrimEnd();
            var rawFields = passportValue.Split(" ").Select(field => field.Split(":")).ToDictionary(key => key[0], value => value[1]);

            int? ToInt(string field)
            {
                var fieldValue = rawFields.GetValueOrDefault(field);
                if (Int32.TryParse(fieldValue, out var outVar))
                    return outVar;
                return null;

            }

            return new Passport(
                ToInt("byr"),
                ToInt("iyr"),
                ToInt("eyr"),
                rawFields.GetValueOrDefault("hgt"),
                rawFields.GetValueOrDefault("hcl"),
                rawFields.GetValueOrDefault("ecl"),
                rawFields.GetValueOrDefault("pid"),
                ToInt("cid")
            );

        }

    }

    public static class PassportValidator
    {
        public static bool IsValid(this Passport passport)
        {
            return passport.BirthYear.HasValue &&
                   passport.IssueYear.HasValue &&
                   passport.ExpirationYear.HasValue &&
                   passport.Height != null &&
                   passport.HairColor != null &&
                   passport.EyeColor != null &&
                   passport.PassportId != null;
        }
    }

    public readonly struct Passport
    {
#nullable enable
        public Passport(int? birthYear, int? issueYear, int? expirationYear, string? height, string? hairColor, string? eyeColor, string? passportId, int? countryId)
        {
            BirthYear = birthYear;
            IssueYear = issueYear;
            ExpirationYear = expirationYear;
            Height = height;
            HairColor = hairColor;
            EyeColor = eyeColor;
            PassportId = passportId;
            CountryId = countryId;
        }

        public int? BirthYear { get; }
        public int? IssueYear { get; }
        public int? ExpirationYear { get; }
        public string? Height { get; }
        public string? HairColor { get; }
        public string? EyeColor { get; }
        public string? PassportId { get; }
        public int? CountryId { get; }
#nullable restore
    }


}
