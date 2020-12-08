using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using OneOf;

public class Day8
{
    private IReadOnlyCollection<string> inputLines;

    [SetUp]
    public async Task Setup()
    {
        inputLines = await System.IO.File.ReadAllLinesAsync("input.txt");
    }

    [Test(ExpectedResult = -1)]
    public int Part1()
    {
        var instructions = inputLines.Where(l => !string.IsNullOrWhiteSpace(l)).Select((l, i) => Instructions.ParseLine(i, l)).ToImmutableList();

        int? FindAccumulatorAtFirstCycle()
        {
            var instructionsExecuted = new HashSet<Instructions.Instruction>();
            int accumulator = 0;

            for (var executionIndex = 0; executionIndex < instructions.Count; executionIndex++)
            {
                var instruction = instructions[executionIndex];
                if (instructionsExecuted.Contains((Instructions.Instruction)instruction.Value))
                {
                    return accumulator;
                }
                else
                {
                    instructionsExecuted.Add((Instructions.Instruction)instruction.Value);
                }

                instruction.Switch(
                    acc => accumulator += acc.argument,
                    jmp => executionIndex = (executionIndex + jmp.argument) - 1,
                    _ => { });
            }

            return null;
        }

        return FindAccumulatorAtFirstCycle().Value;
    }
}

public static class Instructions
{
    public static OneOf<AccInstruction, JmpInstruction, NopInstruction> ParseLine(int index, string value)
    {
        var instructionArgument = int.Parse(value.Substring(4));
        return value.Substring(0, 3) switch
        {
            "acc" => new AccInstruction(index, instructionArgument),
            "jmp" => new JmpInstruction(index, instructionArgument),
            "nop" => new NopInstruction(index),
            _ => throw new ArgumentException(nameof(value))
        };
    }

    public record Instruction(int index);

    public record ParameterizedInstruction : Instruction
    {
        public int argument { get; init; }

        public ParameterizedInstruction(int index, int argument) : base(index)
        {
            this.argument = argument;
        }
    }

    public record AccInstruction : ParameterizedInstruction
    {
        public AccInstruction(int index, int argument)
            : base(index, argument)
        {
        }
    }

    public record JmpInstruction : ParameterizedInstruction
    {
        public JmpInstruction(int index, int argument)
            : base(index, argument)
        {
        }
    }

    public record NopInstruction : Instruction
    {
        public NopInstruction(int index)
            : base(index)
        {
        }
    }
}
