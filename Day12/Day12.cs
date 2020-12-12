using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using MotionInstruction = OneOf.OneOf<Day12.MoveNorth, Day12.MoveSouth, Day12.MoveEast, Day12.MoveWest, Day12.TurnLeft, Day12.TurnRight, Day12.MoveForward>;

public class Day12
{
    private IReadOnlyCollection<string> inputLines;

    public record MoveNorth(int distance);
    public record MoveSouth(int distance);
    public record MoveEast(int distance);
    public record MoveWest(int distance);
    public record TurnLeft(int degrees);
    public record TurnRight(int degrees);
    public record MoveForward(int distance);

    public record BoatOrientation(int x, int y, int rotation);

    [SetUp]
    public async Task Setup()
    {
        inputLines = await System.IO.File.ReadAllLinesAsync("input.txt");
    }

    [Test(ExpectedResult = 1838)]
    public int Part1()
    {
        var initialOrientation = new BoatOrientation(0, 0, CompassDegreesToCircleDegrees(90));

        var motionInstructions = ParseInput();

        var finalOrientation = motionInstructions.Aggregate(initialOrientation, ApplyMotion);

        return Math.Abs(finalOrientation.x) + Math.Abs(finalOrientation.y);
    }

    private static readonly double PiOver180 = Math.PI / 180f;
    public BoatOrientation ApplyMotion(BoatOrientation startOrientation, MotionInstruction instruction)
    {
        static double DegreesToRadians(int degrees)
        {
            return degrees * PiOver180;
        }

        var (x, y, rotation) = startOrientation;
        var resultOrientation = instruction.Match(
                moveNorth => new BoatOrientation(x, y + moveNorth.distance, rotation),
                moveSouth => new BoatOrientation(x, y - moveSouth.distance, rotation),
                moveEast => new BoatOrientation(x + moveEast.distance, y, rotation),
                moveWest => new BoatOrientation(x - moveWest.distance, y, rotation),
                turnRight => new BoatOrientation(x, y, (rotation + turnRight.degrees) % 360),
                turnLeft => new BoatOrientation(x, y, (rotation - turnLeft.degrees) % 360),
                moveForward =>
                {
                    var newX = x + (int)Math.Round(Math.Cos(DegreesToRadians(rotation)) * moveForward.distance);
                    var newY = y + (int)Math.Round(Math.Sin(DegreesToRadians(rotation)) * moveForward.distance);
                    return new BoatOrientation(newX, newY, rotation);
                });
        Print(startOrientation, instruction, resultOrientation);
        return resultOrientation;
    }

    public int CompassDegreesToCircleDegrees(int degrees)
    {
        return degrees - 90;
    }

    public IReadOnlyList<MotionInstruction> ParseInput()
    {
        return (from line in inputLines
               where !string.IsNullOrWhiteSpace(line) && line.Length > 0
               let action = line[0]
               let actionParameter = int.Parse(line[1..])
#pragma warning disable SA1119 // Statement should not use unnecessary parenthesis
                select (MotionInstruction)(action switch
               {
                   'N' => new MoveNorth(actionParameter),
                   'S' => new MoveSouth(actionParameter),
                   'E' => new MoveEast(actionParameter),
                   'W' => new MoveWest(actionParameter),
                   'L' => new TurnLeft(actionParameter),
                   'R' => new TurnRight(actionParameter),
                   'F' => new MoveForward(actionParameter),
                   _ => throw new InvalidOperationException()
               })).ToImmutableList();
#pragma warning restore SA1119 // Statement should not use unnecessary parenthesis
    }

    private void Print(BoatOrientation boatOrientationIn, MotionInstruction motionInstruction, BoatOrientation boatOrientationOut)
    {
        static string FormatOrientation(BoatOrientation bo) => $"({bo.x: 000;-000}, {bo.y: 000;-000})@{bo.rotation: 000;-000}°";

        TestContext.Out.Write(FormatOrientation(boatOrientationIn));
        TestContext.Out.Write(" \t");
        TestContext.Out.Write(motionInstruction.Match(
                moveNorth => $"N{moveNorth.distance:00}",
                moveSouth => $"S{moveSouth.distance:00}",
                moveEast => $"E{moveEast.distance:00}",
                moveWest => $"W{moveWest.distance:00}",
                turnLeft => $"L{turnLeft.degrees:000}",
                turnRight => $"R{turnRight.degrees:000}",
                moveForward => $"F{moveForward.distance:00}"));
        TestContext.Out.Write(" \t");
        TestContext.Out.WriteLine(FormatOrientation(boatOrientationOut));
    }
}
