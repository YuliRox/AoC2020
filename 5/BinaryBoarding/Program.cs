using System;
using System.IO;
using System.Collections;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace BinaryBoarding
{
    public readonly ref struct Ticket
    {
        public Ticket(ReadOnlySpan<char> rawTicket)
        {
            FB = rawTicket[0..7];
            RL = rawTicket[7..];
        }

        public ReadOnlySpan<char> FB { get; }
        public ReadOnlySpan<char> RL { get; }
    }

    public static class Program
    {
        public static void Main(string[] _)
        {
            using var fileReader = new StreamReader(new FileStream("../input.txt", FileMode.Open));

            Span<char> buffer = stackalloc char[11];
            int read = 0;
            var maxId = 0;

            var seatIdList = new List<int>();


            while ((read = fileReader.Read(buffer)) != 0)
            {
                Ticket x = new(buffer[..^1]);
                var seatId = x.GetSeatId();
                seatIdList.Add(seatId);
                //Console.WriteLine($"{x.FB.ToString()}|{x.RL.ToString()}|{x.GetRow()}|{x.GetColumn()}|{seatId}");

                if (seatId > maxId)
                    maxId = seatId;
            }

            var positiveList = Enumerable.Range(seatIdList.Min(),seatIdList.Max()-seatIdList.Min()).Except(seatIdList).ToArray();

            Console.WriteLine($"The maximum seatId is: {maxId}");

            foreach (var hole in positiveList)
            {
                Console.WriteLine($"Seat Candidate: {hole}");
            }

            /*ReadOnlySpan<char> testInput = "FBFBBFFRLR";
            Ticket x = new(testInput);
            Console.WriteLine($"{x.FB.ToString()}|{x.RL.ToString()}|{x.GetRow()}|{x.GetColumn()}|{x.GetSeatId()}");*/
        }

        public static int GetRow(this ref Ticket ticket)
        {
            var min = 0;
            var max = 128;

            for (var index = 0; index < ticket.FB.Length; index++)
            {
                if (ticket.FB[index] == 'F')
                {
                    max = min + ((max - min) / 2);
                }
                else if (ticket.FB[index] == 'B')
                {
                    min = min + ((max - min) / 2);
                }
                //Console.WriteLine($"{ticket.FB[index]} {min:00} {max:00}");
            }

            return min;
        }

        public static int GetColumn(this ref Ticket ticket)
        {
            var min = 0;
            var max = 8;

            for (var index = 0; index < ticket.RL.Length; index++)
            {
                if (ticket.RL[index] == 'L')
                {
                    max = min + ((max - min) / 2);
                }
                else if (ticket.RL[index] == 'R')
                {
                    min = min + ((max - min) / 2);
                }

                //Console.WriteLine($"{ticket.RL[index]} {min:00} {max:00}");
            }

            return min;
        }

        public static int GetSeatId(this ref Ticket ticket)
        {
            return (ticket.GetRow() * 8) + ticket.GetColumn();
        }
    }
}
