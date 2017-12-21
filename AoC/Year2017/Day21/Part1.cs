using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AoC.GraphSolver;

namespace AoC.Year2017.Day21
{
    public class Part1 : BasePart
    {
        private bool[][][] SplitInto(bool[][] grid, int width)
        {
            return Enumerable.Range(0, grid.Length / width)
                .SelectMany(x => Enumerable.Range(0, grid.Length / width)
                    .Select(y => 
                        Enumerable.Range(0, width).Select(x1 => Enumerable.Range(0, width).Select(y1 => grid[x * width + x1][y * width + y1]).ToArray()).ToArray()
                    )
                ).ToArray();
        }

        private bool[][] Join(bool[][][] grids)
        {
            var segments = (int) Math.Pow(grids.Length, 0.5);
            var srcSize = grids[0].Length;
            var outSize = segments * srcSize;
            return Enumerable.Range(0, outSize).Select(y =>
                Enumerable.Range(0, outSize).Select(x =>
                    grids[(y / srcSize) * segments + x / srcSize][x % srcSize][y % srcSize]
                ).ToArray()
            ).ToArray();
        }

        private bool[][] FlipH(bool[][] grid)
        {
            if (grid.Length == 2)
            {
                return new[]
                {
                    new[] {grid[0][1], grid[0][0]},
                    new[] {grid[1][1], grid[1][0]},
                };
            }
            return new[]
            {
                new[] {grid[0][2], grid[0][1], grid[0][0]},
                new[] {grid[1][2], grid[1][1], grid[1][0]},
                new[] {grid[2][2], grid[2][1], grid[2][0]},
            };
        }

        private bool[][] FlipV(bool[][] grid)
        {
            if (grid.Length == 2)
            {
                return new[]
                {
                    new[] {grid[1][0], grid[1][1]},
                    new[] {grid[0][0], grid[0][1]},
                };
            }
            return new[]
            {
                new[] {grid[2][0], grid[2][1], grid[2][2]},
                new[] {grid[1][0], grid[1][1], grid[1][2]},
                new[] {grid[0][0], grid[0][1], grid[0][2]},
            };
        }

        private bool[][] Rotate(bool[][] grid)
        {
            if (grid.Length == 2)
            {
                return new[]
                {
                    new[] {grid[0][1], grid[1][1]},
                    new[] {grid[0][0], grid[1][0]},
                };
            }
            return new[]
            {
                new[] {grid[0][2], grid[1][2], grid[2][2]},
                new[] {grid[0][1], grid[1][1], grid[2][1]},
                new[] {grid[0][0], grid[1][0], grid[2][0]},
            };
        }

        private string ToString(bool[][] grid)
        {
            return string.Join("/", grid.Select(i => string.Join("", i.Select(ii => ii ? "#" : "."))));
        }
        private string[] ToGrid(bool[][] grid)
        {
            return grid.Select(i => string.Join("", i.Select(ii => ii ? "#" : "."))).ToArray();
        }

        int MakeKey(bool[][] part)
        {
            return new[] { part.Length == 3, part.Length == 2 }.Concat(part.SelectMany(i => i)).Aggregate(0, (a, i) => a * 2 + (i ? 1 : 0));
        }

