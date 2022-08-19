using System;
using System.Collections.Generic;
using System.Linq;

namespace TicketScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting TicketScanner...");

            var reader = new InputReader();

            var parameters = reader.ReadConfig("input.txt");

            var valueValidator = new TicketValueValidator(TicketValueValidator.CreateInvalidRanges(parameters.NamedRanges.SelectMany(nR => nR.Ranges)));
            var aggValidator = new AggregatingTicketValueValidator(valueValidator);
            var ticketValidator = new TicketValidator(aggValidator);

            var validTickets = parameters.Tickets.Where(ticketValidator.isValid).ToArray();

            Console.WriteLine($"Ticket Scanning Error Rate: {aggValidator.ErrorRate}");

            var puzzleParameters = new Parameters(parameters.NamedRanges, parameters.MyTicket, validTickets);


            var fieldSolver = new FieldSolver();

            var solution = fieldSolver.Solve(puzzleParameters);
            ulong departureSolution = 1;

            for (var position = 0; position < solution.Length; position++)
            {
                Console.WriteLine($"{position + 1}-> {solution[position].Name}");

                if (solution[position].Name.StartsWith("departure"))
                {
                    departureSolution = departureSolution * (ulong) puzzleParameters.MyTicket.TicketValues[position];
                }

            }

            Console.WriteLine($"Departures: {departureSolution}");
        }
    }
}
