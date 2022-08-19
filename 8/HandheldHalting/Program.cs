using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HandheldHalting
{
    public class Program
    {
        static async Task Main()
        {
            var instructions = InputReader.ReadInstructions("input.txt");
            var loopDetector = new LoopDetector(instructions);
            var result = loopDetector.FindInftyLoop();

            Console.WriteLine($"accumulator value before infinity loop: {result.AccumulatorValue}");

            var jumps = instructions.Where(instruction => instruction.Operator == Operation.Jump).ToArray();
            var nops = instructions.Where(instruction => instruction.Operator == Operation.NoOperation).ToArray();

            var jmpTask = Task.Run(() =>
            {
                foreach (var jump in jumps)
                {
                    var jmpLoopDetector = new LoopDetector(instructions);
                    jmpLoopDetector.CorrectJumpInstruction(jump);
                    var jmpResult = jmpLoopDetector.FindInftyLoop();

                    if (!jmpResult.HasInfinityLoop)
                    {
                        Console.WriteLine($"Terminating Program achieved - Wrong Jump -  Accumulator Value is {jmpResult.AccumulatorValue}");
                        return true;
                    }
                }
                return false;
            });

            var nopTask = Task.Run(() =>
            {
                foreach (var nop in nops)
                {
                    var nopLoopDetector = new LoopDetector(instructions);
                    nopLoopDetector.CorrectNopInstruction(nop);
                    var nopResult = nopLoopDetector.FindInftyLoop();

                    if (!nopResult.HasInfinityLoop)
                    {
                        Console.WriteLine($"Terminating Program achieved - Wrong Nop - Accumulator Value is {nopResult.AccumulatorValue}");
                        return true;
                    }
                }
                return false;
            });

            var searchResult =  await Task.WhenAll(jmpTask, nopTask);

            if (searchResult.All( result => !result))
            {
                Console.WriteLine("No Solution found");
            }


        }
    }

    public class LoopDetector
    {
        private HashSet<int> visitedLineNumbers = new HashSet<int>();
        private Accumulator acc = new Accumulator();

        public int LastIndex = 0;
        private Instruction[] instructions;

        public LoopDetector(Instruction[] instructions)
        {
            this.instructions = instructions.Clone() as Instruction[];
        }

        public void Reset()
        {
            this.visitedLineNumbers = new HashSet<int>();
        }
        public int GetAccumulatorValue()
        {
            return acc.Value;
        }

        public SearchResult FindInftyLoop()
        {
            return ExecuteInstruction(0);
        }

        private SearchResult ExecuteInstruction(int index)
        {
            if (visitedLineNumbers.Contains(index))
            {
                return new SearchResult(acc.Value, index, true);
            }

            var instruction = instructions[index];
            int newIndex;
            LastIndex = index;

            switch (instruction.Operator)
            {
                case Operation.Accumulator:
                    acc.Modify(instruction);
                    visitedLineNumbers.Add(index);
                    newIndex = index + 1;
                    break;
                case Operation.NoOperation:
                    visitedLineNumbers.Add(index);
                    newIndex = index + 1;
                    break;
                case Operation.Jump:
                    visitedLineNumbers.Add(index);
                    if (instruction.Sign == Sign.Positive)
                    {
                        newIndex = index + instruction.Argument;
                    }
                    else
                    {
                        newIndex = index - instruction.Argument;
                    }
                    break;
                default:
                    throw new NotImplementedException("You fucked up");
            }

            if (newIndex >= instructions.Length)
            {
                return new SearchResult(acc.Value, index, false);
            }
            return ExecuteInstruction(newIndex);
        }

        internal void CorrectJumpInstruction(Instruction instruction)
        {
            instructions[instruction.Index] = new Instruction(Operation.NoOperation, Sign.Positive, 0, instruction.Index);
        }

        internal void CorrectNopInstruction(Instruction instruction)
        {
            instructions[instruction.Index] = new Instruction(Operation.Jump, instruction.Sign, instruction.Argument, instruction.Index);
        }
    }

    public class Accumulator
    {
        public int Value { get; private set; } = 0;

        public void Modify(Instruction instruction)
        {

            if (instruction.Sign == Sign.Positive)
            {
                Value += instruction.Argument;
            }
            else
            {
                Value -= instruction.Argument;
            }
        }
    }

    public static class InputReader
    {
        public static Instruction[] ReadInstructions(string inputfile)
        {

            var input = File.ReadAllLines(inputfile);

            return input.Select((line, index) =>
            {
                var rawFields = line.Trim().Split(' ');
                var opcode = Operation.NoOperation;

                if (rawFields[0] == "acc")
                {
                    opcode = Operation.Accumulator;
                }
                else if (rawFields[0] == "jmp")
                {
                    opcode = Operation.Jump;
                }

                var rawSign = rawFields[1].Substring(0, 1);
                var sign = Sign.Positive;
                if (rawSign == "-")
                    sign = Sign.Negative;
                var arg = int.Parse(rawFields[1].Substring(1));
                return new Instruction(opcode, sign, arg, index);
            }).ToArray();
        }
    }

    public readonly struct SearchResult
    {
        public SearchResult(int accumulatorValue, int lastIndex, bool hasInfinityLoop)
        {
            AccumulatorValue = accumulatorValue;
            LastIndex = lastIndex;
            HasInfinityLoop = hasInfinityLoop;
        }

        public readonly int AccumulatorValue { get; }
        public readonly int LastIndex { get; }
        public readonly bool HasInfinityLoop { get; }
    }

    public readonly struct Instruction
    {
        public readonly Operation Operator { get; }
        public readonly Sign Sign { get; }
        public readonly int Argument { get; }
        public readonly int Index { get; }

        public Instruction(Operation operation, Sign sign, int argument, int index)
        {
            Operator = operation;
            Sign = sign;
            Argument = argument;
            Index = index;
        }
    }

    public enum Operation
    {
        Accumulator,
        NoOperation,
        Jump
    }

    public enum Sign
    {
        Positive,
        Negative
    }
}