        protected void RunScenario(string title, int iterations, string input)
        {
            RunScenario(title, () =>
            {
                var initialStateStr = @".#.
..#
###";
                var initialState = initialStateStr.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(i => i.ToCharArray().Select(ii => ii == '#').ToArray())
                    .ToArray();


                var rawBook = input.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(i => i.Split(" => ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                        .Select(ii => ii.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries)
                            .Select(iii =>
                                iii.ToCharArray()
                                    .Select(iiii => iiii == '#').ToArray())
                            .ToArray()
                        ).ToArray()).ToArray();

                //var x = rawBook.GroupBy(i => MakeKey(i[0]), i => i).Select(i => new { i.Key, i, Count = i.Count() }).ToArray();
                var book = rawBook.ToDictionary(i => MakeKey(i[0]), i => i[1]);

                var mapCache = new Dictionary<int, bool[][]>();
                bool[][] Map(bool[][] part)
                {
                    var key = MakeKey(part);
                    {
                        if (mapCache.TryGetValue(key, out var result))
                        {
                            return result;
                        }
                    }

                    var tries = new[]
                    {
                        part,
                        Rotate(part),
                    }
                        .SelectMany(i => new[] { i, FlipH(i), FlipV(i) })
                        .SelectMany(i => new[] { i, Rotate(i) })
                        .SelectMany(i => new[] { i, FlipH(i), FlipV(i) })
                        .SelectMany(i => new[] { i, Rotate(i) })
                        .Select(MakeKey)
                        .Distinct()
                        .ToArray();

                    {
                        var match = tries.Select(i => book.TryGetValue(i, out var result) ? result : null).Single(i => i != null);
                        foreach (var tri in tries)
                        {
                            mapCache[tri] = match;
                        }
                        return match;
                    }
                }
                

                var state = initialState;
                for(var i=0; i <iterations; i++)
                {
                    var parts = SplitInto(state, state.Length % 2 == 0 ? 2 : 3);
                    var mappedParts = parts.Select(Map).ToArray();
                    state = Join(mappedParts);

                    Console.WriteLine($"Iteration: {i}");
                    //for (var x = 0; x < state.Length; x++)
                    //{
                    //    Console.WriteLine(string.Join("", state[x].Select(ii => ii ? "#" : ".")));
                    //}
                    //Console.WriteLine("-----");
                }
                var s = state.SelectMany(i => i).Sum(i => i ? 1 : 0);
                Console.WriteLine(s);
            });
        }

