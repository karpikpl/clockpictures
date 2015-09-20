using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using KattisSolution.IO;

namespace KattisSolution
{
    internal class Program
    {
        private const string POSSIBLE = "possible";
        private const string IMPOSSIBLE = "impossible";

        private static void Main(string[] args)
        {
            Solve(Console.OpenStandardInput(), Console.OpenStandardOutput());
        }

        public static void Solve(Stream stdin, Stream stdout)
        {
            IScanner scanner = new OptimizedPositiveIntReader(stdin);
            // uncomment when you need more advanced reader
            // scanner = new Scanner(stdin);
            // scanner = new LineReader(stdin);
            var writer = new BufferedStdoutWriter(stdout);

            var n = scanner.NextInt();

            var a = new int[n];
            var b = new int[n];

            for (var i = 0; i < n; i++)
            {
                a[i] = scanner.NextInt();
            }

            for (var i = 0; i < n; i++)
            {
                b[i] = scanner.NextInt();
            }

            var result = SolutionCompacting(a, b);

            writer.Write(result);
            writer.Write("\n");
            writer.Flush();
        }

        private static string SolutionCompacting(int[] a, int[] b)
        {
            // first put hands in order
            a = GetDiffs(a).ToArray();
            b = GetDiffs(b).ToArray();

            // compact both lists
            var aS = Compact(a).ToArray();
            var bS = Compact(b).ToArray();

            Debug.WriteLine("A compacted: " + string.Join(", ", aS));
            Debug.WriteLine("B compacted: " + string.Join(", ", bS));

            if (aS.Length != bS.Length)
            {
                return IMPOSSIBLE;
            }

            bool isSolutionOk = AreTwoRingsSame(aS, bS);

            return isSolutionOk ? POSSIBLE : IMPOSSIBLE;
        }

        public static bool AreTwoRingsSame(string[] a, string[] b)
        {
            bool areSame = false;
            for (int i = 0; i < a.Length; i++)
            {
                // assume it's ok
                areSame = true;

                for (int j = 0; j < a.Length; j++)
                {
                    Debug.WriteLine("Compare a[{0}] with b[{1}]", (i + j) % a.Length, j);
                    if (a[(i + j) % a.Length] != b[j])
                    {
                        areSame = false;
                        break;
                    }
                }

                // exit when found
                if (areSame)
                    break;
            }

            return areSame;
        }

        public static IEnumerable<string> Compact(int[] a)
        {
            var compacted = new LinkedList<string>();
            int streak = 0;

            for (int i = 0; i < a.Length - 1; i++)
            {
                if (a[i] == a[i + 1])
                {
                    // streak detected
                    streak++;
                }
                else
                {
                    if (streak > 0)
                    {
                        compacted.AddLast(a[i] + "x" + (streak + 1));
                    }
                    else
                    {
                        compacted.AddLast(a[i].ToString(CultureInfo.InvariantCulture));
                    }
                    streak = 0;
                }
            }

            if (a[a.Length - 1] == a[0])
            {
                if (compacted.First == null)
                    compacted.AddLast(string.Empty);

                var oldStreak = 0;
                // check if streak already found
                if (compacted.First.Value.StartsWith(a[0] + "x"))
                {
                    oldStreak = int.Parse(compacted.First.Value.Substring(compacted.First.Value.IndexOf("x") + 1));
                }
                else
                {
                    streak++;
                }
                compacted.First.Value = a[0] + "x" + (oldStreak + streak + 1);
            }
            else
            {
                if (streak > 0)
                {
                    compacted.AddLast(a[a.Length - 1] + "x" + (streak + 1));
                }
                else
                {
                    compacted.AddLast(a[a.Length - 1].ToString(CultureInfo.InvariantCulture));
                }
            }

            return compacted;
        }

        public static IEnumerable<int> GetDiffs(int[] a)
        {
            var diffs = a.OrderBy(v => v).ToArray();

            for (int i = 1; i < diffs.Length; i++)
            {
                yield return diffs[i] - diffs[i - 1];
            }

            yield return 360000 - diffs[a.Length - 1] + diffs[0];
        }
    }
}