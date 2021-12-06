public static class ImmutablySelfEnumerableExtensions
{
    public static IEnumerable<TSelf> AsEnumerable<TSelf>(this TSelf self)
        where TSelf : IImmutablySelfEnumerable<TSelf>
    {
        self = self.Reset();
        var current = self.MoveNext();

        while (true)
        {
            TSelf? next = default;
            bool isCompleted = false;

            current.Switch((TSelf n) => next = n, (Boundary _) => isCompleted = true);

            if (isCompleted)
            {
                break;
            }

            yield return next!;
            current = self.MoveNext();
        }
    }
}
