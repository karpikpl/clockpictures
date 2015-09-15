using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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

            var result3 = SolutionSubstring(a, b);
            // var result2 = Solution1(a, b);
            var result = Solution2(a, b);
          
            if (result != result3)
                throw new AccessViolationException();


            writer.Write(result);
            writer.Write("\n");
            writer.Flush();
        }

        private static string Solution2(int[] a, int[] b)
        {
            // first put hands in order
            a = GetDiffs(a).ToArray();
            b = GetDiffs(b).ToArray();

            if (a.Sum() != 360000 || b.Sum() != 360000)
                throw new InvalidOperationException();

            //            if (DirtyHack(ref a, ref b))
            //                return "impossible";

            var aS = Compact(a).ToArray();
            var bS = Compact(b).ToArray();

            Debug.WriteLine("A compacted: " + string.Join(", ", aS));
            Debug.WriteLine("B compacted: " + string.Join(", ", bS));

            if (aS.Length != bS.Length)
            {
                return "impossible";
            }

            bool isSolutionOk = false;

            for (int i = 0; i < bS.Length; i++)
            {
                // assume it's ok
                isSolutionOk = true;

                for (int j = 0; j < aS.Length; j++)
                {
                    if (aS[j] != bS[(i + j) % bS.Length])
                    {
                        isSolutionOk = false;
                        break;
                    }
                }

                // exit when found
                if (isSolutionOk)
                    break;
            }

            return isSolutionOk ? "possible" : "impossible";
        }

        public static string SolutionSubstring(int[] a, int[] b)
        {
            Debug.WriteLine("A: " + string.Join(", ", a));
            Debug.WriteLine("B: " + string.Join(", ", b));

            // first put hands in order
            var aDiffs = GetDiffs(a);
            var bDiffs = GetDiffs(b);

            Debug.WriteLine("A diffs: " + string.Join(", ", aDiffs));
            Debug.WriteLine("B diffs: " + string.Join(", ", bDiffs));

            var aSb = ArrayToString(aDiffs);
            aSb.Append(aSb);
            var bSb = ArrayToString(bDiffs);

            Debug.WriteLine("A string: " + aSb);
            Debug.WriteLine("B string: " + bSb);

            return Kmp(aSb.ToString(), bSb.ToString()) >= 0 ? "possible" : "impossible";
        }

        public static IEnumerable<string> Compact(int[] a)
        {
            int streak = 0;
            int streakStart = -1;
            int limit = a.Length;
            int forwardSearch = 0;

            if (a[0] == a[a.Length - 1])
            {
                int value = a[0];
                a[0] = -1;
                a[a.Length - 1] = -1;

                streak = 2;
                int index = 1;
                while (a[index] == value)
                {
                    a[index] = -1;
                    index++;
                    streak++;
                }
                // set the algorithm to skip the begining
                forwardSearch = index;

                var topIndex = a.Length - 2;
                while (a[topIndex] == value)
                {
                    a[topIndex] = -1;
                    topIndex--;
                    streak++;
                }
                //limit = a.Length - bottom - 1;
                limit = topIndex + 2;

                yield return value + "x" + streak;
                streak = -1;
            }

            for (int i = forwardSearch; i < limit - 1; i++)
            {
                if (a[i] == -1)
                    continue;

                if (a[i] == a[i + 1])
                {
                    if (streakStart == -1)
                    {
                        streakStart = i;
                        streak++;
                    }
                    streak++;
                }
                else
                {
                    if (streak > 0)
                    {
                        // end of streak
                        yield return a[i - 1] + "x" + streak;
                        streak = 0;
                        streakStart = -1;
                    }
                    else
                    {
                        yield return a[i].ToString();
                    }

                    if (i == limit - 2 && a[limit - 1] != -1)
                    {
                        yield return a[limit - 1].ToString();
                    }
                }
            }

            if (streak > 0)
            {
                yield return a[limit - 1] + "x" + streak;
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

        private static string Solution1(int[] a, int[] b)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int n = a.Length;

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

        public static StringBuilder ArrayToString(IEnumerable<int> array)
        {
            var sb = new StringBuilder();

            array.ToList().ForEach(a => sb.Append(a));

            return sb;
        }

        public static int Kmp(string text, string searchString)
        {
            if (text.Length == searchString.Length)
            {
                return text == searchString ? 0 : -1;
            }

            int m = 0;
            int i = 0;
            int[] T = new int[text.Length];

            KmpTable(text, T);

            while (m + i < text.Length)
            {
                if (searchString[i] == text[m + i])
                {
                    if (i == searchString.Length - 1)
                        return m;
                    i++;
                }
                else
                {
                    if (T[i] > -1)
                    {
                        m = m + i - T[i];
                        i = T[i];
                    }
                    else
                    {
                        i = 0;
                        m++;
                    }
                }
            }
            return -1;
        }

        private static void KmpTable(string w, int[] T)
        {
            int pos = 2;
            int cnd = 0;

            T[0] = -1;
            T[1] = 0;

            while (pos < w.Length)
            {
                if (w[pos - 1] == w[cnd])
                {
                    cnd++;
                    T[pos] = cnd;
                    pos++;
                }
                else if (cnd > 0)
                {
                    cnd = T[cnd];
                }
                else
                {
                    T[pos] = 0;
                    pos++;
                }
            }
        }
    }
}