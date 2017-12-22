using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AoC.GraphSolver;

namespace AoC.Year2017.Day22
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, int bursts, string input)
        {
            RunScenario(title, () =>
            {
                var initialState = input.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(i => i.ToCharArray().Select(ii => ii == '#').ToArray())
                    .ToArray();

                var dir = new int [] { 0, 1 };
                var loc = new int[] {0, 0};

                string getKey(int[] pos)
                {
                    return $"{pos[0]}_{pos[1]}";
                }
                var grid = new HashSet<string>();
                for (var i = 0; i < initialState.Length; i++)
                {
                    for (var j = 0; j < initialState.Length; j++)
                    {
                        if (initialState[j][i])
                        {
                            grid.Add(getKey(new[] {i - initialState.Length / 2, -(j - initialState.Length / 2) }));
                        }
                    }
                }

                Console.WriteLine(string.Join(" ", grid));

                var infection = 0;
                for (var i = 0; i < bursts; i++)
                {
                    var key = getKey(loc);
                    if (grid.Contains(key))
                    {
                        // right
                        dir = dir[0] == 0 ? new int[] { dir[1], 0} : new int[] { 0, -dir[0] };
                        grid.Remove(key);
                    }
                    else
                    {
                        // left
                        dir = dir[0] == 0 ? new int[] { -dir[1], 0 } : new int[] { 0, dir[0] };
                        grid.Add(key);
                        infection++;
                    }
                    loc = new[] {loc[0] + dir[0], loc[1] + dir[1]};
                }

                Console.WriteLine($"Infections: {infection}");

            });
        }

        public override void Run()
        {
            RunScenario("initial-70", 70, @"..#
#..
...");
            RunScenario("initial-10000", 10000, @"..#
#..
...");
            RunScenario("initial-10000", 10000, @".#...#.#.##..##....##.#.#
###.###..##...##.##....##
....#.###..#...#####..#.#
.##.######..###.##..#...#
#..#..#..##..###...#..###
..####...#.##.#.#.##.####
#......#..####..###..###.
#####.##.#.#.##.###.#.#.#
.#.###....###....##....##
.......########.#.#...#..
...###.####.##..###.##..#
#.#.###.####.###.###.###.
.######...###.....#......
....##.###..#.#.###...##.
#.###..###.#.#.##.#.##.##
#.#.#..###...###.###.....
##..##.##...##.##..##.#.#
.....##......##..#.##...#
..##.#.###.#...#####.#.##
....##..#.#.#.#..###.#..#
###..##.##....##.#....##.
#..####...####.#.##..#.##
####.###...####..##.#.#.#
#.#.#.###.....###.##.###.
.#...##.#.##..###.#.###..");
        }
    }
}
