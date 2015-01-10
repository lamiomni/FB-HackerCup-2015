using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace HackerCup2015
{
    class Program1
    {
        static void Main()
        {
#if DEBUG
            Trace.Listeners.Add(new ConsoleTraceListener());
#endif
            const string inputPath = "test1.in";
            const string outputPath = "test1.out";
            var sb = new StringBuilder();

            using (var sr = new StreamReader(inputPath))
            {
                string[] input = sr.ReadToEnd().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                int nbCase = int.Parse(input[0]);

                for (int i = 1; i < nbCase + 1; i++)
                {
                    string ourNum = input[i];
                    int min, max;

                    Trace.WriteLine(string.Format("input: {0}", ourNum));

                    if (ourNum.Length == 1)
                    {
                        min = int.Parse(ourNum);
                        max = min;
                    }
                    else
                    {
                        Treatment(ourNum.ToCharArray(), out min, out max);
                    }

                    sb.AppendFormat("Case #{0}: {1} {2}{3}", i, min, max, Environment.NewLine);
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

        static void Treatment(char[] input, out int min, out int max)
        {
            int minIndex1 = -1, minnzIndex1 = -1, minnzIndex2 = -1, minIndex2 = -1, maxIndex1 = -1, maxIndex2 = -1;
            char min1 = ':', minnz1 = ':', max1 = '/';

            for (int i = 0; i < input.Length; i++)
            {
                var c = input[i];

                if (c <= min1)
                {
                    min1 = c;
                    minIndex1 = i;
                }

                if (c <= minnz1 && c != '0')
                {
                    minnz1 = c;
                    minnzIndex1 = i;
                }

                if (c >= max1)
                {
                    max1 = c;
                    maxIndex1 = i;
                }
            }

            for (int i = 0; i < input.Length; i++)
            {
                var c = input[i];

                if (minIndex2 == -1 && c > minnz1 && i < minnzIndex1)
                {
                    minIndex2 = i;
                }

                if (i != 0 && minnzIndex2 == -1 && c > min1 && i < minIndex1)
                {
                    minnzIndex2 = i;
                }

                if (maxIndex2 == -1 && c < max1 && i < maxIndex1)
                {
                    maxIndex2 = i;
                }
            }


            Swap(input, minnzIndex1, minIndex2);
            var minStr = new string(input);
            min = int.Parse(minStr);
            Trace.WriteLine("min set: " + min);
            Swap(input, minnzIndex1, minIndex2);

            if (min1 != minnz1)
            {
                Swap(input, minIndex1, minnzIndex2);
                minStr = new string(input);
                min = Math.Min(min, int.Parse(minStr));
                Trace.WriteLine("min set: " + min);
                Swap(input, minIndex1, minnzIndex2);
            }

            Swap(input, maxIndex1, maxIndex2);
            var maxStr = new string(input);
            max = int.Parse(maxStr);
            Trace.WriteLine("max set: " + max);
        }

        static void Swap(char[] input, int index1, int index2)
        {
            if (index1 == -1 || index2 == -1)
            {
                return;
            }
            
            Trace.WriteLine(string.Format("Swap({0}, {1})", index1, index2));

            char tmp = input[index1];

            input[index1] = input[index2];
            input[index2] = tmp;
        }
    }
}
