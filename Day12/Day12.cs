using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NUnit.Framework;
using OrientationAdjustmentInstruction = OneOf.OneOf<Day12.MoveNorth, Day12.MoveSouth, Day12.MoveEast, Day12.MoveWest, Day12.TurnLeft, Day12.TurnRight, Day12.MoveForward>;

public class Day12
{
    private static readonly float PiOver180 = (float)(Math.PI / 180d);

    private IReadOnlyCollection<string> inputLines;

    public record MoveNorth(float distance);
    public record MoveSouth(float distance);
    public record MoveEast(float distance);
    public record MoveWest(float distance);
    public record TurnLeft(float degrees);
    public record TurnRight(float degrees);
    public record MoveForward(float distance);

    public record Orientation(Vector2 position, float rotation);
    public record Velocity(Orientation orientation, float waypointDistance);

    [SetUp]
    public async Task Setup()
    {
        inputLines = await System.IO.File.ReadAllLinesAsync("input.txt");
    }

    [Test(ExpectedResult = 1838)]
    public int Part1()
    {
        var initialBoatOrientation = new Orientation(Vector2.Zero, CompassDegreesToCircleDegrees(90));

        var motionInstructions = ParseInput();

        var finalBoatOrientation = motionInstructions.Aggregate(initialBoatOrientation, ApplyPart1OrientationAdjustment);

        return ManhattenDistanceFromOrigin(finalBoatOrientation.position);
    }

    [Test(ExpectedResult = 89936)]
    public int Part2()
    {
        var initialBoatPosition = Vector2.Zero;
        var initialRelativeWaypointPosition = new Vector2(10, 1);
        var initialRelativeWaypointBearing = CartesianToPolar(initialRelativeWaypointPosition);
        var startVelocity = new Velocity(new Orientation(initialBoatPosition, initialRelativeWaypointBearing.angleDegrees), initialRelativeWaypointBearing.magnitude);

        var motionInstructions = ParseInput();

        var finalVelocity = motionInstructions.Aggregate(startVelocity, ApplyPart2OrientationAdjustment);

        return ManhattenDistanceFromOrigin(finalVelocity.orientation.position);
    }

    public Velocity ApplyPart2OrientationAdjustment(Velocity startVelocity, OrientationAdjustmentInstruction instruction)
    {
        Velocity TranslateWaypointPosition(Vector2 offset)
        {
            var newWaypointBearing = CartesianToPolar(PolarToCartesian(startVelocity.orientation.rotation, startVelocity.waypointDistance) + offset);
            return new Velocity(new Orientation(startVelocity.orientation.position, newWaypointBearing.angleDegrees), newWaypointBearing.magnitude);
        }

        Velocity RotateWaypointAroundPosition(float angleDegrees)
        {
            var newWaypointBearing = (angleDegrees: startVelocity.orientation.rotation + angleDegrees, distance: startVelocity.waypointDistance);
            return new Velocity(new Orientation(startVelocity.orientation.position, newWaypointBearing.angleDegrees), newWaypointBearing.distance);
        }

        Velocity MoveTowardsWaypoint(float magnitude)
        {
            var waypointRelativeCoordinate = PolarToCartesian(startVelocity.orientation.rotation, startVelocity.waypointDistance);
            var newPosition = startVelocity.orientation.position + (waypointRelativeCoordinate * magnitude);
            return new Velocity(new Orientation(newPosition, startVelocity.orientation.rotation), startVelocity.waypointDistance);
        }

        var resultVelocity = instruction.Match(
            moveNorth => TranslateWaypointPosition(new Vector2(0, moveNorth.distance)),
            moveSouth => TranslateWaypointPosition(new Vector2(0, -moveSouth.distance)),
            moveEast => TranslateWaypointPosition(new Vector2(moveEast.distance, 0)),
            moveWest => TranslateWaypointPosition(new Vector2(-moveWest.distance, 0)),
            turnLeft => RotateWaypointAroundPosition(turnLeft.degrees),
            turnRight => RotateWaypointAroundPosition(-turnRight.degrees),
            moveForward => MoveTowardsWaypoint(moveForward.distance));

        Print(startVelocity, instruction, resultVelocity);

        return resultVelocity;
    }

