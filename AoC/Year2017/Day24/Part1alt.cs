using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AoC.GraphSolver;
using Combinatorics.Collections;

namespace AoC.Year2017.Day24
{
    public class Part1Alt : BasePart
    {
        public class StaticInfo
        {
            public int[][] Pieces;
            public long MaxStrength;
        }

        public class BridgeNode : Node<BridgeNode>
        {
            public StaticInfo StaticInfo;
            public int[] UsedPieces;

            public override bool IsValid
            {
                get
                {
                    var last = 0;
                    for (var x = 0; x < UsedPieces.Length; x++)
                    {
                        var piece = StaticInfo.Pieces[UsedPieces[x]];
                        if (piece[0] == last)
                        {
                            last = piece[1];
                        }
                        else if (piece[1] == last)
                        {
                            last = piece[0];
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }

            public override bool IsComplete => false;
            public override decimal CurrentCost => StaticInfo.MaxStrength - UsedPieces.Select(i => StaticInfo.Pieces[i]).Sum(i => i.Sum());
            public override decimal EstimatedCost => 0;
            public override string Description => string.Join("--", UsedPieces.Select(i => StaticInfo.Pieces[i]).Select(i => $"{i[0]}/{i[1]}"));
            public override object[] Keys => UsedPieces.Cast<object>().ToArray();

            public override IEnumerable<BridgeNode> GetAdjacent()
            {
                var nextPieces = StaticInfo.Pieces.Select((i, ix) => new {i, ix}).Where(i => !UsedPieces.Contains(i.ix))
                    .Select(i => i.ix).ToList();
                foreach (var piece in nextPieces)
                {
                    yield return new BridgeNode()
                    {
                        StaticInfo = this.StaticInfo,
                        UsedPieces = UsedPieces.Concat(new[] {piece}).ToArray(),
                    };
                }
            }

        }

        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var initialState = input.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(i => i.Split(new [] { '/'}).Select(int.Parse).ToArray())
                    .ToArray();

                var state = new BridgeNode()
                {
                    StaticInfo = new StaticInfo()
                    {
                        Pieces = initialState,
                        MaxStrength = initialState.Sum(i => i.Sum())
                    },
                    UsedPieces = new int[0],
                };

                var best = new ParallelSolver(8).Evaluate(state, state.Key);

                Console.WriteLine(-(best.CurrentCost - state.StaticInfo.MaxStrength));
            });
        }

        public override void Run()
        {
            RunScenario("initial", @"0/2
2/2
2/3
3/4
3/5
0/1
10/1
9/10");
            //return;
            RunScenario("part1", @"25/13
4/43
42/42
39/40
17/18
30/7
12/12
32/28
9/28
1/1
16/7
47/43
34/16
39/36
6/4
3/2
10/49
46/50
18/25
2/23
3/21
5/24
46/26
50/19
26/41
1/50
47/41
39/50
12/14
11/19
28/2
38/47
5/5
38/34
39/39
17/34
42/16
32/23
13/21
28/6
6/20
1/30
44/21
11/28
14/17
33/33
17/43
31/13
11/21
31/39
0/9
13/50
10/14
16/10
3/24
7/0
50/50");
        }
    }
}
