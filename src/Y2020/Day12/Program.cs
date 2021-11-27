using OrientationAdjustmentInstruction = OneOf.OneOf<Program.MoveNorth, Program.MoveSouth, Program.MoveEast, Program.MoveWest, Program.TurnLeft, Program.TurnRight, Program.MoveForward>;

await NUnitApplication.CreateBuilder().Build().RunAsync();

public partial class Program
{
    private static readonly float PiOver180 = (float)(Math.PI / 180d);

    private IReadOnlyCollection<string> inputLines = null!;

    public readonly record struct MoveNorth(float Distance);
    public readonly record struct MoveSouth(float Distance);
    public readonly record struct MoveEast(float Distance);
    public readonly record struct MoveWest(float Distance);
    public readonly record struct TurnLeft(float Degrees);
    public readonly record struct TurnRight(float Degrees);
    public readonly record struct MoveForward(float Distance);

    public readonly record struct Orientation(Vector2 Position, float Rotation);
    public readonly record struct Velocity(Orientation Orientation, float WaypointDistance);

    [SetUp]
    public async Task Setup()
    {
        inputLines = await new InputFileFacadeFacade().ReadAllLinesAsync();
    }

    [Test(ExpectedResult = 1838)]
    public int Part1()
    {
        var initialBoatOrientation = new Orientation(Vector2.Zero, CompassDegreesToCircleDegrees(90));

        var motionInstructions = ParseInput();

        var finalBoatOrientation = motionInstructions.Aggregate(initialBoatOrientation, ApplyPart1OrientationAdjustment);

        return ManhattenDistanceFromOrigin(finalBoatOrientation.Position);
    }

    [Test(ExpectedResult = 89936)]
    public int Part2()
    {
        var initialBoatPosition = Vector2.Zero;
        var initialRelativeWaypointPosition = new Vector2(10, 1);
        var (angleDegrees, magnitude) = CartesianToPolar(initialRelativeWaypointPosition);
        var startVelocity = new Velocity(new Orientation(initialBoatPosition, angleDegrees), magnitude);

        var motionInstructions = ParseInput();

        var finalVelocity = motionInstructions.Aggregate(startVelocity, ApplyPart2OrientationAdjustment);

        return ManhattenDistanceFromOrigin(finalVelocity.Orientation.Position);
    }

    public Velocity ApplyPart2OrientationAdjustment(Velocity startVelocity, OrientationAdjustmentInstruction instruction)
    {
        Velocity TranslateWaypointPosition(Vector2 offset)
        {
            var newWaypointBearing = CartesianToPolar(PolarToCartesian(startVelocity.Orientation.Rotation, startVelocity.WaypointDistance) + offset);
            return new Velocity(new Orientation(startVelocity.Orientation.Position, newWaypointBearing.angleDegrees), newWaypointBearing.magnitude);
        }

        Velocity RotateWaypointAroundPosition(float angleDegrees)
        {
            var newWaypointBearing = (angleDegrees: startVelocity.Orientation.Rotation + angleDegrees, distance: startVelocity.WaypointDistance);
            return new Velocity(new Orientation(startVelocity.Orientation.Position, newWaypointBearing.angleDegrees), newWaypointBearing.distance);
        }

        Velocity MoveTowardsWaypoint(float magnitude)
        {
            var waypointRelativeCoordinate = PolarToCartesian(startVelocity.Orientation.Rotation, startVelocity.WaypointDistance);
            var newPosition = startVelocity.Orientation.Position + (waypointRelativeCoordinate * magnitude);
            return new Velocity(new Orientation(newPosition, startVelocity.Orientation.Rotation), startVelocity.WaypointDistance);
        }

        var resultVelocity = instruction.Match(
            moveNorth => TranslateWaypointPosition(new Vector2(0, moveNorth.Distance)),
            moveSouth => TranslateWaypointPosition(new Vector2(0, -moveSouth.Distance)),
            moveEast => TranslateWaypointPosition(new Vector2(moveEast.Distance, 0)),
            moveWest => TranslateWaypointPosition(new Vector2(-moveWest.Distance, 0)),
            turnLeft => RotateWaypointAroundPosition(turnLeft.Degrees),
            turnRight => RotateWaypointAroundPosition(-turnRight.Degrees),
            moveForward => MoveTowardsWaypoint(moveForward.Distance));

        Print(startVelocity, instruction, resultVelocity);

        return resultVelocity;
    }

    public Orientation ApplyPart1OrientationAdjustment(Orientation startOrientation, OrientationAdjustmentInstruction instruction)
    {
        var startPosition = startOrientation.Position;
        var rotation = startOrientation.Rotation;

        var resultPosition = startPosition + instruction.Match(
            moveNorth => new Vector2(0, moveNorth.Distance),
            moveSouth => new Vector2(0, -moveSouth.Distance),
            moveEast => new Vector2(moveEast.Distance, 0),
            moveWest => new Vector2(-moveWest.Distance, 0),
            turnLeft => Vector2.Zero,
            turnRight => Vector2.Zero,
            moveForward => PolarToCartesian(rotation, moveForward.Distance));
        var resultRotation = rotation + instruction.Match(
            moveNorth => 0,
            moveSouth => 0,
            moveEast => 0,
            moveWest => 0,
            turnLeft => turnLeft.Degrees,
            turnRight => -turnRight.Degrees,
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
            var o = v.Orientation;
            var cb = PolarToCartesian(v.Orientation.Rotation, v.WaypointDistance);
            return $"({o.Position.X: 000;-000}, {o.Position.Y: 000;-000})->({cb.X: 00;-00}, {cb.Y: 00;-00})";
        }

        TestContext.Out.Write(FormatVelocity(velocityIn));
        TestContext.Out.Write(" \t");
        TestContext.Out.Write(orientationAdjustment.Match(
            moveNorth => $"N{moveNorth.Distance:00}",
            moveSouth => $"S{moveSouth.Distance:00}",
            moveEast => $"E{moveEast.Distance:00}",
            moveWest => $"W{moveWest.Distance:00}",
            turnLeft => $"L{turnLeft.Degrees:000}",
            turnRight => $"R{turnRight.Degrees:000}",
            moveForward => $"F{moveForward.Distance:00}"));
        TestContext.Out.Write(" \t");
        TestContext.Out.WriteLine(FormatVelocity(velocityOut));
    }

    private void Print(Orientation orientationIn, OrientationAdjustmentInstruction orientationAdjustment, Orientation orientationOut)
    {
        static string FormatOrientation(Orientation o) => $"({o.Position.X: 000;-000}, {o.Position.Y: 000;-000})@{o.Rotation: 000;-000}°";

        TestContext.Out.Write(FormatOrientation(orientationIn));
        TestContext.Out.Write(" \t");
        TestContext.Out.Write(orientationAdjustment.Match(
            moveNorth => $"N{moveNorth.Distance:00}",
            moveSouth => $"S{moveSouth.Distance:00}",
            moveEast => $"E{moveEast.Distance:00}",
            moveWest => $"W{moveWest.Distance:00}",
            turnLeft => $"L{turnLeft.Degrees:000}",
            turnRight => $"R{turnRight.Degrees:000}",
            moveForward => $"F{moveForward.Distance:00}"));
        TestContext.Out.Write(" \t");
        TestContext.Out.WriteLine(FormatOrientation(orientationOut));
    }
}