    public Orientation ApplyPart1OrientationAdjustment(Orientation startOrientation, OrientationAdjustmentInstruction instruction)
    {
        var startPosition = startOrientation.position;
        var rotation = startOrientation.rotation;

        var resultPosition = startPosition + instruction.Match(
                moveNorth => new Vector2(0, moveNorth.distance),
                moveSouth => new Vector2(0, -moveSouth.distance),
                moveEast => new Vector2(moveEast.distance, 0),
                moveWest => new Vector2(-moveWest.distance, 0),
                turnLeft => Vector2.Zero,
                turnRight => Vector2.Zero,
                moveForward => PolarToCartesian(rotation, moveForward.distance));
        var resultRotation = rotation + instruction.Match(
                moveNorth => 0,
                moveSouth => 0,
                moveEast => 0,
                moveWest => 0,
                turnLeft => turnLeft.degrees,
                turnRight => -turnRight.degrees,
                moveForward => 0);

        var resultOrientation = new Orientation(resultPosition, resultRotation);
        Print(startOrientation, instruction, resultOrientation);

        return resultOrientation;
    }

    public static int ManhattenDistanceFromOrigin(Vector2 position) =>
        (int)Math.Round(Math.Abs(position.X) + Math.Abs(position.Y));

    public static float CompassDegreesToCircleDegrees(float degrees) =>
        degrees - 90;

    public static float DegreesToRadians(float degrees) =>
        degrees * PiOver180;

    public static float RadiansToDegrees(float radians) =>
        radians / PiOver180;

    public static Vector2 PolarToCartesian(float angleDegrees, float magnitude)
    {
        var angleRadians = DegreesToRadians(angleDegrees);
        return new Vector2(
            (float)Math.Cos(angleRadians) * magnitude,
            (float)Math.Sin(angleRadians) * magnitude);
    }

    public static (float angleDegrees, float magnitude) CartesianToPolar(Vector2 coordinate)
    {
        var angleRadians = Math.Atan2(coordinate.Y, coordinate.X);

        var angleDegrees = (float)RadiansToDegrees((float)angleRadians);
        var magnitude = (float)Math.Sqrt(Math.Pow(coordinate.X, 2) + Math.Pow(coordinate.Y, 2));

        return (angleDegrees, magnitude);
    }

    public IReadOnlyList<OrientationAdjustmentInstruction> ParseInput()
    {
        return (from line in inputLines
               where !string.IsNullOrWhiteSpace(line) && line.Length > 0
               let action = line[0]
               let actionParameter = int.Parse(line[1..])
#pragma warning disable SA1119 // Statement should not use unnecessary parenthesis
                select (OrientationAdjustmentInstruction)(action switch
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

    private void Print(Velocity velocityIn, OrientationAdjustmentInstruction orientationAdjustment, Velocity velocityOut)
    {
        static string FormatVelocity(Velocity v)
        {
            var o = v.orientation;
            var cb = PolarToCartesian(v.orientation.rotation, v.waypointDistance);
            return $"({o.position.X: 000;-000}, {o.position.Y: 000;-000})->({cb.X: 00;-00}, {cb.Y: 00;-00})";
        }

        TestContext.Out.Write(FormatVelocity(velocityIn));
        TestContext.Out.Write(" \t");
        TestContext.Out.Write(orientationAdjustment.Match(
                moveNorth => $"N{moveNorth.distance:00}",
                moveSouth => $"S{moveSouth.distance:00}",
                moveEast => $"E{moveEast.distance:00}",
                moveWest => $"W{moveWest.distance:00}",
                turnLeft => $"L{turnLeft.degrees:000}",
                turnRight => $"R{turnRight.degrees:000}",
                moveForward => $"F{moveForward.distance:00}"));
        TestContext.Out.Write(" \t");
        TestContext.Out.WriteLine(FormatVelocity(velocityOut));
    }

    private void Print(Orientation orientationIn, OrientationAdjustmentInstruction orientationAdjustment, Orientation orientationOut)
    {
        static string FormatOrientation(Orientation o) => $"({o.position.X: 000;-000}, {o.position.Y: 000;-000})@{o.rotation: 000;-000}°";

        TestContext.Out.Write(FormatOrientation(orientationIn));
        TestContext.Out.Write(" \t");
        TestContext.Out.Write(orientationAdjustment.Match(
                moveNorth => $"N{moveNorth.distance:00}",
                moveSouth => $"S{moveSouth.distance:00}",
                moveEast => $"E{moveEast.distance:00}",
                moveWest => $"W{moveWest.distance:00}",
                turnLeft => $"L{turnLeft.degrees:000}",
                turnRight => $"R{turnRight.degrees:000}",
                moveForward => $"F{moveForward.distance:00}"));
        TestContext.Out.Write(" \t");
        TestContext.Out.WriteLine(FormatOrientation(orientationOut));
    }
}
