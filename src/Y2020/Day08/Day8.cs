namespace AdventOfCode.Y2020.Day08;

public class Day8
{
    private IReadOnlyCollection<string> inputLines = null!;

    [SetUp]
    public async Task Setup()
    {
        inputLines = await new InputFileFacadeFacade().ReadAllLinesAsync();
    }

    [Test(ExpectedResult = 1384)]
    public int Part1()
    {
        var instructions = inputLines.Where(l => !string.IsNullOrWhiteSpace(l)).Select((l, i) => Instructions.ParseLine(i, l)).ToImmutableList();

        var executionResult = Execute(instructions);

        return executionResult.Match(
            normal => throw new InvalidOperationException(),
            cycled => cycled.accumulator);
    }

    [Test(ExpectedResult = 761)]
    public int Part2()
    {
        var originalInstructions = inputLines.Where(l => !string.IsNullOrWhiteSpace(l)).Select((l, i) => Instructions.ParseLine(i, l)).ToImmutableList();

        static OneOf<Instructions.AccInstruction, Instructions.JmpInstruction, Instructions.NopInstruction> Invert(OneOf<Instructions.AccInstruction, Instructions.JmpInstruction, Instructions.NopInstruction> instruction)
        {
            return instruction.Match<OneOf<Instructions.AccInstruction, Instructions.JmpInstruction, Instructions.NopInstruction>>(
                acc => acc,
                jmp => new Instructions.NopInstruction(jmp.index, jmp.argument),
                nop => new Instructions.JmpInstruction(nop.index, nop.argument));
        }

        for (var instructionIndexToChange = 0; instructionIndexToChange < originalInstructions.Count; instructionIndexToChange++)
        {
            var invertedInstruction = Invert(originalInstructions[instructionIndexToChange]);
            var newInstructionSet = originalInstructions
                .RemoveAt(instructionIndexToChange)
                .Insert(instructionIndexToChange, invertedInstruction);

            NormalTermination? terminationFound = null;
            var executionResult = Execute(newInstructionSet);
            executionResult.Switch(normal => terminationFound = normal, cycled => { });
            if (terminationFound != null)
            {
                return terminationFound.accumulator;
            }
        }

        Assert.Fail();
        return -1;
    }

    public static OneOf<NormalTermination, CycledTermination> Execute(ImmutableList<OneOf<Instructions.AccInstruction, Instructions.JmpInstruction, Instructions.NopInstruction>> instructions)
    {
        var instructionsExecuted = new HashSet<Instructions.Instruction>();
        int accumulator = 0;

        for (var executionIndex = 0; executionIndex < instructions.Count; executionIndex++)
        {
            var instruction = instructions[executionIndex];
            if (instructionsExecuted.Contains((Instructions.Instruction)instruction.Value))
            {
                return new CycledTermination(accumulator, (Instructions.Instruction)instruction.Value);
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

        return new NormalTermination(accumulator);
    }

    public record NormalTermination(int accumulator);
    public record CycledTermination(int accumulator, Instructions.Instruction revisitedInstruction);
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
            "nop" => new NopInstruction(index, instructionArgument),
            _ => throw new ArgumentException(nameof(value))
        };
    }

    public record Instruction(int index, int argument);

    public record AccInstruction : Instruction
    {
        public AccInstruction(int index, int argument)
            : base(index, argument)
        {
        }
    }

    public record JmpInstruction : Instruction
    {
        public JmpInstruction(int index, int argument)
            : base(index, argument)
        {
        }
    }

    public record NopInstruction : Instruction
    {
        public NopInstruction(int index, int argument)
            : base(index, argument)
        {
        }
    }
}
