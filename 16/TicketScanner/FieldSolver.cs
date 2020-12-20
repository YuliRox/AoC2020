using System;
using System.Collections.Generic;
using System.Linq;

namespace TicketScanner
{

    public class FieldSolver
    {


        public NamedRange[] Solve(Parameters puzzleParams)
        {
            var ticketMatrix = puzzleParams.Tickets.Select(ticket => ticket.TicketValues).ToArray();
            var columnCandidates = new List<HashSet<NamedRange>>();

            var columns = ticketMatrix[0].Length;
            // iterate all columns
            for (var columnIndex = 0; columnIndex < columns; columnIndex++)
            {
                var columCanditate = new HashSet<NamedRange>();
                columnCandidates.Add(columCanditate);

                // check for each column all named ranges
                // OPT: eliminate named ranges which are obvious so far
                foreach (var namedRange in puzzleParams.NamedRanges)
                {
                    var rangeOk = true;
                    // check for a named range if its a valid solution candidate
                    for (var rowIndex = 0; rowIndex < ticketMatrix.Length; rowIndex++)
                    {
                        if (!namedRange.InRange(ticketMatrix[rowIndex][columnIndex]))
                        {
                            rangeOk = false;
                            break;
                        }
                    }
                    if (rangeOk)
                    {
                        // range is valid -> add to solution
                        columCanditate.Add(namedRange);
                    }
                }
            }

            var solution = new NamedRange[columns];
            var indexAsso = Enumerable
                                .Range(0, columns)
                                .Select(index => new
                                {
                                    Index = index,
                                    Candidates = columnCandidates[index]
                                }).ToArray();

            for (var eliminiation = 0; eliminiation < 100; eliminiation++)
            {
                var safeSolutions = indexAsso
                    .Where(x => x.Candidates.Count == 1)
                    .Select(x => new
                    {
                        x.Index,
                        Candidate = x.Candidates.First()
                    })
                    .ToArray();

                if (safeSolutions.Length == 0 && columnCandidates.All(x => x.Count == 0))
                {
                    // no more eliminiations and all columns are empty
                    Console.WriteLine($"Magic Puzzle Solving Algorithm ended after {eliminiation} eliminations");
                    return solution;
                }

                if (safeSolutions.Length == 0)
                    throw new Exception("No Solution");

                foreach (var safeSolution in safeSolutions)
                {
                    solution[safeSolution.Index] = safeSolution.Candidate;
                    foreach (var cc in columnCandidates)
                    {
                        if (cc.Contains(safeSolution.Candidate))
                        {
                            cc.Remove(safeSolution.Candidate);
                        }
                    }
                }
            }
            throw new Exception("No Solution - Iteration");
        }



        public int GetDepartureValues()
        {
            return 0;
        }
    }

}

/*



*/