using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AoC.GraphSolver;

namespace AoC.Year2017.Day23
{
    public class Part2 : BasePart
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
            var targets = lines.SelectMany((line, ix) =>
            {
                switch (line[0])
                {
                    case "jnz":
                        return new [] { ix - int.Parse(line[2]) };
                }
                return new int [0];
            }).Distinct().ToList();

            var meaning = lines.Select((line, ix) =>
            {
                switch (line[0])
                {
                    case "set":
                        return $"{line[1]} = {line[2]};";
                    case "sub":
                        return $"{line[1]} -= {line[2]};";
                    case "mul":
                        return $"{line[1]} *= {line[2]};";
                    case "jnz":
                        return $"if({line[1]} != 0) goto label_{ix + int.Parse(line[2])};";
                }
                return null;
            }).Select((l, i) => $"label_{i}: {l}").ToList();
            Console.WriteLine(string.Join(Environment.NewLine, meaning));
            return;

            var registers = Enumerable.Range(0, 26).Select(i => (long)0).ToArray();

            Func<string, long> getVal = i => {
                int val;
                if (int.TryParse(i, out val))
                {
                    return val;
                }
                return registers[getReg(i)];
            };

            registers[getReg("a")] = 1;

            var mul = 0;

            for (long i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (i == 19)
                {
                    Console.WriteLine($"{i}: {string.Join(" ", line)} -> {registers[getReg(line[1])]}");
                    if (registers[getReg("g")] == -107689)
                    {
                        registers[getReg("g")] = 0;
                    }
                }
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
                if (line[1] == "h")
                {
                    Console.WriteLine($"h: {getReg("h")}");
                }
            }
        }

        public void RunProg()
        {
            const long a = 1;
            //const long c = 79 + 17000;
            long b = 0, c = 0, d = 0, e = 0, f = 0, g = 0, h = 0, i = 0, j = 0;


            //c = 124900;
            //for (b = 107900; b != c; b += 17)
            //{
            //    f = 1;
            //    d = 2;
            //    do
            //    {
            //        e = 2;
            //        do
            //        {
            //            if (d * e == b)
            //                f = 0;
            //            e++;
            //        } while (e != b);
            //        d++;
            //    } while (d != b);
            //    if (f == 0)
            //        h++;
            //}


            //c = 124900;
            //for (b = 107900; b != c; b += 17)
            //{
            //    f = 1;
            //    d = 2;
            //    do
            //    {
            //        e = 2;
            //        do
            //        {
            //            g = d * e - b;
            //            if (g == 0)
            //                f = 0;
            //            e -= -1;
            //            g = e;
            //            g -= b;
            //        } while (g != 0);
            //        d -= -1;
            //        g = d;
            //        g -= b;
            //    } while (g != 0);
            //    if (f == 0)
            //        h++;
            //    Console.WriteLine();
            //}

            //c = 124900;
            //for(b = 107900; b != c; b+= 17)
            //{
            //    f = 1;
            //    label_9: d = 2;
            //    label_10: e = 2;
            //    label_11: g = d;
            //    label_12: g *= e;
            //    label_13: g -= b;
            //    label_14: if (g != 0) goto label_16;
            //    label_15: f = 0;
            //    label_16: e -= -1;
            //    label_17: g = e;
            //    label_18: g -= b;
            //    label_19: if (g != 0) goto label_11;
            //    label_20: d -= -1;
            //    label_21: g = d;
            //    label_22: g -= b;
            //    label_23: if (g != 0) goto label_10;
            //    label_24: if (f != 0) goto label_26;
            //    label_25: h -= -1;
            //    label_26:
            //}

            label_32:

            //label_0: b = 79;
            //label_1: c = b;
            //label_2: if (a != 0) goto label_4;
            //label_3: if (1 != 0) goto label_8;
            //label_4: b *= 100;
            //label_5: b -= -100000;
            //label_6: c = b;
            //label_7: c -= -17000;
            //label_8: f = 1;
            //label_9: d = 2;
            //label_10: e = 2;
            //label_11: g = d;
            //label_12: g *= e;
            //label_13: g -= b;
            //label_14: if (g != 0) goto label_16;
            //label_15: f = 0;
            //label_16: e -= -1;
            //label_17: g = e;
            //label_18: g -= b;
            //label_19: if (g != 0) goto label_11;
            //label_20: d -= -1;
            //label_21: g = d;
            //label_22: g -= b;
            //label_23: if (g != 0) goto label_10;
            //label_24: if (f != 0) goto label_26;
            //label_25: h -= -1;
            //label_26: g = b;
            //label_27: g -= c;
            //label_28: if (g != 0) goto label_30;
            //label_29: if (1 != 0) goto label_32;
            //label_30: b -= -17;
            //label_31: if (1 != 0) goto label_8;

            //c = 79 * 100 + 100000 + 17000;

            //label_0: b = 79 * 100 + 100000;
            //label_8: f = 1;
            //label_9: d = 2;
            //label_10: e = 2;
            //label_11: g = d * e - b;
            //label_14: if (g != 0) goto label_16;
            //label_15: f = 0;
            //label_16: e -= -1;
            //label_17: g = e - b;
            //label_19: if (g != 0) goto label_11;
            //label_20: d -= -1;
            //label_21: g = d - b;
            //label_23: if (g != 0) goto label_10;
            //label_24: if (f != 0) goto label_26;
            //label_25: h -= -1;
            //label_26:

            ////label_32:

            //label_4: b = 79 * 100 + 100000;
            //label_8: f = 1;
            //label_9: d = 2;
            //label_10: e = 2;
            //label_11: g = d * e - b;
            //label_14: if (g != 0) goto label_16;
            //label_15: f = 0;
            //label_16: e -= -1;
            //label_17: g = e - b;
            //label_19: if (g != 0) goto label_11;
            //label_20: d -= -1;
            //label_21: g = d - b;
            //label_23: if (g != 0) goto label_10;




            //label_24: if (f != 0) goto label_26;
            //label_25: h -= -1;
            //label_26: g = b - c;
            //label_28: if (g != 0) goto label_30;
            //label_29: goto label_32;
            //label_30: b -= -17;
            //label_31: goto label_8;

            //label_4: b = 79 * 100 + 100000;
            //label_8: f = 1;
            //label_9: d = 2;
            //label_10: e = 2;
            //label_11: g = d * e - b;
            //label_14: if (g != 0) goto label_16;
            //label_15: f = 0;
            //label_16: e -= -1;
            //label_17: g = e - b;
            //label_19: if (g != 0) goto label_11;
            //label_20: d -= -1;
            //label_21: g = d - b;
            //label_23: if (g != 0) goto label_10;

            //b = 79 * 100 + 100000;
            //do
            //{
            //    f = 1;
            //    d = 2;
            //    do
            //    {
            //        e = 2;
            //        do
            //        {
            //            if (d * e == b)
            //            {
            //                f = 0;
            //            }
            //            e++;
            //        } while (e != b);

            //        d++;
            //    } while (d != b);

            //    if (f == 0)
            //    {
            //        h -= -1;
            //    }
            //    if (b == c)
            //    {
            //        break;
            //    }
            //    b -= -17;
            //} while (true);


            // h = - (number of non-primes from 107900 to 4 step -17)
            //for(b = 107900; b != 17079; b -= 17)
            //{
            //    f = 1;
            //    var limit = (int)Math.Pow(b, 0.5);
            //    for(d=2; d <= limit; d++)
            //    {
            //        //for (e = 2; e != b; e++)
            //        for (e = 2; e <= b/d; e++)
            //        {
            //            if (d * e == b)
            //            {
            //                f = 0;
            //            }
            //        }
            //    }

            //    if (f == 0)
            //    {
            //        h -= -1;
            //    }
            //}


            // h = number of non-primes from 107900 to 124900 step 17 plus one extra-> 907
            for (b = 107900; b != 124900 + 17 /* one extra time through the loop */ ; b += 17)
            {
                f = 1;
                for (d = 2; d != b; d++)
                {
                    if (b % d == 0)
                    {
                        f = 0;
                        break;
                    }
                }
                if (f == 0)
                {
                    h++;
                }
            }

            //// h = number of non-primes from 107900 to 124900 step 17 -> 906
            //for (b = 107900; b != 124900; b += 17)
            //{
            //    f = 1;
            //    for (d = 2; d != b; d++)
            //    {
            //        if (b % d == 0)
            //        {
            //            f = 0;
            //            break;
            //        }
            //    }
            //    if (f == 0)
            //    {
            //        h++;
            //    }
            //}


            //// h = number of multiples of 4 or more from 107900 to 124900 step 17 -> 906
            //for (b = 107900; b != 124900; b += 17)
            //{
            //    f = 1;
            //    for (d = 4; d != b; d++)
            //    {
            //        if (b % d == 0)
            //        {
            //            f = 0;
            //            break;
            //        }
            //    }
            //    if (f == 0)
            //    {
            //        h++;
            //    }
            //}

            //// h = number of non-primes from 107900 to 124900 step 17
            //for (b = 107900; b != 124900; b += 17)
            //{
            //    f = 1;
            //    for (d = 2; d != b; d++)
            //    {
            //        for (e = 2; e != b; e++)
            //        {
            //            if (d * e == b)
            //            {
            //                f = 0;
            //            }
            //        }
            //    }
            //    if (f == 0)
            //    {
            //        h++;
            //    }
            //}



            //// h = number of non-primes from 107900 to 124900 step 17-> 827
            //for (b = 107900; b != 124900; b += 17)
            //{
            //    var limit = (int)Math.Pow(b, 0.5);
            //    for (d = 4; d <= limit; d++)
            //    {
            //        if (b % d == 0)
            //        {
            //            h++;
            //            break;
            //        }
            //    }
            //}
            Console.WriteLine($"h: {h}");
        }

        public override void Run()
        {
            RunScenario("part1-opt", () => RunProg());
            return;
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
