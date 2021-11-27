using System;
using System.Linq;
using OneOf;
using BottomOfMap = AdventOfCode.Y2020.Shared.Mapping.ISemisesquiBoundedInfiniteIntegralPlane<char>.Boundary;
using TobogganPosition = AdventOfCode.Y2020.Shared.Mapping.ISemisesquiBoundedInfiniteIntegralPlane<char>.IPlanchette;

public record Slope(int right, int down);

public static class TobogganPositionExtensions
{
    public static OneOf<TobogganPosition, BottomOfMap> Slide(this TobogganPosition self, Slope slope)
    {

        var result = OneOf<TobogganPosition, BottomOfMap>.FromT0(Enumerable.Range(0, slope.right).Aggregate(self, (p, _) => p.MoveRight()));
        return Enumerable.Range(0, slope.down).Aggregate(result, (p, _) => p.Match(p => p.MoveDown(), p => p));
    }

    public static void RepeatedlySlideToBottomOfMap(this TobogganPosition self, Slope slope, Action<TobogganPosition> actionBetweenEachSlide)
    {
        var tobogganPosition = OneOf<TobogganPosition, BottomOfMap>.FromT0(self);
        do
        {
            tobogganPosition = tobogganPosition.Match(tp => tp.Slide(slope), _ => throw new InvalidOperationException());

            tobogganPosition.Switch(actionBetweenEachSlide, _ => { });
        }
        while (tobogganPosition.Match(_ => true, _ => false));
    }

    public static TAggregate RepeatedlySlideToBottomOfMap<TAggregate>(this TobogganPosition self, Slope slope, TAggregate aggregateSeed, Func<TAggregate, TobogganPosition, TAggregate> aggregateBetweenEachSlide)
    {
        TAggregate result = aggregateSeed;
        self.RepeatedlySlideToBottomOfMap(slope, tp => result = aggregateBetweenEachSlide(result, tp));
        return result;
    }
}
