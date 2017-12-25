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

namespace AoC.Year2017.Day25
{
    public class Part1 : BasePart
    {
        protected void RunScenario(string title)
        {
            RunScenario(title, () =>
            {
                var memory = new Dictionary<int, int>();
                var state = 'A';
                var current = 0;

                for (var step = 0; step < 12919244; step++)
                {
                    int value = 0;
                    memory.TryGetValue(current, out value);
                    switch (state)
                    {
                        case 'A':
                            if (value == 0)
                            {
                                memory[current] = 1;
                                current++;
                                state = 'B';
                            }
                            else
                            {
                                memory[current] = 0;
                                current--;
                                state = 'C';
                            }
                            break;
                        case 'B':
                            if (value == 0)
                            {
                                memory[current] = 1;
                                current--;
                                state = 'A';
                            }
                            else
                            {
                                memory[current] = 1;
                                current++;
                                state = 'D';
                            }
                            break;
                        case 'C':
                            if (value == 0)
                            {
                                memory[current] = 1;
                                current++;
                                state = 'A';
                            }
                            else
                            {
                                memory[current] = 0;
                                current--;
                                state = 'E';
                            }
                            break;
                        case 'D':
                            if (value == 0)
                            {
                                memory[current] = 1;
                                current++;
                                state = 'A';
                            }
                            else
                            {
                                memory[current] = 0;
                                current++;
                                state = 'B';
                            }
                            break;
                        case 'E':
                            if (value == 0)
                            {
                                memory[current] = 1;
                                current--;
                                state = 'F';
                            }
                            else
                            {
                                memory[current] = 1;
                                current--;
                                state = 'C';
                            }
                            break;
                        case 'F':
                            if (value == 0)
                            {
                                memory[current] = 1;
                                current++;
                                state = 'D';
                            }
                            else
                            {
                                memory[current] = 1;
                                current++;
                                state = 'A';
                            }
                            break;
                    }
                }

                Console.WriteLine($"1s: {memory.Values.Sum()}");

            });
        }

        public override void Run()
        {
            RunScenario("part1");
        }
    }
}
