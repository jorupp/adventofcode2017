using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC.Year2015.Day22;

namespace AoC
{
    class Program
    {
        static void Main(string[] args)
        {
            new Part1().Run();
            new Part2().Run();
            if (Debugger.IsAttached)
            {
                Console.ReadLine();
            }
        }
    }
}
