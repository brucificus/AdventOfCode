using System.Collections.Generic;
using System.Linq;

namespace Shared
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<(int, int)> SelectUniquePairs(this IEnumerable<int> self)
        {
            return self.SelectMany(a => self.Where(b => a != b && a <= b).Select(b => (a, b)));
        }

        public static IEnumerable<(int, int, int)> SelectUniqueTriplets(this IEnumerable<int> self)
        {
            return self
                .SelectMany(
                    a => self.Where(b => a != b && a <= b)
                                .Select(b => (a, b))
                                .SelectMany(
                                    p => self.Where(c => p.a != c && p.b != c && p.b <= c)
                                    .Select(c => (p.a, p.b, c))));
        }
    }
}
