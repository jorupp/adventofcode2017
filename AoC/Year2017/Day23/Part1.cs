using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AoC.GraphSolver;

namespace AoC.Year2017.Day23
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title, string input)
        {
            RunScenario(title, () =>
            {
                var initialState = input.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(i => i.Split(new [] { ' '}).ToArray())
                    .ToArray();
                RunIt(initialState);
            });
        }

        int getReg(string x)
        {
            if (x.Length != 1)
                throw new ArgumentException();
            return (int)(x[0] - 'a');
        }

        void RunIt(string[][] lines)
        {
            var registers = Enumerable.Range(0, 26).Select(i => (long)0).ToArray();

            Func<string, long> getVal = i => {
                int val;
                if (int.TryParse(i, out val))
                {
                    return val;
                }
                return registers[getReg(i)];
            };

            var mul = 0;

            for (long i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                switch (line[0])
                {
                    case "set":
                        registers[getReg(line[1])] = getVal(line[2]);
                        break;
                    case "sub":
                        registers[getReg(line[1])] -= getVal(line[2]);
                        break;
                    case "mul":
                        registers[getReg(line[1])] *= getVal(line[2]);
                        mul++;
                        break;
                    case "jnz":
                        if (getVal(line[1]) != 0)
                        {
                            i += getVal(line[2]) - 1;
                        }
                        break;
                }
            }

            Console.WriteLine($"mul: {mul}");

        }

        public override void Run()
        {
            RunScenario("part1", @"set b 79
set c b
jnz a 2
jnz 1 5
mul b 100
sub b -100000
set c b
sub c -17000
set f 1
set d 2
set e 2
set g d
mul g e
sub g b
jnz g 2
set f 0
sub e -1
set g e
sub g b
jnz g -8
sub d -1
set g d
sub g b
jnz g -13
jnz f 2
sub h -1
set g b
sub g c
jnz g 2
jnz 1 3
sub b -17
jnz 1 -23");
        }
    }
}
