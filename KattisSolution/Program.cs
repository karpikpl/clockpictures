using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using KattisSolution.IO;

namespace KattisSolution
{
    internal class Program
    {
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
            var result = Solution2(n, scanner);


            writer.Write(result);
            writer.Write("\n");
            writer.Flush();
        }

        private static string Solution2(int n, IScanner scanner)
        {
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

            // first put hands in order
            a = GetDiffs(a).ToArray();
            b = GetDiffs(b).ToArray();

            //            if (DirtyHack(ref a, ref b))
            //                return "impossible";

            a = Compact(a).ToArray();
            b = Compact(b).ToArray();

            bool isSolutionOk = false;

            for (int i = 0; i < b.Length; i++)
            {
                // assume it's ok
                isSolutionOk = true;

                for (int j = 0; j < a.Length; j++)
                {
                    if (a[j] != b[(i + j) % b.Length])
                    {
                        //                        i++;
                        //                        while (i < a.Length && a[j] != b[(i + j) % a.Length])
                        //                        {
                        //                            i++;
                        //                        }
                        //                        if (i == a.Length)
                        //                        {
                        // set it to false when it's not
                        isSolutionOk = false;
                        break;
                        //                        }
                        //                        else
                        //                        {
                        //                            // check again full array
                        //                            j = 0;
                        //                        }
                    }
                }

                // exit when found
                if (isSolutionOk)
                    break;
            }

            return isSolutionOk ? "possible" : "impossible";
        }

        public static IEnumerable<int> Compact(int[] a)
        {
            int streak = 0;
            int streakStart = -1;
            int limit = a.Length;

            for (int i = 1; i < limit; i++)
            {
                if (a[i] == a[i - 1])
                {
                    if (streakStart == -1)
                    {
                        streakStart = i - 1;
                        streak++;

                        // only for the begining
                        if (i == 1)
                        {
                            // go backwards to check
                            for (int j = a.Length - 1; j > 1; j--)
                            {
                                if (a[i] == a[j])
                                {
                                    streak++;
                                    limit--;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    streak++;
                }
                else
                {
                    if (streak > 0)
                    {
                        // end of streak
                        yield return -(a[i - 1] + streak);
                    }

                    if (i == 1)
                        yield return a[i - 1];

                    yield return a[i];
                    streak = 0;
                    streakStart = -1;
                }
            }
        }

        private static bool DirtyHack(ref int[] a, ref int[] b)
        {
            var aSorted = a.OrderBy(v => v).ToArray();
            var bSorted = b.OrderBy(v => v).ToArray();

            for (int i = 0; i < aSorted.Length; i++)
            {
                if (aSorted[i] != bSorted[i])
                    return true;
            }

            return false;
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

        private static LinkedList<int> Shift(ref int[] array)
        {
            LinkedList<int> a = new LinkedList<int>(array);
            int n = array.Length;
            var shifted = 0;
            var delta = -array[0];
            var elementToMove = array.Length - 1;
            // we get an array that is ascending, we need to check if values from the end can be moved to beginning

            while (360000 - a.Last.Value + delta < a.Last.Value - a.Last.Previous.Value)
            {
                delta += 360000 - a.Last.Value - delta;
                a.AddFirst(0);
                a.RemoveLast();
            }

            return a;
        }

        private static string Solution1(int n, IScanner scanner)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
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

            // first put hands in order
            a = a.OrderBy(v => v).ToArray();
            b = b.OrderBy(v => v).ToArray();
            string result = null;
            sw.Stop();
            Debug.WriteLine("Data read and sorted in " + sw.Elapsed);

            for (var attempt = 0; attempt < n; attempt++)
            {
                Debug.WriteLine("Attempt " + attempt);
                if (CheckSolution(ref a, ref b, attempt))
                {
                    result = "possible";
                    break;
                }
            }

            return result ?? "impossible";
        }

        private static bool CheckSolution(ref int[] a, ref int[] b, int bIndex)
        {
            var startDiff = Math.Abs(a[0] - b[bIndex]);

            startDiff = startDiff <= 180000 ? startDiff : 360000 - startDiff;

            for (var i = 1; i < a.Length; i++)
            {
                var diff = Math.Abs(a[i] - b[(bIndex + i) % b.Length]);

                if (diff > 180000)
                    diff = 360000 - diff;

                if (diff != startDiff)
                    return false;
            }
            return true;
        }
    }
}