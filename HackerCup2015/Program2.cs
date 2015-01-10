using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace HackerCup2015
{
    internal class Program2
    {
        private class Pcf
        {
            private int _p;
            private int _c;
            private int _f;

            public Pcf(int p = 0, int c = 0, int f = 0)
            {
                _p = p;
                _c = c;
                _f = f;
            }

            public Pcf(Pcf pcf)
            {
                _p = pcf._p;
                _c = pcf._c;
                _f = pcf._f;
            }

            public void Add(Pcf pcf)
            {
                _p += pcf._p;
                _c += pcf._c;
                _f += pcf._f;
            }

            public bool IsTarget(Pcf target)
            {
                return _p == target._p && _c == target._c && _f == target._f;
            }

            public bool IsValid(Pcf target)
            {
                return _p <= target._p && _c <= target._c && _f <= target._f;
            }
        }

        private static void Main()
        {
#if DEBUG
            Trace.Listeners.Add(new ConsoleTraceListener());
#endif
            const string inputPath = "test2.in";
            const string outputPath = "test2.out";
            var sb = new StringBuilder();

            using (var sr = new StreamReader(inputPath))
            {
                string[] input = sr.ReadToEnd()
                    .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                int nbCase = int.Parse(input[0]);
                int lineCounter = 1;

                for (int i = 0; i < nbCase; i++)
                {
                    string[] target = input[lineCounter++].Split(" ".ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries);
                    var targetPcf = new Pcf(int.Parse(target[0]), int.Parse(target[1]), int.Parse(target[2]));
                    var foodList = new List<Pcf>();
                    int nbFood = int.Parse(input[lineCounter++]);

                    for (int j = 0; j < nbFood; j++)
                    {
                        target = input[lineCounter++].Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        var food = new Pcf(int.Parse(target[0]), int.Parse(target[1]), int.Parse(target[2]));

                        foodList.Add(food);
                    }

                    var res = Treatment(targetPcf, foodList);

                    sb.AppendFormat("Case #{0}: {1}{2}", i + 1, res ? "yes" : "no", Environment.NewLine);
                }
            }

            var toWrite = sb.ToString();

            using (var sw = new StreamWriter(outputPath))
            {
                sw.Write(toWrite);
            }

            Console.Write(toWrite);
            Console.ReadLine();
        }

        private static bool Treatment(Pcf target, IEnumerable<Pcf> food)
        {
            var tree = new List<Pcf>();

            foreach (var pcf in food)
            {
                var toAdd = new List<Pcf>();

                foreach (var item in tree)
                {
                    var addThis = new Pcf(item);

                    addThis.Add(pcf);

                    if (addThis.IsTarget(target))
                    {
                        return true;
                    }

                    if (addThis.IsValid(target))
                    {
                        toAdd.Add(addThis);
                    }
                }

                if (pcf.IsTarget(target))
                {
                    return true;
                }

                if (pcf.IsValid(target))
                {
                    toAdd.Add(pcf);
                }

                tree.AddRange(toAdd);
            }

            return false;
        }
    }
}
