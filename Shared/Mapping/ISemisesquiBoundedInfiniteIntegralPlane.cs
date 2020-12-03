using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OneOf;

namespace Shared.Mapping
{
    // semisesqui = 3/4s
    public interface ISemisesquiBoundedInfiniteIntegralPlane<TCellContent>
    {

        public record Boundary;

        public interface IPlanchette
        {
            IPlanchette MoveRight();

            OneOf<IPlanchette, Boundary> MoveDown();

            TCellContent Peek();
        }

        IPlanchette Origin { get; }
    }

    public class TextualSemisesquiBoundedInfiniteIntegralPlane : ISemisesquiBoundedInfiniteIntegralPlane<char>
    {
        private readonly IReadOnlyList<string> _mapLines;
        private readonly Lazy<int> _lazyPatternWidth;

        private TextualSemisesquiBoundedInfiniteIntegralPlane(IReadOnlyList<string> mapLines)
        {
            _mapLines = mapLines;
            _lazyPatternWidth = new Lazy<int>(() => _mapLines.Max(l => l.Length));
        }

        public int PatternWidth => _lazyPatternWidth.Value;

        public int Height => _mapLines.Count;

        public ISemisesquiBoundedInfiniteIntegralPlane<char>.IPlanchette Origin => new Planchette(this, 0, 0);

        public static async Task<ISemisesquiBoundedInfiniteIntegralPlane<char>> FromFileAsync(string path)
        {
            var lines = (await System.IO.File.ReadAllLinesAsync(path).ConfigureAwait(false)).AsEnumerable();
            lines = lines.Select(l => l.Trim()).Where(l => l.Length > 0);
            return new TextualSemisesquiBoundedInfiniteIntegralPlane(lines.ToList().AsReadOnly());
        }

        private class Planchette : ISemisesquiBoundedInfiniteIntegralPlane<char>.IPlanchette
        {
            private readonly TextualSemisesquiBoundedInfiniteIntegralPlane _map;
            private readonly int _row;
            private readonly int _column;

            public Planchette(TextualSemisesquiBoundedInfiniteIntegralPlane map, int row, int column)
            {
                _map = map;
                _row = row;
                _column = column;
            }

            public OneOf<ISemisesquiBoundedInfiniteIntegralPlane<char>.IPlanchette, ISemisesquiBoundedInfiniteIntegralPlane<char>.Boundary> MoveDown()
            {
                if (_row == (_map.Height - 1))
                {
                    return new ISemisesquiBoundedInfiniteIntegralPlane<char>.Boundary();
                }
                else
                {
                    return new Planchette(_map, _row + 1, _column);
                }
            }

            public ISemisesquiBoundedInfiniteIntegralPlane<char>.IPlanchette MoveRight()
            {
                if (_column == (_map.PatternWidth - 1))
                {
                    return new Planchette(_map, _row, 0);
                }
                else
                {
                    return new Planchette(_map, _row, _column + 1);
                }
            }

            public char Peek()
            {
                return _map._mapLines[_row][_column];
            }
        }
    }
}
