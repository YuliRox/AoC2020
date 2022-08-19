using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace PassportProcessing
{
    public static class Program
    {
        private static void Main(string[] _)
        {
            var ir = new InputReader();
            var passports = ir.ReadInput("input.txt");

            var validPassports = passports.Where(passport => passport.IsValid());
            Console.WriteLine($"Valid Passports: {validPassports.Count()}");

            var moreValidPassports = validPassports.Where(passport => passport.IsMoreValid());
            Console.WriteLine($"More valid Passports: {moreValidPassports.Count()}");

            /*foreach (var p in moreValidPassports.OrderBy(x => x.IssueYear))
            {
                Console.WriteLine(p);
            }*/
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

        public static bool IsHeightValid(this Passport passport)
        {
            if (passport.Height == null)
                return false;

            var heightPattern = new Regex(@"(\d+)(cm|in)");
            if (!heightPattern.IsMatch(passport.Height))
            {
                return false;
            }

            var matches = heightPattern.Match(passport.Height);
            var decimalHeight = int.Parse(matches.Groups[1].Value);
            if (matches.Groups[2].Value == "cm")
            {
                if (decimalHeight < 150 || decimalHeight > 193)
                    return false;
            }
            else if (matches.Groups[2].Value == "in")
            {
                if (decimalHeight < 59 || decimalHeight > 76)
                    return false;
            }
            return true;
        }

        public static bool IsHairColorValid(this Passport passport)
        {
            if (passport.HairColor == null)
                return false;

            var hairPattern = new Regex("#[a-f0-9]{6}");
            return hairPattern.IsMatch(passport.HairColor);
        }

        public static bool IsEyeValid(this Passport passport)
        {
            if (passport.EyeColor == null)
                return false;
            var allowedEye = new string[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
            return allowedEye.Contains(passport.EyeColor);
        }
        public static bool IsPidValid(this Passport passport)
        {
            if (passport.PassportId == null)
                return false;

            var pidPattern = new Regex(@"\d{9}");
            return pidPattern.IsMatch(passport.PassportId) && passport.PassportId.Length == 9;
        }

        public static bool IsNumberValid(this Passport passport, Func<Passport, int?> selector, int min, int max)
        {
            var number = selector(passport);
            if (number == null)
                return false;
            return number >= min && number <= max;
        }

        public static bool IsMoreValid(this Passport passport)
        {
            if (!passport.IsValid())
                return false;

            if (!passport.IsHeightValid())
                return false;

            if (!passport.IsHairColorValid())
                return false;

            if (!passport.IsEyeValid())
                return false;

            if (!passport.IsPidValid())
                return false;

            if (!passport.IsNumberValid(x => x.BirthYear, 1920, 2002))
                return false;

            if (!passport.IsNumberValid(x => x.IssueYear, 2010, 2020))
                return false;

            if (!passport.IsNumberValid(x => x.ExpirationYear, 2020, 2030))
                return false;

            return true;
        }
    }

    public readonly struct Passport
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(BirthYear);
            sb.Append("|");
            sb.Append(IssueYear);
            sb.Append("|");
            sb.Append(ExpirationYear);
            sb.Append("|");
            sb.Append(Height.PadLeft(5));
            sb.Append("|");
            sb.Append(HairColor);
            sb.Append("|");
            sb.Append(EyeColor);
            sb.Append("|");
            sb.Append(PassportId);
            sb.Append("|");
            sb.Append(CountryId);

            return sb.ToString();
        }
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
