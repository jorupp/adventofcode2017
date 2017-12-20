using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC
{
    class Program
    {
        static void Main(string[] args)
        {
            //new Year2015.Day22.Part1().Run();
            //new Year2015.Day22.Part2().Run();
            new Year2016.Day11.Part1().Run();
            if (Debugger.IsAttached)
            {
                Console.ReadLine();
            }
        }
    }
}