        public override void Run()
        {
            RunScenario("initial", 2, @"../.# => ##./#../...
            .#./..#/### => #..#/..../..../#..#");
            var input = @"../.. => ##./##./.##
#./.. => .../.#./##.
##/.. => .../.##/#.#
.#/#. => ##./#../#..
##/#. => .##/#.#/#..
##/## => ..#/.#./.##
.../.../... => #.../.##./...#/#...
#../.../... => ...#/..../..#./..##
.#./.../... => ..../.##./###./....
##./.../... => ###./#.##/..#./..#.
#.#/.../... => #.../.#../#..#/..#.
###/.../... => ..##/.##./#.../....
.#./#../... => #.##/..../..../#.##
##./#../... => .#.#/.#.#/##../.#..
..#/#../... => .###/####/.###/##..
#.#/#../... => ..../.#.#/..../####
.##/#../... => .##./##.#/.###/#..#
###/#../... => ####/...#/###./.###
.../.#./... => ..##/#..#/###./###.
#../.#./... => ###./..##/.#.#/.#.#
.#./.#./... => ..#./..#./##.#/##..
##./.#./... => #..#/###./..#./#.#.
#.#/.#./... => .###/#.../.#.#/.##.
###/.#./... => #.##/##../#.#./...#
.#./##./... => #.##/#.##/#.##/.###
##./##./... => ..##/#..#/.###/....
..#/##./... => #..#/.##./##../####
#.#/##./... => ###./###./..##/..##
.##/##./... => ###./##.#/.##./###.
###/##./... => ##../#..#/##../....
.../#.#/... => ##.#/..#./..##/##..
#../#.#/... => #..#/.###/.#../#.#.
.#./#.#/... => ####/#.##/.###/###.
##./#.#/... => #.../####/...#/.#.#
#.#/#.#/... => ...#/.#.#/#..#/#.##
###/#.#/... => ###./#.##/##.#/..##
.../###/... => ..../##.#/.#../..##
#../###/... => ####/..##/.##./.###
.#./###/... => #.#./#.#./#.../#..#
##./###/... => #..#/..##/#.##/#.#.
#.#/###/... => .##./##.#/.#../####
###/###/... => ####/##.#/.#../#.#.
..#/.../#.. => #..#/#.##/.###/.###
#.#/.../#.. => .##./#.../.#.#/....
.##/.../#.. => .#.#/.#.#/##../####
###/.../#.. => .#.#/.##./####/##.#
.##/#../#.. => .###/.###/.###/#...
###/#../#.. => ..##/#.../#.#./..#.
..#/.#./#.. => #.#./##../##../####
#.#/.#./#.. => ..../..##/#..#/..#.
.##/.#./#.. => #.##/#..#/##.#/.##.
###/.#./#.. => ...#/#.../#.#./.#..
.##/##./#.. => .##./#..#/.##./...#
###/##./#.. => ##.#/##.#/.##./...#
#../..#/#.. => ##../..#./..#./#.#.
.#./..#/#.. => #.#./##../#..#/#.##
##./..#/#.. => #.##/###./###./.#.#
#.#/..#/#.. => ..../...#/...#/#..#
.##/..#/#.. => #..#/#.#./..##/.##.
###/..#/#.. => ##../.#.#/.#../#.#.
#../#.#/#.. => ####/.##./.##./.##.
.#./#.#/#.. => ...#/.##./..#./.##.
##./#.#/#.. => .#.#/.##./..#./.#.#
..#/#.#/#.. => .#../##.#/##../#...
#.#/#.#/#.. => .#.#/..#./#.../##..
.##/#.#/#.. => ..#./#.#./###./#...
###/#.#/#.. => ..../#.#./..##/##.#
#../.##/#.. => .##./##../.#../..##
.#./.##/#.. => ##../#.#./#.../####
##./.##/#.. => ###./###./#.#./..##
#.#/.##/#.. => ...#/#..#/..#./###.
.##/.##/#.. => ..##/####/..../#.##
###/.##/#.. => .#.#/#.../.##./#...
#../###/#.. => ..#./.#.#/#..#/.##.
.#./###/#.. => ####/..../####/#.##
##./###/#.. => .###/..../#.#./####
..#/###/#.. => ###./#.#./.#.#/#...
#.#/###/#.. => #.#./#.#./..##/.##.
.##/###/#.. => #.##/.###/.##./#.##
###/###/#.. => #..#/.#../.#../.##.
.#./#.#/.#. => .#../.##./##../..##
##./#.#/.#. => .##./#.##/...#/#.#.
#.#/#.#/.#. => ##.#/###./#.#./..#.
###/#.#/.#. => ..../##../.###/###.
.#./###/.#. => .#.#/.###/..../#..#
##./###/.#. => #.../..#./#..#/.#..
#.#/###/.#. => .#../##.#/##.#/.###
###/###/.#. => #..#/.#.#/#.#./..#.
#.#/..#/##. => .#../.###/...#/#.##
###/..#/##. => ...#/...#/..##/...#
.##/#.#/##. => #.#./###./.##./####
###/#.#/##. => #.#./...#/...#/....
#.#/.##/##. => ###./#.../##.#/..#.
###/.##/##. => .#../#.../.###/.#..
.##/###/##. => #.../..#./..#./.###
###/###/##. => .#../.#../####/###.
#.#/.../#.# => ##.#/##../...#/##.#
###/.../#.# => ###./###./#..#/###.
###/#../#.# => .###/..#./.#../#...
#.#/.#./#.# => ##.#/.##./.#.#/##.#
###/.#./#.# => ...#/...#/#.##/.##.
###/##./#.# => #.../##../#.../....
#.#/#.#/#.# => ####/.#../..##/..##
###/#.#/#.# => ##../####/#.##/..##
#.#/###/#.# => ##../..../..../####
###/###/#.# => .#../.#.#/.###/.#.#
###/#.#/### => ##../####/###./...#
###/###/### => ###./#..#/##../.##.";
            RunScenario("part1", 5, input);
            RunScenario("part1", 18, input);
        }
    }
}
