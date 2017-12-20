using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AoC.GraphSolver;

namespace AoC.Year2016.Day11
{
    public class StaticInfo
    {
        public int Floors;
        public int TargetFloor = 4;
        public string[] Types;
    }

    [DebuggerDisplay("{Description}")]
    public class FacilityNode : Node<FacilityNode>
    {
        public StaticInfo StaticInfo;
        public int ElevatorFloor;
        public int[] GeneratorFloors;
        public int[] MicrochipFloors;
        public int Moves;

        public FacilityNode()
        {
        }

        public FacilityNode(FacilityNode parent)
        {
            this.StaticInfo = parent.StaticInfo;
            this.ElevatorFloor = parent.ElevatorFloor;
            this.GeneratorFloors = parent.GeneratorFloors.ToArray();
            this.MicrochipFloors = parent.MicrochipFloors.ToArray();
            this.Moves = parent.Moves + 1;
        }

        public override bool IsValid
        {
            get
            {
                if (!(1 <= ElevatorFloor && ElevatorFloor <= StaticInfo.TargetFloor))
                    // floor out of range
                    return false;
                for (var ix = 0; ix < MicrochipFloors.Length; ix++)
                {
                    if (ix == -1)
                        // doesn't exist
                        continue;
                    if (GeneratorFloors[ix] == MicrochipFloors[ix])
                        // protected
                        continue;
                    if (GeneratorFloors.Any(f => f == MicrochipFloors[ix]))
                        // zap
                        return false;
                }
                return true;
            }
        }

        public override bool IsComplete => MicrochipFloors.All(i => i == -1 || i == StaticInfo.TargetFloor) &&
                                           GeneratorFloors.All(i => i == -1 || i == StaticInfo.TargetFloor);

        public override decimal CurrentCost => Moves;

        public override decimal EstimatedCost =>
            Moves + (MicrochipFloors.Sum(i => StaticInfo.TargetFloor - i) +
                     GeneratorFloors.Sum(i => StaticInfo.TargetFloor - i)) / 2m;

        public override object[] Keys => 
            new object[] { ElevatorFloor + "_" + string.Join("_", GeneratorFloors) + "_" + string.Join("_", MicrochipFloors) };
            //new object[] {ElevatorFloor}.Concat(GeneratorFloors.Cast<object>()).Concat(MicrochipFloors.Cast<object>()).ToArray();

        public override string Description
        {
            get
            {
                return String.Join(Environment.NewLine, Enumerable.Range(1, StaticInfo.Floors).Reverse().Select(f => 
                    $"F{f} {(ElevatorFloor == f ? "E" : ".")} " 
                    + string.Join(" ", GeneratorFloors.Select((g, ix) => g == f ? StaticInfo.Types[ix][0].ToString().ToUpperInvariant() + "G" : ". ")) 
                    + string.Join(" ", MicrochipFloors.Select((g, ix) => g == f ? StaticInfo.Types[ix][0].ToString().ToUpperInvariant() + "M" : ". "))
                ));
            }
        }

        public override IEnumerable<FacilityNode> GetAdjacent()
        {
            var directions = new[] {-1, 1}
                .Where(i => this.ElevatorFloor + i >= 1 && this.ElevatorFloor + i <= StaticInfo.TargetFloor).ToArray();
            foreach (var dir in directions)
            {
                // ok, so what are we taking...
                var options = this.MicrochipFloors.Select((f, ix) => new { type = 1, ix, f}).Where(i => i.f == this.ElevatorFloor)
                    .Concat(this.GeneratorFloors.Select((f, ix) => new { type = 2, ix, f }).Where(i => i.f == this.ElevatorFloor))
                    .ToArray();
                // optimization: if going down, always take one, if going up, always take two
                var sets = dir == -1
                    ? options.Select(i => new[] {i}).ToArray()
                    : options.SelectMany(i => options.Where(ii => i != ii).Select(ii => new[] {i, ii})).ToArray();
                foreach (var set in sets)
                {
                    var next = new FacilityNode(this);
                    next.ElevatorFloor += dir;
                    foreach (var opt in set)
                    {
                        if (opt.type == 1)
                        {
                            next.MicrochipFloors[opt.ix] += dir;
                        }
                        else
                        {
                            next.GeneratorFloors[opt.ix] += dir;
                        }
                    }
                    yield return next;
                }
            }
        }
    }

    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input, Action<FacilityNode> modifyInitialState = null)
        {
            RunScenario(title, () =>
            {
                var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                // https://regex101.com/
                var pattern = new Regex(@"a ([^ -]*)(|-compatible) (generator|microchip)");
                var initialState = new FacilityNode() { ElevatorFloor = 1, GeneratorFloors = new int[0], MicrochipFloors = new int[0] };
                var types = new List<string>();
                for(var i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    foreach (Match match in pattern.Matches(line))
                    {
                        var type = match.Groups[1].Value;
                        var item = match.Groups[3].Value;
                        if (!types.Contains(type))
                        {
                            types.Add(type);
                        }
                        var typeIx = types.IndexOf(type);
                        while (initialState.GeneratorFloors.Length <= typeIx)
                        {
                            initialState.GeneratorFloors = initialState.GeneratorFloors.Concat(new[] { -1 }).ToArray();
                            initialState.MicrochipFloors = initialState.MicrochipFloors.Concat(new[] { -1 }).ToArray();
                        }

                        if (item == "generator")
                        {
                            initialState.GeneratorFloors[typeIx] = i + 1;
                        }
                        else if (item == "microchip")
                        {
                            initialState.MicrochipFloors[typeIx] = i + 1;
                        }
                        else
                        {
                            throw new ArgumentException($"item: {item}");
                        }
                    }
                }
                initialState.StaticInfo = new StaticInfo() {  Floors = lines.Length, TargetFloor = lines.Length, Types = types.ToArray() };
                modifyInitialState?.Invoke(initialState);

                var finalState = new RealSolver().Evaluate(initialState);
                Console.WriteLine($"{title} - moves: {finalState.CurrentCost}");
            });
        }

        public override void Run()
        {
            RunScenario("initial", @"The first floor contains a hydrogen-compatible microchip and a lithium-compatible microchip.
The second floor contains a hydrogen generator.
The third floor contains a lithium generator.
The fourth floor contains nothing relevant.");
            RunScenario("real part 1", @"The first floor contains a polonium generator, a thulium generator, a thulium-compatible microchip, a promethium generator, a ruthenium generator, a ruthenium-compatible microchip, a cobalt generator, and a cobalt-compatible microchip.
The second floor contains a polonium-compatible microchip and a promethium-compatible microchip.
The third floor contains nothing relevant.
The fourth floor contains nothing relevant.");
            RunScenario("real part 2", @"The first floor contains a polonium generator, a thulium generator, a thulium-compatible microchip, a promethium generator, a ruthenium generator, a ruthenium-compatible microchip, a cobalt generator, and a cobalt-compatible microchip, a elerium generator, a elerium-compatible microchip, a dilithium generator, a dilithium-compatible microchip.
The second floor contains a polonium-compatible microchip and a promethium-compatible microchip.
The third floor contains nothing relevant.
The fourth floor contains nothing relevant.");
        }
    }
}
