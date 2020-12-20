using System;
using System.IO;
using System.Linq;

namespace PasswordPhilosophy
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputReader = new InputReader();
            var pwList = inputReader.ReadInput("input.txt");

            var validPasswords = pwList.Count( pw => pw.policy.IsValidPassword(pw.password));
            Console.WriteLine($"Valid Passwords: {validPasswords}");

        }
    }

    public class InputReader
    {
        public PwdParams[] ReadInput(string inputFile)
        {
            var input = File.ReadAllLines(inputFile);

            return input.Select(line =>
            {
                var rawFields = line.Trim().Split(' ');
                var range = rawFields[0].Split('-').Select(Int32.Parse).ToArray();
                var character = rawFields[1].Split(':').First();

                var pwdPolicy = new PwPolicy(range[0], range[1], character[0]);
                return new PwdParams(pwdPolicy, rawFields[2]);
            }
            ).ToArray();
        }

    }

    public readonly struct PwdParams
    {
        public readonly PwPolicy policy;
        public readonly string password;

        public PwdParams(PwPolicy Policy, string Password)
        {
            policy = Policy;
            password = Password;
        }

    }

    public readonly struct PwPolicy
    {
        public readonly int Min;
        public readonly int Max;
        public readonly char Character;

        public PwPolicy(int min, int max, char character)
        {
            Min = min;
            Max = max;
            Character = character;
        }
    }

    public static class PwPolicyExtension {

        public static bool IsValidPassword ( this PwPolicy policy, string password){

                var cCount = password.Count( character => character == policy.Character );

                if ( policy.InRange(cCount))
                    return true;

                return false;
        }

        public static bool InRange ( this PwPolicy policy, int count){
            if ( count >= policy.Min && count <= policy.Max)
                return true;
            
            return false;
        }
    }
}
